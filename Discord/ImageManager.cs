using System;
using System.Runtime.InteropServices;
using UnityEngine;

namespace Discord
{
	// Token: 0x020004EB RID: 1259
	public class ImageManager
	{
		// Token: 0x17000747 RID: 1863
		// (get) Token: 0x0600363F RID: 13887 RVA: 0x0016741E File Offset: 0x0016561E
		private ImageManager.FFIMethods Methods
		{
			get
			{
				if (this.MethodsStructure == null)
				{
					this.MethodsStructure = Marshal.PtrToStructure(this.MethodsPtr, typeof(ImageManager.FFIMethods));
				}
				return (ImageManager.FFIMethods)this.MethodsStructure;
			}
		}

		// Token: 0x06003640 RID: 13888 RVA: 0x00167450 File Offset: 0x00165650
		internal ImageManager(IntPtr ptr, IntPtr eventsPtr, ref ImageManager.FFIEvents events)
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

		// Token: 0x06003641 RID: 13889 RVA: 0x0016749F File Offset: 0x0016569F
		private void InitEvents(IntPtr eventsPtr, ref ImageManager.FFIEvents events)
		{
			Marshal.StructureToPtr<ImageManager.FFIEvents>(events, eventsPtr, false);
		}

		// Token: 0x06003642 RID: 13890 RVA: 0x001674B0 File Offset: 0x001656B0
		[MonoPInvokeCallback]
		private static void FetchCallbackImpl(IntPtr ptr, Result result, ImageHandle handleResult)
		{
			GCHandle gchandle = GCHandle.FromIntPtr(ptr);
			ImageManager.FetchHandler fetchHandler = (ImageManager.FetchHandler)gchandle.Target;
			gchandle.Free();
			fetchHandler(result, handleResult);
		}

		// Token: 0x06003643 RID: 13891 RVA: 0x001674E0 File Offset: 0x001656E0
		public void Fetch(ImageHandle handle, bool refresh, ImageManager.FetchHandler callback)
		{
			GCHandle value = GCHandle.Alloc(callback);
			this.Methods.Fetch(this.MethodsPtr, handle, refresh, GCHandle.ToIntPtr(value), new ImageManager.FFIMethods.FetchCallback(ImageManager.FetchCallbackImpl));
		}

		// Token: 0x06003644 RID: 13892 RVA: 0x00167520 File Offset: 0x00165720
		public ImageDimensions GetDimensions(ImageHandle handle)
		{
			ImageDimensions result = default(ImageDimensions);
			Result result2 = this.Methods.GetDimensions(this.MethodsPtr, handle, ref result);
			if (result2 != Result.Ok)
			{
				throw new ResultException(result2);
			}
			return result;
		}

		// Token: 0x06003645 RID: 13893 RVA: 0x0016755C File Offset: 0x0016575C
		public void GetData(ImageHandle handle, byte[] data)
		{
			Result result = this.Methods.GetData(this.MethodsPtr, handle, data, data.Length);
			if (result != Result.Ok)
			{
				throw new ResultException(result);
			}
		}

		// Token: 0x06003646 RID: 13894 RVA: 0x0016758F File Offset: 0x0016578F
		public void Fetch(ImageHandle handle, ImageManager.FetchHandler callback)
		{
			this.Fetch(handle, false, callback);
		}

		// Token: 0x06003647 RID: 13895 RVA: 0x0016759C File Offset: 0x0016579C
		public byte[] GetData(ImageHandle handle)
		{
			ImageDimensions dimensions = this.GetDimensions(handle);
			byte[] array = new byte[dimensions.Width * dimensions.Height * 4U];
			this.GetData(handle, array);
			return array;
		}

		// Token: 0x06003648 RID: 13896 RVA: 0x001675D0 File Offset: 0x001657D0
		public Texture2D GetTexture(ImageHandle handle)
		{
			ImageDimensions dimensions = this.GetDimensions(handle);
			Texture2D texture2D = new Texture2D((int)dimensions.Width, (int)dimensions.Height, TextureFormat.RGBA32, false, true);
			texture2D.LoadRawTextureData(this.GetData(handle));
			texture2D.Apply();
			return texture2D;
		}

		// Token: 0x0400230B RID: 8971
		private IntPtr MethodsPtr;

		// Token: 0x0400230C RID: 8972
		private object MethodsStructure;

		// Token: 0x02000852 RID: 2130
		internal struct FFIEvents
		{
		}

		// Token: 0x02000853 RID: 2131
		internal struct FFIMethods
		{
			// Token: 0x04002EE8 RID: 12008
			internal ImageManager.FFIMethods.FetchMethod Fetch;

			// Token: 0x04002EE9 RID: 12009
			internal ImageManager.FFIMethods.GetDimensionsMethod GetDimensions;

			// Token: 0x04002EEA RID: 12010
			internal ImageManager.FFIMethods.GetDataMethod GetData;

			// Token: 0x020008E8 RID: 2280
			// (Invoke) Token: 0x06004363 RID: 17251
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate void FetchCallback(IntPtr ptr, Result result, ImageHandle handleResult);

			// Token: 0x020008E9 RID: 2281
			// (Invoke) Token: 0x06004367 RID: 17255
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate void FetchMethod(IntPtr methodsPtr, ImageHandle handle, bool refresh, IntPtr callbackData, ImageManager.FFIMethods.FetchCallback callback);

			// Token: 0x020008EA RID: 2282
			// (Invoke) Token: 0x0600436B RID: 17259
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate Result GetDimensionsMethod(IntPtr methodsPtr, ImageHandle handle, ref ImageDimensions dimensions);

			// Token: 0x020008EB RID: 2283
			// (Invoke) Token: 0x0600436F RID: 17263
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate Result GetDataMethod(IntPtr methodsPtr, ImageHandle handle, byte[] data, int dataLen);
		}

		// Token: 0x02000854 RID: 2132
		// (Invoke) Token: 0x060041AA RID: 16810
		public delegate void FetchHandler(Result result, ImageHandle handleResult);
	}
}
