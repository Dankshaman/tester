using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

// Token: 0x020000E1 RID: 225
public struct FileMagicNumbers
{
	// Token: 0x170001DA RID: 474
	// (get) Token: 0x06000AF2 RID: 2802 RVA: 0x0004BE8E File Offset: 0x0004A08E
	// (set) Token: 0x06000AF3 RID: 2803 RVA: 0x0004BE96 File Offset: 0x0004A096
	public FileMagicNumbers.FileFormat format { get; private set; }

	// Token: 0x170001DB RID: 475
	// (get) Token: 0x06000AF4 RID: 2804 RVA: 0x0004BE9F File Offset: 0x0004A09F
	// (set) Token: 0x06000AF5 RID: 2805 RVA: 0x0004BEA7 File Offset: 0x0004A0A7
	public byte?[] magicNumbers { get; private set; }

	// Token: 0x170001DC RID: 476
	// (get) Token: 0x06000AF6 RID: 2806 RVA: 0x0004BEB0 File Offset: 0x0004A0B0
	// (set) Token: 0x06000AF7 RID: 2807 RVA: 0x0004BEB8 File Offset: 0x0004A0B8
	public int offset { get; private set; }

	// Token: 0x06000AF8 RID: 2808 RVA: 0x0004BEC1 File Offset: 0x0004A0C1
	public FileMagicNumbers(FileMagicNumbers.FileFormat format, byte?[] magicNumbers, int offset = 0)
	{
		this.format = format;
		this.magicNumbers = magicNumbers;
		this.offset = offset;
	}

	// Token: 0x06000AF9 RID: 2809 RVA: 0x0004BED8 File Offset: 0x0004A0D8
	public FileMagicNumbers(FileMagicNumbers.FileFormat format, string magicName, int offset = 0)
	{
		this.format = format;
		this.magicNumbers = new byte?[magicName.Length];
		for (int i = 0; i < magicName.Length; i++)
		{
			this.magicNumbers[i] = new byte?(Convert.ToByte(magicName[i]));
		}
		this.offset = offset;
	}

	// Token: 0x06000AFA RID: 2810 RVA: 0x0004BF34 File Offset: 0x0004A134
	public bool IsMatch(byte[] data)
	{
		if (data == null)
		{
			return false;
		}
		if (data.Length < this.magicNumbers.Length + this.offset)
		{
			return false;
		}
		for (int i = 0; i < this.magicNumbers.Length; i++)
		{
			if (this.magicNumbers[i] != null && data[i + this.offset] != this.magicNumbers[i].Value)
			{
				return false;
			}
		}
		return true;
	}

	// Token: 0x06000AFB RID: 2811 RVA: 0x0004BFA4 File Offset: 0x0004A1A4
	public static FileMagicNumbers.FileFormat GetFileFormat(byte[] data)
	{
		if (data == null)
		{
			return FileMagicNumbers.FileFormat.UNKNOWN;
		}
		for (int i = 0; i < FileMagicNumbers.AllFileMagicNumbers.Length; i++)
		{
			FileMagicNumbers fileMagicNumbers = FileMagicNumbers.AllFileMagicNumbers[i];
			if (fileMagicNumbers.IsMatch(data))
			{
				return fileMagicNumbers.format;
			}
		}
		int num = (FileMagicNumbers.MaxMagicNumberLength() > data.Length) ? data.Length : FileMagicNumbers.MaxMagicNumberLength();
		Debug.Log("Unknown FileFormat: " + BitConverter.ToString(data, 0, num) + " : " + Encoding.ASCII.GetString(data, 0, num));
		return FileMagicNumbers.FileFormat.UNKNOWN;
	}

	// Token: 0x06000AFC RID: 2812 RVA: 0x0004C028 File Offset: 0x0004A228
	public static bool GetImageDimensions(byte[] data, FileMagicNumbers.FileFormat format, out int width, out int height)
	{
		width = (height = 0);
		if (format != FileMagicNumbers.FileFormat.JPG)
		{
			if (format == FileMagicNumbers.FileFormat.PNG)
			{
				width = LibBytes.ReadLittleEndianInt32(data, 16);
				height = LibBytes.ReadLittleEndianInt32(data, 20);
				return true;
			}
			if (format == FileMagicNumbers.FileFormat.GIF)
			{
				width = (int)BitConverter.ToInt16(data, 6);
				height = (int)BitConverter.ToInt16(data, 8);
				return true;
			}
		}
		else
		{
			int i = 2;
			while (i < data.Length)
			{
				if (data[i++] != 255)
				{
					break;
				}
				int num = (int)data[i++];
				short num2 = LibBytes.ReadLittleEndianInt16(data, ref i);
				if (num == 192)
				{
					i++;
					height = (int)LibBytes.ReadLittleEndianInt16(data, ref i);
					width = (int)LibBytes.ReadLittleEndianInt16(data, ref i);
					return true;
				}
				if (num2 < 0)
				{
					ushort num3 = (ushort)num2;
					i += (int)(num3 - 2);
				}
				else
				{
					i += (int)(num2 - 2);
				}
			}
		}
		return false;
	}

	// Token: 0x06000AFD RID: 2813 RVA: 0x0004C0D8 File Offset: 0x0004A2D8
	public static FileMagicNumbers.FileFormat GetFileFormat(string path)
	{
		foreach (KeyValuePair<string, FileMagicNumbers.FileFormat> keyValuePair in FileMagicNumbers.FileFormatExtensions)
		{
			if (path.EndsWith(keyValuePair.Key, StringComparison.OrdinalIgnoreCase))
			{
				return keyValuePair.Value;
			}
		}
		return FileMagicNumbers.FileFormat.UNKNOWN;
	}

	// Token: 0x06000AFE RID: 2814 RVA: 0x0004C13C File Offset: 0x0004A33C
	public static string GetExtension(FileMagicNumbers.FileFormat format)
	{
		foreach (KeyValuePair<string, FileMagicNumbers.FileFormat> keyValuePair in FileMagicNumbers.FileFormatExtensions)
		{
			if (keyValuePair.Value == format)
			{
				return keyValuePair.Key;
			}
		}
		return null;
	}

	// Token: 0x06000AFF RID: 2815 RVA: 0x0004C198 File Offset: 0x0004A398
	public static int MaxMagicNumberLength()
	{
		if (FileMagicNumbers.cachedMaxMagicNumberLength > 0)
		{
			return FileMagicNumbers.cachedMaxMagicNumberLength;
		}
		for (int i = 0; i < FileMagicNumbers.AllFileMagicNumbers.Length; i++)
		{
			FileMagicNumbers fileMagicNumbers = FileMagicNumbers.AllFileMagicNumbers[i];
			int num = fileMagicNumbers.magicNumbers.Length + fileMagicNumbers.offset;
			if (num > FileMagicNumbers.cachedMaxMagicNumberLength)
			{
				FileMagicNumbers.cachedMaxMagicNumberLength = num;
			}
		}
		return FileMagicNumbers.cachedMaxMagicNumberLength;
	}

	// Token: 0x040007AE RID: 1966
	public static readonly Listionary<string, FileMagicNumbers.FileFormat> FileFormatExtensions = new Listionary<string, FileMagicNumbers.FileFormat>
	{
		{
			".jpg",
			FileMagicNumbers.FileFormat.JPG
		},
		{
			".jpeg",
			FileMagicNumbers.FileFormat.JPG
		},
		{
			".png",
			FileMagicNumbers.FileFormat.PNG
		},
		{
			".mp4",
			FileMagicNumbers.FileFormat.MP4
		},
		{
			".m4v",
			FileMagicNumbers.FileFormat.M4V
		},
		{
			".mov",
			FileMagicNumbers.FileFormat.MOV
		},
		{
			".webm",
			FileMagicNumbers.FileFormat.WEBM
		},
		{
			".ogv",
			FileMagicNumbers.FileFormat.OGV
		},
		{
			".webp",
			FileMagicNumbers.FileFormat.WEBP
		},
		{
			".gif",
			FileMagicNumbers.FileFormat.GIF
		},
		{
			".rawt",
			FileMagicNumbers.FileFormat.RAWT
		},
		{
			".rawm",
			FileMagicNumbers.FileFormat.RAWM
		},
		{
			".obj",
			FileMagicNumbers.FileFormat.OBJ
		},
		{
			".unity3d",
			FileMagicNumbers.FileFormat.ASSETBUNDLE
		},
		{
			".mp3",
			FileMagicNumbers.FileFormat.MP3
		},
		{
			".ogg",
			FileMagicNumbers.FileFormat.OGG
		},
		{
			".wav",
			FileMagicNumbers.FileFormat.WAV
		},
		{
			".pdf",
			FileMagicNumbers.FileFormat.PDF
		},
		{
			".txt",
			FileMagicNumbers.FileFormat.TXT
		},
		{
			".sh",
			FileMagicNumbers.FileFormat.TXT
		}
	};

	// Token: 0x040007B2 RID: 1970
	public static readonly FileMagicNumbers[] AllFileMagicNumbers = new FileMagicNumbers[]
	{
		new FileMagicNumbers(FileMagicNumbers.FileFormat.JPG, new byte?[]
		{
			new byte?((byte)255),
			new byte?((byte)216),
			new byte?((byte)255)
		}, 0),
		new FileMagicNumbers(FileMagicNumbers.FileFormat.PNG, new byte?[]
		{
			new byte?((byte)137),
			new byte?((byte)80),
			new byte?((byte)78),
			new byte?((byte)71),
			new byte?((byte)13),
			new byte?((byte)10),
			new byte?((byte)26),
			new byte?((byte)10)
		}, 0),
		new FileMagicNumbers(FileMagicNumbers.FileFormat.MP4, new byte?[]
		{
			new byte?((byte)102),
			new byte?((byte)116),
			new byte?((byte)121),
			new byte?((byte)112),
			new byte?((byte)77),
			new byte?((byte)83),
			new byte?((byte)78),
			new byte?((byte)86)
		}, 4),
		new FileMagicNumbers(FileMagicNumbers.FileFormat.MP4, new byte?[]
		{
			new byte?((byte)102),
			new byte?((byte)116),
			new byte?((byte)121),
			new byte?((byte)112),
			new byte?((byte)105),
			new byte?((byte)115),
			new byte?((byte)111),
			new byte?((byte)109)
		}, 4),
		new FileMagicNumbers(FileMagicNumbers.FileFormat.M4V, new byte?[]
		{
			new byte?((byte)102),
			new byte?((byte)116),
			new byte?((byte)121),
			new byte?((byte)112),
			new byte?((byte)77),
			new byte?((byte)52),
			new byte?((byte)86),
			new byte?((byte)32)
		}, 4),
		new FileMagicNumbers(FileMagicNumbers.FileFormat.M4V, new byte?[]
		{
			new byte?((byte)102),
			new byte?((byte)116),
			new byte?((byte)121),
			new byte?((byte)112),
			new byte?((byte)109),
			new byte?((byte)112),
			new byte?((byte)52),
			new byte?((byte)50)
		}, 4),
		new FileMagicNumbers(FileMagicNumbers.FileFormat.MOV, new byte?[]
		{
			new byte?((byte)102),
			new byte?((byte)116),
			new byte?((byte)121),
			new byte?((byte)112),
			new byte?((byte)113),
			new byte?((byte)116),
			new byte?((byte)32),
			new byte?((byte)32)
		}, 4),
		new FileMagicNumbers(FileMagicNumbers.FileFormat.MOV, new byte?[]
		{
			new byte?((byte)109),
			new byte?((byte)111),
			new byte?((byte)111),
			new byte?((byte)118)
		}, 4),
		new FileMagicNumbers(FileMagicNumbers.FileFormat.MOV, new byte?[]
		{
			new byte?((byte)102),
			new byte?((byte)114),
			new byte?((byte)101),
			new byte?((byte)101)
		}, 4),
		new FileMagicNumbers(FileMagicNumbers.FileFormat.MOV, new byte?[]
		{
			new byte?((byte)109),
			new byte?((byte)100),
			new byte?((byte)97),
			new byte?((byte)116)
		}, 4),
		new FileMagicNumbers(FileMagicNumbers.FileFormat.MOV, new byte?[]
		{
			new byte?((byte)119),
			new byte?((byte)105),
			new byte?((byte)100),
			new byte?((byte)101)
		}, 4),
		new FileMagicNumbers(FileMagicNumbers.FileFormat.MOV, new byte?[]
		{
			new byte?((byte)112),
			new byte?((byte)110),
			new byte?((byte)111),
			new byte?((byte)116)
		}, 4),
		new FileMagicNumbers(FileMagicNumbers.FileFormat.MOV, new byte?[]
		{
			new byte?((byte)115),
			new byte?((byte)107),
			new byte?((byte)105),
			new byte?((byte)112)
		}, 4),
		new FileMagicNumbers(FileMagicNumbers.FileFormat.WEBM, new byte?[]
		{
			new byte?((byte)26),
			new byte?((byte)69),
			new byte?((byte)223),
			new byte?((byte)163)
		}, 0),
		new FileMagicNumbers(FileMagicNumbers.FileFormat.OGV, new byte?[]
		{
			new byte?((byte)79),
			new byte?((byte)103),
			new byte?((byte)103),
			new byte?((byte)83),
			new byte?(0),
			new byte?((byte)2),
			new byte?(0),
			new byte?(0),
			new byte?(0),
			new byte?(0),
			new byte?(0),
			new byte?(0),
			new byte?(0),
			new byte?(0)
		}, 0),
		new FileMagicNumbers(FileMagicNumbers.FileFormat.WEBP, new byte?[]
		{
			new byte?((byte)82),
			new byte?((byte)73),
			new byte?((byte)70),
			new byte?((byte)70),
			null,
			null,
			null,
			null,
			new byte?((byte)87),
			new byte?((byte)69),
			new byte?((byte)66),
			new byte?((byte)80)
		}, 0),
		new FileMagicNumbers(FileMagicNumbers.FileFormat.GIF, new byte?[]
		{
			new byte?((byte)71),
			new byte?((byte)73),
			new byte?((byte)70),
			new byte?((byte)56),
			new byte?((byte)55),
			new byte?((byte)97)
		}, 0),
		new FileMagicNumbers(FileMagicNumbers.FileFormat.GIF, new byte?[]
		{
			new byte?((byte)71),
			new byte?((byte)73),
			new byte?((byte)70),
			new byte?((byte)56),
			new byte?((byte)57),
			new byte?((byte)97)
		}, 0),
		new FileMagicNumbers(FileMagicNumbers.FileFormat.RAWT, "rawt", 0),
		new FileMagicNumbers(FileMagicNumbers.FileFormat.RAWM, "rawm", 0),
		new FileMagicNumbers(FileMagicNumbers.FileFormat.ASSETBUNDLE, "UnityFS", 0),
		new FileMagicNumbers(FileMagicNumbers.FileFormat.MP3, new byte?[]
		{
			new byte?((byte)73),
			new byte?((byte)68),
			new byte?((byte)51)
		}, 0),
		new FileMagicNumbers(FileMagicNumbers.FileFormat.MP3, new byte?[]
		{
			new byte?((byte)255),
			new byte?((byte)251),
			new byte?((byte)224),
			new byte?((byte)68)
		}, 0),
		new FileMagicNumbers(FileMagicNumbers.FileFormat.MP3, "ÿ.", 0),
		new FileMagicNumbers(FileMagicNumbers.FileFormat.OGG, new byte?[]
		{
			new byte?((byte)79),
			new byte?((byte)103),
			new byte?((byte)103),
			new byte?((byte)83),
			new byte?(0),
			new byte?((byte)2),
			new byte?(0),
			new byte?(0),
			new byte?(0),
			new byte?(0),
			new byte?(0),
			new byte?(0),
			new byte?(0),
			new byte?(0)
		}, 0),
		new FileMagicNumbers(FileMagicNumbers.FileFormat.WAV, new byte?[]
		{
			new byte?((byte)82),
			new byte?((byte)73),
			new byte?((byte)70),
			new byte?((byte)70),
			null,
			null,
			null,
			null,
			new byte?((byte)87),
			new byte?((byte)65),
			new byte?((byte)86),
			new byte?((byte)69),
			new byte?((byte)102),
			new byte?((byte)109),
			new byte?((byte)116),
			new byte?((byte)32)
		}, 0),
		new FileMagicNumbers(FileMagicNumbers.FileFormat.PDF, new byte?[]
		{
			new byte?((byte)37),
			new byte?((byte)80),
			new byte?((byte)68),
			new byte?((byte)70)
		}, 0)
	};

	// Token: 0x040007B3 RID: 1971
	private static int cachedMaxMagicNumberLength = 0;

	// Token: 0x020005C6 RID: 1478
	public enum FileFormat
	{
		// Token: 0x04002697 RID: 9879
		UNKNOWN,
		// Token: 0x04002698 RID: 9880
		JPG,
		// Token: 0x04002699 RID: 9881
		PNG,
		// Token: 0x0400269A RID: 9882
		MP4,
		// Token: 0x0400269B RID: 9883
		M4V,
		// Token: 0x0400269C RID: 9884
		MOV,
		// Token: 0x0400269D RID: 9885
		WEBM,
		// Token: 0x0400269E RID: 9886
		OGV,
		// Token: 0x0400269F RID: 9887
		WEBP,
		// Token: 0x040026A0 RID: 9888
		GIF,
		// Token: 0x040026A1 RID: 9889
		RAWT,
		// Token: 0x040026A2 RID: 9890
		RAWM,
		// Token: 0x040026A3 RID: 9891
		OBJ,
		// Token: 0x040026A4 RID: 9892
		ASSETBUNDLE,
		// Token: 0x040026A5 RID: 9893
		MP3,
		// Token: 0x040026A6 RID: 9894
		OGG,
		// Token: 0x040026A7 RID: 9895
		WAV,
		// Token: 0x040026A8 RID: 9896
		PDF,
		// Token: 0x040026A9 RID: 9897
		TXT
	}
}
