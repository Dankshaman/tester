using System;
using NewNet;
using UI.Xml;
using UnityEngine;

// Token: 0x020001D6 RID: 470
public class PermissionsOptions : NetworkSingleton<PermissionsOptions>
{
	// Token: 0x170003DF RID: 991
	// (get) Token: 0x06001865 RID: 6245 RVA: 0x000A608E File Offset: 0x000A428E
	// (set) Token: 0x06001866 RID: 6246 RVA: 0x000A609A File Offset: 0x000A429A
	public static PermissionsOptions.Options options
	{
		get
		{
			return NetworkSingleton<PermissionsOptions>.Instance.opt;
		}
		set
		{
			NetworkSingleton<PermissionsOptions>.Instance.opt = value;
		}
	}

	// Token: 0x170003E0 RID: 992
	// (get) Token: 0x06001867 RID: 6247 RVA: 0x000A60A7 File Offset: 0x000A42A7
	// (set) Token: 0x06001868 RID: 6248 RVA: 0x000A60B0 File Offset: 0x000A42B0
	[Sync(Permission.Admin, null, SerializationMethod.Default, false)]
	private PermissionsOptions.Options opt
	{
		get
		{
			return this._opt;
		}
		set
		{
			if (this._opt != value)
			{
				this._opt = value;
				if (!Network.isSenderOther && Network.isServer)
				{
					PlayerPrefs.SetString("HostPermissions", Json.GetJson(value, true));
				}
				PermissionsOptions.TriggerPermissionUpdated();
				base.DirtySync("opt");
			}
		}
	}

	// Token: 0x06001869 RID: 6249 RVA: 0x000A60FC File Offset: 0x000A42FC
	protected override void Awake()
	{
		base.Awake();
		this.AddValidateFunctions();
		if (PlayerPrefs.HasKey("HostPermissions"))
		{
			PermissionsOptions.options = Json.Load<PermissionsOptions.Options>(PlayerPrefs.GetString("HostPermissions"));
		}
	}

	// Token: 0x0600186A RID: 6250 RVA: 0x000A612C File Offset: 0x000A432C
	private void AddValidateFunctions()
	{
		VarInfo[] vars = typeof(PermissionsOptions.Options).GetVars();
		for (int i = 0; i < vars.Length; i++)
		{
			VarInfo var = vars[i];
			BaseNetworkAttribute.validationFunctions.SetValue("Permissions/" + var.Name, (NetworkPlayer player) => player.isAdmin || (bool)var.GetValue(PermissionsOptions.options));
		}
	}

	// Token: 0x0600186B RID: 6251 RVA: 0x000A6191 File Offset: 0x000A4391
	public void Reset()
	{
		PermissionsOptions.options = new PermissionsOptions.Options();
	}

	// Token: 0x0600186C RID: 6252 RVA: 0x000A61A0 File Offset: 0x000A43A0
	public static bool CheckIfAllowedInMode(PointerMode PM, int ID = -1)
	{
		if (NetworkSingleton<PlayerManager>.Instance.IsAdmin(ID))
		{
			return true;
		}
		if (Pointer.IsVectorTool(PM))
		{
			return PermissionsOptions.options.Drawing;
		}
		if (Pointer.IsCombineTool(PM))
		{
			return PermissionsOptions.options.Combining;
		}
		if (PM <= PointerMode.Hands)
		{
			switch (PM)
			{
			case PointerMode.Grab:
				return true;
			case PointerMode.Paint:
			case PointerMode.Erase:
			case (PointerMode)6:
			case PointerMode.Snap:
				return false;
			case PointerMode.Hidden:
			case PointerMode.Randomize:
				break;
			case PointerMode.Line:
				return PermissionsOptions.options.Line;
			case PointerMode.Flick:
				return PermissionsOptions.options.Flick;
			default:
				if (PM == PointerMode.Text)
				{
					return PermissionsOptions.options.Notes;
				}
				if (PM != PointerMode.Hands)
				{
					return false;
				}
				break;
			}
		}
		else
		{
			if (PM == PointerMode.Decal)
			{
				return PermissionsOptions.options.Decals;
			}
			if (PM != PointerMode.FogOfWar && PM != PointerMode.LayoutZone)
			{
				return false;
			}
		}
		return PermissionsOptions.options.Zones;
	}

	// Token: 0x0600186D RID: 6253 RVA: 0x000A6268 File Offset: 0x000A4468
	public static bool CheckAllowSender(bool check)
	{
		return PermissionsOptions.CheckAllow(check, (int)Network.sender.id);
	}

	// Token: 0x0600186E RID: 6254 RVA: 0x000A6288 File Offset: 0x000A4488
	public static bool CheckAllow(bool check, int ID = -1)
	{
		return check || NetworkSingleton<PlayerManager>.Instance.IsAdmin(ID);
	}

	// Token: 0x0600186F RID: 6255 RVA: 0x000A629A File Offset: 0x000A449A
	public static void BroadcastPermissionWarning(string action)
	{
		UIBroadcast.Log("You do not have permission to " + action + ".", Colour.Red, 2f, 3f);
	}

	// Token: 0x14000056 RID: 86
	// (add) Token: 0x06001870 RID: 6256 RVA: 0x000A62C8 File Offset: 0x000A44C8
	// (remove) Token: 0x06001871 RID: 6257 RVA: 0x000A62FC File Offset: 0x000A44FC
	public static event PermissionsOptions.PermissionUpdated OnPermissionUpdated;

	// Token: 0x06001872 RID: 6258 RVA: 0x000A632F File Offset: 0x000A452F
	public static void TriggerPermissionUpdated()
	{
		PermissionsOptions.PermissionUpdated onPermissionUpdated = PermissionsOptions.OnPermissionUpdated;
		if (onPermissionUpdated == null)
		{
			return;
		}
		onPermissionUpdated();
	}

	// Token: 0x04000EA1 RID: 3745
	private PermissionsOptions.Options _opt = new PermissionsOptions.Options();

	// Token: 0x04000EA2 RID: 3746
	private const string HostPermissionPrefs = "HostPermissions";

	// Token: 0x020006B2 RID: 1714
	public class Options
	{
		// Token: 0x040028F4 RID: 10484
		public bool TableFlip = true;

		// Token: 0x040028F5 RID: 10485
		public bool Contextual = true;

		// Token: 0x040028F6 RID: 10486
		public bool Scaling = true;

		// Token: 0x040028F7 RID: 10487
		public bool ChangeColor = true;

		// Token: 0x040028F8 RID: 10488
		public bool Locking = true;

		// Token: 0x040028F9 RID: 10489
		public bool Notes = true;

		// Token: 0x040028FA RID: 10490
		public bool Zones = true;

		// Token: 0x040028FB RID: 10491
		public bool Drawing = true;

		// Token: 0x040028FC RID: 10492
		public bool Flick = true;

		// Token: 0x040028FD RID: 10493
		public bool Digital = true;

		// Token: 0x040028FE RID: 10494
		public bool Combining = true;

		// Token: 0x040028FF RID: 10495
		public bool ChangeTeam = true;

		// Token: 0x04002900 RID: 10496
		public bool Tablets = true;

		// Token: 0x04002901 RID: 10497
		public bool Music;

		// Token: 0x04002902 RID: 10498
		public bool Saving = true;

		// Token: 0x04002903 RID: 10499
		public bool Peeking = true;

		// Token: 0x04002904 RID: 10500
		public bool Nudging = true;

		// Token: 0x04002905 RID: 10501
		public bool Decals = true;

		// Token: 0x04002906 RID: 10502
		public bool Line = true;

		// Token: 0x04002907 RID: 10503
		public bool NotebookEdit = true;
	}

	// Token: 0x020006B3 RID: 1715
	// (Invoke) Token: 0x06003C47 RID: 15431
	public delegate void PermissionUpdated();
}
