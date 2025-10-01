using System;
using UnityEngine;

// Token: 0x02000309 RID: 777
public class UIObjectIndicator : MonoBehaviour
{
	// Token: 0x060025C1 RID: 9665 RVA: 0x00109CB8 File Offset: 0x00107EB8
	private void OnEnable()
	{
		UIPulse component = base.GetComponent<UIPulse>();
		if (component)
		{
			if (this.type == UIObjectIndicatorManager.IndicatorType.ObjectEnteredContainer)
			{
				this.endTime = Time.time + 0.5f;
				component.Mode = PulseMode.Fade;
				component.StartTime = Time.time;
				component.Duration = 0.5f;
				return;
			}
			component.Mode = PulseMode.Pulse;
		}
	}

	// Token: 0x060025C2 RID: 9666 RVA: 0x00109D13 File Offset: 0x00107F13
	private void Update()
	{
		if (this.type == UIObjectIndicatorManager.IndicatorType.ObjectEnteredContainer && Time.time >= this.endTime)
		{
			Singleton<UIObjectIndicatorManager>.Instance.RemoveIndicator(this.targetNPO, this.type, this.color);
			base.gameObject.SetActive(false);
		}
	}

	// Token: 0x0400186E RID: 6254
	private const float ObjectEnteredContainerFadeDuration = 0.5f;

	// Token: 0x0400186F RID: 6255
	public UISprite sprite;

	// Token: 0x04001870 RID: 6256
	public UIObjectIndicatorManager.IndicatorType type;

	// Token: 0x04001871 RID: 6257
	public UILabel label;

	// Token: 0x04001872 RID: 6258
	public float minTime;

	// Token: 0x04001873 RID: 6259
	public string color;

	// Token: 0x04001874 RID: 6260
	public NetworkPhysicsObject targetNPO;

	// Token: 0x04001875 RID: 6261
	private float endTime;
}
