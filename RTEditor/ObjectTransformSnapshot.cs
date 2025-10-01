using System;
using UnityEngine;

namespace RTEditor
{
	// Token: 0x0200040B RID: 1035
	public class ObjectTransformSnapshot
	{
		// Token: 0x06003002 RID: 12290 RVA: 0x00147410 File Offset: 0x00145610
		public void TakeSnapshot(GameObject gameObject)
		{
			this._gameObject = gameObject;
			Transform transform = gameObject.transform;
			this._absolutePosition = transform.position;
			this._absoluteRotation = transform.rotation;
			this._absoluteScale = transform.lossyScale;
		}

		// Token: 0x06003003 RID: 12291 RVA: 0x00147450 File Offset: 0x00145650
		public void ApplySnapshot()
		{
			if (this._gameObject != null)
			{
				Transform transform = this._gameObject.transform;
				transform.position = this._absolutePosition;
				transform.rotation = this._absoluteRotation;
				this._gameObject.SetAbsoluteScale(this._absoluteScale);
			}
		}

		// Token: 0x04001F6F RID: 8047
		private GameObject _gameObject;

		// Token: 0x04001F70 RID: 8048
		private Vector3 _absolutePosition;

		// Token: 0x04001F71 RID: 8049
		private Quaternion _absoluteRotation;

		// Token: 0x04001F72 RID: 8050
		private Vector3 _absoluteScale;
	}
}
