using System;
using NewNet;
using UnityEngine;

// Token: 0x020000C5 RID: 197
public class CounterScript : NetworkBehavior
{
	// Token: 0x060009E9 RID: 2537 RVA: 0x00045D7F File Offset: 0x00043F7F
	private void Awake()
	{
		NetworkEvents.OnPlayerConnected += this.OnPlayerConnect;
	}

	// Token: 0x060009EA RID: 2538 RVA: 0x00045D92 File Offset: 0x00043F92
	private void OnDestroy()
	{
		NetworkEvents.OnPlayerConnected -= this.OnPlayerConnect;
	}

	// Token: 0x060009EB RID: 2539 RVA: 0x00045DA5 File Offset: 0x00043FA5
	private void Start()
	{
		if (Network.isServer)
		{
			this.RPCSyncValue();
		}
		this.uiInputCollider = this.uiinput.GetComponent<Collider>();
	}

	// Token: 0x060009EC RID: 2540 RVA: 0x00045DC8 File Offset: 0x00043FC8
	private void Update()
	{
		if (!this.uiInputCollider)
		{
			return;
		}
		bool flag = false;
		this.uiInputCollider.enabled = (NetworkSingleton<PlayerManager>.Instance.IsAdmin(-1) || PermissionsOptions.options.Digital);
		if (UICamera.IsPressed(this.IncrementObject))
		{
			flag = true;
			float num = this.PressedSpeed / Mathf.Sqrt((float)this.PressedCount);
			num = Mathf.Max(0.1f, num);
			if (Time.time > this.PressedTimeHolder + num)
			{
				this.PressedTimeHolder = Time.time;
				this.IncrementPressed();
				this.PressedCount++;
				this.bHoldPress = true;
			}
		}
		if (UICamera.IsPressed(this.DecrementObject))
		{
			flag = true;
			float num2 = this.PressedSpeed / Mathf.Sqrt((float)this.PressedCount);
			num2 = Mathf.Max(0.1f, num2);
			if (Time.time > this.PressedTimeHolder + num2)
			{
				this.PressedTimeHolder = Time.time;
				this.DecrementPressed();
				this.PressedCount++;
				this.bHoldPress = true;
			}
		}
		if (!flag)
		{
			this.PressedTimeHolder = Time.time;
			this.PressedCount = 1;
		}
	}

	// Token: 0x060009ED RID: 2541 RVA: 0x00045EE8 File Offset: 0x000440E8
	private void ButtonPressed()
	{
		if (UICamera.IsPressed(this.IncrementObject))
		{
			this.pressed = true;
			this.IncrementPressed();
			return;
		}
		if (UICamera.IsPressed(this.DecrementObject))
		{
			this.pressed = true;
			this.DecrementPressed();
			return;
		}
		base.CancelInvoke("ButtonPressed");
	}

	// Token: 0x060009EE RID: 2542 RVA: 0x00045F36 File Offset: 0x00044136
	public void OnPlayerConnect(NetworkPlayer player)
	{
		if (Network.isServer)
		{
			base.networkView.RPC<int>(player, new Action<int>(this.SetCounterValue), this.GetValue());
		}
	}

	// Token: 0x060009EF RID: 2543 RVA: 0x00045F60 File Offset: 0x00044160
	public void Increment()
	{
		if (!PermissionsOptions.CheckAllow(PermissionsOptions.options.Digital, -1))
		{
			PermissionsOptions.BroadcastPermissionWarning("interact with the Counter (Digital)");
			return;
		}
		if (!PlayerScript.Pointer)
		{
			return;
		}
		if (this.bHoldPress)
		{
			this.bHoldPress = false;
			return;
		}
		if (this.pressed)
		{
			this.pressed = false;
			return;
		}
		int num = this.GetValue();
		num++;
		if (num > 9999)
		{
			num = 9999;
		}
		this.uiinput.value = num.ToString();
		base.networkView.RPC<int>(RPCTarget.Others, new Action<int>(this.SetCounterValue), num);
	}

	// Token: 0x060009F0 RID: 2544 RVA: 0x00045FFC File Offset: 0x000441FC
	private void IncrementPressed()
	{
		if (!PermissionsOptions.CheckAllow(PermissionsOptions.options.Digital, -1))
		{
			PermissionsOptions.BroadcastPermissionWarning("interact with the Counter (Digital)");
			return;
		}
		if (!PlayerScript.Pointer)
		{
			return;
		}
		int num = this.GetValue();
		num++;
		this.uiinput.value = num.ToString();
		base.networkView.RPC<int>(RPCTarget.Others, new Action<int>(this.SetCounterValue), num);
	}

	// Token: 0x060009F1 RID: 2545 RVA: 0x0004606C File Offset: 0x0004426C
	public void Decrement()
	{
		if (!PermissionsOptions.CheckAllow(PermissionsOptions.options.Digital, -1))
		{
			PermissionsOptions.BroadcastPermissionWarning("interact with the Counter (Digital)");
			return;
		}
		if (!PlayerScript.Pointer)
		{
			return;
		}
		if (this.bHoldPress)
		{
			this.bHoldPress = false;
			return;
		}
		if (this.pressed)
		{
			this.pressed = false;
			return;
		}
		int num = this.GetValue();
		num--;
		if (num < -999)
		{
			num = -999;
		}
		this.uiinput.value = num.ToString();
		base.networkView.RPC<int>(RPCTarget.Others, new Action<int>(this.SetCounterValue), num);
	}

	// Token: 0x060009F2 RID: 2546 RVA: 0x00046108 File Offset: 0x00044308
	private void DecrementPressed()
	{
		if (!PermissionsOptions.CheckAllow(PermissionsOptions.options.Digital, -1))
		{
			PermissionsOptions.BroadcastPermissionWarning("interact with the Counter (Digital)");
			return;
		}
		if (!PlayerScript.Pointer)
		{
			return;
		}
		int num = this.GetValue();
		num--;
		this.uiinput.value = num.ToString();
		base.networkView.RPC<int>(RPCTarget.Others, new Action<int>(this.SetCounterValue), num);
	}

	// Token: 0x060009F3 RID: 2547 RVA: 0x00046178 File Offset: 0x00044378
	public void Clear()
	{
		if (!PermissionsOptions.CheckAllow(PermissionsOptions.options.Digital, -1))
		{
			PermissionsOptions.BroadcastPermissionWarning("interact with the Counter (Digital)");
			return;
		}
		if (!PlayerScript.Pointer)
		{
			return;
		}
		this.uiinput.value = "0";
		base.networkView.RPC<int>(RPCTarget.Others, new Action<int>(this.SetCounterValue), 0);
	}

	// Token: 0x060009F4 RID: 2548 RVA: 0x000461D8 File Offset: 0x000443D8
	public int GetValue()
	{
		int result;
		int.TryParse(this.uiinput.value, out result);
		return result;
	}

	// Token: 0x060009F5 RID: 2549 RVA: 0x000461F9 File Offset: 0x000443F9
	public void SyncValue()
	{
		if (!PermissionsOptions.CheckAllow(PermissionsOptions.options.Digital, -1))
		{
			PermissionsOptions.BroadcastPermissionWarning("interact with the Counter (Digital)");
			return;
		}
		if (!PlayerScript.Pointer)
		{
			return;
		}
		this.RPCSyncValue();
	}

	// Token: 0x060009F6 RID: 2550 RVA: 0x0004622B File Offset: 0x0004442B
	private void RPCSyncValue()
	{
		base.networkView.RPC<int>(RPCTarget.Others, new Action<int>(this.SetCounterValue), this.GetValue());
	}

	// Token: 0x060009F7 RID: 2551 RVA: 0x0004624B File Offset: 0x0004444B
	[Remote(Permission.Client, SendType.ReliableBuffered, null, SerializationMethod.Default, permission = Permission.Client, validationFunction = "Permissions/Digital")]
	public void SetCounterValue(int value)
	{
		this.uiinput.value = value.ToString();
	}

	// Token: 0x0400070F RID: 1807
	public UIInput uiinput;

	// Token: 0x04000710 RID: 1808
	public GameObject DecrementObject;

	// Token: 0x04000711 RID: 1809
	public GameObject IncrementObject;

	// Token: 0x04000712 RID: 1810
	private bool pressed;

	// Token: 0x04000713 RID: 1811
	public Collider uiInputCollider;

	// Token: 0x04000714 RID: 1812
	private int PressedCount;

	// Token: 0x04000715 RID: 1813
	private float PressedTimeHolder;

	// Token: 0x04000716 RID: 1814
	private float PressedSpeed = 0.5f;

	// Token: 0x04000717 RID: 1815
	private bool bHoldPress;
}
