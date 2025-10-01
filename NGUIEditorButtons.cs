using System;
using I2.Loc;
using UnityEngine;

// Token: 0x0200026E RID: 622
public class NGUIEditorButtons : MonoBehaviour
{
	// Token: 0x060020C8 RID: 8392 RVA: 0x000ED4ED File Offset: 0x000EB6ED
	private void DirtyScene()
	{
		TTSEditorUtilities.DirtyScene();
	}

	// Token: 0x060020C9 RID: 8393 RVA: 0x000ED4F4 File Offset: 0x000EB6F4
	[ContextMenu("Fix Blurry UI")]
	public void FixBlurryUI()
	{
		this.SetScaleOne();
		this.SetPositionWholeNumber();
	}

	// Token: 0x060020CA RID: 8394 RVA: 0x000ED504 File Offset: 0x000EB704
	[ContextMenu("Remove Localization")]
	public void RemoveLocalization()
	{
		foreach (GameObject gameObject in Utilities.GetSceneRootGameObjects())
		{
			Localize[] componentsInChildren = gameObject.GetComponentsInChildren<Localize>(true);
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				Debug.Log(componentsInChildren[i]);
			}
		}
	}

	// Token: 0x060020CB RID: 8395 RVA: 0x000ED56C File Offset: 0x000EB76C
	public void SetScaleOne()
	{
		bool flag = false;
		foreach (Transform transform in this.targetRoot.GetComponentsInChildren<Transform>(true))
		{
			if (!(transform.gameObject == this.targetRoot) && transform.localScale != Vector3.one)
			{
				if ((double)Vector3.Distance(transform.localScale, Vector3.one) > 0.01)
				{
					Debug.Log("Object is not close to scale 1: " + transform);
				}
				else
				{
					Debug.Log("Converted object to scale 1: " + transform);
					transform.localScale = Vector3.one;
					flag = true;
				}
			}
		}
		if (flag)
		{
			this.DirtyScene();
			Debug.Log("Done modifying objects' scale.");
			return;
		}
		Debug.Log("No objects found that need scale modified.");
	}

	// Token: 0x060020CC RID: 8396 RVA: 0x000ED628 File Offset: 0x000EB828
	public void SetPositionWholeNumber()
	{
		bool flag = false;
		foreach (Transform transform in this.targetRoot.GetComponentsInChildren<Transform>(true))
		{
			if (!(transform.gameObject == this.targetRoot))
			{
				Vector3 localPosition = transform.localPosition;
				if (localPosition.x % 1f != 0f || localPosition.y % 1f != 0f || localPosition.y % 1f != 0f)
				{
					Debug.Log(string.Concat(new object[]
					{
						transform,
						" : ",
						localPosition.x,
						",",
						localPosition.y,
						",",
						localPosition.z
					}));
					localPosition.x = Mathf.Round(localPosition.x);
					localPosition.y = Mathf.Round(localPosition.y);
					localPosition.z = Mathf.Round(localPosition.z);
					transform.localPosition = localPosition;
					flag = true;
				}
			}
		}
		if (flag)
		{
			this.DirtyScene();
			Debug.Log("Done modifying objects' position.");
			return;
		}
		Debug.Log("No objects found that need position modified.");
	}

	// Token: 0x060020CD RID: 8397 RVA: 0x000ED778 File Offset: 0x000EB978
	public void SetLabelSizeEvenNumber()
	{
		foreach (UILabel uiwidget in this.targetRoot.GetComponentsInChildren<UILabel>(true))
		{
			if (!(uiwidget.gameObject == this.targetRoot))
			{
				bool flag = uiwidget.width % 2 == 0;
				bool flag2 = uiwidget.height % 2 == 0;
				if (!flag || !flag2)
				{
					Debug.Log(string.Concat(new object[]
					{
						uiwidget.gameObject,
						" : ",
						uiwidget.width,
						",",
						uiwidget.height
					}));
				}
			}
		}
	}

	// Token: 0x04001445 RID: 5189
	public GameObject targetRoot;
}
