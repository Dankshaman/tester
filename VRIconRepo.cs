using System;
using UnityEngine;

// Token: 0x0200036A RID: 874
public class VRIconRepo : Singleton<VRIconRepo>
{
	// Token: 0x0600293B RID: 10555 RVA: 0x00121E59 File Offset: 0x00120059
	public Material GetToolMaterial(PointerMode tool)
	{
		if (tool >= PointerMode.Grab)
		{
			return new Material(this.ToolIcons[(int)tool]);
		}
		return this.NullIcon;
	}

	// Token: 0x0600293C RID: 10556 RVA: 0x00121E73 File Offset: 0x00120073
	public Material GetControlMaterial(VRControlIcon control)
	{
		if (control >= VRControlIcon.Flip)
		{
			return new Material(this.ControlIcons[(int)control]);
		}
		return this.NullIcon;
	}

	// Token: 0x04001B39 RID: 6969
	public Material[] ToolIcons;

	// Token: 0x04001B3A RID: 6970
	public Material[] ControlIcons;

	// Token: 0x04001B3B RID: 6971
	public Material NullIcon;
}
