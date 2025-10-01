using System;
using System.Runtime.InteropServices;

namespace Discord
{
	// Token: 0x020004EE RID: 1262
	public class NetworkManager
	{
		// Token: 0x1700074A RID: 1866
		// (get) Token: 0x060036A0 RID: 13984 RVA: 0x00168A7E File Offset: 0x00166C7E
		private NetworkManager.FFIMethods Methods
		{
			get
			{
				if (this.MethodsStructure == null)
				{
					this.MethodsStructure = Marshal.PtrToStructure(this.MethodsPtr, typeof(NetworkManager.FFIMethods));
				}
				return (NetworkManager.FFIMethods)this.MethodsStructure;
			}
		}

		// Token: 0x1400008B RID: 139
		// (add) Token: 0x060036A1 RID: 13985 RVA: 0x00168AB0 File Offset: 0x00166CB0
		// (remove) Token: 0x060036A2 RID: 13986 RVA: 0x00168AE8 File Offset: 0x00166CE8
		public event NetworkManager.MessageHandler OnMessage;

		// Token: 0x1400008C RID: 140
		// (add) Token: 0x060036A3 RID: 13987 RVA: 0x00168B20 File Offset: 0x00166D20
		// (remove) Token: 0x060036A4 RID: 13988 RVA: 0x00168B58 File Offset: 0x00166D58
		public event NetworkManager.RouteUpdateHandler OnRouteUpdate;

		// Token: 0x060036A5 RID: 13989 RVA: 0x00168B90 File Offset: 0x00166D90
		internal NetworkManager(IntPtr ptr, IntPtr eventsPtr, ref NetworkManager.FFIEvents events)
		{
			if (eventsPtr == IntPtr.Zero)
			{
				throw new ResultException(Result.InternalError);
			}
			this.InitEvents(eventsPtr, ref events);
			this.MethodsPtr = ptr;
			if (this.MethodsPtr == IntPtr.Zero)
			{
				throw new ResultException(Result.InternalError);
			}
		}

		// Token: 0x060036A6 RID: 13990 RVA: 0x00168BDF File Offset: 0x00166DDF
		private void InitEvents(IntPtr eventsPtr, ref NetworkManager.FFIEvents events)
		{
			events.OnMessage = new NetworkManager.FFIEvents.MessageHandler(NetworkManager.OnMessageImpl);
			events.OnRouteUpdate = new NetworkManager.FFIEvents.RouteUpdateHandler(NetworkManager.OnRouteUpdateImpl);
			Marshal.StructureToPtr<NetworkManager.FFIEvents>(events, eventsPtr, false);
		}

		// Token: 0x060036A7 RID: 13991 RVA: 0x00168C14 File Offset: 0x00166E14
		public ulong GetPeerId()
		{
			ulong result = 0UL;
			this.Methods.GetPeerId(this.MethodsPtr, ref result);
			return result;
		}

		// Token: 0x060036A8 RID: 13992 RVA: 0x00168C40 File Offset: 0x00166E40
		public void Flush()
		{
			Result result = this.Methods.Flush(this.MethodsPtr);
			if (result != Result.Ok)
			{
				throw new ResultException(result);
			}
		}

		// Token: 0x060036A9 RID: 13993 RVA: 0x00168C70 File Offset: 0x00166E70
		public void OpenPeer(ulong peerId, string routeData)
		{
			Result result = this.Methods.OpenPeer(this.MethodsPtr, peerId, routeData);
			if (result != Result.Ok)
			{
				throw new ResultException(result);
			}
		}

		// Token: 0x060036AA RID: 13994 RVA: 0x00168CA0 File Offset: 0x00166EA0
		public void UpdatePeer(ulong peerId, string routeData)
		{
			Result result = this.Methods.UpdatePeer(this.MethodsPtr, peerId, routeData);
			if (result != Result.Ok)
			{
				throw new ResultException(result);
			}
		}

		// Token: 0x060036AB RID: 13995 RVA: 0x00168CD0 File Offset: 0x00166ED0
		public void ClosePeer(ulong peerId)
		{
			Result result = this.Methods.ClosePeer(this.MethodsPtr, peerId);
			if (result != Result.Ok)
			{
				throw new ResultException(result);
			}
		}

		// Token: 0x060036AC RID: 13996 RVA: 0x00168D00 File Offset: 0x00166F00
		public void OpenChannel(ulong peerId, byte channelId, bool reliable)
		{
			Result result = this.Methods.OpenChannel(this.MethodsPtr, peerId, channelId, reliable);
			if (result != Result.Ok)
			{
				throw new ResultException(result);
			}
		}

		// Token: 0x060036AD RID: 13997 RVA: 0x00168D34 File Offset: 0x00166F34
		public void CloseChannel(ulong peerId, byte channelId)
		{
			Result result = this.Methods.CloseChannel(this.MethodsPtr, peerId, channelId);
			if (result != Result.Ok)
			{
				throw new ResultException(result);
			}
		}

		// Token: 0x060036AE RID: 13998 RVA: 0x00168D64 File Offset: 0x00166F64
		public void SendMessage(ulong peerId, byte channelId, byte[] data)
		{
			Result result = this.Methods.SendMessage(this.MethodsPtr, peerId, channelId, data, data.Length);
			if (result != Result.Ok)
			{
				throw new ResultException(result);
			}
		}

		// Token: 0x060036AF RID: 13999 RVA: 0x00168D98 File Offset: 0x00166F98
		[MonoPInvokeCallback]
		private static void OnMessageImpl(IntPtr ptr, ulong peerId, byte channelId, IntPtr dataPtr, int dataLen)
		{
			Discord discord = (Discord)GCHandle.FromIntPtr(ptr).Target;
			if (discord.NetworkManagerInstance.OnMessage != null)
			{
				byte[] array = new byte[dataLen];
				Marshal.Copy(dataPtr, array, 0, dataLen);
				discord.NetworkManagerInstance.OnMessage(peerId, channelId, array);
			}
		}

		// Token: 0x060036B0 RID: 14000 RVA: 0x00168DEC File Offset: 0x00166FEC
		[MonoPInvokeCallback]
		private static void OnRouteUpdateImpl(IntPtr ptr, string routeData)
		{
			Discord discord = (Discord)GCHandle.FromIntPtr(ptr).Target;
			if (discord.NetworkManagerInstance.OnRouteUpdate != null)
			{
				discord.NetworkManagerInstance.OnRouteUpdate(routeData);
			}
		}

		// Token: 0x0400231B RID: 8987
		private IntPtr MethodsPtr;

		// Token: 0x0400231C RID: 8988
		private object MethodsStructure;

		// Token: 0x0200086F RID: 2159
		internal struct FFIEvents
		{
			// Token: 0x04002F1A RID: 12058
			internal NetworkManager.FFIEvents.MessageHandler OnMessage;

			// Token: 0x04002F1B RID: 12059
			internal NetworkManager.FFIEvents.RouteUpdateHandler OnRouteUpdate;

			// Token: 0x02000927 RID: 2343
			// (Invoke) Token: 0x0600445F RID: 17503
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate void MessageHandler(IntPtr ptr, ulong peerId, byte channelId, IntPtr dataPtr, int dataLen);

			// Token: 0x02000928 RID: 2344
			// (Invoke) Token: 0x06004463 RID: 17507
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate void RouteUpdateHandler(IntPtr ptr, [MarshalAs(UnmanagedType.LPStr)] string routeData);
		}

		// Token: 0x02000870 RID: 2160
		internal struct FFIMethods
		{
			// Token: 0x04002F1C RID: 12060
			internal NetworkManager.FFIMethods.GetPeerIdMethod GetPeerId;

			// Token: 0x04002F1D RID: 12061
			internal NetworkManager.FFIMethods.FlushMethod Flush;

			// Token: 0x04002F1E RID: 12062
			internal NetworkManager.FFIMethods.OpenPeerMethod OpenPeer;

			// Token: 0x04002F1F RID: 12063
			internal NetworkManager.FFIMethods.UpdatePeerMethod UpdatePeer;

			// Token: 0x04002F20 RID: 12064
			internal NetworkManager.FFIMethods.ClosePeerMethod ClosePeer;

			// Token: 0x04002F21 RID: 12065
			internal NetworkManager.FFIMethods.OpenChannelMethod OpenChannel;

			// Token: 0x04002F22 RID: 12066
			internal NetworkManager.FFIMethods.CloseChannelMethod CloseChannel;

			// Token: 0x04002F23 RID: 12067
			internal NetworkManager.FFIMethods.SendMessageMethod SendMessage;

			// Token: 0x02000929 RID: 2345
			// (Invoke) Token: 0x06004467 RID: 17511
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate void GetPeerIdMethod(IntPtr methodsPtr, ref ulong peerId);

			// Token: 0x0200092A RID: 2346
			// (Invoke) Token: 0x0600446B RID: 17515
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate Result FlushMethod(IntPtr methodsPtr);

			// Token: 0x0200092B RID: 2347
			// (Invoke) Token: 0x0600446F RID: 17519
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate Result OpenPeerMethod(IntPtr methodsPtr, ulong peerId, [MarshalAs(UnmanagedType.LPStr)] string routeData);

			// Token: 0x0200092C RID: 2348
			// (Invoke) Token: 0x06004473 RID: 17523
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate Result UpdatePeerMethod(IntPtr methodsPtr, ulong peerId, [MarshalAs(UnmanagedType.LPStr)] string routeData);

			// Token: 0x0200092D RID: 2349
			// (Invoke) Token: 0x06004477 RID: 17527
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate Result ClosePeerMethod(IntPtr methodsPtr, ulong peerId);

			// Token: 0x0200092E RID: 2350
			// (Invoke) Token: 0x0600447B RID: 17531
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate Result OpenChannelMethod(IntPtr methodsPtr, ulong peerId, byte channelId, bool reliable);

			// Token: 0x0200092F RID: 2351
			// (Invoke) Token: 0x0600447F RID: 17535
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate Result CloseChannelMethod(IntPtr methodsPtr, ulong peerId, byte channelId);

			// Token: 0x02000930 RID: 2352
			// (Invoke) Token: 0x06004483 RID: 17539
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate Result SendMessageMethod(IntPtr methodsPtr, ulong peerId, byte channelId, byte[] data, int dataLen);
		}

		// Token: 0x02000871 RID: 2161
		// (Invoke) Token: 0x06004206 RID: 16902
		public delegate void MessageHandler(ulong peerId, byte channelId, byte[] data);

		// Token: 0x02000872 RID: 2162
		// (Invoke) Token: 0x0600420A RID: 16906
		public delegate void RouteUpdateHandler(string routeData);
	}
}
