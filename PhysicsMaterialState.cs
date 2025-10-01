using System;
using UnityEngine;

// Token: 0x0200020B RID: 523
public class PhysicsMaterialState : IEquatable<PhysicsMaterialState>
{
	// Token: 0x06001AB0 RID: 6832 RVA: 0x000BA648 File Offset: 0x000B8848
	public bool Equals(PhysicsMaterialState other)
	{
		return other != null && (this == other || (this.StaticFriction.Equals(other.StaticFriction) && this.DynamicFriction.Equals(other.DynamicFriction) && this.Bounciness.Equals(other.Bounciness) && this.FrictionCombine == other.FrictionCombine && this.BounceCombine == other.BounceCombine));
	}

	// Token: 0x06001AB1 RID: 6833 RVA: 0x000BA6B7 File Offset: 0x000B88B7
	public override bool Equals(object obj)
	{
		return obj != null && (this == obj || (!(obj.GetType() != base.GetType()) && this.Equals((PhysicsMaterialState)obj)));
	}

	// Token: 0x06001AB2 RID: 6834 RVA: 0x000BA6E8 File Offset: 0x000B88E8
	public override int GetHashCode()
	{
		return (((this.StaticFriction.GetHashCode() * 397 ^ this.DynamicFriction.GetHashCode()) * 397 ^ this.Bounciness.GetHashCode()) * 397 ^ (int)this.FrictionCombine) * 397 ^ (int)this.BounceCombine;
	}

	// Token: 0x06001AB3 RID: 6835 RVA: 0x000B7302 File Offset: 0x000B5502
	public static bool operator ==(PhysicsMaterialState left, PhysicsMaterialState right)
	{
		return object.Equals(left, right);
	}

	// Token: 0x06001AB4 RID: 6836 RVA: 0x000B730B File Offset: 0x000B550B
	public static bool operator !=(PhysicsMaterialState left, PhysicsMaterialState right)
	{
		return !object.Equals(left, right);
	}

	// Token: 0x040010E5 RID: 4325
	public float StaticFriction = 0.4f;

	// Token: 0x040010E6 RID: 4326
	public float DynamicFriction = 0.4f;

	// Token: 0x040010E7 RID: 4327
	public float Bounciness;

	// Token: 0x040010E8 RID: 4328
	public PhysicMaterialCombine FrictionCombine;

	// Token: 0x040010E9 RID: 4329
	public PhysicMaterialCombine BounceCombine;
}
