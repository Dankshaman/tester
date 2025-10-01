using System;
using UnityEngine;

// Token: 0x02000345 RID: 837
public class UITabMenu : MonoBehaviour
{
	// Token: 0x060027BA RID: 10170 RVA: 0x00119CA3 File Offset: 0x00117EA3
	public bool NameToBool(string name)
	{
		return this.SelectedTab.transform.GetChild(0).GetChild(0).Find(name).GetComponentInChildren<UIToggle>().value;
	}

	// Token: 0x060027BB RID: 10171 RVA: 0x00119CCC File Offset: 0x00117ECC
	public void SetValue(string name, bool value)
	{
		this.SelectedTab.transform.GetChild(0).GetChild(0).Find(name).GetComponentInChildren<UIToggle>().activeAnimation = null;
		this.SelectedTab.transform.GetChild(0).GetChild(0).Find(name).GetComponentInChildren<UIToggle>().animator = null;
		this.SelectedTab.transform.GetChild(0).GetChild(0).Find(name).GetComponentInChildren<UIToggle>().value = value;
	}

	// Token: 0x060027BC RID: 10172 RVA: 0x00119D54 File Offset: 0x00117F54
	public void SetValue(string name, Vector3 value)
	{
		UIInput[] componentsInChildren = this.SelectedTab.transform.GetChild(0).GetChild(0).Find(name).GetChild(0).GetComponentsInChildren<UIInput>();
		componentsInChildren[0].value = value.x.ToString();
		componentsInChildren[1].value = value.y.ToString();
		componentsInChildren[2].value = value.z.ToString();
	}

	// Token: 0x060027BD RID: 10173 RVA: 0x00119DC4 File Offset: 0x00117FC4
	public void SetValue(string name, float value)
	{
		this.SelectedTab.transform.GetChild(0).GetChild(0).Find(name).GetComponentInChildren<UIInput>().value = value.ToString("0.###################################################################################################################################################################################################################################################################################################################################################");
	}

	// Token: 0x060027BE RID: 10174 RVA: 0x00119DFC File Offset: 0x00117FFC
	public Vector3 NameToVector3(string name)
	{
		Vector3 zero = Vector3.zero;
		UIInput[] componentsInChildren = this.SelectedTab.transform.GetChild(0).GetChild(0).Find(name).GetChild(0).GetComponentsInChildren<UIInput>();
		for (int i = 0; i < 3; i++)
		{
			string value = componentsInChildren[i].value;
			if (i == 0)
			{
				float.TryParse(value, out zero.x);
			}
			if (i == 1)
			{
				float.TryParse(value, out zero.y);
			}
			if (i == 2)
			{
				float.TryParse(value, out zero.z);
			}
		}
		return zero;
	}

	// Token: 0x060027BF RID: 10175 RVA: 0x00119E83 File Offset: 0x00118083
	public float NameToFloat(string name)
	{
		return this.UIInputToFloat(this.SelectedTab.transform.GetChild(0).GetChild(0).Find(name).GetComponentInChildren<UIInput>());
	}

	// Token: 0x060027C0 RID: 10176 RVA: 0x00119EB0 File Offset: 0x001180B0
	public float UIInputToFloat(UIInput input)
	{
		float result = 0f;
		float.TryParse(input.value, out result);
		return result;
	}

	// Token: 0x060027C1 RID: 10177 RVA: 0x00119ED2 File Offset: 0x001180D2
	public UIInput NameToUIInput(string name)
	{
		return this.SelectedTab.transform.GetChild(0).GetChild(0).Find(name).GetComponentInChildren<UIInput>();
	}

	// Token: 0x060027C2 RID: 10178 RVA: 0x00119EF6 File Offset: 0x001180F6
	public UIToggle NameToUIToggle(string name)
	{
		return this.SelectedTab.transform.GetChild(0).GetChild(0).Find(name).GetComponentInChildren<UIToggle>();
	}

	// Token: 0x04001A17 RID: 6679
	public GameObject SelectedTab;
}
