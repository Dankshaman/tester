using System;
using NewNet;
using UnityEngine;

// Token: 0x020001C1 RID: 449
public class NetworkSingleton<T> : NetworkBehavior where T : MonoBehaviour
{
	// Token: 0x170003D5 RID: 981
	// (get) Token: 0x06001776 RID: 6006 RVA: 0x000A06E0 File Offset: 0x0009E8E0
	public static T Instance
	{
		get
		{
			if (NetworkSingleton<!0>._Instance == null)
			{
				NetworkSingleton<!0>._Instance = UnityEngine.Object.FindObjectOfType<T>();
				if (NetworkSingleton<!0>._Instance == null)
				{
					NetworkSingleton<!0>._Instance = Utilities.FindObjectOfType<T>(true);
				}
				NetworkSingleton<!0>.TriggerStaticInit();
			}
			return NetworkSingleton<!0>._Instance;
		}
	}

	// Token: 0x06001777 RID: 6007 RVA: 0x000A0730 File Offset: 0x0009E930
	protected virtual void Awake()
	{
		NetworkSingleton<!0>._Instance = base.GetComponent<T>();
		NetworkSingleton<!0>.TriggerStaticInit();
	}

	// Token: 0x06001778 RID: 6008 RVA: 0x000A0744 File Offset: 0x0009E944
	private static void TriggerStaticInit()
	{
		if (NetworkSingleton<!0>._Instance)
		{
			NetworkSingleton<T> component = NetworkSingleton<!0>._Instance.GetComponent<NetworkSingleton<T>>();
			if (component)
			{
				component.TriggerInit();
			}
		}
	}

	// Token: 0x06001779 RID: 6009 RVA: 0x000A0780 File Offset: 0x0009E980
	private void TriggerInit()
	{
		if (this.alreadyInit)
		{
			return;
		}
		this.alreadyInit = true;
		this.SingletonInit();
	}

	// Token: 0x0600177A RID: 6010 RVA: 0x000025B8 File Offset: 0x000007B8
	protected virtual void SingletonInit()
	{
	}

	// Token: 0x04000D76 RID: 3446
	private static T _Instance;

	// Token: 0x04000D77 RID: 3447
	private bool alreadyInit;
}
