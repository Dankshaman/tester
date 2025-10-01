using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

namespace I2.Loc
{
	// Token: 0x02000481 RID: 1153
	[ExecuteInEditMode]
	[Serializable]
	public class LanguageSourceData
	{
		// Token: 0x17000717 RID: 1815
		// (get) Token: 0x060033A7 RID: 13223 RVA: 0x0015B9FB File Offset: 0x00159BFB
		public UnityEngine.Object ownerObject
		{
			get
			{
				return this.owner as UnityEngine.Object;
			}
		}

		// Token: 0x1400007A RID: 122
		// (add) Token: 0x060033A8 RID: 13224 RVA: 0x0015BA08 File Offset: 0x00159C08
		// (remove) Token: 0x060033A9 RID: 13225 RVA: 0x0015BA40 File Offset: 0x00159C40
		public event LanguageSource.fnOnSourceUpdated Event_OnSourceUpdateFromGoogle;

		// Token: 0x060033AA RID: 13226 RVA: 0x0015BA75 File Offset: 0x00159C75
		public void Awake()
		{
			LocalizationManager.AddSource(this);
			this.UpdateDictionary(false);
			this.UpdateAssetDictionary();
			LocalizationManager.LocalizeAll(true);
		}

		// Token: 0x060033AB RID: 13227 RVA: 0x0015BA90 File Offset: 0x00159C90
		public void OnDestroy()
		{
			LocalizationManager.RemoveSource(this);
		}

		// Token: 0x060033AC RID: 13228 RVA: 0x0015BA98 File Offset: 0x00159C98
		public bool IsEqualTo(LanguageSourceData Source)
		{
			if (Source.mLanguages.Count != this.mLanguages.Count)
			{
				return false;
			}
			int i = 0;
			int count = this.mLanguages.Count;
			while (i < count)
			{
				if (Source.GetLanguageIndex(this.mLanguages[i].Name, true, true) < 0)
				{
					return false;
				}
				i++;
			}
			if (Source.mTerms.Count != this.mTerms.Count)
			{
				return false;
			}
			for (int j = 0; j < this.mTerms.Count; j++)
			{
				if (Source.GetTermData(this.mTerms[j].Term, false) == null)
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x060033AD RID: 13229 RVA: 0x0015BB44 File Offset: 0x00159D44
		internal bool ManagerHasASimilarSource()
		{
			int i = 0;
			int count = LocalizationManager.Sources.Count;
			while (i < count)
			{
				LanguageSourceData languageSourceData = LocalizationManager.Sources[i];
				if (languageSourceData != null && languageSourceData.IsEqualTo(this) && languageSourceData != this)
				{
					return true;
				}
				i++;
			}
			return false;
		}

		// Token: 0x060033AE RID: 13230 RVA: 0x0015BB87 File Offset: 0x00159D87
		public void ClearAllData()
		{
			this.mTerms.Clear();
			this.mLanguages.Clear();
			this.mDictionary.Clear();
			this.mAssetDictionary.Clear();
		}

		// Token: 0x060033AF RID: 13231 RVA: 0x0015BBB5 File Offset: 0x00159DB5
		public bool IsGlobalSource()
		{
			return this.mIsGlobalSource;
		}

		// Token: 0x060033B0 RID: 13232 RVA: 0x000025B8 File Offset: 0x000007B8
		public void Editor_SetDirty()
		{
		}

		// Token: 0x060033B1 RID: 13233 RVA: 0x0015BBC0 File Offset: 0x00159DC0
		public void UpdateAssetDictionary()
		{
			this.Assets.RemoveAll((UnityEngine.Object x) => x == null);
			this.mAssetDictionary = (from o in this.Assets.Distinct<UnityEngine.Object>()
			group o by o.name).ToDictionary((IGrouping<string, UnityEngine.Object> g) => g.Key, (IGrouping<string, UnityEngine.Object> g) => g.First<UnityEngine.Object>());
		}

		// Token: 0x060033B2 RID: 13234 RVA: 0x0015BC70 File Offset: 0x00159E70
		public UnityEngine.Object FindAsset(string Name)
		{
			if (this.Assets != null)
			{
				if (this.mAssetDictionary == null || this.mAssetDictionary.Count != this.Assets.Count)
				{
					this.UpdateAssetDictionary();
				}
				UnityEngine.Object result;
				if (this.mAssetDictionary.TryGetValue(Name, out result))
				{
					return result;
				}
			}
			return null;
		}

		// Token: 0x060033B3 RID: 13235 RVA: 0x0015BCBE File Offset: 0x00159EBE
		public bool HasAsset(UnityEngine.Object Obj)
		{
			return this.Assets.Contains(Obj);
		}

		// Token: 0x060033B4 RID: 13236 RVA: 0x0015BCCC File Offset: 0x00159ECC
		public void AddAsset(UnityEngine.Object Obj)
		{
			if (this.Assets.Contains(Obj))
			{
				return;
			}
			this.Assets.Add(Obj);
			this.UpdateAssetDictionary();
		}

		// Token: 0x060033B5 RID: 13237 RVA: 0x0015BCF0 File Offset: 0x00159EF0
		public string Export_I2CSV(string Category, char Separator = ',', bool specializationsAsRows = true)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("Key[*]Type[*]Desc");
			foreach (LanguageData languageData in this.mLanguages)
			{
				stringBuilder.Append("[*]");
				if (!languageData.IsEnabled())
				{
					stringBuilder.Append('$');
				}
				stringBuilder.Append(GoogleLanguages.GetCodedLanguage(languageData.Name, languageData.Code));
			}
			stringBuilder.Append("[ln]");
			this.mTerms.Sort((TermData a, TermData b) => string.CompareOrdinal(a.Term, b.Term));
			int count = this.mLanguages.Count;
			bool flag = true;
			foreach (TermData termData in this.mTerms)
			{
				string term;
				if (string.IsNullOrEmpty(Category) || (Category == LanguageSourceData.EmptyCategory && termData.Term.IndexOfAny(LanguageSourceData.CategorySeparators) < 0))
				{
					term = termData.Term;
				}
				else
				{
					if (!termData.Term.StartsWith(Category + "/") || !(Category != termData.Term))
					{
						continue;
					}
					term = termData.Term.Substring(Category.Length + 1);
				}
				if (!flag)
				{
					stringBuilder.Append("[ln]");
				}
				flag = false;
				if (!specializationsAsRows)
				{
					LanguageSourceData.AppendI2Term(stringBuilder, count, term, termData, Separator, null);
				}
				else
				{
					List<string> allSpecializations = termData.GetAllSpecializations();
					for (int i = 0; i < allSpecializations.Count; i++)
					{
						if (i != 0)
						{
							stringBuilder.Append("[ln]");
						}
						string forceSpecialization = allSpecializations[i];
						LanguageSourceData.AppendI2Term(stringBuilder, count, term, termData, Separator, forceSpecialization);
					}
				}
			}
			return stringBuilder.ToString();
		}

		// Token: 0x060033B6 RID: 13238 RVA: 0x0015BEF8 File Offset: 0x0015A0F8
		private static void AppendI2Term(StringBuilder Builder, int nLanguages, string Term, TermData termData, char Separator, string forceSpecialization)
		{
			LanguageSourceData.AppendI2Text(Builder, Term);
			if (!string.IsNullOrEmpty(forceSpecialization) && forceSpecialization != "Any")
			{
				Builder.Append("[");
				Builder.Append(forceSpecialization);
				Builder.Append("]");
			}
			Builder.Append("[*]");
			Builder.Append(termData.TermType.ToString());
			Builder.Append("[*]");
			Builder.Append(termData.Description);
			for (int i = 0; i < Mathf.Min(nLanguages, termData.Languages.Length); i++)
			{
				Builder.Append("[*]");
				string text = termData.Languages[i];
				if (!string.IsNullOrEmpty(forceSpecialization))
				{
					text = termData.GetTranslation(i, forceSpecialization, false);
				}
				LanguageSourceData.AppendI2Text(Builder, text);
			}
		}

		// Token: 0x060033B7 RID: 13239 RVA: 0x0015BFCA File Offset: 0x0015A1CA
		private static void AppendI2Text(StringBuilder Builder, string text)
		{
			if (string.IsNullOrEmpty(text))
			{
				return;
			}
			if (text.StartsWith("'") || text.StartsWith("="))
			{
				Builder.Append('\'');
			}
			Builder.Append(text);
		}

		// Token: 0x060033B8 RID: 13240 RVA: 0x0015C000 File Offset: 0x0015A200
		private string Export_Language_to_Cache(int langIndex, bool fillTermWithFallback)
		{
			if (!this.mLanguages[langIndex].IsLoaded())
			{
				return null;
			}
			StringBuilder stringBuilder = new StringBuilder();
			for (int i = 0; i < this.mTerms.Count; i++)
			{
				if (i > 0)
				{
					stringBuilder.Append("[i2t]");
				}
				TermData termData = this.mTerms[i];
				stringBuilder.Append(termData.Term);
				stringBuilder.Append("=");
				string text = termData.Languages[langIndex];
				if (this.OnMissingTranslation == LanguageSourceData.MissingTranslationAction.Fallback && string.IsNullOrEmpty(text) && this.TryGetFallbackTranslation(termData, out text, langIndex, null, true))
				{
					stringBuilder.Append("[i2fb]");
					if (fillTermWithFallback)
					{
						termData.Languages[langIndex] = text;
					}
				}
				if (!string.IsNullOrEmpty(text))
				{
					stringBuilder.Append(text);
				}
			}
			return stringBuilder.ToString();
		}

		// Token: 0x060033B9 RID: 13241 RVA: 0x0015C0D0 File Offset: 0x0015A2D0
		public string Export_CSV(string Category, char Separator = ',', bool specializationsAsRows = true)
		{
			StringBuilder stringBuilder = new StringBuilder();
			int count = this.mLanguages.Count;
			stringBuilder.AppendFormat("Key{0}Type{0}Desc", Separator);
			foreach (LanguageData languageData in this.mLanguages)
			{
				stringBuilder.Append(Separator);
				if (!languageData.IsEnabled())
				{
					stringBuilder.Append('$');
				}
				LanguageSourceData.AppendString(stringBuilder, GoogleLanguages.GetCodedLanguage(languageData.Name, languageData.Code), Separator);
			}
			stringBuilder.Append("\n");
			this.mTerms.Sort((TermData a, TermData b) => string.CompareOrdinal(a.Term, b.Term));
			foreach (TermData termData in this.mTerms)
			{
				string term;
				if (string.IsNullOrEmpty(Category) || (Category == LanguageSourceData.EmptyCategory && termData.Term.IndexOfAny(LanguageSourceData.CategorySeparators) < 0))
				{
					term = termData.Term;
				}
				else
				{
					if (!termData.Term.StartsWith(Category + "/") || !(Category != termData.Term))
					{
						continue;
					}
					term = termData.Term.Substring(Category.Length + 1);
				}
				if (specializationsAsRows)
				{
					using (List<string>.Enumerator enumerator3 = termData.GetAllSpecializations().GetEnumerator())
					{
						while (enumerator3.MoveNext())
						{
							string specialization = enumerator3.Current;
							LanguageSourceData.AppendTerm(stringBuilder, count, term, termData, specialization, Separator);
						}
						continue;
					}
				}
				LanguageSourceData.AppendTerm(stringBuilder, count, term, termData, null, Separator);
			}
			return stringBuilder.ToString();
		}

		// Token: 0x060033BA RID: 13242 RVA: 0x0015C2C0 File Offset: 0x0015A4C0
		private static void AppendTerm(StringBuilder Builder, int nLanguages, string Term, TermData termData, string specialization, char Separator)
		{
			LanguageSourceData.AppendString(Builder, Term, Separator);
			if (!string.IsNullOrEmpty(specialization) && specialization != "Any")
			{
				Builder.AppendFormat("[{0}]", specialization);
			}
			Builder.Append(Separator);
			Builder.Append(termData.TermType.ToString());
			Builder.Append(Separator);
			LanguageSourceData.AppendString(Builder, termData.Description, Separator);
			for (int i = 0; i < Mathf.Min(nLanguages, termData.Languages.Length); i++)
			{
				Builder.Append(Separator);
				string text = termData.Languages[i];
				if (!string.IsNullOrEmpty(specialization))
				{
					text = termData.GetTranslation(i, specialization, false);
				}
				LanguageSourceData.AppendTranslation(Builder, text, Separator, null);
			}
			Builder.Append("\n");
		}

		// Token: 0x060033BB RID: 13243 RVA: 0x0015C388 File Offset: 0x0015A588
		private static void AppendString(StringBuilder Builder, string Text, char Separator)
		{
			if (string.IsNullOrEmpty(Text))
			{
				return;
			}
			Text = Text.Replace("\\n", "\n");
			if (Text.IndexOfAny((Separator.ToString() + "\n\"").ToCharArray()) >= 0)
			{
				Text = Text.Replace("\"", "\"\"");
				Builder.AppendFormat("\"{0}\"", Text);
				return;
			}
			Builder.Append(Text);
		}

		// Token: 0x060033BC RID: 13244 RVA: 0x0015C3F8 File Offset: 0x0015A5F8
		private static void AppendTranslation(StringBuilder Builder, string Text, char Separator, string tags)
		{
			if (string.IsNullOrEmpty(Text))
			{
				return;
			}
			Text = Text.Replace("\\n", "\n");
			if (Text.IndexOfAny((Separator.ToString() + "\n\"").ToCharArray()) >= 0)
			{
				Text = Text.Replace("\"", "\"\"");
				Builder.AppendFormat("\"{0}{1}\"", tags, Text);
				return;
			}
			Builder.Append(tags);
			Builder.Append(Text);
		}

		// Token: 0x060033BD RID: 13245 RVA: 0x0015C470 File Offset: 0x0015A670
		public UnityWebRequest Export_Google_CreateWWWcall(eSpreadsheetUpdateMode UpdateMode = eSpreadsheetUpdateMode.Replace)
		{
			string value = this.Export_Google_CreateData();
			WWWForm wwwform = new WWWForm();
			wwwform.AddField("key", this.Google_SpreadsheetKey);
			wwwform.AddField("action", "SetLanguageSource");
			wwwform.AddField("data", value);
			wwwform.AddField("updateMode", UpdateMode.ToString());
			UnityWebRequest unityWebRequest = UnityWebRequest.Post(LocalizationManager.GetWebServiceURL(this), wwwform);
			I2Utils.SendWebRequest(unityWebRequest);
			return unityWebRequest;
		}

		// Token: 0x060033BE RID: 13246 RVA: 0x0015C4E4 File Offset: 0x0015A6E4
		private string Export_Google_CreateData()
		{
			List<string> categories = this.GetCategories(true, null);
			StringBuilder stringBuilder = new StringBuilder();
			bool flag = true;
			foreach (string text in categories)
			{
				if (flag)
				{
					flag = false;
				}
				else
				{
					stringBuilder.Append("<I2Loc>");
				}
				bool specializationsAsRows = true;
				string value = this.Export_I2CSV(text, ',', specializationsAsRows);
				stringBuilder.Append(text);
				stringBuilder.Append("<I2Loc>");
				stringBuilder.Append(value);
			}
			return stringBuilder.ToString();
		}

		// Token: 0x060033BF RID: 13247 RVA: 0x0015C580 File Offset: 0x0015A780
		public string Import_CSV(string Category, string CSVstring, eSpreadsheetUpdateMode UpdateMode = eSpreadsheetUpdateMode.Replace, char Separator = ',')
		{
			List<string[]> csv = LocalizationReader.ReadCSV(CSVstring, Separator);
			return this.Import_CSV(Category, csv, UpdateMode);
		}

		// Token: 0x060033C0 RID: 13248 RVA: 0x0015C5A0 File Offset: 0x0015A7A0
		public string Import_I2CSV(string Category, string I2CSVstring, eSpreadsheetUpdateMode UpdateMode = eSpreadsheetUpdateMode.Replace)
		{
			List<string[]> csv = LocalizationReader.ReadI2CSV(I2CSVstring);
			return this.Import_CSV(Category, csv, UpdateMode);
		}

		// Token: 0x060033C1 RID: 13249 RVA: 0x0015C5C0 File Offset: 0x0015A7C0
		public string Import_CSV(string Category, List<string[]> CSV, eSpreadsheetUpdateMode UpdateMode = eSpreadsheetUpdateMode.Replace)
		{
			string[] array = CSV[0];
			int num = 1;
			int num2 = -1;
			int num3 = -1;
			string[] texts = new string[]
			{
				"Key"
			};
			string[] texts2 = new string[]
			{
				"Type"
			};
			string[] texts3 = new string[]
			{
				"Desc",
				"Description"
			};
			if (array.Length > 1 && this.ArrayContains(array[0], texts))
			{
				if (UpdateMode == eSpreadsheetUpdateMode.Replace)
				{
					this.ClearAllData();
				}
				if (array.Length > 2)
				{
					if (this.ArrayContains(array[1], texts2))
					{
						num2 = 1;
						num = 2;
					}
					if (this.ArrayContains(array[1], texts3))
					{
						num3 = 1;
						num = 2;
					}
				}
				if (array.Length > 3)
				{
					if (this.ArrayContains(array[2], texts2))
					{
						num2 = 2;
						num = 3;
					}
					if (this.ArrayContains(array[2], texts3))
					{
						num3 = 2;
						num = 3;
					}
				}
				int num4 = Mathf.Max(array.Length - num, 0);
				int[] array2 = new int[num4];
				for (int i = 0; i < num4; i++)
				{
					if (string.IsNullOrEmpty(array[i + num]))
					{
						array2[i] = -1;
					}
					else
					{
						string text = array[i + num];
						bool flag = true;
						if (text.StartsWith("$"))
						{
							flag = false;
							text = text.Substring(1);
						}
						string text2;
						string text3;
						GoogleLanguages.UnPackCodeFromLanguageName(text, out text2, out text3);
						int num5;
						if (!string.IsNullOrEmpty(text3))
						{
							num5 = this.GetLanguageIndexFromCode(text3, true, false);
						}
						else
						{
							num5 = this.GetLanguageIndex(text2, true, false);
						}
						if (num5 < 0)
						{
							LanguageData languageData = new LanguageData();
							languageData.Name = text2;
							languageData.Code = text3;
							languageData.Flags = (byte)(0 | (flag ? 0 : 1));
							this.mLanguages.Add(languageData);
							num5 = this.mLanguages.Count - 1;
						}
						array2[i] = num5;
					}
				}
				num4 = this.mLanguages.Count;
				int j = 0;
				int count = this.mTerms.Count;
				while (j < count)
				{
					TermData termData = this.mTerms[j];
					if (termData.Languages.Length < num4)
					{
						Array.Resize<string>(ref termData.Languages, num4);
						Array.Resize<byte>(ref termData.Flags, num4);
					}
					j++;
				}
				int k = 1;
				int count2 = CSV.Count;
				while (k < count2)
				{
					array = CSV[k];
					string text4 = string.IsNullOrEmpty(Category) ? array[0] : (Category + "/" + array[0]);
					string text5 = null;
					if (text4.EndsWith("]"))
					{
						int num6 = text4.LastIndexOf('[');
						if (num6 > 0)
						{
							text5 = text4.Substring(num6 + 1, text4.Length - num6 - 2);
							if (text5 == "touch")
							{
								text5 = "Touch";
							}
							text4 = text4.Remove(num6);
						}
					}
					LanguageSourceData.ValidateFullTerm(ref text4);
					if (!string.IsNullOrEmpty(text4))
					{
						TermData termData2 = this.GetTermData(text4, false);
						if (termData2 == null)
						{
							termData2 = new TermData();
							termData2.Term = text4;
							termData2.Languages = new string[this.mLanguages.Count];
							termData2.Flags = new byte[this.mLanguages.Count];
							for (int l = 0; l < this.mLanguages.Count; l++)
							{
								termData2.Languages[l] = string.Empty;
							}
							this.mTerms.Add(termData2);
							this.mDictionary.Add(text4, termData2);
						}
						else if (UpdateMode == eSpreadsheetUpdateMode.AddNewTerms)
						{
							goto IL_3E1;
						}
						if (num2 > 0)
						{
							termData2.TermType = LanguageSourceData.GetTermType(array[num2]);
						}
						if (num3 > 0)
						{
							termData2.Description = array[num3];
						}
						int num7 = 0;
						while (num7 < array2.Length && num7 < array.Length - num)
						{
							if (!string.IsNullOrEmpty(array[num7 + num]))
							{
								int num8 = array2[num7];
								if (num8 >= 0)
								{
									string text6 = array[num7 + num];
									if (text6 == "-")
									{
										text6 = string.Empty;
									}
									else if (text6 == "")
									{
										text6 = null;
									}
									termData2.SetTranslation(num8, text6, text5);
								}
							}
							num7++;
						}
					}
					IL_3E1:
					k++;
				}
				if (Application.isPlaying)
				{
					this.SaveLanguages(this.HasUnloadedLanguages(), PersistentStorage.eFileType.Temporal);
				}
				return string.Empty;
			}
			return "Bad Spreadsheet Format.\nFirst columns should be 'Key', 'Type' and 'Desc'";
		}

		// Token: 0x060033C2 RID: 13250 RVA: 0x0015C9D8 File Offset: 0x0015ABD8
		private bool ArrayContains(string MainText, params string[] texts)
		{
			int i = 0;
			int num = texts.Length;
			while (i < num)
			{
				if (MainText.IndexOf(texts[i], StringComparison.OrdinalIgnoreCase) >= 0)
				{
					return true;
				}
				i++;
			}
			return false;
		}

		// Token: 0x060033C3 RID: 13251 RVA: 0x0015CA08 File Offset: 0x0015AC08
		public static eTermType GetTermType(string type)
		{
			int i = 0;
			int num = 12;
			while (i <= num)
			{
				eTermType eTermType = (eTermType)i;
				if (string.Equals(eTermType.ToString(), type, StringComparison.OrdinalIgnoreCase))
				{
					return (eTermType)i;
				}
				i++;
			}
			return eTermType.Text;
		}

		// Token: 0x060033C4 RID: 13252 RVA: 0x0015CA40 File Offset: 0x0015AC40
		private void Import_Language_from_Cache(int langIndex, string langData, bool useFallback, bool onlyCurrentSpecialization)
		{
			int num;
			for (int i = 0; i < langData.Length; i = num + 5)
			{
				num = langData.IndexOf("[i2t]", i);
				if (num < 0)
				{
					num = langData.Length;
				}
				int num2 = langData.IndexOf("=", i);
				if (num2 >= num)
				{
					return;
				}
				string term = langData.Substring(i, num2 - i);
				i = num2 + 1;
				TermData termData = this.GetTermData(term, false);
				if (termData != null)
				{
					string text = null;
					if (i != num)
					{
						text = langData.Substring(i, num - i);
						if (text.StartsWith("[i2fb]"))
						{
							text = (useFallback ? text.Substring(6) : null);
						}
						if (onlyCurrentSpecialization && text != null)
						{
							text = SpecializationManager.GetSpecializedText(text, null);
						}
					}
					termData.Languages[langIndex] = text;
				}
			}
		}

		// Token: 0x060033C5 RID: 13253 RVA: 0x0015CAFC File Offset: 0x0015ACFC
		public static void FreeUnusedLanguages()
		{
			LanguageSourceData languageSourceData = LocalizationManager.Sources[0];
			int languageIndex = languageSourceData.GetLanguageIndex(LocalizationManager.CurrentLanguage, true, true);
			for (int i = 0; i < languageSourceData.mTerms.Count; i++)
			{
				TermData termData = languageSourceData.mTerms[i];
				for (int j = 0; j < termData.Languages.Length; j++)
				{
					if (j != languageIndex)
					{
						termData.Languages[j] = null;
					}
				}
			}
		}

		// Token: 0x060033C6 RID: 13254 RVA: 0x0015CB6C File Offset: 0x0015AD6C
		public void Import_Google_FromCache()
		{
			if (this.GoogleUpdateFrequency == LanguageSourceData.eGoogleUpdateFrequency.Never)
			{
				return;
			}
			if (!I2Utils.IsPlaying())
			{
				return;
			}
			string sourcePlayerPrefName = this.GetSourcePlayerPrefName();
			string text = PersistentStorage.LoadFile(PersistentStorage.eFileType.Persistent, "I2Source_" + sourcePlayerPrefName + ".loc", false);
			if (string.IsNullOrEmpty(text))
			{
				return;
			}
			if (text.StartsWith("[i2e]", StringComparison.Ordinal))
			{
				text = StringObfucator.Decode(text.Substring(5, text.Length - 5));
			}
			bool flag = false;
			string text2 = this.Google_LastUpdatedVersion;
			if (PersistentStorage.HasSetting("I2SourceVersion_" + sourcePlayerPrefName))
			{
				text2 = PersistentStorage.GetSetting_String("I2SourceVersion_" + sourcePlayerPrefName, this.Google_LastUpdatedVersion);
				flag = this.IsNewerVersion(this.Google_LastUpdatedVersion, text2);
			}
			if (!flag)
			{
				PersistentStorage.DeleteFile(PersistentStorage.eFileType.Persistent, "I2Source_" + sourcePlayerPrefName + ".loc", false);
				PersistentStorage.DeleteSetting("I2SourceVersion_" + sourcePlayerPrefName);
				return;
			}
			if (text2.Length > 19)
			{
				text2 = string.Empty;
			}
			this.Google_LastUpdatedVersion = text2;
			this.Import_Google_Result(text, eSpreadsheetUpdateMode.Replace, false);
		}

		// Token: 0x060033C7 RID: 13255 RVA: 0x0015CC64 File Offset: 0x0015AE64
		private bool IsNewerVersion(string currentVersion, string newVersion)
		{
			long num;
			long num2;
			return !string.IsNullOrEmpty(newVersion) && (string.IsNullOrEmpty(currentVersion) || (!long.TryParse(newVersion, out num) || !long.TryParse(currentVersion, out num2)) || num > num2);
		}

		// Token: 0x060033C8 RID: 13256 RVA: 0x0015CCA0 File Offset: 0x0015AEA0
		public void Import_Google(bool ForceUpdate, bool justCheck)
		{
			if (!ForceUpdate && this.GoogleUpdateFrequency == LanguageSourceData.eGoogleUpdateFrequency.Never)
			{
				return;
			}
			if (!I2Utils.IsPlaying())
			{
				return;
			}
			LanguageSourceData.eGoogleUpdateFrequency googleUpdateFrequency = this.GoogleUpdateFrequency;
			string sourcePlayerPrefName = this.GetSourcePlayerPrefName();
			if (!ForceUpdate && googleUpdateFrequency != LanguageSourceData.eGoogleUpdateFrequency.Always)
			{
				string setting_String = PersistentStorage.GetSetting_String("LastGoogleUpdate_" + sourcePlayerPrefName, "");
				try
				{
					DateTime d;
					if (DateTime.TryParse(setting_String, out d))
					{
						double totalDays = (DateTime.Now - d).TotalDays;
						switch (googleUpdateFrequency)
						{
						case LanguageSourceData.eGoogleUpdateFrequency.Daily:
							if (totalDays >= 1.0)
							{
								goto IL_BF;
							}
							break;
						case LanguageSourceData.eGoogleUpdateFrequency.Weekly:
							if (totalDays >= 8.0)
							{
								goto IL_BF;
							}
							break;
						case LanguageSourceData.eGoogleUpdateFrequency.Monthly:
							if (totalDays >= 31.0)
							{
								goto IL_BF;
							}
							break;
						case LanguageSourceData.eGoogleUpdateFrequency.OnlyOnce:
							break;
						case LanguageSourceData.eGoogleUpdateFrequency.EveryOtherDay:
							if (totalDays >= 2.0)
							{
								goto IL_BF;
							}
							break;
						default:
							goto IL_BF;
						}
						return;
					}
					IL_BF:;
				}
				catch (Exception)
				{
				}
			}
			PersistentStorage.SetSetting_String("LastGoogleUpdate_" + sourcePlayerPrefName, DateTime.Now.ToString());
			CoroutineManager.Start(this.Import_Google_Coroutine(justCheck));
		}

		// Token: 0x060033C9 RID: 13257 RVA: 0x0015CDAC File Offset: 0x0015AFAC
		private string GetSourcePlayerPrefName()
		{
			if (this.owner == null)
			{
				return null;
			}
			string text = (this.owner as UnityEngine.Object).name;
			if (!string.IsNullOrEmpty(this.Google_SpreadsheetKey))
			{
				text += this.Google_SpreadsheetKey;
			}
			if (Array.IndexOf<string>(LocalizationManager.GlobalSources, (this.owner as UnityEngine.Object).name) >= 0)
			{
				return text;
			}
			return SceneManager.GetActiveScene().name + "_" + text;
		}

		// Token: 0x060033CA RID: 13258 RVA: 0x0015CE25 File Offset: 0x0015B025
		private IEnumerator Import_Google_Coroutine(bool JustCheck)
		{
			UnityWebRequest www = this.Import_Google_CreateWWWcall(false, JustCheck);
			if (www == null)
			{
				yield break;
			}
			while (!www.isDone)
			{
				yield return null;
			}
			if (string.IsNullOrEmpty(www.error))
			{
				byte[] data = www.downloadHandler.data;
				string @string = Encoding.UTF8.GetString(data, 0, data.Length);
				bool flag = string.IsNullOrEmpty(@string) || @string == "\"\"";
				if (JustCheck)
				{
					if (!flag)
					{
						Debug.LogWarning("Spreadsheet is not up-to-date and Google Live Synchronization is enabled\nWhen playing in the device the Spreadsheet will be downloaded and translations may not behave as what you see in the editor.\nTo fix this, Import or Export replace to Google");
						this.GoogleLiveSyncIsUptoDate = false;
					}
					yield break;
				}
				if (!flag)
				{
					this.mDelayedGoogleData = @string;
					switch (this.GoogleUpdateSynchronization)
					{
					case LanguageSourceData.eGoogleUpdateSynchronization.OnSceneLoaded:
						SceneManager.sceneLoaded += this.ApplyDownloadedDataOnSceneLoaded;
						break;
					case LanguageSourceData.eGoogleUpdateSynchronization.AsSoonAsDownloaded:
						this.ApplyDownloadedDataFromGoogle();
						break;
					}
					yield break;
				}
			}
			if (this.Event_OnSourceUpdateFromGoogle != null)
			{
				this.Event_OnSourceUpdateFromGoogle(this, false, www.error);
			}
			Debug.Log("Language Source was up-to-date with Google Spreadsheet");
			yield break;
		}

		// Token: 0x060033CB RID: 13259 RVA: 0x0015CE3B File Offset: 0x0015B03B
		private void ApplyDownloadedDataOnSceneLoaded(Scene scene, LoadSceneMode mode)
		{
			SceneManager.sceneLoaded -= this.ApplyDownloadedDataOnSceneLoaded;
			this.ApplyDownloadedDataFromGoogle();
		}

		// Token: 0x060033CC RID: 13260 RVA: 0x0015CE54 File Offset: 0x0015B054
		public void ApplyDownloadedDataFromGoogle()
		{
			if (string.IsNullOrEmpty(this.mDelayedGoogleData))
			{
				return;
			}
			if (string.IsNullOrEmpty(this.Import_Google_Result(this.mDelayedGoogleData, eSpreadsheetUpdateMode.Replace, true)))
			{
				if (this.Event_OnSourceUpdateFromGoogle != null)
				{
					this.Event_OnSourceUpdateFromGoogle(this, true, "");
				}
				LocalizationManager.LocalizeAll(true);
				Debug.Log("Done Google Sync");
				return;
			}
			if (this.Event_OnSourceUpdateFromGoogle != null)
			{
				this.Event_OnSourceUpdateFromGoogle(this, false, "");
			}
			Debug.Log("Done Google Sync: source was up-to-date");
		}

		// Token: 0x060033CD RID: 13261 RVA: 0x0015CED4 File Offset: 0x0015B0D4
		public UnityWebRequest Import_Google_CreateWWWcall(bool ForceUpdate, bool justCheck)
		{
			if (!this.HasGoogleSpreadsheet())
			{
				return null;
			}
			string text = PersistentStorage.GetSetting_String("I2SourceVersion_" + this.GetSourcePlayerPrefName(), this.Google_LastUpdatedVersion);
			if (text.Length > 19)
			{
				text = string.Empty;
			}
			if (this.IsNewerVersion(text, this.Google_LastUpdatedVersion))
			{
				this.Google_LastUpdatedVersion = text;
			}
			UnityWebRequest unityWebRequest = UnityWebRequest.Get(string.Format("{0}?key={1}&action=GetLanguageSource&version={2}", LocalizationManager.GetWebServiceURL(this), this.Google_SpreadsheetKey, ForceUpdate ? "0" : this.Google_LastUpdatedVersion));
			I2Utils.SendWebRequest(unityWebRequest);
			return unityWebRequest;
		}

		// Token: 0x060033CE RID: 13262 RVA: 0x0015CF5E File Offset: 0x0015B15E
		public bool HasGoogleSpreadsheet()
		{
			return !string.IsNullOrEmpty(this.Google_WebServiceURL) && !string.IsNullOrEmpty(this.Google_SpreadsheetKey) && !string.IsNullOrEmpty(LocalizationManager.GetWebServiceURL(this));
		}

		// Token: 0x060033CF RID: 13263 RVA: 0x0015CF8C File Offset: 0x0015B18C
		public string Import_Google_Result(string JsonString, eSpreadsheetUpdateMode UpdateMode, bool saveInPlayerPrefs = false)
		{
			string result;
			try
			{
				string empty = string.Empty;
				if (string.IsNullOrEmpty(JsonString) || JsonString == "\"\"")
				{
					result = empty;
				}
				else
				{
					int num = JsonString.IndexOf("version=", StringComparison.Ordinal);
					int num2 = JsonString.IndexOf("script_version=", StringComparison.Ordinal);
					if (num < 0 || num2 < 0)
					{
						result = "Invalid Response from Google, Most likely the WebService needs to be updated";
					}
					else
					{
						num += "version=".Length;
						num2 += "script_version=".Length;
						string text = JsonString.Substring(num, JsonString.IndexOf(",", num, StringComparison.Ordinal) - num);
						int num3 = int.Parse(JsonString.Substring(num2, JsonString.IndexOf(",", num2, StringComparison.Ordinal) - num2));
						if (text.Length > 19)
						{
							text = string.Empty;
						}
						if (num3 != LocalizationManager.GetRequiredWebServiceVersion())
						{
							result = "The current Google WebService is not supported.\nPlease, delete the WebService from the Google Drive and Install the latest version.";
						}
						else if (saveInPlayerPrefs && !this.IsNewerVersion(this.Google_LastUpdatedVersion, text))
						{
							result = "LanguageSource is up-to-date";
						}
						else
						{
							if (saveInPlayerPrefs)
							{
								string sourcePlayerPrefName = this.GetSourcePlayerPrefName();
								PersistentStorage.SaveFile(PersistentStorage.eFileType.Persistent, "I2Source_" + sourcePlayerPrefName + ".loc", "[i2e]" + StringObfucator.Encode(JsonString), true);
								PersistentStorage.SetSetting_String("I2SourceVersion_" + sourcePlayerPrefName, text);
								PersistentStorage.ForceSaveSettings();
							}
							this.Google_LastUpdatedVersion = text;
							if (UpdateMode == eSpreadsheetUpdateMode.Replace)
							{
								this.ClearAllData();
							}
							int i = JsonString.IndexOf("[i2category]", StringComparison.Ordinal);
							while (i > 0)
							{
								i += "[i2category]".Length;
								int num4 = JsonString.IndexOf("[/i2category]", i, StringComparison.Ordinal);
								string category = JsonString.Substring(i, num4 - i);
								num4 += "[/i2category]".Length;
								int num5 = JsonString.IndexOf("[/i2csv]", num4, StringComparison.Ordinal);
								string i2CSVstring = JsonString.Substring(num4, num5 - num4);
								i = JsonString.IndexOf("[i2category]", num5, StringComparison.Ordinal);
								this.Import_I2CSV(category, i2CSVstring, UpdateMode);
								if (UpdateMode == eSpreadsheetUpdateMode.Replace)
								{
									UpdateMode = eSpreadsheetUpdateMode.Merge;
								}
							}
							this.GoogleLiveSyncIsUptoDate = true;
							if (I2Utils.IsPlaying())
							{
								this.SaveLanguages(true, PersistentStorage.eFileType.Temporal);
							}
							if (!string.IsNullOrEmpty(empty))
							{
								this.Editor_SetDirty();
							}
							result = empty;
						}
					}
				}
			}
			catch (Exception ex)
			{
				Debug.LogWarning(ex);
				result = ex.ToString();
			}
			return result;
		}

		// Token: 0x060033D0 RID: 13264 RVA: 0x0015D1C0 File Offset: 0x0015B3C0
		public int GetLanguageIndex(string language, bool AllowDiscartingRegion = true, bool SkipDisabled = true)
		{
			int i = 0;
			int count = this.mLanguages.Count;
			while (i < count)
			{
				if ((!SkipDisabled || this.mLanguages[i].IsEnabled()) && string.Compare(this.mLanguages[i].Name, language, StringComparison.OrdinalIgnoreCase) == 0)
				{
					return i;
				}
				i++;
			}
			if (AllowDiscartingRegion)
			{
				int num = -1;
				int num2 = 0;
				int j = 0;
				int count2 = this.mLanguages.Count;
				while (j < count2)
				{
					if (!SkipDisabled || this.mLanguages[j].IsEnabled())
					{
						int commonWordInLanguageNames = LanguageSourceData.GetCommonWordInLanguageNames(this.mLanguages[j].Name, language);
						if (commonWordInLanguageNames > num2)
						{
							num2 = commonWordInLanguageNames;
							num = j;
						}
					}
					j++;
				}
				if (num >= 0)
				{
					return num;
				}
			}
			return -1;
		}

		// Token: 0x060033D1 RID: 13265 RVA: 0x0015D280 File Offset: 0x0015B480
		public LanguageData GetLanguageData(string language, bool AllowDiscartingRegion = true)
		{
			int languageIndex = this.GetLanguageIndex(language, AllowDiscartingRegion, false);
			if (languageIndex >= 0)
			{
				return this.mLanguages[languageIndex];
			}
			return null;
		}

		// Token: 0x060033D2 RID: 13266 RVA: 0x0015D2A9 File Offset: 0x0015B4A9
		public bool IsCurrentLanguage(int languageIndex)
		{
			return LocalizationManager.CurrentLanguage == this.mLanguages[languageIndex].Name;
		}

		// Token: 0x060033D3 RID: 13267 RVA: 0x0015D2C8 File Offset: 0x0015B4C8
		public int GetLanguageIndexFromCode(string Code, bool exactMatch = true, bool ignoreDisabled = false)
		{
			int i = 0;
			int count = this.mLanguages.Count;
			while (i < count)
			{
				if ((!ignoreDisabled || this.mLanguages[i].IsEnabled()) && string.Compare(this.mLanguages[i].Code, Code, StringComparison.OrdinalIgnoreCase) == 0)
				{
					return i;
				}
				i++;
			}
			if (!exactMatch)
			{
				int j = 0;
				int count2 = this.mLanguages.Count;
				while (j < count2)
				{
					if ((!ignoreDisabled || this.mLanguages[j].IsEnabled()) && string.Compare(this.mLanguages[j].Code, 0, Code, 0, 2, StringComparison.OrdinalIgnoreCase) == 0)
					{
						return j;
					}
					j++;
				}
			}
			return -1;
		}

		// Token: 0x060033D4 RID: 13268 RVA: 0x0015D370 File Offset: 0x0015B570
		public static int GetCommonWordInLanguageNames(string Language1, string Language2)
		{
			if (string.IsNullOrEmpty(Language1) || string.IsNullOrEmpty(Language2))
			{
				return 0;
			}
			char[] separator = "( )-/\\".ToCharArray();
			string[] array = Language1.ToLower().Split(separator);
			string[] array2 = Language2.ToLower().Split(separator);
			int num = 0;
			foreach (string value in array)
			{
				if (!string.IsNullOrEmpty(value) && array2.Contains(value))
				{
					num++;
				}
			}
			foreach (string value2 in array2)
			{
				if (!string.IsNullOrEmpty(value2) && array.Contains(value2))
				{
					num++;
				}
			}
			return num;
		}

		// Token: 0x060033D5 RID: 13269 RVA: 0x0015D41F File Offset: 0x0015B61F
		public static bool AreTheSameLanguage(string Language1, string Language2)
		{
			Language1 = LanguageSourceData.GetLanguageWithoutRegion(Language1);
			Language2 = LanguageSourceData.GetLanguageWithoutRegion(Language2);
			return string.Compare(Language1, Language2, StringComparison.OrdinalIgnoreCase) == 0;
		}

		// Token: 0x060033D6 RID: 13270 RVA: 0x0015D43C File Offset: 0x0015B63C
		public static string GetLanguageWithoutRegion(string Language)
		{
			int num = Language.IndexOfAny("(/\\[,{".ToCharArray());
			if (num < 0)
			{
				return Language;
			}
			return Language.Substring(0, num).Trim();
		}

		// Token: 0x060033D7 RID: 13271 RVA: 0x0015D46D File Offset: 0x0015B66D
		public void AddLanguage(string LanguageName)
		{
			this.AddLanguage(LanguageName, GoogleLanguages.GetLanguageCode(LanguageName, false));
		}

		// Token: 0x060033D8 RID: 13272 RVA: 0x0015D480 File Offset: 0x0015B680
		public void AddLanguage(string LanguageName, string LanguageCode)
		{
			if (this.GetLanguageIndex(LanguageName, false, true) >= 0)
			{
				return;
			}
			LanguageData languageData = new LanguageData();
			languageData.Name = LanguageName;
			languageData.Code = LanguageCode;
			this.mLanguages.Add(languageData);
			int count = this.mLanguages.Count;
			int i = 0;
			int count2 = this.mTerms.Count;
			while (i < count2)
			{
				Array.Resize<string>(ref this.mTerms[i].Languages, count);
				Array.Resize<byte>(ref this.mTerms[i].Flags, count);
				i++;
			}
			this.Editor_SetDirty();
		}

		// Token: 0x060033D9 RID: 13273 RVA: 0x0015D514 File Offset: 0x0015B714
		public void RemoveLanguage(string LanguageName)
		{
			int languageIndex = this.GetLanguageIndex(LanguageName, false, false);
			if (languageIndex < 0)
			{
				return;
			}
			int count = this.mLanguages.Count;
			int i = 0;
			int count2 = this.mTerms.Count;
			while (i < count2)
			{
				for (int j = languageIndex + 1; j < count; j++)
				{
					this.mTerms[i].Languages[j - 1] = this.mTerms[i].Languages[j];
					this.mTerms[i].Flags[j - 1] = this.mTerms[i].Flags[j];
				}
				Array.Resize<string>(ref this.mTerms[i].Languages, count - 1);
				Array.Resize<byte>(ref this.mTerms[i].Flags, count - 1);
				i++;
			}
			this.mLanguages.RemoveAt(languageIndex);
			this.Editor_SetDirty();
		}

		// Token: 0x060033DA RID: 13274 RVA: 0x0015D604 File Offset: 0x0015B804
		public List<string> GetLanguages(bool skipDisabled = true)
		{
			List<string> list = new List<string>();
			int i = 0;
			int count = this.mLanguages.Count;
			while (i < count)
			{
				if (!skipDisabled || this.mLanguages[i].IsEnabled())
				{
					list.Add(this.mLanguages[i].Name);
				}
				i++;
			}
			return list;
		}

		// Token: 0x060033DB RID: 13275 RVA: 0x0015D660 File Offset: 0x0015B860
		public List<string> GetLanguagesCode(bool allowRegions = true, bool skipDisabled = true)
		{
			List<string> list = new List<string>();
			int i = 0;
			int count = this.mLanguages.Count;
			while (i < count)
			{
				if (!skipDisabled || this.mLanguages[i].IsEnabled())
				{
					string text = this.mLanguages[i].Code;
					if (!allowRegions && text != null && text.Length > 2)
					{
						text = text.Substring(0, 2);
					}
					if (!string.IsNullOrEmpty(text) && !list.Contains(text))
					{
						list.Add(text);
					}
				}
				i++;
			}
			return list;
		}

		// Token: 0x060033DC RID: 13276 RVA: 0x0015D6E4 File Offset: 0x0015B8E4
		public bool IsLanguageEnabled(string Language)
		{
			int languageIndex = this.GetLanguageIndex(Language, false, true);
			return languageIndex >= 0 && this.mLanguages[languageIndex].IsEnabled();
		}

		// Token: 0x060033DD RID: 13277 RVA: 0x0015D714 File Offset: 0x0015B914
		public void EnableLanguage(string Language, bool bEnabled)
		{
			int languageIndex = this.GetLanguageIndex(Language, false, false);
			if (languageIndex >= 0)
			{
				this.mLanguages[languageIndex].SetEnabled(bEnabled);
			}
		}

		// Token: 0x060033DE RID: 13278 RVA: 0x0015D741 File Offset: 0x0015B941
		public bool AllowUnloadingLanguages()
		{
			return this._AllowUnloadingLanguages > LanguageSourceData.eAllowUnloadLanguages.Never;
		}

		// Token: 0x060033DF RID: 13279 RVA: 0x0015D74C File Offset: 0x0015B94C
		private string GetSavedLanguageFileName(int languageIndex)
		{
			if (languageIndex < 0)
			{
				return null;
			}
			return string.Concat(new string[]
			{
				"LangSource_",
				this.GetSourcePlayerPrefName(),
				"_",
				this.mLanguages[languageIndex].Name,
				".loc"
			});
		}

		// Token: 0x060033E0 RID: 13280 RVA: 0x0015D7A0 File Offset: 0x0015B9A0
		public void LoadLanguage(int languageIndex, bool UnloadOtherLanguages, bool useFallback, bool onlyCurrentSpecialization, bool forceLoad)
		{
			if (!this.AllowUnloadingLanguages())
			{
				return;
			}
			if (!PersistentStorage.CanAccessFiles())
			{
				return;
			}
			if (languageIndex >= 0 && (forceLoad || !this.mLanguages[languageIndex].IsLoaded()))
			{
				string savedLanguageFileName = this.GetSavedLanguageFileName(languageIndex);
				string text = PersistentStorage.LoadFile(PersistentStorage.eFileType.Temporal, savedLanguageFileName, false);
				if (!string.IsNullOrEmpty(text))
				{
					this.Import_Language_from_Cache(languageIndex, text, useFallback, onlyCurrentSpecialization);
					this.mLanguages[languageIndex].SetLoaded(true);
				}
			}
			if (UnloadOtherLanguages && I2Utils.IsPlaying())
			{
				for (int i = 0; i < this.mLanguages.Count; i++)
				{
					if (i != languageIndex)
					{
						this.UnloadLanguage(i);
					}
				}
			}
		}

		// Token: 0x060033E1 RID: 13281 RVA: 0x0015D83C File Offset: 0x0015BA3C
		public void LoadAllLanguages(bool forceLoad = false)
		{
			for (int i = 0; i < this.mLanguages.Count; i++)
			{
				this.LoadLanguage(i, false, false, false, forceLoad);
			}
		}

		// Token: 0x060033E2 RID: 13282 RVA: 0x0015D86C File Offset: 0x0015BA6C
		public void UnloadLanguage(int languageIndex)
		{
			if (!this.AllowUnloadingLanguages())
			{
				return;
			}
			if (!PersistentStorage.CanAccessFiles())
			{
				return;
			}
			if (!I2Utils.IsPlaying() || !this.mLanguages[languageIndex].IsLoaded() || !this.mLanguages[languageIndex].CanBeUnloaded() || this.IsCurrentLanguage(languageIndex) || !PersistentStorage.HasFile(PersistentStorage.eFileType.Temporal, this.GetSavedLanguageFileName(languageIndex), true))
			{
				return;
			}
			foreach (TermData termData in this.mTerms)
			{
				termData.Languages[languageIndex] = null;
			}
			this.mLanguages[languageIndex].SetLoaded(false);
		}

		// Token: 0x060033E3 RID: 13283 RVA: 0x0015D92C File Offset: 0x0015BB2C
		public void SaveLanguages(bool unloadAll, PersistentStorage.eFileType fileLocation = PersistentStorage.eFileType.Temporal)
		{
			if (!this.AllowUnloadingLanguages())
			{
				return;
			}
			if (!PersistentStorage.CanAccessFiles())
			{
				return;
			}
			for (int i = 0; i < this.mLanguages.Count; i++)
			{
				string text = this.Export_Language_to_Cache(i, this.IsCurrentLanguage(i));
				if (!string.IsNullOrEmpty(text))
				{
					PersistentStorage.SaveFile(PersistentStorage.eFileType.Temporal, this.GetSavedLanguageFileName(i), text, true);
				}
			}
			if (unloadAll)
			{
				for (int j = 0; j < this.mLanguages.Count; j++)
				{
					if (unloadAll && !this.IsCurrentLanguage(j))
					{
						this.UnloadLanguage(j);
					}
				}
			}
		}

		// Token: 0x060033E4 RID: 13284 RVA: 0x0015D9B4 File Offset: 0x0015BBB4
		public bool HasUnloadedLanguages()
		{
			for (int i = 0; i < this.mLanguages.Count; i++)
			{
				if (!this.mLanguages[i].IsLoaded())
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x060033E5 RID: 13285 RVA: 0x0015D9F0 File Offset: 0x0015BBF0
		public List<string> GetCategories(bool OnlyMainCategory = false, List<string> Categories = null)
		{
			if (Categories == null)
			{
				Categories = new List<string>();
			}
			foreach (TermData termData in this.mTerms)
			{
				string categoryFromFullTerm = LanguageSourceData.GetCategoryFromFullTerm(termData.Term, OnlyMainCategory);
				if (!Categories.Contains(categoryFromFullTerm))
				{
					Categories.Add(categoryFromFullTerm);
				}
			}
			Categories.Sort();
			return Categories;
		}

		// Token: 0x060033E6 RID: 13286 RVA: 0x0015DA68 File Offset: 0x0015BC68
		public static string GetKeyFromFullTerm(string FullTerm, bool OnlyMainCategory = false)
		{
			int num = OnlyMainCategory ? FullTerm.IndexOfAny(LanguageSourceData.CategorySeparators) : FullTerm.LastIndexOfAny(LanguageSourceData.CategorySeparators);
			if (num >= 0)
			{
				return FullTerm.Substring(num + 1);
			}
			return FullTerm;
		}

		// Token: 0x060033E7 RID: 13287 RVA: 0x0015DAA0 File Offset: 0x0015BCA0
		public static string GetCategoryFromFullTerm(string FullTerm, bool OnlyMainCategory = false)
		{
			int num = OnlyMainCategory ? FullTerm.IndexOfAny(LanguageSourceData.CategorySeparators) : FullTerm.LastIndexOfAny(LanguageSourceData.CategorySeparators);
			if (num >= 0)
			{
				return FullTerm.Substring(0, num);
			}
			return LanguageSourceData.EmptyCategory;
		}

		// Token: 0x060033E8 RID: 13288 RVA: 0x0015DADC File Offset: 0x0015BCDC
		public static void DeserializeFullTerm(string FullTerm, out string Key, out string Category, bool OnlyMainCategory = false)
		{
			int num = OnlyMainCategory ? FullTerm.IndexOfAny(LanguageSourceData.CategorySeparators) : FullTerm.LastIndexOfAny(LanguageSourceData.CategorySeparators);
			if (num < 0)
			{
				Category = LanguageSourceData.EmptyCategory;
				Key = FullTerm;
				return;
			}
			Category = FullTerm.Substring(0, num);
			Key = FullTerm.Substring(num + 1);
		}

		// Token: 0x060033E9 RID: 13289 RVA: 0x0015DB2C File Offset: 0x0015BD2C
		public void UpdateDictionary(bool force = false)
		{
			if (!force && this.mDictionary != null && this.mDictionary.Count == this.mTerms.Count)
			{
				return;
			}
			StringComparer stringComparer = this.CaseInsensitiveTerms ? StringComparer.OrdinalIgnoreCase : StringComparer.Ordinal;
			if (this.mDictionary.Comparer != stringComparer)
			{
				this.mDictionary = new Dictionary<string, TermData>(stringComparer);
			}
			else
			{
				this.mDictionary.Clear();
			}
			int i = 0;
			int count = this.mTerms.Count;
			while (i < count)
			{
				TermData termData = this.mTerms[i];
				LanguageSourceData.ValidateFullTerm(ref termData.Term);
				this.mDictionary[termData.Term] = this.mTerms[i];
				this.mTerms[i].Validate();
				i++;
			}
			if (I2Utils.IsPlaying())
			{
				this.SaveLanguages(true, PersistentStorage.eFileType.Temporal);
			}
		}

		// Token: 0x060033EA RID: 13290 RVA: 0x0015DC08 File Offset: 0x0015BE08
		public string GetTranslation(string term, string overrideLanguage = null, string overrideSpecialization = null, bool skipDisabled = false, bool allowCategoryMistmatch = false)
		{
			string result;
			if (this.TryGetTranslation(term, out result, overrideLanguage, overrideSpecialization, skipDisabled, allowCategoryMistmatch))
			{
				return result;
			}
			return string.Empty;
		}

		// Token: 0x060033EB RID: 13291 RVA: 0x0015DC30 File Offset: 0x0015BE30
		public bool TryGetTranslation(string term, out string Translation, string overrideLanguage = null, string overrideSpecialization = null, bool skipDisabled = false, bool allowCategoryMistmatch = false)
		{
			int languageIndex = this.GetLanguageIndex((overrideLanguage == null) ? LocalizationManager.CurrentLanguage : overrideLanguage, true, false);
			if (languageIndex >= 0 && (!skipDisabled || this.mLanguages[languageIndex].IsEnabled()))
			{
				TermData termData = this.GetTermData(term, allowCategoryMistmatch);
				if (termData != null)
				{
					Translation = termData.GetTranslation(languageIndex, overrideSpecialization, true);
					if (Translation == "---")
					{
						Translation = string.Empty;
						return true;
					}
					if (!string.IsNullOrEmpty(Translation))
					{
						return true;
					}
					Translation = null;
				}
				if (this.OnMissingTranslation == LanguageSourceData.MissingTranslationAction.ShowWarning)
				{
					Translation = string.Format("<!-Missing Translation [{0}]-!>", term);
					return true;
				}
				if (this.OnMissingTranslation == LanguageSourceData.MissingTranslationAction.Fallback && termData != null)
				{
					return this.TryGetFallbackTranslation(termData, out Translation, languageIndex, overrideSpecialization, skipDisabled);
				}
				if (this.OnMissingTranslation == LanguageSourceData.MissingTranslationAction.Empty)
				{
					Translation = string.Empty;
					return true;
				}
				if (this.OnMissingTranslation == LanguageSourceData.MissingTranslationAction.ShowTerm)
				{
					Translation = term;
					return true;
				}
			}
			Translation = null;
			return false;
		}

		// Token: 0x060033EC RID: 13292 RVA: 0x0015DD08 File Offset: 0x0015BF08
		private bool TryGetFallbackTranslation(TermData termData, out string Translation, int langIndex, string overrideSpecialization = null, bool skipDisabled = false)
		{
			string text = this.mLanguages[langIndex].Code;
			if (!string.IsNullOrEmpty(text))
			{
				if (text.Contains('-'))
				{
					text = text.Substring(0, text.IndexOf('-'));
				}
				for (int i = 0; i < this.mLanguages.Count; i++)
				{
					if (i != langIndex && this.mLanguages[i].Code.StartsWith(text) && (!skipDisabled || this.mLanguages[i].IsEnabled()))
					{
						Translation = termData.GetTranslation(i, overrideSpecialization, true);
						if (!string.IsNullOrEmpty(Translation))
						{
							return true;
						}
					}
				}
			}
			for (int j = 0; j < this.mLanguages.Count; j++)
			{
				if (j != langIndex && (!skipDisabled || this.mLanguages[j].IsEnabled()) && (text == null || !this.mLanguages[j].Code.StartsWith(text)))
				{
					Translation = termData.GetTranslation(j, overrideSpecialization, true);
					if (!string.IsNullOrEmpty(Translation))
					{
						return true;
					}
				}
			}
			Translation = null;
			return false;
		}

		// Token: 0x060033ED RID: 13293 RVA: 0x0015DE12 File Offset: 0x0015C012
		public TermData AddTerm(string term)
		{
			return this.AddTerm(term, eTermType.Text, true);
		}

		// Token: 0x060033EE RID: 13294 RVA: 0x0015DE20 File Offset: 0x0015C020
		public TermData GetTermData(string term, bool allowCategoryMistmatch = false)
		{
			if (string.IsNullOrEmpty(term))
			{
				return null;
			}
			if (this.mDictionary.Count == 0)
			{
				this.UpdateDictionary(false);
			}
			TermData result;
			if (this.mDictionary.TryGetValue(term, out result))
			{
				return result;
			}
			TermData termData = null;
			if (allowCategoryMistmatch)
			{
				string keyFromFullTerm = LanguageSourceData.GetKeyFromFullTerm(term, false);
				foreach (KeyValuePair<string, TermData> keyValuePair in this.mDictionary)
				{
					if (keyValuePair.Value.IsTerm(keyFromFullTerm, true))
					{
						if (termData != null)
						{
							return null;
						}
						termData = keyValuePair.Value;
					}
				}
				return termData;
			}
			return termData;
		}

		// Token: 0x060033EF RID: 13295 RVA: 0x0015DED0 File Offset: 0x0015C0D0
		public bool ContainsTerm(string term)
		{
			return this.GetTermData(term, false) != null;
		}

		// Token: 0x060033F0 RID: 13296 RVA: 0x0015DEE0 File Offset: 0x0015C0E0
		public List<string> GetTermsList(string Category = null)
		{
			if (this.mDictionary.Count != this.mTerms.Count)
			{
				this.UpdateDictionary(false);
			}
			if (string.IsNullOrEmpty(Category))
			{
				return new List<string>(this.mDictionary.Keys);
			}
			List<string> list = new List<string>();
			for (int i = 0; i < this.mTerms.Count; i++)
			{
				TermData termData = this.mTerms[i];
				if (LanguageSourceData.GetCategoryFromFullTerm(termData.Term, false) == Category)
				{
					list.Add(termData.Term);
				}
			}
			return list;
		}

		// Token: 0x060033F1 RID: 13297 RVA: 0x0015DF70 File Offset: 0x0015C170
		public TermData AddTerm(string NewTerm, eTermType termType, bool SaveSource = true)
		{
			LanguageSourceData.ValidateFullTerm(ref NewTerm);
			NewTerm = NewTerm.Trim();
			if (this.mLanguages.Count == 0)
			{
				this.AddLanguage("English", "en");
			}
			TermData termData = this.GetTermData(NewTerm, false);
			if (termData == null)
			{
				termData = new TermData();
				termData.Term = NewTerm;
				termData.TermType = termType;
				termData.Languages = new string[this.mLanguages.Count];
				termData.Flags = new byte[this.mLanguages.Count];
				this.mTerms.Add(termData);
				this.mDictionary.Add(NewTerm, termData);
			}
			return termData;
		}

		// Token: 0x060033F2 RID: 13298 RVA: 0x0015E010 File Offset: 0x0015C210
		public void RemoveTerm(string term)
		{
			int i = 0;
			int count = this.mTerms.Count;
			while (i < count)
			{
				if (this.mTerms[i].Term == term)
				{
					this.mTerms.RemoveAt(i);
					this.mDictionary.Remove(term);
					return;
				}
				i++;
			}
		}

		// Token: 0x060033F3 RID: 13299 RVA: 0x0015E068 File Offset: 0x0015C268
		public static void ValidateFullTerm(ref string Term)
		{
			Term = Term.Replace('\\', '/');
			Term = Term.Trim();
			if (Term.StartsWith(LanguageSourceData.EmptyCategory, StringComparison.Ordinal) && Term.Length > LanguageSourceData.EmptyCategory.Length && Term[LanguageSourceData.EmptyCategory.Length] == '/')
			{
				Term = Term.Substring(LanguageSourceData.EmptyCategory.Length + 1);
			}
			Term = I2Utils.GetValidTermName(Term, true);
		}

		// Token: 0x04002118 RID: 8472
		[NonSerialized]
		public ILanguageSource owner;

		// Token: 0x04002119 RID: 8473
		public bool UserAgreesToHaveItOnTheScene;

		// Token: 0x0400211A RID: 8474
		public bool UserAgreesToHaveItInsideThePluginsFolder;

		// Token: 0x0400211B RID: 8475
		public bool GoogleLiveSyncIsUptoDate = true;

		// Token: 0x0400211C RID: 8476
		[NonSerialized]
		public bool mIsGlobalSource;

		// Token: 0x0400211D RID: 8477
		public List<TermData> mTerms = new List<TermData>();

		// Token: 0x0400211E RID: 8478
		public bool CaseInsensitiveTerms;

		// Token: 0x0400211F RID: 8479
		[NonSerialized]
		public Dictionary<string, TermData> mDictionary = new Dictionary<string, TermData>(StringComparer.Ordinal);

		// Token: 0x04002120 RID: 8480
		public LanguageSourceData.MissingTranslationAction OnMissingTranslation = LanguageSourceData.MissingTranslationAction.Fallback;

		// Token: 0x04002121 RID: 8481
		public string mTerm_AppName;

		// Token: 0x04002122 RID: 8482
		public List<LanguageData> mLanguages = new List<LanguageData>();

		// Token: 0x04002123 RID: 8483
		public bool IgnoreDeviceLanguage;

		// Token: 0x04002124 RID: 8484
		public LanguageSourceData.eAllowUnloadLanguages _AllowUnloadingLanguages;

		// Token: 0x04002125 RID: 8485
		public string Google_WebServiceURL;

		// Token: 0x04002126 RID: 8486
		public string Google_SpreadsheetKey;

		// Token: 0x04002127 RID: 8487
		public string Google_SpreadsheetName;

		// Token: 0x04002128 RID: 8488
		public string Google_LastUpdatedVersion;

		// Token: 0x04002129 RID: 8489
		public LanguageSourceData.eGoogleUpdateFrequency GoogleUpdateFrequency = LanguageSourceData.eGoogleUpdateFrequency.Weekly;

		// Token: 0x0400212A RID: 8490
		public LanguageSourceData.eGoogleUpdateFrequency GoogleInEditorCheckFrequency = LanguageSourceData.eGoogleUpdateFrequency.Daily;

		// Token: 0x0400212B RID: 8491
		public LanguageSourceData.eGoogleUpdateSynchronization GoogleUpdateSynchronization = LanguageSourceData.eGoogleUpdateSynchronization.OnSceneLoaded;

		// Token: 0x0400212C RID: 8492
		public float GoogleUpdateDelay;

		// Token: 0x0400212E RID: 8494
		public List<UnityEngine.Object> Assets = new List<UnityEngine.Object>();

		// Token: 0x0400212F RID: 8495
		[NonSerialized]
		public Dictionary<string, UnityEngine.Object> mAssetDictionary = new Dictionary<string, UnityEngine.Object>(StringComparer.Ordinal);

		// Token: 0x04002130 RID: 8496
		private string mDelayedGoogleData;

		// Token: 0x04002131 RID: 8497
		public static string EmptyCategory = "Default";

		// Token: 0x04002132 RID: 8498
		public static char[] CategorySeparators = "/\\".ToCharArray();

		// Token: 0x0200081B RID: 2075
		public enum MissingTranslationAction
		{
			// Token: 0x04002E3F RID: 11839
			Empty,
			// Token: 0x04002E40 RID: 11840
			Fallback,
			// Token: 0x04002E41 RID: 11841
			ShowWarning,
			// Token: 0x04002E42 RID: 11842
			ShowTerm
		}

		// Token: 0x0200081C RID: 2076
		public enum eAllowUnloadLanguages
		{
			// Token: 0x04002E44 RID: 11844
			Never,
			// Token: 0x04002E45 RID: 11845
			OnlyInDevice,
			// Token: 0x04002E46 RID: 11846
			EditorAndDevice
		}

		// Token: 0x0200081D RID: 2077
		public enum eGoogleUpdateFrequency
		{
			// Token: 0x04002E48 RID: 11848
			Always,
			// Token: 0x04002E49 RID: 11849
			Never,
			// Token: 0x04002E4A RID: 11850
			Daily,
			// Token: 0x04002E4B RID: 11851
			Weekly,
			// Token: 0x04002E4C RID: 11852
			Monthly,
			// Token: 0x04002E4D RID: 11853
			OnlyOnce,
			// Token: 0x04002E4E RID: 11854
			EveryOtherDay
		}

		// Token: 0x0200081E RID: 2078
		public enum eGoogleUpdateSynchronization
		{
			// Token: 0x04002E50 RID: 11856
			Manual,
			// Token: 0x04002E51 RID: 11857
			OnSceneLoaded,
			// Token: 0x04002E52 RID: 11858
			AsSoonAsDownloaded
		}
	}
}
