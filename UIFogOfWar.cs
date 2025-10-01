using System;
using UnityEngine;

// Token: 0x020002CE RID: 718
public class UIFogOfWar : MonoBehaviour
{
	// Token: 0x06002325 RID: 8997 RVA: 0x000FA00C File Offset: 0x000F820C
	private void OnEnable()
	{
		FogOfWarZone component = PlayerScript.PointerScript.InfoFogOfWarZoneGO.GetComponent<FogOfWarZone>();
		this.ToggleGmPointer.value = component.HideGmPointer;
		this.ToggleHideObjects.value = component.HideObjects;
		this.ToggleReHideObjects.value = component.ReHideObjects;
		this.ToggleGreyOut.value = component.GreyOut;
		this.HeightSlider.floatValue = component.FogHeight * 10f;
	}

	// Token: 0x06002326 RID: 8998 RVA: 0x000FA084 File Offset: 0x000F8284
	public void OnToggleGmPointer()
	{
		PlayerScript.PointerScript.InfoFogOfWarZoneGO.GetComponent<FogOfWarZone>().HideGmPointer = this.ToggleGmPointer.value;
	}

	// Token: 0x06002327 RID: 8999 RVA: 0x000FA0A5 File Offset: 0x000F82A5
	public void OnToggleHideObjects()
	{
		PlayerScript.PointerScript.InfoFogOfWarZoneGO.GetComponent<FogOfWarZone>().HideObjects = this.ToggleHideObjects.value;
	}

	// Token: 0x06002328 RID: 9000 RVA: 0x000FA0C6 File Offset: 0x000F82C6
	public void OnToggleReHideObjects()
	{
		PlayerScript.PointerScript.InfoFogOfWarZoneGO.GetComponent<FogOfWarZone>().ReHideObjects = this.ToggleReHideObjects.value;
	}

	// Token: 0x06002329 RID: 9001 RVA: 0x000FA0E7 File Offset: 0x000F82E7
	public void OnToggleGreyOut()
	{
		PlayerScript.PointerScript.InfoFogOfWarZoneGO.GetComponent<FogOfWarZone>().GreyOut = this.ToggleGreyOut.value;
	}

	// Token: 0x0600232A RID: 9002 RVA: 0x000FA108 File Offset: 0x000F8308
	public void OnSliderChangeValue()
	{
		PlayerScript.PointerScript.InfoFogOfWarZoneGO.GetComponent<FogOfWarZone>().FogHeight = this.HeightSlider.floatValue / 10f;
	}

	// Token: 0x0600232B RID: 9003 RVA: 0x000FA12F File Offset: 0x000F832F
	public void OnResetFog()
	{
		PlayerScript.PointerScript.InfoFogOfWarZoneGO.GetComponent<FogOfWarZone>().ResetFogOfWar();
	}

	// Token: 0x0400164A RID: 5706
	public UIToggle ToggleGmPointer;

	// Token: 0x0400164B RID: 5707
	public UIToggle ToggleHideObjects;

	// Token: 0x0400164C RID: 5708
	public UIToggle ToggleReHideObjects;

	// Token: 0x0400164D RID: 5709
	public UIToggle ToggleGreyOut;

	// Token: 0x0400164E RID: 5710
	public UISliderRange HeightSlider;
}
