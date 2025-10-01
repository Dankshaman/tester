using System;
using UnityEngine;

namespace RTEditor
{
	// Token: 0x0200045B RID: 1115
	public class MonoSingletonBase<T> : MonoBehaviour where T : MonoBehaviour
	{
		// Token: 0x1700070E RID: 1806
		// (get) Token: 0x060032E7 RID: 13031 RVA: 0x00154B3C File Offset: 0x00152D3C
		public static T Instance
		{
			get
			{
				if (MonoSingletonBase<!0>._instance == null)
				{
					object singletonLock = MonoSingletonBase<!0>._singletonLock;
					lock (singletonLock)
					{
						T[] array = UnityEngine.Object.FindObjectsOfType(typeof(!0)) as !0[];
						if (array.Length == 0)
						{
							return default(!0);
						}
						if (array.Length > 1)
						{
							if (Application.isEditor)
							{
								Debug.LogWarning("MonoSingleton<T>.Instance: Only 1 singleton instance can exist in the scene. Null will be returned.");
							}
							return default(!0);
						}
						MonoSingletonBase<!0>._instance = array[0];
					}
				}
				return MonoSingletonBase<!0>._instance;
			}
		}

		// Token: 0x040020A4 RID: 8356
		private static object _singletonLock = new object();

		// Token: 0x040020A5 RID: 8357
		private static T _instance;
	}
}
