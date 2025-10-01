using System;
using System.Runtime.InteropServices;

namespace Discord
{
	// Token: 0x020004EC RID: 1260
	public class RelationshipManager
	{
		// Token: 0x17000748 RID: 1864
		// (get) Token: 0x06003649 RID: 13897 RVA: 0x0016760C File Offset: 0x0016580C
		private RelationshipManager.FFIMethods Methods
		{
			get
			{
				if (this.MethodsStructure == null)
				{
					this.MethodsStructure = Marshal.PtrToStructure(this.MethodsPtr, typeof(RelationshipManager.FFIMethods));
				}
				return (RelationshipManager.FFIMethods)this.MethodsStructure;
			}
		}

		// Token: 0x14000081 RID: 129
		// (add) Token: 0x0600364A RID: 13898 RVA: 0x0016763C File Offset: 0x0016583C
		// (remove) Token: 0x0600364B RID: 13899 RVA: 0x00167674 File Offset: 0x00165874
		public event RelationshipManager.RefreshHandler OnRefresh;

		// Token: 0x14000082 RID: 130
		// (add) Token: 0x0600364C RID: 13900 RVA: 0x001676AC File Offset: 0x001658AC
		// (remove) Token: 0x0600364D RID: 13901 RVA: 0x001676E4 File Offset: 0x001658E4
		public event RelationshipManager.RelationshipUpdateHandler OnRelationshipUpdate;

		// Token: 0x0600364E RID: 13902 RVA: 0x0016771C File Offset: 0x0016591C
		internal RelationshipManager(IntPtr ptr, IntPtr eventsPtr, ref RelationshipManager.FFIEvents events)
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

		// Token: 0x0600364F RID: 13903 RVA: 0x0016776B File Offset: 0x0016596B
		private void InitEvents(IntPtr eventsPtr, ref RelationshipManager.FFIEvents events)
		{
			events.OnRefresh = new RelationshipManager.FFIEvents.RefreshHandler(RelationshipManager.OnRefreshImpl);
			events.OnRelationshipUpdate = new RelationshipManager.FFIEvents.RelationshipUpdateHandler(RelationshipManager.OnRelationshipUpdateImpl);
			Marshal.StructureToPtr<RelationshipManager.FFIEvents>(events, eventsPtr, false);
		}

		// Token: 0x06003650 RID: 13904 RVA: 0x001677A0 File Offset: 0x001659A0
		[MonoPInvokeCallback]
		private static bool FilterCallbackImpl(IntPtr ptr, ref Relationship relationship)
		{
			return ((RelationshipManager.FilterHandler)GCHandle.FromIntPtr(ptr).Target)(ref relationship);
		}

		// Token: 0x06003651 RID: 13905 RVA: 0x001677C8 File Offset: 0x001659C8
		public void Filter(RelationshipManager.FilterHandler callback)
		{
			GCHandle value = GCHandle.Alloc(callback);
			this.Methods.Filter(this.MethodsPtr, GCHandle.ToIntPtr(value), new RelationshipManager.FFIMethods.FilterCallback(RelationshipManager.FilterCallbackImpl));
			value.Free();
		}

		// Token: 0x06003652 RID: 13906 RVA: 0x0016780C File Offset: 0x00165A0C
		public int Count()
		{
			int result = 0;
			Result result2 = this.Methods.Count(this.MethodsPtr, ref result);
			if (result2 != Result.Ok)
			{
				throw new ResultException(result2);
			}
			return result;
		}

		// Token: 0x06003653 RID: 13907 RVA: 0x00167840 File Offset: 0x00165A40
		public Relationship Get(long userId)
		{
			Relationship result = default(Relationship);
			Result result2 = this.Methods.Get(this.MethodsPtr, userId, ref result);
			if (result2 != Result.Ok)
			{
				throw new ResultException(result2);
			}
			return result;
		}

		// Token: 0x06003654 RID: 13908 RVA: 0x0016787C File Offset: 0x00165A7C
		public Relationship GetAt(uint index)
		{
			Relationship result = default(Relationship);
			Result result2 = this.Methods.GetAt(this.MethodsPtr, index, ref result);
			if (result2 != Result.Ok)
			{
				throw new ResultException(result2);
			}
			return result;
		}

		// Token: 0x06003655 RID: 13909 RVA: 0x001678B8 File Offset: 0x00165AB8
		[MonoPInvokeCallback]
		private static void OnRefreshImpl(IntPtr ptr)
		{
			Discord discord = (Discord)GCHandle.FromIntPtr(ptr).Target;
			if (discord.RelationshipManagerInstance.OnRefresh != null)
			{
				discord.RelationshipManagerInstance.OnRefresh();
			}
		}

		// Token: 0x06003656 RID: 13910 RVA: 0x001678F8 File Offset: 0x00165AF8
		[MonoPInvokeCallback]
		private static void OnRelationshipUpdateImpl(IntPtr ptr, ref Relationship relationship)
		{
			Discord discord = (Discord)GCHandle.FromIntPtr(ptr).Target;
			if (discord.RelationshipManagerInstance.OnRelationshipUpdate != null)
			{
				discord.RelationshipManagerInstance.OnRelationshipUpdate(ref relationship);
			}
		}

		// Token: 0x0400230D RID: 8973
		private IntPtr MethodsPtr;

		// Token: 0x0400230E RID: 8974
		private object MethodsStructure;

		// Token: 0x02000855 RID: 2133
		internal struct FFIEvents
		{
			// Token: 0x04002EEB RID: 12011
			internal RelationshipManager.FFIEvents.RefreshHandler OnRefresh;

			// Token: 0x04002EEC RID: 12012
			internal RelationshipManager.FFIEvents.RelationshipUpdateHandler OnRelationshipUpdate;

			// Token: 0x020008EC RID: 2284
			// (Invoke) Token: 0x06004373 RID: 17267
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate void RefreshHandler(IntPtr ptr);

			// Token: 0x020008ED RID: 2285
			// (Invoke) Token: 0x06004377 RID: 17271
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate void RelationshipUpdateHandler(IntPtr ptr, ref Relationship relationship);
		}

		// Token: 0x02000856 RID: 2134
		internal struct FFIMethods
		{
			// Token: 0x04002EED RID: 12013
			internal RelationshipManager.FFIMethods.FilterMethod Filter;

			// Token: 0x04002EEE RID: 12014
			internal RelationshipManager.FFIMethods.CountMethod Count;

			// Token: 0x04002EEF RID: 12015
			internal RelationshipManager.FFIMethods.GetMethod Get;

			// Token: 0x04002EF0 RID: 12016
			internal RelationshipManager.FFIMethods.GetAtMethod GetAt;

			// Token: 0x020008EE RID: 2286
			// (Invoke) Token: 0x0600437B RID: 17275
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate bool FilterCallback(IntPtr ptr, ref Relationship relationship);

			// Token: 0x020008EF RID: 2287
			// (Invoke) Token: 0x0600437F RID: 17279
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate void FilterMethod(IntPtr methodsPtr, IntPtr callbackData, RelationshipManager.FFIMethods.FilterCallback callback);

			// Token: 0x020008F0 RID: 2288
			// (Invoke) Token: 0x06004383 RID: 17283
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate Result CountMethod(IntPtr methodsPtr, ref int count);

			// Token: 0x020008F1 RID: 2289
			// (Invoke) Token: 0x06004387 RID: 17287
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate Result GetMethod(IntPtr methodsPtr, long userId, ref Relationship relationship);

			// Token: 0x020008F2 RID: 2290
			// (Invoke) Token: 0x0600438B RID: 17291
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate Result GetAtMethod(IntPtr methodsPtr, uint index, ref Relationship relationship);
		}

		// Token: 0x02000857 RID: 2135
		// (Invoke) Token: 0x060041AE RID: 16814
		public delegate bool FilterHandler(ref Relationship relationship);

		// Token: 0x02000858 RID: 2136
		// (Invoke) Token: 0x060041B2 RID: 16818
		public delegate void RefreshHandler();

		// Token: 0x02000859 RID: 2137
		// (Invoke) Token: 0x060041B6 RID: 16822
		public delegate void RelationshipUpdateHandler(ref Relationship relationship);
	}
}
