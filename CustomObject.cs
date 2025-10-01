using System;
using HighlightingSystem;
using NewNet;
using UnityEngine;

// Token: 0x020000E6 RID: 230
public abstract class CustomObject : NetworkBehavior
{
	// Token: 0x170001E4 RID: 484
	// (get) Token: 0x06000B4F RID: 2895 RVA: 0x0004F031 File Offset: 0x0004D231
	// (set) Token: 0x06000B50 RID: 2896 RVA: 0x0004F039 File Offset: 0x0004D239
	public virtual bool bCustomUI { get; set; }

	// Token: 0x06000B51 RID: 2897 RVA: 0x000025B8 File Offset: 0x000007B8
	public virtual void Cancel()
	{
	}

	// Token: 0x06000B52 RID: 2898 RVA: 0x0004F042 File Offset: 0x0004D242
	public virtual void CallCustomRPC()
	{
		if (Network.isServer && this.FreezeDuringLoad)
		{
			this.NPO.IsLocked = this.CacheFreeze;
			this.NPO.ResetPhysicsMaterial();
			this.NPO.ResetBounds();
		}
	}

	// Token: 0x06000B53 RID: 2899 RVA: 0x000025B8 File Offset: 0x000007B8
	public virtual void CallCustomRPC(NetworkPlayer NP)
	{
	}

	// Token: 0x170001E5 RID: 485
	// (get) Token: 0x06000B54 RID: 2900 RVA: 0x0004F07A File Offset: 0x0004D27A
	// (set) Token: 0x06000B55 RID: 2901 RVA: 0x0004F082 File Offset: 0x0004D282
	private protected NetworkPhysicsObject NPO { protected get; private set; }

	// Token: 0x170001E6 RID: 486
	// (get) Token: 0x06000B56 RID: 2902 RVA: 0x0004F08B File Offset: 0x0004D28B
	// (set) Token: 0x06000B57 RID: 2903 RVA: 0x0004F093 File Offset: 0x0004D293
	private protected Rigidbody rigidbody { protected get; private set; }

	// Token: 0x06000B58 RID: 2904 RVA: 0x0004F09C File Offset: 0x0004D29C
	protected virtual void Awake()
	{
		this.NPO = base.GetComponent<NetworkPhysicsObject>();
		this.rigidbody = base.GetComponent<Rigidbody>();
	}

	// Token: 0x06000B59 RID: 2905 RVA: 0x0004F0B8 File Offset: 0x0004D2B8
	protected virtual void Start()
	{
		this.DummyObject = base.GetComponent<DummyObject>();
		if (this.FreezeDuringLoad && !this.DummyObject)
		{
			if (Network.isServer)
			{
				this.CacheFreeze = this.NPO.IsLocked;
				this.NPO.IsLocked = true;
				this.rigidbody.isKinematic = true;
				return;
			}
			if (base.GetComponent<Collider>())
			{
				base.GetComponent<Collider>().isTrigger = false;
			}
		}
	}

	// Token: 0x06000B5A RID: 2906 RVA: 0x000025B8 File Offset: 0x000007B8
	protected virtual void OnDestroy()
	{
	}

	// Token: 0x170001E7 RID: 487
	// (get) Token: 0x06000B5B RID: 2907 RVA: 0x0004F130 File Offset: 0x0004D330
	// (set) Token: 0x06000B5C RID: 2908 RVA: 0x0004F138 File Offset: 0x0004D338
	public int LoadingCount
	{
		get
		{
			return this.loadingCount;
		}
		private set
		{
			this.loadingCount = value;
		}
	}

	// Token: 0x06000B5D RID: 2909 RVA: 0x0004F144 File Offset: 0x0004D344
	protected void AddLoading()
	{
		int num = this.LoadingCount;
		this.LoadingCount = num + 1;
	}

	// Token: 0x06000B5E RID: 2910 RVA: 0x0004F164 File Offset: 0x0004D364
	protected void RemoveLoading()
	{
		int num = this.LoadingCount;
		this.LoadingCount = num - 1;
		if (this.DummyObject)
		{
			EventManager.TriggerDummyObjectFinish(base.gameObject);
		}
		if (this.LoadingCount <= 0)
		{
			this.OnFinishLoad();
		}
		if (this.LoadingCount < 0)
		{
			this.LoadingCount = 0;
			return;
		}
	}

	// Token: 0x06000B5F RID: 2911 RVA: 0x0004F1B9 File Offset: 0x0004D3B9
	public bool CurrentlyLoading()
	{
		return this.LoadingCount > 0;
	}

	// Token: 0x06000B60 RID: 2912 RVA: 0x0004F1C4 File Offset: 0x0004D3C4
	private void OnFinishLoad()
	{
		if (this.bFinished)
		{
			return;
		}
		Wait.Frames(delegate
		{
			this.bFinished = true;
		}, 1);
	}

	// Token: 0x06000B61 RID: 2913 RVA: 0x0004F1E2 File Offset: 0x0004D3E2
	public void ResetObject()
	{
		if (this.NPO)
		{
			this.NPO.ResetBounds();
			this.NPO.ResetPhysicsMaterial();
		}
		this.DirtyHighlighter();
	}

	// Token: 0x06000B62 RID: 2914 RVA: 0x0004F210 File Offset: 0x0004D410
	public void DirtyHighlighter()
	{
		Highlighter component = base.GetComponent<Highlighter>();
		if (component)
		{
			component.SetDirty();
		}
	}

	// Token: 0x040007E0 RID: 2016
	protected bool bcustomUI;

	// Token: 0x040007E2 RID: 2018
	public bool FreezeDuringLoad;

	// Token: 0x040007E3 RID: 2019
	protected bool CacheFreeze;

	// Token: 0x040007E4 RID: 2020
	public DummyObject DummyObject;

	// Token: 0x040007E7 RID: 2023
	[SerializeField]
	private int loadingCount;

	// Token: 0x040007E8 RID: 2024
	public bool bFinished;
}
