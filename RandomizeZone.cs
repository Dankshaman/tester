using System;
using System.Collections.Generic;
using NewNet;
using UnityEngine;

// Token: 0x020001EC RID: 492
public class RandomizeZone : Zone
{
	// Token: 0x060019EF RID: 6639 RVA: 0x000B58AB File Offset: 0x000B3AAB
	protected override void Start()
	{
		base.Start();
		if (base.BoxCollider.enabled && base.networkView.isMine)
		{
			base.networkView.RPC<int>(RPCTarget.All, new Action<int>(this.SetSpawnId), this.SpawnId_);
		}
	}

	// Token: 0x060019F0 RID: 6640 RVA: 0x000B58EB File Offset: 0x000B3AEB
	[Remote(Permission.Client, SendType.ReliableBuffered, null, SerializationMethod.Default)]
	public void SetSpawnId(int id)
	{
		this.SpawnId_ = id;
		if (NetworkID.ID == this.SpawnId_ && RandomizeZone.ShowPromptOnLoad)
		{
			NetworkSingleton<NetworkUI>.Instance.GUIConfirmRandomize.GetComponent<UIRandomize>().Queue(this);
		}
	}

	// Token: 0x060019F1 RID: 6641 RVA: 0x000B591D File Offset: 0x000B3B1D
	protected override bool ValidateAddObject(NetworkPhysicsObject npo)
	{
		return base.ValidateAddObject(npo) && !npo.CompareTag("Board") && !npo.IsLocked;
	}

	// Token: 0x060019F2 RID: 6642 RVA: 0x000B5940 File Offset: 0x000B3B40
	private void OnTriggerEnter(Collider otherCollider)
	{
		NetworkPhysicsObject networkPhysicsObject = NetworkSingleton<ManagerPhysicsObject>.Instance.NPOFromCollider(otherCollider);
		if (!networkPhysicsObject)
		{
			return;
		}
		base.AddObject(otherCollider, networkPhysicsObject);
	}

	// Token: 0x060019F3 RID: 6643 RVA: 0x000B596C File Offset: 0x000B3B6C
	private void OnTriggerExit(Collider otherCollider)
	{
		NetworkPhysicsObject networkPhysicsObject = NetworkSingleton<ManagerPhysicsObject>.Instance.NPOFromCollider(otherCollider);
		if (!networkPhysicsObject)
		{
			return;
		}
		base.RemoveObject(otherCollider, networkPhysicsObject);
	}

	// Token: 0x060019F4 RID: 6644 RVA: 0x000B5998 File Offset: 0x000B3B98
	[Remote(Permission.Client, SendType.ReliableBuffered, null, SerializationMethod.Default)]
	public void Randomize()
	{
		if (Network.isClient)
		{
			base.networkView.RPC(RPCTarget.Server, new Action(this.Randomize));
			return;
		}
		List<TransformState> list = new List<TransformState>();
		List<GameObject> list2 = new List<GameObject>();
		List<int> list3 = new List<int>();
		List<ObjectState> list4 = new List<ObjectState>();
		List<ObjectState> list5 = new List<ObjectState>();
		base.GetComponent<NetworkPhysicsObject>();
		int i = 0;
		while (i < this.ContainedNPOs.Count)
		{
			GameObject gameObject = this.ContainedNPOs[i].gameObject;
			if (gameObject.CompareTag("Deck"))
			{
				List<int> deck = gameObject.GetComponent<DeckScript>().GetDeck();
				List<ObjectState> cardStates = gameObject.GetComponent<DeckScript>().GetCardStates();
				foreach (int item in deck)
				{
					list3.Add(item);
				}
				using (List<ObjectState>.Enumerator enumerator2 = cardStates.GetEnumerator())
				{
					while (enumerator2.MoveNext())
					{
						ObjectState item2 = enumerator2.Current;
						list4.Add(item2);
					}
					goto IL_210;
				}
				goto IL_F8;
			}
			goto IL_F8;
			IL_210:
			i++;
			continue;
			IL_F8:
			if (gameObject.CompareTag("Card"))
			{
				list3.Add(gameObject.GetComponent<CardScript>().card_id_);
				list4.Add(NetworkSingleton<ManagerPhysicsObject>.Instance.SaveObjectState(gameObject));
				goto IL_210;
			}
			if (gameObject.CompareTag("Bag"))
			{
				using (List<ObjectState>.Enumerator enumerator2 = gameObject.GetComponent<StackObject>().ObjectsHolder.GetEnumerator())
				{
					while (enumerator2.MoveNext())
					{
						ObjectState item3 = enumerator2.Current;
						list5.Add(item3);
					}
					goto IL_210;
				}
			}
			Vector3 position = gameObject.transform.position;
			Quaternion rotation = gameObject.transform.rotation;
			TransformState item4 = new TransformState
			{
				posX = position.x,
				posY = position.y,
				posZ = position.z,
				rotX = rotation.eulerAngles.x,
				rotY = rotation.eulerAngles.y,
				rotZ = rotation.eulerAngles.z
			};
			list.Add(item4);
			list2.Add(gameObject);
			goto IL_210;
		}
		if (list3.Count > 0)
		{
			List<int> list6 = new List<int>();
			List<ObjectState> list7 = new List<ObjectState>();
			while (list3.Count > 0)
			{
				int index = UnityEngine.Random.Range(0, list3.Count);
				list6.Add(list3[index]);
				list7.Add(list4[index]);
				list3.RemoveAt(index);
				list4.RemoveAt(index);
			}
			list3 = list6;
			list4 = list7;
			for (int j = 0; j < this.ContainedNPOs.Count; j++)
			{
				GameObject gameObject2 = this.ContainedNPOs[j].gameObject;
				if (gameObject2.CompareTag("Deck"))
				{
					int count = gameObject2.GetComponent<DeckScript>().GetDeck().Count;
					List<int> list8 = new List<int>();
					List<ObjectState> list9 = new List<ObjectState>();
					for (int k = 0; k < count; k++)
					{
						list8.Add(list3[0]);
						list9.Add(list4[0]);
						list4.RemoveAt(0);
						list3.RemoveAt(0);
					}
					gameObject2.GetComponent<DeckScript>().SetDeck(list8, list9);
				}
				if (gameObject2.CompareTag("Card"))
				{
					gameObject2.GetComponent<CardScript>().card_id_ = list3[0];
					NetworkSingleton<CardManagerScript>.Instance.SetupCard(gameObject2, list3[0], -1, false);
					ObjectState objectState = list4[0];
					NetworkPhysicsObject component = gameObject2.GetComponent<NetworkPhysicsObject>();
					component.Name = objectState.Nickname;
					component.Description = objectState.Description;
					component.GMNotes = objectState.GMNotes;
					component.Memo = objectState.Memo;
					list3.RemoveAt(0);
					list4.RemoveAt(0);
				}
			}
		}
		list.Randomize<TransformState>();
		if (list5.Count > 0)
		{
			for (int l = 0; l < this.ContainedNPOs.Count; l++)
			{
				GameObject gameObject3 = this.ContainedNPOs[l].gameObject;
				if (!gameObject3.CompareTag("Deck") && !gameObject3.CompareTag("Card") && !gameObject3.CompareTag("Bag"))
				{
					list5.Add(NetworkSingleton<ManagerPhysicsObject>.Instance.SaveObjectState(gameObject3));
					NetworkSingleton<ManagerPhysicsObject>.Instance.DestroyThisObject(gameObject3);
				}
			}
			list5.Randomize<ObjectState>();
			for (int m = 0; m < this.ContainedNPOs.Count; m++)
			{
				GameObject gameObject4 = this.ContainedNPOs[m].gameObject;
				if (gameObject4.CompareTag("Bag"))
				{
					int count2 = gameObject4.GetComponent<StackObject>().ObjectsHolder.Count;
					List<ObjectState> list10 = new List<ObjectState>();
					for (int n = 0; n < count2; n++)
					{
						list10.Add(list5[0]);
						list5.RemoveAt(0);
					}
					gameObject4.GetComponent<StackObject>().ObjectsHolder = list10;
				}
			}
			for (int num = 0; num < list.Count; num++)
			{
				list[num].scaleX = list5[num].Transform.scaleX;
				list[num].scaleY = list5[num].Transform.scaleY;
				list[num].scaleZ = list5[num].Transform.scaleZ;
				list5[num].Transform = list[num];
				NetworkSingleton<ManagerPhysicsObject>.Instance.LoadObjectState(list5[num], false, false);
			}
			return;
		}
		for (int num2 = 0; num2 < list2.Count; num2++)
		{
			GameObject gameObject5 = list2[num2];
			gameObject5.transform.position = new Vector3(list[num2].posX, list[num2].posY, list[num2].posZ);
			gameObject5.transform.eulerAngles = new Vector3(list[num2].rotX, list[num2].rotY, list[num2].rotZ);
			if (gameObject5.GetComponent<SoundScript>())
			{
				gameObject5.GetComponent<SoundScript>().PickUpSound();
			}
		}
	}

	// Token: 0x04000FFA RID: 4090
	public static bool ShowPromptOnLoad = true;

	// Token: 0x04000FFB RID: 4091
	public int SpawnId_ = 1;
}
