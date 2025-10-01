using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine.Networking;

namespace I2.Loc
{
	// Token: 0x02000478 RID: 1144
	public class TranslationJob_GET : TranslationJob_WWW
	{
		// Token: 0x06003381 RID: 13185 RVA: 0x0015AD5E File Offset: 0x00158F5E
		public TranslationJob_GET(Dictionary<string, TranslationQuery> requests, GoogleTranslation.fnOnTranslationReady OnTranslationReady)
		{
			this._requests = requests;
			this._OnTranslationReady = OnTranslationReady;
			this.mQueries = GoogleTranslation.ConvertTranslationRequest(requests, true);
			this.GetState();
		}

		// Token: 0x06003382 RID: 13186 RVA: 0x0015AD88 File Offset: 0x00158F88
		private void ExecuteNextQuery()
		{
			if (this.mQueries.Count == 0)
			{
				this.mJobState = TranslationJob.eJobState.Succeeded;
				return;
			}
			int index = this.mQueries.Count - 1;
			string arg = this.mQueries[index];
			this.mQueries.RemoveAt(index);
			string uri = string.Format("{0}?action=Translate&list={1}", LocalizationManager.GetWebServiceURL(null), arg);
			this.www = UnityWebRequest.Get(uri);
			I2Utils.SendWebRequest(this.www);
		}

		// Token: 0x06003383 RID: 13187 RVA: 0x0015ADFC File Offset: 0x00158FFC
		public override TranslationJob.eJobState GetState()
		{
			if (this.www != null && this.www.isDone)
			{
				this.ProcessResult(this.www.downloadHandler.data, this.www.error);
				this.www.Dispose();
				this.www = null;
			}
			if (this.www == null)
			{
				this.ExecuteNextQuery();
			}
			return this.mJobState;
		}

		// Token: 0x06003384 RID: 13188 RVA: 0x0015AE68 File Offset: 0x00159068
		public void ProcessResult(byte[] bytes, string errorMsg)
		{
			if (string.IsNullOrEmpty(errorMsg))
			{
				errorMsg = GoogleTranslation.ParseTranslationResult(Encoding.UTF8.GetString(bytes, 0, bytes.Length), this._requests);
				if (string.IsNullOrEmpty(errorMsg))
				{
					if (this._OnTranslationReady != null)
					{
						this._OnTranslationReady(this._requests, null);
					}
					return;
				}
			}
			this.mJobState = TranslationJob.eJobState.Failed;
			this.mErrorMessage = errorMsg;
		}

		// Token: 0x040020E7 RID: 8423
		private Dictionary<string, TranslationQuery> _requests;

		// Token: 0x040020E8 RID: 8424
		private GoogleTranslation.fnOnTranslationReady _OnTranslationReady;

		// Token: 0x040020E9 RID: 8425
		private List<string> mQueries;

		// Token: 0x040020EA RID: 8426
		public string mErrorMessage;
	}
}
