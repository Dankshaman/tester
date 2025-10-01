using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

// Token: 0x020000F5 RID: 245
public static class LibDebug
{
	// Token: 0x06000C01 RID: 3073 RVA: 0x00051CD7 File Offset: 0x0004FED7
	public static void Identify(Transform transform, string pre = "")
	{
		Debug.Log(pre + transform.name);
		if (transform.parent != null)
		{
			LibDebug.Identify(transform.parent, pre + " ");
		}
	}

	// Token: 0x06000C02 RID: 3074 RVA: 0x00051D0E File Offset: 0x0004FF0E
	public static void Identify(GameObject gameObject, string pre = "")
	{
		LibDebug.Identify(gameObject.transform, pre);
	}

	// Token: 0x06000C03 RID: 3075 RVA: 0x00051D1C File Offset: 0x0004FF1C
	public static string IdentifyPath(Transform t, string path = "")
	{
		path = ((path == "") ? t.name : (t.name + ">" + path));
		if (t.parent == null)
		{
			return path;
		}
		return LibDebug.IdentifyPath(t.parent, path);
	}

	// Token: 0x06000C04 RID: 3076 RVA: 0x00051D6D File Offset: 0x0004FF6D
	public static void LogPath(Transform t, bool expanded = false)
	{
		LibDebug.LogPath(t.gameObject, expanded);
	}

	// Token: 0x06000C05 RID: 3077 RVA: 0x00051D7C File Offset: 0x0004FF7C
	public static void LogPath(GameObject go, bool expanded = false)
	{
		if (expanded)
		{
			LibDebug.Identify(go.transform, "");
			return;
		}
		string text = LibDebug.IdentifyPath(go.transform, "");
		Debug.Log(text);
		LibDebug.Paths.Add(text);
	}

	// Token: 0x06000C06 RID: 3078 RVA: 0x00051DBF File Offset: 0x0004FFBF
	public static void LogPath(string message)
	{
		LibDebug.Paths.Add("- " + message);
		Debug.Log(message);
	}

	// Token: 0x06000C07 RID: 3079 RVA: 0x00051DDC File Offset: 0x0004FFDC
	public static bool IsPath(string s)
	{
		return !s.StartsWith("- ");
	}

	// Token: 0x06000C08 RID: 3080 RVA: 0x00051DEC File Offset: 0x0004FFEC
	public static void ClearPaths()
	{
		LibDebug.Paths.Clear();
	}

	// Token: 0x06000C09 RID: 3081 RVA: 0x00051DF8 File Offset: 0x0004FFF8
	public static void log(params object[] args)
	{
		if (args.Length == 0)
		{
			return;
		}
		StringBuilder stringBuilder = new StringBuilder();
		if (args[0] == null)
		{
			stringBuilder.Append("null");
		}
		else
		{
			stringBuilder.Append(args[0].ToString());
		}
		for (int i = 1; i < args.Length; i++)
		{
			stringBuilder.Append(" ");
			stringBuilder.Append(args[i]);
		}
		Debug.Log(stringBuilder.ToString());
	}

	// Token: 0x06000C0A RID: 3082 RVA: 0x00051E61 File Offset: 0x00050061
	public static Colour NextColor()
	{
		LibDebug.currentDebugColorIndex = (LibDebug.currentDebugColorIndex + 1) % LibDebug.debugColors.Length;
		return LibDebug.debugColors[LibDebug.currentDebugColorIndex];
	}

	// Token: 0x06000C0B RID: 3083 RVA: 0x00051E8B File Offset: 0x0005008B
	public static Colour ColorForObject(object o)
	{
		return LibDebug.debugColors[Mathf.Abs(o.GetHashCode()) % LibDebug.debugColors.Length];
	}

	// Token: 0x06000C0C RID: 3084 RVA: 0x00051EB0 File Offset: 0x000500B0
	public static void DrawLines(Colour color, params Vector3[] points)
	{
		for (int i = 0; i < points.Length - 1; i++)
		{
			Singleton<DebugManager>.Instance.Line(points, color, 0.2f);
		}
	}

	// Token: 0x06000C0D RID: 3085 RVA: 0x00051EE3 File Offset: 0x000500E3
	public static void DrawLines(params Vector3[] points)
	{
		LibDebug.DrawLines(LibDebug.NextColor(), points);
	}

	// Token: 0x06000C0E RID: 3086 RVA: 0x00051EF0 File Offset: 0x000500F0
	public static void DrawLines(object o, params Vector3[] points)
	{
		LibDebug.DrawLines(LibDebug.ColorForObject(o), points);
	}

	// Token: 0x06000C0F RID: 3087 RVA: 0x00051EFE File Offset: 0x000500FE
	public static void DrawSphere(Colour color, Vector3 point, float size = 1f)
	{
		Singleton<DebugManager>.Instance.Sphere(point, color, size);
	}

	// Token: 0x06000C10 RID: 3088 RVA: 0x00051F12 File Offset: 0x00050112
	public static void DrawSphere(object o, Vector3 point, float size = 1f)
	{
		LibDebug.DrawSphere(LibDebug.ColorForObject(o), point, size);
	}

	// Token: 0x06000C11 RID: 3089 RVA: 0x00051F21 File Offset: 0x00050121
	public static void DrawSphere(Vector3 point, float size = 1f)
	{
		LibDebug.DrawSphere(LibDebug.NextColor(), point, size);
	}

	// Token: 0x04000840 RID: 2112
	public static List<string> Paths = new List<string>();

	// Token: 0x04000841 RID: 2113
	private static Color[] debugColors = new Color[]
	{
		Color.magenta,
		Color.green,
		Color.red,
		Color.cyan,
		Color.yellow
	};

	// Token: 0x04000842 RID: 2114
	private static int currentDebugColorIndex = -1;

	// Token: 0x04000843 RID: 2115
	private const string pathMessagePrefix = "- ";
}
