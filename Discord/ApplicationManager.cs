using System;
using System.Runtime.InteropServices;
using System.Text;

namespace Discord
{
	// Token: 0x020004E9 RID: 1257
	public class ApplicationManager
	{
		// Token: 0x17000745 RID: 1861
		// (get) Token: 0x06003629 RID: 13865 RVA: 0x00166F75 File Offset: 0x00165175
		private ApplicationManager.FFIMethods Methods
		{
			get
			{
				if (this.MethodsStructure == null)
				{
					this.MethodsStructure = Marshal.PtrToStructure(this.MethodsPtr, typeof(ApplicationManager.FFIMethods));
				}
				return (ApplicationManager.FFIMethods)this.MethodsStructure;
			}
		}

		// Token: 0x0600362A RID: 13866 RVA: 0x00166FA8 File Offset: 0x001651A8
		internal ApplicationManager(IntPtr ptr, IntPtr eventsPtr, ref ApplicationManager.FFIEvents events)
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

		// Token: 0x0600362B RID: 13867 RVA: 0x00166FF7 File Offset: 0x001651F7
		private void InitEvents(IntPtr eventsPtr, ref ApplicationManager.FFIEvents events)
		{
			Marshal.StructureToPtr<ApplicationManager.FFIEvents>(events, eventsPtr, false);
		}

		// Token: 0x0600362C RID: 13868 RVA: 0x00167008 File Offset: 0x00165208
		[MonoPInvokeCallback]
		private static void ValidateOrExitCallbackImpl(IntPtr ptr, Result result)
		{
			GCHandle gchandle = GCHandle.FromIntPtr(ptr);
			ApplicationManager.ValidateOrExitHandler validateOrExitHandler = (ApplicationManager.ValidateOrExitHandler)gchandle.Target;
			gchandle.Free();
			validateOrExitHandler(result);
		}

		// Token: 0x0600362D RID: 13869 RVA: 0x00167038 File Offset: 0x00165238
		public void ValidateOrExit(ApplicationManager.ValidateOrExitHandler callback)
		{
			GCHandle value = GCHandle.Alloc(callback);
			this.Methods.ValidateOrExit(this.MethodsPtr, GCHandle.ToIntPtr(value), new ApplicationManager.FFIMethods.ValidateOrExitCallback(ApplicationManager.ValidateOrExitCallbackImpl));
		}

		// Token: 0x0600362E RID: 13870 RVA: 0x00167074 File Offset: 0x00165274
		public string GetCurrentLocale()
		{
			StringBuilder stringBuilder = new StringBuilder(128);
			this.Methods.GetCurrentLocale(this.MethodsPtr, stringBuilder);
			return stringBuilder.ToString();
		}

		// Token: 0x0600362F RID: 13871 RVA: 0x001670AC File Offset: 0x001652AC
		public string GetCurrentBranch()
		{
			StringBuilder stringBuilder = new StringBuilder(4096);
			this.Methods.GetCurrentBranch(this.MethodsPtr, stringBuilder);
			return stringBuilder.ToString();
		}

		// Token: 0x06003630 RID: 13872 RVA: 0x001670E4 File Offset: 0x001652E4
		[MonoPInvokeCallback]
		private static void GetOAuth2TokenCallbackImpl(IntPtr ptr, Result result, ref OAuth2Token oauth2Token)
		{
			GCHandle gchandle = GCHandle.FromIntPtr(ptr);
			ApplicationManager.GetOAuth2TokenHandler getOAuth2TokenHandler = (ApplicationManager.GetOAuth2TokenHandler)gchandle.Target;
			gchandle.Free();
			getOAuth2TokenHandler(result, ref oauth2Token);
		}

		// Token: 0x06003631 RID: 13873 RVA: 0x00167114 File Offset: 0x00165314
		public void GetOAuth2Token(ApplicationManager.GetOAuth2TokenHandler callback)
		{
			GCHandle value = GCHandle.Alloc(callback);
			this.Methods.GetOAuth2Token(this.MethodsPtr, GCHandle.ToIntPtr(value), new ApplicationManager.FFIMethods.GetOAuth2TokenCallback(ApplicationManager.GetOAuth2TokenCallbackImpl));
		}

		// Token: 0x06003632 RID: 13874 RVA: 0x00167150 File Offset: 0x00165350
		[MonoPInvokeCallback]
		private static void GetTicketCallbackImpl(IntPtr ptr, Result result, ref string data)
		{
			GCHandle gchandle = GCHandle.FromIntPtr(ptr);
			ApplicationManager.GetTicketHandler getTicketHandler = (ApplicationManager.GetTicketHandler)gchandle.Target;
			gchandle.Free();
			getTicketHandler(result, ref data);
		}

		// Token: 0x06003633 RID: 13875 RVA: 0x00167180 File Offset: 0x00165380
		public void GetTicket(ApplicationManager.GetTicketHandler callback)
		{
			GCHandle value = GCHandle.Alloc(callback);
			this.Methods.GetTicket(this.MethodsPtr, GCHandle.ToIntPtr(value), new ApplicationManager.FFIMethods.GetTicketCallback(ApplicationManager.GetTicketCallbackImpl));
		}

		// Token: 0x04002306 RID: 8966
		private IntPtr MethodsPtr;

		// Token: 0x04002307 RID: 8967
		private object MethodsStructure;

		// Token: 0x02000849 RID: 2121
		internal struct FFIEvents
		{
		}

		// Token: 0x0200084A RID: 2122
		internal struct FFIMethods
		{
			// Token: 0x04002EDE RID: 11998
			internal ApplicationManager.FFIMethods.ValidateOrExitMethod ValidateOrExit;

			// Token: 0x04002EDF RID: 11999
			internal ApplicationManager.FFIMethods.GetCurrentLocaleMethod GetCurrentLocale;

			// Token: 0x04002EE0 RID: 12000
			internal ApplicationManager.FFIMethods.GetCurrentBranchMethod GetCurrentBranch;

			// Token: 0x04002EE1 RID: 12001
			internal ApplicationManager.FFIMethods.GetOAuth2TokenMethod GetOAuth2Token;

			// Token: 0x04002EE2 RID: 12002
			internal ApplicationManager.FFIMethods.GetTicketMethod GetTicket;

			// Token: 0x020008DA RID: 2266
			// (Invoke) Token: 0x0600432B RID: 17195
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate void ValidateOrExitCallback(IntPtr ptr, Result result);

			// Token: 0x020008DB RID: 2267
			// (Invoke) Token: 0x0600432F RID: 17199
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate void ValidateOrExitMethod(IntPtr methodsPtr, IntPtr callbackData, ApplicationManager.FFIMethods.ValidateOrExitCallback callback);

			// Token: 0x020008DC RID: 2268
			// (Invoke) Token: 0x06004333 RID: 17203
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate void GetCurrentLocaleMethod(IntPtr methodsPtr, StringBuilder locale);

			// Token: 0x020008DD RID: 2269
			// (Invoke) Token: 0x06004337 RID: 17207
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate void GetCurrentBranchMethod(IntPtr methodsPtr, StringBuilder branch);

			// Token: 0x020008DE RID: 2270
			// (Invoke) Token: 0x0600433B RID: 17211
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate void GetOAuth2TokenCallback(IntPtr ptr, Result result, ref OAuth2Token oauth2Token);

			// Token: 0x020008DF RID: 2271
			// (Invoke) Token: 0x0600433F RID: 17215
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate void GetOAuth2TokenMethod(IntPtr methodsPtr, IntPtr callbackData, ApplicationManager.FFIMethods.GetOAuth2TokenCallback callback);

			// Token: 0x020008E0 RID: 2272
			// (Invoke) Token: 0x06004343 RID: 17219
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate void GetTicketCallback(IntPtr ptr, Result result, [MarshalAs(UnmanagedType.LPStr)] ref string data);

			// Token: 0x020008E1 RID: 2273
			// (Invoke) Token: 0x06004347 RID: 17223
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate void GetTicketMethod(IntPtr methodsPtr, IntPtr callbackData, ApplicationManager.FFIMethods.GetTicketCallback callback);
		}

		// Token: 0x0200084B RID: 2123
		// (Invoke) Token: 0x06004196 RID: 16790
		public delegate void ValidateOrExitHandler(Result result);

		// Token: 0x0200084C RID: 2124
		// (Invoke) Token: 0x0600419A RID: 16794
		public delegate void GetOAuth2TokenHandler(Result result, ref OAuth2Token oauth2Token);

		// Token: 0x0200084D RID: 2125
		// (Invoke) Token: 0x0600419E RID: 16798
		public delegate void GetTicketHandler(Result result, ref string data);
	}
}
