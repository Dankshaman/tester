using System;

// Token: 0x0200035A RID: 858
public class UIVRSettings : UISettingsTemplate
{
	// Token: 0x060028B3 RID: 10419 RVA: 0x0011F3D0 File Offset: 0x0011D5D0
	public new void Start()
	{
		UIVRSettings.Instance = this;
		base.Start();
	}

	// Token: 0x060028B4 RID: 10420 RVA: 0x0011F3E0 File Offset: 0x0011D5E0
	public override void AddSettings()
	{
		base.AddCheckbox("vr_joypad_emulation", "Joypad Emulation");
		base.NextGroup();
		base.AddCheckbox("vr_ui_floating", "Physical UI Screen");
		base.AddButtons("vr_mode_ui_attachment", new string[]
		{
			"Off",
			"Left",
			"Right"
		}, "Attached UI Screen", null);
		base.AddSlider("vr_ui_scale", "UI Scale", 20f, 80f, 0, false);
		base.AddCheckbox("vr_show_missing_binding_warning", "Missing Binding Warning");
		base.NextGroup();
		base.AddButtons("vr_mode_hand_view", new string[]
		{
			"Off",
			"Left",
			"Right"
		}, "Virtual Hand View", null);
		base.AddSlider("vr_hand_view_angle", "Hand View Angle", -45f, 0f, 1, false);
		base.AddSlider("vr_hand_view_scale", "Hand View Scale", 0.5f, 6f, 2, false);
		base.NextGroup();
		base.AddCheckbox("vr_sticky_grab", "Sticky Grab");
		base.AddCheckbox("vr_grabbing_hides_gem", "Grabbing Hides Gem");
		base.AddSlider("vr_orient_object_delay", "Orient Object Delay", 0f, 3f, 2, false);
		base.NextGroup();
		base.AddCheckbox("vr_move_with_inertia", "Move With Inertia");
		base.AddSlider("vr_move_friction", "Move Friction", 0f, 0.1f, 2, false);
		base.NextGroup();
		base.AddCheckbox("vr_teleport_with_pad", "Pad Up = Teleport");
		base.AddButtons("vr_left_controller_mode_pad_down", new string[]
		{
			"Bind",
			"Tool",
			"Zoom"
		}, "Left Pad Down", new bool[]
		{
			true,
			default(bool),
			true
		});
		base.AddButtons("vr_right_controller_mode_pad_down", new string[]
		{
			"Bind",
			"Tool",
			"Zoom"
		}, "Right Pad Down", new bool[]
		{
			true,
			default(bool),
			true
		});
		base.AddCheckbox("vr_zoom_object_aligned", "Align Zoomed Object");
		base.NextGroup();
		base.AddButtons("vr_mode_selection_style", new string[]
		{
			"FIXED",
			"Exact",
			"Anchored"
		}, "Selection Style", null);
		base.AddSlider("vr_selection_height", "Selection Height", 1f, 20f, 0, false);
		base.NextGroup();
		base.AddCheckbox("vr_laser_constant", "Laser Always On");
		base.AddCheckbox("vr_laser_angled", "Laser Angled");
		base.AddCheckbox("vr_laser_beam_visible", "Laser Beam Visible");
		base.AddSlider("vr_laser_beam_opacity", "Laser Beam Opacity", 0.1f, 1f, 2, false);
		base.AddSlider("vr_laser_beam_thickness", "Laser Beam Thickness", 1f, 20f, 0, false);
		base.AddSlider("vr_laser_dot_size", "Laser Dot Size", 1f, 100f, 0, false);
		base.NextGroup();
		base.AddSlider("vr_laser_activation_threshold", "Laser Activation Threshold", 0f, 1f, 2, false);
		base.AddSlider("vr_interface_click_threshold", "Interface Click Threshold", 0f, 1f, 2, false);
		base.AddCheckbox("vr_hover_tooltips", "Hover Tooltips");
		base.NextGroup();
		base.AddCheckbox("vr_left_controller_bind_tool_hotkeys", "Left Tool Hotkeys");
		base.AddCheckbox("vr_right_controller_bind_tool_hotkeys", "Right Tool Hotkeys");
		base.AddSlider("vr_interface_repeat_duration", "Interface Repeat Duration", 0.2f, 1f, 2, false);
		base.AddCheckbox("vr_mode_icon_colored", "Tool Icon Colored");
		base.AddCheckbox("vr_thumbstick_icons_constant", "Pad Icons Always On");
		base.NextGroup();
		base.AddSlider("vr_tooltips_initial_duration", "Tooltips Initial Duration", 0f, 10f, 0, true);
		base.AddCheckbox("vr_tooltips_action_enabled", "Tooltips Action Enabled");
		base.AddCheckbox("vr_tooltips_constant", "Tooltips Always On");
		base.AddCheckbox("vr_tooltips_for_click_when_on_menu", "Click Tooltip on Menu");
		base.NextGroup();
		base.AddCheckbox("vr_tilt_mode", "Tilt Mode");
		base.AddSlider("vr_tilt_angle", "Tilt Angle", 10f, 90f, 0, false);
		base.IndividualEntries();
		base.AddCheckbox("search_close_after_take", "Taking Item Closes Search");
		base.AddCheckbox("drawing_render_fully_visible", "Render Drawing Over UI");
		base.AddCheckbox("fog", "Floor Of Mist");
		base.AddButtons("vr_mode_display_network_players", new string[]
		{
			"Off",
			"Hands",
			"All"
		}, "Display VR Players", null);
		base.AddCheckbox("vr_controls_original", "Original Controls");
	}

	// Token: 0x04001ACA RID: 6858
	public static UIVRSettings Instance;
}
