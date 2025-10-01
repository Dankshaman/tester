using System;

// Token: 0x02000151 RID: 337
public class LineScript : Singleton<LineScript>
{
	// Token: 0x0600110F RID: 4367 RVA: 0x00075B1C File Offset: 0x00073D1C
	private void Start()
	{
		EventDelegate.Add(this.LineModeButton.onClick, new EventDelegate.Callback(this.OnClickLineMode));
		EventDelegate.Add(this.LineUnitButton.onClick, new EventDelegate.Callback(this.OnClickLineUnit));
		EventDelegate.Add(this.LineLogButton.onClick, new EventDelegate.Callback(this.OnClickLineLog));
		EventDelegate.Add(this.LineHoverButton.onClick, new EventDelegate.Callback(this.OnClickLineHover));
		EventDelegate.Add(this.LineFlatButton.onClick, new EventDelegate.Callback(this.OnClickLineFlat));
	}

	// Token: 0x06001110 RID: 4368 RVA: 0x00075BBC File Offset: 0x00073DBC
	private void OnDestroy()
	{
		EventDelegate.Remove(this.LineModeButton.onClick, new EventDelegate.Callback(this.OnClickLineMode));
		EventDelegate.Remove(this.LineUnitButton.onClick, new EventDelegate.Callback(this.OnClickLineUnit));
		EventDelegate.Remove(this.LineLogButton.onClick, new EventDelegate.Callback(this.OnClickLineLog));
		EventDelegate.Remove(this.LineHoverButton.onClick, new EventDelegate.Callback(this.OnClickLineHover));
		EventDelegate.Remove(this.LineFlatButton.onClick, new EventDelegate.Callback(this.OnClickLineFlat));
	}

	// Token: 0x06001111 RID: 4369 RVA: 0x00075C5A File Offset: 0x00073E5A
	public void UpdateLogButton()
	{
		if (LineScript.LOG_MEASUREMENTS)
		{
			Language.UpdateUILabel(this.LineLogButton.GetComponentInChildren<UILabel>(), "Log");
			return;
		}
		Language.UpdateUILabel(this.LineLogButton.GetComponentInChildren<UILabel>(), "No Log");
	}

	// Token: 0x06001112 RID: 4370 RVA: 0x00075C8E File Offset: 0x00073E8E
	public void UpdateFlatButton()
	{
		if (LineScript.MEASURE_FLAT)
		{
			Language.UpdateUILabel(this.LineFlatButton.GetComponentInChildren<UILabel>(), "2D");
			return;
		}
		Language.UpdateUILabel(this.LineFlatButton.GetComponentInChildren<UILabel>(), "3D");
	}

	// Token: 0x06001113 RID: 4371 RVA: 0x00075CC2 File Offset: 0x00073EC2
	public void UpdateModeButton()
	{
		if (LineScript.MEASURE_OBJECTS)
		{
			Language.UpdateUILabel(this.LineModeButton.GetComponentInChildren<UILabel>(), "Auto");
			return;
		}
		Language.UpdateUILabel(this.LineModeButton.GetComponentInChildren<UILabel>(), "Free");
	}

	// Token: 0x06001114 RID: 4372 RVA: 0x00075CF8 File Offset: 0x00073EF8
	public void UpdateUnitButton()
	{
		if (LineScript.GRID_MEASUREMENTS)
		{
			Language.UpdateUILabel(this.LineUnitButton.GetComponentInChildren<UILabel>(), "Grid");
			return;
		}
		if (LineScript.METRIC_MEASUREMENTS)
		{
			Language.UpdateUILabel(this.LineUnitButton.GetComponentInChildren<UILabel>(), "CM");
			return;
		}
		Language.UpdateUILabel(this.LineUnitButton.GetComponentInChildren<UILabel>(), "Inch");
	}

	// Token: 0x06001115 RID: 4373 RVA: 0x00075D54 File Offset: 0x00073F54
	public void UpdateHoverButton()
	{
		if (LineScript.MEASURE_HOVERED_FROM_EDGE)
		{
			Language.UpdateUILabel(this.LineHoverButton.GetComponentInChildren<UILabel>(), "Edge");
			return;
		}
		Language.UpdateUILabel(this.LineHoverButton.GetComponentInChildren<UILabel>(), "Center");
	}

	// Token: 0x06001116 RID: 4374 RVA: 0x00075D88 File Offset: 0x00073F88
	private void OnClickLineMode()
	{
		Singleton<SystemConsole>.Instance.ProcessCommand("!measure_components", false, SystemConsole.CommandEcho.Silent);
	}

	// Token: 0x06001117 RID: 4375 RVA: 0x00075D9C File Offset: 0x00073F9C
	private void OnClickLineUnit()
	{
		if (LineScript.GRID_MEASUREMENTS)
		{
			Singleton<SystemConsole>.Instance.ProcessCommand("!measure_using_grid", false, SystemConsole.CommandEcho.Silent);
			Singleton<SystemConsole>.Instance.ProcessCommand("!measure_in_metric", false, SystemConsole.CommandEcho.Silent);
			return;
		}
		if (LineScript.METRIC_MEASUREMENTS)
		{
			Singleton<SystemConsole>.Instance.ProcessCommand("!measure_using_grid", false, SystemConsole.CommandEcho.Silent);
			return;
		}
		Singleton<SystemConsole>.Instance.ProcessCommand("!measure_in_metric", false, SystemConsole.CommandEcho.Silent);
	}

	// Token: 0x06001118 RID: 4376 RVA: 0x00075DFD File Offset: 0x00073FFD
	private void OnClickLineLog()
	{
		Singleton<SystemConsole>.Instance.ProcessCommand("!measure_logging", false, SystemConsole.CommandEcho.Silent);
	}

	// Token: 0x06001119 RID: 4377 RVA: 0x00075E10 File Offset: 0x00074010
	private void OnClickLineHover()
	{
		Singleton<SystemConsole>.Instance.ProcessCommand("!measure_hovered_from_edge", false, SystemConsole.CommandEcho.Silent);
	}

	// Token: 0x0600111A RID: 4378 RVA: 0x00075E23 File Offset: 0x00074023
	private void OnClickLineFlat()
	{
		Singleton<SystemConsole>.Instance.ProcessCommand("!measure_flat", false, SystemConsole.CommandEcho.Silent);
	}

	// Token: 0x04000ACF RID: 2767
	public static bool METRIC_MEASUREMENTS = false;

	// Token: 0x04000AD0 RID: 2768
	public static bool GRID_MEASUREMENTS = false;

	// Token: 0x04000AD1 RID: 2769
	public static bool MEASURE_OBJECTS = true;

	// Token: 0x04000AD2 RID: 2770
	public static bool MEASURE_FLAT = true;

	// Token: 0x04000AD3 RID: 2771
	public static bool LOG_MEASUREMENTS = false;

	// Token: 0x04000AD4 RID: 2772
	public static bool MEASURE_HOVERED_FROM_EDGE = true;

	// Token: 0x04000AD5 RID: 2773
	public static float MEASURE_MULTIPLIER = 1f;

	// Token: 0x04000AD6 RID: 2774
	private const string modeFree = "Free";

	// Token: 0x04000AD7 RID: 2775
	private const string modeAuto = "Auto";

	// Token: 0x04000AD8 RID: 2776
	private const string unitImperial = "Inch";

	// Token: 0x04000AD9 RID: 2777
	private const string unitMetric = "CM";

	// Token: 0x04000ADA RID: 2778
	private const string unitGrid = "Grid";

	// Token: 0x04000ADB RID: 2779
	private const string flatEnabled = "2D";

	// Token: 0x04000ADC RID: 2780
	private const string flatDisabled = "3D";

	// Token: 0x04000ADD RID: 2781
	private const string logEnabled = "Log";

	// Token: 0x04000ADE RID: 2782
	private const string logDisabled = "No Log";

	// Token: 0x04000ADF RID: 2783
	private const string hoverEdge = "Edge";

	// Token: 0x04000AE0 RID: 2784
	private const string hoverCenter = "Center";

	// Token: 0x04000AE1 RID: 2785
	public UIButton LineModeButton;

	// Token: 0x04000AE2 RID: 2786
	public UIButton LineUnitButton;

	// Token: 0x04000AE3 RID: 2787
	public UIButton LineHoverButton;

	// Token: 0x04000AE4 RID: 2788
	public UIButton LineLogButton;

	// Token: 0x04000AE5 RID: 2789
	public UIButton LineFlatButton;
}
