using System;
using UnityEngine;

// Token: 0x02000147 RID: 327
public static class LibMath
{
	// Token: 0x060010C4 RID: 4292 RVA: 0x00074291 File Offset: 0x00072491
	public static bool CloseEnoughDegrees(float a, float b, float epsilon = 1f)
	{
		return Mathf.Min(new float[]
		{
			Mathf.Abs(a - b),
			Mathf.Abs(a + 360f - b),
			Mathf.Abs(a - 360f - b)
		}) < epsilon;
	}

	// Token: 0x060010C5 RID: 4293 RVA: 0x000742CE File Offset: 0x000724CE
	public static bool CloseEnoughXZ(NetworkPhysicsObject npo1, NetworkPhysicsObject npo2, float epsilon = 0.1f)
	{
		return LibMath.CloseEnoughXZ(npo1.transform.position, npo2.transform.position, epsilon);
	}

	// Token: 0x060010C6 RID: 4294 RVA: 0x000742EC File Offset: 0x000724EC
	public static bool CloseEnoughXZ(NetworkPhysicsObject npo, Vector3 position, float epsilon = 0.1f)
	{
		return LibMath.CloseEnoughXZ(npo.transform.position, position, epsilon);
	}

	// Token: 0x060010C7 RID: 4295 RVA: 0x00074300 File Offset: 0x00072500
	public static bool CloseEnoughXZ(Vector3 pos1, Vector3 pos2, float epsilon = 0.1f)
	{
		float num = Mathf.Abs(pos1.x - pos2.x);
		float num2 = Mathf.Abs(pos1.z - pos2.z);
		return num < epsilon && num2 < epsilon;
	}

	// Token: 0x060010C8 RID: 4296 RVA: 0x0007433C File Offset: 0x0007253C
	public static bool CloseEnoughXYZ(Vector3 pos1, Vector3 pos2, Vector3 checkSize, out float distance)
	{
		float num = Mathf.Abs(pos1.x - pos2.x);
		float num2 = Mathf.Abs(pos1.z - pos2.z);
		float num3 = Mathf.Abs(pos1.y - pos2.y);
		distance = num + num3 + num2;
		return num < checkSize.x && num2 < checkSize.z && num3 < checkSize.y;
	}
}
