using System;

// Token: 0x0200020A RID: 522
public class RigidbodyState : IEquatable<RigidbodyState>
{
	// Token: 0x06001AAA RID: 6826 RVA: 0x000BA538 File Offset: 0x000B8738
	public bool Equals(RigidbodyState other)
	{
		return other != null && (this == other || (this.Mass.Equals(other.Mass) && this.Drag.Equals(other.Drag) && this.AngularDrag.Equals(other.AngularDrag) && this.UseGravity == other.UseGravity));
	}

	// Token: 0x06001AAB RID: 6827 RVA: 0x000BA599 File Offset: 0x000B8799
	public override bool Equals(object obj)
	{
		return obj != null && (this == obj || (!(obj.GetType() != base.GetType()) && this.Equals((RigidbodyState)obj)));
	}

	// Token: 0x06001AAC RID: 6828 RVA: 0x000BA5C8 File Offset: 0x000B87C8
	public override int GetHashCode()
	{
		return ((this.Mass.GetHashCode() * 397 ^ this.Drag.GetHashCode()) * 397 ^ this.AngularDrag.GetHashCode()) * 397 ^ this.UseGravity.GetHashCode();
	}

	// Token: 0x06001AAD RID: 6829 RVA: 0x000B7302 File Offset: 0x000B5502
	public static bool operator ==(RigidbodyState left, RigidbodyState right)
	{
		return object.Equals(left, right);
	}

	// Token: 0x06001AAE RID: 6830 RVA: 0x000B730B File Offset: 0x000B550B
	public static bool operator !=(RigidbodyState left, RigidbodyState right)
	{
		return !object.Equals(left, right);
	}

	// Token: 0x040010E1 RID: 4321
	public float Mass = 1f;

	// Token: 0x040010E2 RID: 4322
	public float Drag = 0.1f;

	// Token: 0x040010E3 RID: 4323
	public float AngularDrag = 0.1f;

	// Token: 0x040010E4 RID: 4324
	public bool UseGravity = true;
}
