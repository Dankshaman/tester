using System;
using UnityEngine;
using UnityEngine.EventSystems;

// Token: 0x020002BB RID: 699
public class UICustomPaletteBox : MonoBehaviour, IPointerClickHandler, IEventSystemHandler
{
	// Token: 0x060022AE RID: 8878 RVA: 0x000F7ED3 File Offset: 0x000F60D3
	public void OnPointerClick(PointerEventData eventData)
	{
		if (eventData.button == PointerEventData.InputButton.Left)
		{
			Debug.Log("Left click");
			return;
		}
		if (eventData.button == PointerEventData.InputButton.Middle)
		{
			Debug.Log("Middle click");
			return;
		}
		if (eventData.button == PointerEventData.InputButton.Right)
		{
			Debug.Log("Right click");
		}
	}
}
