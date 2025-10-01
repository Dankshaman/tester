using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Tayx.Graphy.Utils;
using UnityEngine;

// Token: 0x02000347 RID: 839
public class UITag : MonoBehaviour, INotifyPropertyChanged
{
	// Token: 0x170004BA RID: 1210
	// (get) Token: 0x060027CD RID: 10189 RVA: 0x0011A075 File Offset: 0x00118275
	// (set) Token: 0x060027CE RID: 10190 RVA: 0x0011A07D File Offset: 0x0011827D
	public bool Active
	{
		get
		{
			return this._active;
		}
		private set
		{
			this.SetField<bool>(ref this._active, value, "Active");
		}
	}

	// Token: 0x060027CF RID: 10191 RVA: 0x0011A091 File Offset: 0x00118291
	private void Update()
	{
		if (this.Text == this._oldText)
		{
			return;
		}
		this.Resize();
	}

	// Token: 0x060027D0 RID: 10192 RVA: 0x0011A0B0 File Offset: 0x001182B0
	public void Resize()
	{
		this._oldText = this.Text;
		UILabel.Overflow overflowMethod = this.Label.overflowMethod;
		this.Label.overflowMethod = UILabel.Overflow.ResizeFreely;
		this.Label.text = (this.IsCustomTag ? this.Text : Language.Translate(this.Text));
		int num = 30 + this.Label.printedSize.x.ToInt();
		num += num % 2;
		this.Background.width = num;
		this.Background.height += this.Background.height % 2;
		this.Label.overflowMethod = overflowMethod;
	}

	// Token: 0x060027D1 RID: 10193 RVA: 0x0011A15D File Offset: 0x0011835D
	public void Start()
	{
		this.Toggle.startsActive = this.Active;
	}

	// Token: 0x060027D2 RID: 10194 RVA: 0x0011A170 File Offset: 0x00118370
	public void OnToggleClicked()
	{
		if (this.Toggle.value)
		{
			this.OnAddClicked(null);
			return;
		}
		this.OnRemoveClicked(null);
	}

	// Token: 0x060027D3 RID: 10195 RVA: 0x0011A18E File Offset: 0x0011838E
	public void OnAddClicked(GameObject _)
	{
		this.Active = true;
		this.Background.color = this._selectedColor;
	}

	// Token: 0x060027D4 RID: 10196 RVA: 0x0011A1AD File Offset: 0x001183AD
	public void OnRemoveClicked(GameObject _)
	{
		this.Active = false;
		this.Background.color = this._unselectedColor;
	}

	// Token: 0x14000066 RID: 102
	// (add) Token: 0x060027D5 RID: 10197 RVA: 0x0011A1CC File Offset: 0x001183CC
	// (remove) Token: 0x060027D6 RID: 10198 RVA: 0x0011A204 File Offset: 0x00118404
	public event PropertyChangedEventHandler PropertyChanged;

	// Token: 0x060027D7 RID: 10199 RVA: 0x0011A239 File Offset: 0x00118439
	protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
	{
		PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
		if (propertyChanged == null)
		{
			return;
		}
		propertyChanged(this, new PropertyChangedEventArgs(propertyName));
	}

	// Token: 0x060027D8 RID: 10200 RVA: 0x0011A252 File Offset: 0x00118452
	private void SetField<T>(ref T field, T value, [CallerMemberName] string propertyName = null)
	{
		if (EqualityComparer<!!0>.Default.Equals(field, value))
		{
			return;
		}
		field = value;
		this.OnPropertyChanged(propertyName);
	}

	// Token: 0x04001A1C RID: 6684
	private const int MIN_WIDTH = 30;

	// Token: 0x04001A1D RID: 6685
	private const int CHAR_MULTIPLIER = 12;

	// Token: 0x04001A1E RID: 6686
	private readonly Colour _selectedColor = Colour.ColourFromRGBHex("#0089EA");

	// Token: 0x04001A1F RID: 6687
	private readonly Colour _unselectedColor = Colour.ColourFromRGBHex("#AAAAAA");

	// Token: 0x04001A20 RID: 6688
	private readonly Colour _addButtonHoverColor = Colour.ColourFromRGBHex("#0089EA");

	// Token: 0x04001A21 RID: 6689
	private readonly Colour _removeButtonHoverColor = Colour.ColourFromRGBHex("#B60808");

	// Token: 0x04001A22 RID: 6690
	public UIToggle Toggle;

	// Token: 0x04001A23 RID: 6691
	public GameObject AddButton;

	// Token: 0x04001A24 RID: 6692
	public GameObject RemoveButton;

	// Token: 0x04001A25 RID: 6693
	public UISprite Background;

	// Token: 0x04001A26 RID: 6694
	public UILabel Label;

	// Token: 0x04001A27 RID: 6695
	public int ComponentTagIndex;

	// Token: 0x04001A28 RID: 6696
	public bool IsCustomTag;

	// Token: 0x04001A29 RID: 6697
	public string Text;

	// Token: 0x04001A2A RID: 6698
	private bool _active;

	// Token: 0x04001A2B RID: 6699
	private string _oldText = "";
}
