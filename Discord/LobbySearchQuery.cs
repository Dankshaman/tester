using System;
using System.Runtime.InteropServices;

namespace Discord
{
	// Token: 0x020004E5 RID: 1253
	public struct LobbySearchQuery
	{
		// Token: 0x17000743 RID: 1859
		// (get) Token: 0x0600360E RID: 13838 RVA: 0x00166611 File Offset: 0x00164811
		private LobbySearchQuery.FFIMethods Methods
		{
			get
			{
				if (this.MethodsStructure == null)
				{
					this.MethodsStructure = Marshal.PtrToStructure(this.MethodsPtr, typeof(LobbySearchQuery.FFIMethods));
				}
				return (LobbySearchQuery.FFIMethods)this.MethodsStructure;
			}
		}

		// Token: 0x0600360F RID: 13839 RVA: 0x00166644 File Offset: 0x00164844
		public void Filter(string key, LobbySearchComparison comparison, LobbySearchCast cast, string value)
		{
			if (this.MethodsPtr != IntPtr.Zero)
			{
				Result result = this.Methods.Filter(this.MethodsPtr, key, comparison, cast, value);
				if (result != Result.Ok)
				{
					throw new ResultException(result);
				}
			}
		}

		// Token: 0x06003610 RID: 13840 RVA: 0x0016668C File Offset: 0x0016488C
		public void Sort(string key, LobbySearchCast cast, string value)
		{
			if (this.MethodsPtr != IntPtr.Zero)
			{
				Result result = this.Methods.Sort(this.MethodsPtr, key, cast, value);
				if (result != Result.Ok)
				{
					throw new ResultException(result);
				}
			}
		}

		// Token: 0x06003611 RID: 13841 RVA: 0x001666D0 File Offset: 0x001648D0
		public void Limit(uint limit)
		{
			if (this.MethodsPtr != IntPtr.Zero)
			{
				Result result = this.Methods.Limit(this.MethodsPtr, limit);
				if (result != Result.Ok)
				{
					throw new ResultException(result);
				}
			}
		}

		// Token: 0x06003612 RID: 13842 RVA: 0x00166714 File Offset: 0x00164914
		public void Distance(LobbySearchDistance distance)
		{
			if (this.MethodsPtr != IntPtr.Zero)
			{
				Result result = this.Methods.Distance(this.MethodsPtr, distance);
				if (result != Result.Ok)
				{
					throw new ResultException(result);
				}
			}
		}

		// Token: 0x040022D9 RID: 8921
		internal IntPtr MethodsPtr;

		// Token: 0x040022DA RID: 8922
		internal object MethodsStructure;

		// Token: 0x02000844 RID: 2116
		internal struct FFIMethods
		{
			// Token: 0x04002EAF RID: 11951
			internal LobbySearchQuery.FFIMethods.FilterMethod Filter;

			// Token: 0x04002EB0 RID: 11952
			internal LobbySearchQuery.FFIMethods.SortMethod Sort;

			// Token: 0x04002EB1 RID: 11953
			internal LobbySearchQuery.FFIMethods.LimitMethod Limit;

			// Token: 0x04002EB2 RID: 11954
			internal LobbySearchQuery.FFIMethods.DistanceMethod Distance;

			// Token: 0x020008C6 RID: 2246
			// (Invoke) Token: 0x060042DB RID: 17115
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate Result FilterMethod(IntPtr methodsPtr, [MarshalAs(UnmanagedType.LPStr)] string key, LobbySearchComparison comparison, LobbySearchCast cast, [MarshalAs(UnmanagedType.LPStr)] string value);

			// Token: 0x020008C7 RID: 2247
			// (Invoke) Token: 0x060042DF RID: 17119
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate Result SortMethod(IntPtr methodsPtr, [MarshalAs(UnmanagedType.LPStr)] string key, LobbySearchCast cast, [MarshalAs(UnmanagedType.LPStr)] string value);

			// Token: 0x020008C8 RID: 2248
			// (Invoke) Token: 0x060042E3 RID: 17123
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate Result LimitMethod(IntPtr methodsPtr, uint limit);

			// Token: 0x020008C9 RID: 2249
			// (Invoke) Token: 0x060042E7 RID: 17127
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate Result DistanceMethod(IntPtr methodsPtr, LobbySearchDistance distance);
		}
	}
}
