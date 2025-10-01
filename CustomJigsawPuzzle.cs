using System;
using System.Collections;
using System.Collections.Generic;
using NewNet;
using UnityEngine;

// Token: 0x020000D5 RID: 213
public class CustomJigsawPuzzle : CustomImage
{
	// Token: 0x170001C4 RID: 452
	// (get) Token: 0x06000A75 RID: 2677 RVA: 0x00049D49 File Offset: 0x00047F49
	// (set) Token: 0x06000A76 RID: 2678 RVA: 0x00049D69 File Offset: 0x00047F69
	public int NumPuzzlePieces
	{
		get
		{
			if (CustomJigsawPuzzle.JIGSAW_SIZE_OVERRIDE <= 4)
			{
				return this._numPuzzlePieces;
			}
			return CustomJigsawPuzzle.JIGSAW_SIZE_OVERRIDE * 5 * (CustomJigsawPuzzle.JIGSAW_SIZE_OVERRIDE * 4);
		}
		set
		{
			this._numPuzzlePieces = value;
		}
	}

	// Token: 0x06000A77 RID: 2679 RVA: 0x00049D72 File Offset: 0x00047F72
	protected override void Start()
	{
		base.Start();
		CustomJigsawPuzzle.Instance = this;
		EventManager.OnObjectDrop += this.OnObjectDrop;
	}

	// Token: 0x06000A78 RID: 2680 RVA: 0x00049D91 File Offset: 0x00047F91
	protected override void OnDestroy()
	{
		EventManager.OnObjectDrop -= this.OnObjectDrop;
		base.OnDestroy();
	}

	// Token: 0x170001C5 RID: 453
	// (get) Token: 0x06000A79 RID: 2681 RVA: 0x0004642E File Offset: 0x0004462E
	// (set) Token: 0x06000A7A RID: 2682 RVA: 0x00049DAA File Offset: 0x00047FAA
	public override bool bCustomUI
	{
		get
		{
			return this.bcustomUI;
		}
		set
		{
			if (value != this.bcustomUI)
			{
				this.bcustomUI = value;
				if (value && Network.isServer)
				{
					Singleton<UICustomJigsawPuzzle>.Instance.Queue(this);
				}
			}
		}
	}

	// Token: 0x06000A7B RID: 2683 RVA: 0x00049DD1 File Offset: 0x00047FD1
	protected override void Awake()
	{
		base.Awake();
		this.boardGrid = base.transform.Find("Cube").GetComponent<BoardGrid>();
	}

	// Token: 0x06000A7C RID: 2684 RVA: 0x00049DF4 File Offset: 0x00047FF4
	protected override void OnCallCustomRPC()
	{
		base.OnCallCustomRPC();
		base.networkView.RPC<int, bool>(RPCTarget.All, new Action<int, bool>(this.RPCJigsawPuzzle), this.NumPuzzlePieces, this.bImageOnBoard);
	}

	// Token: 0x06000A7D RID: 2685 RVA: 0x00049E20 File Offset: 0x00048020
	protected override void OnCallCustomRPC(NetworkPlayer NP)
	{
		base.OnCallCustomRPC(NP);
		base.networkView.RPC<int, bool>(NP, new Action<int, bool>(this.RPCJigsawPuzzle), this.NumPuzzlePieces, this.bImageOnBoard);
	}

	// Token: 0x06000A7E RID: 2686 RVA: 0x00049E50 File Offset: 0x00048050
	[Remote(Permission.Admin)]
	private void RPCJigsawPuzzle(int number, bool bImage)
	{
		this.NumPuzzlePieces = number;
		this.bImageOnBoard = bImage;
		int numPuzzlePieces = this.NumPuzzlePieces;
		GameObject spawnPiece;
		if (numPuzzlePieces <= 80)
		{
			if (numPuzzlePieces == 20)
			{
				if (Network.isServer)
				{
					this.boardGrid.GridSize = 4.16f;
					this.boardGrid.bGridOffset = true;
					this.boardGrid.CalculateGrid();
				}
				spawnPiece = NetworkSingleton<GameMode>.Instance.JigsawPuzzlePiece20;
				this.CollidersList = Json.Load<List<BoxColliderState[]>>(this.ColliderStateJson20.text);
				goto IL_1BB;
			}
			if (numPuzzlePieces == 80)
			{
				spawnPiece = NetworkSingleton<GameMode>.Instance.JigsawPuzzlePiece80;
				this.CollidersList = Json.Load<List<BoxColliderState[]>>(this.ColliderStateJson80.text);
				goto IL_1BB;
			}
		}
		else
		{
			if (numPuzzlePieces == 180)
			{
				if (Network.isServer)
				{
					this.boardGrid.GridSize = 1.388f;
					this.boardGrid.bGridOffset = false;
					this.boardGrid.CalculateGrid();
				}
				spawnPiece = NetworkSingleton<GameMode>.Instance.JigsawPuzzlePiece180;
				this.CollidersList = Json.Load<List<BoxColliderState[]>>(this.ColliderStateJson180.text);
				goto IL_1BB;
			}
			if (numPuzzlePieces == 320)
			{
				if (Network.isServer)
				{
					this.boardGrid.GridSize = 1.0405f;
					this.boardGrid.bGridOffset = false;
					this.boardGrid.CalculateGrid();
				}
				spawnPiece = NetworkSingleton<GameMode>.Instance.JigsawPuzzlePiece320;
				this.CollidersList = Json.Load<List<BoxColliderState[]>>(this.ColliderStateJson320.text);
				goto IL_1BB;
			}
		}
		if (Network.isServer)
		{
			this.boardGrid.GridSize = 4.16f / (float)CustomJigsawPuzzle.JIGSAW_SIZE_OVERRIDE;
			this.boardGrid.bGridOffset = false;
			this.boardGrid.CalculateGrid();
		}
		spawnPiece = NetworkSingleton<GameMode>.Instance.JigsawPuzzlePiece320;
		this.CollidersList = Json.Load<List<BoxColliderState[]>>(this.ColliderStateJson320.text);
		IL_1BB:
		if (Network.isServer)
		{
			base.StartCoroutine(this.CheckSpawnJigsawPieces(spawnPiece));
		}
	}

	// Token: 0x06000A7F RID: 2687 RVA: 0x0004A02D File Offset: 0x0004822D
	private IEnumerator CheckSpawnJigsawPieces(GameObject SpawnPiece)
	{
		yield return null;
		bool flag = false;
		using (List<NetworkPhysicsObject>.Enumerator enumerator = NetworkSingleton<ManagerPhysicsObject>.Instance.GrabbableNPOs.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				if (enumerator.Current.CompareTag("Jigsaw"))
				{
					flag = true;
					break;
				}
			}
		}
		if (!flag)
		{
			List<int> list = new List<int>();
			for (int i = 0; i < this.NumPuzzlePieces; i++)
			{
				list.Add(i);
			}
			list.Randomize<int>();
			List<Vector3> list2 = new List<Vector3>(this.NumPuzzlePieces);
			int numPuzzlePieces = this.NumPuzzlePieces;
			if (numPuzzlePieces <= 80)
			{
				if (numPuzzlePieces == 20)
				{
					for (int j = 0; j < this.NumPuzzlePieces; j++)
					{
						float f = 6.2831855f * (float)j / (float)this.NumPuzzlePieces;
						float x = 24f * Mathf.Cos(f);
						float num = 24f * Mathf.Sin(f);
						if (num > 14f)
						{
							num = 14f;
						}
						if (num < -14f)
						{
							num = -14f;
						}
						list2.Add(new Vector3(x, 2f, num));
					}
					goto IL_86C;
				}
				if (numPuzzlePieces == 80)
				{
					for (int k = 0; k < this.NumPuzzlePieces / 2; k++)
					{
						float f2 = 6.2831855f * (float)k / (float)(this.NumPuzzlePieces / 2);
						float x2 = 22f * Mathf.Cos(f2);
						float num2 = 22f * Mathf.Sin(f2);
						if (num2 > 13f)
						{
							num2 = 13f;
						}
						if (num2 < -13f)
						{
							num2 = -13f;
						}
						list2.Add(new Vector3(x2, 2f, num2));
						x2 = 26f * Mathf.Cos(f2);
						num2 = 26f * Mathf.Sin(f2);
						if (num2 > 17f)
						{
							num2 = 17f;
						}
						if (num2 < -17f)
						{
							num2 = -17f;
						}
						list2.Add(new Vector3(x2, 2f, num2));
					}
					goto IL_86C;
				}
			}
			else
			{
				if (numPuzzlePieces == 180)
				{
					for (int l = 0; l < this.NumPuzzlePieces / 4; l++)
					{
						float f3 = 6.2831855f * (float)l / (float)(this.NumPuzzlePieces / 4);
						float x3 = 17f * Mathf.Cos(f3);
						float num3 = 17f * Mathf.Sin(f3);
						if (num3 > 11f)
						{
							num3 = 11f;
						}
						if (num3 < -11f)
						{
							num3 = -11f;
						}
						list2.Add(new Vector3(x3, 2f, num3));
						x3 = 20f * Mathf.Cos(f3);
						num3 = 20f * Mathf.Sin(f3);
						if (num3 > 13f)
						{
							num3 = 13f;
						}
						if (num3 < -13f)
						{
							num3 = -13f;
						}
						list2.Add(new Vector3(x3, 2f, num3));
						x3 = 23f * Mathf.Cos(f3);
						num3 = 23f * Mathf.Sin(f3);
						if (num3 > 15f)
						{
							num3 = 15f;
						}
						if (num3 < -15f)
						{
							num3 = -15f;
						}
						list2.Add(new Vector3(x3, 2f, num3));
						x3 = 26f * Mathf.Cos(f3);
						num3 = 26f * Mathf.Sin(f3);
						if (num3 > 17f)
						{
							num3 = 17f;
						}
						if (num3 < -17f)
						{
							num3 = -17f;
						}
						list2.Add(new Vector3(x3, 2f, num3));
					}
					goto IL_86C;
				}
				if (numPuzzlePieces == 320)
				{
					for (int m = 0; m < (this.NumPuzzlePieces - 28) / 5; m++)
					{
						float f4 = 6.2831855f * (float)m / (float)((this.NumPuzzlePieces - 28) / 5);
						float x4 = 19f * Mathf.Cos(f4);
						float num4 = 19f * Mathf.Sin(f4);
						if (num4 > 10f)
						{
							num4 = 10f;
						}
						if (num4 < -10f)
						{
							num4 = -10f;
						}
						list2.Add(new Vector3(x4, 2f, num4));
						x4 = 21f * Mathf.Cos(f4);
						num4 = 21f * Mathf.Sin(f4);
						if (num4 > 12f)
						{
							num4 = 12f;
						}
						if (num4 < -12f)
						{
							num4 = -12f;
						}
						list2.Add(new Vector3(x4, 2f, num4));
						x4 = 23f * Mathf.Cos(f4);
						num4 = 23f * Mathf.Sin(f4);
						if (num4 > 14f)
						{
							num4 = 14f;
						}
						if (num4 < -14f)
						{
							num4 = -14f;
						}
						list2.Add(new Vector3(x4, 2f, num4));
						x4 = 25.5f * Mathf.Cos(f4);
						num4 = 25.5f * Mathf.Sin(f4);
						if (num4 > 16f)
						{
							num4 = 16f;
						}
						if (num4 < -16f)
						{
							num4 = -16f;
						}
						list2.Add(new Vector3(x4, 2f, num4));
						x4 = 27f * Mathf.Cos(f4);
						num4 = 27f * Mathf.Sin(f4);
						if (num4 > 18f)
						{
							num4 = 18f;
						}
						if (num4 < -18f)
						{
							num4 = -18f;
						}
						list2.Add(new Vector3(x4, 2f, num4));
					}
					for (int n = 0; n < 15; n++)
					{
						float num5 = 13f + (float)(2 * (n % 2));
						float z = -7f + (float)n;
						list2.Add(new Vector3(num5, 2f, z));
						list2.Add(new Vector3(-num5, 2f, z));
					}
					Debug.Log("Num of SpawnPositions: " + list2.Count);
					goto IL_86C;
				}
			}
			for (int num6 = 0; num6 < (this.NumPuzzlePieces - 28) / 5; num6++)
			{
				float f5 = 6.2831855f * (float)num6 / (float)((this.NumPuzzlePieces - 28) / 5);
				float x5 = 19f * Mathf.Cos(f5);
				float num7 = 19f * Mathf.Sin(f5);
				if (num7 > 10f)
				{
					num7 = 10f;
				}
				if (num7 < -10f)
				{
					num7 = -10f;
				}
				list2.Add(new Vector3(x5, 2f, num7));
				x5 = 21f * Mathf.Cos(f5);
				num7 = 21f * Mathf.Sin(f5);
				if (num7 > 12f)
				{
					num7 = 12f;
				}
				if (num7 < -12f)
				{
					num7 = -12f;
				}
				list2.Add(new Vector3(x5, 2f, num7));
				x5 = 23f * Mathf.Cos(f5);
				num7 = 23f * Mathf.Sin(f5);
				if (num7 > 14f)
				{
					num7 = 14f;
				}
				if (num7 < -14f)
				{
					num7 = -14f;
				}
				list2.Add(new Vector3(x5, 2f, num7));
				x5 = 25.5f * Mathf.Cos(f5);
				num7 = 25.5f * Mathf.Sin(f5);
				if (num7 > 16f)
				{
					num7 = 16f;
				}
				if (num7 < -16f)
				{
					num7 = -16f;
				}
				list2.Add(new Vector3(x5, 2f, num7));
				x5 = 27f * Mathf.Cos(f5);
				num7 = 27f * Mathf.Sin(f5);
				if (num7 > 18f)
				{
					num7 = 18f;
				}
				if (num7 < -18f)
				{
					num7 = -18f;
				}
				list2.Add(new Vector3(x5, 2f, num7));
			}
			for (int num8 = 0; num8 < 15; num8++)
			{
				float num9 = 13f + (float)(2 * (num8 % 2));
				float z2 = -7f + (float)num8;
				list2.Add(new Vector3(num9, 2f, z2));
				list2.Add(new Vector3(-num9, 2f, z2));
			}
			Debug.Log("Num of SpawnPositions: " + list2.Count);
			IL_86C:
			for (int num10 = 0; num10 < this.NumPuzzlePieces; num10++)
			{
				GameObject gameObject = Network.Instantiate(SpawnPiece, list2[num10], Quaternion.Euler(new Vector3(0f, UnityEngine.Random.Range(0f, 360f))), default(NetworkPlayer));
				gameObject.GetComponent<MeshSyncScript>().SetMesh(list[0]);
				string text = gameObject.GetComponent<MeshSyncScript>().Meshes[list[0]].name.Replace("_", " ").Replace("-", " ");
				LibString.bite(ref text, false, ' ', false, false, '\0');
				int x6 = int.Parse(LibString.bite(ref text, false, ' ', false, false, '\0'));
				int z3 = int.Parse(LibString.bite(ref text, false, ' ', false, false, '\0'));
				gameObject.GetComponent<JigsawPiece>().desiredPosition = this.boardGrid.PositionFromCoordinate(x6, z3);
				if (CustomJigsawPuzzle.JIGSAW_SIZE_OVERRIDE > 4)
				{
					float num11 = 4f / (float)CustomJigsawPuzzle.JIGSAW_SIZE_OVERRIDE;
					gameObject.transform.localScale = new Vector3(num11, num11, num11);
				}
				list.RemoveAt(0);
			}
		}
		yield break;
	}

	// Token: 0x06000A80 RID: 2688 RVA: 0x0004A044 File Offset: 0x00048244
	protected override void OnSetupImage(Texture T, float AspectRatio, Material mat)
	{
		base.OnSetupImage(T, AspectRatio, mat);
		Renderer component = base.transform.Find("Board_Custom").GetComponent<Renderer>();
		if (this.bImageOnBoard)
		{
			if (mat != null)
			{
				component.sharedMaterial = UnityEngine.Object.Instantiate<Material>(mat);
			}
			component.sharedMaterial.mainTexture = T;
			if (component.sharedMaterial.HasProperty("_Color"))
			{
				component.sharedMaterial.color = new Color(0.4f, 0.4f, 0.4f);
			}
		}
		else if (component.sharedMaterial.HasProperty("_Color"))
		{
			component.sharedMaterial.color = new Color(0.4f, 0.4f, 0.4f);
		}
		base.ResetObject();
		base.StartCoroutine(this.SetupJigsawPieces(T, mat));
	}

	// Token: 0x06000A81 RID: 2689 RVA: 0x0004A112 File Offset: 0x00048312
	private IEnumerator SetupJigsawPieces(Texture T, Material mat)
	{
		yield return new WaitForSeconds(2f);
		if (mat != null)
		{
			mat = UnityEngine.Object.Instantiate<Material>(mat);
		}
		using (List<NetworkPhysicsObject>.Enumerator enumerator = NetworkSingleton<ManagerPhysicsObject>.Instance.GrabbableNPOs.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				NetworkPhysicsObject networkPhysicsObject = enumerator.Current;
				GameObject gameObject = networkPhysicsObject.gameObject;
				if (gameObject.CompareTag("Jigsaw"))
				{
					if (mat != null)
					{
						gameObject.transform.GetComponent<Renderer>().sharedMaterial = mat;
					}
					gameObject.GetComponent<Renderer>().sharedMaterial.mainTexture = T;
					Mesh sharedMesh = gameObject.GetComponent<MeshFilter>().sharedMesh;
					foreach (BoxColliderState[] array in this.CollidersList)
					{
						if (array[0].Name == sharedMesh.name)
						{
							BoxCollider[] componentsInChildren = gameObject.GetComponentsInChildren<BoxCollider>();
							for (int i = 0; i < componentsInChildren.Length; i++)
							{
								UnityEngine.Object.Destroy(componentsInChildren[i].gameObject);
							}
							BoxColliderState.CreateBoxColliders(gameObject, array);
						}
					}
					if (Network.isServer)
					{
						gameObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
						gameObject.GetComponent<NetworkPhysicsObject>().IsLocked = gameObject.GetComponent<JigsawPiece>().bCachedFreeze;
						gameObject.GetComponent<NetworkPhysicsObject>().ResetBounds();
					}
				}
				if (gameObject.name == "JigsawPuzzleBox(Clone)")
				{
					if (mat != null)
					{
						gameObject.transform.GetComponent<Renderer>().sharedMaterial = UnityEngine.Object.Instantiate<Material>(mat);
					}
					gameObject.transform.GetComponent<Renderer>().sharedMaterial.mainTexture = T;
					gameObject.GetComponent<NetworkPhysicsObject>().highlighter.SetDirty();
				}
			}
			yield break;
		}
		yield break;
	}

	// Token: 0x06000A82 RID: 2690 RVA: 0x0004A130 File Offset: 0x00048330
	[Remote(Permission.Admin)]
	public void Solve()
	{
		if (Network.isClient)
		{
			base.networkView.RPC(RPCTarget.Server, new Action(this.Solve));
			return;
		}
		new Vector3(0f, 180f, 0f);
		foreach (NetworkPhysicsObject networkPhysicsObject in NetworkSingleton<ManagerPhysicsObject>.Instance.GrabbableNPOs)
		{
			JigsawPiece component = networkPhysicsObject.GetComponent<JigsawPiece>();
			if (component && !component.IsInPosition())
			{
				component.MovePieceIntoPosition();
			}
		}
	}

	// Token: 0x06000A83 RID: 2691 RVA: 0x0004A1D0 File Offset: 0x000483D0
	[Remote(Permission.Client, SendType.ReliableBuffered, null, SerializationMethod.Default)]
	public void Check()
	{
		if (Network.isClient)
		{
			base.networkView.RPC(RPCTarget.Server, new Action(this.Check));
			return;
		}
		this.Randomize(true);
	}

	// Token: 0x06000A84 RID: 2692 RVA: 0x0004A1FC File Offset: 0x000483FC
	[Remote(Permission.Admin)]
	public void Randomize(bool validate = false)
	{
		if (Network.isClient)
		{
			base.networkView.RPC<bool>(RPCTarget.Server, new Action<bool>(this.Randomize), validate);
			return;
		}
		new Vector3(0f, 180f, 0f);
		int num = 0;
		List<NetworkPhysicsObject> grabbableNPOs = NetworkSingleton<ManagerPhysicsObject>.Instance.GrabbableNPOs;
		foreach (NetworkPhysicsObject networkPhysicsObject in grabbableNPOs)
		{
			JigsawPiece component = networkPhysicsObject.GetComponent<JigsawPiece>();
			if (component)
			{
				num++;
			}
		}
		List<Vector3> list = new List<Vector3>(num);
		if (num <= 20)
		{
			for (int i = 0; i < num; i++)
			{
				float f = 6.2831855f * (float)i / (float)num;
				float x = 24f * Mathf.Cos(f);
				float num2 = 24f * Mathf.Sin(f);
				if (num2 > 14f)
				{
					num2 = 14f;
				}
				if (num2 < -14f)
				{
					num2 = -14f;
				}
				list.Add(new Vector3(x, 2f, num2));
			}
		}
		else if (num <= 80)
		{
			for (int j = 0; j < num / 2; j++)
			{
				float f2 = 6.2831855f * (float)j / (float)(num / 2);
				float x2 = 22f * Mathf.Cos(f2);
				float num3 = 22f * Mathf.Sin(f2);
				if (num3 > 13f)
				{
					num3 = 13f;
				}
				if (num3 < -13f)
				{
					num3 = -13f;
				}
				list.Add(new Vector3(x2, 2f, num3));
				x2 = 26f * Mathf.Cos(f2);
				num3 = 26f * Mathf.Sin(f2);
				if (num3 > 17f)
				{
					num3 = 17f;
				}
				if (num3 < -17f)
				{
					num3 = -17f;
				}
				list.Add(new Vector3(x2, 2f, num3));
			}
		}
		else if (num <= 180)
		{
			for (int k = 0; k < num / 4; k++)
			{
				float f3 = 6.2831855f * (float)k / (float)(num / 4);
				float x3 = 17f * Mathf.Cos(f3);
				float num4 = 17f * Mathf.Sin(f3);
				if (num4 > 11f)
				{
					num4 = 11f;
				}
				if (num4 < -11f)
				{
					num4 = -11f;
				}
				list.Add(new Vector3(x3, 2f, num4));
				x3 = 20f * Mathf.Cos(f3);
				num4 = 20f * Mathf.Sin(f3);
				if (num4 > 13f)
				{
					num4 = 13f;
				}
				if (num4 < -13f)
				{
					num4 = -13f;
				}
				list.Add(new Vector3(x3, 2f, num4));
				x3 = 23f * Mathf.Cos(f3);
				num4 = 23f * Mathf.Sin(f3);
				if (num4 > 15f)
				{
					num4 = 15f;
				}
				if (num4 < -15f)
				{
					num4 = -15f;
				}
				list.Add(new Vector3(x3, 2f, num4));
				x3 = 26f * Mathf.Cos(f3);
				num4 = 26f * Mathf.Sin(f3);
				if (num4 > 17f)
				{
					num4 = 17f;
				}
				if (num4 < -17f)
				{
					num4 = -17f;
				}
				list.Add(new Vector3(x3, 2f, num4));
			}
		}
		else if (num <= 320)
		{
			for (int l = 0; l < (num - 28) / 5; l++)
			{
				float f4 = 6.2831855f * (float)l / (float)((num - 28) / 5);
				float x4 = 19f * Mathf.Cos(f4);
				float num5 = 19f * Mathf.Sin(f4);
				if (num5 > 10f)
				{
					num5 = 10f;
				}
				if (num5 < -10f)
				{
					num5 = -10f;
				}
				list.Add(new Vector3(x4, 2f, num5));
				x4 = 21f * Mathf.Cos(f4);
				num5 = 21f * Mathf.Sin(f4);
				if (num5 > 12f)
				{
					num5 = 12f;
				}
				if (num5 < -12f)
				{
					num5 = -12f;
				}
				list.Add(new Vector3(x4, 2f, num5));
				x4 = 23f * Mathf.Cos(f4);
				num5 = 23f * Mathf.Sin(f4);
				if (num5 > 14f)
				{
					num5 = 14f;
				}
				if (num5 < -14f)
				{
					num5 = -14f;
				}
				list.Add(new Vector3(x4, 2f, num5));
				x4 = 25.5f * Mathf.Cos(f4);
				num5 = 25.5f * Mathf.Sin(f4);
				if (num5 > 16f)
				{
					num5 = 16f;
				}
				if (num5 < -16f)
				{
					num5 = -16f;
				}
				list.Add(new Vector3(x4, 2f, num5));
				x4 = 27f * Mathf.Cos(f4);
				num5 = 27f * Mathf.Sin(f4);
				if (num5 > 18f)
				{
					num5 = 18f;
				}
				if (num5 < -18f)
				{
					num5 = -18f;
				}
				list.Add(new Vector3(x4, 2f, num5));
			}
			for (int m = 0; m < 15; m++)
			{
				float num6 = 13f + (float)(2 * (m % 2));
				float z = -7f + (float)m;
				list.Add(new Vector3(num6, 2f, z));
				list.Add(new Vector3(-num6, 2f, z));
			}
		}
		else
		{
			for (int n = 0; n < (num - 28) / 5; n++)
			{
				float f5 = 6.2831855f * (float)n / (float)((num - 28) / 5);
				float x5 = 19f * Mathf.Cos(f5);
				float num7 = 19f * Mathf.Sin(f5);
				if (num7 > 10f)
				{
					num7 = 10f;
				}
				if (num7 < -10f)
				{
					num7 = -10f;
				}
				list.Add(new Vector3(x5, 2f, num7));
				x5 = 21f * Mathf.Cos(f5);
				num7 = 21f * Mathf.Sin(f5);
				if (num7 > 12f)
				{
					num7 = 12f;
				}
				if (num7 < -12f)
				{
					num7 = -12f;
				}
				list.Add(new Vector3(x5, 2f, num7));
				x5 = 23f * Mathf.Cos(f5);
				num7 = 23f * Mathf.Sin(f5);
				if (num7 > 14f)
				{
					num7 = 14f;
				}
				if (num7 < -14f)
				{
					num7 = -14f;
				}
				list.Add(new Vector3(x5, 2f, num7));
				x5 = 25.5f * Mathf.Cos(f5);
				num7 = 25.5f * Mathf.Sin(f5);
				if (num7 > 16f)
				{
					num7 = 16f;
				}
				if (num7 < -16f)
				{
					num7 = -16f;
				}
				list.Add(new Vector3(x5, 2f, num7));
				x5 = 27f * Mathf.Cos(f5);
				num7 = 27f * Mathf.Sin(f5);
				if (num7 > 18f)
				{
					num7 = 18f;
				}
				if (num7 < -18f)
				{
					num7 = -18f;
				}
				list.Add(new Vector3(x5, 2f, num7));
			}
			for (int num8 = 0; num8 < 15; num8++)
			{
				float num9 = 13f + (float)(2 * (num8 % 2));
				float z2 = -7f + (float)num8;
				list.Add(new Vector3(num9, 2f, z2));
				list.Add(new Vector3(-num9, 2f, z2));
			}
		}
		list.Randomize<Vector3>();
		bool flag = true;
		Vector3 origin = Vector3.zero;
		int num10 = UnityEngine.Random.Range(1, list.Count);
		foreach (NetworkPhysicsObject networkPhysicsObject2 in grabbableNPOs)
		{
			JigsawPiece component = networkPhysicsObject2.GetComponent<JigsawPiece>();
			if (component)
			{
				if (list.Count > 0 && (!validate || !component.IsInPosition()))
				{
					component.MovePieceIntoPosition(list[0], new Vector3(0f, UnityEngine.Random.Range(0f, 360f)));
					list.RemoveAt(0);
					flag = false;
				}
				if (flag && num10 > 0)
				{
					num10--;
					if (num10 == 0)
					{
						origin = component.desiredPosition;
					}
				}
			}
		}
		if (flag)
		{
			base.StartCoroutine(this.Victory(origin));
		}
	}

	// Token: 0x06000A85 RID: 2693 RVA: 0x0004AA90 File Offset: 0x00048C90
	public void CheckForVictory(Vector3 origin)
	{
		foreach (NetworkPhysicsObject networkPhysicsObject in NetworkSingleton<ManagerPhysicsObject>.Instance.GrabbableNPOs)
		{
			JigsawPiece component = networkPhysicsObject.GetComponent<JigsawPiece>();
			if (component && !component.IsInPosition())
			{
				return;
			}
		}
		base.StartCoroutine(this.Victory(origin));
	}

	// Token: 0x06000A86 RID: 2694 RVA: 0x0004AB08 File Offset: 0x00048D08
	private IEnumerator Victory(Vector3 origin)
	{
		yield return new WaitForSeconds(0.5f);
		foreach (NetworkPhysicsObject networkPhysicsObject in NetworkSingleton<ManagerPhysicsObject>.Instance.GrabbableNPOs)
		{
			JigsawPiece component = networkPhysicsObject.GetComponent<JigsawPiece>();
			if (component)
			{
				component.MoveDelayRelativeToDistance(origin);
			}
		}
		base.GetComponent<AudioSource>().Play();
		yield break;
	}

	// Token: 0x06000A87 RID: 2695 RVA: 0x0004AB20 File Offset: 0x00048D20
	public void OnObjectDrop(NetworkPhysicsObject Drop, PlayerState LastPlayerToHold)
	{
		if (!Network.isServer)
		{
			return;
		}
		JigsawPiece component = Drop.GetComponent<JigsawPiece>();
		if (component && component.IsInPosition())
		{
			this.CheckForVictory(component.desiredPosition);
		}
	}

	// Token: 0x0400076A RID: 1898
	public static bool JIGSAW_ANIMATE_BOX;

	// Token: 0x0400076B RID: 1899
	public static int JIGSAW_SIZE_OVERRIDE;

	// Token: 0x0400076C RID: 1900
	public static CustomJigsawPuzzle Instance;

	// Token: 0x0400076D RID: 1901
	public TextAsset ColliderStateJson20;

	// Token: 0x0400076E RID: 1902
	public TextAsset ColliderStateJson80;

	// Token: 0x0400076F RID: 1903
	public TextAsset ColliderStateJson180;

	// Token: 0x04000770 RID: 1904
	public TextAsset ColliderStateJson320;

	// Token: 0x04000771 RID: 1905
	public BoardGrid boardGrid;

	// Token: 0x04000772 RID: 1906
	public Material BoardSharedMaterial;

	// Token: 0x04000773 RID: 1907
	private int _numPuzzlePieces = 80;

	// Token: 0x04000774 RID: 1908
	public bool bImageOnBoard = true;

	// Token: 0x04000775 RID: 1909
	private List<BoxColliderState[]> CollidersList;
}
