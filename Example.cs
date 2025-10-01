using System;
using UnityEngine;

// Token: 0x02000098 RID: 152
public class Example : MonoBehaviour
{
	// Token: 0x06000814 RID: 2068 RVA: 0x00038510 File Offset: 0x00036710
	private void Start()
	{
		MonoBehaviour.print("Global position: " + ProMouse.Instance.GetGlobalMousePosition());
		MonoBehaviour.print("Local  position: " + ProMouse.Instance.GetLocalMousePosition());
		MonoBehaviour.print("Main screen size: " + ProMouse.Instance.GetMainScreenSize());
		MonoBehaviour.print("Set mouse to center of the unity screen.");
		ProMouse.Instance.SetCursorPosition(Screen.width / 2, Screen.height / 2);
	}

	// Token: 0x06000815 RID: 2069 RVA: 0x0003859C File Offset: 0x0003679C
	private void OnGUI()
	{
		if (GUI.Button(new Rect(0f, 0f, 100f, 20f), "Go Down"))
		{
			ProMouse.Instance.SetCursorPosition(50, 10);
		}
		if (GUI.Button(new Rect(0f, (float)(Screen.height - 20), 100f, 20f), "Go Right"))
		{
			ProMouse.Instance.SetCursorPosition(Screen.width - 50, 10);
		}
		if (GUI.Button(new Rect((float)(Screen.width - 100), (float)(Screen.height - 20), 100f, 20f), "Go Up"))
		{
			ProMouse.Instance.SetCursorPosition(Screen.width - 50, Screen.height - 10);
		}
		if (GUI.Button(new Rect((float)(Screen.width - 100), 0f, 100f, 20f), "Go Left"))
		{
			ProMouse.Instance.SetCursorPosition(50, Screen.height - 10);
		}
		GUILayout.BeginArea(new Rect((float)(Screen.width / 2 - 125), (float)(Screen.height / 2 - 75), 250f, 200f));
		GUILayout.Label("<b>Window Position: " + this.GetWindowPosition() + "</b>", Array.Empty<GUILayoutOption>());
		GUILayout.Space(10f);
		GUILayout.Label("Mouse Coord  Unity: " + ProMouse.Instance.GetLocalMousePosition(), Array.Empty<GUILayoutOption>());
		GUILayout.Label("Mouse Coord Global: " + ProMouse.Instance.GetGlobalMousePosition(), Array.Empty<GUILayoutOption>());
		GUILayout.Label(string.Concat(new object[]
		{
			"Main Display  Size: ",
			ProMouse.Instance.GetMainScreenSize().x,
			"x",
			ProMouse.Instance.GetMainScreenSize().y
		}), Array.Empty<GUILayoutOption>());
		GUILayout.Label(string.Concat(new object[]
		{
			"Player Screen Size: ",
			Screen.width,
			"x",
			Screen.height
		}), Array.Empty<GUILayoutOption>());
		GUILayout.Label("Global mouse coordinates:", Array.Empty<GUILayoutOption>());
		GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
		if (GUILayout.Button("To 0,0 global", Array.Empty<GUILayoutOption>()))
		{
			ProMouse.Instance.SetGlobalCursorPosition(0, 0);
		}
		if (GUILayout.Button("To Scr.w,Scr.h", Array.Empty<GUILayoutOption>()))
		{
			ProMouse.Instance.SetGlobalCursorPosition((int)ProMouse.Instance.GetMainScreenSize().x, (int)ProMouse.Instance.GetMainScreenSize().y);
		}
		GUILayout.EndHorizontal();
		GUILayout.EndArea();
	}

	// Token: 0x06000816 RID: 2070 RVA: 0x00038848 File Offset: 0x00036A48
	private Vector2 GetWindowPosition()
	{
		Vector2 vector = ProMouse.Instance.GetGlobalMousePosition() - ProMouse.Instance.GetLocalMousePosition();
		return new Vector2((float)((int)vector.x), (float)((int)(vector.y + (float)Screen.height)));
	}
}
