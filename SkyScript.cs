using System;
using System.Collections;
using mset;
using NewNet;
using UnityEngine;
using UnityEngine.Rendering;

// Token: 0x0200022F RID: 559
public class SkyScript : MonoBehaviour
{
	// Token: 0x1700041D RID: 1053
	// (get) Token: 0x06001BBF RID: 7103 RVA: 0x000BEE19 File Offset: 0x000BD019
	// (set) Token: 0x06001BC0 RID: 7104 RVA: 0x000BEE21 File Offset: 0x000BD021
	public NetworkView networkView { get; private set; }

	// Token: 0x1700041E RID: 1054
	// (get) Token: 0x06001BC1 RID: 7105 RVA: 0x000BEE2A File Offset: 0x000BD02A
	public string InternalName
	{
		get
		{
			if (!this.networkView)
			{
				return Utilities.RemoveCloneFromName(base.gameObject.name);
			}
			return this.networkView.InternalName;
		}
	}

	// Token: 0x06001BC2 RID: 7106 RVA: 0x000BEE55 File Offset: 0x000BD055
	private void Awake()
	{
		this.networkView = base.GetComponent<NetworkView>();
		NetworkSingleton<ManagerPhysicsObject>.Instance.Sky = this;
	}

	// Token: 0x06001BC3 RID: 7107 RVA: 0x000BEE6E File Offset: 0x000BD06E
	private IEnumerator Start()
	{
		SkyManager.Get().GlobalSky = base.GetComponent<Sky>();
		SkyScript.CurrentSpecIntensity = base.GetComponent<Sky>().SpecIntensity;
		SkyScript.UpdateSpecIntensity();
		RenderSettings.defaultReflectionMode = DefaultReflectionMode.Custom;
		RenderSettings.customReflection = this.StandardShaderCubemap;
		yield return null;
		yield return null;
		DynamicGI.UpdateEnvironment();
		yield return new WaitForSeconds(1f);
		DynamicGI.UpdateEnvironment();
		yield break;
	}

	// Token: 0x06001BC4 RID: 7108 RVA: 0x000BEE7D File Offset: 0x000BD07D
	public static void UpdateSpecIntensity()
	{
		SkyManager.Get().GlobalSky.SpecIntensity = SkyScript.CurrentSpecIntensity * NetworkSingleton<LightingScript>.Instance.lightingState.ReflectionIntensity;
	}

	// Token: 0x0400117C RID: 4476
	public Cubemap StandardShaderCubemap;

	// Token: 0x0400117E RID: 4478
	private static float CurrentSpecIntensity = 0.5f;
}
