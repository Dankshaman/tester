using System;
using System.Net.Sockets;
using System.Text;

// Token: 0x02000168 RID: 360
public class LuaEditorPushJob : ThreadedJob
{
	// Token: 0x0600116C RID: 4460 RVA: 0x00078C08 File Offset: 0x00076E08
	protected override void ThreadFunction()
	{
		try
		{
			int port = 39998;
			TcpClient tcpClient = new TcpClient("127.0.0.1", port);
			byte[] bytes = Encoding.UTF8.GetBytes(this.message);
			NetworkStream stream = tcpClient.GetStream();
			stream.Write(bytes, 0, bytes.Length);
			stream.Close();
			tcpClient.Close();
		}
		catch (ArgumentNullException ex)
		{
			this.Ex = ex.Message;
		}
		catch (SocketException ex2)
		{
			this.Ex = ex2.Message;
		}
	}

	// Token: 0x0600116D RID: 4461 RVA: 0x000025B8 File Offset: 0x000007B8
	protected override void OnFinished()
	{
	}

	// Token: 0x0600116E RID: 4462 RVA: 0x00078B0E File Offset: 0x00076D0E
	public override void Reset()
	{
		base.Reset();
	}

	// Token: 0x04000B45 RID: 2885
	public string Ex;

	// Token: 0x04000B46 RID: 2886
	public string message;
}
