using System;
using System.Collections.Generic;

namespace I2.Loc
{
	// Token: 0x02000479 RID: 1145
	public class TranslationJob_Main : TranslationJob
	{
		// Token: 0x06003385 RID: 13189 RVA: 0x0015AECA File Offset: 0x001590CA
		public TranslationJob_Main(Dictionary<string, TranslationQuery> requests, GoogleTranslation.fnOnTranslationReady OnTranslationReady)
		{
			this._requests = requests;
			this._OnTranslationReady = OnTranslationReady;
			this.mPost = new TranslationJob_POST(requests, OnTranslationReady);
		}

		// Token: 0x06003386 RID: 13190 RVA: 0x0015AEF0 File Offset: 0x001590F0
		public override TranslationJob.eJobState GetState()
		{
			if (this.mWeb != null)
			{
				switch (this.mWeb.GetState())
				{
				case TranslationJob.eJobState.Running:
					return TranslationJob.eJobState.Running;
				case TranslationJob.eJobState.Succeeded:
					this.mJobState = TranslationJob.eJobState.Succeeded;
					break;
				case TranslationJob.eJobState.Failed:
					this.mWeb.Dispose();
					this.mWeb = null;
					this.mPost = new TranslationJob_POST(this._requests, this._OnTranslationReady);
					break;
				}
			}
			if (this.mPost != null)
			{
				switch (this.mPost.GetState())
				{
				case TranslationJob.eJobState.Running:
					return TranslationJob.eJobState.Running;
				case TranslationJob.eJobState.Succeeded:
					this.mJobState = TranslationJob.eJobState.Succeeded;
					break;
				case TranslationJob.eJobState.Failed:
					this.mPost.Dispose();
					this.mPost = null;
					this.mGet = new TranslationJob_GET(this._requests, this._OnTranslationReady);
					break;
				}
			}
			if (this.mGet != null)
			{
				switch (this.mGet.GetState())
				{
				case TranslationJob.eJobState.Running:
					return TranslationJob.eJobState.Running;
				case TranslationJob.eJobState.Succeeded:
					this.mJobState = TranslationJob.eJobState.Succeeded;
					break;
				case TranslationJob.eJobState.Failed:
					this.mErrorMessage = this.mGet.mErrorMessage;
					if (this._OnTranslationReady != null)
					{
						this._OnTranslationReady(this._requests, this.mErrorMessage);
					}
					this.mGet.Dispose();
					this.mGet = null;
					break;
				}
			}
			return this.mJobState;
		}

		// Token: 0x06003387 RID: 13191 RVA: 0x0015B030 File Offset: 0x00159230
		public override void Dispose()
		{
			if (this.mPost != null)
			{
				this.mPost.Dispose();
			}
			if (this.mGet != null)
			{
				this.mGet.Dispose();
			}
			this.mPost = null;
			this.mGet = null;
		}

		// Token: 0x040020EB RID: 8427
		private TranslationJob_WEB mWeb;

		// Token: 0x040020EC RID: 8428
		private TranslationJob_POST mPost;

		// Token: 0x040020ED RID: 8429
		private TranslationJob_GET mGet;

		// Token: 0x040020EE RID: 8430
		private Dictionary<string, TranslationQuery> _requests;

		// Token: 0x040020EF RID: 8431
		private GoogleTranslation.fnOnTranslationReady _OnTranslationReady;

		// Token: 0x040020F0 RID: 8432
		public string mErrorMessage;
	}
}
