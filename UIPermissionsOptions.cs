using System;

// Token: 0x02000314 RID: 788
public class UIPermissionsOptions : UIReactiveMenu
{
	// Token: 0x0600265C RID: 9820 RVA: 0x00111B18 File Offset: 0x0010FD18
	protected override void Awake()
	{
		base.Awake();
		this.ReactiveElements.Add(this.TableFlip.onChange);
		this.ReactiveElements.Add(this.Contextual.onChange);
		this.ReactiveElements.Add(this.Scaling.onChange);
		this.ReactiveElements.Add(this.ChangeColor.onChange);
		this.ReactiveElements.Add(this.Locking.onChange);
		this.ReactiveElements.Add(this.Notes.onChange);
		this.ReactiveElements.Add(this.Zones.onChange);
		this.ReactiveElements.Add(this.Drawing.onChange);
		this.ReactiveElements.Add(this.Flick.onChange);
		this.ReactiveElements.Add(this.Digital.onChange);
		this.ReactiveElements.Add(this.Combining.onChange);
		this.ReactiveElements.Add(this.ChangeTeam.onChange);
		this.ReactiveElements.Add(this.Tablets.onChange);
		this.ReactiveElements.Add(this.Music.onChange);
		this.ReactiveElements.Add(this.Saving.onChange);
		this.ReactiveElements.Add(this.Peeking.onChange);
		this.ReactiveElements.Add(this.Nudging.onChange);
		this.ReactiveElements.Add(this.Decals.onChange);
		this.ReactiveElements.Add(this.Line.onChange);
		EventDelegate.Add(this.ResetButton.onClick, new EventDelegate.Callback(this.OnClickReset));
	}

	// Token: 0x0600265D RID: 9821 RVA: 0x00111CEA File Offset: 0x0010FEEA
	protected override void OnDestroy()
	{
		base.OnDestroy();
		EventDelegate.Remove(this.ResetButton.onClick, new EventDelegate.Callback(this.OnClickReset));
	}

	// Token: 0x0600265E RID: 9822 RVA: 0x00111D0F File Offset: 0x0010FF0F
	private void OnClickReset()
	{
		NetworkSingleton<PermissionsOptions>.Instance.Reset();
		base.TriggerReloadUI();
	}

	// Token: 0x0600265F RID: 9823 RVA: 0x000025B8 File Offset: 0x000007B8
	protected override void NetworkSync()
	{
	}

	// Token: 0x06002660 RID: 9824 RVA: 0x00111D24 File Offset: 0x0010FF24
	protected override void ReloadUI()
	{
		PermissionsOptions.Options options = PermissionsOptions.options;
		this.TableFlip.value = options.TableFlip;
		this.Contextual.value = options.Contextual;
		this.Scaling.value = options.Scaling;
		this.ChangeColor.value = options.ChangeColor;
		this.Locking.value = options.Locking;
		this.Notes.value = options.Notes;
		this.Zones.value = options.Zones;
		this.Drawing.value = options.Drawing;
		this.Flick.value = options.Flick;
		this.Digital.value = options.Digital;
		this.Combining.value = options.Combining;
		this.ChangeTeam.value = options.ChangeTeam;
		this.Tablets.value = options.Tablets;
		this.Music.value = options.Music;
		this.Saving.value = options.Saving;
		this.Peeking.value = options.Peeking;
		this.Nudging.value = options.Nudging;
		this.Decals.value = options.Decals;
		this.Line.value = options.Line;
	}

	// Token: 0x06002661 RID: 9825 RVA: 0x00111E7C File Offset: 0x0011007C
	protected override void UpdateSource()
	{
		PermissionsOptions.options = new PermissionsOptions.Options
		{
			TableFlip = this.TableFlip.value,
			Contextual = this.Contextual.value,
			Scaling = this.Scaling.value,
			ChangeColor = this.ChangeColor.value,
			Locking = this.Locking.value,
			Notes = this.Notes.value,
			Zones = this.Zones.value,
			Drawing = this.Drawing.value,
			Flick = this.Flick.value,
			Digital = this.Digital.value,
			Combining = this.Combining.value,
			ChangeTeam = this.ChangeTeam.value,
			Tablets = this.Tablets.value,
			Music = this.Music.value,
			Saving = this.Saving.value,
			Peeking = this.Peeking.value,
			Nudging = this.Nudging.value,
			Decals = this.Decals.value,
			Line = this.Line.value
		};
	}

	// Token: 0x040018EE RID: 6382
	public UIToggle TableFlip;

	// Token: 0x040018EF RID: 6383
	public UIToggle Contextual;

	// Token: 0x040018F0 RID: 6384
	public UIToggle Scaling;

	// Token: 0x040018F1 RID: 6385
	public UIToggle Locking;

	// Token: 0x040018F2 RID: 6386
	public UIToggle Peeking;

	// Token: 0x040018F3 RID: 6387
	public UIToggle Nudging;

	// Token: 0x040018F4 RID: 6388
	public UIToggle Digital;

	// Token: 0x040018F5 RID: 6389
	public UIToggle Tablets;

	// Token: 0x040018F6 RID: 6390
	public UIToggle Music;

	// Token: 0x040018F7 RID: 6391
	public UIToggle Saving;

	// Token: 0x040018F8 RID: 6392
	public UIToggle ChangeColor;

	// Token: 0x040018F9 RID: 6393
	public UIToggle ChangeTeam;

	// Token: 0x040018FA RID: 6394
	public UIToggle Drawing;

	// Token: 0x040018FB RID: 6395
	public UIToggle Zones;

	// Token: 0x040018FC RID: 6396
	public UIToggle Line;

	// Token: 0x040018FD RID: 6397
	public UIToggle Flick;

	// Token: 0x040018FE RID: 6398
	public UIToggle Combining;

	// Token: 0x040018FF RID: 6399
	public UIToggle Decals;

	// Token: 0x04001900 RID: 6400
	public UIToggle Notes;

	// Token: 0x04001901 RID: 6401
	public UIButton ResetButton;
}
