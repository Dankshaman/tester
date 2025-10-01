using System;
using UnityEngine;

namespace RTEditor
{
	// Token: 0x0200041B RID: 1051
	public class InputDevice : MonoSingletonBase<InputDevice>
	{
		// Token: 0x17000638 RID: 1592
		// (get) Token: 0x0600309C RID: 12444 RVA: 0x0014CFE3 File Offset: 0x0014B1E3
		public static int MaxNumberOfTouches
		{
			get
			{
				return 10;
			}
		}

		// Token: 0x17000639 RID: 1593
		// (get) Token: 0x0600309D RID: 12445 RVA: 0x0001E2E9 File Offset: 0x0001C4E9
		public bool UsingMobile
		{
			get
			{
				return false;
			}
		}

		// Token: 0x1700063A RID: 1594
		// (get) Token: 0x0600309E RID: 12446 RVA: 0x0014CFE7 File Offset: 0x0014B1E7
		public int TouchCount
		{
			get
			{
				return Input.touchCount;
			}
		}

		// Token: 0x0600309F RID: 12447 RVA: 0x0014CFEE File Offset: 0x0014B1EE
		public Vector2 GetDeltaSincePressed(int deviceButtonIndex)
		{
			if (deviceButtonIndex < 0 || deviceButtonIndex >= InputDevice.MaxNumberOfTouches)
			{
				return Vector2.zero;
			}
			return this._deltaSincePressed[deviceButtonIndex];
		}

		// Token: 0x060030A0 RID: 12448 RVA: 0x0014D00E File Offset: 0x0014B20E
		public Vector2 GetDeltaSinceLastFrame(int deviceIndex)
		{
			if (deviceIndex < 0 || deviceIndex >= InputDevice.MaxNumberOfTouches)
			{
				return Vector2.zero;
			}
			return this._deltaSinceLastFrame[deviceIndex];
		}

		// Token: 0x060030A1 RID: 12449 RVA: 0x0014D02E File Offset: 0x0014B22E
		public bool IsPressed(int deviceButtonIndex)
		{
			return Input.GetMouseButton(deviceButtonIndex);
		}

		// Token: 0x060030A2 RID: 12450 RVA: 0x0014D036 File Offset: 0x0014B236
		public bool WasPressedInCurrentFrame(int index)
		{
			return Input.GetMouseButtonDown(index);
		}

		// Token: 0x060030A3 RID: 12451 RVA: 0x0014D03E File Offset: 0x0014B23E
		public bool WasReleasedInCurrentFrame(int index)
		{
			return Input.GetMouseButtonUp(index);
		}

		// Token: 0x060030A4 RID: 12452 RVA: 0x0014D046 File Offset: 0x0014B246
		public bool GetPosition(out Vector2 position)
		{
			position = Input.mousePosition;
			return true;
		}

		// Token: 0x060030A5 RID: 12453 RVA: 0x0014D059 File Offset: 0x0014B259
		public bool GetPickRay(Camera camera, out Ray ray)
		{
			ray = camera.ScreenPointToRay(Input.mousePosition);
			return true;
		}

		// Token: 0x060030A6 RID: 12454 RVA: 0x0014D06D File Offset: 0x0014B26D
		public bool WasMoved()
		{
			return Input.GetAxis("Mouse X") != 0f || Input.GetAxis("Mouse Y") != 0f;
		}

		// Token: 0x060030A7 RID: 12455 RVA: 0x0014D098 File Offset: 0x0014B298
		private void Update()
		{
			Vector2 vector = Input.mousePosition;
			this._deltaSinceLastFrame[0] = vector - this._previousFramePositions[0];
			this._previousFramePositions[0] = vector;
			for (int i = 0; i < 3; i++)
			{
				if (this.WasPressedInCurrentFrame(i) || this.WasReleasedInCurrentFrame(i))
				{
					this._deltaSincePressed[i] = Vector2.zero;
				}
				else if (this.IsPressed(i))
				{
					this._deltaSincePressed[i] += this._deltaSinceLastFrame[0];
				}
			}
		}

		// Token: 0x04001FA0 RID: 8096
		private Vector2[] _previousFramePositions = new Vector2[InputDevice.MaxNumberOfTouches];

		// Token: 0x04001FA1 RID: 8097
		private Vector2[] _deltaSinceLastFrame = new Vector2[InputDevice.MaxNumberOfTouches];

		// Token: 0x04001FA2 RID: 8098
		private Vector2[] _deltaSincePressed = new Vector2[InputDevice.MaxNumberOfTouches];
	}
}
