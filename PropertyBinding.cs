using System;
using UnityEngine;

// Token: 0x02000063 RID: 99
[ExecuteInEditMode]
[AddComponentMenu("NGUI/Internal/Property Binding")]
public class PropertyBinding : MonoBehaviour
{
	// Token: 0x06000404 RID: 1028 RVA: 0x0001D6FA File Offset: 0x0001B8FA
	private void Start()
	{
		this.UpdateTarget();
		if (Application.isPlaying && this.update == PropertyBinding.UpdateCondition.OnStart)
		{
			base.enabled = false;
		}
	}

	// Token: 0x06000405 RID: 1029 RVA: 0x0001D718 File Offset: 0x0001B918
	private void Update()
	{
		if (this.update == PropertyBinding.UpdateCondition.OnUpdate)
		{
			this.UpdateTarget();
		}
	}

	// Token: 0x06000406 RID: 1030 RVA: 0x0001D729 File Offset: 0x0001B929
	private void LateUpdate()
	{
		if (this.update == PropertyBinding.UpdateCondition.OnLateUpdate)
		{
			this.UpdateTarget();
		}
	}

	// Token: 0x06000407 RID: 1031 RVA: 0x0001D73A File Offset: 0x0001B93A
	private void FixedUpdate()
	{
		if (this.update == PropertyBinding.UpdateCondition.OnFixedUpdate)
		{
			this.UpdateTarget();
		}
	}

	// Token: 0x06000408 RID: 1032 RVA: 0x0001D74B File Offset: 0x0001B94B
	private void OnValidate()
	{
		if (this.source != null)
		{
			this.source.Reset();
		}
		if (this.target != null)
		{
			this.target.Reset();
		}
	}

	// Token: 0x06000409 RID: 1033 RVA: 0x0001D774 File Offset: 0x0001B974
	[ContextMenu("Update Now")]
	public void UpdateTarget()
	{
		if (this.source != null && this.target != null && this.source.isValid && this.target.isValid)
		{
			if (this.direction == PropertyBinding.Direction.SourceUpdatesTarget)
			{
				this.target.Set(this.source.Get());
				return;
			}
			if (this.direction == PropertyBinding.Direction.TargetUpdatesSource)
			{
				this.source.Set(this.target.Get());
				return;
			}
			if (this.source.GetPropertyType() == this.target.GetPropertyType())
			{
				object obj = this.source.Get();
				if (this.mLastValue == null || !this.mLastValue.Equals(obj))
				{
					this.mLastValue = obj;
					this.target.Set(obj);
					return;
				}
				obj = this.target.Get();
				if (!this.mLastValue.Equals(obj))
				{
					this.mLastValue = obj;
					this.source.Set(obj);
				}
			}
		}
	}

	// Token: 0x040002DC RID: 732
	public PropertyReference source;

	// Token: 0x040002DD RID: 733
	public PropertyReference target;

	// Token: 0x040002DE RID: 734
	public PropertyBinding.Direction direction;

	// Token: 0x040002DF RID: 735
	public PropertyBinding.UpdateCondition update = PropertyBinding.UpdateCondition.OnUpdate;

	// Token: 0x040002E0 RID: 736
	public bool editMode;

	// Token: 0x040002E1 RID: 737
	private object mLastValue;

	// Token: 0x0200052D RID: 1325
	public enum UpdateCondition
	{
		// Token: 0x0400241B RID: 9243
		OnStart,
		// Token: 0x0400241C RID: 9244
		OnUpdate,
		// Token: 0x0400241D RID: 9245
		OnLateUpdate,
		// Token: 0x0400241E RID: 9246
		OnFixedUpdate
	}

	// Token: 0x0200052E RID: 1326
	public enum Direction
	{
		// Token: 0x04002420 RID: 9248
		SourceUpdatesTarget,
		// Token: 0x04002421 RID: 9249
		TargetUpdatesSource,
		// Token: 0x04002422 RID: 9250
		BiDirectional
	}
}
