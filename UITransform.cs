using System;
using System.Collections.Generic;
using System.Linq;
using RTEditor;
using UnityEngine;

// Token: 0x02000354 RID: 852
public class UITransform : MonoBehaviour
{
	// Token: 0x0600285C RID: 10332 RVA: 0x0011CD20 File Offset: 0x0011AF20
	private void Awake()
	{
		UIEventListener uieventListener = UIEventListener.Get(this.Positon.xInput.gameObject);
		uieventListener.onSelect = (UIEventListener.BoolDelegate)Delegate.Combine(uieventListener.onSelect, new UIEventListener.BoolDelegate(this.xPos));
		UIEventListener uieventListener2 = UIEventListener.Get(this.Positon.yInput.gameObject);
		uieventListener2.onSelect = (UIEventListener.BoolDelegate)Delegate.Combine(uieventListener2.onSelect, new UIEventListener.BoolDelegate(this.yPos));
		UIEventListener uieventListener3 = UIEventListener.Get(this.Positon.zInput.gameObject);
		uieventListener3.onSelect = (UIEventListener.BoolDelegate)Delegate.Combine(uieventListener3.onSelect, new UIEventListener.BoolDelegate(this.zPos));
		UIEventListener uieventListener4 = UIEventListener.Get(this.Rotation.xInput.gameObject);
		uieventListener4.onSelect = (UIEventListener.BoolDelegate)Delegate.Combine(uieventListener4.onSelect, new UIEventListener.BoolDelegate(this.xRot));
		UIEventListener uieventListener5 = UIEventListener.Get(this.Rotation.yInput.gameObject);
		uieventListener5.onSelect = (UIEventListener.BoolDelegate)Delegate.Combine(uieventListener5.onSelect, new UIEventListener.BoolDelegate(this.yRot));
		UIEventListener uieventListener6 = UIEventListener.Get(this.Rotation.zInput.gameObject);
		uieventListener6.onSelect = (UIEventListener.BoolDelegate)Delegate.Combine(uieventListener6.onSelect, new UIEventListener.BoolDelegate(this.zRot));
		UIEventListener uieventListener7 = UIEventListener.Get(this.Scale.xInput.gameObject);
		uieventListener7.onSelect = (UIEventListener.BoolDelegate)Delegate.Combine(uieventListener7.onSelect, new UIEventListener.BoolDelegate(this.xScale));
		UIEventListener uieventListener8 = UIEventListener.Get(this.Scale.yInput.gameObject);
		uieventListener8.onSelect = (UIEventListener.BoolDelegate)Delegate.Combine(uieventListener8.onSelect, new UIEventListener.BoolDelegate(this.yScale));
		UIEventListener uieventListener9 = UIEventListener.Get(this.Scale.zInput.gameObject);
		uieventListener9.onSelect = (UIEventListener.BoolDelegate)Delegate.Combine(uieventListener9.onSelect, new UIEventListener.BoolDelegate(this.zScale));
		EventDelegate.Add(this.Positon.copyClipboard.onClick, new EventDelegate.Callback(this.posCopy));
		EventDelegate.Add(this.Rotation.copyClipboard.onClick, new EventDelegate.Callback(this.rotCopy));
		EventDelegate.Add(this.Scale.copyClipboard.onClick, new EventDelegate.Callback(this.scaleCopy));
		UIEventListener uieventListener10 = UIEventListener.Get(this.Positon.dragX.gameObject);
		uieventListener10.onDrag = (UIEventListener.VectorDelegate)Delegate.Combine(uieventListener10.onDrag, new UIEventListener.VectorDelegate(this.xPosDrag));
		UIEventListener uieventListener11 = UIEventListener.Get(this.Positon.dragY.gameObject);
		uieventListener11.onDrag = (UIEventListener.VectorDelegate)Delegate.Combine(uieventListener11.onDrag, new UIEventListener.VectorDelegate(this.yPosDrag));
		UIEventListener uieventListener12 = UIEventListener.Get(this.Positon.dragZ.gameObject);
		uieventListener12.onDrag = (UIEventListener.VectorDelegate)Delegate.Combine(uieventListener12.onDrag, new UIEventListener.VectorDelegate(this.zPosDrag));
		UIEventListener uieventListener13 = UIEventListener.Get(this.Rotation.dragX.gameObject);
		uieventListener13.onDrag = (UIEventListener.VectorDelegate)Delegate.Combine(uieventListener13.onDrag, new UIEventListener.VectorDelegate(this.xRotDrag));
		UIEventListener uieventListener14 = UIEventListener.Get(this.Rotation.dragY.gameObject);
		uieventListener14.onDrag = (UIEventListener.VectorDelegate)Delegate.Combine(uieventListener14.onDrag, new UIEventListener.VectorDelegate(this.yRotDrag));
		UIEventListener uieventListener15 = UIEventListener.Get(this.Rotation.dragZ.gameObject);
		uieventListener15.onDrag = (UIEventListener.VectorDelegate)Delegate.Combine(uieventListener15.onDrag, new UIEventListener.VectorDelegate(this.zRotDrag));
		UIEventListener uieventListener16 = UIEventListener.Get(this.Scale.dragX.gameObject);
		uieventListener16.onDrag = (UIEventListener.VectorDelegate)Delegate.Combine(uieventListener16.onDrag, new UIEventListener.VectorDelegate(this.xScaleDrag));
		UIEventListener uieventListener17 = UIEventListener.Get(this.Scale.dragY.gameObject);
		uieventListener17.onDrag = (UIEventListener.VectorDelegate)Delegate.Combine(uieventListener17.onDrag, new UIEventListener.VectorDelegate(this.yScaleDrag));
		UIEventListener uieventListener18 = UIEventListener.Get(this.Scale.dragZ.gameObject);
		uieventListener18.onDrag = (UIEventListener.VectorDelegate)Delegate.Combine(uieventListener18.onDrag, new UIEventListener.VectorDelegate(this.zScaleDrag));
		UIEventListener uieventListener19 = UIEventListener.Get(this.Positon.dragX.gameObject);
		uieventListener19.onDragEnd = (UIEventListener.VoidDelegate)Delegate.Combine(uieventListener19.onDragEnd, new UIEventListener.VoidDelegate(this.TransformDragEnd));
		UIEventListener uieventListener20 = UIEventListener.Get(this.Positon.dragY.gameObject);
		uieventListener20.onDragEnd = (UIEventListener.VoidDelegate)Delegate.Combine(uieventListener20.onDragEnd, new UIEventListener.VoidDelegate(this.TransformDragEnd));
		UIEventListener uieventListener21 = UIEventListener.Get(this.Positon.dragZ.gameObject);
		uieventListener21.onDragEnd = (UIEventListener.VoidDelegate)Delegate.Combine(uieventListener21.onDragEnd, new UIEventListener.VoidDelegate(this.TransformDragEnd));
		UIEventListener uieventListener22 = UIEventListener.Get(this.Rotation.dragX.gameObject);
		uieventListener22.onDragEnd = (UIEventListener.VoidDelegate)Delegate.Combine(uieventListener22.onDragEnd, new UIEventListener.VoidDelegate(this.TransformDragEnd));
		UIEventListener uieventListener23 = UIEventListener.Get(this.Rotation.dragY.gameObject);
		uieventListener23.onDragEnd = (UIEventListener.VoidDelegate)Delegate.Combine(uieventListener23.onDragEnd, new UIEventListener.VoidDelegate(this.TransformDragEnd));
		UIEventListener uieventListener24 = UIEventListener.Get(this.Rotation.dragZ.gameObject);
		uieventListener24.onDragEnd = (UIEventListener.VoidDelegate)Delegate.Combine(uieventListener24.onDragEnd, new UIEventListener.VoidDelegate(this.TransformDragEnd));
		UIEventListener uieventListener25 = UIEventListener.Get(this.Scale.dragX.gameObject);
		uieventListener25.onDragEnd = (UIEventListener.VoidDelegate)Delegate.Combine(uieventListener25.onDragEnd, new UIEventListener.VoidDelegate(this.TransformDragEnd));
		UIEventListener uieventListener26 = UIEventListener.Get(this.Scale.dragY.gameObject);
		uieventListener26.onDragEnd = (UIEventListener.VoidDelegate)Delegate.Combine(uieventListener26.onDragEnd, new UIEventListener.VoidDelegate(this.TransformDragEnd));
		UIEventListener uieventListener27 = UIEventListener.Get(this.Scale.dragZ.gameObject);
		uieventListener27.onDragEnd = (UIEventListener.VoidDelegate)Delegate.Combine(uieventListener27.onDragEnd, new UIEventListener.VoidDelegate(this.TransformDragEnd));
	}

	// Token: 0x0600285D RID: 10333 RVA: 0x0011D348 File Offset: 0x0011B548
	private void OnDestroy()
	{
		UIEventListener uieventListener = UIEventListener.Get(this.Positon.xInput.gameObject);
		uieventListener.onSelect = (UIEventListener.BoolDelegate)Delegate.Remove(uieventListener.onSelect, new UIEventListener.BoolDelegate(this.xPos));
		UIEventListener uieventListener2 = UIEventListener.Get(this.Positon.yInput.gameObject);
		uieventListener2.onSelect = (UIEventListener.BoolDelegate)Delegate.Remove(uieventListener2.onSelect, new UIEventListener.BoolDelegate(this.yPos));
		UIEventListener uieventListener3 = UIEventListener.Get(this.Positon.zInput.gameObject);
		uieventListener3.onSelect = (UIEventListener.BoolDelegate)Delegate.Remove(uieventListener3.onSelect, new UIEventListener.BoolDelegate(this.zPos));
		UIEventListener uieventListener4 = UIEventListener.Get(this.Rotation.xInput.gameObject);
		uieventListener4.onSelect = (UIEventListener.BoolDelegate)Delegate.Remove(uieventListener4.onSelect, new UIEventListener.BoolDelegate(this.xRot));
		UIEventListener uieventListener5 = UIEventListener.Get(this.Rotation.yInput.gameObject);
		uieventListener5.onSelect = (UIEventListener.BoolDelegate)Delegate.Remove(uieventListener5.onSelect, new UIEventListener.BoolDelegate(this.yRot));
		UIEventListener uieventListener6 = UIEventListener.Get(this.Rotation.zInput.gameObject);
		uieventListener6.onSelect = (UIEventListener.BoolDelegate)Delegate.Remove(uieventListener6.onSelect, new UIEventListener.BoolDelegate(this.zRot));
		UIEventListener uieventListener7 = UIEventListener.Get(this.Scale.xInput.gameObject);
		uieventListener7.onSelect = (UIEventListener.BoolDelegate)Delegate.Remove(uieventListener7.onSelect, new UIEventListener.BoolDelegate(this.xScale));
		UIEventListener uieventListener8 = UIEventListener.Get(this.Scale.yInput.gameObject);
		uieventListener8.onSelect = (UIEventListener.BoolDelegate)Delegate.Remove(uieventListener8.onSelect, new UIEventListener.BoolDelegate(this.yScale));
		UIEventListener uieventListener9 = UIEventListener.Get(this.Scale.zInput.gameObject);
		uieventListener9.onSelect = (UIEventListener.BoolDelegate)Delegate.Remove(uieventListener9.onSelect, new UIEventListener.BoolDelegate(this.zScale));
		EventDelegate.Remove(this.Positon.copyClipboard.onClick, new EventDelegate.Callback(this.posCopy));
		EventDelegate.Remove(this.Rotation.copyClipboard.onClick, new EventDelegate.Callback(this.rotCopy));
		EventDelegate.Remove(this.Scale.copyClipboard.onClick, new EventDelegate.Callback(this.scaleCopy));
		UIEventListener uieventListener10 = UIEventListener.Get(this.Positon.dragX.gameObject);
		uieventListener10.onDrag = (UIEventListener.VectorDelegate)Delegate.Remove(uieventListener10.onDrag, new UIEventListener.VectorDelegate(this.xPosDrag));
		UIEventListener uieventListener11 = UIEventListener.Get(this.Positon.dragY.gameObject);
		uieventListener11.onDrag = (UIEventListener.VectorDelegate)Delegate.Remove(uieventListener11.onDrag, new UIEventListener.VectorDelegate(this.yPosDrag));
		UIEventListener uieventListener12 = UIEventListener.Get(this.Positon.dragZ.gameObject);
		uieventListener12.onDrag = (UIEventListener.VectorDelegate)Delegate.Remove(uieventListener12.onDrag, new UIEventListener.VectorDelegate(this.zPosDrag));
		UIEventListener uieventListener13 = UIEventListener.Get(this.Rotation.dragX.gameObject);
		uieventListener13.onDrag = (UIEventListener.VectorDelegate)Delegate.Remove(uieventListener13.onDrag, new UIEventListener.VectorDelegate(this.xRotDrag));
		UIEventListener uieventListener14 = UIEventListener.Get(this.Rotation.dragY.gameObject);
		uieventListener14.onDrag = (UIEventListener.VectorDelegate)Delegate.Remove(uieventListener14.onDrag, new UIEventListener.VectorDelegate(this.yRotDrag));
		UIEventListener uieventListener15 = UIEventListener.Get(this.Rotation.dragZ.gameObject);
		uieventListener15.onDrag = (UIEventListener.VectorDelegate)Delegate.Remove(uieventListener15.onDrag, new UIEventListener.VectorDelegate(this.zRotDrag));
		UIEventListener uieventListener16 = UIEventListener.Get(this.Scale.dragX.gameObject);
		uieventListener16.onDrag = (UIEventListener.VectorDelegate)Delegate.Remove(uieventListener16.onDrag, new UIEventListener.VectorDelegate(this.xScaleDrag));
		UIEventListener uieventListener17 = UIEventListener.Get(this.Scale.dragY.gameObject);
		uieventListener17.onDrag = (UIEventListener.VectorDelegate)Delegate.Remove(uieventListener17.onDrag, new UIEventListener.VectorDelegate(this.yScaleDrag));
		UIEventListener uieventListener18 = UIEventListener.Get(this.Scale.dragZ.gameObject);
		uieventListener18.onDrag = (UIEventListener.VectorDelegate)Delegate.Remove(uieventListener18.onDrag, new UIEventListener.VectorDelegate(this.zScaleDrag));
		UIEventListener uieventListener19 = UIEventListener.Get(this.Positon.dragX.gameObject);
		uieventListener19.onDragEnd = (UIEventListener.VoidDelegate)Delegate.Remove(uieventListener19.onDragEnd, new UIEventListener.VoidDelegate(this.TransformDragEnd));
		UIEventListener uieventListener20 = UIEventListener.Get(this.Positon.dragY.gameObject);
		uieventListener20.onDragEnd = (UIEventListener.VoidDelegate)Delegate.Remove(uieventListener20.onDragEnd, new UIEventListener.VoidDelegate(this.TransformDragEnd));
		UIEventListener uieventListener21 = UIEventListener.Get(this.Positon.dragZ.gameObject);
		uieventListener21.onDragEnd = (UIEventListener.VoidDelegate)Delegate.Remove(uieventListener21.onDragEnd, new UIEventListener.VoidDelegate(this.TransformDragEnd));
		UIEventListener uieventListener22 = UIEventListener.Get(this.Rotation.dragX.gameObject);
		uieventListener22.onDragEnd = (UIEventListener.VoidDelegate)Delegate.Remove(uieventListener22.onDragEnd, new UIEventListener.VoidDelegate(this.TransformDragEnd));
		UIEventListener uieventListener23 = UIEventListener.Get(this.Rotation.dragY.gameObject);
		uieventListener23.onDragEnd = (UIEventListener.VoidDelegate)Delegate.Remove(uieventListener23.onDragEnd, new UIEventListener.VoidDelegate(this.TransformDragEnd));
		UIEventListener uieventListener24 = UIEventListener.Get(this.Rotation.dragZ.gameObject);
		uieventListener24.onDragEnd = (UIEventListener.VoidDelegate)Delegate.Remove(uieventListener24.onDragEnd, new UIEventListener.VoidDelegate(this.TransformDragEnd));
		UIEventListener uieventListener25 = UIEventListener.Get(this.Scale.dragX.gameObject);
		uieventListener25.onDragEnd = (UIEventListener.VoidDelegate)Delegate.Remove(uieventListener25.onDragEnd, new UIEventListener.VoidDelegate(this.TransformDragEnd));
		UIEventListener uieventListener26 = UIEventListener.Get(this.Scale.dragY.gameObject);
		uieventListener26.onDragEnd = (UIEventListener.VoidDelegate)Delegate.Remove(uieventListener26.onDragEnd, new UIEventListener.VoidDelegate(this.TransformDragEnd));
		UIEventListener uieventListener27 = UIEventListener.Get(this.Scale.dragZ.gameObject);
		uieventListener27.onDragEnd = (UIEventListener.VoidDelegate)Delegate.Remove(uieventListener27.onDragEnd, new UIEventListener.VoidDelegate(this.TransformDragEnd));
	}

	// Token: 0x0600285E RID: 10334 RVA: 0x0011D96D File Offset: 0x0011BB6D
	private void OnEnable()
	{
		this.UpdateInputs();
	}

	// Token: 0x0600285F RID: 10335 RVA: 0x0011D975 File Offset: 0x0011BB75
	private void xPos(GameObject go, bool select)
	{
		if (!select)
		{
			this.Set(this.Positon.xInput.value, UITransform.TransformValue.xPos);
		}
	}

	// Token: 0x06002860 RID: 10336 RVA: 0x0011D991 File Offset: 0x0011BB91
	private void yPos(GameObject go, bool select)
	{
		if (!select)
		{
			this.Set(this.Positon.yInput.value, UITransform.TransformValue.yPos);
		}
	}

	// Token: 0x06002861 RID: 10337 RVA: 0x0011D9AD File Offset: 0x0011BBAD
	private void zPos(GameObject go, bool select)
	{
		if (!select)
		{
			this.Set(this.Positon.zInput.value, UITransform.TransformValue.zPos);
		}
	}

	// Token: 0x06002862 RID: 10338 RVA: 0x0011D9C9 File Offset: 0x0011BBC9
	private void xRot(GameObject go, bool select)
	{
		if (!select)
		{
			this.Set(this.Rotation.xInput.value, UITransform.TransformValue.xRot);
		}
	}

	// Token: 0x06002863 RID: 10339 RVA: 0x0011D9E5 File Offset: 0x0011BBE5
	private void yRot(GameObject go, bool select)
	{
		if (!select)
		{
			this.Set(this.Rotation.yInput.value, UITransform.TransformValue.yRot);
		}
	}

	// Token: 0x06002864 RID: 10340 RVA: 0x0011DA01 File Offset: 0x0011BC01
	private void zRot(GameObject go, bool select)
	{
		if (!select)
		{
			this.Set(this.Rotation.zInput.value, UITransform.TransformValue.zRot);
		}
	}

	// Token: 0x06002865 RID: 10341 RVA: 0x0011DA1D File Offset: 0x0011BC1D
	private void xScale(GameObject go, bool select)
	{
		if (!select)
		{
			this.Set(this.Scale.xInput.value, UITransform.TransformValue.xScale);
		}
	}

	// Token: 0x06002866 RID: 10342 RVA: 0x0011DA39 File Offset: 0x0011BC39
	private void yScale(GameObject go, bool select)
	{
		if (!select)
		{
			this.Set(this.Scale.yInput.value, UITransform.TransformValue.yScale);
		}
	}

	// Token: 0x06002867 RID: 10343 RVA: 0x0011DA55 File Offset: 0x0011BC55
	private void zScale(GameObject go, bool select)
	{
		if (!select)
		{
			this.Set(this.Scale.zInput.value, UITransform.TransformValue.zScale);
		}
	}

	// Token: 0x06002868 RID: 10344 RVA: 0x0011DA71 File Offset: 0x0011BC71
	private void posCopy()
	{
		this.Copy(this.Positon);
	}

	// Token: 0x06002869 RID: 10345 RVA: 0x0011DA7F File Offset: 0x0011BC7F
	private void rotCopy()
	{
		this.Copy(this.Rotation);
	}

	// Token: 0x0600286A RID: 10346 RVA: 0x0011DA8D File Offset: 0x0011BC8D
	private void scaleCopy()
	{
		this.Copy(this.Scale);
	}

	// Token: 0x0600286B RID: 10347 RVA: 0x0011DA9C File Offset: 0x0011BC9C
	private void Copy(UITransform.TransformComponent component)
	{
		TTSUtilities.CopyToClipboard(string.Concat(new string[]
		{
			"{",
			component.xInput.value,
			", ",
			component.yInput.value,
			", ",
			component.zInput.value,
			"}"
		}));
	}

	// Token: 0x0600286C RID: 10348 RVA: 0x0011DB03 File Offset: 0x0011BD03
	private void xPosDrag(GameObject go, Vector2 delta)
	{
		this.Set(delta.x * this.posDragMulti, UITransform.TransformValue.xPos, true);
	}

	// Token: 0x0600286D RID: 10349 RVA: 0x0011DB1A File Offset: 0x0011BD1A
	private void yPosDrag(GameObject go, Vector2 delta)
	{
		this.Set(delta.x * this.posDragMulti, UITransform.TransformValue.yPos, true);
	}

	// Token: 0x0600286E RID: 10350 RVA: 0x0011DB31 File Offset: 0x0011BD31
	private void zPosDrag(GameObject go, Vector2 delta)
	{
		this.Set(delta.x * this.posDragMulti, UITransform.TransformValue.zPos, true);
	}

	// Token: 0x0600286F RID: 10351 RVA: 0x0011DB48 File Offset: 0x0011BD48
	private void xRotDrag(GameObject go, Vector2 delta)
	{
		this.Set(delta.x * this.rotDragMulti, UITransform.TransformValue.xRot, true);
	}

	// Token: 0x06002870 RID: 10352 RVA: 0x0011DB5F File Offset: 0x0011BD5F
	private void yRotDrag(GameObject go, Vector2 delta)
	{
		this.Set(delta.x * this.rotDragMulti, UITransform.TransformValue.yRot, true);
	}

	// Token: 0x06002871 RID: 10353 RVA: 0x0011DB76 File Offset: 0x0011BD76
	private void zRotDrag(GameObject go, Vector2 delta)
	{
		this.Set(delta.x * this.rotDragMulti, UITransform.TransformValue.zRot, true);
	}

	// Token: 0x06002872 RID: 10354 RVA: 0x0011DB8D File Offset: 0x0011BD8D
	private void xScaleDrag(GameObject go, Vector2 delta)
	{
		this.Set(delta.x * this.scaleDragMulti, UITransform.TransformValue.xScale, true);
	}

	// Token: 0x06002873 RID: 10355 RVA: 0x0011DBA4 File Offset: 0x0011BDA4
	private void yScaleDrag(GameObject go, Vector2 delta)
	{
		this.Set(delta.x * this.scaleDragMulti, UITransform.TransformValue.yScale, true);
	}

	// Token: 0x06002874 RID: 10356 RVA: 0x0011DBBB File Offset: 0x0011BDBB
	private void zScaleDrag(GameObject go, Vector2 delta)
	{
		this.Set(delta.x * this.scaleDragMulti, UITransform.TransformValue.zScale, true);
	}

	// Token: 0x06002875 RID: 10357 RVA: 0x0011DBD2 File Offset: 0x0011BDD2
	private void TransformDragEnd(GameObject go)
	{
		this.Sync();
	}

	// Token: 0x06002876 RID: 10358 RVA: 0x0011DBDC File Offset: 0x0011BDDC
	private void Set(string value, UITransform.TransformValue transformValue)
	{
		float value2 = 0f;
		if (!float.TryParse(value, out value2))
		{
			return;
		}
		this.Set(value2, transformValue, false);
	}

	// Token: 0x06002877 RID: 10359 RVA: 0x0011DC04 File Offset: 0x0011BE04
	private void Set(float value, UITransform.TransformValue transformValue, bool Additive = false)
	{
		List<GameObject> selectedObjects = this.GetSelectedObjects();
		for (int i = 0; i < selectedObjects.Count; i++)
		{
			Transform transform = selectedObjects[i].transform;
			NetworkPhysicsObject component = transform.GetComponent<NetworkPhysicsObject>();
			switch (transformValue)
			{
			case UITransform.TransformValue.xPos:
				if (!Additive)
				{
					transform.position = new Vector3(value, transform.position.y, transform.position.z);
				}
				else
				{
					transform.position = new Vector3(value + transform.position.x, transform.position.y, transform.position.z);
				}
				break;
			case UITransform.TransformValue.yPos:
				if (!Additive)
				{
					transform.position = new Vector3(transform.position.x, value, transform.position.z);
				}
				else
				{
					transform.position = new Vector3(transform.position.x, value + transform.position.y, transform.position.z);
				}
				break;
			case UITransform.TransformValue.zPos:
				if (!Additive)
				{
					transform.position = new Vector3(transform.position.x, transform.position.y, value);
				}
				else
				{
					transform.position = new Vector3(transform.position.x, transform.position.y, value + transform.position.z);
				}
				break;
			case UITransform.TransformValue.xRot:
				if (!Additive)
				{
					transform.eulerAngles = new Vector3(value, transform.eulerAngles.y, transform.eulerAngles.z);
				}
				else
				{
					transform.Rotate(value, 0f, 0f, Space.World);
				}
				break;
			case UITransform.TransformValue.yRot:
				if (!Additive)
				{
					transform.eulerAngles = new Vector3(transform.eulerAngles.x, value, transform.eulerAngles.z);
				}
				else
				{
					transform.Rotate(0f, value, 0f, Space.World);
				}
				break;
			case UITransform.TransformValue.zRot:
				if (!Additive)
				{
					transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, value);
				}
				else
				{
					transform.Rotate(0f, 0f, value, Space.World);
				}
				break;
			case UITransform.TransformValue.xScale:
				if (component)
				{
					if (!Additive)
					{
						component.SetScale(new Vector3(value, component.Scale.y, component.Scale.z), true);
					}
					else
					{
						component.SetScale(new Vector3(value + component.Scale.x, component.Scale.y, component.Scale.z), true);
					}
				}
				else if (!Additive)
				{
					transform.localScale = new Vector3(value, transform.localScale.y, transform.localScale.z);
				}
				else
				{
					transform.localScale = new Vector3(value + transform.localScale.x, transform.localScale.y, transform.localScale.z);
				}
				break;
			case UITransform.TransformValue.yScale:
				if (component)
				{
					if (!Additive)
					{
						component.SetScale(new Vector3(component.Scale.x, value, component.Scale.z), true);
					}
					else
					{
						component.SetScale(new Vector3(component.Scale.x, value + component.Scale.y, component.Scale.z), true);
					}
				}
				else if (!Additive)
				{
					transform.localScale = new Vector3(transform.localScale.x, value, transform.localScale.z);
				}
				else
				{
					transform.localScale = new Vector3(transform.localScale.x, value + transform.localScale.y, transform.localScale.z);
				}
				break;
			case UITransform.TransformValue.zScale:
				if (component)
				{
					if (!Additive)
					{
						component.SetScale(new Vector3(component.Scale.x, component.Scale.y, value), true);
					}
					else
					{
						component.SetScale(new Vector3(component.Scale.x, component.Scale.y, value + component.Scale.z), true);
					}
				}
				else if (!Additive)
				{
					transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y, value);
				}
				else
				{
					transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y, value + transform.localScale.z);
				}
				break;
			}
		}
		if (!Additive)
		{
			this.Sync();
		}
		GizmoScript.ClientSyncTransform(transformValue == UITransform.TransformValue.xPos || transformValue == UITransform.TransformValue.yPos || transformValue == UITransform.TransformValue.zPos, transformValue == UITransform.TransformValue.xRot || transformValue == UITransform.TransformValue.yRot || transformValue == UITransform.TransformValue.zRot, transformValue == UITransform.TransformValue.xScale || transformValue == UITransform.TransformValue.yScale || transformValue == UITransform.TransformValue.zScale);
	}

	// Token: 0x06002878 RID: 10360 RVA: 0x000025B8 File Offset: 0x000007B8
	private void Sync()
	{
	}

	// Token: 0x06002879 RID: 10361 RVA: 0x0011E0C7 File Offset: 0x0011C2C7
	private GameObject GetSelectedObject()
	{
		return MonoSingletonBase<EditorObjectSelection>.Instance.LastSelectedGameObject;
	}

	// Token: 0x0600287A RID: 10362 RVA: 0x0011E0D3 File Offset: 0x0011C2D3
	private List<GameObject> GetSelectedObjects()
	{
		return MonoSingletonBase<EditorObjectSelection>.Instance.SelectedGameObjects.ToList<GameObject>();
	}

	// Token: 0x0600287B RID: 10363 RVA: 0x0011E0E4 File Offset: 0x0011C2E4
	private void Update()
	{
		if (!this.GetSelectedObject())
		{
			base.gameObject.SetActive(false);
			return;
		}
		this.UpdateInputs();
	}

	// Token: 0x0600287C RID: 10364 RVA: 0x0011E108 File Offset: 0x0011C308
	private void UpdateInputs()
	{
		GameObject selectedObject = this.GetSelectedObject();
		if (!selectedObject)
		{
			return;
		}
		NetworkPhysicsObject component = selectedObject.GetComponent<NetworkPhysicsObject>();
		this.Positon.SetInputs(selectedObject.transform.position);
		this.Rotation.SetInputs(selectedObject.transform.eulerAngles);
		if (component)
		{
			this.Scale.SetInputs(component.Scale);
			return;
		}
		this.Scale.SetInputs(selectedObject.transform.localScale);
	}

	// Token: 0x04001A9D RID: 6813
	public UITransform.TransformComponent Positon;

	// Token: 0x04001A9E RID: 6814
	public UITransform.TransformComponent Rotation;

	// Token: 0x04001A9F RID: 6815
	public UITransform.TransformComponent Scale;

	// Token: 0x04001AA0 RID: 6816
	private float posDragMulti = 0.025f;

	// Token: 0x04001AA1 RID: 6817
	private float rotDragMulti = 0.2f;

	// Token: 0x04001AA2 RID: 6818
	private float scaleDragMulti = 0.01f;

	// Token: 0x0200079A RID: 1946
	[Serializable]
	public class TransformComponent
	{
		// Token: 0x06003F59 RID: 16217 RVA: 0x00181334 File Offset: 0x0017F534
		public void SetInputs(Vector3 v)
		{
			if (!this.xInput.isSelected)
			{
				this.xInput.value = this.GetString(v.x);
			}
			if (!this.yInput.isSelected)
			{
				this.yInput.value = this.GetString(v.y);
			}
			if (!this.zInput.isSelected)
			{
				this.zInput.value = this.GetString(v.z);
			}
		}

		// Token: 0x06003F5A RID: 16218 RVA: 0x001813AD File Offset: 0x0017F5AD
		private string GetString(float value)
		{
			value = (float)Math.Round((double)value, 7);
			return value.ToString("F2");
		}

		// Token: 0x04002CB0 RID: 11440
		public UIInput xInput;

		// Token: 0x04002CB1 RID: 11441
		public UIInput yInput;

		// Token: 0x04002CB2 RID: 11442
		public UIInput zInput;

		// Token: 0x04002CB3 RID: 11443
		public Collider2D dragX;

		// Token: 0x04002CB4 RID: 11444
		public Collider2D dragY;

		// Token: 0x04002CB5 RID: 11445
		public Collider2D dragZ;

		// Token: 0x04002CB6 RID: 11446
		public UIButton copyClipboard;
	}

	// Token: 0x0200079B RID: 1947
	private enum TransformValue
	{
		// Token: 0x04002CB8 RID: 11448
		xPos,
		// Token: 0x04002CB9 RID: 11449
		yPos,
		// Token: 0x04002CBA RID: 11450
		zPos,
		// Token: 0x04002CBB RID: 11451
		xRot,
		// Token: 0x04002CBC RID: 11452
		yRot,
		// Token: 0x04002CBD RID: 11453
		zRot,
		// Token: 0x04002CBE RID: 11454
		xScale,
		// Token: 0x04002CBF RID: 11455
		yScale,
		// Token: 0x04002CC0 RID: 11456
		zScale
	}
}
