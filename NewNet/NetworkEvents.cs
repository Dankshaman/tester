using System;
using UnityEngine;

namespace NewNet
{
	// Token: 0x020003A8 RID: 936
	public static class NetworkEvents
	{
		// Token: 0x14000068 RID: 104
		// (add) Token: 0x06002BFE RID: 11262 RVA: 0x00134E20 File Offset: 0x00133020
		// (remove) Token: 0x06002BFF RID: 11263 RVA: 0x00134E54 File Offset: 0x00133054
		public static event NetworkEvents.ServerInitializing OnServerInitializing;

		// Token: 0x06002C00 RID: 11264 RVA: 0x00134E88 File Offset: 0x00133088
		internal static void TriggerServerInitializing()
		{
			if (NetworkEvents.OnServerInitializing != null)
			{
				foreach (NetworkEvents.ServerInitializing serverInitializing in NetworkEvents.OnServerInitializing.GetInvocationList())
				{
					try
					{
						serverInitializing();
					}
					catch (Exception exception)
					{
						Debug.LogException(exception);
					}
				}
			}
		}

		// Token: 0x14000069 RID: 105
		// (add) Token: 0x06002C01 RID: 11265 RVA: 0x00134EE0 File Offset: 0x001330E0
		// (remove) Token: 0x06002C02 RID: 11266 RVA: 0x00134F14 File Offset: 0x00133114
		public static event NetworkEvents.ServerInitialized OnServerInitialized;

		// Token: 0x06002C03 RID: 11267 RVA: 0x00134F48 File Offset: 0x00133148
		internal static void TriggerServerInitialized()
		{
			if (NetworkEvents.OnServerInitialized != null)
			{
				foreach (NetworkEvents.ServerInitialized serverInitialized in NetworkEvents.OnServerInitialized.GetInvocationList())
				{
					try
					{
						serverInitialized();
					}
					catch (Exception exception)
					{
						Debug.LogException(exception);
					}
				}
			}
		}

		// Token: 0x1400006A RID: 106
		// (add) Token: 0x06002C04 RID: 11268 RVA: 0x00134FA0 File Offset: 0x001331A0
		// (remove) Token: 0x06002C05 RID: 11269 RVA: 0x00134FD4 File Offset: 0x001331D4
		public static event NetworkEvents.ConnectingToServer OnConnectingToServer;

		// Token: 0x06002C06 RID: 11270 RVA: 0x00135008 File Offset: 0x00133208
		internal static void TriggerConnectingToServer()
		{
			if (NetworkEvents.OnConnectingToServer != null)
			{
				foreach (NetworkEvents.ConnectingToServer connectingToServer in NetworkEvents.OnConnectingToServer.GetInvocationList())
				{
					try
					{
						connectingToServer();
					}
					catch (Exception exception)
					{
						Debug.LogException(exception);
					}
				}
			}
		}

		// Token: 0x1400006B RID: 107
		// (add) Token: 0x06002C07 RID: 11271 RVA: 0x00135060 File Offset: 0x00133260
		// (remove) Token: 0x06002C08 RID: 11272 RVA: 0x00135094 File Offset: 0x00133294
		public static event NetworkEvents.ConnectedToServer OnConnectedToServer;

		// Token: 0x06002C09 RID: 11273 RVA: 0x001350C8 File Offset: 0x001332C8
		internal static void TriggerConnectedToServer()
		{
			if (NetworkEvents.OnConnectedToServer != null)
			{
				foreach (NetworkEvents.ConnectedToServer connectedToServer in NetworkEvents.OnConnectedToServer.GetInvocationList())
				{
					try
					{
						connectedToServer();
					}
					catch (Exception exception)
					{
						Debug.LogException(exception);
					}
				}
			}
		}

		// Token: 0x1400006C RID: 108
		// (add) Token: 0x06002C0A RID: 11274 RVA: 0x00135120 File Offset: 0x00133320
		// (remove) Token: 0x06002C0B RID: 11275 RVA: 0x00135154 File Offset: 0x00133354
		public static event NetworkEvents.FailedToConnect OnFailedToConnect;

		// Token: 0x06002C0C RID: 11276 RVA: 0x00135188 File Offset: 0x00133388
		internal static void TriggerFailedToConnect(ConnectFailedInfo info)
		{
			if (NetworkEvents.OnFailedToConnect != null)
			{
				foreach (NetworkEvents.FailedToConnect failedToConnect in NetworkEvents.OnFailedToConnect.GetInvocationList())
				{
					try
					{
						failedToConnect(info);
					}
					catch (Exception exception)
					{
						Debug.LogException(exception);
					}
				}
			}
		}

		// Token: 0x1400006D RID: 109
		// (add) Token: 0x06002C0D RID: 11277 RVA: 0x001351E0 File Offset: 0x001333E0
		// (remove) Token: 0x06002C0E RID: 11278 RVA: 0x00135214 File Offset: 0x00133414
		public static event NetworkEvents.DisconnectedFromServer OnDisconnectedFromServer;

		// Token: 0x06002C0F RID: 11279 RVA: 0x00135248 File Offset: 0x00133448
		internal static void TriggerDisconnectedFromServer(DisconnectInfo info)
		{
			if (NetworkEvents.OnDisconnectedFromServer != null)
			{
				foreach (NetworkEvents.DisconnectedFromServer disconnectedFromServer in NetworkEvents.OnDisconnectedFromServer.GetInvocationList())
				{
					try
					{
						disconnectedFromServer(info);
					}
					catch (Exception exception)
					{
						Debug.LogException(exception);
					}
				}
			}
		}

		// Token: 0x1400006E RID: 110
		// (add) Token: 0x06002C10 RID: 11280 RVA: 0x001352A0 File Offset: 0x001334A0
		// (remove) Token: 0x06002C11 RID: 11281 RVA: 0x001352D4 File Offset: 0x001334D4
		public static event NetworkEvents.Connecting OnConnecting;

		// Token: 0x06002C12 RID: 11282 RVA: 0x00135308 File Offset: 0x00133508
		internal static void TriggerConnecting()
		{
			if (NetworkEvents.OnConnecting != null)
			{
				foreach (NetworkEvents.Connecting connecting in NetworkEvents.OnConnecting.GetInvocationList())
				{
					try
					{
						connecting();
					}
					catch (Exception exception)
					{
						Debug.LogException(exception);
					}
				}
			}
		}

		// Token: 0x1400006F RID: 111
		// (add) Token: 0x06002C13 RID: 11283 RVA: 0x00135360 File Offset: 0x00133560
		// (remove) Token: 0x06002C14 RID: 11284 RVA: 0x00135394 File Offset: 0x00133594
		public static event NetworkEvents.Connected OnConnected;

		// Token: 0x06002C15 RID: 11285 RVA: 0x001353C8 File Offset: 0x001335C8
		internal static void TriggerConnected()
		{
			if (NetworkEvents.OnConnected != null)
			{
				foreach (NetworkEvents.Connected connected in NetworkEvents.OnConnected.GetInvocationList())
				{
					try
					{
						connected();
					}
					catch (Exception exception)
					{
						Debug.LogException(exception);
					}
				}
			}
		}

		// Token: 0x14000070 RID: 112
		// (add) Token: 0x06002C16 RID: 11286 RVA: 0x00135420 File Offset: 0x00133620
		// (remove) Token: 0x06002C17 RID: 11287 RVA: 0x00135454 File Offset: 0x00133654
		public static event NetworkEvents.PlayerConnected OnPlayerConnected;

		// Token: 0x06002C18 RID: 11288 RVA: 0x00135488 File Offset: 0x00133688
		internal static void TriggerPlayerConnected(NetworkPlayer player)
		{
			if (NetworkEvents.OnPlayerConnected != null)
			{
				foreach (NetworkEvents.PlayerConnected playerConnected in NetworkEvents.OnPlayerConnected.GetInvocationList())
				{
					try
					{
						playerConnected(player);
					}
					catch (Exception exception)
					{
						Debug.LogException(exception);
					}
				}
			}
		}

		// Token: 0x14000071 RID: 113
		// (add) Token: 0x06002C19 RID: 11289 RVA: 0x001354E0 File Offset: 0x001336E0
		// (remove) Token: 0x06002C1A RID: 11290 RVA: 0x00135514 File Offset: 0x00133714
		public static event NetworkEvents.PlayerDisconnected OnPlayerDisconnected;

		// Token: 0x06002C1B RID: 11291 RVA: 0x00135548 File Offset: 0x00133748
		internal static void TriggerPlayerDisconnect(NetworkPlayer player, DisconnectInfo info)
		{
			if (NetworkEvents.OnPlayerDisconnected != null)
			{
				foreach (NetworkEvents.PlayerDisconnected playerDisconnected in NetworkEvents.OnPlayerDisconnected.GetInvocationList())
				{
					try
					{
						playerDisconnected(player, info);
					}
					catch (Exception exception)
					{
						Debug.LogException(exception);
					}
				}
			}
		}

		// Token: 0x14000072 RID: 114
		// (add) Token: 0x06002C1C RID: 11292 RVA: 0x001355A0 File Offset: 0x001337A0
		// (remove) Token: 0x06002C1D RID: 11293 RVA: 0x001355D4 File Offset: 0x001337D4
		public static event NetworkEvents.SettingsChange OnSettingsChange;

		// Token: 0x06002C1E RID: 11294 RVA: 0x00135608 File Offset: 0x00133808
		internal static void TriggerSettingsChange()
		{
			if (NetworkEvents.OnSettingsChange != null)
			{
				foreach (NetworkEvents.SettingsChange settingsChange in NetworkEvents.OnSettingsChange.GetInvocationList())
				{
					try
					{
						settingsChange();
					}
					catch (Exception exception)
					{
						Debug.LogException(exception);
					}
				}
			}
		}

		// Token: 0x020007CD RID: 1997
		// (Invoke) Token: 0x06003FDB RID: 16347
		public delegate void ServerInitializing();

		// Token: 0x020007CE RID: 1998
		// (Invoke) Token: 0x06003FDF RID: 16351
		public delegate void ServerInitialized();

		// Token: 0x020007CF RID: 1999
		// (Invoke) Token: 0x06003FE3 RID: 16355
		public delegate void ConnectingToServer();

		// Token: 0x020007D0 RID: 2000
		// (Invoke) Token: 0x06003FE7 RID: 16359
		public delegate void ConnectedToServer();

		// Token: 0x020007D1 RID: 2001
		// (Invoke) Token: 0x06003FEB RID: 16363
		public delegate void FailedToConnect(ConnectFailedInfo info);

		// Token: 0x020007D2 RID: 2002
		// (Invoke) Token: 0x06003FEF RID: 16367
		public delegate void DisconnectedFromServer(DisconnectInfo info);

		// Token: 0x020007D3 RID: 2003
		// (Invoke) Token: 0x06003FF3 RID: 16371
		public delegate void Connecting();

		// Token: 0x020007D4 RID: 2004
		// (Invoke) Token: 0x06003FF7 RID: 16375
		public delegate void Connected();

		// Token: 0x020007D5 RID: 2005
		// (Invoke) Token: 0x06003FFB RID: 16379
		public delegate void PlayerConnected(NetworkPlayer player);

		// Token: 0x020007D6 RID: 2006
		// (Invoke) Token: 0x06003FFF RID: 16383
		public delegate void PlayerDisconnected(NetworkPlayer player, DisconnectInfo info);

		// Token: 0x020007D7 RID: 2007
		// (Invoke) Token: 0x06004003 RID: 16387
		public delegate void SettingsChange();
	}
}
