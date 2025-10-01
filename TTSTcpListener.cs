using System;
using System.Net;
using System.Net.Sockets;

// Token: 0x020001CA RID: 458
internal class TTSTcpListener : TcpListener
{
	// Token: 0x06001840 RID: 6208 RVA: 0x000A5355 File Offset: 0x000A3555
	public TTSTcpListener(IPAddress localaddr, int port) : base(localaddr, port)
	{
	}

	// Token: 0x06001841 RID: 6209 RVA: 0x000A535F File Offset: 0x000A355F
	public TTSTcpListener(IPEndPoint localEP) : base(localEP)
	{
	}

	// Token: 0x06001842 RID: 6210 RVA: 0x000A5368 File Offset: 0x000A3568
	public new TcpClient AcceptTcpClient()
	{
		if (Environment.OSVersion.Platform != PlatformID.Unix)
		{
			return base.AcceptTcpClient();
		}
		TcpListenerAsyncState tcpListenerAsyncState = new TcpListenerAsyncState(this);
		AsyncCallback callback = new AsyncCallback(TcpListenerAsyncState.AcceptTcpClientCallback);
		base.BeginAcceptTcpClient(callback, tcpListenerAsyncState);
		tcpListenerAsyncState.clientConnected.WaitOne();
		return tcpListenerAsyncState.currentClient;
	}
}
