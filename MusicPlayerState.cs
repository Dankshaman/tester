using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

// Token: 0x020001F4 RID: 500
public class MusicPlayerState : IEquatable<MusicPlayerState>
{
	// Token: 0x06001A29 RID: 6697 RVA: 0x000B87FC File Offset: 0x000B69FC
	public bool Equals(MusicPlayerState other)
	{
		return other != null && (this == other || (this.RepeatSong == other.RepeatSong && this.PlaylistEntry == other.PlaylistEntry && this.CurrentAudioTitle == other.CurrentAudioTitle && this.CurrentAudioURL == other.CurrentAudioURL && this.AudioLibrary.SequenceEqualNullable(other.AudioLibrary)));
	}

	// Token: 0x06001A2A RID: 6698 RVA: 0x000B8869 File Offset: 0x000B6A69
	public override bool Equals(object obj)
	{
		return obj != null && (this == obj || (!(obj.GetType() != base.GetType()) && this.Equals((MusicPlayerState)obj)));
	}

	// Token: 0x06001A2B RID: 6699 RVA: 0x000B8898 File Offset: 0x000B6A98
	public override int GetHashCode()
	{
		return (((this.RepeatSong.GetHashCode() * 397 ^ this.PlaylistEntry) * 397 ^ ((this.CurrentAudioTitle != null) ? this.CurrentAudioTitle.GetHashCode() : 0)) * 397 ^ ((this.CurrentAudioURL != null) ? this.CurrentAudioURL.GetHashCode() : 0)) * 397 ^ ((this.AudioLibrary != null) ? this.AudioLibrary.GetHashCode() : 0);
	}

	// Token: 0x06001A2C RID: 6700 RVA: 0x000B7302 File Offset: 0x000B5502
	public static bool operator ==(MusicPlayerState left, MusicPlayerState right)
	{
		return object.Equals(left, right);
	}

	// Token: 0x06001A2D RID: 6701 RVA: 0x000B730B File Offset: 0x000B550B
	public static bool operator !=(MusicPlayerState left, MusicPlayerState right)
	{
		return !object.Equals(left, right);
	}

	// Token: 0x04001081 RID: 4225
	public bool RepeatSong;

	// Token: 0x04001082 RID: 4226
	public int PlaylistEntry = -1;

	// Token: 0x04001083 RID: 4227
	public string CurrentAudioTitle = "Unknown Title";

	// Token: 0x04001084 RID: 4228
	public string CurrentAudioURL = "";

	// Token: 0x04001085 RID: 4229
	[TupleElementNames(new string[]
	{
		"AudioUrl",
		"AudioName"
	})]
	public List<ValueTuple<string, string>> AudioLibrary = new List<ValueTuple<string, string>>();
}
