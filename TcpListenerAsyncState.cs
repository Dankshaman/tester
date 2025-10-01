using System;
using System.Net.Sockets;
using System.Threading;

// Token: 0x020001C9 RID: 457
internal class TcpListenerAsyncState
{
	// Token: 0x0600183E RID: 6206 RVA: 0x000A5310 File Offset: 0x000A3510
	public TcpListenerAsyncState(TcpListener listener)
	{
		this.clientConnected = new ManualResetEvent(false);
		this.listener = listener;
	}

	// Token: 0x0600183F RID: 6207 RVA: 0x000A532B File Offset: 0x000A352B
	public static void AcceptTcpClientCallback(IAsyncResult ar)
	{
		TcpListenerAsyncState tcpListenerAsyncState = (TcpListenerAsyncState)ar.AsyncState;
		tcpListenerAsyncState.currentClient = tcpListenerAsyncState.listener.EndAcceptTcpClient(ar);
		tcpListenerAsyncState.clientConnected.Set();
	}

	// Token: 0x04000E80 RID: 3712
	public ManualResetEvent clientConnected;

	// Token: 0x04000E81 RID: 3713
	public TcpClient currentClient;

	// Token: 0x04000E82 RID: 3714
	public TcpListener listener;
}
