using System;
using System.Runtime.InteropServices;

namespace Discord
{
	// Token: 0x020004F3 RID: 1267
	public class AchievementManager
	{
		// Token: 0x1700074F RID: 1871
		// (get) Token: 0x060036FC RID: 14076 RVA: 0x00169E32 File Offset: 0x00168032
		private AchievementManager.FFIMethods Methods
		{
			get
			{
				if (this.MethodsStructure == null)
				{
					this.MethodsStructure = Marshal.PtrToStructure(this.MethodsPtr, typeof(AchievementManager.FFIMethods));
				}
				return (AchievementManager.FFIMethods)this.MethodsStructure;
			}
		}

		// Token: 0x14000091 RID: 145
		// (add) Token: 0x060036FD RID: 14077 RVA: 0x00169E64 File Offset: 0x00168064
		// (remove) Token: 0x060036FE RID: 14078 RVA: 0x00169E9C File Offset: 0x0016809C
		public event AchievementManager.UserAchievementUpdateHandler OnUserAchievementUpdate;

		// Token: 0x060036FF RID: 14079 RVA: 0x00169ED4 File Offset: 0x001680D4
		internal AchievementManager(IntPtr ptr, IntPtr eventsPtr, ref AchievementManager.FFIEvents events)
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

		// Token: 0x06003700 RID: 14080 RVA: 0x00169F23 File Offset: 0x00168123
		private void InitEvents(IntPtr eventsPtr, ref AchievementManager.FFIEvents events)
		{
			events.OnUserAchievementUpdate = new AchievementManager.FFIEvents.UserAchievementUpdateHandler(AchievementManager.OnUserAchievementUpdateImpl);
			Marshal.StructureToPtr<AchievementManager.FFIEvents>(events, eventsPtr, false);
		}

		// Token: 0x06003701 RID: 14081 RVA: 0x00169F44 File Offset: 0x00168144
		[MonoPInvokeCallback]
		private static void SetUserAchievementCallbackImpl(IntPtr ptr, Result result)
		{
			GCHandle gchandle = GCHandle.FromIntPtr(ptr);
			AchievementManager.SetUserAchievementHandler setUserAchievementHandler = (AchievementManager.SetUserAchievementHandler)gchandle.Target;
			gchandle.Free();
			setUserAchievementHandler(result);
		}

		// Token: 0x06003702 RID: 14082 RVA: 0x00169F74 File Offset: 0x00168174
		public void SetUserAchievement(long achievementId, byte percentComplete, AchievementManager.SetUserAchievementHandler callback)
		{
			GCHandle value = GCHandle.Alloc(callback);
			this.Methods.SetUserAchievement(this.MethodsPtr, achievementId, percentComplete, GCHandle.ToIntPtr(value), new AchievementManager.FFIMethods.SetUserAchievementCallback(AchievementManager.SetUserAchievementCallbackImpl));
		}

		// Token: 0x06003703 RID: 14083 RVA: 0x00169FB4 File Offset: 0x001681B4
		[MonoPInvokeCallback]
		private static void FetchUserAchievementsCallbackImpl(IntPtr ptr, Result result)
		{
			GCHandle gchandle = GCHandle.FromIntPtr(ptr);
			AchievementManager.FetchUserAchievementsHandler fetchUserAchievementsHandler = (AchievementManager.FetchUserAchievementsHandler)gchandle.Target;
			gchandle.Free();
			fetchUserAchievementsHandler(result);
		}

		// Token: 0x06003704 RID: 14084 RVA: 0x00169FE4 File Offset: 0x001681E4
		public void FetchUserAchievements(AchievementManager.FetchUserAchievementsHandler callback)
		{
			GCHandle value = GCHandle.Alloc(callback);
			this.Methods.FetchUserAchievements(this.MethodsPtr, GCHandle.ToIntPtr(value), new AchievementManager.FFIMethods.FetchUserAchievementsCallback(AchievementManager.FetchUserAchievementsCallbackImpl));
		}

		// Token: 0x06003705 RID: 14085 RVA: 0x0016A020 File Offset: 0x00168220
		public int CountUserAchievements()
		{
			int result = 0;
			this.Methods.CountUserAchievements(this.MethodsPtr, ref result);
			return result;
		}

		// Token: 0x06003706 RID: 14086 RVA: 0x0016A048 File Offset: 0x00168248
		public UserAchievement GetUserAchievement(long userAchievementId)
		{
			UserAchievement result = default(UserAchievement);
			Result result2 = this.Methods.GetUserAchievement(this.MethodsPtr, userAchievementId, ref result);
			if (result2 != Result.Ok)
			{
				throw new ResultException(result2);
			}
			return result;
		}

		// Token: 0x06003707 RID: 14087 RVA: 0x0016A084 File Offset: 0x00168284
		public UserAchievement GetUserAchievementAt(int index)
		{
			UserAchievement result = default(UserAchievement);
			Result result2 = this.Methods.GetUserAchievementAt(this.MethodsPtr, index, ref result);
			if (result2 != Result.Ok)
			{
				throw new ResultException(result2);
			}
			return result;
		}

		// Token: 0x06003708 RID: 14088 RVA: 0x0016A0C0 File Offset: 0x001682C0
		[MonoPInvokeCallback]
		private static void OnUserAchievementUpdateImpl(IntPtr ptr, ref UserAchievement userAchievement)
		{
			Discord discord = (Discord)GCHandle.FromIntPtr(ptr).Target;
			if (discord.AchievementManagerInstance.OnUserAchievementUpdate != null)
			{
				discord.AchievementManagerInstance.OnUserAchievementUpdate(ref userAchievement);
			}
		}

		// Token: 0x0400232B RID: 9003
		private IntPtr MethodsPtr;

		// Token: 0x0400232C RID: 9004
		private object MethodsStructure;

		// Token: 0x0200088A RID: 2186
		internal struct FFIEvents
		{
			// Token: 0x04002F4D RID: 12109
			internal AchievementManager.FFIEvents.UserAchievementUpdateHandler OnUserAchievementUpdate;

			// Token: 0x02000965 RID: 2405
			// (Invoke) Token: 0x06004557 RID: 17751
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate void UserAchievementUpdateHandler(IntPtr ptr, ref UserAchievement userAchievement);
		}

		// Token: 0x0200088B RID: 2187
		internal struct FFIMethods
		{
			// Token: 0x04002F4E RID: 12110
			internal AchievementManager.FFIMethods.SetUserAchievementMethod SetUserAchievement;

			// Token: 0x04002F4F RID: 12111
			internal AchievementManager.FFIMethods.FetchUserAchievementsMethod FetchUserAchievements;

			// Token: 0x04002F50 RID: 12112
			internal AchievementManager.FFIMethods.CountUserAchievementsMethod CountUserAchievements;

			// Token: 0x04002F51 RID: 12113
			internal AchievementManager.FFIMethods.GetUserAchievementMethod GetUserAchievement;

			// Token: 0x04002F52 RID: 12114
			internal AchievementManager.FFIMethods.GetUserAchievementAtMethod GetUserAchievementAt;

			// Token: 0x02000966 RID: 2406
			// (Invoke) Token: 0x0600455B RID: 17755
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate void SetUserAchievementCallback(IntPtr ptr, Result result);

			// Token: 0x02000967 RID: 2407
			// (Invoke) Token: 0x0600455F RID: 17759
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate void SetUserAchievementMethod(IntPtr methodsPtr, long achievementId, byte percentComplete, IntPtr callbackData, AchievementManager.FFIMethods.SetUserAchievementCallback callback);

			// Token: 0x02000968 RID: 2408
			// (Invoke) Token: 0x06004563 RID: 17763
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate void FetchUserAchievementsCallback(IntPtr ptr, Result result);

			// Token: 0x02000969 RID: 2409
			// (Invoke) Token: 0x06004567 RID: 17767
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate void FetchUserAchievementsMethod(IntPtr methodsPtr, IntPtr callbackData, AchievementManager.FFIMethods.FetchUserAchievementsCallback callback);

			// Token: 0x0200096A RID: 2410
			// (Invoke) Token: 0x0600456B RID: 17771
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate void CountUserAchievementsMethod(IntPtr methodsPtr, ref int count);

			// Token: 0x0200096B RID: 2411
			// (Invoke) Token: 0x0600456F RID: 17775
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate Result GetUserAchievementMethod(IntPtr methodsPtr, long userAchievementId, ref UserAchievement userAchievement);

			// Token: 0x0200096C RID: 2412
			// (Invoke) Token: 0x06004573 RID: 17779
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate Result GetUserAchievementAtMethod(IntPtr methodsPtr, int index, ref UserAchievement userAchievement);
		}

		// Token: 0x0200088C RID: 2188
		// (Invoke) Token: 0x0600424A RID: 16970
		public delegate void SetUserAchievementHandler(Result result);

		// Token: 0x0200088D RID: 2189
		// (Invoke) Token: 0x0600424E RID: 16974
		public delegate void FetchUserAchievementsHandler(Result result);

		// Token: 0x0200088E RID: 2190
		// (Invoke) Token: 0x06004252 RID: 16978
		public delegate void UserAchievementUpdateHandler(ref UserAchievement userAchievement);
	}
}
