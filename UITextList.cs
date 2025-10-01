using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

// Token: 0x02000092 RID: 146
[AddComponentMenu("NGUI/UI/Text List")]
public class UITextList : MonoBehaviour
{
	// Token: 0x1700018B RID: 395
	// (get) Token: 0x060007CE RID: 1998 RVA: 0x00036D9C File Offset: 0x00034F9C
	protected BetterList<UITextList.Paragraph> paragraphs
	{
		get
		{
			if (this.mParagraphs == null && !UITextList.mHistory.TryGetValue(base.name, out this.mParagraphs))
			{
				this.mParagraphs = new BetterList<UITextList.Paragraph>();
				UITextList.mHistory.Add(base.name, this.mParagraphs);
			}
			return this.mParagraphs;
		}
	}

	// Token: 0x1700018C RID: 396
	// (get) Token: 0x060007CF RID: 1999 RVA: 0x00036DF0 File Offset: 0x00034FF0
	public bool isValid
	{
		get
		{
			return this.textLabel != null && this.textLabel.ambigiousFont != null;
		}
	}

	// Token: 0x1700018D RID: 397
	// (get) Token: 0x060007D0 RID: 2000 RVA: 0x00036E13 File Offset: 0x00035013
	// (set) Token: 0x060007D1 RID: 2001 RVA: 0x00036E1C File Offset: 0x0003501C
	public float scrollValue
	{
		get
		{
			return this.mScroll;
		}
		set
		{
			value = Mathf.Clamp01(value);
			if (this.isValid && this.mScroll != value)
			{
				if (this.scrollBar != null)
				{
					this.scrollBar.value = value;
					return;
				}
				this.mScroll = value;
				this.UpdateVisibleText();
			}
		}
	}

	// Token: 0x1700018E RID: 398
	// (get) Token: 0x060007D2 RID: 2002 RVA: 0x00036E6A File Offset: 0x0003506A
	protected float lineHeight
	{
		get
		{
			if (!(this.textLabel != null))
			{
				return 20f;
			}
			return (float)this.textLabel.fontSize + this.textLabel.effectiveSpacingY;
		}
	}

	// Token: 0x1700018F RID: 399
	// (get) Token: 0x060007D3 RID: 2003 RVA: 0x00036E98 File Offset: 0x00035098
	protected int scrollHeight
	{
		get
		{
			if (!this.isValid)
			{
				return 0;
			}
			int num = Mathf.FloorToInt((float)this.textLabel.height / this.lineHeight);
			return Mathf.Max(0, this.mTotalLines - num);
		}
	}

	// Token: 0x060007D4 RID: 2004 RVA: 0x00036ED6 File Offset: 0x000350D6
	public void Clear()
	{
		this.paragraphs.Clear();
		this.UpdateVisibleText();
	}

	// Token: 0x060007D5 RID: 2005 RVA: 0x00036EEC File Offset: 0x000350EC
	private void Start()
	{
		if (this.textLabel == null)
		{
			this.textLabel = base.GetComponentInChildren<UILabel>();
		}
		if (this.scrollBar != null)
		{
			EventDelegate.Add(this.scrollBar.onChange, new EventDelegate.Callback(this.OnScrollBar));
		}
		this.textLabel.overflowMethod = UILabel.Overflow.ClampContent;
		if (this.style == UITextList.Style.Chat)
		{
			this.textLabel.pivot = UIWidget.Pivot.BottomLeft;
			this.scrollValue = 1f;
			return;
		}
		this.textLabel.pivot = UIWidget.Pivot.TopLeft;
		this.scrollValue = 0f;
	}

	// Token: 0x060007D6 RID: 2006 RVA: 0x00036F82 File Offset: 0x00035182
	private void Update()
	{
		if (this.isValid && (this.textLabel.width != this.mLastWidth || this.textLabel.height != this.mLastHeight))
		{
			this.Rebuild();
		}
	}

	// Token: 0x060007D7 RID: 2007 RVA: 0x00036FB8 File Offset: 0x000351B8
	public void OnScroll(float val)
	{
		int scrollHeight = this.scrollHeight;
		if (scrollHeight != 0)
		{
			val *= this.lineHeight;
			this.scrollValue = this.mScroll - val / (float)scrollHeight;
		}
	}

	// Token: 0x060007D8 RID: 2008 RVA: 0x00036FEC File Offset: 0x000351EC
	public void OnDrag(Vector2 delta)
	{
		int scrollHeight = this.scrollHeight;
		if (scrollHeight != 0)
		{
			float num = delta.y / this.lineHeight;
			this.scrollValue = this.mScroll + num / (float)scrollHeight;
		}
	}

	// Token: 0x060007D9 RID: 2009 RVA: 0x00037022 File Offset: 0x00035222
	private void OnScrollBar()
	{
		this.mScroll = UIProgressBar.current.value;
		this.UpdateVisibleText();
	}

	// Token: 0x060007DA RID: 2010 RVA: 0x0003703A File Offset: 0x0003523A
	public void Add(string text)
	{
		this.Add(text, true);
	}

	// Token: 0x060007DB RID: 2011 RVA: 0x00037044 File Offset: 0x00035244
	protected void Add(string text, bool updateVisible)
	{
		UITextList.Paragraph paragraph;
		if (this.paragraphs.size < this.paragraphHistory)
		{
			paragraph = new UITextList.Paragraph();
		}
		else
		{
			paragraph = this.mParagraphs[0];
			this.mParagraphs.RemoveAt(0);
		}
		paragraph.text = text;
		this.mParagraphs.Add(paragraph);
		this.Rebuild();
	}

	// Token: 0x060007DC RID: 2012 RVA: 0x000370A0 File Offset: 0x000352A0
	public void Add(List<string> texts)
	{
		for (int i = 0; i < texts.Count; i++)
		{
			string text = texts[i];
			UITextList.Paragraph paragraph;
			if (this.paragraphs.size < this.paragraphHistory)
			{
				paragraph = new UITextList.Paragraph();
			}
			else
			{
				paragraph = this.mParagraphs[0];
				this.mParagraphs.RemoveAt(0);
			}
			paragraph.text = text;
			this.mParagraphs.Add(paragraph);
		}
		this.Rebuild();
	}

	// Token: 0x060007DD RID: 2013 RVA: 0x00037115 File Offset: 0x00035315
	public void Edit(int index, string text)
	{
		if (index < 0)
		{
			index += this.paragraphs.size;
		}
		if (index < 0 || index >= this.paragraphs.size)
		{
			return;
		}
		this.mParagraphs[index].text = text;
		this.Rebuild();
	}

	// Token: 0x060007DE RID: 2014 RVA: 0x00037158 File Offset: 0x00035358
	public string GetText()
	{
		StringBuilder stringBuilder = new StringBuilder();
		for (int i = 0; i < this.paragraphs.size; i++)
		{
			stringBuilder.AppendLine(this.paragraphs[i].text);
		}
		return stringBuilder.ToString();
	}

	// Token: 0x060007DF RID: 2015 RVA: 0x000371A0 File Offset: 0x000353A0
	protected void Rebuild()
	{
		if (this.isValid)
		{
			this.mLastWidth = this.textLabel.width;
			this.mLastHeight = this.textLabel.height;
			this.textLabel.UpdateNGUIText();
			NGUIText.rectHeight = 1000000;
			NGUIText.regionHeight = 1000000;
			this.mTotalLines = 0;
			for (int i = 0; i < this.paragraphs.size; i++)
			{
				UITextList.Paragraph paragraph = this.mParagraphs.buffer[i];
				string text;
				NGUIText.WrapText(paragraph.text, out text, false, true, false);
				paragraph.lines = text.Split(new char[]
				{
					'\n'
				});
				this.mTotalLines += paragraph.lines.Length;
			}
			this.mTotalLines = 0;
			int j = 0;
			int size = this.mParagraphs.size;
			while (j < size)
			{
				this.mTotalLines += this.mParagraphs.buffer[j].lines.Length;
				j++;
			}
			if (this.scrollBar != null)
			{
				UIScrollBar uiscrollBar = this.scrollBar as UIScrollBar;
				if (uiscrollBar != null)
				{
					uiscrollBar.barSize = ((this.mTotalLines == 0) ? 1f : (1f - (float)this.scrollHeight / (float)this.mTotalLines));
				}
				uiscrollBar.value = 1f;
				uiscrollBar.gameObject.SetActive(uiscrollBar.barSize != 1f);
			}
			this.UpdateVisibleText();
		}
	}

	// Token: 0x060007E0 RID: 2016 RVA: 0x00037324 File Offset: 0x00035524
	protected void UpdateVisibleText()
	{
		if (this.isValid)
		{
			if (this.mTotalLines == 0)
			{
				this.textLabel.text = "";
				return;
			}
			int num = Mathf.FloorToInt((float)this.textLabel.height / this.lineHeight);
			int num2 = Mathf.Max(0, this.mTotalLines - num);
			int num3 = Mathf.RoundToInt(this.mScroll * (float)num2);
			if (num3 < 0)
			{
				num3 = 0;
			}
			StringBuilder stringBuilder = new StringBuilder();
			int num4 = 0;
			int size = this.paragraphs.size;
			while (num > 0 && num4 < size)
			{
				UITextList.Paragraph paragraph = this.mParagraphs.buffer[num4];
				int num5 = 0;
				int num6 = paragraph.lines.Length;
				while (num > 0 && num5 < num6)
				{
					string value = paragraph.lines[num5];
					if (num3 > 0)
					{
						num3--;
					}
					else
					{
						if (stringBuilder.Length > 0)
						{
							stringBuilder.Append("\n");
						}
						stringBuilder.Append(value);
						num--;
					}
					num5++;
				}
				num4++;
			}
			this.textLabel.text = stringBuilder.ToString();
		}
	}

	// Token: 0x0400056C RID: 1388
	public UILabel textLabel;

	// Token: 0x0400056D RID: 1389
	public UIProgressBar scrollBar;

	// Token: 0x0400056E RID: 1390
	public UITextList.Style style;

	// Token: 0x0400056F RID: 1391
	public int paragraphHistory = 100;

	// Token: 0x04000570 RID: 1392
	protected char[] mSeparator = new char[]
	{
		'\n'
	};

	// Token: 0x04000571 RID: 1393
	protected float mScroll;

	// Token: 0x04000572 RID: 1394
	protected int mTotalLines;

	// Token: 0x04000573 RID: 1395
	protected int mLastWidth;

	// Token: 0x04000574 RID: 1396
	protected int mLastHeight;

	// Token: 0x04000575 RID: 1397
	private BetterList<UITextList.Paragraph> mParagraphs;

	// Token: 0x04000576 RID: 1398
	private static Dictionary<string, BetterList<UITextList.Paragraph>> mHistory = new Dictionary<string, BetterList<UITextList.Paragraph>>();

	// Token: 0x0200057B RID: 1403
	public enum Style
	{
		// Token: 0x040024FB RID: 9467
		Text,
		// Token: 0x040024FC RID: 9468
		Chat
	}

	// Token: 0x0200057C RID: 1404
	protected class Paragraph
	{
		// Token: 0x040024FD RID: 9469
		public string text;

		// Token: 0x040024FE RID: 9470
		public string[] lines;
	}
}
