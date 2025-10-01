using System;
using System.Text;
using UnityEngine;

// Token: 0x020001D0 RID: 464
public class ObjExporterScript
{
	// Token: 0x06001851 RID: 6225 RVA: 0x000A5608 File Offset: 0x000A3808
	public static void Start()
	{
		ObjExporterScript.StartIndex = 0;
	}

	// Token: 0x06001852 RID: 6226 RVA: 0x000A5608 File Offset: 0x000A3808
	public static void End()
	{
		ObjExporterScript.StartIndex = 0;
	}

	// Token: 0x06001853 RID: 6227 RVA: 0x000A5610 File Offset: 0x000A3810
	public static string MeshToString(MeshFilter mf, Transform t)
	{
		Vector3 localScale = t.localScale;
		Vector3 localPosition = t.localPosition;
		Quaternion localRotation = t.localRotation;
		int num = 0;
		Mesh sharedMesh = mf.sharedMesh;
		if (!sharedMesh)
		{
			return "####Error####";
		}
		Material[] sharedMaterials = mf.GetComponent<Renderer>().sharedMaterials;
		StringBuilder stringBuilder = new StringBuilder();
		foreach (Vector3 position in sharedMesh.vertices)
		{
			Vector3 vector = t.TransformPoint(position);
			num++;
			stringBuilder.Append(string.Format("v {0} {1} {2}\n", vector.x, vector.y, -vector.z));
		}
		stringBuilder.Append("\n");
		foreach (Vector3 point in sharedMesh.normals)
		{
			Vector3 vector2 = localRotation * point;
			stringBuilder.Append(string.Format("vn {0} {1} {2}\n", -vector2.x, -vector2.y, vector2.z));
		}
		stringBuilder.Append("\n");
		Vector2[] uv = sharedMesh.uv;
		for (int i = 0; i < uv.Length; i++)
		{
			Vector3 vector3 = uv[i];
			stringBuilder.Append(string.Format("vt {0} {1}\n", vector3.x, vector3.y));
		}
		for (int j = 0; j < sharedMesh.subMeshCount; j++)
		{
			stringBuilder.Append("\n");
			stringBuilder.Append("usemtl ").Append(sharedMaterials[j].name).Append("\n");
			stringBuilder.Append("usemap ").Append(sharedMaterials[j].name).Append("\n");
			int[] triangles = sharedMesh.GetTriangles(j);
			for (int k = 0; k < triangles.Length; k += 3)
			{
				stringBuilder.Append(string.Format("f {0}/{0}/{0} {1}/{1}/{1} {2}/{2}/{2}\n", triangles[k] + 1 + ObjExporterScript.StartIndex, triangles[k + 1] + 1 + ObjExporterScript.StartIndex, triangles[k + 2] + 1 + ObjExporterScript.StartIndex));
			}
		}
		ObjExporterScript.StartIndex += num;
		return stringBuilder.ToString();
	}

	// Token: 0x04000E84 RID: 3716
	private static int StartIndex;
}
