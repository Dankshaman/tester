using System;
using UnityEngine;

namespace RTEditor
{
	// Token: 0x0200041C RID: 1052
	public class Mouse
	{
		// Token: 0x1700063B RID: 1595
		// (get) Token: 0x060030A9 RID: 12457 RVA: 0x0014D175 File Offset: 0x0014B375
		public bool IsLeftMouseButtonDown
		{
			get
			{
				return this._isLeftMouseButtonDown;
			}
		}

		// Token: 0x1700063C RID: 1596
		// (get) Token: 0x060030AA RID: 12458 RVA: 0x0014D17D File Offset: 0x0014B37D
		public bool IsRightMouseButtonDown
		{
			get
			{
				return this._isRightMouseButtonDown;
			}
		}

		// Token: 0x1700063D RID: 1597
		// (get) Token: 0x060030AB RID: 12459 RVA: 0x0014D185 File Offset: 0x0014B385
		public bool IsMiddleMouseButtonDown
		{
			get
			{
				return this._isMiddleMouseButtonDown;
			}
		}

		// Token: 0x1700063E RID: 1598
		// (get) Token: 0x060030AC RID: 12460 RVA: 0x0014D18D File Offset: 0x0014B38D
		public bool WasLeftMouseButtonPressedInCurrentFrame
		{
			get
			{
				return this._wasLeftMouseButtonPressedInCurrentFrame;
			}
		}

		// Token: 0x1700063F RID: 1599
		// (get) Token: 0x060030AD RID: 12461 RVA: 0x0014D195 File Offset: 0x0014B395
		public bool WasRightMouseButtonPressedInCurrentFrame
		{
			get
			{
				return this._wasRightMouseButtonPressedInCurrentFrame;
			}
		}

		// Token: 0x17000640 RID: 1600
		// (get) Token: 0x060030AE RID: 12462 RVA: 0x0014D19D File Offset: 0x0014B39D
		public bool WasMiddleMouseButtonPressedInCurrentFrame
		{
			get
			{
				return this._wasMiddleMouseButtonPressedInCurrentFrame;
			}
		}

		// Token: 0x17000641 RID: 1601
		// (get) Token: 0x060030AF RID: 12463 RVA: 0x0014D1A5 File Offset: 0x0014B3A5
		public bool WasLeftMouseButtonReleasedInCurrentFrame
		{
			get
			{
				return this._wasLeftMouseButtonReleasedInCurrentFrame;
			}
		}

		// Token: 0x17000642 RID: 1602
		// (get) Token: 0x060030B0 RID: 12464 RVA: 0x0014D1AD File Offset: 0x0014B3AD
		public bool WasRightMouseButtonReleasedInCurrentFrame
		{
			get
			{
				return this._wasRightMouseButtonReleasedInCurrentFrame;
			}
		}

		// Token: 0x17000643 RID: 1603
		// (get) Token: 0x060030B1 RID: 12465 RVA: 0x0014D1B5 File Offset: 0x0014B3B5
		public bool WasMiddleMouseButtonReleasedInCurrentFrame
		{
			get
			{
				return this._wasMiddleMouseButtonReleasedInCurrentFrame;
			}
		}

		// Token: 0x17000644 RID: 1604
		// (get) Token: 0x060030B2 RID: 12466 RVA: 0x0014D1BD File Offset: 0x0014B3BD
		public Vector2 CursorPositionInPreviousFrame
		{
			get
			{
				return this._cursorPositionInPreviousFrame;
			}
		}

		// Token: 0x17000645 RID: 1605
		// (get) Token: 0x060030B3 RID: 12467 RVA: 0x0014D1C5 File Offset: 0x0014B3C5
		public Vector2 CursorOffsetSinceLastFrame
		{
			get
			{
				return this._cursorOffsetSinceLastFrame;
			}
		}

		// Token: 0x17000646 RID: 1606
		// (get) Token: 0x060030B4 RID: 12468 RVA: 0x0014D1CD File Offset: 0x0014B3CD
		public bool WasMouseMovedSinceLastFrame
		{
			get
			{
				return this._cursorOffsetSinceLastFrame.magnitude != 0f;
			}
		}

		// Token: 0x17000647 RID: 1607
		// (get) Token: 0x060030B5 RID: 12469 RVA: 0x0014D1E4 File Offset: 0x0014B3E4
		public Vector2 CursorOffsetSinceLeftMouseButtonDown
		{
			get
			{
				return this._cursorOffsetSinceLeftMouseButtonDown;
			}
		}

		// Token: 0x17000648 RID: 1608
		// (get) Token: 0x060030B6 RID: 12470 RVA: 0x0014D1EC File Offset: 0x0014B3EC
		public Vector2 CursorOffsetSinceRightMouseButtonDown
		{
			get
			{
				return this._cursorOffsetSinceRightMouseButtonDown;
			}
		}

		// Token: 0x17000649 RID: 1609
		// (get) Token: 0x060030B7 RID: 12471 RVA: 0x0014D1F4 File Offset: 0x0014B3F4
		public Vector2 CursorOffsetSinceMiddleMouseButtonDown
		{
			get
			{
				return this._cursorOffsetSinceMiddleMouseButtonDown;
			}
		}

		// Token: 0x060030B8 RID: 12472 RVA: 0x0014D1FC File Offset: 0x0014B3FC
		public Mouse()
		{
			this._cursorPositionInPreviousFrame = Vector2.zero;
		}

		// Token: 0x060030B9 RID: 12473 RVA: 0x0014D20F File Offset: 0x0014B40F
		public void UpdateInfoForCurrentFrame()
		{
			this.UpdateMouseButtonStatesForCurrentFrame();
			this.UpdateCursorPositionAndOffsetInfoForCurrentFrame();
		}

		// Token: 0x060030BA RID: 12474 RVA: 0x0014D21D File Offset: 0x0014B41D
		public void ResetCursorPositionInPreviousFrame()
		{
			this._cursorPositionInPreviousFrame = Input.mousePosition;
		}

		// Token: 0x060030BB RID: 12475 RVA: 0x0014D230 File Offset: 0x0014B430
		private void UpdateMouseButtonStatesForCurrentFrame()
		{
			this._wasLeftMouseButtonPressedInCurrentFrame = InputHelper.WasLeftMouseButtonPressedInCurrentFrame();
			this._wasRightMouseButtonPressedInCurrentFrame = InputHelper.WasRightMouseButtonPressedInCurrentFrame();
			this._wasMiddleMouseButtonPressedInCurrentFrame = InputHelper.WasMiddleMouseButtonPressedInCurrentFrame();
			this._wasLeftMouseButtonReleasedInCurrentFrame = InputHelper.WasLeftMouseButtonReleasedInCurrentFrame();
			this._wasRightMouseButtonReleasedInCurrentFrame = InputHelper.WasRightMouseButtonReleasedInCurrentFrame();
			this._wasMiddleMouseButtonReleasedInCurrentFrame = InputHelper.WasMiddleMouseButtonReleasedInCurrentFrame();
			if (this._wasLeftMouseButtonPressedInCurrentFrame)
			{
				this._isLeftMouseButtonDown = true;
			}
			if (this._wasRightMouseButtonPressedInCurrentFrame)
			{
				this._isRightMouseButtonDown = true;
			}
			if (this._wasMiddleMouseButtonPressedInCurrentFrame)
			{
				this._isMiddleMouseButtonDown = true;
			}
			if (this._wasLeftMouseButtonReleasedInCurrentFrame)
			{
				this._isLeftMouseButtonDown = false;
			}
			if (this._wasRightMouseButtonReleasedInCurrentFrame)
			{
				this._isRightMouseButtonDown = false;
			}
			if (this._wasMiddleMouseButtonReleasedInCurrentFrame)
			{
				this._isMiddleMouseButtonDown = false;
			}
		}

		// Token: 0x060030BC RID: 12476 RVA: 0x0014D2DC File Offset: 0x0014B4DC
		private void UpdateCursorPositionAndOffsetInfoForCurrentFrame()
		{
			Vector2 vector = Input.mousePosition;
			this._cursorOffsetSinceLastFrame = vector - this._cursorPositionInPreviousFrame;
			if (this._isLeftMouseButtonDown)
			{
				this._cursorOffsetSinceLeftMouseButtonDown += this._cursorOffsetSinceLastFrame;
			}
			if (this._isRightMouseButtonDown)
			{
				this._cursorOffsetSinceRightMouseButtonDown += this._cursorOffsetSinceLastFrame;
			}
			if (this._isMiddleMouseButtonDown)
			{
				this._cursorOffsetSinceMiddleMouseButtonDown += this._cursorOffsetSinceLastFrame;
			}
			if (this._wasLeftMouseButtonReleasedInCurrentFrame)
			{
				this._cursorOffsetSinceLeftMouseButtonDown = Vector2.zero;
			}
			if (this._wasRightMouseButtonReleasedInCurrentFrame)
			{
				this._cursorOffsetSinceRightMouseButtonDown = Vector2.zero;
			}
			if (this._wasMiddleMouseButtonReleasedInCurrentFrame)
			{
				this._cursorOffsetSinceMiddleMouseButtonDown = Vector2.zero;
			}
			this._cursorPositionInPreviousFrame = vector;
		}

		// Token: 0x04001FA3 RID: 8099
		private bool _isLeftMouseButtonDown;

		// Token: 0x04001FA4 RID: 8100
		private bool _isRightMouseButtonDown;

		// Token: 0x04001FA5 RID: 8101
		private bool _isMiddleMouseButtonDown;

		// Token: 0x04001FA6 RID: 8102
		private bool _wasLeftMouseButtonPressedInCurrentFrame;

		// Token: 0x04001FA7 RID: 8103
		private bool _wasRightMouseButtonPressedInCurrentFrame;

		// Token: 0x04001FA8 RID: 8104
		private bool _wasMiddleMouseButtonPressedInCurrentFrame;

		// Token: 0x04001FA9 RID: 8105
		private bool _wasLeftMouseButtonReleasedInCurrentFrame;

		// Token: 0x04001FAA RID: 8106
		private bool _wasRightMouseButtonReleasedInCurrentFrame;

		// Token: 0x04001FAB RID: 8107
		private bool _wasMiddleMouseButtonReleasedInCurrentFrame;

		// Token: 0x04001FAC RID: 8108
		private Vector2 _cursorPositionInPreviousFrame;

		// Token: 0x04001FAD RID: 8109
		private Vector2 _cursorOffsetSinceLastFrame;

		// Token: 0x04001FAE RID: 8110
		private Vector2 _cursorOffsetSinceLeftMouseButtonDown;

		// Token: 0x04001FAF RID: 8111
		private Vector2 _cursorOffsetSinceRightMouseButtonDown;

		// Token: 0x04001FB0 RID: 8112
		private Vector2 _cursorOffsetSinceMiddleMouseButtonDown;
	}
}
