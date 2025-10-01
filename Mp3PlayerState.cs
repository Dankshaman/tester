using System;

// Token: 0x0200021D RID: 541
public class Mp3PlayerState : IEquatable<Mp3PlayerState>
{
	// Token: 0x06001B1F RID: 6943 RVA: 0x000BB8CC File Offset: 0x000B9ACC
	public bool Equals(Mp3PlayerState other)
	{
		return other != null && (this == other || (this.songTitle == other.songTitle && this.genre == other.genre && this.volume.Equals(other.volume) && this.isPlaying == other.isPlaying && this.loopOne == other.loopOne && this.menuTitle == other.menuTitle && this.menu == other.menu));
	}

	// Token: 0x06001B20 RID: 6944 RVA: 0x000BB95C File Offset: 0x000B9B5C
	public override bool Equals(object obj)
	{
		return obj != null && (this == obj || (!(obj.GetType() != base.GetType()) && this.Equals((Mp3PlayerState)obj)));
	}

	// Token: 0x06001B21 RID: 6945 RVA: 0x000BB98C File Offset: 0x000B9B8C
	public override int GetHashCode()
	{
		return (((((((this.songTitle != null) ? this.songTitle.GetHashCode() : 0) * 397 ^ ((this.genre != null) ? this.genre.GetHashCode() : 0)) * 397 ^ this.volume.GetHashCode()) * 397 ^ this.isPlaying.GetHashCode()) * 397 ^ this.loopOne.GetHashCode()) * 397 ^ ((this.menuTitle != null) ? this.menuTitle.GetHashCode() : 0)) * 397 ^ (int)this.menu;
	}

	// Token: 0x06001B22 RID: 6946 RVA: 0x000B7302 File Offset: 0x000B5502
	public static bool operator ==(Mp3PlayerState left, Mp3PlayerState right)
	{
		return object.Equals(left, right);
	}

	// Token: 0x06001B23 RID: 6947 RVA: 0x000B730B File Offset: 0x000B550B
	public static bool operator !=(Mp3PlayerState left, Mp3PlayerState right)
	{
		return !object.Equals(left, right);
	}

	// Token: 0x0400112E RID: 4398
	public string songTitle = "";

	// Token: 0x0400112F RID: 4399
	public string genre = "";

	// Token: 0x04001130 RID: 4400
	public float volume = 0.5f;

	// Token: 0x04001131 RID: 4401
	public bool isPlaying;

	// Token: 0x04001132 RID: 4402
	public bool loopOne;

	// Token: 0x04001133 RID: 4403
	public string menuTitle = "GENRES";

	// Token: 0x04001134 RID: 4404
	public Menus menu;
}
