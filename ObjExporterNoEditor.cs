using System;
using System.IO;
using System.Text;
using UnityEngine;

// Token: 0x020001CF RID: 463
public class ObjExporterNoEditor : MonoBehaviour
{
	// Token: 0x0600184B RID: 6219 RVA: 0x000025B8 File Offset: 0x000007B8
	private void Start()
	{
	}

	// Token: 0x0600184C RID: 6220 RVA: 0x000025B8 File Offset: 0x000007B8
	private void Update()
	{
	}

	// Token: 0x0600184D RID: 6221 RVA: 0x000A5430 File Offset: 0x000A3630
	public void DoExport(bool makeSubmeshes, string path)
	{
		string name = base.gameObject.name;
		string text = path + name + ".obj";
		ObjExporterScript.Start();
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.Append(string.Concat(new string[]
		{
			"#",
			name,
			".obj\n#",
			DateTime.Now.ToLongDateString(),
			"\n#",
			DateTime.Now.ToLongTimeString(),
			"\n#-------\n\n"
		}));
		Transform transform = base.gameObject.transform;
		Vector3 position = transform.position;
		transform.position = Vector3.zero;
		if (!makeSubmeshes)
		{
			stringBuilder.Append("g ").Append(transform.name).Append("\n");
		}
		stringBuilder.Append(ObjExporterNoEditor.processTransform(transform, makeSubmeshes));
		ObjExporterNoEditor.WriteToFile(stringBuilder.ToString(), text);
		transform.position = position;
		ObjExporterScript.End();
		Debug.Log("Exported Mesh: " + text);
	}

	// Token: 0x0600184E RID: 6222 RVA: 0x000A5534 File Offset: 0x000A3734
	private static string processTransform(Transform t, bool makeSubmeshes)
	{
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.Append("#" + t.name + "\n#-------\n");
		if (makeSubmeshes)
		{
			stringBuilder.Append("g ").Append(t.name).Append("\n");
		}
		MeshFilter component = t.GetComponent<MeshFilter>();
		if (component)
		{
			stringBuilder.Append(ObjExporterScript.MeshToString(component, t));
		}
		for (int i = 0; i < t.childCount; i++)
		{
			stringBuilder.Append(ObjExporterNoEditor.processTransform(t.GetChild(i), makeSubmeshes));
		}
		return stringBuilder.ToString();
	}

	// Token: 0x0600184F RID: 6223 RVA: 0x000A55D0 File Offset: 0x000A37D0
	public static void WriteToFile(string s, string filename)
	{
		using (StreamWriter streamWriter = new StreamWriter(filename))
		{
			streamWriter.Write(s);
		}
	}
}
