using System;
using System.Collections;
using UnityEngine;

// Token: 0x0200027F RID: 639
public class UIBlindfold : MonoBehaviour
{
	// Token: 0x0600213E RID: 8510 RVA: 0x000F026C File Offset: 0x000EE46C
	private IEnumerator Start()
	{
		yield return null;
		yield return null;
		this.tweenPosition = this.curtain.GetComponent<TweenPosition>();
		if (!VRHMD.isVR)
		{
			this.savedCullindMask = Camera.main.cullingMask;
		}
		else
		{
			this.savedCullindMask = Singleton<VRHMD>.Instance.VRCamera.cullingMask;
		}
		EventDelegate.Add(this.UnblindButton.onClick, new EventDelegate.Callback(this.UnblindButtonOnClick));
		EventManager.OnBlindfold += this.EventManager_OnBlindfold;
		yield break;
	}

	// Token: 0x0600213F RID: 8511 RVA: 0x000F027B File Offset: 0x000EE47B
	private void OnDestroy()
	{
		EventDelegate.Remove(this.UnblindButton.onClick, new EventDelegate.Callback(this.UnblindButtonOnClick));
		EventManager.OnBlindfold -= this.EventManager_OnBlindfold;
	}

	// Token: 0x06002140 RID: 8512 RVA: 0x000F02AB File Offset: 0x000EE4AB
	private void EventManager_OnBlindfold(bool bBlind, int id)
	{
		if (id != NetworkID.ID)
		{
			return;
		}
		if (bBlind)
		{
			this.Blind();
			return;
		}
		this.Unblind();
	}

	// Token: 0x06002141 RID: 8513 RVA: 0x000F02C6 File Offset: 0x000EE4C6
	private void UnblindButtonOnClick()
	{
		NetworkSingleton<PlayerManager>.Instance.ChangeBlindfold(NetworkID.ID, false);
	}

	// Token: 0x06002142 RID: 8514 RVA: 0x000F02D8 File Offset: 0x000EE4D8
	public void Blind()
	{
		Singleton<SpectatorCamera>.Instance.OverrideUICamera(true);
		this.curtain.GetComponent<UITexture>().color = NetworkSingleton<NetworkUI>.Instance.playerColour;
		this.playersGroup.SetActive(false);
		this.visual.SetActive(true);
		this.UnblindButton.gameObject.SetActive(true);
		this.bDisable = false;
		TweenPosition.Begin(this.curtain, 0f, new Vector3(0f, (float)Screen.height * 1.2f, 0f));
		TweenPosition.Begin(this.curtain, 0.4f, Vector3.zero);
		this.tweenPosition.AddOnFinished(new EventDelegate.Callback(this.TweenFinish));
	}

	// Token: 0x06002143 RID: 8515 RVA: 0x000F0398 File Offset: 0x000EE598
	public void Unblind()
	{
		Singleton<SpectatorCamera>.Instance.OverrideUICamera(false);
		this.UnblindButton.gameObject.SetActive(false);
		this.curtain.GetComponent<UITexture>().color = NetworkSingleton<NetworkUI>.Instance.playerColour;
		this.bDisable = true;
		if (!VRHMD.isVR)
		{
			Camera.main.cullingMask = this.savedCullindMask;
		}
		else
		{
			Singleton<VRHMD>.Instance.VRCamera.cullingMask = this.savedCullindMask;
		}
		TweenPosition.Begin(this.curtain, 0.5f, new Vector3(0f, (float)Screen.height * 1.2f, 0f));
		this.tweenPosition.AddOnFinished(new EventDelegate.Callback(this.TweenFinish));
	}

	// Token: 0x06002144 RID: 8516 RVA: 0x000F0458 File Offset: 0x000EE658
	public void TweenFinish()
	{
		if (this.bDisable)
		{
			this.playersGroup.SetActive(true);
			this.visual.SetActive(false);
			return;
		}
		if (!VRHMD.isVR)
		{
			Camera.main.cullingMask = 0;
			return;
		}
		Singleton<VRHMD>.Instance.VRCamera.cullingMask = Layers.Mask(new int[]
		{
			2,
			18
		});
	}

	// Token: 0x04001496 RID: 5270
	public GameObject visual;

	// Token: 0x04001497 RID: 5271
	public GameObject playersGroup;

	// Token: 0x04001498 RID: 5272
	public GameObject curtain;

	// Token: 0x04001499 RID: 5273
	public UIButton UnblindButton;

	// Token: 0x0400149A RID: 5274
	private bool bDisable;

	// Token: 0x0400149B RID: 5275
	private int savedCullindMask;

	// Token: 0x0400149C RID: 5276
	private TweenPosition tweenPosition;
}
