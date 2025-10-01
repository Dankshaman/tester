using System;
using System.Runtime.InteropServices;

namespace Discord
{
	// Token: 0x020004E3 RID: 1251
	public struct LobbyTransaction
	{
		// Token: 0x17000741 RID: 1857
		// (get) Token: 0x06003604 RID: 13828 RVA: 0x00166392 File Offset: 0x00164592
		private LobbyTransaction.FFIMethods Methods
		{
			get
			{
				if (this.MethodsStructure == null)
				{
					this.MethodsStructure = Marshal.PtrToStructure(this.MethodsPtr, typeof(LobbyTransaction.FFIMethods));
				}
				return (LobbyTransaction.FFIMethods)this.MethodsStructure;
			}
		}

		// Token: 0x06003605 RID: 13829 RVA: 0x001663C4 File Offset: 0x001645C4
		public void SetType(LobbyType type)
		{
			if (this.MethodsPtr != IntPtr.Zero)
			{
				Result result = this.Methods.SetType(this.MethodsPtr, type);
				if (result != Result.Ok)
				{
					throw new ResultException(result);
				}
			}
		}

		// Token: 0x06003606 RID: 13830 RVA: 0x00166408 File Offset: 0x00164608
		public void SetOwner(long ownerId)
		{
			if (this.MethodsPtr != IntPtr.Zero)
			{
				Result result = this.Methods.SetOwner(this.MethodsPtr, ownerId);
				if (result != Result.Ok)
				{
					throw new ResultException(result);
				}
			}
		}

		// Token: 0x06003607 RID: 13831 RVA: 0x0016644C File Offset: 0x0016464C
		public void SetCapacity(uint capacity)
		{
			if (this.MethodsPtr != IntPtr.Zero)
			{
				Result result = this.Methods.SetCapacity(this.MethodsPtr, capacity);
				if (result != Result.Ok)
				{
					throw new ResultException(result);
				}
			}
		}

		// Token: 0x06003608 RID: 13832 RVA: 0x00166490 File Offset: 0x00164690
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

		// Token: 0x06003609 RID: 13833 RVA: 0x001664D4 File Offset: 0x001646D4
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

		// Token: 0x0600360A RID: 13834 RVA: 0x00166518 File Offset: 0x00164718
		public void SetLocked(bool locked)
		{
			if (this.MethodsPtr != IntPtr.Zero)
			{
				Result result = this.Methods.SetLocked(this.MethodsPtr, locked);
				if (result != Result.Ok)
				{
					throw new ResultException(result);
				}
			}
		}

		// Token: 0x040022D5 RID: 8917
		internal IntPtr MethodsPtr;

		// Token: 0x040022D6 RID: 8918
		internal object MethodsStructure;

		// Token: 0x02000842 RID: 2114
		internal struct FFIMethods
		{
			// Token: 0x04002EA7 RID: 11943
			internal LobbyTransaction.FFIMethods.SetTypeMethod SetType;

			// Token: 0x04002EA8 RID: 11944
			internal LobbyTransaction.FFIMethods.SetOwnerMethod SetOwner;

			// Token: 0x04002EA9 RID: 11945
			internal LobbyTransaction.FFIMethods.SetCapacityMethod SetCapacity;

			// Token: 0x04002EAA RID: 11946
			internal LobbyTransaction.FFIMethods.SetMetadataMethod SetMetadata;

			// Token: 0x04002EAB RID: 11947
			internal LobbyTransaction.FFIMethods.DeleteMetadataMethod DeleteMetadata;

			// Token: 0x04002EAC RID: 11948
			internal LobbyTransaction.FFIMethods.SetLockedMethod SetLocked;

			// Token: 0x020008BE RID: 2238
			// (Invoke) Token: 0x060042BB RID: 17083
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate Result SetTypeMethod(IntPtr methodsPtr, LobbyType type);

			// Token: 0x020008BF RID: 2239
			// (Invoke) Token: 0x060042BF RID: 17087
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate Result SetOwnerMethod(IntPtr methodsPtr, long ownerId);

			// Token: 0x020008C0 RID: 2240
			// (Invoke) Token: 0x060042C3 RID: 17091
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate Result SetCapacityMethod(IntPtr methodsPtr, uint capacity);

			// Token: 0x020008C1 RID: 2241
			// (Invoke) Token: 0x060042C7 RID: 17095
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate Result SetMetadataMethod(IntPtr methodsPtr, [MarshalAs(UnmanagedType.LPStr)] string key, [MarshalAs(UnmanagedType.LPStr)] string value);

			// Token: 0x020008C2 RID: 2242
			// (Invoke) Token: 0x060042CB RID: 17099
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate Result DeleteMetadataMethod(IntPtr methodsPtr, [MarshalAs(UnmanagedType.LPStr)] string key);

			// Token: 0x020008C3 RID: 2243
			// (Invoke) Token: 0x060042CF RID: 17103
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate Result SetLockedMethod(IntPtr methodsPtr, bool locked);
		}
	}
}
