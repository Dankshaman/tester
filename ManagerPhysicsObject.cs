using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using Force.DeepCloner;
using MoonSharp.Interpreter;
using NewNet;
using UnityEngine;

// Token: 0x020001AB RID: 427
public class ManagerPhysicsObject : NetworkSingleton<ManagerPhysicsObject>
{
	// Token: 0x1700038A RID: 906
	// (get) Token: 0x0600153D RID: 5437 RVA: 0x0008A23A File Offset: 0x0008843A
	// (set) Token: 0x0600153E RID: 5438 RVA: 0x0008A242 File Offset: 0x00088442
	public GameObject CurrentlyZoomedObject
	{
		get
		{
			return this._CurrentlyZoomedObject;
		}
		set
		{
			if (value != this._CurrentlyZoomedObject)
			{
				this._CurrentlyZoomedObject = value;
				EventManager.TriggerZoomObjectChange(value);
			}
		}
	}

	// Token: 0x1700038B RID: 907
	// (get) Token: 0x0600153F RID: 5439 RVA: 0x0008A25F File Offset: 0x0008845F
	// (set) Token: 0x06001540 RID: 5440 RVA: 0x0008A267 File Offset: 0x00088467
	public GameObject Table
	{
		get
		{
			return this._Table;
		}
		set
		{
			this._Table = value;
			if (value)
			{
				this.TableNPO = value.GetComponent<NetworkPhysicsObject>();
				this.TableScript = value.GetComponent<TableScript>();
				return;
			}
			this.TableNPO = null;
			this.TableScript = null;
		}
	}

	// Token: 0x1700038C RID: 908
	// (get) Token: 0x06001541 RID: 5441 RVA: 0x0008A29F File Offset: 0x0008849F
	// (set) Token: 0x06001542 RID: 5442 RVA: 0x0008A2A7 File Offset: 0x000884A7
	public NetworkPhysicsObject TableNPO { get; private set; }

	// Token: 0x1700038D RID: 909
	// (get) Token: 0x06001543 RID: 5443 RVA: 0x0008A2B0 File Offset: 0x000884B0
	// (set) Token: 0x06001544 RID: 5444 RVA: 0x0008A2B8 File Offset: 0x000884B8
	public TableScript TableScript { get; private set; }

	// Token: 0x1700038E RID: 910
	// (get) Token: 0x06001545 RID: 5445 RVA: 0x0008A2C1 File Offset: 0x000884C1
	// (set) Token: 0x06001546 RID: 5446 RVA: 0x0008A2C9 File Offset: 0x000884C9
	public SkyScript Sky { get; set; }

	// Token: 0x06001547 RID: 5447 RVA: 0x0008A2D4 File Offset: 0x000884D4
	protected override void Awake()
	{
		base.Awake();
		DateTime.Now.ToString();
		EventManager.OnPlayerChangeColor += this.OnPlayerChangeColor;
		EventManager.OnChangePlayerTeam += this.OnChangePlayerTeam;
	}

	// Token: 0x06001548 RID: 5448 RVA: 0x0008A317 File Offset: 0x00088517
	private void Start()
	{
		this.uiRoot = NetworkSingleton<NetworkUI>.Instance.GUIUIRoot.GetComponent<UIRoot>();
	}

	// Token: 0x06001549 RID: 5449 RVA: 0x0008A32E File Offset: 0x0008852E
	private void OnDestroy()
	{
		EventManager.OnPlayerChangeColor -= this.OnPlayerChangeColor;
		EventManager.OnChangePlayerTeam -= this.OnChangePlayerTeam;
	}

	// Token: 0x0600154A RID: 5450 RVA: 0x0008A354 File Offset: 0x00088554
	private void Update()
	{
		this.RemainingImpactSounds = 32;
		if (!VRHMD.isVR && this.UnloadInterval > 0f && Time.time > this.unloadTime + this.UnloadInterval)
		{
			UnityEngine.Debug.Log("UnloadUnusedAssets");
			Resources.UnloadUnusedAssets();
			this.unloadTime = Time.time;
		}
	}

	// Token: 0x0600154B RID: 5451 RVA: 0x0008A3AC File Offset: 0x000885AC
	private void LateUpdate()
	{
		for (int i = 0; i < this.AllNPOs.Count; i++)
		{
			this.AllNPOs[i].ManagedLateUpdate();
		}
		if (NetworkPhysicsObject.UseParticleHighlights)
		{
			for (int j = 0; j < this.GrabbableNPOs.Count; j++)
			{
				this.GrabbableNPOs[j].UpdateParticleHighlights();
			}
		}
	}

	// Token: 0x0600154C RID: 5452 RVA: 0x0008A410 File Offset: 0x00088610
	private void FixedUpdate()
	{
		if (Network.isServer)
		{
			for (int i = 0; i < this.GrabbableNPOs.Count; i++)
			{
				NetworkPhysicsObject networkPhysicsObject = this.GrabbableNPOs[i];
				if (networkPhysicsObject.IsHeldBySomebody)
				{
					Pointer pointer = this.PointerFromID(networkPhysicsObject.HeldByPlayerID);
					if (pointer)
					{
						this.UpdateGrabbedNPO(networkPhysicsObject, pointer);
					}
				}
				if (networkPhysicsObject.IsSmoothMoving)
				{
					this.UpdateMovingNPO(networkPhysicsObject);
				}
			}
		}
	}

	// Token: 0x0600154D RID: 5453 RVA: 0x0008A47C File Offset: 0x0008867C
	private void UpdateGrabbedNPO(NetworkPhysicsObject grabbedNPO, Pointer pointer)
	{
		GameObject gameObject = grabbedNPO.gameObject;
		Rigidbody rigidbody = grabbedNPO.rigidbody;
		Quaternion lhs = Quaternion.identity;
		NetworkTracked networkTracked = null;
		bool flag = false;
		grabbedNPO.StopSmoothMove(true);
		Vector3 vector;
		if (grabbedNPO.HeldByTouchID == -1)
		{
			vector = pointer.transform.position;
		}
		else
		{
			networkTracked = pointer.pointerSyncs.NetworkTrackedFromID(grabbedNPO.HeldByTouchID);
			vector = pointer.pointerSyncs.PositionFromID(grabbedNPO.HeldByTouchID);
			lhs = pointer.pointerSyncs.RotationFromID(grabbedNPO.HeldByTouchID);
			if (networkTracked)
			{
				flag = !networkTracked.Laser;
			}
			this.vrTrackedController = VRTrackedController.VRTrackedControllerFromIndex(grabbedNPO.HeldByTouchID);
			if (this.vrTrackedController != null)
			{
				grabbedNPO.DesiredVelocity = new Vector3?(this.vrTrackedController.Velocity);
				grabbedNPO.DesiredAngularVelocity = new Vector3?(this.vrTrackedController.controller.angularVelocity);
			}
			else
			{
				grabbedNPO.DesiredVelocity = null;
				grabbedNPO.DesiredAngularVelocity = null;
			}
		}
		if (!pointer.FirstGrabbedObject || pointer.FirstGrabbedNPO.HeldByPlayerID != pointer.ID)
		{
			pointer.FirstGrabbedObject = gameObject;
		}
		rigidbody.useGravity = false;
		float num = pointer.FirstGrabbedNPO.GetMass();
		num = this.RescaleMass(num);
		num /= 5f;
		rigidbody.mass = num;
		rigidbody.angularDrag = 20f;
		rigidbody.drag = 26.64f / ((num + 1f) / 2f);
		Bounds bounds = grabbedNPO.GetBounds();
		Vector3 vector2;
		Bounds boundsNotNormalized = grabbedNPO.GetBoundsNotNormalized(out vector2);
		Vector3 vector3 = vector;
		float num2 = pointer.AutoRaise;
		float num3 = 1f;
		if (((!pointer.tapping() && grabbedNPO.DoAutoRaise) || ServerOptions.isPhysicsLock) && bounds.extents != Vector3.zero)
		{
			Vector3 center = boundsNotNormalized.center;
			while (Physics.CheckBox(center, bounds.extents, grabbedNPO.rigidbody.rotation, HoverScript.NonHeldLayerMask))
			{
				center.y += Mathf.Max(0.25f, boundsNotNormalized.size.y);
			}
			float maxDistance = center.y - vector3.y + bounds.size.y;
			RaycastHit raycastHit;
			if (Physics.BoxCast(center, bounds.extents, Vector3.down, out raycastHit, grabbedNPO.rigidbody.rotation, maxDistance, HoverScript.NonHeldLayerMask))
			{
				num3 = raycastHit.point.y;
				if (raycastHit.point.y > vector3.y + 0.01f)
				{
					float num4 = raycastHit.point.y - vector3.y;
					if (num4 > num2)
					{
						NetworkPhysicsObject networkPhysicsObject = this.NPOFromGO(this.GOFromCollider(raycastHit.collider));
						if (!networkPhysicsObject || networkPhysicsObject.IsHeldByNobodyAndIsNotMarkedDestroyedInHeldID)
						{
							num2 = num4;
							pointer.AutoRaise = num2;
						}
					}
				}
			}
		}
		if (pointer.PrevAutoRaise > num2)
		{
			num2 = pointer.PrevAutoRaise;
		}
		grabbedNPO.SetLayerToHeld((!pointer.tapping() && grabbedNPO.DoAutoRaise) || ServerOptions.isPhysicsLock);
		if (gameObject != pointer.FirstGrabbedObject)
		{
			UnityEngine.Object joint = grabbedNPO.joint;
			Joint joint2 = pointer.FirstGrabbedNPO.joint;
			if (joint || (joint2 && joint2.connectedBody == rigidbody))
			{
				rigidbody.drag = 5f;
				rigidbody.angularDrag = 5f;
				rigidbody.mass /= 1000f;
				return;
			}
		}
		vector3.y = 0f;
		if (!pointer.tapping() || ServerOptions.isPhysicsLock)
		{
			vector3.y = 2.5f;
			vector3.y *= pointer.RaiseHeight * 2f;
		}
		else
		{
			vector3.y = -0.2f;
		}
		if (flag || grabbedNPO.CurrentPlayerHand)
		{
			vector3.y = 0f;
		}
		if (pointer.raising())
		{
			vector3.y += 3.75f;
		}
		vector3.y += vector.y;
		if (!flag)
		{
			vector3.y += boundsNotNormalized.extents.y;
			vector3.y += vector2.y;
		}
		vector3.y += num2;
		vector3.y += 0.01f;
		grabbedNPO.SetCollision((pointer.tapping() || grabbedNPO.HeldOffset == Vector3.zero || !grabbedNPO.DoAutoRaise) && !ServerOptions.isPhysicsLock);
		Quaternion quaternion = Quaternion.identity;
		if (!gameObject.CompareTag("Dice"))
		{
			Vector3 axis = Vector3.right;
			if (this.FlipsAroundZAxis(gameObject))
			{
				axis = Vector3.forward;
			}
			quaternion = Quaternion.AngleAxis(grabbedNPO.HeldRotationOffset.x, Vector3.right) * quaternion;
			quaternion = Quaternion.AngleAxis(grabbedNPO.HeldRotationOffset.z, Vector3.forward) * quaternion;
			quaternion = Quaternion.AngleAxis((float)(grabbedNPO.HeldFlipRotationIndex * 15), axis) * quaternion;
			quaternion = Quaternion.AngleAxis((float)(grabbedNPO.HeldSpinRotationIndex * 15), Vector3.up) * quaternion;
			if (gameObject.CompareTag("Backgammon Piece") || gameObject.CompareTag("GoPiece"))
			{
				quaternion = Quaternion.identity;
			}
			Quaternion rhs = quaternion * Quaternion.Inverse(rigidbody.rotation);
			if (flag)
			{
				Quaternion lhs2 = lhs * Quaternion.Inverse(grabbedNPO.HeldByControllerPickupRotation);
				rhs = lhs2 * rhs;
				grabbedNPO.DesiredRotation = new Quaternion?(lhs2 * quaternion);
			}
			else
			{
				grabbedNPO.DesiredRotation = new Quaternion?(quaternion);
			}
			float num5;
			Vector3 a;
			rhs.ToAngleAxis(out num5, out a);
			num5 = LibVector.StandardizeAngle(num5);
			if (num5 != 0f)
			{
				a *= num5;
				float num6 = 1.7f;
				Vector3 torque = a * (Time.deltaTime * 75f * num6 * 0.03f);
				rigidbody.AddTorque(torque, ForceMode.VelocityChange);
			}
		}
		if (!flag)
		{
			vector3 += grabbedNPO.HeldOffset;
		}
		else
		{
			vector3 += grabbedNPO.HeldByControllerPickupOffset - vector;
			vector3 = networkTracked.transform.TransformPoint(vector3);
		}
		float num7 = boundsNotNormalized.extents.y + vector2.y + 0.01f + num3;
		grabbedNPO.HeldMinimumY = num7;
		vector3.y = Mathf.Max(num7, vector3.y);
		if (grabbedNPO.CurrentPlayerHand)
		{
			float yheight = grabbedNPO.CurrentPlayerHand.GetYHeight();
			vector3.y = Mathf.Max(yheight, vector3.y);
		}
		grabbedNPO.DesiredPosition = new Vector3?(vector3);
		Vector3 vector4 = (vector3 - rigidbody.position) * (Time.deltaTime * 20000f * 1.25f);
		if (!grabbedNPO.IsCollidable)
		{
			grabbedNPO.bReduceForce = false;
		}
		if (grabbedNPO.bReduceForce)
		{
			vector4 /= 2.5f;
		}
		rigidbody.AddForce(vector4);
	}

	// Token: 0x0600154E RID: 5454 RVA: 0x0008AB74 File Offset: 0x00088D74
	private void UpdateMovingNPO(NetworkPhysicsObject movingNPO)
	{
		Vector3 targetPosition = movingNPO.currentSmoothPosition.TargetPosition;
		Quaternion targetRotation = movingNPO.currentSmoothRotation.TargetRotation;
		Rigidbody rigidbody = movingNPO.rigidbody;
		if (NetworkPhysicsObject.DebugSmoothMove)
		{
			LibDebug.DrawLines(movingNPO, new Vector3[]
			{
				movingNPO.rigidbody.position,
				targetPosition
			});
			if (movingNPO.CurrentPlayerHand)
			{
				LibDebug.DrawSphere(Colour.ColourFromLabel(movingNPO.CurrentPlayerHand.TriggerLabel), movingNPO.rigidbody.position, 1f);
			}
		}
		Joint joint = movingNPO.joint;
		if (joint && !movingNPO.HeldIDIndicatesDestruction)
		{
			if (joint.connectedBody)
			{
				NetworkPhysicsObject component = joint.connectedBody.GetComponent<NetworkPhysicsObject>();
				if (component && component.IsSmoothMoving)
				{
					rigidbody.mass = 0.0001f;
					return;
				}
			}
			movingNPO.StopSmoothMove(false);
			return;
		}
		if (joint)
		{
			UnityEngine.Object.Destroy(joint);
		}
		float num = movingNPO.GetMass();
		num = this.RescaleMass(num);
		rigidbody.mass = num;
		if (!movingNPO.currentSmoothPosition.FastSpeed)
		{
			rigidbody.drag = 20f / ((num + 1f) / 2f);
		}
		else
		{
			rigidbody.drag = 40f / ((num + 1f) / 2f);
		}
		rigidbody.angularDrag = 13f;
		rigidbody.useGravity = false;
		rigidbody.isKinematic = false;
		movingNPO.SetCollision(movingNPO.currentSmoothPosition.Colliding && movingNPO.currentSmoothRotation.Colliding);
		if (movingNPO.currentSmoothPosition.Moving)
		{
			if (movingNPO.currentSmoothPosition.CanStop() && Vector3.Distance(targetPosition, rigidbody.position) < 0.025f)
			{
				movingNPO.StopSmoothPosition(true);
				if (movingNPO.currentSmoothPosition.FastSpeed)
				{
					rigidbody.velocity = Vector3.down * 1f;
				}
			}
			else
			{
				float num2 = ManagerPhysicsObject.SmoothMoveBaseForce;
				if (movingNPO.currentSmoothPosition.FastSpeed)
				{
					num2 *= 10f;
				}
				rigidbody.AddForce((targetPosition - rigidbody.position) * (Time.deltaTime * num2));
			}
		}
		if (movingNPO.currentSmoothRotation.Moving)
		{
			if (Quaternion.Angle(targetRotation, rigidbody.rotation) < 1f)
			{
				movingNPO.StopSmoothRotation(true);
				return;
			}
			float num3;
			Vector3 a;
			(targetRotation * Quaternion.Inverse(rigidbody.rotation)).ToAngleAxis(out num3, out a);
			num3 = LibVector.StandardizeAngle(num3);
			if (num3 != 0f)
			{
				a *= num3;
				rigidbody.AddTorque(a * (Time.deltaTime * 75f * 0.03f), ForceMode.VelocityChange);
			}
		}
	}

	// Token: 0x0600154F RID: 5455 RVA: 0x0008AE18 File Offset: 0x00089018
	public float RescaleMass(float mass)
	{
		if (mass > 1f)
		{
			mass = 1f + Mathf.Sqrt(mass - 1f) / 4f;
		}
		else if (mass < 1f)
		{
			mass = 1f - Mathf.Sqrt(1f - mass) / 1.5f;
		}
		return mass;
	}

	// Token: 0x06001550 RID: 5456 RVA: 0x0008AE6C File Offset: 0x0008906C
	public void PopulateGrabbedObjects(List<NetworkPhysicsObject> grabbedNpos, int grabbedID, int playerID, int touchID = -1)
	{
		NetworkPhysicsObject networkPhysicsObject = this.NPOFromID(grabbedID);
		Pointer pointer = this.PointerFromID(playerID);
		if (!networkPhysicsObject || networkPhysicsObject.IsLocked || (networkPhysicsObject.HeldByPlayerID == pointer.ID && networkPhysicsObject.HeldByTouchID == touchID))
		{
			return;
		}
		using (List<NetworkPhysicsObject>.Enumerator enumerator = grabbedNpos.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				if (enumerator.Current.ID == networkPhysicsObject.ID)
				{
					return;
				}
			}
		}
		grabbedNpos.Add(networkPhysicsObject);
		if (grabbedNpos.Count >= 128)
		{
			return;
		}
		GameObject gameObject = networkPhysicsObject.gameObject;
		Rigidbody rigidbody = networkPhysicsObject.rigidbody;
		Joint joint = networkPhysicsObject.joint;
		if (joint && joint.connectedBody && (!gameObject.CompareTag("Card") || pointer.FirstGrabbedObject != gameObject))
		{
			NetworkPhysicsObject component = joint.connectedBody.GetComponent<NetworkPhysicsObject>();
			if (component && component.HeldByPlayerID != playerID)
			{
				this.PopulateGrabbedObjects(grabbedNpos, component.ID, playerID, touchID);
				if (grabbedNpos.Count >= 128)
				{
					return;
				}
			}
		}
		foreach (NetworkPhysicsObject networkPhysicsObject2 in this.GrabbableNPOs)
		{
			Joint joint2 = networkPhysicsObject2.joint;
			if (joint2 && joint2.connectedBody == rigidbody && networkPhysicsObject2 && networkPhysicsObject2.HeldByPlayerID != playerID)
			{
				this.PopulateGrabbedObjects(grabbedNpos, networkPhysicsObject2.ID, playerID, touchID);
				if (grabbedNpos.Count >= 128)
				{
					return;
				}
			}
		}
		networkPhysicsObject.SetLayerToHeld(true);
		List<NetworkPhysicsObject> stickyObjects = networkPhysicsObject.GetStickyObjects();
		networkPhysicsObject.SetLayerToHeld(false);
		foreach (NetworkPhysicsObject networkPhysicsObject3 in stickyObjects)
		{
			if (networkPhysicsObject3 && networkPhysicsObject3.HeldByPlayerID != playerID)
			{
				this.PopulateGrabbedObjects(grabbedNpos, networkPhysicsObject3.ID, playerID, touchID);
				if (grabbedNpos.Count >= 128)
				{
					break;
				}
			}
		}
	}

	// Token: 0x06001551 RID: 5457 RVA: 0x0008B0B0 File Offset: 0x000892B0
	public void ClientGrab(int grabbedID, int playerID, Vector3 holdOffset, int touchID = -1)
	{
		this.ClientGrab(grabbedID, playerID, holdOffset, Vector3.zero, touchID);
	}

	// Token: 0x06001552 RID: 5458 RVA: 0x0008B0C4 File Offset: 0x000892C4
	public void ClientGrab(int grabbedID, int playerID, Vector3 holdOffset, Vector3 holdRotationOffset, int touchID = -1)
	{
		Pointer pointer = this.PointerFromID(playerID);
		bool isVR = NetworkSingleton<PlayerManager>.Instance.PlayerStateFromID(playerID).isVR;
		if (pointer == null || pointer.num_grabbed >= 128 || (holdOffset == Vector3.zero && pointer.num_grabbed >= 64))
		{
			return;
		}
		Quaternion heldByControllerPickupRotation = Quaternion.identity;
		NetworkTracked networkTracked = null;
		bool flag = false;
		Vector3 a;
		if (touchID == -1)
		{
			a = pointer.transform.position;
		}
		else
		{
			networkTracked = pointer.pointerSyncs.NetworkTrackedFromID(touchID);
			a = pointer.pointerSyncs.PositionFromID(touchID);
			heldByControllerPickupRotation = pointer.pointerSyncs.RotationFromID(touchID);
			if (networkTracked)
			{
				flag = !networkTracked.Laser;
			}
		}
		for (int i = 0; i < this.GrabbableNPOs.Count; i++)
		{
			NetworkPhysicsObject networkPhysicsObject = this.GrabbableNPOs[i];
			GameObject gameObject = networkPhysicsObject.gameObject;
			if (networkPhysicsObject.ID == grabbedID && !networkPhysicsObject.IsLocked && (networkPhysicsObject.HeldByPlayerID != pointer.ID || networkPhysicsObject.HeldByTouchID != touchID))
			{
				if (networkPhysicsObject.IsFloatingFromTeleport)
				{
					holdOffset = Vector3.zero;
				}
				networkPhysicsObject.StopSmoothMove(true);
				pointer.num_grabbed++;
				int heldSpinRotationIndex = this.SpinRotationIndexFromGrabbable(gameObject, isVR ? 15f : ((float)pointer.RotationSnap));
				int num = 0;
				bool flag2 = gameObject.CompareTag("Figurine") || gameObject.CompareTag("Chess") || gameObject.CompareTag("rpgFigurine") || gameObject.CompareTag("Tileset") || gameObject.CompareTag("Bag") || gameObject.CompareTag("Infinite") || gameObject.CompareTag("Jigsaw") || gameObject.CompareTag("Counter") || gameObject.CompareTag("Mp3");
				if (!flag2)
				{
					num = this.FlipRotationIndexFromGrabbable(gameObject, isVR ? 15f : ((float)pointer.RotationSnap));
				}
				networkPhysicsObject.HeldSpinRotationIndex = heldSpinRotationIndex;
				networkPhysicsObject.HeldFlipRotationIndex = num;
				if (playerID == NetworkID.ID)
				{
					networkPhysicsObject.DisableFastDragWhilePickingUp();
				}
				if (!pointer.FirstGrabbedObject || pointer.FirstGrabbedObject == gameObject)
				{
					pointer.FirstGrabbedObject = gameObject;
				}
				networkPhysicsObject.HeldByControllerPickupRotation = Quaternion.identity;
				networkPhysicsObject.bReduceForce = false;
				networkPhysicsObject.PrevHeldByPlayerID = playerID;
				networkPhysicsObject.HeldByPlayerID = playerID;
				networkPhysicsObject.HeldByTouchID = touchID;
				Vector3 heldOffset = holdOffset;
				Vector3 vector = holdRotationOffset;
				if (holdOffset != Vector3.zero)
				{
					networkPhysicsObject.ResetBounds();
					Vector3 vector2;
					heldOffset.y -= networkPhysicsObject.GetBoundsNotNormalized(out vector2).extents.y;
					heldOffset.y -= vector2.y;
				}
				if ((num == 0 || num == 12) && !flag2)
				{
					vector = gameObject.transform.eulerAngles;
					if (vector.x > 180f)
					{
						vector.x -= 360f;
					}
					else if (vector.x < -180f)
					{
						vector.x += 360f;
					}
					if (vector.z > 180f)
					{
						vector.z -= 360f;
					}
					else if (vector.z < -180f)
					{
						vector.z += 360f;
					}
					if (vector.x > 90f)
					{
						vector.x -= 180f;
						vector.z *= -1f;
					}
					else if (vector.x < -90f)
					{
						vector.x += 180f;
						vector.z *= -1f;
					}
					if (vector.z > 90f)
					{
						vector.z -= 180f;
						vector.x *= -1f;
					}
					else if (vector.z < -90f)
					{
						vector.z += 180f;
						vector.x *= -1f;
					}
					if (Mathf.Abs(vector.x) > 45f)
					{
						vector.x = 0f;
					}
					if (Mathf.Abs(vector.z) > 45f)
					{
						vector.z = 0f;
					}
				}
				else
				{
					vector = Vector3.zero;
				}
				networkPhysicsObject.HeldOffset = heldOffset;
				networkPhysicsObject.HeldRotationOffset = vector;
				networkPhysicsObject.SetLayerToHeld(true);
				networkPhysicsObject.bReduceForce = false;
				if (networkPhysicsObject.soundScript)
				{
					networkPhysicsObject.soundScript.PickUpSound();
				}
				if (flag)
				{
					networkPhysicsObject.HeldByControllerPickupOffset = networkTracked.transform.InverseTransformPoint(a + networkPhysicsObject.HeldOffset);
					networkPhysicsObject.HeldByControllerPickupRotation = heldByControllerPickupRotation;
					networkPhysicsObject.ResetVRRotations();
				}
				Rigidbody rigidbody = networkPhysicsObject.rigidbody;
				if (networkPhysicsObject.joint && gameObject.CompareTag("Card") && pointer.FirstGrabbedObject == gameObject)
				{
					networkPhysicsObject.cardScript.ResetCard();
				}
				networkPhysicsObject.ResetIdleFreezeAroundObject();
				return;
			}
		}
	}

	// Token: 0x06001553 RID: 5459 RVA: 0x0008B5E1 File Offset: 0x000897E1
	private IEnumerator DelayGrab(int id, int playerID, Vector3 holdOffset)
	{
		yield return null;
		this.ClientGrab(id, playerID, holdOffset, -1);
		yield break;
	}

	// Token: 0x06001554 RID: 5460 RVA: 0x0008B605 File Offset: 0x00089805
	private IEnumerator DelayGrab(int id, int player_id, Vector3 HoldOffset, Vector3 HoldRotationOffset)
	{
		yield return null;
		this.ClientGrab(id, player_id, HoldOffset, HoldRotationOffset, -1);
		yield break;
	}

	// Token: 0x06001555 RID: 5461 RVA: 0x0008B634 File Offset: 0x00089834
	public void ClientRelease(int playerID, int dropID = -1, int touchID = -1, int hoverID = -1)
	{
		bool flag = false;
		Pointer pointer = this.PointerFromID(playerID);
		Vector3 vector = Vector3.zero;
		if (pointer)
		{
			flag = pointer.tapping();
			pointer.RecentlyDropped.Clear();
			vector = pointer.transform.position;
			if (touchID != -1)
			{
				vector = pointer.pointerSyncs.PositionFromID(touchID);
			}
		}
		NetworkPhysicsObject networkPhysicsObject = null;
		LayoutZone layoutZone = null;
		int targetGroupID = -1;
		bool flag2 = false;
		bool flag3 = false;
		if (hoverID != -1)
		{
			networkPhysicsObject = this.NPOFromID(hoverID);
			LayoutZone.TryNPOInLayoutZone(networkPhysicsObject, out layoutZone, out targetGroupID, LayoutZone.PotentialZoneCheck.None);
		}
		for (int i = 0; i < this.GrabbableNPOs.Count; i++)
		{
			NetworkPhysicsObject networkPhysicsObject2 = this.GrabbableNPOs[i];
			GameObject gameObject = networkPhysicsObject2.gameObject;
			bool flag4 = true;
			if (dropID != -1)
			{
				flag4 = (networkPhysicsObject2.ID == dropID);
			}
			if (networkPhysicsObject2.HeldByPlayerID == playerID && networkPhysicsObject2.HeldByTouchID == touchID && flag4 && !networkPhysicsObject2.IsLocked && !networkPhysicsObject2.HeldIDIndicatesDestruction)
			{
				bool flag5 = false;
				if (touchID != -1)
				{
					NetworkTracked networkTracked = pointer.pointerSyncs.NetworkTrackedFromID(networkPhysicsObject2.HeldByTouchID);
					if (networkTracked)
					{
						flag5 = !networkTracked.Laser;
					}
				}
				networkPhysicsObject2.DesiredPosition = null;
				networkPhysicsObject2.DesiredRotation = null;
				if (networkPhysicsObject2.HeldCloseInVR != 0)
				{
					networkPhysicsObject2.SetHeldClose(0, true);
				}
				if (pointer)
				{
					pointer.num_grabbed--;
					if (pointer.num_grabbed < 0)
					{
						pointer.num_grabbed = 0;
					}
				}
				if (dropID != -1 && dropID == networkPhysicsObject2.ID)
				{
					PlayersSearchingInventory component = networkPhysicsObject2.GetComponent<PlayersSearchingInventory>();
					if (component && component.AnyoneSearching())
					{
						return;
					}
					if (gameObject.CompareTag("Deck"))
					{
						GameObject go = gameObject.GetComponent<DeckScript>().TakeCard(gameObject.transform.up.normalized.y >= 0f, false);
						if (!flag)
						{
							base.StartCoroutine(this.DelaySnapToGrid(go));
						}
						DeckScript component2 = gameObject.GetComponent<DeckScript>();
						if (component2.LastCard)
						{
							NetworkPhysicsObject networkPhysicsObject3 = this.NPOFromGO(component2.LastCard);
							networkPhysicsObject3.SetCollision(false);
							base.StartCoroutine(this.DelayGrab(networkPhysicsObject3.ID, playerID, networkPhysicsObject2.HeldOffset, networkPhysicsObject2.HeldRotationOffset));
							component2.LastCard = null;
						}
						return;
					}
					StackObject component3 = gameObject.GetComponent<StackObject>();
					if (component3 && component3.num_objects_ > 0)
					{
						GameObject gameObject2 = component3.TakeObject(false);
						if (gameObject2.GetComponent<SoundScript>())
						{
							gameObject2.GetComponent<SoundScript>().DropSound();
						}
						if (!flag)
						{
							base.StartCoroutine(this.DelaySnapToGrid(gameObject2));
						}
						if (component3.LastObject)
						{
							NetworkPhysicsObject networkPhysicsObject4 = this.NPOFromGO(component3.gameObject);
							if (networkPhysicsObject4)
							{
								networkPhysicsObject4.SetCollision(false);
								base.StartCoroutine(this.DelayGrab(networkPhysicsObject4.ID, playerID, networkPhysicsObject2.HeldOffset, networkPhysicsObject2.HeldRotationOffset));
							}
							component3.LastObject = null;
						}
						return;
					}
				}
				bool isCollidable = networkPhysicsObject2.IsCollidable;
				networkPhysicsObject2.Drop();
				if (networkPhysicsObject2.soundScript)
				{
					networkPhysicsObject2.soundScript.DropSound();
				}
				if (layoutZone)
				{
					if (layoutZone.TryAddIncomingNPO(networkPhysicsObject2))
					{
						flag2 = true;
						goto IL_7C7;
					}
				}
				else
				{
					List<LayoutZone> list = LayoutZone.LayoutZonesContainingNPO(networkPhysicsObject2);
					bool flag6 = false;
					for (int j = 0; j < list.Count; j++)
					{
						if (list[j].TryAddIncomingNPO(networkPhysicsObject2))
						{
							flag6 = true;
							break;
						}
					}
					if (flag6)
					{
						flag3 = true;
						goto IL_7C7;
					}
				}
				bool flag7 = pointer && (gameObject.CompareTag("Dice") || gameObject.CompareTag("Coin")) && (networkPhysicsObject2.rigidbody.velocity.magnitude > 2f || networkPhysicsObject2.rigidbody.angularVelocity.magnitude > 2f) && this.CheckLuaTryRandomize(networkPhysicsObject2, pointer.ID);
				if (flag7)
				{
					Vector3 vector2 = new Vector3(vector.z - networkPhysicsObject2.rigidbody.position.z + UnityEngine.Random.Range(-1f, 1f) / 2f, UnityEngine.Random.Range(-1f, 1f), -1f * (vector.x - networkPhysicsObject2.rigidbody.position.x) - UnityEngine.Random.Range(-1f, 1f) / 2f);
					if (gameObject.CompareTag("Coin"))
					{
						vector2 = new Vector3(vector2.x + UnityEngine.Random.Range(1f, 3f) / 2f, vector2.y, vector2.z + UnityEngine.Random.Range(1f, 3f) / 2f);
						gameObject.GetComponent<SoundScript>().ShakeSound();
					}
					networkPhysicsObject2.rigidbody.rotation = UnityEngine.Random.rotation;
					networkPhysicsObject2.rigidbody.AddTorque(vector2 * Time.deltaTime * 20f, ForceMode.Impulse);
					EventManager.TriggerObjectRandomize(networkPhysicsObject2, pointer.PointerColorLabel);
				}
				if (!networkPhysicsObject2.bReduceForce)
				{
					networkPhysicsObject2.rigidbody.velocity *= 0.5f;
				}
				else
				{
					networkPhysicsObject2.rigidbody.velocity *= 0.8f;
				}
				if (!flag5 || ServerOptions.isPhysicsLock)
				{
					networkPhysicsObject2.rigidbody.velocity = new Vector3(networkPhysicsObject2.rigidbody.velocity.x, -1f, networkPhysicsObject2.rigidbody.velocity.z);
				}
				if (!flag5)
				{
					if (networkPhysicsObject2.rigidbody.velocity.magnitude > 40f / (this.RescaleMass(networkPhysicsObject2.GetMass()) / 2f + 0.5f))
					{
						networkPhysicsObject2.rigidbody.velocity = networkPhysicsObject2.rigidbody.velocity.normalized * 40f / (this.RescaleMass(networkPhysicsObject2.GetMass()) / 2f + 0.5f);
					}
					if (gameObject.CompareTag("Coin") && flag7)
					{
						networkPhysicsObject2.rigidbody.velocity = new Vector3(networkPhysicsObject2.rigidbody.velocity.x, 10f + UnityEngine.Random.Range(1f, 10f), networkPhysicsObject2.rigidbody.velocity.z);
					}
				}
				if (ServerOptions.isPhysicsLock)
				{
					networkPhysicsObject2.rigidbody.velocity = new Vector3(0f, networkPhysicsObject2.rigidbody.velocity.y, 0f);
					networkPhysicsObject2.rigidbody.angularVelocity = Vector3.zero;
				}
				if (networkPhysicsObject2.transform.position.y < networkPhysicsObject2.HeldMinimumY && networkPhysicsObject2.joint == null)
				{
					networkPhysicsObject2.transform.position = new Vector3(networkPhysicsObject2.transform.position.x, networkPhysicsObject2.HeldMinimumY, networkPhysicsObject2.transform.position.z);
				}
				networkPhysicsObject2.bReduceForce = false;
				if (flag5 && VRTrackedController.ControllerStyle == TrackedControllerStyle.New)
				{
					if (networkPhysicsObject2.DesiredVelocity != null)
					{
						networkPhysicsObject2.rigidbody.velocity = networkPhysicsObject2.DesiredVelocity.Value;
						networkPhysicsObject2.DesiredVelocity = null;
					}
					if (networkPhysicsObject2.DesiredAngularVelocity != null)
					{
						networkPhysicsObject2.rigidbody.angularVelocity = networkPhysicsObject2.DesiredAngularVelocity.Value;
						networkPhysicsObject2.DesiredAngularVelocity = null;
					}
					if (!flag)
					{
						this.SnapToGrid(gameObject, isCollidable, 12f);
					}
				}
				else if (!flag)
				{
					this.SnapToGrid(gameObject, isCollidable, 6.5f);
				}
				base.StartCoroutine(pointer.AddRecentlyDropped(networkPhysicsObject2));
			}
			IL_7C7:;
		}
		if (flag2)
		{
			layoutZone.AddIncomingObjects(networkPhysicsObject, targetGroupID);
		}
		if (flag3)
		{
			LayoutZone.AddAllIncomingObjects(vector);
		}
	}

	// Token: 0x06001556 RID: 5462 RVA: 0x0008BE38 File Offset: 0x0008A038
	public IEnumerator DelaySnapToGrid(GameObject go)
	{
		yield return new WaitForFixedUpdate();
		this.SnapToGrid(go, false, 6.5f);
		yield break;
	}

	// Token: 0x06001557 RID: 5463 RVA: 0x0008BE4E File Offset: 0x0008A04E
	private IEnumerator DelayBlockGridPos(NetworkPhysicsObject npo)
	{
		Vector2 gridPos = new Vector2(npo.currentSmoothPosition.TargetPosition.x, npo.currentSmoothPosition.TargetPosition.z);
		if (this.blockGridPositions.Contains(gridPos))
		{
			npo.StopSmoothMove(false);
			yield break;
		}
		this.blockGridPositions.Add(gridPos);
		yield return null;
		this.blockGridPositions.Remove(gridPos);
		yield break;
	}

	// Token: 0x06001558 RID: 5464 RVA: 0x0008BE64 File Offset: 0x0008A064
	public void SnapToGrid(GameObject grabbable, bool blockPos = false, float threshold = 6.5f)
	{
		if (!grabbable)
		{
			return;
		}
		float num = 1f;
		NetworkPhysicsObject component = grabbable.GetComponent<NetworkPhysicsObject>();
		Vector3 position = component.rigidbody.position;
		if (component.IsLocked)
		{
			return;
		}
		if (component.rigidbody.velocity.magnitude > threshold)
		{
			return;
		}
		bool flag = !component.IgnoresGrid && ConfigGame.Settings.Snapping;
		if (!component.IgnoresSnap && ConfigGame.Settings.Snapping)
		{
			float num2 = 0f;
			SnapPoint snapPoint = null;
			Bounds boundsNotNormalized = component.GetBoundsNotNormalized();
			float x = Mathf.Clamp(boundsNotNormalized.extents.x * 1.25f, 1f, 2f);
			float z = Mathf.Clamp(boundsNotNormalized.extents.z * 1.25f, 1f, 2f);
			Vector3 checkSize = new Vector3(x, 10f, z);
			foreach (SnapPointManager.SnapPointObject snapPointObject in NetworkSingleton<SnapPointManager>.Instance.SnapPointObjects)
			{
				SnapPoint snapPoint2 = snapPointObject.snapPoint;
				float num3;
				if (!(snapPoint2.transform.root == grabbable.transform) && (!component || snapPoint2.TagsAllowActingUpon(component)) && LibMath.CloseEnoughXYZ(position, snapPoint2.transform.position, checkSize, out num3) && position.y + 0.5f > snapPoint2.transform.position.y && (!snapPoint || num3 < num2))
				{
					num2 = num3;
					snapPoint = snapPoint2;
				}
			}
			if (snapPoint)
			{
				Vector3 position2 = new Vector3(snapPoint.transform.position.x, (position.y * num > snapPoint.transform.position.y) ? (position.y * num) : position.y, snapPoint.transform.position.z);
				component.SetSmoothPosition(position2, true, true, false, true, null, false, false, null);
				if (snapPoint.bRotate)
				{
					component.SetSmoothRotation(new Vector3(grabbable.transform.eulerAngles.x, snapPoint.transform.eulerAngles.y, grabbable.transform.eulerAngles.z), true, false, false, true, null, false);
				}
				else if (component.CompareTag("Jigsaw"))
				{
					for (float num4 = 0f; num4 < 360f; num4 += 90f)
					{
						if (Utilities.approxAngle(component.transform.eulerAngles.y, num4, 45f))
						{
							component.SetSmoothRotation(new Vector3(0f, num4, 0f), true, false, false, true, null, false);
							break;
						}
					}
				}
				if (blockPos)
				{
					base.StartCoroutine(this.DelayBlockGridPos(component));
				}
				return;
			}
		}
		GridOptions instance = NetworkSingleton<GridOptions>.Instance;
		if (flag && (component.HasInternalGrid || instance.gridState.Snapping || instance.gridState.Offset || instance.gridState.BothSnapping))
		{
			float num5 = 0f;
			float num6 = 0f;
			bool flag2;
			float num7;
			float num8;
			if (component.HasInternalGrid)
			{
				flag2 = component.InternalGridIsOffset;
				num7 = component.InternalGridSize;
				num8 = component.InternalGridSize;
			}
			else
			{
				flag2 = instance.gridState.Offset;
				num7 = instance.gridState.xSize;
				num8 = instance.gridState.ySize;
			}
			if (flag2 && instance.gridState.Type == GridType.Box)
			{
				num5 = num7 / 2f;
				num6 = num8 / 2f;
			}
			num5 -= instance.gridState.PosOffset.x;
			num6 -= instance.gridState.PosOffset.z;
			if (instance.gridState.Type == GridType.Box || component.HasInternalGrid)
			{
				Vector3 vector = new Vector3((float)Math.Round((double)((position.x + num5) / num7)) * num7 - num5, position.y * num, (float)Math.Round((double)((position.z + num6) / num8)) * num8 - num6);
				if (instance.gridState.BothSnapping)
				{
					num5 = num7 / 2f;
					num6 = num8 / 2f;
					num5 -= instance.gridState.PosOffset.x;
					num6 -= instance.gridState.PosOffset.z;
					Vector3 vector2 = new Vector3((float)Math.Round((double)((position.x + num5) / num7)) * num7 - num5, position.y * num, (float)Math.Round((double)((position.z + num6) / num8)) * num8 - num6);
					if (Vector3.Distance(position, vector2) < Vector3.Distance(position, vector))
					{
						vector = vector2;
					}
				}
				component.SetSmoothPosition(vector, true, true, false, true, null, false, false, null);
			}
			else if (instance.gridState.Type == GridType.HexHorizontal)
			{
				float num9 = num7 * 0.75f;
				float num10 = (float)Math.Sqrt(3.0) * num8 / 2f;
				float x2 = instance.gridState.PosOffset.x;
				float z2 = instance.gridState.PosOffset.z;
				int num11 = (int)Math.Round((double)((position.x - x2) / num9));
				float num12 = 0f;
				if (num11 % 2 != 0)
				{
					num12 = num10 / 2f;
				}
				int num13 = (int)Math.Round((double)((position.z - z2 - num12) / num10));
				Vector3 vector3 = new Vector3((float)num11 * num9 + x2, position.y * num, (float)num13 * num10 + z2 + num12);
				if (instance.gridState.Snapping || instance.gridState.BothSnapping)
				{
					float num14 = num7 / 2f;
					float num15 = num14 / 2f;
					float num16 = num10 / 2f;
					Vector3 vector5;
					Vector3 vector4 = vector5 = vector3;
					vector5.x += num14;
					if (!instance.gridState.BothSnapping)
					{
						vector3 = vector5;
					}
					if (Vector3.Distance(position, vector3) > Vector3.Distance(position, vector5))
					{
						vector3 = vector5;
					}
					Vector3 vector6 = vector4;
					vector6.x -= num14;
					if (Vector3.Distance(position, vector3) > Vector3.Distance(position, vector6))
					{
						vector3 = vector6;
					}
					Vector3 vector7 = vector4;
					vector7.z += num16;
					vector7.x += num15;
					if (Vector3.Distance(position, vector3) > Vector3.Distance(position, vector7))
					{
						vector3 = vector7;
					}
					Vector3 vector8 = vector4;
					vector8.z += num16;
					vector8.x -= num15;
					if (Vector3.Distance(position, vector3) > Vector3.Distance(position, vector8))
					{
						vector3 = vector8;
					}
					Vector3 vector9 = vector4;
					vector9.z -= num16;
					vector9.x += num15;
					if (Vector3.Distance(position, vector3) > Vector3.Distance(position, vector9))
					{
						vector3 = vector9;
					}
					Vector3 vector10 = vector4;
					vector10.z -= num16;
					vector10.x -= num15;
					if (Vector3.Distance(position, vector3) > Vector3.Distance(position, vector10))
					{
						vector3 = vector10;
					}
				}
				component.SetSmoothPosition(vector3, true, true, false, true, null, false, false, null);
			}
			else if (instance.gridState.Type == GridType.HexVertical)
			{
				float num17 = num8 * 0.75f;
				float num18 = (float)Math.Sqrt(3.0) * num7 / 2f;
				float x3 = instance.gridState.PosOffset.x;
				float z3 = instance.gridState.PosOffset.z;
				int num19 = (int)Math.Round((double)((position.z - z3) / num17));
				float num20 = 0f;
				if (num19 % 2 != 0)
				{
					num20 = num18 / 2f;
				}
				int num21 = (int)Math.Round((double)((position.x - x3 - num20) / num18));
				Vector3 vector11 = new Vector3((float)num21 * num18 + x3 + num20, position.y * num, (float)num19 * num17 + z3);
				if (instance.gridState.Snapping || instance.gridState.BothSnapping)
				{
					float num22 = num8 / 2f;
					float num23 = num18 / 2f;
					float num24 = num22 / 2f;
					Vector3 vector13;
					Vector3 vector12 = vector13 = vector11;
					vector13.z += num22;
					if (!instance.gridState.BothSnapping)
					{
						vector11 = vector13;
					}
					if (Vector3.Distance(position, vector11) > Vector3.Distance(position, vector13))
					{
						vector11 = vector13;
					}
					Vector3 vector14 = vector12;
					vector14.z -= num22;
					if (Vector3.Distance(position, vector11) > Vector3.Distance(position, vector14))
					{
						vector11 = vector14;
					}
					Vector3 vector15 = vector12;
					vector15.z += num24;
					vector15.x += num23;
					if (Vector3.Distance(position, vector11) > Vector3.Distance(position, vector15))
					{
						vector11 = vector15;
					}
					Vector3 vector16 = vector12;
					vector16.z += num24;
					vector16.x -= num23;
					if (Vector3.Distance(position, vector11) > Vector3.Distance(position, vector16))
					{
						vector11 = vector16;
					}
					Vector3 vector17 = vector12;
					vector17.z -= num24;
					vector17.x += num23;
					if (Vector3.Distance(position, vector11) > Vector3.Distance(position, vector17))
					{
						vector11 = vector17;
					}
					Vector3 vector18 = vector12;
					vector18.z -= num24;
					vector18.x -= num23;
					if (Vector3.Distance(position, vector11) > Vector3.Distance(position, vector18))
					{
						vector11 = vector18;
					}
				}
				component.SetSmoothPosition(vector11, true, true, false, true, null, false, false, null);
			}
			if (blockPos)
			{
				base.StartCoroutine(this.DelayBlockGridPos(component));
			}
		}
	}

	// Token: 0x06001559 RID: 5465 RVA: 0x0008C814 File Offset: 0x0008AA14
	public bool CheckPointSnap(GameObject grabbable, out Vector3 position, out Vector3 rotation, float threshold = 6.5f)
	{
		return this.CheckPointSnap(grabbable, null, out position, out rotation, threshold);
	}

	// Token: 0x0600155A RID: 5466 RVA: 0x0008C822 File Offset: 0x0008AA22
	public bool CheckPointSnap(NetworkPhysicsObject npo, out Vector3 position, out Vector3 rotation, float threshold = 6.5f)
	{
		return this.CheckPointSnap(npo.gameObject, npo, out position, out rotation, threshold);
	}

	// Token: 0x0600155B RID: 5467 RVA: 0x0008C838 File Offset: 0x0008AA38
	private bool CheckPointSnap(GameObject grabbable, NetworkPhysicsObject npo, out Vector3 position, out Vector3 rotation, float threshold)
	{
		position = grabbable.transform.position;
		rotation = grabbable.transform.rotation.eulerAngles;
		NetworkPhysicsObject networkPhysicsObject = (npo != null) ? npo : grabbable.GetComponent<NetworkPhysicsObject>();
		if (networkPhysicsObject && (networkPhysicsObject.IgnoresSnap || !ConfigGame.Settings.Snapping || networkPhysicsObject.IsLocked || networkPhysicsObject.rigidbody.velocity.magnitude > threshold))
		{
			return false;
		}
		float num = 1f;
		Vector3 position2 = grabbable.transform.position;
		float num2 = 0f;
		SnapPoint snapPoint = null;
		Bounds bounds = networkPhysicsObject ? networkPhysicsObject.GetBoundsNotNormalized() : new Bounds(Vector3.zero, Vector3.one);
		float x = Mathf.Clamp(bounds.extents.x * 1.25f, 1f, 2f);
		float z = Mathf.Clamp(bounds.extents.z * 1.25f, 1f, 2f);
		Vector3 checkSize = new Vector3(x, 10f, z);
		foreach (SnapPointManager.SnapPointObject snapPointObject in NetworkSingleton<SnapPointManager>.Instance.SnapPointObjects)
		{
			SnapPoint snapPoint2 = snapPointObject.snapPoint;
			float num3;
			if (!(snapPoint2.transform.root == grabbable.transform) && (!networkPhysicsObject || snapPoint2.TagsAllowActingUpon(networkPhysicsObject)) && LibMath.CloseEnoughXYZ(position2, snapPoint2.transform.position, checkSize, out num3) && position2.y + 0.5f > snapPoint2.transform.position.y && (!snapPoint || num3 < num2))
			{
				num2 = num3;
				snapPoint = snapPoint2;
			}
		}
		if (snapPoint)
		{
			position = new Vector3(snapPoint.transform.position.x, (position2.y * num > snapPoint.transform.position.y) ? (position2.y * num) : position2.y, snapPoint.transform.position.z);
			if (snapPoint.GetComponent<SnapPoint>().bRotate)
			{
				rotation = new Vector3(grabbable.transform.eulerAngles.x, snapPoint.transform.eulerAngles.y, grabbable.transform.eulerAngles.z);
			}
			else if (grabbable.CompareTag("Jigsaw"))
			{
				for (float num4 = 0f; num4 < 360f; num4 += 90f)
				{
					if (Utilities.approxAngle(grabbable.transform.eulerAngles.y, num4, 45f))
					{
						rotation = new Vector3(0f, num4, 0f);
						break;
					}
				}
			}
			return true;
		}
		return false;
	}

	// Token: 0x0600155C RID: 5468 RVA: 0x0008CB30 File Offset: 0x0008AD30
	public bool CheckGridSnap(GameObject grabbable, out Vector3 position, float threshold = 6.5f)
	{
		position = grabbable.transform.position;
		NetworkPhysicsObject component = grabbable.GetComponent<NetworkPhysicsObject>();
		return !component.IgnoresGrid && ConfigGame.Settings.Snapping && !component.IsLocked && component.rigidbody.velocity.magnitude <= threshold && this.CheckGridSnap(position, out position);
	}

	// Token: 0x0600155D RID: 5469 RVA: 0x0008CB98 File Offset: 0x0008AD98
	public bool CheckGridSnap(Vector3 objectPosition, out Vector3 snappedPosition)
	{
		snappedPosition = objectPosition;
		GridOptions instance = NetworkSingleton<GridOptions>.Instance;
		if (instance.gridState.Snapping || instance.gridState.Offset || instance.gridState.BothSnapping)
		{
			float num = 0f;
			float num2 = 0f;
			float num3 = 1f;
			bool offset = instance.gridState.Offset;
			float xSize = instance.gridState.xSize;
			float ySize = instance.gridState.ySize;
			if (offset && instance.gridState.Type == GridType.Box)
			{
				num = xSize / 2f;
				num2 = ySize / 2f;
			}
			num -= instance.gridState.PosOffset.x;
			num2 -= instance.gridState.PosOffset.z;
			if (instance.gridState.Type == GridType.Box)
			{
				snappedPosition = new Vector3((float)Math.Round((double)((objectPosition.x + num) / xSize)) * xSize - num, objectPosition.y * num3, (float)Math.Round((double)((objectPosition.z + num2) / ySize)) * ySize - num2);
				if (instance.gridState.BothSnapping)
				{
					num = xSize / 2f;
					num2 = ySize / 2f;
					num -= instance.gridState.PosOffset.x;
					num2 -= instance.gridState.PosOffset.z;
					Vector3 vector = new Vector3((float)Math.Round((double)((objectPosition.x + num) / xSize)) * xSize - num, objectPosition.y * num3, (float)Math.Round((double)((objectPosition.z + num2) / ySize)) * ySize - num2);
					if (Vector3.Distance(objectPosition, vector) < Vector3.Distance(objectPosition, snappedPosition))
					{
						snappedPosition = vector;
					}
				}
			}
			else if (instance.gridState.Type == GridType.HexHorizontal)
			{
				float num4 = xSize * 0.75f;
				float num5 = (float)Math.Sqrt(3.0) * ySize / 2f;
				float x = instance.gridState.PosOffset.x;
				float z = instance.gridState.PosOffset.z;
				int num6 = (int)Math.Round((double)((objectPosition.x - x) / num4));
				float num7 = 0f;
				if (num6 % 2 != 0)
				{
					num7 = num5 / 2f;
				}
				int num8 = (int)Math.Round((double)((objectPosition.z - z - num7) / num5));
				snappedPosition = new Vector3((float)num6 * num4 + x, objectPosition.y * num3, (float)num8 * num5 + z + num7);
				if (instance.gridState.Snapping || instance.gridState.BothSnapping)
				{
					float num9 = xSize / 2f;
					float num10 = num9 / 2f;
					float num11 = num5 / 2f;
					Vector3 vector3;
					Vector3 vector2 = vector3 = snappedPosition;
					vector3.x += num9;
					if (!instance.gridState.BothSnapping)
					{
						snappedPosition = vector3;
					}
					if (Vector3.Distance(objectPosition, snappedPosition) > Vector3.Distance(objectPosition, vector3))
					{
						snappedPosition = vector3;
					}
					Vector3 vector4 = vector2;
					vector4.x -= num9;
					if (Vector3.Distance(objectPosition, snappedPosition) > Vector3.Distance(objectPosition, vector4))
					{
						snappedPosition = vector4;
					}
					Vector3 vector5 = vector2;
					vector5.z += num11;
					vector5.x += num10;
					if (Vector3.Distance(objectPosition, snappedPosition) > Vector3.Distance(objectPosition, vector5))
					{
						snappedPosition = vector5;
					}
					Vector3 vector6 = vector2;
					vector6.z += num11;
					vector6.x -= num10;
					if (Vector3.Distance(objectPosition, snappedPosition) > Vector3.Distance(objectPosition, vector6))
					{
						snappedPosition = vector6;
					}
					Vector3 vector7 = vector2;
					vector7.z -= num11;
					vector7.x += num10;
					if (Vector3.Distance(objectPosition, snappedPosition) > Vector3.Distance(objectPosition, vector7))
					{
						snappedPosition = vector7;
					}
					Vector3 vector8 = vector2;
					vector8.z -= num11;
					vector8.x -= num10;
					if (Vector3.Distance(objectPosition, snappedPosition) > Vector3.Distance(objectPosition, vector8))
					{
						snappedPosition = vector8;
					}
				}
			}
			else if (instance.gridState.Type == GridType.HexVertical)
			{
				float num12 = ySize * 0.75f;
				float num13 = (float)Math.Sqrt(3.0) * xSize / 2f;
				float x2 = instance.gridState.PosOffset.x;
				float z2 = instance.gridState.PosOffset.z;
				int num14 = (int)Math.Round((double)((objectPosition.z - z2) / num12));
				float num15 = 0f;
				if (num14 % 2 != 0)
				{
					num15 = num13 / 2f;
				}
				int num16 = (int)Math.Round((double)((objectPosition.x - x2 - num15) / num13));
				snappedPosition = new Vector3((float)num16 * num13 + x2 + num15, objectPosition.y * num3, (float)num14 * num12 + z2);
				if (instance.gridState.Snapping || instance.gridState.BothSnapping)
				{
					float num17 = ySize / 2f;
					float num18 = num13 / 2f;
					float num19 = num17 / 2f;
					Vector3 vector10;
					Vector3 vector9 = vector10 = snappedPosition;
					vector10.z += num17;
					if (!instance.gridState.BothSnapping)
					{
						snappedPosition = vector10;
					}
					if (Vector3.Distance(objectPosition, snappedPosition) > Vector3.Distance(objectPosition, vector10))
					{
						snappedPosition = vector10;
					}
					Vector3 vector11 = vector9;
					vector11.z -= num17;
					if (Vector3.Distance(objectPosition, snappedPosition) > Vector3.Distance(objectPosition, vector11))
					{
						snappedPosition = vector11;
					}
					Vector3 vector12 = vector9;
					vector12.z += num19;
					vector12.x += num18;
					if (Vector3.Distance(objectPosition, snappedPosition) > Vector3.Distance(objectPosition, vector12))
					{
						snappedPosition = vector12;
					}
					Vector3 vector13 = vector9;
					vector13.z += num19;
					vector13.x -= num18;
					if (Vector3.Distance(objectPosition, snappedPosition) > Vector3.Distance(objectPosition, vector13))
					{
						snappedPosition = vector13;
					}
					Vector3 vector14 = vector9;
					vector14.z -= num19;
					vector14.x += num18;
					if (Vector3.Distance(objectPosition, snappedPosition) > Vector3.Distance(objectPosition, vector14))
					{
						snappedPosition = vector14;
					}
					Vector3 vector15 = vector9;
					vector15.z -= num19;
					vector15.x -= num18;
					if (Vector3.Distance(objectPosition, snappedPosition) > Vector3.Distance(objectPosition, vector15))
					{
						snappedPosition = vector15;
					}
				}
			}
			return true;
		}
		return false;
	}

	// Token: 0x0600155E RID: 5470 RVA: 0x0008D221 File Offset: 0x0008B421
	public Vector3 GridSnapPosition(Vector3 position)
	{
		this.CheckGridSnap(position, out position);
		return position;
	}

	// Token: 0x0600155F RID: 5471 RVA: 0x0008D230 File Offset: 0x0008B430
	public void ClientTakeObject(int grabbedID, int playerID, int touchID = -1)
	{
		StackObject stackObject = null;
		for (int i = 0; i < this.GrabbableNPOs.Count; i++)
		{
			NetworkPhysicsObject networkPhysicsObject = this.GrabbableNPOs[i];
			if (networkPhysicsObject.ID == grabbedID && !networkPhysicsObject.HeldIDIndicatesDestruction)
			{
				stackObject = networkPhysicsObject.GetComponent<StackObject>();
				if (stackObject)
				{
					break;
				}
			}
		}
		if (stackObject == null || stackObject.AnyoneSearchingBag())
		{
			return;
		}
		Pointer pointer = this.PointerFromID(playerID);
		if (pointer == null || pointer.num_grabbed >= 128)
		{
			return;
		}
		GameObject gameObject = stackObject.TakeObject(true);
		if (gameObject == null)
		{
			return;
		}
		if (stackObject.num_objects_ <= 1 && !stackObject.bBag)
		{
			this.DestroyThisObject(stackObject.gameObject);
		}
		NetworkPhysicsObject networkPhysicsObject2 = this.NPOFromGO(gameObject);
		if (networkPhysicsObject2 == null)
		{
			return;
		}
		networkPhysicsObject2.bReduceForce = false;
		networkPhysicsObject2.PrevHeldByPlayerID = playerID;
		networkPhysicsObject2.HeldByPlayerID = playerID;
		networkPhysicsObject2.HeldByTouchID = touchID;
		networkPhysicsObject2.HeldSpinRotationIndex = this.SpinRotationIndexFromGrabbable(gameObject, (float)pointer.RotationSnap);
		networkPhysicsObject2.HeldFlipRotationIndex = this.FlipRotationIndexFromGrabbable(gameObject, (float)pointer.RotationSnap);
		pointer.num_grabbed++;
		if (NetworkSingleton<PlayerManager>.Instance.PlayerStateFromID(playerID).isVR || !pointer.FirstGrabbedObject || pointer.FirstGrabbedObject == gameObject)
		{
			pointer.FirstGrabbedObject = gameObject;
			pointer.UpdateFirstGrabbedNPO(networkPhysicsObject2);
		}
		if (gameObject.GetComponent<SoundScript>())
		{
			gameObject.GetComponent<SoundScript>().PickUpSound();
		}
	}

	// Token: 0x06001560 RID: 5472 RVA: 0x0008D3A4 File Offset: 0x0008B5A4
	public void ClientCardPeel(int grabbedID, int playerID, int touchID = -1)
	{
		DeckScript deckScript = null;
		for (int i = 0; i < this.GrabbableNPOs.Count; i++)
		{
			NetworkPhysicsObject networkPhysicsObject = this.GrabbableNPOs[i];
			if (networkPhysicsObject.ID == grabbedID && !networkPhysicsObject.HeldIDIndicatesDestruction)
			{
				deckScript = networkPhysicsObject.GetComponent<DeckScript>();
				if (deckScript)
				{
					break;
				}
			}
		}
		if (!deckScript || deckScript.AnyoneSearchingDeck())
		{
			return;
		}
		Pointer pointer = this.PointerFromID(playerID);
		if (pointer == null || pointer.num_grabbed >= 128)
		{
			return;
		}
		bool isVR = NetworkSingleton<PlayerManager>.Instance.PlayerStateFromID(playerID).isVR;
		GameObject gameObject;
		if ((deckScript.GetComponent<Rigidbody>().rotation * new Vector3(0f, 1f, 0f)).y <= 0f)
		{
			gameObject = deckScript.GetComponent<DeckScript>().TakeCard(true, true);
		}
		else
		{
			gameObject = deckScript.GetComponent<DeckScript>().TakeCard(false, true);
		}
		NetworkPhysicsObject networkPhysicsObject2 = this.NPOFromGO(gameObject);
		networkPhysicsObject2.bReduceForce = false;
		networkPhysicsObject2.PrevHeldByPlayerID = playerID;
		networkPhysicsObject2.HeldByPlayerID = playerID;
		networkPhysicsObject2.HeldByTouchID = touchID;
		networkPhysicsObject2.HeldSpinRotationIndex = this.SpinRotationIndexFromGrabbable(gameObject, isVR ? 15f : ((float)pointer.RotationSnap));
		networkPhysicsObject2.HeldFlipRotationIndex = this.FlipRotationIndexFromGrabbable(gameObject, isVR ? 15f : ((float)pointer.RotationSnap));
		if (isVR)
		{
			NetworkTracked networkTracked = pointer.pointerSyncs.NetworkTrackedFromID(touchID);
			Vector3 a = pointer.pointerSyncs.PositionFromID(touchID);
			Quaternion heldByControllerPickupRotation = pointer.pointerSyncs.RotationFromID(touchID);
			networkPhysicsObject2.HeldOffset = Vector3.zero;
			networkPhysicsObject2.HeldByControllerPickupOffset = networkTracked.transform.InverseTransformPoint(a + networkPhysicsObject2.HeldOffset);
			networkPhysicsObject2.HeldByControllerPickupRotation = heldByControllerPickupRotation;
			networkPhysicsObject2.ResetVRRotations();
		}
		pointer.num_grabbed++;
		if (isVR || !pointer.FirstGrabbedObject || pointer.FirstGrabbedObject == gameObject)
		{
			pointer.FirstGrabbedObject = gameObject;
			pointer.UpdateFirstGrabbedNPO(networkPhysicsObject2);
		}
		if (gameObject.GetComponent<SoundScript>())
		{
			gameObject.GetComponent<SoundScript>().PickUpSound();
		}
	}

	// Token: 0x06001561 RID: 5473 RVA: 0x0008D5B4 File Offset: 0x0008B7B4
	public Vector3 ApplyPointerOffest(Transform pointerTransform, Vector3 setPosition)
	{
		Vector3 forward = pointerTransform.forward;
		Vector3 right = pointerTransform.right;
		Vector3 b = new Vector3(-0.6f * forward.normalized.x * -1f + -0.15f * right.normalized.x, -0.6f * forward.normalized.y * -1f + -0.15f * right.normalized.y, -0.6f * forward.normalized.z * -1f + -0.15f * right.normalized.z);
		return setPosition + b;
	}

	// Token: 0x06001562 RID: 5474 RVA: 0x0008D660 File Offset: 0x0008B860
	public static bool CloseEnoughForContainerMerge(NetworkPhysicsObject bottom, NetworkPhysicsObject top)
	{
		Bounds boundsNotNormalized = bottom.GetBoundsNotNormalized();
		Bounds boundsNotNormalized2 = top.GetBoundsNotNormalized();
		Vector3 center = boundsNotNormalized.center;
		Vector3 center2 = boundsNotNormalized2.center;
		float x = boundsNotNormalized.extents.x * 0.85f + boundsNotNormalized2.extents.x * 0.4f;
		float z = boundsNotNormalized.extents.z * 0.85f + boundsNotNormalized2.extents.z * 0.4f;
		float y = boundsNotNormalized.size.y + boundsNotNormalized2.size.y;
		Vector3 checkSize = new Vector3(x, y, z);
		float num;
		return LibMath.CloseEnoughXYZ(center, center2, checkSize, out num);
	}

	// Token: 0x06001563 RID: 5475 RVA: 0x0008D708 File Offset: 0x0008B908
	public static bool CloseEnoughForCardMerge(NetworkPhysicsObject npo1, NetworkPhysicsObject npo2, bool checkRotatation = false, bool easyCheck = false)
	{
		float num = easyCheck ? 2f : 1f;
		num *= (npo1.ScaleAverage + npo2.ScaleAverage) / 2f;
		float num2 = 0.5f * num;
		Transform transform = npo1.transform;
		Transform transform2 = npo2.transform;
		Vector3 lhs = transform.position - transform2.position;
		return Mathf.Abs(Vector3.Dot(lhs, transform.forward)) < num2 && Mathf.Abs(Vector3.Dot(lhs, transform.right)) < num2 && Mathf.Abs(Vector3.Dot(lhs, transform2.forward)) < num2 && Mathf.Abs(Vector3.Dot(lhs, transform2.right)) < num2 && (!checkRotatation || Mathf.Abs(Vector3.Dot(transform.forward, transform2.forward)) > 0.5f);
	}

	// Token: 0x06001564 RID: 5476 RVA: 0x0008D7DC File Offset: 0x0008B9DC
	public static bool CloseEnoughCardStickyHorizontal(NetworkPhysicsObject npo1, NetworkPhysicsObject npo2, bool checkRotation = false)
	{
		float num = (npo1.ScaleAverage + npo2.ScaleAverage) / 2f;
		float num2 = 0.5f * num;
		float num3 = num2 * 2.25f;
		Vector3 lhs = npo1.transform.position - npo2.transform.position;
		return Mathf.Abs(Vector3.Dot(lhs, npo1.transform.forward)) < num2 && Mathf.Abs(Vector3.Dot(lhs, npo1.transform.right)) > num2 && Mathf.Abs(Vector3.Dot(lhs, npo1.transform.right)) < num3 && Mathf.Abs(Vector3.Dot(lhs, npo2.transform.forward)) < num2 && Mathf.Abs(Vector3.Dot(lhs, npo2.transform.right)) > num2 && Mathf.Abs(Vector3.Dot(lhs, npo2.transform.right)) < num3 && (!checkRotation || Mathf.Abs(Vector3.Dot(npo1.transform.forward, npo2.transform.forward)) > 0.5f);
	}

	// Token: 0x06001565 RID: 5477 RVA: 0x0008D8F4 File Offset: 0x0008BAF4
	public static bool CloseEnoughCardStickyVertical(NetworkPhysicsObject npo1, NetworkPhysicsObject npo2, bool checkRotation = false)
	{
		float num = (npo1.ScaleAverage + npo2.ScaleAverage) / 2f;
		float num2 = 0.5f * num;
		float num3 = num2 * 3f;
		Vector3 lhs = npo1.transform.position - npo2.transform.position;
		return Mathf.Abs(Vector3.Dot(lhs, npo1.transform.right)) < num2 && Mathf.Abs(Vector3.Dot(lhs, npo1.transform.forward)) > num2 && Mathf.Abs(Vector3.Dot(lhs, npo1.transform.forward)) < num3 && Mathf.Abs(Vector3.Dot(lhs, npo2.transform.right)) < num2 && Mathf.Abs(Vector3.Dot(lhs, npo2.transform.forward)) > num2 && Mathf.Abs(Vector3.Dot(lhs, npo2.transform.forward)) < num3 && (!checkRotation || Mathf.Abs(Vector3.Dot(npo1.transform.forward, npo2.transform.forward)) > 0.5f);
	}

	// Token: 0x06001566 RID: 5478 RVA: 0x0008DA0C File Offset: 0x0008BC0C
	public NetworkPhysicsObject NPOHitNPO(NetworkPhysicsObject npo1, NetworkPhysicsObject npo2, bool checkDistanceAndRotation = true)
	{
		if (npo1.IsDestroyed || npo2.IsDestroyed || npo1.IsLocked || npo2.IsLocked || npo1.InsideALayoutZone || npo2.InsideALayoutZone || (npo1.transform.position.y > npo2.transform.position.y && checkDistanceAndRotation))
		{
			return null;
		}
		if (checkDistanceAndRotation && !ManagerPhysicsObject.CloseEnoughForContainerMerge(npo1, npo2))
		{
			return null;
		}
		LuaGameObjectScript luaGameObjectScript = npo2.luaGameObjectScript;
		if (luaGameObjectScript != null && !luaGameObjectScript.CheckObjectEnter(npo1))
		{
			return null;
		}
		GameObject gameObject = null;
		NetworkPhysicsObject networkPhysicsObject = null;
		if (npo1.CompareTag("Chip") && !npo1.GetComponent<CustomMesh>())
		{
			ObjectState objectState = this.SaveObjectState(npo1);
			objectState.Name = NetworkSingleton<GameMode>.Instance.PokerStack.name;
			gameObject = this.LoadObjectState(objectState, false, false);
			networkPhysicsObject = this.NPOFromGO(gameObject);
			gameObject.GetComponent<StackObject>().SetPokerChip(int.Parse(npo1.name.Substring(5, npo1.name.Length - 12)));
		}
		else if (npo1.CompareTag("Checker"))
		{
			ObjectState objectState2 = this.SaveObjectState(npo1);
			string name = objectState2.Name;
			objectState2.Name = NetworkSingleton<GameMode>.Instance.CheckerStack.name;
			gameObject = this.LoadObjectState(objectState2, false, false);
			networkPhysicsObject = this.NPOFromGO(gameObject);
			int checker = 0;
			if (name == "Checker_black")
			{
				checker = 1;
			}
			else if (name == "Checker_white")
			{
				checker = 2;
			}
			gameObject.GetComponent<StackObject>().SetChecker(checker);
			if (objectState2.ColorDiffuse != null)
			{
				networkPhysicsObject.DiffuseColor = objectState2.ColorDiffuse.Value.ToColour();
			}
		}
		else if (npo1.GetComponent<CustomObject>())
		{
			ObjectState objectState3 = this.SaveObjectState(npo1);
			ObjectState objectState4 = objectState3;
			objectState4.Name += "_Stack";
			gameObject = this.LoadObjectState(objectState3, false, false);
			networkPhysicsObject = this.NPOFromGO(gameObject);
		}
		else if (npo1.CompareTag("Stack"))
		{
			ObjectState objectState5 = this.SaveObjectState(npo1);
			objectState5.Name += "Stack";
			gameObject = this.LoadObjectState(objectState5, false, false);
			networkPhysicsObject = this.NPOFromGO(gameObject);
		}
		if (!gameObject)
		{
			return null;
		}
		if (npo1.HeldByPlayerID == npo2.HeldByPlayerID)
		{
			networkPhysicsObject.HeldByPlayerID = npo1.HeldByPlayerID;
		}
		EventManager.TriggerObjectEnterContainer(networkPhysicsObject, npo1);
		EventManager.TriggerObjectEnterContainer(networkPhysicsObject, npo2);
		this.DestroyThisObject(npo1.gameObject);
		this.DestroyThisObject(npo2.gameObject);
		if (checkDistanceAndRotation)
		{
			int prevHeldByPlayerID = npo2.PrevHeldByPlayerID;
			Pointer pointer = this.PointerFromID(prevHeldByPlayerID);
			if (pointer)
			{
				List<NetworkPhysicsObject> recentlyDropped = pointer.RecentlyDropped;
				for (int i = 0; i < recentlyDropped.Count; i++)
				{
					NetworkPhysicsObject networkPhysicsObject2 = recentlyDropped[i];
					if (networkPhysicsObject2 && !(networkPhysicsObject2 == npo1) && !(networkPhysicsObject2 == npo2) && ((networkPhysicsObject2.stackObject && networkPhysicsObject2.stackObject.CheckStackable(gameObject)) || (networkPhysicsObject2.GetComponent<CheckStackObject>() && gameObject.GetComponent<StackObject>().CheckStackable(networkPhysicsObject2))))
					{
						this.StackHitNPO(networkPhysicsObject, networkPhysicsObject2, false);
					}
				}
			}
		}
		return networkPhysicsObject;
	}

	// Token: 0x06001567 RID: 5479 RVA: 0x0008DD4C File Offset: 0x0008BF4C
	public NetworkPhysicsObject StackHitNPO(NetworkPhysicsObject npo1, NetworkPhysicsObject npo2, bool checkAllDroppedObjects = true)
	{
		if (npo1.IsDestroyed || npo2.IsDestroyed || npo1.IsLocked || npo2.IsLocked || npo1.InsideALayoutZone || npo2.InsideALayoutZone)
		{
			return null;
		}
		StackObject stackObject = npo1.stackObject;
		StackObject stackObject2 = npo2.stackObject;
		if ((stackObject && stackObject.AnyoneSearchingBag()) || (stackObject2 && stackObject2.AnyoneSearchingBag()))
		{
			return null;
		}
		if (!checkAllDroppedObjects || ManagerPhysicsObject.CloseEnoughForContainerMerge(npo1, npo2))
		{
			if (npo1.transform.position.y >= npo2.transform.position.y && stackObject2)
			{
				NetworkPhysicsObject networkPhysicsObject = npo1;
				npo1 = npo2;
				npo2 = networkPhysicsObject;
				stackObject = npo1.stackObject;
				stackObject2 = npo2.stackObject;
			}
			bool flag = true;
			LuaGameObjectScript luaGameObjectScript = npo1.luaGameObjectScript;
			if (luaGameObjectScript != null && !luaGameObjectScript.CheckObjectEnter(npo2))
			{
				flag = false;
			}
			if (flag)
			{
				if (!stackObject2)
				{
					stackObject.num_objects_++;
				}
				else
				{
					stackObject.num_objects_ += stackObject2.num_objects_;
				}
				EventManager.TriggerObjectEnterContainer(npo1, npo2);
			}
			if (checkAllDroppedObjects)
			{
				if (flag)
				{
					this.DestroyThisObject(npo2);
				}
				Pointer pointer = this.PointerFromID(npo2.PrevHeldByPlayerID);
				if (pointer)
				{
					List<NetworkPhysicsObject> recentlyDropped = pointer.RecentlyDropped;
					for (int i = 0; i < recentlyDropped.Count; i++)
					{
						NetworkPhysicsObject networkPhysicsObject2 = recentlyDropped[i];
						if (networkPhysicsObject2 && !(networkPhysicsObject2 == npo1) && !(networkPhysicsObject2 == npo2) && ((networkPhysicsObject2.stackObject && networkPhysicsObject2.stackObject.CheckStackable(npo1)) || (networkPhysicsObject2.GetComponent<CheckStackObject>() && stackObject.CheckStackable(networkPhysicsObject2))))
						{
							this.StackHitNPO(npo1, networkPhysicsObject2, false);
						}
					}
				}
			}
			if (flag)
			{
				if (!checkAllDroppedObjects)
				{
					npo2.SetSmoothDestroy(npo1.transform.position, npo1.transform.rotation);
				}
				return npo1;
			}
		}
		return null;
	}

	// Token: 0x06001568 RID: 5480 RVA: 0x0008DF30 File Offset: 0x0008C130
	public NetworkPhysicsObject NotifyCardHitDeck(NetworkPhysicsObject cardNPO, NetworkPhysicsObject deckNPO, bool checkAllDroppedObjects = true)
	{
		CardScript cardScript = cardNPO.cardScript;
		DeckScript deckScript = deckNPO.deckScript;
		if (cardScript == null || deckScript == null || cardScript.card_id() == -1 || deckScript.GetDeck().Count < 1 || deckScript.AnyoneSearchingDeck() || cardNPO.IsDestroyed || deckNPO.IsDestroyed || cardNPO.IsLocked || deckNPO.IsLocked || cardNPO.GetComponent<MeshFilter>().sharedMesh != deckNPO.GetComponent<MeshFilter>().sharedMesh || cardNPO.InsideALayoutZone || deckNPO.InsideALayoutZone)
		{
			return null;
		}
		Vector2 v = new Vector2(cardNPO.Scale.x, cardNPO.Scale.z);
		Vector2 v2 = new Vector2(deckNPO.Scale.x, deckNPO.Scale.z);
		if (checkAllDroppedObjects && Vector3.Distance(v, v2) > 0.15f)
		{
			return null;
		}
		bool flag = (double)Vector3.Dot(cardNPO.transform.up, deckNPO.transform.up) >= 0.0;
		bool flag2 = ManagerPhysicsObject.CloseEnoughForCardMerge(cardNPO, deckNPO, false, true);
		if (!checkAllDroppedObjects)
		{
			flag = true;
			flag2 = true;
		}
		Bounds bounds = cardNPO.GetBounds();
		Bounds bounds2 = deckNPO.GetBounds();
		bool flag3 = cardNPO.transform.position.y - bounds.extents.y < deckNPO.transform.position.y - bounds2.extents.y + 0.1f;
		if (!cardNPO.IsHeldBySomebody || !deckNPO.IsHeldByNobody || !flag3 || !flag || Mathf.Abs(deckNPO.transform.up.y - cardNPO.transform.up.y) >= 0.03f)
		{
			if (cardNPO.IsHeldByNobody && deckNPO.IsHeldByNobody && flag && flag2)
			{
				bool flag4 = true;
				LuaGameObjectScript luaGameObjectScript = deckNPO.luaGameObjectScript;
				if (luaGameObjectScript != null && !luaGameObjectScript.CheckObjectEnter(cardNPO))
				{
					flag4 = false;
				}
				if (flag4)
				{
					bool top = (double)Vector3.Dot(cardNPO.transform.position - deckNPO.transform.position, deckNPO.transform.up) <= 0.0;
					deckScript.PlayDrawCardSound();
					deckScript.AddCard(top, cardScript.card_id(), this.SaveObjectState(cardNPO));
					cardScript.SetCardID(-1);
					EventManager.TriggerObjectEnterContainer(deckNPO, cardNPO);
				}
				if (checkAllDroppedObjects)
				{
					int playerID = (cardNPO.transform.position.y > deckNPO.transform.position.y) ? cardNPO.PrevHeldByPlayerID : deckNPO.PrevHeldByPlayerID;
					Pointer pointer = this.PointerFromID(playerID);
					if (pointer)
					{
						List<NetworkPhysicsObject> recentlyDropped = pointer.RecentlyDropped;
						for (int i = 0; i < recentlyDropped.Count; i++)
						{
							NetworkPhysicsObject networkPhysicsObject = recentlyDropped[i];
							if (networkPhysicsObject && !(networkPhysicsObject == cardNPO) && !(networkPhysicsObject == deckNPO))
							{
								if (networkPhysicsObject.deckScript)
								{
									this.NotifyDeckHitDeck(deckNPO, networkPhysicsObject, false);
								}
								else if (networkPhysicsObject.cardScript)
								{
									this.NotifyCardHitDeck(networkPhysicsObject, deckNPO, false);
								}
							}
						}
					}
				}
				if (flag4)
				{
					if (checkAllDroppedObjects)
					{
						this.DestroyThisObject(cardNPO.gameObject);
					}
					else
					{
						cardNPO.SetSmoothDestroy(deckNPO.transform.position, deckNPO.transform.rotation);
					}
					return deckNPO;
				}
			}
			return null;
		}
		LuaGameObjectScript luaGameObjectScript2 = deckNPO.luaGameObjectScript;
		if (luaGameObjectScript2 != null && !luaGameObjectScript2.CheckObjectEnter(cardNPO))
		{
			return null;
		}
		deckScript.PlayDrawCardSound();
		deckScript.AddCard(deckNPO.transform.up.y >= 0f, cardScript.card_id(), this.SaveObjectState(cardNPO));
		Vector3 position = new Vector3(deckNPO.transform.position.x, deckNPO.transform.position.y - deckNPO.GetComponent<Renderer>().bounds.size.y / 2f, deckNPO.transform.position.z);
		EventManager.TriggerObjectEnterContainer(deckNPO, cardNPO);
		cardNPO.SetSmoothDestroy(position, deckNPO.transform.rotation);
		return deckNPO;
	}

	// Token: 0x06001569 RID: 5481 RVA: 0x0008E368 File Offset: 0x0008C568
	public NetworkPhysicsObject NotifyCardHitCard(NetworkPhysicsObject npo1, NetworkPhysicsObject npo2, bool checkDistanceAndRotation = true)
	{
		CardScript component = npo1.GetComponent<CardScript>();
		CardScript component2 = npo2.GetComponent<CardScript>();
		if (component.card_id() == -1 || component2.card_id() == -1 || npo1.IsDestroyed || npo2.IsDestroyed || npo1.IsHeldBySomebody || npo2.IsHeldBySomebody || npo1.IsLocked || npo2.IsLocked || npo1.GetComponent<MeshFilter>().sharedMesh != npo2.GetComponent<MeshFilter>().sharedMesh)
		{
			return null;
		}
		Vector2 v = new Vector2(npo1.Scale.x, npo1.Scale.z);
		Vector2 v2 = new Vector2(npo2.Scale.x, npo2.Scale.z);
		if (checkDistanceAndRotation && Vector3.Distance(v, v2) > 0.15f)
		{
			return null;
		}
		bool flag = (double)Vector3.Dot(npo1.transform.up, npo2.transform.up) >= 0.0;
		bool flag2 = ManagerPhysicsObject.CloseEnoughForCardMerge(npo1, npo2, true, false);
		if (!checkDistanceAndRotation)
		{
			flag = true;
			flag2 = true;
		}
		else if (npo1.transform.position.y > npo2.transform.position.y)
		{
			return null;
		}
		GameObject gameObject = npo1.gameObject;
		GameObject gameObject2 = npo2.gameObject;
		bool flag3 = false;
		if (npo1.InsideALayoutZone && npo2.InsideALayoutZone)
		{
			LayoutZone layoutZone;
			int num;
			LayoutZone y;
			int num2;
			if (LayoutZone.TryNPOInLayoutZone(npo1, out layoutZone, out num, LayoutZone.PotentialZoneCheck.None) && LayoutZone.TryNPOInLayoutZone(npo2, out y, out num2, LayoutZone.PotentialZoneCheck.None))
			{
				flag3 = (layoutZone == y && num == num2);
			}
			if (flag3 && !layoutZone.Options.StickyCards)
			{
				return null;
			}
		}
		if (!flag3 && (npo1.InsideALayoutZone || npo2.InsideALayoutZone))
		{
			return null;
		}
		if ((flag3 || ManagerPhysicsObject.CloseEnoughCardStickyVertical(npo1, npo2, true) || ManagerPhysicsObject.CloseEnoughCardStickyHorizontal(npo1, npo2, true)) && !npo2.joint && (flag3 || (flag && Mathf.Abs(npo1.transform.up.y - npo2.transform.up.y) < 0.05f)))
		{
			if (component.CardAttachToThis)
			{
				return null;
			}
			component.CardAttachToThis = npo2.gameObject;
			float num3 = npo1.transform.up.normalized.y;
			num3 = num3 * num3 * num3 * num3 * num3;
			if (num3 < 0f)
			{
				num3 = -num3;
			}
			npo2.transform.position = new Vector3(npo2.transform.position.x, (npo1.transform.position.y + 0.05f) * num3, npo2.transform.position.z);
			float y2 = npo2.transform.rotation.eulerAngles.y;
			if ((npo2.transform.right.normalized.x > 0f && npo1.transform.right.normalized.x > 0f) || (npo2.transform.right.normalized.x < 0f && npo1.transform.right.normalized.x < 0f))
			{
				npo2.transform.rotation = npo1.transform.rotation;
			}
			else
			{
				npo2.transform.rotation = npo1.transform.rotation * Quaternion.AngleAxis(180f, Vector3.up);
			}
			npo2.transform.rotation = Quaternion.Euler(new Vector3(npo2.transform.rotation.eulerAngles.x, y2, npo2.transform.rotation.eulerAngles.z));
			gameObject2.AddComponent<FixedJoint>().connectedBody = npo1.GetComponent<Rigidbody>();
			GameObject gameObject3 = gameObject2;
			List<GameObject> list = new List<GameObject>();
			int num4 = 0;
			FixedJoint component3 = gameObject3.GetComponent<FixedJoint>();
			npo2.AddJoint(component3);
			while (component3 && component3.connectedBody)
			{
				num4++;
				if (num4 > 52)
				{
					break;
				}
				component3 = gameObject3.GetComponent<FixedJoint>();
				if (!component3 || !component3.connectedBody)
				{
					break;
				}
				gameObject3 = component3.connectedBody.gameObject;
				list.Add(gameObject3);
				if (npo2.GetComponent<Collider>() != gameObject3.GetComponent<Collider>())
				{
					Physics.IgnoreCollision(npo2.GetComponent<Collider>(), gameObject3.GetComponent<Collider>(), true);
					component2.ignoreColliders.Add(gameObject3.GetComponent<Collider>());
					if (gameObject3.GetComponent<CardScript>())
					{
						gameObject3.GetComponent<CardScript>().ignoreColliders.Add(npo2.GetComponent<Collider>());
					}
				}
			}
			gameObject3 = gameObject2;
			num4 = 0;
			CardScript component4 = gameObject3.GetComponent<CardScript>();
			while (component4 && component4.CardAttachToThis)
			{
				num4++;
				if (num4 > 52)
				{
					break;
				}
				component4 = gameObject3.GetComponent<CardScript>();
				if (!component4 || !component4.CardAttachToThis)
				{
					break;
				}
				gameObject3 = component4.CardAttachToThis;
				for (int i = 0; i < list.Count; i++)
				{
					GameObject gameObject4 = list[i];
					if (gameObject3.GetComponent<Collider>() != gameObject4.GetComponent<Collider>())
					{
						Physics.IgnoreCollision(gameObject3.GetComponent<Collider>(), gameObject4.GetComponent<Collider>(), true);
						component4.ignoreColliders.Add(gameObject4.GetComponent<Collider>());
						if (gameObject4.GetComponent<CardScript>())
						{
							gameObject4.GetComponent<CardScript>().ignoreColliders.Add(gameObject3.GetComponent<Collider>());
						}
					}
				}
			}
			npo2.GetComponent<Rigidbody>().mass = 0.25f;
			return npo1;
		}
		else
		{
			if (npo2.GetComponent<FixedJoint>() || npo1.GetComponent<FixedJoint>() || component.CardAttachToThis || component2.CardAttachToThis)
			{
				return null;
			}
			if (!npo1.IsHeldByNobody || !npo2.IsHeldByNobody || !flag || !flag2)
			{
				return null;
			}
			LuaGameObjectScript luaGameObjectScript = npo1.luaGameObjectScript;
			if (luaGameObjectScript != null && !luaGameObjectScript.CheckObjectEnter(npo2))
			{
				return null;
			}
			bool top = (double)Vector3.Dot(npo1.transform.position - npo2.transform.position, npo1.transform.up) >= 0.0;
			List<ulong> list2 = new List<ulong>();
			ComponentTags.CopyFlags(ref list2, npo1.tags);
			ComponentTags.AndFlags(ref list2, npo2.tags);
			NetworkPhysicsObject component5 = Network.Instantiate(NetworkSingleton<GameMode>.Instance.Deck, npo1.transform.position, npo1.transform.rotation, default(NetworkPlayer)).GetComponent<NetworkPhysicsObject>();
			DeckScript deckScript = component5.deckScript;
			deckScript.bRandomSpawn = false;
			deckScript.num_cards_ = 0;
			deckScript.PlayDrawCardSound();
			deckScript.AddCard(top, npo1.GetComponent<CardScript>().card_id(), this.SaveObjectState(npo1));
			deckScript.AddCard(top, npo2.GetComponent<CardScript>().card_id(), this.SaveObjectState(npo2));
			component5.SetScale(npo1.Scale, false);
			component5.DiffuseColor = npo1.DiffuseColor;
			component5.IgnoresGrid = npo1.IgnoresGrid;
			component5.IsHiddenWhenFaceDown = npo1.IsHiddenWhenFaceDown;
			component5.tags = list2;
			component5.hasTags = (list2.Count > 0);
			deckScript.bSideways = npo1.GetComponent<CardScript>().bSideways;
			deckScript.SetDeckIDs(component5.deckScript.get_bottom_card_id(), component5.deckScript.get_top_card_id());
			EventManager.TriggerObjectEnterContainer(component5, npo1);
			EventManager.TriggerObjectEnterContainer(component5, npo2);
			npo1.GetComponent<CardScript>().SetCardID(-1);
			this.DestroyThisObject(gameObject);
			npo2.GetComponent<CardScript>().SetCardID(-1);
			this.DestroyThisObject(gameObject2);
			if (checkDistanceAndRotation)
			{
				Pointer pointer = this.PointerFromID(npo2.PrevHeldByPlayerID);
				if (pointer)
				{
					List<NetworkPhysicsObject> recentlyDropped = pointer.RecentlyDropped;
					for (int j = 0; j < recentlyDropped.Count; j++)
					{
						NetworkPhysicsObject networkPhysicsObject = recentlyDropped[j];
						if (networkPhysicsObject && !(networkPhysicsObject == npo1) && !(networkPhysicsObject == npo2))
						{
							if (networkPhysicsObject.deckScript)
							{
								this.NotifyDeckHitDeck(component5, networkPhysicsObject, false);
							}
							else if (networkPhysicsObject.cardScript)
							{
								this.NotifyCardHitDeck(networkPhysicsObject, component5, false);
							}
						}
					}
				}
			}
			return component5;
		}
	}

	// Token: 0x0600156A RID: 5482 RVA: 0x0008EBE8 File Offset: 0x0008CDE8
	public NetworkPhysicsObject NotifyDeckHitDeck(NetworkPhysicsObject npo1, NetworkPhysicsObject npo2, bool checkAllDroppedObjects = true)
	{
		DeckScript deckScript = npo1.deckScript;
		DeckScript deckScript2 = npo2.deckScript;
		if (deckScript == null || deckScript2 == null || deckScript.GetDeck().Count == 0 || deckScript2.GetDeck().Count == 0 || npo1.IsDestroyed || npo2.IsDestroyed || npo1.GetComponent<MeshFilter>().sharedMesh != npo2.GetComponent<MeshFilter>().sharedMesh || npo1.IsLocked || npo2.IsLocked || npo1.InsideALayoutZone || npo2.InsideALayoutZone || deckScript.AnyoneSearchingDeck() || deckScript2.AnyoneSearchingDeck())
		{
			return null;
		}
		Vector2 v = new Vector2(npo1.Scale.x, npo1.Scale.z);
		Vector2 v2 = new Vector2(npo2.Scale.x, npo2.Scale.z);
		if (checkAllDroppedObjects && Vector3.Distance(v, v2) > 0.15f)
		{
			return null;
		}
		bool flag = (double)Vector3.Dot(npo1.transform.up, npo2.transform.up) > 0.0;
		bool flag2 = ManagerPhysicsObject.CloseEnoughForCardMerge(npo1, npo2, true, true);
		if (!checkAllDroppedObjects)
		{
			flag = true;
			flag2 = true;
		}
		else if (npo1.transform.position.y > npo2.transform.position.y)
		{
			return null;
		}
		GameObject gameObject = npo1.gameObject;
		GameObject gameObject2 = npo2.gameObject;
		if (npo1.IsHeldByNobody && npo2.IsHeldByNobody && flag && flag2)
		{
			bool flag3 = true;
			LuaGameObjectScript luaGameObjectScript = npo1.luaGameObjectScript;
			if (luaGameObjectScript != null && !luaGameObjectScript.CheckObjectEnter(npo2))
			{
				flag3 = false;
			}
			List<int> list;
			if (flag3)
			{
				bool flag4 = (double)Vector3.Dot(npo1.transform.position - npo2.transform.position, npo1.transform.up) >= 0.0;
				list = deckScript2.GetDeck();
				List<ObjectState> cardStates = deckScript2.GetCardStates();
				deckScript.PlayDrawCardSound();
				if (!flag4)
				{
					for (int i = 0; i < cardStates.Count; i++)
					{
						deckScript.AddCard(flag4, list[i], cardStates[i]);
					}
				}
				else
				{
					for (int j = list.Count - 1; j >= 0; j--)
					{
						deckScript.AddCard(flag4, list[j], cardStates[j]);
					}
				}
				EventManager.TriggerObjectEnterContainer(npo1, npo2);
			}
			else
			{
				list = new List<int>();
			}
			if (checkAllDroppedObjects)
			{
				list.Clear();
				Pointer pointer = this.PointerFromID(npo2.PrevHeldByPlayerID);
				if (pointer)
				{
					List<NetworkPhysicsObject> recentlyDropped = pointer.RecentlyDropped;
					for (int k = 0; k < recentlyDropped.Count; k++)
					{
						NetworkPhysicsObject networkPhysicsObject = recentlyDropped[k];
						if (networkPhysicsObject && !(networkPhysicsObject == npo1) && !(networkPhysicsObject == npo2))
						{
							if (networkPhysicsObject.deckScript)
							{
								this.NotifyDeckHitDeck(npo1, networkPhysicsObject, false);
							}
							else if (networkPhysicsObject.cardScript)
							{
								this.NotifyCardHitDeck(networkPhysicsObject, npo1, false);
							}
						}
					}
				}
			}
			if (flag3)
			{
				if (checkAllDroppedObjects)
				{
					this.DestroyThisObject(gameObject2);
				}
				else
				{
					npo2.SetSmoothDestroy(npo1.transform.position, npo1.transform.rotation);
				}
				return npo1;
			}
		}
		return null;
	}

	// Token: 0x0600156B RID: 5483 RVA: 0x0008EF38 File Offset: 0x0008D138
	public void HandleTypedNumberWrapper(NetworkPhysicsObject npo, int playerID, int number)
	{
		if (Network.isServer)
		{
			this.RPCHandleTypedNumberWrapper(npo, playerID, number);
			return;
		}
		base.networkView.RPC<NetworkPhysicsObject, int, int>(RPCTarget.Server, new Action<NetworkPhysicsObject, int, int>(this.RPCHandleTypedNumberWrapper), npo, playerID, number);
	}

	// Token: 0x0600156C RID: 5484 RVA: 0x0008EF68 File Offset: 0x0008D168
	[Remote(Permission.Client, SendType.ReliableBuffered, null, SerializationMethod.Default)]
	public void RPCHandleTypedNumberWrapper(NetworkPhysicsObject npo, int playerID, int number)
	{
		string text;
		if (NetworkSingleton<NetworkUI>.Instance.bHotseat)
		{
			text = PlayerScript.PointerScript.PointerColorLabel;
		}
		else
		{
			text = Colour.LabelFromColour(NetworkSingleton<PlayerManager>.Instance.ColourFromID(playerID));
		}
		bool flag = false;
		if (npo.luaGameObjectScript.CanCall("onNumberTyped", false))
		{
			DynValue dynValue = npo.luaGameObjectScript.TryCall("onNumberTyped", new object[]
			{
				text,
				number
			});
			if (dynValue != null && dynValue.Boolean && !dynValue.IsNilOrNan())
			{
				flag = true;
			}
		}
		if (LuaGlobalScriptManager.Instance.CanCall("onObjectNumberTyped", false))
		{
			DynValue dynValue = LuaGlobalScriptManager.Instance.TryCall("onObjectNumberTyped", new object[]
			{
				npo.luaGameObjectScript,
				text,
				number
			});
			if (dynValue != null && dynValue.Boolean && !dynValue.IsNilOrNan())
			{
				flag = true;
			}
		}
		if (!flag)
		{
			npo.HandleTypedNumber(npo, playerID, number);
		}
	}

	// Token: 0x0600156D RID: 5485 RVA: 0x0008F058 File Offset: 0x0008D258
	public Quaternion GetSetRotationFromGrabbable(GameObject grabbable)
	{
		Vector3 axis = this.FlipsAroundZAxis(grabbable) ? Vector3.forward : Vector3.right;
		float num = (float)this.SpinRotationIndexFromGrabbable(grabbable, 15f);
		int num2 = this.FlipRotationIndexFromGrabbable(grabbable, 15f);
		return Quaternion.AngleAxis(num * (float)15, Vector3.up) * Quaternion.AngleAxis((float)(num2 * 15), axis);
	}

	// Token: 0x0600156E RID: 5486 RVA: 0x0008F0B4 File Offset: 0x0008D2B4
	public int SpinRotationIndexFromGrabbable(GameObject grabbable, float Degrees = 15f)
	{
		Vector3 vector = grabbable.transform.forward;
		if (grabbable.transform.up.y >= 0f || this.FlipsAroundZAxis(grabbable))
		{
			vector *= -1f;
		}
		float num = Mathf.Atan2(vector.z, -vector.x) * 180f / 3.1415927f;
		int num2 = -1;
		int num3 = (int)Degrees / 15;
		float num4 = -90f - Degrees / 2f;
		float num5 = -90f + Degrees / 2f;
		float num6 = -180f - Degrees / 2f;
		float num7 = -180f + Degrees / 2f;
		int num8 = 24 / num3;
		int num9 = 18 / num3;
		for (int i = 0; i < num8; i++)
		{
			if (i < num9 + 1)
			{
				if (num >= num4 + Degrees * (float)i && num < num5 + Degrees * (float)i)
				{
					num2 = i * num3;
					break;
				}
			}
			else if (num >= num6 + Degrees * (float)(i - num9) && num < num7 + Degrees * (float)(i - num9))
			{
				num2 = i * num3;
				break;
			}
		}
		if (num2 == -1)
		{
			return 18;
		}
		return num2;
	}

	// Token: 0x0600156F RID: 5487 RVA: 0x0008F1CC File Offset: 0x0008D3CC
	public int FlipRotationIndexFromGrabbable(GameObject grabbable, float Degrees = 15f)
	{
		Vector3 forward = grabbable.transform.forward;
		float num = Mathf.Atan2(forward.z, -forward.y) * 180f / 3.1415927f;
		int num2;
		if (num >= -22.5f && num < 22.5f)
		{
			num2 = 2;
		}
		else if (num >= 22.5f && num < 67.5f)
		{
			num2 = 3;
		}
		else if (num >= 67.5f && num < 112.5f)
		{
			num2 = 4;
		}
		else if (num >= 112.5f && num < 157.5f)
		{
			num2 = 5;
		}
		else if (num >= -157.5f && num < -112.5f)
		{
			num2 = 7;
		}
		else if (num >= -112.5f && num < -67.5f)
		{
			num2 = 0;
		}
		else if (num >= -67.5f && num < -22.5f)
		{
			num2 = 1;
		}
		else
		{
			num2 = 6;
		}
		if ((num2 == 6 || num2 == 2) && (grabbable.CompareTag("Domino") || grabbable.CompareTag("Block") || grabbable.CompareTag("Clock")) && grabbable.name.StartsWith("Mahjong"))
		{
			return num2 * 3;
		}
		if (grabbable.transform.up.y >= 0f)
		{
			return 0;
		}
		return 12;
	}

	// Token: 0x06001570 RID: 5488 RVA: 0x0008F2F4 File Offset: 0x0008D4F4
	public Quaternion RotationFromIndex(int index)
	{
		if (index != 0)
		{
			return Quaternion.AngleAxis((float)(index * 15), new Vector3(0f, 1f, 0f));
		}
		return Quaternion.Euler(new Vector3(0f, 1f, 0f));
	}

	// Token: 0x06001571 RID: 5489 RVA: 0x0008F334 File Offset: 0x0008D534
	public Vector3 StaticSurfacePointBelowWorldPos(Vector3 worldPos, out Vector3 surfaceNormal)
	{
		GameObject gameObject;
		return this.StaticSurfacePointBelowWorldPos(worldPos, out surfaceNormal, out gameObject);
	}

	// Token: 0x06001572 RID: 5490 RVA: 0x0008F34C File Offset: 0x0008D54C
	public Vector3 StaticSurfacePointBelowWorldPos(Vector3 worldPos, out Vector3 surfaceNormal, out GameObject hitObject)
	{
		ValueTuple<RaycastHit[], int> valueTuple = PhysicsNonAlloc.RayCast(worldPos, Vector3.down, 1000f, -5, true);
		RaycastHit[] item = valueTuple.Item1;
		int item2 = valueTuple.Item2;
		for (int i = 0; i < item2; i++)
		{
			RaycastHit raycastHit = item[i];
			GameObject gameObject = this.GOFromCollider(raycastHit.collider);
			NetworkPhysicsObject networkPhysicsObject = this.NPOFromGO(gameObject);
			if ((networkPhysicsObject == null || networkPhysicsObject.IsLocked) && !gameObject.CompareTag("Pointer"))
			{
				surfaceNormal = raycastHit.normal;
				hitObject = gameObject;
				return raycastHit.point;
			}
		}
		surfaceNormal = Vector3.zero;
		hitObject = null;
		return Vector3.zero;
	}

	// Token: 0x06001573 RID: 5491 RVA: 0x0008F3F4 File Offset: 0x0008D5F4
	public Vector3 SurfacePointBelowWorldPos(Vector3 worldPos)
	{
		ValueTuple<RaycastHit[], int> valueTuple = PhysicsNonAlloc.RayCast(worldPos, Vector3.down, 1000f, -5, true);
		RaycastHit[] item = valueTuple.Item1;
		int item2 = valueTuple.Item2;
		for (int i = 0; i < item2; i++)
		{
			RaycastHit raycastHit = item[i];
			if (!this.GOFromCollider(raycastHit.collider).CompareTag("Pointer"))
			{
				return raycastHit.point;
			}
		}
		return Vector3.zero;
	}

	// Token: 0x06001574 RID: 5492 RVA: 0x0008F45C File Offset: 0x0008D65C
	public Vector3 SurfacePointBelowObject(GameObject grabbable)
	{
		NetworkPhysicsObject networkPhysicsObject = this.NPOFromGO(grabbable);
		Vector3 origin;
		if (networkPhysicsObject)
		{
			Vector3 vector;
			origin = networkPhysicsObject.GetBoundsNotNormalized(out vector).center;
		}
		else
		{
			origin = grabbable.transform.position;
		}
		ValueTuple<RaycastHit[], int> valueTuple = PhysicsNonAlloc.RayCast(origin, Vector3.down, 1000f, -5, true);
		RaycastHit[] item = valueTuple.Item1;
		int item2 = valueTuple.Item2;
		for (int i = 0; i < item2; i++)
		{
			RaycastHit raycastHit = item[i];
			GameObject gameObject = this.GOFromCollider(raycastHit.collider);
			if (gameObject != grabbable && !gameObject.CompareTag("Pointer"))
			{
				return raycastHit.point;
			}
		}
		return grabbable.transform.position;
	}

	// Token: 0x06001575 RID: 5493 RVA: 0x0008F510 File Offset: 0x0008D710
	public GameObject GOBelowGO(GameObject grabbable)
	{
		NetworkPhysicsObject networkPhysicsObject = this.NPOFromGO(grabbable);
		float y;
		if (networkPhysicsObject)
		{
			Vector3 vector;
			y = networkPhysicsObject.GetBoundsNotNormalized(out vector).center.y;
		}
		else
		{
			y = grabbable.transform.position.y;
		}
		Vector3 position = grabbable.transform.position;
		ValueTuple<RaycastHit[], int> valueTuple = PhysicsNonAlloc.RayCast(new Vector3(position.x, y, position.z), Vector3.down, 1000f, -5, true);
		RaycastHit[] item = valueTuple.Item1;
		int item2 = valueTuple.Item2;
		for (int i = 0; i < item2; i++)
		{
			RaycastHit raycastHit = item[i];
			GameObject gameObject = this.GOFromCollider(raycastHit.collider);
			if (gameObject != grabbable && !gameObject.CompareTag("Pointer"))
			{
				return gameObject;
			}
		}
		return null;
	}

	// Token: 0x06001576 RID: 5494 RVA: 0x0008F5DC File Offset: 0x0008D7DC
	public NetworkPhysicsObject NPOFromGO(GameObject grabbable)
	{
		NetworkPhysicsObject result;
		if (!this.grabbableDirectory.TryGetValue(grabbable, out result))
		{
			return grabbable.GetComponent<NetworkPhysicsObject>();
		}
		return result;
	}

	// Token: 0x06001577 RID: 5495 RVA: 0x0008F604 File Offset: 0x0008D804
	public int IDFromGO(GameObject grabbable)
	{
		NetworkPhysicsObject networkPhysicsObject = this.NPOFromGO(grabbable);
		if (networkPhysicsObject)
		{
			return networkPhysicsObject.ID;
		}
		return -1;
	}

	// Token: 0x06001578 RID: 5496 RVA: 0x0008F62C File Offset: 0x0008D82C
	public GameObject GOFromID(int ID)
	{
		for (int i = 0; i < this.GrabbableNPOs.Count; i++)
		{
			NetworkPhysicsObject networkPhysicsObject = this.GrabbableNPOs[i];
			if (ID == networkPhysicsObject.ID)
			{
				return networkPhysicsObject.gameObject;
			}
		}
		return null;
	}

	// Token: 0x06001579 RID: 5497 RVA: 0x0008F670 File Offset: 0x0008D870
	public NetworkPhysicsObject NPOFromID(int ID)
	{
		for (int i = 0; i < this.GrabbableNPOs.Count; i++)
		{
			NetworkPhysicsObject networkPhysicsObject = this.GrabbableNPOs[i];
			if (ID == networkPhysicsObject.ID)
			{
				return networkPhysicsObject;
			}
		}
		return null;
	}

	// Token: 0x0600157A RID: 5498 RVA: 0x0008F6AC File Offset: 0x0008D8AC
	public Pointer PointerFromID(int playerID)
	{
		for (int i = 0; i < this.Pointers.Count; i++)
		{
			if (playerID == this.Pointers[i].ID)
			{
				return this.Pointers[i];
			}
		}
		return null;
	}

	// Token: 0x0600157B RID: 5499 RVA: 0x0008F6F4 File Offset: 0x0008D8F4
	public GameObject GOFromGUID(string guid)
	{
		NetworkPhysicsObject networkPhysicsObject = this.NPOFromGUID(guid);
		if (!networkPhysicsObject)
		{
			return null;
		}
		return networkPhysicsObject.gameObject;
	}

	// Token: 0x0600157C RID: 5500 RVA: 0x0008F71C File Offset: 0x0008D91C
	public NetworkPhysicsObject NPOFromGUID(string guid)
	{
		for (int i = this.GrabbableNPOs.Count - 1; i >= 0; i--)
		{
			if (guid == this.GrabbableNPOs[i].GUID)
			{
				return this.GrabbableNPOs[i];
			}
		}
		return null;
	}

	// Token: 0x0600157D RID: 5501 RVA: 0x0008F768 File Offset: 0x0008D968
	public static InventoryTypes InventoryTypeFromObject(GameObject go)
	{
		if (go.CompareTag("Deck"))
		{
			return InventoryTypes.Deck;
		}
		if (go.CompareTag("Bag"))
		{
			return InventoryTypes.Bag;
		}
		return InventoryTypes.None;
	}

	// Token: 0x0600157E RID: 5502 RVA: 0x0008F78C File Offset: 0x0008D98C
	public bool CheckLuaTryRandomize(NetworkPhysicsObject npo, int pointerID)
	{
		Pointer pointer = this.PointerFromID(pointerID);
		string text = (pointer != null) ? pointer.PointerColorLabel : "";
		DynValue dynValue = LuaGlobalScriptManager.Instance.TryCall("tryObjectRandomize", new object[]
		{
			npo.luaGameObjectScript,
			text
		});
		if (dynValue != null && !dynValue.IsNilOrNan() && !dynValue.Boolean)
		{
			return false;
		}
		dynValue = npo.luaGameObjectScript.TryCall("tryRandomize", new object[]
		{
			text
		});
		return dynValue == null || dynValue.IsNilOrNan() || dynValue.Boolean;
	}

	// Token: 0x0600157F RID: 5503 RVA: 0x0008F824 File Offset: 0x0008DA24
	public bool Randomize(NetworkPhysicsObject npo, int pointerID)
	{
		if (!this.CheckLuaTryRandomize(npo, pointerID))
		{
			return false;
		}
		GameObject gameObject = npo.gameObject;
		if (npo.deckScript)
		{
			return npo.deckScript.RandomizeDeck();
		}
		if (npo.stackObject)
		{
			return npo.stackObject.ShuffleBag();
		}
		if (npo.GetSelectedStateId() != -1 && !gameObject.CompareTag("Dice") && !gameObject.CompareTag("Coin"))
		{
			return npo.ShuffleStates() != null;
		}
		if (!npo.IsLocked)
		{
			npo.ResetIdleFreeze();
			npo.rigidbody.isKinematic = false;
			float num = 6f;
			Vector3 a = new Vector3(UnityEngine.Random.Range(-num, num), UnityEngine.Random.Range(-num, num), UnityEngine.Random.Range(-num, num));
			if (gameObject.CompareTag("Dice") || gameObject.CompareTag("Coin"))
			{
				npo.rigidbody.AddTorque(a * Time.deltaTime * 20f * 4f, ForceMode.VelocityChange);
			}
			Vector3 vector = this.SurfacePointBelowObject(gameObject);
			float num2 = ManagerPhysicsObject.DiceRollForceMultiplier * Mathf.Pow(-Physics.gravity.y, 0.5f);
			if (npo.rigidbody.position.y - vector.y < 6f)
			{
				npo.rigidbody.velocity = new Vector3(0f, num2 + UnityEngine.Random.Range(1f, num2), 0f);
			}
			if (gameObject.GetComponent<SoundScript>())
			{
				if (gameObject.CompareTag("Dice") || gameObject.CompareTag("Coin"))
				{
					base.StartCoroutine(this.DelayedRandomRotation(npo.rigidbody));
					gameObject.GetComponent<SoundScript>().ShakeSound();
				}
				else if (!ServerOptions.isPhysicsLock)
				{
					gameObject.GetComponent<SoundScript>().PickUpSound();
				}
			}
			return true;
		}
		return false;
	}

	// Token: 0x06001580 RID: 5504 RVA: 0x0008F9F8 File Offset: 0x0008DBF8
	public bool RandomizeObjectsInHand(HandZone handZone)
	{
		List<NetworkPhysicsObject> handObjects = handZone.GetHandObjects(false);
		return this.RandomizeObjectsInHand(handObjects);
	}

	// Token: 0x06001581 RID: 5505 RVA: 0x0008FA14 File Offset: 0x0008DC14
	public bool RandomizeObjectsInHand(List<NetworkPhysicsObject> npos)
	{
		if (!npos[0].CurrentPlayerHand.bPositionHandObjects)
		{
			return false;
		}
		base.StartCoroutine(this.RandomizeObjectsInHandCoroutine(npos));
		return true;
	}

	// Token: 0x06001582 RID: 5506 RVA: 0x0008FA3A File Offset: 0x0008DC3A
	private IEnumerator RandomizeObjectsInHandCoroutine(List<NetworkPhysicsObject> npos)
	{
		HandZone currentHand = npos[0].CurrentPlayerHand;
		currentHand.bPositionHandObjects = false;
		npos[0].soundScript.ShakeSound();
		int num;
		for (int c = 0; c < 3; c = num + 1)
		{
			for (int i = npos.Count - 1; i >= 0; i--)
			{
				if (npos[i] == null)
				{
					npos.RemoveAt(i);
				}
			}
			List<NetworkPhysicsObject> list = new List<NetworkPhysicsObject>(npos);
			list.Randomize<NetworkPhysicsObject>();
			for (int j = 0; j < npos.Count; j++)
			{
				npos[j].SetSmoothPosition(list[j].gameObject.transform.position, false, true, false, true, null, false, false, null);
			}
			bool stillMoving = true;
			while (stillMoving)
			{
				stillMoving = false;
				for (int k = 0; k < npos.Count; k++)
				{
					if (npos[k] != null && npos[k].IsSmoothMoving)
					{
						stillMoving = true;
						break;
					}
				}
				yield return null;
			}
			num = c;
		}
		currentHand.bPositionHandObjects = true;
		yield break;
	}

	// Token: 0x06001583 RID: 5507 RVA: 0x0008FA49 File Offset: 0x0008DC49
	private IEnumerator DelayedRandomRotation(Rigidbody rigid)
	{
		yield return this.waitTenthOfSecond;
		rigid.rotation = UnityEngine.Random.rotation;
		yield break;
	}

	// Token: 0x06001584 RID: 5508 RVA: 0x0008FA60 File Offset: 0x0008DC60
	[Remote(Permission.Client, SendType.ReliableBuffered, null, SerializationMethod.Default)]
	public void SearchInventory(int objectID, int playerID, int maxCards = -1)
	{
		if (Network.isClient)
		{
			base.networkView.RPC<int, int, int>(RPCTarget.Server, new Action<int, int, int>(this.SearchInventory), objectID, playerID, maxCards);
			return;
		}
		GameObject gameObject = this.GOFromID(objectID);
		if (!gameObject)
		{
			return;
		}
		InventoryTypes inventoryTypes = ManagerPhysicsObject.InventoryTypeFromObject(gameObject);
		List<ObjectState> list;
		if (inventoryTypes == InventoryTypes.Deck)
		{
			DeckScript component = gameObject.GetComponent<DeckScript>();
			list = component.GetCardStates();
			for (int i = 0; i < list.Count; i++)
			{
				Vector3 scale = this.NPOFromGO(gameObject).Scale;
				list[i].Transform.scaleX = scale.x;
				list[i].Transform.scaleY = scale.y;
				list[i].Transform.scaleZ = scale.z;
			}
			component.playersSearchingInventory.AddSearchId(playerID, maxCards);
		}
		else
		{
			if (inventoryTypes != InventoryTypes.Bag)
			{
				return;
			}
			StackObject component2 = gameObject.GetComponent<StackObject>();
			list = component2.ObjectsHolder;
			component2.playersSearchingInventory.AddSearchId(playerID, -1);
		}
		if (playerID == NetworkID.ID)
		{
			NetworkSingleton<NetworkUI>.Instance.GUIInventory.Init(list, gameObject, inventoryTypes, maxCards);
			return;
		}
		this.SendSearchInventoryBytesToPlayer(playerID, objectID, list, maxCards);
	}

	// Token: 0x06001585 RID: 5509 RVA: 0x0008FB80 File Offset: 0x0008DD80
	public void SendSearchInventoryBytesToPlayer(int playerID, int objectId, List<ObjectState> objectStates, int maxCards)
	{
		NetworkPlayer networkPlayer = Network.IdToNetworkPlayer(playerID);
		if (networkPlayer != Network.player)
		{
			base.networkView.RPC<int, List<ObjectState>, int>(networkPlayer, new Action<int, List<ObjectState>, int>(this.SearchInventoryBytes), objectId, objectStates, maxCards);
			return;
		}
		this.SearchInventoryBytes(objectId, objectStates, maxCards);
	}

	// Token: 0x06001586 RID: 5510 RVA: 0x0008FBC8 File Offset: 0x0008DDC8
	[Remote(Permission.Client, SendType.ReliableBuffered, null, SerializationMethod.Default, serializationMethod = SerializationMethod.Json)]
	private void SearchInventoryBytes(int id, List<ObjectState> objectStates, int maxCards)
	{
		GameObject gameObject = this.GOFromID(id);
		if (!gameObject)
		{
			return;
		}
		NetworkSingleton<NetworkUI>.Instance.GUIInventory.Init(objectStates, gameObject, ManagerPhysicsObject.InventoryTypeFromObject(gameObject), maxCards);
	}

	// Token: 0x06001587 RID: 5511 RVA: 0x0008FC00 File Offset: 0x0008DE00
	[Remote(Permission.Client, SendType.ReliableBuffered, null, SerializationMethod.Default)]
	public void Shuffle(int deckID, int playerID)
	{
		if (Network.isClient)
		{
			base.networkView.RPC<int, int>(RPCTarget.Server, new Action<int, int>(this.Shuffle), deckID, playerID);
			return;
		}
		GameObject gameObject = this.GOFromID(deckID);
		if (!gameObject)
		{
			return;
		}
		NetworkPhysicsObject component = gameObject.GetComponent<NetworkPhysicsObject>();
		if (!component)
		{
			return;
		}
		bool flag = false;
		DeckScript component2 = gameObject.GetComponent<DeckScript>();
		if (component2 && !component2.AnyoneSearchingDeck() && component2.RandomizeDeck())
		{
			flag = true;
		}
		StackObject component3 = gameObject.GetComponent<StackObject>();
		if (component3 && component3.bBag && !component3.AnyoneSearchingBag() && component3.ShuffleBag())
		{
			flag = true;
		}
		if (component.GetSelectedStateId() != -1 && component.ShuffleStates() != null)
		{
			flag = true;
		}
		Pointer pointer = this.PointerFromID(playerID);
		if (pointer)
		{
			if (flag)
			{
				EventManager.TriggerObjectRandomize(component, pointer.PointerColorLabel);
			}
			component.HighlightNotify(pointer.PointerDarkColour);
		}
	}

	// Token: 0x06001588 RID: 5512 RVA: 0x0008FCF0 File Offset: 0x0008DEF0
	[Remote(Permission.Client, SendType.ReliableBuffered, null, SerializationMethod.Default)]
	public void DealObjectToColor(int objectId, string colorLabel, int numCards = 1, int index = 0)
	{
		NetworkPhysicsObject networkPhysicsObject = this.NPOFromID(objectId);
		if (!networkPhysicsObject || (networkPhysicsObject.GetComponent<PlayersSearchingInventory>() && networkPhysicsObject.GetComponent<PlayersSearchingInventory>().AnyoneSearching()))
		{
			return;
		}
		if (Network.isClient)
		{
			base.networkView.RPC<int, string, int, int>(RPCTarget.Server, new Action<int, string, int, int>(this.DealObjectToColor), objectId, colorLabel, numCards, index);
			return;
		}
		if (colorLabel == "Black" || colorLabel == "All")
		{
			for (int i = 0; i < 10; i++)
			{
				for (int j = 0; j < numCards; j++)
				{
					this.DealToColor(networkPhysicsObject, Colour.LabelFromColour(Colour.ColourFromID(i)), index);
				}
			}
			return;
		}
		if (colorLabel == "Seated")
		{
			List<PlayerState> playersList = NetworkSingleton<PlayerManager>.Instance.PlayersList;
			for (int k = 0; k < playersList.Count; k++)
			{
				for (int l = 0; l < numCards; l++)
				{
					this.DealToColor(networkPhysicsObject, playersList[k].stringColor, index);
				}
			}
			return;
		}
		for (int m = 0; m < numCards; m++)
		{
			this.DealToColor(networkPhysicsObject, colorLabel, index);
		}
	}

	// Token: 0x06001589 RID: 5513 RVA: 0x0008FE04 File Offset: 0x0008E004
	private void DealToColor(NetworkPhysicsObject npo, string colorLabel, int index)
	{
		HandZone handZone = HandZone.GetHandZone(colorLabel, index, true);
		if (handZone && !handZone.bDisabled)
		{
			NetworkPhysicsObject networkPhysicsObject = null;
			if (npo.deckScript)
			{
				GameObject gameObject = npo.deckScript.TakeCard(npo.deckScript.transform.up.y <= 0f, true);
				if (!gameObject && npo.deckScript.LastCard)
				{
					gameObject = npo.deckScript.LastCard;
					npo.deckScript.LastCard = null;
				}
				if (gameObject)
				{
					networkPhysicsObject = this.NPOFromGO(gameObject);
				}
			}
			else if (npo.stackObject)
			{
				GameObject gameObject2 = npo.stackObject.TakeObject(true);
				if (gameObject2)
				{
					networkPhysicsObject = this.NPOFromGO(gameObject2);
				}
			}
			else
			{
				networkPhysicsObject = npo;
			}
			if (networkPhysicsObject)
			{
				handZone.DealToEnd(networkPhysicsObject, true, false);
			}
		}
	}

	// Token: 0x0600158A RID: 5514 RVA: 0x0008FEF0 File Offset: 0x0008E0F0
	[Remote(Permission.Client, SendType.ReliableBuffered, null, SerializationMethod.Default)]
	public GameObject ResetDeck(int deckID)
	{
		if (Network.isClient)
		{
			base.networkView.RPC<int, GameObject>(RPCTarget.Server, new Func<int, GameObject>(this.ResetDeck), deckID);
			return null;
		}
		GameObject gameObject = this.GOFromID(deckID);
		if (!gameObject)
		{
			return null;
		}
		DeckScript component = gameObject.GetComponent<DeckScript>();
		if (!component || component.AnyoneSearchingDeck())
		{
			return null;
		}
		Vector3 position = gameObject.transform.position;
		this.NPOFromGO(gameObject);
		List<int> list = new List<int>();
		List<ObjectState> list2 = new List<ObjectState>();
		for (int i = 0; i < this.GrabbableNPOs.Count; i++)
		{
			NetworkPhysicsObject networkPhysicsObject = this.GrabbableNPOs[i];
			if (networkPhysicsObject.CompareTag("Card"))
			{
				CardScript cardScript = networkPhysicsObject.cardScript;
				if (cardScript && cardScript.deck_id_ == deckID)
				{
					list.Add(cardScript.card_id_);
					list2.Add(this.SaveObjectState(networkPhysicsObject));
					networkPhysicsObject.SetSmoothDestroy(position, Quaternion.AngleAxis(180f, new Vector3(1f, 0f, 0f)));
				}
			}
		}
		List<int> deck = component.GetDeck();
		for (int j = 0; j < deck.Count; j++)
		{
			list.Add(deck[j]);
		}
		List<ObjectState> cardStates = component.GetCardStates();
		for (int k = 0; k < cardStates.Count; k++)
		{
			list2.Add(cardStates[k]);
		}
		component.SetDeck(list, list2);
		component.RandomizeDeck();
		return gameObject;
	}

	// Token: 0x0600158B RID: 5515 RVA: 0x00090074 File Offset: 0x0008E274
	[Remote(Permission.Client, SendType.ReliableBuffered, null, SerializationMethod.Default)]
	public GameObject ResetAllCards()
	{
		if (Network.isClient)
		{
			base.networkView.RPC<GameObject>(RPCTarget.Server, new Func<GameObject>(this.ResetAllCards));
			return null;
		}
		List<int> list = new List<int>();
		for (int i = 0; i < this.GrabbableNPOs.Count; i++)
		{
			NetworkPhysicsObject networkPhysicsObject = this.GrabbableNPOs[i];
			if (networkPhysicsObject.CompareTag("Card") && !networkPhysicsObject.IsDestroyed)
			{
				list.Add(networkPhysicsObject.cardScript.card_id_);
			}
		}
		Vector3 spawnPos = NetworkSingleton<NetworkUI>.Instance.SpawnPos;
		Vector3 scale = Vector3.one;
		Color diffuseColor = Colour.UnityWhite;
		bool bSideways = false;
		for (int j = 0; j < this.GrabbableNPOs.Count; j++)
		{
			NetworkPhysicsObject networkPhysicsObject2 = this.GrabbableNPOs[j];
			if (networkPhysicsObject2.CompareTag("Deck") && !networkPhysicsObject2.IsDestroyed)
			{
				DeckScript deckScript = networkPhysicsObject2.deckScript;
				if (deckScript && !deckScript.AnyoneSearchingDeck())
				{
					List<int> deck = deckScript.GetDeck();
					for (int k = 0; k < deck.Count; k++)
					{
						list.Add(deck[k]);
					}
					spawnPos = new Vector3(networkPhysicsObject2.transform.position.x, networkPhysicsObject2.transform.position.y * 1.5f, networkPhysicsObject2.transform.position.z);
					scale = networkPhysicsObject2.Scale;
					diffuseColor = networkPhysicsObject2.DiffuseColor;
					bSideways = deckScript.bSideways;
				}
			}
		}
		this.ResetCards(spawnPos);
		GameObject gameObject = Network.Instantiate(NetworkSingleton<GameMode>.Instance.Deck, spawnPos, NetworkSingleton<GameMode>.Instance.Deck.transform.rotation, default(NetworkPlayer));
		gameObject.GetComponent<NetworkPhysicsObject>().SetScale(scale, false);
		gameObject.GetComponent<NetworkPhysicsObject>().DiffuseColor = diffuseColor;
		gameObject.GetComponent<DeckScript>().bRandomSpawn = false;
		gameObject.GetComponent<DeckScript>().SetDeck(list, null);
		gameObject.GetComponent<DeckScript>().RandomizeDeck();
		gameObject.GetComponent<DeckScript>().bSideways = bSideways;
		return gameObject;
	}

	// Token: 0x0600158C RID: 5516 RVA: 0x00090290 File Offset: 0x0008E490
	[Remote(Permission.Client, SendType.ReliableBuffered, null, SerializationMethod.Default)]
	public List<GameObject> CutStack(int id, int amount)
	{
		if (Network.isClient)
		{
			base.networkView.RPC<int, int, List<GameObject>>(RPCTarget.Server, new Func<int, int, List<GameObject>>(this.CutStack), id, amount);
			return null;
		}
		GameObject gameObject = this.GOFromID(id);
		if (gameObject == null || gameObject.GetComponent<StackObject>().IsInfiniteStack || gameObject.GetComponent<StackObject>().bBag)
		{
			return null;
		}
		StackObject component = gameObject.GetComponent<StackObject>();
		if (!component || component.num_objects_ / 2 < 2)
		{
			return null;
		}
		ObjectState objectState = this.SaveObjectState(gameObject);
		objectState.Number = new int?(gameObject.GetComponent<StackObject>().num_objects_ - amount);
		component.num_objects_ = amount;
		Vector3 vector = gameObject.GetComponent<NetworkPhysicsObject>().GetBounds().size + Vector3.one * 0.2f;
		Vector3 eulerAngles = gameObject.transform.eulerAngles;
		objectState.Transform.posX = gameObject.transform.position.x + (float)Math.Cos((double)eulerAngles.y * 3.141592653589793 / 180.0) * vector.x;
		objectState.Transform.posY = gameObject.transform.position.y;
		objectState.Transform.posZ = gameObject.transform.position.z + (float)Math.Sin((double)eulerAngles.y * 3.141592653589793 / 180.0) * vector.z;
		GameObject gameObject2 = this.LoadObjectState(objectState, false, false);
		SoundScript component2 = gameObject2.GetComponent<SoundScript>();
		if (component2)
		{
			component2.PickUpSound();
		}
		return new List<GameObject>
		{
			gameObject,
			gameObject2
		};
	}

	// Token: 0x0600158D RID: 5517 RVA: 0x00090444 File Offset: 0x0008E644
	[Remote(Permission.Client, SendType.ReliableBuffered, null, SerializationMethod.Default)]
	public List<GameObject> SplitStack(int id, int stackCount = 2)
	{
		if (Network.isClient)
		{
			base.networkView.RPC<int, int, List<GameObject>>(RPCTarget.Server, new Func<int, int, List<GameObject>>(this.SplitStack), id, stackCount);
			return null;
		}
		if (stackCount < 2)
		{
			return null;
		}
		GameObject gameObject = this.GOFromID(id);
		if (gameObject == null || gameObject.GetComponent<StackObject>().bBag || gameObject.GetComponent<StackObject>().IsInfiniteStack)
		{
			return null;
		}
		StackObject component = gameObject.GetComponent<StackObject>();
		if (!component || component.num_objects_ / stackCount < 2)
		{
			return null;
		}
		List<GameObject> list = new List<GameObject>();
		Vector3 vector = gameObject.GetComponent<NetworkPhysicsObject>().GetBounds().size + Vector3.one * 0.2f;
		int num = component.num_objects_ / stackCount;
		int num2 = 0;
		int num3 = component.num_objects_ % stackCount;
		for (int i = 0; i < stackCount - 1; i++)
		{
			ObjectState objectState = this.SaveObjectState(gameObject);
			if (num3 - 1 > i)
			{
				objectState.Number = new int?(num + 1);
				component.num_objects_ -= num + 1;
			}
			else
			{
				objectState.Number = new int?(num);
				component.num_objects_ -= num;
			}
			if (i % 8 == 0)
			{
				num2++;
			}
			objectState.Transform.posX = gameObject.transform.position.x + (float)((int)Math.Round(Math.Cos((double)i * 3.141592653589793 / 4.0), 0, MidpointRounding.AwayFromZero)) * (vector.x * (float)num2);
			objectState.Transform.posY = gameObject.transform.position.y;
			objectState.Transform.posZ = gameObject.transform.position.z + (float)((int)Math.Round(Math.Sin((double)i * 3.141592653589793 / 4.0), 0, MidpointRounding.AwayFromZero)) * (vector.z * (float)num2);
			GameObject gameObject2 = this.LoadObjectState(objectState, false, false);
			list.Add(gameObject2);
			SoundScript component2 = gameObject2.GetComponent<SoundScript>();
			if (component2)
			{
				component2.PickUpSound();
			}
		}
		for (int j = 0; j < list.Count; j++)
		{
			GameObject gameObject3 = list[j];
			gameObject3.transform.RotateAround(gameObject.transform.position, Vector3.up, gameObject.transform.eulerAngles.y);
			gameObject3.transform.rotation = gameObject.transform.rotation;
		}
		list.Insert(0, gameObject);
		return list;
	}

	// Token: 0x0600158E RID: 5518 RVA: 0x000906C8 File Offset: 0x0008E8C8
	[Remote(Permission.Client, SendType.ReliableBuffered, null, SerializationMethod.Default)]
	public List<GameObject> CutDeck(int deckId, int amount)
	{
		if (Network.isClient)
		{
			base.networkView.RPC<int, int, List<GameObject>>(RPCTarget.Server, new Func<int, int, List<GameObject>>(this.CutDeck), deckId, amount);
			return null;
		}
		GameObject gameObject = this.GOFromID(deckId);
		if (!gameObject)
		{
			return null;
		}
		DeckScript component = gameObject.GetComponent<DeckScript>();
		if (!component || component.AnyoneSearchingDeck() || component.num_cards_ / 2 < 2 || amount > component.num_cards_ - 2)
		{
			return null;
		}
		List<int> deck = component.GetDeck();
		List<ObjectState> cardStates = component.GetCardStates();
		List<int> list = new List<int>();
		List<ObjectState> list2 = new List<ObjectState>();
		for (int i = 0; i < amount; i++)
		{
			list.Add(deck[0]);
			deck.RemoveAt(0);
			list2.Add(cardStates[0]);
			cardStates.RemoveAt(0);
		}
		ObjectState objectState = this.SaveObjectState(gameObject);
		objectState.Transform.rotY += 90f;
		objectState.Transform.posY += gameObject.GetComponent<NetworkPhysicsObject>().GetBounds().size.y;
		objectState.DeckIDs = deck;
		objectState.ContainedObjects = cardStates;
		GameObject item = this.LoadObjectState(objectState, false, false);
		component.SetDeck(list, list2);
		return new List<GameObject>
		{
			gameObject,
			item
		};
	}

	// Token: 0x0600158F RID: 5519 RVA: 0x00090818 File Offset: 0x0008EA18
	[Remote(Permission.Client, SendType.ReliableBuffered, null, SerializationMethod.Default)]
	public List<GameObject> SplitDeck(int deckId, int stackCount = 2)
	{
		if (Network.isClient)
		{
			base.networkView.RPC<int, int, List<GameObject>>(RPCTarget.Server, new Func<int, int, List<GameObject>>(this.SplitDeck), deckId, stackCount);
			return null;
		}
		if (stackCount < 2)
		{
			return null;
		}
		GameObject gameObject = this.GOFromID(deckId);
		if (!gameObject)
		{
			return null;
		}
		DeckScript component = gameObject.GetComponent<DeckScript>();
		if (!component || component.AnyoneSearchingDeck() || component.num_cards_ / stackCount < 2)
		{
			return null;
		}
		List<int> deck = component.GetDeck();
		List<ObjectState> cardStates = component.GetCardStates();
		List<List<int>> list = new List<List<int>>();
		List<List<ObjectState>> list2 = new List<List<ObjectState>>();
		for (int i = 0; i < stackCount; i++)
		{
			List<int> item = new List<int>();
			list.Add(item);
			List<ObjectState> item2 = new List<ObjectState>();
			list2.Add(item2);
		}
		int num = component.num_cards_ / stackCount;
		for (int j = 0; j < stackCount; j++)
		{
			for (int k = 0; k < num; k++)
			{
				list[j].Add(deck[0]);
				deck.RemoveAt(0);
				list2[j].Add(cardStates[0]);
				cardStates.RemoveAt(0);
			}
		}
		for (int l = 0; l < cardStates.Count; l++)
		{
			list[l].Add(deck[l]);
			list2[l].Add(cardStates[l]);
		}
		List<GameObject> list3 = new List<GameObject>();
		float num2 = Mathf.Min(5f, (float)Mathf.Max(1, num + 1) * 0.023f + 0.1f) + 0.1f;
		for (int m = 1; m < stackCount; m++)
		{
			ObjectState objectState = this.SaveObjectState(gameObject);
			if (m % 2 != 0)
			{
				objectState.Transform.rotY += 90f;
			}
			objectState.Transform.posY += (float)m * num2;
			objectState.DeckIDs = list[m];
			objectState.ContainedObjects = list2[m];
			GameObject item3 = this.LoadObjectState(objectState, false, false);
			list3.Add(item3);
		}
		component.SetDeck(list[0], list2[0]);
		list3.Add(gameObject);
		return list3;
	}

	// Token: 0x06001590 RID: 5520 RVA: 0x00090A4C File Offset: 0x0008EC4C
	[Remote(Permission.Client, SendType.ReliableBuffered, null, SerializationMethod.Default)]
	public List<GameObject> SpreadDeck(int deckId, float distance = 0f, float rowDistance = 0f, int cardsPerRow = 0)
	{
		NetworkPhysicsObject networkPhysicsObject = this.NPOFromID(deckId);
		if (!networkPhysicsObject)
		{
			return null;
		}
		DeckScript component = networkPhysicsObject.GetComponent<DeckScript>();
		if (!component || component.AnyoneSearchingDeck())
		{
			return null;
		}
		if (distance == 0f)
		{
			distance = Pointer.SPREAD_ACTION_DISTANCE * component.transform.localScale.x;
		}
		if (rowDistance == 0f)
		{
			rowDistance = Pointer.SPREAD_ACTION_ROW_DISTANCE * component.transform.localScale.z;
		}
		if (cardsPerRow == 0)
		{
			cardsPerRow = Pointer.SPREAD_ACTION_CARDS_PER_ROW;
		}
		if (Network.isClient)
		{
			base.networkView.RPC<int, float, float, int, List<GameObject>>(RPCTarget.Server, new Func<int, float, float, int, List<GameObject>>(this.SpreadDeck), deckId, distance, rowDistance, cardsPerRow);
			return null;
		}
		bool flag = component.transform.up.y <= 0f;
		Vector3 vector = component.transform.right;
		if (!flag)
		{
			vector = -vector;
		}
		bool flag2 = flag && ManagerPhysicsObject.SpreadDeckRespectsCurrentOrientation;
		Vector3 a = Quaternion.Euler(0f, 90f, 0f) * vector;
		Vector3 eulerAngles = component.transform.eulerAngles;
		eulerAngles.z = 180f;
		if (!flag2)
		{
			component.transform.eulerAngles = eulerAngles;
			eulerAngles.z = 0f;
		}
		Vector3 position = networkPhysicsObject.transform.position;
		Vector3 vector2 = position;
		position.y += 1f;
		networkPhysicsObject.SetPosition(position);
		Vector3 vector3 = vector2 - vector * distance;
		List<GameObject> list = new List<GameObject>();
		int num_cards_ = component.num_cards_;
		NetworkPhysicsObject networkPhysicsObject2 = null;
		int num = 0;
		int num2 = 0;
		for (int i = 0; i < num_cards_; i++)
		{
			GameObject gameObject = component.TakeCard(!flag2, true);
			if (!gameObject && component.LastCard)
			{
				gameObject = component.LastCard;
				component.LastCard = null;
			}
			if (gameObject)
			{
				num++;
				if (num > cardsPerRow)
				{
					num = 1;
					num2++;
					vector3 = vector2;
					vector3 += a * rowDistance * (float)num2;
					networkPhysicsObject2 = null;
				}
				else
				{
					vector3 += vector * distance;
				}
				vector3.y += 0.26f;
				NetworkPhysicsObject networkPhysicsObject3 = this.NPOFromGO(gameObject);
				if (networkPhysicsObject2 != null)
				{
					networkPhysicsObject3.SetSmoothPosition(vector3, false, false, false, true, null, false, false, networkPhysicsObject2);
				}
				else
				{
					networkPhysicsObject3.SetSmoothPosition(vector3, false, false, false, true, null, false, false, null);
				}
				networkPhysicsObject3.SetRotation(eulerAngles);
				list.Add(gameObject);
				networkPhysicsObject2 = networkPhysicsObject3;
			}
		}
		return list;
	}

	// Token: 0x06001591 RID: 5521 RVA: 0x00090CE4 File Offset: 0x0008EEE4
	public void ResetCards(Vector3 pos)
	{
		if (Network.isClient)
		{
			return;
		}
		for (int i = 0; i < this.GrabbableNPOs.Count; i++)
		{
			NetworkPhysicsObject networkPhysicsObject = this.GrabbableNPOs[i];
			if (networkPhysicsObject.CompareTag("Card") || networkPhysicsObject.CompareTag("Deck"))
			{
				networkPhysicsObject.SetSmoothDestroy(pos, Quaternion.AngleAxis(180f, new Vector3(1f, 0f, 0f)));
				FixedJoint fixedJoint = networkPhysicsObject.fixedJoint;
				if (fixedJoint)
				{
					UnityEngine.Object.Destroy(fixedJoint);
				}
			}
		}
	}

	// Token: 0x06001592 RID: 5522 RVA: 0x00090D70 File Offset: 0x0008EF70
	public void DestroySky()
	{
		this.DestroyThisObject(this.Sky.gameObject);
	}

	// Token: 0x06001593 RID: 5523 RVA: 0x00090D83 File Offset: 0x0008EF83
	public void DestroyCustomSky()
	{
		if (CustomSky.ActiveCustomSky)
		{
			this.DestroyThisObject(CustomSky.ActiveCustomSky);
		}
	}

	// Token: 0x06001594 RID: 5524 RVA: 0x00090D9C File Offset: 0x0008EF9C
	public void DestroyMyHeldObjects()
	{
		int id = NetworkID.ID;
		for (int i = this.GrabbableNPOs.Count - 1; i >= 0; i--)
		{
			NetworkPhysicsObject networkPhysicsObject = this.GrabbableNPOs[i];
			if (networkPhysicsObject.HeldByPlayerID == id)
			{
				this.DestroyThisObject(networkPhysicsObject.gameObject);
			}
		}
	}

	// Token: 0x06001595 RID: 5525 RVA: 0x00090DE9 File Offset: 0x0008EFE9
	public bool PlayerColorAvailable(string colorLabel)
	{
		return this.PointerFromColorLabel(colorLabel) == null;
	}

	// Token: 0x06001596 RID: 5526 RVA: 0x00090DF8 File Offset: 0x0008EFF8
	public Pointer PointerFromColorLabel(string colorLabel)
	{
		for (int i = 0; i < this.Pointers.Count; i++)
		{
			if (this.Pointers[i].PointerColorLabel == colorLabel)
			{
				return this.Pointers[i];
			}
		}
		return null;
	}

	// Token: 0x06001597 RID: 5527 RVA: 0x00090E44 File Offset: 0x0008F044
	public string ColorLabelFromPointerID(int id)
	{
		for (int i = 0; i < this.Pointers.Count; i++)
		{
			if (this.Pointers[i].ID == id)
			{
				return this.Pointers[i].PointerColorLabel;
			}
		}
		return null;
	}

	// Token: 0x06001598 RID: 5528 RVA: 0x00090E8E File Offset: 0x0008F08E
	public GameObject GOFromCollider(Collider collider)
	{
		return collider.transform.root.gameObject;
	}

	// Token: 0x06001599 RID: 5529 RVA: 0x00090EA0 File Offset: 0x0008F0A0
	public NetworkPhysicsObject NPOFromCollider(Collider collider)
	{
		return this.NPOFromGO(this.GOFromCollider(collider));
	}

	// Token: 0x0600159A RID: 5530 RVA: 0x00090EB0 File Offset: 0x0008F0B0
	public bool FlipsAroundZAxis(GameObject grabbable)
	{
		return !grabbable.CompareTag("Domino") && !grabbable.CompareTag("Tablet") && !grabbable.CompareTag("Calculator") && !grabbable.CompareTag("Counter") && !grabbable.CompareTag("Mp3") && !grabbable.CompareTag("Clock") && !grabbable.CompareTag("Notecard");
	}

	// Token: 0x0600159B RID: 5531 RVA: 0x00090F1C File Offset: 0x0008F11C
	public Vector3 ClampToScreen(Vector3 viewPos, float paddingX, float paddingY, SpriteAlignment alignment = SpriteAlignment.Center, bool inPixels = false)
	{
		if (inPixels)
		{
			float num = this.UIResolutionScale();
			paddingX = paddingX / (float)Screen.width / num;
			paddingY = paddingY / (float)Screen.height / num;
		}
		paddingX = Mathf.Min(0.5f, paddingX);
		paddingY = Mathf.Min(0.5f, paddingY);
		float num2 = 0f;
		float min = 0f;
		if (alignment == SpriteAlignment.Center || alignment == SpriteAlignment.TopCenter || alignment == SpriteAlignment.BottomCenter)
		{
			num2 = paddingX;
			min = paddingX;
		}
		else if (alignment == SpriteAlignment.RightCenter || alignment == SpriteAlignment.TopRight || alignment == SpriteAlignment.BottomRight)
		{
			num2 = paddingX;
			min = 0f;
		}
		else if (alignment == SpriteAlignment.LeftCenter || alignment == SpriteAlignment.TopLeft || alignment == SpriteAlignment.BottomLeft)
		{
			min = paddingX;
			num2 = 0f;
		}
		float num3 = 0f;
		float min2 = 0f;
		if (alignment == SpriteAlignment.Center || alignment == SpriteAlignment.RightCenter || alignment == SpriteAlignment.LeftCenter)
		{
			num3 = paddingY;
			min2 = paddingY;
		}
		else if (alignment == SpriteAlignment.TopCenter || alignment == SpriteAlignment.TopRight || alignment == SpriteAlignment.TopLeft)
		{
			num3 = paddingY;
			min2 = 0f;
		}
		else if (alignment == SpriteAlignment.BottomCenter || alignment == SpriteAlignment.BottomRight || alignment == SpriteAlignment.BottomLeft)
		{
			min2 = paddingY;
			num3 = 0f;
		}
		viewPos.x = Mathf.Clamp(viewPos.x, min, 1f - num2);
		viewPos.y = Mathf.Clamp(viewPos.y, min2, 1f - num3);
		return viewPos;
	}

	// Token: 0x0600159C RID: 5532 RVA: 0x0009103E File Offset: 0x0008F23E
	public float UIResolutionScale()
	{
		return this.uiRoot.pixelSizeAdjustment;
	}

	// Token: 0x0600159D RID: 5533 RVA: 0x0009104C File Offset: 0x0008F24C
	public ObjectState SaveObjectState(GameObject grabbable)
	{
		NetworkPhysicsObject npo = this.NPOFromGO(grabbable);
		return this.SaveObjectState(npo);
	}

	// Token: 0x0600159E RID: 5534 RVA: 0x00091068 File Offset: 0x0008F268
	public ObjectState SaveObjectState(NetworkPhysicsObject npo)
	{
		GameObject gameObject = npo.gameObject;
		Vector3 position = gameObject.transform.position;
		Vector3 eulerAngles = gameObject.transform.eulerAngles;
		Vector3 scale = npo.Scale;
		TransformState transform = new TransformState
		{
			posX = position.x,
			posY = position.y,
			posZ = position.z,
			rotX = eulerAngles.x,
			rotY = eulerAngles.y,
			rotZ = eulerAngles.z,
			scaleX = scale.x,
			scaleY = scale.y,
			scaleZ = scale.z
		};
		ObjectState objectState = new ObjectState
		{
			Name = npo.InternalName,
			Transform = transform
		};
		HiddenZone hiddenZone = npo.hiddenZone;
		if (hiddenZone)
		{
			objectState.FogColor = hiddenZone.OwningColorLabel;
			objectState.FogHidePointers = new bool?(hiddenZone.pointersAreHidden);
			objectState.FogReverseHiding = new bool?(hiddenZone.isReversed);
			objectState.FogSeethrough = new bool?(hiddenZone.isTranslucent);
		}
		HandZone handZone = npo.handZone;
		if (handZone)
		{
			objectState.FogColor = handZone.TriggerLabel;
			objectState.miscID = (handZone.Stash ? handZone.Stash.GUID : null);
		}
		FogOfWarZone fogOfWarZone = npo.fogOfWarZone;
		if (fogOfWarZone)
		{
			FogOfWarState fogOfWar = new FogOfWarState
			{
				HideGmPointer = fogOfWarZone.HideGmPointer,
				HideObjects = fogOfWarZone.HideObjects,
				ReHideObjects = fogOfWarZone.ReHideObjects,
				Height = fogOfWarZone.FogHeight,
				RevealedLocations = fogOfWarZone.RevealedLocations
			};
			objectState.FogOfWar = fogOfWar;
		}
		FogOfWarRevealer fogOfWarRevealer = npo.fogOfWarRevealer;
		if (fogOfWarRevealer && (fogOfWarRevealer.Active || fogOfWarRevealer.Color != "All" || (double)Math.Abs(fogOfWarRevealer.Range - 5f) > 0.01))
		{
			FogOfWarRevealerState fogOfWarRevealer2 = new FogOfWarRevealerState
			{
				Active = fogOfWarRevealer.Active,
				ShowOutLine = fogOfWarRevealer.ShowFoWOutline,
				Color = fogOfWarRevealer.Color,
				Range = fogOfWarRevealer.Range,
				Height = fogOfWarRevealer.Height,
				FoV = fogOfWarRevealer.FoV,
				FoVOffset = fogOfWarRevealer.FoVOffset
			};
			objectState.FogOfWarRevealer = fogOfWarRevealer2;
		}
		LayoutZone layoutZone = npo.layoutZone;
		if (layoutZone)
		{
			LayoutZoneState layoutZone2 = new LayoutZoneState
			{
				Options = layoutZone.Options,
				GroupsInZone = layoutZone.SerializeGroups()
			};
			objectState.LayoutZone = layoutZone2;
		}
		JigsawPiece component = npo.GetComponent<JigsawPiece>();
		if (component)
		{
			objectState.vector = new VectorState?(new VectorState(component.desiredPosition));
		}
		if (npo.UseAltSounds)
		{
			objectState.AltSound = new bool?(npo.UseAltSounds);
		}
		if (npo.materialSyncScript)
		{
			objectState.MaterialIndex = new int?(npo.materialSyncScript.GetMaterial());
		}
		if (npo.meshSyncScript)
		{
			objectState.MeshIndex = new int?(npo.meshSyncScript.GetMesh());
		}
		StackObject stackObject = npo.stackObject;
		if (stackObject)
		{
			objectState.MeshIndex = new int?(stackObject.MeshInt);
			objectState.MaterialIndex = new int?(stackObject.MaterialInt);
			if (stackObject.ObjectsHolder.Count > 0)
			{
				objectState.ContainedObjects = new List<ObjectState>(stackObject.ObjectsHolder);
			}
			else
			{
				objectState.Number = new int?(stackObject.num_objects_);
			}
			if (stackObject.bBag)
			{
				objectState.Bag = new BagState
				{
					Order = stackObject.Order
				};
			}
		}
		DeckScript deckScript = npo.deckScript;
		if (deckScript)
		{
			objectState.SidewaysCard = new bool?(deckScript.bSideways);
			List<int> deck = deckScript.GetDeck();
			List<int> list = new List<int>();
			objectState.DeckIDs = new List<int>();
			for (int i = 0; i < deck.Count; i++)
			{
				int num = deck[i];
				objectState.DeckIDs.Add(num);
				int num2 = 0;
				int j = num;
				while (j > 99)
				{
					j -= 100;
					num2++;
				}
				if (num2 > 0 && !list.Contains(num2))
				{
					list.Add(num2);
				}
			}
			if (list.Count > 0)
			{
				objectState.CustomDeck = new Dictionary<int, CustomDeckState>();
				for (int k = 0; k < list.Count; k++)
				{
					int key = list[k];
					CustomDeckData customDeckData;
					if (NetworkSingleton<CardManagerScript>.Instance.GetCustomDecks(false).TryGetValue(key, out customDeckData))
					{
						CustomDeckState value = new CustomDeckState
						{
							FaceURL = customDeckData.FaceURL,
							BackURL = customDeckData.BackURL,
							NumWidth = customDeckData.NumWidth,
							NumHeight = customDeckData.NumHeight,
							BackIsHidden = customDeckData.BackIsHidden,
							UniqueBack = customDeckData.UniqueBack,
							Type = customDeckData.Type
						};
						objectState.CustomDeck.Add(key, value);
					}
					else
					{
						UnityEngine.Debug.LogError("Failure to find custom deck data");
					}
				}
			}
			objectState.ContainedObjects = new List<ObjectState>(deckScript.GetCardStates());
		}
		CardScript cardScript = npo.cardScript;
		if (cardScript)
		{
			objectState.SidewaysCard = new bool?(cardScript.bSideways);
			objectState.CardID = new int?(cardScript.card_id_);
			int num3 = 0;
			int l = objectState.CardID.Value;
			while (l > 99)
			{
				l -= 100;
				num3++;
			}
			if (num3 > 0)
			{
				objectState.CustomDeck = new Dictionary<int, CustomDeckState>();
				CustomDeckData customDeckData2;
				if (NetworkSingleton<CardManagerScript>.Instance.GetCustomDecks(false).TryGetValue(num3, out customDeckData2))
				{
					CustomDeckState value2 = new CustomDeckState
					{
						FaceURL = customDeckData2.FaceURL,
						BackURL = customDeckData2.BackURL,
						NumWidth = customDeckData2.NumWidth,
						NumHeight = customDeckData2.NumHeight,
						BackIsHidden = customDeckData2.BackIsHidden,
						UniqueBack = customDeckData2.UniqueBack,
						Type = customDeckData2.Type
					};
					objectState.CustomDeck.Add(num3, value2);
				}
				else
				{
					UnityEngine.Debug.LogError("Failure to find custom card data");
				}
			}
		}
		CustomImage customImage = npo.customImage;
		if (customImage)
		{
			CustomImageState customImageState = new CustomImageState
			{
				ImageURL = customImage.CustomImageURL,
				ImageSecondaryURL = customImage.CustomImageSecondaryURL,
				ImageScalar = customImage.CardScalar
			};
			if (npo.customBoardScript)
			{
				customImageState.WidthScale = npo.customBoardScript.xMulti;
			}
			if (npo.customDice)
			{
				customImageState.CustomDice = new CustomDiceState
				{
					Type = npo.customDice.CurrentDiceType
				};
			}
			if (npo.customToken)
			{
				customImageState.CustomToken = new CustomTokenState
				{
					Thickness = npo.customToken.Thickness,
					MergeDistancePixels = npo.customToken.MergeDistancePixels,
					StandUp = npo.customToken.bStandUp,
					Stackable = npo.customToken.bStackable
				};
			}
			if (npo.customJigsawPuzzle)
			{
				customImageState.CustomJigsawPuzzle = new CustomJigsawPuzzleState
				{
					NumPuzzlePieces = npo.customJigsawPuzzle.NumPuzzlePieces,
					ImageOnBoard = npo.customJigsawPuzzle.bImageOnBoard
				};
			}
			if (npo.customTile)
			{
				customImageState.CustomTile = new CustomTileState
				{
					Type = npo.customTile.CurrentTileType,
					Thickness = npo.customTile.Thickness,
					Stackable = npo.customTile.bStackable,
					Stretch = npo.customTile.bStretch
				};
			}
			objectState.CustomImage = customImageState;
		}
		CustomMesh customMesh = npo.customMesh;
		if (customMesh)
		{
			objectState.CustomMesh = customMesh.customMeshState.DeepClone<CustomMeshState>();
		}
		CustomAssetbundle customAssetbundle = npo.customAssetbundle;
		if (customAssetbundle)
		{
			CustomAssetbundleState customAssetbundle2 = new CustomAssetbundleState
			{
				AssetbundleURL = customAssetbundle.CustomAssetbundleURL,
				AssetbundleSecondaryURL = customAssetbundle.CustomAssetbundleSecondaryURL,
				TypeIndex = customAssetbundle.TypeInt,
				MaterialIndex = customAssetbundle.MaterialInt,
				LoopingEffectIndex = customAssetbundle.LoopEffectIndex
			};
			objectState.CustomAssetbundle = customAssetbundle2;
		}
		CustomPDF customPDF = npo.customPDF;
		if (customPDF)
		{
			CustomPDFState customPDF2 = new CustomPDFState
			{
				PDFUrl = customPDF.CustomPDFURL,
				PDFPassword = customPDF.CustomPDFPassword,
				PDFPage = customPDF.CurrentPDFPage,
				PDFPageOffset = customPDF.PageDisplayOffset
			};
			objectState.CustomPDF = customPDF2;
		}
		ClockScript clockScript = npo.clockScript;
		if (clockScript)
		{
			ClockState clock = new ClockState
			{
				Mode = clockScript.currentClockMode,
				SecondsPassed = clockScript.GetHowManySecondsPassed(),
				Paused = clockScript.bPaused
			};
			objectState.Clock = clock;
		}
		CounterScript counterScript = npo.counterScript;
		if (counterScript)
		{
			CounterState counterState = new CounterState();
			int value3;
			int.TryParse(counterScript.uiinput.value, out value3);
			counterState.value = value3;
			objectState.Counter = counterState;
		}
		TabletScript tabletScript = npo.tabletScript;
		if (tabletScript && !string.IsNullOrEmpty(tabletScript.CurrentURL))
		{
			TabletState tablet = new TabletState
			{
				PageURL = tabletScript.CurrentURL
			};
			objectState.Tablet = tablet;
		}
		Mp3PlayerScript mp3PlayerScript = npo.mp3PlayerScript;
		if (mp3PlayerScript)
		{
			Mp3PlayerSong component2 = mp3PlayerScript.transform.GetChild(0).GetChild(0).GetChild(3).GetComponent<Mp3PlayerSong>();
			Mp3PlayerState mp3PlayerState = new Mp3PlayerState
			{
				genre = component2.genre,
				songTitle = (component2.songName ?? ""),
				volume = mp3PlayerScript.theVolume,
				isPlaying = mp3PlayerScript.audio.isPlaying,
				loopOne = mp3PlayerScript.loopOne
			};
			MenuStruct activeMenu = mp3PlayerScript.GetActiveMenu();
			mp3PlayerState.menuTitle = activeMenu.title;
			mp3PlayerState.menu = Menus.GENRES;
			if (activeMenu.menu == mp3PlayerScript.GenreGO)
			{
				mp3PlayerState.menu = Menus.GENRES;
			}
			else if (activeMenu.menu == mp3PlayerScript.SongListGO)
			{
				mp3PlayerState.menu = Menus.SONGLIST;
			}
			else if (activeMenu.menu == mp3PlayerScript.PlayingSong)
			{
				mp3PlayerState.menu = Menus.PLAYINGSONG;
			}
			else if (activeMenu.menu == mp3PlayerScript.VolumeMenu)
			{
				mp3PlayerState.menu = Menus.VOLUME;
			}
			objectState.Mp3Player = mp3PlayerState;
		}
		CalculatorScript calculatorScript = npo.calculatorScript;
		if (calculatorScript)
		{
			CalculatorState calculator = new CalculatorState
			{
				value = calculatorScript.screen.text,
				memory = calculatorScript.memory
			};
			objectState.Calculator = calculator;
		}
		TextTool textTool = npo.textTool;
		if (textTool)
		{
			TextState text = new TextState
			{
				Text = textTool.input.value,
				colorstate = new ColourState?(new ColourState(textTool.input.label.color)),
				fontSize = textTool.input.label.fontSize
			};
			objectState.Text = text;
		}
		RPGFigurines rpgFigurines = npo.rpgFigurines;
		if (rpgFigurines)
		{
			objectState.RPGmode = new bool?(rpgFigurines.bMode);
			objectState.RPGdead = new bool?(rpgFigurines.bDead);
		}
		XmlUIScript xmlUI = npo.xmlUI;
		if (xmlUI)
		{
			objectState.XmlUI = xmlUI.xmlui_code;
			if (xmlUI.CustomAssets.Count > 0)
			{
				objectState.CustomUIAssets = xmlUI.CustomAssets;
			}
		}
		LuaGameObjectScript luaGameObjectScript = npo.luaGameObjectScript;
		if (luaGameObjectScript)
		{
			objectState.LuaScript = luaGameObjectScript.script_code;
			objectState.LuaScriptState = luaGameObjectScript.script_state;
		}
		objectState.Tags = null;
		if (npo.tags.Count > 0)
		{
			List<TagLabel> list2 = new List<TagLabel>();
			for (int m = 0; m < npo.tags.Count; m++)
			{
				for (int n = 0; n < 64; n++)
				{
					int flagIndex = n + 64 * m;
					if (NetworkSingleton<ComponentTags>.Instance.IsTagActive(flagIndex) && (npo.tags[m] & 1UL << n) != 0UL)
					{
						list2.Add(NetworkSingleton<ComponentTags>.Instance.TagLabelFromIndex(flagIndex));
					}
				}
			}
			if (list2.Count > 0)
			{
				list2.Sort((TagLabel a, TagLabel b) => a.normalized.CompareTo(b.normalized));
				objectState.Tags = new List<string>();
				for (int num4 = 0; num4 < list2.Count; num4++)
				{
					objectState.Tags.Add(list2[num4].displayed);
				}
			}
		}
		objectState.Nickname = npo.Name;
		objectState.Description = npo.Description;
		objectState.GMNotes = npo.GMNotes;
		objectState.Memo = npo.Memo;
		objectState.AltLookAngle = npo.AltLookAngle;
		objectState.Locked = npo.IsLocked;
		objectState.ColorDiffuse = new ColourState?(new ColourState(npo.DiffuseColor));
		objectState.Grid = !npo.IgnoresGrid;
		objectState.Snap = !npo.IgnoresSnap;
		objectState.Autoraise = npo.DoAutoRaise;
		objectState.IgnoreFoW = npo.IgnoresFogOfWar;
		objectState.MeasureMovement = npo.ShowRulerWhenHeld;
		objectState.DragSelectable = npo.IsDragSelectable;
		objectState.Sticky = npo.IsSticky;
		objectState.Tooltip = npo.ShowTooltip;
		objectState.GridProjection = npo.ShowGridProjection;
		objectState.HideWhenFaceDown = new bool?(npo.IsHiddenWhenFaceDown);
		objectState.Hands = new bool?(npo.CanBeHeldInHand);
		objectState.LayoutGroupSortIndex = npo.LayoutGroupSortIndex;
		objectState.Value = npo.Value;
		RigidbodyState overrideRigidbody = npo.OverrideRigidbody;
		if (overrideRigidbody != null)
		{
			objectState.Rigidbody = new RigidbodyState
			{
				Mass = overrideRigidbody.Mass,
				Drag = overrideRigidbody.Drag,
				AngularDrag = overrideRigidbody.AngularDrag,
				UseGravity = overrideRigidbody.UseGravity
			};
		}
		PhysicsMaterialState overridePhysicsMaterial = npo.OverridePhysicsMaterial;
		if (overridePhysicsMaterial != null)
		{
			objectState.PhysicsMaterial = new PhysicsMaterialState
			{
				DynamicFriction = overridePhysicsMaterial.DynamicFriction,
				StaticFriction = overridePhysicsMaterial.StaticFriction,
				Bounciness = overridePhysicsMaterial.Bounciness,
				FrictionCombine = overridePhysicsMaterial.FrictionCombine,
				BounceCombine = overridePhysicsMaterial.BounceCombine
			};
		}
		Joint joint = npo.joint;
		if (!gameObject.CompareTag("Card") && joint && joint.connectedBody)
		{
			JointState jointState = new JointState();
			if (joint.connectedBody)
			{
				jointState.ConnectedBodyGUID = joint.connectedBody.GetComponent<NetworkPhysicsObject>().GUID;
			}
			jointState.EnableCollision = joint.enableCollision;
			jointState.Axis = new VectorState(joint.axis);
			jointState.Anchor = new VectorState(joint.anchor);
			jointState.ConnectedAnchor = new VectorState(joint.connectedAnchor);
			jointState.BreakForce = joint.breakForce;
			jointState.BreakTorgue = joint.breakTorque;
			if (npo.fixedJoint)
			{
				objectState.JointFixed = new JointFixedState();
				objectState.JointFixed.Assign(jointState);
			}
			HingeJoint hingeJoint = npo.hingeJoint;
			if (hingeJoint)
			{
				objectState.JointHinge = new JointHingeState
				{
					UseLimits = hingeJoint.useLimits,
					Limits = hingeJoint.limits,
					UseMotor = hingeJoint.useMotor,
					Motor = hingeJoint.motor,
					UseSpring = hingeJoint.useSpring,
					Spring = hingeJoint.spring
				};
				objectState.JointHinge.Assign(jointState);
			}
			SpringJoint springJoint = npo.springJoint;
			if (springJoint)
			{
				objectState.JointSpring = new JointSpringState
				{
					Damper = springJoint.damper,
					MaxDistance = springJoint.maxDistance,
					MinDistance = springJoint.minDistance,
					Spring = springJoint.spring
				};
				objectState.JointSpring.Assign(jointState);
			}
		}
		objectState.GUID = npo.GUID;
		objectState.AttachedSnapPoints = NetworkSingleton<SnapPointManager>.Instance.GetSnapPointSaveStates(npo.networkView);
		objectState.AttachedVectorLines = NetworkSingleton<ToolVector>.Instance.GetLineStates(npo);
		objectState.AttachedDecals = NetworkSingleton<DecalManager>.Instance.GetDecalStates(npo.networkView);
		if (npo.States != null)
		{
			objectState.States = new Dictionary<int, ObjectState>(npo.States);
		}
		if (npo.RotationValues != null && npo.RotationValues.Count > 0)
		{
			List<RotationValueState> list3 = new List<RotationValueState>();
			for (int num5 = 0; num5 < npo.RotationValues.Count; num5++)
			{
				RotationValue rotationValue = npo.RotationValues[num5];
				RotationValueState item = new RotationValueState
				{
					Value = rotationValue.value,
					Rotation = new VectorState(rotationValue.rotation)
				};
				list3.Add(item);
			}
			objectState.RotationValues = list3;
		}
		List<ObjectState> children = npo.childSpawner.GetChildren();
		if (children.Count > 0)
		{
			objectState.ChildObjects = children;
		}
		return objectState;
	}

	// Token: 0x0600159F RID: 5535 RVA: 0x00092208 File Offset: 0x00090408
	public ObjectState ObjectStateFromPhysicsState(PhysicsState ps)
	{
		ObjectState objectState = new ObjectState();
		string text = Utilities.RemoveCloneFromName(ps.name);
		GameObject gameObject = (GameObject)Resources.Load(text);
		if (!gameObject)
		{
			UnityEngine.Debug.LogError("Error PhysicsStateToObjectState resource load: " + text);
			return null;
		}
		if (!gameObject.GetComponent<NetworkPhysicsObject>())
		{
			Chat.Log("Tried to load non networkphysics object, PhysicsStateConvert.", Colour.Red, ChatMessageType.Game, false);
			return null;
		}
		objectState.Name = text;
		TransformState transformState = new TransformState
		{
			posX = ps.posX,
			posY = ps.posY,
			posZ = ps.posZ,
			rotX = ps.rotX,
			rotY = ps.rotY,
			rotZ = ps.rotZ
		};
		if (!gameObject.GetComponent<HiddenZone>())
		{
			if (ps.StringList != null && ps.StringList.Count > 1)
			{
				float num = float.Parse(ps.StringList[1]);
				transformState.scaleX = num;
				transformState.scaleY = num;
				transformState.scaleZ = num;
			}
		}
		else
		{
			string[] array = ps.Note.Split(new char[]
			{
				' '
			});
			if (array.Length > 3)
			{
				Vector3 vector = new Vector3(float.Parse(array[0]), float.Parse(array[1]), float.Parse(array[2]));
				transformState.scaleX = vector.x;
				transformState.scaleY = vector.y;
				transformState.scaleZ = vector.z;
				objectState.FogColor = array[3];
			}
		}
		objectState.Transform = transformState;
		if (ps.bAltSound)
		{
			objectState.AltSound = new bool?(ps.bAltSound);
		}
		if (gameObject.GetComponent<MaterialSyncScript>())
		{
			objectState.MaterialIndex = new int?(ps.MatInt);
		}
		if (gameObject.GetComponent<MeshSyncScript>())
		{
			objectState.MeshIndex = new int?(ps.MeshInt);
		}
		if (gameObject.GetComponent<StackObject>())
		{
			objectState.Number = new int?(ps.Num);
			objectState.MeshIndex = new int?(ps.MatInt);
		}
		if (gameObject.GetComponent<DeckScript>())
		{
			objectState.DeckIDs = ps.List;
			objectState.SidewaysCard = new bool?(false);
			if (ps.Note == "1" || ps.Note == "True")
			{
				objectState.SidewaysCard = new bool?(true);
			}
			else
			{
				string[] array2 = ps.Note.Split(new char[]
				{
					' '
				});
				if (array2.Length > 1)
				{
					objectState.SidewaysCard = new bool?(bool.Parse(array2[0]));
					objectState.CustomDeck = new Dictionary<int, CustomDeckState>();
					for (int i = 1; i < array2.Length; i++)
					{
						string[] array3 = array2[i].Split(new char[]
						{
							','
						});
						int key = int.Parse(array3[0]);
						CustomDeckState value = new CustomDeckState
						{
							FaceURL = array3[2],
							BackURL = array3[1]
						};
						objectState.CustomDeck.Add(key, value);
					}
				}
			}
		}
		if (gameObject.GetComponent<CardScript>())
		{
			objectState.CardID = new int?(ps.idint);
			objectState.SidewaysCard = new bool?(false);
			if (ps.Note == "1" || ps.Note == "True")
			{
				objectState.SidewaysCard = new bool?(true);
			}
			else
			{
				string[] array4 = ps.Note.Split(new char[]
				{
					' '
				});
				if (array4.Length > 1)
				{
					objectState.SidewaysCard = new bool?(bool.Parse(array4[0]));
					objectState.CustomDeck = new Dictionary<int, CustomDeckState>();
					for (int j = 1; j < array4.Length; j++)
					{
						string[] array5 = array4[j].Split(new char[]
						{
							','
						});
						int key2 = int.Parse(array5[0]);
						CustomDeckState customDeckState = new CustomDeckState();
						customDeckState.FaceURL = array5[2];
						customDeckState.BackURL = array5[1];
						objectState.CustomDeck.Add(key2, customDeckState);
					}
				}
			}
		}
		if (gameObject.GetComponent<CustomImage>())
		{
			CustomImageState customImageState = new CustomImageState();
			CustomBoardScript component = gameObject.GetComponent<CustomBoardScript>();
			if (component)
			{
				string[] array6 = ps.Note.Split(new char[]
				{
					' '
				}, StringSplitOptions.RemoveEmptyEntries);
				customImageState.ImageURL = array6[0];
				customImageState.WidthScale = component.xMulti;
			}
			else
			{
				customImageState.ImageURL = ps.Note;
			}
			objectState.CustomImage = customImageState;
		}
		if (gameObject.GetComponent<CustomMesh>())
		{
			CustomMeshState customMeshState = new CustomMeshState();
			try
			{
				string[] array7 = ps.Note.Split(new char[]
				{
					' '
				});
				if (array7.Length > 3)
				{
					customMeshState.MeshURL = array7[0];
					customMeshState.DiffuseURL = array7[1];
					customMeshState.MaterialIndex = int.Parse(array7[2]);
					customMeshState.TypeIndex = int.Parse(array7[3]);
				}
				if (array7.Length > 5)
				{
					customMeshState.ColliderURL = array7[4];
					customMeshState.Convex = (array7[5] != "1");
				}
				if (array7.Length > 6)
				{
					customMeshState.NormalURL = array7[6];
				}
			}
			catch (Exception ex)
			{
				Chat.LogWarning("Error with custom object. Attempting workaround. " + ex.Message, true);
			}
			finally
			{
				string[] array8 = ps.Note.Split(new char[]
				{
					' '
				}, StringSplitOptions.RemoveEmptyEntries);
				if (array8.Length > 3)
				{
					customMeshState.MeshURL = array8[0];
					customMeshState.DiffuseURL = array8[1];
					customMeshState.MaterialIndex = int.Parse(array8[2]);
					customMeshState.TypeIndex = int.Parse(array8[3]);
				}
				if (array8.Length > 5)
				{
					customMeshState.ColliderURL = array8[4];
					customMeshState.Convex = (array8[5] != "1");
				}
				if (array8.Length > 6)
				{
					customMeshState.NormalURL = array8[6];
				}
			}
			objectState.CustomMesh = customMeshState;
		}
		if (gameObject.GetComponent<ClockScript>())
		{
			string[] array9 = ps.Note.Split(new char[]
			{
				' '
			});
			ClockState clockState = new ClockState();
			if (array9.Length > 1)
			{
				ClockScript.ClockMode mode = (ClockScript.ClockMode)int.Parse(array9[0]);
				clockState.Mode = mode;
				clockState.SecondsPassed = int.Parse(array9[1]);
			}
			if (array9.Length > 2)
			{
				clockState.Paused = bool.Parse(array9[2]);
			}
			objectState.Clock = clockState;
		}
		if (ps.StringList != null && ps.StringList.Count > 0)
		{
			objectState.Locked = (ps.StringList[0] == "1");
			if (ps.StringList.Count > 3)
			{
				objectState.Nickname = ps.StringList[2];
				objectState.Description = ps.StringList[3];
			}
			if (ps.StringList.Count > 5)
			{
				string[] array10 = ps.StringList[4].Split(new char[]
				{
					' '
				});
				objectState.ColorDiffuse = new ColourState?(new ColourState(float.Parse(array10[0]), float.Parse(array10[1]), float.Parse(array10[2]), 1f));
				objectState.Grid = !bool.Parse(ps.StringList[5]);
			}
		}
		return objectState;
	}

	// Token: 0x060015A0 RID: 5536 RVA: 0x00092954 File Offset: 0x00090B54
	public GameObject LoadObjectState(ObjectState os, bool dummyObject = false, bool hidden = false)
	{
		GameObject prefab = Resources.Load<GameObject>(os.Name);
		return this.LoadObjectState(os, prefab, dummyObject, hidden);
	}

	// Token: 0x060015A1 RID: 5537 RVA: 0x00092978 File Offset: 0x00090B78
	private GameObject LoadObjectState(ObjectState os, GameObject prefab, bool dummyObject, bool hidden = false)
	{
		if (!prefab)
		{
			Chat.LogError("Failed to load Prefab that does not exist: " + os.Name, true);
			return null;
		}
		try
		{
			if (os.Transform.Position() == Vector3.zero && os.Transform.Scale() == Vector3.zero)
			{
				Chat.LogError("Not loading garbage object, position = (0,0,0) and scale = (0,0,0). Resave this file.", true);
				return null;
			}
			if (float.IsNaN(os.Transform.posX) || float.IsNaN(os.Transform.posY) || float.IsNaN(os.Transform.posZ))
			{
				Chat.LogError("Not loading garbage object, position = NaN. Resave this file.", true);
				return null;
			}
			if (float.IsNaN(os.Transform.rotX) || float.IsNaN(os.Transform.rotY) || float.IsNaN(os.Transform.rotZ))
			{
				Chat.LogError("Not loading garbage object, rotation = NaN. Resave this file.", true);
				return null;
			}
			if (float.IsNaN(os.Transform.scaleX) || float.IsNaN(os.Transform.scaleY) || float.IsNaN(os.Transform.scaleZ))
			{
				Chat.LogError("Not loading garbage object, scale = NaN. Resave this file.", true);
				return null;
			}
			GameObject gameObject;
			if (!dummyObject)
			{
				gameObject = Network.Instantiate(prefab, os.Transform.Position(), Quaternion.Euler(os.Transform.Rotation()), default(NetworkPlayer));
			}
			else
			{
				gameObject = UnityEngine.Object.Instantiate<GameObject>(prefab, os.Transform.Position(), Quaternion.Euler(os.Transform.Rotation()));
				NetworkView component = gameObject.GetComponent<NetworkView>();
				if (component)
				{
					component.disableNetworking = true;
				}
				gameObject.AddComponent<DummyObject>();
			}
			NetworkPhysicsObject networkPhysicsObject = this.NPOFromGO(gameObject);
			if (!networkPhysicsObject)
			{
				Chat.LogError("Tried to load non network physics object.", true);
				return null;
			}
			if (os.AltSound != null || os.MaterialIndex != null || os.MeshIndex != null)
			{
				bool bAltSounds = false;
				int matInt = -1;
				int meshInt = -1;
				if (os.AltSound != null)
				{
					bAltSounds = os.AltSound.Value;
				}
				if (os.MaterialIndex != null)
				{
					matInt = os.MaterialIndex.Value;
				}
				if (os.MeshIndex != null)
				{
					meshInt = os.MeshIndex.Value;
				}
				networkPhysicsObject.SetObject(bAltSounds, meshInt, matInt);
			}
			if (networkPhysicsObject.stackObject)
			{
				if (os.Number != null)
				{
					networkPhysicsObject.stackObject.num_objects_ = os.Number.Value;
				}
				if (os.MeshIndex != null && gameObject.CompareTag("Chip"))
				{
					networkPhysicsObject.stackObject.SetPokerChip(os.MeshIndex.Value);
				}
				if (os.MaterialIndex != null && gameObject.CompareTag("Checker"))
				{
					networkPhysicsObject.stackObject.SetChecker(os.MaterialIndex.Value);
				}
				if (os.Bag != null)
				{
					networkPhysicsObject.stackObject.Order = os.Bag.Order;
				}
			}
			if (networkPhysicsObject.deckScript)
			{
				DeckScript deckScript = networkPhysicsObject.deckScript;
				deckScript.bRandomSpawn = false;
				if (os.SidewaysCard != null)
				{
					deckScript.bSideways = os.SidewaysCard.Value;
				}
				List<int> list = new List<int>();
				for (int i = 0; i < os.DeckIDs.Count; i++)
				{
					list.Add(os.DeckIDs[i]);
				}
				List<ObjectState> list2 = null;
				if (os.ContainedObjects != null)
				{
					list2 = new List<ObjectState>();
					for (int j = 0; j < os.ContainedObjects.Count; j++)
					{
						list2.Add(os.ContainedObjects[j].Clone());
					}
				}
				Dictionary<int, CustomDeckState> customDeck = os.CustomDeck;
				if (customDeck != null && customDeck.Count > 0)
				{
					Dictionary<int, int> dictionary = new Dictionary<int, int>();
					foreach (KeyValuePair<int, CustomDeckState> keyValuePair in customDeck)
					{
						int key = keyValuePair.Key;
						CustomDeckState value = keyValuePair.Value;
						string faceURL = value.FaceURL;
						string backURL = value.BackURL;
						int numWidth = value.NumWidth;
						int numHeight = value.NumHeight;
						bool backIsHidden = value.BackIsHidden;
						bool flag = value.UniqueBack || CardManagerScript.CheckUnique(backURL);
						CustomDeckData cdd = new CustomDeckData(faceURL, backURL, numWidth, numHeight, backIsHidden, flag, value.Type);
						if (flag && os.HideWhenFaceDown == null)
						{
							networkPhysicsObject.IsHiddenWhenFaceDown = false;
						}
						CustomDeckData customDeckData;
						if (!NetworkSingleton<CardManagerScript>.Instance.GetCustomDecks(dummyObject).TryGetValue(key, out customDeckData))
						{
							NetworkSingleton<CardManagerScript>.Instance.DownloadDeckData(key, cdd, dummyObject);
						}
						else if (customDeckData.FaceURL != faceURL || customDeckData.BackURL != backURL || customDeckData.NumWidth != numWidth || customDeckData.NumHeight != numHeight || customDeckData.BackIsHidden != backIsHidden || customDeckData.UniqueBack != flag)
						{
							int nextIndex = NetworkSingleton<CardManagerScript>.Instance.GetNextIndex(dummyObject);
							if (key != nextIndex)
							{
								dictionary[key] = nextIndex;
							}
							NetworkSingleton<CardManagerScript>.Instance.DownloadDeckData(nextIndex, cdd, dummyObject);
						}
					}
					if (dictionary.Count > 0)
					{
						for (int k = 0; k < list.Count; k++)
						{
							int num = 0;
							int l = list[k];
							while (l > 99)
							{
								l -= 100;
								num++;
							}
							int num2;
							if (dictionary.TryGetValue(num, out num2))
							{
								int value2 = l + 100 * num2;
								list[k] = value2;
								if (list2 != null)
								{
									list2[k].CardID = new int?(value2);
								}
							}
						}
					}
				}
				deckScript.SetDeck(list, list2);
				NetworkSingleton<CardManagerScript>.Instance.SetupCard(gameObject, deckScript.get_bottom_card_id(), deckScript.get_top_card_id(), hidden);
			}
			if (os.CardID != null && networkPhysicsObject.cardScript)
			{
				NetworkSingleton<CardManagerScript>.Instance.SetupCard(gameObject, os.CardID.Value, -1, hidden);
				networkPhysicsObject.cardScript.card_id_ = os.CardID.Value;
				if (os.SidewaysCard != null)
				{
					networkPhysicsObject.cardScript.bSideways = os.SidewaysCard.Value;
				}
				Dictionary<int, CustomDeckState> customDeck2 = os.CustomDeck;
				if (customDeck2 != null && customDeck2.Count > 0)
				{
					foreach (KeyValuePair<int, CustomDeckState> keyValuePair2 in customDeck2)
					{
						int key2 = keyValuePair2.Key;
						CustomDeckState value3 = keyValuePair2.Value;
						string faceURL2 = value3.FaceURL;
						string backURL2 = value3.BackURL;
						int numWidth2 = value3.NumWidth;
						int numHeight2 = value3.NumHeight;
						bool backIsHidden2 = value3.BackIsHidden;
						bool flag2 = value3.UniqueBack || CardManagerScript.CheckUnique(backURL2);
						CustomDeckData cdd2 = new CustomDeckData(faceURL2, backURL2, numWidth2, numHeight2, backIsHidden2, flag2, value3.Type);
						if (flag2 && os.HideWhenFaceDown == null)
						{
							networkPhysicsObject.IsHiddenWhenFaceDown = false;
						}
						CustomDeckData customDeckData2;
						if (!NetworkSingleton<CardManagerScript>.Instance.GetCustomDecks(dummyObject).TryGetValue(key2, out customDeckData2))
						{
							NetworkSingleton<CardManagerScript>.Instance.DownloadDeckData(key2, cdd2, dummyObject);
						}
						else if (customDeckData2.FaceURL != faceURL2 || customDeckData2.BackURL != backURL2 || customDeckData2.NumWidth != numWidth2 || customDeckData2.NumHeight != numHeight2 || customDeckData2.BackIsHidden != backIsHidden2 || customDeckData2.UniqueBack != flag2)
						{
							int nextIndex2 = NetworkSingleton<CardManagerScript>.Instance.GetNextIndex(dummyObject);
							int num3 = 0;
							int m = os.CardID.Value;
							while (m > 99)
							{
								m -= 100;
								num3++;
							}
							if (key2 == num3)
							{
								networkPhysicsObject.cardScript.card_id_ = m + 100 * nextIndex2;
							}
							NetworkSingleton<CardManagerScript>.Instance.DownloadDeckData(nextIndex2, cdd2, dummyObject);
						}
					}
				}
				NetworkSingleton<CardManagerScript>.Instance.SetupCard(gameObject, networkPhysicsObject.cardScript.card_id_, networkPhysicsObject.cardScript.card_id_, hidden);
			}
			if (networkPhysicsObject.customImage)
			{
				CustomImageState customImage = os.CustomImage;
				if (customImage != null)
				{
					networkPhysicsObject.customImage.CustomImageURL = customImage.ImageURL;
					networkPhysicsObject.customImage.CustomImageSecondaryURL = customImage.ImageSecondaryURL;
					networkPhysicsObject.customImage.CardScalar = customImage.ImageScalar;
					if (networkPhysicsObject.customBoardScript)
					{
						networkPhysicsObject.customBoardScript.xMulti = customImage.WidthScale;
					}
					CustomDice customDice = networkPhysicsObject.customDice;
					if (networkPhysicsObject && customImage.CustomDice != null)
					{
						customDice.CurrentDiceType = customImage.CustomDice.Type;
					}
					CustomToken customToken = networkPhysicsObject.customToken;
					if (networkPhysicsObject.customToken && customImage.CustomToken != null)
					{
						customToken.Thickness = customImage.CustomToken.Thickness;
						customToken.MergeDistancePixels = customImage.CustomToken.MergeDistancePixels;
						customToken.bStandUp = customImage.CustomToken.StandUp;
						customToken.bStackable = customImage.CustomToken.Stackable;
					}
					CustomJigsawPuzzle customJigsawPuzzle = networkPhysicsObject.customJigsawPuzzle;
					if (customJigsawPuzzle && customImage.CustomJigsawPuzzle != null)
					{
						customJigsawPuzzle.NumPuzzlePieces = customImage.CustomJigsawPuzzle.NumPuzzlePieces;
						customJigsawPuzzle.bImageOnBoard = customImage.CustomJigsawPuzzle.ImageOnBoard;
					}
					CustomTile customTile = networkPhysicsObject.customTile;
					if (customTile && customImage.CustomTile != null)
					{
						customTile.Thickness = customImage.CustomTile.Thickness;
						customTile.CurrentTileType = customImage.CustomTile.Type;
						customTile.bStackable = customImage.CustomTile.Stackable;
						customTile.bStretch = customImage.CustomTile.Stretch;
					}
				}
			}
			if (os.vector != null)
			{
				JigsawPiece component2 = networkPhysicsObject.GetComponent<JigsawPiece>();
				if (component2)
				{
					component2.desiredPosition = os.vector.Value.ToVector();
				}
			}
			networkPhysicsObject.IsLocked = os.Locked;
			networkPhysicsObject.Name = os.Nickname;
			networkPhysicsObject.Description = os.Description;
			networkPhysicsObject.GMNotes = os.GMNotes;
			networkPhysicsObject.Memo = os.Memo;
			networkPhysicsObject.AltLookAngle = os.AltLookAngle;
			if (os.ColorDiffuse != null)
			{
				networkPhysicsObject.DiffuseColor = os.ColorDiffuse.Value.ToColour();
			}
			networkPhysicsObject.IgnoresGrid = !os.Grid;
			networkPhysicsObject.IgnoresSnap = !os.Snap;
			networkPhysicsObject.IgnoresFogOfWar = os.IgnoreFoW;
			networkPhysicsObject.ShowRulerWhenHeld = os.MeasureMovement;
			networkPhysicsObject.IsDragSelectable = os.DragSelectable;
			networkPhysicsObject.DoAutoRaise = os.Autoraise;
			networkPhysicsObject.IsSticky = os.Sticky;
			networkPhysicsObject.ShowTooltip = os.Tooltip;
			networkPhysicsObject.ShowGridProjection = os.GridProjection;
			if (os.HideWhenFaceDown != null)
			{
				networkPhysicsObject.IsHiddenWhenFaceDown = os.HideWhenFaceDown.Value;
			}
			if (os.Hands != null)
			{
				networkPhysicsObject.CanBeHeldInHand = os.Hands.Value;
			}
			networkPhysicsObject.LayoutGroupSortIndex = os.LayoutGroupSortIndex;
			networkPhysicsObject.SetScale(os.Transform.Scale(), false);
			if (os.Tags != null)
			{
				networkPhysicsObject.LoadTags(os.Tags);
			}
			StackObject stackObject = networkPhysicsObject.stackObject;
			if (stackObject && os.ContainedObjects != null)
			{
				List<ObjectState> list3 = new List<ObjectState>();
				for (int n = 0; n < os.ContainedObjects.Count; n++)
				{
					list3.Add(os.ContainedObjects[n].Clone());
				}
				if (stackObject.IsInfiniteStack && list3.Count > 0)
				{
					stackObject.AddToInfiniteBag(list3[0]);
				}
				else
				{
					stackObject.ObjectsHolder = list3;
					stackObject.num_objects_ = list3.Count;
				}
			}
			CustomMesh customMesh = networkPhysicsObject.customMesh;
			if (customMesh)
			{
				customMesh.customMeshState = os.CustomMesh;
			}
			CustomAssetbundle customAssetbundle = networkPhysicsObject.customAssetbundle;
			if (customAssetbundle)
			{
				CustomAssetbundleState customAssetbundle2 = os.CustomAssetbundle;
				if (customAssetbundle2 != null)
				{
					customAssetbundle.CustomAssetbundleURL = customAssetbundle2.AssetbundleURL;
					customAssetbundle.CustomAssetbundleSecondaryURL = customAssetbundle2.AssetbundleSecondaryURL;
					customAssetbundle.TypeInt = customAssetbundle2.TypeIndex;
					customAssetbundle.MaterialInt = customAssetbundle2.MaterialIndex;
					customAssetbundle.LoopEffectIndex = customAssetbundle2.LoopingEffectIndex;
				}
			}
			CustomPDF customPDF = networkPhysicsObject.customPDF;
			if (customPDF)
			{
				CustomPDFState customPDF2 = os.CustomPDF;
				if (customPDF2 != null)
				{
					customPDF.CustomPDFURL = customPDF2.PDFUrl;
					customPDF.CustomPDFPassword = customPDF2.PDFPassword;
					customPDF.CurrentPDFPage = customPDF2.PDFPage;
					customPDF.PageDisplayOffset = customPDF2.PDFPageOffset;
				}
			}
			HiddenZone hiddenZone = networkPhysicsObject.hiddenZone;
			if (hiddenZone)
			{
				if (os.FogColor != null)
				{
					hiddenZone.SetOwningColor(os.FogColor);
				}
				if (os.FogHidePointers != null)
				{
					hiddenZone.SetPointersAreHidden(os.FogHidePointers.Value);
				}
				if (os.FogReverseHiding != null)
				{
					hiddenZone.SetHidingIsReversed(os.FogReverseHiding.Value);
				}
				if (os.FogSeethrough != null)
				{
					hiddenZone.SetTranslucent(os.FogSeethrough.Value);
				}
			}
			HandZone handZone = networkPhysicsObject.handZone;
			if (handZone)
			{
				if (os.FogColor != null)
				{
					handZone.TriggerLabel = os.FogColor;
				}
				if (os.miscID != null)
				{
					Action <>9__2;
					Wait.Condition(delegate
					{
						Action action;
						if ((action = <>9__2) == null)
						{
							action = (<>9__2 = delegate()
							{
								handZone.MoveToStash(this.NPOFromGUID(os.miscID));
							});
						}
						Wait.Frames(action, 2);
					}, () => this.NPOFromGUID(os.miscID), float.PositiveInfinity, null);
				}
			}
			FogOfWarZone fogOfWarZone = networkPhysicsObject.fogOfWarZone;
			if (fogOfWarZone)
			{
				FogOfWarState fogOfWar = os.FogOfWar;
				if (fogOfWar != null)
				{
					fogOfWarZone.HideGmPointer = fogOfWar.HideGmPointer;
					fogOfWarZone.HideObjects = fogOfWar.HideObjects;
					fogOfWarZone.ReHideObjects = fogOfWar.ReHideObjects;
					fogOfWarZone.FogHeight = fogOfWar.Height;
					fogOfWarZone.RevealedLocations = fogOfWar.RevealedLocations;
				}
			}
			FogOfWarRevealer fogOfWarRevealer = networkPhysicsObject.fogOfWarRevealer;
			if (fogOfWarRevealer)
			{
				FogOfWarRevealerState fogOfWarRevealer2 = os.FogOfWarRevealer;
				if (fogOfWarRevealer2 != null)
				{
					fogOfWarRevealer.Active = fogOfWarRevealer2.Active;
					fogOfWarRevealer.ShowFoWOutline = fogOfWarRevealer2.ShowOutLine;
					fogOfWarRevealer.Color = fogOfWarRevealer2.Color;
					fogOfWarRevealer.Range = fogOfWarRevealer2.Range;
					fogOfWarRevealer.Height = fogOfWarRevealer2.Height;
					fogOfWarRevealer.FoV = fogOfWarRevealer2.FoV;
					fogOfWarRevealer.FoVOffset = fogOfWarRevealer2.FoVOffset;
				}
			}
			LayoutZone layoutZone = networkPhysicsObject.layoutZone;
			if (layoutZone)
			{
				LayoutZoneState layoutZone2 = os.LayoutZone;
				if (layoutZone2 != null)
				{
					layoutZone.SetOptions(layoutZone2.Options, false);
					layoutZone.DelayedLoadSerializedGroups(layoutZone2.GroupsInZone);
				}
			}
			ClockScript clockScript = networkPhysicsObject.clockScript;
			if (clockScript)
			{
				ClockState clock = os.Clock;
				if (clock != null)
				{
					ClockScript.ClockMode mode = clock.Mode;
					clockScript.currentClockMode = mode;
					if (mode == ClockScript.ClockMode.Timer)
					{
						clockScript.StartTimer(clock.SecondsPassed, false);
					}
					else if (mode == ClockScript.ClockMode.Stopwatch)
					{
						clockScript.StopwatchDate = DateTime.Now.AddSeconds((double)(-(double)clock.SecondsPassed));
					}
					clockScript.bPaused = clock.Paused;
					if (clockScript.bPaused)
					{
						clockScript.PausedTime = clock.SecondsPassed;
					}
				}
			}
			CounterScript counterScript = networkPhysicsObject.counterScript;
			if (counterScript)
			{
				CounterState counter = os.Counter;
				if (counter != null)
				{
					counterScript.uiinput.value = counter.value.ToString();
				}
			}
			TabletScript tabletScript = networkPhysicsObject.tabletScript;
			if (tabletScript)
			{
				TabletState tablet = os.Tablet;
				if (tablet != null)
				{
					tabletScript.OverrideURL = tablet.PageURL;
				}
			}
			Mp3PlayerScript mp3PlayerScript = networkPhysicsObject.mp3PlayerScript;
			if (mp3PlayerScript)
			{
				Mp3PlayerState mp3Player = os.Mp3Player;
				if (mp3Player != null)
				{
					mp3PlayerScript.SyncPlayer(new Mp3PlayerScript.State(mp3Player.songTitle, mp3Player.genre, mp3Player.volume, 0f, mp3Player.isPlaying, (int)mp3Player.menu, mp3Player.menuTitle, mp3Player.loopOne));
				}
			}
			CalculatorScript calculatorScript = networkPhysicsObject.calculatorScript;
			if (calculatorScript)
			{
				CalculatorState calculator = os.Calculator;
				if (calculator != null)
				{
					float value4;
					float.TryParse(calculator.value, out value4);
					calculatorScript.SyncPlayer(new CalculatorScript.State(calculator.value, value4, calculator.memory, CalculatorActions.NONE, false));
				}
			}
			TextTool textTool = networkPhysicsObject.textTool;
			if (textTool)
			{
				TextState text = os.Text;
				if (text != null)
				{
					if (text.colorstate != null)
					{
						textTool.SetColor(text.colorstate.Value.ToColour());
					}
					textTool.input.label.fontSize = os.Text.fontSize;
					textTool.input.value = text.Text;
				}
			}
			RPGFigurines rpgFigurines = networkPhysicsObject.rpgFigurines;
			if (rpgFigurines)
			{
				if (os.RPGmode != null)
				{
					rpgFigurines.bMode = os.RPGmode.Value;
				}
				if (os.RPGdead != null)
				{
					rpgFigurines.bDead = os.RPGdead.Value;
				}
			}
			if (os.CustomUIAssets != null)
			{
				networkPhysicsObject.xmlUI.CustomAssets = os.CustomUIAssets;
			}
			if (!string.IsNullOrEmpty(os.XmlUI))
			{
				networkPhysicsObject.xmlUI.xmlui_code = os.XmlUI;
			}
			networkPhysicsObject.xmlUI.Init(os.XmlUI, false);
			if (!dummyObject)
			{
				if (!string.IsNullOrEmpty(os.LuaScript))
				{
					networkPhysicsObject.luaGameObjectScript.enabled = true;
					networkPhysicsObject.luaGameObjectScript.script_code = os.LuaScript;
				}
				networkPhysicsObject.luaGameObjectScript.script_state = os.LuaScriptState;
			}
			if (os.Rigidbody != null)
			{
				networkPhysicsObject.OverrideRigidbody = new RigidbodyState
				{
					Mass = os.Rigidbody.Mass,
					Drag = os.Rigidbody.Drag,
					AngularDrag = os.Rigidbody.AngularDrag,
					UseGravity = os.Rigidbody.UseGravity
				};
			}
			if (os.PhysicsMaterial != null)
			{
				networkPhysicsObject.OverridePhysicsMaterial = new PhysicsMaterialState
				{
					DynamicFriction = os.PhysicsMaterial.DynamicFriction,
					StaticFriction = os.PhysicsMaterial.StaticFriction,
					Bounciness = os.PhysicsMaterial.Bounciness,
					FrictionCombine = os.PhysicsMaterial.FrictionCombine,
					BounceCombine = os.PhysicsMaterial.BounceCombine
				};
			}
			if (os.JointFixed != null || os.JointHinge != null || os.JointSpring != null)
			{
				JointState jointState = null;
				if (os.JointFixed != null && !string.IsNullOrEmpty(os.JointFixed.ConnectedBodyGUID))
				{
					jointState = os.JointFixed;
				}
				else if (os.JointHinge != null && !string.IsNullOrEmpty(os.JointHinge.ConnectedBodyGUID))
				{
					jointState = os.JointHinge;
				}
				else if (os.JointSpring != null && !string.IsNullOrEmpty(os.JointSpring.ConnectedBodyGUID))
				{
					jointState = os.JointSpring;
				}
				if (jointState != null)
				{
					foreach (KeyValuePair<string, ManagerPhysicsObject.JointRelation> keyValuePair3 in this.JointRelationFromGUID)
					{
						if (keyValuePair3.Key == jointState.ConnectedBodyGUID)
						{
							ManagerPhysicsObject.JointRelation value5 = keyValuePair3.Value;
							if (value5.Target)
							{
								this.AddJoint(gameObject, jointState, value5.Target.GetComponent<Rigidbody>());
								break;
							}
							value5.JointStateFromGO[gameObject] = jointState;
							break;
						}
					}
				}
			}
			foreach (KeyValuePair<string, ManagerPhysicsObject.JointRelation> keyValuePair4 in this.JointRelationFromGUID)
			{
				if (keyValuePair4.Key == os.GUID)
				{
					ManagerPhysicsObject.JointRelation value6 = keyValuePair4.Value;
					value6.Target = gameObject;
					using (Dictionary<GameObject, JointState>.Enumerator enumerator3 = value6.JointStateFromGO.GetEnumerator())
					{
						while (enumerator3.MoveNext())
						{
							KeyValuePair<GameObject, JointState> keyValuePair5 = enumerator3.Current;
							this.AddJoint(keyValuePair5.Key, keyValuePair5.Value, networkPhysicsObject.rigidbody);
						}
						break;
					}
				}
			}
			if (os.AttachedSnapPoints != null && os.AttachedSnapPoints.Count > 0 && !dummyObject)
			{
				NetworkSingleton<SnapPointManager>.Instance.AddSnapPoints(os.AttachedSnapPoints, networkPhysicsObject.networkView);
			}
			if (os.AttachedVectorLines != null && os.AttachedVectorLines.Count > 0)
			{
				NetworkSingleton<ToolVector>.Instance.AddLineStates(os.AttachedVectorLines, networkPhysicsObject);
			}
			if (os.AttachedDecals != null)
			{
				for (int num4 = 0; num4 < os.AttachedDecals.Count; num4++)
				{
					NetworkSingleton<DecalManager>.Instance.AddDecal(os.AttachedDecals[num4], networkPhysicsObject.networkView);
				}
			}
			if (os.States != null)
			{
				Dictionary<int, ObjectState> dictionary2 = new Dictionary<int, ObjectState>(os.States.Count);
				foreach (KeyValuePair<int, ObjectState> keyValuePair6 in os.States)
				{
					dictionary2.Add(keyValuePair6.Key, keyValuePair6.Value.Clone());
				}
				networkPhysicsObject.States = dictionary2;
			}
			if (os.RotationValues != null)
			{
				List<RotationValue> list4 = new List<RotationValue>(os.RotationValues.Count);
				for (int num5 = 0; num5 < os.RotationValues.Count; num5++)
				{
					RotationValueState rotationValueState = os.RotationValues[num5];
					list4.Add(new RotationValue(rotationValueState.Value, rotationValueState.Rotation.ToVector()));
				}
				networkPhysicsObject.SetRotationValues(list4);
			}
			networkPhysicsObject.Value = os.Value;
			if (!string.IsNullOrEmpty(os.GUID))
			{
				networkPhysicsObject.GUID = os.GUID;
			}
			if (os.ChildObjects != null && os.ChildObjects.Count > 0)
			{
				networkPhysicsObject.childSpawner.SetChildren(os.ChildObjects);
			}
			if (dummyObject)
			{
				ManagerPhysicsObject.CleanupDummyObject(gameObject);
			}
			return gameObject;
		}
		catch (Exception ex)
		{
			UnityEngine.Debug.LogException(ex);
			Chat.LogError(string.Concat(new string[]
			{
				"Failed to load ",
				os.Name,
				". ",
				ex.Message,
				"."
			}), true);
		}
		return null;
	}

	// Token: 0x060015A2 RID: 5538 RVA: 0x00094798 File Offset: 0x00092998
	public GameObject LoadPhysicsState(PhysicsState ps)
	{
		try
		{
			if (ps.name.Substring(0, 5) != "*bag*")
			{
				GameObject gameObject = (GameObject)Resources.Load(ps.name.Substring(0, ps.name.Length - 7));
				if (gameObject)
				{
					GameObject gameObject2 = Network.Instantiate(gameObject, new Vector3(ps.posX, ps.posY, ps.posZ), Quaternion.Euler(new Vector3(ps.rotX, ps.rotY, ps.rotZ)), default(NetworkPlayer));
					NetworkPhysicsObject networkPhysicsObject = this.NPOFromGO(gameObject2);
					if (networkPhysicsObject && !gameObject2.GetComponent<DeckScript>() && (ps.bAltSound || ps.MatInt != -1 || ps.MeshInt != -1))
					{
						networkPhysicsObject.SetObject(ps.bAltSound, ps.MeshInt, ps.MatInt);
					}
					StackObject component = gameObject2.GetComponent<StackObject>();
					if (component && ps.Num != -1)
					{
						component.num_objects_ = ps.Num;
						component.SetPokerChip(ps.MatInt);
					}
					if (gameObject2.GetComponent<DeckScript>())
					{
						DeckScript component2 = gameObject2.GetComponent<DeckScript>();
						component2.bRandomSpawn = false;
						List<int> list = new List<int>();
						for (int i = 0; i < ps.List.Count; i++)
						{
							list.Add(ps.List[i]);
						}
						if (ps.Note == "1" || ps.Note == "True")
						{
							component2.bSideways = true;
						}
						else
						{
							string[] array = ps.Note.Split(new char[]
							{
								' '
							});
							if (array.Length > 1)
							{
								component2.bSideways = bool.Parse(array[0]);
								Dictionary<int, int> dictionary = new Dictionary<int, int>();
								for (int j = 1; j < array.Length; j++)
								{
									string[] array2 = array[j].Split(new char[]
									{
										','
									});
									int num = int.Parse(array2[0]);
									string text = array2[2];
									string text2 = array2[1];
									CustomDeckData cdd = new CustomDeckData(text, text2, 10, 7, false, CardManagerScript.CheckUnique(text2), CardType.RectangleRounded);
									CustomDeckData customDeckData;
									if (!NetworkSingleton<CardManagerScript>.Instance.GetCustomDecks(false).TryGetValue(num, out customDeckData))
									{
										NetworkSingleton<CardManagerScript>.Instance.DownloadDeckData(num, cdd, false);
									}
									else if (customDeckData.FaceURL != text || customDeckData.BackURL != text2)
									{
										int nextIndex = NetworkSingleton<CardManagerScript>.Instance.GetNextIndex(false);
										if (num != nextIndex)
										{
											dictionary.Add(num, nextIndex);
										}
										NetworkSingleton<CardManagerScript>.Instance.DownloadDeckData(nextIndex, cdd, false);
									}
								}
								if (dictionary.Count > 0)
								{
									for (int k = 0; k < list.Count; k++)
									{
										int num2 = 0;
										int l = list[k];
										while (l > 99)
										{
											l -= 100;
											num2++;
										}
										int num3;
										if (dictionary.TryGetValue(num2, out num3))
										{
											list[k] = l + 100 * num3;
										}
									}
								}
							}
						}
						component2.SetDeck(list, null);
						NetworkSingleton<CardManagerScript>.Instance.SetupCard(gameObject2, component2.get_bottom_card_id(), component2.get_top_card_id(), false);
					}
					CardScript component3 = gameObject2.GetComponent<CardScript>();
					if (component3 && ps.idint != -1)
					{
						NetworkSingleton<CardManagerScript>.Instance.SetupCard(gameObject2, ps.idint, -1, false);
						component3.card_id_ = ps.idint;
						if (ps.Note == "1" || ps.Note == "True")
						{
							component3.bSideways = true;
						}
						else
						{
							string[] array3 = ps.Note.Split(new char[]
							{
								' '
							});
							if (array3.Length > 1)
							{
								component3.bSideways = bool.Parse(array3[0]);
								for (int m = 1; m < array3.Length; m++)
								{
									string[] array4 = array3[m].Split(new char[]
									{
										','
									});
									int num4 = int.Parse(array4[0]);
									string text3 = array4[2];
									string text4 = array4[1];
									CustomDeckData cdd2 = new CustomDeckData(text3, text4, 10, 7, false, CardManagerScript.CheckUnique(text4), CardType.RectangleRounded);
									CustomDeckData customDeckData2;
									if (!NetworkSingleton<CardManagerScript>.Instance.GetCustomDecks(false).TryGetValue(num4, out customDeckData2))
									{
										NetworkSingleton<CardManagerScript>.Instance.DownloadDeckData(num4, cdd2, false);
									}
									else if (customDeckData2.FaceURL != text3 || customDeckData2.BackURL != text4)
									{
										int nextIndex2 = NetworkSingleton<CardManagerScript>.Instance.GetNextIndex(false);
										int num5 = 0;
										int n = ps.idint;
										while (n > 99)
										{
											n -= 100;
											num5++;
										}
										if (num4 == num5)
										{
											gameObject2.GetComponent<CardScript>().card_id_ = n + 100 * nextIndex2;
										}
										NetworkSingleton<CardManagerScript>.Instance.DownloadDeckData(nextIndex2, cdd2, false);
									}
								}
							}
						}
					}
					CustomImage component4 = gameObject2.GetComponent<CustomImage>();
					if (component4)
					{
						CustomBoardScript component5 = gameObject2.GetComponent<CustomBoardScript>();
						if (component5)
						{
							string[] array5 = ps.Note.Split(new char[]
							{
								' '
							}, StringSplitOptions.RemoveEmptyEntries);
							component4.CustomImageURL = array5[0];
							if (array5.Length > 1)
							{
								component5.xMulti = float.Parse(array5[1]);
							}
						}
						else
						{
							component4.CustomImageURL = ps.Note;
						}
					}
					if (ps.StringList != null && ps.StringList.Count > 0 && ps.StringList[0] == "1")
					{
						networkPhysicsObject.IsLocked = true;
					}
					if (ps.StringList != null && ps.StringList.Count > 1)
					{
						float num6 = float.Parse(ps.StringList[1]);
						networkPhysicsObject.SetScale(new Vector3(num6, num6, num6), false);
						if (ps.StringList.Count > 3)
						{
							networkPhysicsObject.Name = ps.StringList[2];
							networkPhysicsObject.Description = ps.StringList[3];
						}
						if (ps.StringList.Count > 5)
						{
							string[] array6 = ps.StringList[4].Split(new char[]
							{
								' '
							});
							if (!gameObject2.CompareTag("Bag"))
							{
								networkPhysicsObject.DiffuseColor = new Color(float.Parse(array6[0]), float.Parse(array6[1]), float.Parse(array6[2]));
							}
							networkPhysicsObject.IgnoresGrid = bool.Parse(ps.StringList[5]);
						}
					}
					this.saveBag = null;
					if (component && component.bBag)
					{
						this.saveBag = gameObject2;
						for (int num7 = 0; num7 < this.bagList.Count; num7++)
						{
							component.ObjectsHolder.Add(this.ObjectStateFromPhysicsState(this.bagList[num7]));
						}
						this.bagList.Clear();
					}
					CustomMesh component6 = gameObject2.GetComponent<CustomMesh>();
					if (component6)
					{
						try
						{
							string[] array7 = ps.Note.Split(new char[]
							{
								' '
							});
							CustomMeshState customMeshState = component6.customMeshState;
							if (array7.Length > 3)
							{
								customMeshState.MeshURL = array7[0];
								customMeshState.DiffuseURL = array7[1];
								customMeshState.MaterialIndex = int.Parse(array7[2]);
								customMeshState.TypeIndex = int.Parse(array7[3]);
							}
							if (array7.Length > 5)
							{
								customMeshState.ColliderURL = array7[4];
								customMeshState.Convex = (array7[5] != "1");
							}
							if (array7.Length > 6)
							{
								customMeshState.NormalURL = array7[6];
							}
						}
						catch (Exception ex)
						{
							Chat.LogWarning("Error with custom object. Attempting workaround. " + ex.Message, true);
						}
						finally
						{
							string[] array8 = ps.Note.Split(new char[]
							{
								' '
							}, StringSplitOptions.RemoveEmptyEntries);
							CustomMeshState customMeshState2 = component6.customMeshState;
							if (array8.Length > 3)
							{
								customMeshState2.MeshURL = array8[0];
								customMeshState2.DiffuseURL = array8[1];
								customMeshState2.MaterialIndex = int.Parse(array8[2]);
								customMeshState2.TypeIndex = int.Parse(array8[3]);
							}
							if (array8.Length > 5)
							{
								customMeshState2.ColliderURL = array8[4];
								customMeshState2.Convex = (array8[5] != "1");
							}
							if (array8.Length > 6)
							{
								customMeshState2.NormalURL = array8[6];
							}
						}
					}
					HiddenZone component7 = gameObject2.GetComponent<HiddenZone>();
					if (component7)
					{
						string[] array9 = ps.Note.Split(new char[]
						{
							' '
						});
						if (array9.Length > 3)
						{
							Vector3 scale = new Vector3(float.Parse(array9[0]), float.Parse(array9[1]), float.Parse(array9[2]));
							networkPhysicsObject.SetScale(scale, false);
							component7.SetOwningColor(array9[3]);
						}
					}
					ClockScript component8 = gameObject2.GetComponent<ClockScript>();
					if (component8)
					{
						string[] array10 = ps.Note.Split(new char[]
						{
							' '
						});
						if (array10.Length > 1)
						{
							ClockScript.ClockMode clockMode = (ClockScript.ClockMode)int.Parse(array10[0]);
							component8.currentClockMode = clockMode;
							if (clockMode == ClockScript.ClockMode.Timer)
							{
								component8.StartTimer(int.Parse(array10[1]), false);
							}
							else if (clockMode == ClockScript.ClockMode.Stopwatch)
							{
								component8.StopwatchDate = DateTime.Now.AddSeconds((double)(-(double)int.Parse(array10[1])));
							}
						}
						if (array10.Length > 2)
						{
							component8.bPaused = bool.Parse(array10[2]);
							if (component8.bPaused)
							{
								component8.PausedTime = int.Parse(array10[1]);
							}
						}
					}
					return gameObject2;
				}
			}
			else
			{
				ps.name = ps.name.Substring(5, ps.name.Length - 5);
				if (this.saveBag)
				{
					this.saveBag.GetComponent<StackObject>().ObjectsHolder.Add(this.ObjectStateFromPhysicsState(ps));
				}
				else
				{
					this.bagList.Add(ps);
				}
			}
		}
		catch (Exception ex2)
		{
			Chat.LogError(string.Concat(new string[]
			{
				"Failed to load ",
				ps.name,
				". ",
				ex2.Message,
				"."
			}), true);
		}
		return null;
	}

	// Token: 0x060015A3 RID: 5539 RVA: 0x000951B8 File Offset: 0x000933B8
	public SaveState CurrentState()
	{
		EventManager.TriggerGameSave();
		SaveState saveState = new SaveState
		{
			SaveName = NetworkSingleton<GameOptions>.Instance.GameName,
			EpochTime = new uint?(SerializationScript.GetTimeFromEpoch(DateTime.UtcNow)),
			GameMode = NetworkSingleton<GameOptions>.Instance.GameName,
			Gravity = NetworkSingleton<GameOptions>.Instance.Gravity,
			PlayArea = NetworkSingleton<GameOptions>.Instance.PlayArea,
			GameType = NetworkSingleton<GameOptions>.Instance.GameType,
			GameComplexity = NetworkSingleton<GameOptions>.Instance.GameComplexity,
			PlayingTime = NetworkSingleton<GameOptions>.Instance.PlayingTime,
			PlayerCounts = NetworkSingleton<GameOptions>.Instance.PlayerCounts,
			Tags = NetworkSingleton<GameOptions>.Instance.Tags,
			Date = DateTime.Now.ToString()
		};
		NetworkPhysicsObject tableNPO = NetworkSingleton<ManagerPhysicsObject>.Instance.TableNPO;
		saveState.Table = tableNPO.InternalName;
		CustomImage customImage = tableNPO.customImage;
		if (customImage)
		{
			saveState.TableURL = customImage.CustomImageURL;
		}
		saveState.Note = NetworkSingleton<NetworkUI>.Instance.notepadstring;
		UINotebook component = NetworkSingleton<NetworkUI>.Instance.GUIRules.GetComponent<UINotebook>();
		saveState.TabStates = component.GetSaveState();
		saveState.Sky = this.Sky.InternalName;
		if (CustomSky.ActiveCustomSky && !string.IsNullOrEmpty(CustomSky.ActiveCustomSky.CustomSkyURL))
		{
			saveState.SkyURL = CustomSky.ActiveCustomSky.CustomSkyURL;
		}
		CustomMusicPlayer instance = NetworkSingleton<CustomMusicPlayer>.Instance;
		if (instance.HasSongsLoaded())
		{
			saveState.MusicPlayer = new MusicPlayerState
			{
				AudioLibrary = new List<ValueTuple<string, string>>(instance.SortedAudioLibrary),
				CurrentAudioTitle = instance.CurrentAudioName,
				CurrentAudioURL = instance.CurrentAudioUrl,
				RepeatSong = instance.RepeatSong,
				PlaylistEntry = instance.PlaylistEntry
			};
		}
		saveState.Grid = NetworkSingleton<GridOptions>.Instance.gridState;
		HandsState handsState = NetworkSingleton<Hands>.Instance.handsState;
		saveState.Hands = handsState;
		saveState.Lighting = NetworkSingleton<LightingScript>.Instance.lightingState;
		saveState.ComponentTags = NetworkSingleton<ComponentTags>.Instance.tagsState;
		saveState.Turns = NetworkSingleton<Turns>.Instance.turnsState;
		saveState.VectorLines = NetworkSingleton<ToolVector>.Instance.GetLineStates(null);
		LuaGlobalScriptManager instance2 = LuaGlobalScriptManager.Instance;
		if (instance2.XmlUI.CustomAssets.Count > 0)
		{
			saveState.CustomUIAssets = instance2.XmlUI.CustomAssets;
		}
		saveState.XmlUI = instance2.XmlUI.xmlui_code;
		saveState.LuaScript = instance2.script_code;
		saveState.LuaScriptState = instance2.script_state;
		saveState.SnapPoints = NetworkSingleton<SnapPointManager>.Instance.GetSnapPointSaveStates(null);
		saveState.DecalPallet = new List<CustomDecalState>(NetworkSingleton<DecalManager>.Instance.DecalPallet);
		saveState.Decals = NetworkSingleton<DecalManager>.Instance.GetDecalStates(null);
		for (int i = 0; i < 10; i++)
		{
			if (Singleton<CameraController>.Instance.CameraStateInUse(i))
			{
				saveState.CameraStates = Singleton<CameraController>.Instance.CameraStates;
				break;
			}
		}
		saveState.VersionNumber = NetworkSingleton<NetworkUI>.Instance.VersionNumber;
		saveState.ObjectStates = new List<ObjectState>(this.GrabbableNPOs.Count);
		for (int j = 0; j < this.GrabbableNPOs.Count; j++)
		{
			NetworkPhysicsObject networkPhysicsObject = this.GrabbableNPOs[j];
			if (networkPhysicsObject.IsSaved && !networkPhysicsObject.IsDestroyed)
			{
				saveState.ObjectStates.Add(this.SaveObjectState(networkPhysicsObject));
			}
		}
		return saveState;
	}

	// Token: 0x060015A4 RID: 5540 RVA: 0x00095524 File Offset: 0x00093724
	public void LoadSaveState(SaveState ss, bool resetCardManager = false, bool resetCustomManager = true)
	{
		this.ResetTable(resetCardManager, resetCustomManager);
		LuaGlobalScriptManager instance = LuaGlobalScriptManager.Instance;
		instance.loaded = false;
		NetworkSingleton<NetworkUI>.Instance.notepadstring = ss.Note;
		UINotebook component = NetworkSingleton<NetworkUI>.Instance.GUIRules.GetComponent<UINotebook>();
		component.LoadNotepad(ss.TabStates);
		component.NotepadChange();
		if (ss.TabStates.Count <= 0 && !string.IsNullOrEmpty(ss.Rules))
		{
			component.SetTabFromRulesLegacy(ss.Rules);
		}
		if (!string.IsNullOrEmpty(ss.Table))
		{
			this.ChangeTable(ss.Table);
		}
		if (!string.IsNullOrEmpty(ss.TableURL))
		{
			CustomImage component2 = NetworkSingleton<ManagerPhysicsObject>.Instance.Table.GetComponent<CustomImage>();
			if (component2 && component2.CustomImageURL != ss.TableURL)
			{
				component2.CustomImageURL = ss.TableURL;
			}
		}
		if (!string.IsNullOrEmpty(ss.GameMode))
		{
			if (ss.GameMode != "None")
			{
				if (NetworkSingleton<GameOptions>.Instance.GameName != ss.GameMode)
				{
					NetworkSingleton<NetworkUI>.Instance.SetCurrentGame(ss.GameMode);
				}
			}
			else if (NetworkSingleton<GameOptions>.Instance.GameName != ss.SaveName)
			{
				NetworkSingleton<NetworkUI>.Instance.SetCurrentGame(ss.SaveName);
			}
		}
		NetworkSingleton<GameOptions>.Instance.Gravity = ss.Gravity;
		NetworkSingleton<GameOptions>.Instance.PlayArea = ss.PlayArea;
		NetworkSingleton<GameOptions>.Instance.GameType = ss.GameType;
		NetworkSingleton<GameOptions>.Instance.GameComplexity = ss.GameComplexity;
		NetworkSingleton<GameOptions>.Instance.PlayingTime = ss.PlayingTime;
		NetworkSingleton<GameOptions>.Instance.PlayerCounts = ss.PlayerCounts;
		NetworkSingleton<GameOptions>.Instance.Tags = ss.Tags;
		NetworkSingleton<CustomMusicPlayer>.Instance.InitFromState(ss.MusicPlayer);
		if (ss.Grid != null)
		{
			NetworkSingleton<GridOptions>.Instance.gridState = ss.Grid;
		}
		if (ss.Lighting != null)
		{
			NetworkSingleton<LightingScript>.Instance.lightingState = ss.Lighting;
		}
		if (ss.ComponentTags != null)
		{
			NetworkSingleton<ComponentTags>.Instance.SetTagsState(ss.ComponentTags);
		}
		else
		{
			NetworkSingleton<ComponentTags>.Instance.Reset();
		}
		HandsState hands = ss.Hands;
		if (hands != null)
		{
			NetworkSingleton<Hands>.Instance.handsState = hands;
			this.TableScript.CustomHands = true;
			if (hands.HandTransforms != null)
			{
				this.TableScript.LoadHands(hands.HandTransforms);
				hands.HandTransforms = null;
			}
		}
		if (ss.Turns != null)
		{
			NetworkSingleton<Turns>.Instance.turnsState = ss.Turns;
		}
		if (ss.VectorLines != null && ss.VectorLines.Count > 0)
		{
			NetworkSingleton<ToolVector>.Instance.AddLineStates(ss.VectorLines, null);
		}
		XmlUIScript xmlUI = instance.XmlUI;
		if (ss.CustomUIAssets != null)
		{
			xmlUI.CustomAssets = ss.CustomUIAssets;
		}
		if (!string.IsNullOrEmpty(ss.XmlUI))
		{
			xmlUI.xmlui_code = ss.XmlUI;
		}
		xmlUI.Init(ss.XmlUI, false);
		if (!string.IsNullOrEmpty(ss.LuaScript))
		{
			instance.script_code = ss.LuaScript;
			instance.GlobalDummyObject.script_code = ss.LuaScript;
			instance.script_state = ss.LuaScriptState;
			instance.Init();
		}
		if (!string.IsNullOrEmpty(ss.Sky) && this.Sky.InternalName != ss.Sky)
		{
			this.DestroySky();
			GameObject gameObject = (GameObject)Resources.Load(ss.Sky);
			Network.Instantiate(gameObject, Vector3.zero, gameObject.transform.rotation, default(NetworkPlayer));
		}
		if (!string.IsNullOrEmpty(ss.SkyURL))
		{
			if ((CustomSky.ActiveCustomSky && CustomSky.ActiveCustomSky.CustomSkyURL != ss.SkyURL) || !CustomSky.ActiveCustomSky)
			{
				this.DestroyCustomSky();
				Network.Instantiate(NetworkSingleton<GameMode>.Instance.Sky_Custom, Vector3.zero, NetworkSingleton<GameMode>.Instance.Sky_Custom.transform.rotation, default(NetworkPlayer)).GetComponent<CustomSky>().CustomSkyURL = ss.SkyURL;
			}
		}
		else
		{
			this.DestroyCustomSky();
		}
		if (ss.SnapPoints != null && ss.SnapPoints.Count > 0)
		{
			NetworkSingleton<SnapPointManager>.Instance.AddSnapPoints(ss.SnapPoints, null);
		}
		if (ss.DecalPallet != null)
		{
			for (int i = 0; i < ss.DecalPallet.Count; i++)
			{
				NetworkSingleton<DecalManager>.Instance.AddDecalPallet(ss.DecalPallet[i]);
			}
		}
		if (ss.Decals != null)
		{
			for (int j = 0; j < ss.Decals.Count; j++)
			{
				NetworkSingleton<DecalManager>.Instance.AddDecal(ss.Decals[j], null);
			}
		}
		if (ss.CameraStates != null)
		{
			this.SetCameraStates(ss.CameraStates);
		}
		this.GenerateJointRelations(ss.ObjectStates);
		for (int k = 0; k < ss.ObjectStates.Count; k++)
		{
			this.LoadObjectState(ss.ObjectStates[k], false, false);
		}
		this.JointRelationFromGUID.Clear();
		base.networkView.RPC(RPCTarget.All, new Action(this.RPCCheckLoadingSaveComplete));
	}

	// Token: 0x060015A5 RID: 5541 RVA: 0x00095A58 File Offset: 0x00093C58
	public void LoadStateList(List<PhysicsState> loadState, bool resetCardManager = false, bool resetCustomManager = true)
	{
		this.ResetTable(resetCardManager, resetCustomManager);
		LuaGlobalScriptManager.Instance.loaded = false;
		NetworkSingleton<NetworkUI>.Instance.notepadstring = loadState[0].Note;
		bool flag = false;
		if (loadState[0].StringList != null && loadState[0].StringList[0] != "")
		{
			this.ChangeTable(loadState[0].StringList[0]);
		}
		else
		{
			flag = true;
		}
		if (loadState[0].StringList != null && loadState[0].StringList.Count > 1 && loadState[0].StringList[1] != "")
		{
			CustomImage component = NetworkSingleton<ManagerPhysicsObject>.Instance.Table.GetComponent<CustomImage>();
			if (component && component.CustomImageURL != loadState[0].StringList[1])
			{
				component.CustomImageURL = loadState[0].StringList[1];
				if (flag)
				{
					component.CallCustomRPC();
				}
			}
		}
		if (loadState[0].StringList != null && loadState[0].StringList.Count > 2 && loadState[0].StringList[2] != "")
		{
			NetworkSingleton<NetworkUI>.Instance.SetCurrentGame(loadState[0].StringList[2]);
		}
		if (loadState[0].StringList != null && loadState[0].StringList.Count > 4 && loadState[0].StringList[4] != "")
		{
			GridState gridState = new GridState();
			string[] array = loadState[0].StringList[4].Split(new char[]
			{
				' '
			});
			gridState.Lines = (array[0] == "1");
			gridState.Snapping = (array[1] == "1" && array[2] != "1");
			gridState.Offset = (array[2] == "1" && array[1] == "1");
			gridState.BothSnapping = false;
			float.TryParse(array[3], out gridState.xSize);
			if (array.Length > 4)
			{
				float.TryParse(array[4], out gridState.ySize);
			}
			else
			{
				gridState.ySize = gridState.xSize;
			}
			if (array.Length > 5)
			{
				gridState.Type = ((array[5] == "1") ? GridType.HexHorizontal : GridType.Box);
			}
			else
			{
				gridState.Type = GridType.Box;
			}
			NetworkSingleton<GridOptions>.Instance.gridState = gridState;
		}
		if (loadState[0].idint > 0 && NetworkSingleton<NetworkUI>.Instance.SkyToID(this.Sky.name) != loadState[0].idint)
		{
			this.DestroySky();
			this.DestroyCustomSky();
			GameObject gameObject = NetworkSingleton<NetworkUI>.Instance.SkyFromID(loadState[0].idint);
			Network.Instantiate(gameObject, Vector3.zero, gameObject.transform.rotation, default(NetworkPlayer));
		}
		List<string> list = new List<string>();
		List<string> urlbackList = NetworkSingleton<CardManagerScript>.Instance.URLBackList;
		List<string> urlfaceList = NetworkSingleton<CardManagerScript>.Instance.URLFaceList;
		List<string> customList = loadState[0].CustomList;
		bool flag2 = customList.Count != urlbackList.Count + urlfaceList.Count;
		if (!flag2)
		{
			for (int i = 0; i < urlbackList.Count; i++)
			{
				list.Add(urlbackList[i]);
			}
			for (int j = 0; j < urlfaceList.Count; j++)
			{
				list.Add(urlfaceList[j]);
			}
			for (int k = 0; k < list.Count; k++)
			{
				if (list[k] != customList[k])
				{
					flag2 = true;
				}
			}
		}
		if (flag2)
		{
			NetworkSingleton<CardManagerScript>.Instance.ResetCardManager();
			if (customList.Count > 0)
			{
				for (int l = 0; l < customList.Count / 2; l++)
				{
					NetworkSingleton<CardManagerScript>.Instance.URLBackList.Add(customList[l]);
				}
				for (int m = customList.Count / 2; m < customList.Count; m++)
				{
					NetworkSingleton<CardManagerScript>.Instance.URLFaceList.Add(customList[m]);
				}
			}
		}
		for (int n = 0; n < loadState.Count; n++)
		{
			PhysicsState physicsState = loadState[n];
			if (loadState.IndexOf(physicsState) != 0)
			{
				this.LoadPhysicsState(physicsState);
			}
		}
		if (flag2)
		{
			NetworkSingleton<CardManagerScript>.Instance.BeginSetupCustomCJC();
		}
		base.networkView.RPC(RPCTarget.All, new Action(this.RPCCheckLoadingSaveComplete));
	}

	// Token: 0x060015A6 RID: 5542 RVA: 0x00095F35 File Offset: 0x00094135
	public SaveState LastLoadedSaveState()
	{
		if (string.IsNullOrEmpty(SerializationScript.LastLoadedJsonFilePath))
		{
			return this.CurrentState();
		}
		return SerializationScript.LoadJson(SerializationScript.LastLoadedJsonFilePath, false);
	}

	// Token: 0x060015A7 RID: 5543 RVA: 0x00095F58 File Offset: 0x00094158
	public void Recompile(bool save)
	{
		SaveState saveState = this.LastLoadedSaveState();
		SaveState saveState2 = this.CurrentState();
		saveState.LuaScript = saveState2.LuaScript;
		saveState.ObjectStates = this.AssignScriptToObjectStates(saveState2.ObjectStates, saveState.ObjectStates);
		saveState.XmlUI = saveState2.XmlUI;
		saveState.CustomUIAssets = saveState2.CustomUIAssets;
		if (save)
		{
			if (!string.IsNullOrEmpty(SerializationScript.LastLoadedJsonFilePath))
			{
				if (!SerializationScript.LastLoadedJsonFilePath.Contains(DirectoryScript.workshopFilePath))
				{
					Chat.LogSystem("Saving script changes to " + saveState.SaveName + ".", Colour.Green, false);
					SerializationScript.Save(saveState, SerializationScript.LastLoadedJsonFilePath, saveState.SaveName, false);
				}
				else
				{
					Chat.LogError("Error cannot commit save changes to a workshop mod create your own save.", true);
				}
			}
			else
			{
				Chat.LogError("Error no save found. You must load a save first.", true);
			}
		}
		string lastLoadedJsonFilePath = SerializationScript.LastLoadedJsonFilePath;
		this.LoadSaveState(saveState, false, true);
		SerializationScript.LastLoadedJsonFilePath = lastLoadedJsonFilePath;
		Chat.LogSystem("Recompiled. Any changes made that weren't script related were lost.", Colour.Green, false);
	}

	// Token: 0x060015A8 RID: 5544 RVA: 0x00096048 File Offset: 0x00094248
	private List<ObjectState> AssignScriptToObjectStates(List<ObjectState> fromStates, List<ObjectState> toStates)
	{
		for (int i = 0; i < toStates.Count; i++)
		{
			ObjectState objectState = toStates[i];
			int j = 0;
			while (j < fromStates.Count)
			{
				ObjectState objectState2 = fromStates[j];
				if (objectState.GUID == objectState2.GUID)
				{
					objectState.LuaScript = objectState2.LuaScript;
					objectState.XmlUI = objectState2.XmlUI;
					objectState.CustomUIAssets = objectState2.CustomUIAssets;
					if (objectState.ContainedObjects != null && objectState2.ContainedObjects != null)
					{
						objectState.ContainedObjects = this.AssignScriptToObjectStates(objectState2.ContainedObjects, objectState.ContainedObjects);
						break;
					}
					break;
				}
				else
				{
					j++;
				}
			}
		}
		return toStates;
	}

	// Token: 0x060015A9 RID: 5545 RVA: 0x000960F0 File Offset: 0x000942F0
	public void ResetTable(bool resetCardManager = true, bool resetCustomManager = true)
	{
		if (Network.isServer)
		{
			for (int i = this.GrabbableNPOs.Count - 1; i >= 0; i--)
			{
				NetworkPhysicsObject networkPhysicsObject = this.GrabbableNPOs[i];
				if (networkPhysicsObject.DoesNotPersist || !resetCustomManager)
				{
					this.DestroyThisObject(networkPhysicsObject.gameObject);
					if (networkPhysicsObject.luaGameObjectScript)
					{
						networkPhysicsObject.luaGameObjectScript.BlockEvents = true;
					}
				}
			}
			if (resetCardManager)
			{
				NetworkSingleton<CardManagerScript>.Instance.ResetCardManager();
			}
			HandZone.ResetHandZones();
			NetworkSingleton<ManagerPhysicsObject>.Instance.TableScript.Reset();
			NetworkSingleton<GridOptions>.Instance.Reset();
			NetworkSingleton<LightingScript>.Instance.Reset();
			NetworkSingleton<DecalManager>.Instance.Reset();
			NetworkSingleton<Turns>.Instance.Reset();
			NetworkSingleton<Hands>.Instance.Reset();
			NetworkSingleton<GameOptions>.Instance.Reset();
			this.GarbageCollection();
			UINotebook component = NetworkSingleton<NetworkUI>.Instance.GUIRules.GetComponent<UINotebook>();
			component.ClearTabs();
			component.Init();
			LuaGlobalScriptManager.Instance.Reset();
			SerializationScript.LastLoadedJsonFilePath = "";
			this.SetCameraStates(new CameraState[10]);
			Physics.gravity = NetworkUI.DefaultGravity;
			EventManager.TriggerResetTable();
		}
	}

	// Token: 0x060015AA RID: 5546 RVA: 0x0009620C File Offset: 0x0009440C
	public void DisableAllObjectLuaEvents()
	{
		if (!Network.isServer)
		{
			return;
		}
		for (int i = this.GrabbableNPOs.Count - 1; i >= 0; i--)
		{
			NetworkPhysicsObject networkPhysicsObject = this.GrabbableNPOs[i];
			if (networkPhysicsObject.luaGameObjectScript)
			{
				networkPhysicsObject.luaGameObjectScript.BlockEvents = true;
			}
		}
	}

	// Token: 0x060015AB RID: 5547 RVA: 0x00096260 File Offset: 0x00094460
	[Remote(Permission.Client, SendType.ReliableBuffered, null, SerializationMethod.Default, sendType = SendType.ReliableNoDelay)]
	public void CheckPutInContainerDestroy(NetworkPhysicsObject container, NetworkPhysicsObject obj)
	{
		if (Network.isClient)
		{
			base.networkView.RPC<NetworkPhysicsObject, NetworkPhysicsObject>(RPCTarget.Server, new Action<NetworkPhysicsObject, NetworkPhysicsObject>(this.CheckPutInContainerDestroy), container, obj);
			return;
		}
		if ((container.deckScript && (obj.deckScript || obj.cardScript)) || (container.stackObject && container.stackObject.bBag))
		{
			this.DestroyThisObject(obj.gameObject);
		}
	}

	// Token: 0x060015AC RID: 5548 RVA: 0x000962DC File Offset: 0x000944DC
	public void DestroyThisObject(GameObject go)
	{
		NetworkView networkView = null;
		if (go)
		{
			networkView = go.GetComponent<NetworkView>();
		}
		if (networkView)
		{
			this.DestroyThisObject(networkView);
			return;
		}
		UnityEngine.Object.Destroy(go);
	}

	// Token: 0x060015AD RID: 5549 RVA: 0x00096310 File Offset: 0x00094510
	public void DestroyThisObject(NetworkBehavior networkBehavior)
	{
		if (networkBehavior)
		{
			this.DestroyThisObject(networkBehavior.networkView);
		}
	}

	// Token: 0x060015AE RID: 5550 RVA: 0x00096326 File Offset: 0x00094526
	public void DestroyThisObject(NetworkView view)
	{
		if (view)
		{
			base.networkView.RPC<NetworkView>(RPCTarget.Server, new Action<NetworkView>(this.RPCDestroyThisObject), view);
		}
	}

	// Token: 0x060015AF RID: 5551 RVA: 0x00096349 File Offset: 0x00094549
	[Remote(Permission.Admin)]
	private void RPCDestroyThisObject(NetworkView view)
	{
		if (!view)
		{
			return;
		}
		Network.Destroy(view);
	}

	// Token: 0x060015B0 RID: 5552 RVA: 0x0009635C File Offset: 0x0009455C
	public void SetCameraStates(CameraState[] cameraStates)
	{
		if (cameraStates.Length != 10)
		{
			CameraState[] array = new CameraState[10];
			int num = (cameraStates.Length < 10) ? cameraStates.Length : 10;
			for (int i = 0; i < num; i++)
			{
				array[i] = cameraStates[i];
			}
			cameraStates = array;
		}
		base.networkView.RPC<CameraState[]>(RPCTarget.All, new Action<CameraState[]>(this.RPCCameraStates), cameraStates);
	}

	// Token: 0x060015B1 RID: 5553 RVA: 0x000963B8 File Offset: 0x000945B8
	[Remote(Permission.Server, serializationMethod = SerializationMethod.Json)]
	private void RPCCameraStates(CameraState[] cameraStates)
	{
		Singleton<CameraController>.Instance.CameraStates = cameraStates;
		bool flag = false;
		string text = "Camera # ";
		for (int i = 1; i < 10; i++)
		{
			if (Singleton<CameraController>.Instance.CameraStateInUse(i))
			{
				if (flag)
				{
					text = text + ", " + i.ToString();
				}
				else
				{
					text += i.ToString();
				}
				flag = true;
			}
		}
		if (flag)
		{
			text += " available.";
			Chat.LogWarning(text, true);
		}
	}

	// Token: 0x060015B2 RID: 5554 RVA: 0x00096430 File Offset: 0x00094630
	public void RequestChestSaveState(int objectID, string name)
	{
		if (Network.isClient)
		{
			this.saveToChestName = name;
			base.networkView.RPC<int>(RPCTarget.Server, new Action<int>(this.RequestChestSaveStateRPC), objectID);
		}
	}

	// Token: 0x060015B3 RID: 5555 RVA: 0x0009645C File Offset: 0x0009465C
	[Remote("Permissions/Saving")]
	private void RequestChestSaveStateRPC(int objectID)
	{
		NetworkPlayer sender = Network.sender;
		Pointer pointer = this.PointerFromID(NetworkID.IDFromNetworkPlayer(sender));
		if (!pointer)
		{
			return;
		}
		SaveState saveState = new SaveState
		{
			ObjectStates = pointer.GetObjectStates(-1, true, true, false, true)
		};
		if (saveState.ObjectStates.Count == 0)
		{
			GameObject gameObject = this.GOFromID(objectID);
			if (!gameObject)
			{
				Chat.LogError("Error: Client requests to save zero objects to chest.", true);
				return;
			}
			NetworkPhysicsObject networkPhysicsObject = this.NPOFromGO(gameObject);
			if (networkPhysicsObject.IsSaved)
			{
				saveState.ObjectStates.Add(this.SaveObjectState(networkPhysicsObject));
			}
		}
		if (saveState.ObjectStates.Count == 0)
		{
			return;
		}
		try
		{
			base.networkView.RPC<SaveState, int>(sender, new Action<SaveState, int>(this.SaveObjectToClient), saveState, objectID);
		}
		catch (Exception)
		{
			Chat.LogError("Saved object is too large to transfer.", true);
		}
	}

	// Token: 0x060015B4 RID: 5556 RVA: 0x00096534 File Offset: 0x00094734
	[Remote(Permission.Server, serializationMethod = SerializationMethod.Json)]
	private void SaveObjectToClient(SaveState ss, int thumbnailID)
	{
		if (this.saveToChestName == "")
		{
			return;
		}
		SerializationScript.Save(ss, this.saveToChestName, this.GOFromID(thumbnailID));
		this.saveToChestName = "";
	}

	// Token: 0x060015B5 RID: 5557 RVA: 0x00096567 File Offset: 0x00094767
	public void RequestSaveState(string filePath, string saveName)
	{
		if (Network.isClient)
		{
			this.requestedSaveName = saveName;
			this.requestedFilePath = filePath;
			base.networkView.RPC(RPCTarget.Server, new Action(this.RequestSaveStateRPC));
		}
	}

	// Token: 0x060015B6 RID: 5558 RVA: 0x00096598 File Offset: 0x00094798
	[Remote("Permissions/Saving")]
	private void RequestSaveStateRPC()
	{
		try
		{
			base.networkView.RPC<SaveState>(Network.sender, new Action<SaveState>(this.ClientSaveState), this.CurrentState());
		}
		catch (Exception)
		{
			Chat.LogError("Save is too large to transfer.", true);
		}
	}

	// Token: 0x060015B7 RID: 5559 RVA: 0x000965E8 File Offset: 0x000947E8
	[Remote(Permission.Server, serializationMethod = SerializationMethod.Json)]
	private void ClientSaveState(SaveState ss)
	{
		if (this.requestedSaveName == "" || this.requestedFilePath == "")
		{
			return;
		}
		SerializationScript.Save(ss, this.requestedFilePath, this.requestedSaveName, true);
		this.requestedSaveName = "";
		this.requestedFilePath = "";
	}

	// Token: 0x060015B8 RID: 5560 RVA: 0x00096643 File Offset: 0x00094843
	public void SpawnObject(ObjectState os)
	{
		this.SpawnObjects(new List<ObjectState>
		{
			os
		});
	}

	// Token: 0x060015B9 RID: 5561 RVA: 0x00096658 File Offset: 0x00094858
	[Remote(Permission.Admin, serializationMethod = SerializationMethod.Json)]
	public void SpawnObjects(List<ObjectState> objectStates)
	{
		if (Network.isClient)
		{
			try
			{
				base.networkView.RPC<List<ObjectState>>(RPCTarget.Server, new Action<List<ObjectState>>(this.SpawnObjects), objectStates);
			}
			catch (Exception)
			{
				Chat.LogError("Save is too large to transfer.", true);
			}
			return;
		}
		for (int i = 0; i < objectStates.Count; i++)
		{
			this.LoadObjectState(objectStates[i], false, false);
		}
	}

	// Token: 0x060015BA RID: 5562 RVA: 0x000966C8 File Offset: 0x000948C8
	public void SpawnObjectOffset(ObjectState os, Vector3 spawnPos, bool offsetY = true)
	{
		this.SpawnObjectsOffset(new List<ObjectState>
		{
			os
		}, spawnPos, offsetY);
	}

	// Token: 0x060015BB RID: 5563 RVA: 0x000966E0 File Offset: 0x000948E0
	[Remote(Permission.Admin, serializationMethod = SerializationMethod.Json)]
	public void SpawnObjectsOffset(List<ObjectState> objectStates, Vector3 spawnPos, bool offsetY = true)
	{
		if (Network.isClient)
		{
			try
			{
				base.networkView.RPC<List<ObjectState>, Vector3, bool>(RPCTarget.Server, new Action<List<ObjectState>, Vector3, bool>(this.SpawnObjectsOffset), objectStates, spawnPos, offsetY);
			}
			catch (Exception)
			{
				Chat.LogError("Save is too large to transfer.", true);
			}
			return;
		}
		this.SpawnObjectStatesOffset(objectStates, spawnPos, true, false, offsetY, true);
	}

	// Token: 0x060015BC RID: 5564 RVA: 0x00096740 File Offset: 0x00094940
	public void SpawnCJCObjectOffset(PhysicsState ps, bool offsetY = true)
	{
		ObjectState objectState = this.ObjectStateFromPhysicsState(ps);
		this.SpawnObjectOffset(objectState, objectState.Transform.Position(), offsetY);
	}

	// Token: 0x060015BD RID: 5565 RVA: 0x00096768 File Offset: 0x00094968
	public List<GameObject> SpawnObjectStatesOffset(List<ObjectState> objectStates, Vector3 spawnPos, bool snapToGrid = true, bool isDummyObject = false, bool offsetY = true, bool playSpawnSound = true)
	{
		GameObject gameObject = null;
		List<GameObject> list = new List<GameObject>();
		this.GenerateJointRelations(objectStates);
		Vector3 vector = Vector3.zero;
		float num = -1f;
		for (int i = 0; i < objectStates.Count; i++)
		{
			ObjectState objectState = objectStates[i];
			if (objectState != null)
			{
				Vector3 vector2 = objectState.Transform.Position();
				if (objectStates.IndexOf(objectState) != 0)
				{
					objectState.Transform.posX += vector.x;
					objectState.Transform.posY += vector.y;
					objectState.Transform.posZ += vector.z;
					Vector3 vector3 = this.SurfacePointBelowWorldPos(new Vector3(objectState.Transform.posX, objectState.Transform.posY + 4f, objectState.Transform.posZ));
					if (vector3.y - 0.96f > spawnPos.y)
					{
						objectState.Transform.posY += vector3.y - spawnPos.y - 0.96f;
						if (num < vector3.y)
						{
							num = vector3.y;
						}
					}
				}
				else
				{
					objectState.Transform.posX = spawnPos.x;
					if (offsetY)
					{
						objectState.Transform.posY += spawnPos.y;
					}
					else
					{
						objectState.Transform.posY = spawnPos.y;
					}
					objectState.Transform.posZ = spawnPos.z;
					Vector3 vector4 = this.SurfacePointBelowWorldPos(new Vector3(objectState.Transform.posX, objectState.Transform.posY + 4f, objectState.Transform.posZ));
					if (vector4.y - 0.96f > spawnPos.y)
					{
						objectState.Transform.posY += vector4.y - spawnPos.y - 0.96f;
						if (num < vector4.y)
						{
							num = vector4.y;
						}
					}
					vector = objectState.Transform.Position();
				}
				objectState.Transform.posX = vector2.x;
				objectState.Transform.posY = vector2.y;
				objectState.Transform.posZ = vector2.z;
			}
		}
		for (int j = 0; j < objectStates.Count; j++)
		{
			ObjectState objectState2 = objectStates[j];
			if (objectState2 != null)
			{
				Vector3 vector5 = objectState2.Transform.Position();
				if (objectStates.IndexOf(objectState2) != 0)
				{
					objectState2.Transform.posX += vector.x;
					objectState2.Transform.posY += vector.y;
					objectState2.Transform.posZ += vector.z;
					if (num - 0.96f > spawnPos.y)
					{
						objectState2.Transform.posY += num - spawnPos.y - 0.96f;
					}
				}
				else
				{
					objectState2.Transform.posX = spawnPos.x;
					if (offsetY)
					{
						objectState2.Transform.posY += spawnPos.y;
					}
					else
					{
						objectState2.Transform.posY = spawnPos.y;
					}
					objectState2.Transform.posZ = spawnPos.z;
					if (num - 0.96f > spawnPos.y)
					{
						objectState2.Transform.posY += num - spawnPos.y - 0.96f;
					}
					vector = objectState2.Transform.Position();
				}
				GameObject gameObject2 = this.LoadObjectState(objectState2, isDummyObject, false);
				list.Add(gameObject2);
				if (gameObject2)
				{
					if (!gameObject && gameObject2.GetComponent<SoundScript>())
					{
						gameObject = gameObject2;
					}
					NetworkPhysicsObject networkPhysicsObject = this.NPOFromGO(gameObject2);
					if (networkPhysicsObject)
					{
						networkPhysicsObject.spawnedByUI = true;
					}
					if (snapToGrid)
					{
						base.StartCoroutine(this.DelaySnapToGrid(gameObject2));
					}
				}
				objectState2.Transform.posX = vector5.x;
				objectState2.Transform.posY = vector5.y;
				objectState2.Transform.posZ = vector5.z;
			}
		}
		this.JointRelationFromGUID.Clear();
		if (playSpawnSound && gameObject != null)
		{
			gameObject.GetComponent<SoundScript>().CopyPasteSound();
		}
		return list;
	}

	// Token: 0x060015BE RID: 5566 RVA: 0x00096BFC File Offset: 0x00094DFC
	[Remote(Permission.Admin, serializationMethod = SerializationMethod.Json)]
	public void LoadPromotedSaveState(SaveState saveState)
	{
		if (Network.isClient)
		{
			try
			{
				base.networkView.RPC<SaveState>(RPCTarget.Server, new Action<SaveState>(this.LoadPromotedSaveState), saveState);
			}
			catch (Exception)
			{
				Chat.LogError("Save is too large to transfer.", true);
			}
			return;
		}
		try
		{
			this.LoadSaveState(saveState, false, true);
		}
		catch (Exception ex)
		{
			Chat.LogError("Error: Loading promoted player's json save. " + ex.Message, true);
		}
	}

	// Token: 0x060015BF RID: 5567 RVA: 0x00096C7C File Offset: 0x00094E7C
	[Remote(Permission.Admin)]
	public void LoadPromotedPhysicsState(byte[] bytes)
	{
		if (Network.isClient)
		{
			try
			{
				base.networkView.RPC<byte[]>(RPCTarget.Server, new Action<byte[]>(this.LoadPromotedPhysicsState), bytes);
			}
			catch (Exception)
			{
				Chat.LogError("Save is too large to transfer.", true);
			}
			return;
		}
		try
		{
			this.LoadStateList(SerializationScript.GetPhysicsState(bytes), false, true);
		}
		catch (Exception ex)
		{
			Chat.LogError("Error: Loading promoted player's cjc save. " + ex.Message, true);
		}
	}

	// Token: 0x060015C0 RID: 5568 RVA: 0x00096D00 File Offset: 0x00094F00
	public void GenerateJointRelations(List<ObjectState> objectStates)
	{
		this.JointRelationFromGUID.Clear();
		for (int i = 0; i < objectStates.Count; i++)
		{
			ObjectState objectState = objectStates[i];
			if (objectState.JointFixed != null || objectState.JointHinge != null || objectState.JointSpring != null)
			{
				if (objectState.JointFixed != null)
				{
					if (!string.IsNullOrEmpty(objectState.JointFixed.ConnectedBodyGUID) && !this.JointRelationFromGUID.ContainsKey(objectState.JointFixed.ConnectedBodyGUID))
					{
						this.JointRelationFromGUID.Add(objectState.JointFixed.ConnectedBodyGUID, new ManagerPhysicsObject.JointRelation());
					}
				}
				else if (objectState.JointHinge != null)
				{
					if (!string.IsNullOrEmpty(objectState.JointHinge.ConnectedBodyGUID) && !this.JointRelationFromGUID.ContainsKey(objectState.JointHinge.ConnectedBodyGUID))
					{
						this.JointRelationFromGUID.Add(objectState.JointHinge.ConnectedBodyGUID, new ManagerPhysicsObject.JointRelation());
					}
				}
				else if (objectState.JointSpring != null && !string.IsNullOrEmpty(objectState.JointSpring.ConnectedBodyGUID) && !this.JointRelationFromGUID.ContainsKey(objectState.JointSpring.ConnectedBodyGUID))
				{
					this.JointRelationFromGUID.Add(objectState.JointSpring.ConnectedBodyGUID, new ManagerPhysicsObject.JointRelation());
				}
			}
		}
	}

	// Token: 0x060015C1 RID: 5569 RVA: 0x00096E70 File Offset: 0x00095070
	public void AddJoint(GameObject copyObject, JointState jointState, Rigidbody connectedBody)
	{
		Joint joint = null;
		JointFixedState jointFixedState;
		if ((jointFixedState = (jointState as JointFixedState)) != null && !string.IsNullOrEmpty(jointFixedState.ConnectedBodyGUID))
		{
			Joint joint2 = joint = copyObject.AddComponent<FixedJoint>();
			joint.connectedBody = connectedBody;
			joint2.autoConfigureConnectedAnchor = false;
		}
		JointHingeState jointHingeState;
		if ((jointHingeState = (jointState as JointHingeState)) != null && !string.IsNullOrEmpty(jointHingeState.ConnectedBodyGUID))
		{
			Joint joint3 = joint = copyObject.AddComponent<HingeJoint>();
			joint.connectedBody = connectedBody;
			joint3.autoConfigureConnectedAnchor = false;
			joint3.useLimits = jointHingeState.UseLimits;
			joint3.limits = jointHingeState.Limits;
			joint3.useMotor = jointHingeState.UseMotor;
			joint3.motor = jointHingeState.Motor;
			joint3.useSpring = jointHingeState.UseSpring;
			joint3.spring = jointHingeState.Spring;
		}
		JointSpringState jointSpringState;
		if ((jointSpringState = (jointState as JointSpringState)) != null && !string.IsNullOrEmpty(jointSpringState.ConnectedBodyGUID))
		{
			Joint joint4 = joint = copyObject.AddComponent<SpringJoint>();
			joint.connectedBody = connectedBody;
			joint4.autoConfigureConnectedAnchor = false;
			joint4.damper = jointSpringState.Damper;
			joint4.maxDistance = jointSpringState.MaxDistance;
			joint4.minDistance = jointSpringState.MinDistance;
			joint4.spring = jointSpringState.Spring;
		}
		if (joint)
		{
			this.NPOFromGO(copyObject).AddJoint(joint);
			joint.enableCollision = jointState.EnableCollision;
			joint.axis = jointState.Axis.ToVector();
			joint.anchor = jointState.Anchor.ToVector();
			joint.connectedAnchor = jointState.ConnectedAnchor.ToVector();
			joint.breakForce = jointState.BreakForce;
			joint.breakTorque = jointState.BreakTorgue;
		}
	}

	// Token: 0x060015C2 RID: 5570 RVA: 0x00096FE8 File Offset: 0x000951E8
	public void SetObjectRotation(NetworkPhysicsObject npo, int spinDelta, int flipDelta, int id = -1)
	{
		if (npo.IsLocked)
		{
			return;
		}
		if (npo.InsideALayoutZone)
		{
			LayoutZone.UpdateIfInGroup(npo);
		}
		if (npo.RotatesThroughRotationValues)
		{
			int num = npo.GetRotationNumber(false) + 1;
			if (num > npo.RotationValues.Count)
			{
				num = 1;
			}
			npo.SetRotationNumber(num, id);
			return;
		}
		GameObject gameObject = npo.gameObject;
		if (gameObject.CompareTag("Dice"))
		{
			if (flipDelta == 12 && spinDelta == 0)
			{
				DiceScript.FlipDice(gameObject, id);
				return;
			}
			if (spinDelta < 12 && flipDelta == 0)
			{
				DiceScript.IncrementDice(gameObject, true, id);
				return;
			}
			if (spinDelta > 12 && flipDelta == 0)
			{
				DiceScript.IncrementDice(gameObject, false, id);
			}
			return;
		}
		else
		{
			Pointer pointer = this.PointerFromID(id);
			int num2 = pointer ? pointer.RotationSnap : 15;
			int num3 = this.SpinRotationIndexFromGrabbable(gameObject, (float)num2);
			int num4 = this.FlipRotationIndexFromGrabbable(gameObject, (float)num2);
			int num5 = (num3 + spinDelta) % 24;
			int num6 = (num4 + flipDelta) % 24;
			LuaGameObjectScript luaGameObjectScript = npo.luaGameObjectScript;
			PlayerState playerState = NetworkSingleton<PlayerManager>.Instance.PlayerStateFromID(id);
			string playerColor = (playerState != null) ? playerState.stringColor : null;
			if (luaGameObjectScript != null && !luaGameObjectScript.CheckObjectRotate(num5, num6, playerColor, num3, num4))
			{
				return;
			}
			Vector3 vector = this.SurfacePointBelowObject(gameObject);
			npo.ResetBounds();
			Vector3 vector2;
			npo.GetBoundsNotNormalized(out vector2);
			Vector3 boundsCenterOffset = npo.GetBoundsCenterOffset();
			Bounds bounds = npo.GetBounds();
			Vector3 axis;
			Vector3 position;
			if (this.FlipsAroundZAxis(gameObject))
			{
				axis = Vector3.forward;
				position = new Vector3(npo.rigidbody.position.x, vector.y + bounds.extents.x + bounds.extents.y - boundsCenterOffset.y * gameObject.transform.up.normalized.y + 0.1f, npo.rigidbody.position.z);
			}
			else
			{
				axis = Vector3.right;
				position = new Vector3(npo.rigidbody.position.x, vector.y + bounds.extents.z + bounds.extents.y - boundsCenterOffset.y * gameObject.transform.up.normalized.y + 0.1f, npo.rigidbody.position.z);
			}
			Vector3 position2 = npo.rigidbody.position;
			Vector3 vector3 = new Vector3(npo.rigidbody.position.x, npo.rigidbody.position.y - vector2.y * 2f, npo.rigidbody.position.z);
			if (flipDelta == 0)
			{
				position.y = vector.y + bounds.extents.y + 0.5f + boundsCenterOffset.y * gameObject.transform.up.normalized.y;
			}
			Color? highlightColor = null;
			if (pointer)
			{
				highlightColor = new Color?(pointer.PointerDarkColour);
			}
			npo.SetCollision(false);
			if (!npo.CurrentPlayerHand)
			{
				npo.SetSmoothPosition(position, false, true, false, false, highlightColor, false, false, null);
			}
			Quaternion quaternion = Quaternion.identity;
			quaternion = Quaternion.AngleAxis((float)(num6 * 15), axis) * quaternion;
			quaternion = Quaternion.AngleAxis((float)(num5 * 15), Vector3.up) * quaternion;
			if (gameObject.CompareTag("Card") && gameObject.GetComponent<FixedJoint>())
			{
				npo.cardScript.ResetCard();
			}
			if (npo.cardScript && npo.cardScript.CardAttachToThis)
			{
				npo.cardScript.CardAttachToThis.GetComponent<CardScript>().ResetCard();
			}
			npo.SetSmoothRotation(quaternion, false, false, false, true, null, false);
			if (!this.enableCollisionNPOs.Contains(npo))
			{
				base.StartCoroutine(this.EnableCollisionAfterMove(npo, position2.y, vector3.y));
			}
			npo.SetLayerToHeld(true);
			List<NetworkPhysicsObject> overlapObjects = npo.GetOverlapObjects();
			for (int i = 0; i < overlapObjects.Count; i++)
			{
				NetworkPhysicsObject networkPhysicsObject = overlapObjects[i];
				if (networkPhysicsObject && networkPhysicsObject.IsHeldByNobodyAndIsNotMarkedDestroyedInHeldID)
				{
					networkPhysicsObject.NPOBeingRotated = npo;
				}
			}
			EventManager.TriggerObjectRotate(npo, num5, num6, playerColor, num3, num4);
			return;
		}
	}

	// Token: 0x060015C3 RID: 5571 RVA: 0x00097434 File Offset: 0x00095634
	public void SetHeldObjectSpinRotationIndex(NetworkPhysicsObject npo, int spinDelta, int id, NetworkPhysicsObject firstGrabbedNPO)
	{
		int heldSpinRotationIndex = npo.HeldSpinRotationIndex;
		int num = (heldSpinRotationIndex + spinDelta) % 24;
		int heldFlipRotationIndex = npo.HeldFlipRotationIndex;
		LuaGameObjectScript luaGameObjectScript = npo.luaGameObjectScript;
		PlayerState playerState = NetworkSingleton<PlayerManager>.Instance.PlayerStateFromID(id);
		string playerColor = (playerState != null) ? playerState.stringColor : null;
		if (luaGameObjectScript != null && !luaGameObjectScript.CheckObjectRotate(num, heldFlipRotationIndex, playerColor, heldSpinRotationIndex, heldFlipRotationIndex))
		{
			return;
		}
		npo.HeldSpinRotationIndex = num;
		npo.HeldOffset = Utilities.RotatePointAroundPivot(npo.HeldOffset, firstGrabbedNPO.HeldOffset, new Vector3(0f, (float)((num - heldSpinRotationIndex) * 15), 0f));
		npo.DisableFastDragWhileAnimating();
		EventManager.TriggerObjectRotate(npo, num, heldFlipRotationIndex, playerColor, heldSpinRotationIndex, heldFlipRotationIndex);
	}

	// Token: 0x060015C4 RID: 5572 RVA: 0x000974DC File Offset: 0x000956DC
	public void SetHeldObjectFlipRotationIndex(NetworkPhysicsObject npo, int flipDelta, int id)
	{
		int heldFlipRotationIndex = npo.HeldFlipRotationIndex;
		int num = (heldFlipRotationIndex + flipDelta) % 24;
		int heldSpinRotationIndex = npo.HeldSpinRotationIndex;
		LuaGameObjectScript luaGameObjectScript = npo.luaGameObjectScript;
		PlayerState playerState = NetworkSingleton<PlayerManager>.Instance.PlayerStateFromID(id);
		string playerColor = (playerState != null) ? playerState.stringColor : null;
		if (luaGameObjectScript != null && !luaGameObjectScript.CheckObjectRotate(heldSpinRotationIndex, num, playerColor, heldSpinRotationIndex, heldFlipRotationIndex))
		{
			return;
		}
		npo.HeldFlipRotationIndex = num;
		npo.DisableFastDragWhileAnimating();
		EventManager.TriggerObjectRotate(npo, heldSpinRotationIndex, num, playerColor, heldSpinRotationIndex, heldFlipRotationIndex);
	}

	// Token: 0x060015C5 RID: 5573 RVA: 0x00097553 File Offset: 0x00095753
	private IEnumerator EnableCollisionAfterMove(NetworkPhysicsObject npo, float yPos, float yPosFlipped)
	{
		this.enableCollisionNPOs.Add(npo);
		float breakAfter = Time.time + 30f;
		bool objectUp = npo.transform.up.normalized.y >= 0f;
		while (npo && ((npo.IsHeldByNobody && npo.transform.position.y > ((objectUp == npo.transform.up.normalized.y >= 0f) ? yPos : yPosFlipped)) || npo.IsSmoothMoving) && Time.time <= breakAfter)
		{
			yield return this.waitForFixedUpdate;
		}
		if (npo)
		{
			if (npo.transform.position.y < ((objectUp == npo.transform.up.normalized.y >= 0f) ? yPos : yPosFlipped) && npo.IsHeldByNobody)
			{
				npo.transform.position = new Vector3(npo.transform.position.x, (objectUp == npo.transform.up.normalized.y >= 0f) ? yPos : yPosFlipped, npo.transform.position.z);
			}
			npo.SetCollision(true);
		}
		this.enableCollisionNPOs.Remove(npo);
		yield break;
	}

	// Token: 0x060015C6 RID: 5574 RVA: 0x00097577 File Offset: 0x00095777
	public void Align(int alignToID, List<GameObject> objectsToAlign)
	{
		this.NPOFromID(alignToID);
	}

	// Token: 0x060015C7 RID: 5575 RVA: 0x00097584 File Offset: 0x00095784
	public List<NetworkPhysicsObject> Group(List<NetworkPhysicsObject> selectedNPOs)
	{
		List<GameObject> list = new List<GameObject>();
		for (int i = 0; i < selectedNPOs.Count; i++)
		{
			list.Add(selectedNPOs[i].gameObject);
		}
		return this.Group(list);
	}

	// Token: 0x060015C8 RID: 5576 RVA: 0x000975C4 File Offset: 0x000957C4
	public List<NetworkPhysicsObject> Group(List<GameObject> selectedGOs)
	{
		List<NetworkPhysicsObject> list = new List<NetworkPhysicsObject>();
		List<NetworkPhysicsObject> list2 = new List<NetworkPhysicsObject>();
		List<NetworkPhysicsObject> list3 = new List<NetworkPhysicsObject>();
		List<NetworkPhysicsObject> list4 = new List<NetworkPhysicsObject>();
		NetworkPhysicsObject networkPhysicsObject = null;
		int num = 0;
		for (int i = 0; i < selectedGOs.Count; i++)
		{
			NetworkPhysicsObject networkPhysicsObject2 = this.NPOFromGO(selectedGOs[i]);
			if (networkPhysicsObject2)
			{
				DeckScript component = networkPhysicsObject2.GetComponent<DeckScript>();
				StackObject component2 = networkPhysicsObject2.GetComponent<StackObject>();
				if ((component && !component.AnyoneSearchingDeck()) || (component2 && !component2.AnyoneSearchingBag()))
				{
					list.Add(networkPhysicsObject2);
				}
				else
				{
					list2.Add(networkPhysicsObject2);
				}
				if (component2 && component2.bBag && !component2.IsInfiniteStack)
				{
					networkPhysicsObject = networkPhysicsObject2;
					num++;
				}
			}
		}
		if (ManagerPhysicsObject.PrioritizeBagWhenGrouping && num == 1 && !networkPhysicsObject.IsDestroyed && networkPhysicsObject.ID != -1)
		{
			if (ManagerPhysicsObject.putIntoBag(networkPhysicsObject, selectedGOs))
			{
				list3.Add(networkPhysicsObject);
			}
			return list3;
		}
		bool flag = false;
		for (int j = 0; j < list2.Count; j++)
		{
			NetworkPhysicsObject networkPhysicsObject3 = list2[j];
			if (!networkPhysicsObject3.IsDestroyed && networkPhysicsObject3.ID != -1)
			{
				CardScript component3 = networkPhysicsObject3.GetComponent<CardScript>();
				if (component3)
				{
					bool flag2 = false;
					for (int k = 0; k < list.Count; k++)
					{
						NetworkPhysicsObject networkPhysicsObject4 = list[k];
						DeckScript component4 = networkPhysicsObject4.GetComponent<DeckScript>();
						if (component4 && !networkPhysicsObject4.IsDestroyed && networkPhysicsObject4.ID != -1)
						{
							LuaGameObjectScript luaGameObjectScript = networkPhysicsObject4.luaGameObjectScript;
							if (!(luaGameObjectScript != null) || luaGameObjectScript.CheckObjectEnter(networkPhysicsObject3))
							{
								if (list4.Contains(networkPhysicsObject4))
								{
									ComponentTags.AndFlags(ref networkPhysicsObject4.tags, networkPhysicsObject3.tags);
								}
								component4.AddCard(networkPhysicsObject4.transform.up.y <= 0f, component3.card_id_, this.SaveObjectState(networkPhysicsObject3));
								EventManager.TriggerObjectEnterContainer(networkPhysicsObject4, networkPhysicsObject3);
								networkPhysicsObject3.SetSmoothDestroy(networkPhysicsObject4.transform.position, networkPhysicsObject4.transform.rotation);
								SoundScript component5 = networkPhysicsObject3.GetComponent<SoundScript>();
								if (component5)
								{
									component5.PickUpSound();
								}
								flag2 = true;
								flag = true;
								break;
							}
						}
					}
					if (!flag2)
					{
						GameObject gameObject = null;
						NetworkPhysicsObject networkPhysicsObject5 = null;
						DeckScript deckScript = null;
						for (int l = 0; l < list2.Count; l++)
						{
							NetworkPhysicsObject networkPhysicsObject6 = list2[l];
							CardScript component6 = networkPhysicsObject6.GetComponent<CardScript>();
							if (networkPhysicsObject3 != networkPhysicsObject6 && component6 && !networkPhysicsObject6.IsDestroyed && networkPhysicsObject6.ID != -1)
							{
								bool top = networkPhysicsObject3.transform.up.y <= 0f;
								if (!deckScript)
								{
									LuaGameObjectScript luaGameObjectScript2 = networkPhysicsObject6.luaGameObjectScript;
									if (luaGameObjectScript2 != null && !luaGameObjectScript2.CheckObjectEnter(networkPhysicsObject3))
									{
										goto IL_49D;
									}
									gameObject = Network.Instantiate(NetworkSingleton<GameMode>.Instance.Deck, networkPhysicsObject3.transform.position, networkPhysicsObject3.transform.rotation, default(NetworkPlayer));
									networkPhysicsObject5 = this.NPOFromGO(gameObject);
									deckScript = gameObject.GetComponent<DeckScript>();
									deckScript.bRandomSpawn = false;
									deckScript.num_cards_ = 0;
									deckScript.AddCard(top, component3.card_id(), this.SaveObjectState(networkPhysicsObject3));
									deckScript.AddCard(top, component6.card_id(), this.SaveObjectState(networkPhysicsObject6));
									deckScript.bSideways = component3.bSideways;
									networkPhysicsObject5.SetScale(networkPhysicsObject3.Scale, false);
									networkPhysicsObject5.DiffuseColor = networkPhysicsObject3.DiffuseColor;
									networkPhysicsObject5.IgnoresGrid = networkPhysicsObject3.IgnoresGrid;
									if (networkPhysicsObject3.IsHeldBySomebody)
									{
										networkPhysicsObject5.HeldByPlayerID = networkPhysicsObject3.HeldByPlayerID;
										networkPhysicsObject5.HeldSpinRotationIndex = networkPhysicsObject3.HeldSpinRotationIndex;
										networkPhysicsObject5.HeldFlipRotationIndex = networkPhysicsObject3.HeldFlipRotationIndex;
										networkPhysicsObject5.HeldOffset = networkPhysicsObject3.HeldOffset;
									}
									list.Add(networkPhysicsObject5);
									networkPhysicsObject5.tags = networkPhysicsObject3.tags;
									list4.Add(networkPhysicsObject5);
								}
								else
								{
									LuaGameObjectScript luaGameObjectScript3 = networkPhysicsObject5.luaGameObjectScript;
									if (luaGameObjectScript3 != null && !luaGameObjectScript3.CheckObjectEnter(networkPhysicsObject3))
									{
										goto IL_49D;
									}
									deckScript.AddCard(top, component6.card_id(), this.SaveObjectState(networkPhysicsObject6));
									ComponentTags.AndFlags(ref networkPhysicsObject5.tags, networkPhysicsObject3.tags);
								}
								EventManager.TriggerObjectEnterContainer(gameObject.GetComponent<NetworkPhysicsObject>(), networkPhysicsObject6);
								networkPhysicsObject6.SetSmoothDestroy(networkPhysicsObject3.transform.position, networkPhysicsObject3.transform.rotation);
								SoundScript component7 = networkPhysicsObject3.GetComponent<SoundScript>();
								if (component7)
								{
									component7.PickUpSound();
								}
								flag = true;
							}
							IL_49D:;
						}
						if (deckScript)
						{
							networkPhysicsObject3.HeldByPlayerID = -10;
							this.DestroyThisObject(networkPhysicsObject3.gameObject);
						}
					}
				}
				else
				{
					CheckStackObject component8 = networkPhysicsObject3.GetComponent<CheckStackObject>();
					if (component8)
					{
						bool flag3 = false;
						for (int m = 0; m < list.Count; m++)
						{
							NetworkPhysicsObject networkPhysicsObject7 = list[m];
							StackObject component9 = networkPhysicsObject7.GetComponent<StackObject>();
							if (component9 && component9.CheckStackable(networkPhysicsObject3.gameObject) && !networkPhysicsObject7.IsDestroyed && networkPhysicsObject7.ID != -1)
							{
								LuaGameObjectScript luaGameObjectScript4 = networkPhysicsObject7.luaGameObjectScript;
								if (!(luaGameObjectScript4 != null) || luaGameObjectScript4.CheckObjectEnter(networkPhysicsObject3))
								{
									if (list4.Contains(networkPhysicsObject7))
									{
										ComponentTags.AndFlags(ref networkPhysicsObject7.tags, networkPhysicsObject3.tags);
									}
									StackObject stackObject = component9;
									int num_objects_ = stackObject.num_objects_;
									stackObject.num_objects_ = num_objects_ + 1;
									EventManager.TriggerObjectEnterContainer(networkPhysicsObject7, networkPhysicsObject3);
									networkPhysicsObject3.SetSmoothDestroy(networkPhysicsObject7.transform.position, networkPhysicsObject7.transform.rotation);
									SoundScript component10 = networkPhysicsObject3.GetComponent<SoundScript>();
									if (component10)
									{
										component10.PickUpSound();
									}
									flag3 = true;
									flag = true;
									break;
								}
							}
						}
						if (!flag3)
						{
							NetworkPhysicsObject networkPhysicsObject8 = null;
							StackObject stackObject2 = null;
							LuaGameObjectScript luaGameObjectScript5 = networkPhysicsObject3.luaGameObjectScript;
							for (int n = 0; n < list2.Count; n++)
							{
								NetworkPhysicsObject networkPhysicsObject9 = list2[n];
								if (networkPhysicsObject3 != networkPhysicsObject9 && networkPhysicsObject9.GetComponent<CheckStackObject>() && component8.CheckStackable(networkPhysicsObject9) && !networkPhysicsObject9.IsDestroyed && networkPhysicsObject9.ID != -1)
								{
									LuaGameObjectScript luaGameObjectScript6 = networkPhysicsObject9.luaGameObjectScript;
									if ((!(luaGameObjectScript5 != null) || luaGameObjectScript5.CheckObjectEnter(networkPhysicsObject9)) && (!(luaGameObjectScript6 != null) || luaGameObjectScript6.CheckObjectEnter(networkPhysicsObject3)))
									{
										if (!networkPhysicsObject8)
										{
											networkPhysicsObject8 = this.NPOHitNPO(networkPhysicsObject3, networkPhysicsObject9, false);
											list.Add(networkPhysicsObject8);
											stackObject2 = networkPhysicsObject8.GetComponent<StackObject>();
											list4.Add(networkPhysicsObject8);
											networkPhysicsObject8.tags = networkPhysicsObject3.tags;
										}
										else
										{
											StackObject stackObject3 = stackObject2;
											int num_objects_ = stackObject3.num_objects_;
											stackObject3.num_objects_ = num_objects_ + 1;
											ComponentTags.AndFlags(ref networkPhysicsObject8.tags, networkPhysicsObject3.tags);
										}
										networkPhysicsObject9.SetSmoothDestroy(networkPhysicsObject3.transform.position, networkPhysicsObject3.transform.rotation);
										SoundScript component11 = networkPhysicsObject3.GetComponent<SoundScript>();
										if (component11)
										{
											component11.PickUpSound();
										}
										flag = true;
									}
								}
							}
							if (stackObject2)
							{
								networkPhysicsObject3.HeldByPlayerID = -10;
								this.DestroyThisObject(networkPhysicsObject3.gameObject);
							}
						}
					}
				}
			}
		}
		for (int num2 = 0; num2 < list.Count; num2++)
		{
			NetworkPhysicsObject networkPhysicsObject10 = list[num2];
			if (!networkPhysicsObject10.IsDestroyed && networkPhysicsObject10.ID != -1)
			{
				DeckScript component12 = networkPhysicsObject10.GetComponent<DeckScript>();
				if (component12)
				{
					for (int num3 = 0; num3 < list.Count; num3++)
					{
						NetworkPhysicsObject networkPhysicsObject11 = list[num3];
						if (!(networkPhysicsObject10 == networkPhysicsObject11))
						{
							DeckScript component13 = networkPhysicsObject11.GetComponent<DeckScript>();
							if (component13 && !networkPhysicsObject11.IsDestroyed && networkPhysicsObject11.ID != -1)
							{
								LuaGameObjectScript luaGameObjectScript7 = networkPhysicsObject10.luaGameObjectScript;
								if (!(luaGameObjectScript7 != null) || luaGameObjectScript7.CheckObjectEnter(networkPhysicsObject11))
								{
									List<int> deck = component13.GetDeck();
									List<ObjectState> cardStates = component13.GetCardStates();
									bool flag4 = networkPhysicsObject10.transform.up.y <= 0f;
									if (!flag4)
									{
										for (int num4 = 0; num4 < deck.Count; num4++)
										{
											component12.AddCard(flag4, deck[num4], cardStates[num4]);
										}
									}
									else
									{
										for (int num5 = deck.Count - 1; num5 >= 0; num5--)
										{
											component12.AddCard(flag4, deck[num5], cardStates[num5]);
										}
									}
									EventManager.TriggerObjectEnterContainer(networkPhysicsObject10, networkPhysicsObject11);
									networkPhysicsObject11.SetSmoothDestroy(networkPhysicsObject10.transform.position, networkPhysicsObject10.transform.rotation);
									SoundScript component14 = networkPhysicsObject10.GetComponent<SoundScript>();
									if (component14)
									{
										component14.PickUpSound();
									}
									flag = true;
								}
							}
						}
					}
				}
				else
				{
					StackObject component15 = networkPhysicsObject10.GetComponent<StackObject>();
					if (component15)
					{
						for (int num6 = 0; num6 < list.Count; num6++)
						{
							NetworkPhysicsObject networkPhysicsObject12 = list[num6];
							if (!(networkPhysicsObject12 == networkPhysicsObject10))
							{
								StackObject component16 = networkPhysicsObject12.GetComponent<StackObject>();
								if (component16 && component15.CheckStackable(networkPhysicsObject12) && !networkPhysicsObject12.IsDestroyed && networkPhysicsObject12.ID != -1)
								{
									LuaGameObjectScript luaGameObjectScript8 = networkPhysicsObject10.luaGameObjectScript;
									if (!(luaGameObjectScript8 != null) || luaGameObjectScript8.CheckObjectEnter(networkPhysicsObject12))
									{
										component15.num_objects_ += component16.num_objects_;
										EventManager.TriggerObjectEnterContainer(networkPhysicsObject10, networkPhysicsObject12);
										networkPhysicsObject12.SetSmoothDestroy(networkPhysicsObject10.transform.position, networkPhysicsObject10.transform.rotation);
										SoundScript component17 = networkPhysicsObject10.GetComponent<SoundScript>();
										if (component17)
										{
											component17.PickUpSound();
										}
										flag = true;
									}
								}
							}
						}
					}
				}
			}
		}
		for (int num7 = 0; num7 < list.Count; num7++)
		{
			NetworkPhysicsObject networkPhysicsObject13 = list[num7];
			if (networkPhysicsObject13 && !networkPhysicsObject13.IsDestroyed)
			{
				list3.Add(networkPhysicsObject13);
			}
		}
		if (!flag && num == 1 && !networkPhysicsObject.IsDestroyed && networkPhysicsObject.ID != -1 && ManagerPhysicsObject.putIntoBag(networkPhysicsObject, selectedGOs))
		{
			list3.Clear();
			list3.Add(networkPhysicsObject);
		}
		return list3;
	}

	// Token: 0x060015C9 RID: 5577 RVA: 0x00098068 File Offset: 0x00096268
	private static bool putIntoBag(NetworkPhysicsObject bagNPO, List<GameObject> GOs)
	{
		LuaGameObjectScript luaGameObjectScript = bagNPO.luaGameObjectScript;
		StackObject component = bagNPO.GetComponent<StackObject>();
		bool result = false;
		for (int i = 0; i < GOs.Count; i++)
		{
			NetworkPhysicsObject component2 = GOs[i].GetComponent<NetworkPhysicsObject>();
			if (!(component2 == null) && !(component2 == bagNPO) && (!(luaGameObjectScript != null) || luaGameObjectScript.CheckObjectEnter(component2)))
			{
				component.AddToBag(component2);
				SoundScript component3 = component2.GetComponent<SoundScript>();
				if (component3)
				{
					component3.PickUpSound();
				}
				result = true;
			}
		}
		return result;
	}

	// Token: 0x060015CA RID: 5578 RVA: 0x000980F0 File Offset: 0x000962F0
	public bool CheckGroup(List<GameObject> selectedObjects)
	{
		List<NetworkPhysicsObject> list = new List<NetworkPhysicsObject>();
		List<NetworkPhysicsObject> list2 = new List<NetworkPhysicsObject>();
		for (int i = 0; i < selectedObjects.Count; i++)
		{
			NetworkPhysicsObject networkPhysicsObject = this.NPOFromGO(selectedObjects[i]);
			if (networkPhysicsObject)
			{
				if (networkPhysicsObject.GetComponent<DeckScript>() || networkPhysicsObject.GetComponent<StackObject>())
				{
					list.Add(networkPhysicsObject);
				}
				else
				{
					list2.Add(networkPhysicsObject);
				}
			}
		}
		for (int j = 0; j < list2.Count; j++)
		{
			NetworkPhysicsObject networkPhysicsObject2 = list2[j];
			if (!networkPhysicsObject2.IsDestroyed)
			{
				if (networkPhysicsObject2.GetComponent<CardScript>())
				{
					for (int k = 0; k < list.Count; k++)
					{
						NetworkPhysicsObject networkPhysicsObject3 = list[k];
						if (networkPhysicsObject3.GetComponent<DeckScript>() && !networkPhysicsObject3.IsDestroyed)
						{
							return true;
						}
					}
					for (int l = 0; l < list2.Count; l++)
					{
						NetworkPhysicsObject networkPhysicsObject4 = list2[l];
						if (networkPhysicsObject2 != networkPhysicsObject4 && networkPhysicsObject4.GetComponent<CardScript>() && !networkPhysicsObject4.IsDestroyed)
						{
							return true;
						}
					}
				}
				else
				{
					CheckStackObject component = networkPhysicsObject2.GetComponent<CheckStackObject>();
					if (component)
					{
						for (int m = 0; m < list.Count; m++)
						{
							NetworkPhysicsObject networkPhysicsObject5 = list[m];
							if (networkPhysicsObject5 && networkPhysicsObject5.GetComponent<StackObject>().CheckStackable(networkPhysicsObject2) && !networkPhysicsObject5.IsDestroyed)
							{
								return true;
							}
						}
						for (int n = 0; n < list2.Count; n++)
						{
							NetworkPhysicsObject networkPhysicsObject6 = list2[n];
							if (!(networkPhysicsObject6 == networkPhysicsObject2) && networkPhysicsObject6.GetComponent<CheckStackObject>() && component.CheckStackable(networkPhysicsObject6) && !networkPhysicsObject6.IsDestroyed)
							{
								return true;
							}
						}
					}
				}
			}
		}
		for (int num = 0; num < list.Count; num++)
		{
			NetworkPhysicsObject networkPhysicsObject7 = list[num];
			if (!networkPhysicsObject7.IsDestroyed)
			{
				if (networkPhysicsObject7.GetComponent<DeckScript>())
				{
					for (int num2 = 0; num2 < list.Count; num2++)
					{
						NetworkPhysicsObject networkPhysicsObject8 = list[num2];
						if (networkPhysicsObject7 != networkPhysicsObject8 && networkPhysicsObject8.GetComponent<DeckScript>() && !networkPhysicsObject8.IsDestroyed)
						{
							return true;
						}
					}
				}
				else
				{
					StackObject component2 = networkPhysicsObject7.GetComponent<StackObject>();
					if (component2)
					{
						for (int num3 = 0; num3 < list.Count; num3++)
						{
							NetworkPhysicsObject networkPhysicsObject9 = list[num3];
							if (!(networkPhysicsObject7 == networkPhysicsObject9) && networkPhysicsObject9.GetComponent<StackObject>() && component2.CheckStackable(networkPhysicsObject9) && !networkPhysicsObject9.IsDestroyed)
							{
								return true;
							}
						}
					}
				}
			}
		}
		return false;
	}

	// Token: 0x060015CB RID: 5579 RVA: 0x000983A7 File Offset: 0x000965A7
	[Remote(Permission.Server)]
	public void RPCCheckLoadingSaveComplete()
	{
		if (this.checkLoadCoroutine != null)
		{
			base.StopCoroutine(this.checkLoadCoroutine);
		}
		this.checkLoadCoroutine = base.StartCoroutine(this.CheckLoadingSaveComplete());
	}

	// Token: 0x060015CC RID: 5580 RVA: 0x000983CF File Offset: 0x000965CF
	private IEnumerator CheckLoadingSaveComplete()
	{
		Stopwatch stopWatch = Stopwatch.StartNew();
		yield return null;
		while (UILoading.IsLoading)
		{
			yield return null;
		}
		if (Network.isServer && Network.maxConnections > 0)
		{
			this.CheckLocalFiles();
		}
		yield return null;
		Chat.LogSystem("Loading complete.", Colour.Green, true);
		Chat.LogSystem(stopWatch.ToPrettyString("Game Load"), false);
		EventManager.TriggerLoadingSaveComplete();
		yield break;
	}

	// Token: 0x060015CD RID: 5581 RVA: 0x000983E0 File Offset: 0x000965E0
	public void CheckLocalFiles()
	{
		List<string> localURLs = SaveScript.GetLocalURLs(this.CurrentState());
		if (localURLs.Count > 0)
		{
			Chat.LogError("Local files currently loaded. These will not work in multiplayer.", true);
			string text = "Local File(s): " + localURLs[0];
			for (int i = 1; i < localURLs.Count; i++)
			{
				text = text + ", " + localURLs[i];
			}
			text += ".";
			Chat.Log(text, ChatMessageType.Game);
		}
	}

	// Token: 0x060015CE RID: 5582 RVA: 0x00098456 File Offset: 0x00096656
	public bool CanPlayImpactSound()
	{
		if (this.RemainingImpactSounds > 0)
		{
			this.RemainingImpactSounds--;
			return true;
		}
		return false;
	}

	// Token: 0x060015CF RID: 5583 RVA: 0x00098472 File Offset: 0x00096672
	public void ChangeTable(string TableName)
	{
		TableName = Utilities.RemoveCloneFromName(TableName);
		this.ChangeTable(NetworkSingleton<GameMode>.Instance.GetPrefab(TableName));
	}

	// Token: 0x060015D0 RID: 5584 RVA: 0x00098490 File Offset: 0x00096690
	public void ChangeTable(GameObject GO)
	{
		GameObject gameObject = this.Table.transform.Find("Trigger").gameObject;
		int childCount = gameObject.transform.childCount;
		NetworkSingleton<ManagerPhysicsObject>.Instance.DestroyThisObject(this.Table);
		this.Table = NetworkSingleton<NetworkUI>.Instance.NetworkSpawn(GO, default(Vector3));
		for (int i = 0; i < childCount; i++)
		{
			GameObject gameObject2 = gameObject.transform.GetChild(i).gameObject;
			Transform transform = this.Table.transform.Find("Trigger").gameObject.transform.Find(gameObject2.name);
			List<NetworkPhysicsObject> handObjects = gameObject2.GetComponent<HandZone>().GetHandObjects(true);
			if (handObjects.Count > 0)
			{
				Vector3 position = new Vector3(0f, 3f, 0f);
				foreach (NetworkPhysicsObject networkPhysicsObject in this.GrabbableNPOs)
				{
					if (networkPhysicsObject.CompareTag("Deck") && !networkPhysicsObject.IsDestroyed)
					{
						networkPhysicsObject.deckScript.PlayDrawCardSound();
						position = networkPhysicsObject.transform.position;
						break;
					}
				}
				foreach (NetworkPhysicsObject networkPhysicsObject2 in handObjects)
				{
					networkPhysicsObject2.GetComponent<Rigidbody>().position = position;
					if (transform)
					{
						networkPhysicsObject2.SetSmoothPosition(transform.position, false, false, false, true, null, false, false, null);
					}
					else
					{
						gameObject2.GetComponent<HandZone>().ResetHandObject(networkPhysicsObject2);
					}
				}
			}
		}
	}

	// Token: 0x060015D1 RID: 5585 RVA: 0x00098668 File Offset: 0x00096868
	public void LoadClassGame(string SGM)
	{
		NetworkSingleton<ManagerPhysicsObject>.Instance.ResetTable(true, true);
		NetworkSingleton<NetworkUI>.Instance.SetCurrentGame(SGM);
		NetworkSingleton<GameMode>.Instance.Spawn("Game_" + SGM);
		LuaGlobalScriptManager.Instance.Init();
		if (SGM == NetworkSingleton<NetworkUI>.Instance.GameBackgammon && this.TableNPO.InternalName == "Table_Circular")
		{
			this.ChangeTable(NetworkSingleton<GameMode>.Instance.OctagonTable);
		}
		if (SGM == NetworkSingleton<NetworkUI>.Instance.GameCardBots && this.TableNPO.InternalName != "Table_RPG")
		{
			this.ChangeTable(NetworkSingleton<GameMode>.Instance.RPGTable);
		}
		if (SGM == NetworkSingleton<NetworkUI>.Instance.GameMahjong && this.TableNPO.InternalName == "Table_Hexagon")
		{
			this.ChangeTable(NetworkSingleton<GameMode>.Instance.OctagonTable);
		}
		if (SGM == NetworkSingleton<NetworkUI>.Instance.GameRPG && this.TableNPO.InternalName != "Table_RPG" && this.TableNPO.InternalName != "Table_Custom")
		{
			this.ChangeTable(NetworkSingleton<GameMode>.Instance.RPGTable);
		}
		if (SGM == NetworkSingleton<NetworkUI>.Instance.GameSolitaire && this.TableNPO.InternalName == "Table_Circular")
		{
			this.ChangeTable(NetworkSingleton<GameMode>.Instance.OctagonTable);
		}
		if (SGM == "Jigsaw" && this.TableNPO.InternalName != "Table_RPG" && this.TableNPO.InternalName != "Table_Custom")
		{
			this.ChangeTable(NetworkSingleton<GameMode>.Instance.RPGTable);
		}
		TableScript.UpdatePureMode();
	}

	// Token: 0x060015D2 RID: 5586 RVA: 0x0009882F File Offset: 0x00096A2F
	[Remote(Permission.Server)]
	public void GarbageCollection()
	{
		if (Network.isServer)
		{
			base.networkView.RPC(RPCTarget.Others, new Action(this.GarbageCollection));
		}
		Singleton<SteamManager>.Instance.StoreStats();
		Resources.UnloadUnusedAssets();
	}

	// Token: 0x060015D3 RID: 5587 RVA: 0x00098860 File Offset: 0x00096A60
	public void RegisterGrabbableObject(NetworkPhysicsObject npo)
	{
		this.GrabbableNPOs.Add(npo);
		this.grabbableDirectory[npo.gameObject] = npo;
		this.AllNPOs.Add(npo);
	}

	// Token: 0x060015D4 RID: 5588 RVA: 0x0009888C File Offset: 0x00096A8C
	public void UnRegisterGrabbableObject(NetworkPhysicsObject npo)
	{
		this.GrabbableNPOs.Remove(npo);
		this.grabbableDirectory.Remove(npo.gameObject);
		this.AllNPOs.Remove(npo);
	}

	// Token: 0x060015D5 RID: 5589 RVA: 0x000988BA File Offset: 0x00096ABA
	public void RegisterPointer(Pointer pointer)
	{
		this.Pointers.Add(pointer);
	}

	// Token: 0x060015D6 RID: 5590 RVA: 0x000988C8 File Offset: 0x00096AC8
	public void UnRegisterPointer(Pointer pointer)
	{
		this.Pointers.Remove(pointer);
	}

	// Token: 0x060015D7 RID: 5591 RVA: 0x000988D7 File Offset: 0x00096AD7
	public void RegisterManagedObject(NetworkPhysicsObject npo)
	{
		this.AllNPOs.Add(npo);
	}

	// Token: 0x060015D8 RID: 5592 RVA: 0x000988E5 File Offset: 0x00096AE5
	public void UnRegisterManagedObject(NetworkPhysicsObject npo)
	{
		this.AllNPOs.Remove(npo);
	}

	// Token: 0x060015D9 RID: 5593 RVA: 0x000988F4 File Offset: 0x00096AF4
	private void OnPlayerChangeColor(PlayerState playerState)
	{
		this.UpdateVisibility();
	}

	// Token: 0x060015DA RID: 5594 RVA: 0x000988F4 File Offset: 0x00096AF4
	private void OnChangePlayerTeam(bool join, int id)
	{
		this.UpdateVisibility();
	}

	// Token: 0x060015DB RID: 5595 RVA: 0x000988FC File Offset: 0x00096AFC
	public void UpdateVisibility()
	{
		Singleton<TeamScript>.Instance.CalculateAllFlags();
		for (int i = 0; i < this.AllNPOs.Count; i++)
		{
			this.AllNPOs[i].UpdateVisiblity(false);
		}
	}

	// Token: 0x060015DC RID: 5596 RVA: 0x0009893C File Offset: 0x00096B3C
	public static void CleanupDummyObject(GameObject dummyGameObject)
	{
		dummyGameObject.AddMissingComponent<DummyObject>();
		MonoBehaviour[] components = dummyGameObject.GetComponents<MonoBehaviour>();
		for (int i = 0; i < components.Length; i++)
		{
			bool flag = true;
			Type type = components[i].GetType();
			foreach (Type type2 in ManagerPhysicsObject.dummyComponentWhitelist)
			{
				if (type == type2 || type.IsSubclassOf(type2))
				{
					flag = false;
					break;
				}
			}
			if (flag)
			{
				UnityEngine.Object.Destroy(components[i]);
			}
		}
		Rigidbody component = dummyGameObject.GetComponent<Rigidbody>();
		if (component)
		{
			component.isKinematic = true;
		}
	}

	// Token: 0x04000C10 RID: 3088
	private const int RAISE_FORCE = 12000;

	// Token: 0x04000C11 RID: 3089
	public const int DRAG_FORCE = 20000;

	// Token: 0x04000C12 RID: 3090
	public const int ROTATION_FORCE = 75;

	// Token: 0x04000C13 RID: 3091
	private const int SCROLL_FORCE = 1000;

	// Token: 0x04000C14 RID: 3092
	public static float SmoothMoveBaseForce = 10000f;

	// Token: 0x04000C15 RID: 3093
	public const int SHAKE_DOT_THRESHOLD = 3;

	// Token: 0x04000C16 RID: 3094
	public const float SHAKE_WINDOW_DURATION = 1f;

	// Token: 0x04000C17 RID: 3095
	private const int SHAKE_FORCE = 100;

	// Token: 0x04000C18 RID: 3096
	public static int ShakeThreshold = 0;

	// Token: 0x04000C19 RID: 3097
	private const int DROP_FORCE = 1;

	// Token: 0x04000C1A RID: 3098
	public const int DICE_ROTATION_FORCE = 20;

	// Token: 0x04000C1B RID: 3099
	public static float DiceRollForceMultiplier = 2f;

	// Token: 0x04000C1C RID: 3100
	public const int MAX_NUM_GRABBED = 128;

	// Token: 0x04000C1D RID: 3101
	public const int MAX_NUM_GRABBED_CLUMPED = 64;

	// Token: 0x04000C1E RID: 3102
	private const int VELOCITY_MAX = 40;

	// Token: 0x04000C1F RID: 3103
	private const float CLOSENESS_THRESHOLD = 0.5f;

	// Token: 0x04000C20 RID: 3104
	public const float RAISE_HEIGHT = 2.5f;

	// Token: 0x04000C21 RID: 3105
	private const float FORWARD_OFFSET = -0.6f;

	// Token: 0x04000C22 RID: 3106
	private const float RIGHT_OFFSET = -0.15f;

	// Token: 0x04000C23 RID: 3107
	public const int MAX_IMPACT_SOUNDS_PER_FRAME = 32;

	// Token: 0x04000C24 RID: 3108
	private const float CARD_SCALE_DIFFERENCE_STACK = 0.15f;

	// Token: 0x04000C25 RID: 3109
	public static bool SpreadDeckRespectsCurrentOrientation = false;

	// Token: 0x04000C26 RID: 3110
	public static bool PrioritizeBagWhenGrouping = false;

	// Token: 0x04000C27 RID: 3111
	public const int MIN_ROTATION_DEG = 15;

	// Token: 0x04000C28 RID: 3112
	public const int MAX_ROTATION_INDEX = 24;

	// Token: 0x04000C29 RID: 3113
	public const int HALF_ROTATION_INDEX = 12;

	// Token: 0x04000C2A RID: 3114
	public const int THREE_QUARTER_ROTATION_INDEX = 18;

	// Token: 0x04000C2B RID: 3115
	private GameObject _CurrentlyZoomedObject;

	// Token: 0x04000C2C RID: 3116
	private readonly Dictionary<GameObject, NetworkPhysicsObject> grabbableDirectory = new Dictionary<GameObject, NetworkPhysicsObject>();

	// Token: 0x04000C2D RID: 3117
	public readonly List<NetworkPhysicsObject> GrabbableNPOs = new List<NetworkPhysicsObject>();

	// Token: 0x04000C2E RID: 3118
	public readonly List<NetworkPhysicsObject> AllNPOs = new List<NetworkPhysicsObject>();

	// Token: 0x04000C2F RID: 3119
	public readonly List<Pointer> Pointers = new List<Pointer>();

	// Token: 0x04000C30 RID: 3120
	public readonly List<HandZone> HandZones = new List<HandZone>();

	// Token: 0x04000C31 RID: 3121
	private GameObject _Table;

	// Token: 0x04000C35 RID: 3125
	public float UnloadInterval = 180f;

	// Token: 0x04000C36 RID: 3126
	private float unloadTime;

	// Token: 0x04000C37 RID: 3127
	public PhysicMaterial DefaultPhysicsMaterial;

	// Token: 0x04000C38 RID: 3128
	public int RemainingImpactSounds = 32;

	// Token: 0x04000C39 RID: 3129
	private VRTrackedController vrTrackedController;

	// Token: 0x04000C3A RID: 3130
	private UIRoot uiRoot;

	// Token: 0x04000C3B RID: 3131
	private List<Vector2> blockGridPositions = new List<Vector2>();

	// Token: 0x04000C3C RID: 3132
	private WaitForSeconds waitTenthOfSecond = new WaitForSeconds(0.1f);

	// Token: 0x04000C3D RID: 3133
	private GameObject saveBag;

	// Token: 0x04000C3E RID: 3134
	private List<PhysicsState> bagList = new List<PhysicsState>();

	// Token: 0x04000C3F RID: 3135
	private string saveToChestName = "";

	// Token: 0x04000C40 RID: 3136
	private string requestedFilePath = "";

	// Token: 0x04000C41 RID: 3137
	private string requestedSaveName = "";

	// Token: 0x04000C42 RID: 3138
	public Dictionary<string, ManagerPhysicsObject.JointRelation> JointRelationFromGUID = new Dictionary<string, ManagerPhysicsObject.JointRelation>();

	// Token: 0x04000C43 RID: 3139
	private List<NetworkPhysicsObject> enableCollisionNPOs = new List<NetworkPhysicsObject>();

	// Token: 0x04000C44 RID: 3140
	private WaitForFixedUpdate waitForFixedUpdate = new WaitForFixedUpdate();

	// Token: 0x04000C45 RID: 3141
	private UnityEngine.Coroutine checkLoadCoroutine;

	// Token: 0x04000C46 RID: 3142
	private static readonly List<Type> dummyComponentWhitelist = new List<Type>
	{
		typeof(CustomObject),
		typeof(DeckScript),
		typeof(CardScript),
		typeof(HideObject),
		typeof(NetworkView),
		typeof(ChildSpawner),
		typeof(DummyObject)
	};

	// Token: 0x0200068C RID: 1676
	public class JointRelation
	{
		// Token: 0x04002877 RID: 10359
		public GameObject Target;

		// Token: 0x04002878 RID: 10360
		public Dictionary<GameObject, JointState> JointStateFromGO = new Dictionary<GameObject, JointState>();
	}
}
