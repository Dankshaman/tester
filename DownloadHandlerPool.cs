using System;
using System.Text;
using UnityEngine.Networking;

// Token: 0x02000104 RID: 260
public class DownloadHandlerPool : DownloadHandlerScript
{
	// Token: 0x06000CB9 RID: 3257 RVA: 0x000568E0 File Offset: 0x00054AE0
	public DownloadHandlerPool(ListBuffer<byte> preallocatedBuffer, ListBuffer<byte> downloadBuffer) : base(preallocatedBuffer.GetBuffer())
	{
		this.preallocatedBuffer = preallocatedBuffer;
		this.downloadBuffer = downloadBuffer;
	}

	// Token: 0x06000CBA RID: 3258 RVA: 0x000568FC File Offset: 0x00054AFC
	~DownloadHandlerPool()
	{
		this.Cleanup();
	}

	// Token: 0x06000CBB RID: 3259 RVA: 0x00056928 File Offset: 0x00054B28
	public static DownloadHandlerPool Create(ListBuffer<byte> downloadBuffer)
	{
		return new DownloadHandlerPool(PoolBuffer<ListBuffer<byte>>.Get(1000000), downloadBuffer);
	}

	// Token: 0x06000CBC RID: 3260 RVA: 0x0005693A File Offset: 0x00054B3A
	protected override void ReceiveContentLengthHeader(ulong contentLength)
	{
		if (this.downloadBuffer.GetBufferSize() < (int)contentLength)
		{
			this.downloadBuffer.Resize((int)contentLength);
		}
	}

	// Token: 0x06000CBD RID: 3261 RVA: 0x00056958 File Offset: 0x00054B58
	protected override bool ReceiveData(byte[] data, int dataLength)
	{
		this.downloadBuffer.Add(data, dataLength);
		return true;
	}

	// Token: 0x06000CBE RID: 3262 RVA: 0x00056968 File Offset: 0x00054B68
	protected override void CompleteContent()
	{
		this.CleanupPreallocatedBuffer();
	}

	// Token: 0x06000CBF RID: 3263 RVA: 0x00056970 File Offset: 0x00054B70
	protected override byte[] GetData()
	{
		return this.downloadBuffer.GetData();
	}

	// Token: 0x06000CC0 RID: 3264 RVA: 0x0005697D File Offset: 0x00054B7D
	protected override string GetText()
	{
		return Encoding.UTF8.GetString(this.downloadBuffer.GetBuffer(), 0, this.downloadBuffer.Count);
	}

	// Token: 0x06000CC1 RID: 3265 RVA: 0x00056968 File Offset: 0x00054B68
	public void Cleanup()
	{
		this.CleanupPreallocatedBuffer();
	}

	// Token: 0x06000CC2 RID: 3266 RVA: 0x000569A0 File Offset: 0x00054BA0
	private void CleanupPreallocatedBuffer()
	{
		if (this.preallocatedBuffer != null)
		{
			PoolBuffer<ListBuffer<byte>>.Return(this.preallocatedBuffer);
			this.preallocatedBuffer = null;
		}
	}

	// Token: 0x0400089E RID: 2206
	private const int PREALLOCATED_BUFFER_SIZE = 1000000;

	// Token: 0x0400089F RID: 2207
	private ListBuffer<byte> preallocatedBuffer;

	// Token: 0x040008A0 RID: 2208
	private ListBuffer<byte> downloadBuffer;
}
