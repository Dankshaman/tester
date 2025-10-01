using System;

// Token: 0x02000187 RID: 391
public class LuaLayoutZoneOptions
{
	// Token: 0x060011D9 RID: 4569 RVA: 0x00002594 File Offset: 0x00000794
	public LuaLayoutZoneOptions()
	{
	}

	// Token: 0x060011DA RID: 4570 RVA: 0x00079630 File Offset: 0x00077830
	public LuaLayoutZoneOptions(LayoutZoneOptions options)
	{
		this.direction = new int?((int)options.Direction);
		this.meld_direction = new int?((int)options.MeldDirection);
		this.new_object_facing = new int?((int)options.NewObjectFacing);
		this.trigger_for_face_up = new bool?(options.TriggerForFaceUp);
		this.trigger_for_face_down = new bool?(options.TriggerForFaceDown);
		this.trigger_for_non_cards = new bool?(options.TriggerForNonCards);
		this.allow_swapping = new bool?(options.AllowSwapping);
		this.max_objects_per_new_group = new int?(options.MaxObjectsPerNewGroup);
		this.max_objects_per_group = new int?(options.MaxObjectsPerGroup);
		this.meld_sort = new int?((int)options.MeldSort);
		this.meld_reverse_sort = new bool?(options.MeldReverseSort);
		this.meld_sort_existing = new bool?(options.MeldSortExisting);
		this.sticky_cards = new bool?(options.StickyCards);
		this.horizontal_spread = new float?(options.HorizontalSpread);
		this.vertical_spread = new float?(options.VerticalSpread);
		this.horizontal_group_padding = new float?(options.HorizontalGroupPadding);
		this.vertical_group_padding = new float?(options.VerticalGroupPadding);
		this.split_added_decks = new bool?(options.SplitAddedDecks);
		this.combine_into_decks = new bool?(options.CombineIntoDecks);
		this.cards_per_deck = new int?(options.CardsPerDeck);
		this.alternate_direction = new bool?(options.AlternateDirection);
		this.randomize = new bool?(options.Randomize);
		this.instant_refill = new bool?(options.InstantRefill);
		this.manual_only = new bool?(options.ManualOnly);
	}

	// Token: 0x04000B6C RID: 2924
	public int? direction;

	// Token: 0x04000B6D RID: 2925
	public int? meld_direction;

	// Token: 0x04000B6E RID: 2926
	public int? new_object_facing;

	// Token: 0x04000B6F RID: 2927
	public bool? trigger_for_face_up;

	// Token: 0x04000B70 RID: 2928
	public bool? trigger_for_face_down;

	// Token: 0x04000B71 RID: 2929
	public bool? trigger_for_non_cards;

	// Token: 0x04000B72 RID: 2930
	public bool? allow_swapping;

	// Token: 0x04000B73 RID: 2931
	public int? max_objects_per_new_group;

	// Token: 0x04000B74 RID: 2932
	public int? max_objects_per_group;

	// Token: 0x04000B75 RID: 2933
	public int? meld_sort;

	// Token: 0x04000B76 RID: 2934
	public bool? meld_reverse_sort;

	// Token: 0x04000B77 RID: 2935
	public bool? meld_sort_existing;

	// Token: 0x04000B78 RID: 2936
	public bool? sticky_cards;

	// Token: 0x04000B79 RID: 2937
	public float? horizontal_spread;

	// Token: 0x04000B7A RID: 2938
	public float? vertical_spread;

	// Token: 0x04000B7B RID: 2939
	public float? horizontal_group_padding;

	// Token: 0x04000B7C RID: 2940
	public float? vertical_group_padding;

	// Token: 0x04000B7D RID: 2941
	public bool? split_added_decks;

	// Token: 0x04000B7E RID: 2942
	public bool? combine_into_decks;

	// Token: 0x04000B7F RID: 2943
	public int? cards_per_deck;

	// Token: 0x04000B80 RID: 2944
	public bool? alternate_direction;

	// Token: 0x04000B81 RID: 2945
	public bool? randomize;

	// Token: 0x04000B82 RID: 2946
	public bool? instant_refill;

	// Token: 0x04000B83 RID: 2947
	public bool? manual_only;
}
