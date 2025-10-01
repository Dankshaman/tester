using System;
using Newtonsoft.Json;
using UnityEngine;

// Token: 0x020001F0 RID: 496
public class RotationValue
{
	// Token: 0x06001A03 RID: 6659 RVA: 0x00002594 File Offset: 0x00000794
	[JsonConstructor]
	public RotationValue()
	{
	}

	// Token: 0x06001A04 RID: 6660 RVA: 0x000B6684 File Offset: 0x000B4884
	public RotationValue(string value, Vector3 rotation)
	{
		this.value = value;
		float num;
		if (float.TryParse(value, out num))
		{
			this.floatValue = new float?(num);
		}
		this.rotation = rotation;
		this.direction = Utilities.GetDirectionFromRotation(rotation, true);
	}

	// Token: 0x06001A05 RID: 6661 RVA: 0x000B66C8 File Offset: 0x000B48C8
	public RotationValue(float value, Vector3 rotation)
	{
		this.value = value.ToString();
		this.floatValue = new float?(value);
		this.rotation = rotation;
		this.direction = Utilities.GetDirectionFromRotation(rotation, true);
	}

	// Token: 0x04001006 RID: 4102
	public string value;

	// Token: 0x04001007 RID: 4103
	public float? floatValue;

	// Token: 0x04001008 RID: 4104
	public Vector3 rotation;

	// Token: 0x04001009 RID: 4105
	public Vector3 direction;
}
