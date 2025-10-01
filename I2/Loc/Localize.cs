using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.Events;

namespace I2.Loc
{
	// Token: 0x02000484 RID: 1156
	[AddComponentMenu("I2/Localization/I2 Localize")]
	public class Localize : MonoBehaviour
	{
		// Token: 0x17000718 RID: 1816
		// (get) Token: 0x06003401 RID: 13313 RVA: 0x0015E5F0 File Offset: 0x0015C7F0
		// (set) Token: 0x06003402 RID: 13314 RVA: 0x0015E5F8 File Offset: 0x0015C7F8
		public string Term
		{
			get
			{
				return this.mTerm;
			}
			set
			{
				this.SetTerm(value);
			}
		}

		// Token: 0x17000719 RID: 1817
		// (get) Token: 0x06003403 RID: 13315 RVA: 0x0015E601 File Offset: 0x0015C801
		// (set) Token: 0x06003404 RID: 13316 RVA: 0x0015E609 File Offset: 0x0015C809
		public string SecondaryTerm
		{
			get
			{
				return this.mTermSecondary;
			}
			set
			{
				this.SetTerm(null, value);
			}
		}

		// Token: 0x06003405 RID: 13317 RVA: 0x0015E613 File Offset: 0x0015C813
		private void Awake()
		{
			this.UpdateAssetDictionary();
			this.FindTarget();
			if (this.LocalizeOnAwake)
			{
				this.OnLocalize(false);
			}
		}

		// Token: 0x06003406 RID: 13318 RVA: 0x0015E631 File Offset: 0x0015C831
		private void OnEnable()
		{
			this.OnLocalize(false);
		}

		// Token: 0x06003407 RID: 13319 RVA: 0x0015E63A File Offset: 0x0015C83A
		public bool HasCallback()
		{
			return this.LocalizeCallBack.HasCallback() || this.LocalizeEvent.GetPersistentEventCount() > 0;
		}

		// Token: 0x06003408 RID: 13320 RVA: 0x0015E65C File Offset: 0x0015C85C
		public void OnLocalize(bool Force = false)
		{
			if (!Force && (!base.enabled || base.gameObject == null || !base.gameObject.activeInHierarchy))
			{
				return;
			}
			if (string.IsNullOrEmpty(LocalizationManager.CurrentLanguage))
			{
				return;
			}
			if (!this.AlwaysForceLocalize && !Force && !this.HasCallback() && this.LastLocalizedLanguage == LocalizationManager.CurrentLanguage)
			{
				return;
			}
			this.LastLocalizedLanguage = LocalizationManager.CurrentLanguage;
			if (string.IsNullOrEmpty(this.FinalTerm) || string.IsNullOrEmpty(this.FinalSecondaryTerm))
			{
				this.GetFinalTerms(out this.FinalTerm, out this.FinalSecondaryTerm);
			}
			bool flag = I2Utils.IsPlaying() && this.HasCallback();
			if (!flag && string.IsNullOrEmpty(this.FinalTerm) && string.IsNullOrEmpty(this.FinalSecondaryTerm))
			{
				return;
			}
			Localize.CallBackTerm = this.FinalTerm;
			Localize.CallBackSecondaryTerm = this.FinalSecondaryTerm;
			Localize.MainTranslation = ((string.IsNullOrEmpty(this.FinalTerm) || this.FinalTerm == "-") ? null : LocalizationManager.GetTranslation(this.FinalTerm, false, 0, true, false, null, null));
			Localize.SecondaryTranslation = ((string.IsNullOrEmpty(this.FinalSecondaryTerm) || this.FinalSecondaryTerm == "-") ? null : LocalizationManager.GetTranslation(this.FinalSecondaryTerm, false, 0, true, false, null, null));
			if (!flag && string.IsNullOrEmpty(this.FinalTerm) && string.IsNullOrEmpty(Localize.SecondaryTranslation))
			{
				return;
			}
			Localize.CurrentLocalizeComponent = this;
			this.LocalizeCallBack.Execute(this);
			this.LocalizeEvent.Invoke();
			LocalizationManager.ApplyLocalizationParams(ref Localize.MainTranslation, base.gameObject, this.AllowLocalizedParameters);
			if (!this.FindTarget())
			{
				return;
			}
			bool flag2 = LocalizationManager.IsRight2Left && !this.IgnoreRTL;
			if (Localize.MainTranslation != null)
			{
				switch (this.PrimaryTermModifier)
				{
				case Localize.TermModification.ToUpper:
					Localize.MainTranslation = Localize.MainTranslation.ToUpper();
					break;
				case Localize.TermModification.ToLower:
					Localize.MainTranslation = Localize.MainTranslation.ToLower();
					break;
				case Localize.TermModification.ToUpperFirst:
					Localize.MainTranslation = GoogleTranslation.UppercaseFirst(Localize.MainTranslation);
					break;
				case Localize.TermModification.ToTitle:
					Localize.MainTranslation = GoogleTranslation.TitleCase(Localize.MainTranslation);
					break;
				}
				if (!string.IsNullOrEmpty(this.TermPrefix))
				{
					Localize.MainTranslation = (flag2 ? (Localize.MainTranslation + this.TermPrefix) : (this.TermPrefix + Localize.MainTranslation));
				}
				if (!string.IsNullOrEmpty(this.TermSuffix))
				{
					Localize.MainTranslation = (flag2 ? (this.TermSuffix + Localize.MainTranslation) : (Localize.MainTranslation + this.TermSuffix));
				}
				if (this.AddSpacesToJoinedLanguages && LocalizationManager.HasJoinedWords && !string.IsNullOrEmpty(Localize.MainTranslation))
				{
					StringBuilder stringBuilder = new StringBuilder();
					stringBuilder.Append(Localize.MainTranslation[0]);
					int i = 1;
					int length = Localize.MainTranslation.Length;
					while (i < length)
					{
						stringBuilder.Append(' ');
						stringBuilder.Append(Localize.MainTranslation[i]);
						i++;
					}
					Localize.MainTranslation = stringBuilder.ToString();
				}
				if (flag2 && this.mLocalizeTarget.AllowMainTermToBeRTL() && !string.IsNullOrEmpty(Localize.MainTranslation))
				{
					Localize.MainTranslation = LocalizationManager.ApplyRTLfix(Localize.MainTranslation, this.MaxCharactersInRTL, this.IgnoreNumbersInRTL);
				}
			}
			if (Localize.SecondaryTranslation != null)
			{
				switch (this.SecondaryTermModifier)
				{
				case Localize.TermModification.ToUpper:
					Localize.SecondaryTranslation = Localize.SecondaryTranslation.ToUpper();
					break;
				case Localize.TermModification.ToLower:
					Localize.SecondaryTranslation = Localize.SecondaryTranslation.ToLower();
					break;
				case Localize.TermModification.ToUpperFirst:
					Localize.SecondaryTranslation = GoogleTranslation.UppercaseFirst(Localize.SecondaryTranslation);
					break;
				case Localize.TermModification.ToTitle:
					Localize.SecondaryTranslation = GoogleTranslation.TitleCase(Localize.SecondaryTranslation);
					break;
				}
				if (flag2 && this.mLocalizeTarget.AllowSecondTermToBeRTL() && !string.IsNullOrEmpty(Localize.SecondaryTranslation))
				{
					Localize.SecondaryTranslation = LocalizationManager.ApplyRTLfix(Localize.SecondaryTranslation);
				}
			}
			if (LocalizationManager.HighlightLocalizedTargets)
			{
				Localize.MainTranslation = "LOC:" + this.FinalTerm;
			}
			this.mLocalizeTarget.DoLocalize(this, Localize.MainTranslation, Localize.SecondaryTranslation);
			Localize.CurrentLocalizeComponent = null;
		}

		// Token: 0x06003409 RID: 13321 RVA: 0x0015EA78 File Offset: 0x0015CC78
		public bool FindTarget()
		{
			if (this.mLocalizeTarget != null && this.mLocalizeTarget.IsValid(this))
			{
				return true;
			}
			if (this.mLocalizeTarget != null)
			{
				UnityEngine.Object.DestroyImmediate(this.mLocalizeTarget);
				this.mLocalizeTarget = null;
				this.mLocalizeTargetName = null;
			}
			if (!string.IsNullOrEmpty(this.mLocalizeTargetName))
			{
				foreach (ILocalizeTargetDescriptor localizeTargetDescriptor in LocalizationManager.mLocalizeTargets)
				{
					if (this.mLocalizeTargetName == localizeTargetDescriptor.GetTargetType().ToString())
					{
						if (localizeTargetDescriptor.CanLocalize(this))
						{
							this.mLocalizeTarget = localizeTargetDescriptor.CreateTarget(this);
						}
						if (this.mLocalizeTarget != null)
						{
							return true;
						}
					}
				}
			}
			foreach (ILocalizeTargetDescriptor localizeTargetDescriptor2 in LocalizationManager.mLocalizeTargets)
			{
				if (localizeTargetDescriptor2.CanLocalize(this))
				{
					this.mLocalizeTarget = localizeTargetDescriptor2.CreateTarget(this);
					this.mLocalizeTargetName = localizeTargetDescriptor2.GetTargetType().ToString();
					if (this.mLocalizeTarget != null)
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x0600340A RID: 13322 RVA: 0x0015EBD0 File Offset: 0x0015CDD0
		public void GetFinalTerms(out string primaryTerm, out string secondaryTerm)
		{
			primaryTerm = string.Empty;
			secondaryTerm = string.Empty;
			if (!this.FindTarget())
			{
				return;
			}
			if (this.mLocalizeTarget != null)
			{
				this.mLocalizeTarget.GetFinalTerms(this, this.mTerm, this.mTermSecondary, out primaryTerm, out secondaryTerm);
				primaryTerm = I2Utils.GetValidTermName(primaryTerm, false);
			}
			if (!string.IsNullOrEmpty(this.mTerm))
			{
				primaryTerm = this.mTerm;
			}
			if (!string.IsNullOrEmpty(this.mTermSecondary))
			{
				secondaryTerm = this.mTermSecondary;
			}
			if (primaryTerm != null)
			{
				primaryTerm = primaryTerm.Trim();
			}
			if (secondaryTerm != null)
			{
				secondaryTerm = secondaryTerm.Trim();
			}
		}

		// Token: 0x0600340B RID: 13323 RVA: 0x0015EC6C File Offset: 0x0015CE6C
		public string GetMainTargetsText()
		{
			string text = null;
			string text2 = null;
			if (this.mLocalizeTarget != null)
			{
				this.mLocalizeTarget.GetFinalTerms(this, null, null, out text, out text2);
			}
			if (!string.IsNullOrEmpty(text))
			{
				return text;
			}
			return this.mTerm;
		}

		// Token: 0x0600340C RID: 13324 RVA: 0x0015ECAD File Offset: 0x0015CEAD
		public void SetFinalTerms(string Main, string Secondary, out string primaryTerm, out string secondaryTerm, bool RemoveNonASCII)
		{
			primaryTerm = (RemoveNonASCII ? I2Utils.GetValidTermName(Main, false) : Main);
			secondaryTerm = Secondary;
		}

		// Token: 0x0600340D RID: 13325 RVA: 0x0015ECC4 File Offset: 0x0015CEC4
		public void SetTerm(string primary)
		{
			if (!string.IsNullOrEmpty(primary))
			{
				this.mTerm = primary;
				this.FinalTerm = primary;
			}
			this.OnLocalize(true);
		}

		// Token: 0x0600340E RID: 13326 RVA: 0x0015ECF0 File Offset: 0x0015CEF0
		public void SetTerm(string primary, string secondary)
		{
			if (!string.IsNullOrEmpty(primary))
			{
				this.mTerm = primary;
				this.FinalTerm = primary;
			}
			this.mTermSecondary = secondary;
			this.FinalSecondaryTerm = secondary;
			this.OnLocalize(true);
		}

		// Token: 0x0600340F RID: 13327 RVA: 0x0015ED2C File Offset: 0x0015CF2C
		internal T GetSecondaryTranslatedObj<T>(ref string mainTranslation, ref string secondaryTranslation) where T : UnityEngine.Object
		{
			string text;
			string text2;
			this.DeserializeTranslation(mainTranslation, out text, out text2);
			T t = default(!!0);
			if (!string.IsNullOrEmpty(text2))
			{
				t = this.GetObject<T>(text2);
				if (t != null)
				{
					mainTranslation = text;
					secondaryTranslation = text2;
				}
			}
			if (t == null)
			{
				t = this.GetObject<T>(secondaryTranslation);
			}
			return t;
		}

		// Token: 0x06003410 RID: 13328 RVA: 0x0015ED8C File Offset: 0x0015CF8C
		public void UpdateAssetDictionary()
		{
			this.TranslatedObjects.RemoveAll((UnityEngine.Object x) => x == null);
			this.mAssetDictionary = (from o in this.TranslatedObjects.Distinct<UnityEngine.Object>()
			group o by o.name).ToDictionary((IGrouping<string, UnityEngine.Object> g) => g.Key, (IGrouping<string, UnityEngine.Object> g) => g.First<UnityEngine.Object>());
		}

		// Token: 0x06003411 RID: 13329 RVA: 0x0015EE3C File Offset: 0x0015D03C
		internal T GetObject<T>(string Translation) where T : UnityEngine.Object
		{
			if (string.IsNullOrEmpty(Translation))
			{
				return default(!!0);
			}
			return this.GetTranslatedObject<T>(Translation);
		}

		// Token: 0x06003412 RID: 13330 RVA: 0x0015EE62 File Offset: 0x0015D062
		private T GetTranslatedObject<T>(string Translation) where T : UnityEngine.Object
		{
			return this.FindTranslatedObject<T>(Translation);
		}

		// Token: 0x06003413 RID: 13331 RVA: 0x0015EE6C File Offset: 0x0015D06C
		private void DeserializeTranslation(string translation, out string value, out string secondary)
		{
			if (!string.IsNullOrEmpty(translation) && translation.Length > 1 && translation[0] == '[')
			{
				int num = translation.IndexOf(']');
				if (num > 0)
				{
					secondary = translation.Substring(1, num - 1);
					value = translation.Substring(num + 1);
					return;
				}
			}
			value = translation;
			secondary = string.Empty;
		}

		// Token: 0x06003414 RID: 13332 RVA: 0x0015EEC4 File Offset: 0x0015D0C4
		public T FindTranslatedObject<T>(string value) where T : UnityEngine.Object
		{
			if (string.IsNullOrEmpty(value))
			{
				T result = default(!!0);
				return result;
			}
			if (this.mAssetDictionary == null || this.mAssetDictionary.Count != this.TranslatedObjects.Count)
			{
				this.UpdateAssetDictionary();
			}
			foreach (KeyValuePair<string, UnityEngine.Object> keyValuePair in this.mAssetDictionary)
			{
				if (keyValuePair.Value is !!0 && value.EndsWith(keyValuePair.Key, StringComparison.OrdinalIgnoreCase) && string.Compare(value, keyValuePair.Key, StringComparison.OrdinalIgnoreCase) == 0)
				{
					return (!!0)((object)keyValuePair.Value);
				}
			}
			T t = LocalizationManager.FindAsset(value) as !!0;
			if (t)
			{
				return t;
			}
			return ResourceManager.pInstance.GetAsset<T>(value);
		}

		// Token: 0x06003415 RID: 13333 RVA: 0x0015EFB4 File Offset: 0x0015D1B4
		public bool HasTranslatedObject(UnityEngine.Object Obj)
		{
			return this.TranslatedObjects.Contains(Obj) || ResourceManager.pInstance.HasAsset(Obj);
		}

		// Token: 0x06003416 RID: 13334 RVA: 0x0015EFD1 File Offset: 0x0015D1D1
		public void AddTranslatedObject(UnityEngine.Object Obj)
		{
			if (this.TranslatedObjects.Contains(Obj))
			{
				return;
			}
			this.TranslatedObjects.Add(Obj);
			this.UpdateAssetDictionary();
		}

		// Token: 0x06003417 RID: 13335 RVA: 0x0015EFF4 File Offset: 0x0015D1F4
		public void SetGlobalLanguage(string Language)
		{
			LocalizationManager.CurrentLanguage = Language;
		}

		// Token: 0x04002138 RID: 8504
		public string mTerm = string.Empty;

		// Token: 0x04002139 RID: 8505
		public string mTermSecondary = string.Empty;

		// Token: 0x0400213A RID: 8506
		[NonSerialized]
		public string FinalTerm;

		// Token: 0x0400213B RID: 8507
		[NonSerialized]
		public string FinalSecondaryTerm;

		// Token: 0x0400213C RID: 8508
		public Localize.TermModification PrimaryTermModifier;

		// Token: 0x0400213D RID: 8509
		public Localize.TermModification SecondaryTermModifier;

		// Token: 0x0400213E RID: 8510
		public string TermPrefix;

		// Token: 0x0400213F RID: 8511
		public string TermSuffix;

		// Token: 0x04002140 RID: 8512
		public bool LocalizeOnAwake = true;

		// Token: 0x04002141 RID: 8513
		private string LastLocalizedLanguage;

		// Token: 0x04002142 RID: 8514
		public bool IgnoreRTL;

		// Token: 0x04002143 RID: 8515
		public int MaxCharactersInRTL;

		// Token: 0x04002144 RID: 8516
		public bool IgnoreNumbersInRTL = true;

		// Token: 0x04002145 RID: 8517
		public bool CorrectAlignmentForRTL = true;

		// Token: 0x04002146 RID: 8518
		public bool AddSpacesToJoinedLanguages;

		// Token: 0x04002147 RID: 8519
		public bool AllowLocalizedParameters = true;

		// Token: 0x04002148 RID: 8520
		public List<UnityEngine.Object> TranslatedObjects = new List<UnityEngine.Object>();

		// Token: 0x04002149 RID: 8521
		[NonSerialized]
		public Dictionary<string, UnityEngine.Object> mAssetDictionary = new Dictionary<string, UnityEngine.Object>(StringComparer.Ordinal);

		// Token: 0x0400214A RID: 8522
		public UnityEvent LocalizeEvent = new UnityEvent();

		// Token: 0x0400214B RID: 8523
		public static string MainTranslation;

		// Token: 0x0400214C RID: 8524
		public static string SecondaryTranslation;

		// Token: 0x0400214D RID: 8525
		public static string CallBackTerm;

		// Token: 0x0400214E RID: 8526
		public static string CallBackSecondaryTerm;

		// Token: 0x0400214F RID: 8527
		public static Localize CurrentLocalizeComponent;

		// Token: 0x04002150 RID: 8528
		public bool AlwaysForceLocalize;

		// Token: 0x04002151 RID: 8529
		[SerializeField]
		public EventCallback LocalizeCallBack = new EventCallback();

		// Token: 0x04002152 RID: 8530
		public bool mGUI_ShowReferences;

		// Token: 0x04002153 RID: 8531
		public bool mGUI_ShowTems = true;

		// Token: 0x04002154 RID: 8532
		public bool mGUI_ShowCallback;

		// Token: 0x04002155 RID: 8533
		public ILocalizeTarget mLocalizeTarget;

		// Token: 0x04002156 RID: 8534
		public string mLocalizeTargetName;

		// Token: 0x02000821 RID: 2081
		public enum TermModification
		{
			// Token: 0x04002E60 RID: 11872
			DontModify,
			// Token: 0x04002E61 RID: 11873
			ToUpper,
			// Token: 0x04002E62 RID: 11874
			ToLower,
			// Token: 0x04002E63 RID: 11875
			ToUpperFirst,
			// Token: 0x04002E64 RID: 11876
			ToTitle
		}
	}
}
