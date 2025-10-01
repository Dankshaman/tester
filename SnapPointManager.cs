using System;
using System.Collections.Generic;
using NewNet;
using UnityEngine;

// Token: 0x02000231 RID: 561
public class SnapPointManager : NetworkSingleton<SnapPointManager>
{
	// Token: 0x1700041F RID: 1055
	// (get) Token: 0x06001BCA RID: 7114 RVA: 0x000BEEFE File Offset: 0x000BD0FE
	// (set) Token: 0x06001BCB RID: 7115 RVA: 0x000BEF06 File Offset: 0x000BD106
	public bool IsInSnapTool { get; private set; }

	// Token: 0x06001BCC RID: 7116 RVA: 0x000BEF0F File Offset: 0x000BD10F
	private void Start()
	{
		EventManager.OnNetworkObjectDestroy += this.OnNetworkObjectDestroy;
		EventManager.OnResetTable += this.OnResetTable;
		NetworkEvents.OnPlayerConnected += this.OnPlayerConnect;
	}

	// Token: 0x06001BCD RID: 7117 RVA: 0x000BEF44 File Offset: 0x000BD144
	private void OnDestroy()
	{
		EventManager.OnNetworkObjectDestroy -= this.OnNetworkObjectDestroy;
		EventManager.OnResetTable -= this.OnResetTable;
		NetworkEvents.OnPlayerConnected -= this.OnPlayerConnect;
	}

	// Token: 0x06001BCE RID: 7118 RVA: 0x000BEF7C File Offset: 0x000BD17C
	private void LateUpdate()
	{
		bool flag = PlayerScript.PointerScript && Pointer.IsSnapTool(PlayerScript.PointerScript.CurrentPointerMode);
		if (flag != this.IsInSnapTool)
		{
			this.IsInSnapTool = flag;
			this.SetSnapPointVisibility(flag, null);
		}
		if (!flag)
		{
			return;
		}
		if (!UICamera.HoveredUIObject && zInput.GetButtonDown("Grab", ControlType.All))
		{
			this.CreateSnapPoint(HoverScript.PointerPosition);
		}
	}

	// Token: 0x06001BCF RID: 7119 RVA: 0x000BEFE8 File Offset: 0x000BD1E8
	public void GUIEditDefaultTags()
	{
		Singleton<UIComponentTagDialog>.Instance.EditFlags(ref this.DefaultTags);
	}

	// Token: 0x06001BD0 RID: 7120 RVA: 0x000BEFFC File Offset: 0x000BD1FC
	public bool CheckSnapPointMenuClick(Vector3 clickPosition)
	{
		if (HoverScript.HoverObject)
		{
			Vector3 position = HoverScript.HoverObject.transform.position;
			clickPosition = new Vector3(position.x, clickPosition.y, position.z);
			Vector3 eulerAngles = HoverScript.HoverObject.transform.eulerAngles;
			float num = VRHMD.isVR ? 0.1f : 0.01f;
			for (int i = 0; i < this.SnapPointObjects.Count; i++)
			{
				SnapPointManager.SnapPointObject snapPointObject = this.SnapPointObjects[i];
				if (Vector3.Distance(clickPosition, snapPointObject.snapPoint.transform.position) < num)
				{
					this.EditSnapPointTags(snapPointObject);
					return true;
				}
			}
		}
		return false;
	}

	// Token: 0x06001BD1 RID: 7121 RVA: 0x000BF0AC File Offset: 0x000BD2AC
	public void EditSnapPointTags(SnapPoint snapPoint)
	{
		SnapPointManager.SnapPointObject snapPointObject = this.SnapPointObjectFromSnapPoint(snapPoint);
		if (snapPointObject != null)
		{
			this.EditSnapPointTags(snapPointObject);
		}
	}

	// Token: 0x06001BD2 RID: 7122 RVA: 0x000BF0CB File Offset: 0x000BD2CB
	public void EditSnapPointTags(SnapPointManager.SnapPointObject snapPointObject)
	{
		Singleton<UIComponentTagDialog>.Instance.EditSnapPoint(snapPointObject);
	}

	// Token: 0x06001BD3 RID: 7123 RVA: 0x000BF0D8 File Offset: 0x000BD2D8
	public void CreateSnapPoint(Vector3 clickPosition)
	{
		Vector3 eulerAngles = new Vector3(0f, 180f, 0f);
		if (HoverScript.HoverObject)
		{
			Vector3 position = HoverScript.HoverObject.transform.position;
			clickPosition = new Vector3(position.x, clickPosition.y, position.z);
			eulerAngles = HoverScript.HoverObject.transform.eulerAngles;
			int i = 0;
			while (i < this.SnapPointObjects.Count)
			{
				SnapPointManager.SnapPointObject snapPointObject = this.SnapPointObjects[i];
				if (Vector3.Distance(clickPosition, snapPointObject.snapPoint.transform.position) < (VRHMD.isVR ? 0.1f : 0.01f))
				{
					this.RemoveSnapPoint(snapPointObject.snapPoint);
					if (VRHMD.isVR)
					{
						return;
					}
					break;
				}
				else
				{
					i++;
				}
			}
		}
		else if (VRHMD.isVR)
		{
			int j = 0;
			while (j < this.SnapPointObjects.Count)
			{
				SnapPointManager.SnapPointObject snapPointObject2 = this.SnapPointObjects[j];
				if (Vector3.Distance(clickPosition, snapPointObject2.snapPoint.transform.position) < 0.1f)
				{
					this.RemoveSnapPoint(snapPointObject2.snapPoint);
					if (VRHMD.isVR)
					{
						return;
					}
					break;
				}
				else
				{
					j++;
				}
			}
		}
		Vector3 position2 = clickPosition;
		Vector3? rotation = null;
		NetworkView arg = null;
		if (PlayerScript.PointerScript.CurrentPointerMode == PointerMode.SnapRotate)
		{
			rotation = new Vector3?(eulerAngles);
		}
		if (HoverScript.SurfaceObject && HoverScript.SurfaceObject.GetComponent<NetworkPhysicsObject>() && HoverScript.SurfaceObject.GetComponent<NetworkPhysicsObject>().IsGrabbable)
		{
			arg = HoverScript.SurfaceObject.GetComponent<NetworkPhysicsObject>().networkView;
		}
		base.networkView.RPC<SnapPointManager.SnapPointData, NetworkView>(RPCTarget.All, new Action<SnapPointManager.SnapPointData, NetworkView>(this.RPCAddSnapPointWorld), new SnapPointManager.SnapPointData(position2, rotation, SnapPointManager.GetGuid(), this.DefaultTags), arg);
	}

	// Token: 0x06001BD4 RID: 7124 RVA: 0x000BF2A0 File Offset: 0x000BD4A0
	public void SetSnapPointVisibility(bool bShow, SnapPoint SnapObject = null)
	{
		if (VRHMD.isVR)
		{
			if (SnapObject)
			{
				SnapObject.GetComponent<Renderer>().enabled = bShow;
				return;
			}
			for (int i = 0; i < this.SnapPointObjects.Count; i++)
			{
				this.SnapPointObjects[i].snapPoint.GetComponent<Renderer>().enabled = bShow;
			}
			return;
		}
		else
		{
			if (bShow)
			{
				for (int j = NetworkSingleton<NetworkUI>.Instance.GUISnapDotPanel.transform.childCount; j < this.SnapPointObjects.Count; j++)
				{
					UIWidget[] componentsInChildren = NetworkSingleton<NetworkUI>.Instance.GUISnapDotPanel.AddChild(NetworkSingleton<NetworkUI>.Instance.GUISnapDot).GetComponentsInChildren<UIWidget>(true);
					int num = j * 2;
					componentsInChildren[0].depth = num - 1;
					componentsInChildren[1].depth = num;
					componentsInChildren[2].depth = num;
					componentsInChildren[3].depth = num;
				}
				for (int k = 0; k < this.SnapPointObjects.Count; k++)
				{
					GameObject gameObject = NetworkSingleton<NetworkUI>.Instance.GUISnapDotPanel.transform.GetChild(k).gameObject;
					if (!SnapObject)
					{
						gameObject.GetComponent<UIAttachToObject>().AttachTo = this.SnapPointObjects[k].snapPoint.transform;
						gameObject.SetActive(true);
					}
					else if (!gameObject.activeSelf)
					{
						gameObject.GetComponent<UIAttachToObject>().AttachTo = SnapObject.transform;
						gameObject.SetActive(true);
						return;
					}
				}
				return;
			}
			for (int l = 0; l < NetworkSingleton<NetworkUI>.Instance.GUISnapDotPanel.transform.childCount; l++)
			{
				GameObject gameObject2 = NetworkSingleton<NetworkUI>.Instance.GUISnapDotPanel.transform.GetChild(l).gameObject;
				if (!SnapObject)
				{
					gameObject2.SetActive(false);
				}
				else if (gameObject2.activeSelf && gameObject2.GetComponent<UIAttachToObject>().AttachTo == SnapObject.transform)
				{
					gameObject2.SetActive(false);
					return;
				}
			}
			return;
		}
	}

	// Token: 0x06001BD5 RID: 7125 RVA: 0x000BF480 File Offset: 0x000BD680
	private void OnNetworkObjectDestroy(NetworkPhysicsObject NPO)
	{
		for (int i = this.SnapPointObjects.Count - 1; i >= 0; i--)
		{
			if (this.SnapPointObjects[i].parentNPO == NPO)
			{
				this.SnapPointObjects.RemoveAt(i);
			}
		}
	}

	// Token: 0x06001BD6 RID: 7126 RVA: 0x000BF4CA File Offset: 0x000BD6CA
	private void OnResetTable()
	{
		base.networkView.RPC<NetworkView>(RPCTarget.All, new Action<NetworkView>(this.RPCRemoveAllFromParent), null);
	}

	// Token: 0x06001BD7 RID: 7127 RVA: 0x000BF4E8 File Offset: 0x000BD6E8
	private void OnPlayerConnect(NetworkPlayer player)
	{
		for (int i = 0; i < this.SnapPointObjects.Count; i++)
		{
			SnapPointManager.SnapPointObject snapPointObject = this.SnapPointObjects[i];
			Vector3? rotation = null;
			if (snapPointObject.snapPoint.bRotate)
			{
				rotation = new Vector3?(snapPointObject.snapPoint.transform.localEulerAngles);
			}
			base.networkView.RPC<SnapPointManager.SnapPointData, NetworkView>(player, new Action<SnapPointManager.SnapPointData, NetworkView>(this.RPCAddSnapPointLocal), new SnapPointManager.SnapPointData(snapPointObject.snapPoint.transform.localPosition, rotation, snapPointObject.guid, snapPointObject.snapPoint.tags), snapPointObject.parent);
		}
	}

	// Token: 0x06001BD8 RID: 7128 RVA: 0x000BF590 File Offset: 0x000BD790
	public bool TagIsSet(int tagIndex)
	{
		for (int i = 0; i < this.SnapPointObjects.Count; i++)
		{
			if (ComponentTags.GetFlag(this.SnapPointObjects[i].snapPoint.tags, tagIndex))
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x06001BD9 RID: 7129 RVA: 0x000BF5D4 File Offset: 0x000BD7D4
	public void RemoveTagFromAllSnapPoints(int tagIndex)
	{
		for (int i = 0; i < this.SnapPointObjects.Count; i++)
		{
			ComponentTags.SetFlag(ref this.SnapPointObjects[i].snapPoint.tags, tagIndex, false);
		}
	}

	// Token: 0x06001BDA RID: 7130 RVA: 0x000BF614 File Offset: 0x000BD814
	public SnapPointManager.SnapPointObject SnapPointObjectFromSnapPoint(SnapPoint snapPoint)
	{
		for (int i = 0; i < this.SnapPointObjects.Count; i++)
		{
			if (this.SnapPointObjects[i].snapPoint == snapPoint)
			{
				return this.SnapPointObjects[i];
			}
		}
		return null;
	}

	// Token: 0x06001BDB RID: 7131 RVA: 0x000BF65E File Offset: 0x000BD85E
	public void AddSnapPoint(Vector3 position, Vector3? rotation, NetworkView parent, List<ulong> tags)
	{
		base.networkView.RPC<SnapPointManager.SnapPointData, NetworkView>(RPCTarget.All, new Action<SnapPointManager.SnapPointData, NetworkView>(this.RPCAddSnapPointLocal), new SnapPointManager.SnapPointData(position, rotation, SnapPointManager.GetGuid(), tags), parent);
	}

	// Token: 0x06001BDC RID: 7132 RVA: 0x000BF688 File Offset: 0x000BD888
	public void AddSnapPoints(List<SnapPointInfo> snapStates, NetworkView parent)
	{
		List<SnapPointManager.SnapPointData> list = new List<SnapPointManager.SnapPointData>(snapStates.Count);
		for (int i = 0; i < snapStates.Count; i++)
		{
			SnapPointInfo snapPointInfo = snapStates[i];
			Vector3? rotation = null;
			if (snapPointInfo.Rotation != null)
			{
				rotation = new Vector3?(snapPointInfo.Rotation.Value.ToVector());
			}
			list.Add(new SnapPointManager.SnapPointData(snapPointInfo.Position.ToVector(), rotation, SnapPointManager.GetGuid(), snapPointInfo.Tags));
		}
		base.networkView.RPC<List<SnapPointManager.SnapPointData>, NetworkView>(RPCTarget.All, new Action<List<SnapPointManager.SnapPointData>, NetworkView>(this.RPCAddSnapPoints), list, parent);
	}

	// Token: 0x06001BDD RID: 7133 RVA: 0x000BF728 File Offset: 0x000BD928
	public void AddSnapPoints(List<SnapPointState> snapStates, NetworkView parent)
	{
		List<SnapPointManager.SnapPointData> list = new List<SnapPointManager.SnapPointData>(snapStates.Count);
		for (int i = 0; i < snapStates.Count; i++)
		{
			SnapPointInfo snapPointInfo = new SnapPointInfo(snapStates[i]);
			Vector3? rotation = null;
			if (snapPointInfo.Rotation != null)
			{
				rotation = new Vector3?(snapPointInfo.Rotation.Value.ToVector());
			}
			list.Add(new SnapPointManager.SnapPointData(snapPointInfo.Position.ToVector(), rotation, SnapPointManager.GetGuid(), snapPointInfo.Tags));
		}
		base.networkView.RPC<List<SnapPointManager.SnapPointData>, NetworkView>(RPCTarget.All, new Action<List<SnapPointManager.SnapPointData>, NetworkView>(this.RPCAddSnapPoints), list, parent);
	}

	// Token: 0x06001BDE RID: 7134 RVA: 0x000BF7CC File Offset: 0x000BD9CC
	[Remote(Permission.Admin)]
	private void RPCAddSnapPoints(List<SnapPointManager.SnapPointData> snapDatas, NetworkView parent)
	{
		for (int i = 0; i < snapDatas.Count; i++)
		{
			this.RPCAddSnapPointLocal(snapDatas[i], parent);
		}
	}

	// Token: 0x06001BDF RID: 7135 RVA: 0x000BF7F8 File Offset: 0x000BD9F8
	[Remote(Permission.Admin)]
	private void RPCAddSnapPointLocal(SnapPointManager.SnapPointData snapData, NetworkView parent)
	{
		SnapPoint snapPoint = UnityEngine.Object.Instantiate<GameObject>(NetworkSingleton<GameMode>.Instance.SnapPoint, Vector3.zero, Quaternion.identity).GetComponent<SnapPoint>();
		if (parent)
		{
			snapPoint.transform.parent = parent.transform;
		}
		snapPoint.transform.localPosition = snapData.position;
		if (snapData.rotation != null)
		{
			snapPoint.transform.localEulerAngles = snapData.rotation.Value;
			snapPoint.bRotate = true;
		}
		snapPoint.tags = ComponentTags.NewCopyOfFlags(snapData.tags);
		this.SnapPointObjects.Add(new SnapPointManager.SnapPointObject(snapData.guid, snapPoint, parent));
		Wait.Frames(delegate
		{
			if (NetworkSingleton<SnapPointManager>.Instance.IsInSnapTool)
			{
				this.SetSnapPointVisibility(true, snapPoint);
			}
		}, 1);
	}

	// Token: 0x06001BE0 RID: 7136 RVA: 0x000BF8E8 File Offset: 0x000BDAE8
	[Remote(Permission.Admin)]
	private void RPCAddSnapPointWorld(SnapPointManager.SnapPointData snapData, NetworkView parent)
	{
		SnapPoint snapPoint = UnityEngine.Object.Instantiate<GameObject>(NetworkSingleton<GameMode>.Instance.SnapPoint, Vector3.zero, Quaternion.identity).GetComponent<SnapPoint>();
		if (parent)
		{
			snapPoint.transform.parent = parent.transform;
		}
		snapPoint.transform.position = snapData.position;
		if (snapData.rotation != null)
		{
			snapPoint.transform.eulerAngles = snapData.rotation.Value;
			snapPoint.bRotate = true;
		}
		snapPoint.tags = ComponentTags.NewCopyOfFlags(snapData.tags);
		this.SnapPointObjects.Add(new SnapPointManager.SnapPointObject(snapData.guid, snapPoint, parent));
		Wait.Frames(delegate
		{
			if (NetworkSingleton<SnapPointManager>.Instance.IsInSnapTool)
			{
				this.SetSnapPointVisibility(true, snapPoint);
			}
		}, 1);
	}

	// Token: 0x06001BE1 RID: 7137 RVA: 0x000BF9D8 File Offset: 0x000BDBD8
	public void RemoveSnapPoint(SnapPoint snapPoint)
	{
		for (int i = 0; i < this.SnapPointObjects.Count; i++)
		{
			SnapPointManager.SnapPointObject snapPointObject = this.SnapPointObjects[i];
			if (snapPointObject.snapPoint == snapPoint)
			{
				base.networkView.RPC<uint>(RPCTarget.All, new Action<uint>(this.RPCRemoveSnapPoint), snapPointObject.guid);
				return;
			}
		}
	}

	// Token: 0x06001BE2 RID: 7138 RVA: 0x000BFA38 File Offset: 0x000BDC38
	[Remote(Permission.Admin)]
	private void RPCRemoveSnapPoint(uint guid)
	{
		for (int i = this.SnapPointObjects.Count - 1; i >= 0; i--)
		{
			SnapPointManager.SnapPointObject snapPointObject = this.SnapPointObjects[i];
			if (snapPointObject.guid == guid)
			{
				UnityEngine.Object.Destroy(snapPointObject.snapPoint.gameObject);
				this.SnapPointObjects.RemoveAt(i);
				return;
			}
		}
	}

	// Token: 0x06001BE3 RID: 7139 RVA: 0x000BFA90 File Offset: 0x000BDC90
	public void SetSnapPoints(List<LuaGameObjectScript.LuaSnapPointParameters> snapPointList, NetworkView parent)
	{
		List<SnapPointManager.SnapPointData> list = new List<SnapPointManager.SnapPointData>(snapPointList.Count);
		for (int i = 0; i < snapPointList.Count; i++)
		{
			LuaGameObjectScript.LuaSnapPointParameters luaSnapPointParameters = snapPointList[i];
			Vector3? rotation = null;
			if (luaSnapPointParameters.rotation_snap)
			{
				rotation = new Vector3?(luaSnapPointParameters.rotation);
			}
			list.Add(new SnapPointManager.SnapPointData(luaSnapPointParameters.position, rotation, SnapPointManager.GetGuid(), NetworkSingleton<ComponentTags>.Instance.FlagsFromDisplayedTagLabels(luaSnapPointParameters.tags)));
		}
		base.networkView.RPC<List<SnapPointManager.SnapPointData>, NetworkView>(RPCTarget.All, new Action<List<SnapPointManager.SnapPointData>, NetworkView>(this.RPCSetSnapPoints), list, parent);
	}

	// Token: 0x06001BE4 RID: 7140 RVA: 0x000BFB20 File Offset: 0x000BDD20
	[Remote(Permission.Admin)]
	private void RPCSetSnapPoints(List<SnapPointManager.SnapPointData> snapDatas, NetworkView parent)
	{
		this.RPCRemoveAllFromParent(parent);
		this.RPCAddSnapPoints(snapDatas, parent);
	}

	// Token: 0x06001BE5 RID: 7141 RVA: 0x000BFB34 File Offset: 0x000BDD34
	public void UpdateSnapPoint(SnapPoint snapPoint)
	{
		for (int i = 0; i < this.SnapPointObjects.Count; i++)
		{
			SnapPointManager.SnapPointObject snapPointObject = this.SnapPointObjects[i];
			if (snapPointObject.snapPoint == snapPoint)
			{
				Vector3? rotation = null;
				if (snapPoint.bRotate)
				{
					rotation = new Vector3?(new Vector3(snapPoint.transform.localEulerAngles.x, snapPoint.transform.localEulerAngles.y, snapPoint.transform.localEulerAngles.z));
				}
				SnapPointManager.SnapPointData arg = new SnapPointManager.SnapPointData(snapPoint.transform.localPosition, rotation, snapPointObject.guid, snapPoint.tags);
				base.networkView.RPC<SnapPointManager.SnapPointData>(RPCTarget.All, new Action<SnapPointManager.SnapPointData>(this.RPCUpdateSnapPoint), arg);
				return;
			}
		}
	}

	// Token: 0x06001BE6 RID: 7142 RVA: 0x000BFC00 File Offset: 0x000BDE00
	[Remote(Permission.Admin)]
	private void RPCUpdateSnapPoint(SnapPointManager.SnapPointData snapPointData)
	{
		for (int i = 0; i < this.SnapPointObjects.Count; i++)
		{
			SnapPointManager.SnapPointObject snapPointObject = this.SnapPointObjects[i];
			if (snapPointData.guid == snapPointObject.guid)
			{
				snapPointObject.snapPoint.transform.localPosition = snapPointData.position;
				if (snapPointData.rotation != null)
				{
					snapPointObject.snapPoint.transform.localEulerAngles = snapPointData.rotation.Value;
				}
				snapPointObject.snapPoint.tags = snapPointData.tags;
				return;
			}
		}
	}

	// Token: 0x06001BE7 RID: 7143 RVA: 0x000BFC90 File Offset: 0x000BDE90
	public List<SnapPointInfo> GetSnapPointStates(NetworkView parent = null)
	{
		List<SnapPointInfo> list = null;
		for (int i = 0; i < this.SnapPointObjects.Count; i++)
		{
			SnapPointManager.SnapPointObject snapPointObject = this.SnapPointObjects[i];
			if (snapPointObject.parent == parent)
			{
				if (list == null)
				{
					list = new List<SnapPointInfo>();
				}
				VectorState? rotation = null;
				if (snapPointObject.snapPoint.bRotate)
				{
					rotation = new VectorState?(new VectorState(snapPointObject.snapPoint.transform.localEulerAngles));
				}
				list.Add(new SnapPointInfo
				{
					Position = new VectorState(snapPointObject.snapPoint.transform.localPosition),
					Rotation = rotation,
					Tags = snapPointObject.snapPoint.tags
				});
			}
		}
		return list;
	}

	// Token: 0x06001BE8 RID: 7144 RVA: 0x000BFD50 File Offset: 0x000BDF50
	public List<SnapPointState> GetSnapPointSaveStates(NetworkView parent = null)
	{
		List<SnapPointState> list = null;
		for (int i = 0; i < this.SnapPointObjects.Count; i++)
		{
			SnapPointManager.SnapPointObject snapPointObject = this.SnapPointObjects[i];
			if (snapPointObject.parent == parent)
			{
				if (list == null)
				{
					list = new List<SnapPointState>();
				}
				VectorState? rotation = null;
				if (snapPointObject.snapPoint.bRotate)
				{
					rotation = new VectorState?(new VectorState(snapPointObject.snapPoint.transform.localEulerAngles));
				}
				List<string> tags = null;
				if (snapPointObject.snapPoint.tags != null && snapPointObject.snapPoint.tags.Count > 0)
				{
					tags = NetworkSingleton<ComponentTags>.Instance.DisplayedTagLabelsFromTags(snapPointObject.snapPoint.tags);
				}
				list.Add(new SnapPointState
				{
					Position = new VectorState(snapPointObject.snapPoint.transform.localPosition),
					Rotation = rotation,
					Tags = tags
				});
			}
		}
		return list;
	}

	// Token: 0x06001BE9 RID: 7145 RVA: 0x000BFE43 File Offset: 0x000BE043
	public void GUIRemoveAllSnapPoints()
	{
		UIDialog.Show("Delete all snap points?", "Yes", "No", delegate()
		{
			base.networkView.RPC(RPCTarget.All, new Action(this.RPCRemoveAll));
		}, null);
	}

	// Token: 0x06001BEA RID: 7146 RVA: 0x000BFE68 File Offset: 0x000BE068
	[Remote(Permission.Admin)]
	private void RPCRemoveAll()
	{
		Chat.NotifyFromNetworkSender("has deleted all snap points");
		for (int i = this.SnapPointObjects.Count - 1; i >= 0; i--)
		{
			UnityEngine.Object.Destroy(this.SnapPointObjects[i].snapPoint.gameObject);
		}
		this.SnapPointObjects.Clear();
	}

	// Token: 0x06001BEB RID: 7147 RVA: 0x000BFEC0 File Offset: 0x000BE0C0
	[Remote(Permission.Admin)]
	private void RPCRemoveAllFromParent(NetworkView parent)
	{
		for (int i = this.SnapPointObjects.Count - 1; i >= 0; i--)
		{
			SnapPointManager.SnapPointObject snapPointObject = this.SnapPointObjects[i];
			if (snapPointObject.parent == parent)
			{
				UnityEngine.Object.Destroy(snapPointObject.snapPoint.gameObject);
				this.SnapPointObjects.RemoveAt(i);
			}
		}
	}

	// Token: 0x06001BEC RID: 7148 RVA: 0x0005215F File Offset: 0x0005035F
	private static uint GetGuid()
	{
		return Utilities.RandomUint();
	}

	// Token: 0x04001181 RID: 4481
	[NonSerialized]
	public List<SnapPointManager.SnapPointObject> SnapPointObjects = new List<SnapPointManager.SnapPointObject>();

	// Token: 0x04001182 RID: 4482
	[NonSerialized]
	public List<ulong> DefaultTags = new List<ulong>();

	// Token: 0x020006BF RID: 1727
	public class SnapPointObject
	{
		// Token: 0x170007DC RID: 2012
		// (get) Token: 0x06003C6E RID: 15470 RVA: 0x00179321 File Offset: 0x00177521
		// (set) Token: 0x06003C6F RID: 15471 RVA: 0x00179329 File Offset: 0x00177529
		public uint guid { get; private set; }

		// Token: 0x170007DD RID: 2013
		// (get) Token: 0x06003C70 RID: 15472 RVA: 0x00179332 File Offset: 0x00177532
		// (set) Token: 0x06003C71 RID: 15473 RVA: 0x0017933A File Offset: 0x0017753A
		public SnapPoint snapPoint { get; private set; }

		// Token: 0x170007DE RID: 2014
		// (get) Token: 0x06003C72 RID: 15474 RVA: 0x00179343 File Offset: 0x00177543
		// (set) Token: 0x06003C73 RID: 15475 RVA: 0x0017934B File Offset: 0x0017754B
		public NetworkView parent { get; private set; }

		// Token: 0x170007DF RID: 2015
		// (get) Token: 0x06003C74 RID: 15476 RVA: 0x00179354 File Offset: 0x00177554
		// (set) Token: 0x06003C75 RID: 15477 RVA: 0x0017935C File Offset: 0x0017755C
		public NetworkPhysicsObject parentNPO { get; private set; }

		// Token: 0x06003C76 RID: 15478 RVA: 0x00179365 File Offset: 0x00177565
		public SnapPointObject(uint guid, SnapPoint snapPoint, NetworkView parent)
		{
			this.guid = guid;
			this.snapPoint = snapPoint;
			this.parent = parent;
			if (parent)
			{
				this.parentNPO = parent.GetComponent<NetworkPhysicsObject>();
			}
		}
	}

	// Token: 0x020006C0 RID: 1728
	public struct SnapPointData
	{
		// Token: 0x06003C77 RID: 15479 RVA: 0x00179396 File Offset: 0x00177596
		public SnapPointData(Vector3 position, Vector3? rotation, uint guid, List<ulong> tags)
		{
			this.position = position;
			this.rotation = rotation;
			this.guid = guid;
			this.tags = ComponentTags.NewCopyOfFlags(tags);
		}

		// Token: 0x04002930 RID: 10544
		public Vector3 position;

		// Token: 0x04002931 RID: 10545
		public Vector3? rotation;

		// Token: 0x04002932 RID: 10546
		public uint guid;

		// Token: 0x04002933 RID: 10547
		public List<ulong> tags;
	}
}
