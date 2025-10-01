using System;
using NewNet;
using UnityEngine;

// Token: 0x020002B6 RID: 694
public class UICustomJigsawPuzzle : UICustomObject<UICustomJigsawPuzzle>
{
	// Token: 0x1700046D RID: 1133
	// (get) Token: 0x06002260 RID: 8800 RVA: 0x000F6860 File Offset: 0x000F4A60
	// (set) Token: 0x06002261 RID: 8801 RVA: 0x000F686D File Offset: 0x000F4A6D
	private string CustomImageURL
	{
		get
		{
			return this.ImageInput.value;
		}
		set
		{
			this.ImageInput.value = value;
		}
	}

	// Token: 0x1700046E RID: 1134
	// (get) Token: 0x06002262 RID: 8802 RVA: 0x000F687C File Offset: 0x000F4A7C
	// (set) Token: 0x06002263 RID: 8803 RVA: 0x000F68E8 File Offset: 0x000F4AE8
	private int NumPuzzlePieces
	{
		get
		{
			int num = 0;
			for (int i = 0; i < this.JigsawTypeToggles.Length; i++)
			{
				if (this.JigsawTypeToggles[i].value)
				{
					num = i;
					break;
				}
			}
			int result = 80;
			switch (num)
			{
			case 0:
				result = 20;
				break;
			case 1:
				result = 80;
				break;
			case 2:
				result = 180;
				break;
			case 3:
				result = 320;
				break;
			}
			return result;
		}
		set
		{
			int num = 0;
			if (value <= 80)
			{
				if (value != 20)
				{
					if (value == 80)
					{
						num = 1;
					}
				}
				else
				{
					num = 0;
				}
			}
			else if (value != 180)
			{
				if (value == 320)
				{
					num = 3;
				}
			}
			else
			{
				num = 2;
			}
			int group = this.JigsawTypeToggles[0].group;
			for (int i = 0; i < this.JigsawTypeToggles.Length; i++)
			{
				this.JigsawTypeToggles[i].group = 0;
				this.JigsawTypeToggles[i].value = (i == num);
				this.JigsawTypeToggles[i].group = group;
			}
		}
	}

	// Token: 0x1700046F RID: 1135
	// (get) Token: 0x06002264 RID: 8804 RVA: 0x000F6976 File Offset: 0x000F4B76
	// (set) Token: 0x06002265 RID: 8805 RVA: 0x000F6983 File Offset: 0x000F4B83
	private bool bImageOnBoard
	{
		get
		{
			return this.ImageOnBoardToggle.value;
		}
		set
		{
			this.ImageOnBoardToggle.value = value;
		}
	}

	// Token: 0x06002266 RID: 8806 RVA: 0x000F6994 File Offset: 0x000F4B94
	protected override void OnEnable()
	{
		base.OnEnable();
		this.TargetCustomJigsaw = this.TargetCustomObject.GetComponent<CustomJigsawPuzzle>();
		this.ImageInput.SelectAllTextOnClick = true;
		this.CustomImageURL = PlayerPrefs.GetString("JigsawURL", "");
		this.NumPuzzlePieces = PlayerPrefs.GetInt("JigsawPieces", 80);
		this.bImageOnBoard = (PlayerPrefs.GetInt("JigsawImageOnBoard", 1) != 0);
	}

	// Token: 0x06002267 RID: 8807 RVA: 0x000F6A00 File Offset: 0x000F4C00
	public override void Import()
	{
		this.CustomImageURL = this.CustomImageURL.Trim();
		if (string.IsNullOrEmpty(this.CustomImageURL))
		{
			Chat.LogError("You must supply a custom image URL.", true);
			return;
		}
		if (Network.isServer)
		{
			this.TargetCustomJigsaw.CustomImageURL = this.CustomImageURL;
			this.TargetCustomJigsaw.NumPuzzlePieces = this.NumPuzzlePieces;
			this.TargetCustomJigsaw.bImageOnBoard = this.bImageOnBoard;
			this.TargetCustomJigsaw.CallCustomRPC();
			PlayerPrefs.SetString("JigsawURL", this.CustomImageURL);
			PlayerPrefs.SetInt("JigsawPieces", this.NumPuzzlePieces);
			PlayerPrefs.SetInt("JigsawImageOnBoard", this.bImageOnBoard ? 1 : 0);
		}
		base.Close();
	}

	// Token: 0x040015B8 RID: 5560
	private CustomJigsawPuzzle TargetCustomJigsaw;

	// Token: 0x040015B9 RID: 5561
	public UIToggle[] JigsawTypeToggles;

	// Token: 0x040015BA RID: 5562
	public UIInput ImageInput;

	// Token: 0x040015BB RID: 5563
	public UIToggle ImageOnBoardToggle;
}
