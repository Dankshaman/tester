using System;

// Token: 0x02000204 RID: 516
public class RotationValueState : IEquatable<RotationValueState>
{
	// Token: 0x06001A7B RID: 6779 RVA: 0x000B9D82 File Offset: 0x000B7F82
	public bool Equals(RotationValueState other)
	{
		return other != null && (this == other || (object.Equals(this.Value, other.Value) && this.Rotation.Equals(other.Rotation)));
	}

	// Token: 0x06001A7C RID: 6780 RVA: 0x000B9DB5 File Offset: 0x000B7FB5
	public override bool Equals(object obj)
	{
		return obj != null && (this == obj || (!(obj.GetType() != base.GetType()) && this.Equals((RotationValueState)obj)));
	}

	// Token: 0x06001A7D RID: 6781 RVA: 0x000B9DE3 File Offset: 0x000B7FE3
	public override int GetHashCode()
	{
		return ((this.Value != null) ? this.Value.GetHashCode() : 0) * 397 ^ this.Rotation.GetHashCode();
	}

	// Token: 0x06001A7E RID: 6782 RVA: 0x000B7302 File Offset: 0x000B5502
	public static bool operator ==(RotationValueState left, RotationValueState right)
	{
		return object.Equals(left, right);
	}

	// Token: 0x06001A7F RID: 6783 RVA: 0x000B730B File Offset: 0x000B550B
	public static bool operator !=(RotationValueState left, RotationValueState right)
	{
		return !object.Equals(left, right);
	}

	// Token: 0x040010C9 RID: 4297
	public string Value;

	// Token: 0x040010CA RID: 4298
	public VectorState Rotation;
}
