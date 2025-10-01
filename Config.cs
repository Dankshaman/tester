using System;
using UnityEngine;

// Token: 0x020000BF RID: 191
public abstract class Config<SettingsClass, SingletonClass> : Singleton<!1> where SingletonClass : MonoBehaviour
{
	// Token: 0x170001B8 RID: 440
	// (get) Token: 0x060009C3 RID: 2499 RVA: 0x0004571E File Offset: 0x0004391E
	// (set) Token: 0x060009C4 RID: 2500 RVA: 0x00045726 File Offset: 0x00043926
	public SettingsClass settings
	{
		get
		{
			return this._settings;
		}
		set
		{
			this._settings = value;
			this.TriggerSettingChanged();
		}
	}

	// Token: 0x060009C5 RID: 2501 RVA: 0x00045735 File Offset: 0x00043935
	public void TriggerSettingChanged()
	{
		PlayerPrefs.SetString(this.GetPlayerPrefsName(), Json.GetJson(this.settings, false));
		this.OnSettingsChanged();
	}

	// Token: 0x060009C6 RID: 2502 RVA: 0x000025B8 File Offset: 0x000007B8
	protected virtual void OnSettingsChanged()
	{
	}

	// Token: 0x060009C7 RID: 2503 RVA: 0x00045759 File Offset: 0x00043959
	private string GetPlayerPrefsName()
	{
		return base.GetType().ToString();
	}

	// Token: 0x060009C8 RID: 2504 RVA: 0x00045768 File Offset: 0x00043968
	protected override void Awake()
	{
		base.Awake();
		if (PlayerPrefs.HasKey(this.GetPlayerPrefsName()))
		{
			try
			{
				this._settings = Json.Load<SettingsClass>(PlayerPrefs.GetString(this.GetPlayerPrefsName()));
			}
			catch (Exception)
			{
				Chat.LogError("Failed to load settings for " + this.GetPlayerPrefsName() + ".", true);
			}
		}
	}

	// Token: 0x060009C9 RID: 2505 RVA: 0x000457D0 File Offset: 0x000439D0
	protected virtual void Start()
	{
		this.OnSettingsChanged();
	}

	// Token: 0x04000704 RID: 1796
	public SettingsClass _settings;
}
