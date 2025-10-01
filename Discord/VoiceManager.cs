using System;
using System.Runtime.InteropServices;

namespace Discord
{
	// Token: 0x020004F2 RID: 1266
	public class VoiceManager
	{
		// Token: 0x1700074E RID: 1870
		// (get) Token: 0x060036EB RID: 14059 RVA: 0x00169AA8 File Offset: 0x00167CA8
		private VoiceManager.FFIMethods Methods
		{
			get
			{
				if (this.MethodsStructure == null)
				{
					this.MethodsStructure = Marshal.PtrToStructure(this.MethodsPtr, typeof(VoiceManager.FFIMethods));
				}
				return (VoiceManager.FFIMethods)this.MethodsStructure;
			}
		}

		// Token: 0x14000090 RID: 144
		// (add) Token: 0x060036EC RID: 14060 RVA: 0x00169AD8 File Offset: 0x00167CD8
		// (remove) Token: 0x060036ED RID: 14061 RVA: 0x00169B10 File Offset: 0x00167D10
		public event VoiceManager.SettingsUpdateHandler OnSettingsUpdate;

		// Token: 0x060036EE RID: 14062 RVA: 0x00169B48 File Offset: 0x00167D48
		internal VoiceManager(IntPtr ptr, IntPtr eventsPtr, ref VoiceManager.FFIEvents events)
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

		// Token: 0x060036EF RID: 14063 RVA: 0x00169B97 File Offset: 0x00167D97
		private void InitEvents(IntPtr eventsPtr, ref VoiceManager.FFIEvents events)
		{
			events.OnSettingsUpdate = new VoiceManager.FFIEvents.SettingsUpdateHandler(VoiceManager.OnSettingsUpdateImpl);
			Marshal.StructureToPtr<VoiceManager.FFIEvents>(events, eventsPtr, false);
		}

		// Token: 0x060036F0 RID: 14064 RVA: 0x00169BB8 File Offset: 0x00167DB8
		public InputMode GetInputMode()
		{
			InputMode result = default(InputMode);
			Result result2 = this.Methods.GetInputMode(this.MethodsPtr, ref result);
			if (result2 != Result.Ok)
			{
				throw new ResultException(result2);
			}
			return result;
		}

		// Token: 0x060036F1 RID: 14065 RVA: 0x00169BF4 File Offset: 0x00167DF4
		[MonoPInvokeCallback]
		private static void SetInputModeCallbackImpl(IntPtr ptr, Result result)
		{
			GCHandle gchandle = GCHandle.FromIntPtr(ptr);
			VoiceManager.SetInputModeHandler setInputModeHandler = (VoiceManager.SetInputModeHandler)gchandle.Target;
			gchandle.Free();
			setInputModeHandler(result);
		}

		// Token: 0x060036F2 RID: 14066 RVA: 0x00169C24 File Offset: 0x00167E24
		public void SetInputMode(InputMode inputMode, VoiceManager.SetInputModeHandler callback)
		{
			GCHandle value = GCHandle.Alloc(callback);
			this.Methods.SetInputMode(this.MethodsPtr, inputMode, GCHandle.ToIntPtr(value), new VoiceManager.FFIMethods.SetInputModeCallback(VoiceManager.SetInputModeCallbackImpl));
		}

		// Token: 0x060036F3 RID: 14067 RVA: 0x00169C64 File Offset: 0x00167E64
		public bool IsSelfMute()
		{
			bool result = false;
			Result result2 = this.Methods.IsSelfMute(this.MethodsPtr, ref result);
			if (result2 != Result.Ok)
			{
				throw new ResultException(result2);
			}
			return result;
		}

		// Token: 0x060036F4 RID: 14068 RVA: 0x00169C98 File Offset: 0x00167E98
		public void SetSelfMute(bool mute)
		{
			Result result = this.Methods.SetSelfMute(this.MethodsPtr, mute);
			if (result != Result.Ok)
			{
				throw new ResultException(result);
			}
		}

		// Token: 0x060036F5 RID: 14069 RVA: 0x00169CC8 File Offset: 0x00167EC8
		public bool IsSelfDeaf()
		{
			bool result = false;
			Result result2 = this.Methods.IsSelfDeaf(this.MethodsPtr, ref result);
			if (result2 != Result.Ok)
			{
				throw new ResultException(result2);
			}
			return result;
		}

		// Token: 0x060036F6 RID: 14070 RVA: 0x00169CFC File Offset: 0x00167EFC
		public void SetSelfDeaf(bool deaf)
		{
			Result result = this.Methods.SetSelfDeaf(this.MethodsPtr, deaf);
			if (result != Result.Ok)
			{
				throw new ResultException(result);
			}
		}

		// Token: 0x060036F7 RID: 14071 RVA: 0x00169D2C File Offset: 0x00167F2C
		public bool IsLocalMute(long userId)
		{
			bool result = false;
			Result result2 = this.Methods.IsLocalMute(this.MethodsPtr, userId, ref result);
			if (result2 != Result.Ok)
			{
				throw new ResultException(result2);
			}
			return result;
		}

		// Token: 0x060036F8 RID: 14072 RVA: 0x00169D60 File Offset: 0x00167F60
		public void SetLocalMute(long userId, bool mute)
		{
			Result result = this.Methods.SetLocalMute(this.MethodsPtr, userId, mute);
			if (result != Result.Ok)
			{
				throw new ResultException(result);
			}
		}

		// Token: 0x060036F9 RID: 14073 RVA: 0x00169D90 File Offset: 0x00167F90
		public byte GetLocalVolume(long userId)
		{
			byte result = 0;
			Result result2 = this.Methods.GetLocalVolume(this.MethodsPtr, userId, ref result);
			if (result2 != Result.Ok)
			{
				throw new ResultException(result2);
			}
			return result;
		}

		// Token: 0x060036FA RID: 14074 RVA: 0x00169DC4 File Offset: 0x00167FC4
		public void SetLocalVolume(long userId, byte volume)
		{
			Result result = this.Methods.SetLocalVolume(this.MethodsPtr, userId, volume);
			if (result != Result.Ok)
			{
				throw new ResultException(result);
			}
		}

		// Token: 0x060036FB RID: 14075 RVA: 0x00169DF4 File Offset: 0x00167FF4
		[MonoPInvokeCallback]
		private static void OnSettingsUpdateImpl(IntPtr ptr)
		{
			Discord discord = (Discord)GCHandle.FromIntPtr(ptr).Target;
			if (discord.VoiceManagerInstance.OnSettingsUpdate != null)
			{
				discord.VoiceManagerInstance.OnSettingsUpdate();
			}
		}

		// Token: 0x04002328 RID: 9000
		private IntPtr MethodsPtr;

		// Token: 0x04002329 RID: 9001
		private object MethodsStructure;

		// Token: 0x02000886 RID: 2182
		internal struct FFIEvents
		{
			// Token: 0x04002F42 RID: 12098
			internal VoiceManager.FFIEvents.SettingsUpdateHandler OnSettingsUpdate;

			// Token: 0x02000959 RID: 2393
			// (Invoke) Token: 0x06004527 RID: 17703
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate void SettingsUpdateHandler(IntPtr ptr);
		}

		// Token: 0x02000887 RID: 2183
		internal struct FFIMethods
		{
			// Token: 0x04002F43 RID: 12099
			internal VoiceManager.FFIMethods.GetInputModeMethod GetInputMode;

			// Token: 0x04002F44 RID: 12100
			internal VoiceManager.FFIMethods.SetInputModeMethod SetInputMode;

			// Token: 0x04002F45 RID: 12101
			internal VoiceManager.FFIMethods.IsSelfMuteMethod IsSelfMute;

			// Token: 0x04002F46 RID: 12102
			internal VoiceManager.FFIMethods.SetSelfMuteMethod SetSelfMute;

			// Token: 0x04002F47 RID: 12103
			internal VoiceManager.FFIMethods.IsSelfDeafMethod IsSelfDeaf;

			// Token: 0x04002F48 RID: 12104
			internal VoiceManager.FFIMethods.SetSelfDeafMethod SetSelfDeaf;

			// Token: 0x04002F49 RID: 12105
			internal VoiceManager.FFIMethods.IsLocalMuteMethod IsLocalMute;

			// Token: 0x04002F4A RID: 12106
			internal VoiceManager.FFIMethods.SetLocalMuteMethod SetLocalMute;

			// Token: 0x04002F4B RID: 12107
			internal VoiceManager.FFIMethods.GetLocalVolumeMethod GetLocalVolume;

			// Token: 0x04002F4C RID: 12108
			internal VoiceManager.FFIMethods.SetLocalVolumeMethod SetLocalVolume;

			// Token: 0x0200095A RID: 2394
			// (Invoke) Token: 0x0600452B RID: 17707
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate Result GetInputModeMethod(IntPtr methodsPtr, ref InputMode inputMode);

			// Token: 0x0200095B RID: 2395
			// (Invoke) Token: 0x0600452F RID: 17711
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate void SetInputModeCallback(IntPtr ptr, Result result);

			// Token: 0x0200095C RID: 2396
			// (Invoke) Token: 0x06004533 RID: 17715
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate void SetInputModeMethod(IntPtr methodsPtr, InputMode inputMode, IntPtr callbackData, VoiceManager.FFIMethods.SetInputModeCallback callback);

			// Token: 0x0200095D RID: 2397
			// (Invoke) Token: 0x06004537 RID: 17719
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate Result IsSelfMuteMethod(IntPtr methodsPtr, ref bool mute);

			// Token: 0x0200095E RID: 2398
			// (Invoke) Token: 0x0600453B RID: 17723
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate Result SetSelfMuteMethod(IntPtr methodsPtr, bool mute);

			// Token: 0x0200095F RID: 2399
			// (Invoke) Token: 0x0600453F RID: 17727
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate Result IsSelfDeafMethod(IntPtr methodsPtr, ref bool deaf);

			// Token: 0x02000960 RID: 2400
			// (Invoke) Token: 0x06004543 RID: 17731
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate Result SetSelfDeafMethod(IntPtr methodsPtr, bool deaf);

			// Token: 0x02000961 RID: 2401
			// (Invoke) Token: 0x06004547 RID: 17735
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate Result IsLocalMuteMethod(IntPtr methodsPtr, long userId, ref bool mute);

			// Token: 0x02000962 RID: 2402
			// (Invoke) Token: 0x0600454B RID: 17739
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate Result SetLocalMuteMethod(IntPtr methodsPtr, long userId, bool mute);

			// Token: 0x02000963 RID: 2403
			// (Invoke) Token: 0x0600454F RID: 17743
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate Result GetLocalVolumeMethod(IntPtr methodsPtr, long userId, ref byte volume);

			// Token: 0x02000964 RID: 2404
			// (Invoke) Token: 0x06004553 RID: 17747
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate Result SetLocalVolumeMethod(IntPtr methodsPtr, long userId, byte volume);
		}

		// Token: 0x02000888 RID: 2184
		// (Invoke) Token: 0x06004242 RID: 16962
		public delegate void SetInputModeHandler(Result result);

		// Token: 0x02000889 RID: 2185
		// (Invoke) Token: 0x06004246 RID: 16966
		public delegate void SettingsUpdateHandler();
	}
}
