using System;
using UnityEngine;

// Token: 0x02000088 RID: 136
[RequireComponent(typeof(UIInput))]
public class UIInputOnGUI : MonoBehaviour
{
	// Token: 0x060006AC RID: 1708 RVA: 0x0003042D File Offset: 0x0002E62D
	private void Awake()
	{
		this.mInput = base.GetComponent<UIInput>();
	}

	// Token: 0x060006AD RID: 1709 RVA: 0x0003043B File Offset: 0x0002E63B
	private void OnGUI()
	{
		if (Event.current.rawType == EventType.KeyDown)
		{
			this.mInput.ProcessEvent(Event.current, false, false, false);
		}
	}

	// Token: 0x040004CD RID: 1229
	[NonSerialized]
	private UIInput mInput;
}
