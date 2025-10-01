using System;
using System.Threading;
using UnityEngine;

// Token: 0x02000257 RID: 599
public class TextureScale
{
	// Token: 0x06001F95 RID: 8085 RVA: 0x000E18F1 File Offset: 0x000DFAF1
	public static void Bilinear(Texture2D tex, int newWidth, int newHeight, bool Compress)
	{
		TextureScale.ThreadedScale(tex, newWidth, newHeight, true, Compress);
	}

	// Token: 0x06001F96 RID: 8086 RVA: 0x000E1900 File Offset: 0x000DFB00
	private static void ThreadedScale(Texture2D tex, int newWidth, int newHeight, bool useBilinear, bool Compress)
	{
		if (tex.width == newWidth && tex.height == newHeight)
		{
			if (Compress)
			{
				tex.Compress(true);
			}
			return;
		}
		TextureScale.texColors = tex.GetPixels();
		TextureScale.newColors = new Color[newWidth * newHeight];
		if (useBilinear)
		{
			TextureScale.ratioX = 1f / ((float)newWidth / (float)(tex.width - 1));
			TextureScale.ratioY = 1f / ((float)newHeight / (float)(tex.height - 1));
		}
		else
		{
			TextureScale.ratioX = (float)tex.width / (float)newWidth;
			TextureScale.ratioY = (float)tex.height / (float)newHeight;
		}
		TextureScale.w = tex.width;
		TextureScale.w2 = newWidth;
		int num = Mathf.Min(SystemInfo.processorCount, newHeight);
		int num2 = newHeight / num;
		TextureScale.finishCount = 0;
		if (TextureScale.mutex == null)
		{
			TextureScale.mutex = new Mutex(false);
		}
		if (num > 1)
		{
			int i;
			TextureScale.ThreadData threadData;
			for (i = 0; i < num - 1; i++)
			{
				threadData = new TextureScale.ThreadData(num2 * i, num2 * (i + 1));
				new Thread(useBilinear ? new ParameterizedThreadStart(TextureScale.BilinearScale) : new ParameterizedThreadStart(TextureScale.PointScale)).Start(threadData);
			}
			threadData = new TextureScale.ThreadData(num2 * i, newHeight);
			if (useBilinear)
			{
				TextureScale.BilinearScale(threadData);
			}
			else
			{
				TextureScale.PointScale(threadData);
			}
			while (TextureScale.finishCount < num)
			{
				Thread.Sleep(1);
			}
		}
		else
		{
			TextureScale.ThreadData obj = new TextureScale.ThreadData(0, newHeight);
			if (useBilinear)
			{
				TextureScale.BilinearScale(obj);
			}
			else
			{
				TextureScale.PointScale(obj);
			}
		}
		tex.Resize(newWidth, newHeight);
		tex.SetPixels(TextureScale.newColors);
		tex.Apply();
		if (Compress)
		{
			tex.Compress(true);
		}
	}

	// Token: 0x06001F97 RID: 8087 RVA: 0x000E1A88 File Offset: 0x000DFC88
	public static void BilinearScale(object obj)
	{
		TextureScale.ThreadData threadData = (TextureScale.ThreadData)obj;
		for (int i = threadData.start; i < threadData.end; i++)
		{
			int num = (int)Mathf.Floor((float)i * TextureScale.ratioY);
			int num2 = num * TextureScale.w;
			int num3 = (num + 1) * TextureScale.w;
			int num4 = i * TextureScale.w2;
			for (int j = 0; j < TextureScale.w2; j++)
			{
				int num5 = (int)Mathf.Floor((float)j * TextureScale.ratioX);
				float value = (float)j * TextureScale.ratioX - (float)num5;
				TextureScale.newColors[num4 + j] = TextureScale.ColorLerpUnclamped(TextureScale.ColorLerpUnclamped(TextureScale.texColors[num2 + num5], TextureScale.texColors[num2 + num5 + 1], value), TextureScale.ColorLerpUnclamped(TextureScale.texColors[num3 + num5], TextureScale.texColors[num3 + num5 + 1], value), (float)i * TextureScale.ratioY - (float)num);
			}
		}
		TextureScale.mutex.WaitOne();
		TextureScale.finishCount++;
		TextureScale.mutex.ReleaseMutex();
	}

	// Token: 0x06001F98 RID: 8088 RVA: 0x000E1BAC File Offset: 0x000DFDAC
	public static void PointScale(object obj)
	{
		TextureScale.ThreadData threadData = (TextureScale.ThreadData)obj;
		for (int i = threadData.start; i < threadData.end; i++)
		{
			int num = (int)(TextureScale.ratioY * (float)i) * TextureScale.w;
			int num2 = i * TextureScale.w2;
			for (int j = 0; j < TextureScale.w2; j++)
			{
				TextureScale.newColors[num2 + j] = TextureScale.texColors[(int)((float)num + TextureScale.ratioX * (float)j)];
			}
		}
		TextureScale.mutex.WaitOne();
		TextureScale.finishCount++;
		TextureScale.mutex.ReleaseMutex();
	}

	// Token: 0x06001F99 RID: 8089 RVA: 0x000E1C48 File Offset: 0x000DFE48
	private static Color ColorLerpUnclamped(Color c1, Color c2, float value)
	{
		return new Color(c1.r + (c2.r - c1.r) * value, c1.g + (c2.g - c1.g) * value, c1.b + (c2.b - c1.b) * value, c1.a + (c2.a - c1.a) * value);
	}

	// Token: 0x04001362 RID: 4962
	private static Color[] texColors;

	// Token: 0x04001363 RID: 4963
	private static Color[] newColors;

	// Token: 0x04001364 RID: 4964
	private static int w;

	// Token: 0x04001365 RID: 4965
	private static float ratioX;

	// Token: 0x04001366 RID: 4966
	private static float ratioY;

	// Token: 0x04001367 RID: 4967
	private static int w2;

	// Token: 0x04001368 RID: 4968
	private static int finishCount;

	// Token: 0x04001369 RID: 4969
	private static Mutex mutex;

	// Token: 0x020006FA RID: 1786
	public class ThreadData
	{
		// Token: 0x06003D55 RID: 15701 RVA: 0x0017B74E File Offset: 0x0017994E
		public ThreadData(int s, int e)
		{
			this.start = s;
			this.end = e;
		}

		// Token: 0x04002A3C RID: 10812
		public int start;

		// Token: 0x04002A3D RID: 10813
		public int end;
	}
}
