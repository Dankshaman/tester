using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

// Token: 0x02000371 RID: 881
public class VRTest : MonoBehaviour
{
	// Token: 0x06002986 RID: 10630 RVA: 0x001234B0 File Offset: 0x001216B0
	private IEnumerator Start()
	{
		string[] supportedDevices = XRSettings.supportedDevices;
		List<string> list = new List<string>();
		for (int i = 0; i < supportedDevices.Length; i++)
		{
			if (supportedDevices[i] != "None")
			{
				list.Add(supportedDevices[i]);
			}
		}
		XRSettings.LoadDeviceByName(list.ToArray());
		yield return null;
		Debug.Log(Camera.main);
		Camera.main.ResetAspect();
		yield return null;
		XRSettings.enabled = true;
		yield break;
	}
}
