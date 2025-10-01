using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using Assets.Scripts;
using Assets.Scripts.UI;
using Valve.Newtonsoft.Json.Utilities;

// Token: 0x020002EE RID: 750
public class UIInfoOptions : UIReactiveMenu
{
	// Token: 0x0600246B RID: 9323 RVA: 0x00100EB0 File Offset: 0x000FF0B0
	protected override void Awake()
	{
		if (this._isAwake)
		{
			return;
		}
		this._isAwake = true;
		base.Awake();
		this.ReactiveInputs.Add(this.GameNameInput.gameObject);
		this.PlayingTime.PropertyChanged += delegate(object _, PropertyChangedEventArgs __)
		{
			this.OnChanged();
		};
		this.PlayerCount.PropertyChanged += delegate(object _, PropertyChangedEventArgs __)
		{
			this.OnChanged();
		};
		this.AppliedTags.CollectionChanged += delegate(object _, NotifyCollectionChangedEventArgs __)
		{
			this.OnChanged();
		};
		NetworkSingleton<GameOptions>.Instance.PropertyChanged += this.OnPropertyChangedGameOptions;
		this.PlayingTime.sliderMin.GetComponentInChildren<UISliderRange>().PropertyChanged += this.OnPropertyChangedSlider;
		this.PlayingTime.sliderMax.GetComponentInChildren<UISliderRange>().PropertyChanged += this.OnPropertyChangedSlider;
		this.UITagMenu.ActiveTags.CollectionChanged += this.ActiveTagsOnCollectionChanged;
		this.GameType.items.Clear();
		this.GameType.items.Add("None");
		this.GameType.items.AddRange(Tags.Type);
		this.GameComplexity.items.Clear();
		this.GameComplexity.items.Add("None");
		this.GameComplexity.items.AddRange(Tags.Complexity);
	}

	// Token: 0x0600246C RID: 9324 RVA: 0x00101018 File Offset: 0x000FF218
	private void ActiveTagsOnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
	{
		NotifyCollectionChangedAction action = e.Action;
		if (action != NotifyCollectionChangedAction.Add)
		{
			if (action != NotifyCollectionChangedAction.Remove)
			{
				return;
			}
		}
		else
		{
			using (IEnumerator<string> enumerator = (from string x in e.NewItems
			where !this.AppliedTags.Contains(x)
			select x).GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					string item = enumerator.Current;
					this.AppliedTags.Add(item);
				}
				return;
			}
		}
		foreach (string item2 in e.OldItems.Cast<string>())
		{
			this.AppliedTags.Remove(item2);
		}
	}

	// Token: 0x0600246D RID: 9325 RVA: 0x001010D8 File Offset: 0x000FF2D8
	public void OnGameTypeChanged()
	{
		if (this.GameTypeLabel.text == "None")
		{
			this.GameTypeLabel.text = string.Empty;
		}
		this.OnChanged();
	}

	// Token: 0x0600246E RID: 9326 RVA: 0x00101107 File Offset: 0x000FF307
	public void OnGameComplexityChanged()
	{
		if (this.GameComplexityLabel.text == "None")
		{
			this.GameComplexityLabel.text = string.Empty;
		}
		this.OnChanged();
	}

	// Token: 0x0600246F RID: 9327 RVA: 0x00101138 File Offset: 0x000FF338
	private void OnPropertyChangedSlider(object sender, PropertyChangedEventArgs e)
	{
		UISliderRange uisliderRange;
		if ((uisliderRange = (sender as UISliderRange)) == null)
		{
			return;
		}
		if (uisliderRange.stringValue == "0")
		{
			uisliderRange.stringValue = "10";
		}
	}

	// Token: 0x06002470 RID: 9328 RVA: 0x0010116D File Offset: 0x000FF36D
	private void OnChanged()
	{
		if (this.BlockUpdateSource)
		{
			return;
		}
		this.UpdateSource();
	}

	// Token: 0x06002471 RID: 9329 RVA: 0x00101180 File Offset: 0x000FF380
	private void OnPropertyChangedGameOptions(object sender, PropertyChangedEventArgs e)
	{
		if (this.BlockUpdateSource)
		{
			return;
		}
		this.BlockUpdateSource = true;
		string propertyName = e.PropertyName;
		if (!(propertyName == "GameName"))
		{
			if (!(propertyName == "PlayerCounts"))
			{
				if (!(propertyName == "PlayingTime"))
				{
					if (!(propertyName == "Tags"))
					{
						if (!(propertyName == "GameType"))
						{
							if (propertyName == "GameComplexity")
							{
								this.GameComplexity.value = NetworkSingleton<GameOptions>.Instance.GameComplexity;
							}
						}
						else
						{
							this.GameType.value = NetworkSingleton<GameOptions>.Instance.GameType;
						}
					}
					else
					{
						this.AppliedTags.Clear();
						this.AppliedTags.AddRange(NetworkSingleton<GameOptions>.Instance.Tags);
					}
				}
				else
				{
					this.ReadGameOptionPlayingTime(NetworkSingleton<GameOptions>.Instance.PlayingTime);
				}
			}
			else
			{
				this.ReadGameOptionPlayerCount(NetworkSingleton<GameOptions>.Instance.PlayerCounts);
			}
		}
		else
		{
			this.GameNameInput.value = NetworkSingleton<GameOptions>.Instance.GameName;
		}
		this.BlockUpdateSource = false;
	}

	// Token: 0x06002472 RID: 9330 RVA: 0x000025B8 File Offset: 0x000007B8
	protected override void NetworkSync()
	{
	}

	// Token: 0x06002473 RID: 9331 RVA: 0x00101287 File Offset: 0x000FF487
	protected override void OnEnable()
	{
		base.OnEnable();
		this.Awake();
		this.UITagMenu.Awake();
		Wait.Frames(delegate
		{
			this.UITagMenu.Initialize(this.AppliedTags.ToList<string>());
		}, 1);
		this.ReloadUI();
	}

	// Token: 0x06002474 RID: 9332 RVA: 0x001012B9 File Offset: 0x000FF4B9
	public void OpenTagMenu()
	{
		this.UITagMenu.gameObject.SetActive(!this.UITagMenu.gameObject.activeSelf);
		this.UITagMenu.Initialize(this.AppliedTags.ToList<string>());
	}

	// Token: 0x06002475 RID: 9333 RVA: 0x001012F4 File Offset: 0x000FF4F4
	protected override void ReloadUI()
	{
		this.BlockUpdateSource = true;
		this.GameNameInput.value = NetworkSingleton<GameOptions>.Instance.GameName;
		this.ReadGameOptionPlayingTime(NetworkSingleton<GameOptions>.Instance.PlayingTime);
		this.ReadGameOptionPlayerCount(NetworkSingleton<GameOptions>.Instance.PlayerCounts);
		this.AppliedTags.Clear();
		this.AppliedTags.AddRange(NetworkSingleton<GameOptions>.Instance.Tags);
		this.UpdateActiveTags(NetworkSingleton<GameOptions>.Instance.Tags);
		this.GameComplexityLabel.text = NetworkSingleton<GameOptions>.Instance.GameComplexity;
		this.GameTypeLabel.text = NetworkSingleton<GameOptions>.Instance.GameType;
		this.BlockUpdateSource = false;
	}

	// Token: 0x06002476 RID: 9334 RVA: 0x001013A0 File Offset: 0x000FF5A0
	protected override void UpdateSource()
	{
		this.BlockUpdateSource = true;
		NetworkSingleton<GameOptions>.Instance.GameName = this.GameNameInput.value;
		NetworkSingleton<GameOptions>.Instance.PlayingTime = this.WriteGameOptionPlayingTime();
		NetworkSingleton<GameOptions>.Instance.PlayerCounts = this.WriteGameOptionPlayerCount();
		List<string> tags = this.AppliedTags.ToList<string>();
		NetworkSingleton<GameOptions>.Instance.Tags = tags;
		this.UpdateActiveTags(tags);
		if (this.GameComplexity.value == "None")
		{
			NetworkSingleton<GameOptions>.Instance.GameComplexity = "";
		}
		else
		{
			NetworkSingleton<GameOptions>.Instance.GameComplexity = this.GameComplexity.value;
		}
		if (this.GameType.value == "None")
		{
			NetworkSingleton<GameOptions>.Instance.GameType = "";
		}
		else
		{
			NetworkSingleton<GameOptions>.Instance.GameType = this.GameType.value;
		}
		this.BlockUpdateSource = false;
	}

	// Token: 0x06002477 RID: 9335 RVA: 0x00101488 File Offset: 0x000FF688
	private int[] WriteGameOptionPlayerCount()
	{
		return new int[]
		{
			this.PlayerCount.Range.Item1,
			this.PlayerCount.Range.Item2
		};
	}

	// Token: 0x06002478 RID: 9336 RVA: 0x001014B6 File Offset: 0x000FF6B6
	private void ReadGameOptionPlayerCount(int[] playerCounts)
	{
		if (playerCounts == null)
		{
			this.PlayerCount.SetRange(0, 0);
			return;
		}
		this.PlayerCount.SetRange(playerCounts[0], playerCounts[1]);
	}

	// Token: 0x06002479 RID: 9337 RVA: 0x001014DA File Offset: 0x000FF6DA
	private int[] WriteGameOptionPlayingTime()
	{
		return new int[]
		{
			this.PlayingTime.Range.Item1,
			this.PlayingTime.Range.Item2
		};
	}

	// Token: 0x0600247A RID: 9338 RVA: 0x00101508 File Offset: 0x000FF708
	private void ReadGameOptionPlayingTime(int[] playingTime)
	{
		if (playingTime == null)
		{
			this.PlayingTime.SetRange(0, 0);
			return;
		}
		this.PlayingTime.SetRange(playingTime[0], playingTime[1]);
	}

	// Token: 0x0600247B RID: 9339 RVA: 0x0010152C File Offset: 0x000FF72C
	private void UpdateActiveTags(IReadOnlyCollection<string> tags)
	{
		string text = "";
		if (tags.Any<string>())
		{
			text = tags.Aggregate("Active Tags: ", (string current, string s) => current + s + ", ");
			text = text.TrimEnd(", ".ToCharArray());
		}
		this.ActiveTags.text = text;
		this.ActiveTagsHover.Tooltip = text;
	}

	// Token: 0x04001772 RID: 6002
	public UIInput GameNameInput;

	// Token: 0x04001773 RID: 6003
	public UIFixedStepDualSlider PlayingTime;

	// Token: 0x04001774 RID: 6004
	public UIDualSlider PlayerCount;

	// Token: 0x04001775 RID: 6005
	public ObservableCollection<string> AppliedTags = new ObservableCollection<string>();

	// Token: 0x04001776 RID: 6006
	public UILabel ActiveTags;

	// Token: 0x04001777 RID: 6007
	public UITooltipScript ActiveTagsHover;

	// Token: 0x04001778 RID: 6008
	public UIPopupList GameType;

	// Token: 0x04001779 RID: 6009
	public UIPopupList GameComplexity;

	// Token: 0x0400177A RID: 6010
	public UILabel GameTypeLabel;

	// Token: 0x0400177B RID: 6011
	public UILabel GameComplexityLabel;

	// Token: 0x0400177C RID: 6012
	public UITagMenu UITagMenu;

	// Token: 0x0400177D RID: 6013
	private bool _isAwake;
}
