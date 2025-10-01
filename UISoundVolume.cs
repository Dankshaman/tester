using System;
using UnityEngine;

// Token: 0x0200004F RID: 79
[RequireComponent(typeof(UISlider))]
[AddComponentMenu("NGUI/Interaction/Sound Volume")]
public class UISoundVolume : MonoBehaviour
{
	// Token: 0x0600028C RID: 652 RVA: 0x00010BE2 File Offset: 0x0000EDE2
	private void Awake()
	{
		UISlider component = base.GetComponent<UISlider>();
		component.value = NGUITools.soundVolume;
		EventDelegate.Add(component.onChange, new EventDelegate.Callback(this.OnChange));
	}

	// Token: 0x0600028D RID: 653 RVA: 0x00010C0C File Offset: 0x0000EE0C
	private void OnChange()
	{
		NGUITools.soundVolume = UIProgressBar.current.value;
	}
}
