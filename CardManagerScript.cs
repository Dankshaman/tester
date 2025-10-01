using System;
using System.Collections.Generic;
using HighlightingSystem;
using NewNet;
using UnityEngine;

// Token: 0x020000B2 RID: 178
public class CardManagerScript : NetworkSingleton<CardManagerScript>
{
	// Token: 0x060008A8 RID: 2216 RVA: 0x0003DCC2 File Offset: 0x0003BEC2
	public Dictionary<int, CustomDeckData> GetCustomDecks(bool DummyObject = false)
	{
		if (DummyObject)
		{
			return this.DummyCustomDecks;
		}
		return this.CustomDecks;
	}

	// Token: 0x060008A9 RID: 2217 RVA: 0x0003DCD4 File Offset: 0x0003BED4
	public bool TryGetCustomDeckData(bool DummyObject, int index, out CustomDeckData CDD)
	{
		return (DummyObject && this.DummyCustomDecks.TryGetValue(index, out CDD)) || this.CustomDecks.TryGetValue(index, out CDD);
	}

	// Token: 0x060008AA RID: 2218 RVA: 0x0003DCF8 File Offset: 0x0003BEF8
	public void SetupCard(GameObject card, int front_id, int back_id = -1, bool bHide = false)
	{
		if (VRHMD.isVR)
		{
			NetworkPhysicsObject component = card.GetComponent<NetworkPhysicsObject>();
			if (component)
			{
				component.MostRecentCardSetup = new CardSetup
				{
					frontID = front_id,
					backID = back_id,
					isHidden = bHide
				};
				VRVirtualHandObject.VirtualHandObject virtualHandObject;
				if (Singleton<VRVirtualHandObject>.Instance && Singleton<VRVirtualHandObject>.Instance.HandObjects.TryGetValue(component, out virtualHandObject))
				{
					this.SetupCard(virtualHandObject.copy, front_id, back_id, bHide);
				}
			}
		}
		if (front_id == -1)
		{
			return;
		}
		if (back_id == -1)
		{
			back_id = front_id;
		}
		DummyObject component2 = card.GetComponent<DummyObject>();
		if (front_id >= 0)
		{
			int num = 0;
			while (front_id > 99)
			{
				front_id -= 100;
				num++;
			}
			int num2 = 0;
			while (back_id > 99)
			{
				back_id -= 100;
				num2++;
			}
			CustomDeckData customDeckData;
			if (bHide && (!this.TryGetCustomDeckData(component2, num2, out customDeckData) || !customDeckData.UniqueBack || !customDeckData.BackIsHidden))
			{
				front_id = 69;
				back_id = 69;
			}
			if (card.GetComponent<DeckScript>())
			{
				CustomDeck component3 = card.GetComponent<CustomDeck>();
				if (num > 0 || num2 > 0)
				{
					if (!component3)
					{
						card.AddComponent<CustomDeck>();
					}
				}
				else
				{
					UnityEngine.Object.Destroy(component3);
				}
			}
			this.ConvertMaterial(card, front_id, num, back_id, num2, false, bHide, component2);
			return;
		}
		if (front_id > -1000)
		{
			int num3 = 0;
			while (front_id < -299)
			{
				front_id += 100;
				num3++;
			}
			int num4 = 0;
			while (back_id < -299)
			{
				back_id += 100;
				num4++;
			}
			if (front_id <= -200 && front_id >= -269)
			{
				front_id += 200;
			}
			else
			{
				Debug.Log("Front ID ERROR: " + front_id);
			}
			if (back_id <= -200 && back_id >= -269)
			{
				back_id += 200;
			}
			else
			{
				Debug.Log("Back ID ERROR: " + back_id);
			}
			CustomDeckData customDeckData2;
			if (bHide && (!this.TryGetCustomDeckData(component2, num4, out customDeckData2) || !customDeckData2.UniqueBack || !customDeckData2.BackIsHidden))
			{
				front_id = -69;
				back_id = -69;
			}
			this.ConvertMaterial(card, front_id, num3, back_id, num4, true, bHide, component2);
		}
	}

	// Token: 0x060008AB RID: 2219 RVA: 0x0003DF20 File Offset: 0x0003C120
	private void ConvertMaterial(GameObject card, int front_id, int FrontIndex, int back_id, int BackIndex, bool bNegative, bool bHide, bool DummyObject)
	{
		Material[] materials = card.GetComponent<Renderer>().materials;
		front_id = Mathf.Abs(front_id);
		back_id = Mathf.Abs(back_id);
		CustomDeckData customDeckData;
		if (this.TryGetCustomDeckData(DummyObject, FrontIndex, out customDeckData) && customDeckData.FaceMaterial && (materials[1].shader != customDeckData.FaceMaterial.shader || materials[1].mainTexture != customDeckData.FaceMaterial.mainTexture))
		{
			materials[1] = UnityEngine.Object.Instantiate<Material>(customDeckData.FaceMaterial);
		}
		CustomDeckData customDeckData2;
		if (this.TryGetCustomDeckData(DummyObject, BackIndex, out customDeckData2) && customDeckData2.BackMaterial && (materials[2].shader != customDeckData2.BackMaterial.shader || materials[2].mainTexture != customDeckData2.BackMaterial.mainTexture))
		{
			materials[2] = UnityEngine.Object.Instantiate<Material>(customDeckData2.BackMaterial);
		}
		if (front_id >= 0 && front_id <= 69)
		{
			Vector2 vector = new Vector2(0.1f, 0.1428f);
			int numWidth = 10;
			CustomDeckData customDeckData3;
			if (this.TryGetCustomDeckData(DummyObject, FrontIndex, out customDeckData3))
			{
				vector = new Vector2(1f / (float)customDeckData3.NumWidth, 1f / (float)customDeckData3.NumHeight);
				front_id = Mathf.Clamp(front_id, 0, customDeckData3.NumWidth * customDeckData3.NumHeight - 1);
				numWidth = customDeckData3.NumWidth;
			}
			materials[1].mainTextureScale = vector;
			materials[1].mainTextureOffset = this.IdToOffset(front_id, vector, numWidth);
		}
		bool flag = false;
		CustomDeckData customDeckData4;
		if (this.TryGetCustomDeckData(DummyObject, BackIndex, out customDeckData4))
		{
			flag = customDeckData4.UniqueBack;
		}
		if (flag && back_id >= 0 && back_id <= 69)
		{
			Vector2 vector2 = new Vector2(0.1f, 0.1428f);
			int numWidth2 = 10;
			CustomDeckData customDeckData5;
			if (this.TryGetCustomDeckData(DummyObject, BackIndex, out customDeckData5))
			{
				vector2 = new Vector2(1f / (float)customDeckData5.NumWidth, 1f / (float)customDeckData5.NumHeight);
				back_id = Mathf.Clamp(back_id, 0, customDeckData5.NumWidth * customDeckData5.NumHeight - 1);
				numWidth2 = customDeckData5.NumWidth;
			}
			materials[2].mainTextureScale = vector2;
			materials[2].mainTextureOffset = this.IdToOffset(back_id, vector2, numWidth2);
		}
		else
		{
			materials[2].mainTextureScale = Vector2.one;
			materials[2].mainTextureOffset = Vector2.zero;
		}
		float num = -1f;
		if (!bNegative)
		{
			CustomDeckData customDeckData6;
			if (FrontIndex == 0)
			{
				materials[1].shader = Shader.Find("Marmoset/Specular IBL");
				materials[1].mainTexture = this.front_textures[FrontIndex];
				card.GetComponent<MeshFilter>().mesh = this.CardTypeMeshes[0].mesh;
			}
			else if (this.TryGetCustomDeckData(DummyObject, FrontIndex, out customDeckData6) && customDeckData6.FaceTexture != null)
			{
				materials[1].mainTexture = customDeckData6.FaceTexture;
				if (customDeckData6.FaceMaterial == null)
				{
					materials[1].shader = Shader.Find("Marmoset/Specular IBL");
					TextureScript.UpdateMaterialTransparency(materials[1]);
				}
				num = customDeckData6.AspectRatio * 1.4286f / ((float)customDeckData6.NumWidth / (float)customDeckData6.NumHeight);
				if (DLCManager.URLisDLC(customDeckData6.FaceURL) && ((!customDeckData6.FaceURL.StartsWith("<Darkest Night>") && !customDeckData6.FaceURL.StartsWith("<Battle For Souls>")) || num < 1.33f))
				{
					num = 1f;
				}
				if (customDeckData6.Type == CardType.Hex || customDeckData6.Type == CardType.HexRounded)
				{
					num = 1.4286f;
				}
				if (bHide && customDeckData6.BackIsHidden)
				{
					materials[1].mainTextureScale = materials[2].mainTextureScale;
					materials[1].mainTextureOffset = materials[2].mainTextureOffset;
					materials[1].mainTexture = customDeckData6.BackTexture;
				}
				CardManagerScript.CardTypeMeshData cardTypeMeshData = this.CardTypeMeshes[(int)customDeckData6.Type];
				MeshFilter component = card.GetComponent<MeshFilter>();
				if (component.sharedMesh != cardTypeMeshData.mesh)
				{
					component.mesh = cardTypeMeshData.mesh;
					if (cardTypeMeshData.collider)
					{
						BoxCollider component2 = card.GetComponent<BoxCollider>();
						if (component2)
						{
							UnityEngine.Object.Destroy(component2);
							card.AddComponent<MeshCollider>().convex = true;
						}
						card.GetComponent<MeshCollider>().sharedMesh = cardTypeMeshData.collider;
					}
					else
					{
						MeshCollider component3 = card.GetComponent<MeshCollider>();
						if (component3)
						{
							UnityEngine.Object.Destroy(component3);
							card.AddComponent<BoxCollider>();
						}
					}
				}
			}
			CustomDeckData customDeckData7;
			if (BackIndex == 0)
			{
				materials[2].shader = Shader.Find("Marmoset/Specular IBL");
				materials[2].mainTexture = this.back_textures[BackIndex];
			}
			else if (this.TryGetCustomDeckData(DummyObject, BackIndex, out customDeckData7) && customDeckData7.BackTexture != null)
			{
				materials[2].mainTexture = customDeckData7.BackTexture;
				if (customDeckData7.BackMaterial == null)
				{
					materials[2].shader = Shader.Find("Marmoset/Specular IBL");
					TextureScript.UpdateMaterialTransparency(materials[2]);
				}
			}
		}
		else
		{
			if (this.default_front_textures.Count > FrontIndex)
			{
				materials[1].mainTexture = this.default_front_textures[FrontIndex];
			}
			if (this.default_back_textures.Count > BackIndex)
			{
				materials[2].mainTexture = this.default_back_textures[BackIndex];
			}
		}
		card.GetComponent<Renderer>().materials = materials;
		if (card.GetComponent<Highlighter>())
		{
			card.GetComponent<Highlighter>().SetDirty();
		}
		if (materials[1].mainTexture && card.GetComponent<NetworkPhysicsObject>() && num != -1f)
		{
			card.transform.localScale = new Vector3(num * card.GetComponent<NetworkPhysicsObject>().Scale.x, card.transform.localScale.y, card.transform.localScale.z);
			card.GetComponent<NetworkPhysicsObject>().BaseScale = new Vector3(num, card.GetComponent<NetworkPhysicsObject>().BaseScale.y, card.GetComponent<NetworkPhysicsObject>().BaseScale.z);
		}
		NetworkPhysicsObject component4 = card.GetComponent<NetworkPhysicsObject>();
		if (component4)
		{
			component4.ResetBounds();
		}
	}

	// Token: 0x060008AC RID: 2220 RVA: 0x0003E528 File Offset: 0x0003C728
	private Vector2 IdToOffset(int ID, Vector2 AxisScale, int NumWidth)
	{
		float num = (float)((ID + 1) % NumWidth - 1);
		if (num == -1f)
		{
			num = (float)(NumWidth - 1);
		}
		float num2 = (float)(ID / NumWidth + 1);
		num *= AxisScale.x;
		num2 *= AxisScale.y;
		num2 = 1f - num2;
		return new Vector2(num, num2);
	}

	// Token: 0x060008AD RID: 2221 RVA: 0x0003E574 File Offset: 0x0003C774
	public int GetNextIndex(bool DummyObject = false)
	{
		int num = 0;
		foreach (int num2 in this.GetCustomDecks(DummyObject).Keys)
		{
			if (num2 > num)
			{
				num = num2;
			}
		}
		return ++num;
	}

	// Token: 0x060008AE RID: 2222 RVA: 0x0003E5D4 File Offset: 0x0003C7D4
	public void SyncCustomDecksToPlayer(NetworkPlayer NP)
	{
		if (Network.isServer)
		{
			foreach (KeyValuePair<int, CustomDeckData> keyValuePair in this.CustomDecks)
			{
				base.networkView.RPC<int, CustomDeckData, bool>(NP, new Action<int, CustomDeckData, bool>(this.DownloadDeckData), keyValuePair.Key, keyValuePair.Value, false);
			}
		}
	}

	// Token: 0x060008AF RID: 2223 RVA: 0x0003E650 File Offset: 0x0003C850
	public void SetupCustomDeck(DeckScript deckScript, CustomDeckData CDD, int numberCards, bool sideways)
	{
		int nextIndex = this.GetNextIndex(false);
		deckScript.ResetDeck();
		deckScript.bSideways = sideways;
		deckScript.num_cards_ = numberCards;
		deckScript.GenerateDeck(nextIndex);
		this.DownloadDeckData(nextIndex, CDD, false);
	}

	// Token: 0x060008B0 RID: 2224 RVA: 0x0003E68C File Offset: 0x0003C88C
	public void SetupCustomCard(CardScript cardScript, CustomDeckData CDD, bool sideways)
	{
		int nextIndex = this.GetNextIndex(false);
		this.CleanupIndex(cardScript.GetCustomIndexes(), false);
		cardScript.bSideways = sideways;
		cardScript.card_id_ = nextIndex * 100;
		this.DownloadDeckData(nextIndex, CDD, false);
	}

	// Token: 0x060008B1 RID: 2225 RVA: 0x0003E6C8 File Offset: 0x0003C8C8
	[Remote(Permission.Server)]
	public void DownloadDeckData(int Index, CustomDeckData CDD, bool DummyObject = false)
	{
		CDD.FaceURL = CDD.FaceURL.Trim();
		CDD.BackURL = CDD.BackURL.Trim();
		CDD.BackURL = CardManagerScript.ConvertUnique(CDD.BackURL);
		if (Network.isServer && !DummyObject)
		{
			base.networkView.RPC<int, CustomDeckData, bool>(RPCTarget.Others, new Action<int, CustomDeckData, bool>(this.DownloadDeckData), Index, CDD, DummyObject);
		}
		if (this.GetCustomDecks(DummyObject).ContainsKey(Index))
		{
			Debug.Log("Index already in use: " + Index);
			this.ResetCardSetups();
			return;
		}
		this.GetCustomDecks(DummyObject).Add(Index, CDD);
		Debug.Log(string.Format("DownloadDeckData, Index: {0}, DummyObject: {1}", Index, DummyObject));
		Singleton<CustomLoadingManager>.Instance.Texture.Load(CDD.FaceURL, this.GetOnLoadFunc(DummyObject), true, false, false, true, true, false, 8192, CustomLoadingManager.LoadType.Auto);
		Singleton<CustomLoadingManager>.Instance.Texture.Load(CDD.BackURL, this.GetOnLoadFunc(DummyObject), true, false, false, true, true, false, 8192, CustomLoadingManager.LoadType.Auto);
	}

	// Token: 0x060008B2 RID: 2226 RVA: 0x0003E7D1 File Offset: 0x0003C9D1
	public Action<CustomTextureContainer> GetOnLoadFunc(bool DummyObject = false)
	{
		if (DummyObject)
		{
			return new Action<CustomTextureContainer>(this.OnLoadDummyTextureFinish);
		}
		return new Action<CustomTextureContainer>(this.OnLoadTextureFinish);
	}

	// Token: 0x060008B3 RID: 2227 RVA: 0x0003E7F0 File Offset: 0x0003C9F0
	private void OnLoadTextureFinish(CustomTextureContainer container)
	{
		if (container.texture == null)
		{
			if (Network.isServer)
			{
				List<NetworkPhysicsObject> grabbableNPOs = NetworkSingleton<ManagerPhysicsObject>.Instance.GrabbableNPOs;
				for (int i = 0; i < grabbableNPOs.Count; i++)
				{
					if (grabbableNPOs[i].deckScript)
					{
						CustomDeck component = grabbableNPOs[i].GetComponent<CustomDeck>();
						if (component && (component.URLFace == container.nonCodeStrippedURL || component.URLBack == container.nonCodeStrippedURL))
						{
							component.bCustomUI = true;
						}
					}
					if (grabbableNPOs[i].cardScript)
					{
						CustomCard component2 = grabbableNPOs[i].GetComponent<CustomCard>();
						if (component2 && (component2.URLFace == container.nonCodeStrippedURL || component2.URLBack == container.nonCodeStrippedURL))
						{
							component2.bCustomUI = true;
						}
					}
				}
			}
			return;
		}
		bool flag = false;
		foreach (KeyValuePair<int, CustomDeckData> keyValuePair in this.CustomDecks)
		{
			if (keyValuePair.Value.FaceURL == container.nonCodeStrippedURL && keyValuePair.Value.FaceTexture == null)
			{
				flag = true;
				keyValuePair.Value.FaceTexture = container.texture;
				keyValuePair.Value.AspectRatio = container.aspectRatio;
				keyValuePair.Value.FaceMaterial = container.material;
			}
			if (keyValuePair.Value.BackURL == container.nonCodeStrippedURL && keyValuePair.Value.BackTexture == null)
			{
				flag = true;
				keyValuePair.Value.BackTexture = container.texture;
				keyValuePair.Value.BackMaterial = container.material;
			}
		}
		if (flag)
		{
			this.ResetCardSetups();
		}
	}

	// Token: 0x060008B4 RID: 2228 RVA: 0x0003E9F4 File Offset: 0x0003CBF4
	private void OnLoadDummyTextureFinish(CustomTextureContainer container)
	{
		bool flag = false;
		foreach (KeyValuePair<int, CustomDeckData> keyValuePair in this.DummyCustomDecks)
		{
			if (keyValuePair.Value.FaceURL == container.nonCodeStrippedURL && keyValuePair.Value.FaceTexture == null)
			{
				flag = true;
				keyValuePair.Value.FaceTexture = container.texture;
				keyValuePair.Value.AspectRatio = container.aspectRatio;
				keyValuePair.Value.FaceMaterial = container.material;
			}
			if (keyValuePair.Value.BackURL == container.nonCodeStrippedURL && keyValuePair.Value.BackTexture == null)
			{
				flag = true;
				keyValuePair.Value.BackTexture = container.texture;
				keyValuePair.Value.BackMaterial = container.material;
			}
		}
		if (flag)
		{
			this.ResetCardSetups();
		}
	}

	// Token: 0x060008B5 RID: 2229 RVA: 0x0003EB0C File Offset: 0x0003CD0C
	private void OnDestroy()
	{
		this.ResetCardManager();
	}

	// Token: 0x060008B6 RID: 2230 RVA: 0x0003EB14 File Offset: 0x0003CD14
	[Remote(Permission.Server)]
	public void ResetCardManager()
	{
		Debug.Log("ResetCardManager");
		if (Network.isServer)
		{
			base.networkView.RPC(RPCTarget.Others, new Action(this.ResetCardManager));
		}
		this.URLBackList.Clear();
		this.URLFaceList.Clear();
		if (Singleton<CustomLoadingManager>.Instance)
		{
			foreach (KeyValuePair<int, CustomDeckData> keyValuePair in this.CustomDecks)
			{
				Singleton<CustomLoadingManager>.Instance.Texture.Cleanup(keyValuePair.Value.FaceURL, new Action<CustomTextureContainer>(this.OnLoadTextureFinish), true);
				Singleton<CustomLoadingManager>.Instance.Texture.Cleanup(keyValuePair.Value.BackURL, new Action<CustomTextureContainer>(this.OnLoadTextureFinish), true);
			}
		}
		this.CustomDecks.Clear();
	}

	// Token: 0x060008B7 RID: 2231 RVA: 0x0003EC08 File Offset: 0x0003CE08
	private void ResetCardSetups()
	{
		GameObject[] array = GameObject.FindGameObjectsWithTag("Card");
		GameObject[] array2 = GameObject.FindGameObjectsWithTag("Deck");
		for (int i = 0; i < array.Length; i++)
		{
			if (array[i] && array[i].GetComponent<CardScript>())
			{
				int card_id_ = array[i].GetComponent<CardScript>().card_id_;
				this.SetupCard(array[i], card_id_, -1, false);
				HideObject component = array[i].GetComponent<HideObject>();
				if (component.bHidden)
				{
					component.bHidden = false;
					component.Hide(true, false);
				}
			}
		}
		for (int j = 0; j < array2.Length; j++)
		{
			if (array2[j] && array2[j].GetComponent<DeckScript>())
			{
				this.SetupCard(array2[j], array2[j].GetComponent<DeckScript>().get_bottom_card_id(), array2[j].GetComponent<DeckScript>().get_top_card_id(), false);
			}
		}
	}

	// Token: 0x060008B8 RID: 2232 RVA: 0x0003ECE4 File Offset: 0x0003CEE4
	public void BeginSetupCustomCJC()
	{
		if (Network.isServer)
		{
			this.CleanUpURLListCJC();
			for (int i = 0; i < this.URLFaceList.Count; i++)
			{
				if (this.URLFaceList[i] != "Removed" && this.URLBackList.Count > i && this.URLBackList[i] != "Removed")
				{
					int index = i + 1;
					this.DownloadDeckData(index, new CustomDeckData(this.URLFaceList[i], this.URLBackList[i], 10, 7, false, CardManagerScript.CheckUnique(this.URLBackList[i]), CardType.RectangleRounded), false);
				}
			}
		}
	}

	// Token: 0x060008B9 RID: 2233 RVA: 0x0003ED9C File Offset: 0x0003CF9C
	private void CleanUpURLListCJC()
	{
		GameObject[] array = GameObject.FindGameObjectsWithTag("Card");
		GameObject[] array2 = GameObject.FindGameObjectsWithTag("Deck");
		List<int> list = new List<int>();
		for (int i = 0; i < array2.Length; i++)
		{
			foreach (int num in array2[i].GetComponent<DeckScript>().GetDeck())
			{
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
		}
		for (int k = 0; k < array.Length; k++)
		{
			int num3 = 0;
			int l = array[k].GetComponent<CardScript>().card_id_;
			while (l > 99)
			{
				l -= 100;
				num3++;
			}
			if (num3 > 0 && !list.Contains(num3))
			{
				list.Add(num3);
			}
		}
		for (int m = 0; m < this.URLFaceList.Count; m++)
		{
			bool flag = false;
			using (List<int>.Enumerator enumerator = list.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current == m + 1)
					{
						flag = true;
					}
				}
			}
			if (!flag)
			{
				Debug.Log("Cleaned up unused texture");
				this.URLFaceList[m] = "Removed";
				if (this.URLBackList.Count > m)
				{
					this.URLBackList[m] = "Removed";
				}
			}
		}
	}

	// Token: 0x060008BA RID: 2234 RVA: 0x0003EF3C File Offset: 0x0003D13C
	public static bool CheckUnique(string URL)
	{
		return URL.Length > 8 && URL.Substring(URL.Length - 8, 8) == "{Unique}";
	}

	// Token: 0x060008BB RID: 2235 RVA: 0x0003EF62 File Offset: 0x0003D162
	public static string ConvertUnique(string URL)
	{
		if (CardManagerScript.CheckUnique(URL))
		{
			URL = URL.Substring(0, URL.Length - 8);
		}
		return URL;
	}

	// Token: 0x060008BC RID: 2236 RVA: 0x0003EF80 File Offset: 0x0003D180
	public void CleanupIndex(List<int> CustomIndex, bool DummyObject = false)
	{
		if (CustomIndex.Count == 0)
		{
			return;
		}
		GameObject[] array = GameObject.FindGameObjectsWithTag("Card");
		GameObject[] array2 = GameObject.FindGameObjectsWithTag("Deck");
		List<int> list = new List<int>();
		GameObject[] array3 = array;
		for (int i = 0; i < array3.Length; i++)
		{
			CardScript component = array3[i].GetComponent<CardScript>();
			if (component && component.DummyObject == DummyObject)
			{
				foreach (int item in component.GetCustomIndexes())
				{
					if (!list.Contains(item))
					{
						list.Add(item);
					}
				}
			}
		}
		array3 = array2;
		for (int i = 0; i < array3.Length; i++)
		{
			DeckScript component2 = array3[i].GetComponent<DeckScript>();
			if (component2 && component2.DummyObject == DummyObject)
			{
				foreach (int item2 in component2.GetCustomIndexes())
				{
					if (!list.Contains(item2))
					{
						list.Add(item2);
					}
				}
			}
		}
		foreach (int num in CustomIndex)
		{
			bool flag = true;
			using (List<int>.Enumerator enumerator2 = list.GetEnumerator())
			{
				while (enumerator2.MoveNext())
				{
					if (enumerator2.Current == num)
					{
						flag = false;
						break;
					}
				}
			}
			CustomDeckData customDeckData;
			if (flag && this.GetCustomDecks(DummyObject).TryGetValue(num, out customDeckData))
			{
				bool flag2 = true;
				bool flag3 = true;
				this.GetCustomDecks(DummyObject).Remove(num);
				foreach (KeyValuePair<int, CustomDeckData> keyValuePair in this.GetCustomDecks(DummyObject))
				{
					if (keyValuePair.Value.FaceURL == customDeckData.FaceURL)
					{
						flag2 = false;
					}
					if (keyValuePair.Value.BackURL == customDeckData.BackURL)
					{
						flag3 = false;
					}
				}
				Debug.Log(string.Format("Cleanup Custom Cards OnDestroy, Dummy: {0} Face: {1} Back: {2}", DummyObject, flag2, flag3));
				if (flag2 && Singleton<CustomLoadingManager>.Instance)
				{
					Singleton<CustomLoadingManager>.Instance.Texture.Cleanup(customDeckData.FaceURL, this.GetOnLoadFunc(DummyObject), true);
				}
				if (flag3 && Singleton<CustomLoadingManager>.Instance)
				{
					Singleton<CustomLoadingManager>.Instance.Texture.Cleanup(customDeckData.BackURL, this.GetOnLoadFunc(DummyObject), true);
				}
				if (!DummyObject)
				{
					base.networkView.RPC<int, bool, bool>(RPCTarget.Others, new Action<int, bool, bool>(this.RPCCleanupIndex), num, flag2, flag3);
				}
			}
		}
	}

	// Token: 0x060008BD RID: 2237 RVA: 0x0003F2C0 File Offset: 0x0003D4C0
	[Remote(Permission.Server)]
	private void RPCCleanupIndex(int CustomID, bool bCleanupFace, bool bCleanupBack)
	{
		CustomDeckData customDeckData;
		if (this.CustomDecks.TryGetValue(CustomID, out customDeckData))
		{
			this.CustomDecks.Remove(CustomID);
			Debug.Log(string.Format("Cleanup Custom Cards OnDestroy, Face: {0} Back: {1}", bCleanupFace, bCleanupBack));
			if (bCleanupFace && Singleton<CustomLoadingManager>.Instance)
			{
				Singleton<CustomLoadingManager>.Instance.Texture.Cleanup(customDeckData.FaceURL, new Action<CustomTextureContainer>(this.OnLoadTextureFinish), true);
			}
			if (bCleanupBack && Singleton<CustomLoadingManager>.Instance)
			{
				Singleton<CustomLoadingManager>.Instance.Texture.Cleanup(customDeckData.BackURL, new Action<CustomTextureContainer>(this.OnLoadTextureFinish), true);
			}
		}
	}

	// Token: 0x0400064C RID: 1612
	public static List<string> CardTypeNames = new List<string>
	{
		"Rectangle (Rounded)",
		"Rectangle",
		"Hex (Rounded)",
		"Hex",
		"Circle"
	};

	// Token: 0x0400064D RID: 1613
	public List<CardManagerScript.CardTypeMeshData> CardTypeMeshes = new List<CardManagerScript.CardTypeMeshData>();

	// Token: 0x0400064E RID: 1614
	public List<Texture2D> back_textures = new List<Texture2D>();

	// Token: 0x0400064F RID: 1615
	public List<Texture2D> front_textures = new List<Texture2D>();

	// Token: 0x04000650 RID: 1616
	public List<Texture2D> default_back_textures = new List<Texture2D>();

	// Token: 0x04000651 RID: 1617
	public List<Texture2D> default_front_textures = new List<Texture2D>();

	// Token: 0x04000652 RID: 1618
	public List<string> URLBackList = new List<string>();

	// Token: 0x04000653 RID: 1619
	public List<string> URLFaceList = new List<string>();

	// Token: 0x04000654 RID: 1620
	private Dictionary<int, CustomDeckData> CustomDecks = new Dictionary<int, CustomDeckData>();

	// Token: 0x04000655 RID: 1621
	private Dictionary<int, CustomDeckData> DummyCustomDecks = new Dictionary<int, CustomDeckData>();

	// Token: 0x04000656 RID: 1622
	private const float CARD_RATIO = 1.4286f;

	// Token: 0x02000582 RID: 1410
	[Serializable]
	public class CardTypeMeshData
	{
		// Token: 0x04002514 RID: 9492
		public Mesh mesh;

		// Token: 0x04002515 RID: 9493
		public Mesh collider;
	}
}
