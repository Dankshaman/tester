using System;
using System.Runtime.InteropServices;

namespace Discord
{
	// Token: 0x020004EF RID: 1263
	public class OverlayManager
	{
		// Token: 0x1700074B RID: 1867
		// (get) Token: 0x060036B1 RID: 14001 RVA: 0x00168E2B File Offset: 0x0016702B
		private OverlayManager.FFIMethods Methods
		{
			get
			{
				if (this.MethodsStructure == null)
				{
					this.MethodsStructure = Marshal.PtrToStructure(this.MethodsPtr, typeof(OverlayManager.FFIMethods));
				}
				return (OverlayManager.FFIMethods)this.MethodsStructure;
			}
		}

		// Token: 0x1400008D RID: 141
		// (add) Token: 0x060036B2 RID: 14002 RVA: 0x00168E5C File Offset: 0x0016705C
		// (remove) Token: 0x060036B3 RID: 14003 RVA: 0x00168E94 File Offset: 0x00167094
		public event OverlayManager.ToggleHandler OnToggle;

		// Token: 0x060036B4 RID: 14004 RVA: 0x00168ECC File Offset: 0x001670CC
		internal OverlayManager(IntPtr ptr, IntPtr eventsPtr, ref OverlayManager.FFIEvents events)
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

		// Token: 0x060036B5 RID: 14005 RVA: 0x00168F1B File Offset: 0x0016711B
		private void InitEvents(IntPtr eventsPtr, ref OverlayManager.FFIEvents events)
		{
			events.OnToggle = new OverlayManager.FFIEvents.ToggleHandler(OverlayManager.OnToggleImpl);
			Marshal.StructureToPtr<OverlayManager.FFIEvents>(events, eventsPtr, false);
		}

		// Token: 0x060036B6 RID: 14006 RVA: 0x00168F3C File Offset: 0x0016713C
		public bool IsEnabled()
		{
			bool result = false;
			this.Methods.IsEnabled(this.MethodsPtr, ref result);
			return result;
		}

		// Token: 0x060036B7 RID: 14007 RVA: 0x00168F64 File Offset: 0x00167164
		public bool IsLocked()
		{
			bool result = false;
			this.Methods.IsLocked(this.MethodsPtr, ref result);
			return result;
		}

		// Token: 0x060036B8 RID: 14008 RVA: 0x00168F8C File Offset: 0x0016718C
		[MonoPInvokeCallback]
		private static void SetLockedCallbackImpl(IntPtr ptr, Result result)
		{
			GCHandle gchandle = GCHandle.FromIntPtr(ptr);
			OverlayManager.SetLockedHandler setLockedHandler = (OverlayManager.SetLockedHandler)gchandle.Target;
			gchandle.Free();
			setLockedHandler(result);
		}

		// Token: 0x060036B9 RID: 14009 RVA: 0x00168FBC File Offset: 0x001671BC
		public void SetLocked(bool locked, OverlayManager.SetLockedHandler callback)
		{
			GCHandle value = GCHandle.Alloc(callback);
			this.Methods.SetLocked(this.MethodsPtr, locked, GCHandle.ToIntPtr(value), new OverlayManager.FFIMethods.SetLockedCallback(OverlayManager.SetLockedCallbackImpl));
		}

		// Token: 0x060036BA RID: 14010 RVA: 0x00168FFC File Offset: 0x001671FC
		[MonoPInvokeCallback]
		private static void OpenActivityInviteCallbackImpl(IntPtr ptr, Result result)
		{
			GCHandle gchandle = GCHandle.FromIntPtr(ptr);
			OverlayManager.OpenActivityInviteHandler openActivityInviteHandler = (OverlayManager.OpenActivityInviteHandler)gchandle.Target;
			gchandle.Free();
			openActivityInviteHandler(result);
		}

		// Token: 0x060036BB RID: 14011 RVA: 0x0016902C File Offset: 0x0016722C
		public void OpenActivityInvite(ActivityActionType type, OverlayManager.OpenActivityInviteHandler callback)
		{
			GCHandle value = GCHandle.Alloc(callback);
			this.Methods.OpenActivityInvite(this.MethodsPtr, type, GCHandle.ToIntPtr(value), new OverlayManager.FFIMethods.OpenActivityInviteCallback(OverlayManager.OpenActivityInviteCallbackImpl));
		}

		// Token: 0x060036BC RID: 14012 RVA: 0x0016906C File Offset: 0x0016726C
		[MonoPInvokeCallback]
		private static void OpenGuildInviteCallbackImpl(IntPtr ptr, Result result)
		{
			GCHandle gchandle = GCHandle.FromIntPtr(ptr);
			OverlayManager.OpenGuildInviteHandler openGuildInviteHandler = (OverlayManager.OpenGuildInviteHandler)gchandle.Target;
			gchandle.Free();
			openGuildInviteHandler(result);
		}

		// Token: 0x060036BD RID: 14013 RVA: 0x0016909C File Offset: 0x0016729C
		public void OpenGuildInvite(string code, OverlayManager.OpenGuildInviteHandler callback)
		{
			GCHandle value = GCHandle.Alloc(callback);
			this.Methods.OpenGuildInvite(this.MethodsPtr, code, GCHandle.ToIntPtr(value), new OverlayManager.FFIMethods.OpenGuildInviteCallback(OverlayManager.OpenGuildInviteCallbackImpl));
		}

		// Token: 0x060036BE RID: 14014 RVA: 0x001690DC File Offset: 0x001672DC
		[MonoPInvokeCallback]
		private static void OpenVoiceSettingsCallbackImpl(IntPtr ptr, Result result)
		{
			GCHandle gchandle = GCHandle.FromIntPtr(ptr);
			OverlayManager.OpenVoiceSettingsHandler openVoiceSettingsHandler = (OverlayManager.OpenVoiceSettingsHandler)gchandle.Target;
			gchandle.Free();
			openVoiceSettingsHandler(result);
		}

		// Token: 0x060036BF RID: 14015 RVA: 0x0016910C File Offset: 0x0016730C
		public void OpenVoiceSettings(OverlayManager.OpenVoiceSettingsHandler callback)
		{
			GCHandle value = GCHandle.Alloc(callback);
			this.Methods.OpenVoiceSettings(this.MethodsPtr, GCHandle.ToIntPtr(value), new OverlayManager.FFIMethods.OpenVoiceSettingsCallback(OverlayManager.OpenVoiceSettingsCallbackImpl));
		}

		// Token: 0x060036C0 RID: 14016 RVA: 0x00169148 File Offset: 0x00167348
		[MonoPInvokeCallback]
		private static void OnToggleImpl(IntPtr ptr, bool locked)
		{
			Discord discord = (Discord)GCHandle.FromIntPtr(ptr).Target;
			if (discord.OverlayManagerInstance.OnToggle != null)
			{
				discord.OverlayManagerInstance.OnToggle(locked);
			}
		}

		// Token: 0x0400231F RID: 8991
		private IntPtr MethodsPtr;

		// Token: 0x04002320 RID: 8992
		private object MethodsStructure;

		// Token: 0x02000873 RID: 2163
		internal struct FFIEvents
		{
			// Token: 0x04002F24 RID: 12068
			internal OverlayManager.FFIEvents.ToggleHandler OnToggle;

			// Token: 0x02000931 RID: 2353
			// (Invoke) Token: 0x06004487 RID: 17543
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate void ToggleHandler(IntPtr ptr, bool locked);
		}

		// Token: 0x02000874 RID: 2164
		internal struct FFIMethods
		{
			// Token: 0x04002F25 RID: 12069
			internal OverlayManager.FFIMethods.IsEnabledMethod IsEnabled;

			// Token: 0x04002F26 RID: 12070
			internal OverlayManager.FFIMethods.IsLockedMethod IsLocked;

			// Token: 0x04002F27 RID: 12071
			internal OverlayManager.FFIMethods.SetLockedMethod SetLocked;

			// Token: 0x04002F28 RID: 12072
			internal OverlayManager.FFIMethods.OpenActivityInviteMethod OpenActivityInvite;

			// Token: 0x04002F29 RID: 12073
			internal OverlayManager.FFIMethods.OpenGuildInviteMethod OpenGuildInvite;

			// Token: 0x04002F2A RID: 12074
			internal OverlayManager.FFIMethods.OpenVoiceSettingsMethod OpenVoiceSettings;

			// Token: 0x02000932 RID: 2354
			// (Invoke) Token: 0x0600448B RID: 17547
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate void IsEnabledMethod(IntPtr methodsPtr, ref bool enabled);

			// Token: 0x02000933 RID: 2355
			// (Invoke) Token: 0x0600448F RID: 17551
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate void IsLockedMethod(IntPtr methodsPtr, ref bool locked);

			// Token: 0x02000934 RID: 2356
			// (Invoke) Token: 0x06004493 RID: 17555
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate void SetLockedCallback(IntPtr ptr, Result result);

			// Token: 0x02000935 RID: 2357
			// (Invoke) Token: 0x06004497 RID: 17559
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate void SetLockedMethod(IntPtr methodsPtr, bool locked, IntPtr callbackData, OverlayManager.FFIMethods.SetLockedCallback callback);

			// Token: 0x02000936 RID: 2358
			// (Invoke) Token: 0x0600449B RID: 17563
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate void OpenActivityInviteCallback(IntPtr ptr, Result result);

			// Token: 0x02000937 RID: 2359
			// (Invoke) Token: 0x0600449F RID: 17567
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate void OpenActivityInviteMethod(IntPtr methodsPtr, ActivityActionType type, IntPtr callbackData, OverlayManager.FFIMethods.OpenActivityInviteCallback callback);

			// Token: 0x02000938 RID: 2360
			// (Invoke) Token: 0x060044A3 RID: 17571
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate void OpenGuildInviteCallback(IntPtr ptr, Result result);

			// Token: 0x02000939 RID: 2361
			// (Invoke) Token: 0x060044A7 RID: 17575
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate void OpenGuildInviteMethod(IntPtr methodsPtr, [MarshalAs(UnmanagedType.LPStr)] string code, IntPtr callbackData, OverlayManager.FFIMethods.OpenGuildInviteCallback callback);

			// Token: 0x0200093A RID: 2362
			// (Invoke) Token: 0x060044AB RID: 17579
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate void OpenVoiceSettingsCallback(IntPtr ptr, Result result);

			// Token: 0x0200093B RID: 2363
			// (Invoke) Token: 0x060044AF RID: 17583
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate void OpenVoiceSettingsMethod(IntPtr methodsPtr, IntPtr callbackData, OverlayManager.FFIMethods.OpenVoiceSettingsCallback callback);
		}

		// Token: 0x02000875 RID: 2165
		// (Invoke) Token: 0x0600420E RID: 16910
		public delegate void SetLockedHandler(Result result);

		// Token: 0x02000876 RID: 2166
		// (Invoke) Token: 0x06004212 RID: 16914
		public delegate void OpenActivityInviteHandler(Result result);

		// Token: 0x02000877 RID: 2167
		// (Invoke) Token: 0x06004216 RID: 16918
		public delegate void OpenGuildInviteHandler(Result result);

		// Token: 0x02000878 RID: 2168
		// (Invoke) Token: 0x0600421A RID: 16922
		public delegate void OpenVoiceSettingsHandler(Result result);

		// Token: 0x02000879 RID: 2169
		// (Invoke) Token: 0x0600421E RID: 16926
		public delegate void ToggleHandler(bool locked);
	}
}
