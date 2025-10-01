using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using NewNet;
using UnityEngine;

// Token: 0x02000117 RID: 279
public class GameOptions : NetworkSingleton<GameOptions>, INotifyPropertyChanged
{
	// Token: 0x06000EE4 RID: 3812 RVA: 0x00065E32 File Offset: 0x00064032
	public void Start()
	{
		Utilities.InvertNormals(this.BoundsVisual);
	}

	// Token: 0x170002BC RID: 700
	// (get) Token: 0x06000EE5 RID: 3813 RVA: 0x00065E3F File Offset: 0x0006403F
	// (set) Token: 0x06000EE6 RID: 3814 RVA: 0x00065E48 File Offset: 0x00064048
	[Sync(Permission.Admin, null, SerializationMethod.Default, false)]
	public float Gravity
	{
		get
		{
			return this._Gravity;
		}
		set
		{
			if (this._Gravity == value)
			{
				return;
			}
			this._Gravity = value;
			if (this._Gravity == 0f)
			{
				Achievements.Set("ACH_PLAY_GAME_ZERO_GRAVITY");
			}
			Physics.gravity = new Vector3(0f, NetworkUI.DefaultGravity.y * this._Gravity * 2f, 0f);
			NetworkSingleton<GameOptions>.Instance.DirtySync("Gravity");
		}
	}

	// Token: 0x170002BD RID: 701
	// (get) Token: 0x06000EE7 RID: 3815 RVA: 0x00065EB7 File Offset: 0x000640B7
	// (set) Token: 0x06000EE8 RID: 3816 RVA: 0x00065EC0 File Offset: 0x000640C0
	[Sync(Permission.Admin, null, SerializationMethod.Default, false)]
	public float PlayArea
	{
		get
		{
			return this._PlayArea;
		}
		set
		{
			if (this._PlayArea == value)
			{
				return;
			}
			this._PlayArea = Mathf.Max(0f, value);
			float num = 1.4285f * (this._PlayArea + 0.2f);
			this.BoundsObject.transform.localScale = Vector3.one * num;
			Singleton<CameraController>.Instance.distanceMax = 140f * num;
			Singleton<CameraController>.Instance.distance = Singleton<CameraController>.Instance.distance;
			NetworkSingleton<GameOptions>.Instance.DirtySync("PlayArea");
		}
	}

	// Token: 0x170002BE RID: 702
	// (get) Token: 0x06000EE9 RID: 3817 RVA: 0x00065F4A File Offset: 0x0006414A
	// (set) Token: 0x06000EEA RID: 3818 RVA: 0x00065F51 File Offset: 0x00064151
	[Sync(Permission.Admin, null, SerializationMethod.Default, false)]
	public string GameName
	{
		get
		{
			return Network.gameName;
		}
		set
		{
			if (Network.gameName == value)
			{
				return;
			}
			Network.gameName = value;
			this.OnPropertyChanged("GameName");
			NetworkSingleton<GameOptions>.Instance.DirtySync("GameName");
		}
	}

	// Token: 0x170002BF RID: 703
	// (get) Token: 0x06000EEB RID: 3819 RVA: 0x00065F81 File Offset: 0x00064181
	// (set) Token: 0x06000EEC RID: 3820 RVA: 0x00065F89 File Offset: 0x00064189
	[Sync(Permission.Admin, null, SerializationMethod.Default, false)]
	public int[] PlayingTime
	{
		get
		{
			return this._playingTime;
		}
		set
		{
			if (value != null && this._playingTime != null && this._playingTime.SequenceEqual(value))
			{
				return;
			}
			this._playingTime = value;
			this.OnPropertyChanged("PlayingTime");
			NetworkSingleton<GameOptions>.Instance.DirtySync("PlayingTime");
		}
	}

	// Token: 0x170002C0 RID: 704
	// (get) Token: 0x06000EED RID: 3821 RVA: 0x00065FC6 File Offset: 0x000641C6
	// (set) Token: 0x06000EEE RID: 3822 RVA: 0x00065FCE File Offset: 0x000641CE
	[Sync(Permission.Admin, null, SerializationMethod.Default, false)]
	public string GameType
	{
		get
		{
			return this._gameType;
		}
		set
		{
			if (value != null && this._gameType != null && this._gameType == value)
			{
				return;
			}
			this._gameType = value;
			this.OnPropertyChanged("GameType");
			NetworkSingleton<GameOptions>.Instance.DirtySync("GameType");
		}
	}

	// Token: 0x170002C1 RID: 705
	// (get) Token: 0x06000EEF RID: 3823 RVA: 0x0006600B File Offset: 0x0006420B
	// (set) Token: 0x06000EF0 RID: 3824 RVA: 0x00066013 File Offset: 0x00064213
	[Sync(Permission.Admin, null, SerializationMethod.Default, false)]
	public string GameComplexity
	{
		get
		{
			return this._gameComplexity;
		}
		set
		{
			if (value != null && this._gameComplexity != null && this._gameComplexity == value)
			{
				return;
			}
			this._gameComplexity = value;
			this.OnPropertyChanged("GameComplexity");
			NetworkSingleton<GameOptions>.Instance.DirtySync("GameComplexity");
		}
	}

	// Token: 0x170002C2 RID: 706
	// (get) Token: 0x06000EF1 RID: 3825 RVA: 0x00066050 File Offset: 0x00064250
	// (set) Token: 0x06000EF2 RID: 3826 RVA: 0x00066058 File Offset: 0x00064258
	[Sync(Permission.Admin, null, SerializationMethod.Default, false)]
	public int[] PlayerCounts
	{
		get
		{
			return this._playerCounts;
		}
		set
		{
			if (value != null && this._playerCounts != null && this._playerCounts.SequenceEqual(value))
			{
				return;
			}
			this._playerCounts = value;
			this.OnPropertyChanged("PlayerCounts");
			NetworkSingleton<GameOptions>.Instance.DirtySync("PlayerCounts");
		}
	}

	// Token: 0x170002C3 RID: 707
	// (get) Token: 0x06000EF3 RID: 3827 RVA: 0x00066095 File Offset: 0x00064295
	// (set) Token: 0x06000EF4 RID: 3828 RVA: 0x0006609D File Offset: 0x0006429D
	[Sync(Permission.Admin, null, SerializationMethod.Default, false)]
	public List<string> Tags
	{
		get
		{
			return this._gameTypes;
		}
		set
		{
			if (this._gameTypes.SequenceEqual(value))
			{
				return;
			}
			this._gameTypes = value;
			this.OnPropertyChanged("Tags");
			NetworkSingleton<GameOptions>.Instance.DirtySync("Tags");
		}
	}

	// Token: 0x06000EF5 RID: 3829 RVA: 0x000660CF File Offset: 0x000642CF
	protected override void Awake()
	{
		base.Awake();
		this.Reset();
		NetworkEvents.OnSettingsChange += this.NetworkEvents_OnSettingsChange;
	}

	// Token: 0x06000EF6 RID: 3830 RVA: 0x000660EE File Offset: 0x000642EE
	private void OnDestroy()
	{
		NetworkEvents.OnSettingsChange -= this.NetworkEvents_OnSettingsChange;
	}

	// Token: 0x06000EF7 RID: 3831 RVA: 0x00066101 File Offset: 0x00064301
	private void NetworkEvents_OnSettingsChange()
	{
		NetworkSingleton<GameOptions>.Instance.DirtySync(null);
	}

	// Token: 0x06000EF8 RID: 3832 RVA: 0x0006610E File Offset: 0x0006430E
	public void Reset()
	{
		this.Gravity = 0.5f;
		this.PlayArea = 0.5f;
		this.GameName = Language.Translate("None");
	}

	// Token: 0x14000054 RID: 84
	// (add) Token: 0x06000EF9 RID: 3833 RVA: 0x00066138 File Offset: 0x00064338
	// (remove) Token: 0x06000EFA RID: 3834 RVA: 0x00066170 File Offset: 0x00064370
	public event PropertyChangedEventHandler PropertyChanged;

	// Token: 0x06000EFB RID: 3835 RVA: 0x000661A5 File Offset: 0x000643A5
	protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
	{
		PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
		if (propertyChanged == null)
		{
			return;
		}
		propertyChanged(this, new PropertyChangedEventArgs(propertyName));
	}

	// Token: 0x04000946 RID: 2374
	public GameObject BoundsObject;

	// Token: 0x04000947 RID: 2375
	public GameObject BoundsVisual;

	// Token: 0x04000948 RID: 2376
	private float _Gravity = 0.5f;

	// Token: 0x04000949 RID: 2377
	private float _PlayArea = 0.5f;

	// Token: 0x0400094A RID: 2378
	private int[] _playingTime;

	// Token: 0x0400094B RID: 2379
	private string _gameType;

	// Token: 0x0400094C RID: 2380
	private string _gameComplexity;

	// Token: 0x0400094D RID: 2381
	private int[] _playerCounts;

	// Token: 0x0400094E RID: 2382
	private List<string> _gameTypes = new List<string>();
}
