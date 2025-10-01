using System;
using UnityEngine;

// Token: 0x0200032B RID: 811
public class UISaveToPref : MonoBehaviour
{
	// Token: 0x060026D8 RID: 9944 RVA: 0x0011421C File Offset: 0x0011241C
	private void OnEnable()
	{
		if (this.TargetObject == null)
		{
			this.TargetObject = base.gameObject;
		}
		if (string.IsNullOrEmpty(this.SaveName))
		{
			this.SaveName = base.gameObject.name;
		}
		if (PlayerPrefs.HasKey(this.SaveName))
		{
			string @string = PlayerPrefs.GetString(this.SaveName);
			if (this.TargetObject.GetComponent<UIToggle>())
			{
				this.TargetObject.GetComponent<UIToggle>().value = bool.Parse(@string);
			}
			if (this.TargetObject.GetComponent<UIInput>())
			{
				this.TargetObject.GetComponent<UIInput>().value = @string;
			}
			if (this.TargetObject.GetComponent<UISlider>())
			{
				this.TargetObject.GetComponent<UISlider>().value = float.Parse(@string);
			}
		}
	}

	// Token: 0x060026D9 RID: 9945 RVA: 0x001142F0 File Offset: 0x001124F0
	private void OnDisable()
	{
		string text = null;
		if (this.TargetObject.GetComponent<UIToggle>())
		{
			text = this.TargetObject.GetComponent<UIToggle>().value.ToString();
		}
		if (this.TargetObject.GetComponent<UIInput>())
		{
			text = this.TargetObject.GetComponent<UIInput>().value;
		}
		if (this.TargetObject.GetComponent<UISlider>())
		{
			text = this.TargetObject.GetComponent<UISlider>().value.ToString();
		}
		if (text != null)
		{
			PlayerPrefs.SetString(this.SaveName, text);
		}
	}

	// Token: 0x04001954 RID: 6484
	public string SaveName;

	// Token: 0x04001955 RID: 6485
	public GameObject TargetObject;
}
