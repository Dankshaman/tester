using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200032A RID: 810
public class UIRotationValue : MonoBehaviour
{
	// Token: 0x060026D2 RID: 9938 RVA: 0x00113F26 File Offset: 0x00112126
	private void Awake()
	{
		EventDelegate.Add(this.SubmitButton.onClick, new EventDelegate.Callback(this.SubmitOnClick));
		EventDelegate.Add(this.RotationButton.onClick, new EventDelegate.Callback(this.RotationOnClick));
	}

	// Token: 0x060026D3 RID: 9939 RVA: 0x00113F62 File Offset: 0x00112162
	private void OnDestroy()
	{
		EventDelegate.Remove(this.SubmitButton.onClick, new EventDelegate.Callback(this.SubmitOnClick));
		EventDelegate.Remove(this.RotationButton.onClick, new EventDelegate.Callback(this.RotationOnClick));
	}

	// Token: 0x060026D4 RID: 9940 RVA: 0x00113FA0 File Offset: 0x001121A0
	private void SubmitOnClick()
	{
		float x = 0f;
		float.TryParse(this.RotXInput.value, out x);
		float y = 0f;
		float.TryParse(this.RotYInput.value, out y);
		float z = 0f;
		float.TryParse(this.RotZInput.value, out z);
		Vector3 rotation = new Vector3(x, y, z);
		RotationValue rotationValue = new RotationValue(this.ValueInput.value, rotation);
		List<RotationValue> rotationValues = this.TargetNPO.RotationValues;
		if (this.TargetIndex == -1)
		{
			rotationValues.Add(rotationValue);
		}
		else
		{
			rotationValues[this.TargetIndex] = rotationValue;
		}
		this.TargetNPO.SetRotationValues(rotationValues);
		NetworkSingleton<NetworkUI>.Instance.GUIRotationValue.Reload(true);
		base.gameObject.SetActive(false);
	}

	// Token: 0x060026D5 RID: 9941 RVA: 0x00114070 File Offset: 0x00112270
	private void RotationOnClick()
	{
		this.RotXInput.value = Mathf.Round(this.TargetNPO.transform.eulerAngles.x).ToString();
		this.RotYInput.value = Mathf.Round(this.TargetNPO.transform.eulerAngles.y).ToString();
		this.RotZInput.value = Mathf.Round(this.TargetNPO.transform.eulerAngles.z).ToString();
	}

	// Token: 0x060026D6 RID: 9942 RVA: 0x00114104 File Offset: 0x00112304
	public void Begin(NetworkPhysicsObject NPO, int index = -1)
	{
		this.TargetNPO = NPO;
		this.TargetIndex = index;
		string value = "";
		float num = 0f;
		float num2 = 0f;
		float num3 = 0f;
		int num4;
		if (index == -1)
		{
			num4 = NPO.RotationValues.Count + 1;
		}
		else
		{
			num4 = index + 1;
			RotationValue rotationValue = this.TargetNPO.RotationValues[index];
			value = rotationValue.value;
			num = Mathf.Round(rotationValue.rotation.x);
			num2 = Mathf.Round(rotationValue.rotation.y);
			num3 = Mathf.Round(rotationValue.rotation.z);
		}
		this.LabelIndex.text = "(" + num4 + ")";
		this.ValueInput.value = value;
		this.RotXInput.value = num.ToString();
		this.RotYInput.value = num2.ToString();
		this.RotZInput.value = num3.ToString();
		base.gameObject.SetActive(true);
	}

	// Token: 0x0400194B RID: 6475
	public UIInput ValueInput;

	// Token: 0x0400194C RID: 6476
	public UIInput RotXInput;

	// Token: 0x0400194D RID: 6477
	public UIInput RotYInput;

	// Token: 0x0400194E RID: 6478
	public UIInput RotZInput;

	// Token: 0x0400194F RID: 6479
	public UIButton RotationButton;

	// Token: 0x04001950 RID: 6480
	public UIButton SubmitButton;

	// Token: 0x04001951 RID: 6481
	public UILabel LabelIndex;

	// Token: 0x04001952 RID: 6482
	private NetworkPhysicsObject TargetNPO;

	// Token: 0x04001953 RID: 6483
	private int TargetIndex = -1;
}
