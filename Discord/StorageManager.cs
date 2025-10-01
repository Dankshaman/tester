using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace Discord
{
	// Token: 0x020004F0 RID: 1264
	public class StorageManager
	{
		// Token: 0x1700074C RID: 1868
		// (get) Token: 0x060036C1 RID: 14017 RVA: 0x00169187 File Offset: 0x00167387
		private StorageManager.FFIMethods Methods
		{
			get
			{
				if (this.MethodsStructure == null)
				{
					this.MethodsStructure = Marshal.PtrToStructure(this.MethodsPtr, typeof(StorageManager.FFIMethods));
				}
				return (StorageManager.FFIMethods)this.MethodsStructure;
			}
		}

		// Token: 0x060036C2 RID: 14018 RVA: 0x001691B8 File Offset: 0x001673B8
		internal StorageManager(IntPtr ptr, IntPtr eventsPtr, ref StorageManager.FFIEvents events)
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

		// Token: 0x060036C3 RID: 14019 RVA: 0x00169207 File Offset: 0x00167407
		private void InitEvents(IntPtr eventsPtr, ref StorageManager.FFIEvents events)
		{
			Marshal.StructureToPtr<StorageManager.FFIEvents>(events, eventsPtr, false);
		}

		// Token: 0x060036C4 RID: 14020 RVA: 0x00169218 File Offset: 0x00167418
		public uint Read(string name, byte[] data)
		{
			uint result = 0U;
			Result result2 = this.Methods.Read(this.MethodsPtr, name, data, data.Length, ref result);
			if (result2 != Result.Ok)
			{
				throw new ResultException(result2);
			}
			return result;
		}

		// Token: 0x060036C5 RID: 14021 RVA: 0x00169250 File Offset: 0x00167450
		[MonoPInvokeCallback]
		private static void ReadAsyncCallbackImpl(IntPtr ptr, Result result, IntPtr dataPtr, int dataLen)
		{
			GCHandle gchandle = GCHandle.FromIntPtr(ptr);
			StorageManager.ReadAsyncHandler readAsyncHandler = (StorageManager.ReadAsyncHandler)gchandle.Target;
			gchandle.Free();
			byte[] array = new byte[dataLen];
			Marshal.Copy(dataPtr, array, 0, dataLen);
			readAsyncHandler(result, array);
		}

		// Token: 0x060036C6 RID: 14022 RVA: 0x00169290 File Offset: 0x00167490
		public void ReadAsync(string name, StorageManager.ReadAsyncHandler callback)
		{
			GCHandle value = GCHandle.Alloc(callback);
			this.Methods.ReadAsync(this.MethodsPtr, name, GCHandle.ToIntPtr(value), new StorageManager.FFIMethods.ReadAsyncCallback(StorageManager.ReadAsyncCallbackImpl));
		}

		// Token: 0x060036C7 RID: 14023 RVA: 0x001692D0 File Offset: 0x001674D0
		[MonoPInvokeCallback]
		private static void ReadAsyncPartialCallbackImpl(IntPtr ptr, Result result, IntPtr dataPtr, int dataLen)
		{
			GCHandle gchandle = GCHandle.FromIntPtr(ptr);
			StorageManager.ReadAsyncPartialHandler readAsyncPartialHandler = (StorageManager.ReadAsyncPartialHandler)gchandle.Target;
			gchandle.Free();
			byte[] array = new byte[dataLen];
			Marshal.Copy(dataPtr, array, 0, dataLen);
			readAsyncPartialHandler(result, array);
		}

		// Token: 0x060036C8 RID: 14024 RVA: 0x00169310 File Offset: 0x00167510
		public void ReadAsyncPartial(string name, ulong offset, ulong length, StorageManager.ReadAsyncPartialHandler callback)
		{
			GCHandle value = GCHandle.Alloc(callback);
			this.Methods.ReadAsyncPartial(this.MethodsPtr, name, offset, length, GCHandle.ToIntPtr(value), new StorageManager.FFIMethods.ReadAsyncPartialCallback(StorageManager.ReadAsyncPartialCallbackImpl));
		}

		// Token: 0x060036C9 RID: 14025 RVA: 0x00169350 File Offset: 0x00167550
		public void Write(string name, byte[] data)
		{
			Result result = this.Methods.Write(this.MethodsPtr, name, data, data.Length);
			if (result != Result.Ok)
			{
				throw new ResultException(result);
			}
		}

		// Token: 0x060036CA RID: 14026 RVA: 0x00169384 File Offset: 0x00167584
		[MonoPInvokeCallback]
		private static void WriteAsyncCallbackImpl(IntPtr ptr, Result result)
		{
			GCHandle gchandle = GCHandle.FromIntPtr(ptr);
			StorageManager.WriteAsyncHandler writeAsyncHandler = (StorageManager.WriteAsyncHandler)gchandle.Target;
			gchandle.Free();
			writeAsyncHandler(result);
		}

		// Token: 0x060036CB RID: 14027 RVA: 0x001693B4 File Offset: 0x001675B4
		public void WriteAsync(string name, byte[] data, StorageManager.WriteAsyncHandler callback)
		{
			GCHandle value = GCHandle.Alloc(callback);
			this.Methods.WriteAsync(this.MethodsPtr, name, data, data.Length, GCHandle.ToIntPtr(value), new StorageManager.FFIMethods.WriteAsyncCallback(StorageManager.WriteAsyncCallbackImpl));
		}

		// Token: 0x060036CC RID: 14028 RVA: 0x001693F8 File Offset: 0x001675F8
		public void Delete(string name)
		{
			Result result = this.Methods.Delete(this.MethodsPtr, name);
			if (result != Result.Ok)
			{
				throw new ResultException(result);
			}
		}

		// Token: 0x060036CD RID: 14029 RVA: 0x00169428 File Offset: 0x00167628
		public bool Exists(string name)
		{
			bool result = false;
			Result result2 = this.Methods.Exists(this.MethodsPtr, name, ref result);
			if (result2 != Result.Ok)
			{
				throw new ResultException(result2);
			}
			return result;
		}

		// Token: 0x060036CE RID: 14030 RVA: 0x0016945C File Offset: 0x0016765C
		public int Count()
		{
			int result = 0;
			this.Methods.Count(this.MethodsPtr, ref result);
			return result;
		}

		// Token: 0x060036CF RID: 14031 RVA: 0x00169484 File Offset: 0x00167684
		public FileStat Stat(string name)
		{
			FileStat result = default(FileStat);
			Result result2 = this.Methods.Stat(this.MethodsPtr, name, ref result);
			if (result2 != Result.Ok)
			{
				throw new ResultException(result2);
			}
			return result;
		}

		// Token: 0x060036D0 RID: 14032 RVA: 0x001694C0 File Offset: 0x001676C0
		public FileStat StatAt(int index)
		{
			FileStat result = default(FileStat);
			Result result2 = this.Methods.StatAt(this.MethodsPtr, index, ref result);
			if (result2 != Result.Ok)
			{
				throw new ResultException(result2);
			}
			return result;
		}

		// Token: 0x060036D1 RID: 14033 RVA: 0x001694FC File Offset: 0x001676FC
		public string GetPath()
		{
			StringBuilder stringBuilder = new StringBuilder(4096);
			Result result = this.Methods.GetPath(this.MethodsPtr, stringBuilder);
			if (result != Result.Ok)
			{
				throw new ResultException(result);
			}
			return stringBuilder.ToString();
		}

		// Token: 0x060036D2 RID: 14034 RVA: 0x0016953C File Offset: 0x0016773C
		public IEnumerable<FileStat> Files()
		{
			int num = this.Count();
			List<FileStat> list = new List<FileStat>();
			for (int i = 0; i < num; i++)
			{
				list.Add(this.StatAt(i));
			}
			return list;
		}

		// Token: 0x04002322 RID: 8994
		private IntPtr MethodsPtr;

		// Token: 0x04002323 RID: 8995
		private object MethodsStructure;

		// Token: 0x0200087A RID: 2170
		internal struct FFIEvents
		{
		}

		// Token: 0x0200087B RID: 2171
		internal struct FFIMethods
		{
			// Token: 0x04002F2B RID: 12075
			internal StorageManager.FFIMethods.ReadMethod Read;

			// Token: 0x04002F2C RID: 12076
			internal StorageManager.FFIMethods.ReadAsyncMethod ReadAsync;

			// Token: 0x04002F2D RID: 12077
			internal StorageManager.FFIMethods.ReadAsyncPartialMethod ReadAsyncPartial;

			// Token: 0x04002F2E RID: 12078
			internal StorageManager.FFIMethods.WriteMethod Write;

			// Token: 0x04002F2F RID: 12079
			internal StorageManager.FFIMethods.WriteAsyncMethod WriteAsync;

			// Token: 0x04002F30 RID: 12080
			internal StorageManager.FFIMethods.DeleteMethod Delete;

			// Token: 0x04002F31 RID: 12081
			internal StorageManager.FFIMethods.ExistsMethod Exists;

			// Token: 0x04002F32 RID: 12082
			internal StorageManager.FFIMethods.CountMethod Count;

			// Token: 0x04002F33 RID: 12083
			internal StorageManager.FFIMethods.StatMethod Stat;

			// Token: 0x04002F34 RID: 12084
			internal StorageManager.FFIMethods.StatAtMethod StatAt;

			// Token: 0x04002F35 RID: 12085
			internal StorageManager.FFIMethods.GetPathMethod GetPath;

			// Token: 0x0200093C RID: 2364
			// (Invoke) Token: 0x060044B3 RID: 17587
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate Result ReadMethod(IntPtr methodsPtr, [MarshalAs(UnmanagedType.LPStr)] string name, byte[] data, int dataLen, ref uint read);

			// Token: 0x0200093D RID: 2365
			// (Invoke) Token: 0x060044B7 RID: 17591
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate void ReadAsyncCallback(IntPtr ptr, Result result, IntPtr dataPtr, int dataLen);

			// Token: 0x0200093E RID: 2366
			// (Invoke) Token: 0x060044BB RID: 17595
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate void ReadAsyncMethod(IntPtr methodsPtr, [MarshalAs(UnmanagedType.LPStr)] string name, IntPtr callbackData, StorageManager.FFIMethods.ReadAsyncCallback callback);

			// Token: 0x0200093F RID: 2367
			// (Invoke) Token: 0x060044BF RID: 17599
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate void ReadAsyncPartialCallback(IntPtr ptr, Result result, IntPtr dataPtr, int dataLen);

			// Token: 0x02000940 RID: 2368
			// (Invoke) Token: 0x060044C3 RID: 17603
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate void ReadAsyncPartialMethod(IntPtr methodsPtr, [MarshalAs(UnmanagedType.LPStr)] string name, ulong offset, ulong length, IntPtr callbackData, StorageManager.FFIMethods.ReadAsyncPartialCallback callback);

			// Token: 0x02000941 RID: 2369
			// (Invoke) Token: 0x060044C7 RID: 17607
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate Result WriteMethod(IntPtr methodsPtr, [MarshalAs(UnmanagedType.LPStr)] string name, byte[] data, int dataLen);

			// Token: 0x02000942 RID: 2370
			// (Invoke) Token: 0x060044CB RID: 17611
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate void WriteAsyncCallback(IntPtr ptr, Result result);

			// Token: 0x02000943 RID: 2371
			// (Invoke) Token: 0x060044CF RID: 17615
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate void WriteAsyncMethod(IntPtr methodsPtr, [MarshalAs(UnmanagedType.LPStr)] string name, byte[] data, int dataLen, IntPtr callbackData, StorageManager.FFIMethods.WriteAsyncCallback callback);

			// Token: 0x02000944 RID: 2372
			// (Invoke) Token: 0x060044D3 RID: 17619
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate Result DeleteMethod(IntPtr methodsPtr, [MarshalAs(UnmanagedType.LPStr)] string name);

			// Token: 0x02000945 RID: 2373
			// (Invoke) Token: 0x060044D7 RID: 17623
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate Result ExistsMethod(IntPtr methodsPtr, [MarshalAs(UnmanagedType.LPStr)] string name, ref bool exists);

			// Token: 0x02000946 RID: 2374
			// (Invoke) Token: 0x060044DB RID: 17627
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate void CountMethod(IntPtr methodsPtr, ref int count);

			// Token: 0x02000947 RID: 2375
			// (Invoke) Token: 0x060044DF RID: 17631
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate Result StatMethod(IntPtr methodsPtr, [MarshalAs(UnmanagedType.LPStr)] string name, ref FileStat stat);

			// Token: 0x02000948 RID: 2376
			// (Invoke) Token: 0x060044E3 RID: 17635
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate Result StatAtMethod(IntPtr methodsPtr, int index, ref FileStat stat);

			// Token: 0x02000949 RID: 2377
			// (Invoke) Token: 0x060044E7 RID: 17639
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate Result GetPathMethod(IntPtr methodsPtr, StringBuilder path);
		}

		// Token: 0x0200087C RID: 2172
		// (Invoke) Token: 0x06004222 RID: 16930
		public delegate void ReadAsyncHandler(Result result, byte[] data);

		// Token: 0x0200087D RID: 2173
		// (Invoke) Token: 0x06004226 RID: 16934
		public delegate void ReadAsyncPartialHandler(Result result, byte[] data);

		// Token: 0x0200087E RID: 2174
		// (Invoke) Token: 0x0600422A RID: 16938
		public delegate void WriteAsyncHandler(Result result);
	}
}
