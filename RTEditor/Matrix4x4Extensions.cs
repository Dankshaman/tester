using System;
using UnityEngine;

namespace RTEditor
{
	// Token: 0x020003F6 RID: 1014
	public static class Matrix4x4Extensions
	{
		// Token: 0x06002EA7 RID: 11943 RVA: 0x0013FED1 File Offset: 0x0013E0D1
		public static Vector3[] GetAllAxes(this Matrix4x4 matrix)
		{
			return new Vector3[]
			{
				matrix.GetAxis(0),
				matrix.GetAxis(1),
				matrix.GetAxis(2)
			};
		}

		// Token: 0x06002EA8 RID: 11944 RVA: 0x0013FF04 File Offset: 0x0013E104
		public static Vector3 GetAxis(this Matrix4x4 matrix, int axisIndex)
		{
			Vector3 result = matrix.GetColumn(axisIndex);
			result.Normalize();
			return result;
		}

		// Token: 0x06002EA9 RID: 11945 RVA: 0x0013FF27 File Offset: 0x0013E127
		public static Vector3 GetTranslation(this Matrix4x4 matrix)
		{
			return matrix.GetColumn(3);
		}

		// Token: 0x06002EAA RID: 11946 RVA: 0x0013FF36 File Offset: 0x0013E136
		public static Matrix4x4 SetTranslation(this Matrix4x4 matrix, Vector3 translation)
		{
			matrix.SetColumn(3, new Vector4(translation.x, translation.y, translation.z, 1f));
			return matrix;
		}

		// Token: 0x06002EAB RID: 11947 RVA: 0x0013FF60 File Offset: 0x0013E160
		public static Quaternion GetRotation(this Matrix4x4 matrix)
		{
			Vector3 axis = matrix.GetAxis(1);
			return Quaternion.LookRotation(matrix.GetAxis(2), axis);
		}

		// Token: 0x06002EAC RID: 11948 RVA: 0x0013FF84 File Offset: 0x0013E184
		public static Matrix4x4 SetRotation(this Matrix4x4 matrix, Quaternion rotation)
		{
			Vector3 translation = matrix.GetTranslation();
			Vector3 xyzscale = matrix.GetXYZScale();
			return Matrix4x4.TRS(translation, rotation, xyzscale);
		}

		// Token: 0x06002EAD RID: 11949 RVA: 0x0013FFA8 File Offset: 0x0013E1A8
		public static Vector3 GetSignedXYZScale(this Matrix4x4 matrix)
		{
			return new Vector3(matrix.GetColumn(0)[0], matrix.GetColumn(1)[1], matrix.GetColumn(2)[2]);
		}

		// Token: 0x06002EAE RID: 11950 RVA: 0x0013FFF0 File Offset: 0x0013E1F0
		public static Vector3 GetXYZScale(this Matrix4x4 matrix)
		{
			return new Vector3(matrix.GetColumn(0).magnitude, matrix.GetColumn(1).magnitude, matrix.GetColumn(2).magnitude);
		}

		// Token: 0x06002EAF RID: 11951 RVA: 0x00140034 File Offset: 0x0013E234
		public static Matrix4x4 SetXYZScale(this Matrix4x4 matrix, Vector3 scale)
		{
			Vector3 translation = matrix.GetTranslation();
			Quaternion rotation = matrix.GetRotation();
			return Matrix4x4.TRS(translation, rotation, scale);
		}

		// Token: 0x06002EB0 RID: 11952 RVA: 0x00140055 File Offset: 0x0013E255
		public static Matrix4x4 SetXYZScale(this Matrix4x4 matrix, float scale)
		{
			return matrix.SetXYZScale(new Vector3(scale, scale, scale));
		}

		// Token: 0x06002EB1 RID: 11953 RVA: 0x00140068 File Offset: 0x0013E268
		public static Matrix4x4 SetScaleToOneOnAllAxes(this Matrix4x4 matrix)
		{
			for (int i = 0; i < 3; i++)
			{
				Vector4 column = matrix.GetColumn(i);
				Vector3 vector = column;
				vector.Normalize();
				matrix.SetColumn(i, new Vector4(vector.x, vector.y, vector.z, column.w));
			}
			return matrix;
		}

		// Token: 0x06002EB2 RID: 11954 RVA: 0x001400C0 File Offset: 0x0013E2C0
		public static Matrix4x4 Lerp(Matrix4x4 first, Matrix4x4 second, float t)
		{
			Vector4 column = first.GetColumn(0);
			Vector4 column2 = first.GetColumn(1);
			Vector4 column3 = first.GetColumn(2);
			Vector4 column4 = first.GetColumn(3);
			Vector4 column5 = second.GetColumn(0);
			Vector4 column6 = second.GetColumn(1);
			Vector4 column7 = second.GetColumn(2);
			Vector4 column8 = second.GetColumn(3);
			Vector4 column9 = Vector4.Lerp(column, column5, t);
			Vector4 column10 = Vector4.Lerp(column2, column6, t);
			Vector4 column11 = Vector4.Lerp(column3, column7, t);
			Vector4 column12 = Vector4.Lerp(column4, column8, t);
			Matrix4x4 result = default(Matrix4x4);
			result.SetColumn(0, column9);
			result.SetColumn(1, column10);
			result.SetColumn(2, column11);
			result.SetColumn(3, column12);
			return result;
		}
	}
}
