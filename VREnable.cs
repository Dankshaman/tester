using System;
using System.Collections;
using NewNet;
using UnityEngine;
using UnityEngine.XR;
using Valve.VR;

// Token: 0x02000367 RID: 871
public class VREnable : MonoBehaviour
{
	// Token: 0x06002914 RID: 10516 RVA: 0x00120F3D File Offset: 0x0011F13D
	private IEnumerator Start()
	{
		bool flag = Utilities.IsLaunchOption("-vr");
		UIOnScreenKeyboard.ON_SCREEN_KEYBOARD = ((flag && UIOnScreenKeyboard.DEFAULT_KEYBOARD_STATE == UIOnScreenKeyboard.KeyboardDefaultState.EnabledInVR) || UIOnScreenKeyboard.DEFAULT_KEYBOARD_STATE == UIOnScreenKeyboard.KeyboardDefaultState.Enabled);
		if (!flag)
		{
			XRSettings.enabled = false;
			base.gameObject.SetActive(false);
			yield break;
		}
		base.StartCoroutine(this.CheckForSteamVRBehaviour());
		Camera mainCamera = HoverScript.MainCamera;
		if (mainCamera)
		{
			mainCamera.tag = "Untagged";
			mainCamera.GetComponent<AudioListener>().enabled = false;
			Camera[] componentsInChildren = mainCamera.GetComponentsInChildren<Camera>(true);
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				componentsInChildren[i].enabled = false;
			}
		}
		else
		{
			Debug.Log("No main camera!");
		}
		SteamVR.Initialize(true);
		while (SteamVR.initializedState != SteamVR.InitializedStates.InitializeSuccess)
		{
			yield return null;
		}
		XRSettings.enabled = true;
		this.VR.SetActive(true);
		FastDrag.Enabled = true;
		SteamVR.settings.pauseGameWhenDashboardVisible = false;
		while (!SystemConsole.doneInitializing)
		{
			yield return null;
		}
		Singleton<SystemConsole>.Instance.RecheckTouchpadBindings();
		this.nextBindingCheck = Time.time + this.bindingDelay;
		yield break;
	}

	// Token: 0x06002915 RID: 10517 RVA: 0x00120F4C File Offset: 0x0011F14C
	private IEnumerator CheckForSteamVRBehaviour()
	{
		int tick = -1;
		SteamVR_Behaviour behaviour = null;
		while (behaviour == null)
		{
			int num = tick;
			tick = num + 1;
			behaviour = UnityEngine.Object.FindObjectOfType<SteamVR_Behaviour>();
			yield return null;
		}
		Debug.Log("Found SteamVR_Behaviour on tick " + tick);
		yield break;
	}

	// Token: 0x06002916 RID: 10518 RVA: 0x00120F54 File Offset: 0x0011F154
	private void Awake()
	{
		VREnable.Instance = this;
	}

	// Token: 0x06002917 RID: 10519 RVA: 0x00120F5C File Offset: 0x0011F15C
	private void Update()
	{
		if (!VRHMD.isVR)
		{
			return;
		}
		if ((Network.peerType == NetworkPeerMode.Disconnected || this.MissingBindingsWarning.activeInHierarchy) && Time.time > this.nextBindingCheck)
		{
			SteamVR_ActionSet_Manager.UpdateActionStates(false);
			if (this.MissingBindingsWarning.activeInHierarchy)
			{
				if (SteamVR_Actions.default_grab.activeBinding && SteamVR_Actions.default_main_menu.activeBinding)
				{
					this.MissingBindingsWarning.SetActive(false);
					this.bindingDelay = 5f;
				}
			}
			else if (VREnable.SHOW_MISSING_BINDING_WARNING && (!SteamVR_Actions.default_grab.activeBinding || !SteamVR_Actions.default_main_menu.activeBinding))
			{
				this.MissingBindingsWarning.SetActive(true);
				this.bindingDelay = 0.1f;
			}
			this.nextBindingCheck += this.bindingDelay;
		}
	}

	// Token: 0x06002918 RID: 10520 RVA: 0x00121025 File Offset: 0x0011F225
	public void HideMissingBindingsWarning()
	{
		this.MissingBindingsWarning.SetActive(false);
	}

	// Token: 0x04001AF5 RID: 6901
	[Header("###Use Tabletop Simulator window to toggle VR###")]
	public GameObject VR;

	// Token: 0x04001AF6 RID: 6902
	public GameObject MissingBindingsWarning;

	// Token: 0x04001AF7 RID: 6903
	public static bool SHOW_MISSING_BINDING_WARNING = true;

	// Token: 0x04001AF8 RID: 6904
	public static VREnable Instance;

	// Token: 0x04001AF9 RID: 6905
	public static VREnable.VRAvatarDisplay DISPLAY_VR_PERIPHERALS = VREnable.VRAvatarDisplay.All;

	// Token: 0x04001AFA RID: 6906
	private float nextBindingCheck;

	// Token: 0x04001AFB RID: 6907
	private float bindingDelay = 5f;

	// Token: 0x020007A6 RID: 1958
	public enum VRAvatarDisplay
	{
		// Token: 0x04002CE3 RID: 11491
		None,
		// Token: 0x04002CE4 RID: 11492
		Hands,
		// Token: 0x04002CE5 RID: 11493
		All
	}
}
