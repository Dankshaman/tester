using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020002FE RID: 766
public class UILayoutZoneSettings : MonoBehaviour
{
	// Token: 0x060024ED RID: 9453 RVA: 0x001046E0 File Offset: 0x001028E0
	private void SetUIFromZoneOptions(LayoutZone zone)
	{
		this.TriggerForFaceUp.value = zone.Options.TriggerForFaceUp;
		this.TriggerForFaceDown.value = zone.Options.TriggerForFaceDown;
		this.TriggerForNonCards.value = zone.Options.TriggerForNonCards;
		this.SplitAddedDecks.value = zone.Options.SplitAddedDecks;
		this.CombineIntoDecks.value = zone.Options.CombineIntoDecks;
		UIInput cardsPerDeck = this.CardsPerDeck;
		LayoutZoneOptions options = zone.Options;
		cardsPerDeck.value = options.CardsPerDeck.ToString();
		UIPopupList directionPopupList = this.DirectionPopupList;
		options = zone.Options;
		directionPopupList.value = options.Direction.ToString();
		UIPopupList facingPopupList = this.FacingPopupList;
		options = zone.Options;
		facingPopupList.value = options.NewObjectFacing.ToString();
		this.HorizontalPadding.value = this.paddingSliderFromValue(zone.Options.HorizontalGroupPadding);
		this.VerticalPadding.value = this.paddingSliderFromValue(zone.Options.VerticalGroupPadding);
		this.StickyCards.value = zone.Options.StickyCards;
		this.InstantRefill.value = zone.Options.InstantRefill;
		this.Randomize.value = zone.Options.Randomize;
		this.ManualOnly.value = zone.Options.ManualOnly;
		UIPopupList meldDirectionPopupList = this.MeldDirectionPopupList;
		options = zone.Options;
		meldDirectionPopupList.value = options.MeldDirection.ToString();
		UIPopupList meldSortPopupList = this.MeldSortPopupList;
		options = zone.Options;
		meldSortPopupList.value = options.MeldSort.ToString();
		this.MeldSortReversed.value = zone.Options.MeldReverseSort;
		this.MeldSortExisting.value = zone.Options.MeldSortExisting;
		this.HorizontalSpread.value = this.spreadSliderFromValue(zone.Options.HorizontalSpread);
		this.VerticalSpread.value = this.spreadSliderFromValue(zone.Options.VerticalSpread);
		UIInput objectsPerGroup = this.ObjectsPerGroup;
		options = zone.Options;
		objectsPerGroup.value = options.MaxObjectsPerNewGroup.ToString();
		this.AlternateDirection.value = zone.Options.AlternateDirection;
		UIInput maxObjectsPerGroup = this.MaxObjectsPerGroup;
		options = zone.Options;
		maxObjectsPerGroup.value = options.MaxObjectsPerGroup.ToString();
		this.AllowSwapping.value = zone.Options.AllowSwapping;
	}

	// Token: 0x060024EE RID: 9454 RVA: 0x00104968 File Offset: 0x00102B68
	private void SetZoneFromUIOptions()
	{
		if (this.TargetLayoutZone == null)
		{
			return;
		}
		this.TargetLayoutZone.SetOptions(new LayoutZoneOptions(LayoutZone.DirectionFromString(this.DirectionPopupList.value), LayoutZone.MeldDirectionFromString(this.MeldDirectionPopupList.value), LayoutZone.FacingFromString(this.FacingPopupList.value), this.TriggerForFaceUp.value, this.TriggerForFaceDown.value, this.TriggerForNonCards.value, this.AllowSwapping.value, LibString.IntOrDefault(this.ObjectsPerGroup.value, this.TargetLayoutZone.Options.MaxObjectsPerNewGroup), LibString.IntOrDefault(this.MaxObjectsPerGroup.value, this.TargetLayoutZone.Options.MaxObjectsPerGroup), LayoutZone.MeldSortFromString(this.MeldSortPopupList.value), this.MeldSortReversed.value, this.MeldSortExisting.value, this.StickyCards.value, this.valueFromSpreadSlider(this.HorizontalSpread.value), this.valueFromSpreadSlider(this.VerticalSpread.value), this.valueFromPaddingSlider(this.HorizontalPadding.value), this.valueFromPaddingSlider(this.VerticalPadding.value), this.SplitAddedDecks.value, this.CombineIntoDecks.value, LibString.IntOrDefault(this.CardsPerDeck.value, this.TargetLayoutZone.Options.CardsPerDeck), this.AlternateDirection.value, this.Randomize.value, this.InstantRefill.value, this.ManualOnly.value), false);
	}

	// Token: 0x060024EF RID: 9455 RVA: 0x00104B08 File Offset: 0x00102D08
	private void Awake()
	{
		this.DirectionPopupList.items = new List<string>(LayoutZone.DirectionList);
		this.MeldDirectionPopupList.items = new List<string>(LayoutZone.MeldDirectionList);
		this.MeldSortPopupList.items = new List<string>(LayoutZone.SortList);
		this.FacingPopupList.items = new List<string>(LayoutZone.FacingList);
		this.HorizontalPaddingSliderRange.SliderFromValueConverter = new UISliderRange.ValueConverterDelegate(this.paddingSliderFromValue);
		this.VerticalPaddingSliderRange.SliderFromValueConverter = new UISliderRange.ValueConverterDelegate(this.paddingSliderFromValue);
		this.HorizontalSpreadSliderRange.SliderFromValueConverter = new UISliderRange.ValueConverterDelegate(this.spreadSliderFromValue);
		this.VerticalSpreadSliderRange.SliderFromValueConverter = new UISliderRange.ValueConverterDelegate(this.spreadSliderFromValue);
		this.HorizontalPaddingSliderRange.ValueFromSliderConverter = new UISliderRange.ValueConverterDelegate(this.valueFromPaddingSlider);
		this.VerticalPaddingSliderRange.ValueFromSliderConverter = new UISliderRange.ValueConverterDelegate(this.valueFromPaddingSlider);
		this.HorizontalSpreadSliderRange.ValueFromSliderConverter = new UISliderRange.ValueConverterDelegate(this.valueFromSpreadSlider);
		this.VerticalSpreadSliderRange.ValueFromSliderConverter = new UISliderRange.ValueConverterDelegate(this.valueFromSpreadSlider);
		this.CardsPerDeck.SelectAllTextOnClick = true;
		this.MaxObjectsPerGroup.SelectAllTextOnClick = true;
		this.ObjectsPerGroup.SelectAllTextOnClick = true;
	}

	// Token: 0x060024F0 RID: 9456 RVA: 0x00104C48 File Offset: 0x00102E48
	private void Start()
	{
		EventDelegate.Add(this.TriggerForFaceUp.onChange, new EventDelegate.Callback(this.SetPassiveOption));
		EventDelegate.Add(this.TriggerForFaceDown.onChange, new EventDelegate.Callback(this.SetPassiveOption));
		EventDelegate.Add(this.TriggerForNonCards.onChange, new EventDelegate.Callback(this.SetPassiveOption));
		EventDelegate.Add(this.SplitAddedDecks.onChange, new EventDelegate.Callback(this.SetPassiveOption));
		EventDelegate.Add(this.CombineIntoDecks.onChange, new EventDelegate.Callback(this.SetPassiveOption));
		EventDelegate.Add(this.CardsPerDeck.onChange, new EventDelegate.Callback(this.SetPassiveOption));
		EventDelegate.Add(this.DirectionPopupList.onChange, new EventDelegate.Callback(this.SetActiveOption));
		EventDelegate.Add(this.FacingPopupList.onChange, new EventDelegate.Callback(this.SetPassiveOption));
		EventDelegate.Add(this.HorizontalPadding.onChange, new EventDelegate.Callback(this.SetActiveOption));
		EventDelegate.Add(this.VerticalPadding.onChange, new EventDelegate.Callback(this.SetActiveOption));
		EventDelegate.Add(this.StickyCards.onChange, new EventDelegate.Callback(this.SetActiveOptionAndForceLayout));
		EventDelegate.Add(this.InstantRefill.onChange, new EventDelegate.Callback(this.SetPassiveOption));
		EventDelegate.Add(this.Randomize.onChange, new EventDelegate.Callback(this.SetPassiveOption));
		EventDelegate.Add(this.ManualOnly.onChange, new EventDelegate.Callback(this.SetPassiveOption));
		EventDelegate.Add(this.MeldDirectionPopupList.onChange, new EventDelegate.Callback(this.SetActiveOptionAndForceLayout));
		EventDelegate.Add(this.MeldSortPopupList.onChange, new EventDelegate.Callback(this.SetSortOption));
		EventDelegate.Add(this.MeldSortReversed.onChange, new EventDelegate.Callback(this.SetSortOption));
		EventDelegate.Add(this.MeldSortExisting.onChange, new EventDelegate.Callback(this.SetActiveOptionAndForceLayout));
		EventDelegate.Add(this.HorizontalSpread.onChange, new EventDelegate.Callback(this.SetActiveOption));
		EventDelegate.Add(this.VerticalSpread.onChange, new EventDelegate.Callback(this.SetActiveOption));
		EventDelegate.Add(this.ObjectsPerGroup.onChange, new EventDelegate.Callback(this.SetPassiveOption));
		EventDelegate.Add(this.AlternateDirection.onChange, new EventDelegate.Callback(this.SetActiveOptionAndForceLayout));
		EventDelegate.Add(this.MaxObjectsPerGroup.onChange, new EventDelegate.Callback(this.SetActiveOption));
		EventDelegate.Add(this.AllowSwapping.onChange, new EventDelegate.Callback(this.SetPassiveOption));
	}

	// Token: 0x060024F1 RID: 9457 RVA: 0x00104F10 File Offset: 0x00103110
	private void OnDestroy()
	{
		EventDelegate.Remove(this.TriggerForFaceUp.onChange, new EventDelegate.Callback(this.SetPassiveOption));
		EventDelegate.Remove(this.TriggerForFaceDown.onChange, new EventDelegate.Callback(this.SetPassiveOption));
		EventDelegate.Remove(this.TriggerForNonCards.onChange, new EventDelegate.Callback(this.SetPassiveOption));
		EventDelegate.Remove(this.SplitAddedDecks.onChange, new EventDelegate.Callback(this.SetPassiveOption));
		EventDelegate.Remove(this.CombineIntoDecks.onChange, new EventDelegate.Callback(this.SetPassiveOption));
		EventDelegate.Remove(this.CardsPerDeck.onChange, new EventDelegate.Callback(this.SetPassiveOption));
		EventDelegate.Remove(this.DirectionPopupList.onChange, new EventDelegate.Callback(this.SetActiveOption));
		EventDelegate.Remove(this.FacingPopupList.onChange, new EventDelegate.Callback(this.SetPassiveOption));
		EventDelegate.Remove(this.HorizontalPadding.onChange, new EventDelegate.Callback(this.SetActiveOption));
		EventDelegate.Remove(this.VerticalPadding.onChange, new EventDelegate.Callback(this.SetActiveOption));
		EventDelegate.Remove(this.StickyCards.onChange, new EventDelegate.Callback(this.SetActiveOptionAndForceLayout));
		EventDelegate.Remove(this.InstantRefill.onChange, new EventDelegate.Callback(this.SetPassiveOption));
		EventDelegate.Remove(this.Randomize.onChange, new EventDelegate.Callback(this.SetPassiveOption));
		EventDelegate.Remove(this.ManualOnly.onChange, new EventDelegate.Callback(this.SetPassiveOption));
		EventDelegate.Remove(this.MeldDirectionPopupList.onChange, new EventDelegate.Callback(this.SetActiveOption));
		EventDelegate.Remove(this.MeldSortPopupList.onChange, new EventDelegate.Callback(this.SetSortOption));
		EventDelegate.Remove(this.MeldSortReversed.onChange, new EventDelegate.Callback(this.SetSortOption));
		EventDelegate.Remove(this.MeldSortExisting.onChange, new EventDelegate.Callback(this.SetActiveOptionAndForceLayout));
		EventDelegate.Remove(this.HorizontalSpread.onChange, new EventDelegate.Callback(this.SetActiveOption));
		EventDelegate.Remove(this.VerticalSpread.onChange, new EventDelegate.Callback(this.SetActiveOption));
		EventDelegate.Remove(this.ObjectsPerGroup.onChange, new EventDelegate.Callback(this.SetPassiveOption));
		EventDelegate.Remove(this.AlternateDirection.onChange, new EventDelegate.Callback(this.SetActiveOptionAndForceLayout));
		EventDelegate.Remove(this.MaxObjectsPerGroup.onChange, new EventDelegate.Callback(this.SetActiveOption));
		EventDelegate.Remove(this.AllowSwapping.onChange, new EventDelegate.Callback(this.SetPassiveOption));
	}

	// Token: 0x060024F2 RID: 9458 RVA: 0x001051D5 File Offset: 0x001033D5
	private void SetPassiveOption()
	{
		if (!this.eventsActive || this.TargetLayoutZone == null)
		{
			return;
		}
		this.SetZoneFromUIOptions();
	}

	// Token: 0x060024F3 RID: 9459 RVA: 0x001051F4 File Offset: 0x001033F4
	private void SetActiveOption()
	{
		if (!this.eventsActive || this.TargetLayoutZone == null)
		{
			return;
		}
		this.SetZoneFromUIOptions();
		this.TargetLayoutZone.QueueUpdate(-1, 1f, false);
	}

	// Token: 0x060024F4 RID: 9460 RVA: 0x00105225 File Offset: 0x00103425
	private void SetActiveOptionAndForceLayout()
	{
		if (!this.eventsActive || this.TargetLayoutZone == null)
		{
			return;
		}
		this.SetZoneFromUIOptions();
		this.TargetLayoutZone.QueueUpdate(-1, 1f, true);
	}

	// Token: 0x060024F5 RID: 9461 RVA: 0x00105256 File Offset: 0x00103456
	private void SetSortOption()
	{
		if (!this.eventsActive || this.TargetLayoutZone == null)
		{
			return;
		}
		if (this.MeldSortExisting.value)
		{
			this.SetActiveOptionAndForceLayout();
			return;
		}
		this.SetPassiveOption();
	}

	// Token: 0x060024F6 RID: 9462 RVA: 0x0010528C File Offset: 0x0010348C
	public void Activate(LayoutZone zone)
	{
		if (!zone)
		{
			return;
		}
		this.eventsActive = false;
		this.TargetLayoutZone = zone;
		base.gameObject.SetActive(true);
		this.SetUIFromZoneOptions(this.TargetLayoutZone);
		Wait.Frames(new Action(this.EnableEvents), 1);
	}

	// Token: 0x060024F7 RID: 9463 RVA: 0x001052DB File Offset: 0x001034DB
	private void EnableEvents()
	{
		this.eventsActive = true;
	}

	// Token: 0x060024F8 RID: 9464 RVA: 0x000BCD9D File Offset: 0x000BAF9D
	public void Cancel()
	{
		base.gameObject.SetActive(false);
	}

	// Token: 0x060024F9 RID: 9465 RVA: 0x001052E4 File Offset: 0x001034E4
	private float paddingSliderFromValue(float value)
	{
		if (value > 2f)
		{
			return (value - 2f) / 16f + 0.5f;
		}
		return value / 4f;
	}

	// Token: 0x060024FA RID: 9466 RVA: 0x00105309 File Offset: 0x00103509
	private float valueFromPaddingSlider(float slider)
	{
		if (slider <= 0.5f)
		{
			return slider * 4f;
		}
		return (slider - 0.5f) * 16f + 2f;
	}

	// Token: 0x060024FB RID: 9467 RVA: 0x0010532E File Offset: 0x0010352E
	private float spreadSliderFromValue(float value)
	{
		if (value > 1f)
		{
			return (value - 1f) / 8f + 0.5f;
		}
		return value / 2f;
	}

	// Token: 0x060024FC RID: 9468 RVA: 0x00105353 File Offset: 0x00103553
	private float valueFromSpreadSlider(float slider)
	{
		if (slider <= 0.5f)
		{
			return slider * 2f;
		}
		return (slider - 0.5f) * 8f + 1f;
	}

	// Token: 0x060024FD RID: 9469 RVA: 0x00105378 File Offset: 0x00103578
	public void TabChange()
	{
		this.eventsActive = false;
		this.GroupsTabGroup.SetActive(this.GroupsTab.value);
		this.LayoutTabGroup.SetActive(this.LayoutTab.value);
		Wait.Frames(new Action(this.EnableEvents), 1);
	}

	// Token: 0x040017E2 RID: 6114
	private const float DELAY_BEFORE_UPDATE = 1f;

	// Token: 0x040017E3 RID: 6115
	private LayoutZone TargetLayoutZone;

	// Token: 0x040017E4 RID: 6116
	public GameObject LayoutTabGroup;

	// Token: 0x040017E5 RID: 6117
	public GameObject GroupsTabGroup;

	// Token: 0x040017E6 RID: 6118
	public UIToggle LayoutTab;

	// Token: 0x040017E7 RID: 6119
	public UIToggle GroupsTab;

	// Token: 0x040017E8 RID: 6120
	public UIToggle TriggerForFaceUp;

	// Token: 0x040017E9 RID: 6121
	public UIToggle TriggerForFaceDown;

	// Token: 0x040017EA RID: 6122
	public UIToggle TriggerForNonCards;

	// Token: 0x040017EB RID: 6123
	public UIToggle SplitAddedDecks;

	// Token: 0x040017EC RID: 6124
	public UIToggle CombineIntoDecks;

	// Token: 0x040017ED RID: 6125
	public UIInput CardsPerDeck;

	// Token: 0x040017EE RID: 6126
	public UIPopupList DirectionPopupList;

	// Token: 0x040017EF RID: 6127
	public UIPopupList FacingPopupList;

	// Token: 0x040017F0 RID: 6128
	public UIScrollBar HorizontalPadding;

	// Token: 0x040017F1 RID: 6129
	public UIScrollBar VerticalPadding;

	// Token: 0x040017F2 RID: 6130
	public UISliderRange HorizontalPaddingSliderRange;

	// Token: 0x040017F3 RID: 6131
	public UISliderRange VerticalPaddingSliderRange;

	// Token: 0x040017F4 RID: 6132
	public UIToggle StickyCards;

	// Token: 0x040017F5 RID: 6133
	public UIToggle InstantRefill;

	// Token: 0x040017F6 RID: 6134
	public UIToggle Randomize;

	// Token: 0x040017F7 RID: 6135
	public UIToggle ManualOnly;

	// Token: 0x040017F8 RID: 6136
	public UIPopupList MeldDirectionPopupList;

	// Token: 0x040017F9 RID: 6137
	public UIPopupList MeldSortPopupList;

	// Token: 0x040017FA RID: 6138
	public UIToggle MeldSortReversed;

	// Token: 0x040017FB RID: 6139
	public UIToggle MeldSortExisting;

	// Token: 0x040017FC RID: 6140
	public UIScrollBar HorizontalSpread;

	// Token: 0x040017FD RID: 6141
	public UIScrollBar VerticalSpread;

	// Token: 0x040017FE RID: 6142
	public UISliderRange HorizontalSpreadSliderRange;

	// Token: 0x040017FF RID: 6143
	public UISliderRange VerticalSpreadSliderRange;

	// Token: 0x04001800 RID: 6144
	public UIInput ObjectsPerGroup;

	// Token: 0x04001801 RID: 6145
	public UIToggle AlternateDirection;

	// Token: 0x04001802 RID: 6146
	public UIInput MaxObjectsPerGroup;

	// Token: 0x04001803 RID: 6147
	public UIToggle AllowSwapping;

	// Token: 0x04001804 RID: 6148
	private bool eventsActive;
}
