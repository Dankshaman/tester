using System;
using System.Collections;
using UnityEngine;

namespace I2.Loc
{
	// Token: 0x020004A1 RID: 1185
	public class CoroutineManager : MonoBehaviour
	{
		// Token: 0x17000720 RID: 1824
		// (get) Token: 0x06003522 RID: 13602 RVA: 0x00162524 File Offset: 0x00160724
		private static CoroutineManager pInstance
		{
			get
			{
				if (CoroutineManager.mInstance == null)
				{
					GameObject gameObject = new GameObject("_Coroutiner");
					gameObject.hideFlags = HideFlags.HideAndDontSave;
					CoroutineManager.mInstance = gameObject.AddComponent<CoroutineManager>();
					if (Application.isPlaying)
					{
						UnityEngine.Object.DontDestroyOnLoad(gameObject);
					}
				}
				return CoroutineManager.mInstance;
			}
		}

		// Token: 0x06003523 RID: 13603 RVA: 0x0016256E File Offset: 0x0016076E
		private void Awake()
		{
			if (Application.isPlaying)
			{
				UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
			}
		}

		// Token: 0x06003524 RID: 13604 RVA: 0x00162582 File Offset: 0x00160782
		public static Coroutine Start(IEnumerator coroutine)
		{
			return CoroutineManager.pInstance.StartCoroutine(coroutine);
		}

		// Token: 0x04002197 RID: 8599
		private static CoroutineManager mInstance;
	}
}
