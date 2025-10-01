using System;
using UnityEngine;

// Token: 0x02000025 RID: 37
[AddComponentMenu("NGUI/Examples/Slider Colors")]
public class UISliderColors : MonoBehaviour
{
	// Token: 0x060000AE RID: 174 RVA: 0x000050DB File Offset: 0x000032DB
	private void Start()
	{
		this.mBar = base.GetComponent<UIProgressBar>();
		this.mSprite = base.GetComponent<UIBasicSprite>();
		this.Update();
	}

	// Token: 0x060000AF RID: 175 RVA: 0x000050FC File Offset: 0x000032FC
	private void Update()
	{
		if (this.sprite == null || this.colors.Length == 0)
		{
			return;
		}
		float num = (this.mBar != null) ? this.mBar.value : this.mSprite.fillAmount;
		num *= (float)(this.colors.Length - 1);
		int num2 = Mathf.FloorToInt(num);
		Color color = this.colors[0];
		if (num2 >= 0)
		{
			if (num2 + 1 < this.colors.Length)
			{
				float t = num - (float)num2;
				color = Color.Lerp(this.colors[num2], this.colors[num2 + 1], t);
			}
			else if (num2 < this.colors.Length)
			{
				color = this.colors[num2];
			}
			else
			{
				color = this.colors[this.colors.Length - 1];
			}
		}
		color.a = this.sprite.color.a;
		this.sprite.color = color;
	}

	// Token: 0x04000079 RID: 121
	public UISprite sprite;

	// Token: 0x0400007A RID: 122
	public Color[] colors = new Color[]
	{
		Color.red,
		Color.yellow,
		Color.green
	};

	// Token: 0x0400007B RID: 123
	private UIProgressBar mBar;

	// Token: 0x0400007C RID: 124
	private UIBasicSprite mSprite;
}
