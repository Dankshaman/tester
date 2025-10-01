using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

namespace I2.Loc
{
	// Token: 0x0200047A RID: 1146
	public class TranslationJob_POST : TranslationJob_WWW
	{
		// Token: 0x06003388 RID: 13192 RVA: 0x0015B068 File Offset: 0x00159268
		public TranslationJob_POST(Dictionary<string, TranslationQuery> requests, GoogleTranslation.fnOnTranslationReady OnTranslationReady)
		{
			this._requests = requests;
			this._OnTranslationReady = OnTranslationReady;
			List<string> list = GoogleTranslation.ConvertTranslationRequest(requests, false);
			WWWForm wwwform = new WWWForm();
			wwwform.AddField("action", "Translate");
			wwwform.AddField("list", list[0]);
			this.www = UnityWebRequest.Post(LocalizationManager.GetWebServiceURL(null), wwwform);
			I2Utils.SendWebRequest(this.www);
		}

		// Token: 0x06003389 RID: 13193 RVA: 0x0015B0D8 File Offset: 0x001592D8
		public override TranslationJob.eJobState GetState()
		{
			if (this.www != null && this.www.isDone)
			{
				this.ProcessResult(this.www.downloadHandler.data, this.www.error);
				this.www.Dispose();
				this.www = null;
			}
			return this.mJobState;
		}

		// Token: 0x0600338A RID: 13194 RVA: 0x0015B134 File Offset: 0x00159334
		public void ProcessResult(byte[] bytes, string errorMsg)
		{
			if (!string.IsNullOrEmpty(errorMsg))
			{
				this.mJobState = TranslationJob.eJobState.Failed;
				return;
			}
			errorMsg = GoogleTranslation.ParseTranslationResult(Encoding.UTF8.GetString(bytes, 0, bytes.Length), this._requests);
			if (this._OnTranslationReady != null)
			{
				this._OnTranslationReady(this._requests, errorMsg);
			}
			this.mJobState = TranslationJob.eJobState.Succeeded;
		}

		// Token: 0x040020F1 RID: 8433
		private Dictionary<string, TranslationQuery> _requests;

		// Token: 0x040020F2 RID: 8434
		private GoogleTranslation.fnOnTranslationReady _OnTranslationReady;
	}
}
