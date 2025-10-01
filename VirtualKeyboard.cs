using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using UnityEngine;

// Token: 0x02000380 RID: 896
public static class VirtualKeyboard
{
	// Token: 0x06002A07 RID: 10759
	[DllImport("user32")]
	private static extern IntPtr FindWindow(string sClassName, string sAppName);

	// Token: 0x06002A08 RID: 10760
	[DllImport("user32")]
	private static extern bool PostMessage(IntPtr hWnd, uint Msg, int wParam, int lParam);

	// Token: 0x06002A09 RID: 10761 RVA: 0x0012BEDE File Offset: 0x0012A0DE
	public static void ShowTouchKeyboard()
	{
		VirtualKeyboard.ExternalCall("TABTIP", null, false);
	}

	// Token: 0x06002A0A RID: 10762 RVA: 0x0012BEF0 File Offset: 0x0012A0F0
	public static void HideTouchKeyboard()
	{
		uint msg = 274U;
		int wParam = 61536;
		VirtualKeyboard.PostMessage(VirtualKeyboard.FindWindow("IPTip_Main_Window", null), msg, wParam, 0);
	}

	// Token: 0x06002A0B RID: 10763 RVA: 0x0012BF1D File Offset: 0x0012A11D
	public static void ShowOnScreenKeyboard()
	{
		if (VirtualKeyboard._onScreenKeyboardProcess == null || VirtualKeyboard._onScreenKeyboardProcess.HasExited)
		{
			VirtualKeyboard._onScreenKeyboardProcess = VirtualKeyboard.ExternalCall("OSK", null, false);
		}
	}

	// Token: 0x06002A0C RID: 10764 RVA: 0x0012BF43 File Offset: 0x0012A143
	public static void HideOnScreenKeyboard()
	{
		if (VirtualKeyboard._onScreenKeyboardProcess != null && !VirtualKeyboard._onScreenKeyboardProcess.HasExited)
		{
			VirtualKeyboard._onScreenKeyboardProcess.Kill();
		}
	}

	// Token: 0x06002A0D RID: 10765 RVA: 0x0012BF64 File Offset: 0x0012A164
	public static void RepositionOnScreenKeyboard(Rect rect)
	{
		VirtualKeyboard.ExternalCall("REG", "ADD HKCU\\Software\\Microsoft\\Osk /v WindowLeft /t REG_DWORD /d " + (int)rect.x + " /f", true);
		VirtualKeyboard.ExternalCall("REG", "ADD HKCU\\Software\\Microsoft\\Osk /v WindowTop /t REG_DWORD /d " + (int)rect.y + " /f", true);
		VirtualKeyboard.ExternalCall("REG", "ADD HKCU\\Software\\Microsoft\\Osk /v WindowWidth /t REG_DWORD /d " + (int)rect.width + " /f", true);
		VirtualKeyboard.ExternalCall("REG", "ADD HKCU\\Software\\Microsoft\\Osk /v WindowHeight /t REG_DWORD /d " + (int)rect.height + " /f", true);
	}

	// Token: 0x06002A0E RID: 10766 RVA: 0x0012C014 File Offset: 0x0012A214
	private static Process ExternalCall(string filename, string arguments, bool hideWindow)
	{
		ProcessStartInfo processStartInfo = new ProcessStartInfo();
		processStartInfo.FileName = filename;
		processStartInfo.Arguments = arguments;
		if (hideWindow)
		{
			processStartInfo.RedirectStandardOutput = true;
			processStartInfo.RedirectStandardError = true;
			processStartInfo.UseShellExecute = false;
			processStartInfo.CreateNoWindow = true;
		}
		Process process = new Process();
		process.StartInfo = processStartInfo;
		process.Start();
		return process;
	}

	// Token: 0x04001CAD RID: 7341
	private static Process _onScreenKeyboardProcess;
}
