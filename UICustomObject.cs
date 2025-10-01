using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020002B9 RID: 697
public abstract class UICustomObject<T> : Singleton<!0> where T : MonoBehaviour
{
	// Token: 0x1700047A RID: 1146
	// (set) Token: 0x060022A0 RID: 8864 RVA: 0x000F7B34 File Offset: 0x000F5D34
	public int NumberInQueue
	{
		set
		{
			if (!this.QueueLabel)
			{
				return;
			}
			if (value > 0)
			{
				this.QueueLabel.gameObject.SetActive(true);
				this.QueueLabel.text = string.Format("({0})", value);
				return;
			}
			this.QueueLabel.gameObject.SetActive(false);
		}
	}

	// Token: 0x060022A1 RID: 8865 RVA: 0x000F7B94 File Offset: 0x000F5D94
	protected virtual void OnEnable()
	{
		if (this.CustomObjectQueue.Count > 0)
		{
			this.TargetCustomObject = this.CustomObjectQueue[0];
		}
		if (!this.TargetCustomObject)
		{
			this.Close();
			return;
		}
		base.GetComponent<UIHighlightTargets>().Reset();
		base.GetComponent<UIHighlightTargets>().Add(this.TargetCustomObject.gameObject);
	}

	// Token: 0x060022A2 RID: 8866 RVA: 0x000F7BF8 File Offset: 0x000F5DF8
	public void Queue(CustomObject cm)
	{
		if (cm.DummyObject)
		{
			return;
		}
		if (!this.CustomObjectQueue.Contains(cm))
		{
			this.CustomObjectQueue.Add(cm);
			this.NumberInQueue = this.CustomObjectQueue.Count - 1;
			base.gameObject.SetActive(true);
		}
	}

	// Token: 0x060022A3 RID: 8867 RVA: 0x000F7C4C File Offset: 0x000F5E4C
	public void Close()
	{
		if (this.CustomObjectQueue.Contains(this.TargetCustomObject))
		{
			this.CustomObjectQueue.Remove(this.TargetCustomObject);
		}
		if (this.TargetCustomObject)
		{
			this.TargetCustomObject.bCustomUI = false;
			base.GetComponent<UIHighlightTargets>().Remove(this.TargetCustomObject.gameObject);
		}
		this.NumberInQueue = this.CustomObjectQueue.Count - 1;
		this.TargetCustomObject = null;
		base.gameObject.SetActive(false);
		if (this.CustomObjectQueue.Count > 0)
		{
			base.gameObject.SetActive(true);
		}
	}

	// Token: 0x060022A4 RID: 8868 RVA: 0x000F7CED File Offset: 0x000F5EED
	public void Cancel()
	{
		if (this.TargetCustomObject)
		{
			this.TargetCustomObject.Cancel();
		}
		this.Close();
	}

	// Token: 0x060022A5 RID: 8869 RVA: 0x000025B8 File Offset: 0x000007B8
	public virtual void Import()
	{
	}

	// Token: 0x060022A6 RID: 8870 RVA: 0x000F7D0D File Offset: 0x000F5F0D
	private void Update()
	{
		if (this.TargetCustomObject == null)
		{
			this.Close();
			return;
		}
	}

	// Token: 0x060022A7 RID: 8871 RVA: 0x000F7D24 File Offset: 0x000F5F24
	protected void CheckUpdateMatchingCustomObjects()
	{
		if (!SystemConsole.CheckForMatchingCustomObjects)
		{
			return;
		}
		NetworkPhysicsObject component = this.TargetCustomObject.GetComponent<NetworkPhysicsObject>();
		string guid = component.GUID;
		ObjectState oldOS = NetworkSingleton<ManagerPhysicsObject>.Instance.SaveObjectState(component);
		SaveState SS = NetworkSingleton<ManagerPhysicsObject>.Instance.CurrentState();
		Action <>9__1;
		Wait.Frames(delegate
		{
			ObjectState newOS = NetworkSingleton<ManagerPhysicsObject>.Instance.SaveObjectState(NetworkSingleton<ManagerPhysicsObject>.Instance.NPOFromGUID(guid));
			int num = SaveScript.UpdateMatchingCustomObjects(SS, oldOS, newOS);
			if (num > 1)
			{
				string description = Language.Translate("Found {0} matching Custom Object(s), do you also want to update them to match?", (num - 1).ToString());
				string leftButtonText = "Update";
				string rightButtonText = "Cancel";
				Action leftButtonFunc;
				if ((leftButtonFunc = <>9__1) == null)
				{
					leftButtonFunc = (<>9__1 = delegate()
					{
						NetworkSingleton<ManagerPhysicsObject>.Instance.LoadSaveState(SS, false, true);
					});
				}
				UIDialog.Show(description, leftButtonText, rightButtonText, leftButtonFunc, null);
			}
		}, 1);
	}

	// Token: 0x040015E6 RID: 5606
	protected List<CustomObject> CustomObjectQueue = new List<CustomObject>();

	// Token: 0x040015E7 RID: 5607
	protected CustomObject TargetCustomObject;

	// Token: 0x040015E8 RID: 5608
	public UILabel QueueLabel;
}
