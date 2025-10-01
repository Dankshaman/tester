using System;
using System.Text;
using NewNet;
using UnityEngine;
using UnityEngine.Networking;

// Token: 0x02000103 RID: 259
public class DownloadHandlerManaged : DownloadHandlerScript
{
	// Token: 0x06000CAE RID: 3246 RVA: 0x00056795 File Offset: 0x00054995
	public DownloadHandlerManaged(BitStream bitStream, byte[] preallocatedBuffer) : base(preallocatedBuffer)
	{
		this.bitStream = bitStream;
	}

	// Token: 0x06000CAF RID: 3247 RVA: 0x000567A5 File Offset: 0x000549A5
	protected override void ReceiveContentLength(int contentLength)
	{
		if (!this.receivedContentHeader)
		{
			Debug.Log("ReceiveContentLength: " + contentLength);
			this.receivedContentHeader = true;
			this.buffer = new byte[contentLength];
		}
	}

	// Token: 0x06000CB0 RID: 3248 RVA: 0x000567D8 File Offset: 0x000549D8
	private static byte[] CreateOrExpandArray(byte[] array, int dataLength)
	{
		byte[] array2 = new byte[dataLength];
		if (array != null)
		{
			Array.Copy(array, array2, array.Length);
		}
		return array2;
	}

	// Token: 0x06000CB1 RID: 3249 RVA: 0x000567FA File Offset: 0x000549FA
	protected override float GetProgress()
	{
		return (float)(this.currentIndex / this.buffer.Length);
	}

	// Token: 0x06000CB2 RID: 3250 RVA: 0x0005680C File Offset: 0x00054A0C
	protected override bool ReceiveData(byte[] data, int dataLength)
	{
		if (this.buffer == null)
		{
			Debug.LogError("ReceiveData before content length header!");
			return false;
		}
		if (data == null || dataLength == 0)
		{
			return false;
		}
		for (int i = 0; i < dataLength; i++)
		{
			this.buffer[this.currentIndex + i] = data[i];
		}
		this.currentIndex += dataLength;
		return true;
	}

	// Token: 0x06000CB3 RID: 3251 RVA: 0x00056862 File Offset: 0x00054A62
	protected override void CompleteContent()
	{
		this.Cleanup();
	}

	// Token: 0x06000CB4 RID: 3252 RVA: 0x0005686A File Offset: 0x00054A6A
	private new void Dispose()
	{
		this.Cleanup();
		base.Dispose();
	}

	// Token: 0x06000CB5 RID: 3253 RVA: 0x00056878 File Offset: 0x00054A78
	private void Cleanup()
	{
		if (this.bitStream != null)
		{
			BitStream.ReturnPooled(this.bitStream);
			this.bitStream = null;
		}
	}

	// Token: 0x06000CB6 RID: 3254 RVA: 0x00056894 File Offset: 0x00054A94
	protected override byte[] GetData()
	{
		return this.buffer;
	}

	// Token: 0x06000CB7 RID: 3255 RVA: 0x0005689C File Offset: 0x00054A9C
	protected override string GetText()
	{
		return Encoding.UTF8.GetString(this.buffer);
	}

	// Token: 0x06000CB8 RID: 3256 RVA: 0x000568B0 File Offset: 0x00054AB0
	public static UnityWebRequest GetWebRequest(string url)
	{
		BitStream pooled = BitStream.GetPooled(65536);
		return new UnityWebRequest(url, "GET", new DownloadHandlerManaged(pooled, pooled.Buffer), null);
	}

	// Token: 0x0400089A RID: 2202
	private byte[] buffer;

	// Token: 0x0400089B RID: 2203
	private int currentIndex;

	// Token: 0x0400089C RID: 2204
	private bool receivedContentHeader;

	// Token: 0x0400089D RID: 2205
	private BitStream bitStream;
}
