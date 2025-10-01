using System;

// Token: 0x02000219 RID: 537
public class TabletState : IEquatable<TabletState>
{
	// Token: 0x06001B07 RID: 6919 RVA: 0x000BB726 File Offset: 0x000B9926
	public bool Equals(TabletState other)
	{
		return other != null && (this == other || this.PageURL == other.PageURL);
	}

	// Token: 0x06001B08 RID: 6920 RVA: 0x000BB744 File Offset: 0x000B9944
	public override bool Equals(object obj)
	{
		return obj != null && (this == obj || (!(obj.GetType() != base.GetType()) && this.Equals((TabletState)obj)));
	}

	// Token: 0x06001B09 RID: 6921 RVA: 0x000BB772 File Offset: 0x000B9972
	public override int GetHashCode()
	{
		if (this.PageURL == null)
		{
			return 0;
		}
		return this.PageURL.GetHashCode();
	}

	// Token: 0x06001B0A RID: 6922 RVA: 0x000B7302 File Offset: 0x000B5502
	public static bool operator ==(TabletState left, TabletState right)
	{
		return object.Equals(left, right);
	}

	// Token: 0x06001B0B RID: 6923 RVA: 0x000B730B File Offset: 0x000B550B
	public static bool operator !=(TabletState left, TabletState right)
	{
		return !object.Equals(left, right);
	}

	// Token: 0x04001128 RID: 4392
	[Tag(TagType.URL)]
	public string PageURL = "";
}
