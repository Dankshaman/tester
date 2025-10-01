using System;
using System.Collections.Generic;

// Token: 0x020001F2 RID: 498
public class SaveState : IEquatable<SaveState>
{
	// Token: 0x06001A1A RID: 6682 RVA: 0x000B6BA8 File Offset: 0x000B4DA8
	public bool Equals(SaveState other)
	{
		if (other == null)
		{
			return false;
		}
		if (this == other)
		{
			return true;
		}
		if (this.SaveName == other.SaveName)
		{
			uint? epochTime = this.EpochTime;
			uint? epochTime2 = other.EpochTime;
			if ((epochTime.GetValueOrDefault() == epochTime2.GetValueOrDefault() & epochTime != null == (epochTime2 != null)) && this.Date == other.Date && this.VersionNumber == other.VersionNumber && this.GameMode == other.GameMode && this.GameType == other.GameType && this.GameComplexity == other.GameComplexity && this.PlayingTime.SequenceEqualNullable(other.PlayingTime) && this.PlayerCounts.SequenceEqualNullable(other.PlayerCounts) && this.Tags.SequenceEqualNullable(other.Tags) && this.Gravity.Equals(other.Gravity) && this.PlayArea.Equals(other.PlayArea) && this.Table == other.Table && this.TableURL == other.TableURL && this.Sky == other.Sky && this.SkyURL == other.SkyURL && this.Note == other.Note && this.Rules == other.Rules && object.Equals(this.MusicPlayer, other.MusicPlayer) && object.Equals(this.Grid, other.Grid) && object.Equals(this.Lighting, other.Lighting) && object.Equals(this.Hands, other.Hands) && object.Equals(this.ComponentTags, other.ComponentTags) && object.Equals(this.Turns, other.Turns) && this.TabStates.SequenceEqualNullable(other.TabStates) && this.CameraStates.SequenceEqualNullable(other.CameraStates) && this.DecalPallet.SequenceEqualNullable(other.DecalPallet) && this.LuaScript == other.LuaScript && this.LuaScriptState == other.LuaScriptState && this.XmlUI == other.XmlUI && this.CustomUIAssets.SequenceEqualNullable(other.CustomUIAssets) && this.Decals.SequenceEqualNullable(other.Decals) && this.VectorLines.SequenceEqualNullable(other.VectorLines) && this.SnapPoints.SequenceEqualNullable(other.SnapPoints))
			{
				return this.ObjectStates.SequenceEqualNullable(other.ObjectStates);
			}
		}
		return false;
	}

	// Token: 0x06001A1B RID: 6683 RVA: 0x000B6ECB File Offset: 0x000B50CB
	public override bool Equals(object obj)
	{
		return obj != null && (this == obj || (!(obj.GetType() != base.GetType()) && this.Equals((SaveState)obj)));
	}

	// Token: 0x06001A1C RID: 6684 RVA: 0x000B6EFC File Offset: 0x000B50FC
	public override int GetHashCode()
	{
		return (((((((((((((((((((((((((((((((((((this.SaveName != null) ? this.SaveName.GetHashCode() : 0) * 397 ^ this.EpochTime.GetHashCode()) * 397 ^ ((this.Date != null) ? this.Date.GetHashCode() : 0)) * 397 ^ ((this.VersionNumber != null) ? this.VersionNumber.GetHashCode() : 0)) * 397 ^ ((this.GameMode != null) ? this.GameMode.GetHashCode() : 0)) * 397 ^ ((this.GameType != null) ? this.GameType.GetHashCode() : 0)) * 397 ^ ((this.GameComplexity != null) ? this.GameComplexity.GetHashCode() : 0)) * 397 ^ ((this.PlayingTime != null) ? this.PlayingTime.GetHashCode() : 0)) * 397 ^ ((this.PlayerCounts != null) ? this.PlayerCounts.GetHashCode() : 0)) * 397 ^ ((this.Tags != null) ? this.Tags.GetHashCode() : 0)) * 397 ^ this.Gravity.GetHashCode()) * 397 ^ this.PlayArea.GetHashCode()) * 397 ^ ((this.Table != null) ? this.Table.GetHashCode() : 0)) * 397 ^ ((this.TableURL != null) ? this.TableURL.GetHashCode() : 0)) * 397 ^ ((this.Sky != null) ? this.Sky.GetHashCode() : 0)) * 397 ^ ((this.SkyURL != null) ? this.SkyURL.GetHashCode() : 0)) * 397 ^ ((this.Note != null) ? this.Note.GetHashCode() : 0)) * 397 ^ ((this.Rules != null) ? this.Rules.GetHashCode() : 0)) * 397 ^ ((this.MusicPlayer != null) ? this.MusicPlayer.GetHashCode() : 0)) * 397 ^ ((this.Grid != null) ? this.Grid.GetHashCode() : 0)) * 397 ^ ((this.Lighting != null) ? this.Lighting.GetHashCode() : 0)) * 397 ^ ((this.Hands != null) ? this.Hands.GetHashCode() : 0)) * 397 ^ ((this.ComponentTags != null) ? this.ComponentTags.GetHashCode() : 0)) * 397 ^ ((this.Turns != null) ? this.Turns.GetHashCode() : 0)) * 397 ^ ((this.TabStates != null) ? this.TabStates.GetHashCode() : 0)) * 397 ^ ((this.CameraStates != null) ? this.CameraStates.GetHashCode() : 0)) * 397 ^ ((this.DecalPallet != null) ? this.DecalPallet.GetHashCode() : 0)) * 397 ^ ((this.LuaScript != null) ? this.LuaScript.GetHashCode() : 0)) * 397 ^ ((this.LuaScriptState != null) ? this.LuaScriptState.GetHashCode() : 0)) * 397 ^ ((this.XmlUI != null) ? this.XmlUI.GetHashCode() : 0)) * 397 ^ ((this.CustomUIAssets != null) ? this.CustomUIAssets.GetHashCode() : 0)) * 397 ^ ((this.Decals != null) ? this.Decals.GetHashCode() : 0)) * 397 ^ ((this.VectorLines != null) ? this.VectorLines.GetHashCode() : 0)) * 397 ^ ((this.SnapPoints != null) ? this.SnapPoints.GetHashCode() : 0)) * 397 ^ ((this.ObjectStates != null) ? this.ObjectStates.GetHashCode() : 0);
	}

	// Token: 0x06001A1D RID: 6685 RVA: 0x000B7302 File Offset: 0x000B5502
	public static bool operator ==(SaveState left, SaveState right)
	{
		return object.Equals(left, right);
	}

	// Token: 0x06001A1E RID: 6686 RVA: 0x000B730B File Offset: 0x000B550B
	public static bool operator !=(SaveState left, SaveState right)
	{
		return !object.Equals(left, right);
	}

	// Token: 0x04001018 RID: 4120
	public string SaveName = "";

	// Token: 0x04001019 RID: 4121
	public uint? EpochTime;

	// Token: 0x0400101A RID: 4122
	public string Date = "";

	// Token: 0x0400101B RID: 4123
	public string VersionNumber = "";

	// Token: 0x0400101C RID: 4124
	public string GameMode = "";

	// Token: 0x0400101D RID: 4125
	public string GameType = "";

	// Token: 0x0400101E RID: 4126
	public string GameComplexity = "";

	// Token: 0x0400101F RID: 4127
	public int[] PlayingTime;

	// Token: 0x04001020 RID: 4128
	public int[] PlayerCounts;

	// Token: 0x04001021 RID: 4129
	public List<string> Tags = new List<string>();

	// Token: 0x04001022 RID: 4130
	public float Gravity = 0.5f;

	// Token: 0x04001023 RID: 4131
	public float PlayArea = 0.5f;

	// Token: 0x04001024 RID: 4132
	public string Table = "";

	// Token: 0x04001025 RID: 4133
	[Tag(TagType.URL)]
	public string TableURL;

	// Token: 0x04001026 RID: 4134
	public string Sky = "";

	// Token: 0x04001027 RID: 4135
	[Tag(TagType.URL)]
	public string SkyURL;

	// Token: 0x04001028 RID: 4136
	public string Note = "";

	// Token: 0x04001029 RID: 4137
	public Dictionary<int, TabState> TabStates = new Dictionary<int, TabState>();

	// Token: 0x0400102A RID: 4138
	public MusicPlayerState MusicPlayer;

	// Token: 0x0400102B RID: 4139
	public GridState Grid;

	// Token: 0x0400102C RID: 4140
	public LightingState Lighting;

	// Token: 0x0400102D RID: 4141
	public HandsState Hands;

	// Token: 0x0400102E RID: 4142
	public TagsState ComponentTags;

	// Token: 0x0400102F RID: 4143
	public TurnsState Turns;

	// Token: 0x04001030 RID: 4144
	public CameraState[] CameraStates;

	// Token: 0x04001031 RID: 4145
	public List<CustomDecalState> DecalPallet;

	// Token: 0x04001032 RID: 4146
	public string LuaScript = "";

	// Token: 0x04001033 RID: 4147
	public string LuaScriptState = "";

	// Token: 0x04001034 RID: 4148
	public string XmlUI = "";

	// Token: 0x04001035 RID: 4149
	public List<CustomAssetState> CustomUIAssets;

	// Token: 0x04001036 RID: 4150
	public List<DecalState> Decals;

	// Token: 0x04001037 RID: 4151
	public List<VectorLineState> VectorLines;

	// Token: 0x04001038 RID: 4152
	public List<SnapPointState> SnapPoints;

	// Token: 0x04001039 RID: 4153
	public List<ObjectState> ObjectStates;

	// Token: 0x0400103A RID: 4154
	public string Rules;
}
