using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200008D RID: 141
[ExecuteInEditMode]
[AddComponentMenu("NGUI/UI/Root")]
public class UIRoot : MonoBehaviour
{
	// Token: 0x17000171 RID: 369
	// (get) Token: 0x06000782 RID: 1922 RVA: 0x00035205 File Offset: 0x00033405
	public UIRoot.Constraint constraint
	{
		get
		{
			if (this.fitWidth)
			{
				if (this.fitHeight)
				{
					return UIRoot.Constraint.Fit;
				}
				return UIRoot.Constraint.FitWidth;
			}
			else
			{
				if (this.fitHeight)
				{
					return UIRoot.Constraint.FitHeight;
				}
				return UIRoot.Constraint.Fill;
			}
		}
	}

	// Token: 0x17000172 RID: 370
	// (get) Token: 0x06000783 RID: 1923 RVA: 0x00035228 File Offset: 0x00033428
	public UIRoot.Scaling activeScaling
	{
		get
		{
			UIRoot.Scaling scaling = this.scalingStyle;
			if (scaling == UIRoot.Scaling.ConstrainedOnMobiles)
			{
				return UIRoot.Scaling.Flexible;
			}
			return scaling;
		}
	}

	// Token: 0x17000173 RID: 371
	// (get) Token: 0x06000784 RID: 1924 RVA: 0x00035244 File Offset: 0x00033444
	public int activeHeight
	{
		get
		{
			if (this.activeScaling == UIRoot.Scaling.Flexible)
			{
				Vector2 screenSize = NGUITools.screenSize;
				float num = screenSize.x / screenSize.y;
				if (screenSize.y < (float)this.minimumHeight)
				{
					screenSize.y = (float)this.minimumHeight;
					screenSize.x = screenSize.y * num;
				}
				else if (screenSize.y > (float)this.maximumHeight)
				{
					screenSize.y = (float)this.maximumHeight;
					screenSize.x = screenSize.y * num;
				}
				int num2 = Mathf.RoundToInt((this.shrinkPortraitUI && screenSize.y > screenSize.x) ? (screenSize.y / num) : screenSize.y);
				if (!this.adjustByDPI)
				{
					return num2;
				}
				return NGUIMath.AdjustByDPI((float)num2);
			}
			else
			{
				UIRoot.Constraint constraint = this.constraint;
				if (constraint == UIRoot.Constraint.FitHeight)
				{
					return this.manualHeight;
				}
				Vector2 screenSize2 = NGUITools.screenSize;
				float num3 = screenSize2.x / screenSize2.y;
				float num4 = (float)this.manualWidth / (float)this.manualHeight;
				switch (constraint)
				{
				case UIRoot.Constraint.Fit:
					if (num4 <= num3)
					{
						return this.manualHeight;
					}
					return Mathf.RoundToInt((float)this.manualWidth / num3);
				case UIRoot.Constraint.Fill:
					if (num4 >= num3)
					{
						return this.manualHeight;
					}
					return Mathf.RoundToInt((float)this.manualWidth / num3);
				case UIRoot.Constraint.FitWidth:
					return Mathf.RoundToInt((float)this.manualWidth / num3);
				default:
					return this.manualHeight;
				}
			}
		}
	}

	// Token: 0x17000174 RID: 372
	// (get) Token: 0x06000785 RID: 1925 RVA: 0x000353A8 File Offset: 0x000335A8
	public float pixelSizeAdjustment
	{
		get
		{
			int num = Mathf.RoundToInt(NGUITools.screenSize.y);
			if (num != -1)
			{
				return this.GetPixelSizeAdjustment(num);
			}
			return 1f;
		}
	}

	// Token: 0x06000786 RID: 1926 RVA: 0x000353D8 File Offset: 0x000335D8
	public static float GetPixelSizeAdjustment(GameObject go)
	{
		UIRoot uiroot = NGUITools.FindInParents<UIRoot>(go);
		if (!(uiroot != null))
		{
			return 1f;
		}
		return uiroot.pixelSizeAdjustment;
	}

	// Token: 0x06000787 RID: 1927 RVA: 0x00035404 File Offset: 0x00033604
	public float GetPixelSizeAdjustment(int height)
	{
		height = Mathf.Max(2, height);
		if (this.activeScaling == UIRoot.Scaling.Constrained)
		{
			return (float)this.activeHeight / (float)height;
		}
		if (height < this.minimumHeight)
		{
			return (float)this.minimumHeight / (float)height;
		}
		if (height > this.maximumHeight)
		{
			return (float)this.maximumHeight / (float)height;
		}
		return 1f;
	}

	// Token: 0x06000788 RID: 1928 RVA: 0x0003545B File Offset: 0x0003365B
	protected virtual void Awake()
	{
		this.mTrans = base.transform;
	}

	// Token: 0x06000789 RID: 1929 RVA: 0x00035469 File Offset: 0x00033669
	protected virtual void OnEnable()
	{
		UIRoot.list.Add(this);
	}

	// Token: 0x0600078A RID: 1930 RVA: 0x00035476 File Offset: 0x00033676
	protected virtual void OnDisable()
	{
		UIRoot.list.Remove(this);
	}

	// Token: 0x0600078B RID: 1931 RVA: 0x00035484 File Offset: 0x00033684
	protected virtual void Start()
	{
		UIOrthoCamera componentInChildren = base.GetComponentInChildren<UIOrthoCamera>();
		if (componentInChildren != null)
		{
			Debug.LogWarning("UIRoot should not be active at the same time as UIOrthoCamera. Disabling UIOrthoCamera.", componentInChildren);
			Camera component = componentInChildren.gameObject.GetComponent<Camera>();
			componentInChildren.enabled = false;
			if (component != null)
			{
				component.orthographicSize = 1f;
				return;
			}
		}
		else
		{
			this.UpdateScale(false);
		}
	}

	// Token: 0x0600078C RID: 1932 RVA: 0x000354DB File Offset: 0x000336DB
	private void Update()
	{
		this.UpdateScale(true);
	}

	// Token: 0x0600078D RID: 1933 RVA: 0x000354E4 File Offset: 0x000336E4
	public void UpdateScale(bool updateAnchors = true)
	{
		if (this.mTrans != null)
		{
			float num = (float)this.activeHeight;
			if (num > 0f)
			{
				float num2 = 2f / num;
				Vector3 localScale = this.mTrans.localScale;
				if (Mathf.Abs(localScale.x - num2) > 1E-45f || Mathf.Abs(localScale.y - num2) > 1E-45f || Mathf.Abs(localScale.z - num2) > 1E-45f)
				{
					this.mTrans.localScale = new Vector3(num2, num2, num2);
					if (updateAnchors)
					{
						base.BroadcastMessage("UpdateAnchors", SendMessageOptions.DontRequireReceiver);
					}
				}
			}
		}
	}

	// Token: 0x0600078E RID: 1934 RVA: 0x00035584 File Offset: 0x00033784
	public static void Broadcast(string funcName)
	{
		int i = 0;
		int count = UIRoot.list.Count;
		while (i < count)
		{
			UIRoot uiroot = UIRoot.list[i];
			if (uiroot != null)
			{
				uiroot.BroadcastMessage(funcName, SendMessageOptions.DontRequireReceiver);
			}
			i++;
		}
	}

	// Token: 0x0600078F RID: 1935 RVA: 0x000355C8 File Offset: 0x000337C8
	public static void Broadcast(string funcName, object param)
	{
		if (param == null)
		{
			Debug.LogError("SendMessage is bugged when you try to pass 'null' in the parameter field. It behaves as if no parameter was specified.");
			return;
		}
		int i = 0;
		int count = UIRoot.list.Count;
		while (i < count)
		{
			UIRoot uiroot = UIRoot.list[i];
			if (uiroot != null)
			{
				uiroot.BroadcastMessage(funcName, param, SendMessageOptions.DontRequireReceiver);
			}
			i++;
		}
	}

	// Token: 0x04000533 RID: 1331
	public static List<UIRoot> list = new List<UIRoot>();

	// Token: 0x04000534 RID: 1332
	public UIRoot.Scaling scalingStyle;

	// Token: 0x04000535 RID: 1333
	public int manualWidth = 1280;

	// Token: 0x04000536 RID: 1334
	public int manualHeight = 720;

	// Token: 0x04000537 RID: 1335
	public int minimumHeight = 320;

	// Token: 0x04000538 RID: 1336
	public int maximumHeight = 1536;

	// Token: 0x04000539 RID: 1337
	public bool fitWidth;

	// Token: 0x0400053A RID: 1338
	public bool fitHeight = true;

	// Token: 0x0400053B RID: 1339
	public bool adjustByDPI;

	// Token: 0x0400053C RID: 1340
	public bool shrinkPortraitUI;

	// Token: 0x0400053D RID: 1341
	private Transform mTrans;

	// Token: 0x02000578 RID: 1400
	public enum Scaling
	{
		// Token: 0x040024EA RID: 9450
		Flexible,
		// Token: 0x040024EB RID: 9451
		Constrained,
		// Token: 0x040024EC RID: 9452
		ConstrainedOnMobiles
	}

	// Token: 0x02000579 RID: 1401
	public enum Constraint
	{
		// Token: 0x040024EE RID: 9454
		Fit,
		// Token: 0x040024EF RID: 9455
		Fill,
		// Token: 0x040024F0 RID: 9456
		FitWidth,
		// Token: 0x040024F1 RID: 9457
		FitHeight
	}
}
