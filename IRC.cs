using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using UnityEngine;

// Token: 0x0200012F RID: 303
[Serializable]
public class IRC : IDisposable
{
	// Token: 0x170002EA RID: 746
	// (get) Token: 0x0600100D RID: 4109 RVA: 0x0006D131 File Offset: 0x0006B331
	// (set) Token: 0x0600100E RID: 4110 RVA: 0x0006D139 File Offset: 0x0006B339
	public string[] users { get; set; }

	// Token: 0x170002EB RID: 747
	// (get) Token: 0x0600100F RID: 4111 RVA: 0x0006D142 File Offset: 0x0006B342
	// (set) Token: 0x06001010 RID: 4112 RVA: 0x0006D14A File Offset: 0x0006B34A
	[HideInInspector]
	public bool success { get; set; }

	// Token: 0x06001011 RID: 4113 RVA: 0x0006D154 File Offset: 0x0006B354
	public void Start()
	{
		this.users = new string[0];
		if (this.started)
		{
			return;
		}
		this.started = true;
		Debug.Log("Irc Started");
		if (this.Trial && Application.platform != RuntimePlatform.WindowsEditor && Application.platform != RuntimePlatform.OSXEditor)
		{
			this.OnMessage("Disconnected:This is trial version you can use it only in Editor, Please donate 4$ to dorumonstr@gmail.com and i send you full open source");
			return;
		}
		this._thread = new Thread(new ThreadStart(this.StartThread));
		this._thread.IsBackground = true;
		this._thread.Start();
	}

	// Token: 0x06001012 RID: 4114 RVA: 0x0006D1D8 File Offset: 0x0006B3D8
	private void Connect()
	{
		this.OnMessage("Connecting");
		if (this.enableSOCKS5)
		{
			this._Socket = Proxy.Socks5Connect(this.SOCKS5Server, this.SOCKS5Port, this.host, this.port);
		}
		else
		{
			this._Socket = new TcpClient(this.host, this.port);
		}
		this._NetworkStream = this._Socket.GetStream();
		this.sw = new StreamWriter(this._NetworkStream, Encoding.Default);
		this.sr = new StreamReader(this._NetworkStream, Encoding.Default);
		this.sw.AutoFlush = true;
	}

	// Token: 0x06001013 RID: 4115 RVA: 0x0006D27D File Offset: 0x0006B47D
	public void Dispose()
	{
		if (this._Socket != null)
		{
			this._Socket.Close();
		}
		if (this._thread != null)
		{
			this._thread.Abort();
		}
	}

	// Token: 0x06001014 RID: 4116 RVA: 0x0006D2A8 File Offset: 0x0006B4A8
	public void SendIrcMessage(string s)
	{
		if (this.sw != null)
		{
			this.OnMessage(this.ircNick + ": " + s);
			this.sw.WriteLine("PRIVMSG #" + this.channel + " :" + s);
			return;
		}
		this.OnMessage("Not Connected");
	}

	// Token: 0x06001015 RID: 4117 RVA: 0x0006D304 File Offset: 0x0006B504
	public void SendIrcPrivateMessage(string u, string s)
	{
		if (this.sw != null)
		{
			this.OnPmMessage(u, this.ircNick + ": " + s);
			this.sw.WriteLine("PRIVMSG " + u + " :" + s);
			return;
		}
		this.OnPmMessage(u, "Not Connected");
	}

	// Token: 0x06001016 RID: 4118 RVA: 0x0006D35C File Offset: 0x0006B55C
	private void StartThread()
	{
		try
		{
			this.Connect();
			this.Write(string.Format("NICK {0}", this.ircNick));
			this.Write(string.Concat(new string[]
			{
				"USER ",
				this.user,
				" ",
				this.user2,
				" server :",
				this.about
			}));
			for (;;)
			{
				string text = this.sr.ReadLine();
				if (this.enableLog)
				{
					Debug.Log(text);
				}
				if (text == null || text == "")
				{
					break;
				}
				if (Regex.Match(text, ":.+? 005").Success && !this.success)
				{
					this.Write("JOIN #" + this.channel);
					this.OnMessage("Connected");
					this.controller.OnConnected();
					this.success = true;
				}
				Match m;
				if ((m = Regex.Match(text, "PING \\:(\\w+)", RegexOptions.IgnoreCase)).Success)
				{
					this.Write("PONG :" + m.Groups[1]);
				}
				if ((m = Regex.Match(text, "\\:.+? 353 .+? = #(.+?) \\:(.+)")).Success)
				{
					this.users = this.users.Union(from a in m.Groups[2].Value.Trim().Split(new char[]
					{
						' '
					})
					select a.Trim(new char[]
					{
						'@'
					})).ToArray<string>();
					this.success = true;
				}
				if ((m = Regex.Match(text, "\\:(.+?)!.*? PRIVMSG " + this.ircNick.Trim(new char[]
				{
					'@'
				}) + " \\:(.*)")).Success)
				{
					this.OnPmMessage(m.Groups[1].Value, m.Groups[1].Value + ": " + m.Groups[2].Value);
				}
				else if ((m = Regex.Match(text, "\\:(.+?)!.*? PRIVMSG .*? \\:(.*)")).Success)
				{
					this.OnMessage(m.Groups[1].Value + ": " + m.Groups[2].Value);
				}
				if ((m = Regex.Match(text, "\\:(.+?)!.*? PART")).Success)
				{
					this.users = (from a in this.users
					where !a.Contains(m.Groups[1].Value)
					select a).ToArray<string>();
					this.OnMessage(m.Groups[1].Value + " leaved");
				}
				if ((m = Regex.Match(text, "\\:(.+?)!.*? JOIN")).Success)
				{
					this.users = this.users.Union(new string[]
					{
						m.Groups[1].Value
					}).ToArray<string>();
					this.OnMessage(m.Groups[1].Value + " joined");
				}
			}
			throw new Exception("string is null");
		}
		catch (Exception ex)
		{
			Debug.Log(ex);
			this.OnMessage("Disconnected:" + ex);
		}
	}

	// Token: 0x06001017 RID: 4119 RVA: 0x0006D714 File Offset: 0x0006B914
	public void Update()
	{
		if (this.actions.Count > 0)
		{
			this.actions.Dequeue()();
		}
	}

	// Token: 0x06001018 RID: 4120 RVA: 0x0006D734 File Offset: 0x0006B934
	private void OnMessage(string m)
	{
		this.actions.Enqueue(delegate
		{
			if (this.controller != null)
			{
				this.controller.OnMessage(m);
			}
		});
	}

	// Token: 0x06001019 RID: 4121 RVA: 0x0006D76C File Offset: 0x0006B96C
	private void OnPmMessage(string u, string s)
	{
		this.actions.Enqueue(delegate
		{
			if (this.controller != null)
			{
				this.controller.OnPm(u, s);
			}
		});
	}

	// Token: 0x0600101A RID: 4122 RVA: 0x0006D7AB File Offset: 0x0006B9AB
	private void Write(string s)
	{
		if (this.enableLog)
		{
			Debug.Log(s);
		}
		this.sw.WriteLine(s);
	}

	// Token: 0x040009EE RID: 2542
	private bool Trial = true;

	// Token: 0x040009EF RID: 2543
	public bool enableLog;

	// Token: 0x040009F2 RID: 2546
	[HideInInspector]
	public string user = "UnityIrcChat";

	// Token: 0x040009F3 RID: 2547
	[HideInInspector]
	public string user2 = "localhost";

	// Token: 0x040009F4 RID: 2548
	[HideInInspector]
	public string about = "about";

	// Token: 0x040009F5 RID: 2549
	public string host = "irc.freenode.net";

	// Token: 0x040009F6 RID: 2550
	public int port = 6667;

	// Token: 0x040009F7 RID: 2551
	public bool enableSOCKS5;

	// Token: 0x040009F8 RID: 2552
	public string SOCKS5Server = "127.0.0.1";

	// Token: 0x040009F9 RID: 2553
	public int SOCKS5Port = 1080;

	// Token: 0x040009FA RID: 2554
	public iIrc controller;

	// Token: 0x040009FB RID: 2555
	private TcpClient _Socket;

	// Token: 0x040009FC RID: 2556
	private Stream _NetworkStream;

	// Token: 0x040009FD RID: 2557
	private StreamWriter sw;

	// Token: 0x040009FE RID: 2558
	private StreamReader sr;

	// Token: 0x040009FF RID: 2559
	private Queue<Action> actions = new Queue<Action>();

	// Token: 0x04000A00 RID: 2560
	public Thread _thread;

	// Token: 0x04000A01 RID: 2561
	public string ircNick = "MyIrcNickName1";

	// Token: 0x04000A02 RID: 2562
	public string channel = "unity3d";

	// Token: 0x04000A03 RID: 2563
	private bool started;
}
