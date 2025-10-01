using System;
using System.Collections.Generic;
using UnityEngine;

namespace I2.Loc
{
	// Token: 0x0200047E RID: 1150
	[AddComponentMenu("I2/Localization/Source")]
	[ExecuteInEditMode]
	public class LanguageSource : MonoBehaviour, ISerializationCallbackReceiver, ILanguageSource
	{
		// Token: 0x17000714 RID: 1812
		// (get) Token: 0x06003398 RID: 13208 RVA: 0x0015B636 File Offset: 0x00159836
		// (set) Token: 0x06003399 RID: 13209 RVA: 0x0015B63E File Offset: 0x0015983E
		public LanguageSourceData SourceData
		{
			get
			{
				return this.mSource;
			}
			set
			{
				this.mSource = value;
			}
		}

		// Token: 0x14000079 RID: 121
		// (add) Token: 0x0600339A RID: 13210 RVA: 0x0015B648 File Offset: 0x00159848
		// (remove) Token: 0x0600339B RID: 13211 RVA: 0x0015B680 File Offset: 0x00159880
		public event LanguageSource.fnOnSourceUpdated Event_OnSourceUpdateFromGoogle;

		// Token: 0x0600339C RID: 13212 RVA: 0x0015B6B5 File Offset: 0x001598B5
		private void Awake()
		{
			this.mSource.owner = this;
			this.mSource.Awake();
		}

		// Token: 0x0600339D RID: 13213 RVA: 0x0015B6CE File Offset: 0x001598CE
		private void OnDestroy()
		{
			this.NeverDestroy = false;
			if (!this.NeverDestroy)
			{
				this.mSource.OnDestroy();
			}
		}

		// Token: 0x0600339E RID: 13214 RVA: 0x0015B6EC File Offset: 0x001598EC
		public string GetSourceName()
		{
			string text = base.gameObject.name;
			Transform parent = base.transform.parent;
			while (parent)
			{
				text = parent.name + "_" + text;
				parent = parent.parent;
			}
			return text;
		}

		// Token: 0x0600339F RID: 13215 RVA: 0x0015B735 File Offset: 0x00159935
		public void OnBeforeSerialize()
		{
			this.version = 1;
		}

		// Token: 0x060033A0 RID: 13216 RVA: 0x0015B740 File Offset: 0x00159940
		public void OnAfterDeserialize()
		{
			if (this.version == 0 || this.mSource == null)
			{
				this.mSource = new LanguageSourceData();
				this.mSource.owner = this;
				this.mSource.UserAgreesToHaveItOnTheScene = this.UserAgreesToHaveItOnTheScene;
				this.mSource.UserAgreesToHaveItInsideThePluginsFolder = this.UserAgreesToHaveItInsideThePluginsFolder;
				this.mSource.IgnoreDeviceLanguage = this.IgnoreDeviceLanguage;
				this.mSource._AllowUnloadingLanguages = this._AllowUnloadingLanguages;
				this.mSource.CaseInsensitiveTerms = this.CaseInsensitiveTerms;
				this.mSource.OnMissingTranslation = this.OnMissingTranslation;
				this.mSource.mTerm_AppName = this.mTerm_AppName;
				this.mSource.GoogleLiveSyncIsUptoDate = this.GoogleLiveSyncIsUptoDate;
				this.mSource.Google_WebServiceURL = this.Google_WebServiceURL;
				this.mSource.Google_SpreadsheetKey = this.Google_SpreadsheetKey;
				this.mSource.Google_SpreadsheetName = this.Google_SpreadsheetName;
				this.mSource.Google_LastUpdatedVersion = this.Google_LastUpdatedVersion;
				this.mSource.GoogleUpdateFrequency = this.GoogleUpdateFrequency;
				this.mSource.GoogleUpdateDelay = this.GoogleUpdateDelay;
				this.mSource.Event_OnSourceUpdateFromGoogle += this.Event_OnSourceUpdateFromGoogle;
				if (this.mLanguages != null && this.mLanguages.Count > 0)
				{
					this.mSource.mLanguages.Clear();
					this.mSource.mLanguages.AddRange(this.mLanguages);
					this.mLanguages.Clear();
				}
				if (this.Assets != null && this.Assets.Count > 0)
				{
					this.mSource.Assets.Clear();
					this.mSource.Assets.AddRange(this.Assets);
					this.Assets.Clear();
				}
				if (this.mTerms != null && this.mTerms.Count > 0)
				{
					this.mSource.mTerms.Clear();
					for (int i = 0; i < this.mTerms.Count; i++)
					{
						this.mSource.mTerms.Add(this.mTerms[i]);
					}
					this.mTerms.Clear();
				}
				this.version = 1;
				this.Event_OnSourceUpdateFromGoogle = null;
			}
		}

		// Token: 0x04002102 RID: 8450
		public LanguageSourceData mSource = new LanguageSourceData();

		// Token: 0x04002103 RID: 8451
		public int version;

		// Token: 0x04002104 RID: 8452
		public bool NeverDestroy;

		// Token: 0x04002105 RID: 8453
		public bool UserAgreesToHaveItOnTheScene;

		// Token: 0x04002106 RID: 8454
		public bool UserAgreesToHaveItInsideThePluginsFolder;

		// Token: 0x04002107 RID: 8455
		public bool GoogleLiveSyncIsUptoDate = true;

		// Token: 0x04002108 RID: 8456
		public List<UnityEngine.Object> Assets = new List<UnityEngine.Object>();

		// Token: 0x04002109 RID: 8457
		public string Google_WebServiceURL;

		// Token: 0x0400210A RID: 8458
		public string Google_SpreadsheetKey;

		// Token: 0x0400210B RID: 8459
		public string Google_SpreadsheetName;

		// Token: 0x0400210C RID: 8460
		public string Google_LastUpdatedVersion;

		// Token: 0x0400210D RID: 8461
		public LanguageSourceData.eGoogleUpdateFrequency GoogleUpdateFrequency = LanguageSourceData.eGoogleUpdateFrequency.Weekly;

		// Token: 0x0400210E RID: 8462
		public float GoogleUpdateDelay = 5f;

		// Token: 0x04002110 RID: 8464
		public List<LanguageData> mLanguages = new List<LanguageData>();

		// Token: 0x04002111 RID: 8465
		public bool IgnoreDeviceLanguage;

		// Token: 0x04002112 RID: 8466
		public LanguageSourceData.eAllowUnloadLanguages _AllowUnloadingLanguages;

		// Token: 0x04002113 RID: 8467
		public List<TermData> mTerms = new List<TermData>();

		// Token: 0x04002114 RID: 8468
		public bool CaseInsensitiveTerms;

		// Token: 0x04002115 RID: 8469
		public LanguageSourceData.MissingTranslationAction OnMissingTranslation = LanguageSourceData.MissingTranslationAction.Fallback;

		// Token: 0x04002116 RID: 8470
		public string mTerm_AppName;

		// Token: 0x0200081A RID: 2074
		// (Invoke) Token: 0x060040FB RID: 16635
		public delegate void fnOnSourceUpdated(LanguageSourceData source, bool ReceivedNewData, string errorMsg);
	}
}
