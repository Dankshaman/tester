using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NewNet;
using UnityEngine;

// Token: 0x020001F1 RID: 497
public class SaveManager : NetworkSingleton<SaveManager>
{
	// Token: 0x17000411 RID: 1041
	// (get) Token: 0x06001A06 RID: 6662 RVA: 0x000B66FD File Offset: 0x000B48FD
	// (set) Token: 0x06001A07 RID: 6663 RVA: 0x000B6705 File Offset: 0x000B4905
	[Sync(Permission.Owner, null, SerializationMethod.Default, false)]
	public int RewindReverseIndex
	{
		get
		{
			return this._RewindReverseIndex;
		}
		set
		{
			if (value == this._RewindReverseIndex)
			{
				return;
			}
			this._RewindReverseIndex = value;
			base.DirtySync("RewindReverseIndex");
			Action onRewindSaveChange = this.OnRewindSaveChange;
			if (onRewindSaveChange == null)
			{
				return;
			}
			onRewindSaveChange();
		}
	}

	// Token: 0x14000057 RID: 87
	// (add) Token: 0x06001A08 RID: 6664 RVA: 0x000B6734 File Offset: 0x000B4934
	// (remove) Token: 0x06001A09 RID: 6665 RVA: 0x000B676C File Offset: 0x000B496C
	public event Action OnRewindSaveChange;

	// Token: 0x17000412 RID: 1042
	// (get) Token: 0x06001A0A RID: 6666 RVA: 0x000B67A1 File Offset: 0x000B49A1
	// (set) Token: 0x06001A0B RID: 6667 RVA: 0x000B67A9 File Offset: 0x000B49A9
	public bool RewindSaveEnable
	{
		get
		{
			return this._RewindSaveEnable;
		}
		set
		{
			if (this._RewindSaveEnable != value)
			{
				this._RewindSaveEnable = value;
				Action onRewindSaveChange = this.OnRewindSaveChange;
				if (onRewindSaveChange == null)
				{
					return;
				}
				onRewindSaveChange();
			}
		}
	}

	// Token: 0x06001A0C RID: 6668 RVA: 0x000B67CB File Offset: 0x000B49CB
	private void Start()
	{
		this.ManagerInstance = NetworkSingleton<ManagerPhysicsObject>.Instance;
		NetworkEvents.OnServerInitialized += this.NetworkEventsOnServerInitialized;
		NetworkEvents.OnPlayerConnected += this.NetworkEventsOnPlayerConnected;
	}

	// Token: 0x06001A0D RID: 6669 RVA: 0x000B67FA File Offset: 0x000B49FA
	private void OnDestroy()
	{
		NetworkEvents.OnServerInitialized -= this.NetworkEventsOnServerInitialized;
		NetworkEvents.OnPlayerConnected -= this.NetworkEventsOnPlayerConnected;
	}

	// Token: 0x06001A0E RID: 6670 RVA: 0x000B681E File Offset: 0x000B4A1E
	private void NetworkEventsOnServerInitialized()
	{
		Wait.Time(new Action(this.CheckSave), 0.25f, -1);
	}

	// Token: 0x06001A0F RID: 6671 RVA: 0x000B6838 File Offset: 0x000B4A38
	private void NetworkEventsOnPlayerConnected(NetworkPlayer player)
	{
		if (Network.isServer)
		{
			for (int i = 0; i < this.rewindTimes.Count; i++)
			{
				base.networkView.RPC<uint>(player, new Action<uint>(this.AddRewindTime), this.rewindTimes[i]);
			}
		}
	}

	// Token: 0x06001A10 RID: 6672 RVA: 0x000B6888 File Offset: 0x000B4A88
	private void CheckSave()
	{
		if (this.ManagerInstance.TableScript && !this.ManagerInstance.TableScript.IsTableFlipping)
		{
			if (this.RewindSaveEnable && this.RewindSaveInterval > 0f && Time.time > this.rewindSaveTime + this.RewindSaveInterval)
			{
				this.SaveRewindState();
			}
			if (this.AutoSaveInterval > 0f && this.AutoSaveCount > 0)
			{
				if (this.autoSaveTime == 0f & this.ManagerInstance.GrabbableNPOs.Count > 0)
				{
					this.autoSaveTime = Time.time;
					return;
				}
				if (this.autoSaveTime != 0f && Time.time > this.autoSaveTime + this.AutoSaveInterval)
				{
					this.autoSaveTime = Time.time;
					SerializationScript.Save(this.GetAutoSavePath(), "", this.AutoSaveLog);
				}
			}
		}
	}

	// Token: 0x06001A11 RID: 6673 RVA: 0x000B6978 File Offset: 0x000B4B78
	public async Task<bool> SaveRewindState()
	{
		bool result;
		if (this.savingRewindState)
		{
			result = false;
		}
		else
		{
			this.savingRewindState = true;
			SaveState currentState = this.ManagerInstance.CurrentState();
			this.rewindSaveTime = Time.time;
			bool flag = await Task.Run<bool>(delegate()
			{
				bool result2 = true;
				if (this.rewindStates.Count > 0)
				{
					SaveState saveState = this.rewindStates[this.rewindStates.Count - this.RewindReverseIndex - 1];
					uint? epochTime = currentState.EpochTime;
					uint? epochTime2 = saveState.EpochTime;
					string date = currentState.Date;
					string date2 = saveState.Date;
					currentState.EpochTime = null;
					saveState.EpochTime = null;
					currentState.Date = null;
					saveState.Date = null;
					result2 = (currentState != saveState);
					currentState.EpochTime = epochTime;
					saveState.EpochTime = epochTime2;
					currentState.Date = date;
					saveState.Date = date2;
				}
				return result2;
			});
			this.savingRewindState = false;
			if (flag)
			{
				this.RewindReverseIndex = 0;
				if (this.rewindStates.Count == 20)
				{
					this.rewindStates.RemoveAt(0);
				}
				this.rewindStates.Add(currentState);
				base.networkView.RPC<uint>(RPCTarget.All, new Action<uint>(this.AddRewindTime), currentState.EpochTime.Value);
			}
			result = flag;
		}
		return result;
	}

	// Token: 0x06001A12 RID: 6674 RVA: 0x000B69BD File Offset: 0x000B4BBD
	[Remote(Permission.Server)]
	private void AddRewindTime(uint time)
	{
		if (this.rewindTimes.Count == 20)
		{
			this.rewindTimes.RemoveAt(0);
		}
		this.rewindTimes.Add(time);
		Action onRewindSaveChange = this.OnRewindSaveChange;
		if (onRewindSaveChange == null)
		{
			return;
		}
		onRewindSaveChange();
	}

	// Token: 0x06001A13 RID: 6675 RVA: 0x000B69F6 File Offset: 0x000B4BF6
	public void LoadRewindState(int rewindReverseIndex)
	{
		base.networkView.RPC<uint>(RPCTarget.Server, new Action<uint>(this.LoadRewindStateFromTime), this.GetRewindTimeFromReverseIndex(rewindReverseIndex));
	}

	// Token: 0x06001A14 RID: 6676 RVA: 0x000B6A17 File Offset: 0x000B4C17
	public void LoadRewindState(uint time)
	{
		base.networkView.RPC<uint>(RPCTarget.Server, new Action<uint>(this.LoadRewindStateFromTime), time);
	}

	// Token: 0x06001A15 RID: 6677 RVA: 0x000B6A34 File Offset: 0x000B4C34
	[Remote(Permission.Admin, SendType.ReliableNoDelay, null, SerializationMethod.Default)]
	private void LoadRewindStateFromTime(uint time)
	{
		int num = this.rewindTimes.IndexOf(time);
		if (num == -1)
		{
			return;
		}
		int num2 = this.rewindStates.Count - num - 1;
		Chat.SendChat((num2 < this.RewindReverseIndex) ? "Fast-forwarding time..." : "Rewinding time...");
		this.RewindReverseIndex = num2;
		this.rewindSaveTime = Time.time;
		SaveState ss = this.rewindStates[this.rewindStates.Count - this.RewindReverseIndex - 1];
		this.ManagerInstance.LoadSaveState(ss, false, false);
	}

	// Token: 0x06001A16 RID: 6678 RVA: 0x000B6ABD File Offset: 0x000B4CBD
	public uint GetRewindTimeFromReverseIndex(int reverseIndex)
	{
		return this.rewindTimes[this.rewindTimes.Count - reverseIndex - 1];
	}

	// Token: 0x06001A17 RID: 6679 RVA: 0x000B6AD9 File Offset: 0x000B4CD9
	public int GetRewindSaveCount()
	{
		return this.rewindTimes.Count;
	}

	// Token: 0x06001A18 RID: 6680 RVA: 0x000B6AE8 File Offset: 0x000B4CE8
	private string GetAutoSavePath()
	{
		uint num = uint.MaxValue;
		string result = null;
		for (int i = 0; i < this.AutoSaveCount; i++)
		{
			string text;
			if (i == 0)
			{
				text = DirectoryScript.saveFilePath + "//TS_AutoSave.json";
			}
			else
			{
				text = DirectoryScript.saveFilePath + string.Format("//TS_AutoSave_{0}.json", i + 1);
			}
			uint fileTime = SerializationScript.GetFileTime(text);
			if (fileTime < num)
			{
				num = fileTime;
				result = text;
				if (fileTime == 0U)
				{
					break;
				}
			}
		}
		return result;
	}

	// Token: 0x0400100A RID: 4106
	private readonly List<SaveState> rewindStates = new List<SaveState>(20);

	// Token: 0x0400100B RID: 4107
	private readonly List<uint> rewindTimes = new List<uint>(20);

	// Token: 0x0400100C RID: 4108
	[NonSerialized]
	public float RewindSaveInterval = 10f;

	// Token: 0x0400100D RID: 4109
	private float rewindSaveTime;

	// Token: 0x0400100E RID: 4110
	private const int RewindMaxCount = 20;

	// Token: 0x0400100F RID: 4111
	[NonSerialized]
	public bool AutoSaveLog;

	// Token: 0x04001010 RID: 4112
	[NonSerialized]
	public float AutoSaveInterval = 300f;

	// Token: 0x04001011 RID: 4113
	[NonSerialized]
	public int AutoSaveCount = 3;

	// Token: 0x04001012 RID: 4114
	private float autoSaveTime;

	// Token: 0x04001013 RID: 4115
	private int _RewindReverseIndex;

	// Token: 0x04001014 RID: 4116
	private ManagerPhysicsObject ManagerInstance;

	// Token: 0x04001016 RID: 4118
	private bool _RewindSaveEnable = true;

	// Token: 0x04001017 RID: 4119
	private bool savingRewindState;
}
