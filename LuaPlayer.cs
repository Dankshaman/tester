using System;
using System.Collections.Generic;
using MoonSharp.Interpreter;
using NewNet;
using UnityEngine;

// Token: 0x0200018A RID: 394
public class LuaPlayer
{
	// Token: 0x0600133A RID: 4922 RVA: 0x00081424 File Offset: 0x0007F624
	static LuaPlayer()
	{
		foreach (string text in Colour.HandPlayerLabels)
		{
			LuaPlayer.handPlayers[text] = new LuaPlayer(text);
		}
	}

	// Token: 0x0600133B RID: 4923 RVA: 0x00081464 File Offset: 0x0007F664
	public static LuaPlayer GetHandPlayer(string colorLabel)
	{
		LuaPlayer result;
		if (colorLabel == null || !LuaPlayer.handPlayers.TryGetValue(colorLabel, out result))
		{
			return null;
		}
		return result;
	}

	// Token: 0x0600133C RID: 4924 RVA: 0x00081486 File Offset: 0x0007F686
	[MoonSharpHidden]
	public LuaPlayer(string string_color)
	{
		this.color = string_color;
		this.playerColor = new Color?(Colour.ColourFromLabel(string_color));
	}

	// Token: 0x0600133D RID: 4925 RVA: 0x000814BD File Offset: 0x0007F6BD
	[MoonSharpHidden]
	public LuaPlayer(string string_color, int id)
	{
		this.overrideId = id;
		this.color = string_color;
		this.playerColor = new Color?(Colour.ColourFromLabel(string_color));
	}

	// Token: 0x17000338 RID: 824
	// (get) Token: 0x0600133E RID: 4926 RVA: 0x000814FB File Offset: 0x0007F6FB
	public bool seated
	{
		get
		{
			if (this.overrideId != -1)
			{
				return true;
			}
			if (NetworkSingleton<NetworkUI>.Instance.bHotseat)
			{
				return NetworkSingleton<PlayerManager>.Instance.ColourInUse(this.color);
			}
			return this.pointer != null;
		}
	}

	// Token: 0x17000339 RID: 825
	// (get) Token: 0x0600133F RID: 4927 RVA: 0x00081534 File Offset: 0x0007F734
	// (set) Token: 0x06001340 RID: 4928 RVA: 0x0008156C File Offset: 0x0007F76C
	public float lift_height
	{
		get
		{
			if (!this.seated)
			{
				return -1f;
			}
			Pointer pointer = this.pointer;
			if (pointer != null)
			{
				return pointer.RaiseHeight;
			}
			return -1f;
		}
		set
		{
			if (!this.seated)
			{
				return;
			}
			Pointer pointer = this.pointer;
			if (pointer != null)
			{
				pointer.RaiseHeight = value;
				return;
			}
		}
	}

	// Token: 0x1700033A RID: 826
	// (get) Token: 0x06001341 RID: 4929 RVA: 0x0008159C File Offset: 0x0007F79C
	public string steam_name
	{
		get
		{
			if (!this.seated)
			{
				return null;
			}
			if (this.overrideId != -1)
			{
				return NetworkSingleton<PlayerManager>.Instance.NameFromID(this.overrideId);
			}
			string result = "";
			try
			{
				result = NetworkSingleton<PlayerManager>.Instance.NameFromColour(this.playerColor.Value);
			}
			catch (Exception ex)
			{
				Debug.LogError("Error in Player.steam_name (probably color isn't seated/doesn't exist): " + ex.Message);
				return null;
			}
			return result;
		}
	}

	// Token: 0x1700033B RID: 827
	// (get) Token: 0x06001342 RID: 4930 RVA: 0x00081620 File Offset: 0x0007F820
	public string steam_id
	{
		get
		{
			if (!this.seated)
			{
				return null;
			}
			if (this.overrideId != -1)
			{
				return NetworkSingleton<PlayerManager>.Instance.SteamIDFromID(this.overrideId);
			}
			string result = "";
			try
			{
				result = NetworkSingleton<PlayerManager>.Instance.SteamIDFromID(NetworkSingleton<PlayerManager>.Instance.IDFromColour(this.playerColor.Value));
			}
			catch (Exception ex)
			{
				Debug.LogError("Error in Player.steam_id (probably color isn't seated/doesn't exist): " + ex.Message);
				return null;
			}
			return result;
		}
	}

	// Token: 0x1700033C RID: 828
	// (get) Token: 0x06001343 RID: 4931 RVA: 0x000816AC File Offset: 0x0007F8AC
	[MoonSharpHidden]
	public int id
	{
		get
		{
			if (!this.seated)
			{
				return -1;
			}
			if (this.overrideId != -1)
			{
				return this.overrideId;
			}
			if (this.playerColor != null)
			{
				return NetworkSingleton<PlayerManager>.Instance.IDFromColour(this.playerColor.Value);
			}
			return -1;
		}
	}

	// Token: 0x1700033D RID: 829
	// (get) Token: 0x06001344 RID: 4932 RVA: 0x000816FC File Offset: 0x0007F8FC
	// (set) Token: 0x06001345 RID: 4933 RVA: 0x0008173A File Offset: 0x0007F93A
	public bool? blindfolded
	{
		get
		{
			if (!this.seated)
			{
				return null;
			}
			return new bool?(NetworkSingleton<PlayerManager>.Instance.ObjectFromID(this.id).GetComponent<UINameButton>().bBlind);
		}
		set
		{
			if (!this.seated)
			{
				return;
			}
			if (value.Value)
			{
				this.Blind();
				return;
			}
			this.Unblind();
		}
	}

	// Token: 0x1700033E RID: 830
	// (get) Token: 0x06001346 RID: 4934 RVA: 0x0008175D File Offset: 0x0007F95D
	[MoonSharpHidden]
	private Pointer pointer
	{
		get
		{
			return NetworkSingleton<ManagerPhysicsObject>.Instance.PointerFromColorLabel(this.color);
		}
	}

	// Token: 0x1700033F RID: 831
	// (get) Token: 0x06001347 RID: 4935 RVA: 0x00081770 File Offset: 0x0007F970
	// (set) Token: 0x06001348 RID: 4936 RVA: 0x000817B0 File Offset: 0x0007F9B0
	public bool? promoted
	{
		get
		{
			if (!this.seated)
			{
				return null;
			}
			return new bool?(NetworkSingleton<PlayerManager>.Instance.PlayersDictionary[this.id].promoted);
		}
		set
		{
			if (!this.seated)
			{
				return;
			}
			bool? flag = value;
			bool promoted = NetworkSingleton<PlayerManager>.Instance.PlayersDictionary[this.id].promoted;
			if (!(flag.GetValueOrDefault() == promoted & flag != null))
			{
				NetworkSingleton<PlayerManager>.Instance.PromoteThisPlayer(this.steam_name);
			}
		}
	}

	// Token: 0x17000340 RID: 832
	// (get) Token: 0x06001349 RID: 4937 RVA: 0x00081808 File Offset: 0x0007FA08
	public bool? host
	{
		get
		{
			if (!this.seated)
			{
				return null;
			}
			return new bool?(this.id == 1);
		}
	}

	// Token: 0x17000341 RID: 833
	// (get) Token: 0x0600134A RID: 4938 RVA: 0x00081838 File Offset: 0x0007FA38
	public bool? admin
	{
		get
		{
			if (!this.seated)
			{
				return null;
			}
			return new bool?(NetworkSingleton<PlayerManager>.Instance.IsAdmin(this.id));
		}
	}

	// Token: 0x17000342 RID: 834
	// (get) Token: 0x0600134B RID: 4939 RVA: 0x0008186C File Offset: 0x0007FA6C
	// (set) Token: 0x0600134C RID: 4940 RVA: 0x000818AB File Offset: 0x0007FAAB
	public string team
	{
		get
		{
			if (!this.seated)
			{
				return null;
			}
			return NetworkSingleton<PlayerManager>.Instance.PlayersDictionary[this.id].team.ToString();
		}
		set
		{
			this.ChangeTeam(value);
		}
	}

	// Token: 0x17000343 RID: 835
	// (get) Token: 0x0600134D RID: 4941 RVA: 0x000818B8 File Offset: 0x0007FAB8
	public string hand_choice_label
	{
		get
		{
			if (!this.seated)
			{
				return null;
			}
			HandZone handZone = HandZone.GetHandZone(this.color, 0, true);
			if (handZone && handZone.HasQueuedHandSelectMode)
			{
				return handZone.HandSelectModeSettingsQueue[0].label;
			}
			return null;
		}
	}

	// Token: 0x0600134E RID: 4942 RVA: 0x00081900 File Offset: 0x0007FB00
	public bool PingTable(Vector3 position)
	{
		this.pointer.SpawnArrow(new Vector3(position.x, position.y + 1.5f, position.z));
		return true;
	}

	// Token: 0x0600134F RID: 4943 RVA: 0x0008192C File Offset: 0x0007FB2C
	public Vector3? GetPointerPosition()
	{
		if (this.pointer == null)
		{
			return null;
		}
		return new Vector3?(this.pointer.gameObject.transform.position);
	}

	// Token: 0x06001350 RID: 4944 RVA: 0x0008196C File Offset: 0x0007FB6C
	public float? GetPointerRotation()
	{
		if (this.pointer == null)
		{
			return null;
		}
		return new float?(this.pointer.gameObject.transform.localRotation.eulerAngles.y);
	}

	// Token: 0x06001351 RID: 4945 RVA: 0x000819B8 File Offset: 0x0007FBB8
	public int GetHandCount()
	{
		return HandZone.GetHandCount(this.color);
	}

	// Token: 0x06001352 RID: 4946 RVA: 0x000819C8 File Offset: 0x0007FBC8
	public LuaTransform GetHandTransform(int index = 1)
	{
		GameObject hand = HandZone.GetHand(this.color, index - 1);
		if (hand == null)
		{
			return null;
		}
		return new LuaTransform(hand.transform);
	}

	// Token: 0x06001353 RID: 4947 RVA: 0x000819FC File Offset: 0x0007FBFC
	public bool SetHandTransform(LuaTransform transform, int index = 1)
	{
		GameObject hand = HandZone.GetHand(this.color, index - 1);
		if (hand == null || transform == null)
		{
			return false;
		}
		hand.transform.position = transform.position;
		hand.transform.eulerAngles = transform.rotation;
		hand.GetComponent<NetworkPhysicsObject>().SetScale(transform.scale, false);
		return true;
	}

	// Token: 0x06001354 RID: 4948 RVA: 0x00081A5C File Offset: 0x0007FC5C
	[Obsolete("Use getHandTransform() instead")]
	public Table GetPlayerHand(Script script)
	{
		Table table = new Table(script);
		GameObject hand = HandZone.GetHand(this.color, 0);
		if (hand == null)
		{
			return null;
		}
		Vector3 worldPos = new Vector3(hand.transform.position.x, hand.transform.position.y, hand.transform.position.z);
		Vector3 vector = NetworkSingleton<ManagerPhysicsObject>.Instance.SurfacePointBelowWorldPos(worldPos);
		Vector3 eulerAngles = hand.transform.rotation.eulerAngles;
		table["pos_x"] = vector.x;
		table["pos_y"] = vector.y;
		table["pos_z"] = vector.z;
		table["rot_x"] = eulerAngles.x;
		table["rot_y"] = eulerAngles.y;
		table["rot_z"] = eulerAngles.z;
		table["trigger_forward_x"] = hand.transform.forward.normalized.x;
		table["trigger_forward_y"] = hand.transform.forward.normalized.y;
		table["trigger_forward_z"] = hand.transform.forward.normalized.z;
		table["trigger_right_x"] = hand.transform.right.normalized.x;
		table["trigger_right_y"] = hand.transform.right.normalized.y;
		table["trigger_right_z"] = hand.transform.right.normalized.z;
		table["trigger_up_x"] = hand.transform.up.normalized.x;
		table["trigger_up_y"] = hand.transform.up.normalized.y;
		table["trigger_up_z"] = hand.transform.up.normalized.z;
		return table;
	}

	// Token: 0x06001355 RID: 4949 RVA: 0x00081CDA File Offset: 0x0007FEDA
	public List<LuaGameObjectScript> GetHandObjects(int index = 1)
	{
		return HandZone.GetHandZone(this.color, index - 1, false).GetHandObjects(false).ToLGOS();
	}

	// Token: 0x06001356 RID: 4950 RVA: 0x00081CF8 File Offset: 0x0007FEF8
	public LuaGameObjectScript GetHandStash()
	{
		HandZone handZone = HandZone.GetHandZone(this.color, 0, true);
		if (!handZone || !handZone.Stash)
		{
			return null;
		}
		return handZone.Stash.luaGameObjectScript;
	}

	// Token: 0x06001357 RID: 4951 RVA: 0x00081D38 File Offset: 0x0007FF38
	public bool DrawHandStash()
	{
		HandZone handZone = HandZone.GetHandZone(this.color, 0, true);
		if (!handZone || !handZone.Stash)
		{
			return false;
		}
		handZone.MoveStashToHand();
		return true;
	}

	// Token: 0x06001358 RID: 4952 RVA: 0x00081D74 File Offset: 0x0007FF74
	public bool SetHandStashLocation(Vector3? position, int rotation)
	{
		HandZone handZone = HandZone.GetHandZone(this.color, 0, true);
		if (!handZone)
		{
			return false;
		}
		if (position != null)
		{
			position = new Vector3?(position.Value.Clamp(Vector3.one * -1f, Vector3.one));
		}
		handZone.SetStashLocation(position, rotation);
		return true;
	}

	// Token: 0x06001359 RID: 4953 RVA: 0x00081DD4 File Offset: 0x0007FFD4
	public List<LuaGameObjectScript> GetSelectedObjects()
	{
		if (!this.pointer)
		{
			return null;
		}
		List<GameObject> selectedObjects = this.pointer.GetSelectedObjects(-1, true, false);
		List<LuaGameObjectScript> list = new List<LuaGameObjectScript>();
		foreach (GameObject gameObject in selectedObjects)
		{
			if (gameObject)
			{
				list.Add(gameObject.GetComponent<LuaGameObjectScript>());
			}
		}
		return list;
	}

	// Token: 0x0600135A RID: 4954 RVA: 0x00081E54 File Offset: 0x00080054
	public bool ClearSelectedObjects()
	{
		if (!this.pointer)
		{
			return false;
		}
		this.pointer.ResetHighlight();
		return true;
	}

	// Token: 0x0600135B RID: 4955 RVA: 0x00081E74 File Offset: 0x00080074
	public List<LuaGameObjectScript> GetHoldingObjects()
	{
		if (!this.pointer)
		{
			return null;
		}
		List<NetworkPhysicsObject> grabbableNPOs = NetworkSingleton<ManagerPhysicsObject>.Instance.GrabbableNPOs;
		List<LuaGameObjectScript> list = new List<LuaGameObjectScript>();
		foreach (NetworkPhysicsObject networkPhysicsObject in grabbableNPOs)
		{
			if (networkPhysicsObject.HeldByPlayerID == this.id)
			{
				list.Add(networkPhysicsObject.luaGameObjectScript);
			}
		}
		return list;
	}

	// Token: 0x0600135C RID: 4956 RVA: 0x00081EF4 File Offset: 0x000800F4
	public LuaGameObjectScript GetHoverObject()
	{
		if (!this.pointer)
		{
			return null;
		}
		NetworkView hoverView = this.pointer.HoverView;
		if (hoverView)
		{
			return hoverView.GetComponent<LuaGameObjectScript>();
		}
		return null;
	}

	// Token: 0x0600135D RID: 4957 RVA: 0x00081F2C File Offset: 0x0008012C
	public bool Blind()
	{
		if (this.id < 0)
		{
			return false;
		}
		if (this.blindfolded.Value)
		{
			return false;
		}
		NetworkSingleton<PlayerManager>.Instance.ChangeBlindfold(this.id, true);
		return true;
	}

	// Token: 0x0600135E RID: 4958 RVA: 0x00081F68 File Offset: 0x00080168
	public bool Unblind()
	{
		if (this.id < 0)
		{
			return false;
		}
		if (!this.blindfolded.Value)
		{
			return false;
		}
		NetworkSingleton<PlayerManager>.Instance.ChangeBlindfold(this.id, false);
		return true;
	}

	// Token: 0x0600135F RID: 4959 RVA: 0x00081FA4 File Offset: 0x000801A4
	public bool Copy(List<LuaGameObjectScript> objects)
	{
		if (this.pointer == null)
		{
			return false;
		}
		this.pointer.Copy(objects);
		return true;
	}

	// Token: 0x06001360 RID: 4960 RVA: 0x00081FC3 File Offset: 0x000801C3
	public bool Paste(Vector3 position)
	{
		if (this.pointer == null)
		{
			return false;
		}
		this.pointer.Paste(position, true, true, false);
		return true;
	}

	// Token: 0x06001361 RID: 4961 RVA: 0x00081FE8 File Offset: 0x000801E8
	public bool LookAt(LuaPlayer.LuaCameraParameters parameters)
	{
		if (!this.seated)
		{
			return false;
		}
		float min = 10f;
		if (parameters.position.y < -2f)
		{
			min = 10f * Mathf.Max(30f / parameters.distance, 1f);
		}
		parameters.pitch = CameraController.ClampAngle(parameters.pitch, min, 90f);
		parameters.yaw = CameraController.ClampAngle(parameters.yaw, 0f, 360f);
		List<float> list = new List<float>
		{
			parameters.position.x,
			parameters.position.y,
			parameters.position.z,
			parameters.distance,
			parameters.pitch,
			parameters.yaw
		};
		LuaGlobalScriptManager.Instance.networkView.RPC<float[]>(Network.IdToNetworkPlayer(this.id), new Action<float[]>(LuaGlobalScriptManager.Instance.RPCLookAt), list.ToArray());
		return true;
	}

	// Token: 0x06001362 RID: 4962 RVA: 0x000820F4 File Offset: 0x000802F4
	public bool AttachCameraToObject(LuaPlayer.LuaCameraParameters parameters)
	{
		if (!this.seated)
		{
			return false;
		}
		if (parameters.@object == null)
		{
			return false;
		}
		parameters.pitch = CameraController.ClampAngle(parameters.pitch, 0f, 360f);
		parameters.yaw = CameraController.ClampAngle(parameters.yaw, 0f, 360f);
		List<float> list = new List<float>
		{
			parameters.offset.x,
			parameters.offset.y,
			parameters.offset.z,
			parameters.pitch,
			parameters.yaw
		};
		LuaGlobalScriptManager.Instance.networkView.RPC<float[], int, bool>(Network.IdToNetworkPlayer(this.id), new Action<float[], int, bool>(LuaGlobalScriptManager.Instance.RPCAttachCameraToObject), list.ToArray(), parameters.@object.NPO.ID, parameters.rotate);
		return true;
	}

	// Token: 0x06001363 RID: 4963 RVA: 0x000821EC File Offset: 0x000803EC
	public bool SetCameraMode(string cameraMode)
	{
		if (!this.seated)
		{
			return false;
		}
		CameraController.CameraMode arg;
		if (Enum.TryParse<CameraController.CameraMode>(cameraMode, true, out arg))
		{
			LuaGlobalScriptManager.Instance.networkView.RPC<CameraController.CameraMode>(Network.IdToNetworkPlayer(this.id), new Action<CameraController.CameraMode>(LuaGlobalScriptManager.Instance.RPCSetCameraMode), arg);
			return true;
		}
		Chat.LogError("SetCameraMode: Unkown camera mode " + cameraMode, true);
		return false;
	}

	// Token: 0x06001364 RID: 4964 RVA: 0x0008224D File Offset: 0x0008044D
	public bool Kick()
	{
		if (!this.seated)
		{
			return false;
		}
		NetworkSingleton<PlayerManager>.Instance.KickThisPlayer(this.steam_name);
		return true;
	}

	// Token: 0x06001365 RID: 4965 RVA: 0x0008226A File Offset: 0x0008046A
	public bool Mute()
	{
		if (!this.seated)
		{
			return false;
		}
		NetworkSingleton<PlayerManager>.Instance.MuteThisPlayer(this.steam_name);
		return true;
	}

	// Token: 0x06001366 RID: 4966 RVA: 0x00082287 File Offset: 0x00080487
	public bool Promote()
	{
		if (!this.seated)
		{
			return false;
		}
		NetworkSingleton<PlayerManager>.Instance.PromoteThisPlayer(this.steam_name);
		return true;
	}

	// Token: 0x06001367 RID: 4967 RVA: 0x000822A4 File Offset: 0x000804A4
	public bool ChangeColor(string NewColor)
	{
		if (string.IsNullOrEmpty(NewColor))
		{
			return false;
		}
		if (!this.seated)
		{
			return false;
		}
		if (NetworkSingleton<PlayerManager>.Instance.ColourInUse(Colour.ColourFromLabel(NewColor)))
		{
			return false;
		}
		NetworkSingleton<NetworkUI>.Instance.CheckColor(NewColor, this.id);
		return true;
	}

	// Token: 0x06001368 RID: 4968 RVA: 0x000822E0 File Offset: 0x000804E0
	public bool ChangeTeam(string TeamName)
	{
		if (string.IsNullOrEmpty(TeamName))
		{
			return false;
		}
		if (!this.seated)
		{
			return false;
		}
		bool result;
		try
		{
			Team team = (Team)Enum.Parse(typeof(Team), TeamName);
			NetworkSingleton<PlayerManager>.Instance.ChangeTeam(this.id, team);
			result = true;
		}
		catch (Exception)
		{
			result = false;
		}
		return result;
	}

	// Token: 0x06001369 RID: 4969 RVA: 0x00082344 File Offset: 0x00080544
	public bool print(string Message)
	{
		return this.print(Message, Colour.UnityWhite);
	}

	// Token: 0x0600136A RID: 4970 RVA: 0x00082358 File Offset: 0x00080558
	public bool print(string Message, Color Color)
	{
		if (this.id < 0)
		{
			return false;
		}
		if (NetworkID.ID == this.id)
		{
			LuaGlobalScriptManager.Instance.RPCPrint(Message, Color, false);
			return true;
		}
		LuaGlobalScriptManager.Instance.networkView.RPC<string, Color, bool>(Network.IdToNetworkPlayer(this.id), new Action<string, Color, bool>(LuaGlobalScriptManager.Instance.RPCPrint), Message, Color, false);
		LuaGlobalScriptManager.Instance.PushLuaPrintMessage(Message);
		return true;
	}

	// Token: 0x0600136B RID: 4971 RVA: 0x000823C5 File Offset: 0x000805C5
	public bool broadcast(string Message)
	{
		return this.broadcast(Message, Colour.UnityWhite);
	}

	// Token: 0x0600136C RID: 4972 RVA: 0x000823D8 File Offset: 0x000805D8
	public bool broadcast(string Message, Color Color)
	{
		if (this.id < 0)
		{
			return false;
		}
		if (NetworkID.ID == this.id)
		{
			LuaGlobalScriptManager.Instance.RPCPrint(Message, Color, true);
			return true;
		}
		LuaGlobalScriptManager.Instance.networkView.RPC<string, Color, bool>(Network.IdToNetworkPlayer(this.id), new Action<string, Color, bool>(LuaGlobalScriptManager.Instance.RPCPrint), Message, Color, true);
		LuaGlobalScriptManager.Instance.PushLuaPrintMessage(Message);
		return true;
	}

	// Token: 0x0600136D RID: 4973 RVA: 0x00082445 File Offset: 0x00080645
	public bool SetUITheme(string themeScript)
	{
		return LuaGlobalScriptManager.Instance.SetUITheme(this.color, themeScript);
	}

	// Token: 0x0600136E RID: 4974 RVA: 0x00082458 File Offset: 0x00080658
	public bool ShowColorDialog(Closure function)
	{
		LuaPlayer.<>c__DisplayClass69_0 CS$<>8__locals1 = new LuaPlayer.<>c__DisplayClass69_0();
		CS$<>8__locals1.function = function;
		CS$<>8__locals1.<>4__this = this;
		if (!this.seated)
		{
			return false;
		}
		Singleton<UIColorPickerStandaloneDialog>.Instance.Show(Network.IdToNetworkPlayer(this.id), new Action<Color>(CS$<>8__locals1.<ShowColorDialog>g__callback|0));
		return true;
	}

	// Token: 0x0600136F RID: 4975 RVA: 0x000824A8 File Offset: 0x000806A8
	public bool ShowColorDialog(Color defaultColor, Closure function)
	{
		LuaPlayer.<>c__DisplayClass70_0 CS$<>8__locals1 = new LuaPlayer.<>c__DisplayClass70_0();
		CS$<>8__locals1.function = function;
		CS$<>8__locals1.<>4__this = this;
		if (!this.seated)
		{
			return false;
		}
		Singleton<UIColorPickerStandaloneDialog>.Instance.Show(Network.IdToNetworkPlayer(this.id), new Action<Color>(CS$<>8__locals1.<ShowColorDialog>g__callback|0), defaultColor);
		return true;
	}

	// Token: 0x06001370 RID: 4976 RVA: 0x000824F8 File Offset: 0x000806F8
	public bool ShowColorDialog(string defaultColor, Closure function)
	{
		Colour colour;
		if (Colour.TryColourFromLabel(defaultColor, out colour))
		{
			return this.ShowColorDialog(colour, function);
		}
		return this.ShowColorDialog(Colour.ColourFromRGBHex(defaultColor), function);
	}

	// Token: 0x06001371 RID: 4977 RVA: 0x0008252F File Offset: 0x0008072F
	public bool ShowInfoDialog(string info)
	{
		if (!this.seated)
		{
			return false;
		}
		UIDialog.ShowInfoForLua(Network.IdToNetworkPlayer(this.id), info);
		return true;
	}

	// Token: 0x06001372 RID: 4978 RVA: 0x00082550 File Offset: 0x00080750
	public bool ShowConfirmDialog(string info, Closure function)
	{
		if (!this.seated)
		{
			return false;
		}
		UIDialog.ShowConfirmForLua(Network.IdToNetworkPlayer(this.id), info, delegate
		{
			LuaScript.TryCall(function, new object[]
			{
				this.color
			});
		});
		return true;
	}

	// Token: 0x06001373 RID: 4979 RVA: 0x00082599 File Offset: 0x00080799
	public bool ShowInputDialog(Closure function)
	{
		return this.ShowInputDialog("Enter Text", function);
	}

	// Token: 0x06001374 RID: 4980 RVA: 0x000825A7 File Offset: 0x000807A7
	public bool ShowInputDialog(string description, Closure function)
	{
		return this.ShowInputDialog(description, "", function);
	}

	// Token: 0x06001375 RID: 4981 RVA: 0x000825B8 File Offset: 0x000807B8
	public bool ShowInputDialog(string description, string defaultText, Closure function)
	{
		if (!this.seated)
		{
			return false;
		}
		UIDialog.ShowInputForLua(Network.IdToNetworkPlayer(this.id), description, defaultText, delegate(string text)
		{
			LuaScript.TryCall(function, new object[]
			{
				text,
				this.color
			});
		});
		return true;
	}

	// Token: 0x06001376 RID: 4982 RVA: 0x00082602 File Offset: 0x00080802
	public bool ShowMemoDialog(Closure function)
	{
		return this.ShowMemoDialog("Enter Text", function);
	}

	// Token: 0x06001377 RID: 4983 RVA: 0x00082610 File Offset: 0x00080810
	public bool ShowMemoDialog(string description, Closure function)
	{
		return this.ShowMemoDialog(description, "", function);
	}

	// Token: 0x06001378 RID: 4984 RVA: 0x00082620 File Offset: 0x00080820
	public bool ShowMemoDialog(string description, string defaultText, Closure function)
	{
		if (!this.seated)
		{
			return false;
		}
		UIDialog.ShowMemoInputForLua(Network.IdToNetworkPlayer(this.id), description, defaultText, delegate(string text)
		{
			LuaScript.TryCall(function, new object[]
			{
				text,
				this.color
			});
		});
		return true;
	}

	// Token: 0x06001379 RID: 4985 RVA: 0x0008266A File Offset: 0x0008086A
	public bool ShowOptionsDialog(string description, List<string> dropDownOptions, Closure function)
	{
		return this.ShowOptionsDialog(description, dropDownOptions, dropDownOptions[0], function);
	}

	// Token: 0x0600137A RID: 4986 RVA: 0x0008267C File Offset: 0x0008087C
	public bool ShowOptionsDialog(string description, List<string> dropDownOptions, int defaultDropDownValue, Closure function)
	{
		defaultDropDownValue--;
		return defaultDropDownValue >= 0 && defaultDropDownValue < dropDownOptions.Count && this.ShowOptionsDialog(description, dropDownOptions, dropDownOptions[defaultDropDownValue], function);
	}

	// Token: 0x0600137B RID: 4987 RVA: 0x000826A4 File Offset: 0x000808A4
	public bool ShowOptionsDialog(string description, List<string> dropDownOptions, string defaultDropDownValue, Closure function)
	{
		if (!this.seated || dropDownOptions.Count == 0)
		{
			return false;
		}
		UIDialog.ShowDropDownForLua(Network.IdToNetworkPlayer(this.id), description, dropDownOptions, defaultDropDownValue, delegate(string text)
		{
			int num = dropDownOptions.Count - 1;
			while (num >= 0 && !(dropDownOptions[num] == text))
			{
				num--;
			}
			LuaScript.TryCall(function, new object[]
			{
				text,
				num + 1,
				this.color
			});
		});
		return true;
	}

	// Token: 0x04000BA3 RID: 2979
	private static readonly Dictionary<string, LuaPlayer> handPlayers = new Dictionary<string, LuaPlayer>();

	// Token: 0x04000BA4 RID: 2980
	public string color = "";

	// Token: 0x04000BA5 RID: 2981
	[MoonSharpHidden]
	public Color? playerColor;

	// Token: 0x04000BA6 RID: 2982
	[MoonSharpHidden]
	private int overrideId = -1;

	// Token: 0x02000666 RID: 1638
	public class LuaCameraParameters
	{
		// Token: 0x04002810 RID: 10256
		public Vector3 position = Vector3.zero;

		// Token: 0x04002811 RID: 10257
		public Vector3 offset = Vector3.zero;

		// Token: 0x04002812 RID: 10258
		public float pitch = -1f;

		// Token: 0x04002813 RID: 10259
		public float yaw = -1f;

		// Token: 0x04002814 RID: 10260
		public float distance = -1f;

		// Token: 0x04002815 RID: 10261
		public bool rotate;

		// Token: 0x04002816 RID: 10262
		public LuaGameObjectScript @object;
	}
}
