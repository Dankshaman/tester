using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using UnityEngine;

namespace I2.Loc
{
	// Token: 0x02000486 RID: 1158
	public static class LocalizationManager
	{
		// Token: 0x06003422 RID: 13346 RVA: 0x0015F317 File Offset: 0x0015D517
		public static void InitializeIfNeeded()
		{
			if (string.IsNullOrEmpty(LocalizationManager.mCurrentLanguage) || LocalizationManager.Sources.Count == 0)
			{
				LocalizationManager.AutoLoadGlobalParamManagers();
				LocalizationManager.UpdateSources();
				LocalizationManager.SelectStartupLanguage();
			}
		}

		// Token: 0x06003423 RID: 13347 RVA: 0x0015F341 File Offset: 0x0015D541
		public static string GetVersion()
		{
			return "2.8.13 f2";
		}

		// Token: 0x06003424 RID: 13348 RVA: 0x0015F348 File Offset: 0x0015D548
		public static int GetRequiredWebServiceVersion()
		{
			return 5;
		}

		// Token: 0x06003425 RID: 13349 RVA: 0x0015F34C File Offset: 0x0015D54C
		public static string GetWebServiceURL(LanguageSourceData source = null)
		{
			if (source != null && !string.IsNullOrEmpty(source.Google_WebServiceURL))
			{
				return source.Google_WebServiceURL;
			}
			LocalizationManager.InitializeIfNeeded();
			for (int i = 0; i < LocalizationManager.Sources.Count; i++)
			{
				if (LocalizationManager.Sources[i] != null && !string.IsNullOrEmpty(LocalizationManager.Sources[i].Google_WebServiceURL))
				{
					return LocalizationManager.Sources[i].Google_WebServiceURL;
				}
			}
			return string.Empty;
		}

		// Token: 0x1700071A RID: 1818
		// (get) Token: 0x06003426 RID: 13350 RVA: 0x0015F3C4 File Offset: 0x0015D5C4
		// (set) Token: 0x06003427 RID: 13351 RVA: 0x0015F3D0 File Offset: 0x0015D5D0
		public static string CurrentLanguage
		{
			get
			{
				LocalizationManager.InitializeIfNeeded();
				return LocalizationManager.mCurrentLanguage;
			}
			set
			{
				LocalizationManager.InitializeIfNeeded();
				string supportedLanguage = LocalizationManager.GetSupportedLanguage(value, false);
				if (!string.IsNullOrEmpty(supportedLanguage) && LocalizationManager.mCurrentLanguage != supportedLanguage)
				{
					LocalizationManager.SetLanguageAndCode(supportedLanguage, LocalizationManager.GetLanguageCode(supportedLanguage), true, false);
				}
			}
		}

		// Token: 0x1700071B RID: 1819
		// (get) Token: 0x06003428 RID: 13352 RVA: 0x0015F40D File Offset: 0x0015D60D
		// (set) Token: 0x06003429 RID: 13353 RVA: 0x0015F41C File Offset: 0x0015D61C
		public static string CurrentLanguageCode
		{
			get
			{
				LocalizationManager.InitializeIfNeeded();
				return LocalizationManager.mLanguageCode;
			}
			set
			{
				LocalizationManager.InitializeIfNeeded();
				if (LocalizationManager.mLanguageCode != value)
				{
					string languageFromCode = LocalizationManager.GetLanguageFromCode(value, true);
					if (!string.IsNullOrEmpty(languageFromCode))
					{
						LocalizationManager.SetLanguageAndCode(languageFromCode, value, true, false);
					}
				}
			}
		}

		// Token: 0x1700071C RID: 1820
		// (get) Token: 0x0600342A RID: 13354 RVA: 0x0015F454 File Offset: 0x0015D654
		// (set) Token: 0x0600342B RID: 13355 RVA: 0x0015F4C4 File Offset: 0x0015D6C4
		public static string CurrentRegion
		{
			get
			{
				string currentLanguage = LocalizationManager.CurrentLanguage;
				int num = currentLanguage.IndexOfAny("/\\".ToCharArray());
				if (num > 0)
				{
					return currentLanguage.Substring(num + 1);
				}
				num = currentLanguage.IndexOfAny("[(".ToCharArray());
				int num2 = currentLanguage.LastIndexOfAny("])".ToCharArray());
				if (num > 0 && num != num2)
				{
					return currentLanguage.Substring(num + 1, num2 - num - 1);
				}
				return string.Empty;
			}
			set
			{
				string text = LocalizationManager.CurrentLanguage;
				int num = text.IndexOfAny("/\\".ToCharArray());
				if (num > 0)
				{
					LocalizationManager.CurrentLanguage = text.Substring(num + 1) + value;
					return;
				}
				num = text.IndexOfAny("[(".ToCharArray());
				int num2 = text.LastIndexOfAny("])".ToCharArray());
				if (num > 0 && num != num2)
				{
					text = text.Substring(num);
				}
				LocalizationManager.CurrentLanguage = text + "(" + value + ")";
			}
		}

		// Token: 0x1700071D RID: 1821
		// (get) Token: 0x0600342C RID: 13356 RVA: 0x0015F54C File Offset: 0x0015D74C
		// (set) Token: 0x0600342D RID: 13357 RVA: 0x0015F584 File Offset: 0x0015D784
		public static string CurrentRegionCode
		{
			get
			{
				string currentLanguageCode = LocalizationManager.CurrentLanguageCode;
				int num = currentLanguageCode.IndexOfAny(" -_/\\".ToCharArray());
				if (num >= 0)
				{
					return currentLanguageCode.Substring(num + 1);
				}
				return string.Empty;
			}
			set
			{
				string text = LocalizationManager.CurrentLanguageCode;
				int num = text.IndexOfAny(" -_/\\".ToCharArray());
				if (num > 0)
				{
					text = text.Substring(0, num);
				}
				LocalizationManager.CurrentLanguageCode = text + "-" + value;
			}
		}

		// Token: 0x1700071E RID: 1822
		// (get) Token: 0x0600342E RID: 13358 RVA: 0x0015F5C6 File Offset: 0x0015D7C6
		public static CultureInfo CurrentCulture
		{
			get
			{
				return LocalizationManager.mCurrentCulture;
			}
		}

		// Token: 0x0600342F RID: 13359 RVA: 0x0015F5D0 File Offset: 0x0015D7D0
		public static void SetLanguageAndCode(string LanguageName, string LanguageCode, bool RememberLanguage = true, bool Force = false)
		{
			if (LocalizationManager.mCurrentLanguage != LanguageName || LocalizationManager.mLanguageCode != LanguageCode || Force)
			{
				if (RememberLanguage)
				{
					PersistentStorage.SetSetting_String("I2 Language", LanguageName);
				}
				LocalizationManager.mCurrentLanguage = LanguageName;
				LocalizationManager.mLanguageCode = LanguageCode;
				LocalizationManager.mCurrentCulture = LocalizationManager.CreateCultureForCode(LanguageCode);
				if (LocalizationManager.mChangeCultureInfo)
				{
					LocalizationManager.SetCurrentCultureInfo();
				}
				LocalizationManager.IsRight2Left = LocalizationManager.IsRTL(LocalizationManager.mLanguageCode);
				LocalizationManager.HasJoinedWords = GoogleLanguages.LanguageCode_HasJoinedWord(LocalizationManager.mLanguageCode);
				LocalizationManager.LocalizeAll(Force);
			}
		}

		// Token: 0x06003430 RID: 13360 RVA: 0x0015F654 File Offset: 0x0015D854
		private static CultureInfo CreateCultureForCode(string code)
		{
			CultureInfo result;
			try
			{
				result = CultureInfo.CreateSpecificCulture(code);
			}
			catch (Exception)
			{
				result = CultureInfo.InvariantCulture;
			}
			return result;
		}

		// Token: 0x06003431 RID: 13361 RVA: 0x0015F684 File Offset: 0x0015D884
		public static void EnableChangingCultureInfo(bool bEnable)
		{
			if (!LocalizationManager.mChangeCultureInfo && bEnable)
			{
				LocalizationManager.SetCurrentCultureInfo();
			}
			LocalizationManager.mChangeCultureInfo = bEnable;
		}

		// Token: 0x06003432 RID: 13362 RVA: 0x0015F69D File Offset: 0x0015D89D
		private static void SetCurrentCultureInfo()
		{
			Thread.CurrentThread.CurrentCulture = LocalizationManager.mCurrentCulture;
		}

		// Token: 0x06003433 RID: 13363 RVA: 0x0015F6B0 File Offset: 0x0015D8B0
		private static void SelectStartupLanguage()
		{
			if (LocalizationManager.Sources.Count == 0)
			{
				return;
			}
			string setting_String = PersistentStorage.GetSetting_String("I2 Language", string.Empty);
			string currentDeviceLanguage = LocalizationManager.GetCurrentDeviceLanguage(false);
			if (!string.IsNullOrEmpty(setting_String) && LocalizationManager.HasLanguage(setting_String, true, false, true))
			{
				LocalizationManager.SetLanguageAndCode(setting_String, LocalizationManager.GetLanguageCode(setting_String), true, false);
				return;
			}
			if (!LocalizationManager.Sources[0].IgnoreDeviceLanguage)
			{
				string supportedLanguage = LocalizationManager.GetSupportedLanguage(currentDeviceLanguage, true);
				if (!string.IsNullOrEmpty(supportedLanguage))
				{
					LocalizationManager.SetLanguageAndCode(supportedLanguage, LocalizationManager.GetLanguageCode(supportedLanguage), false, false);
					return;
				}
			}
			int i = 0;
			int count = LocalizationManager.Sources.Count;
			while (i < count)
			{
				if (LocalizationManager.Sources[i].mLanguages.Count > 0)
				{
					for (int j = 0; j < LocalizationManager.Sources[i].mLanguages.Count; j++)
					{
						if (LocalizationManager.Sources[i].mLanguages[j].IsEnabled())
						{
							LocalizationManager.SetLanguageAndCode(LocalizationManager.Sources[i].mLanguages[j].Name, LocalizationManager.Sources[i].mLanguages[j].Code, false, false);
							return;
						}
					}
				}
				i++;
			}
		}

		// Token: 0x06003434 RID: 13364 RVA: 0x0015F7F0 File Offset: 0x0015D9F0
		public static bool HasLanguage(string Language, bool AllowDiscartingRegion = true, bool Initialize = true, bool SkipDisabled = true)
		{
			if (Initialize)
			{
				LocalizationManager.InitializeIfNeeded();
			}
			int i = 0;
			int count = LocalizationManager.Sources.Count;
			while (i < count)
			{
				if (LocalizationManager.Sources[i].GetLanguageIndex(Language, false, SkipDisabled) >= 0)
				{
					return true;
				}
				i++;
			}
			if (AllowDiscartingRegion)
			{
				int j = 0;
				int count2 = LocalizationManager.Sources.Count;
				while (j < count2)
				{
					if (LocalizationManager.Sources[j].GetLanguageIndex(Language, true, SkipDisabled) >= 0)
					{
						return true;
					}
					j++;
				}
			}
			return false;
		}

		// Token: 0x06003435 RID: 13365 RVA: 0x0015F868 File Offset: 0x0015DA68
		public static string GetSupportedLanguage(string Language, bool ignoreDisabled = false)
		{
			string languageCode = GoogleLanguages.GetLanguageCode(Language, false);
			if (!string.IsNullOrEmpty(languageCode))
			{
				int i = 0;
				int count = LocalizationManager.Sources.Count;
				while (i < count)
				{
					int languageIndexFromCode = LocalizationManager.Sources[i].GetLanguageIndexFromCode(languageCode, true, ignoreDisabled);
					if (languageIndexFromCode >= 0)
					{
						return LocalizationManager.Sources[i].mLanguages[languageIndexFromCode].Name;
					}
					i++;
				}
				int j = 0;
				int count2 = LocalizationManager.Sources.Count;
				while (j < count2)
				{
					int languageIndexFromCode2 = LocalizationManager.Sources[j].GetLanguageIndexFromCode(languageCode, false, ignoreDisabled);
					if (languageIndexFromCode2 >= 0)
					{
						return LocalizationManager.Sources[j].mLanguages[languageIndexFromCode2].Name;
					}
					j++;
				}
			}
			int k = 0;
			int count3 = LocalizationManager.Sources.Count;
			while (k < count3)
			{
				int languageIndex = LocalizationManager.Sources[k].GetLanguageIndex(Language, false, ignoreDisabled);
				if (languageIndex >= 0)
				{
					return LocalizationManager.Sources[k].mLanguages[languageIndex].Name;
				}
				k++;
			}
			int l = 0;
			int count4 = LocalizationManager.Sources.Count;
			while (l < count4)
			{
				int languageIndex2 = LocalizationManager.Sources[l].GetLanguageIndex(Language, true, ignoreDisabled);
				if (languageIndex2 >= 0)
				{
					return LocalizationManager.Sources[l].mLanguages[languageIndex2].Name;
				}
				l++;
			}
			return string.Empty;
		}

		// Token: 0x06003436 RID: 13366 RVA: 0x0015F9DC File Offset: 0x0015DBDC
		public static string GetLanguageCode(string Language)
		{
			if (LocalizationManager.Sources.Count == 0)
			{
				LocalizationManager.UpdateSources();
			}
			int i = 0;
			int count = LocalizationManager.Sources.Count;
			while (i < count)
			{
				int languageIndex = LocalizationManager.Sources[i].GetLanguageIndex(Language, true, true);
				if (languageIndex >= 0)
				{
					return LocalizationManager.Sources[i].mLanguages[languageIndex].Code;
				}
				i++;
			}
			return string.Empty;
		}

		// Token: 0x06003437 RID: 13367 RVA: 0x0015FA4C File Offset: 0x0015DC4C
		public static string GetLanguageFromCode(string Code, bool exactMatch = true)
		{
			if (LocalizationManager.Sources.Count == 0)
			{
				LocalizationManager.UpdateSources();
			}
			int i = 0;
			int count = LocalizationManager.Sources.Count;
			while (i < count)
			{
				int languageIndexFromCode = LocalizationManager.Sources[i].GetLanguageIndexFromCode(Code, exactMatch, false);
				if (languageIndexFromCode >= 0)
				{
					return LocalizationManager.Sources[i].mLanguages[languageIndexFromCode].Name;
				}
				i++;
			}
			return string.Empty;
		}

		// Token: 0x06003438 RID: 13368 RVA: 0x0015FABC File Offset: 0x0015DCBC
		public static List<string> GetAllLanguages(bool SkipDisabled = true)
		{
			if (LocalizationManager.Sources.Count == 0)
			{
				LocalizationManager.UpdateSources();
			}
			List<string> Languages = new List<string>();
			int i = 0;
			int count = LocalizationManager.Sources.Count;
			Func<string, bool> <>9__0;
			while (i < count)
			{
				List<string> languages = Languages;
				IEnumerable<string> languages2 = LocalizationManager.Sources[i].GetLanguages(SkipDisabled);
				Func<string, bool> predicate;
				if ((predicate = <>9__0) == null)
				{
					predicate = (<>9__0 = ((string x) => !Languages.Contains(x)));
				}
				languages.AddRange(languages2.Where(predicate));
				i++;
			}
			return Languages;
		}

		// Token: 0x06003439 RID: 13369 RVA: 0x0015FB4C File Offset: 0x0015DD4C
		public static List<string> GetAllLanguagesCode(bool allowRegions = true, bool SkipDisabled = true)
		{
			List<string> Languages = new List<string>();
			int i = 0;
			int count = LocalizationManager.Sources.Count;
			Func<string, bool> <>9__0;
			while (i < count)
			{
				List<string> languages = Languages;
				IEnumerable<string> languagesCode = LocalizationManager.Sources[i].GetLanguagesCode(allowRegions, SkipDisabled);
				Func<string, bool> predicate;
				if ((predicate = <>9__0) == null)
				{
					predicate = (<>9__0 = ((string x) => !Languages.Contains(x)));
				}
				languages.AddRange(languagesCode.Where(predicate));
				i++;
			}
			return Languages;
		}

		// Token: 0x0600343A RID: 13370 RVA: 0x0015FBC8 File Offset: 0x0015DDC8
		public static bool IsLanguageEnabled(string Language)
		{
			int i = 0;
			int count = LocalizationManager.Sources.Count;
			while (i < count)
			{
				if (!LocalizationManager.Sources[i].IsLanguageEnabled(Language))
				{
					return false;
				}
				i++;
			}
			return true;
		}

		// Token: 0x0600343B RID: 13371 RVA: 0x0015FC04 File Offset: 0x0015DE04
		private static void LoadCurrentLanguage()
		{
			for (int i = 0; i < LocalizationManager.Sources.Count; i++)
			{
				int languageIndex = LocalizationManager.Sources[i].GetLanguageIndex(LocalizationManager.mCurrentLanguage, true, false);
				LocalizationManager.Sources[i].LoadLanguage(languageIndex, true, true, true, false);
			}
		}

		// Token: 0x0600343C RID: 13372 RVA: 0x0015FC53 File Offset: 0x0015DE53
		public static void PreviewLanguage(string NewLanguage)
		{
			LocalizationManager.mCurrentLanguage = NewLanguage;
			LocalizationManager.mLanguageCode = LocalizationManager.GetLanguageCode(LocalizationManager.mCurrentLanguage);
			LocalizationManager.IsRight2Left = LocalizationManager.IsRTL(LocalizationManager.mLanguageCode);
			LocalizationManager.HasJoinedWords = GoogleLanguages.LanguageCode_HasJoinedWord(LocalizationManager.mLanguageCode);
		}

		// Token: 0x0600343D RID: 13373 RVA: 0x0015FC88 File Offset: 0x0015DE88
		public static void AutoLoadGlobalParamManagers()
		{
			foreach (LocalizationParamsManager localizationParamsManager in UnityEngine.Object.FindObjectsOfType<LocalizationParamsManager>())
			{
				if (localizationParamsManager._IsGlobalManager && !LocalizationManager.ParamManagers.Contains(localizationParamsManager))
				{
					Debug.Log(localizationParamsManager);
					LocalizationManager.ParamManagers.Add(localizationParamsManager);
				}
			}
		}

		// Token: 0x0600343E RID: 13374 RVA: 0x0015FCD3 File Offset: 0x0015DED3
		public static void ApplyLocalizationParams(ref string translation, bool allowLocalizedParameters = true)
		{
			LocalizationManager.ApplyLocalizationParams(ref translation, (string p) => LocalizationManager.GetLocalizationParam(p, null), allowLocalizedParameters);
		}

		// Token: 0x0600343F RID: 13375 RVA: 0x0015FCFC File Offset: 0x0015DEFC
		public static void ApplyLocalizationParams(ref string translation, GameObject root, bool allowLocalizedParameters = true)
		{
			LocalizationManager.ApplyLocalizationParams(ref translation, (string p) => LocalizationManager.GetLocalizationParam(p, root), allowLocalizedParameters);
		}

		// Token: 0x06003440 RID: 13376 RVA: 0x0015FD2C File Offset: 0x0015DF2C
		public static void ApplyLocalizationParams(ref string translation, Dictionary<string, object> parameters, bool allowLocalizedParameters = true)
		{
			LocalizationManager.ApplyLocalizationParams(ref translation, delegate(string p)
			{
				object result = null;
				if (parameters.TryGetValue(p, out result))
				{
					return result;
				}
				return null;
			}, allowLocalizedParameters);
		}

		// Token: 0x06003441 RID: 13377 RVA: 0x0015FD5C File Offset: 0x0015DF5C
		public static void ApplyLocalizationParams(ref string translation, LocalizationManager._GetParam getParam, bool allowLocalizedParameters = true)
		{
			if (translation == null)
			{
				return;
			}
			string text = null;
			int num = translation.Length;
			int num2 = 0;
			while (num2 >= 0 && num2 < translation.Length)
			{
				int num3 = translation.IndexOf("{[", num2);
				if (num3 < 0)
				{
					break;
				}
				int num4 = translation.IndexOf("]}", num3);
				if (num4 < 0)
				{
					break;
				}
				int num5 = translation.IndexOf("{[", num3 + 1);
				if (num5 > 0 && num5 < num4)
				{
					num2 = num5;
				}
				else
				{
					int num6 = (translation[num3 + 2] == '#') ? 3 : 2;
					string param = translation.Substring(num3 + num6, num4 - num3 - num6);
					string text2 = (string)getParam(param);
					if (text2 != null && allowLocalizedParameters)
					{
						LanguageSourceData languageSourceData;
						TermData termData = LocalizationManager.GetTermData(text2, out languageSourceData);
						if (termData != null)
						{
							int languageIndex = languageSourceData.GetLanguageIndex(LocalizationManager.CurrentLanguage, true, true);
							if (languageIndex >= 0)
							{
								text2 = termData.GetTranslation(languageIndex, null, false);
							}
						}
						string oldValue = translation.Substring(num3, num4 - num3 + 2);
						translation = translation.Replace(oldValue, text2);
						int n = 0;
						if (int.TryParse(text2, out n))
						{
							text = GoogleLanguages.GetPluralType(LocalizationManager.CurrentLanguageCode, n).ToString();
						}
						num2 = num3 + text2.Length;
					}
					else
					{
						num2 = num4 + 2;
					}
				}
			}
			if (text != null)
			{
				string text3 = "[i2p_" + text + "]";
				int num7 = translation.IndexOf(text3, StringComparison.OrdinalIgnoreCase);
				if (num7 < 0)
				{
					num7 = 0;
				}
				else
				{
					num7 += text3.Length;
				}
				num = translation.IndexOf("[i2p_", num7 + 1, StringComparison.OrdinalIgnoreCase);
				if (num < 0)
				{
					num = translation.Length;
				}
				translation = translation.Substring(num7, num - num7);
			}
		}

		// Token: 0x06003442 RID: 13378 RVA: 0x0015FF14 File Offset: 0x0015E114
		internal static string GetLocalizationParam(string ParamName, GameObject root)
		{
			if (root)
			{
				MonoBehaviour[] components = root.GetComponents<MonoBehaviour>();
				int i = 0;
				int num = components.Length;
				while (i < num)
				{
					ILocalizationParamsManager localizationParamsManager = components[i] as ILocalizationParamsManager;
					if (localizationParamsManager != null && components[i].enabled)
					{
						string parameterValue = localizationParamsManager.GetParameterValue(ParamName);
						if (parameterValue != null)
						{
							return parameterValue;
						}
					}
					i++;
				}
			}
			int j = 0;
			int count = LocalizationManager.ParamManagers.Count;
			while (j < count)
			{
				string parameterValue = LocalizationManager.ParamManagers[j].GetParameterValue(ParamName);
				if (parameterValue != null)
				{
					return parameterValue;
				}
				j++;
			}
			return null;
		}

		// Token: 0x06003443 RID: 13379 RVA: 0x0015FFA0 File Offset: 0x0015E1A0
		private static string GetPluralType(MatchCollection matches, string langCode, LocalizationManager._GetParam getParam)
		{
			int i = 0;
			int count = matches.Count;
			while (i < count)
			{
				Match match = matches[i];
				string value = match.Groups[match.Groups.Count - 1].Value;
				string text = (string)getParam(value);
				if (text != null)
				{
					int n = 0;
					if (int.TryParse(text, out n))
					{
						return GoogleLanguages.GetPluralType(langCode, n).ToString();
					}
				}
				i++;
			}
			return null;
		}

		// Token: 0x06003444 RID: 13380 RVA: 0x0016001F File Offset: 0x0015E21F
		public static string ApplyRTLfix(string line)
		{
			return LocalizationManager.ApplyRTLfix(line, 0, true);
		}

		// Token: 0x06003445 RID: 13381 RVA: 0x0016002C File Offset: 0x0015E22C
		public static string ApplyRTLfix(string line, int maxCharacters, bool ignoreNumbers)
		{
			if (string.IsNullOrEmpty(line))
			{
				return line;
			}
			char c = line[0];
			if (c == '!' || c == '.' || c == '?')
			{
				line = line.Substring(1) + c.ToString();
			}
			int num = -1;
			int num2 = 0;
			int num3 = 40000;
			num2 = 0;
			List<string> list = new List<string>();
			while (I2Utils.FindNextTag(line, num2, out num, out num2))
			{
				string str = "@@" + ((char)(num3 + list.Count)).ToString() + "@@";
				list.Add(line.Substring(num, num2 - num + 1));
				line = line.Substring(0, num) + str + line.Substring(num2 + 1);
				num2 = num + 5;
			}
			line = line.Replace("\r\n", "\n");
			line = I2Utils.SplitLine(line, maxCharacters);
			line = RTLFixer.Fix(line, true, !ignoreNumbers);
			for (int i = 0; i < list.Count; i++)
			{
				int length = line.Length;
				for (int j = 0; j < length; j++)
				{
					if (line[j] == '@' && line[j + 1] == '@' && (int)line[j + 2] >= num3 && line[j + 3] == '@' && line[j + 4] == '@')
					{
						int num4 = (int)line[j + 2] - num3;
						if (num4 % 2 == 0)
						{
							num4++;
						}
						else
						{
							num4--;
						}
						if (num4 >= list.Count)
						{
							num4 = list.Count - 1;
						}
						line = line.Substring(0, j) + list[num4] + line.Substring(j + 5);
						break;
					}
				}
			}
			return line;
		}

		// Token: 0x06003446 RID: 13382 RVA: 0x001601F0 File Offset: 0x0015E3F0
		public static string FixRTL_IfNeeded(string text, int maxCharacters = 0, bool ignoreNumber = false)
		{
			if (LocalizationManager.IsRight2Left)
			{
				return LocalizationManager.ApplyRTLfix(text, maxCharacters, ignoreNumber);
			}
			return text;
		}

		// Token: 0x06003447 RID: 13383 RVA: 0x00160203 File Offset: 0x0015E403
		public static bool IsRTL(string Code)
		{
			return Array.IndexOf<string>(LocalizationManager.LanguagesRTL, Code) >= 0;
		}

		// Token: 0x06003448 RID: 13384 RVA: 0x00160216 File Offset: 0x0015E416
		public static bool UpdateSources()
		{
			LocalizationManager.UnregisterDeletededSources();
			LocalizationManager.RegisterSourceInResources();
			LocalizationManager.RegisterSceneSources();
			return LocalizationManager.Sources.Count > 0;
		}

		// Token: 0x06003449 RID: 13385 RVA: 0x00160234 File Offset: 0x0015E434
		private static void UnregisterDeletededSources()
		{
			for (int i = LocalizationManager.Sources.Count - 1; i >= 0; i--)
			{
				if (LocalizationManager.Sources[i] == null)
				{
					LocalizationManager.RemoveSource(LocalizationManager.Sources[i]);
				}
			}
		}

		// Token: 0x0600344A RID: 13386 RVA: 0x00160278 File Offset: 0x0015E478
		private static void RegisterSceneSources()
		{
			foreach (LanguageSource languageSource in (LanguageSource[])Resources.FindObjectsOfTypeAll(typeof(LanguageSource)))
			{
				if (!LocalizationManager.Sources.Contains(languageSource.mSource))
				{
					if (languageSource.mSource.owner == null)
					{
						languageSource.mSource.owner = languageSource;
					}
					LocalizationManager.AddSource(languageSource.mSource);
				}
			}
		}

		// Token: 0x0600344B RID: 13387 RVA: 0x001602E4 File Offset: 0x0015E4E4
		private static void RegisterSourceInResources()
		{
			foreach (string name in LocalizationManager.GlobalSources)
			{
				LanguageSourceAsset asset = ResourceManager.pInstance.GetAsset<LanguageSourceAsset>(name);
				if (asset && !LocalizationManager.Sources.Contains(asset.mSource))
				{
					if (!asset.mSource.mIsGlobalSource)
					{
						asset.mSource.mIsGlobalSource = true;
					}
					asset.mSource.owner = asset;
					LocalizationManager.AddSource(asset.mSource);
				}
			}
		}

		// Token: 0x0600344C RID: 13388 RVA: 0x00160360 File Offset: 0x0015E560
		internal static void AddSource(LanguageSourceData Source)
		{
			if (LocalizationManager.Sources.Contains(Source))
			{
				return;
			}
			LocalizationManager.Sources.Add(Source);
			if (Source.HasGoogleSpreadsheet() && Source.GoogleUpdateFrequency != LanguageSourceData.eGoogleUpdateFrequency.Never)
			{
				Source.Import_Google_FromCache();
				bool justCheck = false;
				if (Source.GoogleUpdateDelay > 0f)
				{
					CoroutineManager.Start(LocalizationManager.Delayed_Import_Google(Source, Source.GoogleUpdateDelay, justCheck));
				}
				else
				{
					Source.Import_Google(false, justCheck);
				}
			}
			for (int i = 0; i < Source.mLanguages.Count<LanguageData>(); i++)
			{
				Source.mLanguages[i].SetLoaded(true);
			}
			if (Source.mDictionary.Count == 0)
			{
				Source.UpdateDictionary(true);
			}
		}

		// Token: 0x0600344D RID: 13389 RVA: 0x00160405 File Offset: 0x0015E605
		private static IEnumerator Delayed_Import_Google(LanguageSourceData source, float delay, bool justCheck)
		{
			yield return new WaitForSeconds(delay);
			if (source != null)
			{
				source.Import_Google(false, justCheck);
			}
			yield break;
		}

		// Token: 0x0600344E RID: 13390 RVA: 0x00160422 File Offset: 0x0015E622
		internal static void RemoveSource(LanguageSourceData Source)
		{
			LocalizationManager.Sources.Remove(Source);
		}

		// Token: 0x0600344F RID: 13391 RVA: 0x00160430 File Offset: 0x0015E630
		public static bool IsGlobalSource(string SourceName)
		{
			return Array.IndexOf<string>(LocalizationManager.GlobalSources, SourceName) >= 0;
		}

		// Token: 0x06003450 RID: 13392 RVA: 0x00160444 File Offset: 0x0015E644
		public static LanguageSourceData GetSourceContaining(string term, bool fallbackToFirst = true)
		{
			if (!string.IsNullOrEmpty(term))
			{
				int i = 0;
				int count = LocalizationManager.Sources.Count;
				while (i < count)
				{
					if (LocalizationManager.Sources[i].GetTermData(term, false) != null)
					{
						return LocalizationManager.Sources[i];
					}
					i++;
				}
			}
			if (!fallbackToFirst || LocalizationManager.Sources.Count <= 0)
			{
				return null;
			}
			return LocalizationManager.Sources[0];
		}

		// Token: 0x06003451 RID: 13393 RVA: 0x001604B0 File Offset: 0x0015E6B0
		public static UnityEngine.Object FindAsset(string value)
		{
			int i = 0;
			int count = LocalizationManager.Sources.Count;
			while (i < count)
			{
				UnityEngine.Object @object = LocalizationManager.Sources[i].FindAsset(value);
				if (@object)
				{
					return @object;
				}
				i++;
			}
			return null;
		}

		// Token: 0x06003452 RID: 13394 RVA: 0x001604F4 File Offset: 0x0015E6F4
		public static void ApplyDownloadedDataFromGoogle()
		{
			int i = 0;
			int count = LocalizationManager.Sources.Count;
			while (i < count)
			{
				LocalizationManager.Sources[i].ApplyDownloadedDataFromGoogle();
				i++;
			}
		}

		// Token: 0x06003453 RID: 13395 RVA: 0x00160528 File Offset: 0x0015E728
		public static string GetCurrentDeviceLanguage(bool force = false)
		{
			if (force || string.IsNullOrEmpty(LocalizationManager.mCurrentDeviceLanguage))
			{
				LocalizationManager.DetectDeviceLanguage();
			}
			return LocalizationManager.mCurrentDeviceLanguage;
		}

		// Token: 0x06003454 RID: 13396 RVA: 0x00160544 File Offset: 0x0015E744
		private static void DetectDeviceLanguage()
		{
			LocalizationManager.mCurrentDeviceLanguage = Application.systemLanguage.ToString();
			if (LocalizationManager.mCurrentDeviceLanguage == "ChineseSimplified")
			{
				LocalizationManager.mCurrentDeviceLanguage = "Chinese (Simplified)";
			}
			if (LocalizationManager.mCurrentDeviceLanguage == "ChineseTraditional")
			{
				LocalizationManager.mCurrentDeviceLanguage = "Chinese (Traditional)";
			}
		}

		// Token: 0x06003455 RID: 13397 RVA: 0x001605A0 File Offset: 0x0015E7A0
		public static void RegisterTarget(ILocalizeTargetDescriptor desc)
		{
			if (LocalizationManager.mLocalizeTargets.FindIndex((ILocalizeTargetDescriptor x) => x.Name == desc.Name) != -1)
			{
				return;
			}
			for (int i = 0; i < LocalizationManager.mLocalizeTargets.Count; i++)
			{
				if (LocalizationManager.mLocalizeTargets[i].Priority > desc.Priority)
				{
					LocalizationManager.mLocalizeTargets.Insert(i, desc);
					return;
				}
			}
			LocalizationManager.mLocalizeTargets.Add(desc);
		}

		// Token: 0x1400007B RID: 123
		// (add) Token: 0x06003456 RID: 13398 RVA: 0x00160628 File Offset: 0x0015E828
		// (remove) Token: 0x06003457 RID: 13399 RVA: 0x0016065C File Offset: 0x0015E85C
		public static event LocalizationManager.OnLocalizeCallback OnLocalizeEvent;

		// Token: 0x06003458 RID: 13400 RVA: 0x00160690 File Offset: 0x0015E890
		public static string GetTranslation(string Term, bool FixForRTL = true, int maxLineLengthForRTL = 0, bool ignoreRTLnumbers = true, bool applyParameters = false, GameObject localParametersRoot = null, string overrideLanguage = null)
		{
			string result = null;
			LocalizationManager.TryGetTranslation(Term, out result, FixForRTL, maxLineLengthForRTL, ignoreRTLnumbers, applyParameters, localParametersRoot, overrideLanguage);
			return result;
		}

		// Token: 0x06003459 RID: 13401 RVA: 0x001606B2 File Offset: 0x0015E8B2
		public static string GetTermTranslation(string Term, bool FixForRTL = true, int maxLineLengthForRTL = 0, bool ignoreRTLnumbers = true, bool applyParameters = false, GameObject localParametersRoot = null, string overrideLanguage = null)
		{
			return LocalizationManager.GetTranslation(Term, FixForRTL, maxLineLengthForRTL, ignoreRTLnumbers, applyParameters, localParametersRoot, overrideLanguage);
		}

		// Token: 0x0600345A RID: 13402 RVA: 0x001606C4 File Offset: 0x0015E8C4
		public static bool TryGetTranslation(string Term, out string Translation, bool FixForRTL = true, int maxLineLengthForRTL = 0, bool ignoreRTLnumbers = true, bool applyParameters = false, GameObject localParametersRoot = null, string overrideLanguage = null)
		{
			Translation = null;
			if (string.IsNullOrEmpty(Term))
			{
				return false;
			}
			LocalizationManager.InitializeIfNeeded();
			int i = 0;
			int count = LocalizationManager.Sources.Count;
			while (i < count)
			{
				if (LocalizationManager.Sources[i].TryGetTranslation(Term, out Translation, overrideLanguage, null, false, false))
				{
					if (applyParameters)
					{
						LocalizationManager.ApplyLocalizationParams(ref Translation, localParametersRoot, true);
					}
					if (LocalizationManager.IsRight2Left && FixForRTL)
					{
						Translation = LocalizationManager.ApplyRTLfix(Translation, maxLineLengthForRTL, ignoreRTLnumbers);
					}
					return true;
				}
				i++;
			}
			return false;
		}

		// Token: 0x0600345B RID: 13403 RVA: 0x00160738 File Offset: 0x0015E938
		public static T GetTranslatedObject<T>(string AssetName, Localize optionalLocComp = null) where T : UnityEngine.Object
		{
			if (optionalLocComp != null)
			{
				return optionalLocComp.FindTranslatedObject<T>(AssetName);
			}
			T t = LocalizationManager.FindAsset(AssetName) as !!0;
			if (t)
			{
				return t;
			}
			return ResourceManager.pInstance.GetAsset<T>(AssetName);
		}

		// Token: 0x0600345C RID: 13404 RVA: 0x00160783 File Offset: 0x0015E983
		public static T GetTranslatedObjectByTermName<T>(string Term, Localize optionalLocComp = null) where T : UnityEngine.Object
		{
			return LocalizationManager.GetTranslatedObject<T>(LocalizationManager.GetTranslation(Term, false, 0, true, false, null, null), null);
		}

		// Token: 0x0600345D RID: 13405 RVA: 0x00160798 File Offset: 0x0015E998
		public static string GetAppName(string languageCode)
		{
			if (!string.IsNullOrEmpty(languageCode))
			{
				for (int i = 0; i < LocalizationManager.Sources.Count; i++)
				{
					if (!string.IsNullOrEmpty(LocalizationManager.Sources[i].mTerm_AppName))
					{
						int languageIndexFromCode = LocalizationManager.Sources[i].GetLanguageIndexFromCode(languageCode, false, false);
						if (languageIndexFromCode >= 0)
						{
							TermData termData = LocalizationManager.Sources[i].GetTermData(LocalizationManager.Sources[i].mTerm_AppName, false);
							if (termData != null)
							{
								string translation = termData.GetTranslation(languageIndexFromCode, null, false);
								if (!string.IsNullOrEmpty(translation))
								{
									return translation;
								}
							}
						}
					}
				}
			}
			return Application.productName;
		}

		// Token: 0x0600345E RID: 13406 RVA: 0x0016082F File Offset: 0x0015EA2F
		public static void LocalizeAll(bool Force = false)
		{
			LocalizationManager.LoadCurrentLanguage();
			if (!Application.isPlaying)
			{
				LocalizationManager.DoLocalizeAll(Force);
				return;
			}
			LocalizationManager.mLocalizeIsScheduledWithForcedValue = (LocalizationManager.mLocalizeIsScheduledWithForcedValue || Force);
			if (LocalizationManager.mLocalizeIsScheduled)
			{
				return;
			}
			CoroutineManager.Start(LocalizationManager.Coroutine_LocalizeAll());
		}

		// Token: 0x0600345F RID: 13407 RVA: 0x00160863 File Offset: 0x0015EA63
		private static IEnumerator Coroutine_LocalizeAll()
		{
			LocalizationManager.mLocalizeIsScheduled = true;
			yield return null;
			LocalizationManager.mLocalizeIsScheduled = false;
			bool force = LocalizationManager.mLocalizeIsScheduledWithForcedValue;
			LocalizationManager.mLocalizeIsScheduledWithForcedValue = false;
			LocalizationManager.DoLocalizeAll(force);
			yield break;
		}

		// Token: 0x06003460 RID: 13408 RVA: 0x0016086C File Offset: 0x0015EA6C
		private static void DoLocalizeAll(bool Force = false)
		{
			Localize[] array = (Localize[])Resources.FindObjectsOfTypeAll(typeof(Localize));
			int i = 0;
			int num = array.Length;
			while (i < num)
			{
				array[i].OnLocalize(Force);
				i++;
			}
			if (LocalizationManager.OnLocalizeEvent != null)
			{
				LocalizationManager.OnLocalizeEvent();
			}
		}

		// Token: 0x06003461 RID: 13409 RVA: 0x001608B8 File Offset: 0x0015EAB8
		public static List<string> GetCategories()
		{
			List<string> list = new List<string>();
			int i = 0;
			int count = LocalizationManager.Sources.Count;
			while (i < count)
			{
				LocalizationManager.Sources[i].GetCategories(false, list);
				i++;
			}
			return list;
		}

		// Token: 0x06003462 RID: 13410 RVA: 0x001608F8 File Offset: 0x0015EAF8
		public static List<string> GetTermsList(string Category = null)
		{
			if (LocalizationManager.Sources.Count == 0)
			{
				LocalizationManager.UpdateSources();
			}
			if (LocalizationManager.Sources.Count == 1)
			{
				return LocalizationManager.Sources[0].GetTermsList(Category);
			}
			HashSet<string> hashSet = new HashSet<string>();
			int i = 0;
			int count = LocalizationManager.Sources.Count;
			while (i < count)
			{
				hashSet.UnionWith(LocalizationManager.Sources[i].GetTermsList(Category));
				i++;
			}
			return new List<string>(hashSet);
		}

		// Token: 0x06003463 RID: 13411 RVA: 0x00160970 File Offset: 0x0015EB70
		public static TermData GetTermData(string term)
		{
			LocalizationManager.InitializeIfNeeded();
			int i = 0;
			int count = LocalizationManager.Sources.Count;
			while (i < count)
			{
				TermData termData = LocalizationManager.Sources[i].GetTermData(term, false);
				if (termData != null)
				{
					return termData;
				}
				i++;
			}
			return null;
		}

		// Token: 0x06003464 RID: 13412 RVA: 0x001609B4 File Offset: 0x0015EBB4
		public static TermData GetTermData(string term, out LanguageSourceData source)
		{
			LocalizationManager.InitializeIfNeeded();
			int i = 0;
			int count = LocalizationManager.Sources.Count;
			while (i < count)
			{
				TermData termData = LocalizationManager.Sources[i].GetTermData(term, false);
				if (termData != null)
				{
					source = LocalizationManager.Sources[i];
					return termData;
				}
				i++;
			}
			source = null;
			return null;
		}

		// Token: 0x04002158 RID: 8536
		private static string mCurrentLanguage;

		// Token: 0x04002159 RID: 8537
		private static string mLanguageCode;

		// Token: 0x0400215A RID: 8538
		private static CultureInfo mCurrentCulture;

		// Token: 0x0400215B RID: 8539
		private static bool mChangeCultureInfo = false;

		// Token: 0x0400215C RID: 8540
		public static bool IsRight2Left = false;

		// Token: 0x0400215D RID: 8541
		public static bool HasJoinedWords = false;

		// Token: 0x0400215E RID: 8542
		public static List<ILocalizationParamsManager> ParamManagers = new List<ILocalizationParamsManager>();

		// Token: 0x0400215F RID: 8543
		private static string[] LanguagesRTL = new string[]
		{
			"ar-DZ",
			"ar",
			"ar-BH",
			"ar-EG",
			"ar-IQ",
			"ar-JO",
			"ar-KW",
			"ar-LB",
			"ar-LY",
			"ar-MA",
			"ar-OM",
			"ar-QA",
			"ar-SA",
			"ar-SY",
			"ar-TN",
			"ar-AE",
			"ar-YE",
			"fa",
			"he",
			"ur",
			"ji"
		};

		// Token: 0x04002160 RID: 8544
		public static List<LanguageSourceData> Sources = new List<LanguageSourceData>();

		// Token: 0x04002161 RID: 8545
		public static string[] GlobalSources = new string[]
		{
			"I2Languages"
		};

		// Token: 0x04002162 RID: 8546
		private static string mCurrentDeviceLanguage;

		// Token: 0x04002163 RID: 8547
		public static List<ILocalizeTargetDescriptor> mLocalizeTargets = new List<ILocalizeTargetDescriptor>();

		// Token: 0x04002165 RID: 8549
		private static bool mLocalizeIsScheduled = false;

		// Token: 0x04002166 RID: 8550
		private static bool mLocalizeIsScheduledWithForcedValue = false;

		// Token: 0x04002167 RID: 8551
		public static bool HighlightLocalizedTargets = false;

		// Token: 0x02000823 RID: 2083
		// (Invoke) Token: 0x06004113 RID: 16659
		public delegate object _GetParam(string param);

		// Token: 0x02000824 RID: 2084
		// (Invoke) Token: 0x06004117 RID: 16663
		public delegate void OnLocalizeCallback();
	}
}
