using System;
using System.Collections.Generic;
using Force.DeepCloner;
using UnityEngine;

// Token: 0x020001F3 RID: 499
public class ObjectState : IEquatable<ObjectState>
{
	// Token: 0x06001A20 RID: 6688 RVA: 0x000B73DC File Offset: 0x000B55DC
	public bool EqualsObject(ObjectState os)
	{
		if (os == null)
		{
			return false;
		}
		if (this.Name != os.Name)
		{
			return false;
		}
		TransformState transform = this.Transform;
		TransformState transform2 = os.Transform;
		ColourState? colorDiffuse = this.ColorDiffuse;
		ColourState? colorDiffuse2 = os.ColorDiffuse;
		if (colorDiffuse != null && colorDiffuse2 != null && colorDiffuse.Value.ToColour() != colorDiffuse2.Value.ToColour())
		{
			return false;
		}
		string guid = this.GUID;
		string guid2 = os.GUID;
		List<string> tags = this.Tags;
		List<string> tags2 = os.Tags;
		bool locked = this.Locked;
		bool grid = this.Grid;
		bool snap = this.Snap;
		bool ignoreFoW = this.IgnoreFoW;
		bool dragSelectable = this.DragSelectable;
		bool measureMovement = this.MeasureMovement;
		bool autoraise = this.Autoraise;
		bool sticky = this.Sticky;
		bool tooltip = this.Tooltip;
		bool gridProjection = this.GridProjection;
		bool? hideWhenFaceDown = this.HideWhenFaceDown;
		bool? hands = this.Hands;
		int layoutGroupSortIndex = this.LayoutGroupSortIndex;
		string memo = this.Memo;
		Vector3 altLookAngle = this.AltLookAngle;
		bool locked2 = os.Locked;
		bool grid2 = os.Grid;
		bool snap2 = os.Snap;
		bool ignoreFoW2 = os.IgnoreFoW;
		bool dragSelectable2 = os.DragSelectable;
		bool measureMovement2 = os.MeasureMovement;
		bool autoraise2 = os.Autoraise;
		bool sticky2 = os.Sticky;
		bool tooltip2 = os.Tooltip;
		bool gridProjection2 = os.GridProjection;
		bool? hideWhenFaceDown2 = os.HideWhenFaceDown;
		bool? hands2 = os.Hands;
		int layoutGroupSortIndex2 = os.LayoutGroupSortIndex;
		string memo2 = os.Memo;
		Vector3 altLookAngle2 = os.AltLookAngle;
		this.Transform = null;
		os.Transform = null;
		this.ColorDiffuse = null;
		os.ColorDiffuse = null;
		this.GUID = null;
		os.GUID = null;
		this.Tags = null;
		os.Tags = null;
		this.Memo = null;
		os.Memo = null;
		this.AltLookAngle = Vector3.zero;
		os.AltLookAngle = Vector3.zero;
		this.Locked = false;
		this.Grid = false;
		this.Snap = false;
		this.IgnoreFoW = false;
		this.DragSelectable = false;
		this.MeasureMovement = false;
		this.Autoraise = false;
		this.Sticky = false;
		this.Tooltip = false;
		this.GridProjection = false;
		this.HideWhenFaceDown = new bool?(false);
		this.Hands = null;
		this.LayoutGroupSortIndex = 0;
		os.Locked = false;
		os.Grid = false;
		os.Snap = false;
		os.IgnoreFoW = false;
		os.DragSelectable = false;
		os.MeasureMovement = false;
		os.Autoraise = false;
		os.Sticky = false;
		os.Tooltip = false;
		os.GridProjection = false;
		os.HideWhenFaceDown = new bool?(false);
		os.Hands = null;
		os.LayoutGroupSortIndex = 0;
		bool result = this == os;
		this.Transform = transform;
		os.Transform = transform2;
		this.ColorDiffuse = colorDiffuse;
		os.ColorDiffuse = colorDiffuse2;
		this.GUID = guid;
		os.GUID = guid2;
		this.Tags = tags;
		os.Tags = tags2;
		this.Memo = memo;
		os.Memo = memo2;
		this.AltLookAngle = altLookAngle;
		os.AltLookAngle = altLookAngle2;
		this.Locked = locked;
		this.Grid = grid;
		this.Snap = snap;
		this.IgnoreFoW = ignoreFoW;
		this.DragSelectable = dragSelectable;
		this.MeasureMovement = measureMovement;
		this.Autoraise = autoraise;
		this.Sticky = sticky;
		this.Tooltip = tooltip;
		this.GridProjection = gridProjection;
		this.HideWhenFaceDown = hideWhenFaceDown;
		this.Hands = hands;
		this.LayoutGroupSortIndex = layoutGroupSortIndex;
		os.Locked = locked2;
		os.Grid = grid2;
		os.Snap = snap2;
		os.IgnoreFoW = ignoreFoW2;
		os.DragSelectable = dragSelectable2;
		os.MeasureMovement = measureMovement2;
		os.Autoraise = autoraise2;
		os.Sticky = sticky2;
		os.Tooltip = tooltip2;
		os.GridProjection = gridProjection2;
		os.HideWhenFaceDown = hideWhenFaceDown2;
		os.Hands = hands2;
		os.LayoutGroupSortIndex = layoutGroupSortIndex2;
		return result;
	}

	// Token: 0x06001A21 RID: 6689 RVA: 0x000B77D3 File Offset: 0x000B59D3
	public ObjectState Clone()
	{
		return this.DeepClone<ObjectState>();
	}

	// Token: 0x06001A22 RID: 6690 RVA: 0x000B77DC File Offset: 0x000B59DC
	public static List<ObjectState> RemoveDuplicates(List<ObjectState> objectStates)
	{
		List<ObjectState> list = new List<ObjectState>();
		for (int i = 0; i < objectStates.Count; i++)
		{
			ObjectState objectState = objectStates[i];
			if (!(objectState == null))
			{
				bool flag = false;
				for (int j = 0; j < list.Count; j++)
				{
					ObjectState os = list[j];
					if (objectState.EqualsObject(os))
					{
						flag = true;
						break;
					}
				}
				if (!flag)
				{
					list.Add(objectState);
				}
			}
		}
		return list;
	}

	// Token: 0x06001A23 RID: 6691 RVA: 0x000B784C File Offset: 0x000B5A4C
	public bool Equals(ObjectState other)
	{
		if (other == null)
		{
			return false;
		}
		if (this == other)
		{
			return true;
		}
		if (this.GUID == other.GUID && this.Name == other.Name && object.Equals(this.Transform, other.Transform) && this.Nickname == other.Nickname && this.Description == other.Description && this.GMNotes == other.GMNotes && this.Memo == other.Memo && this.AltLookAngle == other.AltLookAngle && Nullable.Equals<ColourState>(this.ColorDiffuse, other.ColorDiffuse) && this.Tags.SequenceEqualNullable(other.Tags) && this.LayoutGroupSortIndex == other.LayoutGroupSortIndex && this.Value == other.Value && this.Locked == other.Locked && this.Grid == other.Grid && this.Snap == other.Snap && this.IgnoreFoW == other.IgnoreFoW && this.MeasureMovement == other.MeasureMovement && this.DragSelectable == other.DragSelectable && this.Autoraise == other.Autoraise && this.Sticky == other.Sticky && this.Tooltip == other.Tooltip && this.GridProjection == other.GridProjection)
		{
			bool? flag = this.HideWhenFaceDown;
			bool? flag2 = other.HideWhenFaceDown;
			if (flag.GetValueOrDefault() == flag2.GetValueOrDefault() & flag != null == (flag2 != null))
			{
				flag2 = this.Hands;
				flag = other.Hands;
				if (flag2.GetValueOrDefault() == flag.GetValueOrDefault() & flag2 != null == (flag != null))
				{
					flag = this.AltSound;
					flag2 = other.AltSound;
					if (flag.GetValueOrDefault() == flag2.GetValueOrDefault() & flag != null == (flag2 != null))
					{
						int? num = this.MaterialIndex;
						int? num2 = other.MaterialIndex;
						if (num.GetValueOrDefault() == num2.GetValueOrDefault() & num != null == (num2 != null))
						{
							num2 = this.MeshIndex;
							num = other.MeshIndex;
							if (num2.GetValueOrDefault() == num.GetValueOrDefault() & num2 != null == (num != null))
							{
								num = this.Number;
								num2 = other.Number;
								if (num.GetValueOrDefault() == num2.GetValueOrDefault() & num != null == (num2 != null))
								{
									flag2 = this.RPGmode;
									flag = other.RPGmode;
									if (flag2.GetValueOrDefault() == flag.GetValueOrDefault() & flag2 != null == (flag != null))
									{
										flag = this.RPGdead;
										flag2 = other.RPGdead;
										if ((flag.GetValueOrDefault() == flag2.GetValueOrDefault() & flag != null == (flag2 != null)) && this.FogColor == other.FogColor)
										{
											flag2 = this.FogHidePointers;
											flag = other.FogHidePointers;
											if (flag2.GetValueOrDefault() == flag.GetValueOrDefault() & flag2 != null == (flag != null))
											{
												flag = this.FogReverseHiding;
												flag2 = other.FogReverseHiding;
												if (flag.GetValueOrDefault() == flag2.GetValueOrDefault() & flag != null == (flag2 != null))
												{
													flag2 = this.FogSeethrough;
													flag = other.FogSeethrough;
													if ((flag2.GetValueOrDefault() == flag.GetValueOrDefault() & flag2 != null == (flag != null)) && Nullable.Equals<VectorState>(this.vector, other.vector) && this.miscID == other.miscID)
													{
														num2 = this.CardID;
														num = other.CardID;
														if (num2.GetValueOrDefault() == num.GetValueOrDefault() & num2 != null == (num != null))
														{
															flag = this.SidewaysCard;
															flag2 = other.SidewaysCard;
															if ((flag.GetValueOrDefault() == flag2.GetValueOrDefault() & flag != null == (flag2 != null)) && this.DeckIDs.SequenceEqualNullable(other.DeckIDs) && this.CustomDeck.SequenceEqualNullable(other.CustomDeck) && object.Equals(this.CustomMesh, other.CustomMesh) && object.Equals(this.CustomImage, other.CustomImage) && object.Equals(this.CustomAssetbundle, other.CustomAssetbundle) && object.Equals(this.CustomPDF, other.CustomPDF) && object.Equals(this.FogOfWar, other.FogOfWar) && object.Equals(this.FogOfWarRevealer, other.FogOfWarRevealer) && object.Equals(this.LayoutZone, other.LayoutZone) && object.Equals(this.Clock, other.Clock) && object.Equals(this.Counter, other.Counter) && object.Equals(this.Tablet, other.Tablet) && object.Equals(this.Mp3Player, other.Mp3Player) && object.Equals(this.Calculator, other.Calculator) && object.Equals(this.Text, other.Text) && object.Equals(this.Bag, other.Bag) && this.LuaScript == other.LuaScript && this.LuaScriptState == other.LuaScriptState && this.XmlUI == other.XmlUI && this.CustomUIAssets.SequenceEqualNullable(other.CustomUIAssets) && object.Equals(this.PhysicsMaterial, other.PhysicsMaterial) && object.Equals(this.Rigidbody, other.Rigidbody) && object.Equals(this.JointFixed, other.JointFixed) && object.Equals(this.JointHinge, other.JointHinge) && object.Equals(this.JointSpring, other.JointSpring) && this.ContainedObjects.SequenceEqualNullable(other.ContainedObjects) && this.AttachedSnapPoints.SequenceEqualNullable(other.AttachedSnapPoints) && this.AttachedVectorLines.SequenceEqualNullable(other.AttachedVectorLines) && this.AttachedDecals.SequenceEqualNullable(other.AttachedDecals) && this.States.SequenceEqualNullable(other.States) && this.RotationValues.SequenceEqualNullable(other.RotationValues))
															{
																return this.ChildObjects.SequenceEqualNullable(other.ChildObjects);
															}
														}
													}
												}
											}
										}
									}
								}
							}
						}
					}
				}
			}
		}
		return false;
	}

	// Token: 0x06001A24 RID: 6692 RVA: 0x000B7F9D File Offset: 0x000B619D
	public override bool Equals(object obj)
	{
		return obj != null && (this == obj || (!(obj.GetType() != base.GetType()) && this.Equals((ObjectState)obj)));
	}

	// Token: 0x06001A25 RID: 6693 RVA: 0x000B7FCC File Offset: 0x000B61CC
	public override int GetHashCode()
	{
		return ((((((((((((((((((((((((((((((((((((((((((((((((((((((((((((((((((((((this.GUID != null) ? this.GUID.GetHashCode() : 0) * 397 ^ ((this.Name != null) ? this.Name.GetHashCode() : 0)) * 397 ^ ((this.Transform != null) ? this.Transform.GetHashCode() : 0)) * 397 ^ ((this.Nickname != null) ? this.Nickname.GetHashCode() : 0)) * 397 ^ ((this.Description != null) ? this.Description.GetHashCode() : 0)) * 397 ^ ((this.GMNotes != null) ? this.GMNotes.GetHashCode() : 0)) * 397 ^ ((this.Memo != null) ? this.Memo.GetHashCode() : 0)) * 397 ^ this.AltLookAngle.GetHashCode()) * 397 ^ this.ColorDiffuse.GetHashCode()) * 397 ^ ((this.Tags != null) ? this.Tags.GetHashCode() : 0)) * 397 ^ this.LayoutGroupSortIndex) * 397 ^ this.Locked.GetHashCode()) * 397 ^ this.Grid.GetHashCode()) * 397 ^ this.Snap.GetHashCode()) * 397 ^ this.IgnoreFoW.GetHashCode()) * 397 ^ this.MeasureMovement.GetHashCode()) * 397 ^ this.DragSelectable.GetHashCode()) * 397 ^ this.Autoraise.GetHashCode()) * 397 ^ this.Sticky.GetHashCode()) * 397 ^ this.Tooltip.GetHashCode()) * 397 ^ this.GridProjection.GetHashCode()) * 397 ^ this.HideWhenFaceDown.GetHashCode()) * 397 ^ this.Hands.GetHashCode()) * 397 ^ this.AltSound.GetHashCode()) * 397 ^ this.MaterialIndex.GetHashCode()) * 397 ^ this.MeshIndex.GetHashCode()) * 397 ^ this.Number.GetHashCode()) * 397 ^ this.RPGmode.GetHashCode()) * 397 ^ this.RPGdead.GetHashCode()) * 397 ^ ((this.FogColor != null) ? this.FogColor.GetHashCode() : 0)) * 397 ^ this.FogHidePointers.GetHashCode()) * 397 ^ this.FogReverseHiding.GetHashCode()) * 397 ^ this.FogSeethrough.GetHashCode()) * 397 ^ this.vector.GetHashCode()) * 397 ^ ((this.miscID != null) ? this.miscID.GetHashCode() : 0)) * 397 ^ this.CardID.GetHashCode()) * 397 ^ this.SidewaysCard.GetHashCode()) * 397 ^ ((this.DeckIDs != null) ? this.DeckIDs.GetHashCode() : 0)) * 397 ^ ((this.CustomDeck != null) ? this.CustomDeck.GetHashCode() : 0)) * 397 ^ ((this.CustomMesh != null) ? this.CustomMesh.GetHashCode() : 0)) * 397 ^ ((this.CustomImage != null) ? this.CustomImage.GetHashCode() : 0)) * 397 ^ ((this.CustomAssetbundle != null) ? this.CustomAssetbundle.GetHashCode() : 0)) * 397 ^ ((this.CustomPDF != null) ? this.CustomPDF.GetHashCode() : 0)) * 397 ^ ((this.FogOfWar != null) ? this.FogOfWar.GetHashCode() : 0)) * 397 ^ ((this.FogOfWarRevealer != null) ? this.FogOfWarRevealer.GetHashCode() : 0)) * 397 ^ ((this.LayoutZone != null) ? this.LayoutZone.GetHashCode() : 0)) * 397 ^ ((this.Clock != null) ? this.Clock.GetHashCode() : 0)) * 397 ^ ((this.Counter != null) ? this.Counter.GetHashCode() : 0)) * 397 ^ ((this.Tablet != null) ? this.Tablet.GetHashCode() : 0)) * 397 ^ ((this.Mp3Player != null) ? this.Mp3Player.GetHashCode() : 0)) * 397 ^ ((this.Calculator != null) ? this.Calculator.GetHashCode() : 0)) * 397 ^ ((this.Text != null) ? this.Text.GetHashCode() : 0)) * 397 ^ ((this.Bag != null) ? this.Bag.GetHashCode() : 0)) * 397 ^ ((this.LuaScript != null) ? this.LuaScript.GetHashCode() : 0)) * 397 ^ ((this.LuaScriptState != null) ? this.LuaScriptState.GetHashCode() : 0)) * 397 ^ ((this.XmlUI != null) ? this.XmlUI.GetHashCode() : 0)) * 397 ^ ((this.CustomUIAssets != null) ? this.CustomUIAssets.GetHashCode() : 0)) * 397 ^ ((this.PhysicsMaterial != null) ? this.PhysicsMaterial.GetHashCode() : 0)) * 397 ^ ((this.Rigidbody != null) ? this.Rigidbody.GetHashCode() : 0)) * 397 ^ ((this.JointFixed != null) ? this.JointFixed.GetHashCode() : 0)) * 397 ^ ((this.JointHinge != null) ? this.JointHinge.GetHashCode() : 0)) * 397 ^ ((this.JointSpring != null) ? this.JointSpring.GetHashCode() : 0)) * 397 ^ ((this.ContainedObjects != null) ? this.ContainedObjects.GetHashCode() : 0)) * 397 ^ ((this.AttachedSnapPoints != null) ? this.AttachedSnapPoints.GetHashCode() : 0)) * 397 ^ ((this.AttachedVectorLines != null) ? this.AttachedVectorLines.GetHashCode() : 0)) * 397 ^ ((this.AttachedDecals != null) ? this.AttachedDecals.GetHashCode() : 0)) * 397 ^ ((this.States != null) ? this.States.GetHashCode() : 0)) * 397 ^ ((this.RotationValues != null) ? this.RotationValues.GetHashCode() : 0)) * 397 ^ this.Value) * 397 ^ ((this.ChildObjects != null) ? this.ChildObjects.GetHashCode() : 0);
	}

	// Token: 0x06001A26 RID: 6694 RVA: 0x000B7302 File Offset: 0x000B5502
	public static bool operator ==(ObjectState left, ObjectState right)
	{
		return object.Equals(left, right);
	}

	// Token: 0x06001A27 RID: 6695 RVA: 0x000B730B File Offset: 0x000B550B
	public static bool operator !=(ObjectState left, ObjectState right)
	{
		return !object.Equals(left, right);
	}

	// Token: 0x0400103B RID: 4155
	public string GUID = "";

	// Token: 0x0400103C RID: 4156
	public string Name = "";

	// Token: 0x0400103D RID: 4157
	public TransformState Transform;

	// Token: 0x0400103E RID: 4158
	public string Nickname = "";

	// Token: 0x0400103F RID: 4159
	public string Description = "";

	// Token: 0x04001040 RID: 4160
	public string GMNotes = "";

	// Token: 0x04001041 RID: 4161
	public string Memo;

	// Token: 0x04001042 RID: 4162
	public Vector3 AltLookAngle = Vector3.zero;

	// Token: 0x04001043 RID: 4163
	public ColourState? ColorDiffuse;

	// Token: 0x04001044 RID: 4164
	public List<string> Tags;

	// Token: 0x04001045 RID: 4165
	public int LayoutGroupSortIndex;

	// Token: 0x04001046 RID: 4166
	public int Value;

	// Token: 0x04001047 RID: 4167
	public bool Locked;

	// Token: 0x04001048 RID: 4168
	public bool Grid = true;

	// Token: 0x04001049 RID: 4169
	public bool Snap = true;

	// Token: 0x0400104A RID: 4170
	public bool IgnoreFoW;

	// Token: 0x0400104B RID: 4171
	public bool MeasureMovement;

	// Token: 0x0400104C RID: 4172
	public bool DragSelectable = true;

	// Token: 0x0400104D RID: 4173
	public bool Autoraise = true;

	// Token: 0x0400104E RID: 4174
	public bool Sticky = true;

	// Token: 0x0400104F RID: 4175
	public bool Tooltip = true;

	// Token: 0x04001050 RID: 4176
	public bool GridProjection;

	// Token: 0x04001051 RID: 4177
	public bool? HideWhenFaceDown;

	// Token: 0x04001052 RID: 4178
	public bool? Hands;

	// Token: 0x04001053 RID: 4179
	public bool? AltSound;

	// Token: 0x04001054 RID: 4180
	public int? MaterialIndex;

	// Token: 0x04001055 RID: 4181
	public int? MeshIndex;

	// Token: 0x04001056 RID: 4182
	public int? Number;

	// Token: 0x04001057 RID: 4183
	public bool? RPGmode;

	// Token: 0x04001058 RID: 4184
	public bool? RPGdead;

	// Token: 0x04001059 RID: 4185
	public string FogColor;

	// Token: 0x0400105A RID: 4186
	public bool? FogHidePointers;

	// Token: 0x0400105B RID: 4187
	public bool? FogReverseHiding;

	// Token: 0x0400105C RID: 4188
	public bool? FogSeethrough;

	// Token: 0x0400105D RID: 4189
	public string miscID;

	// Token: 0x0400105E RID: 4190
	public VectorState? vector;

	// Token: 0x0400105F RID: 4191
	public int? CardID;

	// Token: 0x04001060 RID: 4192
	public bool? SidewaysCard;

	// Token: 0x04001061 RID: 4193
	public List<int> DeckIDs;

	// Token: 0x04001062 RID: 4194
	public Dictionary<int, CustomDeckState> CustomDeck;

	// Token: 0x04001063 RID: 4195
	public CustomMeshState CustomMesh;

	// Token: 0x04001064 RID: 4196
	public CustomImageState CustomImage;

	// Token: 0x04001065 RID: 4197
	public CustomAssetbundleState CustomAssetbundle;

	// Token: 0x04001066 RID: 4198
	public CustomPDFState CustomPDF;

	// Token: 0x04001067 RID: 4199
	public FogOfWarState FogOfWar;

	// Token: 0x04001068 RID: 4200
	public FogOfWarRevealerState FogOfWarRevealer;

	// Token: 0x04001069 RID: 4201
	public LayoutZoneState LayoutZone;

	// Token: 0x0400106A RID: 4202
	public ClockState Clock;

	// Token: 0x0400106B RID: 4203
	public CounterState Counter;

	// Token: 0x0400106C RID: 4204
	public TabletState Tablet;

	// Token: 0x0400106D RID: 4205
	public Mp3PlayerState Mp3Player;

	// Token: 0x0400106E RID: 4206
	public CalculatorState Calculator;

	// Token: 0x0400106F RID: 4207
	public TextState Text;

	// Token: 0x04001070 RID: 4208
	public BagState Bag;

	// Token: 0x04001071 RID: 4209
	public string LuaScript = "";

	// Token: 0x04001072 RID: 4210
	public string LuaScriptState = "";

	// Token: 0x04001073 RID: 4211
	public string XmlUI = "";

	// Token: 0x04001074 RID: 4212
	public List<CustomAssetState> CustomUIAssets;

	// Token: 0x04001075 RID: 4213
	public PhysicsMaterialState PhysicsMaterial;

	// Token: 0x04001076 RID: 4214
	public RigidbodyState Rigidbody;

	// Token: 0x04001077 RID: 4215
	public JointFixedState JointFixed;

	// Token: 0x04001078 RID: 4216
	public JointHingeState JointHinge;

	// Token: 0x04001079 RID: 4217
	public JointSpringState JointSpring;

	// Token: 0x0400107A RID: 4218
	public List<ObjectState> ContainedObjects;

	// Token: 0x0400107B RID: 4219
	public List<SnapPointState> AttachedSnapPoints;

	// Token: 0x0400107C RID: 4220
	public List<VectorLineState> AttachedVectorLines;

	// Token: 0x0400107D RID: 4221
	public List<DecalState> AttachedDecals;

	// Token: 0x0400107E RID: 4222
	public Dictionary<int, ObjectState> States;

	// Token: 0x0400107F RID: 4223
	public List<RotationValueState> RotationValues;

	// Token: 0x04001080 RID: 4224
	public List<ObjectState> ChildObjects;
}
