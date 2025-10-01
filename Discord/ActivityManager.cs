using System;
using System.Runtime.InteropServices;

namespace Discord
{
	// Token: 0x020004BC RID: 1212
	public class ActivityManager
	{
		// Token: 0x060035E6 RID: 13798 RVA: 0x00165D1B File Offset: 0x00163F1B
		public void RegisterCommand()
		{
			this.RegisterCommand(null);
		}

		// Token: 0x17000740 RID: 1856
		// (get) Token: 0x060035E7 RID: 13799 RVA: 0x00165D24 File Offset: 0x00163F24
		private ActivityManager.FFIMethods Methods
		{
			get
			{
				if (this.MethodsStructure == null)
				{
					this.MethodsStructure = Marshal.PtrToStructure(this.MethodsPtr, typeof(ActivityManager.FFIMethods));
				}
				return (ActivityManager.FFIMethods)this.MethodsStructure;
			}
		}

		// Token: 0x1400007C RID: 124
		// (add) Token: 0x060035E8 RID: 13800 RVA: 0x00165D54 File Offset: 0x00163F54
		// (remove) Token: 0x060035E9 RID: 13801 RVA: 0x00165D8C File Offset: 0x00163F8C
		public event ActivityManager.ActivityJoinHandler OnActivityJoin;

		// Token: 0x1400007D RID: 125
		// (add) Token: 0x060035EA RID: 13802 RVA: 0x00165DC4 File Offset: 0x00163FC4
		// (remove) Token: 0x060035EB RID: 13803 RVA: 0x00165DFC File Offset: 0x00163FFC
		public event ActivityManager.ActivitySpectateHandler OnActivitySpectate;

		// Token: 0x1400007E RID: 126
		// (add) Token: 0x060035EC RID: 13804 RVA: 0x00165E34 File Offset: 0x00164034
		// (remove) Token: 0x060035ED RID: 13805 RVA: 0x00165E6C File Offset: 0x0016406C
		public event ActivityManager.ActivityJoinRequestHandler OnActivityJoinRequest;

		// Token: 0x1400007F RID: 127
		// (add) Token: 0x060035EE RID: 13806 RVA: 0x00165EA4 File Offset: 0x001640A4
		// (remove) Token: 0x060035EF RID: 13807 RVA: 0x00165EDC File Offset: 0x001640DC
		public event ActivityManager.ActivityInviteHandler OnActivityInvite;

		// Token: 0x060035F0 RID: 13808 RVA: 0x00165F14 File Offset: 0x00164114
		internal ActivityManager(IntPtr ptr, IntPtr eventsPtr, ref ActivityManager.FFIEvents events)
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

		// Token: 0x060035F1 RID: 13809 RVA: 0x00165F64 File Offset: 0x00164164
		private void InitEvents(IntPtr eventsPtr, ref ActivityManager.FFIEvents events)
		{
			events.OnActivityJoin = new ActivityManager.FFIEvents.ActivityJoinHandler(ActivityManager.OnActivityJoinImpl);
			events.OnActivitySpectate = new ActivityManager.FFIEvents.ActivitySpectateHandler(ActivityManager.OnActivitySpectateImpl);
			events.OnActivityJoinRequest = new ActivityManager.FFIEvents.ActivityJoinRequestHandler(ActivityManager.OnActivityJoinRequestImpl);
			events.OnActivityInvite = new ActivityManager.FFIEvents.ActivityInviteHandler(ActivityManager.OnActivityInviteImpl);
			Marshal.StructureToPtr<ActivityManager.FFIEvents>(events, eventsPtr, false);
		}

		// Token: 0x060035F2 RID: 13810 RVA: 0x00165FC8 File Offset: 0x001641C8
		public void RegisterCommand(string command)
		{
			Result result = this.Methods.RegisterCommand(this.MethodsPtr, command);
			if (result != Result.Ok)
			{
				throw new ResultException(result);
			}
		}

		// Token: 0x060035F3 RID: 13811 RVA: 0x00165FF8 File Offset: 0x001641F8
		public void RegisterSteam(uint steamId)
		{
			Result result = this.Methods.RegisterSteam(this.MethodsPtr, steamId);
			if (result != Result.Ok)
			{
				throw new ResultException(result);
			}
		}

		// Token: 0x060035F4 RID: 13812 RVA: 0x00166028 File Offset: 0x00164228
		[MonoPInvokeCallback]
		private static void UpdateActivityCallbackImpl(IntPtr ptr, Result result)
		{
			GCHandle gchandle = GCHandle.FromIntPtr(ptr);
			ActivityManager.UpdateActivityHandler updateActivityHandler = (ActivityManager.UpdateActivityHandler)gchandle.Target;
			gchandle.Free();
			updateActivityHandler(result);
		}

		// Token: 0x060035F5 RID: 13813 RVA: 0x00166058 File Offset: 0x00164258
		public void UpdateActivity(Activity activity, ActivityManager.UpdateActivityHandler callback)
		{
			GCHandle value = GCHandle.Alloc(callback);
			this.Methods.UpdateActivity(this.MethodsPtr, ref activity, GCHandle.ToIntPtr(value), new ActivityManager.FFIMethods.UpdateActivityCallback(ActivityManager.UpdateActivityCallbackImpl));
		}

		// Token: 0x060035F6 RID: 13814 RVA: 0x00166098 File Offset: 0x00164298
		[MonoPInvokeCallback]
		private static void ClearActivityCallbackImpl(IntPtr ptr, Result result)
		{
			GCHandle gchandle = GCHandle.FromIntPtr(ptr);
			ActivityManager.ClearActivityHandler clearActivityHandler = (ActivityManager.ClearActivityHandler)gchandle.Target;
			gchandle.Free();
			clearActivityHandler(result);
		}

		// Token: 0x060035F7 RID: 13815 RVA: 0x001660C8 File Offset: 0x001642C8
		public void ClearActivity(ActivityManager.ClearActivityHandler callback)
		{
			GCHandle value = GCHandle.Alloc(callback);
			this.Methods.ClearActivity(this.MethodsPtr, GCHandle.ToIntPtr(value), new ActivityManager.FFIMethods.ClearActivityCallback(ActivityManager.ClearActivityCallbackImpl));
		}

		// Token: 0x060035F8 RID: 13816 RVA: 0x00166104 File Offset: 0x00164304
		[MonoPInvokeCallback]
		private static void SendRequestReplyCallbackImpl(IntPtr ptr, Result result)
		{
			GCHandle gchandle = GCHandle.FromIntPtr(ptr);
			ActivityManager.SendRequestReplyHandler sendRequestReplyHandler = (ActivityManager.SendRequestReplyHandler)gchandle.Target;
			gchandle.Free();
			sendRequestReplyHandler(result);
		}

		// Token: 0x060035F9 RID: 13817 RVA: 0x00166134 File Offset: 0x00164334
		public void SendRequestReply(long userId, ActivityJoinRequestReply reply, ActivityManager.SendRequestReplyHandler callback)
		{
			GCHandle value = GCHandle.Alloc(callback);
			this.Methods.SendRequestReply(this.MethodsPtr, userId, reply, GCHandle.ToIntPtr(value), new ActivityManager.FFIMethods.SendRequestReplyCallback(ActivityManager.SendRequestReplyCallbackImpl));
		}

		// Token: 0x060035FA RID: 13818 RVA: 0x00166174 File Offset: 0x00164374
		[MonoPInvokeCallback]
		private static void SendInviteCallbackImpl(IntPtr ptr, Result result)
		{
			GCHandle gchandle = GCHandle.FromIntPtr(ptr);
			ActivityManager.SendInviteHandler sendInviteHandler = (ActivityManager.SendInviteHandler)gchandle.Target;
			gchandle.Free();
			sendInviteHandler(result);
		}

		// Token: 0x060035FB RID: 13819 RVA: 0x001661A4 File Offset: 0x001643A4
		public void SendInvite(long userId, ActivityActionType type, string content, ActivityManager.SendInviteHandler callback)
		{
			GCHandle value = GCHandle.Alloc(callback);
			this.Methods.SendInvite(this.MethodsPtr, userId, type, content, GCHandle.ToIntPtr(value), new ActivityManager.FFIMethods.SendInviteCallback(ActivityManager.SendInviteCallbackImpl));
		}

		// Token: 0x060035FC RID: 13820 RVA: 0x001661E4 File Offset: 0x001643E4
		[MonoPInvokeCallback]
		private static void AcceptInviteCallbackImpl(IntPtr ptr, Result result)
		{
			GCHandle gchandle = GCHandle.FromIntPtr(ptr);
			ActivityManager.AcceptInviteHandler acceptInviteHandler = (ActivityManager.AcceptInviteHandler)gchandle.Target;
			gchandle.Free();
			acceptInviteHandler(result);
		}

		// Token: 0x060035FD RID: 13821 RVA: 0x00166214 File Offset: 0x00164414
		public void AcceptInvite(long userId, ActivityManager.AcceptInviteHandler callback)
		{
			GCHandle value = GCHandle.Alloc(callback);
			this.Methods.AcceptInvite(this.MethodsPtr, userId, GCHandle.ToIntPtr(value), new ActivityManager.FFIMethods.AcceptInviteCallback(ActivityManager.AcceptInviteCallbackImpl));
		}

		// Token: 0x060035FE RID: 13822 RVA: 0x00166254 File Offset: 0x00164454
		[MonoPInvokeCallback]
		private static void OnActivityJoinImpl(IntPtr ptr, string secret)
		{
			Discord discord = (Discord)GCHandle.FromIntPtr(ptr).Target;
			if (discord.ActivityManagerInstance.OnActivityJoin != null)
			{
				discord.ActivityManagerInstance.OnActivityJoin(secret);
			}
		}

		// Token: 0x060035FF RID: 13823 RVA: 0x00166294 File Offset: 0x00164494
		[MonoPInvokeCallback]
		private static void OnActivitySpectateImpl(IntPtr ptr, string secret)
		{
			Discord discord = (Discord)GCHandle.FromIntPtr(ptr).Target;
			if (discord.ActivityManagerInstance.OnActivitySpectate != null)
			{
				discord.ActivityManagerInstance.OnActivitySpectate(secret);
			}
		}

		// Token: 0x06003600 RID: 13824 RVA: 0x001662D4 File Offset: 0x001644D4
		[MonoPInvokeCallback]
		private static void OnActivityJoinRequestImpl(IntPtr ptr, ref User user)
		{
			Discord discord = (Discord)GCHandle.FromIntPtr(ptr).Target;
			if (discord.ActivityManagerInstance.OnActivityJoinRequest != null)
			{
				discord.ActivityManagerInstance.OnActivityJoinRequest(ref user);
			}
		}

		// Token: 0x06003601 RID: 13825 RVA: 0x00166314 File Offset: 0x00164514
		[MonoPInvokeCallback]
		private static void OnActivityInviteImpl(IntPtr ptr, ActivityActionType type, ref User user, ref Activity activity)
		{
			Discord discord = (Discord)GCHandle.FromIntPtr(ptr).Target;
			if (discord.ActivityManagerInstance.OnActivityInvite != null)
			{
				discord.ActivityManagerInstance.OnActivityInvite(type, ref user, ref activity);
			}
		}

		// Token: 0x04002212 RID: 8722
		private IntPtr MethodsPtr;

		// Token: 0x04002213 RID: 8723
		private object MethodsStructure;

		// Token: 0x02000837 RID: 2103
		internal struct FFIEvents
		{
			// Token: 0x04002E9C RID: 11932
			internal ActivityManager.FFIEvents.ActivityJoinHandler OnActivityJoin;

			// Token: 0x04002E9D RID: 11933
			internal ActivityManager.FFIEvents.ActivitySpectateHandler OnActivitySpectate;

			// Token: 0x04002E9E RID: 11934
			internal ActivityManager.FFIEvents.ActivityJoinRequestHandler OnActivityJoinRequest;

			// Token: 0x04002E9F RID: 11935
			internal ActivityManager.FFIEvents.ActivityInviteHandler OnActivityInvite;

			// Token: 0x020008AE RID: 2222
			// (Invoke) Token: 0x0600427B RID: 17019
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate void ActivityJoinHandler(IntPtr ptr, [MarshalAs(UnmanagedType.LPStr)] string secret);

			// Token: 0x020008AF RID: 2223
			// (Invoke) Token: 0x0600427F RID: 17023
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate void ActivitySpectateHandler(IntPtr ptr, [MarshalAs(UnmanagedType.LPStr)] string secret);

			// Token: 0x020008B0 RID: 2224
			// (Invoke) Token: 0x06004283 RID: 17027
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate void ActivityJoinRequestHandler(IntPtr ptr, ref User user);

			// Token: 0x020008B1 RID: 2225
			// (Invoke) Token: 0x06004287 RID: 17031
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate void ActivityInviteHandler(IntPtr ptr, ActivityActionType type, ref User user, ref Activity activity);
		}

		// Token: 0x02000838 RID: 2104
		internal struct FFIMethods
		{
			// Token: 0x04002EA0 RID: 11936
			internal ActivityManager.FFIMethods.RegisterCommandMethod RegisterCommand;

			// Token: 0x04002EA1 RID: 11937
			internal ActivityManager.FFIMethods.RegisterSteamMethod RegisterSteam;

			// Token: 0x04002EA2 RID: 11938
			internal ActivityManager.FFIMethods.UpdateActivityMethod UpdateActivity;

			// Token: 0x04002EA3 RID: 11939
			internal ActivityManager.FFIMethods.ClearActivityMethod ClearActivity;

			// Token: 0x04002EA4 RID: 11940
			internal ActivityManager.FFIMethods.SendRequestReplyMethod SendRequestReply;

			// Token: 0x04002EA5 RID: 11941
			internal ActivityManager.FFIMethods.SendInviteMethod SendInvite;

			// Token: 0x04002EA6 RID: 11942
			internal ActivityManager.FFIMethods.AcceptInviteMethod AcceptInvite;

			// Token: 0x020008B2 RID: 2226
			// (Invoke) Token: 0x0600428B RID: 17035
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate Result RegisterCommandMethod(IntPtr methodsPtr, [MarshalAs(UnmanagedType.LPStr)] string command);

			// Token: 0x020008B3 RID: 2227
			// (Invoke) Token: 0x0600428F RID: 17039
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate Result RegisterSteamMethod(IntPtr methodsPtr, uint steamId);

			// Token: 0x020008B4 RID: 2228
			// (Invoke) Token: 0x06004293 RID: 17043
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate void UpdateActivityCallback(IntPtr ptr, Result result);

			// Token: 0x020008B5 RID: 2229
			// (Invoke) Token: 0x06004297 RID: 17047
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate void UpdateActivityMethod(IntPtr methodsPtr, ref Activity activity, IntPtr callbackData, ActivityManager.FFIMethods.UpdateActivityCallback callback);

			// Token: 0x020008B6 RID: 2230
			// (Invoke) Token: 0x0600429B RID: 17051
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate void ClearActivityCallback(IntPtr ptr, Result result);

			// Token: 0x020008B7 RID: 2231
			// (Invoke) Token: 0x0600429F RID: 17055
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate void ClearActivityMethod(IntPtr methodsPtr, IntPtr callbackData, ActivityManager.FFIMethods.ClearActivityCallback callback);

			// Token: 0x020008B8 RID: 2232
			// (Invoke) Token: 0x060042A3 RID: 17059
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate void SendRequestReplyCallback(IntPtr ptr, Result result);

			// Token: 0x020008B9 RID: 2233
			// (Invoke) Token: 0x060042A7 RID: 17063
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate void SendRequestReplyMethod(IntPtr methodsPtr, long userId, ActivityJoinRequestReply reply, IntPtr callbackData, ActivityManager.FFIMethods.SendRequestReplyCallback callback);

			// Token: 0x020008BA RID: 2234
			// (Invoke) Token: 0x060042AB RID: 17067
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate void SendInviteCallback(IntPtr ptr, Result result);

			// Token: 0x020008BB RID: 2235
			// (Invoke) Token: 0x060042AF RID: 17071
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate void SendInviteMethod(IntPtr methodsPtr, long userId, ActivityActionType type, [MarshalAs(UnmanagedType.LPStr)] string content, IntPtr callbackData, ActivityManager.FFIMethods.SendInviteCallback callback);

			// Token: 0x020008BC RID: 2236
			// (Invoke) Token: 0x060042B3 RID: 17075
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate void AcceptInviteCallback(IntPtr ptr, Result result);

			// Token: 0x020008BD RID: 2237
			// (Invoke) Token: 0x060042B7 RID: 17079
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate void AcceptInviteMethod(IntPtr methodsPtr, long userId, IntPtr callbackData, ActivityManager.FFIMethods.AcceptInviteCallback callback);
		}

		// Token: 0x02000839 RID: 2105
		// (Invoke) Token: 0x0600416E RID: 16750
		public delegate void UpdateActivityHandler(Result result);

		// Token: 0x0200083A RID: 2106
		// (Invoke) Token: 0x06004172 RID: 16754
		public delegate void ClearActivityHandler(Result result);

		// Token: 0x0200083B RID: 2107
		// (Invoke) Token: 0x06004176 RID: 16758
		public delegate void SendRequestReplyHandler(Result result);

		// Token: 0x0200083C RID: 2108
		// (Invoke) Token: 0x0600417A RID: 16762
		public delegate void SendInviteHandler(Result result);

		// Token: 0x0200083D RID: 2109
		// (Invoke) Token: 0x0600417E RID: 16766
		public delegate void AcceptInviteHandler(Result result);

		// Token: 0x0200083E RID: 2110
		// (Invoke) Token: 0x06004182 RID: 16770
		public delegate void ActivityJoinHandler(string secret);

		// Token: 0x0200083F RID: 2111
		// (Invoke) Token: 0x06004186 RID: 16774
		public delegate void ActivitySpectateHandler(string secret);

		// Token: 0x02000840 RID: 2112
		// (Invoke) Token: 0x0600418A RID: 16778
		public delegate void ActivityJoinRequestHandler(ref User user);

		// Token: 0x02000841 RID: 2113
		// (Invoke) Token: 0x0600418E RID: 16782
		public delegate void ActivityInviteHandler(ActivityActionType type, ref User user, ref Activity activity);
	}
}
