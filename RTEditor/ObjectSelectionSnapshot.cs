using System;
using System.Collections.Generic;
using UnityEngine;

namespace RTEditor
{
	// Token: 0x020003E8 RID: 1000
	public class ObjectSelectionSnapshot
	{
		// Token: 0x170005B7 RID: 1463
		// (get) Token: 0x06002E40 RID: 11840 RVA: 0x0013E113 File Offset: 0x0013C313
		public HashSet<GameObject> SelectedGameObjects
		{
			get
			{
				return this._selectedGameObjects;
			}
		}

		// Token: 0x170005B8 RID: 1464
		// (get) Token: 0x06002E41 RID: 11841 RVA: 0x0013E11B File Offset: 0x0013C31B
		public GameObject LastSelectedGameObject
		{
			get
			{
				return this._lastSelectedGameObject;
			}
		}

		// Token: 0x170005B9 RID: 1465
		// (get) Token: 0x06002E42 RID: 11842 RVA: 0x0013E123 File Offset: 0x0013C323
		public int NumberOfSelectedObjects
		{
			get
			{
				return this._selectedGameObjects.Count;
			}
		}

		// Token: 0x06002E43 RID: 11843 RVA: 0x0013E130 File Offset: 0x0013C330
		public void TakeSnapshot()
		{
			EditorObjectSelection instance = MonoSingletonBase<EditorObjectSelection>.Instance;
			this._selectedGameObjects = new HashSet<GameObject>(instance.SelectedGameObjects);
			this._lastSelectedGameObject = instance.LastSelectedGameObject;
		}

		// Token: 0x06002E44 RID: 11844 RVA: 0x0013E160 File Offset: 0x0013C360
		public bool Contains(GameObject gameObject)
		{
			return this._selectedGameObjects.Contains(gameObject);
		}

		// Token: 0x06002E45 RID: 11845 RVA: 0x0013E170 File Offset: 0x0013C370
		public List<GameObject> GetDiff(ObjectSelectionSnapshot other)
		{
			if (this.NumberOfSelectedObjects == 0)
			{
				return new List<GameObject>();
			}
			List<GameObject> list = new List<GameObject>();
			foreach (GameObject gameObject in this._selectedGameObjects)
			{
				if (!other.Contains(gameObject))
				{
					list.Add(gameObject);
				}
			}
			return list;
		}

		// Token: 0x04001EDA RID: 7898
		private HashSet<GameObject> _selectedGameObjects = new HashSet<GameObject>();

		// Token: 0x04001EDB RID: 7899
		private GameObject _lastSelectedGameObject;
	}
}
