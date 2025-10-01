using System;
using UnityEngine;

// Token: 0x020002F8 RID: 760
public class UIKeepVisible : MonoBehaviour
{
	// Token: 0x060024C1 RID: 9409 RVA: 0x001038D8 File Offset: 0x00101AD8
	private void Update()
	{
		Camera component = GameObject.Find("UICamera").GetComponent<Camera>();
		Vector3 vector = component.WorldToScreenPoint(base.transform.position);
		vector = new Vector3(vector.x, vector.y, 0f);
		Debug.Log(vector);
		base.transform.position = component.ViewportToWorldPoint(vector);
	}
}
