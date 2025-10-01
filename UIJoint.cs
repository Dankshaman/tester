using System;
using UnityEngine;

// Token: 0x020002F7 RID: 759
public class UIJoint : UITabMenu
{
	// Token: 0x060024BC RID: 9404 RVA: 0x00103314 File Offset: 0x00101514
	private void OnEnable()
	{
		if (PlayerScript.Pointer && PlayerScript.PointerScript.InfoObject)
		{
			this.HighlightJoint.Reset();
			this.HighlightAttached.Reset();
			this.HighlightJoint.Add(PlayerScript.PointerScript.InfoObject);
			this.SourceID = PlayerScript.PointerScript.InfoObject.GetComponent<NetworkPhysicsObject>().ID;
			Joint component = PlayerScript.PointerScript.InfoObject.GetComponent<Joint>();
			if (PlayerScript.PointerScript.InfoObject.GetComponent<Joint>() && PlayerScript.PointerScript.InfoObject.GetComponent<Joint>().connectedBody)
			{
				this.TargetID = PlayerScript.PointerScript.InfoObject.GetComponent<Joint>().connectedBody.GetComponent<NetworkPhysicsObject>().ID;
				this.HighlightAttached.Add(component.connectedBody.gameObject);
				for (int i = 0; i < 3; i++)
				{
					if (i == 0)
					{
						this.SelectedTab = this.Fixed;
					}
					if (i == 1)
					{
						this.SelectedTab = this.Hinge;
					}
					if (i == 2)
					{
						this.SelectedTab = this.Spring;
					}
					base.SetValue("01 Collision", component.enableCollision);
					base.SetValue("05 Break Force", component.breakForce);
					base.SetValue("06 Break Torque", component.breakTorque);
				}
				int group = this.tabFixed.group;
				this.tabFixed.group = 0;
				this.tabHinge.group = 0;
				this.tabSpring.group = 0;
				this.tabFixed.value = (component is FixedJoint);
				this.tabHinge.value = (component is HingeJoint);
				this.tabSpring.value = (component is SpringJoint);
				this.tabFixed.group = group;
				this.tabHinge.group = group;
				this.tabSpring.group = group;
				this.Fixed.SetActive(component is FixedJoint);
				this.Hinge.SetActive(component is HingeJoint);
				this.Spring.SetActive(component is SpringJoint);
				if (component is HingeJoint)
				{
					HingeJoint hingeJoint = component as HingeJoint;
					this.SelectedTab = this.Hinge;
					base.SetValue("02 Axis", hingeJoint.axis);
					base.SetValue("03 Anchor", hingeJoint.anchor);
					base.SetValue("07 Motor Force", hingeJoint.motor.force);
					base.SetValue("08 Motor Velocity", hingeJoint.motor.targetVelocity);
					base.SetValue("09 Free Spin", hingeJoint.motor.freeSpin);
				}
				if (component is SpringJoint)
				{
					SpringJoint springJoint = component as SpringJoint;
					this.SelectedTab = this.Spring;
					base.SetValue("07 Spring", springJoint.spring);
					base.SetValue("08 Damper", springJoint.damper);
					base.SetValue("09 Max Distance", springJoint.maxDistance);
					base.SetValue("10 Min Distance", springJoint.minDistance);
				}
				this.SetSelectedTab();
				return;
			}
		}
		else
		{
			base.gameObject.SetActive(false);
		}
	}

	// Token: 0x060024BD RID: 9405 RVA: 0x0010364C File Offset: 0x0010184C
	private void SetSelectedTab()
	{
		this.SelectedTab = null;
		if (this.Fixed.activeSelf)
		{
			this.SelectedTab = this.Fixed;
		}
		if (this.Hinge.activeSelf)
		{
			this.SelectedTab = this.Hinge;
		}
		if (this.Spring.activeSelf)
		{
			this.SelectedTab = this.Spring;
		}
	}

	// Token: 0x060024BE RID: 9406 RVA: 0x001036AC File Offset: 0x001018AC
	public void Apply()
	{
		if (PlayerScript.Pointer)
		{
			if (this.Fixed.activeSelf)
			{
				JointFixedState jointState = new JointFixedState();
				this.ApplyFixed(jointState);
				PlayerScript.PointerScript.JointFixed(this.TargetID, this.SourceID, jointState);
				return;
			}
			if (this.Hinge.activeSelf)
			{
				JointHingeState jointHingeState = new JointHingeState();
				this.ApplyFixed(jointHingeState);
				jointHingeState.Axis = new VectorState(base.NameToVector3("02 Axis"));
				jointHingeState.Anchor = new VectorState(base.NameToVector3("03 Anchor"));
				jointHingeState.Motor.force = base.NameToFloat("07 Motor Force");
				jointHingeState.Motor.targetVelocity = base.NameToFloat("08 Motor Velocity");
				jointHingeState.Motor.freeSpin = base.NameToBool("09 Free Spin");
				PlayerScript.PointerScript.JointHinge(this.TargetID, this.SourceID, jointHingeState);
				return;
			}
			if (this.Spring.activeSelf)
			{
				JointSpringState jointSpringState = new JointSpringState();
				this.ApplyFixed(jointSpringState);
				jointSpringState.Spring = base.NameToFloat("07 Spring");
				jointSpringState.Damper = base.NameToFloat("08 Damper");
				jointSpringState.MaxDistance = base.NameToFloat("09 Max Distance");
				jointSpringState.MinDistance = base.NameToFloat("10 Min Distance");
				PlayerScript.PointerScript.JointSpring(this.TargetID, this.SourceID, jointSpringState);
			}
		}
	}

	// Token: 0x060024BF RID: 9407 RVA: 0x00103814 File Offset: 0x00101A14
	private void ApplyFixed(JointState jointState)
	{
		Vector3 vector = default(Vector3);
		Vector3 vector2 = default(Vector3);
		Vector3 vector3 = default(Vector3);
		this.SetSelectedTab();
		bool enableCollision = base.NameToBool("01 Collision");
		float num = base.NameToFloat("05 Break Force");
		if (num == 0f)
		{
			num = float.PositiveInfinity;
		}
		float num2 = base.NameToFloat("06 Break Torque");
		if (num2 == 0f)
		{
			num2 = float.PositiveInfinity;
		}
		jointState.EnableCollision = enableCollision;
		jointState.Axis = new VectorState(vector);
		jointState.Anchor = new VectorState(vector2);
		jointState.ConnectedAnchor = new VectorState(vector3);
		jointState.BreakForce = num;
		jointState.BreakTorgue = num2;
	}

	// Token: 0x040017C2 RID: 6082
	public UIToggle tabFixed;

	// Token: 0x040017C3 RID: 6083
	public UIToggle tabHinge;

	// Token: 0x040017C4 RID: 6084
	public UIToggle tabSpring;

	// Token: 0x040017C5 RID: 6085
	public GameObject Fixed;

	// Token: 0x040017C6 RID: 6086
	public GameObject Hinge;

	// Token: 0x040017C7 RID: 6087
	public GameObject Spring;

	// Token: 0x040017C8 RID: 6088
	public UIHighlightTargets HighlightJoint;

	// Token: 0x040017C9 RID: 6089
	public UIHighlightTargets HighlightAttached;

	// Token: 0x040017CA RID: 6090
	public int SourceID;

	// Token: 0x040017CB RID: 6091
	public int TargetID;
}
