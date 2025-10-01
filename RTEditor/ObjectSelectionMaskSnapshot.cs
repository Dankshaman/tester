using System;
using System.Collections.Generic;
using UnityEngine;

namespace RTEditor
{
	// Token: 0x020003E7 RID: 999
	public class ObjectSelectionMaskSnapshot
	{
		// Token: 0x170005B6 RID: 1462
		// (get) Token: 0x06002E3D RID: 11837 RVA: 0x0013E0E9 File Offset: 0x0013C2E9
		public HashSet<GameObject> MaskedObjects
		{
			get
			{
				return this._maskedObjects;
			}
		}

		// Token: 0x06002E3E RID: 11838 RVA: 0x0013E0F4 File Offset: 0x0013C2F4
		public void TakeSnapshot()
		{
			EditorObjectSelection instance = MonoSingletonBase<EditorObjectSelection>.Instance;
			this._maskedObjects = instance.MaskedObjects;
		}

		// Token: 0x04001ED9 RID: 7897
		private HashSet<GameObject> _maskedObjects;
	}
}
