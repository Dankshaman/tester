using System;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using Valve.VR.InteractionSystem;

// Token: 0x02000370 RID: 880
public class VRSteamControllerDevice : MonoBehaviour
{
	// Token: 0x170004CE RID: 1230
	// (get) Token: 0x0600295C RID: 10588 RVA: 0x00122AF1 File Offset: 0x00120CF1
	// (set) Token: 0x0600295D RID: 10589 RVA: 0x00122B16 File Offset: 0x00120D16
	public static float ACTIVATE_LASER_THRESHOLD
	{
		get
		{
			if (VRSteamControllerDevice.activateLaserThreshold >= VRSteamControllerDevice.INTERFACE_CLICK_THRESHOLD - 0.1f)
			{
				return VRSteamControllerDevice.INTERFACE_CLICK_THRESHOLD - 0.1f;
			}
			return VRSteamControllerDevice.activateLaserThreshold;
		}
		set
		{
			VRSteamControllerDevice.activateLaserThreshold = value;
		}
	}

	// Token: 0x0600295E RID: 10590 RVA: 0x00122B20 File Offset: 0x00120D20
	private void Start()
	{
		this.hand = base.GetComponent<Hand>();
		this.behaviourPose = base.GetComponent<SteamVR_Behaviour_Pose>();
		this.actions[VRAction.INTERFACE_CLICK] = new VRSteamControllerDevice.ActionStates(SteamVR_Actions.default_interface_click);
		this.actions[VRAction.PAD_CLICK] = new VRSteamControllerDevice.ActionStates(SteamVR_Actions.default_pad_click);
		this.actions[VRAction.NORTH_TOUCH] = new VRSteamControllerDevice.ActionStates(SteamVR_Actions.default_up_touch);
		this.actions[VRAction.SOUTH_TOUCH] = new VRSteamControllerDevice.ActionStates(SteamVR_Actions.default_down_touch);
		this.actions[VRAction.EAST_TOUCH] = new VRSteamControllerDevice.ActionStates(SteamVR_Actions.default_right_touch);
		this.actions[VRAction.WEST_TOUCH] = new VRSteamControllerDevice.ActionStates(SteamVR_Actions.default_left_touch);
		this.actions[VRAction.CENTER_TOUCH] = new VRSteamControllerDevice.ActionStates(SteamVR_Actions.default_center_touch);
		this.actions[VRAction.NORTH_CLICK] = new VRSteamControllerDevice.ActionStates(SteamVR_Actions.default_up_click);
		this.actions[VRAction.SOUTH_CLICK] = new VRSteamControllerDevice.ActionStates(SteamVR_Actions.default_down_click);
		this.actions[VRAction.EAST_CLICK] = new VRSteamControllerDevice.ActionStates(SteamVR_Actions.default_right_click);
		this.actions[VRAction.WEST_CLICK] = new VRSteamControllerDevice.ActionStates(SteamVR_Actions.default_left_click);
		this.actions[VRAction.CENTER_CLICK] = new VRSteamControllerDevice.ActionStates(SteamVR_Actions.default_center_click);
		this.actions[VRAction.TELEPORT_START] = new VRSteamControllerDevice.ActionStates(SteamVR_Actions.default_teleport_start);
		this.actions[VRAction.TELEPORT_CONFIRM] = new VRSteamControllerDevice.ActionStates(SteamVR_Actions.default_teleport_confirm);
	}

	// Token: 0x0600295F RID: 10591 RVA: 0x00122C80 File Offset: 0x00120E80
	private void Update()
	{
		float axis = SteamVR_Actions.default___trigger_press.GetAxis(this.hand.handType);
		this.trigger_pressed_previous = this.trigger_pressed;
		this.trigger_pressed = (axis >= 0.1f);
		this.trigger_hair = (axis >= 0.001f);
		this.trigger_click = (axis >= 1f);
		this.trigger_pressed_down = false;
		this.trigger_pressed_up = false;
		if (this.trigger_pressed)
		{
			if (!this.trigger_pressed_previous)
			{
				this.trigger_pressed_down = true;
			}
		}
		else if (this.trigger_pressed_previous)
		{
			this.trigger_pressed_up = true;
		}
		axis = SteamVR_Actions.default_grab.GetAxis(this.hand.handType);
		this.grabbing.previous = this.grabbing.value;
		this.grabbing.value = (axis >= 0.1f);
		this.grabbing.press = false;
		this.grabbing.release = false;
		if (this.grabbing)
		{
			if (!this.grabbing.previous)
			{
				this.grabbing.press = true;
			}
		}
		else if (this.grabbing.previous)
		{
			this.grabbing.release = true;
		}
		this.laser_axis_activated = (SteamVR_Actions.default_activate_laser_axis.GetAxis(this.hand.handType) >= VRSteamControllerDevice.ACTIVATE_LASER_THRESHOLD);
		this.click_axis_activated = (SteamVR_Actions.default_interface_axis.GetAxis(this.hand.handType) >= VRSteamControllerDevice.INTERFACE_CLICK_THRESHOLD);
		this.orient_pulse = false;
		if (SteamVR_Actions.default_orient_object.GetState(this.hand.handType))
		{
			if (!this.orient_state)
			{
				this.orient_pulse = true;
				this.orient_state = true;
			}
		}
		else
		{
			this.orient_state = false;
		}
		this.peek = SteamVR_Actions.default_peek.GetState(this.hand.handType);
		this.actions[VRAction.INTERFACE_CLICK].Update(this.hand.handType, this.click_axis_activated);
		for (int i = 1; i < 14; i++)
		{
			this.actions[(VRAction)i].Update(this.hand.handType, false);
		}
		if (VRSteamControllerDevice.ENABLE_JOYPAD_EMULATION)
		{
			if (SteamVR_Actions.default_button_a.GetState(this.hand.handType))
			{
				TTSInput.Override(KeyCode.Joystick2Button0);
			}
			if (SteamVR_Actions.default_button_b.GetState(this.hand.handType))
			{
				TTSInput.Override(KeyCode.Joystick2Button1);
			}
			if (SteamVR_Actions.default_button_x.GetState(this.hand.handType))
			{
				TTSInput.Override(KeyCode.Joystick2Button2);
			}
			if (SteamVR_Actions.default_button_y.GetState(this.hand.handType))
			{
				TTSInput.Override(KeyCode.Joystick2Button3);
			}
			if (SteamVR_Actions.default_button_lb.GetState(this.hand.handType))
			{
				TTSInput.Override(KeyCode.Joystick2Button4);
			}
			if (SteamVR_Actions.default_button_rb.GetState(this.hand.handType))
			{
				TTSInput.Override(KeyCode.Joystick2Button5);
			}
			if (SteamVR_Actions.default_button_back.GetState(this.hand.handType))
			{
				TTSInput.Override(KeyCode.Joystick2Button6);
			}
			if (SteamVR_Actions.default_button_start.GetState(this.hand.handType))
			{
				TTSInput.Override(KeyCode.Joystick2Button7);
			}
			if (SteamVR_Actions.default_button_ls.GetState(this.hand.handType))
			{
				TTSInput.Override(KeyCode.Joystick2Button8);
			}
			if (SteamVR_Actions.default_button_rs.GetState(this.hand.handType))
			{
				TTSInput.Override(KeyCode.Joystick2Button9);
			}
			if (SteamVR_Actions.default_button_10.GetState(this.hand.handType))
			{
				TTSInput.Override(KeyCode.Joystick2Button10);
			}
			if (SteamVR_Actions.default_button_11.GetState(this.hand.handType))
			{
				TTSInput.Override(KeyCode.Joystick2Button11);
			}
			if (SteamVR_Actions.default_button_12.GetState(this.hand.handType))
			{
				TTSInput.Override(KeyCode.Joystick2Button12);
			}
			if (SteamVR_Actions.default_button_13.GetState(this.hand.handType))
			{
				TTSInput.Override(KeyCode.Joystick2Button13);
			}
			if (SteamVR_Actions.default_button_14.GetState(this.hand.handType))
			{
				TTSInput.Override(KeyCode.Joystick2Button14);
			}
			if (SteamVR_Actions.default_button_15.GetState(this.hand.handType))
			{
				TTSInput.Override(KeyCode.Joystick2Button15);
			}
			if (SteamVR_Actions.default_button_16.GetState(this.hand.handType))
			{
				TTSInput.Override(KeyCode.Joystick2Button16);
			}
			if (SteamVR_Actions.default_button_17.GetState(this.hand.handType))
			{
				TTSInput.Override(KeyCode.Joystick2Button17);
			}
			if (SteamVR_Actions.default_button_18.GetState(this.hand.handType))
			{
				TTSInput.Override(KeyCode.Joystick2Button18);
			}
			if (SteamVR_Actions.default_button_19.GetState(this.hand.handType))
			{
				TTSInput.Override(KeyCode.Joystick2Button19);
			}
		}
	}

	// Token: 0x06002960 RID: 10592 RVA: 0x00123140 File Offset: 0x00121340
	private void ResetActions(VRAction first, VRAction last)
	{
		for (int i = (int)first; i <= (int)last; i++)
		{
			this.actions[(VRAction)i].Reset();
		}
	}

	// Token: 0x06002961 RID: 10593 RVA: 0x0012316C File Offset: 0x0012136C
	public Vector3 GetTrackedObjectVelocity()
	{
		return this.behaviourPose.GetVelocity();
	}

	// Token: 0x170004CF RID: 1231
	// (get) Token: 0x06002962 RID: 10594 RVA: 0x00123179 File Offset: 0x00121379
	internal Vector3 angularVelocity
	{
		get
		{
			return this.behaviourPose.GetAngularVelocity();
		}
	}

	// Token: 0x06002963 RID: 10595 RVA: 0x00123186 File Offset: 0x00121386
	internal bool GetPressGrip()
	{
		return SteamVR_Actions.default___grip_held.GetState(this.hand.handType);
	}

	// Token: 0x06002964 RID: 10596 RVA: 0x0012319D File Offset: 0x0012139D
	internal bool GetPressGripDown()
	{
		return SteamVR_Actions.default___grip_held.GetStateDown(this.hand.handType);
	}

	// Token: 0x06002965 RID: 10597 RVA: 0x001231B4 File Offset: 0x001213B4
	internal bool GetPressTrigger()
	{
		return this.trigger_pressed;
	}

	// Token: 0x06002966 RID: 10598 RVA: 0x001231BC File Offset: 0x001213BC
	internal bool GetPressTriggerDown()
	{
		return this.trigger_pressed_down;
	}

	// Token: 0x06002967 RID: 10599 RVA: 0x001231C4 File Offset: 0x001213C4
	internal bool GetPressTriggerUp()
	{
		return this.trigger_pressed_up;
	}

	// Token: 0x06002968 RID: 10600 RVA: 0x001231CC File Offset: 0x001213CC
	internal bool GetTriggerClick()
	{
		return this.trigger_click;
	}

	// Token: 0x06002969 RID: 10601 RVA: 0x001231D4 File Offset: 0x001213D4
	internal bool GetTriggerHair()
	{
		return this.trigger_hair;
	}

	// Token: 0x0600296A RID: 10602 RVA: 0x001231DC File Offset: 0x001213DC
	internal bool GetPressMenu()
	{
		return SteamVR_Actions.default___menu_held.GetState(this.hand.handType);
	}

	// Token: 0x0600296B RID: 10603 RVA: 0x001231F3 File Offset: 0x001213F3
	internal bool GetPressMenuDown()
	{
		return SteamVR_Actions.default___menu_held.GetStateDown(this.hand.handType);
	}

	// Token: 0x0600296C RID: 10604 RVA: 0x0012320A File Offset: 0x0012140A
	internal bool GetPressMenuUp()
	{
		return SteamVR_Actions.default___menu_held.GetStateUp(this.hand.handType);
	}

	// Token: 0x0600296D RID: 10605 RVA: 0x00123221 File Offset: 0x00121421
	internal bool GetTouchTouchpad()
	{
		return SteamVR_Actions.default___touchpad_touch.GetState(this.hand.handType);
	}

	// Token: 0x0600296E RID: 10606 RVA: 0x00123238 File Offset: 0x00121438
	internal bool GetPressTouchpad()
	{
		return SteamVR_Actions.default___touchpad_click.GetState(this.hand.handType);
	}

	// Token: 0x0600296F RID: 10607 RVA: 0x0012324F File Offset: 0x0012144F
	internal bool GetPressTouchpadDown()
	{
		return SteamVR_Actions.default___touchpad_click.GetStateDown(this.hand.handType);
	}

	// Token: 0x06002970 RID: 10608 RVA: 0x00123266 File Offset: 0x00121466
	internal bool GetPressTouchpadUp()
	{
		return SteamVR_Actions.default___touchpad_click.GetStateUp(this.hand.handType);
	}

	// Token: 0x06002971 RID: 10609 RVA: 0x0012327D File Offset: 0x0012147D
	internal Vector2 GetTouchpadAxis()
	{
		return SteamVR_Actions.default___touchpad_position.GetAxis(this.hand.handType);
	}

	// Token: 0x06002972 RID: 10610 RVA: 0x00123294 File Offset: 0x00121494
	internal void TriggerHapticPulse(float intensity = 0.4f)
	{
		if (VRTrackedController.ViveHaptics)
		{
			this.hand.TriggerHapticPulse((ushort)(intensity * 1000f));
			return;
		}
		this.hand.TriggerHapticPulse(0.01f, 100f, intensity * 0.125f);
	}

	// Token: 0x06002973 RID: 10611 RVA: 0x001232CD File Offset: 0x001214CD
	internal bool IsMoveEnabled()
	{
		return SteamVR_Actions.default_enable_move.GetState(this.hand.handType);
	}

	// Token: 0x06002974 RID: 10612 RVA: 0x001232E4 File Offset: 0x001214E4
	internal bool DisplayTooltips()
	{
		return SteamVR_Actions.default_display_tooltips.GetState(this.hand.handType);
	}

	// Token: 0x06002975 RID: 10613 RVA: 0x001232FB File Offset: 0x001214FB
	internal bool IsGrabbing()
	{
		return this.grabbing;
	}

	// Token: 0x06002976 RID: 10614 RVA: 0x00123308 File Offset: 0x00121508
	internal bool StartedGrab()
	{
		return this.grabbing.press;
	}

	// Token: 0x06002977 RID: 10615 RVA: 0x00123315 File Offset: 0x00121515
	internal bool ReleasedGrab()
	{
		return this.grabbing.release;
	}

	// Token: 0x06002978 RID: 10616 RVA: 0x00123322 File Offset: 0x00121522
	internal bool ActivateLaser()
	{
		return this.laser_axis_activated || SteamVR_Actions.default_activate_laser.GetState(this.hand.handType);
	}

	// Token: 0x06002979 RID: 10617 RVA: 0x00123343 File Offset: 0x00121543
	internal bool OrientHeldObject()
	{
		return this.orient_pulse;
	}

	// Token: 0x0600297A RID: 10618 RVA: 0x0012334B File Offset: 0x0012154B
	internal bool Peek()
	{
		return this.peek;
	}

	// Token: 0x0600297B RID: 10619 RVA: 0x00123353 File Offset: 0x00121553
	internal bool ToggleMainMenu()
	{
		return SteamVR_Actions.default_main_menu.GetState(this.hand.handType);
	}

	// Token: 0x0600297C RID: 10620 RVA: 0x0012336A File Offset: 0x0012156A
	internal bool ResetPosition()
	{
		return SteamVR_Actions.default_reset_position.GetState(this.hand.handType);
	}

	// Token: 0x0600297D RID: 10621 RVA: 0x00123381 File Offset: 0x00121581
	internal bool Action(VRAction action)
	{
		return this.actions[action];
	}

	// Token: 0x0600297E RID: 10622 RVA: 0x00123394 File Offset: 0x00121594
	internal bool ActionPress(VRAction action)
	{
		return this.actions[action].press;
	}

	// Token: 0x0600297F RID: 10623 RVA: 0x001233A7 File Offset: 0x001215A7
	internal bool ActionRelease(VRAction action)
	{
		return this.actions[action].release;
	}

	// Token: 0x06002980 RID: 10624 RVA: 0x001233BA File Offset: 0x001215BA
	internal bool ActionRepeated(VRAction action)
	{
		return this.actions[action].RepeatPress(VRTrackedController.REPEAT_DURATION);
	}

	// Token: 0x06002981 RID: 10625 RVA: 0x001233D2 File Offset: 0x001215D2
	internal bool PadTouched()
	{
		return this.Action(VRAction.NORTH_TOUCH) || this.Action(VRAction.SOUTH_TOUCH) || this.Action(VRAction.EAST_TOUCH) || this.Action(VRAction.WEST_TOUCH) || this.Action(VRAction.CENTER_TOUCH);
	}

	// Token: 0x06002982 RID: 10626 RVA: 0x00123402 File Offset: 0x00121602
	internal bool NewPadTouched()
	{
		return this.ActionPress(VRAction.NORTH_TOUCH) || this.ActionPress(VRAction.SOUTH_TOUCH) || this.ActionPress(VRAction.EAST_TOUCH) || this.ActionPress(VRAction.WEST_TOUCH) || this.ActionPress(VRAction.CENTER_TOUCH);
	}

	// Token: 0x06002983 RID: 10627 RVA: 0x00123434 File Offset: 0x00121634
	internal void DebugScroll()
	{
		Vector3 axis = SteamVR_Actions.default_scroll.GetAxis(this.hand.handType);
		if (axis != Vector3.zero)
		{
			Chat.LogSystem(string.Concat(axis), false);
		}
	}

	// Token: 0x04001B7C RID: 7036
	private const float GRAB_THRESHOLD = 0.1f;

	// Token: 0x04001B7D RID: 7037
	private const float TRIGGER_HAIR_THRESHOLD = 0.001f;

	// Token: 0x04001B7E RID: 7038
	private const float TRIGGER_PRESS_THRESHOLD = 0.1f;

	// Token: 0x04001B7F RID: 7039
	private const float TRIGGER_CLICK_THRESHOLD = 1f;

	// Token: 0x04001B80 RID: 7040
	public static bool ENABLE_JOYPAD_EMULATION = false;

	// Token: 0x04001B81 RID: 7041
	public static float INTERFACE_CLICK_THRESHOLD = 0.9f;

	// Token: 0x04001B82 RID: 7042
	private static float activateLaserThreshold = 0.01f;

	// Token: 0x04001B83 RID: 7043
	private Hand hand;

	// Token: 0x04001B84 RID: 7044
	private SteamVR_Behaviour_Pose behaviourPose;

	// Token: 0x04001B85 RID: 7045
	private Dictionary<VRAction, VRSteamControllerDevice.ActionStates> actions = new Dictionary<VRAction, VRSteamControllerDevice.ActionStates>();

	// Token: 0x04001B86 RID: 7046
	private VRSteamControllerDevice.ActionStates grabbing = new VRSteamControllerDevice.ActionStates(null);

	// Token: 0x04001B87 RID: 7047
	private bool orient_state;

	// Token: 0x04001B88 RID: 7048
	private bool orient_pulse;

	// Token: 0x04001B89 RID: 7049
	private bool peek;

	// Token: 0x04001B8A RID: 7050
	private bool trigger_pressed;

	// Token: 0x04001B8B RID: 7051
	private bool trigger_pressed_previous;

	// Token: 0x04001B8C RID: 7052
	private bool trigger_pressed_up;

	// Token: 0x04001B8D RID: 7053
	private bool trigger_pressed_down;

	// Token: 0x04001B8E RID: 7054
	private bool trigger_hair;

	// Token: 0x04001B8F RID: 7055
	private bool trigger_click;

	// Token: 0x04001B90 RID: 7056
	private bool laser_axis_activated;

	// Token: 0x04001B91 RID: 7057
	private bool click_axis_activated;

	// Token: 0x020007AC RID: 1964
	public class ActionStates
	{
		// Token: 0x06003F8C RID: 16268 RVA: 0x00181BFF File Offset: 0x0017FDFF
		public ActionStates(SteamVR_Action_Boolean action = null)
		{
			this.value = false;
			this.previous = false;
			this.press = false;
			this.release = false;
			this.time = 0f;
			this.action = action;
		}

		// Token: 0x06003F8D RID: 16269 RVA: 0x00181C35 File Offset: 0x0017FE35
		public void Reset()
		{
			if (this.value && this.time != Time.time)
			{
				this.release = true;
			}
			else
			{
				this.release = false;
			}
			this.value = false;
			this.previous = false;
			this.press = false;
		}

		// Token: 0x06003F8E RID: 16270 RVA: 0x00181C74 File Offset: 0x0017FE74
		public void Update(SteamVR_Input_Sources hand, bool positive_override = false)
		{
			if (positive_override)
			{
				this.value = true;
			}
			else
			{
				this.value = this.action.GetState(hand);
			}
			this.press = false;
			this.release = false;
			if (this.value)
			{
				if (!this.previous)
				{
					this.press = true;
					this.time = Time.time;
					this.previous = this.value;
					return;
				}
			}
			else if (this.previous)
			{
				this.release = true;
				this.time = Time.time;
				this.previous = this.value;
			}
		}

		// Token: 0x06003F8F RID: 16271 RVA: 0x00181D04 File Offset: 0x0017FF04
		public bool RepeatPress(float duration)
		{
			float num = Time.time - Time.deltaTime;
			return this.value && Mathf.Floor((num - this.time) / duration) != Mathf.Floor((Time.time - this.time) / duration);
		}

		// Token: 0x06003F90 RID: 16272 RVA: 0x00181D4E File Offset: 0x0017FF4E
		public static implicit operator bool(VRSteamControllerDevice.ActionStates actionStates)
		{
			return actionStates.value;
		}

		// Token: 0x04002CFC RID: 11516
		public bool value;

		// Token: 0x04002CFD RID: 11517
		public bool previous;

		// Token: 0x04002CFE RID: 11518
		public bool press;

		// Token: 0x04002CFF RID: 11519
		public bool release;

		// Token: 0x04002D00 RID: 11520
		public float time;

		// Token: 0x04002D01 RID: 11521
		public SteamVR_Action_Boolean action;
	}
}
