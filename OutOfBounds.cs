using System;
using NewNet;
using UnityEngine;

// Token: 0x020001D3 RID: 467
public class OutOfBounds : MonoBehaviour
{
	// Token: 0x0600185B RID: 6235 RVA: 0x000A5A05 File Offset: 0x000A3C05
	private void Start()
	{
		if (OutOfBounds.roomCeiling == null)
		{
			OutOfBounds.roomCeiling = Singleton<MonoBehaviour>.Instance.gameObject;
		}
	}

	// Token: 0x0600185C RID: 6236 RVA: 0x000A5A24 File Offset: 0x000A3C24
	private void OnTriggerEnter(Collider Other)
	{
		if (Network.isServer)
		{
			if (OutOfBounds.bStopTeleport)
			{
				return;
			}
			if (Other.GetComponent<Collider>() && Other.gameObject.layer != 2)
			{
				if (Other.GetComponent<NetworkPhysicsObject>())
				{
					Other.GetComponent<NetworkPhysicsObject>().SetCollision(true);
				}
				else
				{
					Other.GetComponent<Collider>().isTrigger = false;
				}
			}
			if (Other.CompareTag("Pointer") || Other.CompareTag("Fog"))
			{
				return;
			}
			if (Other.CompareTag("Card"))
			{
				if (Other.GetComponent<CardScript>().CardAttachToThis)
				{
					Other.GetComponent<CardScript>().CardAttachToThis.GetComponent<CardScript>().ResetCard();
				}
				if (Other.GetComponent<FixedJoint>())
				{
					Other.GetComponent<CardScript>().ResetCard();
				}
			}
			if (Other.GetComponent<HideObject>())
			{
				Other.GetComponent<HideObject>().Hide(false, false);
			}
			NetworkPhysicsObject networkPhysicsObject = NetworkSingleton<ManagerPhysicsObject>.Instance.NPOFromCollider(Other);
			if (!networkPhysicsObject || networkPhysicsObject.tableScript)
			{
				return;
			}
			if (networkPhysicsObject && !networkPhysicsObject.IsLocked)
			{
				if (networkPhysicsObject.IsDestroyed)
				{
					NetworkSingleton<ManagerPhysicsObject>.Instance.DestroyThisObject(networkPhysicsObject.gameObject);
					return;
				}
				float num = 50f;
				Vector3 velocity;
				Vector3 vector = NetworkSingleton<ManagerPhysicsObject>.Instance.StaticSurfacePointBelowWorldPos(new Vector3(0f, num, 0f), out velocity);
				float maxDistance = 7.5f + num - vector.y;
				Vector3 vector2 = new Vector3(networkPhysicsObject.transform.position.x, num, networkPhysicsObject.transform.position.z);
				Vector3 vector3 = Vector3.zero;
				float num2 = 1f;
				Vector3 vector4;
				Bounds boundsNotNormalized = networkPhysicsObject.GetBoundsNotNormalized(out vector4);
				float num3 = vector4.y + boundsNotNormalized.extents.y + 4f;
				for (;;)
				{
					num2 -= 0.025f;
					Vector3 vector5 = new Vector3(vector2.x * num2, vector2.y, vector2.z * num2);
					if (Mathf.Abs(vector5.x) < 1f && Mathf.Abs(vector5.z) < 1f)
					{
						break;
					}
					int num4 = Physics.RaycastNonAlloc(vector5, Vector3.down, this.raycastHits, maxDistance, OutOfBounds.TeleportLayerMask);
					bool flag = false;
					int i = 0;
					while (i < num4)
					{
						RaycastHit raycastHit = this.raycastHits[i];
						Rigidbody componentInParent = raycastHit.collider.GetComponentInParent<Rigidbody>();
						if (componentInParent)
						{
							velocity = componentInParent.velocity;
							if (velocity.sqrMagnitude > 1f || (!componentInParent.useGravity && !componentInParent.isKinematic))
							{
								i++;
								continue;
							}
						}
						Vector3 point = raycastHit.point;
						Vector3 vector6 = new Vector3(point.x + vector4.x, point.y + num3, point.z + vector4.z);
						while (Physics.CheckBox(vector6, boundsNotNormalized.extents, Quaternion.identity, OutOfBounds.TeleportCheckBoxMask))
						{
							vector6.y += 0.5f;
							if (vector3.y > 20f)
							{
								break;
							}
						}
						flag = true;
						vector3 = vector6;
						break;
					}
					if (flag)
					{
						goto IL_336;
					}
				}
				vector3 = new Vector3(0f, 3f, 0f);
				vector3.y += num3;
				IL_336:
				if (OutOfBounds.roomCeiling && vector3.y > OutOfBounds.roomCeiling.transform.position.y)
				{
					vector3.y = OutOfBounds.roomCeiling.transform.position.y;
				}
				networkPhysicsObject.SetCollision(false);
				networkPhysicsObject.rigidbody.position = vector3;
				networkPhysicsObject.rigidbody.velocity = Vector3.zero;
				networkPhysicsObject.rigidbody.angularVelocity = Vector3.zero;
				networkPhysicsObject.rigidbody.useGravity = false;
				if (networkPhysicsObject.soundScript)
				{
					networkPhysicsObject.soundScript.TeleportSound();
				}
			}
		}
	}

	// Token: 0x04000E95 RID: 3733
	private float SpawnSpace = 15f;

	// Token: 0x04000E96 RID: 3734
	public static bool bStopTeleport = false;

	// Token: 0x04000E97 RID: 3735
	public static GameObject roomCeiling = null;

	// Token: 0x04000E98 RID: 3736
	public static int TeleportLayerMask = Layers.Mask(new int[]
	{
		10
	});

	// Token: 0x04000E99 RID: 3737
	public static int TeleportCheckBoxMask = Layers.Mask(new int[]
	{
		10,
		2
	});

	// Token: 0x04000E9A RID: 3738
	private RaycastHit[] raycastHits = new RaycastHit[6];
}
