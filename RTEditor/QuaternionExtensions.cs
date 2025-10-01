using System;
using System.IO;
using UnityEngine;

namespace RTEditor
{
	// Token: 0x020003F8 RID: 1016
	public static class QuaternionExtensions
	{
		// Token: 0x06002ECB RID: 11979 RVA: 0x00140A8E File Offset: 0x0013EC8E
		public static void WriteBinary(this Quaternion quaternion, BinaryWriter writer)
		{
			writer.Write(quaternion.x);
			writer.Write(quaternion.y);
			writer.Write(quaternion.z);
			writer.Write(quaternion.w);
		}

		// Token: 0x06002ECC RID: 11980 RVA: 0x00140AC0 File Offset: 0x0013ECC0
		public static Quaternion ReadBinary(BinaryReader reader)
		{
			float x = reader.ReadSingle();
			float y = reader.ReadSingle();
			float z = reader.ReadSingle();
			float w = reader.ReadSingle();
			return new Quaternion(x, y, z, w);
		}
	}
}
