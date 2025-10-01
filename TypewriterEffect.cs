using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

// Token: 0x0200002A RID: 42
[RequireComponent(typeof(UILabel))]
[AddComponentMenu("NGUI/Interaction/Typewriter Effect")]
public class TypewriterEffect : MonoBehaviour
{
	// Token: 0x17000011 RID: 17
	// (get) Token: 0x060000C0 RID: 192 RVA: 0x0000557C File Offset: 0x0000377C
	public bool isActive
	{
		get
		{
			return this.mActive;
		}
	}

	// Token: 0x060000C1 RID: 193 RVA: 0x00005584 File Offset: 0x00003784
	public void ResetToBeginning()
	{
		this.Finish();
		this.mReset = true;
		this.mActive = true;
		this.mNextChar = 0f;
		this.mCurrentOffset = 0;
		this.Update();
	}

	// Token: 0x060000C2 RID: 194 RVA: 0x000055B4 File Offset: 0x000037B4
	public void Finish()
	{
		if (this.mActive)
		{
			this.mActive = false;
			if (!this.mReset)
			{
				this.mCurrentOffset = this.mFullText.Length;
				this.mFade.Clear();
				this.mLabel.text = this.mFullText;
			}
			if (this.keepFullDimensions && this.scrollView != null)
			{
				this.scrollView.UpdatePosition();
			}
			TypewriterEffect.current = this;
			EventDelegate.Execute(this.onFinished);
			TypewriterEffect.current = null;
		}
	}

	// Token: 0x060000C3 RID: 195 RVA: 0x0000563D File Offset: 0x0000383D
	private void OnEnable()
	{
		this.mReset = true;
		this.mActive = true;
	}

	// Token: 0x060000C4 RID: 196 RVA: 0x0000564D File Offset: 0x0000384D
	private void OnDisable()
	{
		this.Finish();
	}

	// Token: 0x060000C5 RID: 197 RVA: 0x00005658 File Offset: 0x00003858
	private void Update()
	{
		if (!this.mActive)
		{
			return;
		}
		if (this.mReset)
		{
			this.mCurrentOffset = 0;
			this.mReset = false;
			this.mLabel = base.GetComponent<UILabel>();
			this.mFullText = this.mLabel.processedText;
			this.mFade.Clear();
			if (this.keepFullDimensions && this.scrollView != null)
			{
				this.scrollView.UpdatePosition();
			}
		}
		if (string.IsNullOrEmpty(this.mFullText))
		{
			return;
		}
		int length = this.mFullText.Length;
		while (this.mCurrentOffset < length && this.mNextChar <= RealTime.time)
		{
			int num = this.mCurrentOffset;
			this.charsPerSecond = Mathf.Max(1, this.charsPerSecond);
			if (this.mLabel.supportEncoding)
			{
				while (NGUIText.ParseSymbol(this.mFullText, ref this.mCurrentOffset))
				{
				}
			}
			this.mCurrentOffset++;
			if (this.mCurrentOffset > length)
			{
				break;
			}
			float num2 = 1f / (float)this.charsPerSecond;
			char c = (num < length) ? this.mFullText[num] : '\n';
			if (c == '\n')
			{
				num2 += this.delayOnNewLine;
			}
			else if (num + 1 == length || this.mFullText[num + 1] <= ' ')
			{
				if (c == '.')
				{
					if (num + 2 < length && this.mFullText[num + 1] == '.' && this.mFullText[num + 2] == '.')
					{
						num2 += this.delayOnPeriod * 3f;
						num += 2;
					}
					else
					{
						num2 += this.delayOnPeriod;
					}
				}
				else if (c == '!' || c == '?')
				{
					num2 += this.delayOnPeriod;
				}
			}
			if (this.mNextChar == 0f)
			{
				this.mNextChar = RealTime.time + num2;
			}
			else
			{
				this.mNextChar += num2;
			}
			if (this.fadeInTime != 0f)
			{
				TypewriterEffect.FadeEntry item = default(TypewriterEffect.FadeEntry);
				item.index = num;
				item.alpha = 0f;
				item.text = this.mFullText.Substring(num, this.mCurrentOffset - num);
				this.mFade.Add(item);
			}
			else
			{
				this.mLabel.text = (this.keepFullDimensions ? (this.mFullText.Substring(0, this.mCurrentOffset) + "[00]" + this.mFullText.Substring(this.mCurrentOffset)) : this.mFullText.Substring(0, this.mCurrentOffset));
				if (!this.keepFullDimensions && this.scrollView != null)
				{
					this.scrollView.UpdatePosition();
				}
			}
		}
		if (this.mCurrentOffset >= length && this.mFade.size == 0)
		{
			this.mLabel.text = this.mFullText;
			TypewriterEffect.current = this;
			EventDelegate.Execute(this.onFinished);
			TypewriterEffect.current = null;
			this.mActive = false;
			return;
		}
		if (this.mFade.size != 0)
		{
			int i = 0;
			while (i < this.mFade.size)
			{
				TypewriterEffect.FadeEntry fadeEntry = this.mFade[i];
				fadeEntry.alpha += RealTime.deltaTime / this.fadeInTime;
				if (fadeEntry.alpha < 1f)
				{
					this.mFade[i] = fadeEntry;
					i++;
				}
				else
				{
					this.mFade.RemoveAt(i);
				}
			}
			if (this.mFade.size == 0)
			{
				if (this.keepFullDimensions)
				{
					this.mLabel.text = this.mFullText.Substring(0, this.mCurrentOffset) + "[00]" + this.mFullText.Substring(this.mCurrentOffset);
					return;
				}
				this.mLabel.text = this.mFullText.Substring(0, this.mCurrentOffset);
				return;
			}
			else
			{
				StringBuilder stringBuilder = new StringBuilder();
				for (int j = 0; j < this.mFade.size; j++)
				{
					TypewriterEffect.FadeEntry fadeEntry2 = this.mFade[j];
					if (j == 0)
					{
						stringBuilder.Append(this.mFullText.Substring(0, fadeEntry2.index));
					}
					stringBuilder.Append('[');
					stringBuilder.Append(NGUIText.EncodeAlpha(fadeEntry2.alpha));
					stringBuilder.Append(']');
					stringBuilder.Append(fadeEntry2.text);
				}
				if (this.keepFullDimensions)
				{
					stringBuilder.Append("[00]");
					stringBuilder.Append(this.mFullText.Substring(this.mCurrentOffset));
				}
				this.mLabel.text = stringBuilder.ToString();
			}
		}
	}

	// Token: 0x0400008E RID: 142
	public static TypewriterEffect current;

	// Token: 0x0400008F RID: 143
	public int charsPerSecond = 20;

	// Token: 0x04000090 RID: 144
	public float fadeInTime;

	// Token: 0x04000091 RID: 145
	public float delayOnPeriod;

	// Token: 0x04000092 RID: 146
	public float delayOnNewLine;

	// Token: 0x04000093 RID: 147
	public UIScrollView scrollView;

	// Token: 0x04000094 RID: 148
	public bool keepFullDimensions;

	// Token: 0x04000095 RID: 149
	public List<EventDelegate> onFinished = new List<EventDelegate>();

	// Token: 0x04000096 RID: 150
	private UILabel mLabel;

	// Token: 0x04000097 RID: 151
	private string mFullText = "";

	// Token: 0x04000098 RID: 152
	private int mCurrentOffset;

	// Token: 0x04000099 RID: 153
	private float mNextChar;

	// Token: 0x0400009A RID: 154
	private bool mReset = true;

	// Token: 0x0400009B RID: 155
	private bool mActive;

	// Token: 0x0400009C RID: 156
	private BetterList<TypewriterEffect.FadeEntry> mFade = new BetterList<TypewriterEffect.FadeEntry>();

	// Token: 0x02000501 RID: 1281
	private struct FadeEntry
	{
		// Token: 0x0400238A RID: 9098
		public int index;

		// Token: 0x0400238B RID: 9099
		public string text;

		// Token: 0x0400238C RID: 9100
		public float alpha;
	}
}
