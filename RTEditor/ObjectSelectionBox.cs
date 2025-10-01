using System;
using UnityEngine;

namespace RTEditor
{
	// Token: 0x020003D2 RID: 978
	public class ObjectSelectionBox
	{
		// Token: 0x06002DE5 RID: 11749 RVA: 0x0013D9AA File Offset: 0x0013BBAA
		public ObjectSelectionBox()
		{
			this._modelSpaceBox = Box.GetInvalid();
			this._transformMatrix = Matrix4x4.identity;
		}

		// Token: 0x06002DE6 RID: 11750 RVA: 0x0013D9C8 File Offset: 0x0013BBC8
		public ObjectSelectionBox(Box modelSpaceBox)
		{
			this._modelSpaceBox = modelSpaceBox;
			this._transformMatrix = Matrix4x4.identity;
		}

		// Token: 0x06002DE7 RID: 11751 RVA: 0x0013D9E2 File Offset: 0x0013BBE2
		public ObjectSelectionBox(Box modelSpaceBox, Matrix4x4 transformMatrix)
		{
			this._modelSpaceBox = modelSpaceBox;
			this._transformMatrix = transformMatrix;
		}

		// Token: 0x17000591 RID: 1425
		// (get) Token: 0x06002DE8 RID: 11752 RVA: 0x0013D9F8 File Offset: 0x0013BBF8
		// (set) Token: 0x06002DE9 RID: 11753 RVA: 0x0013DA00 File Offset: 0x0013BC00
		public Box ModelSpaceBox
		{
			get
			{
				return this._modelSpaceBox;
			}
			set
			{
				this._modelSpaceBox = value;
			}
		}

		// Token: 0x17000592 RID: 1426
		// (get) Token: 0x06002DEA RID: 11754 RVA: 0x0013DA09 File Offset: 0x0013BC09
		// (set) Token: 0x06002DEB RID: 11755 RVA: 0x0013DA11 File Offset: 0x0013BC11
		public Matrix4x4 TransformMatrix
		{
			get
			{
				return this._transformMatrix;
			}
			set
			{
				this._transformMatrix = value;
			}
		}

		// Token: 0x04001E9A RID: 7834
		private Box _modelSpaceBox;

		// Token: 0x04001E9B RID: 7835
		private Matrix4x4 _transformMatrix;
	}
}
