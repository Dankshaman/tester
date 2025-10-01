using System;
using System.Collections.Generic;
using Discord;
using UnityEngine;

// Token: 0x02000350 RID: 848
public class UIToast : Singleton<UIToast>
{
	// Token: 0x170004BF RID: 1215
	// (get) Token: 0x0600282D RID: 10285 RVA: 0x0011BDF7 File Offset: 0x00119FF7
	private Vector3 homePosition
	{
		get
		{
			return this.Anchor.transform.position;
		}
	}

	// Token: 0x170004C0 RID: 1216
	// (get) Token: 0x0600282E RID: 10286 RVA: 0x0011BE09 File Offset: 0x0011A009
	private Vector3 belowScreenPosition
	{
		get
		{
			return new Vector3(this.homePosition.x, this.homePosition.y - 0.24f, this.homePosition.z);
		}
	}

	// Token: 0x170004C1 RID: 1217
	// (get) Token: 0x0600282F RID: 10287 RVA: 0x0011BE37 File Offset: 0x0011A037
	private Vector3 rightOfScreenPosition
	{
		get
		{
			return new Vector3(this.homePosition.x + 1.1f, this.homePosition.y, this.homePosition.z);
		}
	}

	// Token: 0x06002830 RID: 10288 RVA: 0x0011BE68 File Offset: 0x0011A068
	protected override void Awake()
	{
		base.Awake();
		EventDelegate.Add(this.Yes.onClick, new EventDelegate.Callback(this.onYesClick));
		EventDelegate.Add(this.No.onClick, new EventDelegate.Callback(this.onNoClick));
		this.Background.transform.position = this.belowScreenPosition;
	}

	// Token: 0x06002831 RID: 10289 RVA: 0x0011BECB File Offset: 0x0011A0CB
	public void OnDestroy()
	{
		EventDelegate.Remove(this.Yes.onClick, new EventDelegate.Callback(this.onYesClick));
		EventDelegate.Remove(this.No.onClick, new EventDelegate.Callback(this.onNoClick));
	}

	// Token: 0x06002832 RID: 10290 RVA: 0x0011BF08 File Offset: 0x0011A108
	private void Update()
	{
		if (this.currentRequest != null)
		{
			this.Background.gameObject.SetActive(true);
			float num = (Time.time - this.stateStartedAt) / this.stateDuration;
			if (num >= 1f)
			{
				num = 0f;
				switch (this.state)
				{
				case ToastState.HOMING:
					this.setState(ToastState.RESTING, this.homePosition, this.homePosition, UIToast.DiscordRequestDuration, UIToast.RESTING_CURVE);
					break;
				case ToastState.RESTING:
					this.setState(ToastState.LEAVING, this.homePosition, this.rightOfScreenPosition, 0.4f, UIToast.LEAVING_CURVE);
					this.enableButtons(false);
					break;
				case ToastState.LEAVING:
					if (this.currentRequest != null && !this.acceptedCurrentRequest)
					{
						Singleton<DiscordController>.Instance.RejectJoinRequest(this.currentRequest.Value);
					}
					this.currentRequest = null;
					break;
				}
			}
			if (this.currentRequest != null && this.stateCurve != null)
			{
				this.Background.transform.position = Vector3.Lerp(this.fromPosition, this.toPosition, this.stateCurve(num));
			}
		}
		else
		{
			this.Background.gameObject.SetActive(false);
		}
		if (this.currentRequest == null && this.requestQueue.Count > 0)
		{
			this.currentRequest = new User?(this.requestQueue.Dequeue());
			User value = this.currentRequest.Value;
			this.Text.text = value.Username;
			this.acceptedCurrentRequest = false;
			NetworkSingleton<NetworkUI>.Instance.GetComponent<SoundScript>().PlayGUISound(this.AlertSound, 0.5f, 1.4f);
			this.enableButtons(true);
			this.setState(ToastState.HOMING, this.belowScreenPosition, this.homePosition, 0.4f, UIToast.HOMING_CURVE);
		}
	}

	// Token: 0x06002833 RID: 10291 RVA: 0x0011C0E1 File Offset: 0x0011A2E1
	public void AddJoinRequest(User request)
	{
		this.requestQueue.Enqueue(request);
	}

	// Token: 0x06002834 RID: 10292 RVA: 0x0011C0F0 File Offset: 0x0011A2F0
	private void setState(ToastState newState, Vector3 startPosition, Vector3 endPosition, float duration, CurveDelegate curve)
	{
		Transform transform = this.Background.transform;
		this.fromPosition = startPosition;
		transform.position = startPosition;
		this.toPosition = endPosition;
		this.state = newState;
		this.stateStartedAt = Time.time;
		this.stateDuration = duration;
		this.stateCurve = curve;
	}

	// Token: 0x06002835 RID: 10293 RVA: 0x0011C140 File Offset: 0x0011A340
	private void enableButtons(bool enable)
	{
		this.Yes.enabled = enable;
		this.No.enabled = enable;
	}

	// Token: 0x06002836 RID: 10294 RVA: 0x0011C15A File Offset: 0x0011A35A
	private void onYesClick()
	{
		this.nextState();
		if (this.currentRequest != null)
		{
			Singleton<DiscordController>.Instance.AcceptJoinRequest(this.currentRequest.Value);
			this.acceptedCurrentRequest = true;
		}
	}

	// Token: 0x06002837 RID: 10295 RVA: 0x0011C18B File Offset: 0x0011A38B
	private void onNoClick()
	{
		this.nextState();
	}

	// Token: 0x06002838 RID: 10296 RVA: 0x0011C193 File Offset: 0x0011A393
	private void nextState()
	{
		this.stateStartedAt = -UIToast.DiscordRequestDuration;
	}

	// Token: 0x04001A6A RID: 6762
	private const float BELOW_SCREEN_Y_DELTA = 0.24f;

	// Token: 0x04001A6B RID: 6763
	private const float RIGHT_OF_SCREEN_X_DELTA = 1.1f;

	// Token: 0x04001A6C RID: 6764
	private const float HOMING_DURATION = 0.4f;

	// Token: 0x04001A6D RID: 6765
	private const float LEAVING_DURATION = 0.4f;

	// Token: 0x04001A6E RID: 6766
	private static readonly CurveDelegate HOMING_CURVE = new CurveDelegate(Curves.SquareComplement);

	// Token: 0x04001A6F RID: 6767
	private static readonly CurveDelegate RESTING_CURVE = null;

	// Token: 0x04001A70 RID: 6768
	private static readonly CurveDelegate LEAVING_CURVE = new CurveDelegate(Curves.Square);

	// Token: 0x04001A71 RID: 6769
	public static float DiscordRequestDuration = 15f;

	// Token: 0x04001A72 RID: 6770
	public UISprite Icon;

	// Token: 0x04001A73 RID: 6771
	public UILabel Text;

	// Token: 0x04001A74 RID: 6772
	public UIButton Yes;

	// Token: 0x04001A75 RID: 6773
	public UIButton No;

	// Token: 0x04001A76 RID: 6774
	public AudioClip AlertSound;

	// Token: 0x04001A77 RID: 6775
	public GameObject Anchor;

	// Token: 0x04001A78 RID: 6776
	public GameObject Background;

	// Token: 0x04001A79 RID: 6777
	private Vector3 fromPosition;

	// Token: 0x04001A7A RID: 6778
	private Vector3 toPosition;

	// Token: 0x04001A7B RID: 6779
	private ToastState state;

	// Token: 0x04001A7C RID: 6780
	private float stateStartedAt;

	// Token: 0x04001A7D RID: 6781
	private float stateDuration;

	// Token: 0x04001A7E RID: 6782
	private CurveDelegate stateCurve;

	// Token: 0x04001A7F RID: 6783
	private Queue<User> requestQueue = new Queue<User>();

	// Token: 0x04001A80 RID: 6784
	private User? currentRequest;

	// Token: 0x04001A81 RID: 6785
	private bool acceptedCurrentRequest;
}
