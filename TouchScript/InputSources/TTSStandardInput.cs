using System;
using TouchScript.InputSources.InputHandlers;
using UnityEngine;

namespace TouchScript.InputSources
{
	// Token: 0x02000395 RID: 917
	[AddComponentMenu("TTSTouchScript/Input Sources/Standard Input")]
	[HelpURL("http://touchscript.github.io/docs/html/T_TouchScript_InputSources_StandardInput.htm")]
	public sealed class TTSStandardInput : InputSource
	{
		// Token: 0x06002B05 RID: 11013 RVA: 0x00130B54 File Offset: 0x0012ED54
		public override void UpdateInput()
		{
			base.UpdateInput();
			if (this.touchHandler != null)
			{
				this.touchHandler.Update();
				if (this.mouseHandler != null)
				{
					if (this.touchHandler.HasTouches)
					{
						this.mouseHandler.EndTouches();
						return;
					}
					this.mouseHandler.Update();
					return;
				}
			}
			else if (this.mouseHandler != null)
			{
				this.mouseHandler.Update();
			}
		}

		// Token: 0x06002B06 RID: 11014 RVA: 0x00130BBC File Offset: 0x0012EDBC
		public override void CancelTouch(TouchPoint touch, bool @return)
		{
			base.CancelTouch(touch, @return);
			bool flag = false;
			if (this.touchHandler != null)
			{
				flag = this.touchHandler.CancelTouch(touch, @return);
			}
			if (this.mouseHandler != null && !flag)
			{
				flag = this.mouseHandler.CancelTouch(touch, @return);
			}
			if (this.windows7TouchHandler != null && !flag)
			{
				flag = this.windows7TouchHandler.CancelTouch(touch, @return);
			}
			if (this.windows8TouchHandler != null && !flag)
			{
				this.windows8TouchHandler.CancelTouch(touch, @return);
			}
		}

		// Token: 0x06002B07 RID: 11015 RVA: 0x00130C34 File Offset: 0x0012EE34
		protected override void OnEnable()
		{
			base.OnEnable();
			if (Environment.OSVersion.Platform == PlatformID.Win32NT)
			{
				if (Environment.OSVersion.Version >= TTSStandardInput.WIN8_VERSION)
				{
					switch (this.Windows8Touch)
					{
					case TTSStandardInput.Windows8TouchAPIType.Windows8:
						this.enableWindows8Touch();
						return;
					case TTSStandardInput.Windows8TouchAPIType.Windows7:
						this.enableWindows7Touch();
						return;
					case TTSStandardInput.Windows8TouchAPIType.Unity:
						this.enableTouch();
						return;
					case TTSStandardInput.Windows8TouchAPIType.None:
						break;
					default:
						return;
					}
				}
				else if (Environment.OSVersion.Version >= TTSStandardInput.WIN7_VERSION)
				{
					switch (this.Windows7Touch)
					{
					case TTSStandardInput.Windows7TouchAPIType.Windows7:
						this.enableWindows7Touch();
						return;
					case TTSStandardInput.Windows7TouchAPIType.Unity:
						this.enableTouch();
						break;
					case TTSStandardInput.Windows7TouchAPIType.None:
						break;
					default:
						return;
					}
				}
			}
		}

		// Token: 0x06002B08 RID: 11016 RVA: 0x00130CDD File Offset: 0x0012EEDD
		protected override void OnDisable()
		{
			this.disableMouse();
			this.disableTouch();
			this.disableWindows8Mouse();
			this.disableWindows7Touch();
			this.disableWindows8Touch();
			base.OnDisable();
		}

		// Token: 0x06002B09 RID: 11017 RVA: 0x000025B8 File Offset: 0x000007B8
		private void enableMouse()
		{
		}

		// Token: 0x06002B0A RID: 11018 RVA: 0x00130D03 File Offset: 0x0012EF03
		private void disableMouse()
		{
			if (this.mouseHandler != null)
			{
				this.mouseHandler.Dispose();
				this.mouseHandler = null;
			}
		}

		// Token: 0x06002B0B RID: 11019 RVA: 0x00130D20 File Offset: 0x0012EF20
		private void enableTouch()
		{
			this.touchHandler = new TouchHandler(this.TouchTags, new Func<Vector2, Tags, bool, TouchPoint>(this.beginTouch), new Action<int, Vector2>(this.moveTouch), new Action<int>(this.endTouch), new Action<int>(this.cancelTouch));
			Debug.Log("[TouchScript] Initialized Unity touch input.");
		}

		// Token: 0x06002B0C RID: 11020 RVA: 0x00130D7C File Offset: 0x0012EF7C
		private void disableTouch()
		{
			if (this.touchHandler != null)
			{
				this.touchHandler.Dispose();
				this.touchHandler = null;
			}
		}

		// Token: 0x06002B0D RID: 11021 RVA: 0x00130D98 File Offset: 0x0012EF98
		private void enableWindows8Mouse()
		{
			this.windows8MouseHandler = new Windows8MouseHandler();
			Debug.Log("[TouchScript] Initialized Windows 8 mouse input.");
		}

		// Token: 0x06002B0E RID: 11022 RVA: 0x00130DAF File Offset: 0x0012EFAF
		private void disableWindows8Mouse()
		{
			if (this.windows8MouseHandler != null)
			{
				this.windows8MouseHandler.Dispose();
				this.windows8MouseHandler = null;
			}
		}

		// Token: 0x06002B0F RID: 11023 RVA: 0x00130DCC File Offset: 0x0012EFCC
		private void enableWindows7Touch()
		{
			this.windows7TouchHandler = new Windows7TouchHandler(this.TouchTags, new Func<Vector2, Tags, bool, TouchPoint>(this.beginTouch), new Action<int, Vector2>(this.moveTouch), new Action<int>(this.endTouch), new Action<int>(this.cancelTouch));
			Debug.Log("[TouchScript] Initialized Windows 7 touch input.");
		}

		// Token: 0x06002B10 RID: 11024 RVA: 0x00130E28 File Offset: 0x0012F028
		private void disableWindows7Touch()
		{
			if (this.windows7TouchHandler != null)
			{
				this.windows7TouchHandler.Dispose();
				this.windows7TouchHandler = null;
			}
		}

		// Token: 0x06002B11 RID: 11025 RVA: 0x00130E44 File Offset: 0x0012F044
		private void enableWindows8Touch()
		{
			this.windows8TouchHandler = new Windows8TouchHandler(this.TouchTags, this.MouseTags, this.PenTags, new Func<Vector2, Tags, bool, TouchPoint>(this.beginTouch), new Action<int, Vector2>(this.moveTouch), new Action<int>(this.endTouch), new Action<int>(this.cancelTouch));
			Debug.Log("[TouchScript] Initialized Windows 8 touch input.");
		}

		// Token: 0x06002B12 RID: 11026 RVA: 0x00130EAC File Offset: 0x0012F0AC
		private void disableWindows8Touch()
		{
			if (this.windows8TouchHandler != null)
			{
				this.windows8TouchHandler.Dispose();
				this.windows8TouchHandler = null;
			}
		}

		// Token: 0x04001D2A RID: 7466
		private static readonly Version WIN7_VERSION = new Version(6, 1, 0, 0);

		// Token: 0x04001D2B RID: 7467
		private static readonly Version WIN8_VERSION = new Version(6, 2, 0, 0);

		// Token: 0x04001D2C RID: 7468
		public Tags TouchTags = new Tags("Touch");

		// Token: 0x04001D2D RID: 7469
		public Tags MouseTags = new Tags("Mouse");

		// Token: 0x04001D2E RID: 7470
		public Tags PenTags = new Tags("Pen");

		// Token: 0x04001D2F RID: 7471
		public TTSStandardInput.Windows8TouchAPIType Windows8Touch;

		// Token: 0x04001D30 RID: 7472
		public TTSStandardInput.Windows7TouchAPIType Windows7Touch;

		// Token: 0x04001D31 RID: 7473
		public bool WebPlayerTouch = true;

		// Token: 0x04001D32 RID: 7474
		public bool WebGLTouch = true;

		// Token: 0x04001D33 RID: 7475
		public bool EmulateTouchEditor;

		// Token: 0x04001D34 RID: 7476
		private MouseHandler mouseHandler;

		// Token: 0x04001D35 RID: 7477
		private TouchHandler touchHandler;

		// Token: 0x04001D36 RID: 7478
		private Windows8MouseHandler windows8MouseHandler;

		// Token: 0x04001D37 RID: 7479
		private Windows8TouchHandler windows8TouchHandler;

		// Token: 0x04001D38 RID: 7480
		private Windows7TouchHandler windows7TouchHandler;

		// Token: 0x020007C6 RID: 1990
		public enum Windows8TouchAPIType
		{
			// Token: 0x04002D5C RID: 11612
			Windows8,
			// Token: 0x04002D5D RID: 11613
			Windows7,
			// Token: 0x04002D5E RID: 11614
			Unity,
			// Token: 0x04002D5F RID: 11615
			None
		}

		// Token: 0x020007C7 RID: 1991
		public enum Windows7TouchAPIType
		{
			// Token: 0x04002D61 RID: 11617
			Windows7,
			// Token: 0x04002D62 RID: 11618
			Unity,
			// Token: 0x04002D63 RID: 11619
			None
		}
	}
}
