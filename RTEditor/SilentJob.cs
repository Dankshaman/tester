using System;
using System.Threading;

namespace RTEditor
{
	// Token: 0x020003B6 RID: 950
	public abstract class SilentJob
	{
		// Token: 0x1700052F RID: 1327
		// (get) Token: 0x06002CE2 RID: 11490 RVA: 0x0013A244 File Offset: 0x00138444
		public bool IsRunning
		{
			get
			{
				object lockHandle = this._lockHandle;
				bool isRunning;
				lock (lockHandle)
				{
					isRunning = this._isRunning;
				}
				return isRunning;
			}
		}

		// Token: 0x06002CE3 RID: 11491 RVA: 0x0013A288 File Offset: 0x00138488
		public void Start()
		{
			if (this.IsRunning)
			{
				return;
			}
			this._thread = new Thread(new ThreadStart(this.JobThread));
			this._thread.Start();
		}

		// Token: 0x06002CE4 RID: 11492 RVA: 0x0013A2B5 File Offset: 0x001384B5
		public void Abort()
		{
			if (this.IsRunning)
			{
				this._thread.Abort();
			}
		}

		// Token: 0x06002CE5 RID: 11493
		protected abstract void DoJob();

		// Token: 0x06002CE6 RID: 11494 RVA: 0x0013A2CA File Offset: 0x001384CA
		private void JobThread()
		{
			this.SetIsRunning(true);
			this.DoJob();
			this.SetIsRunning(false);
		}

		// Token: 0x06002CE7 RID: 11495 RVA: 0x0013A2E0 File Offset: 0x001384E0
		private void SetIsRunning(bool isRunning)
		{
			object lockHandle = this._lockHandle;
			lock (lockHandle)
			{
				this._isRunning = isRunning;
			}
		}

		// Token: 0x04001E1C RID: 7708
		private bool _isRunning;

		// Token: 0x04001E1D RID: 7709
		private Thread _thread;

		// Token: 0x04001E1E RID: 7710
		private object _lockHandle = new object();
	}
}
