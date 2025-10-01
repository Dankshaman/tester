using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200030F RID: 783
public class UIPages : MonoBehaviour
{
	// Token: 0x0600260F RID: 9743 RVA: 0x0010C6AC File Offset: 0x0010A8AC
	private void Start()
	{
		EventDelegate.Add(this.LeftArrow.onClick, delegate()
		{
			Action pageLeft = this.PageLeft;
			if (pageLeft == null)
			{
				return;
			}
			pageLeft();
		});
		EventDelegate.Add(this.RightArrow.onClick, delegate()
		{
			Action pageRight = this.PageRight;
			if (pageRight == null)
			{
				return;
			}
			pageRight();
		});
		if (this.LeftBigArrow)
		{
			EventDelegate.Add(this.LeftBigArrow.onClick, delegate()
			{
				Action pageBigLeft = this.PageBigLeft;
				if (pageBigLeft == null)
				{
					return;
				}
				pageBigLeft();
			});
		}
		if (this.RightBigArrow)
		{
			EventDelegate.Add(this.RightBigArrow.onClick, delegate()
			{
				Action pageBigRight = this.PageBigRight;
				if (pageBigRight == null)
				{
					return;
				}
				pageBigRight();
			});
		}
		using (List<UIPagesButton>.Enumerator enumerator = this.PageButtons.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				UIPagesButton page = enumerator.Current;
				EventDelegate.Add(page.GetComponent<UIButton>().onClick, delegate()
				{
					Action<int> setPage = this.SetPage;
					if (setPage == null)
					{
						return;
					}
					setPage(page.Number);
				});
			}
		}
	}

	// Token: 0x06002610 RID: 9744 RVA: 0x0010C7BC File Offset: 0x0010A9BC
	private void OnDestroy()
	{
		EventDelegate.Remove(this.LeftArrow.onClick, delegate()
		{
			Action pageLeft = this.PageLeft;
			if (pageLeft == null)
			{
				return;
			}
			pageLeft();
		});
		EventDelegate.Remove(this.RightArrow.onClick, delegate()
		{
			Action pageRight = this.PageRight;
			if (pageRight == null)
			{
				return;
			}
			pageRight();
		});
		if (this.LeftBigArrow)
		{
			EventDelegate.Remove(this.LeftBigArrow.onClick, delegate()
			{
				Action pageBigLeft = this.PageBigLeft;
				if (pageBigLeft == null)
				{
					return;
				}
				pageBigLeft();
			});
		}
		if (this.RightBigArrow)
		{
			EventDelegate.Remove(this.RightBigArrow.onClick, delegate()
			{
				Action pageBigRight = this.PageBigRight;
				if (pageBigRight == null)
				{
					return;
				}
				pageBigRight();
			});
		}
		using (List<UIPagesButton>.Enumerator enumerator = this.PageButtons.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				UIPagesButton page = enumerator.Current;
				EventDelegate.Remove(page.GetComponent<UIButton>().onClick, delegate()
				{
					Action<int> setPage = this.SetPage;
					if (setPage == null)
					{
						return;
					}
					setPage(page.Number);
				});
			}
		}
	}

	// Token: 0x14000060 RID: 96
	// (add) Token: 0x06002611 RID: 9745 RVA: 0x0010C8CC File Offset: 0x0010AACC
	// (remove) Token: 0x06002612 RID: 9746 RVA: 0x0010C904 File Offset: 0x0010AB04
	public event Action PageLeft;

	// Token: 0x14000061 RID: 97
	// (add) Token: 0x06002613 RID: 9747 RVA: 0x0010C93C File Offset: 0x0010AB3C
	// (remove) Token: 0x06002614 RID: 9748 RVA: 0x0010C974 File Offset: 0x0010AB74
	public event Action PageRight;

	// Token: 0x14000062 RID: 98
	// (add) Token: 0x06002615 RID: 9749 RVA: 0x0010C9AC File Offset: 0x0010ABAC
	// (remove) Token: 0x06002616 RID: 9750 RVA: 0x0010C9E4 File Offset: 0x0010ABE4
	public event Action PageBigLeft;

	// Token: 0x14000063 RID: 99
	// (add) Token: 0x06002617 RID: 9751 RVA: 0x0010CA1C File Offset: 0x0010AC1C
	// (remove) Token: 0x06002618 RID: 9752 RVA: 0x0010CA54 File Offset: 0x0010AC54
	public event Action PageBigRight;

	// Token: 0x14000064 RID: 100
	// (add) Token: 0x06002619 RID: 9753 RVA: 0x0010CA8C File Offset: 0x0010AC8C
	// (remove) Token: 0x0600261A RID: 9754 RVA: 0x0010CAC4 File Offset: 0x0010ACC4
	public event Action<int> SetPage;

	// Token: 0x0600261B RID: 9755 RVA: 0x0010CAFC File Offset: 0x0010ACFC
	public void Set(int currentPage, int numberPages)
	{
		if (numberPages <= 1)
		{
			base.gameObject.SetActive(false);
			return;
		}
		bool flag = currentPage != 1;
		bool flag2 = currentPage != numberPages;
		this.LeftArrow.gameObject.SetActive(flag);
		this.RightArrow.gameObject.SetActive(flag2);
		this.LeftArrowDisable.SetActive(!flag);
		this.RightArrowDisable.SetActive(!flag2);
		if (this.LeftBigArrow)
		{
			this.LeftBigArrow.gameObject.SetActive(flag);
		}
		if (this.RightBigArrow)
		{
			this.RightBigArrow.gameObject.SetActive(flag2);
		}
		if (this.LeftBigArrowDisable)
		{
			this.LeftBigArrowDisable.SetActive(!flag);
		}
		if (this.RightBigArrowDisable)
		{
			this.RightBigArrowDisable.SetActive(!flag2);
		}
		base.gameObject.SetActive(true);
		this.LeftEllipse.SetActive(false);
		this.RightEllips.SetActive(false);
		for (int i = 0; i < this.PageButtons.Count; i++)
		{
			this.PageButtons[i].gameObject.SetActive(false);
		}
		if (numberPages <= 9)
		{
			for (int j = 0; j < numberPages; j++)
			{
				UIPagesButton uipagesButton = this.PageButtons[j];
				int num = j + 1;
				uipagesButton.gameObject.SetActive(true);
				uipagesButton.Set(num, currentPage == num, this.PageDisplayOffset);
			}
		}
		else
		{
			this.PageButtons[0].gameObject.SetActive(true);
			this.PageButtons[0].Set(1, currentPage == 1, this.PageDisplayOffset);
			this.PageButtons[8].gameObject.SetActive(true);
			this.PageButtons[8].Set(numberPages, numberPages == currentPage, this.PageDisplayOffset);
			if (currentPage > 5)
			{
				this.PageButtons[1].gameObject.SetActive(false);
				this.LeftEllipse.gameObject.SetActive(true);
				this.PageButtons[2].gameObject.SetActive(true);
				this.PageButtons[2].Set(currentPage - 2, false, this.PageDisplayOffset);
				this.PageButtons[3].gameObject.SetActive(true);
				this.PageButtons[3].Set(currentPage - 1, false, this.PageDisplayOffset);
				this.PageButtons[4].gameObject.SetActive(true);
				this.PageButtons[4].Set(currentPage, true, this.PageDisplayOffset);
				this.PageButtons[5].gameObject.SetActive(true);
				this.PageButtons[5].Set(currentPage + 1, false, this.PageDisplayOffset);
				this.PageButtons[6].gameObject.SetActive(true);
				this.PageButtons[6].Set(currentPage + 2, false, this.PageDisplayOffset);
			}
			else
			{
				for (int k = 1; k < 7; k++)
				{
					int num2 = k + 1;
					this.PageButtons[k].gameObject.SetActive(numberPages > num2);
					this.PageButtons[k].Set(num2, currentPage == num2, this.PageDisplayOffset);
				}
			}
			if (numberPages - currentPage > 4)
			{
				this.PageButtons[7].gameObject.SetActive(false);
				this.RightEllips.gameObject.SetActive(true);
			}
			else
			{
				for (int l = 0; l < 6; l++)
				{
					this.PageButtons[7 - l].gameObject.SetActive(true);
					this.PageButtons[7 - l].Set(numberPages - 1 - l, currentPage == numberPages - 1 - l, this.PageDisplayOffset);
				}
			}
		}
		this.Grid.Reposition();
	}

	// Token: 0x040018BC RID: 6332
	public UIGrid Grid;

	// Token: 0x040018BD RID: 6333
	public UIButton LeftArrow;

	// Token: 0x040018BE RID: 6334
	public UIButton RightArrow;

	// Token: 0x040018BF RID: 6335
	public UIButton LeftBigArrow;

	// Token: 0x040018C0 RID: 6336
	public UIButton RightBigArrow;

	// Token: 0x040018C1 RID: 6337
	public GameObject LeftArrowDisable;

	// Token: 0x040018C2 RID: 6338
	public GameObject RightArrowDisable;

	// Token: 0x040018C3 RID: 6339
	public GameObject LeftBigArrowDisable;

	// Token: 0x040018C4 RID: 6340
	public GameObject RightBigArrowDisable;

	// Token: 0x040018C5 RID: 6341
	public GameObject LeftEllipse;

	// Token: 0x040018C6 RID: 6342
	public GameObject RightEllips;

	// Token: 0x040018C7 RID: 6343
	public List<UIPagesButton> PageButtons;

	// Token: 0x040018C8 RID: 6344
	public int PageDisplayOffset;
}
