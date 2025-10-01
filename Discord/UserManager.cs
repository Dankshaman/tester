using System;
using System.Runtime.InteropServices;

namespace Discord
{
	// Token: 0x020004EA RID: 1258
	public class UserManager
	{
		// Token: 0x17000746 RID: 1862
		// (get) Token: 0x06003634 RID: 13876 RVA: 0x001671BC File Offset: 0x001653BC
		private UserManager.FFIMethods Methods
		{
			get
			{
				if (this.MethodsStructure == null)
				{
					this.MethodsStructure = Marshal.PtrToStructure(this.MethodsPtr, typeof(UserManager.FFIMethods));
				}
				return (UserManager.FFIMethods)this.MethodsStructure;
			}
		}

		// Token: 0x14000080 RID: 128
		// (add) Token: 0x06003635 RID: 13877 RVA: 0x001671EC File Offset: 0x001653EC
		// (remove) Token: 0x06003636 RID: 13878 RVA: 0x00167224 File Offset: 0x00165424
		public event UserManager.CurrentUserUpdateHandler OnCurrentUserUpdate;

		// Token: 0x06003637 RID: 13879 RVA: 0x0016725C File Offset: 0x0016545C
		internal UserManager(IntPtr ptr, IntPtr eventsPtr, ref UserManager.FFIEvents events)
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

		// Token: 0x06003638 RID: 13880 RVA: 0x001672AB File Offset: 0x001654AB
		private void InitEvents(IntPtr eventsPtr, ref UserManager.FFIEvents events)
		{
			events.OnCurrentUserUpdate = new UserManager.FFIEvents.CurrentUserUpdateHandler(UserManager.OnCurrentUserUpdateImpl);
			Marshal.StructureToPtr<UserManager.FFIEvents>(events, eventsPtr, false);
		}

		// Token: 0x06003639 RID: 13881 RVA: 0x001672CC File Offset: 0x001654CC
		public User GetCurrentUser()
		{
			User result = default(User);
			Result result2 = this.Methods.GetCurrentUser(this.MethodsPtr, ref result);
			if (result2 != Result.Ok)
			{
				throw new ResultException(result2);
			}
			return result;
		}

		// Token: 0x0600363A RID: 13882 RVA: 0x00167308 File Offset: 0x00165508
		[MonoPInvokeCallback]
		private static void GetUserCallbackImpl(IntPtr ptr, Result result, ref User user)
		{
			GCHandle gchandle = GCHandle.FromIntPtr(ptr);
			UserManager.GetUserHandler getUserHandler = (UserManager.GetUserHandler)gchandle.Target;
			gchandle.Free();
			getUserHandler(result, ref user);
		}

		// Token: 0x0600363B RID: 13883 RVA: 0x00167338 File Offset: 0x00165538
		public void GetUser(long userId, UserManager.GetUserHandler callback)
		{
			GCHandle value = GCHandle.Alloc(callback);
			this.Methods.GetUser(this.MethodsPtr, userId, GCHandle.ToIntPtr(value), new UserManager.FFIMethods.GetUserCallback(UserManager.GetUserCallbackImpl));
		}

		// Token: 0x0600363C RID: 13884 RVA: 0x00167378 File Offset: 0x00165578
		public PremiumType GetCurrentUserPremiumType()
		{
			PremiumType result = PremiumType.None;
			Result result2 = this.Methods.GetCurrentUserPremiumType(this.MethodsPtr, ref result);
			if (result2 != Result.Ok)
			{
				throw new ResultException(result2);
			}
			return result;
		}

		// Token: 0x0600363D RID: 13885 RVA: 0x001673AC File Offset: 0x001655AC
		public bool CurrentUserHasFlag(UserFlag flag)
		{
			bool result = false;
			Result result2 = this.Methods.CurrentUserHasFlag(this.MethodsPtr, flag, ref result);
			if (result2 != Result.Ok)
			{
				throw new ResultException(result2);
			}
			return result;
		}

		// Token: 0x0600363E RID: 13886 RVA: 0x001673E0 File Offset: 0x001655E0
		[MonoPInvokeCallback]
		private static void OnCurrentUserUpdateImpl(IntPtr ptr)
		{
			Discord discord = (Discord)GCHandle.FromIntPtr(ptr).Target;
			if (discord.UserManagerInstance.OnCurrentUserUpdate != null)
			{
				discord.UserManagerInstance.OnCurrentUserUpdate();
			}
		}

		// Token: 0x04002308 RID: 8968
		private IntPtr MethodsPtr;

		// Token: 0x04002309 RID: 8969
		private object MethodsStructure;

		// Token: 0x0200084E RID: 2126
		internal struct FFIEvents
		{
			// Token: 0x04002EE3 RID: 12003
			internal UserManager.FFIEvents.CurrentUserUpdateHandler OnCurrentUserUpdate;

			// Token: 0x020008E2 RID: 2274
			// (Invoke) Token: 0x0600434B RID: 17227
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate void CurrentUserUpdateHandler(IntPtr ptr);
		}

		// Token: 0x0200084F RID: 2127
		internal struct FFIMethods
		{
			// Token: 0x04002EE4 RID: 12004
			internal UserManager.FFIMethods.GetCurrentUserMethod GetCurrentUser;

			// Token: 0x04002EE5 RID: 12005
			internal UserManager.FFIMethods.GetUserMethod GetUser;

			// Token: 0x04002EE6 RID: 12006
			internal UserManager.FFIMethods.GetCurrentUserPremiumTypeMethod GetCurrentUserPremiumType;

			// Token: 0x04002EE7 RID: 12007
			internal UserManager.FFIMethods.CurrentUserHasFlagMethod CurrentUserHasFlag;

			// Token: 0x020008E3 RID: 2275
			// (Invoke) Token: 0x0600434F RID: 17231
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate Result GetCurrentUserMethod(IntPtr methodsPtr, ref User currentUser);

			// Token: 0x020008E4 RID: 2276
			// (Invoke) Token: 0x06004353 RID: 17235
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate void GetUserCallback(IntPtr ptr, Result result, ref User user);

			// Token: 0x020008E5 RID: 2277
			// (Invoke) Token: 0x06004357 RID: 17239
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate void GetUserMethod(IntPtr methodsPtr, long userId, IntPtr callbackData, UserManager.FFIMethods.GetUserCallback callback);

			// Token: 0x020008E6 RID: 2278
			// (Invoke) Token: 0x0600435B RID: 17243
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate Result GetCurrentUserPremiumTypeMethod(IntPtr methodsPtr, ref PremiumType premiumType);

			// Token: 0x020008E7 RID: 2279
			// (Invoke) Token: 0x0600435F RID: 17247
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate Result CurrentUserHasFlagMethod(IntPtr methodsPtr, UserFlag flag, ref bool hasFlag);
		}

		// Token: 0x02000850 RID: 2128
		// (Invoke) Token: 0x060041A2 RID: 16802
		public delegate void GetUserHandler(Result result, ref User user);

		// Token: 0x02000851 RID: 2129
		// (Invoke) Token: 0x060041A6 RID: 16806
		public delegate void CurrentUserUpdateHandler();
	}
}
