using System;
using UnityEngine;

// Token: 0x02000034 RID: 52
[AddComponentMenu("NGUI/Interaction/Center Scroll View on Click")]
public class UICenterOnClick : MonoBehaviour
{
	// Token: 0x06000119 RID: 281 RVA: 0x00007530 File Offset: 0x00005730
	private void OnClick()
	{
		UICenterOnChild uicenterOnChild = NGUITools.FindInParents<UICenterOnChild>(base.gameObject);
		UIPanel uipanel = NGUITools.FindInParents<UIPanel>(base.gameObject);
		if (uicenterOnChild != null)
		{
			if (uicenterOnChild.enabled)
			{
				uicenterOnChild.CenterOn(base.transform);
				return;
			}
		}
		else if (uipanel != null && uipanel.clipping != UIDrawCall.Clipping.None)
		{
			UIScrollView component = uipanel.GetComponent<UIScrollView>();
			Vector3 pos = -uipanel.cachedTransform.InverseTransformPoint(base.transform.position);
			if (!component.canMoveHorizontally)
			{
				pos.x = uipanel.cachedTransform.localPosition.x;
			}
			if (!component.canMoveVertically)
			{
				pos.y = uipanel.cachedTransform.localPosition.y;
			}
			SpringPanel.Begin(uipanel.cachedGameObject, pos, 6f);
		}
	}
}
