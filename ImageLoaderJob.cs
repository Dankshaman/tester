using System;
using System.Runtime.InteropServices;
using System.Text;
using UnityEngine;

// Token: 0x02000132 RID: 306
public class ImageLoaderJob : ThreadedJob
{
	// Token: 0x0600101F RID: 4127
	[DllImport("TTSMasterNativePlugin")]
	private static extern IntPtr ConvertToDXTFromJPGOrPNG(IntPtr srcBytes, int sizeOfSrc, int flags, int max_supported_size, IntPtr out_size, IntPtr dxt_format, IntPtr width_original, IntPtr height_original, IntPtr width_new, IntPtr height_new);

	// Token: 0x06001020 RID: 4128
	[DllImport("TTSMasterNativePlugin")]
	private static extern IntPtr ConvertToDXTSquish(IntPtr rgba, IntPtr width, IntPtr height, int channels, int flags, int max_supported_size, IntPtr out_size, IntPtr dxt_format, IntPtr width_new, IntPtr height_new, bool alpha, int quality);

	// Token: 0x06001021 RID: 4129
	[DllImport("TTSMasterNativePlugin")]
	private static extern IntPtr ConvertToDXT(IntPtr rgba, IntPtr width, IntPtr height, int channels, int flags, int max_supported_size, IntPtr out_size, IntPtr dxt_format, IntPtr width_new, IntPtr height_new);

	// Token: 0x06001022 RID: 4130
	[DllImport("TTSMasterNativePlugin")]
	private static extern IntPtr ConvertJPGToRGB(IntPtr srcBytes, int sizeOfSrc, IntPtr width_original, IntPtr height_original);

	// Token: 0x06001023 RID: 4131
	[DllImport("TTSMasterNativePlugin")]
	private static extern IntPtr ConvertJPGToRGBA(IntPtr srcBytes, int sizeOfSrc, IntPtr width_original, IntPtr height_original);

	// Token: 0x06001024 RID: 4132
	[DllImport("TTSMasterNativePlugin")]
	private static extern IntPtr ConvertPNGToRGBA(IntPtr srcBytes, int sizeOfSrc, IntPtr width_original, IntPtr height_original, IntPtr transparency);

	// Token: 0x06001025 RID: 4133
	[DllImport("TTSMasterNativePlugin")]
	private static extern void GetJPGSize(IntPtr srcBytes, int sizeOfSrc, IntPtr width_original, IntPtr height_original);

	// Token: 0x06001026 RID: 4134
	[DllImport("TTSMasterNativePlugin")]
	private static extern void GetPNGSize(IntPtr srcBytes, int sizeOfSrc, IntPtr width_original, IntPtr height_original);

	// Token: 0x06001027 RID: 4135 RVA: 0x0006D9BC File Offset: 0x0006BBBC
	protected override void ThreadFunction()
	{
		this.initialFormat = FileMagicNumbers.GetFileFormat(this.InRawData);
		if (this.initialFormat == FileMagicNumbers.FileFormat.JPG && ImageLoaderJob.jpg_exif_magic.IsMatch(this.InRawData) && this.InRawData.IndexOfSequence(ImageLoaderJob.jpg_cmyk_sequence) != -1)
		{
			this.FailGracefully("Image Loader does not support CMYK JPG.");
			return;
		}
		GCHandle gchandle = GCHandle.Alloc(this.InRawData, GCHandleType.Pinned);
		IntPtr srcBytes = gchandle.AddrOfPinnedObject();
		this.out_sizePtr = Marshal.AllocHGlobal(4);
		this.dxt_formatPtr = Marshal.AllocHGlobal(4);
		this.width_originalPtr = Marshal.AllocHGlobal(4);
		this.height_originalPtr = Marshal.AllocHGlobal(4);
		this.width_newPtr = Marshal.AllocHGlobal(4);
		this.height_newPtr = Marshal.AllocHGlobal(4);
		if (this.initialFormat == FileMagicNumbers.FileFormat.JPG)
		{
			this.rgbaPtr = ImageLoaderJob.ConvertJPGToRGB(srcBytes, this.InRawData.Length, this.width_originalPtr, this.height_originalPtr);
			gchandle.Free();
			this.width_original = Marshal.ReadInt32(this.width_originalPtr);
			this.height_original = Marshal.ReadInt32(this.height_originalPtr);
			if (this.height_original > 10000 || this.width_original > 10000)
			{
				this.FailGracefully("Image is too large, width or height greater than 10k pixels.");
				return;
			}
			if (this.height_original < 0 || this.width_original < 0)
			{
				this.FailGracefully("Error converting image to RGB.");
				return;
			}
			if (this.compress)
			{
				this.dxtPtr = ImageLoaderJob.ConvertToDXT(this.rgbaPtr, this.width_originalPtr, this.height_originalPtr, 3, 50, this.image_max_size, this.out_sizePtr, this.dxt_formatPtr, this.width_newPtr, this.height_newPtr);
			}
		}
		else
		{
			if (this.initialFormat != FileMagicNumbers.FileFormat.PNG)
			{
				gchandle.Free();
				this.FailGracefully("Image Loader unknown format.");
				return;
			}
			this.png_transparencyPtr = Marshal.AllocHGlobal(4);
			this.rgbaPtr = ImageLoaderJob.ConvertPNGToRGBA(srcBytes, this.InRawData.Length, this.width_originalPtr, this.height_originalPtr, this.png_transparencyPtr);
			gchandle.Free();
			this.width_original = Marshal.ReadInt32(this.width_originalPtr);
			this.height_original = Marshal.ReadInt32(this.height_originalPtr);
			if (this.height_original > 10000 || this.width_original > 10000)
			{
				this.FailGracefully("Image is too large, width or height greater than 10k pixels.");
				return;
			}
			if (this.height_original < 0 || this.width_original < 0)
			{
				this.FailGracefully("Error converting image to RGB.");
				return;
			}
			this.png_transparency = Marshal.ReadInt32(this.png_transparencyPtr);
			this.color_channels = 4;
			if (this.png_transparency == 0)
			{
				this.color_channels = 3;
			}
			else if (this.png_transparency == 1)
			{
				this.color_channels = 4;
			}
			else if (this.png_transparency == 2)
			{
				this.color_channels = 3;
			}
			else if (this.png_transparency == 3)
			{
				this.color_channels = 4;
			}
			if (this.compress)
			{
				this.dxtPtr = ImageLoaderJob.ConvertToDXT(this.rgbaPtr, this.width_originalPtr, this.height_originalPtr, this.color_channels, 50, this.image_max_size, this.out_sizePtr, this.dxt_formatPtr, this.width_newPtr, this.height_newPtr);
			}
		}
		if (this.compress)
		{
			this.width_new = Marshal.ReadInt32(this.width_newPtr);
			this.height_new = Marshal.ReadInt32(this.height_newPtr);
			if (this.width_new < 1 || this.height_new < 1)
			{
				this.FailGracefully("Error converting image to DXT");
				return;
			}
			this.out_size = Marshal.ReadInt32(this.out_sizePtr);
			this.OutRawData = new byte[this.out_size];
			Marshal.Copy(this.dxtPtr, this.OutRawData, 0, this.OutRawData.Length);
			if (Marshal.ReadInt32(this.dxt_formatPtr) == 5)
			{
				this.format = TextureFormat.DXT5;
			}
		}
		else
		{
			this.width_new = this.width_original;
			this.height_new = this.height_original;
			bool flag = false;
			int num;
			if (this.color_channels == 4)
			{
				num = 4;
				this.format = TextureFormat.RGBA32;
			}
			else if (this.normalMap)
			{
				flag = true;
				num = 4;
				this.format = TextureFormat.RGBA32;
			}
			else
			{
				num = 3;
				this.format = TextureFormat.RGB24;
			}
			int num2 = num * this.width_original * this.height_original;
			if (this.mipMaps && this.width_original > 2 && this.height_original > 2)
			{
				this.out_size = num2;
				int num3 = this.width_original;
				int num4 = this.height_original;
				int num5 = 0;
				while (num3 > 1 || num4 > 1)
				{
					num3 /= 2;
					num4 /= 2;
					num3 = Mathf.Max(num3, 1);
					num4 = Mathf.Max(num4, 1);
					this.out_size += num * num3 * num4;
					num5++;
				}
				this.OutRawData = new byte[this.out_size];
				if (!flag)
				{
					int num6 = num * this.width_original;
					for (int i = 0; i < this.height_original; i++)
					{
						for (int j = 0; j < num6; j++)
						{
							this.OutRawData[num2 - (i + 1) * num6 + j] = Marshal.ReadByte(this.rgbaPtr, i * num6 + j);
						}
					}
				}
				else
				{
					int num7 = num * this.width_original;
					int num8 = 3 * this.width_original;
					int num9 = 0;
					int num10 = 0;
					for (int k = 0; k < this.height_original; k++)
					{
						for (int l = 0; l < num7; l++)
						{
							if (num9 < 3)
							{
								this.OutRawData[num2 - (k + 1) * num7 + l] = Marshal.ReadByte(this.rgbaPtr, k * num8 + l - num10);
								num9++;
							}
							else
							{
								if (!this.normalMap)
								{
									this.OutRawData[num2 - (k + 1) * num7 + l] = byte.MaxValue;
								}
								num9 = 0;
								num10++;
							}
						}
						num10 = 0;
					}
				}
				if (this.normalMap)
				{
					for (int m = 0; m < this.height_original; m++)
					{
						for (int n = 0; n < this.width_original; n++)
						{
							int num11 = m * this.width_original * num + n * num;
							byte b = this.OutRawData[num11];
							this.OutRawData[num11] = 0;
							this.OutRawData[num11 + 2] = 0;
							this.OutRawData[num11 + 3] = b;
						}
					}
				}
				int num12 = num2;
				num3 = this.width_original;
				num4 = this.height_original;
				for (int num13 = 0; num13 < num5; num13++)
				{
					num3 /= 2;
					num4 /= 2;
					num3 = Mathf.Max(num3, 1);
					num4 = Mathf.Max(num4, 1);
					int num14 = num3 * num4;
					int num15 = num3;
					int num16 = num4;
					Vector2 vector = new Vector2((float)this.width_original, (float)this.height_original);
					Vector2 vector2 = default(Vector2);
					Vector2 vector3 = new Vector2(vector.x / (float)num15, vector.y / (float)num16);
					for (int num17 = 0; num17 < num14; num17++)
					{
						float num18 = (float)num17 % (float)num15;
						float num19 = Mathf.Floor((float)num17 / (float)num15);
						vector2.x = num18 / (float)num15 * vector.x;
						vector2.y = num19 / (float)num16 * vector.y;
						Color32 color = default(Color32);
						switch (this.mipMapFilter)
						{
						case ImageLoaderJob.ImageFilterMode.Nearest:
						{
							int num20 = (int)(vector2.y * vector.x + vector2.x);
							color = new Color32(this.OutRawData[num20 * num], this.OutRawData[num20 * num + 1], this.OutRawData[num20 * num + 2], (num == 4) ? this.OutRawData[num20 * num + 3] : byte.MaxValue);
							break;
						}
						case ImageLoaderJob.ImageFilterMode.Bilinear:
						{
							float t = vector2.x - Mathf.Floor(vector2.x);
							float t2 = vector2.y - Mathf.Floor(vector2.y);
							int num21 = (int)(Mathf.Floor(vector2.y) * vector.x + Mathf.Floor(vector2.x));
							int num22 = (int)(Mathf.Floor(vector2.y) * vector.x + Mathf.Ceil(vector2.x));
							int num23 = (int)(Mathf.Ceil(vector2.y) * vector.x + Mathf.Floor(vector2.x));
							int num24 = (int)(Mathf.Ceil(vector2.y) * vector.x + Mathf.Ceil(vector2.x));
							Color32 a = new Color32(this.OutRawData[num21 * num], this.OutRawData[num21 * num + 1], this.OutRawData[num21 * num + 2], (num == 4) ? this.OutRawData[num21 * num + 3] : byte.MaxValue);
							Color32 b2 = new Color32(this.OutRawData[num22 * num], this.OutRawData[num22 * num + 1], this.OutRawData[num22 * num + 2], (num == 4) ? this.OutRawData[num22 * num + 3] : byte.MaxValue);
							Color32 a2 = new Color32(this.OutRawData[num23 * num], this.OutRawData[num23 * num + 1], this.OutRawData[num23 * num + 2], (num == 4) ? this.OutRawData[num23 * num + 3] : byte.MaxValue);
							Color32 b3 = new Color32(this.OutRawData[num24 * num], this.OutRawData[num24 * num + 1], this.OutRawData[num24 * num + 2], (num == 4) ? this.OutRawData[num24 * num + 3] : byte.MaxValue);
							color = Color32.Lerp(Color32.Lerp(a, b2, t), Color32.Lerp(a2, b3, t), t2);
							break;
						}
						case ImageLoaderJob.ImageFilterMode.Average:
						{
							int num25 = (int)Mathf.Max(Mathf.Floor(vector2.x - vector3.x * 0.5f), 0f);
							int num26 = (int)Mathf.Min(Mathf.Ceil(vector2.x + vector3.x * 0.5f), vector.x);
							int num27 = (int)Mathf.Max(Mathf.Floor(vector2.y - vector3.y * 0.5f), 0f);
							int num28 = (int)Mathf.Min(Mathf.Ceil(vector2.y + vector3.y * 0.5f), vector.y);
							Color a3 = default(Color);
							float num29 = 0f;
							for (int num30 = num27; num30 < num28; num30++)
							{
								for (int num31 = num25; num31 < num26; num31++)
								{
									int num32 = (int)((float)num30 * vector.x + (float)num31);
									a3 += new Color32(this.OutRawData[num32 * num], this.OutRawData[num32 * num + 1], this.OutRawData[num32 * num + 2], (num == 4) ? this.OutRawData[num32 * num + 3] : byte.MaxValue);
									num29 += 1f;
								}
							}
							color = a3 / num29;
							break;
						}
						}
						this.OutRawData[num12 + num17 * num] = color.r;
						this.OutRawData[num12 + num17 * num + 1] = color.g;
						this.OutRawData[num12 + num17 * num + 2] = color.b;
						if (num == 4)
						{
							this.OutRawData[num12 + num17 * num + 3] = color.a;
						}
					}
					num12 += num * num14;
				}
			}
			else
			{
				this.out_size = num2;
				this.OutRawData = new byte[this.out_size];
				if (!flag)
				{
					int num33 = num * this.width_original;
					for (int num34 = 0; num34 < this.height_original; num34++)
					{
						for (int num35 = 0; num35 < num33; num35++)
						{
							this.OutRawData[num2 - (num34 + 1) * num33 + num35] = Marshal.ReadByte(this.rgbaPtr, num34 * num33 + num35);
						}
					}
				}
				else
				{
					int num36 = num * this.width_original;
					int num37 = 3 * this.width_original;
					int num38 = 0;
					int num39 = 0;
					for (int num40 = 0; num40 < this.height_original; num40++)
					{
						for (int num41 = 0; num41 < num36; num41++)
						{
							if (num38 < 3)
							{
								this.OutRawData[num2 - (num40 + 1) * num36 + num41] = Marshal.ReadByte(this.rgbaPtr, num40 * num37 + num41 - num39);
								num38++;
							}
							else
							{
								if (!this.normalMap)
								{
									this.OutRawData[num2 - (num40 + 1) * num36 + num41] = byte.MaxValue;
								}
								num38 = 0;
								num39++;
							}
						}
						num39 = 0;
					}
				}
				if (this.normalMap)
				{
					for (int num42 = 0; num42 < this.height_original; num42++)
					{
						for (int num43 = 0; num43 < this.width_original; num43++)
						{
							int num44 = num42 * this.width_original * num + num43 * num;
							byte b4 = this.OutRawData[num44];
							this.OutRawData[num44] = 0;
							this.OutRawData[num44 + 2] = 0;
							this.OutRawData[num44 + 3] = b4;
						}
					}
				}
			}
		}
		this.FreeAll();
	}

	// Token: 0x06001028 RID: 4136 RVA: 0x000025B8 File Offset: 0x000007B8
	protected override void OnFinished()
	{
	}

	// Token: 0x06001029 RID: 4137 RVA: 0x0006E687 File Offset: 0x0006C887
	private void FailGracefully(string error)
	{
		this.FreeAll();
		this.width_new = 0;
		this.height_new = 0;
		base.isError = true;
		this.errorMessage = error;
	}

	// Token: 0x0600102A RID: 4138 RVA: 0x0006E6AC File Offset: 0x0006C8AC
	private void FreeAll()
	{
		if (this.out_sizePtr != IntPtr.Zero)
		{
			Marshal.FreeHGlobal(this.out_sizePtr);
			this.out_sizePtr = IntPtr.Zero;
		}
		if (this.dxtPtr != IntPtr.Zero)
		{
			Marshal.FreeHGlobal(this.dxtPtr);
			this.dxtPtr = IntPtr.Zero;
		}
		if (this.dxt_formatPtr != IntPtr.Zero)
		{
			Marshal.FreeHGlobal(this.dxt_formatPtr);
			this.dxt_formatPtr = IntPtr.Zero;
		}
		if (this.width_originalPtr != IntPtr.Zero)
		{
			Marshal.FreeHGlobal(this.width_originalPtr);
			this.width_originalPtr = IntPtr.Zero;
		}
		if (this.height_originalPtr != IntPtr.Zero)
		{
			Marshal.FreeHGlobal(this.height_originalPtr);
			this.height_originalPtr = IntPtr.Zero;
		}
		if (this.width_newPtr != IntPtr.Zero)
		{
			Marshal.FreeHGlobal(this.width_newPtr);
			this.width_newPtr = IntPtr.Zero;
		}
		if (this.height_newPtr != IntPtr.Zero)
		{
			Marshal.FreeHGlobal(this.height_newPtr);
			this.height_newPtr = IntPtr.Zero;
		}
		if (this.rgbaPtr != IntPtr.Zero)
		{
			Marshal.FreeHGlobal(this.rgbaPtr);
			this.rgbaPtr = IntPtr.Zero;
		}
		if (this.png_transparencyPtr != IntPtr.Zero)
		{
			Marshal.FreeHGlobal(this.png_transparencyPtr);
			this.png_transparencyPtr = IntPtr.Zero;
		}
	}

	// Token: 0x0600102B RID: 4139 RVA: 0x0006E824 File Offset: 0x0006CA24
	public override void Reset()
	{
		base.Reset();
		this.InRawData = null;
		this.OutRawData = null;
		this.height_original = 4096;
		this.width_original = 4096;
		this.height_new = 4096;
		this.width_new = 4096;
		this.mipMaps = true;
		this.format = TextureFormat.DXT1;
		this.initialFormat = FileMagicNumbers.FileFormat.UNKNOWN;
		this.image_max_size = 8192;
		this.color_channels = 3;
		this.png_transparency = 0;
		this.compress = true;
		this.normalMap = false;
		this.mipMapFilter = ImageLoaderJob.ImageFilterMode.Bilinear;
		base.isError = false;
		this.FreeAll();
	}

	// Token: 0x04000A04 RID: 2564
	public byte[] InRawData;

	// Token: 0x04000A05 RID: 2565
	public byte[] OutRawData;

	// Token: 0x04000A06 RID: 2566
	public int height_original = 4096;

	// Token: 0x04000A07 RID: 2567
	public int width_original = 4096;

	// Token: 0x04000A08 RID: 2568
	public int height_new = 4096;

	// Token: 0x04000A09 RID: 2569
	public int width_new = 4096;

	// Token: 0x04000A0A RID: 2570
	public bool mipMaps = true;

	// Token: 0x04000A0B RID: 2571
	public TextureFormat format = TextureFormat.DXT1;

	// Token: 0x04000A0C RID: 2572
	public FileMagicNumbers.FileFormat initialFormat;

	// Token: 0x04000A0D RID: 2573
	public int image_max_size = 8192;

	// Token: 0x04000A0E RID: 2574
	public int color_channels = 3;

	// Token: 0x04000A0F RID: 2575
	public int png_transparency;

	// Token: 0x04000A10 RID: 2576
	public int out_size;

	// Token: 0x04000A11 RID: 2577
	public bool compress = true;

	// Token: 0x04000A12 RID: 2578
	public bool normalMap;

	// Token: 0x04000A13 RID: 2579
	public ImageLoaderJob.ImageFilterMode mipMapFilter = ImageLoaderJob.ImageFilterMode.Bilinear;

	// Token: 0x04000A14 RID: 2580
	private IntPtr out_sizePtr;

	// Token: 0x04000A15 RID: 2581
	private IntPtr dxt_formatPtr;

	// Token: 0x04000A16 RID: 2582
	private IntPtr width_originalPtr;

	// Token: 0x04000A17 RID: 2583
	private IntPtr height_originalPtr;

	// Token: 0x04000A18 RID: 2584
	private IntPtr width_newPtr;

	// Token: 0x04000A19 RID: 2585
	private IntPtr height_newPtr;

	// Token: 0x04000A1A RID: 2586
	public IntPtr dxtPtr;

	// Token: 0x04000A1B RID: 2587
	private IntPtr rgbaPtr;

	// Token: 0x04000A1C RID: 2588
	private IntPtr png_transparencyPtr;

	// Token: 0x04000A1D RID: 2589
	private static FileMagicNumbers jpg_exif_magic = new FileMagicNumbers(FileMagicNumbers.FileFormat.JPG, new byte?[]
	{
		new byte?((byte)255),
		new byte?((byte)216),
		new byte?((byte)255),
		new byte?((byte)225),
		null,
		null,
		new byte?((byte)69),
		new byte?((byte)120),
		new byte?((byte)105),
		new byte?((byte)102),
		new byte?(0),
		new byte?(0)
	}, 0);

	// Token: 0x04000A1E RID: 2590
	private static readonly byte[] jpg_cmyk_sequence = Encoding.ASCII.GetBytes("prtrCMYKLab");

	// Token: 0x02000642 RID: 1602
	public enum ImageFilterMode
	{
		// Token: 0x04002767 RID: 10087
		Nearest,
		// Token: 0x04002768 RID: 10088
		Bilinear,
		// Token: 0x04002769 RID: 10089
		Average
	}

	// Token: 0x02000643 RID: 1603
	public enum SOIL_FLAGS
	{
		// Token: 0x0400276B RID: 10091
		SOIL_FLAG_POWER_OF_TWO = 1,
		// Token: 0x0400276C RID: 10092
		SOIL_FLAG_MIPMAPS,
		// Token: 0x0400276D RID: 10093
		SOIL_FLAG_MULTIPLY_ALPHA = 8,
		// Token: 0x0400276E RID: 10094
		SOIL_FLAG_INVERT_Y = 16,
		// Token: 0x0400276F RID: 10095
		SOIL_FLAG_COMPRESS_TO_DXT = 32,
		// Token: 0x04002770 RID: 10096
		SOIL_FLAG_NEAREST_POWER_OF_TWO = 1024
	}

	// Token: 0x02000644 RID: 1604
	public enum SQUISH_FLAGS
	{
		// Token: 0x04002772 RID: 10098
		kDxt1 = 1,
		// Token: 0x04002773 RID: 10099
		kDxt3,
		// Token: 0x04002774 RID: 10100
		kDxt5 = 4,
		// Token: 0x04002775 RID: 10101
		kColourIterativeClusterFit = 256,
		// Token: 0x04002776 RID: 10102
		kColourClusterFit = 8,
		// Token: 0x04002777 RID: 10103
		kColourRangeFit = 16,
		// Token: 0x04002778 RID: 10104
		kWeightColourByAlpha = 128
	}
}
