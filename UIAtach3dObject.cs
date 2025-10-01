using System;
using UnityEngine;

// Token: 0x0200027C RID: 636
public class UIAtach3dObject : MonoBehaviour
{
	// Token: 0x06002130 RID: 8496 RVA: 0x000EFC68 File Offset: 0x000EDE68
	private void Start()
	{
		if (!UIAtach3dObject.MainCamera)
		{
			UIAtach3dObject.MainCamera = Camera.main;
		}
		if (!UIAtach3dObject.UIDrawCamera)
		{
			UIAtach3dObject.UIDrawCamera = GameObject.Find("UICamera").GetComponent<Camera>();
		}
		Renderer renderer;
		if (base.GetComponent<Renderer>())
		{
			renderer = this.ObjectToAttach.GetComponent<Renderer>();
		}
		else
		{
			renderer = this.ObjectToAttach.GetComponentInChildren<Renderer>();
		}
		this.ObjectToAttach.layer = 8;
		float num = NetworkSingleton<ManagerPhysicsObject>.Instance.UIResolutionScale();
		this.ZDistance = renderer.bounds.size.magnitude * 6.5f / num;
	}

	// Token: 0x06002131 RID: 8497 RVA: 0x000EFD10 File Offset: 0x000EDF10
	private void Update()
	{
		Vector3 vector = UIAtach3dObject.UIDrawCamera.WorldToScreenPoint(new Vector3(base.transform.position.x, base.transform.position.y + 0.1f, base.transform.position.z));
		Vector3 position = UIAtach3dObject.MainCamera.ScreenToWorldPoint(new Vector3(vector.x, vector.y, this.ZDistance));
		this.ObjectToAttach.transform.position = position;
		this.ObjectToAttach.transform.RotateAround(base.transform.position, base.transform.up, 0.5f);
	}

	// Token: 0x06002132 RID: 8498 RVA: 0x000EFDC1 File Offset: 0x000EDFC1
	private void OnEnable()
	{
		if (this.ObjectToAttach)
		{
			this.ObjectToAttach.SetActive(true);
		}
	}

	// Token: 0x06002133 RID: 8499 RVA: 0x000EFDDC File Offset: 0x000EDFDC
	private void OnDisable()
	{
		if (this.ObjectToAttach)
		{
			this.ObjectToAttach.SetActive(false);
		}
	}

	// Token: 0x0400147E RID: 5246
	public GameObject ObjectToAttach;

	// Token: 0x0400147F RID: 5247
	private static Camera MainCamera;

	// Token: 0x04001480 RID: 5248
	private static Camera UIDrawCamera;

	// Token: 0x04001481 RID: 5249
	private float ZDistance;
}
