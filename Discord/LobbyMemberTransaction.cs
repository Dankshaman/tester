using System;
using System.Runtime.InteropServices;

namespace Discord
{
	// Token: 0x020004E4 RID: 1252
	public struct LobbyMemberTransaction
	{
		// Token: 0x17000742 RID: 1858
		// (get) Token: 0x0600360B RID: 13835 RVA: 0x00166559 File Offset: 0x00164759
		private LobbyMemberTransaction.FFIMethods Methods
		{
			get
			{
				if (this.MethodsStructure == null)
				{
					this.MethodsStructure = Marshal.PtrToStructure(this.MethodsPtr, typeof(LobbyMemberTransaction.FFIMethods));
				}
				return (LobbyMemberTransaction.FFIMethods)this.MethodsStructure;
			}
		}

		// Token: 0x0600360C RID: 13836 RVA: 0x0016658C File Offset: 0x0016478C
		public void SetMetadata(string key, string value)
		{
			if (this.MethodsPtr != IntPtr.Zero)
			{
				Result result = this.Methods.SetMetadata(this.MethodsPtr, key, value);
				if (result != Result.Ok)
				{
					throw new ResultException(result);
				}
			}
		}

		// Token: 0x0600360D RID: 13837 RVA: 0x001665D0 File Offset: 0x001647D0
		public void DeleteMetadata(string key)
		{
			if (this.MethodsPtr != IntPtr.Zero)
			{
				Result result = this.Methods.DeleteMetadata(this.MethodsPtr, key);
				if (result != Result.Ok)
				{
					throw new ResultException(result);
				}
			}
		}

		// Token: 0x040022D7 RID: 8919
		internal IntPtr MethodsPtr;

		// Token: 0x040022D8 RID: 8920
		internal object MethodsStructure;

		// Token: 0x02000843 RID: 2115
		internal struct FFIMethods
		{
			// Token: 0x04002EAD RID: 11949
			internal LobbyMemberTransaction.FFIMethods.SetMetadataMethod SetMetadata;

			// Token: 0x04002EAE RID: 11950
			internal LobbyMemberTransaction.FFIMethods.DeleteMetadataMethod DeleteMetadata;

			// Token: 0x020008C4 RID: 2244
			// (Invoke) Token: 0x060042D3 RID: 17107
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate Result SetMetadataMethod(IntPtr methodsPtr, [MarshalAs(UnmanagedType.LPStr)] string key, [MarshalAs(UnmanagedType.LPStr)] string value);

			// Token: 0x020008C5 RID: 2245
			// (Invoke) Token: 0x060042D7 RID: 17111
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate Result DeleteMetadataMethod(IntPtr methodsPtr, [MarshalAs(UnmanagedType.LPStr)] string key);
		}
	}
}
