using System;

// Token: 0x02000212 RID: 530
public class CustomTileState : IEquatable<CustomTileState>
{
	// Token: 0x06001ADA RID: 6874 RVA: 0x000BAE1C File Offset: 0x000B901C
	public bool Equals(CustomTileState other)
	{
		return other != null && (this == other || (this.Type == other.Type && this.Thickness.Equals(other.Thickness) && this.Stackable == other.Stackable && this.Stretch == other.Stretch));
	}

	// Token: 0x06001ADB RID: 6875 RVA: 0x000BAE73 File Offset: 0x000B9073
	public override bool Equals(object obj)
	{
		return obj != null && (this == obj || (!(obj.GetType() != base.GetType()) && this.Equals((CustomTileState)obj)));
	}

	// Token: 0x06001ADC RID: 6876 RVA: 0x000BAEA1 File Offset: 0x000B90A1
	public override int GetHashCode()
	{
		return (int)(((this.Type * (TileType)397 ^ (TileType)this.Thickness.GetHashCode()) * (TileType)397 ^ (TileType)this.Stackable.GetHashCode()) * (TileType)397 ^ (TileType)this.Stretch.GetHashCode());
	}

	// Token: 0x06001ADD RID: 6877 RVA: 0x000B7302 File Offset: 0x000B5502
	public static bool operator ==(CustomTileState left, CustomTileState right)
	{
		return object.Equals(left, right);
	}

	// Token: 0x06001ADE RID: 6878 RVA: 0x000B730B File Offset: 0x000B550B
	public static bool operator !=(CustomTileState left, CustomTileState right)
	{
		return !object.Equals(left, right);
	}

	// Token: 0x04001107 RID: 4359
	public TileType Type;

	// Token: 0x04001108 RID: 4360
	public float Thickness;

	// Token: 0x04001109 RID: 4361
	public bool Stackable;

	// Token: 0x0400110A RID: 4362
	public bool Stretch;
}
