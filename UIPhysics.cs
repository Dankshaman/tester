using System;
using UnityEngine;

// Token: 0x02000315 RID: 789
public class UIPhysics : UITabMenu
{
	// Token: 0x06002663 RID: 9827 RVA: 0x00111FD8 File Offset: 0x001101D8
	private void Awake()
	{
		this.RigidbodyTab.SetActive(true);
		this.SelectedTab = this.RigidbodyTab;
		this.usegravity = base.NameToUIToggle("01 UseGravity");
		this.mass = base.NameToUIInput("05 Mass");
		this.drag = base.NameToUIInput("06 Drag");
		this.angulardrag = base.NameToUIInput("07 Angular Drag");
		this.MaterialTab.SetActive(true);
		this.SelectedTab = this.MaterialTab;
		this.staticfriction = base.NameToUIInput("05 Static Friction");
		this.dynamicfriction = base.NameToUIInput("06 Dynamic Friction");
		this.bounciness = base.NameToUIInput("07 Bounciness");
		this.MaterialTab.SetActive(false);
		this.SelectedTab = this.RigidbodyTab;
	}

	// Token: 0x06002664 RID: 9828 RVA: 0x001120A4 File Offset: 0x001102A4
	private void OnEnable()
	{
		if (PlayerScript.Pointer && PlayerScript.PointerScript.InfoObject)
		{
			NetworkPhysicsObject component = PlayerScript.PointerScript.InfoObject.GetComponent<NetworkPhysicsObject>();
			this.SourceID = component.ID;
			UIHighlightTargets component2 = base.GetComponent<UIHighlightTargets>();
			component2.Reset();
			component2.Add(component.gameObject);
			this.usegravity.value = component.GetUseGravity();
			this.mass.value = component.GetMass().ToString();
			this.drag.value = component.GetDrag().ToString();
			this.angulardrag.value = component.GetAngularDrag().ToString();
			PhysicMaterial material = component.GetComponentInChildren<Collider>().material;
			this.staticfriction.value = material.staticFriction.ToString();
			this.dynamicfriction.value = material.dynamicFriction.ToString();
			this.bounciness.value = material.bounciness.ToString();
			return;
		}
		base.gameObject.SetActive(false);
	}

	// Token: 0x06002665 RID: 9829 RVA: 0x001121C8 File Offset: 0x001103C8
	public void Apply()
	{
		if (!PlayerScript.Pointer)
		{
			return;
		}
		this.SelectedTab = null;
		if (this.RigidbodyTab.activeSelf)
		{
			this.SelectedTab = this.RigidbodyTab;
		}
		if (this.MaterialTab.activeSelf)
		{
			this.SelectedTab = this.MaterialTab;
		}
		bool value = this.usegravity.value;
		float num = base.UIInputToFloat(this.mass);
		float num2 = base.UIInputToFloat(this.drag);
		float angularDrag = base.UIInputToFloat(this.angulardrag);
		float staticFriction = Mathf.Clamp01(base.UIInputToFloat(this.staticfriction));
		float dynamicFriction = Mathf.Clamp01(base.UIInputToFloat(this.dynamicfriction));
		float num3 = Mathf.Clamp01(base.UIInputToFloat(this.bounciness));
		RigidbodyState rigidbodyState = new RigidbodyState();
		rigidbodyState.UseGravity = value;
		rigidbodyState.Mass = num;
		rigidbodyState.Drag = num2;
		rigidbodyState.AngularDrag = angularDrag;
		PhysicsMaterialState physicsMaterialState = new PhysicsMaterialState();
		physicsMaterialState.StaticFriction = staticFriction;
		physicsMaterialState.DynamicFriction = dynamicFriction;
		physicsMaterialState.Bounciness = num3;
		PlayerScript.PointerScript.SetPhysics(this.SourceID, rigidbodyState, physicsMaterialState);
	}

	// Token: 0x04001902 RID: 6402
	public GameObject RigidbodyTab;

	// Token: 0x04001903 RID: 6403
	public GameObject MaterialTab;

	// Token: 0x04001904 RID: 6404
	private int SourceID;

	// Token: 0x04001905 RID: 6405
	private UIToggle usegravity;

	// Token: 0x04001906 RID: 6406
	private UIInput mass;

	// Token: 0x04001907 RID: 6407
	private UIInput drag;

	// Token: 0x04001908 RID: 6408
	private UIInput angulardrag;

	// Token: 0x04001909 RID: 6409
	private UIInput staticfriction;

	// Token: 0x0400190A RID: 6410
	private UIInput dynamicfriction;

	// Token: 0x0400190B RID: 6411
	private UIInput bounciness;
}
