using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace Discord
{
	// Token: 0x020004F1 RID: 1265
	public class StoreManager
	{
		// Token: 0x1700074D RID: 1869
		// (get) Token: 0x060036D3 RID: 14035 RVA: 0x00169570 File Offset: 0x00167770
		private StoreManager.FFIMethods Methods
		{
			get
			{
				if (this.MethodsStructure == null)
				{
					this.MethodsStructure = Marshal.PtrToStructure(this.MethodsPtr, typeof(StoreManager.FFIMethods));
				}
				return (StoreManager.FFIMethods)this.MethodsStructure;
			}
		}

		// Token: 0x1400008E RID: 142
		// (add) Token: 0x060036D4 RID: 14036 RVA: 0x001695A0 File Offset: 0x001677A0
		// (remove) Token: 0x060036D5 RID: 14037 RVA: 0x001695D8 File Offset: 0x001677D8
		public event StoreManager.EntitlementCreateHandler OnEntitlementCreate;

		// Token: 0x1400008F RID: 143
		// (add) Token: 0x060036D6 RID: 14038 RVA: 0x00169610 File Offset: 0x00167810
		// (remove) Token: 0x060036D7 RID: 14039 RVA: 0x00169648 File Offset: 0x00167848
		public event StoreManager.EntitlementDeleteHandler OnEntitlementDelete;

		// Token: 0x060036D8 RID: 14040 RVA: 0x00169680 File Offset: 0x00167880
		internal StoreManager(IntPtr ptr, IntPtr eventsPtr, ref StoreManager.FFIEvents events)
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

		// Token: 0x060036D9 RID: 14041 RVA: 0x001696CF File Offset: 0x001678CF
		private void InitEvents(IntPtr eventsPtr, ref StoreManager.FFIEvents events)
		{
			events.OnEntitlementCreate = new StoreManager.FFIEvents.EntitlementCreateHandler(StoreManager.OnEntitlementCreateImpl);
			events.OnEntitlementDelete = new StoreManager.FFIEvents.EntitlementDeleteHandler(StoreManager.OnEntitlementDeleteImpl);
			Marshal.StructureToPtr<StoreManager.FFIEvents>(events, eventsPtr, false);
		}

		// Token: 0x060036DA RID: 14042 RVA: 0x00169704 File Offset: 0x00167904
		[MonoPInvokeCallback]
		private static void FetchSkusCallbackImpl(IntPtr ptr, Result result)
		{
			GCHandle gchandle = GCHandle.FromIntPtr(ptr);
			StoreManager.FetchSkusHandler fetchSkusHandler = (StoreManager.FetchSkusHandler)gchandle.Target;
			gchandle.Free();
			fetchSkusHandler(result);
		}

		// Token: 0x060036DB RID: 14043 RVA: 0x00169734 File Offset: 0x00167934
		public void FetchSkus(StoreManager.FetchSkusHandler callback)
		{
			GCHandle value = GCHandle.Alloc(callback);
			this.Methods.FetchSkus(this.MethodsPtr, GCHandle.ToIntPtr(value), new StoreManager.FFIMethods.FetchSkusCallback(StoreManager.FetchSkusCallbackImpl));
		}

		// Token: 0x060036DC RID: 14044 RVA: 0x00169770 File Offset: 0x00167970
		public int CountSkus()
		{
			int result = 0;
			this.Methods.CountSkus(this.MethodsPtr, ref result);
			return result;
		}

		// Token: 0x060036DD RID: 14045 RVA: 0x00169798 File Offset: 0x00167998
		public Sku GetSku(long skuId)
		{
			Sku result = default(Sku);
			Result result2 = this.Methods.GetSku(this.MethodsPtr, skuId, ref result);
			if (result2 != Result.Ok)
			{
				throw new ResultException(result2);
			}
			return result;
		}

		// Token: 0x060036DE RID: 14046 RVA: 0x001697D4 File Offset: 0x001679D4
		public Sku GetSkuAt(int index)
		{
			Sku result = default(Sku);
			Result result2 = this.Methods.GetSkuAt(this.MethodsPtr, index, ref result);
			if (result2 != Result.Ok)
			{
				throw new ResultException(result2);
			}
			return result;
		}

		// Token: 0x060036DF RID: 14047 RVA: 0x00169810 File Offset: 0x00167A10
		[MonoPInvokeCallback]
		private static void FetchEntitlementsCallbackImpl(IntPtr ptr, Result result)
		{
			GCHandle gchandle = GCHandle.FromIntPtr(ptr);
			StoreManager.FetchEntitlementsHandler fetchEntitlementsHandler = (StoreManager.FetchEntitlementsHandler)gchandle.Target;
			gchandle.Free();
			fetchEntitlementsHandler(result);
		}

		// Token: 0x060036E0 RID: 14048 RVA: 0x00169840 File Offset: 0x00167A40
		public void FetchEntitlements(StoreManager.FetchEntitlementsHandler callback)
		{
			GCHandle value = GCHandle.Alloc(callback);
			this.Methods.FetchEntitlements(this.MethodsPtr, GCHandle.ToIntPtr(value), new StoreManager.FFIMethods.FetchEntitlementsCallback(StoreManager.FetchEntitlementsCallbackImpl));
		}

		// Token: 0x060036E1 RID: 14049 RVA: 0x0016987C File Offset: 0x00167A7C
		public int CountEntitlements()
		{
			int result = 0;
			this.Methods.CountEntitlements(this.MethodsPtr, ref result);
			return result;
		}

		// Token: 0x060036E2 RID: 14050 RVA: 0x001698A4 File Offset: 0x00167AA4
		public Entitlement GetEntitlement(long entitlementId)
		{
			Entitlement result = default(Entitlement);
			Result result2 = this.Methods.GetEntitlement(this.MethodsPtr, entitlementId, ref result);
			if (result2 != Result.Ok)
			{
				throw new ResultException(result2);
			}
			return result;
		}

		// Token: 0x060036E3 RID: 14051 RVA: 0x001698E0 File Offset: 0x00167AE0
		public Entitlement GetEntitlementAt(int index)
		{
			Entitlement result = default(Entitlement);
			Result result2 = this.Methods.GetEntitlementAt(this.MethodsPtr, index, ref result);
			if (result2 != Result.Ok)
			{
				throw new ResultException(result2);
			}
			return result;
		}

		// Token: 0x060036E4 RID: 14052 RVA: 0x0016991C File Offset: 0x00167B1C
		public bool HasSkuEntitlement(long skuId)
		{
			bool result = false;
			Result result2 = this.Methods.HasSkuEntitlement(this.MethodsPtr, skuId, ref result);
			if (result2 != Result.Ok)
			{
				throw new ResultException(result2);
			}
			return result;
		}

		// Token: 0x060036E5 RID: 14053 RVA: 0x00169950 File Offset: 0x00167B50
		[MonoPInvokeCallback]
		private static void StartPurchaseCallbackImpl(IntPtr ptr, Result result)
		{
			GCHandle gchandle = GCHandle.FromIntPtr(ptr);
			StoreManager.StartPurchaseHandler startPurchaseHandler = (StoreManager.StartPurchaseHandler)gchandle.Target;
			gchandle.Free();
			startPurchaseHandler(result);
		}

		// Token: 0x060036E6 RID: 14054 RVA: 0x00169980 File Offset: 0x00167B80
		public void StartPurchase(long skuId, StoreManager.StartPurchaseHandler callback)
		{
			GCHandle value = GCHandle.Alloc(callback);
			this.Methods.StartPurchase(this.MethodsPtr, skuId, GCHandle.ToIntPtr(value), new StoreManager.FFIMethods.StartPurchaseCallback(StoreManager.StartPurchaseCallbackImpl));
		}

		// Token: 0x060036E7 RID: 14055 RVA: 0x001699C0 File Offset: 0x00167BC0
		[MonoPInvokeCallback]
		private static void OnEntitlementCreateImpl(IntPtr ptr, ref Entitlement entitlement)
		{
			Discord discord = (Discord)GCHandle.FromIntPtr(ptr).Target;
			if (discord.StoreManagerInstance.OnEntitlementCreate != null)
			{
				discord.StoreManagerInstance.OnEntitlementCreate(ref entitlement);
			}
		}

		// Token: 0x060036E8 RID: 14056 RVA: 0x00169A00 File Offset: 0x00167C00
		[MonoPInvokeCallback]
		private static void OnEntitlementDeleteImpl(IntPtr ptr, ref Entitlement entitlement)
		{
			Discord discord = (Discord)GCHandle.FromIntPtr(ptr).Target;
			if (discord.StoreManagerInstance.OnEntitlementDelete != null)
			{
				discord.StoreManagerInstance.OnEntitlementDelete(ref entitlement);
			}
		}

		// Token: 0x060036E9 RID: 14057 RVA: 0x00169A40 File Offset: 0x00167C40
		public IEnumerable<Entitlement> GetEntitlements()
		{
			int num = this.CountEntitlements();
			List<Entitlement> list = new List<Entitlement>();
			for (int i = 0; i < num; i++)
			{
				list.Add(this.GetEntitlementAt(i));
			}
			return list;
		}

		// Token: 0x060036EA RID: 14058 RVA: 0x00169A74 File Offset: 0x00167C74
		public IEnumerable<Sku> GetSkus()
		{
			int num = this.CountSkus();
			List<Sku> list = new List<Sku>();
			for (int i = 0; i < num; i++)
			{
				list.Add(this.GetSkuAt(i));
			}
			return list;
		}

		// Token: 0x04002324 RID: 8996
		private IntPtr MethodsPtr;

		// Token: 0x04002325 RID: 8997
		private object MethodsStructure;

		// Token: 0x0200087F RID: 2175
		internal struct FFIEvents
		{
			// Token: 0x04002F36 RID: 12086
			internal StoreManager.FFIEvents.EntitlementCreateHandler OnEntitlementCreate;

			// Token: 0x04002F37 RID: 12087
			internal StoreManager.FFIEvents.EntitlementDeleteHandler OnEntitlementDelete;

			// Token: 0x0200094A RID: 2378
			// (Invoke) Token: 0x060044EB RID: 17643
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate void EntitlementCreateHandler(IntPtr ptr, ref Entitlement entitlement);

			// Token: 0x0200094B RID: 2379
			// (Invoke) Token: 0x060044EF RID: 17647
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate void EntitlementDeleteHandler(IntPtr ptr, ref Entitlement entitlement);
		}

		// Token: 0x02000880 RID: 2176
		internal struct FFIMethods
		{
			// Token: 0x04002F38 RID: 12088
			internal StoreManager.FFIMethods.FetchSkusMethod FetchSkus;

			// Token: 0x04002F39 RID: 12089
			internal StoreManager.FFIMethods.CountSkusMethod CountSkus;

			// Token: 0x04002F3A RID: 12090
			internal StoreManager.FFIMethods.GetSkuMethod GetSku;

			// Token: 0x04002F3B RID: 12091
			internal StoreManager.FFIMethods.GetSkuAtMethod GetSkuAt;

			// Token: 0x04002F3C RID: 12092
			internal StoreManager.FFIMethods.FetchEntitlementsMethod FetchEntitlements;

			// Token: 0x04002F3D RID: 12093
			internal StoreManager.FFIMethods.CountEntitlementsMethod CountEntitlements;

			// Token: 0x04002F3E RID: 12094
			internal StoreManager.FFIMethods.GetEntitlementMethod GetEntitlement;

			// Token: 0x04002F3F RID: 12095
			internal StoreManager.FFIMethods.GetEntitlementAtMethod GetEntitlementAt;

			// Token: 0x04002F40 RID: 12096
			internal StoreManager.FFIMethods.HasSkuEntitlementMethod HasSkuEntitlement;

			// Token: 0x04002F41 RID: 12097
			internal StoreManager.FFIMethods.StartPurchaseMethod StartPurchase;

			// Token: 0x0200094C RID: 2380
			// (Invoke) Token: 0x060044F3 RID: 17651
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate void FetchSkusCallback(IntPtr ptr, Result result);

			// Token: 0x0200094D RID: 2381
			// (Invoke) Token: 0x060044F7 RID: 17655
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate void FetchSkusMethod(IntPtr methodsPtr, IntPtr callbackData, StoreManager.FFIMethods.FetchSkusCallback callback);

			// Token: 0x0200094E RID: 2382
			// (Invoke) Token: 0x060044FB RID: 17659
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate void CountSkusMethod(IntPtr methodsPtr, ref int count);

			// Token: 0x0200094F RID: 2383
			// (Invoke) Token: 0x060044FF RID: 17663
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate Result GetSkuMethod(IntPtr methodsPtr, long skuId, ref Sku sku);

			// Token: 0x02000950 RID: 2384
			// (Invoke) Token: 0x06004503 RID: 17667
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate Result GetSkuAtMethod(IntPtr methodsPtr, int index, ref Sku sku);

			// Token: 0x02000951 RID: 2385
			// (Invoke) Token: 0x06004507 RID: 17671
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate void FetchEntitlementsCallback(IntPtr ptr, Result result);

			// Token: 0x02000952 RID: 2386
			// (Invoke) Token: 0x0600450B RID: 17675
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate void FetchEntitlementsMethod(IntPtr methodsPtr, IntPtr callbackData, StoreManager.FFIMethods.FetchEntitlementsCallback callback);

			// Token: 0x02000953 RID: 2387
			// (Invoke) Token: 0x0600450F RID: 17679
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate void CountEntitlementsMethod(IntPtr methodsPtr, ref int count);

			// Token: 0x02000954 RID: 2388
			// (Invoke) Token: 0x06004513 RID: 17683
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate Result GetEntitlementMethod(IntPtr methodsPtr, long entitlementId, ref Entitlement entitlement);

			// Token: 0x02000955 RID: 2389
			// (Invoke) Token: 0x06004517 RID: 17687
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate Result GetEntitlementAtMethod(IntPtr methodsPtr, int index, ref Entitlement entitlement);

			// Token: 0x02000956 RID: 2390
			// (Invoke) Token: 0x0600451B RID: 17691
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate Result HasSkuEntitlementMethod(IntPtr methodsPtr, long skuId, ref bool hasEntitlement);

			// Token: 0x02000957 RID: 2391
			// (Invoke) Token: 0x0600451F RID: 17695
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate void StartPurchaseCallback(IntPtr ptr, Result result);

			// Token: 0x02000958 RID: 2392
			// (Invoke) Token: 0x06004523 RID: 17699
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate void StartPurchaseMethod(IntPtr methodsPtr, long skuId, IntPtr callbackData, StoreManager.FFIMethods.StartPurchaseCallback callback);
		}

		// Token: 0x02000881 RID: 2177
		// (Invoke) Token: 0x0600422E RID: 16942
		public delegate void FetchSkusHandler(Result result);

		// Token: 0x02000882 RID: 2178
		// (Invoke) Token: 0x06004232 RID: 16946
		public delegate void FetchEntitlementsHandler(Result result);

		// Token: 0x02000883 RID: 2179
		// (Invoke) Token: 0x06004236 RID: 16950
		public delegate void StartPurchaseHandler(Result result);

		// Token: 0x02000884 RID: 2180
		// (Invoke) Token: 0x0600423A RID: 16954
		public delegate void EntitlementCreateHandler(ref Entitlement entitlement);

		// Token: 0x02000885 RID: 2181
		// (Invoke) Token: 0x0600423E RID: 16958
		public delegate void EntitlementDeleteHandler(ref Entitlement entitlement);
	}
}
