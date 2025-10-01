using System;
using UnityEngine;

// Token: 0x0200000B RID: 11
[RequireComponent(typeof(UISprite))]
[AddComponentMenu("NGUI/Examples/UI Cursor")]
public class UICursor : MonoBehaviour
{
	// Token: 0x06000041 RID: 65 RVA: 0x0000332F File Offset: 0x0000152F
	private void Awake()
	{
		UICursor.instance = this;
	}

	// Token: 0x06000042 RID: 66 RVA: 0x00003337 File Offset: 0x00001537
	private void OnDestroy()
	{
		UICursor.instance = null;
	}

	// Token: 0x06000043 RID: 67 RVA: 0x00003340 File Offset: 0x00001540
	private void Start()
	{
		this.mTrans = base.transform;
		this.mSprite = base.GetComponentInChildren<UISprite>();
		if (this.uiCamera == null)
		{
			this.uiCamera = NGUITools.FindCameraForLayer(base.gameObject.layer);
		}
		if (this.mSprite != null)
		{
			this.mAtlas = this.mSprite.atlas;
			this.mSpriteName = this.mSprite.spriteName;
			if (this.mSprite.depth < 100)
			{
				this.mSprite.depth = 100;
			}
		}
	}

	// Token: 0x06000044 RID: 68 RVA: 0x000033D8 File Offset: 0x000015D8
	private void Update()
	{
		Vector3 mousePosition = Input.mousePosition;
		if (this.uiCamera != null)
		{
			mousePosition.x = Mathf.Clamp01(mousePosition.x / (float)Screen.width);
			mousePosition.y = Mathf.Clamp01(mousePosition.y / (float)Screen.height);
			this.mTrans.position = this.uiCamera.ViewportToWorldPoint(mousePosition);
			if (this.uiCamera.orthographic)
			{
				Vector3 localPosition = this.mTrans.localPosition;
				localPosition.x = Mathf.Round(localPosition.x);
				localPosition.y = Mathf.Round(localPosition.y);
				this.mTrans.localPosition = localPosition;
				return;
			}
		}
		else
		{
			mousePosition.x -= (float)Screen.width * 0.5f;
			mousePosition.y -= (float)Screen.height * 0.5f;
			mousePosition.x = Mathf.Round(mousePosition.x);
			mousePosition.y = Mathf.Round(mousePosition.y);
			this.mTrans.localPosition = mousePosition;
		}
	}

	// Token: 0x06000045 RID: 69 RVA: 0x000034F0 File Offset: 0x000016F0
	public static void Clear()
	{
		if (UICursor.instance != null && UICursor.instance.mSprite != null)
		{
			UICursor.Set(UICursor.instance.mAtlas, UICursor.instance.mSpriteName);
		}
	}

	// Token: 0x06000046 RID: 70 RVA: 0x0000352C File Offset: 0x0000172C
	public static void Set(UIAtlas atlas, string sprite)
	{
		if (UICursor.instance != null && UICursor.instance.mSprite)
		{
			UICursor.instance.mSprite.atlas = atlas;
			UICursor.instance.mSprite.spriteName = sprite;
			UICursor.instance.mSprite.MakePixelPerfect();
			UICursor.instance.Update();
		}
	}

	// Token: 0x04000018 RID: 24
	public static UICursor instance;

	// Token: 0x04000019 RID: 25
	public Camera uiCamera;

	// Token: 0x0400001A RID: 26
	private Transform mTrans;

	// Token: 0x0400001B RID: 27
	private UISprite mSprite;

	// Token: 0x0400001C RID: 28
	private UIAtlas mAtlas;

	// Token: 0x0400001D RID: 29
	private string mSpriteName;
}
