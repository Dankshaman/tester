using System;
using UnityEngine;

// Token: 0x020001DA RID: 474
public class PlayerPortrait : MonoBehaviour
{
	// Token: 0x060018EA RID: 6378 RVA: 0x000A7C1C File Offset: 0x000A5E1C
	private void Start()
	{
		if (VRHMD.isVR)
		{
			this.CameraTransform = Singleton<VRHMD>.Instance.transform;
		}
		else
		{
			this.CameraTransform = HoverScript.MainCamera.transform;
		}
		this.PotraitTransform = base.transform;
		this.renderer = base.GetComponent<Renderer>();
		EventManager.OnPlayersUpdate += this.UpdatePlayer;
		EventManager.OnHandZoneChange += this.OnHandZoneChange;
		this.UpdatePlayer();
	}

	// Token: 0x060018EB RID: 6379 RVA: 0x000A7C92 File Offset: 0x000A5E92
	private void OnDestroy()
	{
		EventManager.OnPlayersUpdate -= this.UpdatePlayer;
		EventManager.OnHandZoneChange -= this.OnHandZoneChange;
	}

	// Token: 0x060018EC RID: 6380 RVA: 0x000A7CB6 File Offset: 0x000A5EB6
	private void OnHandZoneChange(HandZone handzone)
	{
		if (this.stringColor == handzone.TriggerLabel)
		{
			this.TriggerTransform = null;
		}
	}

	// Token: 0x060018ED RID: 6381 RVA: 0x000A7CD4 File Offset: 0x000A5ED4
	private void UpdatePlayer()
	{
		PlayerState playerState = NetworkSingleton<PlayerManager>.Instance.PlayerStateFromID(this.id);
		if (playerState != null)
		{
			this.stringColor = playerState.stringColor;
			this.TriggerTransform = null;
			return;
		}
		UnityEngine.Object.Destroy(base.gameObject);
	}

	// Token: 0x060018EE RID: 6382 RVA: 0x000A7D14 File Offset: 0x000A5F14
	private void Update()
	{
		if (!this.TriggerTransform)
		{
			GameObject hand = HandZone.GetHand(this.stringColor, 0);
			if (hand)
			{
				this.TriggerTransform = hand.transform;
				base.transform.position = hand.transform.position;
				base.transform.eulerAngles = new Vector3(base.transform.eulerAngles.x, hand.transform.eulerAngles.y, base.transform.eulerAngles.z);
				base.transform.Translate(new Vector3(0f, -2f, -5f));
			}
		}
		this.renderer.enabled = (!VRHMD.isVR && this.TriggerTransform && !NetworkSingleton<NetworkUI>.Instance.bHotseat && Singleton<ConfigGame>.Instance.settings.Portraits && Quaternion.Angle(Quaternion.Euler(new Vector3(0f, this.PotraitTransform.eulerAngles.y, 0f)), Quaternion.Euler(new Vector3(0f, this.CameraTransform.eulerAngles.y, 0f))) > 45f);
	}

	// Token: 0x04000EBE RID: 3774
	public int id;

	// Token: 0x04000EBF RID: 3775
	public string stringColor;

	// Token: 0x04000EC0 RID: 3776
	private Transform CameraTransform;

	// Token: 0x04000EC1 RID: 3777
	private Transform PotraitTransform;

	// Token: 0x04000EC2 RID: 3778
	private Transform TriggerTransform;

	// Token: 0x04000EC3 RID: 3779
	private Renderer renderer;
}
