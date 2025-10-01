using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using UnityEngine;

// Token: 0x0200015D RID: 349
public class LuaEditorJob : ThreadedJob
{
	// Token: 0x0600115E RID: 4446 RVA: 0x00078898 File Offset: 0x00076A98
	protected override void ThreadFunction()
	{
		try
		{
			int port = 39999;
			IPAddress localaddr = IPAddress.Parse("127.0.0.1");
			this.server = new TTSTcpListener(localaddr, port);
			this.server.Server.ReceiveTimeout = 3000;
			this.server.Start();
			byte[] array = new byte[10000000];
			string text = null;
			while (this.alive)
			{
				TcpClient tcpClient = this.server.AcceptTcpClient();
				tcpClient.Client.Blocking = true;
				text = "";
				NetworkStream stream = tcpClient.GetStream();
				DateTime now = DateTime.Now;
				DateTime d = now;
				SystemConsole.UserDebug(DebugType.External_API, "╲ Datastream started.");
				int count;
				while ((count = stream.Read(array, 0, array.Length)) != 0 && this.alive)
				{
					if ((DateTime.Now - d).TotalSeconds > 3.0)
					{
						text = Encoding.UTF8.GetString(array, 0, count);
						SystemConsole.UserDebug(DebugType.External_API, "/ Datastream timeout.");
						SystemConsole.UserDebug(DebugType.External_API, "╲ Datastream restarted.");
					}
					else
					{
						text += Encoding.UTF8.GetString(array, 0, count);
					}
					this.response = null;
					try
					{
						this.response = Json.Load<LuaEditorResponse>(text);
					}
					catch (Exception)
					{
						SystemConsole.UserDebug(DebugType.External_API, "▕ JSON incomplete, continuing...");
						continue;
					}
					SystemConsole.UserDebug(DebugType.External_API, "▕ JSON complete");
					this.messageID = this.response.messageID;
					if (this.messageID != LuaReceiveExternalMessageType.None)
					{
						while (!this.mainThreadDone)
						{
							SystemConsole.UserDebug(DebugType.External_API, "▕ Sleeping until main thread finishes");
							Thread.Sleep(10);
						}
						stream.Flush();
						text = "";
						this.ResetResponse();
						break;
					}
					if ((DateTime.Now - now).Milliseconds > 1000)
					{
						text = "";
						break;
					}
				}
				SystemConsole.UserDebug(DebugType.External_API, "╱ Datastream ended.");
				stream.Close();
				tcpClient.Close();
			}
		}
		catch (SocketException ex)
		{
			this.Ex = ex.Message;
		}
		finally
		{
			this.server.Stop();
		}
	}

	// Token: 0x0600115F RID: 4447 RVA: 0x00078AE0 File Offset: 0x00076CE0
	private void ResetResponse()
	{
		this.mainThreadDone = false;
		this.messageID = LuaReceiveExternalMessageType.None;
		this.response = null;
	}

	// Token: 0x06001160 RID: 4448 RVA: 0x00078AF7 File Offset: 0x00076CF7
	protected override void OnFinished()
	{
		Debug.LogError("LuaEditorJob terminated.");
		Debug.LogError(this.Ex);
	}

	// Token: 0x06001161 RID: 4449 RVA: 0x00078B0E File Offset: 0x00076D0E
	public override void Reset()
	{
		base.Reset();
	}

	// Token: 0x04000B26 RID: 2854
	public bool received;

	// Token: 0x04000B27 RID: 2855
	public string Ex;

	// Token: 0x04000B28 RID: 2856
	public LuaReceiveExternalMessageType messageID = LuaReceiveExternalMessageType.None;

	// Token: 0x04000B29 RID: 2857
	public bool mainThreadDone;

	// Token: 0x04000B2A RID: 2858
	public LuaEditorResponse response;

	// Token: 0x04000B2B RID: 2859
	public TcpListener server;

	// Token: 0x04000B2C RID: 2860
	public bool alive = true;

	// Token: 0x04000B2D RID: 2861
	public const float CONNECTION_TIMEOUT = 3f;
}
