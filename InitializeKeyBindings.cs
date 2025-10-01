using System;
using UnityEngine;

// Token: 0x02000133 RID: 307
public class InitializeKeyBindings : MonoBehaviour
{
	// Token: 0x0600102E RID: 4142 RVA: 0x0006EA0C File Offset: 0x0006CC0C
	private void Awake()
	{
		if (InitializeKeyBindings.Instance)
		{
			UnityEngine.Object.Destroy(base.gameObject);
			return;
		}
		InitializeKeyBindings.Instance = this;
		UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
		cInput.Init(false);
		cInput.ForbidKey(KeyCode.Escape);
		cInput.ForbidKey(KeyCode.Delete);
		cInput.ForbidKey(KeyCode.Backspace);
		cInput.allowDuplicates = true;
		cInput.SetKey("Left", "A");
		cInput.SetKey("Right", "D");
		cInput.SetAxis("Move Horizontal", "Left", "Right", 3f, 2f, 0.001f);
		cInput.SetKey("Down", "S");
		cInput.SetKey("Up", "W");
		cInput.SetAxis("Move Vertical", "Down", "Up", 3f, 2f, 0.001f);
		cInput.SetKey("Grab", "Mouse0");
		cInput.SetKey("Flip", "F", "Mouse2");
		cInput.SetKey("Tap", "T", "Mouse1");
		cInput.SetKey("Raise", "R");
		cInput.SetKey("Lock", "L");
		cInput.SetKey("Group", "G");
		cInput.SetKey("Blindfold", "B");
		cInput.SetKey("Under", "U");
		cInput.SetKey("Rotate Left", "Q", "Mouse Wheel Down");
		cInput.SetKey("Rotate Right", "E", "Mouse Wheel Up");
		cInput.SetAxis("Rotate", "Rotate Left", "Rotate Right");
		cInput.SetKey("Zoom Out", "Mouse Wheel Down");
		cInput.SetKey("Zoom In", "Mouse Wheel Up");
		cInput.SetAxis("Zoom", "Zoom Out", "Zoom In");
		cInput.SetKey("Zoom Toggle", "Z", "Mouse2");
		cInput.SetKey("Pan", "Z", "Mouse2");
		cInput.SetKey("Reset View", "Space");
		cInput.SetKey("Magnify", "M");
		cInput.SetKey("Nudge", "N");
		cInput.SetKey("Voice Chat", "C");
		cInput.SetKey("Voice Team Chat", "V");
		cInput.SetKey("Text Chat", "Return", "KeypadEnter");
		cInput.SetKey("Hide Hand", "H");
		cInput.SetKey("Hide GUI", "F11");
		cInput.SetKey("Help", "Question", "Slash");
		cInput.SetKey("Previous State", "PageUp");
		cInput.SetKey("Next State", "PageDown");
		cInput.SetKey("RPG Mode", "Alpha1");
		cInput.SetKey("RPG Attack", "Alpha2");
		cInput.SetKey("RPG Die", "Alpha3");
		cInput.SetKey("Camera Hold Rotate", "Mouse1");
		cInput.SetKey("Camera Left", "LeftArrow");
		cInput.SetKey("Camera Right", "RightArrow");
		cInput.SetAxis("Camera Horizontal", "Camera Left", "Camera Right");
		cInput.SetKey("Camera Down", "DownArrow");
		cInput.SetKey("Camera Up", "UpArrow");
		cInput.SetAxis("Camera Vertical", "Camera Down", "Camera Up");
		cInput.SetKey("Scale Down", "Minus", "Underscore");
		cInput.SetKey("Scale Up", "Plus", "Equals");
		cInput.SetKey("Camera Mode", "P");
		cInput.SetKey("Line Tool", "Tab");
		cInput.SetKey("Fly Down", "LeftControl");
		cInput.SetKey("Fly Up", "Space");
		cInput.SetAxis("Fly", "Fly Down", "Fly Up");
		cInput.SetKey("Alt", "LeftAlt", "RightAlt");
		cInput.SetKey("Ctrl", "LeftControl", "RightControl");
		cInput.SetKey("Shift", "LeftShift", "RightShift");
		cInput.SetKey("Cut", "X", "None");
		cInput.SetKey("Copy", "C", "None");
		cInput.SetKey("Paste", "V", "None");
		for (int i = 1; i <= zInput.ScriptingButtons.Count; i++)
		{
			zInput.KeyInfo keyInfo = zInput.ScriptingButtons[i];
			cInput.SetKey(keyInfo.Name, keyInfo.Key);
		}
		cInput.SetKey("System Console", "BackQuote");
		cInput.SetKey("Left (Controller)", Keys.Xbox1DPadLeft);
		cInput.SetKey("Right (Controller)", Keys.Xbox1DPadRight);
		cInput.SetAxis("Move Horizontal (Controller)", "Left (Controller)", "Right (Controller)", 3f, 2f, 0.001f);
		cInput.SetKey("Down (Controller)", Keys.Xbox1DPadDown);
		cInput.SetKey("Up (Controller)", Keys.Xbox1DPadUp);
		cInput.SetAxis("Move Vertical (Controller)", "Down (Controller)", "Up (Controller)", 3f, 2f, 0.001f);
		cInput.SetKey("Grab (Controller)", Keys.Xbox1BumperRight);
		cInput.SetKey("Flip (Controller)", Keys.Xbox1X);
		cInput.SetKey("Tap (Controller)", Keys.Xbox1A);
		cInput.SetKey("Raise (Controller)", "None");
		cInput.SetKey("Lock (Controller)", "None");
		cInput.SetKey("Rotate Left (Controller)", Keys.Xbox1TriggerLeft);
		cInput.SetKey("Rotate Right (Controller)", Keys.Xbox1TriggerRight);
		cInput.SetAxis("Rotate (Controller)", "Rotate Left (Controller)", "Rotate Right (Controller)");
		cInput.SetKey("Alt (Controller)", Keys.Xbox1BumperLeft);
		cInput.SetKey("Ctrl (Controller)", "None");
		cInput.SetKey("Zoom Out (Controller)", Keys.Xbox1TriggerLeft);
		cInput.SetKey("Zoom In (Controller)", Keys.Xbox1TriggerRight);
		cInput.SetAxis("Zoom (Controller)", "Zoom Out (Controller)", "Zoom In (Controller)", 0.1f);
		cInput.SetKey("Zoom Toggle (Controller)", Keys.Xbox1LStickButton);
		cInput.SetKey("Reset View (Controller)", Keys.Xbox1RStickButton);
		cInput.SetKey("Nudge (Controller)", "None");
		cInput.SetKey("Main Menu (Controller)", Keys.Xbox1Start);
		cInput.SetKey("Voice Chat (Controller)", Keys.Xbox1B);
		cInput.SetKey("Text Chat (Controller)", "None");
		cInput.SetKey("Hide GUI (Controller)", "None");
		cInput.SetKey("Help (Controller)", Keys.Xbox1Back);
		cInput.SetKey("RPG Mode (Controller)", "None");
		cInput.SetKey("RPG Attack (Controller)", "None");
		cInput.SetKey("RPG Die (Controller)", "None");
		cInput.SetKey("Camera Hold Rotate (Controller)", "None");
		cInput.SetKey("Camera Left (Controller)", Keys.Xbox1RStickLeft);
		cInput.SetKey("Camera Right (Controller)", Keys.Xbox1RStickRight);
		cInput.SetAxis("Camera Horizontal (Controller)", "Camera Left (Controller)", "Camera Right (Controller)", 2f, 2f, 0.1f);
		cInput.SetKey("Camera Down (Controller)", Keys.Xbox1RStickDown);
		cInput.SetKey("Camera Up (Controller)", Keys.Xbox1RStickUp);
		cInput.SetAxis("Camera Vertical (Controller)", "Camera Down (Controller)", "Camera Up (Controller)", 2f, 2f, 0.1f);
		cInput.SetKey("Pointer Left (Controller)", Keys.Xbox1LStickLeft);
		cInput.SetKey("Pointer Right (Controller)", Keys.Xbox1LStickRight);
		cInput.SetAxis("Pointer Horizontal (Controller)", "Pointer Left (Controller)", "Pointer Right (Controller)", 20f, float.MaxValue, 0.1f);
		cInput.SetKey("Pointer Down (Controller)", Keys.Xbox1LStickDown);
		cInput.SetKey("Pointer Up (Controller)", Keys.Xbox1LStickUp);
		cInput.SetAxis("Pointer Vertical (Controller)", "Pointer Down (Controller)", "Pointer Up (Controller)", 20f, float.MaxValue, 0.1f);
		cInput.SetKey("Scale Down (Controller)", "None");
		cInput.SetKey("Scale Up (Controller)", "None");
		cInput.SetKey("Camera Mode (Controller)", "None");
		cInput.SetKey("Line Tool (Controller)", Keys.Xbox1Y);
		cInput.SetKey("Fly Down (Controller)", "None");
		cInput.SetKey("Fly Up (Controller)", "None");
		cInput.SetAxis("Fly (Controller)", "Fly Down (Controller)", "Fly Up (Controller)");
		cInput.SetKey("Cut (Controller)", "None");
		cInput.SetKey("Copy (Controller)", "None");
		cInput.SetKey("Paste (Controller)", "None");
	}

	// Token: 0x04000A1F RID: 2591
	private static InitializeKeyBindings Instance;
}
