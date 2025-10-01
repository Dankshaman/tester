using System;
using UnityEngine;

// Token: 0x0200027D RID: 637
public class UIAttachToObject : MonoBehaviour
{
	// Token: 0x06002135 RID: 8501 RVA: 0x000EFDF8 File Offset: 0x000EDFF8
	private void Awake()
	{
		if (!UIAttachToObject.MainCamera)
		{
			UIAttachToObject.MainCamera = Camera.main;
		}
		if (!UIAttachToObject.UIDrawCamera)
		{
			UIAttachToObject.UIDrawCamera = GameObject.Find("UICamera").GetComponent<Camera>();
		}
		this.ThisTransform = base.gameObject.transform;
		this.ThisBoxColliders = base.GetComponentsInChildren<BoxCollider2D>();
		this.ThisSprites = base.GetComponentsInChildren<UISprite>(true);
	}

	// Token: 0x06002136 RID: 8502 RVA: 0x000EFE65 File Offset: 0x000EE065
	private void OnEnable()
	{
		this.Update();
	}

	// Token: 0x06002137 RID: 8503 RVA: 0x000EFE70 File Offset: 0x000EE070
	private void Update()
	{
		if (!this.AttachTo)
		{
			base.gameObject.SetActive(false);
			return;
		}
		if (!UIAttachToObject.MainCamera)
		{
			return;
		}
		Vector3 vector = UIAttachToObject.MainCamera.WorldToScreenPoint(this.AttachTo.position);
		if (vector.z >= 0f)
		{
			vector.z = 0f;
		}
		Vector3 position = UIAttachToObject.UIDrawCamera.ScreenToWorldPoint(vector);
		bool flag = Mathf.Abs(vector.x - Input.mousePosition.x) < this.DistanceCheck && Mathf.Abs(vector.y - Input.mousePosition.y) < this.DistanceCheck;
		if (this.bCacheEnable != flag)
		{
			this.bCacheEnable = flag;
			for (int i = 0; i < this.ThisBoxColliders.Length; i++)
			{
				this.ThisBoxColliders[i].enabled = flag;
			}
		}
		for (int j = 0; j < this.ThisSprites.Length; j++)
		{
			UISprite uisprite = this.ThisSprites[j];
			if (uisprite.gameObject.activeSelf)
			{
				uisprite.depth = (int)(100000f / Vector3.Distance(this.AttachTo.position, UIAttachToObject.MainCamera.transform.position));
				if (j == 3)
				{
					uisprite.depth++;
				}
			}
		}
		this.ThisTransform.position = position;
	}

	// Token: 0x04001482 RID: 5250
	public Transform AttachTo;

	// Token: 0x04001483 RID: 5251
	public float DistanceCheck = 50f;

	// Token: 0x04001484 RID: 5252
	private static Camera MainCamera;

	// Token: 0x04001485 RID: 5253
	private static Camera UIDrawCamera;

	// Token: 0x04001486 RID: 5254
	private UISprite[] ThisSprites;

	// Token: 0x04001487 RID: 5255
	private Transform ThisTransform;

	// Token: 0x04001488 RID: 5256
	private BoxCollider2D[] ThisBoxColliders;

	// Token: 0x04001489 RID: 5257
	private bool bCacheEnable = true;
}
