using System;
using UnityEngine;

// Token: 0x02000209 RID: 521
public struct VectorState : IEquatable<VectorState>
{
	// Token: 0x06001AA0 RID: 6816 RVA: 0x000BA423 File Offset: 0x000B8623
	public VectorState(float x, float y, float z)
	{
		this.x = x;
		this.y = y;
		this.z = z;
	}

	// Token: 0x06001AA1 RID: 6817 RVA: 0x000BA43A File Offset: 0x000B863A
	public VectorState(Vector3 vector)
	{
		this.x = vector.x;
		this.y = vector.y;
		this.z = vector.z;
	}

	// Token: 0x06001AA2 RID: 6818 RVA: 0x000BA460 File Offset: 0x000B8660
	public Vector3 ToVector()
	{
		return new Vector3(this.x, this.y, this.z);
	}

	// Token: 0x06001AA3 RID: 6819 RVA: 0x000BA479 File Offset: 0x000B8679
	public static implicit operator Vector3(VectorState vectorState)
	{
		return vectorState.ToVector();
	}

	// Token: 0x06001AA4 RID: 6820 RVA: 0x000BA482 File Offset: 0x000B8682
	public static implicit operator VectorState(Vector3 vector)
	{
		return new VectorState(vector);
	}

	// Token: 0x06001AA5 RID: 6821 RVA: 0x000BA48A File Offset: 0x000B868A
	public bool Equals(VectorState other)
	{
		return this.x.Equals(other.x) && this.y.Equals(other.y) && this.z.Equals(other.z);
	}

	// Token: 0x06001AA6 RID: 6822 RVA: 0x000BA4C8 File Offset: 0x000B86C8
	public override bool Equals(object obj)
	{
		if (obj is VectorState)
		{
			VectorState other = (VectorState)obj;
			return this.Equals(other);
		}
		return false;
	}

	// Token: 0x06001AA7 RID: 6823 RVA: 0x000BA4EF File Offset: 0x000B86EF
	public override int GetHashCode()
	{
		return (this.x.GetHashCode() * 397 ^ this.y.GetHashCode()) * 397 ^ this.z.GetHashCode();
	}

	// Token: 0x06001AA8 RID: 6824 RVA: 0x000BA520 File Offset: 0x000B8720
	public static bool operator ==(VectorState left, VectorState right)
	{
		return left.Equals(right);
	}

	// Token: 0x06001AA9 RID: 6825 RVA: 0x000BA52A File Offset: 0x000B872A
	public static bool operator !=(VectorState left, VectorState right)
	{
		return !left.Equals(right);
	}

	// Token: 0x040010DE RID: 4318
	public float x;

	// Token: 0x040010DF RID: 4319
	public float y;

	// Token: 0x040010E0 RID: 4320
	public float z;
}
