using System;
using System.Collections.Generic;
using NewNet;
using UnityEngine;

// Token: 0x020001EB RID: 491
public class RPGFigurines : NetworkBehavior
{
	// Token: 0x1700040F RID: 1039
	// (get) Token: 0x060019DD RID: 6621 RVA: 0x000B5174 File Offset: 0x000B3374
	// (set) Token: 0x060019DE RID: 6622 RVA: 0x000B517C File Offset: 0x000B337C
	[Sync(Permission.Owner, null, SerializationMethod.Default, false)]
	public bool bMode
	{
		get
		{
			return this._bMode;
		}
		set
		{
			if (value != this._bMode)
			{
				this._bMode = value;
				base.DirtySync("bMode");
			}
		}
	}

	// Token: 0x17000410 RID: 1040
	// (get) Token: 0x060019DF RID: 6623 RVA: 0x000B5199 File Offset: 0x000B3399
	// (set) Token: 0x060019E0 RID: 6624 RVA: 0x000B51A1 File Offset: 0x000B33A1
	[Sync(Permission.Owner, null, SerializationMethod.Default, false)]
	public bool bDead
	{
		get
		{
			return this._bDead;
		}
		set
		{
			if (value != this._bDead)
			{
				this._bDead = value;
				base.DirtySync("bDead");
			}
		}
	}

	// Token: 0x060019E1 RID: 6625 RVA: 0x000B51BE File Offset: 0x000B33BE
	public override void OnSync()
	{
		this.UpdateAnims();
	}

	// Token: 0x060019E2 RID: 6626 RVA: 0x000B51C8 File Offset: 0x000B33C8
	private void Start()
	{
		base.transform.localScale *= 0.9f;
		foreach (object obj in base.GetComponent<Animation>())
		{
			AnimationState animationState = (AnimationState)obj;
			this.AnimationArray.Add(animationState.name);
		}
		this.ReferenceNPO = base.GetComponent<NetworkPhysicsObject>();
		this.UpdateAnims();
	}

	// Token: 0x060019E3 RID: 6627 RVA: 0x000B5258 File Offset: 0x000B3458
	private void Update()
	{
		if (this.enhancedPrecisionBase != SystemConsole.EnhancedFigurinePrecision)
		{
			this.UpdateBase(SystemConsole.EnhancedFigurinePrecision);
		}
		if (base.GetComponent<Animation>().IsPlaying(this.OverrideAnim) && base.GetComponent<Animation>()[this.OverrideAnim].time <= base.GetComponent<Animation>()[this.OverrideAnim].length)
		{
			return;
		}
		this.OverrideAnim = "";
		if (this.bDead)
		{
			return;
		}
		if (this.ReferenceNPO.IsHeldByNobody)
		{
			base.GetComponent<Animation>().CrossFade(this.CurrentIdle, 0.4f);
			return;
		}
		Vector3 vector = new Vector3(base.GetComponent<Rigidbody>().position.x, 0f, base.GetComponent<Rigidbody>().position.z);
		if (Vector3.Distance(vector, this.PrevLoc) < 0.025f)
		{
			base.GetComponent<Animation>().CrossFade(this.CurrentWalk, 0.4f);
		}
		else
		{
			base.GetComponent<Animation>().CrossFade(this.CurrentRun, 0.4f);
		}
		this.PrevLoc = vector;
	}

	// Token: 0x060019E4 RID: 6628 RVA: 0x000B536B File Offset: 0x000B356B
	public void Attack()
	{
		base.networkView.RPC(RPCTarget.All, new Action(this.RPCAttack));
	}

	// Token: 0x060019E5 RID: 6629 RVA: 0x000B5388 File Offset: 0x000B3588
	[Remote(Permission.Client, SendType.ReliableBuffered, null, SerializationMethod.Default)]
	private void RPCAttack()
	{
		this.OverrideAnim = this.AnimationArray[this.CurrentAttacks[UnityEngine.Random.Range(0, this.CurrentAttacks.Length)]];
		Debug.Log(this.OverrideAnim);
		base.Invoke("AttackCheck", 0.25f);
		this.bDead = false;
		if (this.OverrideAnim != "")
		{
			base.GetComponent<Animation>().CrossFade(this.OverrideAnim, 0.2f);
			base.GetComponent<Animation>()[this.OverrideAnim].wrapMode = WrapMode.ClampForever;
		}
	}

	// Token: 0x060019E6 RID: 6630 RVA: 0x000B541C File Offset: 0x000B361C
	private void AttackCheck()
	{
		base.transform.Find("Attack").GetComponent<AttackCheckTrigger>().AttackRPG();
	}

	// Token: 0x060019E7 RID: 6631 RVA: 0x000B5438 File Offset: 0x000B3638
	public void GetHit()
	{
		if (this.bDead)
		{
			return;
		}
		Achievements.Set("ACH_RPG_FIGURINES_ATTACK");
		this.OverrideAnim = this.CurrentGetHit;
		if (this.OverrideAnim != "")
		{
			base.GetComponent<Animation>().CrossFade(this.OverrideAnim, 0.25f);
			base.GetComponent<Animation>()[this.OverrideAnim].wrapMode = WrapMode.ClampForever;
		}
	}

	// Token: 0x060019E8 RID: 6632 RVA: 0x000B54A3 File Offset: 0x000B36A3
	public void ChangeMode()
	{
		Debug.Log("Change Mode");
		if (Network.isClient)
		{
			base.networkView.RPC(RPCTarget.Server, new Action(this.RPCChangeMode));
			return;
		}
		this.RPCChangeMode();
	}

	// Token: 0x060019E9 RID: 6633 RVA: 0x000B54D5 File Offset: 0x000B36D5
	[Remote(Permission.Client, SendType.ReliableBuffered, null, SerializationMethod.Default)]
	public void RPCChangeMode()
	{
		this.bMode = !this.bMode;
		this.bDead = false;
		this.UpdateAnims();
	}

	// Token: 0x060019EA RID: 6634 RVA: 0x000B54F3 File Offset: 0x000B36F3
	public void Die()
	{
		if (Network.isClient)
		{
			base.networkView.RPC(RPCTarget.Server, new Action(this.RPCDie));
			return;
		}
		this.RPCDie();
	}

	// Token: 0x060019EB RID: 6635 RVA: 0x000B551C File Offset: 0x000B371C
	[Remote(Permission.Client, SendType.ReliableBuffered, null, SerializationMethod.Default)]
	public void RPCDie()
	{
		this.OverrideAnim = this.CurrentDie;
		this.bDead = !this.bDead;
		if (this.bDead && this.OverrideAnim != "")
		{
			base.GetComponent<Animation>().CrossFade(this.OverrideAnim, 0.25f);
			base.GetComponent<Animation>()[this.OverrideAnim].wrapMode = WrapMode.ClampForever;
		}
	}

	// Token: 0x060019EC RID: 6636 RVA: 0x000B558C File Offset: 0x000B378C
	private void UpdateAnims()
	{
		if (this.AnimationArray.Count == 0)
		{
			return;
		}
		if (!this.bMode)
		{
			this.CurrentIdle = this.AnimationArray[this.idle];
			this.CurrentWalk = this.AnimationArray[this.walk];
			this.CurrentRun = this.AnimationArray[this.run];
			this.CurrentGetHit = this.AnimationArray[this.gethit];
			this.CurrentDie = this.AnimationArray[this.die];
			if (this.attacks.Length != 0)
			{
				this.CurrentAttacks = this.attacks;
			}
			else
			{
				this.CurrentAttacks = this.attacks2;
			}
			for (int i = 0; i < this.Weapon.Length; i++)
			{
				this.Weapon[i].GetComponent<Renderer>().enabled = true;
			}
			for (int j = 0; j < this.Weapon2.Length; j++)
			{
				this.Weapon2[j].GetComponent<Renderer>().enabled = false;
			}
		}
		else
		{
			this.CurrentIdle = this.AnimationArray[this.idle2];
			this.CurrentWalk = this.AnimationArray[this.walk2];
			this.CurrentRun = this.AnimationArray[this.run2];
			this.CurrentGetHit = this.AnimationArray[this.gethit2];
			this.CurrentDie = this.AnimationArray[this.die2];
			if (this.attacks2.Length != 0)
			{
				this.CurrentAttacks = this.attacks2;
			}
			else
			{
				this.CurrentAttacks = this.attacks;
			}
			for (int k = 0; k < this.Weapon2.Length; k++)
			{
				this.Weapon2[k].GetComponent<Renderer>().enabled = true;
			}
			for (int l = 0; l < this.Weapon.Length; l++)
			{
				this.Weapon[l].GetComponent<Renderer>().enabled = false;
			}
		}
		if (this.bDead)
		{
			this.OverrideAnim = this.CurrentDie;
			if (this.bDead && this.OverrideAnim != "")
			{
				base.GetComponent<Animation>().CrossFade(this.OverrideAnim, 0.25f);
				base.GetComponent<Animation>()[this.OverrideAnim].wrapMode = WrapMode.ClampForever;
			}
		}
	}

	// Token: 0x060019ED RID: 6637 RVA: 0x000B57D8 File Offset: 0x000B39D8
	private void UpdateBase(bool enhanced)
	{
		this.enhancedPrecisionBase = enhanced;
		GameObject gameObject = base.transform.Find("!Base").gameObject;
		GameObject gameObject2 = base.transform.Find("!Cube").gameObject;
		if (gameObject == null || gameObject2 == null)
		{
			return;
		}
		MeshCollider meshCollider = gameObject.GetComponent<MeshCollider>();
		if (enhanced)
		{
			if (meshCollider == null)
			{
				meshCollider = gameObject.AddComponent<MeshCollider>();
				meshCollider.convex = true;
				meshCollider.sharedMesh = gameObject.GetComponent<MeshFilter>().sharedMesh;
			}
			meshCollider.enabled = true;
			gameObject2.SetActive(false);
			return;
		}
		if (meshCollider)
		{
			meshCollider.enabled = false;
		}
		gameObject2.SetActive(true);
	}

	// Token: 0x04000FDF RID: 4063
	private NetworkPhysicsObject ReferenceNPO;

	// Token: 0x04000FE0 RID: 4064
	public int idle;

	// Token: 0x04000FE1 RID: 4065
	public int idle2;

	// Token: 0x04000FE2 RID: 4066
	public int walk;

	// Token: 0x04000FE3 RID: 4067
	public int walk2;

	// Token: 0x04000FE4 RID: 4068
	public int run;

	// Token: 0x04000FE5 RID: 4069
	public int run2;

	// Token: 0x04000FE6 RID: 4070
	public int gethit;

	// Token: 0x04000FE7 RID: 4071
	public int gethit2;

	// Token: 0x04000FE8 RID: 4072
	public int die;

	// Token: 0x04000FE9 RID: 4073
	public int die2;

	// Token: 0x04000FEA RID: 4074
	public int[] attacks;

	// Token: 0x04000FEB RID: 4075
	public int[] attacks2;

	// Token: 0x04000FEC RID: 4076
	public GameObject[] Weapon;

	// Token: 0x04000FED RID: 4077
	public GameObject[] Weapon2;

	// Token: 0x04000FEE RID: 4078
	private List<string> AnimationArray = new List<string>();

	// Token: 0x04000FEF RID: 4079
	private string CurrentIdle;

	// Token: 0x04000FF0 RID: 4080
	private string CurrentWalk;

	// Token: 0x04000FF1 RID: 4081
	private string CurrentRun;

	// Token: 0x04000FF2 RID: 4082
	private string CurrentGetHit;

	// Token: 0x04000FF3 RID: 4083
	private string CurrentDie;

	// Token: 0x04000FF4 RID: 4084
	private int[] CurrentAttacks;

	// Token: 0x04000FF5 RID: 4085
	private string OverrideAnim = "";

	// Token: 0x04000FF6 RID: 4086
	private bool _bMode;

	// Token: 0x04000FF7 RID: 4087
	private bool _bDead;

	// Token: 0x04000FF8 RID: 4088
	private Vector3 PrevLoc = Vector3.zero;

	// Token: 0x04000FF9 RID: 4089
	private bool enhancedPrecisionBase;
}
