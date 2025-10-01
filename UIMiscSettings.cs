using System;

// Token: 0x02000303 RID: 771
public class UIMiscSettings : UISettingsTemplate
{
	// Token: 0x0600253D RID: 9533 RVA: 0x001069DF File Offset: 0x00104BDF
	public new void Start()
	{
		UIMiscSettings.Instance = this;
		base.Start();
	}

	// Token: 0x0600253E RID: 9534 RVA: 0x001069F0 File Offset: 0x00104BF0
	public override void AddSettings()
	{
		base.AddCheckbox("ui_hand_view", "Hand View");
		base.AddSlider("ui_hand_minimized_size", "Hand Minimized Size", 0f, 1f, 2, false);
		base.NextGroup();
		base.AddCheckbox("hotseat_ask_for_names", "Hotseat: Ask For Names");
		base.AddCheckbox("hotseat_turn_button", "Hotseat: Show Turn Button");
		base.AddCheckbox("hotseat_camera_reset", "Hotseat: Reset Camera On Turn");
		base.AddCheckbox("hotseat_turn_confirmation", "Hotseat: Confirm Turn Start");
		base.AddSlider("hotseat_turn_delay", "Hotseat: Delay Between Turns", 0f, 10f, 1, false);
		base.NextGroup();
		base.AddCheckbox("pure_fog", "Pure Mode: Floor Mist");
		base.AddCheckbox("pure_override_custom_table", "Pure Mode: Hide Custom Image");
		base.NextGroup();
		base.AddSlider("measure_font_size", "Measure Font Size", 14f, 96f, 0, true);
		base.AddSlider("measure_arrows_angle", "Measure Arrows Angle", 0f, 90f, 0, false);
		base.NextGroup();
		base.AddCheckbox("ui_visualize_spawn_hide_window", "Spawning Hides Components");
		base.AddCheckbox("ui_visualize_spawn_object_snap", "Spawn Snap-To-Object");
		base.AddCheckbox("ui_visualize_spawn_in_air", "Spawn In Air");
		base.AddSlider("ui_visualize_spawn_opacity", "Opacity Of Spawn Indicator", 0.01f, 1f, 2, false);
		base.NextGroup();
		base.AddCheckbox("ui_visualize_drops", "Display Object Drop Position");
		base.AddSlider("ui_visualize_drop_opacity", "Opacity Of Drop Indicator", 0.01f, 1f, 2, false);
		base.AddCheckbox("ui_visualize_drop_snap", "Show For Snap Points");
		base.AddCheckbox("ui_visualize_drop_grid", "Show For Grid");
		base.AddCheckbox("ui_visualize_drop_free", "Show When Not Snapping");
		base.NextGroup();
		base.AddCheckbox("mouse_wheel_zoom_and_center", "Centre On Mouse Wheel");
		base.AddSlider("camera_rotation_rate", "Camera Load Spin Rate", 100f, 1000f, 0, false);
		base.AddButtons("vr_mode_display_network_players", new string[]
		{
			"Off",
			"Hands",
			"All"
		}, "Display VR Players", null);
		base.AddCheckbox("drawing_render_fully_visible", "Render Drawing over UI");
		base.NextGroup();
		base.AddSlider("hidden_zone_showing_opacity", "Hidden Zone Show Opacity", 0f, 0.5f, 2, false);
		base.AddSlider("hidden_zone_hiding_opacity", "Hidden Zone Hide Opacity", 0f, 1f, 2, false);
		base.AddCheckbox("randomize_zone_prompt_on_load", "Randomize Zone Load Prompt");
		base.NextGroup();
		base.AddCheckbox("ui_book_buttons_on_hover", "Book Buttons On Hover");
		base.AddSlider("ui_book_panel_opacity", "Book UI Opacity", 0f, 1f, 2, false);
		base.AddSlider("ui_book_page_opacity", "Book Page Opacity", 0.1f, 1f, 2, false);
		base.NextGroup();
		base.AddSlider("autosave_interval", 0f, 900f, 1, false);
		base.AddSlider("autosave_slots", 0f, 10f, 0, true);
		base.AddSlider("autosave_games_window_count", 0f, 3f, 0, true);
		base.AddCheckbox("autosave_log", "");
		base.NextGroup();
		base.AddCheckbox("console_hotkey_lock", "");
		base.AddCheckbox("timestamp_system", "");
		base.AddCheckbox("timestamp_game", "");
		base.AddCheckbox("timestamp_global", "");
		base.AddCheckbox("timestamp_team", "");
		base.NextGroup();
		base.AddSlider("ui_keyboard_scale", "On-screen Keyboard Scale", 0.5f, 3f, 1, false);
		base.AddSlider("ui_keyboard_echo_duration", "On-screen Keyboard Echo", 0f, 30f, 1, false);
		base.NextGroup();
		base.AddCheckbox("ui_collapsing_context_menus", "Context Menus Can Collapse");
		base.AddSlider("ui_context_menus_collapsed_height", "Size Of Collapsed Section", 8f, 32f, 0, false);
		base.AddCheckbox("ui_context_menus_show_gm_notes", "Context Menus Show GM Notes");
		base.AddCheckbox("ui_context_menus_from_games", "Games Add To Context Menus");
		base.AddCheckbox("game_hotkey_config_can_open", "Games Can Show Controls UI");
		base.NextGroup();
		base.AddSlider("component_spread_distance", "Card Spread Distance", 0.6f, 5f, 1, false);
		base.AddSlider("component_spread_row_distance", "Card Spread Next Row Distance", 3.2f, 8f, 1, false);
		base.AddSlider("component_spread_cards_per_row", "Card Spread Cards-Per-Row", 1f, 104f, 0, false);
		base.AddCheckbox("deck_can_spread_facedown", "");
		base.AddCheckbox("container_logging", "");
		base.AddCheckbox("group_into_bag_first", "");
		base.NextGroup();
		base.AddSlider("mouse_shake_threshold", 0f, 20f, 0, false);
		base.AddCheckbox("component_wrap_states", "Component States Wrap");
		base.AddCheckbox("double_click_container_grab", "Double-Click Container Pickup");
		base.AddCheckbox("card_is_a_deck_for_hotkeys", "");
		base.AddCheckbox("hand_component_hotkey_draw", "");
		base.AddCheckbox("keyboard_single_digit_by_default", "");
		base.AddCheckbox("negative_typed_numbers", "");
		base.AddCheckbox("component_hotkey_state_change", "");
		base.NextGroup();
		base.AddCheckbox("zoom_follow_pointer", "Alt-Zoom Follows Pointer");
		base.AddCheckbox("zoom_always", "Alt-Zoom Always On");
		base.AddSlider("magnify_zoom_size", "Magnify Zoom Size", 0.5f, 1f, 1, false);
		base.NextGroup();
		base.AddSlider("component_tooltip_delay", 0f, 10f, 1, false);
		base.AddSlider("ui_tooltip_delay", 0f, 10f, 1, false);
		base.AddSlider("ui_toast_duration", 0f, 30f, 0, false);
		base.AddCheckbox("ui_container_enter_indicator", "Show Container-Enter Indicator");
		base.IndividualEntries();
		base.AddCheckbox("file_browser_native", "");
		base.AddCheckbox("search_close_after_take", "Taking Item Closes Search");
		base.AddCheckbox("ui_autofocus_search", "Autofocus Search Input");
		base.AddCheckbox("ui_custom_object_check", "Matching Custom Object Query");
		base.AddCheckbox("enhanced_base_precision", "");
		base.AddSlider("ui_gizmo_scale", "Gizmo Tool Size", 0.5f, 2f, 2, false);
		base.AddCheckbox("stats_monitor", "");
	}

	// Token: 0x04001829 RID: 6185
	public static UIMiscSettings Instance;
}
