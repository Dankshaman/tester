using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using MoonSharp.Interpreter;
using NewNet;
using UI.Xml;
using UnityEngine;
using UnityEngine.Rendering;

// Token: 0x02000188 RID: 392
[MoonSharpHideMember("Invoke")]
[MoonSharpHideMember("InvokeRepeating")]
[MoonSharpHideMember("BroadcastMessage")]
[MoonSharpHideMember("SendMessage")]
[MoonSharpHideMember("SendMessageUpwards")]
[MoonSharpHideMember("networkView")]
public class LuaGameObjectScript : LuaScript
{
	// Token: 0x170002FD RID: 765
	// (get) Token: 0x060011DB RID: 4571 RVA: 0x000797DB File Offset: 0x000779DB
	public LuaClock Clock
	{
		get
		{
			base.enabled = true;
			if (this._Clock == null && this.NPO.clockScript != null)
			{
				this._Clock = new LuaClock(this);
			}
			return this._Clock;
		}
	}

	// Token: 0x170002FE RID: 766
	// (get) Token: 0x060011DC RID: 4572 RVA: 0x00079811 File Offset: 0x00077A11
	public LuaCounter Counter
	{
		get
		{
			base.enabled = true;
			if (this._Counter == null && this.NPO.counterScript != null)
			{
				this._Counter = new LuaCounter(this);
			}
			return this._Counter;
		}
	}

	// Token: 0x170002FF RID: 767
	// (get) Token: 0x060011DD RID: 4573 RVA: 0x00079847 File Offset: 0x00077A47
	public LuaRPGFigurine RPGFigurine
	{
		get
		{
			base.enabled = true;
			if (this._RPGFigurine == null && this.NPO.rpgFigurines != null)
			{
				this._RPGFigurine = new LuaRPGFigurine(this);
			}
			return this._RPGFigurine;
		}
	}

	// Token: 0x17000300 RID: 768
	// (get) Token: 0x060011DE RID: 4574 RVA: 0x0007987D File Offset: 0x00077A7D
	public LuaBook Book
	{
		get
		{
			base.enabled = true;
			if (this._Book == null && this.NPO.customPDF != null)
			{
				this._Book = new LuaBook(this);
			}
			return this._Book;
		}
	}

	// Token: 0x17000301 RID: 769
	// (get) Token: 0x060011DF RID: 4575 RVA: 0x000798B3 File Offset: 0x00077AB3
	public LuaBrowser Browser
	{
		get
		{
			base.enabled = true;
			if (this._Browser == null && this.NPO.tabletScript != null)
			{
				this._Browser = new LuaBrowser(this);
			}
			return this._Browser;
		}
	}

	// Token: 0x17000302 RID: 770
	// (get) Token: 0x060011E0 RID: 4576 RVA: 0x000798E9 File Offset: 0x00077AE9
	public LuaTextTool TextTool
	{
		get
		{
			base.enabled = true;
			if (this._TextTool == null && this.NPO.textTool != null)
			{
				this._TextTool = new LuaTextTool(this);
			}
			return this._TextTool;
		}
	}

	// Token: 0x17000303 RID: 771
	// (get) Token: 0x060011E1 RID: 4577 RVA: 0x0007991F File Offset: 0x00077B1F
	public LuaAssetBundle AssetBundle
	{
		get
		{
			base.enabled = true;
			if (this._AssetBundle == null && this.NPO.customAssetbundle != null)
			{
				this._AssetBundle = new LuaAssetBundle(this);
			}
			return this._AssetBundle;
		}
	}

	// Token: 0x17000304 RID: 772
	// (get) Token: 0x060011E2 RID: 4578 RVA: 0x00079958 File Offset: 0x00077B58
	public LuaContainer Container
	{
		get
		{
			base.enabled = true;
			if (this._Container == null)
			{
				if (this.NPO.stackObject)
				{
					this._Container = new LuaStack(this);
				}
				else if (this.NPO.deckScript)
				{
					this._Container = new LuaDeck(this);
				}
				else if (this.NPO.cardScript)
				{
					this._Container = new LuaCard(this);
				}
			}
			return this._Container;
		}
	}

	// Token: 0x17000305 RID: 773
	// (get) Token: 0x060011E3 RID: 4579 RVA: 0x000799D8 File Offset: 0x00077BD8
	public LuaZone Zone
	{
		get
		{
			base.enabled = true;
			if (this._Zone == null)
			{
				if (this.NPO.layoutZone)
				{
					this._Zone = new LuaLayoutZone(this);
				}
				else if (this.NPO.zone)
				{
					this._Zone = new LuaZone(this);
				}
			}
			return this._Zone;
		}
	}

	// Token: 0x17000306 RID: 774
	// (get) Token: 0x060011E4 RID: 4580 RVA: 0x00079A38 File Offset: 0x00077C38
	public LuaLayoutZone LayoutZone
	{
		get
		{
			base.enabled = true;
			if (this._LayoutZone == null && this.NPO.layoutZone)
			{
				this._LayoutZone = new LuaLayoutZone(this);
			}
			return this._LayoutZone;
		}
	}

	// Token: 0x17000307 RID: 775
	// (get) Token: 0x060011E5 RID: 4581 RVA: 0x00079A6D File Offset: 0x00077C6D
	public new string name
	{
		get
		{
			return base.InternalName;
		}
	}

	// Token: 0x17000308 RID: 776
	// (get) Token: 0x060011E6 RID: 4582 RVA: 0x00079A75 File Offset: 0x00077C75
	[Obsolete("Use `type` instead.")]
	public new string tag
	{
		get
		{
			return base.gameObject.tag;
		}
	}

	// Token: 0x17000309 RID: 777
	// (get) Token: 0x060011E7 RID: 4583 RVA: 0x00079A75 File Offset: 0x00077C75
	public string type
	{
		get
		{
			return base.gameObject.tag;
		}
	}

	// Token: 0x1700030A RID: 778
	// (get) Token: 0x060011E8 RID: 4584 RVA: 0x00079A82 File Offset: 0x00077C82
	public override string guid
	{
		get
		{
			if (!this.IsGlobalDummyObject)
			{
				return this.NPO.GUID;
			}
			return "-1";
		}
	}

	// Token: 0x1700030B RID: 779
	// (get) Token: 0x060011E9 RID: 4585 RVA: 0x00079A9D File Offset: 0x00077C9D
	// (set) Token: 0x060011EA RID: 4586 RVA: 0x00079AAF File Offset: 0x00077CAF
	public bool resting
	{
		get
		{
			return this.NPO.rigidbody.IsSleeping();
		}
		set
		{
			if (value)
			{
				this.NPO.rigidbody.Sleep();
				return;
			}
			this.NPO.rigidbody.WakeUp();
		}
	}

	// Token: 0x1700030C RID: 780
	// (get) Token: 0x060011EB RID: 4587 RVA: 0x00079AD5 File Offset: 0x00077CD5
	// (set) Token: 0x060011EC RID: 4588 RVA: 0x00079AE2 File Offset: 0x00077CE2
	public float mass
	{
		get
		{
			return this.NPO.GetMass();
		}
		set
		{
			this.NPO.SetMass(value);
		}
	}

	// Token: 0x1700030D RID: 781
	// (get) Token: 0x060011ED RID: 4589 RVA: 0x00079AF0 File Offset: 0x00077CF0
	// (set) Token: 0x060011EE RID: 4590 RVA: 0x00079AFD File Offset: 0x00077CFD
	public bool use_gravity
	{
		get
		{
			return this.NPO.GetUseGravity();
		}
		set
		{
			this.NPO.SetUseGravity(value);
		}
	}

	// Token: 0x1700030E RID: 782
	// (get) Token: 0x060011EF RID: 4591 RVA: 0x00079B0B File Offset: 0x00077D0B
	// (set) Token: 0x060011F0 RID: 4592 RVA: 0x00079B18 File Offset: 0x00077D18
	public float drag
	{
		get
		{
			return this.NPO.GetDrag();
		}
		set
		{
			this.NPO.SetDrag(value);
		}
	}

	// Token: 0x1700030F RID: 783
	// (get) Token: 0x060011F1 RID: 4593 RVA: 0x00079B26 File Offset: 0x00077D26
	// (set) Token: 0x060011F2 RID: 4594 RVA: 0x00079B33 File Offset: 0x00077D33
	public float angular_drag
	{
		get
		{
			return this.NPO.GetAngularDrag();
		}
		set
		{
			this.NPO.SetAngularDrag(value);
		}
	}

	// Token: 0x17000310 RID: 784
	// (get) Token: 0x060011F3 RID: 4595 RVA: 0x00079B41 File Offset: 0x00077D41
	// (set) Token: 0x060011F4 RID: 4596 RVA: 0x00079B4E File Offset: 0x00077D4E
	public float static_friction
	{
		get
		{
			return this.NPO.GetStaticFriction();
		}
		set
		{
			this.NPO.SetStaticFriction(value);
		}
	}

	// Token: 0x17000311 RID: 785
	// (get) Token: 0x060011F5 RID: 4597 RVA: 0x00079B5C File Offset: 0x00077D5C
	// (set) Token: 0x060011F6 RID: 4598 RVA: 0x00079B69 File Offset: 0x00077D69
	public float dynamic_friction
	{
		get
		{
			return this.NPO.GetDynamicFriction();
		}
		set
		{
			this.NPO.SetDynamicFriction(value);
		}
	}

	// Token: 0x17000312 RID: 786
	// (get) Token: 0x060011F7 RID: 4599 RVA: 0x00079B77 File Offset: 0x00077D77
	// (set) Token: 0x060011F8 RID: 4600 RVA: 0x00079B84 File Offset: 0x00077D84
	public float bounciness
	{
		get
		{
			return this.NPO.GetBounciness();
		}
		set
		{
			this.NPO.SetBounciness(value);
		}
	}

	// Token: 0x17000313 RID: 787
	// (get) Token: 0x060011F9 RID: 4601 RVA: 0x00079B92 File Offset: 0x00077D92
	public string held_by_color
	{
		get
		{
			if (this.NPO.IsHeldBySomebody && NetworkSingleton<PlayerManager>.Instance.ContainsID(this.NPO.HeldByPlayerID))
			{
				return NetworkSingleton<PlayerManager>.Instance.ColourLabelFromID(this.NPO.HeldByPlayerID);
			}
			return null;
		}
	}

	// Token: 0x17000314 RID: 788
	// (get) Token: 0x060011FA RID: 4602 RVA: 0x00079BCF File Offset: 0x00077DCF
	// (set) Token: 0x060011FB RID: 4603 RVA: 0x00079BDC File Offset: 0x00077DDC
	public Vector3 held_position_offset
	{
		get
		{
			return this.NPO.HeldOffset;
		}
		set
		{
			this.NPO.HeldOffset = value;
		}
	}

	// Token: 0x17000315 RID: 789
	// (get) Token: 0x060011FC RID: 4604 RVA: 0x00079BEA File Offset: 0x00077DEA
	// (set) Token: 0x060011FD RID: 4605 RVA: 0x00079BF7 File Offset: 0x00077DF7
	public Vector3 held_rotation_offset
	{
		get
		{
			return this.NPO.HeldRotationOffset;
		}
		set
		{
			this.NPO.HeldRotationOffset = value;
		}
	}

	// Token: 0x17000316 RID: 790
	// (get) Token: 0x060011FE RID: 4606 RVA: 0x00079C05 File Offset: 0x00077E05
	// (set) Token: 0x060011FF RID: 4607 RVA: 0x00079C12 File Offset: 0x00077E12
	public int held_spin_index
	{
		get
		{
			return this.NPO.HeldSpinRotationIndex;
		}
		set
		{
			this.NPO.HeldSpinRotationIndex = value;
		}
	}

	// Token: 0x17000317 RID: 791
	// (get) Token: 0x06001200 RID: 4608 RVA: 0x00079C20 File Offset: 0x00077E20
	// (set) Token: 0x06001201 RID: 4609 RVA: 0x00079C2D File Offset: 0x00077E2D
	public int held_flip_index
	{
		get
		{
			return this.NPO.HeldFlipRotationIndex;
		}
		set
		{
			this.NPO.HeldFlipRotationIndex = value;
		}
	}

	// Token: 0x17000318 RID: 792
	// (get) Token: 0x06001202 RID: 4610 RVA: 0x00079C3B File Offset: 0x00077E3B
	// (set) Token: 0x06001203 RID: 4611 RVA: 0x00079C48 File Offset: 0x00077E48
	public bool held_reduce_force
	{
		get
		{
			return this.NPO.bReduceForce;
		}
		set
		{
			this.NPO.bReduceForce = value;
		}
	}

	// Token: 0x17000319 RID: 793
	// (get) Token: 0x06001204 RID: 4612 RVA: 0x00079C56 File Offset: 0x00077E56
	public Vector3 pick_up_position
	{
		get
		{
			return this.NPO.PickedUpPosition;
		}
	}

	// Token: 0x1700031A RID: 794
	// (get) Token: 0x06001205 RID: 4613 RVA: 0x00079C63 File Offset: 0x00077E63
	public Vector3 pick_up_rotation
	{
		get
		{
			return this.NPO.PickedUpRotation;
		}
	}

	// Token: 0x1700031B RID: 795
	// (get) Token: 0x06001206 RID: 4614 RVA: 0x00079C70 File Offset: 0x00077E70
	// (set) Token: 0x06001207 RID: 4615 RVA: 0x00079C7D File Offset: 0x00077E7D
	public bool locked
	{
		get
		{
			return this.NPO.IsLocked;
		}
		set
		{
			this.NPO.IsLocked = value;
		}
	}

	// Token: 0x1700031C RID: 796
	// (get) Token: 0x06001208 RID: 4616 RVA: 0x00079C8B File Offset: 0x00077E8B
	// (set) Token: 0x06001209 RID: 4617 RVA: 0x00079C98 File Offset: 0x00077E98
	public bool use_rotation_value_flip
	{
		get
		{
			return this.NPO.RotatesThroughRotationValues;
		}
		set
		{
			this.NPO.RotatesThroughRotationValues = value;
		}
	}

	// Token: 0x1700031D RID: 797
	// (get) Token: 0x0600120A RID: 4618 RVA: 0x00079CA6 File Offset: 0x00077EA6
	// (set) Token: 0x0600120B RID: 4619 RVA: 0x00079CB3 File Offset: 0x00077EB3
	public bool ignore_fog_of_war
	{
		get
		{
			return this.NPO.IgnoresFogOfWar;
		}
		set
		{
			this.NPO.IgnoresFogOfWar = value;
		}
	}

	// Token: 0x1700031E RID: 798
	// (get) Token: 0x0600120C RID: 4620 RVA: 0x00079CC1 File Offset: 0x00077EC1
	public bool is_face_down
	{
		get
		{
			return this.NPO.IsFaceDown;
		}
	}

	// Token: 0x1700031F RID: 799
	// (get) Token: 0x0600120D RID: 4621 RVA: 0x00079CCE File Offset: 0x00077ECE
	// (set) Token: 0x0600120E RID: 4622 RVA: 0x00079CDB File Offset: 0x00077EDB
	public bool hide_when_face_down
	{
		get
		{
			return this.NPO.IsHiddenWhenFaceDown;
		}
		set
		{
			this.NPO.IsHiddenWhenFaceDown = value;
		}
	}

	// Token: 0x17000320 RID: 800
	// (get) Token: 0x0600120F RID: 4623 RVA: 0x00079CE9 File Offset: 0x00077EE9
	// (set) Token: 0x06001210 RID: 4624 RVA: 0x00079CF9 File Offset: 0x00077EF9
	public bool use_grid
	{
		get
		{
			return !this.NPO.IgnoresGrid;
		}
		set
		{
			this.NPO.IgnoresGrid = !value;
		}
	}

	// Token: 0x17000321 RID: 801
	// (get) Token: 0x06001211 RID: 4625 RVA: 0x00079D0A File Offset: 0x00077F0A
	// (set) Token: 0x06001212 RID: 4626 RVA: 0x00079D1A File Offset: 0x00077F1A
	public bool use_snap_points
	{
		get
		{
			return !this.NPO.IgnoresSnap;
		}
		set
		{
			this.NPO.IgnoresSnap = !value;
		}
	}

	// Token: 0x17000322 RID: 802
	// (get) Token: 0x06001213 RID: 4627 RVA: 0x00079D2B File Offset: 0x00077F2B
	// (set) Token: 0x06001214 RID: 4628 RVA: 0x00079D38 File Offset: 0x00077F38
	public bool auto_raise
	{
		get
		{
			return this.NPO.DoAutoRaise;
		}
		set
		{
			this.NPO.DoAutoRaise = value;
		}
	}

	// Token: 0x17000323 RID: 803
	// (get) Token: 0x06001215 RID: 4629 RVA: 0x00079D46 File Offset: 0x00077F46
	// (set) Token: 0x06001216 RID: 4630 RVA: 0x00079D53 File Offset: 0x00077F53
	public bool sticky
	{
		get
		{
			return this.NPO.IsSticky;
		}
		set
		{
			this.NPO.IsSticky = value;
		}
	}

	// Token: 0x17000324 RID: 804
	// (get) Token: 0x06001217 RID: 4631 RVA: 0x00079D61 File Offset: 0x00077F61
	// (set) Token: 0x06001218 RID: 4632 RVA: 0x00079D6E File Offset: 0x00077F6E
	public bool tooltip
	{
		get
		{
			return this.NPO.ShowTooltip;
		}
		set
		{
			this.NPO.ShowTooltip = value;
		}
	}

	// Token: 0x17000325 RID: 805
	// (get) Token: 0x06001219 RID: 4633 RVA: 0x00079D7C File Offset: 0x00077F7C
	// (set) Token: 0x0600121A RID: 4634 RVA: 0x00079DAD File Offset: 0x00077FAD
	public bool interactable
	{
		get
		{
			if (!(this.NPO.textTool != null))
			{
				return this.NPO.IsGrabbable;
			}
			return this.NPO.textTool.interactable;
		}
		set
		{
			if (this.NPO.textTool != null)
			{
				this.NPO.textTool.interactable = value;
				return;
			}
			this.NPO.IsGrabbable = value;
		}
	}

	// Token: 0x17000326 RID: 806
	// (get) Token: 0x0600121B RID: 4635 RVA: 0x00079DE0 File Offset: 0x00077FE0
	// (set) Token: 0x0600121C RID: 4636 RVA: 0x00079DED File Offset: 0x00077FED
	public bool grid_projection
	{
		get
		{
			return this.NPO.ShowGridProjection;
		}
		set
		{
			this.NPO.ShowGridProjection = value;
		}
	}

	// Token: 0x17000327 RID: 807
	// (get) Token: 0x0600121D RID: 4637 RVA: 0x00079DFB File Offset: 0x00077FFB
	// (set) Token: 0x0600121E RID: 4638 RVA: 0x00079E08 File Offset: 0x00078008
	public bool use_hands
	{
		get
		{
			return this.NPO.CanBeHeldInHand;
		}
		set
		{
			this.NPO.CanBeHeldInHand = value;
		}
	}

	// Token: 0x17000328 RID: 808
	// (get) Token: 0x0600121F RID: 4639 RVA: 0x00079E16 File Offset: 0x00078016
	// (set) Token: 0x06001220 RID: 4640 RVA: 0x00079E23 File Offset: 0x00078023
	public int max_typed_number
	{
		get
		{
			return this.NPO.ManualMaxTypedNumber;
		}
		set
		{
			this.NPO.ManualMaxTypedNumber = value;
		}
	}

	// Token: 0x17000329 RID: 809
	// (get) Token: 0x06001221 RID: 4641 RVA: 0x00079E31 File Offset: 0x00078031
	// (set) Token: 0x06001222 RID: 4642 RVA: 0x00079E3E File Offset: 0x0007803E
	public int value
	{
		get
		{
			return this.NPO.Value;
		}
		set
		{
			this.NPO.Value = value;
		}
	}

	// Token: 0x1700032A RID: 810
	// (get) Token: 0x06001223 RID: 4643 RVA: 0x00079E4C File Offset: 0x0007804C
	// (set) Token: 0x06001224 RID: 4644 RVA: 0x00079E59 File Offset: 0x00078059
	public int value_flags
	{
		get
		{
			return this.NPO.ValueFlags;
		}
		set
		{
			this.NPO.ValueFlags = value;
		}
	}

	// Token: 0x1700032B RID: 811
	// (get) Token: 0x06001225 RID: 4645 RVA: 0x00079E67 File Offset: 0x00078067
	// (set) Token: 0x06001226 RID: 4646 RVA: 0x00079E74 File Offset: 0x00078074
	public bool drag_selectable
	{
		get
		{
			return this.NPO.IsDragSelectable;
		}
		set
		{
			this.NPO.IsDragSelectable = value;
		}
	}

	// Token: 0x1700032C RID: 812
	// (get) Token: 0x06001227 RID: 4647 RVA: 0x00079E82 File Offset: 0x00078082
	// (set) Token: 0x06001228 RID: 4648 RVA: 0x00079E8F File Offset: 0x0007808F
	public bool gizmo_selectable
	{
		get
		{
			return this.NPO.IsGizmoSelectable;
		}
		set
		{
			this.NPO.IsGizmoSelectable = value;
		}
	}

	// Token: 0x1700032D RID: 813
	// (get) Token: 0x06001229 RID: 4649 RVA: 0x00079E9D File Offset: 0x0007809D
	// (set) Token: 0x0600122A RID: 4650 RVA: 0x00079EAA File Offset: 0x000780AA
	public bool measure_movement
	{
		get
		{
			return this.NPO.ShowRulerWhenHeld;
		}
		set
		{
			this.NPO.ShowRulerWhenHeld = value;
		}
	}

	// Token: 0x1700032E RID: 814
	// (get) Token: 0x0600122B RID: 4651 RVA: 0x00079EB8 File Offset: 0x000780B8
	// (set) Token: 0x0600122C RID: 4652 RVA: 0x00079EC5 File Offset: 0x000780C5
	public string memo
	{
		get
		{
			return this.NPO.Memo;
		}
		set
		{
			this.NPO.Memo = value;
		}
	}

	// Token: 0x1700032F RID: 815
	// (get) Token: 0x0600122D RID: 4653 RVA: 0x00079ED3 File Offset: 0x000780D3
	// (set) Token: 0x0600122E RID: 4654 RVA: 0x00079EDB File Offset: 0x000780DB
	public bool spawning { get; private set; } = true;

	// Token: 0x0600122F RID: 4655 RVA: 0x00079EE4 File Offset: 0x000780E4
	[MoonSharpHidden]
	public void Spawned()
	{
		this.spawning = false;
	}

	// Token: 0x17000330 RID: 816
	// (get) Token: 0x06001230 RID: 4656 RVA: 0x00079EED File Offset: 0x000780ED
	public bool loading_custom
	{
		get
		{
			return this.spawning || this.NPO.IsLoadingCustom();
		}
	}

	// Token: 0x17000331 RID: 817
	// (get) Token: 0x06001231 RID: 4657 RVA: 0x00079F04 File Offset: 0x00078104
	// (set) Token: 0x06001232 RID: 4658 RVA: 0x00079F11 File Offset: 0x00078111
	public Vector3 alt_view_angle
	{
		get
		{
			return this.NPO.AltLookAngle;
		}
		set
		{
			this.NPO.AltLookAngle = value;
		}
	}

	// Token: 0x17000332 RID: 818
	// (get) Token: 0x06001233 RID: 4659 RVA: 0x00079F1F File Offset: 0x0007811F
	public LuaGameObjectScript remainder
	{
		get
		{
			return this.Remainder();
		}
	}

	// Token: 0x17000333 RID: 819
	// (get) Token: 0x06001234 RID: 4660 RVA: 0x00079F27 File Offset: 0x00078127
	// (set) Token: 0x06001235 RID: 4661 RVA: 0x00079F2F File Offset: 0x0007812F
	public LuaUI UI { get; private set; }

	// Token: 0x17000334 RID: 820
	// (get) Token: 0x06001236 RID: 4662 RVA: 0x00079F38 File Offset: 0x00078138
	public LuaScriptWrapper script
	{
		get
		{
			LuaScriptWrapper result;
			if ((result = this._script) == null)
			{
				result = (this._script = new LuaScriptWrapper(this));
			}
			return result;
		}
	}

	// Token: 0x06001237 RID: 4663 RVA: 0x00079F5E File Offset: 0x0007815E
	[MoonSharpHidden]
	protected override void Awake()
	{
		base.Awake();
		this.NPO = base.GetComponent<NetworkPhysicsObject>();
		base.XmlUI = base.GetComponent<XmlUIScript>();
		this.UI = new LuaUI(base.XmlUI);
	}

	// Token: 0x06001238 RID: 4664 RVA: 0x00079F90 File Offset: 0x00078190
	[MoonSharpHidden]
	protected override void Start()
	{
		base.Start();
		if (this.IsGlobalDummyObject || Network.isClient)
		{
			return;
		}
		this.CheckLua();
		this.lua.Options.DebugPrint = delegate(string s)
		{
			Chat.Log(s, Colour.White, ChatMessageType.All, false);
			LuaGlobalScriptManager.Instance.PushLuaPrintMessage(s);
		};
		this.lua.Globals["self"] = this;
		LuaGlobalScriptManager.Instance.AddFunctions(this.lua);
		try
		{
			if (!string.IsNullOrEmpty(this.script_code) && ConfigGame.Settings.Scripting)
			{
				if (NetworkSingleton<NetworkUI>.Instance.bAutoRunScripts || LuaGlobalScriptManager.Instance.autoLoadOnce)
				{
					base.DoString();
				}
				else
				{
					NetworkSingleton<NetworkUI>.Instance.GUIStartScripts.AddScript(this);
				}
			}
		}
		catch (Exception e)
		{
			base.LogError(e);
		}
		if (LuaGlobalScriptManager.Instance.loaded)
		{
			this.OnLoad();
		}
		this.CheckCollisionEvents();
		NetworkEvents.OnPlayerConnected += this.OnPlayerConnect;
	}

	// Token: 0x06001239 RID: 4665 RVA: 0x0007A09C File Offset: 0x0007829C
	[MoonSharpHidden]
	protected override void OnDestroy()
	{
		base.OnDestroy();
		if (this.IsGlobalDummyObject || Network.isClient)
		{
			return;
		}
		NetworkEvents.OnPlayerConnected -= this.OnPlayerConnect;
	}

	// Token: 0x0600123A RID: 4666 RVA: 0x0007A0C8 File Offset: 0x000782C8
	[MoonSharpHidden]
	public void OnPlayerConnect(NetworkPlayer NP)
	{
		if (Network.isClient)
		{
			return;
		}
		if (this.IsGlobalDummyObject)
		{
			return;
		}
		for (int i = 0; i < this.luaButtonsHolder.Count; i++)
		{
			base.networkView.RPC<LuaUIButtonState>(NP, new Action<LuaUIButtonState>(this.RPCCreateButton), this.luaButtonsHolder[i].luaButtonState);
		}
		for (int j = 0; j < this.luaInputsHolder.Count; j++)
		{
			this.luaInputsHolder[j].luaInputState.value = this.luaInputsHolder[j].input.value;
			base.networkView.RPC<LuaUIInputState>(NP, new Action<LuaUIInputState>(this.RPCCreateInput), this.luaInputsHolder[j].luaInputState);
		}
		foreach (KeyValuePair<LuaGameObjectScript.ComponentVariable, object> keyValuePair in this.ComponentValues)
		{
			Type varType = keyValuePair.Key.varInfo.VarType;
			LuaGameObjectScript.RPCComponentVariable arg = new LuaGameObjectScript.RPCComponentVariable(this.sourceGetComponentGameObject, keyValuePair.Key.component, keyValuePair.Key.varInfo);
			object value = keyValuePair.Value;
			if (varType == typeof(float) || varType == typeof(int))
			{
				base.networkView.RPC<LuaGameObjectScript.RPCComponentVariable, float>(NP, new Action<LuaGameObjectScript.RPCComponentVariable, float>(this.RPCSetComponentVar), arg, (float)value);
			}
			else if (varType == typeof(bool))
			{
				base.networkView.RPC<LuaGameObjectScript.RPCComponentVariable, bool>(NP, new Action<LuaGameObjectScript.RPCComponentVariable, bool>(this.RPCSetComponentVar), arg, (bool)value);
			}
			else if (varType == typeof(Vector3))
			{
				base.networkView.RPC<LuaGameObjectScript.RPCComponentVariable, Vector3>(NP, new Action<LuaGameObjectScript.RPCComponentVariable, Vector3>(this.RPCSetComponentVar), arg, (Vector3)value);
			}
			else if (varType == typeof(Color))
			{
				base.networkView.RPC<LuaGameObjectScript.RPCComponentVariable, Color>(NP, new Action<LuaGameObjectScript.RPCComponentVariable, Color>(this.RPCSetComponentVar), arg, (Color)value);
			}
		}
		foreach (KeyValuePair<LuaGameObjectScript.MaterialVariable, object> keyValuePair2 in this.MaterialValues)
		{
			LuaGameObjectScript.RPCMaterialVariable arg2 = new LuaGameObjectScript.RPCMaterialVariable(this.NPO.Renderers, keyValuePair2.Key.material, keyValuePair2.Key.varName);
			object value2 = keyValuePair2.Value;
			Type type = value2.GetType();
			if (type == typeof(float))
			{
				base.networkView.RPC<LuaGameObjectScript.RPCMaterialVariable, float>(NP, new Action<LuaGameObjectScript.RPCMaterialVariable, float>(this.RPCSetMaterialVar), arg2, (float)value2);
			}
			else if (type == typeof(Vector4))
			{
				base.networkView.RPC<LuaGameObjectScript.RPCMaterialVariable, Vector4>(NP, new Action<LuaGameObjectScript.RPCMaterialVariable, Vector4>(this.RPCSetMaterialVar), arg2, (Vector4)value2);
			}
			else if (type == typeof(Color))
			{
				base.networkView.RPC<LuaGameObjectScript.RPCMaterialVariable, Color>(NP, new Action<LuaGameObjectScript.RPCMaterialVariable, Color>(this.RPCSetMaterialVar), arg2, (Color)value2);
			}
		}
	}

	// Token: 0x0600123B RID: 4667 RVA: 0x0007A43C File Offset: 0x0007863C
	[MoonSharpHidden]
	public void CheckLua()
	{
		if (this.IsGlobalDummyObject)
		{
			return;
		}
		if (this.lua == null)
		{
			this.lua = new Script(CoreModules.Preset_SoftSandbox);
		}
	}

	// Token: 0x0600123C RID: 4668 RVA: 0x0007A45F File Offset: 0x0007865F
	public override string GetScriptName()
	{
		if (!this.IsGlobalDummyObject)
		{
			return TTSUtilities.CleanName(this.NPO) + " - " + this.NPO.GUID;
		}
		return base.GetScriptName();
	}

	// Token: 0x0600123D RID: 4669 RVA: 0x0007A490 File Offset: 0x00078690
	[MoonSharpHidden]
	public override void OnObjectSpawn(LuaGameObjectScript LGOS)
	{
		base.OnObjectSpawn(LGOS);
		if (LGOS != this)
		{
			return;
		}
		if (base.CanCall("onSpawn", false))
		{
			base.TryCall("onSpawn");
		}
	}

	// Token: 0x0600123E RID: 4670 RVA: 0x0007A4BD File Offset: 0x000786BD
	[MoonSharpHidden]
	public override void OnObjectDestroy(LuaGameObjectScript LGOS)
	{
		base.OnObjectDestroy(LGOS);
		if (LGOS != this)
		{
			return;
		}
		if (base.CanCall("onDestroy", false))
		{
			base.TryCall("onDestroy");
		}
	}

	// Token: 0x0600123F RID: 4671 RVA: 0x0007A4EC File Offset: 0x000786EC
	[MoonSharpHidden]
	public override void OnObjectHover(GameObject HoverObject, Color Player)
	{
		base.OnObjectHover(HoverObject, Player);
		if (HoverObject != base.gameObject)
		{
			return;
		}
		if (base.CanCall("onHover", false))
		{
			base.TryCall("onHover", new object[]
			{
				Colour.LabelFromColour(Player)
			});
		}
	}

	// Token: 0x06001240 RID: 4672 RVA: 0x0007A540 File Offset: 0x00078740
	[MoonSharpHidden]
	public override void OnObjectPickUp(NetworkPhysicsObject PickedUpObject, PlayerState Player)
	{
		base.OnObjectPickUp(PickedUpObject, Player);
		if (PickedUpObject != this.NPO)
		{
			return;
		}
		if (base.CanCall("onPickUp", false))
		{
			base.TryCall("onPickUp", new object[]
			{
				Player.stringColor
			});
		}
		if (base.CanCall("onPickedUp", false))
		{
			base.TryCall("onPickedUp", new object[]
			{
				Player.stringColor
			});
		}
	}

	// Token: 0x06001241 RID: 4673 RVA: 0x0007A5B8 File Offset: 0x000787B8
	[MoonSharpHidden]
	public override void OnObjectDrop(NetworkPhysicsObject DropObject, PlayerState LastPlayerToHold)
	{
		base.OnObjectDrop(DropObject, LastPlayerToHold);
		if (DropObject != this.NPO)
		{
			return;
		}
		if (base.CanCall("onDrop", false))
		{
			base.TryCall("onDrop", new object[]
			{
				LastPlayerToHold.stringColor
			});
		}
		if (base.CanCall("onDropped", false))
		{
			base.TryCall("onDropped", new object[]
			{
				LastPlayerToHold.stringColor
			});
		}
	}

	// Token: 0x06001242 RID: 4674 RVA: 0x0007A630 File Offset: 0x00078830
	[MoonSharpHidden]
	public override void OnObjectSearchStart(NetworkPhysicsObject npo, string playerColor)
	{
		base.OnObjectSearchStart(npo, playerColor);
		if (npo.gameObject != base.gameObject)
		{
			return;
		}
		if (base.CanCall("onSearchStart", false))
		{
			base.TryCall("onSearchStart", new object[]
			{
				playerColor
			});
		}
	}

	// Token: 0x06001243 RID: 4675 RVA: 0x0007A680 File Offset: 0x00078880
	[MoonSharpHidden]
	public override void OnObjectSearchEnd(NetworkPhysicsObject npo, string playerColor)
	{
		base.OnObjectSearchEnd(npo, playerColor);
		if (npo.gameObject != base.gameObject)
		{
			return;
		}
		if (base.CanCall("onSearchEnd", false))
		{
			base.TryCall("onSearchEnd", new object[]
			{
				playerColor
			});
		}
	}

	// Token: 0x06001244 RID: 4676 RVA: 0x0007A6D0 File Offset: 0x000788D0
	[MoonSharpHidden]
	public override void OnObjectPeek(NetworkPhysicsObject npo, string playerColor)
	{
		base.OnObjectPeek(npo, playerColor);
		if (npo.gameObject != base.gameObject)
		{
			return;
		}
		if (base.CanCall("onPeek", false))
		{
			base.TryCall("onPeek", new object[]
			{
				playerColor
			});
		}
	}

	// Token: 0x06001245 RID: 4677 RVA: 0x0007A720 File Offset: 0x00078920
	[MoonSharpHidden]
	public override void OnObjectRandomize(NetworkPhysicsObject npo, string playerColor)
	{
		base.OnObjectRandomize(npo, playerColor);
		if (npo.gameObject != base.gameObject)
		{
			return;
		}
		if (base.CanCall("onRandomize", false))
		{
			base.TryCall("onRandomize", new object[]
			{
				playerColor
			});
		}
	}

	// Token: 0x06001246 RID: 4678 RVA: 0x0007A770 File Offset: 0x00078970
	[MoonSharpHidden]
	public override void OnObjectRotate(NetworkPhysicsObject npo, int spinIndex, int flipIndex, string playerColor, int previousSpinIndex, int previousFlipIndex)
	{
		base.OnObjectRotate(npo, spinIndex, flipIndex, playerColor, previousSpinIndex, previousFlipIndex);
		if (npo.gameObject != base.gameObject)
		{
			return;
		}
		if (base.CanCall("onRotate", false))
		{
			base.TryCall("onRotate", new object[]
			{
				spinIndex * 15,
				flipIndex * 15,
				playerColor,
				previousSpinIndex * 15,
				previousFlipIndex * 15
			});
		}
	}

	// Token: 0x06001247 RID: 4679 RVA: 0x0007A7F8 File Offset: 0x000789F8
	[MoonSharpHidden]
	public override void OnObjectFlick(NetworkPhysicsObject npo, string playerColor, Vector3 force)
	{
		base.OnObjectFlick(npo, playerColor, force);
		if (npo.gameObject != base.gameObject)
		{
			return;
		}
		if (base.CanCall("onFlick", false))
		{
			base.TryCall("onFlick", new object[]
			{
				playerColor,
				force
			});
		}
	}

	// Token: 0x06001248 RID: 4680 RVA: 0x0007A84F File Offset: 0x00078A4F
	[MoonSharpHidden]
	public override void OnObjectPageChange(NetworkPhysicsObject npo)
	{
		base.OnObjectPageChange(npo);
		if (npo.gameObject != base.gameObject)
		{
			return;
		}
		if (base.CanCall("onPageChange", false))
		{
			base.TryCall("onPageChange");
		}
	}

	// Token: 0x06001249 RID: 4681 RVA: 0x0007A888 File Offset: 0x00078A88
	private bool TryCallbacks(LuaScript script, string[] callbackNames, params object[] args)
	{
		foreach (string functionName in callbackNames)
		{
			if (script.CanCall(functionName, true))
			{
				DynValue dynValue = script.Call(functionName, args);
				if (dynValue != null && (dynValue.IsNilOrNan() || !dynValue.Boolean))
				{
					return false;
				}
			}
		}
		return true;
	}

	// Token: 0x0600124A RID: 4682 RVA: 0x0007A8D4 File Offset: 0x00078AD4
	[MoonSharpHidden]
	public bool CheckObjectEnter(NetworkPhysicsObject npo)
	{
		return this.TryCallbacks(LuaGlobalScriptManager.Instance, LuaGameObjectScript.checkObjectEnterGlobalCallbacks, new object[]
		{
			this,
			npo.luaGameObjectScript
		}) && this.TryCallbacks(this, LuaGameObjectScript.checkObjectEnterCallbacks, new object[]
		{
			npo.luaGameObjectScript
		});
	}

	// Token: 0x0600124B RID: 4683 RVA: 0x0007A924 File Offset: 0x00078B24
	[MoonSharpHidden]
	public bool CheckObjectRotate(int spinIndex, int flipIndex, string playerColor, int previousSpinIndex, int previousFlipIndex)
	{
		return this.TryCallbacks(LuaGlobalScriptManager.Instance, LuaGameObjectScript.checkObjectRotateGlobalNames, new object[]
		{
			this,
			spinIndex * 15,
			flipIndex * 15,
			playerColor,
			previousSpinIndex * 15,
			previousFlipIndex * 15
		}) && this.TryCallbacks(this, LuaGameObjectScript.checkObjectRotateNames, new object[]
		{
			spinIndex * 15,
			flipIndex * 15,
			playerColor,
			previousSpinIndex * 15,
			previousFlipIndex * 15
		});
	}

	// Token: 0x17000335 RID: 821
	// (get) Token: 0x0600124C RID: 4684 RVA: 0x0007A9CD File Offset: 0x00078BCD
	// (set) Token: 0x0600124D RID: 4685 RVA: 0x0007A9D5 File Offset: 0x00078BD5
	[MoonSharpHidden]
	public LuaCollision luaCollision { get; private set; }

	// Token: 0x0600124E RID: 4686 RVA: 0x0007A9E0 File Offset: 0x00078BE0
	[MoonSharpHidden]
	public void CheckCollisionEvents()
	{
		if (this.lua == null || this.IsGlobalDummyObject)
		{
			return;
		}
		this.luaCollision = base.gameObject.GetComponent<LuaCollision>();
		if (this.lua.Globals["onCollisionEnter"] != null && this.luaCollision == null)
		{
			this.luaCollision = base.gameObject.AddComponent<LuaCollision>();
			this.luaCollision.lua = this.lua;
		}
		if (this.lua.Globals["onCollisionStay"] != null)
		{
			if (this.luaCollision == null)
			{
				this.luaCollision = base.gameObject.AddComponent<LuaCollision>();
				this.luaCollision.lua = this.lua;
			}
			this.luaCollision.bOnCollisionStay = true;
		}
		if (this.lua.Globals["onCollisionExit"] != null && this.luaCollision == null)
		{
			this.luaCollision = base.gameObject.AddComponent<LuaCollision>();
			this.luaCollision.lua = this.lua;
		}
		if (this.luaCollision != null && this.lua.Globals["onCollisionEnter"] == null && this.lua.Globals["onCollisionStay"] == null && this.lua.Globals["onCollisionExit"] == null)
		{
			UnityEngine.Object.Destroy(this.luaCollision);
		}
	}

	// Token: 0x0600124F RID: 4687 RVA: 0x0007AB4E File Offset: 0x00078D4E
	public string GetGUID()
	{
		return this.guid;
	}

	// Token: 0x06001250 RID: 4688 RVA: 0x0007AB56 File Offset: 0x00078D56
	public string GetName()
	{
		return this.NPO.Name;
	}

	// Token: 0x06001251 RID: 4689 RVA: 0x0007AB63 File Offset: 0x00078D63
	public bool SetName(string Name)
	{
		this.NPO.Name = Name;
		return true;
	}

	// Token: 0x06001252 RID: 4690 RVA: 0x0007AB72 File Offset: 0x00078D72
	public string GetDescription()
	{
		return this.NPO.Description;
	}

	// Token: 0x06001253 RID: 4691 RVA: 0x0007AB7F File Offset: 0x00078D7F
	public bool SetDescription(string Description)
	{
		this.NPO.Description = Description;
		return true;
	}

	// Token: 0x06001254 RID: 4692 RVA: 0x0007AB8E File Offset: 0x00078D8E
	public string GetGMNotes()
	{
		return this.NPO.GMNotes;
	}

	// Token: 0x06001255 RID: 4693 RVA: 0x0007AB9B File Offset: 0x00078D9B
	public bool SetGMNotes(string Notes)
	{
		this.NPO.GMNotes = Notes;
		return true;
	}

	// Token: 0x06001256 RID: 4694 RVA: 0x00079EB8 File Offset: 0x000780B8
	public string GetMemo()
	{
		return this.NPO.Memo;
	}

	// Token: 0x06001257 RID: 4695 RVA: 0x0007ABAA File Offset: 0x00078DAA
	public bool SetMemo(string Memo)
	{
		this.NPO.Memo = Memo;
		return true;
	}

	// Token: 0x06001258 RID: 4696 RVA: 0x0007ABB9 File Offset: 0x00078DB9
	public string GetLuaScript()
	{
		return this.script_code;
	}

	// Token: 0x06001259 RID: 4697 RVA: 0x0007ABC1 File Offset: 0x00078DC1
	public bool SetLuaScript(string Script)
	{
		base.enabled = true;
		this.script_code = Script;
		return true;
	}

	// Token: 0x0600125A RID: 4698 RVA: 0x0007ABD2 File Offset: 0x00078DD2
	public bool StartLuaCoroutine(string LuaFunction)
	{
		return LuaGlobalScriptManager.Instance.StartLuaCoroutine(this, LuaFunction, this.lua);
	}

	// Token: 0x0600125B RID: 4699 RVA: 0x0007ABE6 File Offset: 0x00078DE6
	public DynValue Call(Script script, string LuaFunction, Table Parameters)
	{
		return LuaGlobalScriptManager.Instance.CallLuaFunctionInOtherScript(script, this, LuaFunction, Parameters);
	}

	// Token: 0x0600125C RID: 4700 RVA: 0x0007ABF6 File Offset: 0x00078DF6
	[Obsolete("Use Call() instead.")]
	public DynValue CallLuaFunctionInOtherScript(Script script, string LuaFunction)
	{
		return LuaGlobalScriptManager.Instance.CallLuaFunctionInOtherScript(script, this, LuaFunction);
	}

	// Token: 0x0600125D RID: 4701 RVA: 0x0007ABE6 File Offset: 0x00078DE6
	[Obsolete("Use Call() instead.")]
	public DynValue CallLuaFunctionInOtherScript(Script script, string LuaFunction, Table param)
	{
		return LuaGlobalScriptManager.Instance.CallLuaFunctionInOtherScript(script, this, LuaFunction, param);
	}

	// Token: 0x0600125E RID: 4702 RVA: 0x0007AC05 File Offset: 0x00078E05
	public object Get(string name)
	{
		this.CheckLua();
		return this.lua.Globals[name];
	}

	// Token: 0x0600125F RID: 4703 RVA: 0x0007AC1E File Offset: 0x00078E1E
	public bool Set(string name, DynValue value)
	{
		this.CheckLua();
		this.lua.Globals[name] = value;
		return true;
	}

	// Token: 0x06001260 RID: 4704 RVA: 0x0007AC39 File Offset: 0x00078E39
	public Table GetTable(Script script, string TableName)
	{
		this.CheckLua();
		return LuaGlobalScriptManager.CopyLuaTable((Table)this.lua.Globals[TableName], script);
	}

	// Token: 0x06001261 RID: 4705 RVA: 0x0007AC60 File Offset: 0x00078E60
	public bool SetTable(string TableName, Table SourceTable)
	{
		this.CheckLua();
		if (this == null)
		{
			return false;
		}
		Table value = LuaGlobalScriptManager.CopyLuaTable(SourceTable, this.lua);
		this.lua.Globals[TableName] = value;
		return true;
	}

	// Token: 0x06001262 RID: 4706 RVA: 0x0007AC05 File Offset: 0x00078E05
	public object GetVar(string VarName)
	{
		this.CheckLua();
		return this.lua.Globals[VarName];
	}

	// Token: 0x06001263 RID: 4707 RVA: 0x0007AC1E File Offset: 0x00078E1E
	public bool SetVar(string VarName, DynValue Value)
	{
		this.CheckLua();
		this.lua.Globals[VarName] = Value;
		return true;
	}

	// Token: 0x06001264 RID: 4708 RVA: 0x0007AC9E File Offset: 0x00078E9E
	public bool ClearContextMenu()
	{
		this.NPO.ClearCustomContextMenu();
		return true;
	}

	// Token: 0x06001265 RID: 4709 RVA: 0x0007ACAC File Offset: 0x00078EAC
	public bool AddContextMenuItem(string label, Closure function, bool keepOpen = false)
	{
		int num = NetworkSingleton<UserDefinedContextualManager>.Instance.AddObjectItem(label, function, keepOpen);
		if (num == -1)
		{
			LuaGlobalScriptManager.Instance.LogError("addContextMenuItem", string.Concat(new string[]
			{
				"Function required for context menu (label = \"",
				label,
				"\", object = ",
				this.GetScriptName(),
				")"
			}), null);
			return false;
		}
		this.NPO.AddCustomContextMenu(num);
		return true;
	}

	// Token: 0x06001266 RID: 4710 RVA: 0x0007AD1A File Offset: 0x00078F1A
	public bool HighlightOn(Color color)
	{
		this.NPO.LuaHighlightOn(color);
		return true;
	}

	// Token: 0x06001267 RID: 4711 RVA: 0x0007AD29 File Offset: 0x00078F29
	public bool HighlightOn(Color color, float duration)
	{
		this.NPO.LuaHighlightOn(color, duration);
		return true;
	}

	// Token: 0x06001268 RID: 4712 RVA: 0x0007AD39 File Offset: 0x00078F39
	public bool HighlightOff()
	{
		this.NPO.LuaHighlightOff();
		return true;
	}

	// Token: 0x06001269 RID: 4713 RVA: 0x0007AD47 File Offset: 0x00078F47
	public Color? GetHighlightColor()
	{
		return this.NPO.LuaHighlightColor;
	}

	// Token: 0x0600126A RID: 4714 RVA: 0x0007AD54 File Offset: 0x00078F54
	public bool IsDestroyed()
	{
		return this.NPO.IsDestroyed;
	}

	// Token: 0x0600126B RID: 4715 RVA: 0x0007AD61 File Offset: 0x00078F61
	public bool IsSmoothMoving()
	{
		return this.NPO.IsSmoothMoving;
	}

	// Token: 0x0600126C RID: 4716 RVA: 0x0007AD6E File Offset: 0x00078F6E
	public Vector3 GetTransformForward()
	{
		return base.transform.forward;
	}

	// Token: 0x0600126D RID: 4717 RVA: 0x0007AD7B File Offset: 0x00078F7B
	public Vector3 GetTransformRight()
	{
		return base.transform.right;
	}

	// Token: 0x0600126E RID: 4718 RVA: 0x0007AD88 File Offset: 0x00078F88
	public Vector3 GetTransformUp()
	{
		return base.transform.up;
	}

	// Token: 0x0600126F RID: 4719 RVA: 0x0007AD95 File Offset: 0x00078F95
	public Vector3 PositionToLocal(Vector3 pos)
	{
		return base.transform.InverseTransformPoint(pos);
	}

	// Token: 0x06001270 RID: 4720 RVA: 0x0007ADA3 File Offset: 0x00078FA3
	public Vector3 PositionToWorld(Vector3 pos)
	{
		return base.transform.TransformPoint(pos);
	}

	// Token: 0x06001271 RID: 4721 RVA: 0x0007ADB1 File Offset: 0x00078FB1
	public Vector3 GetPosition()
	{
		return base.transform.position;
	}

	// Token: 0x06001272 RID: 4722 RVA: 0x0007ADBE File Offset: 0x00078FBE
	public bool SetPosition(Vector3 Position)
	{
		this.NPO.ResetCardJoint();
		this.NPO.StopSmoothPosition(false);
		base.transform.position = Position;
		return true;
	}

	// Token: 0x06001273 RID: 4723 RVA: 0x0007ADE4 File Offset: 0x00078FE4
	public Vector3? GetPositionSmooth()
	{
		return this.NPO.GetSmoothPosition();
	}

	// Token: 0x06001274 RID: 4724 RVA: 0x0007ADF4 File Offset: 0x00078FF4
	public bool SetPositionSmooth(Vector3 Position, bool Collide = false, bool Fast = false)
	{
		this.NPO.ResetCardJoint();
		this.NPO.SetSmoothPosition(Position, Collide, Fast, false, true, null, this.NPO.IsLocked, false, null);
		return true;
	}

	// Token: 0x06001275 RID: 4725 RVA: 0x0007AE34 File Offset: 0x00079034
	public bool Translate(Vector3 Position)
	{
		this.NPO.ResetCardJoint();
		this.NPO.SetSmoothPosition(new Vector3(base.transform.position.x + Position.x, base.transform.position.y + Position.y, base.transform.position.z + Position.z), true, false, false, true, null, this.NPO.IsLocked, false, null);
		return true;
	}

	// Token: 0x06001276 RID: 4726 RVA: 0x0007AEBC File Offset: 0x000790BC
	public Vector3 GetRotation()
	{
		return base.transform.eulerAngles;
	}

	// Token: 0x06001277 RID: 4727 RVA: 0x0007AEC9 File Offset: 0x000790C9
	public bool SetRotation(Vector3 Rotation)
	{
		this.NPO.ResetCardJoint();
		this.NPO.StopSmoothRotation(false);
		base.transform.rotation = Quaternion.Euler(Rotation);
		return true;
	}

	// Token: 0x06001278 RID: 4728 RVA: 0x0007AEF4 File Offset: 0x000790F4
	public Vector3? GetRotationSmooth()
	{
		return this.NPO.GetSmoothRotation();
	}

	// Token: 0x06001279 RID: 4729 RVA: 0x0007AF04 File Offset: 0x00079104
	public bool SetRotationSmooth(Vector3 Rotation, bool Collide = false, bool Fast = false)
	{
		this.NPO.ResetCardJoint();
		this.NPO.SetSmoothRotation(Rotation, Collide, Fast, false, true, null, this.NPO.IsLocked);
		return true;
	}

	// Token: 0x0600127A RID: 4730 RVA: 0x0007AF44 File Offset: 0x00079144
	public bool Rotate(Vector3 Rotation)
	{
		this.NPO.ResetCardJoint();
		this.NPO.SetSmoothRotation(new Vector3(base.transform.eulerAngles.x + Rotation.x, base.transform.eulerAngles.y + Rotation.y, base.transform.eulerAngles.z + Rotation.z), true, false, false, true, null, this.NPO.IsLocked);
		return true;
	}

	// Token: 0x0600127B RID: 4731 RVA: 0x0007AFCC File Offset: 0x000791CC
	public bool SendToHand(string playerColorLabel, bool face_up = true, bool fast = false)
	{
		HandZone handZone = HandZone.GetHandZone(playerColorLabel, 0, true);
		return this.SendToHand(handZone, face_up, fast);
	}

	// Token: 0x0600127C RID: 4732 RVA: 0x0007AFEB File Offset: 0x000791EB
	public bool SendToHand(HandZone handZone, bool face_up = true, bool fast = false)
	{
		if (!handZone)
		{
			return false;
		}
		handZone.DealToEnd(this.NPO, face_up, fast);
		return true;
	}

	// Token: 0x0600127D RID: 4733 RVA: 0x0007B008 File Offset: 0x00079208
	public bool MoveToHandStash()
	{
		HandZone currentPlayerHand = this.NPO.CurrentPlayerHand;
		if (!currentPlayerHand || currentPlayerHand.GetHandIndex(true) != 0)
		{
			return false;
		}
		currentPlayerHand.MoveToStash(this.NPO);
		return true;
	}

	// Token: 0x0600127E RID: 4734 RVA: 0x0007B041 File Offset: 0x00079241
	public Vector3 GetScale()
	{
		return this.NPO.Scale;
	}

	// Token: 0x0600127F RID: 4735 RVA: 0x0007B04E File Offset: 0x0007924E
	public bool SetScale(Vector3 Scale)
	{
		this.NPO.SetScale(Scale, true);
		return true;
	}

	// Token: 0x06001280 RID: 4736 RVA: 0x0007B05E File Offset: 0x0007925E
	public bool Scale(float scale)
	{
		return this.Scale(Vector3.one * scale);
	}

	// Token: 0x06001281 RID: 4737 RVA: 0x0007B074 File Offset: 0x00079274
	public bool Scale(Vector3 Scale)
	{
		this.NPO.SetScale(new Vector3(this.NPO.Scale.x * Scale.x, this.NPO.Scale.y * Scale.y, this.NPO.Scale.z * Scale.z), true);
		return true;
	}

	// Token: 0x06001282 RID: 4738 RVA: 0x0007B0D8 File Offset: 0x000792D8
	public Vector3 GetNearestPointFromObject(LuaGameObjectScript other)
	{
		NetworkPhysicsObject component = other.GetComponent<NetworkPhysicsObject>();
		Vector3 vector = other.transform.position;
		Vector3 vector2 = LibVector.nearestPointOnNPO(vector, this.NPO);
		if (vector2 != Vector3.zero)
		{
			vector = LibVector.nearestPointOnNPO(vector2, component);
			if (vector != Vector3.zero)
			{
				vector2 = LibVector.nearestPointOnNPO(vector, this.NPO);
			}
		}
		return vector2;
	}

	// Token: 0x06001283 RID: 4739 RVA: 0x0007B135 File Offset: 0x00079335
	public Vector3 GetNearestPointFromPosition(Vector3 position)
	{
		return LibVector.nearestPointOnNPO(position, this.NPO);
	}

	// Token: 0x06001284 RID: 4740 RVA: 0x0007B143 File Offset: 0x00079343
	public bool Flip()
	{
		NetworkSingleton<ManagerPhysicsObject>.Instance.SetObjectRotation(this.NPO, 0, 12, -1);
		return true;
	}

	// Token: 0x06001285 RID: 4741 RVA: 0x0007B15C File Offset: 0x0007935C
	public bool AddForce(Vector3 ForceVector, int ForceType = 3)
	{
		ForceMode mode;
		switch (ForceType)
		{
		case 1:
			mode = ForceMode.Force;
			break;
		case 2:
			mode = ForceMode.Acceleration;
			break;
		case 3:
			mode = ForceMode.Impulse;
			break;
		case 4:
			mode = ForceMode.VelocityChange;
			break;
		default:
			mode = ForceMode.Impulse;
			break;
		}
		this.NPO.rigidbody.AddForce(ForceVector, mode);
		return true;
	}

	// Token: 0x06001286 RID: 4742 RVA: 0x0007B1AC File Offset: 0x000793AC
	public bool AddTorque(Vector3 TorqueVector, int ForceType = 3)
	{
		ForceMode mode;
		switch (ForceType)
		{
		case 1:
			mode = ForceMode.Force;
			break;
		case 2:
			mode = ForceMode.Acceleration;
			break;
		case 3:
			mode = ForceMode.Impulse;
			break;
		case 4:
			mode = ForceMode.VelocityChange;
			break;
		default:
			mode = ForceMode.Impulse;
			break;
		}
		this.NPO.rigidbody.AddTorque(TorqueVector, mode);
		return true;
	}

	// Token: 0x06001287 RID: 4743 RVA: 0x0007B1FA File Offset: 0x000793FA
	public Vector3 GetVelocity()
	{
		return this.NPO.rigidbody.velocity;
	}

	// Token: 0x06001288 RID: 4744 RVA: 0x0007B20C File Offset: 0x0007940C
	public bool SetVelocity(Vector3 VelocityVector)
	{
		this.NPO.rigidbody.velocity = VelocityVector;
		return true;
	}

	// Token: 0x06001289 RID: 4745 RVA: 0x0007B220 File Offset: 0x00079420
	public Vector3 GetAngularVelocity()
	{
		return this.NPO.rigidbody.angularVelocity;
	}

	// Token: 0x0600128A RID: 4746 RVA: 0x0007B232 File Offset: 0x00079432
	public bool SetAngularVelocity(Vector3 AngularVelocityVector)
	{
		this.NPO.rigidbody.angularVelocity = AngularVelocityVector;
		return true;
	}

	// Token: 0x0600128B RID: 4747 RVA: 0x0007B246 File Offset: 0x00079446
	public Color GetColorTint()
	{
		return this.NPO.DiffuseColor;
	}

	// Token: 0x0600128C RID: 4748 RVA: 0x0007B253 File Offset: 0x00079453
	public bool SetColorTint(Color Color)
	{
		this.NPO.DiffuseColor = Color;
		return true;
	}

	// Token: 0x0600128D RID: 4749 RVA: 0x0007B262 File Offset: 0x00079462
	public bool Destroy()
	{
		return LuaGameObjectScript.DestroyObject(this);
	}

	// Token: 0x0600128E RID: 4750 RVA: 0x0007B262 File Offset: 0x00079462
	[Obsolete("Use destroy() instead.")]
	public bool DestroyObject()
	{
		return LuaGameObjectScript.DestroyObject(this);
	}

	// Token: 0x0600128F RID: 4751 RVA: 0x0007B262 File Offset: 0x00079462
	[Obsolete("Use destroy() instead.")]
	public bool Destruct()
	{
		return LuaGameObjectScript.DestroyObject(this);
	}

	// Token: 0x06001290 RID: 4752 RVA: 0x0007B26C File Offset: 0x0007946C
	public static bool DestroyObject(LuaGameObjectScript LGOS)
	{
		if (LGOS && LGOS.NPO.tableScript)
		{
			LuaGlobalScriptManager.Instance.LogError("destroyObject", "Cannot destroy the table.", null);
			return false;
		}
		NetworkSingleton<ManagerPhysicsObject>.Instance.DestroyThisObject(LGOS.gameObject);
		return true;
	}

	// Token: 0x06001291 RID: 4753 RVA: 0x0007B2BC File Offset: 0x000794BC
	public bool RegisterCollisions(bool stay = false)
	{
		this.luaCollision = base.gameObject.GetComponent<LuaCollision>();
		if (this.luaCollision == null)
		{
			this.luaCollision = base.gameObject.AddComponent<LuaCollision>();
		}
		this.luaCollision.global = LuaGlobalScriptManager.Instance.lua;
		this.luaCollision.bOnCollisionStay = (stay || this.luaCollision.bOnCollisionStay);
		return true;
	}

	// Token: 0x06001292 RID: 4754 RVA: 0x0007B32C File Offset: 0x0007952C
	public bool UnregisterCollisions()
	{
		this.luaCollision = base.gameObject.GetComponent<LuaCollision>();
		if (this.luaCollision == null || this.luaCollision.global == null)
		{
			return false;
		}
		UnityEngine.Object.Destroy(this.luaCollision);
		Wait.Frames(new Action(this.CheckCollisionEvents), 1);
		return true;
	}

	// Token: 0x06001293 RID: 4755 RVA: 0x0007B388 File Offset: 0x00079588
	public LuaGameObjectScript Clone(LuaGlobalScriptManager.LuaJsonObjectParameters parameters)
	{
		if (parameters == null)
		{
			parameters = new LuaGlobalScriptManager.LuaJsonObjectParameters();
		}
		if (parameters.position == null)
		{
			parameters.position = new Vector3?(new Vector3(0f, 3f, 0f));
		}
		List<ObjectState> objectStates = new List<ObjectState>
		{
			NetworkSingleton<ManagerPhysicsObject>.Instance.SaveObjectState(this.NPO)
		};
		List<GameObject> list = NetworkSingleton<ManagerPhysicsObject>.Instance.SpawnObjectStatesOffset(objectStates, global::Pointer.GetSpawnPosition(parameters.position.Value, true, 1f), parameters.snap_to_grid, false, true, parameters.sound);
		NetworkPhysicsObject clonedNPO = list[0].GetComponent<NetworkPhysicsObject>();
		if (parameters.rotation != null)
		{
			clonedNPO.transform.eulerAngles = parameters.rotation.Value;
		}
		if (parameters.scale != null)
		{
			clonedNPO.SetScale(parameters.scale.Value, false);
		}
		if (parameters.HasCallback())
		{
			Wait.Condition(delegate
			{
				if (clonedNPO)
				{
					parameters.TryCall(new object[]
					{
						clonedNPO.luaGameObjectScript
					});
				}
			}, () => !clonedNPO || !clonedNPO.luaGameObjectScript.spawning, float.PositiveInfinity, null);
		}
		return clonedNPO.luaGameObjectScript;
	}

	// Token: 0x06001294 RID: 4756 RVA: 0x0007B4F4 File Offset: 0x000796F4
	public LuaGameObjectScript Reload()
	{
		ObjectState os = NetworkSingleton<ManagerPhysicsObject>.Instance.SaveObjectState(base.gameObject);
		NetworkSingleton<ManagerPhysicsObject>.Instance.DestroyThisObject(base.gameObject);
		GameObject gameObject = NetworkSingleton<ManagerPhysicsObject>.Instance.LoadObjectState(os, false, false);
		if (gameObject)
		{
			return gameObject.GetComponent<LuaGameObjectScript>();
		}
		return null;
	}

	// Token: 0x06001295 RID: 4757 RVA: 0x00079C70 File Offset: 0x00077E70
	public bool GetLock()
	{
		return this.NPO.IsLocked;
	}

	// Token: 0x06001296 RID: 4758 RVA: 0x0007B540 File Offset: 0x00079740
	public bool SetLock(bool Lock)
	{
		this.NPO.IsLocked = Lock;
		return true;
	}

	// Token: 0x06001297 RID: 4759 RVA: 0x0007B54F File Offset: 0x0007974F
	[Obsolete("Use setLock() instead.")]
	public bool Lock()
	{
		this.NPO.IsLocked = true;
		return true;
	}

	// Token: 0x06001298 RID: 4760 RVA: 0x0007B55E File Offset: 0x0007975E
	[Obsolete("Use setLock() instead.")]
	public bool Unlock()
	{
		this.NPO.IsLocked = false;
		return true;
	}

	// Token: 0x06001299 RID: 4761 RVA: 0x0007B570 File Offset: 0x00079770
	public bool Randomize(string player = "")
	{
		global::Pointer pointer = NetworkSingleton<ManagerPhysicsObject>.Instance.PointerFromColorLabel(player);
		int pointerID = (pointer != null) ? pointer.ID : -1;
		bool flag = NetworkSingleton<ManagerPhysicsObject>.Instance.Randomize(this.NPO, pointerID);
		if (flag && player != "")
		{
			EventManager.TriggerObjectRandomize(this.NPO, player);
		}
		return flag;
	}

	// Token: 0x0600129A RID: 4762 RVA: 0x0007B5C9 File Offset: 0x000797C9
	[Obsolete("Use randomize() instead.")]
	public bool Shuffle()
	{
		return this.Randomize("");
	}

	// Token: 0x0600129B RID: 4763 RVA: 0x0007B5C9 File Offset: 0x000797C9
	[Obsolete("Use randomize() instead.")]
	public bool Roll()
	{
		return this.Randomize("");
	}

	// Token: 0x0600129C RID: 4764 RVA: 0x0007B5D6 File Offset: 0x000797D6
	public bool Reset()
	{
		return this.ResetInfiniteBag() || this.ResetDeck();
	}

	// Token: 0x0600129D RID: 4765 RVA: 0x0007B5E8 File Offset: 0x000797E8
	public bool Cut()
	{
		return this.Split(2) != null;
	}

	// Token: 0x0600129E RID: 4766 RVA: 0x0007B5F8 File Offset: 0x000797F8
	public List<LuaGameObjectScript> Cut(int amount)
	{
		StackObject stackObject = this.NPO.stackObject;
		if (stackObject)
		{
			int num_objects_ = stackObject.num_objects_;
			List<GameObject> list = NetworkSingleton<ManagerPhysicsObject>.Instance.CutStack(this.NPO.ID, num_objects_ - amount);
			if (list == null)
			{
				return null;
			}
			List<LuaGameObjectScript> list2 = new List<LuaGameObjectScript>();
			foreach (GameObject gameObject in list)
			{
				list2.Add(gameObject.GetComponent<LuaGameObjectScript>());
			}
			return list2;
		}
		else
		{
			DeckScript deckScript = this.NPO.deckScript;
			if (!deckScript)
			{
				return null;
			}
			int num_cards_ = deckScript.num_cards_;
			List<GameObject> list3 = NetworkSingleton<ManagerPhysicsObject>.Instance.CutDeck(this.NPO.ID, num_cards_ - amount);
			if (list3 == null)
			{
				return null;
			}
			List<LuaGameObjectScript> list4 = new List<LuaGameObjectScript>();
			foreach (GameObject gameObject2 in list3)
			{
				list4.Add(gameObject2.GetComponent<LuaGameObjectScript>());
			}
			return list4;
		}
	}

	// Token: 0x0600129F RID: 4767 RVA: 0x0007B724 File Offset: 0x00079924
	public List<LuaGameObjectScript> Split(int stackCount = 2)
	{
		if (this.NPO.stackObject)
		{
			List<GameObject> list = NetworkSingleton<ManagerPhysicsObject>.Instance.SplitStack(this.NPO.ID, stackCount);
			if (list == null)
			{
				return null;
			}
			List<LuaGameObjectScript> list2 = new List<LuaGameObjectScript>();
			foreach (GameObject gameObject in list)
			{
				list2.Add(gameObject.GetComponent<LuaGameObjectScript>());
			}
			return list2;
		}
		else
		{
			if (!this.NPO.deckScript)
			{
				return null;
			}
			List<GameObject> list3 = NetworkSingleton<ManagerPhysicsObject>.Instance.SplitDeck(this.NPO.ID, stackCount);
			if (list3 == null)
			{
				return null;
			}
			List<LuaGameObjectScript> list4 = new List<LuaGameObjectScript>();
			foreach (GameObject gameObject2 in list3)
			{
				list4.Add(gameObject2.GetComponent<LuaGameObjectScript>());
			}
			return list4;
		}
	}

	// Token: 0x060012A0 RID: 4768 RVA: 0x0007B830 File Offset: 0x00079A30
	public List<LuaGameObjectScript> Spread(float distance = 0.6f)
	{
		if (this.NPO.deckScript == null)
		{
			return null;
		}
		List<GameObject> list = NetworkSingleton<ManagerPhysicsObject>.Instance.SpreadDeck(this.NPO.ID, distance, 0f, 0);
		if (list == null)
		{
			return null;
		}
		List<LuaGameObjectScript> list2 = new List<LuaGameObjectScript>();
		foreach (GameObject gameObject in list)
		{
			list2.Add(gameObject.GetComponent<LuaGameObjectScript>());
		}
		return list2;
	}

	// Token: 0x060012A1 RID: 4769 RVA: 0x0007B8C4 File Offset: 0x00079AC4
	public DynValue GetValue()
	{
		if (this.NPO.HasRotationsValues())
		{
			return DynValue.NewNumber((double)this.NPO.GetRotationNumber(false));
		}
		if (this.NPO.clockScript != null)
		{
			return DynValue.NewNumber((double)this.NPO.clockScript.GetHowManySecondsPassed());
		}
		if (this.NPO.counterScript != null)
		{
			return DynValue.NewNumber((double)this.NPO.counterScript.GetValue());
		}
		if (base.CompareTag("Chip"))
		{
			MeshFilter component = base.gameObject.GetComponent<MeshFilter>();
			if (component != null)
			{
				string name = component.sharedMesh.name;
				if (name.StartsWith("1000"))
				{
					return DynValue.NewNumber(1000.0);
				}
				if (name.StartsWith("500"))
				{
					return DynValue.NewNumber(500.0);
				}
				if (name.StartsWith("100"))
				{
					return DynValue.NewNumber(100.0);
				}
				if (name.StartsWith("50"))
				{
					return DynValue.NewNumber(50.0);
				}
				if (name.StartsWith("10"))
				{
					return DynValue.NewNumber(10.0);
				}
			}
		}
		if (this.NPO.tabletScript != null)
		{
			return DynValue.NewString(this.NPO.tabletScript.CurrentURL);
		}
		if (this.NPO.textTool != null)
		{
			return DynValue.NewString(this.NPO.textTool.input.label.text);
		}
		if (this.NPO.hiddenZone != null)
		{
			return DynValue.NewString(this.NPO.hiddenZone.OwningColorLabel);
		}
		if (this.NPO.handZone != null)
		{
			return DynValue.NewString(this.NPO.handZone.TriggerLabel);
		}
		return null;
	}

	// Token: 0x060012A2 RID: 4770 RVA: 0x0007BAB4 File Offset: 0x00079CB4
	public bool SetValue(DynValue Value)
	{
		if (this.NPO.HasRotationsValues())
		{
			this.NPO.SetRotationNumber((int)Value.Number, -1);
			return true;
		}
		if (this.NPO.clockScript != null)
		{
			this.NPO.clockScript.StartTimer((int)Value.Number, true);
			return true;
		}
		if (this.NPO.counterScript != null)
		{
			this.NPO.counterScript.SetCounterValue((int)Value.Number);
			this.NPO.counterScript.SyncValue();
			return true;
		}
		if (this.NPO.tabletScript != null)
		{
			this.NPO.tabletScript.LoadSearchURL(Value.String);
			return true;
		}
		if (this.NPO.textTool != null)
		{
			this.NPO.textTool.SetText(Value.String);
			return true;
		}
		if (this.NPO.hiddenZone != null)
		{
			this.NPO.hiddenZone.SyncSetZoneColor(Value.String);
			return true;
		}
		if (this.NPO.handZone != null)
		{
			this.NPO.handZone.TriggerLabel = Value.String;
			return true;
		}
		return false;
	}

	// Token: 0x060012A3 RID: 4771 RVA: 0x0007BBFC File Offset: 0x00079DFC
	public List<LuaGameObjectScript.LuaRotationValue> GetRotationValues()
	{
		List<RotationValue> rotationValues = this.NPO.RotationValues;
		List<LuaGameObjectScript.LuaRotationValue> list = new List<LuaGameObjectScript.LuaRotationValue>(rotationValues.Count);
		for (int i = 0; i < rotationValues.Count; i++)
		{
			RotationValue rotationValue = rotationValues[i];
			list.Add(new LuaGameObjectScript.LuaRotationValue(rotationValue));
		}
		return list;
	}

	// Token: 0x060012A4 RID: 4772 RVA: 0x0007BC48 File Offset: 0x00079E48
	public bool SetRotationValues(List<LuaGameObjectScript.LuaRotationValue> luaRotationValues)
	{
		List<RotationValue> list = new List<RotationValue>(luaRotationValues.Count);
		for (int i = 0; i < luaRotationValues.Count; i++)
		{
			LuaGameObjectScript.LuaRotationValue luaRotationValue = luaRotationValues[i];
			if (luaRotationValue != null)
			{
				object value;
				if ((value = luaRotationValue.value) is double)
				{
					double num = (double)value;
					list.Add(new RotationValue((float)num, luaRotationValue.rotation));
				}
				else
				{
					string value2;
					if ((value2 = (luaRotationValue.value as string)) == null)
					{
						return false;
					}
					list.Add(new RotationValue(value2, luaRotationValue.rotation));
				}
			}
		}
		this.NPO.SetRotationValues(list);
		return true;
	}

	// Token: 0x060012A5 RID: 4773 RVA: 0x0007BCDE File Offset: 0x00079EDE
	public object GetRotationValue()
	{
		return LuaGameObjectScript.LuaRotationValue.GetValue(this.NPO.GetRotationValue(false));
	}

	// Token: 0x060012A6 RID: 4774 RVA: 0x0007BCF4 File Offset: 0x00079EF4
	public bool SetRotationValue(object value)
	{
		if (value is double)
		{
			double num = (double)value;
			this.NPO.SetRotationValue(num.ToString());
			return true;
		}
		string rotationValue;
		if ((rotationValue = (value as string)) != null)
		{
			this.NPO.SetRotationValue(rotationValue);
			return true;
		}
		return false;
	}

	// Token: 0x060012A7 RID: 4775 RVA: 0x0007BD40 File Offset: 0x00079F40
	public List<string> GetSelectingPlayers()
	{
		List<PlayerState> playersList = NetworkSingleton<PlayerManager>.Instance.PlayersList;
		List<string> list = new List<string>();
		for (int i = 0; i < playersList.Count; i++)
		{
			PlayerState playerState = playersList[i];
			if (!(playerState.color == Color.grey))
			{
				List<GameObject> selectedObjects = NetworkSingleton<ManagerPhysicsObject>.Instance.PointerFromColorLabel(playerState.stringColor).GetSelectedObjects(-1, true, false);
				for (int j = 0; j < selectedObjects.Count; j++)
				{
					Debug.Log(selectedObjects[j].GetComponent<LuaGameObjectScript>());
					if (selectedObjects[j].GetComponent<LuaGameObjectScript>() == this)
					{
						list.Add(playerState.stringColor);
						break;
					}
				}
			}
		}
		return list;
	}

	// Token: 0x060012A8 RID: 4776 RVA: 0x0007BDF8 File Offset: 0x00079FF8
	public bool AddToPlayerSelection(string player)
	{
		global::Pointer pointer = NetworkSingleton<ManagerPhysicsObject>.Instance.PointerFromColorLabel(player);
		if (!pointer)
		{
			return false;
		}
		if (!pointer.HighLightedObjects.Contains(this.NPO))
		{
			pointer.AddHighlight(this.NPO, true);
			return true;
		}
		return false;
	}

	// Token: 0x060012A9 RID: 4777 RVA: 0x0007BE40 File Offset: 0x0007A040
	public bool RemoveFromPlayerSelection(string player)
	{
		global::Pointer pointer = NetworkSingleton<ManagerPhysicsObject>.Instance.PointerFromColorLabel(player);
		if (!pointer)
		{
			return false;
		}
		if (pointer.HighLightedObjects.Contains(this.NPO))
		{
			pointer.RemoveHighlight(this.NPO, true);
			return true;
		}
		return false;
	}

	// Token: 0x060012AA RID: 4778 RVA: 0x0007BE88 File Offset: 0x0007A088
	public bool SetHiddenFrom(List<string> players)
	{
		string hiderLabel = "user_hidden";
		uint num = 0U;
		if (players != null)
		{
			foreach (string text in players)
			{
				uint num2 = Colour.FlagFromLabel(text);
				if (num2 == 4294967295U)
				{
					base.LogError("setHiddenFrom", "Not a player colour: " + text, null);
				}
				else
				{
					num |= num2;
				}
			}
		}
		if (num != 0U)
		{
			this.NPO.SetObscured(hiderLabel, true, num, true);
		}
		else
		{
			this.NPO.SetObscured(hiderLabel, false, 0U, true);
		}
		return true;
	}

	// Token: 0x060012AB RID: 4779 RVA: 0x0007BF2C File Offset: 0x0007A12C
	public bool SetInvisibleTo(List<string> players)
	{
		string hiderLabel = "user_hidden";
		uint num = 0U;
		if (players != null)
		{
			foreach (string text in players)
			{
				uint num2 = Colour.FlagFromLabel(text);
				if (num2 == 4294967295U)
				{
					base.LogError("setInvisibleTo", "Not a player colour: " + text, null);
				}
				else
				{
					num |= num2;
				}
			}
		}
		if (num != 0U)
		{
			this.NPO.SetInvisible(hiderLabel, true, num, true, false);
		}
		else
		{
			this.NPO.SetInvisible(hiderLabel, false, 0U, true, false);
		}
		return true;
	}

	// Token: 0x060012AC RID: 4780 RVA: 0x0007BFD0 File Offset: 0x0007A1D0
	public bool AttachHider(string id, bool hidden, List<string> players)
	{
		id = "user__" + id;
		uint num = 0U;
		if (players != null)
		{
			using (List<string>.Enumerator enumerator = players.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					string text = enumerator.Current;
					uint num2 = Colour.FlagFromLabel(text);
					if (num2 == 4294967295U)
					{
						base.LogError("attachHider", "Not a player colour: " + text, null);
					}
					else
					{
						num |= num2;
					}
				}
				goto IL_66;
			}
		}
		num = uint.MaxValue;
		IL_66:
		this.NPO.SetObscured(id, hidden, num, true);
		return true;
	}

	// Token: 0x060012AD RID: 4781 RVA: 0x0007C064 File Offset: 0x0007A264
	public bool AttachInvisibleHider(string id, bool hidden, List<string> players)
	{
		id = "user__" + id;
		uint num = 0U;
		if (players != null)
		{
			using (List<string>.Enumerator enumerator = players.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					string text = enumerator.Current;
					uint num2 = Colour.FlagFromLabel(text);
					if (num2 == 4294967295U)
					{
						base.LogError("attachInvisibleHider", "Not a player colour: " + text, null);
					}
					else
					{
						num |= num2;
					}
				}
				goto IL_66;
			}
		}
		num = uint.MaxValue;
		IL_66:
		this.NPO.SetInvisible(id, hidden, num, true, false);
		return true;
	}

	// Token: 0x060012AE RID: 4782 RVA: 0x0007C0F8 File Offset: 0x0007A2F8
	public List<string> GetTags()
	{
		List<int> list = new List<int>();
		List<string> list2 = new List<string>();
		foreach (KeyValuePair<int, TagLabel> keyValuePair in NetworkSingleton<ComponentTags>.Instance.activeLabels)
		{
			if (this.NPO.TagIsSet(keyValuePair.Key))
			{
				list.Add(keyValuePair.Key);
			}
		}
		for (int i = 0; i < list.Count; i++)
		{
			list2.Add(NetworkSingleton<ComponentTags>.Instance.activeLabels[list[i]].displayed);
		}
		return list2;
	}

	// Token: 0x060012AF RID: 4783 RVA: 0x0007C1B0 File Offset: 0x0007A3B0
	public bool SetTags(List<string> tags)
	{
		this.NPO.LoadTags(tags);
		return true;
	}

	// Token: 0x060012B0 RID: 4784 RVA: 0x0007C1BF File Offset: 0x0007A3BF
	public bool HasTag(string label)
	{
		return this.NPO.TagIsSet(label);
	}

	// Token: 0x060012B1 RID: 4785 RVA: 0x0007C1CD File Offset: 0x0007A3CD
	public bool HasAnyTag()
	{
		return this.NPO.hasTags;
	}

	// Token: 0x060012B2 RID: 4786 RVA: 0x0007C1DA File Offset: 0x0007A3DA
	public bool HasMatchingTag(LuaGameObjectScript other)
	{
		return ComponentTags.HaveMatchingFlag(this.NPO.tags, other.NPO.tags);
	}

	// Token: 0x060012B3 RID: 4787 RVA: 0x0007C1F8 File Offset: 0x0007A3F8
	public bool AddTag(string label)
	{
		TagLabel tagLabel = new TagLabel(label);
		int num = NetworkSingleton<ComponentTags>.Instance.TagIndexFromLabel(tagLabel);
		if (num == -1)
		{
			num = NetworkSingleton<ComponentTags>.Instance.AddTag(tagLabel);
		}
		if (num >= 0)
		{
			this.NPO.SetTag(num, true);
			return true;
		}
		return false;
	}

	// Token: 0x060012B4 RID: 4788 RVA: 0x0007C240 File Offset: 0x0007A440
	public bool RemoveTag(string label)
	{
		TagLabel label2 = new TagLabel(label);
		int num = NetworkSingleton<ComponentTags>.Instance.TagIndexFromLabel(label2);
		if (num >= 0)
		{
			this.NPO.SetTag(num, false);
			return true;
		}
		return false;
	}

	// Token: 0x060012B5 RID: 4789 RVA: 0x0007C278 File Offset: 0x0007A478
	public bool SetFogOfWarReveal(LuaGameObjectScript.LUAFogOfWarReveal Params)
	{
		if (Params == null)
		{
			return false;
		}
		FogOfWarRevealer component = this.NPO.GetComponent<FogOfWarRevealer>();
		if (component == null)
		{
			base.LogError("setFogOfWarReveal", "Object has no FogOfWarRevealer attached", null);
			return false;
		}
		component.Active = Params.reveal;
		component.Range = Params.range;
		string text = Params.color;
		if (text == "All")
		{
			text = "Black";
		}
		if (!Colour.IsColourLabel(text))
		{
			base.LogError("setFogOfWarReveal", "Not a valid color: must be a player colour or 'All'", null);
			return false;
		}
		component.Color = text;
		component.FoV = Params.fov;
		component.FoVOffset = Params.fov_offset;
		return true;
	}

	// Token: 0x060012B6 RID: 4790 RVA: 0x0007C320 File Offset: 0x0007A520
	public LuaGameObjectScript.LUAFogOfWarReveal GetFogOfWarReveal()
	{
		FogOfWarRevealer component = this.NPO.GetComponent<FogOfWarRevealer>();
		if (component == null)
		{
			base.LogError("getFogOfWarReveal", "Object has no FogOfWarRevealer attached", null);
			return null;
		}
		return new LuaGameObjectScript.LUAFogOfWarReveal
		{
			reveal = component.Active,
			range = component.Range,
			color = component.Color,
			fov = component.FoV,
			fov_offset = component.FoVOffset
		};
	}

	// Token: 0x060012B7 RID: 4791 RVA: 0x0007C398 File Offset: 0x0007A598
	public bool SetFogOfWar(LuaGameObjectScript.LUAFogOfWar Params)
	{
		if (Params == null)
		{
			return false;
		}
		FogOfWarZone component = this.NPO.GetComponent<FogOfWarZone>();
		if (component == null)
		{
			base.LogError("setFogOfWar", "Object is not a FogOfWar Zone", null);
			return false;
		}
		component.HideGmPointer = Params.hide_gm_pointer;
		component.HideObjects = Params.hide_objects;
		component.ReHideObjects = Params.re_hide_objects;
		component.FogHeight = Params.fog_height;
		return true;
	}

	// Token: 0x060012B8 RID: 4792 RVA: 0x0007C404 File Offset: 0x0007A604
	public LuaGameObjectScript.LUAFogOfWar GetFogOfWar()
	{
		FogOfWarZone component = this.NPO.GetComponent<FogOfWarZone>();
		if (component == null)
		{
			base.LogError("getFogOfWar", "Object is not a FogOfWar Zone", null);
			return null;
		}
		return new LuaGameObjectScript.LUAFogOfWar
		{
			hide_gm_pointer = component.HideGmPointer,
			hide_objects = component.HideObjects,
			re_hide_objects = component.ReHideObjects,
			fog_height = component.FogHeight
		};
	}

	// Token: 0x060012B9 RID: 4793 RVA: 0x0007C470 File Offset: 0x0007A670
	public bool SetCustomObject(Table Params)
	{
		if (Params == null)
		{
			return false;
		}
		if (this.NPO.customImage != null)
		{
			if (Params["image"] != null)
			{
				this.NPO.customImage.CustomImageURL = (string)Params["image"];
			}
			if (Params["image_bottom"] != null)
			{
				this.NPO.customImage.CustomImageSecondaryURL = (string)Params["image_bottom"];
			}
			if (Params["image_secondary"] != null)
			{
				this.NPO.customImage.CustomImageSecondaryURL = (string)Params["image_secondary"];
			}
			if (Params["image_scalar"] != null)
			{
				float cardScalar = this.NPO.customImage.CardScalar;
				float.TryParse(Params["image_scalar"].ToString(), out cardScalar);
				this.NPO.customImage.CardScalar = cardScalar;
			}
			if (this.NPO.customDice != null && Params["type"] != null)
			{
				int currentDiceType = (int)this.NPO.customDice.CurrentDiceType;
				int.TryParse(Params["type"].ToString(), out currentDiceType);
				this.NPO.customDice.CurrentDiceType = (DiceType)currentDiceType;
			}
			if (this.NPO.customTile != null)
			{
				if (Params["type"] != null)
				{
					int currentTileType = (int)this.NPO.customTile.CurrentTileType;
					int.TryParse(Params["type"].ToString(), out currentTileType);
					this.NPO.customTile.CurrentTileType = (TileType)currentTileType;
				}
				if (Params["thickness"] != null)
				{
					float thickness = this.NPO.customTile.Thickness;
					float.TryParse(Params["thickness"].ToString(), out thickness);
					this.NPO.customTile.Thickness = thickness;
				}
				if (Params["stackable"] != null)
				{
					bool bStackable = this.NPO.customTile.bStackable;
					bool.TryParse(Params["stackable"].ToString(), out bStackable);
					this.NPO.customTile.bStackable = bStackable;
				}
			}
			if (this.NPO.customToken != null)
			{
				if (Params["thickness"] != null)
				{
					float thickness2 = this.NPO.customToken.Thickness;
					float.TryParse(Params["thickness"].ToString(), out thickness2);
					this.NPO.customToken.Thickness = thickness2;
				}
				if (Params["merge_distance"] != null)
				{
					float mergeDistancePixels = this.NPO.customToken.MergeDistancePixels;
					float.TryParse(Params["merge_distance"].ToString(), out mergeDistancePixels);
					this.NPO.customToken.MergeDistancePixels = mergeDistancePixels;
				}
				if (Params["stand_up"] != null)
				{
					bool bStandUp = this.NPO.customToken.bStandUp;
					bool.TryParse(Params["stand_up"].ToString(), out bStandUp);
					this.NPO.customToken.bStandUp = bStandUp;
				}
				if (Params["stackable"] != null)
				{
					bool bStackable2 = this.NPO.customToken.bStackable;
					bool.TryParse(Params["stackable"].ToString(), out bStackable2);
					this.NPO.customToken.bStackable = bStackable2;
				}
			}
		}
		else if (this.NPO.deckScript)
		{
			string faceURL = "";
			if (Params["face"] != null)
			{
				faceURL = (string)Params["face"];
			}
			bool uniqueBack = false;
			if (Params["unique_back"] != null)
			{
				bool.TryParse(Params["unique_back"].ToString(), out uniqueBack);
			}
			string backURL = "";
			if (Params["back"] != null)
			{
				backURL = (string)Params["back"];
			}
			int numWidth = 10;
			if (Params["width"] != null)
			{
				numWidth = (int)Convert.ToSingle(Params["width"]);
			}
			int numHeight = 7;
			if (Params["height"] != null)
			{
				numHeight = (int)Convert.ToSingle(Params["height"]);
			}
			int numberCards = 52;
			if (Params["number"] != null)
			{
				numberCards = (int)Convert.ToSingle(Params["number"]);
			}
			bool sideways = false;
			if (Params["sideways"] != null)
			{
				bool.TryParse(Params["sideways"].ToString(), out sideways);
			}
			bool backIsHidden = false;
			if (Params["back_is_hidden"] != null)
			{
				bool.TryParse(Params["back_is_hidden"].ToString(), out backIsHidden);
			}
			CardType type = CardType.RectangleRounded;
			if (Params["type"] != null)
			{
				int num;
				int.TryParse(Params["type"].ToString(), out num);
				type = (CardType)num;
			}
			NetworkSingleton<CardManagerScript>.Instance.SetupCustomDeck(this.NPO.deckScript, new CustomDeckData(faceURL, backURL, numWidth, numHeight, backIsHidden, uniqueBack, type), numberCards, sideways);
		}
		else if (this.NPO.cardScript)
		{
			string faceURL2 = "";
			if (Params["face"] != null)
			{
				faceURL2 = (string)Params["face"];
			}
			bool uniqueBack2 = false;
			if (Params["unique_back"] != null)
			{
				bool.TryParse(Params["unique_back"].ToString(), out uniqueBack2);
			}
			string backURL2 = "";
			if (Params["back"] != null)
			{
				backURL2 = (string)Params["back"];
			}
			int numWidth2 = 1;
			if (Params["width"] != null)
			{
				numWidth2 = (int)Convert.ToSingle(Params["width"]);
			}
			int numHeight2 = 1;
			if (Params["height"] != null)
			{
				numHeight2 = (int)Convert.ToSingle(Params["height"]);
			}
			bool sideways2 = false;
			if (Params["sideways"] != null)
			{
				bool.TryParse(Params["sideways"].ToString(), out sideways2);
			}
			bool backIsHidden2 = true;
			if (Params["back_is_hidden"] != null)
			{
				bool.TryParse(Params["back_is_hidden"].ToString(), out backIsHidden2);
			}
			CardType type2 = CardType.RectangleRounded;
			if (Params["type"] != null)
			{
				int num2;
				int.TryParse(Params["type"].ToString(), out num2);
				type2 = (CardType)num2;
			}
			NetworkSingleton<CardManagerScript>.Instance.SetupCustomCard(this.NPO.cardScript, new CustomDeckData(faceURL2, backURL2, numWidth2, numHeight2, backIsHidden2, uniqueBack2, type2), sideways2);
		}
		else if (this.NPO.customMesh != null)
		{
			if (Params["mesh"] != null)
			{
				this.NPO.customMesh.customMeshState.MeshURL = (string)Params["mesh"];
			}
			if (Params["diffuse"] != null)
			{
				this.NPO.customMesh.customMeshState.DiffuseURL = (string)Params["diffuse"];
			}
			if (Params["normal"] != null)
			{
				this.NPO.customMesh.customMeshState.NormalURL = (string)Params["normal"];
			}
			if (Params["collider"] != null)
			{
				this.NPO.customMesh.customMeshState.ColliderURL = (string)Params["collider"];
			}
			if (Params["convex"] != null)
			{
				bool convex = this.NPO.customMesh.customMeshState.Convex;
				bool.TryParse(Params["convex"].ToString(), out convex);
				this.NPO.customMesh.customMeshState.Convex = convex;
			}
			if (Params["type"] != null)
			{
				int typeIndex = this.NPO.customMesh.customMeshState.TypeIndex;
				int.TryParse(Params["type"].ToString(), out typeIndex);
				this.NPO.customMesh.customMeshState.TypeIndex = typeIndex;
			}
			if (Params["material"] != null)
			{
				int materialIndex = this.NPO.customMesh.customMeshState.MaterialIndex;
				int.TryParse(Params["material"].ToString(), out materialIndex);
				this.NPO.customMesh.customMeshState.MaterialIndex = materialIndex;
			}
			CustomShaderState customShaderState = this.NPO.customMesh.GetCustomShaderState();
			bool flag = false;
			if (Params["specular_intensity"] != null)
			{
				float specularIntensity = customShaderState.SpecularIntensity;
				float.TryParse(Params["specular_intensity"].ToString(), out specularIntensity);
				customShaderState.SpecularIntensity = specularIntensity;
				flag = true;
			}
			Color color = Colour.UnityWhite;
			if (LuaCustomConverter.TryParse(Params, "specular_color", ref color))
			{
				customShaderState.SpecularColor = new ColourState(color);
				flag = true;
			}
			if (Params["specular_sharpness"] != null)
			{
				float specularSharpness = customShaderState.SpecularSharpness;
				float.TryParse(Params["specular_sharpness"].ToString(), out specularSharpness);
				customShaderState.SpecularSharpness = specularSharpness;
				flag = true;
			}
			if (Params["fresnel_strength"] != null)
			{
				float fresnelStrength = customShaderState.FresnelStrength;
				float.TryParse(Params["fresnel_strength"].ToString(), out fresnelStrength);
				customShaderState.FresnelStrength = fresnelStrength;
				flag = true;
			}
			if (flag)
			{
				this.NPO.customMesh.customMeshState.CustomShader = customShaderState;
			}
			if (Params["cast_shadows"] != null)
			{
				bool castShadows = this.NPO.customMesh.customMeshState.CastShadows;
				bool.TryParse(Params["cast_shadows"].ToString(), out castShadows);
				this.NPO.customMesh.customMeshState.CastShadows = castShadows;
			}
		}
		else if (this.NPO.customAssetbundle)
		{
			if (Params["assetbundle"] != null)
			{
				this.NPO.customAssetbundle.CustomAssetbundleURL = (string)Params["assetbundle"];
			}
			if (Params["assetbundle_secondary"] != null)
			{
				this.NPO.customAssetbundle.CustomAssetbundleSecondaryURL = (string)Params["assetbundle_secondary"];
			}
			if (Params["type"] != null)
			{
				int typeInt = this.NPO.customAssetbundle.TypeInt;
				int.TryParse(Params["type"].ToString(), out typeInt);
				this.NPO.customAssetbundle.TypeInt = typeInt;
			}
			if (Params["material"] != null)
			{
				int materialInt = this.NPO.customAssetbundle.MaterialInt;
				int.TryParse(Params["material"].ToString(), out materialInt);
				this.NPO.customAssetbundle.MaterialInt = materialInt;
			}
		}
		return this.NPO.customObject;
	}

	// Token: 0x060012BA RID: 4794 RVA: 0x0007CF5C File Offset: 0x0007B15C
	public Table GetCustomObject(Script script)
	{
		Table table = new Table(script);
		if (this.NPO.customImage != null)
		{
			table["image"] = this.NPO.customImage.CustomImageURL;
			table["image_bottom"] = this.NPO.customImage.CustomImageSecondaryURL;
			table["image_secondary"] = this.NPO.customImage.CustomImageSecondaryURL;
			table["image_scalar"] = this.NPO.customImage.CardScalar;
			if (this.NPO.customDice != null)
			{
				table["type"] = (int)this.NPO.customDice.CurrentDiceType;
			}
			if (this.NPO.customTile != null)
			{
				table["type"] = (int)this.NPO.customTile.CurrentTileType;
				table["thickness"] = this.NPO.customTile.Thickness;
				table["stackable"] = this.NPO.customTile.bStackable;
			}
			if (this.NPO.customToken != null)
			{
				table["thickness"] = this.NPO.customToken.Thickness;
				table["merge_distance"] = this.NPO.customToken.MergeDistancePixels;
				table["stand_up"] = this.NPO.customToken.bStandUp;
				table["stackable"] = this.NPO.customToken.bStackable;
			}
		}
		else if (this.NPO.deckScript != null)
		{
			List<CustomDeckData> customDeckDatas = this.NPO.deckScript.GetCustomDeckDatas();
			for (int i = 0; i < customDeckDatas.Count; i++)
			{
				CustomDeckData customDeckData = customDeckDatas[i];
				Table table2 = new Table(script);
				table[i + 1] = table2;
				table2["face"] = customDeckData.FaceURL;
				table2["unique_back"] = customDeckData.UniqueBack;
				table2["back"] = customDeckData.BackURL;
				table2["width"] = customDeckData.NumWidth;
				table2["height"] = customDeckData.NumHeight;
				table2["number"] = this.NPO.deckScript.num_cards_;
				table2["sideways"] = this.NPO.deckScript.bSideways;
				table2["back_is_hidden"] = customDeckData.BackIsHidden;
				table2["type"] = (int)customDeckData.Type;
			}
		}
		else if (this.NPO.cardScript != null)
		{
			List<CustomDeckData> customDeckDatas2 = this.NPO.cardScript.GetCustomDeckDatas();
			if (customDeckDatas2.Count > 0)
			{
				CustomDeckData customDeckData2 = customDeckDatas2[0];
				table["face"] = customDeckData2.FaceURL;
				table["unique_back"] = customDeckData2.UniqueBack;
				table["back"] = customDeckData2.BackURL;
				table["width"] = customDeckData2.NumWidth;
				table["height"] = customDeckData2.NumHeight;
				table["sideways"] = this.NPO.cardScript.bSideways;
				table["back_is_hidden"] = customDeckData2.BackIsHidden;
				table["type"] = (int)customDeckData2.Type;
			}
		}
		else if (this.NPO.customMesh != null)
		{
			table["mesh"] = this.NPO.customMesh.customMeshState.MeshURL;
			table["diffuse"] = this.NPO.customMesh.customMeshState.DiffuseURL;
			table["normal"] = this.NPO.customMesh.customMeshState.NormalURL;
			table["collider"] = this.NPO.customMesh.customMeshState.ColliderURL;
			table["convex"] = this.NPO.customMesh.customMeshState.Convex;
			table["type"] = this.NPO.customMesh.customMeshState.TypeIndex;
			table["material"] = this.NPO.customMesh.customMeshState.MaterialIndex;
			table["cast_shadows"] = this.NPO.customMesh.customMeshState.CastShadows;
			CustomShaderState customShaderState = this.NPO.customMesh.GetCustomShaderState();
			table["specular_intensity"] = customShaderState.SpecularIntensity;
			table["specular_color"] = LuaCustomConverter.GetTable(customShaderState.SpecularColor.ToColour(), script);
			table["specular_sharpness"] = customShaderState.SpecularSharpness;
			table["fresnel_strength"] = customShaderState.FresnelStrength;
		}
		else if (this.NPO.customAssetbundle)
		{
			table["assetbundle"] = this.NPO.customAssetbundle.CustomAssetbundleURL;
			table["assetbundle_secondary"] = this.NPO.customAssetbundle.CustomAssetbundleSecondaryURL;
			table["type"] = this.NPO.customAssetbundle.TypeInt;
			table["material"] = this.NPO.customAssetbundle.MaterialInt;
		}
		else if (this.NPO.GetComponent<JigsawPiece>() != null)
		{
			table["desired_position"] = this.NPO.GetComponent<JigsawPiece>().desiredPosition;
		}
		return table;
	}

	// Token: 0x060012BB RID: 4795 RVA: 0x0007D5D0 File Offset: 0x0007B7D0
	public bool Deal(int NumCards, string Color = "Seated", int HandIndex = 1)
	{
		NetworkSingleton<ManagerPhysicsObject>.Instance.DealObjectToColor(this.NPO.ID, Color, NumCards, HandIndex - 1);
		return true;
	}

	// Token: 0x060012BC RID: 4796 RVA: 0x0007D5ED File Offset: 0x0007B7ED
	[Obsolete("Use deal() instead.")]
	public bool DealToAll(int NumCards)
	{
		return this.Deal(NumCards, "Seated", 1);
	}

	// Token: 0x060012BD RID: 4797 RVA: 0x0007D5FC File Offset: 0x0007B7FC
	[Obsolete("Use deal() instead.")]
	public bool DealToColor(int NumCards, string Color)
	{
		return this.Deal(NumCards, Color, 1);
	}

	// Token: 0x060012BE RID: 4798 RVA: 0x0007D607 File Offset: 0x0007B807
	public LuaGameObjectScript DealToColorWithOffset(List<float> Offset, bool Flip, string Color)
	{
		return LuaGlobalScriptManager.Instance.DealCardToColorWithOffset(this, Offset, Flip, Color);
	}

	// Token: 0x060012BF RID: 4799 RVA: 0x0007D618 File Offset: 0x0007B818
	private LuaGameObjectScript Remainder()
	{
		StackObject stackObject = this.NPO.stackObject;
		if (stackObject && stackObject.LastObject != null)
		{
			return stackObject.LastObject.GetComponent<LuaGameObjectScript>();
		}
		DeckScript deckScript = this.NPO.deckScript;
		if (deckScript && deckScript.LastCard != null)
		{
			return deckScript.LastCard.GetComponent<LuaGameObjectScript>();
		}
		return null;
	}

	// Token: 0x060012C0 RID: 4800 RVA: 0x0007D684 File Offset: 0x0007B884
	public LuaGameObjectScript TakeObject(LuaGameObjectScript.LuaTakeObjectParameters Params)
	{
		if (Params == null)
		{
			Params = new LuaGameObjectScript.LuaTakeObjectParameters();
		}
		if (Params.position == null)
		{
			Params.position = new Vector3?(new Vector3(base.transform.position.x + 2f, base.transform.position.y, base.transform.position.z));
		}
		if (Params.rotation == null)
		{
			Params.rotation = new Vector3?(base.transform.eulerAngles);
		}
		Vector3 value = Params.position.Value;
		Vector3 value2 = Params.rotation.Value;
		if (Params.callback_owner == null)
		{
			Params.callback_owner = this;
		}
		StackObject stackObject = this.NPO.stackObject;
		if (stackObject)
		{
			GameObject gameObject = null;
			NetworkPhysicsObject component;
			if (!stackObject.IsInfiniteStack && (Params.guid != null || Params.index != null))
			{
				if (Params.guid != null)
				{
					gameObject = stackObject.RemoveItemByGUID(Params.guid, stackObject.transform.position);
				}
				else if (Params.index != null)
				{
					gameObject = stackObject.RemoveItemRPC(Params.index.Value, stackObject.transform.position);
				}
				component = gameObject.GetComponent<NetworkPhysicsObject>();
			}
			else
			{
				gameObject = stackObject.TakeObject(true);
				component = gameObject.GetComponent<NetworkPhysicsObject>();
			}
			if (!Params.smooth)
			{
				component.transform.position = value;
				component.transform.eulerAngles = value2;
			}
			else
			{
				component.SetSmoothPosition(value, false, false, false, true, null, false, false, null);
				component.SetSmoothRotation(value2, false, false, false, true, null, false);
			}
			this.TakeObjectCallback(component, Params);
			return component.luaGameObjectScript;
		}
		DeckScript deckScript = this.NPO.deckScript;
		if (deckScript)
		{
			GameObject gameObject2 = null;
			if (Params.guid != null || Params.index != null)
			{
				if (Params.guid != null)
				{
					gameObject2 = deckScript.RemoveCardByGUID(Params.guid, deckScript.transform.position);
				}
				else if (Params.index != null)
				{
					gameObject2 = deckScript.RemoveCardRPC(Params.index.Value, deckScript.transform.position);
				}
			}
			else
			{
				bool top = deckScript.transform.up.normalized.y < 0f;
				if (!Params.top)
				{
					top = (deckScript.transform.up.normalized.y > 0f);
				}
				gameObject2 = deckScript.TakeCard(top, true);
			}
			NetworkPhysicsObject component2 = gameObject2.GetComponent<NetworkPhysicsObject>();
			if (Params.flip)
			{
				value2 = new Vector3(value2.x, value2.y, gameObject2.transform.localEulerAngles.z + 180f);
			}
			if (!Params.smooth)
			{
				component2.transform.position = value;
				component2.transform.eulerAngles = value2;
			}
			else
			{
				component2.SetSmoothPosition(value, false, false, false, true, null, false, false, null);
				component2.SetSmoothRotation(value2, false, false, false, true, null, false);
			}
			this.TakeObjectCallback(component2, Params);
			return component2.luaGameObjectScript;
		}
		return null;
	}

	// Token: 0x060012C1 RID: 4801 RVA: 0x0007D9D0 File Offset: 0x0007BBD0
	private void TakeObjectCallback(NetworkPhysicsObject TakenNPO, LuaGameObjectScript.LuaTakeObjectParameters Params)
	{
		TakenNPO.SetCollision(false);
		Wait.Condition(delegate
		{
			if (TakenNPO)
			{
				Params.TryCall(new object[]
				{
					TakenNPO.luaGameObjectScript
				});
				TakenNPO.SetCollision(true);
			}
		}, () => !TakenNPO || !TakenNPO.IsSmoothMoving, float.PositiveInfinity, null);
	}

	// Token: 0x060012C2 RID: 4802 RVA: 0x0007DA24 File Offset: 0x0007BC24
	public LuaGameObjectScript PutObject(LuaGameObjectScript luaObject)
	{
		NetworkPhysicsObject npo = luaObject.NPO;
		if (this.NPO.stackObject)
		{
			if (this.NPO.stackObject.bBag)
			{
				this.NPO.stackObject.AddToBag(npo);
				return this.NPO.luaGameObjectScript;
			}
			if (this.NPO.stackObject.IsInfiniteStack)
			{
				if (!this.NPO.stackObject.AddToInfiniteBag(npo))
				{
					return null;
				}
				return this.NPO.luaGameObjectScript;
			}
			else if (this.NPO.stackObject.CheckStackable(npo))
			{
				NetworkPhysicsObject networkPhysicsObject = NetworkSingleton<ManagerPhysicsObject>.Instance.StackHitNPO(this.NPO, npo, false);
				if (!networkPhysicsObject)
				{
					return null;
				}
				return networkPhysicsObject.luaGameObjectScript;
			}
		}
		if (this.NPO.deckScript)
		{
			if (npo.deckScript)
			{
				NetworkPhysicsObject networkPhysicsObject2 = NetworkSingleton<ManagerPhysicsObject>.Instance.NotifyDeckHitDeck(this.NPO, npo, false);
				if (!networkPhysicsObject2)
				{
					return null;
				}
				return networkPhysicsObject2.luaGameObjectScript;
			}
			else if (npo.cardScript)
			{
				NetworkPhysicsObject networkPhysicsObject3 = NetworkSingleton<ManagerPhysicsObject>.Instance.NotifyCardHitDeck(npo, this.NPO, false);
				if (!networkPhysicsObject3)
				{
					return null;
				}
				return networkPhysicsObject3.luaGameObjectScript;
			}
		}
		if (this.NPO.cardScript)
		{
			if (npo.deckScript)
			{
				NetworkPhysicsObject networkPhysicsObject4 = NetworkSingleton<ManagerPhysicsObject>.Instance.NotifyCardHitDeck(this.NPO, npo, false);
				if (!networkPhysicsObject4)
				{
					return null;
				}
				return networkPhysicsObject4.luaGameObjectScript;
			}
			else if (npo.cardScript)
			{
				NetworkPhysicsObject networkPhysicsObject5 = NetworkSingleton<ManagerPhysicsObject>.Instance.NotifyCardHitCard(this.NPO, npo, false);
				if (!networkPhysicsObject5)
				{
					return null;
				}
				return networkPhysicsObject5.luaGameObjectScript;
			}
		}
		if (!this.NPO.GetComponent<CheckStackObject>() || !npo.GetComponent<CheckStackObject>() || !this.NPO.GetComponent<CheckStackObject>().CheckStackable(npo))
		{
			return null;
		}
		NetworkPhysicsObject networkPhysicsObject6 = NetworkSingleton<ManagerPhysicsObject>.Instance.NPOHitNPO(this.NPO, npo, false);
		if (!networkPhysicsObject6)
		{
			return null;
		}
		return networkPhysicsObject6.GetComponent<LuaGameObjectScript>();
	}

	// Token: 0x060012C3 RID: 4803 RVA: 0x0007DC30 File Offset: 0x0007BE30
	public List<LuaGameObjectScript.LuaSnapPointParameters> GetSnapPoints()
	{
		List<LuaGameObjectScript.LuaSnapPointParameters> list = new List<LuaGameObjectScript.LuaSnapPointParameters>();
		List<SnapPointInfo> snapPointStates = NetworkSingleton<SnapPointManager>.Instance.GetSnapPointStates(this.GetNPONetworkView());
		if (snapPointStates == null)
		{
			return list;
		}
		foreach (SnapPointInfo snapPointInfo in snapPointStates)
		{
			List<LuaGameObjectScript.LuaSnapPointParameters> list2 = list;
			LuaGameObjectScript.LuaSnapPointParameters luaSnapPointParameters = new LuaGameObjectScript.LuaSnapPointParameters();
			luaSnapPointParameters.position = snapPointInfo.Position.ToVector();
			SnapPointInfo snapPointInfo2 = snapPointInfo;
			luaSnapPointParameters.rotation = ((snapPointInfo2.Rotation != null) ? snapPointInfo2.Rotation.GetValueOrDefault().ToVector() : Vector3.zero);
			luaSnapPointParameters.rotation_snap = (snapPointInfo.Rotation != null);
			luaSnapPointParameters.tags = NetworkSingleton<ComponentTags>.Instance.DisplayedTagLabelsFromTags(snapPointInfo.Tags);
			list2.Add(luaSnapPointParameters);
		}
		return list;
	}

	// Token: 0x060012C4 RID: 4804 RVA: 0x0007DD04 File Offset: 0x0007BF04
	public bool SetSnapPoints(List<LuaGameObjectScript.LuaSnapPointParameters> snapPointList)
	{
		NetworkSingleton<SnapPointManager>.Instance.SetSnapPoints(snapPointList, this.GetNPONetworkView());
		return true;
	}

	// Token: 0x060012C5 RID: 4805 RVA: 0x0007DD18 File Offset: 0x0007BF18
	public List<LuaGameObjectScript.LuaJointParameters> GetJoints()
	{
		if (this.NPO == null)
		{
			return null;
		}
		List<LuaGameObjectScript.LuaJointParameters> list = new List<LuaGameObjectScript.LuaJointParameters>();
		FixedJoint[] components = this.NPO.GetComponents<FixedJoint>();
		if (components != null)
		{
			foreach (FixedJoint fixedJoint in components)
			{
				LuaGameObjectScript.LuaJointParameters item = new LuaGameObjectScript.LuaJointParameters
				{
					type = "Fixed",
					joint_object_guid = fixedJoint.connectedBody.gameObject.GetComponent<NetworkPhysicsObject>().GUID,
					collision = fixedJoint.enableCollision,
					break_force = fixedJoint.breakForce,
					break_torgue = fixedJoint.breakTorque,
					axis = fixedJoint.axis,
					anchor = fixedJoint.anchor,
					connected_anchor = fixedJoint.connectedAnchor
				};
				list.Add(item);
			}
		}
		HingeJoint[] components2 = this.NPO.GetComponents<HingeJoint>();
		if (components2 != null)
		{
			foreach (HingeJoint hingeJoint in components2)
			{
				LuaGameObjectScript.LuaJointParameters item2 = new LuaGameObjectScript.LuaJointParameters
				{
					type = "Hinge",
					joint_object_guid = hingeJoint.connectedBody.gameObject.GetComponent<NetworkPhysicsObject>().GUID,
					collision = hingeJoint.enableCollision,
					break_force = hingeJoint.breakForce,
					break_torgue = hingeJoint.breakTorque,
					axis = hingeJoint.axis,
					anchor = hingeJoint.anchor,
					connected_anchor = hingeJoint.connectedAnchor,
					motor_force = hingeJoint.motor.force,
					motor_velocity = hingeJoint.motor.targetVelocity,
					motor_free_spin = hingeJoint.motor.freeSpin
				};
				list.Add(item2);
			}
		}
		SpringJoint[] components3 = this.NPO.GetComponents<SpringJoint>();
		if (components3 != null)
		{
			foreach (SpringJoint springJoint in components3)
			{
				LuaGameObjectScript.LuaJointParameters item3 = new LuaGameObjectScript.LuaJointParameters
				{
					type = "Spring",
					joint_object_guid = springJoint.connectedBody.gameObject.GetComponent<NetworkPhysicsObject>().GUID,
					collision = springJoint.enableCollision,
					break_force = springJoint.breakForce,
					break_torgue = springJoint.breakTorque,
					axis = springJoint.axis,
					anchor = springJoint.anchor,
					connected_anchor = springJoint.connectedAnchor,
					spring = springJoint.spring,
					damper = springJoint.damper,
					max_distance = springJoint.maxDistance,
					min_distance = springJoint.minDistance
				};
				list.Add(item3);
			}
		}
		return list;
	}

	// Token: 0x060012C6 RID: 4806 RVA: 0x0007DFE0 File Offset: 0x0007C1E0
	public void JointTo()
	{
		if (this.NPO.gameObject.GetComponent<Joint>())
		{
			Joint[] components = this.NPO.gameObject.GetComponents<Joint>();
			for (int i = 0; i < components.Length; i++)
			{
				UnityEngine.Object.Destroy(components[i]);
			}
		}
	}

	// Token: 0x060012C7 RID: 4807 RVA: 0x0007E02B File Offset: 0x0007C22B
	public bool JointTo(LuaGameObjectScript.LuaJointParameters parameters)
	{
		return parameters.type != null && parameters.joint_object_guid != string.Empty && this.JointTo(NetworkSingleton<ManagerPhysicsObject>.Instance.NPOFromGUID(parameters.joint_object_guid).luaGameObjectScript, parameters);
	}

	// Token: 0x060012C8 RID: 4808 RVA: 0x0007E068 File Offset: 0x0007C268
	public bool JointTo(LuaGameObjectScript luaObject, LuaGameObjectScript.LuaJointParameters parameters)
	{
		if (this.NPO == null)
		{
			return false;
		}
		if (luaObject != null && parameters.type != null)
		{
			NetworkPhysicsObject npo = luaObject.NPO;
			if (parameters.type == "Fixed")
			{
				FixedJoint fixedJoint = this.NPO.gameObject.AddComponent<FixedJoint>();
				this.NPO.AddJoint(fixedJoint);
				Joint joint = fixedJoint;
				joint.connectedBody = npo.GetComponent<Rigidbody>();
				joint.enableCollision = parameters.collision;
				joint.anchor = parameters.anchor;
				if (parameters.axis != Vector3.zero)
				{
					joint.axis = parameters.axis;
				}
				if (parameters.connected_anchor != Vector3.zero)
				{
					joint.connectedAnchor = parameters.connected_anchor;
				}
				if (parameters.break_force != 0f)
				{
					joint.breakForce = parameters.break_force;
				}
				if (parameters.break_torgue != 0f)
				{
					joint.breakTorque = parameters.break_torgue;
				}
				return true;
			}
			if (parameters.type == "Hinge")
			{
				HingeJoint hingeJoint = this.NPO.gameObject.AddComponent<HingeJoint>();
				this.NPO.AddJoint(hingeJoint);
				Joint joint2 = hingeJoint;
				joint2.connectedBody = npo.GetComponent<Rigidbody>();
				joint2.enableCollision = parameters.collision;
				joint2.anchor = parameters.anchor;
				if (parameters.axis != Vector3.zero)
				{
					joint2.axis = parameters.axis;
				}
				if (parameters.connected_anchor != Vector3.zero)
				{
					joint2.connectedAnchor = parameters.connected_anchor;
				}
				if (parameters.break_force != 0f)
				{
					joint2.breakForce = parameters.break_force;
				}
				if (parameters.break_torgue != 0f)
				{
					joint2.breakTorque = parameters.break_torgue;
				}
				if (parameters.motor_force != 0f || parameters.motor_velocity != 0f)
				{
					hingeJoint.useMotor = true;
					JointMotor motor = new JointMotor
					{
						force = parameters.motor_force,
						targetVelocity = parameters.motor_velocity,
						freeSpin = parameters.motor_free_spin
					};
					hingeJoint.motor = motor;
				}
				return true;
			}
			if (parameters.type == "Spring")
			{
				SpringJoint springJoint = this.NPO.gameObject.AddComponent<SpringJoint>();
				this.NPO.AddJoint(springJoint);
				Joint joint3 = springJoint;
				joint3.connectedBody = npo.GetComponent<Rigidbody>();
				joint3.enableCollision = parameters.collision;
				joint3.anchor = parameters.anchor;
				if (parameters.axis != Vector3.zero)
				{
					joint3.axis = parameters.axis;
				}
				if (parameters.connected_anchor != Vector3.zero)
				{
					joint3.connectedAnchor = parameters.connected_anchor;
				}
				if (parameters.break_force != 0f)
				{
					joint3.breakForce = parameters.break_force;
				}
				if (parameters.break_torgue != 0f)
				{
					joint3.breakTorque = parameters.break_torgue;
				}
				if (parameters.spring != 0f)
				{
					springJoint.spring = parameters.spring;
				}
				if (parameters.damper != 0f)
				{
					springJoint.damper = parameters.damper;
				}
				if (parameters.max_distance != 0f)
				{
					springJoint.maxDistance = parameters.max_distance;
				}
				if (parameters.min_distance != 0f)
				{
					springJoint.minDistance = parameters.min_distance;
				}
				return true;
			}
		}
		else if (this.NPO.gameObject.GetComponent<Joint>())
		{
			Joint[] components = this.NPO.gameObject.GetComponents<Joint>();
			for (int i = 0; i < components.Length; i++)
			{
				UnityEngine.Object.Destroy(components[i]);
			}
		}
		return false;
	}

	// Token: 0x060012C9 RID: 4809 RVA: 0x0007E41C File Offset: 0x0007C61C
	[MoonSharpHidden]
	public bool ResetInfiniteBag()
	{
		StackObject component = base.GetComponent<StackObject>();
		if (component == null)
		{
			return false;
		}
		component.ResetObjectsContained();
		return true;
	}

	// Token: 0x060012CA RID: 4810 RVA: 0x0007E442 File Offset: 0x0007C642
	[MoonSharpHidden]
	public bool ResetDeck()
	{
		if (this == null)
		{
			return false;
		}
		if (base.GetComponent<DeckScript>() == null)
		{
			return false;
		}
		NetworkSingleton<ManagerPhysicsObject>.Instance.ResetDeck(this.NPO.ID);
		return true;
	}

	// Token: 0x060012CB RID: 4811 RVA: 0x0007E478 File Offset: 0x0007C678
	public int GetQuantity()
	{
		if (this.NPO.stackObject)
		{
			return this.NPO.stackObject.num_objects_;
		}
		if (this.NPO.deckScript)
		{
			return this.NPO.deckScript.num_cards_;
		}
		return -1;
	}

	// Token: 0x060012CC RID: 4812 RVA: 0x0007E4CC File Offset: 0x0007C6CC
	private static Table GetTableFromList<T>(Script script, List<T> list)
	{
		Table table = new Table(script);
		if (list != null)
		{
			for (int i = 0; i < list.Count; i++)
			{
				table[i + 1] = list[i];
			}
		}
		return table;
	}

	// Token: 0x060012CD RID: 4813 RVA: 0x0007E510 File Offset: 0x0007C710
	public Table GetObjects(Script objectScript, bool all_objects = false)
	{
		if (!this.NPO)
		{
			return LuaGameObjectScript.GetTableFromList<LuaGameObjectScript>(objectScript, LuaGlobalScriptManager.GetObjects());
		}
		if (this.NPO.zone)
		{
			return LuaGameObjectScript.GetObjectsInZone(objectScript, this, all_objects);
		}
		if (base.CompareTag("Bag"))
		{
			return LuaGameObjectScript.GetObjectsInBag(objectScript, this);
		}
		if (base.CompareTag("Deck"))
		{
			return LuaGameObjectScript.GetCardsInDeck(objectScript, this);
		}
		base.LogError("getObjects", "Attempting to call getObjects() on an object that does not support getObjects()", null);
		return null;
	}

	// Token: 0x060012CE RID: 4814 RVA: 0x0007E590 File Offset: 0x0007C790
	[MoonSharpHidden]
	public static Table GetObjectsInZone(Script script, LuaGameObjectScript zone, bool ignoreTags)
	{
		List<NetworkPhysicsObject> containedNPOs = zone.NPO.zone.ContainedNPOs;
		Table table = new Table(script);
		HandZone handZone = zone.NPO.zone as HandZone;
		int i;
		for (i = 0; i < containedNPOs.Count; i++)
		{
			table[i + 1] = DynValue.FromObject(zone.lua, containedNPOs[i].luaGameObjectScript);
		}
		if (ignoreTags)
		{
			List<NetworkPhysicsObject> invalidContainedNPOs = zone.NPO.zone.InvalidContainedNPOs;
			for (int j = 0; j < invalidContainedNPOs.Count; j++)
			{
				table[++i] = DynValue.FromObject(zone.lua, invalidContainedNPOs[j].luaGameObjectScript);
			}
			if (handZone && handZone.Stash)
			{
				table[i + 1] = DynValue.FromObject(zone.lua, handZone.Stash.luaGameObjectScript);
			}
		}
		return table;
	}

	// Token: 0x060012CF RID: 4815 RVA: 0x0007E690 File Offset: 0x0007C890
	[MoonSharpHidden]
	public static Table GetObjectsInBag(Script script, LuaGameObjectScript Bag)
	{
		List<ObjectState> objectsHolder = Bag.NPO.stackObject.ObjectsHolder;
		Table table = new Table(script);
		for (int i = 0; i < objectsHolder.Count; i++)
		{
			ObjectState os = objectsHolder[i];
			Table table2 = LuaGameObjectScript.ObjectStateToTable(script, os);
			table2["index"] = i;
			table[i + 1] = table2;
		}
		return table;
	}

	// Token: 0x060012D0 RID: 4816 RVA: 0x0007E6FC File Offset: 0x0007C8FC
	[MoonSharpHidden]
	public static Table GetCardsInDeck(Script script, LuaGameObjectScript Deck)
	{
		List<ObjectState> cardStates = Deck.NPO.deckScript.GetCardStates();
		Table table = new Table(script);
		for (int i = 0; i < cardStates.Count; i++)
		{
			ObjectState os = cardStates[i];
			Table table2 = LuaGameObjectScript.ObjectStateToTable(script, os);
			table2["index"] = i;
			table[i + 1] = table2;
		}
		return table;
	}

	// Token: 0x060012D1 RID: 4817 RVA: 0x0007E768 File Offset: 0x0007C968
	public List<LuaGameObjectScript> GetZones()
	{
		List<LuaGameObjectScript> list = new List<LuaGameObjectScript>();
		foreach (NetworkPhysicsObject networkPhysicsObject in NetworkSingleton<ManagerPhysicsObject>.Instance.GrabbableNPOs)
		{
			if (networkPhysicsObject.zone && networkPhysicsObject.zone.ContainedNPOs.Contains(this.NPO))
			{
				list.Add(networkPhysicsObject.luaGameObjectScript);
			}
		}
		return list;
	}

	// Token: 0x060012D2 RID: 4818 RVA: 0x0007E7F0 File Offset: 0x0007C9F0
	public Table GetStates(Script script)
	{
		if (this.NPO.States == null)
		{
			return null;
		}
		Table table = new Table(script);
		int num = 0;
		foreach (KeyValuePair<int, ObjectState> keyValuePair in this.NPO.States)
		{
			ObjectState value = keyValuePair.Value;
			Table table2 = LuaGameObjectScript.ObjectStateToTable(script, keyValuePair.Value);
			table2["id"] = keyValuePair.Key;
			num++;
			table[num] = table2;
		}
		return table;
	}

	// Token: 0x060012D3 RID: 4819 RVA: 0x0007E89C File Offset: 0x0007CA9C
	private static Table ObjectStateToTable(Script script, ObjectState os)
	{
		Table table = new Table(script);
		object key = "name";
		table[key] = TTSUtilities.CleanName(os);
		object key2 = "nickname";
		table[key2] = os.Nickname;
		object key3 = "description";
		table[key3] = os.Description;
		object key4 = "gm_notes";
		table[key4] = os.GMNotes;
		object key5 = "guid";
		table[key5] = os.GUID;
		object key6 = "lua_script";
		table[key6] = os.LuaScript;
		object key7 = "lua_script_state";
		table[key7] = os.LuaScriptState;
		object key8 = "memo";
		table[key8] = (os.Memo ?? "");
		object key9 = "tags";
		table[key9] = LuaGameObjectScript.GetTableFromList<string>(script, os.Tags);
		return table;
	}

	// Token: 0x060012D4 RID: 4820 RVA: 0x0007E973 File Offset: 0x0007CB73
	public int GetStateId()
	{
		return this.NPO.GetSelectedStateId();
	}

	// Token: 0x060012D5 RID: 4821 RVA: 0x0007E980 File Offset: 0x0007CB80
	public LuaGameObjectScript SetState(int id)
	{
		if (this.NPO.GetStatesCount() == -1 || id < 0 || id > this.NPO.GetStatesCount())
		{
			return null;
		}
		LuaGameObjectScript result = null;
		GameObject gameObject = this.NPO.ChangeState(id);
		if (gameObject)
		{
			result = gameObject.GetComponent<LuaGameObjectScript>();
		}
		else
		{
			base.LogError("setState", "This state id does not exist", null);
		}
		return result;
	}

	// Token: 0x060012D6 RID: 4822 RVA: 0x0007E9E1 File Offset: 0x0007CBE1
	[Obsolete("Use getStates() instead.")]
	public int GetStatesCount()
	{
		return this.NPO.GetStatesCount();
	}

	// Token: 0x060012D7 RID: 4823 RVA: 0x0007E9EE File Offset: 0x0007CBEE
	public LuaGameObjectScript ShuffleStates()
	{
		if (this.NPO.GetStatesCount() == -1)
		{
			return null;
		}
		return this.NPO.ShuffleStates().GetComponent<LuaGameObjectScript>();
	}

	// Token: 0x060012D8 RID: 4824 RVA: 0x0007EA10 File Offset: 0x0007CC10
	public LuaBoundsState GetBounds()
	{
		Vector3 offset;
		return new LuaBoundsState(this.NPO.GetBoundsNotNormalized(out offset), offset);
	}

	// Token: 0x060012D9 RID: 4825 RVA: 0x0007EA30 File Offset: 0x0007CC30
	public LuaBoundsState GetBoundsNormalized()
	{
		Vector3 boundsCenterOffset = this.NPO.GetBoundsCenterOffset();
		Bounds bounds = this.NPO.GetBounds();
		Vector3 center = new Vector3(base.transform.position.x - boundsCenterOffset.x, base.transform.position.y - boundsCenterOffset.y, base.transform.position.z - boundsCenterOffset.z);
		Vector3 size = bounds.size;
		return new LuaBoundsState(center, size, boundsCenterOffset);
	}

	// Token: 0x060012DA RID: 4826 RVA: 0x0007EAB0 File Offset: 0x0007CCB0
	public LuaBoundsState GetVisualBoundsNormalized()
	{
		Vector3 rendererBoundsCenterOffset = this.NPO.GetRendererBoundsCenterOffset();
		Bounds rendererBounds = this.NPO.GetRendererBounds();
		Vector3 center = new Vector3(base.transform.position.x - rendererBoundsCenterOffset.x, base.transform.position.y - rendererBoundsCenterOffset.y, base.transform.position.z - rendererBoundsCenterOffset.z);
		Vector3 size = rendererBounds.size;
		return new LuaBoundsState(center, size, rendererBoundsCenterOffset);
	}

	// Token: 0x060012DB RID: 4827 RVA: 0x0007EB2E File Offset: 0x0007CD2E
	public string GetJSON(bool indented = true)
	{
		return Json.GetJson(NetworkSingleton<ManagerPhysicsObject>.Instance.SaveObjectState(this.NPO), indented);
	}

	// Token: 0x060012DC RID: 4828 RVA: 0x0007EB46 File Offset: 0x0007CD46
	public ObjectState GetData()
	{
		return NetworkSingleton<ManagerPhysicsObject>.Instance.SaveObjectState(this.NPO);
	}

	// Token: 0x17000336 RID: 822
	// (get) Token: 0x060012DD RID: 4829 RVA: 0x0007EB58 File Offset: 0x0007CD58
	private GameObject sourceGetComponentGameObject
	{
		get
		{
			if (this._sourceGetComponentGameObject == null)
			{
				this._sourceGetComponentGameObject = base.gameObject;
			}
			return this._sourceGetComponentGameObject;
		}
	}

	// Token: 0x060012DE RID: 4830 RVA: 0x0007EB7A File Offset: 0x0007CD7A
	public LuaGameObjectReference getChild(string name)
	{
		return this.getChild(this.sourceGetComponentGameObject, name);
	}

	// Token: 0x060012DF RID: 4831 RVA: 0x0007EB8C File Offset: 0x0007CD8C
	[MoonSharpHidden]
	public LuaGameObjectReference getChild(GameObject target, string name)
	{
		Transform transform = target.transform.Find(name);
		if (transform)
		{
			return new LuaGameObjectReference(this, transform.gameObject);
		}
		return null;
	}

	// Token: 0x060012E0 RID: 4832 RVA: 0x0007EBBC File Offset: 0x0007CDBC
	public List<LuaGameObjectReference> getChildren()
	{
		return this.getChildren(this.sourceGetComponentGameObject);
	}

	// Token: 0x060012E1 RID: 4833 RVA: 0x0007EBCC File Offset: 0x0007CDCC
	[MoonSharpHidden]
	public List<LuaGameObjectReference> getChildren(GameObject target)
	{
		int childCount = target.transform.childCount;
		List<LuaGameObjectReference> list = new List<LuaGameObjectReference>(childCount);
		for (int i = 0; i < childCount; i++)
		{
			list.Add(new LuaGameObjectReference(this, target.transform.GetChild(i).gameObject));
		}
		return list;
	}

	// Token: 0x060012E2 RID: 4834 RVA: 0x0007EC16 File Offset: 0x0007CE16
	public LuaComponentReference getComponent(string componentName)
	{
		return this.getComponent(this.sourceGetComponentGameObject, componentName);
	}

	// Token: 0x060012E3 RID: 4835 RVA: 0x0007EC28 File Offset: 0x0007CE28
	[MoonSharpHidden]
	public LuaComponentReference getComponent(GameObject target, string componentName)
	{
		Type type = Utilities.GetType(componentName);
		if (type == null)
		{
			return null;
		}
		Component component = target.GetComponent(type);
		if (!component)
		{
			return null;
		}
		if (component.GetType().IsSubclassOf(typeof(MonoBehaviour)))
		{
			base.LogError("getComponent", "Does not work on custom scripts", null);
			return null;
		}
		return new LuaComponentReference(this, component);
	}

	// Token: 0x060012E4 RID: 4836 RVA: 0x0007EC8A File Offset: 0x0007CE8A
	public LuaComponentReference getComponentInChildren(string componentName)
	{
		return this.getComponentInChildren(this.sourceGetComponentGameObject, componentName);
	}

	// Token: 0x060012E5 RID: 4837 RVA: 0x0007EC9C File Offset: 0x0007CE9C
	[MoonSharpHidden]
	public LuaComponentReference getComponentInChildren(GameObject target, string componentName)
	{
		Type type = Utilities.GetType(componentName);
		if (type == null)
		{
			return null;
		}
		Component componentInChildren = target.GetComponentInChildren(type, true);
		if (!componentInChildren)
		{
			return null;
		}
		if (componentInChildren.GetType().IsSubclassOf(typeof(MonoBehaviour)))
		{
			base.LogError("getComponentInChildren", "Does not work on custom scripts", null);
			return null;
		}
		return new LuaComponentReference(this, componentInChildren);
	}

	// Token: 0x060012E6 RID: 4838 RVA: 0x0007ECFF File Offset: 0x0007CEFF
	public List<LuaComponentReference> getComponents(string componentName = null)
	{
		return this.getComponents(this.sourceGetComponentGameObject, componentName);
	}

	// Token: 0x060012E7 RID: 4839 RVA: 0x0007ED0E File Offset: 0x0007CF0E
	private List<Component> GetReusableComponentsList()
	{
		if (this._components == null)
		{
			this._components = new List<Component>();
		}
		else
		{
			this._components.Clear();
		}
		return this._components;
	}

	// Token: 0x060012E8 RID: 4840 RVA: 0x0007ED38 File Offset: 0x0007CF38
	[MoonSharpHidden]
	public List<LuaComponentReference> getComponents(GameObject target, string componentName)
	{
		if (string.IsNullOrEmpty(componentName))
		{
			componentName = "Component";
		}
		Type type = Utilities.GetType(componentName);
		if (type == null)
		{
			return null;
		}
		List<LuaComponentReference> list = new List<LuaComponentReference>();
		List<Component> reusableComponentsList = this.GetReusableComponentsList();
		target.GetComponents(type, reusableComponentsList);
		for (int i = 0; i < reusableComponentsList.Count; i++)
		{
			Component component = reusableComponentsList[i];
			if (!component.GetType().IsSubclassOf(typeof(MonoBehaviour)))
			{
				list.Add(new LuaComponentReference(this, component));
			}
		}
		reusableComponentsList.Clear();
		return list;
	}

	// Token: 0x060012E9 RID: 4841 RVA: 0x0007EDC2 File Offset: 0x0007CFC2
	public List<LuaComponentReference> getComponentsInChildren(string componentName = null)
	{
		return this.getComponentsInChildren(this.sourceGetComponentGameObject, componentName);
	}

	// Token: 0x060012EA RID: 4842 RVA: 0x0007EDD4 File Offset: 0x0007CFD4
	[MoonSharpHidden]
	public List<LuaComponentReference> getComponentsInChildren(GameObject target, string componentName)
	{
		if (string.IsNullOrEmpty(componentName))
		{
			componentName = "Component";
		}
		Type type = Utilities.GetType(componentName);
		if (type == null)
		{
			return null;
		}
		List<LuaComponentReference> list = new List<LuaComponentReference>();
		foreach (Component component in target.GetComponentsInChildren(type, true))
		{
			if (!component.GetType().IsSubclassOf(typeof(MonoBehaviour)))
			{
				list.Add(new LuaComponentReference(this, component));
			}
		}
		return list;
	}

	// Token: 0x060012EB RID: 4843 RVA: 0x0007EE4C File Offset: 0x0007D04C
	[MoonSharpHidden]
	public DynValue getComponentVar(Component comp, string varName, Script script)
	{
		VarInfo var = comp.GetType().GetVar(varName);
		if (var == null || !LuaGameObjectScript.WhiteListedGetComponentTypes.Contains(var.VarType))
		{
			return null;
		}
		object value = var.GetValue(comp);
		if (LuaCustomConverter.IsVectorOrColor(value.GetType()))
		{
			return DynValue.NewTable(LuaCustomConverter.GetTable(value, script));
		}
		return DynValue.FromObject(script, value);
	}

	// Token: 0x060012EC RID: 4844 RVA: 0x0007EEA8 File Offset: 0x0007D0A8
	[MoonSharpHidden]
	public Dictionary<string, string> getComponentVars(Component comp)
	{
		Dictionary<string, string> dictionary = new Dictionary<string, string>();
		foreach (VarInfo varInfo in comp.GetType().GetVars())
		{
			if (LuaGameObjectScript.WhiteListedGetComponentTypes.Contains(varInfo.VarType))
			{
				dictionary.Add(varInfo.Name, varInfo.VarType.ToString());
			}
		}
		return dictionary;
	}

	// Token: 0x060012ED RID: 4845 RVA: 0x0007EF04 File Offset: 0x0007D104
	[MoonSharpHidden]
	public bool setComponentVar(Component comp, string varName, object value)
	{
		Type type = comp.GetType();
		if (type.IsSubclassOf(typeof(MonoBehaviour)))
		{
			return false;
		}
		if (comp.gameObject == this.sourceGetComponentGameObject && type == typeof(Transform))
		{
			base.LogError("Component.set", "Don't set the transform directly on the root GameObject with Component", null);
			return false;
		}
		VarInfo var = type.GetVar(varName);
		if (var == null || !LuaGameObjectScript.WhiteListedGetComponentTypes.Contains(var.VarType))
		{
			return false;
		}
		Type varType = var.VarType;
		object obj = value;
		if (Network.isServer && LuaCustomConverter.IsVectorOrColor(varType))
		{
			obj = LuaCustomConverter.Parse(varType, value);
		}
		if (obj is double)
		{
			obj = Convert.ToSingle(obj);
		}
		var.SetValue(comp, obj);
		this.ComponentValues.SetValue(new LuaGameObjectScript.ComponentVariable(comp, var), obj);
		if (Network.isServer)
		{
			LuaGameObjectScript.RPCComponentVariable arg = new LuaGameObjectScript.RPCComponentVariable(this.sourceGetComponentGameObject, comp, var);
			if (varType == typeof(float) || varType == typeof(int))
			{
				base.networkView.RPC<LuaGameObjectScript.RPCComponentVariable, float>(RPCTarget.Others, new Action<LuaGameObjectScript.RPCComponentVariable, float>(this.RPCSetComponentVar), arg, (float)obj);
			}
			else if (varType == typeof(bool))
			{
				base.networkView.RPC<LuaGameObjectScript.RPCComponentVariable, bool>(RPCTarget.Others, new Action<LuaGameObjectScript.RPCComponentVariable, bool>(this.RPCSetComponentVar), arg, (bool)obj);
			}
			else if (varType == typeof(Vector3))
			{
				base.networkView.RPC<LuaGameObjectScript.RPCComponentVariable, Vector3>(RPCTarget.Others, new Action<LuaGameObjectScript.RPCComponentVariable, Vector3>(this.RPCSetComponentVar), arg, (Vector3)obj);
			}
			else if (varType == typeof(Color))
			{
				base.networkView.RPC<LuaGameObjectScript.RPCComponentVariable, Color>(RPCTarget.Others, new Action<LuaGameObjectScript.RPCComponentVariable, Color>(this.RPCSetComponentVar), arg, (Color)obj);
			}
		}
		return true;
	}

	// Token: 0x060012EE RID: 4846 RVA: 0x0007F0D0 File Offset: 0x0007D2D0
	[MoonSharpHidden]
	private void setComponentVar(LuaGameObjectScript.RPCComponentVariable rpcVar, object value)
	{
		Wait.Condition(delegate
		{
			Component comp;
			string varName;
			if (rpcVar.GetComponentVariable(this.sourceGetComponentGameObject.transform, out comp, out varName))
			{
				this.setComponentVar(comp, varName, value);
			}
		}, () => !this.loading_custom, float.PositiveInfinity, null);
	}

	// Token: 0x060012EF RID: 4847 RVA: 0x0007F11C File Offset: 0x0007D31C
	[MoonSharpHidden]
	[Remote(Permission.Server, SendType.ReliableNoDelay, null, SerializationMethod.Default)]
	private void RPCSetComponentVar(LuaGameObjectScript.RPCComponentVariable rpcVar, float value)
	{
		this.setComponentVar(rpcVar, value);
	}

	// Token: 0x060012F0 RID: 4848 RVA: 0x0007F12B File Offset: 0x0007D32B
	[Remote(Permission.Server, SendType.ReliableNoDelay, null, SerializationMethod.Default)]
	private void RPCSetComponentVar(LuaGameObjectScript.RPCComponentVariable rpcVar, bool value)
	{
		this.setComponentVar(rpcVar, value);
	}

	// Token: 0x060012F1 RID: 4849 RVA: 0x0007F13A File Offset: 0x0007D33A
	[Remote(Permission.Server, SendType.ReliableNoDelay, null, SerializationMethod.Default)]
	private void RPCSetComponentVar(LuaGameObjectScript.RPCComponentVariable rpcVar, Vector3 value)
	{
		this.setComponentVar(rpcVar, value);
	}

	// Token: 0x060012F2 RID: 4850 RVA: 0x0007F149 File Offset: 0x0007D349
	[Remote(Permission.Server, SendType.ReliableNoDelay, null, SerializationMethod.Default)]
	private void RPCSetComponentVar(LuaGameObjectScript.RPCComponentVariable rpcVar, Color value)
	{
		this.setComponentVar(rpcVar, value);
	}

	// Token: 0x060012F3 RID: 4851 RVA: 0x0007F158 File Offset: 0x0007D358
	[MoonSharpHidden]
	public DynValue callComponentFunc(string componentName, string funcName, params object[] parameters)
	{
		Component component = this.sourceGetComponentGameObject.GetComponent(Utilities.GetType(componentName));
		if (!component)
		{
			return null;
		}
		if (component.GetType().IsSubclassOf(typeof(MonoBehaviour)))
		{
			base.LogError("getComponentVar", "Does not work on custom scripts", null);
			return null;
		}
		MethodInfo method = component.GetType().GetMethod(funcName);
		if (method == null)
		{
			return null;
		}
		object obj = method.Invoke(component, parameters);
		if (LuaCustomConverter.IsVectorOrColor(obj.GetType()))
		{
			return DynValue.NewTable(LuaCustomConverter.GetTable(obj, this.lua));
		}
		return DynValue.FromObject(this.lua, obj);
	}

	// Token: 0x060012F4 RID: 4852 RVA: 0x0007F1F7 File Offset: 0x0007D3F7
	public List<LuaMaterialReference> getMaterials()
	{
		return this.getMaterials(base.gameObject);
	}

	// Token: 0x060012F5 RID: 4853 RVA: 0x0007F208 File Offset: 0x0007D408
	[MoonSharpHidden]
	public List<LuaMaterialReference> getMaterials(GameObject go)
	{
		List<LuaMaterialReference> list = new List<LuaMaterialReference>();
		LuaGameObjectReference game_object = new LuaGameObjectReference(this, go);
		List<Renderer> renderers = this.NPO.Renderers;
		for (int i = 0; i < renderers.Count; i++)
		{
			if (!(renderers[i].gameObject != go))
			{
				Material[] sharedMaterials = renderers[i].sharedMaterials;
				for (int j = 0; j < sharedMaterials.Length; j++)
				{
					list.Add(new LuaMaterialReference(this, sharedMaterials[j], game_object));
				}
			}
		}
		return list;
	}

	// Token: 0x060012F6 RID: 4854 RVA: 0x0007F288 File Offset: 0x0007D488
	public List<LuaMaterialReference> getMaterialsInChildren()
	{
		return this.getMaterialsInChildren(base.gameObject);
	}

	// Token: 0x060012F7 RID: 4855 RVA: 0x0007F298 File Offset: 0x0007D498
	[MoonSharpHidden]
	public List<LuaMaterialReference> getMaterialsInChildren(GameObject go)
	{
		List<LuaMaterialReference> list = new List<LuaMaterialReference>();
		LuaGameObjectReference game_object = new LuaGameObjectReference(this, go);
		List<Renderer> renderers = this.NPO.Renderers;
		for (int i = 0; i < renderers.Count; i++)
		{
			Transform transform = renderers[i].gameObject.transform;
			bool flag = false;
			while (transform != null)
			{
				if (transform == go.transform)
				{
					flag = true;
					break;
				}
				transform = transform.parent;
			}
			if (flag)
			{
				Material[] sharedMaterials = renderers[i].sharedMaterials;
				for (int j = 0; j < sharedMaterials.Length; j++)
				{
					list.Add(new LuaMaterialReference(this, sharedMaterials[j], game_object));
				}
			}
		}
		return list;
	}

	// Token: 0x060012F8 RID: 4856 RVA: 0x0007F34C File Offset: 0x0007D54C
	[MoonSharpHidden]
	public Dictionary<string, string> getMaterialVars(Material mat)
	{
		Dictionary<string, string> dictionary = new Dictionary<string, string>();
		Shader shader = mat.shader;
		int propertyCount = shader.GetPropertyCount();
		for (int i = 0; i < propertyCount; i++)
		{
			string propertyName = shader.GetPropertyName(i);
			ShaderPropertyType propertyType = shader.GetPropertyType(i);
			if (propertyType == ShaderPropertyType.Texture)
			{
				dictionary.Add(propertyName + "Tiling", "Vector");
				dictionary.Add(propertyName + "Offset", "Vector");
			}
			else
			{
				dictionary.Add(propertyName, propertyType.ToString());
			}
		}
		return dictionary;
	}

	// Token: 0x060012F9 RID: 4857 RVA: 0x0007F3D8 File Offset: 0x0007D5D8
	private static bool IsValidMaterialVar(Material mat, string varName)
	{
		string name;
		bool flag;
		return mat.HasProperty(varName) || (LuaGameObjectScript.GetTexturePropertyInfo(mat, varName, out name, out flag) && mat.HasProperty(name));
	}

	// Token: 0x060012FA RID: 4858 RVA: 0x0007F406 File Offset: 0x0007D606
	private static bool GetTexturePropertyInfo(Material mat, string varName, out string textureName, out bool tiling)
	{
		tiling = varName.EndsWith("Tiling");
		if (tiling || varName.EndsWith("Offset"))
		{
			textureName = varName.Substring(0, varName.Length - 6);
			return true;
		}
		textureName = null;
		return false;
	}

	// Token: 0x060012FB RID: 4859 RVA: 0x0007F440 File Offset: 0x0007D640
	private static ShaderPropertyType GetPropertyType(Material mat, string varName)
	{
		for (int i = 0; i < mat.shader.GetPropertyCount(); i++)
		{
			if (!(mat.shader.GetPropertyName(i) != varName))
			{
				return mat.shader.GetPropertyType(i);
			}
		}
		return ShaderPropertyType.Float;
	}

	// Token: 0x060012FC RID: 4860 RVA: 0x0007F488 File Offset: 0x0007D688
	[MoonSharpHidden]
	public DynValue getMaterialValue(Material mat, string varName, Script script)
	{
		if (!LuaGameObjectScript.IsValidMaterialVar(mat, varName))
		{
			return null;
		}
		string name;
		bool flag;
		if (LuaGameObjectScript.GetTexturePropertyInfo(mat, varName, out name, out flag))
		{
			return DynValue.NewTable(LuaCustomConverter.GetTable(flag ? mat.GetTextureScale(name) : mat.GetTextureOffset(name), script));
		}
		switch (LuaGameObjectScript.GetPropertyType(mat, varName))
		{
		case ShaderPropertyType.Color:
			return DynValue.NewTable(LuaCustomConverter.GetTable(mat.GetColor(varName), script));
		case ShaderPropertyType.Vector:
			return DynValue.NewTable(LuaCustomConverter.GetTable(mat.GetVector(varName), script));
		case ShaderPropertyType.Float:
		case ShaderPropertyType.Range:
			return DynValue.FromObject(script, mat.GetFloat(varName));
		default:
			return null;
		}
	}

	// Token: 0x060012FD RID: 4861 RVA: 0x0007F528 File Offset: 0x0007D728
	[MoonSharpHidden]
	public bool setMaterialValue(Material mat, string varName, object value)
	{
		if (!LuaGameObjectScript.IsValidMaterialVar(mat, varName))
		{
			return false;
		}
		LuaGameObjectScript.RPCMaterialVariable arg = new LuaGameObjectScript.RPCMaterialVariable(this.NPO.Renderers, mat, varName);
		LuaGameObjectScript.MaterialVariable key = new LuaGameObjectScript.MaterialVariable
		{
			material = mat,
			varName = varName
		};
		string name;
		bool flag;
		if (LuaGameObjectScript.GetTexturePropertyInfo(mat, varName, out name, out flag))
		{
			Vector4 vector;
			if (value is Vector4)
			{
				vector = (Vector4)value;
			}
			else
			{
				vector = LuaCustomConverter.Parse<Vector4>((Table)value);
			}
			if (flag)
			{
				mat.SetTextureScale(name, vector);
			}
			else
			{
				mat.SetTextureOffset(name, vector);
			}
			if (Network.isServer)
			{
				this.MaterialValues[key] = value;
				base.networkView.RPC<LuaGameObjectScript.RPCMaterialVariable, Vector4>(RPCTarget.Others, new Action<LuaGameObjectScript.RPCMaterialVariable, Vector4>(this.RPCSetMaterialVar), arg, vector);
			}
		}
		else
		{
			switch (LuaGameObjectScript.GetPropertyType(mat, varName))
			{
			case ShaderPropertyType.Color:
			{
				Color color;
				if (value is Color)
				{
					color = (Color)value;
				}
				else
				{
					color = LuaCustomConverter.Parse<Color>((Table)value);
				}
				mat.SetColor(varName, color);
				if (Network.isServer)
				{
					this.MaterialValues[key] = value;
					base.networkView.RPC<LuaGameObjectScript.RPCMaterialVariable, Color>(RPCTarget.Others, new Action<LuaGameObjectScript.RPCMaterialVariable, Color>(this.RPCSetMaterialVar), arg, color);
				}
				break;
			}
			case ShaderPropertyType.Vector:
			{
				Vector4 vector2;
				if (value is Vector4)
				{
					vector2 = (Vector4)value;
				}
				else
				{
					vector2 = LuaCustomConverter.Parse<Vector4>((Table)value);
				}
				mat.SetVector(varName, vector2);
				if (Network.isServer)
				{
					this.MaterialValues[key] = value;
					base.networkView.RPC<LuaGameObjectScript.RPCMaterialVariable, Vector4>(RPCTarget.Others, new Action<LuaGameObjectScript.RPCMaterialVariable, Vector4>(this.RPCSetMaterialVar), arg, vector2);
				}
				break;
			}
			case ShaderPropertyType.Float:
			case ShaderPropertyType.Range:
			{
				float num;
				if (value is float)
				{
					num = (float)value;
				}
				else
				{
					num = Convert.ToSingle(value);
				}
				mat.SetFloat(varName, num);
				if (Network.isServer)
				{
					this.MaterialValues[key] = value;
					base.networkView.RPC<LuaGameObjectScript.RPCMaterialVariable, float>(RPCTarget.Others, new Action<LuaGameObjectScript.RPCMaterialVariable, float>(this.RPCSetMaterialVar), arg, num);
				}
				break;
			}
			}
		}
		return true;
	}

	// Token: 0x060012FE RID: 4862 RVA: 0x0007F738 File Offset: 0x0007D938
	[MoonSharpHidden]
	private void setMaterialValue(LuaGameObjectScript.RPCMaterialVariable rpcMaterialVariable, object value)
	{
		Wait.Condition(delegate
		{
			Material mat;
			string varName;
			if (rpcMaterialVariable.GetMaterialVariable(this.sourceGetComponentGameObject.transform, out mat, out varName))
			{
				this.setMaterialValue(mat, varName, value);
			}
		}, () => !this.loading_custom, float.PositiveInfinity, null);
	}

	// Token: 0x060012FF RID: 4863 RVA: 0x0007F784 File Offset: 0x0007D984
	[Remote(Permission.Server, SendType.ReliableNoDelay, null, SerializationMethod.Default)]
	private void RPCSetMaterialVar(LuaGameObjectScript.RPCMaterialVariable rpcMaterialVariable, float value)
	{
		this.setMaterialValue(rpcMaterialVariable, value);
	}

	// Token: 0x06001300 RID: 4864 RVA: 0x0007F793 File Offset: 0x0007D993
	[Remote(Permission.Server, SendType.ReliableNoDelay, null, SerializationMethod.Default)]
	private void RPCSetMaterialVar(LuaGameObjectScript.RPCMaterialVariable rpcMaterialVariable, Vector4 value)
	{
		this.setMaterialValue(rpcMaterialVariable, value);
	}

	// Token: 0x06001301 RID: 4865 RVA: 0x0007F7A2 File Offset: 0x0007D9A2
	[Remote(Permission.Server, SendType.ReliableNoDelay, null, SerializationMethod.Default)]
	private void RPCSetMaterialVar(LuaGameObjectScript.RPCMaterialVariable rpcMaterialVariable, Color value)
	{
		this.setMaterialValue(rpcMaterialVariable, value);
	}

	// Token: 0x06001302 RID: 4866 RVA: 0x0007F7B4 File Offset: 0x0007D9B4
	public List<LuaGameObjectScript.LuaVectorLine> GetVectorLines()
	{
		List<VectorLineState> lineStates = NetworkSingleton<ToolVector>.Instance.GetLineStates(this.NPO);
		if (lineStates == null)
		{
			return null;
		}
		List<LuaGameObjectScript.LuaVectorLine> list = new List<LuaGameObjectScript.LuaVectorLine>();
		foreach (VectorLineState vectorLineState in lineStates)
		{
			List<Vector3> list2 = new List<Vector3>(vectorLineState.points3.Count);
			foreach (VectorState vectorState in vectorLineState.points3)
			{
				list2.Add(vectorState.ToVector());
			}
			List<LuaGameObjectScript.LuaVectorLine> list3 = list;
			LuaGameObjectScript.LuaVectorLine luaVectorLine = new LuaGameObjectScript.LuaVectorLine();
			luaVectorLine.points = list2;
			luaVectorLine.color = vectorLineState.color.ToColour();
			luaVectorLine.thickness = vectorLineState.thickness;
			VectorLineState vectorLineState2 = vectorLineState;
			luaVectorLine.rotation = ((vectorLineState2.rotation != null) ? vectorLineState2.rotation.GetValueOrDefault().ToVector() : Vector3.zero);
			luaVectorLine.loop = (vectorLineState.loop ?? false);
			luaVectorLine.square = (vectorLineState.square ?? false);
			list3.Add(luaVectorLine);
		}
		return list;
	}

	// Token: 0x06001303 RID: 4867 RVA: 0x0007F934 File Offset: 0x0007DB34
	public bool SetVectorLines(List<LuaGameObjectScript.LuaVectorLine> luaLines)
	{
		if (luaLines == null)
		{
			luaLines = new List<LuaGameObjectScript.LuaVectorLine>();
		}
		List<VectorLineState> list = new List<VectorLineState>();
		foreach (LuaGameObjectScript.LuaVectorLine luaVectorLine in luaLines)
		{
			List<VectorState> list2 = new List<VectorState>(luaVectorLine.points.Count);
			foreach (Vector3 vector in luaVectorLine.points)
			{
				list2.Add(new VectorState(vector));
			}
			VectorState? rotation = null;
			if (luaVectorLine.rotation != Vector3.zero)
			{
				rotation = new VectorState?(new VectorState(luaVectorLine.rotation));
			}
			list.Add(new VectorLineState
			{
				points3 = list2,
				color = new ColourState(luaVectorLine.color),
				thickness = luaVectorLine.thickness,
				rotation = rotation,
				loop = (luaVectorLine.loop ? new bool?(true) : null),
				square = (luaVectorLine.square ? new bool?(true) : null)
			});
		}
		NetworkSingleton<ToolVector>.Instance.RemoveLinesAttached(this.NPO);
		NetworkSingleton<ToolVector>.Instance.AddLineStates(list, this.NPO);
		return true;
	}

	// Token: 0x06001304 RID: 4868 RVA: 0x0007FAD0 File Offset: 0x0007DCD0
	public bool Drop()
	{
		int heldByPlayerID = this.NPO.HeldByPlayerID;
		this.NPO.Drop();
		return heldByPlayerID >= 0;
	}

	// Token: 0x06001305 RID: 4869 RVA: 0x0007FAEE File Offset: 0x0007DCEE
	private NetworkView GetNPONetworkView()
	{
		if (this.NPO)
		{
			return this.NPO.networkView;
		}
		return null;
	}

	// Token: 0x06001306 RID: 4870 RVA: 0x0007FB0C File Offset: 0x0007DD0C
	public List<LuaGameObjectScript.LuaDecal> GetDecals()
	{
		List<DecalState> decalStates = NetworkSingleton<DecalManager>.Instance.GetDecalStates(this.GetNPONetworkView());
		if (decalStates == null)
		{
			return null;
		}
		List<LuaGameObjectScript.LuaDecal> list = new List<LuaGameObjectScript.LuaDecal>();
		foreach (DecalState decalState in decalStates)
		{
			list.Add(new LuaGameObjectScript.LuaDecal
			{
				name = decalState.CustomDecal.Name,
				url = decalState.CustomDecal.ImageURL,
				position = decalState.Transform.Position(),
				rotation = decalState.Transform.Rotation(),
				scale = decalState.Transform.Scale()
			});
		}
		return list;
	}

	// Token: 0x06001307 RID: 4871 RVA: 0x0007FBD0 File Offset: 0x0007DDD0
	public bool SetDecals(List<LuaGameObjectScript.LuaDecal> decals)
	{
		if (decals == null)
		{
			base.LogError("SetDecals", "Decal table is empty!", null);
			return false;
		}
		List<DecalState> list = new List<DecalState>();
		foreach (LuaGameObjectScript.LuaDecal luaDecal in decals)
		{
			if (string.IsNullOrEmpty(luaDecal.url))
			{
				base.LogError("SetDecals", "url was null or an empty string", null);
				return false;
			}
			if (string.IsNullOrEmpty(luaDecal.name))
			{
				base.LogError("SetDecals", "name was null or an empty string", null);
				return false;
			}
			DecalState item = new DecalState
			{
				CustomDecal = new CustomDecalState
				{
					Name = luaDecal.name,
					ImageURL = luaDecal.url,
					Size = 1f
				},
				Transform = new TransformState(luaDecal.position, luaDecal.rotation, luaDecal.scale)
			};
			list.Add(item);
		}
		NetworkSingleton<DecalManager>.Instance.SetDecalStates(list, this.GetNPONetworkView());
		return true;
	}

	// Token: 0x06001308 RID: 4872 RVA: 0x0007FCF4 File Offset: 0x0007DEF4
	public bool AddDecal(LuaGameObjectScript.LuaDecal decal)
	{
		if (string.IsNullOrEmpty(decal.url))
		{
			base.LogError("AddDecal", "url was null or an empty string", null);
			return false;
		}
		if (string.IsNullOrEmpty(decal.name))
		{
			base.LogError("AddDecal", "name was null or an empty string", null);
			return false;
		}
		CustomDecalState selectedDecal = new CustomDecalState
		{
			Name = decal.name,
			ImageURL = decal.url,
			Size = 1f
		};
		NetworkSingleton<DecalManager>.Instance.SelectedDecal = selectedDecal;
		DecalState decalState = new DecalState
		{
			CustomDecal = NetworkSingleton<DecalManager>.Instance.SelectedDecal,
			Transform = new TransformState(decal.position, decal.rotation, decal.scale)
		};
		NetworkSingleton<DecalManager>.Instance.AddDecal(decalState, this.GetNPONetworkView());
		return true;
	}

	// Token: 0x06001309 RID: 4873 RVA: 0x0007FDBA File Offset: 0x0007DFBA
	public bool AddAttachment(LuaGameObjectScript luaGameObject)
	{
		if (this.NPO.tableScript || luaGameObject.NPO.tableScript)
		{
			return false;
		}
		this.NPO.childSpawner.AddChild(luaGameObject.NPO);
		return true;
	}

	// Token: 0x0600130A RID: 4874 RVA: 0x0007FDF9 File Offset: 0x0007DFF9
	public LuaGameObjectScript RemoveAttachment(int index)
	{
		return this.NPO.childSpawner.RemoveChild(index).luaGameObjectScript;
	}

	// Token: 0x0600130B RID: 4875 RVA: 0x0007FE11 File Offset: 0x0007E011
	public bool DestroyAttachment(int index)
	{
		this.NPO.childSpawner.DestroyChild(index);
		return true;
	}

	// Token: 0x0600130C RID: 4876 RVA: 0x0007FE28 File Offset: 0x0007E028
	public Table GetAttachments(Script script)
	{
		List<ObjectState> children = this.NPO.childSpawner.GetChildren();
		Table table = new Table(script);
		for (int i = 0; i < children.Count; i++)
		{
			ObjectState os = children[i];
			Table table2 = LuaGameObjectScript.ObjectStateToTable(script, os);
			table2["index"] = i;
			table[i + 1] = table2;
		}
		return table;
	}

	// Token: 0x0600130D RID: 4877 RVA: 0x0007FE92 File Offset: 0x0007E092
	public List<LuaGameObjectScript> RemoveAttachments()
	{
		return this.NPO.childSpawner.RemoveChildren().ToLGOS();
	}

	// Token: 0x0600130E RID: 4878 RVA: 0x0007FEA9 File Offset: 0x0007E0A9
	public bool DestroyAttachments()
	{
		this.NPO.childSpawner.DestroyChildren();
		return true;
	}

	// Token: 0x0600130F RID: 4879 RVA: 0x0007FEBC File Offset: 0x0007E0BC
	public bool CreateButton(Dictionary<string, object> dict)
	{
		if (dict != null)
		{
			LuaUIButtonState @object = LuaCustomConverter.GetObject<LuaUIButtonState>(dict, null);
			if (@object != null)
			{
				if (string.IsNullOrEmpty(@object.click_function))
				{
					base.LogError("createButton", "click_function was null or an empty string", null);
					return false;
				}
				base.networkView.RPC<LuaUIButtonState>(RPCTarget.All, new Action<LuaUIButtonState>(this.RPCCreateButton), @object);
				return true;
			}
		}
		return false;
	}

	// Token: 0x06001310 RID: 4880 RVA: 0x0007FF14 File Offset: 0x0007E114
	public bool CreateInput(Dictionary<string, object> dict)
	{
		if (dict != null)
		{
			LuaUIInputState @object = LuaCustomConverter.GetObject<LuaUIInputState>(dict, null);
			if (@object != null)
			{
				if (string.IsNullOrEmpty(@object.input_function))
				{
					base.LogError("createInput", "input_function was null or an empty string", null);
					return false;
				}
				try
				{
					base.networkView.RPC<LuaUIInputState>(RPCTarget.All, new Action<LuaUIInputState>(this.RPCCreateInput), @object);
				}
				catch (Exception exception)
				{
					Debug.LogException(exception);
				}
				return true;
			}
		}
		return false;
	}

	// Token: 0x06001311 RID: 4881 RVA: 0x0007FF88 File Offset: 0x0007E188
	[MoonSharpHidden]
	private void DoUI(LuaUIInteractState luaInteractState, Action<LuaUIInteractState, LuaGameObjectScript> func)
	{
		base.enabled = true;
		LuaGameObjectScript function_owner = luaInteractState.function_owner;
		func(luaInteractState, function_owner);
	}

	// Token: 0x06001312 RID: 4882 RVA: 0x0007FFAB File Offset: 0x0007E1AB
	[Remote(Permission.Server)]
	[MoonSharpHidden]
	private void RPCCreateButton(LuaUIButtonState luaButtonState)
	{
		this.DoUI(luaButtonState, new Action<LuaUIInteractState, LuaGameObjectScript>(this.ActionCreateUI));
	}

	// Token: 0x06001313 RID: 4883 RVA: 0x0007FFAB File Offset: 0x0007E1AB
	[Remote(Permission.Server)]
	[MoonSharpHidden]
	private void RPCCreateInput(LuaUIInputState luaInputState)
	{
		this.DoUI(luaInputState, new Action<LuaUIInteractState, LuaGameObjectScript>(this.ActionCreateUI));
	}

	// Token: 0x06001314 RID: 4884 RVA: 0x0007FFC0 File Offset: 0x0007E1C0
	[MoonSharpHidden]
	private void ActionCreateUI(LuaUIInteractState luaInteractState, LuaGameObjectScript functionOwner)
	{
		Transform transform = base.transform.Find("LuaPanel");
		GameObject gameObject;
		if (transform == null)
		{
			gameObject = new GameObject();
			gameObject.transform.parent = base.transform;
			gameObject.name = "LuaPanel";
			gameObject.transform.name = "LuaPanel";
			gameObject.transform.localPosition = Vector3.zero;
			gameObject.transform.localEulerAngles = new Vector3(-90f, 180f, 0f);
			gameObject.transform.localScale = Vector3.one;
			gameObject.layer = 17;
			gameObject.AddComponent<UIPanel>();
		}
		else
		{
			gameObject = transform.gameObject;
		}
		UIPanel component = gameObject.GetComponent<UIPanel>();
		if (!this.NPO.UIPanels.Contains(component))
		{
			this.NPO.UIPanels.Add(component);
		}
		this.NPO.UpdateVisiblity(false);
		if (luaInteractState is LuaUIButtonState)
		{
			GameObject gameObject2 = UnityEngine.Object.Instantiate<GameObject>(Resources.Load<GameObject>("LuaButton"));
			gameObject2.transform.parent = gameObject.transform;
			GameObject gameObject3 = UnityEngine.Object.Instantiate<GameObject>(Resources.Load<GameObject>("LuaButtonLabel"));
			gameObject3.transform.parent = gameObject2.transform;
			gameObject3.transform.Reset();
			this.SetUI(gameObject2, gameObject3, functionOwner, luaInteractState);
			luaInteractState.index = this.luaButtonsHolder.Count;
			LuaGameObjectScript.LuaUIButtonHolder item = new LuaGameObjectScript.LuaUIButtonHolder((LuaUIButtonState)luaInteractState, gameObject2, gameObject3);
			this.luaButtonsHolder.Add(item);
		}
		else if (luaInteractState is LuaUIInputState)
		{
			GameObject gameObject4 = UnityEngine.Object.Instantiate<GameObject>(Resources.Load<GameObject>("LuaInput"));
			gameObject4.transform.parent = gameObject.transform;
			GameObject gameObject5 = gameObject4.GetComponentInChildren<UILabel>().gameObject;
			this.SetUI(gameObject4, gameObject5, functionOwner, luaInteractState);
			luaInteractState.index = this.luaInputsHolder.Count;
			LuaGameObjectScript.LuaUIInputHolder item2 = new LuaGameObjectScript.LuaUIInputHolder((LuaUIInputState)luaInteractState, gameObject4, gameObject5);
			this.luaInputsHolder.Add(item2);
			this.CalculateTabInputs();
		}
		this.NPO.ResetBounds();
	}

	// Token: 0x06001315 RID: 4885 RVA: 0x000801C8 File Offset: 0x0007E3C8
	[MoonSharpHidden]
	private void SetUI(GameObject button, GameObject label, LuaGameObjectScript functionOwner, LuaUIInteractState luaInteractState)
	{
		UISprite component = button.GetComponent<UISprite>();
		button.transform.localPosition = new Vector3(luaInteractState.position.x, luaInteractState.position.z, luaInteractState.position.y);
		button.transform.localRotation = Quaternion.Euler(new Vector3(luaInteractState.rotation.x, luaInteractState.rotation.z, luaInteractState.rotation.y));
		button.transform.localRotation = button.transform.localRotation * Quaternion.Euler(new Vector3(180f, 0f, 0f));
		button.transform.localScale = new Vector3(0.00208f, 0.00208f, 0.00208f) * 3f;
		button.transform.localScale = new Vector3(button.transform.localScale.x * luaInteractState.scale.x, button.transform.localScale.y * luaInteractState.scale.z, button.transform.localScale.z * luaInteractState.scale.y);
		int num = (int)(luaInteractState.width / 3f);
		int num2 = (int)(luaInteractState.height / 3f);
		component.type = (((num > 60 || num2 > 60) && num2 != 0 && num != 0) ? UIBasicSprite.Type.Sliced : UIBasicSprite.Type.Simple);
		component.width = num;
		component.height = num2;
		component.enabled = (num != 0 || num2 != 0);
		component.color = luaInteractState.color;
		if (luaInteractState.color.a == 0f)
		{
			Color color = component.color;
			color.a = 0.01f;
			component.color = color;
		}
		UIButton uibutton = button.AddMissingComponent<UIButton>();
		float num3 = 0.2f;
		float num4 = 0.4f;
		float num5 = 0.4f;
		Color color2 = component.color;
		uibutton.defaultColor = color2;
		if (luaInteractState.hover_color.r == 0f && luaInteractState.hover_color.g == 0f && luaInteractState.hover_color.b == 0f && luaInteractState.hover_color.a == 0f)
		{
			uibutton.hover = new Color((color2.r > num5) ? (color2.r - num3) : (color2.r + num3), (color2.g > num5) ? (color2.g - num3) : (color2.g + num3), (color2.b > num5) ? (color2.b - num3) : (color2.b + num3), color2.a);
		}
		else
		{
			uibutton.hover = luaInteractState.hover_color;
		}
		if (luaInteractState.press_color.r == 0f && luaInteractState.press_color.g == 0f && luaInteractState.press_color.b == 0f && luaInteractState.press_color.a == 0f)
		{
			uibutton.pressed = new Color((color2.r > num5) ? (color2.r - num4) : (color2.r + num4), (color2.g > num5) ? (color2.g - num4) : (color2.g + num4), (color2.b > num5) ? (color2.b - num4) : (color2.b + num4), color2.a);
		}
		else
		{
			uibutton.pressed = luaInteractState.press_color;
		}
		UITooltipScript uitooltipScript = button.GetComponent<UITooltipScript>();
		if (string.IsNullOrEmpty(luaInteractState.tooltip))
		{
			UnityEngine.Object.Destroy(uitooltipScript);
		}
		else
		{
			if (uitooltipScript == null)
			{
				uitooltipScript = button.AddComponent<UITooltipScript>();
			}
			uitooltipScript.Tooltip = luaInteractState.tooltip;
		}
		UILabel component2 = label.GetComponent<UILabel>();
		component2.text = luaInteractState.label;
		component2.fontSize = (int)(luaInteractState.font_size / 3f);
		component2.transform.localScale = Vector3.one;
		component2.color = luaInteractState.font_color;
		component2.alignment = (NGUIText.Alignment)(luaInteractState.alignment - 1);
		if (luaInteractState is LuaUIButtonState)
		{
			LuaButton luaButton = button.AddMissingComponent<LuaButton>();
			luaButton.buttonState = (LuaUIButtonState)luaInteractState;
			luaButton.luaObject = this;
			return;
		}
		LuaUIInputState luaUIInputState;
		if ((luaUIInputState = (luaInteractState as LuaUIInputState)) != null)
		{
			UIInput component3 = button.GetComponent<UIInput>();
			Color color3 = new Color(luaInteractState.font_color.r, luaInteractState.font_color.g, luaInteractState.font_color.b, luaInteractState.font_color.a * 0.8f);
			component2.color = color3;
			component3.defaultColor = color3;
			component3.defaultText = luaInteractState.label;
			component3.validation = (UIInput.Validation)(luaUIInputState.validation - 1);
			component3.TabIsFourSpaces = (luaUIInputState.tab == 3);
			component3.activeTextColor = luaInteractState.font_color;
			component3.value = luaUIInputState.value;
			LuaInput luaInput = button.AddMissingComponent<LuaInput>();
			luaInput.inputState = luaUIInputState;
			luaInput.luaObject = this;
		}
	}

	// Token: 0x06001316 RID: 4886 RVA: 0x000806F8 File Offset: 0x0007E8F8
	public Table GetButtons(Script script)
	{
		if (this.luaButtonsHolder.Count == 0)
		{
			return null;
		}
		LuaUIButtonState[] array = new LuaUIButtonState[this.luaButtonsHolder.Count];
		for (int i = 0; i < this.luaButtonsHolder.Count; i++)
		{
			this.luaButtonsHolder[i].luaButtonState.index = i;
			array[i] = this.luaButtonsHolder[i].luaButtonState;
		}
		object[] objs = array;
		return LuaCustomConverter.GetTable(objs, script);
	}

	// Token: 0x06001317 RID: 4887 RVA: 0x00080770 File Offset: 0x0007E970
	public Table GetInputs(Script script)
	{
		if (this.luaInputsHolder.Count == 0)
		{
			return null;
		}
		LuaUIInputState[] array = new LuaUIInputState[this.luaInputsHolder.Count];
		for (int i = 0; i < this.luaInputsHolder.Count; i++)
		{
			this.luaInputsHolder[i].luaInputState.index = i;
			this.luaInputsHolder[i].luaInputState.value = this.luaInputsHolder[i].input.value;
			array[i] = this.luaInputsHolder[i].luaInputState;
		}
		object[] objs = array;
		return LuaCustomConverter.GetTable(objs, script);
	}

	// Token: 0x06001318 RID: 4888 RVA: 0x00080814 File Offset: 0x0007EA14
	public bool EditButton(Dictionary<string, object> dict)
	{
		if (dict != null)
		{
			LuaUIButtonState @object = LuaCustomConverter.GetObject<LuaUIButtonState>(dict, null);
			if (@object != null)
			{
				LuaGameObjectScript.LuaUIButtonHolder luaUIButtonHolder = this.IndexToButtonHolder(@object.index);
				@object = LuaCustomConverter.GetObject<LuaUIButtonState>(dict, luaUIButtonHolder.luaButtonState);
				if (string.IsNullOrEmpty(@object.click_function))
				{
					base.LogError("editButton", "click_function was null or an empty string", null);
					return false;
				}
				base.networkView.RPC<LuaUIButtonState>(RPCTarget.All, new Action<LuaUIButtonState>(this.RPCEditButton), @object);
				return true;
			}
		}
		return false;
	}

	// Token: 0x06001319 RID: 4889 RVA: 0x00080888 File Offset: 0x0007EA88
	public bool EditInput(Dictionary<string, object> dict)
	{
		if (dict != null)
		{
			LuaUIInputState @object = LuaCustomConverter.GetObject<LuaUIInputState>(dict, null);
			if (@object != null)
			{
				LuaGameObjectScript.LuaUIInputHolder luaUIInputHolder = this.IndexToInputHolder(@object.index);
				@object = LuaCustomConverter.GetObject<LuaUIInputState>(dict, luaUIInputHolder.luaInputState);
				if (string.IsNullOrEmpty(@object.input_function))
				{
					base.LogError("editInput", "input_function was null or an empty string", null);
					return false;
				}
				base.networkView.RPC<LuaUIInputState>(RPCTarget.All, new Action<LuaUIInputState>(this.RPCEditInput), @object);
				return true;
			}
		}
		return false;
	}

	// Token: 0x0600131A RID: 4890 RVA: 0x000808F9 File Offset: 0x0007EAF9
	[Remote(Permission.Server)]
	[MoonSharpHidden]
	private void RPCEditButton(LuaUIButtonState luaButtonState)
	{
		this.DoUI(luaButtonState, new Action<LuaUIInteractState, LuaGameObjectScript>(this.ActionEditUI));
	}

	// Token: 0x0600131B RID: 4891 RVA: 0x000808F9 File Offset: 0x0007EAF9
	[Remote(Permission.Server)]
	[MoonSharpHidden]
	private void RPCEditInput(LuaUIInputState luaInputState)
	{
		this.DoUI(luaInputState, new Action<LuaUIInteractState, LuaGameObjectScript>(this.ActionEditUI));
	}

	// Token: 0x0600131C RID: 4892 RVA: 0x00080910 File Offset: 0x0007EB10
	[MoonSharpHidden]
	private void ActionEditUI(LuaUIInteractState luaInteractState, LuaGameObjectScript functionOwner)
	{
		if (!(luaInteractState is LuaUIButtonState))
		{
			if (luaInteractState is LuaUIInputState)
			{
				LuaGameObjectScript.LuaUIInputHolder luaUIInputHolder = this.IndexToInputHolder(luaInteractState.index);
				if (luaUIInputHolder != null)
				{
					luaUIInputHolder.luaInputState = (LuaUIInputState)luaInteractState;
					this.SetUI(luaUIInputHolder.button, luaUIInputHolder.label, functionOwner, luaUIInputHolder.luaInputState);
					return;
				}
				base.LogError("editInput", "Could not find matching input index", null);
			}
			return;
		}
		LuaGameObjectScript.LuaUIButtonHolder luaUIButtonHolder = this.IndexToButtonHolder(luaInteractState.index);
		if (luaUIButtonHolder != null)
		{
			luaUIButtonHolder.luaButtonState = (LuaUIButtonState)luaInteractState;
			this.SetUI(luaUIButtonHolder.button, luaUIButtonHolder.label, functionOwner, luaUIButtonHolder.luaButtonState);
			return;
		}
		base.LogError("editButton", "Could not find matching button index", null);
	}

	// Token: 0x0600131D RID: 4893 RVA: 0x000809BC File Offset: 0x0007EBBC
	[MoonSharpHidden]
	private int ButtonStateToIndex(LuaUIButtonState buttonState)
	{
		for (int i = 0; i < this.luaButtonsHolder.Count; i++)
		{
			if (this.luaButtonsHolder[i].luaButtonState == buttonState)
			{
				return i;
			}
		}
		return -1;
	}

	// Token: 0x0600131E RID: 4894 RVA: 0x000809F8 File Offset: 0x0007EBF8
	[MoonSharpHidden]
	private int InputStateToIndex(LuaUIInputState inputState)
	{
		for (int i = 0; i < this.luaInputsHolder.Count; i++)
		{
			if (this.luaInputsHolder[i].luaInputState == inputState)
			{
				return i;
			}
		}
		return -1;
	}

	// Token: 0x0600131F RID: 4895 RVA: 0x00080A32 File Offset: 0x0007EC32
	[MoonSharpHidden]
	private int ButtonHolderToIndex(LuaGameObjectScript.LuaUIButtonHolder buttonHolder)
	{
		return this.luaButtonsHolder.IndexOf(buttonHolder);
	}

	// Token: 0x06001320 RID: 4896 RVA: 0x00080A40 File Offset: 0x0007EC40
	[MoonSharpHidden]
	private int InputHolderToIndex(LuaGameObjectScript.LuaUIInputHolder inputHolder)
	{
		return this.luaInputsHolder.IndexOf(inputHolder);
	}

	// Token: 0x06001321 RID: 4897 RVA: 0x00080A4E File Offset: 0x0007EC4E
	[MoonSharpHidden]
	private LuaGameObjectScript.LuaUIButtonHolder IndexToButtonHolder(int index)
	{
		if (this.luaButtonsHolder.Count > index && index >= 0)
		{
			return this.luaButtonsHolder[index];
		}
		return null;
	}

	// Token: 0x06001322 RID: 4898 RVA: 0x00080A70 File Offset: 0x0007EC70
	[MoonSharpHidden]
	private LuaGameObjectScript.LuaUIInputHolder IndexToInputHolder(int index)
	{
		if (this.luaInputsHolder.Count > index && index >= 0)
		{
			return this.luaInputsHolder[index];
		}
		return null;
	}

	// Token: 0x06001323 RID: 4899 RVA: 0x00080A92 File Offset: 0x0007EC92
	public bool RemoveButton(int index)
	{
		base.networkView.RPC<int, int>(RPCTarget.All, new Action<int, int>(this.RPCRemoveUI), index, 0);
		return this.IndexToButtonHolder(index) != null;
	}

	// Token: 0x06001324 RID: 4900 RVA: 0x00080AB8 File Offset: 0x0007ECB8
	public bool RemoveInput(int index)
	{
		base.networkView.RPC<int, int>(RPCTarget.All, new Action<int, int>(this.RPCRemoveUI), index, 1);
		return this.IndexToInputHolder(index) != null;
	}

	// Token: 0x06001325 RID: 4901 RVA: 0x00080AE0 File Offset: 0x0007ECE0
	[Remote(Permission.Server)]
	[MoonSharpHidden]
	private void RPCRemoveUI(int index, int type)
	{
		if (type != 0)
		{
			if (type != 1)
			{
				return;
			}
			LuaGameObjectScript.LuaUIInputHolder luaUIInputHolder = this.IndexToInputHolder(index);
			if (luaUIInputHolder != null)
			{
				UnityEngine.Object.Destroy(luaUIInputHolder.button);
				base.StartCoroutine(this.DelayRemoveInput(luaUIInputHolder));
				return;
			}
			base.LogError("removeInput", "Could not find matching input index", null);
			return;
		}
		else
		{
			LuaGameObjectScript.LuaUIButtonHolder luaUIButtonHolder = this.IndexToButtonHolder(index);
			if (luaUIButtonHolder != null)
			{
				UnityEngine.Object.Destroy(luaUIButtonHolder.button);
				base.StartCoroutine(this.DelayRemoveButton(luaUIButtonHolder));
				return;
			}
			base.LogError("removeButton", "Could not find matching button index", null);
			return;
		}
	}

	// Token: 0x06001326 RID: 4902 RVA: 0x00080B62 File Offset: 0x0007ED62
	private IEnumerator DelayRemoveButton(LuaGameObjectScript.LuaUIButtonHolder buttonHolder)
	{
		yield return null;
		this.luaButtonsHolder.Remove(buttonHolder);
		yield break;
	}

	// Token: 0x06001327 RID: 4903 RVA: 0x00080B78 File Offset: 0x0007ED78
	private IEnumerator DelayRemoveInput(LuaGameObjectScript.LuaUIInputHolder inputHolder)
	{
		yield return null;
		this.luaInputsHolder.Remove(inputHolder);
		this.CalculateTabInputs();
		yield break;
	}

	// Token: 0x06001328 RID: 4904 RVA: 0x00080B90 File Offset: 0x0007ED90
	private void CalculateTabInputs()
	{
		for (int i = 0; i < this.luaInputsHolder.Count; i++)
		{
			LuaGameObjectScript.LuaUIInputHolder luaUIInputHolder = this.luaInputsHolder[i];
			if (luaUIInputHolder.luaInputState.tab != 2)
			{
				luaUIInputHolder.input.selectOnTab = null;
			}
			else
			{
				int num = i + 1;
				if (num >= this.luaInputsHolder.Count)
				{
					num = 0;
				}
				luaUIInputHolder.input.selectOnTab = this.luaInputsHolder[num].input.gameObject;
			}
		}
	}

	// Token: 0x06001329 RID: 4905 RVA: 0x00080C11 File Offset: 0x0007EE11
	public bool ClearButtons()
	{
		base.networkView.RPC<int>(RPCTarget.All, new Action<int>(this.RPCClearUI), 0);
		return true;
	}

	// Token: 0x0600132A RID: 4906 RVA: 0x00080C2D File Offset: 0x0007EE2D
	public bool ClearInputs()
	{
		base.networkView.RPC<int>(RPCTarget.All, new Action<int>(this.RPCClearUI), 1);
		return true;
	}

	// Token: 0x0600132B RID: 4907 RVA: 0x00080C4C File Offset: 0x0007EE4C
	[Remote(Permission.Server)]
	[MoonSharpHidden]
	public void RPCClearUI(int type)
	{
		if (type == 0)
		{
			for (int i = 0; i < this.luaButtonsHolder.Count; i++)
			{
				UnityEngine.Object.Destroy(this.luaButtonsHolder[i].button);
			}
			this.luaButtonsHolder.Clear();
			return;
		}
		if (type != 1)
		{
			return;
		}
		for (int j = 0; j < this.luaInputsHolder.Count; j++)
		{
			UnityEngine.Object.Destroy(this.luaInputsHolder[j].button);
		}
		this.luaInputsHolder.Clear();
	}

	// Token: 0x0600132C RID: 4908 RVA: 0x00080CD0 File Offset: 0x0007EED0
	[MoonSharpHidden]
	public void ClickUIButton(LuaUIButtonState buttonState, bool altClick = false)
	{
		if (!PlayerScript.Pointer)
		{
			return;
		}
		int num = this.ButtonStateToIndex(buttonState);
		if (Network.isServer)
		{
			this.ClickButtonRPC(num, PlayerScript.PointerScript.PointerColorLabel, altClick);
			return;
		}
		base.networkView.RPC<int, string, bool>(RPCTarget.Server, new Action<int, string, bool>(this.ClickButtonRPC), num, PlayerScript.PointerScript.PointerColorLabel, altClick);
	}

	// Token: 0x0600132D RID: 4909 RVA: 0x00080D30 File Offset: 0x0007EF30
	[Remote(SendType.ReliableNoDelay)]
	[MoonSharpHidden]
	private void ClickButtonRPC(int index, string clickColor, bool altClick = false)
	{
		LuaGameObjectScript.LuaUIButtonHolder luaUIButtonHolder = this.IndexToButtonHolder(index);
		LuaGameObjectScript function_owner = luaUIButtonHolder.luaButtonState.function_owner;
		Script script = LuaGlobalScriptManager.FunctionOwnerToScript(ref function_owner);
		try
		{
			script.Call(script.Globals[luaUIButtonHolder.luaButtonState.click_function], new object[]
			{
				this,
				clickColor,
				altClick
			});
		}
		catch (Exception e)
		{
			base.LogError(luaUIButtonHolder.luaButtonState.click_function, e, function_owner);
		}
	}

	// Token: 0x0600132E RID: 4910 RVA: 0x00080DB8 File Offset: 0x0007EFB8
	[MoonSharpHidden]
	public void SubmitUIInput(LuaUIInputState inputState, bool selected)
	{
		if (!PlayerScript.Pointer)
		{
			return;
		}
		int num = this.InputStateToIndex(inputState);
		LuaGameObjectScript.LuaUIInputHolder luaUIInputHolder = this.luaInputsHolder[num];
		string value = luaUIInputHolder.input.value;
		if (selected && luaUIInputHolder.cacheValue == value)
		{
			return;
		}
		luaUIInputHolder.cacheValue = "";
		if (Network.isServer)
		{
			this.SubmitInputRPC(num, PlayerScript.PointerScript.PointerColorLabel, value, selected);
			return;
		}
		base.networkView.RPC<int, string, string, bool>(RPCTarget.Server, new Action<int, string, string, bool>(this.SubmitInputRPC), num, PlayerScript.PointerScript.PointerColorLabel, value, selected);
	}

	// Token: 0x0600132F RID: 4911 RVA: 0x00080E50 File Offset: 0x0007F050
	[Remote(Permission.Client, SendType.ReliableBuffered, null, SerializationMethod.Default)]
	[MoonSharpHidden]
	private void SubmitInputRPC(int index, string clickColor, string value, bool selected)
	{
		LuaGameObjectScript.LuaUIInputHolder luaUIInputHolder = this.IndexToInputHolder(index);
		if (Network.isServer)
		{
			LuaGameObjectScript function_owner = luaUIInputHolder.luaInputState.function_owner;
			Script script = LuaGlobalScriptManager.FunctionOwnerToScript(ref function_owner);
			string text = null;
			bool flag = false;
			try
			{
				DynValue dynValue = script.Call(script.Globals[luaUIInputHolder.luaInputState.input_function], new object[]
				{
					this,
					clickColor,
					value,
					selected
				});
				if (dynValue != null && !dynValue.IsNilOrNan() && dynValue.Type == DataType.String)
				{
					flag = true;
					text = dynValue.String;
				}
			}
			catch (Exception e)
			{
				base.LogError(luaUIInputHolder.luaInputState.input_function, e, null);
			}
			if (flag && text != value)
			{
				value = text;
				base.networkView.RPC<int, string, string, bool>(RPCTarget.Others, new Action<int, string, string, bool>(this.SubmitInputRPC), index, clickColor, value, selected);
			}
			else
			{
				List<PlayerState> playersList = NetworkSingleton<PlayerManager>.Instance.PlayersList;
				for (int i = 0; i < playersList.Count; i++)
				{
					PlayerState playerState = playersList[i];
					if (playerState.id != NetworkID.ID && playerState.stringColor != clickColor && (!NetworkSingleton<NetworkUI>.Instance.bHotseat || playerState.steamId != NetworkSingleton<PlayerManager>.Instance.MyPlayerState().steamId))
					{
						base.networkView.RPC<int, string, string, bool>(playerState.networkPlayer, new Action<int, string, string, bool>(this.SubmitInputRPC), index, clickColor, value, selected);
					}
				}
			}
		}
		luaUIInputHolder.cacheValue = value;
		if (value != luaUIInputHolder.input.value)
		{
			luaUIInputHolder.input.value = value;
		}
	}

	// Token: 0x04000B84 RID: 2948
	private LuaClock _Clock;

	// Token: 0x04000B85 RID: 2949
	private LuaCounter _Counter;

	// Token: 0x04000B86 RID: 2950
	private LuaRPGFigurine _RPGFigurine;

	// Token: 0x04000B87 RID: 2951
	private LuaBook _Book;

	// Token: 0x04000B88 RID: 2952
	private LuaBrowser _Browser;

	// Token: 0x04000B89 RID: 2953
	private LuaTextTool _TextTool;

	// Token: 0x04000B8A RID: 2954
	private LuaAssetBundle _AssetBundle;

	// Token: 0x04000B8B RID: 2955
	private LuaContainer _Container;

	// Token: 0x04000B8C RID: 2956
	private LuaZone _Zone;

	// Token: 0x04000B8D RID: 2957
	private LuaLayoutZone _LayoutZone;

	// Token: 0x04000B90 RID: 2960
	private LuaScriptWrapper _script;

	// Token: 0x04000B91 RID: 2961
	[MoonSharpHidden]
	[NonSerialized]
	public NetworkPhysicsObject NPO;

	// Token: 0x04000B92 RID: 2962
	private static readonly string[] checkObjectEnterGlobalCallbacks = new string[]
	{
		"tryObjectEnterContainer",
		"filterObjectEnterContainer"
	};

	// Token: 0x04000B93 RID: 2963
	private static readonly string[] checkObjectEnterCallbacks = new string[]
	{
		"tryObjectEnter",
		"filterObjectEnter"
	};

	// Token: 0x04000B94 RID: 2964
	private static readonly string[] checkObjectRotateGlobalNames = new string[]
	{
		"tryObjectRotate"
	};

	// Token: 0x04000B95 RID: 2965
	private static readonly string[] checkObjectRotateNames = new string[]
	{
		"tryRotate"
	};

	// Token: 0x04000B97 RID: 2967
	private GameObject _sourceGetComponentGameObject;

	// Token: 0x04000B98 RID: 2968
	private static readonly List<Type> WhiteListedGetComponentTypes = new List<Type>
	{
		typeof(float),
		typeof(int),
		typeof(bool),
		typeof(Vector3),
		typeof(Color)
	};

	// Token: 0x04000B99 RID: 2969
	private List<Component> _components;

	// Token: 0x04000B9A RID: 2970
	private Dictionary<LuaGameObjectScript.ComponentVariable, object> ComponentValues = new Dictionary<LuaGameObjectScript.ComponentVariable, object>();

	// Token: 0x04000B9B RID: 2971
	private Dictionary<LuaGameObjectScript.MaterialVariable, object> MaterialValues = new Dictionary<LuaGameObjectScript.MaterialVariable, object>();

	// Token: 0x04000B9C RID: 2972
	private List<LuaGameObjectScript.LuaUIButtonHolder> luaButtonsHolder = new List<LuaGameObjectScript.LuaUIButtonHolder>();

	// Token: 0x04000B9D RID: 2973
	private List<LuaGameObjectScript.LuaUIInputHolder> luaInputsHolder = new List<LuaGameObjectScript.LuaUIInputHolder>();

	// Token: 0x02000650 RID: 1616
	public class LuaRotationValue
	{
		// Token: 0x170007AC RID: 1964
		// (get) Token: 0x06003B35 RID: 15157 RVA: 0x001767E5 File Offset: 0x001749E5
		// (set) Token: 0x06003B36 RID: 15158 RVA: 0x001767ED File Offset: 0x001749ED
		public object value { get; set; }

		// Token: 0x170007AD RID: 1965
		// (get) Token: 0x06003B37 RID: 15159 RVA: 0x001767F6 File Offset: 0x001749F6
		// (set) Token: 0x06003B38 RID: 15160 RVA: 0x001767FE File Offset: 0x001749FE
		public Vector3 rotation { get; set; }

		// Token: 0x06003B39 RID: 15161 RVA: 0x00002594 File Offset: 0x00000794
		public LuaRotationValue()
		{
		}

		// Token: 0x06003B3A RID: 15162 RVA: 0x00176807 File Offset: 0x00174A07
		public LuaRotationValue(RotationValue rotationValue)
		{
			this.rotation = rotationValue.rotation;
			this.value = LuaGameObjectScript.LuaRotationValue.GetValue(rotationValue);
		}

		// Token: 0x06003B3B RID: 15163 RVA: 0x00176827 File Offset: 0x00174A27
		public static object GetValue(RotationValue rotationValue)
		{
			if (rotationValue.floatValue != null)
			{
				return rotationValue.floatValue.Value;
			}
			return rotationValue.value;
		}
	}

	// Token: 0x02000651 RID: 1617
	public class LUAFogOfWarReveal
	{
		// Token: 0x040027B6 RID: 10166
		public bool reveal;

		// Token: 0x040027B7 RID: 10167
		public float range = 5f;

		// Token: 0x040027B8 RID: 10168
		public string color = "All";

		// Token: 0x040027B9 RID: 10169
		public float fov = 360f;

		// Token: 0x040027BA RID: 10170
		public float fov_offset;
	}

	// Token: 0x02000652 RID: 1618
	public class LUAFogOfWar
	{
		// Token: 0x040027BB RID: 10171
		public bool hide_gm_pointer = true;

		// Token: 0x040027BC RID: 10172
		public bool hide_objects = true;

		// Token: 0x040027BD RID: 10173
		public bool re_hide_objects;

		// Token: 0x040027BE RID: 10174
		public float fog_height = -0.49f;
	}

	// Token: 0x02000653 RID: 1619
	public class LuaTakeObjectParameters : LuaGlobalScriptManager.LuaCallbackDeprecated
	{
		// Token: 0x040027BF RID: 10175
		public Vector3? position;

		// Token: 0x040027C0 RID: 10176
		public Vector3? rotation;

		// Token: 0x040027C1 RID: 10177
		public string guid;

		// Token: 0x040027C2 RID: 10178
		public int? index;

		// Token: 0x040027C3 RID: 10179
		public bool flip;

		// Token: 0x040027C4 RID: 10180
		public bool top = true;

		// Token: 0x040027C5 RID: 10181
		public bool smooth = true;
	}

	// Token: 0x02000654 RID: 1620
	public class LuaSnapPointParameters
	{
		// Token: 0x040027C6 RID: 10182
		public Vector3 position = Vector3.zero;

		// Token: 0x040027C7 RID: 10183
		public Vector3 rotation = Vector3.zero;

		// Token: 0x040027C8 RID: 10184
		public bool rotation_snap;

		// Token: 0x040027C9 RID: 10185
		public List<string> tags;
	}

	// Token: 0x02000655 RID: 1621
	public class LuaJointParameters
	{
		// Token: 0x040027CA RID: 10186
		public string type;

		// Token: 0x040027CB RID: 10187
		public string joint_object_guid = string.Empty;

		// Token: 0x040027CC RID: 10188
		public bool collision;

		// Token: 0x040027CD RID: 10189
		public float break_force;

		// Token: 0x040027CE RID: 10190
		public float break_torgue;

		// Token: 0x040027CF RID: 10191
		public Vector3 axis;

		// Token: 0x040027D0 RID: 10192
		public Vector3 anchor;

		// Token: 0x040027D1 RID: 10193
		public Vector3 connected_anchor;

		// Token: 0x040027D2 RID: 10194
		public float motor_force;

		// Token: 0x040027D3 RID: 10195
		public float motor_velocity;

		// Token: 0x040027D4 RID: 10196
		public bool motor_free_spin;

		// Token: 0x040027D5 RID: 10197
		public float spring = 10f;

		// Token: 0x040027D6 RID: 10198
		public float damper = 0.2f;

		// Token: 0x040027D7 RID: 10199
		public float max_distance;

		// Token: 0x040027D8 RID: 10200
		public float min_distance;
	}

	// Token: 0x02000656 RID: 1622
	private struct ComponentVariable
	{
		// Token: 0x06003B41 RID: 15169 RVA: 0x001768F4 File Offset: 0x00174AF4
		public ComponentVariable(Component component, VarInfo varInfo)
		{
			this.component = component;
			this.varInfo = varInfo;
		}

		// Token: 0x040027D9 RID: 10201
		public Component component;

		// Token: 0x040027DA RID: 10202
		public VarInfo varInfo;
	}

	// Token: 0x02000657 RID: 1623
	public struct ChildPair
	{
		// Token: 0x06003B42 RID: 15170 RVA: 0x00176904 File Offset: 0x00174B04
		public static GameObject GetChildFrom(Transform topParent, List<LuaGameObjectScript.ChildPair> childPairs)
		{
			Transform transform = topParent;
			if (childPairs != null)
			{
				for (int i = childPairs.Count - 1; i >= 0; i--)
				{
					string b = childPairs[i].name;
					int num = childPairs[i].index;
					int num2 = 0;
					for (int j = 0; j < transform.childCount; j++)
					{
						Transform child = transform.GetChild(j);
						if (child.name == b)
						{
							if (num2 == num)
							{
								transform = child;
								break;
							}
							num2++;
						}
					}
				}
			}
			return transform.gameObject;
		}

		// Token: 0x06003B43 RID: 15171 RVA: 0x0017698C File Offset: 0x00174B8C
		public static List<LuaGameObjectScript.ChildPair> GetChildPairs(Transform topParent, Transform child)
		{
			if (child == topParent)
			{
				return null;
			}
			List<LuaGameObjectScript.ChildPair> list = new List<LuaGameObjectScript.ChildPair>();
			while (child != topParent)
			{
				string b = child.name;
				Transform parent = child.parent;
				int num = 0;
				for (int i = 0; i < parent.childCount; i++)
				{
					Transform child2 = parent.GetChild(i);
					if (child2.name == b)
					{
						if (child2 == child)
						{
							break;
						}
						num++;
					}
				}
				list.Add(new LuaGameObjectScript.ChildPair
				{
					name = b,
					index = num
				});
				child = parent;
				if (child == null)
				{
					return null;
				}
			}
			return list;
		}

		// Token: 0x040027DB RID: 10203
		public string name;

		// Token: 0x040027DC RID: 10204
		public int index;
	}

	// Token: 0x02000658 RID: 1624
	private struct RPCMaterialVariable
	{
		// Token: 0x06003B44 RID: 15172 RVA: 0x00176A34 File Offset: 0x00174C34
		public RPCMaterialVariable(List<Renderer> renderers, Material mat, string varName)
		{
			foreach (Renderer renderer in renderers)
			{
				renderer.GetSharedMaterials(LuaGameObjectScript.RPCMaterialVariable.cachedMaterials);
				for (int i = 0; i < LuaGameObjectScript.RPCMaterialVariable.cachedMaterials.Count; i++)
				{
					Material y = LuaGameObjectScript.RPCMaterialVariable.cachedMaterials[i];
					if (mat == y)
					{
						this.childPairs = LuaGameObjectScript.ChildPair.GetChildPairs(renderer.transform.root, renderer.transform);
						this.varName = varName;
						this.materialIndex = i;
						this.rendererIndex = 0;
						renderer.GetComponents<Renderer>(LuaGameObjectScript.RPCMaterialVariable.cachedRenderers);
						for (int j = 0; j < LuaGameObjectScript.RPCMaterialVariable.cachedRenderers.Count; j++)
						{
							if (LuaGameObjectScript.RPCMaterialVariable.cachedRenderers[j] == renderer)
							{
								this.rendererIndex = j;
							}
						}
						LuaGameObjectScript.RPCMaterialVariable.cachedRenderers.Clear();
						LuaGameObjectScript.RPCMaterialVariable.cachedMaterials.Clear();
						return;
					}
				}
			}
			this.childPairs = null;
			this.materialIndex = 0;
			this.rendererIndex = 0;
			this.varName = null;
			LuaGameObjectScript.RPCMaterialVariable.cachedMaterials.Clear();
		}

		// Token: 0x06003B45 RID: 15173 RVA: 0x00176B6C File Offset: 0x00174D6C
		public bool GetMaterialVariable(Transform topParent, out Material mat, out string varName)
		{
			mat = null;
			varName = this.varName;
			GameObject childFrom = LuaGameObjectScript.ChildPair.GetChildFrom(topParent, this.childPairs);
			if (!childFrom)
			{
				Debug.Log("fail target");
				return false;
			}
			childFrom.GetComponents<Renderer>(LuaGameObjectScript.RPCMaterialVariable.cachedRenderers);
			if (this.rendererIndex >= LuaGameObjectScript.RPCMaterialVariable.cachedRenderers.Count)
			{
				Debug.Log("fail renderer");
				return false;
			}
			LuaGameObjectScript.RPCMaterialVariable.cachedRenderers[this.rendererIndex].GetSharedMaterials(LuaGameObjectScript.RPCMaterialVariable.cachedMaterials);
			LuaGameObjectScript.RPCMaterialVariable.cachedRenderers.Clear();
			if (this.materialIndex >= LuaGameObjectScript.RPCMaterialVariable.cachedMaterials.Count)
			{
				Debug.Log("fail materialIndex");
				return false;
			}
			mat = LuaGameObjectScript.RPCMaterialVariable.cachedMaterials[this.materialIndex];
			LuaGameObjectScript.RPCMaterialVariable.cachedMaterials.Clear();
			return true;
		}

		// Token: 0x040027DD RID: 10205
		public List<LuaGameObjectScript.ChildPair> childPairs;

		// Token: 0x040027DE RID: 10206
		public int rendererIndex;

		// Token: 0x040027DF RID: 10207
		public int materialIndex;

		// Token: 0x040027E0 RID: 10208
		public string varName;

		// Token: 0x040027E1 RID: 10209
		private static readonly List<Material> cachedMaterials = new List<Material>();

		// Token: 0x040027E2 RID: 10210
		private static readonly List<Renderer> cachedRenderers = new List<Renderer>();
	}

	// Token: 0x02000659 RID: 1625
	private struct RPCComponentVariable
	{
		// Token: 0x06003B47 RID: 15175 RVA: 0x00176C44 File Offset: 0x00174E44
		public RPCComponentVariable(GameObject topParent, Component component, VarInfo varInfo)
		{
			this.childPairs = LuaGameObjectScript.ChildPair.GetChildPairs(topParent.transform, component.transform);
			this.componentName = component.GetType().Name;
			this.componentIndex = 0;
			Component[] components = component.gameObject.GetComponents(component.GetType());
			for (int i = 0; i < components.Length; i++)
			{
				if (components[i] == component)
				{
					this.componentIndex = i;
				}
			}
			this.varName = varInfo.Name;
		}

		// Token: 0x06003B48 RID: 15176 RVA: 0x00176CC0 File Offset: 0x00174EC0
		public bool GetComponentVariable(Transform topParent, out Component comp, out string varName)
		{
			comp = null;
			varName = this.varName;
			GameObject childFrom = LuaGameObjectScript.ChildPair.GetChildFrom(topParent, this.childPairs);
			if (!childFrom)
			{
				Debug.Log("fail target");
				return false;
			}
			if (this.componentIndex == 0)
			{
				Type type = Utilities.GetType(this.componentName);
				if (type == null)
				{
					Debug.Log("fail type");
					return false;
				}
				comp = childFrom.GetComponent(type);
			}
			else
			{
				Type type2 = Utilities.GetType(this.componentName);
				if (type2 == null)
				{
					Debug.Log("fail type");
					return false;
				}
				Component[] components = childFrom.GetComponents(type2);
				if (this.componentIndex >= components.Length)
				{
					Debug.Log("fail componentIndex");
					return false;
				}
				comp = components[this.componentIndex];
			}
			return comp;
		}

		// Token: 0x040027E3 RID: 10211
		public List<LuaGameObjectScript.ChildPair> childPairs;

		// Token: 0x040027E4 RID: 10212
		public string componentName;

		// Token: 0x040027E5 RID: 10213
		public int componentIndex;

		// Token: 0x040027E6 RID: 10214
		public string varName;
	}

	// Token: 0x0200065A RID: 1626
	private struct MaterialVariable
	{
		// Token: 0x040027E7 RID: 10215
		public Material material;

		// Token: 0x040027E8 RID: 10216
		public string varName;
	}

	// Token: 0x0200065B RID: 1627
	public class LuaVectorLine
	{
		// Token: 0x040027E9 RID: 10217
		public List<Vector3> points = new List<Vector3>();

		// Token: 0x040027EA RID: 10218
		public Color color = Color.white;

		// Token: 0x040027EB RID: 10219
		public float thickness = 0.1f;

		// Token: 0x040027EC RID: 10220
		public Vector3 rotation = Vector3.zero;

		// Token: 0x040027ED RID: 10221
		public bool loop;

		// Token: 0x040027EE RID: 10222
		public bool square;
	}

	// Token: 0x0200065C RID: 1628
	public class LuaDecal
	{
		// Token: 0x040027EF RID: 10223
		public string name;

		// Token: 0x040027F0 RID: 10224
		public string url;

		// Token: 0x040027F1 RID: 10225
		public Vector3 position;

		// Token: 0x040027F2 RID: 10226
		public Vector3 rotation;

		// Token: 0x040027F3 RID: 10227
		public Vector3 scale = Vector3.one;
	}

	// Token: 0x0200065D RID: 1629
	public class LuaUIButtonHolder
	{
		// Token: 0x06003B4B RID: 15179 RVA: 0x00176DC5 File Offset: 0x00174FC5
		public LuaUIButtonHolder(LuaUIButtonState luaButtonState, GameObject button, GameObject label)
		{
			this.luaButtonState = luaButtonState;
			this.button = button;
			this.label = label;
		}

		// Token: 0x040027F4 RID: 10228
		public LuaUIButtonState luaButtonState;

		// Token: 0x040027F5 RID: 10229
		public GameObject button;

		// Token: 0x040027F6 RID: 10230
		public GameObject label;
	}

	// Token: 0x0200065E RID: 1630
	public class LuaUIInputHolder
	{
		// Token: 0x06003B4C RID: 15180 RVA: 0x00176DE2 File Offset: 0x00174FE2
		public LuaUIInputHolder(LuaUIInputState luaInputState, GameObject button, GameObject label)
		{
			this.luaInputState = luaInputState;
			this.button = button;
			this.label = label;
			this.input = button.GetComponent<UIInput>();
		}

		// Token: 0x040027F7 RID: 10231
		public LuaUIInputState luaInputState;

		// Token: 0x040027F8 RID: 10232
		public GameObject button;

		// Token: 0x040027F9 RID: 10233
		public GameObject label;

		// Token: 0x040027FA RID: 10234
		public UIInput input;

		// Token: 0x040027FB RID: 10235
		public string cacheValue;
	}
}
