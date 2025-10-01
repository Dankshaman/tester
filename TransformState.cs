using System;
using Newtonsoft.Json;
using UnityEngine;

// Token: 0x02000207 RID: 519
public class TransformState : IEquatable<TransformState>
{
	// Token: 0x06001A87 RID: 6791 RVA: 0x00002594 File Offset: 0x00000794
	[JsonConstructor]
	public TransformState()
	{
	}

	// Token: 0x06001A88 RID: 6792 RVA: 0x000B9EE0 File Offset: 0x000B80E0
	public TransformState(Transform T)
	{
		Vector3 position = T.position;
		this.posX = position.x;
		this.posY = position.y;
		this.posZ = position.z;
		Vector3 eulerAngles = T.eulerAngles;
		this.rotX = eulerAngles.x;
		this.rotY = eulerAngles.y;
		this.rotZ = eulerAngles.z;
		Vector3 localScale = T.localScale;
		this.scaleX = localScale.x;
		this.scaleY = localScale.y;
		this.scaleZ = localScale.z;
	}

	// Token: 0x06001A89 RID: 6793 RVA: 0x000B9F74 File Offset: 0x000B8174
	public TransformState(Vector3 pos, Vector3 rot, Vector3 scale)
	{
		this.posX = pos.x;
		this.posY = pos.y;
		this.posZ = pos.z;
		this.rotX = rot.x;
		this.rotY = rot.y;
		this.rotZ = rot.z;
		this.scaleX = scale.x;
		this.scaleY = scale.y;
		this.scaleZ = scale.z;
	}

	// Token: 0x06001A8A RID: 6794 RVA: 0x000B9FF3 File Offset: 0x000B81F3
	public Vector3 Position()
	{
		return new Vector3(this.posX, this.posY, this.posZ);
	}

	// Token: 0x06001A8B RID: 6795 RVA: 0x000BA00C File Offset: 0x000B820C
	public Vector3 Rotation()
	{
		return new Vector3(this.rotX, this.rotY, this.rotZ);
	}

	// Token: 0x06001A8C RID: 6796 RVA: 0x000BA025 File Offset: 0x000B8225
	public Vector3 Scale()
	{
		return new Vector3(this.scaleX, this.scaleY, this.scaleZ);
	}

	// Token: 0x06001A8D RID: 6797 RVA: 0x000BA040 File Offset: 0x000B8240
	public bool Equals(TransformState other)
	{
		return other != null && (this == other || (this.posX.ApproximatelyEquals(other.posX, 0.01f) && this.posY.ApproximatelyEquals(other.posY, 0.01f) && this.posZ.ApproximatelyEquals(other.posZ, 0.01f) && this.rotX.ApproximatelyEquals(other.rotX, 0.01f) && this.rotY.ApproximatelyEquals(other.rotY, 0.01f) && this.rotZ.ApproximatelyEquals(other.rotZ, 0.01f) && this.scaleX.ApproximatelyEquals(other.scaleX, 0.01f) && this.scaleY.ApproximatelyEquals(other.scaleY, 0.01f) && this.scaleZ.ApproximatelyEquals(other.scaleZ, 0.01f)));
	}

	// Token: 0x06001A8E RID: 6798 RVA: 0x000BA139 File Offset: 0x000B8339
	public override bool Equals(object obj)
	{
		return obj != null && (this == obj || (!(obj.GetType() != base.GetType()) && this.Equals((TransformState)obj)));
	}

	// Token: 0x06001A8F RID: 6799 RVA: 0x000BA168 File Offset: 0x000B8368
	public override int GetHashCode()
	{
		return (((((((this.posX.GetHashCode() * 397 ^ this.posY.GetHashCode()) * 397 ^ this.posZ.GetHashCode()) * 397 ^ this.rotX.GetHashCode()) * 397 ^ this.rotY.GetHashCode()) * 397 ^ this.rotZ.GetHashCode()) * 397 ^ this.scaleX.GetHashCode()) * 397 ^ this.scaleY.GetHashCode()) * 397 ^ this.scaleZ.GetHashCode();
	}

	// Token: 0x06001A90 RID: 6800 RVA: 0x000B7302 File Offset: 0x000B5502
	public static bool operator ==(TransformState left, TransformState right)
	{
		return object.Equals(left, right);
	}

	// Token: 0x06001A91 RID: 6801 RVA: 0x000B730B File Offset: 0x000B550B
	public static bool operator !=(TransformState left, TransformState right)
	{
		return !object.Equals(left, right);
	}

	// Token: 0x040010D1 RID: 4305
	public float posX;

	// Token: 0x040010D2 RID: 4306
	public float posY;

	// Token: 0x040010D3 RID: 4307
	public float posZ;

	// Token: 0x040010D4 RID: 4308
	public float rotX;

	// Token: 0x040010D5 RID: 4309
	public float rotY;

	// Token: 0x040010D6 RID: 4310
	public float rotZ;

	// Token: 0x040010D7 RID: 4311
	public float scaleX;

	// Token: 0x040010D8 RID: 4312
	public float scaleY;

	// Token: 0x040010D9 RID: 4313
	public float scaleZ;
}
