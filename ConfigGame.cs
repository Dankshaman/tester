using System;
using UnityEngine;

// Token: 0x020000C0 RID: 192
public class ConfigGame : Config<ConfigGame.ConfigGameState, ConfigGame>
{
	// Token: 0x170001B9 RID: 441
	// (get) Token: 0x060009CB RID: 2507 RVA: 0x000457E0 File Offset: 0x000439E0
	// (set) Token: 0x060009CC RID: 2508 RVA: 0x000457EC File Offset: 0x000439EC
	public static ConfigGame.ConfigGameState Settings
	{
		get
		{
			return Singleton<ConfigGame>.Instance.settings;
		}
		set
		{
			Singleton<ConfigGame>.Instance.settings = value;
		}
	}

	// Token: 0x060009CD RID: 2509 RVA: 0x000457F9 File Offset: 0x000439F9
	protected override void Awake()
	{
		base.Awake();
		if (this._settings == null)
		{
			this._settings = new ConfigGame.ConfigGameState();
		}
	}

	// Token: 0x060009CE RID: 2510 RVA: 0x00045814 File Offset: 0x00043A14
	protected override void OnSettingsChanged()
	{
		base.OnSettingsChanged();
		Camera[] componentsInChildren = HoverScript.MainCamera.GetComponentsInChildren<Camera>(true);
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			if (!componentsInChildren[i].GetComponent<ZoomCamera>())
			{
				componentsInChildren[i].fieldOfView = (float)ConfigGame.Settings.ConfigCamera.FOV;
			}
		}
		if (this.prevModLocation == null)
		{
			this.prevModLocation = new ConfigGame.ModLocation?(ConfigGame.Settings.ConfigMods.Location);
			return;
		}
		ConfigGame.ModLocation? modLocation = this.prevModLocation;
		ConfigGame.ModLocation location = ConfigGame.Settings.ConfigMods.Location;
		if (!(modLocation.GetValueOrDefault() == location & modLocation != null))
		{
			this.prevModLocation = new ConfigGame.ModLocation?(ConfigGame.Settings.ConfigMods.Location);
			DirectoryScript.CheckDirectories();
		}
	}

	// Token: 0x04000705 RID: 1797
	private ConfigGame.ModLocation? prevModLocation;

	// Token: 0x0200058F RID: 1423
	public enum ModLocation
	{
		// Token: 0x04002548 RID: 9544
		Documents,
		// Token: 0x04002549 RID: 9545
		GameData
	}

	// Token: 0x02000590 RID: 1424
	[Serializable]
	public class ConfigGameState
	{
		// Token: 0x0400254A RID: 9546
		public ConfigGame.ConfigCameraState ConfigCamera = new ConfigGame.ConfigCameraState();

		// Token: 0x0400254B RID: 9547
		public ConfigGame.ConfigModsState ConfigMods = new ConfigGame.ConfigModsState();

		// Token: 0x0400254C RID: 9548
		public bool Portraits = true;

		// Token: 0x0400254D RID: 9549
		public bool Scripting = true;

		// Token: 0x0400254E RID: 9550
		public bool Snapping = true;
	}

	// Token: 0x02000591 RID: 1425
	[Serializable]
	public class ConfigCameraState
	{
		// Token: 0x0400254F RID: 9551
		public float LookSpeed = 0.5f;

		// Token: 0x04002550 RID: 9552
		public bool InvertVertical;

		// Token: 0x04002551 RID: 9553
		public bool InvertHorizontal;

		// Token: 0x04002552 RID: 9554
		public float MovementSpeed = 0.5f;

		// Token: 0x04002553 RID: 9555
		public int FOV = 60;
	}

	// Token: 0x02000592 RID: 1426
	[Serializable]
	public class ConfigModsState
	{
		// Token: 0x04002554 RID: 9556
		public bool Caching = true;

		// Token: 0x04002555 RID: 9557
		public bool RawCaching = true;

		// Token: 0x04002556 RID: 9558
		public ConfigGame.ModLocation Location;

		// Token: 0x04002557 RID: 9559
		public bool Threading = true;
	}
}
