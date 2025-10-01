using System;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;

// Token: 0x02000102 RID: 258
public class DownloadHandlerFile : DownloadHandlerScript
{
	// Token: 0x17000213 RID: 531
	// (get) Token: 0x06000CA6 RID: 3238 RVA: 0x000566A0 File Offset: 0x000548A0
	public int contentLength
	{
		get
		{
			if (this._received <= this._contentLength)
			{
				return this._contentLength;
			}
			return this._received;
		}
	}

	// Token: 0x06000CA7 RID: 3239 RVA: 0x000566C0 File Offset: 0x000548C0
	public DownloadHandlerFile(string localFilePath, int bufferSize = 4096, FileShare fileShare = FileShare.ReadWrite) : base(new byte[bufferSize])
	{
		string directoryName = Path.GetDirectoryName(localFilePath);
		if (!Directory.Exists(directoryName))
		{
			Directory.CreateDirectory(directoryName);
		}
		this._contentLength = -1;
		this._received = 0;
		this._stream = new FileStream(localFilePath, FileMode.Create, FileAccess.Write, fileShare, bufferSize);
	}

	// Token: 0x06000CA8 RID: 3240 RVA: 0x0005670D File Offset: 0x0005490D
	protected override float GetProgress()
	{
		if (this.contentLength > 0)
		{
			return Mathf.Clamp01((float)this._received / (float)this.contentLength);
		}
		return 0f;
	}

	// Token: 0x06000CA9 RID: 3241 RVA: 0x00056732 File Offset: 0x00054932
	protected override void ReceiveContentLength(int contentLength)
	{
		this._contentLength = contentLength;
	}

	// Token: 0x06000CAA RID: 3242 RVA: 0x0005673B File Offset: 0x0005493B
	protected override bool ReceiveData(byte[] data, int dataLength)
	{
		if (data == null || data.Length == 0)
		{
			return false;
		}
		this._received += dataLength;
		this._stream.Write(data, 0, dataLength);
		return true;
	}

	// Token: 0x06000CAB RID: 3243 RVA: 0x00056763 File Offset: 0x00054963
	protected override void CompleteContent()
	{
		this.CloseStream();
	}

	// Token: 0x06000CAC RID: 3244 RVA: 0x0005676B File Offset: 0x0005496B
	public new void Dispose()
	{
		this.CloseStream();
		base.Dispose();
	}

	// Token: 0x06000CAD RID: 3245 RVA: 0x00056779 File Offset: 0x00054979
	private void CloseStream()
	{
		if (this._stream != null)
		{
			this._stream.Dispose();
			this._stream = null;
		}
	}

	// Token: 0x04000897 RID: 2199
	private int _contentLength;

	// Token: 0x04000898 RID: 2200
	private int _received;

	// Token: 0x04000899 RID: 2201
	private FileStream _stream;
}
