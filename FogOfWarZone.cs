using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Assets.Scripts.FogOfWar;
using NewNet;
using RTEditor;
using UnityEngine;

// Token: 0x02000113 RID: 275
public class FogOfWarZone : Zone
{
	// Token: 0x06000E07 RID: 3591 RVA: 0x00059E34 File Offset: 0x00058034
	protected override void Start()
	{
		base.Start();
		this.BuildPlaneMesh();
		if (this._verticesTransformed != null)
		{
			this._meshBuilt = true;
		}
		this._mesh = this.Plane.GetComponent<MeshFilter>().mesh;
		this._vertices = this._mesh.vertices;
		this._hider = "FOW_" + base.NPO.ID.ToString();
		this._lastScale = base.gameObject.transform.localScale;
		this._previousColor = ((NetworkSingleton<PlayerManager>.Instance.MyPlayerState() != null) ? NetworkSingleton<PlayerManager>.Instance.MyPlayerState().stringColor : "Grey");
		for (int i = 0; i < this._vertices.Length; i++)
		{
			this._fogColor.Add(Color.black);
		}
		if (this.RevealedLocations.Count == 0)
		{
			this._loadingComplete = true;
		}
		this.RPCResetFogOfWar();
		this.LoadFogOfWar();
		EventManager.OnFogOfWarRevealerAdd += this.OnFogOfWarRevealerAdd;
		EventManager.OnFogOfWarRevealerDestroy += this.OnFogOfWarRevealerDestroy;
		EventManager.OnPlayerChangeColor += this.OnPlayerChangeColor;
		EventManager.OnObjectTagsChange += this.OnObjectTagsChange;
		EventManager.OnChangePlayerTeam += this.OnPlayerChangeTeam;
		EventManager.OnObjectPickUp += this.OnObjectPickup;
		NetworkEvents.OnPlayerConnected += this.OnPlayerConnect;
	}

	// Token: 0x06000E08 RID: 3592 RVA: 0x00059FA0 File Offset: 0x000581A0
	protected override void OnDestroy()
	{
		base.OnDestroy();
		EventManager.OnFogOfWarRevealerAdd -= this.OnFogOfWarRevealerAdd;
		EventManager.OnFogOfWarRevealerDestroy -= this.OnFogOfWarRevealerDestroy;
		EventManager.OnPlayerChangeColor -= this.OnPlayerChangeColor;
		EventManager.OnObjectTagsChange -= this.OnObjectTagsChange;
		EventManager.OnChangePlayerTeam -= this.OnPlayerChangeTeam;
		EventManager.OnObjectPickUp -= this.OnObjectPickup;
		NetworkEvents.OnPlayerConnected -= this.OnPlayerConnect;
		foreach (NetworkPhysicsObject networkPhysicsObject in this._revealerObjects.Keys)
		{
			if (networkPhysicsObject)
			{
				networkPhysicsObject.GetComponent<FogOfWarRevealer>().HideOutline();
			}
		}
		foreach (NetworkPhysicsObject networkPhysicsObject2 in this._hiddenObjects)
		{
			if (networkPhysicsObject2)
			{
				networkPhysicsObject2.SetInvisible(this._hider, false, 2147483647U, false, false);
			}
		}
		if (Colour.MyColorLabel() == "Black")
		{
			this.TurnOffGMHighlighting();
		}
		foreach (Pointer pointer in this.pointersInZone)
		{
			if (pointer)
			{
				pointer.ReferenceFollower.GetComponent<MeshLerpToObject>().IsInvisible = false;
			}
		}
	}

	// Token: 0x06000E09 RID: 3593 RVA: 0x0005A148 File Offset: 0x00058348
	protected override void Update()
	{
		base.Update();
		if (!this._loadingComplete || !this._meshBuilt || !this._triggerLoaded)
		{
			return;
		}
		if (this._counter != 0)
		{
			return;
		}
		this.OnScaleChange();
		this.FoWGreyOut();
		this.RehideGameObjectsPre();
		this.RevealFogAndObjects();
		this.HideHeldObjectsInDark();
		this.RehideGameObjectsPost();
	}

	// Token: 0x06000E0A RID: 3594 RVA: 0x0005A1A4 File Offset: 0x000583A4
	private void LateUpdate()
	{
		if (this._counter != 1)
		{
			this._counter++;
			return;
		}
		if (this._updateAllObjects)
		{
			this._updateAllObjects = false;
			this.PlayerChangeColorOrTeam(NetworkSingleton<PlayerManager>.Instance.MyPlayerState());
		}
		try
		{
			for (int i = 0; i < this._aaData.Count; i++)
			{
				AntiAliasingData antiAliasingData = this._aaData[i];
				if (FogOfWarZone.IsColorImpactingMe(antiAliasingData.Color))
				{
					FogOfWarAntiAliasing.AntiAliasing(this._fogColor, 2f, antiAliasingData.IndicesInPolygon, antiAliasingData.Scale, 2);
				}
			}
			this._aaData.Clear();
		}
		catch (InvalidOperationException)
		{
		}
		this._mesh.SetColors(this._fogColor);
		this._counter = 0;
	}

	// Token: 0x06000E0B RID: 3595 RVA: 0x0005A270 File Offset: 0x00058470
	private static bool IsColorImpactingMe(string color)
	{
		string text = Colour.MyColorLabel();
		return color == text || text == "Black" || NetworkSingleton<PlayerManager>.Instance.SameTeam(color, -1) || color == "All" || color == "Black";
	}

	// Token: 0x06000E0C RID: 3596 RVA: 0x0005A2C4 File Offset: 0x000584C4
	private async void SetColors()
	{
		using (List<AntiAliasingData>.Enumerator enumerator = this._aaData.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				AntiAliasingData data = enumerator.Current;
				if (FogOfWarZone.IsColorImpactingMe(data.Color))
				{
					this._aaTasks.Add(Task.Run(delegate()
					{
						FogOfWarAntiAliasing.AntiAliasing(this._fogColor, 2f, data.IndicesInPolygon, data.Scale, 2);
					}));
				}
			}
		}
		await Task.WhenAll(this._aaTasks);
		this._aaData.Clear();
		this._aaTasks.Clear();
		this._mesh.SetColors(this._fogColor);
	}

	// Token: 0x06000E0D RID: 3597 RVA: 0x0005A300 File Offset: 0x00058500
	private void OnScaleChange()
	{
		if (this._lastScale == base.gameObject.transform.localScale)
		{
			return;
		}
		this._lastScale = base.gameObject.transform.localScale;
		this._meshBuilt = false;
		this._triggerLoaded = false;
		this.BuildPlaneMesh();
		this._meshBuilt = true;
		this._mesh = this.Plane.GetComponent<MeshFilter>().mesh;
		this._vertices = this._mesh.vertices;
		this.RPCResetFogOfWar();
	}

	// Token: 0x06000E0E RID: 3598 RVA: 0x0005A38C File Offset: 0x0005858C
	private void FoWGreyOut()
	{
		for (int i = 0; i < this._fogColor.Count; i++)
		{
			if (this._fogColor[i].a <= 0.33f)
			{
				if (this.GreyOut)
				{
					Color value = this._fogColor[i];
					value.a = 0.33f;
					this._fogColor[i] = value;
				}
			}
			else if (this._previousColor == "Black")
			{
				Color value2 = this._fogColor[i];
				value2.a = 0.45f;
				this._fogColor[i] = value2;
			}
		}
	}

	// Token: 0x06000E0F RID: 3599 RVA: 0x0005A438 File Offset: 0x00058638
	private void RehideGameObjectsPre()
	{
		if (!this._reHideObjects)
		{
			return;
		}
		this._clonedRevealedObjects.Clear();
		foreach (NetworkPhysicsObject item in this._revealedObjects.Keys)
		{
			this._clonedRevealedObjects.Add(item);
		}
		this._revealedObjects.Clear();
	}

	// Token: 0x06000E10 RID: 3600 RVA: 0x0005A4B4 File Offset: 0x000586B4
	private void RehideGameObjectsPost()
	{
		if (!this._reHideObjects)
		{
			return;
		}
		for (int i = 0; i < this._clonedRevealedObjects.Count; i++)
		{
			NetworkPhysicsObject networkPhysicsObject = this._clonedRevealedObjects[i];
			if (!(networkPhysicsObject == null) && (!this._revealedObjects.ContainsKey(networkPhysicsObject) || !this.IsObjectRevealedToColour(networkPhysicsObject, Colour.MyColorLabel())))
			{
				this.HideGameObject(networkPhysicsObject);
			}
		}
	}

	// Token: 0x06000E11 RID: 3601 RVA: 0x0005A51C File Offset: 0x0005871C
	private async void RevealFogAndObjects()
	{
		if (this._revealerObjects.Count > 0)
		{
			this._vertexTasks.Clear();
			this._workNPOs.Clear();
			this._nullNPOs.Clear();
			foreach (NetworkPhysicsObject networkPhysicsObject in this._revealerObjects.Keys)
			{
				if (networkPhysicsObject != null)
				{
					this._workNPOs.Add(networkPhysicsObject);
				}
				else
				{
					this._nullNPOs.Add(networkPhysicsObject);
				}
			}
			for (int i = 0; i < this._workNPOs.Count; i++)
			{
				NetworkPhysicsObject networkPhysicsObject2 = this._workNPOs[i];
				Vector3 position = networkPhysicsObject2.transform.position;
				Vector3 eulerAngles = networkPhysicsObject2.transform.eulerAngles;
				FogOfWarRevealer component = networkPhysicsObject2.GetComponent<FogOfWarRevealer>();
				float range = component.Range;
				if (!Physics.CheckSphere(position, range, this.FogLayer, QueryTriggerInteraction.Collide))
				{
					component.HideOutline();
				}
				else if (range <= 0f)
				{
					component.HideOutline();
				}
				else if (Vector3.Distance(position, this._revealerObjects[networkPhysicsObject2].localPosition) < 0.1f)
				{
					this.RevealFogOfWar(this._revealerObjects[networkPhysicsObject2].localPosition, this._revealerObjects[networkPhysicsObject2].eulerAngles, component);
				}
				else
				{
					this._revealerObjects[networkPhysicsObject2].localPosition = position;
					this._revealerObjects[networkPhysicsObject2].eulerAngles = eulerAngles;
					this.RevealFogOfWar(position, eulerAngles, component);
				}
			}
			for (int j = 0; j < this._nullNPOs.Count; j++)
			{
				this._revealerObjects.Remove(this._nullNPOs[j]);
			}
			await Task.WhenAll(this._vertexTasks);
		}
	}

	// Token: 0x06000E12 RID: 3602 RVA: 0x0005A558 File Offset: 0x00058758
	private void HideHeldObjectsInDark()
	{
		for (int i = this._heldObjects.Count - 1; i >= 0; i--)
		{
			NetworkPhysicsObject networkPhysicsObject = this._heldObjects[i];
			if (networkPhysicsObject == null || networkPhysicsObject.IsHeldByNobody || this._revealerObjects.ContainsKey(networkPhysicsObject))
			{
				this._heldObjects.RemoveAt(i);
			}
			else
			{
				this.RehideObjectInFog(networkPhysicsObject, 0.45f, true);
			}
		}
	}

	// Token: 0x06000E13 RID: 3603 RVA: 0x0005A5C4 File Offset: 0x000587C4
	private void RehideObjectInFog(NetworkPhysicsObject npo, float fogAlpha, bool reReveal)
	{
		if (!base.IsNPOCollidersInZone(npo))
		{
			this.RevealGameObject(npo, false, "All");
			return;
		}
		Vector3 position = npo.gameObject.transform.position;
		float num = float.PositiveInfinity;
		int num2 = 0;
		for (int i = 0; i < this._vertices.Length; i++)
		{
			Vector3 position2 = this._vertices[i];
			Vector3 a = this.Plane.transform.TransformPoint(position2);
			a.y = 100f;
			float num3 = Vector3.Distance(a, position);
			if (num3 < num)
			{
				num = num3;
				num2 = i;
			}
		}
		if (this._fogColor[num2].a >= fogAlpha)
		{
			this._revealedObjects.Remove(npo);
			this.HideGameObject(npo);
			return;
		}
		if (reReveal)
		{
			foreach (KeyValuePair<string, HashSet<int>> keyValuePair in this.RevealedLocations)
			{
				if (keyValuePair.Value.Contains(num2))
				{
					this.RevealGameObject(npo, true, keyValuePair.Key);
				}
			}
		}
	}

	// Token: 0x06000E14 RID: 3604 RVA: 0x0005A6E4 File Offset: 0x000588E4
	protected override bool ValidateAddObject(NetworkPhysicsObject npo)
	{
		return base.ValidateAddObject(npo) && !npo.CompareTag("Board") && !npo.IgnoresFogOfWar && this._hideObjects;
	}

	// Token: 0x06000E15 RID: 3605 RVA: 0x0005A70C File Offset: 0x0005890C
	protected override void OnAddObject(NetworkPhysicsObject npo)
	{
		this.HideGameObject(npo);
		base.OnAddObject(npo);
	}

	// Token: 0x06000E16 RID: 3606 RVA: 0x0005A71C File Offset: 0x0005891C
	protected override void OnRemoveObject(NetworkPhysicsObject npo)
	{
		this.RemoveObjectHiding(npo);
		base.OnRemoveObject(npo);
	}

	// Token: 0x06000E17 RID: 3607 RVA: 0x0005A72C File Offset: 0x0005892C
	private void OnTriggerEnter(Collider otherCollider)
	{
		this._triggerLoaded = true;
		GameObject gameObject = NetworkSingleton<ManagerPhysicsObject>.Instance.GOFromCollider(otherCollider);
		if (gameObject.CompareTag("Pointer"))
		{
			Pointer component = gameObject.GetComponent<Pointer>();
			if (this._hideGmPointer && component.PointerColorLabel == "Black" && !component.networkView.isMine && this.pointersInZone.TryAddUnique(component))
			{
				component.ReferenceFollower.GetComponent<MeshLerpToObject>().IsInvisible = true;
				return;
			}
			return;
		}
		else
		{
			NetworkPhysicsObject networkPhysicsObject = NetworkSingleton<ManagerPhysicsObject>.Instance.NPOFromGO(gameObject);
			if (!networkPhysicsObject)
			{
				return;
			}
			base.AddObject(otherCollider, networkPhysicsObject);
			return;
		}
	}

	// Token: 0x06000E18 RID: 3608 RVA: 0x0005A7C8 File Offset: 0x000589C8
	private void OnTriggerExit(Collider otherCollider)
	{
		GameObject gameObject = NetworkSingleton<ManagerPhysicsObject>.Instance.GOFromCollider(otherCollider);
		if (gameObject.CompareTag("Pointer"))
		{
			Pointer component = gameObject.GetComponent<Pointer>();
			if (component.PointerColorLabel == "Black" && this.pointersInZone.Remove(component))
			{
				component.ReferenceFollower.GetComponent<MeshLerpToObject>().IsInvisible = false;
				return;
			}
			return;
		}
		else
		{
			NetworkPhysicsObject networkPhysicsObject = NetworkSingleton<ManagerPhysicsObject>.Instance.NPOFromGO(gameObject);
			if (!networkPhysicsObject)
			{
				return;
			}
			base.RemoveObject(otherCollider, networkPhysicsObject);
			return;
		}
	}

	// Token: 0x06000E19 RID: 3609 RVA: 0x0005A845 File Offset: 0x00058A45
	private void OnObjectPickup(NetworkPhysicsObject npo, PlayerState player)
	{
		if (!npo.IgnoresFogOfWar)
		{
			this._heldObjects.Add(npo);
		}
	}

	// Token: 0x06000E1A RID: 3610 RVA: 0x0005A85C File Offset: 0x00058A5C
	private void OnFogOfWarRevealerAdd(GameObject go)
	{
		if (go == null)
		{
			return;
		}
		NetworkPhysicsObject component = go.GetComponent<NetworkPhysicsObject>();
		if (component == null)
		{
			return;
		}
		go.GetComponent<FogOfWarRevealer>().Active = true;
		go.GetComponent<FogOfWarRevealer>().Color = "All";
		if (this._hiddenObjects.Contains(component))
		{
			this._hiddenObjects.Remove(component);
			component.SetInvisible(this._hider, false, 2147483647U, false, false);
		}
		if (this._revealedObjects.ContainsKey(component))
		{
			this._revealedObjects.Remove(component);
		}
		if (!this._revealerObjects.ContainsKey(component))
		{
			this._revealerObjects.Add(component, go.transform);
		}
	}

	// Token: 0x06000E1B RID: 3611 RVA: 0x0005A90C File Offset: 0x00058B0C
	private void OnFogOfWarRevealerDestroy(GameObject go)
	{
		NetworkPhysicsObject component = go.GetComponent<NetworkPhysicsObject>();
		if (!component)
		{
			return;
		}
		this._revealerObjects.Remove(component);
		go.GetComponent<FogOfWarRevealer>().Active = false;
	}

	// Token: 0x06000E1C RID: 3612 RVA: 0x0005A942 File Offset: 0x00058B42
	private void OnPlayerConnect(NetworkPlayer networkPlayer)
	{
		this.SyncFogOfWar(networkPlayer);
	}

	// Token: 0x06000E1D RID: 3613 RVA: 0x0005A94C File Offset: 0x00058B4C
	private void OnPlayerChangeColor(PlayerState playerChanging)
	{
		PlayerState playerState = NetworkSingleton<PlayerManager>.Instance.MyPlayerState();
		if ((playerChanging == playerState && playerChanging.stringColor != this._previousColor) || (playerChanging.team == playerState.team && !playerState.team.Equals(Team.None)))
		{
			Debug.Log("FogOfWar: OnPlayerChangeColor");
			this.PlayerChangeColorOrTeam(NetworkSingleton<PlayerManager>.Instance.MyPlayerState());
		}
		if (playerChanging != playerState)
		{
			return;
		}
		if (playerChanging.stringColor != "Grey")
		{
			return;
		}
		this.SwitchToGrey();
		this.TurnOffGMHighlighting();
		this._previousColor = "Grey";
	}

	// Token: 0x06000E1E RID: 3614 RVA: 0x0005A9EE File Offset: 0x00058BEE
	private void OnObjectTagsChange(NetworkPhysicsObject npo, List<ulong> oldTags, List<ulong> newTags)
	{
		this._updateAllObjects = true;
	}

	// Token: 0x06000E1F RID: 3615 RVA: 0x0005A9F8 File Offset: 0x00058BF8
	private void SwitchToGrey()
	{
		foreach (KeyValuePair<NetworkPhysicsObject, List<string>> keyValuePair in this._revealedObjects)
		{
			if (!(keyValuePair.Key == null))
			{
				NetworkPhysicsObject key = keyValuePair.Key;
				if (!key.IgnoresFogOfWar && base.NPO.TagsAllowActingUpon(key) && !keyValuePair.Value.Contains("All"))
				{
					key.SetInvisible(this._hider, true, 2147483647U, false, false);
				}
			}
		}
		for (int i = 0; i < this._fogColor.Count; i++)
		{
			Color value = this._fogColor[i];
			value.a = 1f;
			this._fogColor[i] = value;
		}
		foreach (KeyValuePair<string, HashSet<int>> keyValuePair2 in this.RevealedLocations)
		{
			if (keyValuePair2.Key == "All")
			{
				foreach (int index in keyValuePair2.Value)
				{
					Color value2 = this._fogColor[index];
					value2.a = 0f;
					this._fogColor[index] = value2;
				}
			}
			this._indices.Clear();
			foreach (int item in keyValuePair2.Value)
			{
				this._indices.Add(item);
			}
			FogOfWarAntiAliasing.AntiAliasing(this._fogColor, 2f, this._indices, base.gameObject.transform.localScale, 2);
		}
	}

	// Token: 0x06000E20 RID: 3616 RVA: 0x0005AC14 File Offset: 0x00058E14
	private void TurnOffGMHighlighting()
	{
		for (int i = 0; i < this._hiddenObjects.Count; i++)
		{
			NetworkPhysicsObject networkPhysicsObject = this._hiddenObjects[i];
			if (networkPhysicsObject != null && networkPhysicsObject.IsHeldByNobody)
			{
				networkPhysicsObject.HighlightOff();
			}
		}
	}

	// Token: 0x06000E21 RID: 3617 RVA: 0x0005AC5C File Offset: 0x00058E5C
	private void OnPlayerChangeTeam(bool join, int id)
	{
		PlayerState playerState = NetworkSingleton<PlayerManager>.Instance.MyPlayerState();
		if (NetworkSingleton<PlayerManager>.Instance.PlayerStateFromID(id) != playerState && playerState.team.Equals(Team.None))
		{
			return;
		}
		this.PlayerChangeColorOrTeam(NetworkSingleton<PlayerManager>.Instance.MyPlayerState());
	}

	// Token: 0x06000E22 RID: 3618 RVA: 0x0005ACB0 File Offset: 0x00058EB0
	public void RemoveObjectHiding(NetworkPhysicsObject npo)
	{
		this.RevealGameObject(npo, false, "All");
		if (this._revealedObjects.ContainsKey(npo))
		{
			this._revealedObjects.Remove(npo);
		}
		if (Colour.MyColorLabel() == "Black")
		{
			npo.HighlightOff();
		}
	}

	// Token: 0x17000221 RID: 545
	// (get) Token: 0x06000E23 RID: 3619 RVA: 0x0005ACFD File Offset: 0x00058EFD
	// (set) Token: 0x06000E24 RID: 3620 RVA: 0x0005AD05 File Offset: 0x00058F05
	[Sync(Permission.Client, null, SerializationMethod.Default, false)]
	public bool HideGmPointer
	{
		get
		{
			return this._hideGmPointer;
		}
		set
		{
			if (value == this._hideGmPointer)
			{
				return;
			}
			this._hideGmPointer = value;
			if (!value)
			{
				this.RevealGMPointer();
			}
			base.DirtySync("HideGmPointer");
		}
	}

	// Token: 0x06000E25 RID: 3621 RVA: 0x0005AD2C File Offset: 0x00058F2C
	private void RevealGMPointer()
	{
		foreach (NetworkPhysicsObject networkPhysicsObject in this._hiddenObjects)
		{
			if (networkPhysicsObject.CompareTag("Pointer") && !(networkPhysicsObject.GetComponent<Pointer>().PointerColorLabel != "Black"))
			{
				this._hiddenObjects.Remove(networkPhysicsObject);
				networkPhysicsObject.GetComponent<Pointer>().ReferenceFollower.GetComponent<MeshLerpToObject>().IsInvisible = false;
			}
		}
	}

	// Token: 0x17000222 RID: 546
	// (get) Token: 0x06000E26 RID: 3622 RVA: 0x0005ADC0 File Offset: 0x00058FC0
	// (set) Token: 0x06000E27 RID: 3623 RVA: 0x0005ADC8 File Offset: 0x00058FC8
	[Sync(Permission.Client, null, SerializationMethod.Default, false)]
	public bool HideObjects
	{
		get
		{
			return this._hideObjects;
		}
		set
		{
			if (value == this._hideObjects)
			{
				return;
			}
			this._hideObjects = value;
			if (!value)
			{
				using (List<NetworkPhysicsObject>.Enumerator enumerator = this._hiddenObjects.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						NetworkPhysicsObject npo = enumerator.Current;
						this.RevealGameObject(npo, true, "All");
					}
					goto IL_B4;
				}
			}
			foreach (NetworkPhysicsObject networkPhysicsObject in this._hiddenObjects)
			{
				if (!networkPhysicsObject || !base.NPO.TagsAllowActingUpon(networkPhysicsObject))
				{
					return;
				}
				if (networkPhysicsObject.IsGrabbable)
				{
					networkPhysicsObject.SetInvisible(this._hider, true, 2147483647U, false, false);
				}
			}
			IL_B4:
			base.DirtySync("HideObjects");
		}
	}

	// Token: 0x17000223 RID: 547
	// (get) Token: 0x06000E28 RID: 3624 RVA: 0x0005AEB0 File Offset: 0x000590B0
	// (set) Token: 0x06000E29 RID: 3625 RVA: 0x0005AEB8 File Offset: 0x000590B8
	[Sync(Permission.Client, null, SerializationMethod.Default, false)]
	public bool ReHideObjects
	{
		get
		{
			return this._reHideObjects;
		}
		set
		{
			if (value == this._reHideObjects)
			{
				return;
			}
			this._reHideObjects = value;
			base.DirtySync("ReHideObjects");
		}
	}

	// Token: 0x17000224 RID: 548
	// (get) Token: 0x06000E2A RID: 3626 RVA: 0x0005AED6 File Offset: 0x000590D6
	// (set) Token: 0x06000E2B RID: 3627 RVA: 0x0005AEDE File Offset: 0x000590DE
	[Sync(Permission.Client, null, SerializationMethod.Default, false)]
	public bool GreyOut
	{
		get
		{
			return this._greyOut;
		}
		set
		{
			if (value == this._greyOut)
			{
				return;
			}
			this._greyOut = value;
			base.DirtySync("GreyOut");
		}
	}

	// Token: 0x17000225 RID: 549
	// (get) Token: 0x06000E2C RID: 3628 RVA: 0x0005AEFC File Offset: 0x000590FC
	// (set) Token: 0x06000E2D RID: 3629 RVA: 0x0005AF0C File Offset: 0x0005910C
	[Sync(Permission.Client, null, SerializationMethod.Default, false)]
	public float FogHeight
	{
		get
		{
			return this._fogHeight + 0.49f;
		}
		set
		{
			if ((double)Math.Abs(value - 0.49f - this._fogHeight) <= 0.001)
			{
				return;
			}
			this._fogHeight = value - 0.49f;
			this.Plane.transform.localPosition = new Vector3(-0.5f, this._fogHeight, -0.5f);
			this.AdjustWalls();
			base.DirtySync("FogHeight");
		}
	}

	// Token: 0x06000E2E RID: 3630 RVA: 0x0005AF7C File Offset: 0x0005917C
	private void BuildPlaneMesh()
	{
		FogOfWarMeshBuilder.BuildPlaneMesh(base.gameObject.transform.localScale, this.Plane, 2f, out this._verticesTransformed);
		this.AdjustWalls();
	}

	// Token: 0x06000E2F RID: 3631 RVA: 0x0005AFAC File Offset: 0x000591AC
	private void AdjustWalls()
	{
		foreach (GameObject gameObject in this.Walls.GetAllChildren())
		{
			Vector3 localScale = gameObject.transform.localScale;
			localScale.z = this.FogHeight;
			gameObject.transform.localScale = localScale;
			Vector3 localPosition = this.Walls.transform.localPosition;
			localPosition.y = this.Plane.transform.localPosition.y - 0.5f * this.FogHeight;
			this.Walls.transform.localPosition = localPosition;
		}
	}

	// Token: 0x06000E30 RID: 3632 RVA: 0x0005B06C File Offset: 0x0005926C
	private void RevealFogOfWar(Vector3 position, Vector3 heading, FogOfWarRevealer revealer)
	{
		if (base.NPO.hasTags)
		{
			NetworkPhysicsObject component = revealer.GetComponent<NetworkPhysicsObject>();
			if (component == null || !base.NPO.TagsAllowActingUpon(component))
			{
				return;
			}
		}
		Vector3 position2 = position;
		position.y = 100f;
		if (!this.RevealedLocations.ContainsKey(revealer.Color))
		{
			this.RevealedLocations.Add(revealer.Color, new HashSet<int>());
		}
		List<Vector3> list = new List<Vector3>();
		if (this._hideObjects)
		{
			for (int i = 0; i < 10; i++)
			{
				int num = 17;
				if (i == 9)
				{
					num = 119;
				}
				else if (i == 0)
				{
					num = 1;
				}
				for (int j = 0; j <= num; j++)
				{
					float num2 = (float)j * revealer.FoV / (float)num - revealer.FoV / 2f - heading.y + revealer.FoVOffset;
					int num3 = i * 10;
					Vector3 direction = new Vector3(Mathf.Cos(num2 * 0.017453292f) * Mathf.Sin((float)num3 * 0.017453292f), -Mathf.Cos((float)num3 * 0.017453292f), Mathf.Sin(num2 * 0.017453292f) * Mathf.Sin((float)num3 * 0.017453292f));
					int num4;
					if (i == 10)
					{
						num4 = Physics.RaycastNonAlloc(new Ray(position2, direction), FogOfWarZone.raycastHits, revealer.Range, this.VisionLayer, QueryTriggerInteraction.Collide);
					}
					else
					{
						Vector3 origin = new Vector3(position2.x, this.Plane.transform.position.y + revealer.Height, position2.z);
						num4 = Physics.RaycastNonAlloc(new Ray(origin, direction), FogOfWarZone.raycastHits, 1.1f * revealer.Range, this.VisionLayer, QueryTriggerInteraction.Collide);
					}
					this._hits.Clear();
					for (int k = 0; k < num4; k++)
					{
						RaycastHit item = FogOfWarZone.raycastHits[k];
						if (Vector3.Distance(new Vector3(position2.x, this.Plane.transform.position.y + revealer.Height, position2.z), item.point) <= revealer.Range)
						{
							this._hits.Add(item);
						}
					}
					this._hits.Sort((RaycastHit d1, RaycastHit d2) => d1.distance.CompareTo(d2.distance));
					bool flag = false;
					foreach (RaycastHit raycastHit in this._hits)
					{
						NetworkPhysicsObject networkPhysicsObject = NetworkSingleton<ManagerPhysicsObject>.Instance.NPOFromCollider(raycastHit.collider);
						if (networkPhysicsObject)
						{
							if (this.IsObjectRevealedToColour(networkPhysicsObject, revealer.Color))
							{
								if (i == 9)
								{
									list.Add(raycastHit.point);
									flag = true;
									break;
								}
								break;
							}
							else if (this.RevealGameObject(networkPhysicsObject, true, revealer.Color))
							{
								if (i == 9)
								{
									list.Add(raycastHit.point);
									flag = true;
									break;
								}
								break;
							}
						}
					}
					if (i == 9 && !flag)
					{
						list.Add(position2 + direction.normalized * revealer.Range);
					}
				}
			}
		}
		Vector3 position3 = new Vector3(position2.x, this.Plane.transform.position.y + revealer.Height, position2.z);
		list = FogOfWarPolygon.CleanPolygonData(list);
		if (revealer.FoV > 359f && revealer.ShowFoWOutline && FogOfWarZone.IsColorImpactingMe(revealer.Color))
		{
			revealer.ShowOutline(list, position3, revealer.Color);
		}
		list.Add(position2);
		list.Insert(0, position2);
		if (revealer.FoV <= 359f && revealer.ShowFoWOutline && FogOfWarZone.IsColorImpactingMe(revealer.Color))
		{
			revealer.ShowOutline(list, position3, revealer.Color);
		}
		Vector3[] polygonArray = list.ToArray();
		Vector3 scale = base.gameObject.transform.localScale;
		this._vertexTasks.Add(Task.Run(() => this.RevealVertices(position, revealer.Range, polygonArray, revealer.Color, scale)));
	}

	// Token: 0x06000E31 RID: 3633 RVA: 0x0005B544 File Offset: 0x00059744
	private Task RevealVertices(Vector3 position, float radius, Vector3[] polygonArray, string color, Vector3 scale)
	{
		List<int> list = new List<int>();
		for (int i = 0; i < this._verticesTransformed.Length; i++)
		{
			Vector3 vector = this._verticesTransformed[i];
			vector.y = position.y;
			if (Vector3.Distance(vector, position) <= radius && FogOfWarPolygon.IsPointInPolygon(vector, polygonArray))
			{
				if (FogOfWarZone.IsColorImpactingMe(color))
				{
					Color value = this._fogColor[i];
					value.a = 0f;
					this._fogColor[i] = value;
				}
				list.Add(i);
				this.RevealedLocations[color].Add(i);
			}
		}
		this._aaData.Add(new AntiAliasingData(color, scale, list));
		return Task.CompletedTask;
	}

	// Token: 0x06000E32 RID: 3634 RVA: 0x0005B600 File Offset: 0x00059800
	private void PlayerChangeColorOrTeam(PlayerState playerState)
	{
		foreach (KeyValuePair<NetworkPhysicsObject, List<string>> keyValuePair in this._revealedObjects)
		{
			if (!(keyValuePair.Key == null))
			{
				NetworkPhysicsObject key = keyValuePair.Key;
				bool flag = key.IgnoresFogOfWar || !base.NPO.TagsAllowActingUpon(key) || this.IsObjectRevealedToColour(keyValuePair.Key, Colour.MyColorLabel());
				key.SetInvisible(this._hider, !flag, 2147483647U, false, false);
			}
		}
		foreach (NetworkPhysicsObject networkPhysicsObject in this._revealerObjects.Keys)
		{
			FogOfWarRevealer component = networkPhysicsObject.GetComponent<FogOfWarRevealer>();
			if (!FogOfWarZone.IsColorImpactingMe(component.Color))
			{
				component.HideOutline();
			}
		}
		for (int i = 0; i < this._fogColor.Count; i++)
		{
			Color value = this._fogColor[i];
			value.a = ((playerState.stringColor == "Black") ? 0.45f : 1f);
			this._fogColor[i] = value;
		}
		if (playerState.stringColor == "Black")
		{
			for (int j = 0; j < this._hiddenObjects.Count; j++)
			{
				NetworkPhysicsObject networkPhysicsObject2 = this._hiddenObjects[j];
				if (networkPhysicsObject2 != null)
				{
					networkPhysicsObject2.HighlightOn(Color.gray);
				}
			}
		}
		else
		{
			this.TurnOffGMHighlighting();
		}
		foreach (KeyValuePair<string, HashSet<int>> keyValuePair2 in this.RevealedLocations)
		{
			if (keyValuePair2.Key == playerState.stringColor || NetworkSingleton<PlayerManager>.Instance.SameTeam(keyValuePair2.Key, -1) || keyValuePair2.Key == "All")
			{
				foreach (int index in keyValuePair2.Value)
				{
					Color value2 = this._fogColor[index];
					value2.a = 0f;
					this._fogColor[index] = value2;
				}
			}
			this._indices.Clear();
			foreach (int item in keyValuePair2.Value)
			{
				this._indices.Add(item);
			}
			FogOfWarAntiAliasing.AntiAliasing(this._fogColor, 2f, this._indices, base.gameObject.transform.localScale, 2);
		}
		this._previousColor = Colour.MyColorLabel();
	}

	// Token: 0x06000E33 RID: 3635 RVA: 0x0005B964 File Offset: 0x00059B64
	private bool IsObjectRevealedToColour(NetworkPhysicsObject npo, string colour)
	{
		if (!this._revealedObjects.ContainsKey(npo))
		{
			return false;
		}
		List<PlayerState> playersList = NetworkSingleton<PlayerManager>.Instance.PlayersList;
		PlayerState playerState = playersList.FirstOrDefault((PlayerState ps) => ps.stringColor == colour);
		using (List<string>.Enumerator enumerator = this._revealedObjects[npo].GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				string color = enumerator.Current;
				if (color == colour || color == "All")
				{
					return true;
				}
				PlayerState playerState2 = playersList.FirstOrDefault((PlayerState ps) => ps.stringColor == color);
				if (playerState != null && playerState2 != null && !playerState.team.Equals(Team.None) && playerState2.team == playerState.team)
				{
					return true;
				}
			}
		}
		return false;
	}

	// Token: 0x06000E34 RID: 3636 RVA: 0x0005BA84 File Offset: 0x00059C84
	private void HideGameObject(NetworkPhysicsObject npo)
	{
		if (!this.ValidateAddObject(npo))
		{
			return;
		}
		if (!npo || (this._revealedObjects.ContainsKey(npo) && this.IsObjectRevealedToColour(npo, Colour.MyColorLabel())) || this._hiddenObjects.Contains(npo))
		{
			return;
		}
		this._hiddenObjects.Add(npo);
		npo.SetInvisible(this._hider, true, 2147483647U, false, false);
		if (Colour.MyColorLabel() == "Black")
		{
			npo.HighlightOn(Color.gray);
		}
	}

	// Token: 0x06000E35 RID: 3637 RVA: 0x0005BB0C File Offset: 0x00059D0C
	private bool RevealGameObject(NetworkPhysicsObject npo, bool inZone, string color)
	{
		if (!npo)
		{
			return false;
		}
		if (!this._hiddenObjects.Contains(npo) && this.IsObjectRevealedToColour(npo, color))
		{
			return true;
		}
		if (npo.CompareTag("Board"))
		{
			return false;
		}
		if (!npo.IsGrabbable)
		{
			return false;
		}
		if (this._hideObjects)
		{
			if (inZone)
			{
				if (!this._revealedObjects.ContainsKey(npo))
				{
					List<string> value = new List<string>
					{
						color
					};
					this._revealedObjects.Add(npo, value);
				}
				else
				{
					this._revealedObjects[npo].Add(color);
				}
			}
			else if (this._revealedObjects.ContainsKey(npo))
			{
				this._revealedObjects.Remove(npo);
			}
			this._hiddenObjects.Remove(npo);
			if (npo.IsHeldByNobody)
			{
				npo.HighlightOff();
			}
		}
		if (Colour.MyColorLabel() != "Black" && color != "All" && !NetworkSingleton<PlayerManager>.Instance.SameTeam(color, -1) && color != Colour.MyColorLabel())
		{
			return false;
		}
		if (this._hider != null)
		{
			npo.SetInvisible(this._hider, false, 2147483647U, false, false);
		}
		else
		{
			Wait.Condition(delegate
			{
				npo.SetInvisible(this._hider, false, 2147483647U, false, false);
			}, () => this._hider != null, 60f, delegate
			{
				Chat.LogError("Unable to hide object (" + npo.GUID + ")", true);
			});
		}
		return true;
	}

	// Token: 0x06000E36 RID: 3638 RVA: 0x0005BCBC File Offset: 0x00059EBC
	private void GetRevealerObjects()
	{
		foreach (FogOfWarRevealer fogOfWarRevealer in UnityEngine.Object.FindObjectsOfType<FogOfWarRevealer>())
		{
			if (fogOfWarRevealer.Active)
			{
				this._revealerObjects.Add(fogOfWarRevealer.gameObject.GetComponentInChildren<NetworkPhysicsObject>(), fogOfWarRevealer.gameObject.transform);
			}
		}
	}

	// Token: 0x06000E37 RID: 3639 RVA: 0x0005BD0A File Offset: 0x00059F0A
	private void LoadFogOfWar()
	{
		Wait.Condition(new Action(this.LoadFogOfWarAction), () => this._meshBuilt && this._triggerLoaded, float.PositiveInfinity, null);
	}

	// Token: 0x06000E38 RID: 3640 RVA: 0x0005BD30 File Offset: 0x00059F30
	private void LoadFogOfWarAction()
	{
		bool flag = false;
		foreach (KeyValuePair<string, HashSet<int>> keyValuePair in this.RevealedLocations)
		{
			using (HashSet<int>.Enumerator enumerator2 = keyValuePair.Value.GetEnumerator())
			{
				while (enumerator2.MoveNext())
				{
					if (enumerator2.Current > this._fogColor.Count)
					{
						flag = true;
						break;
					}
				}
			}
			if (flag)
			{
				this.RevealedLocations.Clear();
				Chat.LogError("FoW is outdated. Resetting FoW. Sorry!", true);
				break;
			}
		}
		foreach (KeyValuePair<string, HashSet<int>> keyValuePair2 in this.RevealedLocations)
		{
			List<int> list = new List<int>();
			foreach (int num in keyValuePair2.Value)
			{
				if (FogOfWarZone.IsColorImpactingMe(keyValuePair2.Key))
				{
					Color value = this._fogColor[num];
					value.a = 0f;
					this._fogColor[num] = value;
				}
				list.Add(num);
				foreach (int num2 in FogOfWarAntiAliasing.GetNeighborIndices(this._fogColor, 2f, num, base.gameObject.transform.localScale))
				{
					if (Math.Abs(this._fogColor[num2].a - 1f) < 0.01f)
					{
						list.Add(num2);
					}
				}
			}
			this._indices.Clear();
			foreach (int item in keyValuePair2.Value)
			{
				this._indices.Add(item);
			}
			FogOfWarAntiAliasing.AntiAliasing(this._fogColor, 2f, this._indices, base.gameObject.transform.localScale, 2);
			foreach (int num3 in list)
			{
				Vector3 origin = this.Plane.transform.TransformPoint(this._vertices[num3]);
				origin.y = 100f;
				int num4 = Physics.RaycastNonAlloc(new Ray(origin, Vector3.down), FogOfWarZone.raycastHits, float.PositiveInfinity, this.VisionLayer, QueryTriggerInteraction.Collide);
				for (int i = 0; i < num4; i++)
				{
					NetworkPhysicsObject networkPhysicsObject = NetworkSingleton<ManagerPhysicsObject>.Instance.NPOFromCollider(FogOfWarZone.raycastHits[i].collider);
					if (networkPhysicsObject)
					{
						this.RevealGameObject(networkPhysicsObject, true, keyValuePair2.Key);
					}
				}
			}
		}
		this._loadingComplete = true;
		this.SyncFogOfWar(RPCTarget.Others);
	}

	// Token: 0x06000E39 RID: 3641 RVA: 0x0005C0F4 File Offset: 0x0005A2F4
	public void ResetFogOfWar()
	{
		Debug.Log("FogOfWar: ResetFogOfWar");
		base.networkView.RPC(RPCTarget.All, new Action(this.RPCResetFogOfWar));
	}

	// Token: 0x06000E3A RID: 3642 RVA: 0x0005C118 File Offset: 0x0005A318
	[Remote(Permission.Client, SendType.ReliableBuffered, null, SerializationMethod.Default)]
	private void RPCResetFogOfWar()
	{
		Debug.Log("FogOfWar: RPCResetFogOfWar");
		if (this._loadingComplete)
		{
			this.RevealedLocations = new Dictionary<string, HashSet<int>>();
		}
		this._revealerObjects = new Dictionary<NetworkPhysicsObject, Transform>();
		this._revealedObjects = new Dictionary<NetworkPhysicsObject, List<string>>();
		this._hiddenObjects = new List<NetworkPhysicsObject>();
		this._fogColor = new List<Color>();
		for (int i = 0; i < this._vertices.Length; i++)
		{
			this._fogColor.Add(Color.black);
		}
		this._mesh.SetColors(this._fogColor);
		this.GetRevealerObjects();
		base.ResetZone();
	}

	// Token: 0x06000E3B RID: 3643 RVA: 0x0005C1B0 File Offset: 0x0005A3B0
	[Remote(Permission.Client, SendType.ReliableBuffered, null, SerializationMethod.Default)]
	private void RPCForceSyncFogOfWar()
	{
		Debug.Log("FogOfWar: RPCForceSyncFogOfWar");
		this._revealerObjects = new Dictionary<NetworkPhysicsObject, Transform>();
		this._revealedObjects = new Dictionary<NetworkPhysicsObject, List<string>>();
		this._hiddenObjects = new List<NetworkPhysicsObject>();
		this._fogColor = new List<Color>();
		for (int i = 0; i < this._vertices.Length; i++)
		{
			this._fogColor.Add(Color.black);
		}
		this._mesh.SetColors(this._fogColor);
		this.GetRevealerObjects();
		base.ResetZone();
		this.SyncFogOfWar(RPCTarget.All);
	}

	// Token: 0x06000E3C RID: 3644 RVA: 0x0005C23C File Offset: 0x0005A43C
	private void SyncFogOfWar(RPCTarget target)
	{
		if (this.RevealedLocations.Count <= 0)
		{
			return;
		}
		if (!Network.isServer)
		{
			return;
		}
		int arg;
		int arg2;
		Dictionary<string, HashSet<int>> arg3;
		this.CreateFoWSyncData(out arg, out arg2, out arg3);
		base.networkView.RPC<Dictionary<string, HashSet<int>>, int, int>(target, new Action<Dictionary<string, HashSet<int>>, int, int>(this.RPCRevealLocations), arg3, arg, arg2);
	}

	// Token: 0x06000E3D RID: 3645 RVA: 0x0005C288 File Offset: 0x0005A488
	private void SyncFogOfWar(NetworkPlayer player)
	{
		if (this.RevealedLocations.Count <= 0)
		{
			return;
		}
		if (!Network.isServer)
		{
			return;
		}
		int arg;
		int arg2;
		Dictionary<string, HashSet<int>> arg3;
		this.CreateFoWSyncData(out arg, out arg2, out arg3);
		base.networkView.RPC<Dictionary<string, HashSet<int>>, int, int>(player, new Action<Dictionary<string, HashSet<int>>, int, int>(this.RPCRevealLocations), arg3, arg, arg2);
	}

	// Token: 0x06000E3E RID: 3646 RVA: 0x0005C2D4 File Offset: 0x0005A4D4
	private void CreateFoWSyncData(out int sizeX, out int sizeZ, out Dictionary<string, HashSet<int>> revealedLocations)
	{
		Vector3 localScale = base.gameObject.transform.localScale;
		sizeX = Mathf.FloorToInt(localScale.x * 2f);
		sizeZ = Mathf.FloorToInt(localScale.z * 2f);
		revealedLocations = this.RevealedLocations;
	}

	// Token: 0x06000E3F RID: 3647 RVA: 0x0005C320 File Offset: 0x0005A520
	[Remote(SerializationMethod.Json)]
	private void RPCRevealLocations(Dictionary<string, HashSet<int>> revealedLocations, int sizeX, int sizeZ)
	{
		Wait.Condition(delegate
		{
			this.RPCRevealLocationsAction(revealedLocations);
		}, this.RPCRevealLocationsCondition(sizeX, sizeZ), float.PositiveInfinity, null);
	}

	// Token: 0x06000E40 RID: 3648 RVA: 0x0005C354 File Offset: 0x0005A554
	private void RPCRevealLocationsAction(Dictionary<string, HashSet<int>> revealedLocations)
	{
		Wait.Frames(delegate
		{
			this.RevealedLocations = revealedLocations;
			this.LoadFogOfWar();
		}, 1);
	}

	// Token: 0x06000E41 RID: 3649 RVA: 0x0005C37C File Offset: 0x0005A57C
	private Func<bool> RPCRevealLocationsCondition(int sizeX, int sizeZ)
	{
		Vector3 localScale = base.gameObject.transform.localScale;
		int cX = Mathf.FloorToInt(localScale.x * 2f);
		int cZ = Mathf.FloorToInt(localScale.z * 2f);
		return () => this._meshBuilt && this._triggerLoaded && cX == sizeX && cZ == sizeZ;
	}

	// Token: 0x0400091C RID: 2332
	public GameObject Plane;

	// Token: 0x0400091D RID: 2333
	public LayerMask FogLayer;

	// Token: 0x0400091E RID: 2334
	public LayerMask VisionLayer;

	// Token: 0x0400091F RID: 2335
	public GameObject Walls;

	// Token: 0x04000920 RID: 2336
	public Dictionary<string, HashSet<int>> RevealedLocations = new Dictionary<string, HashSet<int>>();

	// Token: 0x04000921 RID: 2337
	private readonly List<int> _indices = new List<int>();

	// Token: 0x04000922 RID: 2338
	public const float Resolution = 2f;

	// Token: 0x04000923 RID: 2339
	private readonly List<NetworkPhysicsObject> _workNPOs = new List<NetworkPhysicsObject>();

	// Token: 0x04000924 RID: 2340
	private readonly List<NetworkPhysicsObject> _nullNPOs = new List<NetworkPhysicsObject>();

	// Token: 0x04000925 RID: 2341
	private Dictionary<NetworkPhysicsObject, Transform> _revealerObjects = new Dictionary<NetworkPhysicsObject, Transform>();

	// Token: 0x04000926 RID: 2342
	private Dictionary<NetworkPhysicsObject, List<string>> _revealedObjects = new Dictionary<NetworkPhysicsObject, List<string>>();

	// Token: 0x04000927 RID: 2343
	private readonly List<NetworkPhysicsObject> _clonedRevealedObjects = new List<NetworkPhysicsObject>();

	// Token: 0x04000928 RID: 2344
	private readonly List<NetworkPhysicsObject> _heldObjects = new List<NetworkPhysicsObject>();

	// Token: 0x04000929 RID: 2345
	private List<NetworkPhysicsObject> _hiddenObjects = new List<NetworkPhysicsObject>();

	// Token: 0x0400092A RID: 2346
	private readonly List<RaycastHit> _hits = new List<RaycastHit>();

	// Token: 0x0400092B RID: 2347
	private string _previousColor;

	// Token: 0x0400092C RID: 2348
	private Mesh _mesh;

	// Token: 0x0400092D RID: 2349
	private Vector3[] _vertices;

	// Token: 0x0400092E RID: 2350
	private Vector3[] _verticesTransformed;

	// Token: 0x0400092F RID: 2351
	private List<Color> _fogColor = new List<Color>();

	// Token: 0x04000930 RID: 2352
	private readonly List<Task> _vertexTasks = new List<Task>();

	// Token: 0x04000931 RID: 2353
	private readonly List<Task> _aaTasks = new List<Task>();

	// Token: 0x04000932 RID: 2354
	private readonly List<AntiAliasingData> _aaData = new List<AntiAliasingData>();

	// Token: 0x04000933 RID: 2355
	private Vector3 _lastScale;

	// Token: 0x04000934 RID: 2356
	private string _hider;

	// Token: 0x04000935 RID: 2357
	private bool _hideGmPointer = true;

	// Token: 0x04000936 RID: 2358
	private bool _hideObjects = true;

	// Token: 0x04000937 RID: 2359
	private bool _reHideObjects;

	// Token: 0x04000938 RID: 2360
	private bool _greyOut;

	// Token: 0x04000939 RID: 2361
	private float _fogHeight = -0.49f;

	// Token: 0x0400093A RID: 2362
	private bool _loadingComplete;

	// Token: 0x0400093B RID: 2363
	private bool _meshBuilt;

	// Token: 0x0400093C RID: 2364
	private bool _triggerLoaded;

	// Token: 0x0400093D RID: 2365
	private bool _updateAllObjects;

	// Token: 0x0400093E RID: 2366
	private static readonly RaycastHit[] raycastHits = new RaycastHit[50];

	// Token: 0x0400093F RID: 2367
	private readonly List<Pointer> pointersInZone = new List<Pointer>();

	// Token: 0x04000940 RID: 2368
	private int _counter;
}
