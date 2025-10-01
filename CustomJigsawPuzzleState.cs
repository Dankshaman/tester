using System;

// Token: 0x02000213 RID: 531
public class CustomJigsawPuzzleState : IEquatable<CustomJigsawPuzzleState>
{
	// Token: 0x06001AE0 RID: 6880 RVA: 0x000BAEDF File Offset: 0x000B90DF
	public bool Equals(CustomJigsawPuzzleState other)
	{
		return other != null && (this == other || (this.NumPuzzlePieces == other.NumPuzzlePieces && this.ImageOnBoard == other.ImageOnBoard));
	}

	// Token: 0x06001AE1 RID: 6881 RVA: 0x000BAF0A File Offset: 0x000B910A
	public override bool Equals(object obj)
	{
		return obj != null && (this == obj || (!(obj.GetType() != base.GetType()) && this.Equals((CustomJigsawPuzzleState)obj)));
	}

	// Token: 0x06001AE2 RID: 6882 RVA: 0x000BAF38 File Offset: 0x000B9138
	public override int GetHashCode()
	{
		return this.NumPuzzlePieces * 397 ^ this.ImageOnBoard.GetHashCode();
	}

	// Token: 0x06001AE3 RID: 6883 RVA: 0x000B7302 File Offset: 0x000B5502
	public static bool operator ==(CustomJigsawPuzzleState left, CustomJigsawPuzzleState right)
	{
		return object.Equals(left, right);
	}

	// Token: 0x06001AE4 RID: 6884 RVA: 0x000B730B File Offset: 0x000B550B
	public static bool operator !=(CustomJigsawPuzzleState left, CustomJigsawPuzzleState right)
	{
		return !object.Equals(left, right);
	}

	// Token: 0x0400110B RID: 4363
	public int NumPuzzlePieces = 80;

	// Token: 0x0400110C RID: 4364
	public bool ImageOnBoard = true;
}
