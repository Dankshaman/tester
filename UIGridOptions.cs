using System;

// Token: 0x020002E1 RID: 737
public class UIGridOptions : UIReactiveMenu
{
	// Token: 0x06002428 RID: 9256 RVA: 0x000FFD74 File Offset: 0x000FDF74
	protected override void Awake()
	{
		base.Awake();
		this.ReactiveElements.Add(this.TypeBoxToggle.onChange);
		this.ReactiveElements.Add(this.TypeHexToggle.onChange);
		this.ReactiveElements.Add(this.TypeHexRotatedToggle.onChange);
		this.ReactiveElements.Add(this.ShowLinesToggle.onChange);
		this.ReactiveElements.Add(this.ColorPicker.onChange);
		this.ReactiveElements.Add(this.OpacitySlider.onChange);
		this.ReactiveElements.Add(this.LineThickToggle.onChange);
		this.ReactiveElements.Add(this.LineThinToggle.onChange);
		this.ReactiveElements.Add(this.SnapOffToggle.onChange);
		this.ReactiveElements.Add(this.SnapLineToggle.onChange);
		this.ReactiveElements.Add(this.SnapCenterToggle.onChange);
		this.ReactiveElements.Add(this.SnapBothToggle.onChange);
		this.ReactiveElements.Add(this.OffsetXInput.onChange);
		this.ReactiveElements.Add(this.OffsetYInput.onChange);
		this.ReactiveElements.Add(this.OffsetZInput.onChange);
		this.ReactiveElements.Add(this.SizeXInput.onChange);
		this.ReactiveElements.Add(this.SizeYInput.onChange);
		EventDelegate.Add(this.ResetButton.onClick, new EventDelegate.Callback(this.ResetOnClick));
		EventManager.OnLoadingComplete += this.EventManager_OnLoadingComplete;
	}

	// Token: 0x06002429 RID: 9257 RVA: 0x000FFF2B File Offset: 0x000FE12B
	protected override void OnDestroy()
	{
		base.OnDestroy();
		EventDelegate.Remove(this.ResetButton.onClick, new EventDelegate.Callback(this.ResetOnClick));
		EventManager.OnLoadingComplete -= this.EventManager_OnLoadingComplete;
	}

	// Token: 0x0600242A RID: 9258 RVA: 0x000FFF61 File Offset: 0x000FE161
	private void EventManager_OnLoadingComplete()
	{
		if (base.gameObject.activeSelf)
		{
			base.TriggerReloadUI();
		}
	}

	// Token: 0x0600242B RID: 9259 RVA: 0x000FFF76 File Offset: 0x000FE176
	private void Update()
	{
		this.SyncBinding.enabled = this.SyncSizeToggle.value;
	}

	// Token: 0x0600242C RID: 9260 RVA: 0x000FFF8E File Offset: 0x000FE18E
	private void ResetOnClick()
	{
		NetworkSingleton<GridOptions>.Instance.Reset();
		base.TriggerReloadUI();
	}

	// Token: 0x0600242D RID: 9261 RVA: 0x000FFFA0 File Offset: 0x000FE1A0
	protected override void ReloadUI()
	{
		GridState gridState = NetworkSingleton<GridOptions>.Instance.gridState;
		this.TypeBoxToggle.value = (gridState.Type == GridType.Box);
		this.TypeHexToggle.value = (gridState.Type == GridType.HexHorizontal);
		this.TypeHexRotatedToggle.value = (gridState.Type == GridType.HexVertical);
		this.ShowLinesToggle.value = gridState.Lines;
		this.ColorPicker.value = gridState.Color.ToColour();
		this.OpacitySlider.value = gridState.Opacity;
		this.LineThickToggle.value = gridState.ThickLines;
		this.LineThinToggle.value = !gridState.ThickLines;
		this.SnapLineToggle.value = gridState.Snapping;
		this.SnapCenterToggle.value = gridState.Offset;
		this.SnapBothToggle.value = gridState.BothSnapping;
		this.SnapOffToggle.value = (!gridState.Snapping && !gridState.Offset && !gridState.BothSnapping);
		this.SizeXInput.value = gridState.xSize.ToString();
		this.SizeYInput.value = gridState.ySize.ToString();
		this.SyncSizeToggle.value = (this.SizeXInput.value == this.SizeYInput.value);
		this.OffsetXInput.value = gridState.PosOffset.x.ToString();
		this.OffsetYInput.value = gridState.PosOffset.y.ToString();
		this.OffsetZInput.value = gridState.PosOffset.z.ToString();
	}

	// Token: 0x0600242E RID: 9262 RVA: 0x00100153 File Offset: 0x000FE353
	protected override void UpdateSource()
	{
		NetworkSingleton<GridOptions>.Instance.gridState = this.GetGridState();
	}

	// Token: 0x0600242F RID: 9263 RVA: 0x000025B8 File Offset: 0x000007B8
	protected override void NetworkSync()
	{
	}

	// Token: 0x06002430 RID: 9264 RVA: 0x00100168 File Offset: 0x000FE368
	public GridState GetGridState()
	{
		GridState gridState = new GridState();
		if (this.TypeBoxToggle.value)
		{
			gridState.Type = GridType.Box;
		}
		else if (this.TypeHexToggle.value)
		{
			gridState.Type = GridType.HexHorizontal;
		}
		else if (this.TypeHexRotatedToggle.value)
		{
			gridState.Type = GridType.HexVertical;
		}
		gridState.Lines = this.ShowLinesToggle.value;
		gridState.Color = new ColourState(this.ColorPicker.value);
		gridState.Opacity = this.OpacitySlider.value;
		gridState.ThickLines = this.LineThickToggle.value;
		gridState.Snapping = this.SnapLineToggle.value;
		gridState.Offset = this.SnapCenterToggle.value;
		gridState.BothSnapping = this.SnapBothToggle.value;
		float.TryParse(this.SizeXInput.value, out gridState.xSize);
		float.TryParse(this.SizeYInput.value, out gridState.ySize);
		float.TryParse(this.OffsetXInput.value, out gridState.PosOffset.x);
		float.TryParse(this.OffsetYInput.value, out gridState.PosOffset.y);
		float.TryParse(this.OffsetZInput.value, out gridState.PosOffset.z);
		return gridState;
	}

	// Token: 0x0400172A RID: 5930
	public UIToggle TypeBoxToggle;

	// Token: 0x0400172B RID: 5931
	public UIToggle TypeHexToggle;

	// Token: 0x0400172C RID: 5932
	public UIToggle TypeHexRotatedToggle;

	// Token: 0x0400172D RID: 5933
	public UIToggle ShowLinesToggle;

	// Token: 0x0400172E RID: 5934
	public UIColorPickerInput ColorPicker;

	// Token: 0x0400172F RID: 5935
	public UISlider OpacitySlider;

	// Token: 0x04001730 RID: 5936
	public UIToggle LineThickToggle;

	// Token: 0x04001731 RID: 5937
	public UIToggle LineThinToggle;

	// Token: 0x04001732 RID: 5938
	public UIToggle SnapOffToggle;

	// Token: 0x04001733 RID: 5939
	public UIToggle SnapLineToggle;

	// Token: 0x04001734 RID: 5940
	public UIToggle SnapCenterToggle;

	// Token: 0x04001735 RID: 5941
	public UIToggle SnapBothToggle;

	// Token: 0x04001736 RID: 5942
	public UIInput OffsetXInput;

	// Token: 0x04001737 RID: 5943
	public UIInput OffsetYInput;

	// Token: 0x04001738 RID: 5944
	public UIInput OffsetZInput;

	// Token: 0x04001739 RID: 5945
	public UIInput SizeXInput;

	// Token: 0x0400173A RID: 5946
	public UIInput SizeYInput;

	// Token: 0x0400173B RID: 5947
	public UIToggle SyncSizeToggle;

	// Token: 0x0400173C RID: 5948
	public PropertyBinding SyncBinding;

	// Token: 0x0400173D RID: 5949
	public UIButton ResetButton;

	// Token: 0x0400173E RID: 5950
	public UIButton GizmoButton;
}
