using System;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;

// Token: 0x020000BB RID: 187
public struct Colour : IEquatable<Colour>
{
	// Token: 0x0600094E RID: 2382 RVA: 0x00042437 File Offset: 0x00040637
	public Colour(float r, float g, float b, float a)
	{
		this.r = r;
		this.g = g;
		this.b = b;
		this.a = a;
	}

	// Token: 0x0600094F RID: 2383 RVA: 0x00042456 File Offset: 0x00040656
	public Colour(float r, float g, float b)
	{
		this.r = r;
		this.g = g;
		this.b = b;
		this.a = 1f;
	}

	// Token: 0x06000950 RID: 2384 RVA: 0x00042478 File Offset: 0x00040678
	public Colour(Color color)
	{
		this.r = color.r;
		this.g = color.g;
		this.b = color.b;
		this.a = color.a;
	}

	// Token: 0x06000951 RID: 2385 RVA: 0x000424AA File Offset: 0x000406AA
	public Colour(byte r, byte g, byte b, byte a)
	{
		this.r = (float)r / 255f;
		this.g = (float)g / 255f;
		this.b = (float)b / 255f;
		this.a = (float)a / 255f;
	}

	// Token: 0x06000952 RID: 2386 RVA: 0x000424E5 File Offset: 0x000406E5
	public Colour(byte r, byte g, byte b)
	{
		this.r = (float)r / 255f;
		this.g = (float)g / 255f;
		this.b = (float)b / 255f;
		this.a = 1f;
	}

	// Token: 0x06000953 RID: 2387 RVA: 0x0004251C File Offset: 0x0004071C
	public Colour(Color32 color)
	{
		this.r = (float)color.r / 255f;
		this.g = (float)color.g / 255f;
		this.b = (float)color.b / 255f;
		this.a = (float)color.a / 255f;
	}

	// Token: 0x170001AE RID: 430
	// (get) Token: 0x06000954 RID: 2388 RVA: 0x00042575 File Offset: 0x00040775
	// (set) Token: 0x06000955 RID: 2389 RVA: 0x00042584 File Offset: 0x00040784
	public string Label
	{
		get
		{
			return Colour.LabelFromColour(this);
		}
		set
		{
			Colour colour = Colour.ColourFromLabel(value);
			this.r = colour.r;
			this.g = colour.g;
			this.b = colour.b;
			this.a = colour.a;
		}
	}

	// Token: 0x170001AF RID: 431
	// (get) Token: 0x06000956 RID: 2390 RVA: 0x000425C8 File Offset: 0x000407C8
	// (set) Token: 0x06000957 RID: 2391 RVA: 0x000425D8 File Offset: 0x000407D8
	public string Hex
	{
		get
		{
			return Colour.HexFromColour(this);
		}
		set
		{
			Colour colour = Colour.ColourFromHex(value);
			this.r = colour.r;
			this.g = colour.g;
			this.b = colour.b;
			this.a = colour.a;
		}
	}

	// Token: 0x170001B0 RID: 432
	// (get) Token: 0x06000958 RID: 2392 RVA: 0x00042621 File Offset: 0x00040821
	// (set) Token: 0x06000959 RID: 2393 RVA: 0x00042630 File Offset: 0x00040830
	public string RawRGBHex
	{
		get
		{
			return Colour.RawRGBHexFromColour(this);
		}
		set
		{
			Colour colour = Colour.ColourFromRGBHex(value);
			this.r = colour.r;
			this.g = colour.g;
			this.b = colour.b;
			this.a = colour.a;
		}
	}

	// Token: 0x170001B1 RID: 433
	// (get) Token: 0x0600095A RID: 2394 RVA: 0x00042674 File Offset: 0x00040874
	// (set) Token: 0x0600095B RID: 2395 RVA: 0x00042684 File Offset: 0x00040884
	public string RGBHex
	{
		get
		{
			return Colour.RGBHexFromColour(this);
		}
		set
		{
			Colour colour = Colour.ColourFromRGBHex(value);
			this.r = colour.r;
			this.g = colour.g;
			this.b = colour.b;
			this.a = colour.a;
		}
	}

	// Token: 0x170001B2 RID: 434
	// (get) Token: 0x0600095C RID: 2396 RVA: 0x000426C8 File Offset: 0x000408C8
	// (set) Token: 0x0600095D RID: 2397 RVA: 0x000426D8 File Offset: 0x000408D8
	public int ID
	{
		get
		{
			return Colour.IDFromColour(this);
		}
		set
		{
			Colour colour = Colour.ColourFromID(value);
			this.r = colour.r;
			this.g = colour.g;
			this.b = colour.b;
			this.a = colour.a;
		}
	}

	// Token: 0x0600095E RID: 2398 RVA: 0x0004271C File Offset: 0x0004091C
	public static implicit operator Color(Colour colour)
	{
		return new Color(colour.r, colour.g, colour.b, colour.a);
	}

	// Token: 0x0600095F RID: 2399 RVA: 0x0004273B File Offset: 0x0004093B
	public static implicit operator Colour(Color color)
	{
		return new Colour(color);
	}

	// Token: 0x06000960 RID: 2400 RVA: 0x00042743 File Offset: 0x00040943
	public static implicit operator Color32(Colour colour)
	{
		return colour;
	}

	// Token: 0x06000961 RID: 2401 RVA: 0x00042750 File Offset: 0x00040950
	public static implicit operator Colour(Color32 color32)
	{
		return color32;
	}

	// Token: 0x06000962 RID: 2402 RVA: 0x00042760 File Offset: 0x00040960
	public bool Equals(Colour colour)
	{
		return Mathf.Abs(this.r - colour.r) < 0.003f && Mathf.Abs(this.g - colour.g) < 0.003f && Mathf.Abs(this.b - colour.b) < 0.003f;
	}

	// Token: 0x06000963 RID: 2403 RVA: 0x000427BA File Offset: 0x000409BA
	public static bool operator ==(Colour c1, Colour c2)
	{
		return c1.Equals(c2);
	}

	// Token: 0x06000964 RID: 2404 RVA: 0x000427C4 File Offset: 0x000409C4
	public static bool operator !=(Colour c1, Colour c2)
	{
		return !c1.Equals(c2);
	}

	// Token: 0x06000965 RID: 2405 RVA: 0x000427D4 File Offset: 0x000409D4
	public static bool operator ==(Color color, Colour colour)
	{
		return color.Equals(colour);
	}

	// Token: 0x06000966 RID: 2406 RVA: 0x000427F0 File Offset: 0x000409F0
	public static bool operator !=(Color color, Colour colour)
	{
		return !color.Equals(colour);
	}

	// Token: 0x06000967 RID: 2407 RVA: 0x00042810 File Offset: 0x00040A10
	public static bool operator ==(Colour colour, Color color)
	{
		Colour colour2 = color;
		return colour.Equals(colour2);
	}

	// Token: 0x06000968 RID: 2408 RVA: 0x0004282C File Offset: 0x00040A2C
	public static bool operator !=(Colour colour, Color color)
	{
		Colour colour2 = color;
		return !colour.Equals(colour2);
	}

	// Token: 0x06000969 RID: 2409 RVA: 0x0004284C File Offset: 0x00040A4C
	public static Colour GetPlayerPrefColour(string pref, Colour defaultColour)
	{
		try
		{
			return Json.Load<ColourState>(PlayerPrefs.GetString(pref, Json.GetJson(new ColourState(defaultColour), true))).ToColour();
		}
		catch (Exception)
		{
		}
		return defaultColour;
	}

	// Token: 0x0600096A RID: 2410 RVA: 0x00042898 File Offset: 0x00040A98
	public static void SetPlayerPrefColour(string pref, Colour colour)
	{
		PlayerPrefs.SetString(pref, Json.GetJson(new ColourState(colour), true));
	}

	// Token: 0x0600096B RID: 2411 RVA: 0x000428B4 File Offset: 0x00040AB4
	public static Colour ColourFromLabel(string label)
	{
		uint num = <PrivateImplementationDetails>.ComputeStringHash(label);
		if (num <= 1920678155U)
		{
			if (num <= 721696580U)
			{
				if (num != 45660623U)
				{
					if (num != 606718655U)
					{
						if (num == 721696580U)
						{
							if (label == "Black")
							{
								return Colour.Black;
							}
						}
					}
					else if (label == "Teal")
					{
						return Colour.Teal;
					}
				}
				else if (label == "Brown")
				{
					return Colour.Brown;
				}
			}
			else if (num != 851310790U)
			{
				if (num != 1827351814U)
				{
					if (num == 1920678155U)
					{
						if (label == "Orange")
						{
							return Colour.Orange;
						}
					}
				}
				else if (label == "White")
				{
					return Colour.White;
				}
			}
			else if (label == "Grey")
			{
				return Colour.Grey;
			}
		}
		else if (num <= 2840840028U)
		{
			if (num != 2344092557U)
			{
				if (num != 2743015548U)
				{
					if (num == 2840840028U)
					{
						if (label == "Green")
						{
							return Colour.Green;
						}
					}
				}
				else if (label == "Red")
				{
					return Colour.Red;
				}
			}
			else if (label == "Pink")
			{
				return Colour.Pink;
			}
		}
		else if (num != 3062020639U)
		{
			if (num != 3654151273U)
			{
				if (num == 3923582957U)
				{
					if (label == "Blue")
					{
						return Colour.Blue;
					}
				}
			}
			else if (label == "Yellow")
			{
				return Colour.Yellow;
			}
		}
		else if (label == "Purple")
		{
			return Colour.Purple;
		}
		return Colour.White;
	}

	// Token: 0x0600096C RID: 2412 RVA: 0x00042A9C File Offset: 0x00040C9C
	public static bool IsColourLabel(string label)
	{
		uint num = <PrivateImplementationDetails>.ComputeStringHash(label);
		if (num <= 1920678155U)
		{
			if (num <= 721696580U)
			{
				if (num != 45660623U)
				{
					if (num != 606718655U)
					{
						if (num != 721696580U)
						{
							return false;
						}
						if (!(label == "Black"))
						{
							return false;
						}
					}
					else if (!(label == "Teal"))
					{
						return false;
					}
				}
				else if (!(label == "Brown"))
				{
					return false;
				}
			}
			else if (num != 851310790U)
			{
				if (num != 1827351814U)
				{
					if (num != 1920678155U)
					{
						return false;
					}
					if (!(label == "Orange"))
					{
						return false;
					}
				}
				else if (!(label == "White"))
				{
					return false;
				}
			}
			else if (!(label == "Grey"))
			{
				return false;
			}
		}
		else if (num <= 2840840028U)
		{
			if (num != 2344092557U)
			{
				if (num != 2743015548U)
				{
					if (num != 2840840028U)
					{
						return false;
					}
					if (!(label == "Green"))
					{
						return false;
					}
				}
				else if (!(label == "Red"))
				{
					return false;
				}
			}
			else if (!(label == "Pink"))
			{
				return false;
			}
		}
		else if (num != 3062020639U)
		{
			if (num != 3654151273U)
			{
				if (num != 3923582957U)
				{
					return false;
				}
				if (!(label == "Blue"))
				{
					return false;
				}
			}
			else if (!(label == "Yellow"))
			{
				return false;
			}
		}
		else if (!(label == "Purple"))
		{
			return false;
		}
		return true;
	}

	// Token: 0x0600096D RID: 2413 RVA: 0x00042C20 File Offset: 0x00040E20
	public static Color? NullableColorFromLabel(string label)
	{
		Colour colour;
		Color? result;
		if (Colour.TryColourFromLabel(label, out colour))
		{
			result = new Color?(colour);
		}
		else
		{
			result = null;
		}
		return result;
	}

	// Token: 0x0600096E RID: 2414 RVA: 0x00042C50 File Offset: 0x00040E50
	public static bool TryColourFromLabel(string label, out Colour colour)
	{
		uint num = <PrivateImplementationDetails>.ComputeStringHash(label);
		if (num <= 1920678155U)
		{
			if (num <= 721696580U)
			{
				if (num != 45660623U)
				{
					if (num != 606718655U)
					{
						if (num == 721696580U)
						{
							if (label == "Black")
							{
								colour = Colour.Black;
								return true;
							}
						}
					}
					else if (label == "Teal")
					{
						colour = Colour.Teal;
						return true;
					}
				}
				else if (label == "Brown")
				{
					colour = Colour.Brown;
					return true;
				}
			}
			else if (num != 851310790U)
			{
				if (num != 1827351814U)
				{
					if (num == 1920678155U)
					{
						if (label == "Orange")
						{
							colour = Colour.Orange;
							return true;
						}
					}
				}
				else if (label == "White")
				{
					colour = Colour.White;
					return true;
				}
			}
			else if (label == "Grey")
			{
				colour = Colour.Grey;
				return true;
			}
		}
		else if (num <= 2840840028U)
		{
			if (num != 2344092557U)
			{
				if (num != 2743015548U)
				{
					if (num == 2840840028U)
					{
						if (label == "Green")
						{
							colour = Colour.Green;
							return true;
						}
					}
				}
				else if (label == "Red")
				{
					colour = Colour.Red;
					return true;
				}
			}
			else if (label == "Pink")
			{
				colour = Colour.Pink;
				return true;
			}
		}
		else if (num != 3062020639U)
		{
			if (num != 3654151273U)
			{
				if (num == 3923582957U)
				{
					if (label == "Blue")
					{
						colour = Colour.Blue;
						return true;
					}
				}
			}
			else if (label == "Yellow")
			{
				colour = Colour.Yellow;
				return true;
			}
		}
		else if (label == "Purple")
		{
			colour = Colour.Purple;
			return true;
		}
		colour = Colour.Black;
		return false;
	}

	// Token: 0x0600096F RID: 2415 RVA: 0x00042EB0 File Offset: 0x000410B0
	public static string LabelFromColour(Colour colour)
	{
		string result;
		if (Colour.labelFromColour.TryGetValue(colour, out result))
		{
			return result;
		}
		return "White";
	}

	// Token: 0x06000970 RID: 2416 RVA: 0x00042ED4 File Offset: 0x000410D4
	public static int IDFromColour(Colour colour)
	{
		int result;
		if (Colour.idFromColour.TryGetValue(colour, out result))
		{
			return result;
		}
		return -1;
	}

	// Token: 0x06000971 RID: 2417 RVA: 0x00042EF4 File Offset: 0x000410F4
	public static Colour ColourFromID(int id)
	{
		switch (id)
		{
		case -2:
			return Colour.Clear;
		case -1:
			return Colour.Grey;
		case 0:
			return Colour.White;
		case 1:
			return Colour.Brown;
		case 2:
			return Colour.Red;
		case 3:
			return Colour.Orange;
		case 4:
			return Colour.Yellow;
		case 5:
			return Colour.Green;
		case 6:
			return Colour.Teal;
		case 7:
			return Colour.Blue;
		case 8:
			return Colour.Purple;
		case 9:
			return Colour.Pink;
		case 10:
			return Colour.Black;
		default:
			return Colour.Grey;
		}
	}

	// Token: 0x06000972 RID: 2418 RVA: 0x00042F94 File Offset: 0x00041194
	public static Colour DarkenedFromColour(Colour colour)
	{
		Colour result;
		if (Colour.darkenedFromColour.TryGetValue(colour, out result))
		{
			return result;
		}
		return Colour.GreyDark;
	}

	// Token: 0x06000973 RID: 2419 RVA: 0x00042FB8 File Offset: 0x000411B8
	public static uint FlagFromColor(Color color)
	{
		uint result;
		if (Colour.flagFromColour.TryGetValue(color, out result))
		{
			return result;
		}
		return uint.MaxValue;
	}

	// Token: 0x06000974 RID: 2420 RVA: 0x00042FDC File Offset: 0x000411DC
	public static uint FlagFromLabel(string label)
	{
		uint num = <PrivateImplementationDetails>.ComputeStringHash(label);
		if (num <= 1920678155U)
		{
			if (num <= 721696580U)
			{
				if (num != 45660623U)
				{
					if (num != 606718655U)
					{
						if (num == 721696580U)
						{
							if (label == "Black")
							{
								return 0U;
							}
						}
					}
					else if (label == "Teal")
					{
						return 64U;
					}
				}
				else if (label == "Brown")
				{
					return 2U;
				}
			}
			else if (num != 851310790U)
			{
				if (num != 1827351814U)
				{
					if (num == 1920678155U)
					{
						if (label == "Orange")
						{
							return 8U;
						}
					}
				}
				else if (label == "White")
				{
					return 1U;
				}
			}
			else if (label == "Grey")
			{
				return 1024U;
			}
		}
		else if (num <= 2840840028U)
		{
			if (num != 2344092557U)
			{
				if (num != 2743015548U)
				{
					if (num == 2840840028U)
					{
						if (label == "Green")
						{
							return 32U;
						}
					}
				}
				else if (label == "Red")
				{
					return 4U;
				}
			}
			else if (label == "Pink")
			{
				return 512U;
			}
		}
		else if (num != 3062020639U)
		{
			if (num != 3654151273U)
			{
				if (num == 3923582957U)
				{
					if (label == "Blue")
					{
						return 128U;
					}
				}
			}
			else if (label == "Yellow")
			{
				return 16U;
			}
		}
		else if (label == "Purple")
		{
			return 256U;
		}
		return uint.MaxValue;
	}

	// Token: 0x06000975 RID: 2421 RVA: 0x00043195 File Offset: 0x00041395
	public static uint InverseFlagsFromLabel(string label)
	{
		return 2047U & ~Colour.FlagFromLabel(label);
	}

	// Token: 0x06000976 RID: 2422 RVA: 0x000431A4 File Offset: 0x000413A4
	public static void LogFlags(uint flags)
	{
		if ((flags & 2147483648U) != 0U)
		{
			Singleton<SystemConsole>.Instance.Log(" INVERSE", false);
		}
		if ((flags & 1U) != 0U)
		{
			Singleton<SystemConsole>.Instance.Log(" White", false);
		}
		if ((flags & 2U) != 0U)
		{
			Singleton<SystemConsole>.Instance.Log(" Brown", false);
		}
		if ((flags & 4U) != 0U)
		{
			Singleton<SystemConsole>.Instance.Log(" Red", false);
		}
		if ((flags & 8U) != 0U)
		{
			Singleton<SystemConsole>.Instance.Log(" Orange", false);
		}
		if ((flags & 16U) != 0U)
		{
			Singleton<SystemConsole>.Instance.Log(" Yellow", false);
		}
		if ((flags & 32U) != 0U)
		{
			Singleton<SystemConsole>.Instance.Log(" Green", false);
		}
		if ((flags & 64U) != 0U)
		{
			Singleton<SystemConsole>.Instance.Log(" Teal", false);
		}
		if ((flags & 128U) != 0U)
		{
			Singleton<SystemConsole>.Instance.Log(" Blue", false);
		}
		if ((flags & 256U) != 0U)
		{
			Singleton<SystemConsole>.Instance.Log(" Purple", false);
		}
		if ((flags & 512U) != 0U)
		{
			Singleton<SystemConsole>.Instance.Log(" Pink", false);
		}
		if ((flags & 0U) != 0U)
		{
			Singleton<SystemConsole>.Instance.Log(" Black", false);
		}
		if ((flags & 1024U) != 0U)
		{
			Singleton<SystemConsole>.Instance.Log(" Grey", false);
		}
	}

	// Token: 0x06000977 RID: 2423 RVA: 0x000432DC File Offset: 0x000414DC
	public static string HexFromLabel(string label)
	{
		uint num = <PrivateImplementationDetails>.ComputeStringHash(label);
		if (num <= 1920678155U)
		{
			if (num <= 721696580U)
			{
				if (num != 45660623U)
				{
					if (num != 606718655U)
					{
						if (num == 721696580U)
						{
							if (label == "Black")
							{
								return "[191919]";
							}
						}
					}
					else if (label == "Teal")
					{
						return "[21B19B]";
					}
				}
				else if (label == "Brown")
				{
					return "[713B17]";
				}
			}
			else if (num != 851310790U)
			{
				if (num != 1827351814U)
				{
					if (num == 1920678155U)
					{
						if (label == "Orange")
						{
							return "[F4641D]";
						}
					}
				}
				else if (label == "White")
				{
					return "[FFFFFF]";
				}
			}
			else if (label == "Grey")
			{
				return "[AAAAAA]";
			}
		}
		else if (num <= 2840840028U)
		{
			if (num != 2344092557U)
			{
				if (num != 2743015548U)
				{
					if (num == 2840840028U)
					{
						if (label == "Green")
						{
							return "[31B32B]";
						}
					}
				}
				else if (label == "Red")
				{
					return "[DA1918]";
				}
			}
			else if (label == "Pink")
			{
				return "[F570CE]";
			}
		}
		else if (num != 3062020639U)
		{
			if (num != 3654151273U)
			{
				if (num == 3923582957U)
				{
					if (label == "Blue")
					{
						return "[1F87FF]";
					}
				}
			}
			else if (label == "Yellow")
			{
				return "[E7E52C]";
			}
		}
		else if (label == "Purple")
		{
			return "[A020F0]";
		}
		return "[FFFFFF]";
	}

	// Token: 0x06000978 RID: 2424 RVA: 0x000434C4 File Offset: 0x000416C4
	public static string LabelFromHex(string hex)
	{
		uint num = <PrivateImplementationDetails>.ComputeStringHash(hex);
		if (num <= 2271353875U)
		{
			if (num <= 1448528205U)
			{
				if (num != 888488889U)
				{
					if (num != 1332545330U)
					{
						if (num == 1448528205U)
						{
							if (hex == "[DA1918]")
							{
								return "Red";
							}
						}
					}
					else if (hex == "[E7E52C]")
					{
						return "Yellow";
					}
				}
				else if (hex == "[FFFFFF]")
				{
					return "White";
				}
			}
			else if (num != 1531026718U)
			{
				if (num != 1964215731U)
				{
					if (num == 2271353875U)
					{
						if (hex == "[1F87FF]")
						{
							return "Blue";
						}
					}
				}
				else if (hex == "[AAAAAA]")
				{
					return "Grey";
				}
			}
			else if (hex == "[A020F0]")
			{
				return "Purple";
			}
		}
		else if (num <= 2954927690U)
		{
			if (num != 2375118104U)
			{
				if (num != 2742069150U)
				{
					if (num == 2954927690U)
					{
						if (hex == "[713B17]")
						{
							return "Brown";
						}
					}
				}
				else if (hex == "[F4641D]")
				{
					return "Orange";
				}
			}
			else if (hex == "[21B19B]")
			{
				return "Teal";
			}
		}
		else if (num != 2974597355U)
		{
			if (num != 3187386039U)
			{
				if (num == 3918294248U)
				{
					if (hex == "[31B32B]")
					{
						return "Green";
					}
				}
			}
			else if (hex == "[F570CE]")
			{
				return "Pink";
			}
		}
		else if (hex == "[191919]")
		{
			return "Black";
		}
		return "White";
	}

	// Token: 0x06000979 RID: 2425 RVA: 0x000436AA File Offset: 0x000418AA
	public Colour WithAlpha(float a)
	{
		return new Colour(this.r, this.g, this.b, a);
	}

	// Token: 0x0600097A RID: 2426 RVA: 0x000436C4 File Offset: 0x000418C4
	public static Color ColourFromHex(string hex)
	{
		uint num = <PrivateImplementationDetails>.ComputeStringHash(hex);
		if (num <= 2271353875U)
		{
			if (num <= 1448528205U)
			{
				if (num != 888488889U)
				{
					if (num != 1332545330U)
					{
						if (num == 1448528205U)
						{
							if (hex == "[DA1918]")
							{
								return Colour.Red;
							}
						}
					}
					else if (hex == "[E7E52C]")
					{
						return Colour.Yellow;
					}
				}
				else if (hex == "[FFFFFF]")
				{
					return Colour.White;
				}
			}
			else if (num != 1531026718U)
			{
				if (num != 1964215731U)
				{
					if (num == 2271353875U)
					{
						if (hex == "[1F87FF]")
						{
							return Colour.Blue;
						}
					}
				}
				else if (hex == "[AAAAAA]")
				{
					return Colour.Grey;
				}
			}
			else if (hex == "[A020F0]")
			{
				return Colour.Purple;
			}
		}
		else if (num <= 2954927690U)
		{
			if (num != 2375118104U)
			{
				if (num != 2742069150U)
				{
					if (num == 2954927690U)
					{
						if (hex == "[713B17]")
						{
							return Colour.Brown;
						}
					}
				}
				else if (hex == "[F4641D]")
				{
					return Colour.Orange;
				}
			}
			else if (hex == "[21B19B]")
			{
				return Colour.Teal;
			}
		}
		else if (num != 2974597355U)
		{
			if (num != 3187386039U)
			{
				if (num == 3918294248U)
				{
					if (hex == "[31B32B]")
					{
						return Colour.Green;
					}
				}
			}
			else if (hex == "[F570CE]")
			{
				return Colour.Pink;
			}
		}
		else if (hex == "[191919]")
		{
			return Colour.Black;
		}
		return Colour.White;
	}

	// Token: 0x0600097B RID: 2427 RVA: 0x00043908 File Offset: 0x00041B08
	public static string HexFromColour(Colour colour)
	{
		string result;
		if (Colour.hexFromColour.TryGetValue(colour, out result))
		{
			return result;
		}
		return "[FFFFFF]";
	}

	// Token: 0x0600097C RID: 2428 RVA: 0x0004392B File Offset: 0x00041B2B
	public static Colour UIColourFromColour(Colour colour)
	{
		return Singleton<UIPalette>.Instance.CurrentThemeColours[Colour.UIFromColour(colour)];
	}

	// Token: 0x0600097D RID: 2429 RVA: 0x00043944 File Offset: 0x00041B44
	public static UIPalette.UI UIFromColour(Colour colour)
	{
		UIPalette.UI result;
		if (Colour.uiFromColour.TryGetValue(colour, out result))
		{
			return result;
		}
		return UIPalette.UI.PlayerGrey;
	}

	// Token: 0x0600097E RID: 2430 RVA: 0x00043964 File Offset: 0x00041B64
	public static Colour ColourFromUIColour(Colour colour)
	{
		if (colour == Singleton<UIPalette>.Instance.CurrentThemeColours[UIPalette.UI.PlayerWhite])
		{
			return Colour.White;
		}
		if (colour == Singleton<UIPalette>.Instance.CurrentThemeColours[UIPalette.UI.PlayerBrown])
		{
			return Colour.Brown;
		}
		if (colour == Singleton<UIPalette>.Instance.CurrentThemeColours[UIPalette.UI.PlayerRed])
		{
			return Colour.Red;
		}
		if (colour == Singleton<UIPalette>.Instance.CurrentThemeColours[UIPalette.UI.PlayerOrange])
		{
			return Colour.Orange;
		}
		if (colour == Singleton<UIPalette>.Instance.CurrentThemeColours[UIPalette.UI.PlayerYellow])
		{
			return Colour.Yellow;
		}
		if (colour == Singleton<UIPalette>.Instance.CurrentThemeColours[UIPalette.UI.PlayerGreen])
		{
			return Colour.Green;
		}
		if (colour == Singleton<UIPalette>.Instance.CurrentThemeColours[UIPalette.UI.PlayerTeal])
		{
			return Colour.Teal;
		}
		if (colour == Singleton<UIPalette>.Instance.CurrentThemeColours[UIPalette.UI.PlayerBlue])
		{
			return Colour.Blue;
		}
		if (colour == Singleton<UIPalette>.Instance.CurrentThemeColours[UIPalette.UI.PlayerPurple])
		{
			return Colour.Purple;
		}
		if (colour == Singleton<UIPalette>.Instance.CurrentThemeColours[UIPalette.UI.PlayerPink])
		{
			return Colour.Pink;
		}
		if (colour == Singleton<UIPalette>.Instance.CurrentThemeColours[UIPalette.UI.PlayerBlack])
		{
			return Colour.Black;
		}
		return Colour.Grey;
	}

	// Token: 0x0600097F RID: 2431 RVA: 0x00043ACC File Offset: 0x00041CCC
	public static bool IsUIColour(Colour colour)
	{
		return colour == Singleton<UIPalette>.Instance.CurrentThemeColours[UIPalette.UI.PlayerWhite] || colour == Singleton<UIPalette>.Instance.CurrentThemeColours[UIPalette.UI.PlayerBrown] || colour == Singleton<UIPalette>.Instance.CurrentThemeColours[UIPalette.UI.PlayerRed] || colour == Singleton<UIPalette>.Instance.CurrentThemeColours[UIPalette.UI.PlayerOrange] || colour == Singleton<UIPalette>.Instance.CurrentThemeColours[UIPalette.UI.PlayerYellow] || colour == Singleton<UIPalette>.Instance.CurrentThemeColours[UIPalette.UI.PlayerGreen] || colour == Singleton<UIPalette>.Instance.CurrentThemeColours[UIPalette.UI.PlayerTeal] || colour == Singleton<UIPalette>.Instance.CurrentThemeColours[UIPalette.UI.PlayerBlue] || colour == Singleton<UIPalette>.Instance.CurrentThemeColours[UIPalette.UI.PlayerPurple] || colour == Singleton<UIPalette>.Instance.CurrentThemeColours[UIPalette.UI.PlayerPink] || colour == Singleton<UIPalette>.Instance.CurrentThemeColours[UIPalette.UI.PlayerBlack] || colour == Singleton<UIPalette>.Instance.CurrentThemeColours[UIPalette.UI.PlayerGrey];
	}

	// Token: 0x06000980 RID: 2432 RVA: 0x00043C18 File Offset: 0x00041E18
	public static bool IsPlayerColour(Colour colour)
	{
		return colour == Colour.White || colour == Colour.Brown || colour == Colour.Red || colour == Colour.Orange || colour == Colour.Yellow || colour == Colour.Green || colour == Colour.Teal || colour == Colour.Blue || colour == Colour.Purple || colour == Colour.Pink || colour == Colour.Black || colour == Colour.Grey;
	}

	// Token: 0x06000981 RID: 2433 RVA: 0x00043CC8 File Offset: 0x00041EC8
	public static string RGBHexFromColour(Colour colour)
	{
		if (colour == Colour.White)
		{
			return "[FFFFFF]";
		}
		if (colour == Colour.Brown)
		{
			return "[713B17]";
		}
		if (colour == Colour.Red)
		{
			return "[DA1918]";
		}
		if (colour == Colour.Orange)
		{
			return "[F4641D]";
		}
		if (colour == Colour.Yellow)
		{
			return "[E7E52C]";
		}
		if (colour == Colour.Green)
		{
			return "[31B32B]";
		}
		if (colour == Colour.Teal)
		{
			return "[21B19B]";
		}
		if (colour == Colour.Blue)
		{
			return "[1F87FF]";
		}
		if (colour == Colour.Purple)
		{
			return "[A020F0]";
		}
		if (colour == Colour.Pink)
		{
			return "[F570CE]";
		}
		if (colour == Colour.Grey)
		{
			return "[AAAAAA]";
		}
		if (colour == Colour.Black)
		{
			return "[191919]";
		}
		Color32 color = colour;
		if (color.a == 255)
		{
			return string.Concat(new string[]
			{
				"[",
				color.r.ToString("X2"),
				color.g.ToString("X2"),
				color.b.ToString("X2"),
				"]"
			});
		}
		return string.Concat(new string[]
		{
			"[",
			color.r.ToString("X2"),
			color.g.ToString("X2"),
			color.b.ToString("X2"),
			color.a.ToString("X2"),
			"]"
		});
	}

	// Token: 0x06000982 RID: 2434 RVA: 0x00043E90 File Offset: 0x00042090
	public static string RawRGBHexFromColour(Colour colour)
	{
		Color32 color = colour;
		if (color.a == 255)
		{
			return color.r.ToString("X2") + color.g.ToString("X2") + color.b.ToString("X2");
		}
		return color.r.ToString("X2") + color.g.ToString("X2") + color.b.ToString("X2") + color.a.ToString("X2");
	}

	// Token: 0x06000983 RID: 2435 RVA: 0x00043F34 File Offset: 0x00042134
	public static Colour ColourFromRGBHex(string hex)
	{
		hex = hex.Replace("#", "").Replace("[", "").Replace("]", "");
		byte b = byte.Parse(hex.Substring(0, 2), NumberStyles.HexNumber);
		byte b2 = byte.Parse(hex.Substring(2, 2), NumberStyles.HexNumber);
		byte b3 = byte.Parse(hex.Substring(4, 2), NumberStyles.HexNumber);
		byte b4 = (hex.Length > 6) ? byte.Parse(hex.Substring(6, 2), NumberStyles.HexNumber) : byte.MaxValue;
		return new Colour(b, b2, b3, b4);
	}

	// Token: 0x06000984 RID: 2436 RVA: 0x00043FD4 File Offset: 0x000421D4
	public static string MyColorLabel()
	{
		if (NetworkSingleton<PlayerManager>.Instance)
		{
			PlayerState playerState = NetworkSingleton<PlayerManager>.Instance.MyPlayerState();
			if (playerState != null)
			{
				return playerState.stringColor;
			}
		}
		return "Grey";
	}

	// Token: 0x170001B3 RID: 435
	public float this[int index]
	{
		get
		{
			if (index == 0)
			{
				return this.r;
			}
			if (index == 1)
			{
				return this.g;
			}
			if (index == 2)
			{
				return this.b;
			}
			if (index == 3)
			{
				return this.a;
			}
			throw new IndexOutOfRangeException();
		}
		set
		{
			if (index == 0)
			{
				this.r = value;
				return;
			}
			if (index == 1)
			{
				this.g = value;
				return;
			}
			if (index == 2)
			{
				this.b = value;
				return;
			}
			if (index == 3)
			{
				this.a = value;
				return;
			}
			throw new IndexOutOfRangeException();
		}
	}

	// Token: 0x170001B4 RID: 436
	// (get) Token: 0x06000987 RID: 2439 RVA: 0x00044070 File Offset: 0x00042270
	public float grayscale
	{
		get
		{
			return this.grayscale;
		}
	}

	// Token: 0x170001B5 RID: 437
	// (get) Token: 0x06000988 RID: 2440 RVA: 0x00044090 File Offset: 0x00042290
	public float maxColorComponent
	{
		get
		{
			return this.maxColorComponent;
		}
	}

	// Token: 0x170001B6 RID: 438
	// (get) Token: 0x06000989 RID: 2441 RVA: 0x000440B0 File Offset: 0x000422B0
	public Colour linear
	{
		get
		{
			return this.linear;
		}
	}

	// Token: 0x170001B7 RID: 439
	// (get) Token: 0x0600098A RID: 2442 RVA: 0x000440D8 File Offset: 0x000422D8
	public Colour gamma
	{
		get
		{
			return this.gamma;
		}
	}

	// Token: 0x0600098B RID: 2443 RVA: 0x000440FD File Offset: 0x000422FD
	public static Colour HSVToRGB(float H, float S, float V, bool hdr)
	{
		return Color.HSVToRGB(H, S, V, hdr);
	}

	// Token: 0x0600098C RID: 2444 RVA: 0x0004410D File Offset: 0x0004230D
	public static Colour HSVToRGB(float H, float S, float V)
	{
		return Color.HSVToRGB(H, S, V);
	}

	// Token: 0x0600098D RID: 2445 RVA: 0x0004411C File Offset: 0x0004231C
	public static Colour Lerp(Colour a, Colour b, float t)
	{
		return Color.Lerp(a, b, t);
	}

	// Token: 0x0600098E RID: 2446 RVA: 0x00044135 File Offset: 0x00042335
	public static Colour LerpUnclamped(Colour a, Colour b, float t)
	{
		return Color.LerpUnclamped(a, b, t);
	}

	// Token: 0x0600098F RID: 2447 RVA: 0x00044150 File Offset: 0x00042350
	public static Colour Lighten(Colour a, float t)
	{
		Colour white = Colour.White;
		white.a = a.a;
		return Colour.Lerp(a, white, t);
	}

	// Token: 0x06000990 RID: 2448 RVA: 0x00044178 File Offset: 0x00042378
	public static Colour Darken(Colour a, float t)
	{
		Colour black = Colour.Black;
		black.a = a.a;
		return Colour.Lerp(a, black, t);
	}

	// Token: 0x06000991 RID: 2449 RVA: 0x000441A0 File Offset: 0x000423A0
	public static Colour AlphaFade(Colour a, float t)
	{
		return new Colour(a.r, a.g, a.b)
		{
			a = a.a * t
		};
	}

	// Token: 0x06000992 RID: 2450 RVA: 0x000441D6 File Offset: 0x000423D6
	public static void RGBtoHSV(Colour rgbColour, out float H, out float S, out float V)
	{
		Color.RGBToHSV(rgbColour, out H, out S, out V);
	}

	// Token: 0x06000993 RID: 2451 RVA: 0x000441E8 File Offset: 0x000423E8
	public override bool Equals(object other)
	{
		return this.Equals(other);
	}

	// Token: 0x06000994 RID: 2452 RVA: 0x00044210 File Offset: 0x00042410
	public override int GetHashCode()
	{
		return (int)(this.r * 255f) * 255 * 255 * 255 + (int)(this.g * 255f) * 255 * 255 + (int)(this.b * 255f) * 255 + (int)(this.a * 255f);
	}

	// Token: 0x06000995 RID: 2453 RVA: 0x00044278 File Offset: 0x00042478
	public string ToString(string format)
	{
		return this.ToString(format);
	}

	// Token: 0x06000996 RID: 2454 RVA: 0x0004429C File Offset: 0x0004249C
	public override string ToString()
	{
		return this.ToString();
	}

	// Token: 0x06000997 RID: 2455 RVA: 0x000442C4 File Offset: 0x000424C4
	public static Colour operator +(Colour a, Colour b)
	{
		Color color = a;
		Color color2 = b;
		return color + color2;
	}

	// Token: 0x06000998 RID: 2456 RVA: 0x000442EC File Offset: 0x000424EC
	public static Colour operator -(Colour a, Colour b)
	{
		Color color = a;
		Color color2 = b;
		return color - color2;
	}

	// Token: 0x06000999 RID: 2457 RVA: 0x00044314 File Offset: 0x00042514
	public static Colour operator *(float b, Colour a)
	{
		Color color = a;
		return b * color;
	}

	// Token: 0x0600099A RID: 2458 RVA: 0x00044334 File Offset: 0x00042534
	public static Colour operator *(Colour a, float b)
	{
		return a * b;
	}

	// Token: 0x0600099B RID: 2459 RVA: 0x00044348 File Offset: 0x00042548
	public static Colour operator *(Colour a, Colour b)
	{
		Color color = a;
		Color color2 = b;
		return color * color2;
	}

	// Token: 0x0600099C RID: 2460 RVA: 0x0004436D File Offset: 0x0004256D
	public static Colour operator /(Colour c, float f)
	{
		return c / f;
	}

	// Token: 0x0600099D RID: 2461 RVA: 0x00044380 File Offset: 0x00042580
	public static implicit operator Colour(Vector4 v)
	{
		return v;
	}

	// Token: 0x0600099E RID: 2462 RVA: 0x0004438D File Offset: 0x0004258D
	public static implicit operator Vector4(Colour c)
	{
		return c;
	}

	// Token: 0x04000694 RID: 1684
	public float r;

	// Token: 0x04000695 RID: 1685
	public float g;

	// Token: 0x04000696 RID: 1686
	public float b;

	// Token: 0x04000697 RID: 1687
	public float a;

	// Token: 0x04000698 RID: 1688
	private const float EPSILON = 0.003f;

	// Token: 0x04000699 RID: 1689
	public static readonly Colour UnityRed = Color.red;

	// Token: 0x0400069A RID: 1690
	public static readonly Colour UnityMagenta = Color.magenta;

	// Token: 0x0400069B RID: 1691
	public static readonly Colour UnityWhite = Color.white;

	// Token: 0x0400069C RID: 1692
	public static readonly Colour UnityYellow = Color.yellow;

	// Token: 0x0400069D RID: 1693
	public static readonly Colour UnityGrey = Color.grey;

	// Token: 0x0400069E RID: 1694
	public static readonly Colour UnityGreen = Color.green;

	// Token: 0x0400069F RID: 1695
	public static readonly Colour UnityGray = Color.gray;

	// Token: 0x040006A0 RID: 1696
	public static readonly Colour UnityCyan = Color.cyan;

	// Token: 0x040006A1 RID: 1697
	public static readonly Colour UnityClear = Color.clear;

	// Token: 0x040006A2 RID: 1698
	public static readonly Colour UnityBlue = Color.blue;

	// Token: 0x040006A3 RID: 1699
	public static readonly Colour UnityBlack = Color.black;

	// Token: 0x040006A4 RID: 1700
	public static readonly Colour White = Colour.UnityWhite;

	// Token: 0x040006A5 RID: 1701
	public static readonly Colour Brown = new Colour(0.443f, 0.231f, 0.09f);

	// Token: 0x040006A6 RID: 1702
	public static readonly Colour Red = new Colour(0.856f, 0.1f, 0.094f);

	// Token: 0x040006A7 RID: 1703
	public static readonly Colour Orange = new Colour(0.956f, 0.392f, 0.113f);

	// Token: 0x040006A8 RID: 1704
	public static readonly Colour Yellow = new Colour(0.905f, 0.898f, 0.172f);

	// Token: 0x040006A9 RID: 1705
	public static readonly Colour Green = new Colour(0.192f, 0.701f, 0.168f);

	// Token: 0x040006AA RID: 1706
	public static readonly Colour Teal = new Colour(0.129f, 0.694f, 0.607f);

	// Token: 0x040006AB RID: 1707
	public static readonly Colour Blue = new Colour(0.118f, 0.53f, 1f);

	// Token: 0x040006AC RID: 1708
	public static readonly Colour Purple = new Colour(0.627f, 0.125f, 0.941f);

	// Token: 0x040006AD RID: 1709
	public static readonly Colour Pink = new Colour(0.96f, 0.439f, 0.807f);

	// Token: 0x040006AE RID: 1710
	public static readonly Colour Grey = Colour.UnityGrey;

	// Token: 0x040006AF RID: 1711
	public static readonly Colour Black = new Colour(0.25f, 0.25f, 0.25f);

	// Token: 0x040006B0 RID: 1712
	public static readonly Colour Clear = Colour.UnityClear;

	// Token: 0x040006B1 RID: 1713
	public static readonly Colour BrownDark = new Colour(0.298f, 0.145f, 0.043f);

	// Token: 0x040006B2 RID: 1714
	public static readonly Colour RedDark = new Colour(0.588f, 0.003f, 0f);

	// Token: 0x040006B3 RID: 1715
	public static readonly Colour OrangeDark = new Colour(0.65f, 0.294f, 0f);

	// Token: 0x040006B4 RID: 1716
	public static readonly Colour YellowDark = new Colour(0.721f, 0.713f, 0.003f);

	// Token: 0x040006B5 RID: 1717
	public static readonly Colour GreenDark = new Colour(0.027f, 0.419f, 0.007f);

	// Token: 0x040006B6 RID: 1718
	public static readonly Colour TealDark = new Colour(0.058f, 0.541f, 0.466f);

	// Token: 0x040006B7 RID: 1719
	public static readonly Colour BlueDark = new Colour(0.011f, 0.274f, 0.58f);

	// Token: 0x040006B8 RID: 1720
	public static readonly Colour PurpleDark = new Colour(0.345f, 0.007f, 0.552f);

	// Token: 0x040006B9 RID: 1721
	public static readonly Colour PinkDark = new Colour(0.596f, 0.027f, 0.427f);

	// Token: 0x040006BA RID: 1722
	public static readonly Colour GreyDark = new Colour(0.415f, 0.415f, 0.415f);

	// Token: 0x040006BB RID: 1723
	public static readonly Colour BlackDark = new Colour(0f, 0f, 0f);

	// Token: 0x040006BC RID: 1724
	public const string WhiteLabel = "White";

	// Token: 0x040006BD RID: 1725
	public const string BrownLabel = "Brown";

	// Token: 0x040006BE RID: 1726
	public const string RedLabel = "Red";

	// Token: 0x040006BF RID: 1727
	public const string OrangeLabel = "Orange";

	// Token: 0x040006C0 RID: 1728
	public const string YellowLabel = "Yellow";

	// Token: 0x040006C1 RID: 1729
	public const string GreenLabel = "Green";

	// Token: 0x040006C2 RID: 1730
	public const string TealLabel = "Teal";

	// Token: 0x040006C3 RID: 1731
	public const string BlueLabel = "Blue";

	// Token: 0x040006C4 RID: 1732
	public const string PurpleLabel = "Purple";

	// Token: 0x040006C5 RID: 1733
	public const string PinkLabel = "Pink";

	// Token: 0x040006C6 RID: 1734
	public const string GreyLabel = "Grey";

	// Token: 0x040006C7 RID: 1735
	public const string BlackLabel = "Black";

	// Token: 0x040006C8 RID: 1736
	public const string AllLabel = "All";

	// Token: 0x040006C9 RID: 1737
	public const string SeatedLabel = "Seated";

	// Token: 0x040006CA RID: 1738
	public const uint WhiteFlag = 1U;

	// Token: 0x040006CB RID: 1739
	public const uint BrownFlag = 2U;

	// Token: 0x040006CC RID: 1740
	public const uint RedFlag = 4U;

	// Token: 0x040006CD RID: 1741
	public const uint OrangeFlag = 8U;

	// Token: 0x040006CE RID: 1742
	public const uint YellowFlag = 16U;

	// Token: 0x040006CF RID: 1743
	public const uint GreenFlag = 32U;

	// Token: 0x040006D0 RID: 1744
	public const uint TealFlag = 64U;

	// Token: 0x040006D1 RID: 1745
	public const uint BlueFlag = 128U;

	// Token: 0x040006D2 RID: 1746
	public const uint PurpleFlag = 256U;

	// Token: 0x040006D3 RID: 1747
	public const uint PinkFlag = 512U;

	// Token: 0x040006D4 RID: 1748
	public const uint BlackFlag = 0U;

	// Token: 0x040006D5 RID: 1749
	public const uint GreyFlag = 1024U;

	// Token: 0x040006D6 RID: 1750
	public const uint AllFlags = 4294967295U;

	// Token: 0x040006D7 RID: 1751
	public const uint AllPlayerFlags = 2047U;

	// Token: 0x040006D8 RID: 1752
	public static Colour UIBlue = Colour.ColourFromRGBHex("1C97FFFF");

	// Token: 0x040006D9 RID: 1753
	private static readonly Dictionary<Colour, string> labelFromColour = new Dictionary<Colour, string>
	{
		{
			Colour.White,
			"White"
		},
		{
			Colour.Brown,
			"Brown"
		},
		{
			Colour.Red,
			"Red"
		},
		{
			Colour.Orange,
			"Orange"
		},
		{
			Colour.Yellow,
			"Yellow"
		},
		{
			Colour.Green,
			"Green"
		},
		{
			Colour.Teal,
			"Teal"
		},
		{
			Colour.Blue,
			"Blue"
		},
		{
			Colour.Purple,
			"Purple"
		},
		{
			Colour.Pink,
			"Pink"
		},
		{
			Colour.Grey,
			"Grey"
		},
		{
			Colour.Black,
			"Black"
		}
	};

	// Token: 0x040006DA RID: 1754
	private static readonly Dictionary<Colour, int> idFromColour = new Dictionary<Colour, int>
	{
		{
			Colour.White,
			0
		},
		{
			Colour.Brown,
			1
		},
		{
			Colour.Red,
			2
		},
		{
			Colour.Orange,
			3
		},
		{
			Colour.Yellow,
			4
		},
		{
			Colour.Green,
			5
		},
		{
			Colour.Teal,
			6
		},
		{
			Colour.Blue,
			7
		},
		{
			Colour.Purple,
			8
		},
		{
			Colour.Pink,
			9
		},
		{
			Colour.Black,
			10
		},
		{
			Colour.Grey,
			-1
		},
		{
			Colour.Clear,
			-2
		}
	};

	// Token: 0x040006DB RID: 1755
	private static readonly Dictionary<Colour, Colour> darkenedFromColour = new Dictionary<Colour, Colour>
	{
		{
			Colour.White,
			Colour.GreyDark
		},
		{
			Colour.Brown,
			Colour.BrownDark
		},
		{
			Colour.Red,
			Colour.RedDark
		},
		{
			Colour.Orange,
			Colour.OrangeDark
		},
		{
			Colour.Yellow,
			Colour.YellowDark
		},
		{
			Colour.Green,
			Colour.GreenDark
		},
		{
			Colour.Teal,
			Colour.TealDark
		},
		{
			Colour.Blue,
			Colour.BlueDark
		},
		{
			Colour.Purple,
			Colour.PurpleDark
		},
		{
			Colour.Pink,
			Colour.PinkDark
		},
		{
			Colour.Grey,
			Colour.GreyDark
		},
		{
			Colour.Black,
			Colour.BlackDark
		}
	};

	// Token: 0x040006DC RID: 1756
	private static readonly Dictionary<Colour, uint> flagFromColour = new Dictionary<Colour, uint>
	{
		{
			Colour.White,
			1U
		},
		{
			Colour.Brown,
			2U
		},
		{
			Colour.Red,
			4U
		},
		{
			Colour.Orange,
			8U
		},
		{
			Colour.Yellow,
			16U
		},
		{
			Colour.Green,
			32U
		},
		{
			Colour.Teal,
			64U
		},
		{
			Colour.Blue,
			128U
		},
		{
			Colour.Purple,
			256U
		},
		{
			Colour.Pink,
			512U
		},
		{
			Colour.Black,
			0U
		},
		{
			Colour.Grey,
			1024U
		}
	};

	// Token: 0x040006DD RID: 1757
	public const string WhiteHex = "[FFFFFF]";

	// Token: 0x040006DE RID: 1758
	public const string BrownHex = "[713B17]";

	// Token: 0x040006DF RID: 1759
	public const string RedHex = "[DA1918]";

	// Token: 0x040006E0 RID: 1760
	public const string OrangeHex = "[F4641D]";

	// Token: 0x040006E1 RID: 1761
	public const string YellowHex = "[E7E52C]";

	// Token: 0x040006E2 RID: 1762
	public const string GreenHex = "[31B32B]";

	// Token: 0x040006E3 RID: 1763
	public const string TealHex = "[21B19B]";

	// Token: 0x040006E4 RID: 1764
	public const string BlueHex = "[1F87FF]";

	// Token: 0x040006E5 RID: 1765
	public const string PurpleHex = "[A020F0]";

	// Token: 0x040006E6 RID: 1766
	public const string PinkHex = "[F570CE]";

	// Token: 0x040006E7 RID: 1767
	public const string GreyHex = "[AAAAAA]";

	// Token: 0x040006E8 RID: 1768
	public const string BlackHex = "[191919]";

	// Token: 0x040006E9 RID: 1769
	public const string BrownDarkHex = "[4C250B]";

	// Token: 0x040006EA RID: 1770
	public const string RedDarkHex = "[960100]";

	// Token: 0x040006EB RID: 1771
	public const string OrangeDarkHex = "[A64B00]";

	// Token: 0x040006EC RID: 1772
	public const string YellowDarkHex = "[B8B601]";

	// Token: 0x040006ED RID: 1773
	public const string GreenDarkHex = "[076B02]";

	// Token: 0x040006EE RID: 1774
	public const string TealDarkHex = "[006D5C]";

	// Token: 0x040006EF RID: 1775
	public const string BlueDarkHex = "[034694]";

	// Token: 0x040006F0 RID: 1776
	public const string PurpleDarkHex = "[58028D]";

	// Token: 0x040006F1 RID: 1777
	public const string PinkDarkHex = "[98076D]";

	// Token: 0x040006F2 RID: 1778
	public const string GreyDarkHex = "[6A6A6A]";

	// Token: 0x040006F3 RID: 1779
	public const string BlackDarkHex = "[000000]";

	// Token: 0x040006F4 RID: 1780
	private static readonly Dictionary<Colour, string> hexFromColour = new Dictionary<Colour, string>
	{
		{
			Colour.White,
			"[FFFFFF]"
		},
		{
			Colour.Brown,
			"[713B17]"
		},
		{
			Colour.Red,
			"[DA1918]"
		},
		{
			Colour.UnityRed,
			"[DA1918]"
		},
		{
			Colour.Orange,
			"[F4641D]"
		},
		{
			Colour.Yellow,
			"[E7E52C]"
		},
		{
			Colour.UnityYellow,
			"[E7E52C]"
		},
		{
			Colour.Green,
			"[31B32B]"
		},
		{
			Colour.UnityGreen,
			"[31B32B]"
		},
		{
			Colour.Teal,
			"[21B19B]"
		},
		{
			Colour.UnityCyan,
			"[21B19B]"
		},
		{
			Colour.Blue,
			"[1F87FF]"
		},
		{
			Colour.UnityBlue,
			"[1F87FF]"
		},
		{
			Colour.Purple,
			"[A020F0]"
		},
		{
			Colour.Pink,
			"[F570CE]"
		},
		{
			Colour.Grey,
			"[AAAAAA]"
		},
		{
			Colour.Black,
			"[191919]"
		}
	};

	// Token: 0x040006F5 RID: 1781
	private static readonly Dictionary<Colour, UIPalette.UI> uiFromColour = new Dictionary<Colour, UIPalette.UI>
	{
		{
			Colour.White,
			UIPalette.UI.PlayerWhite
		},
		{
			Colour.Brown,
			UIPalette.UI.PlayerBrown
		},
		{
			Colour.Red,
			UIPalette.UI.PlayerRed
		},
		{
			Colour.UnityRed,
			UIPalette.UI.PlayerRed
		},
		{
			Colour.Orange,
			UIPalette.UI.PlayerOrange
		},
		{
			Colour.Yellow,
			UIPalette.UI.PlayerYellow
		},
		{
			Colour.UnityYellow,
			UIPalette.UI.PlayerYellow
		},
		{
			Colour.Green,
			UIPalette.UI.PlayerGreen
		},
		{
			Colour.UnityGreen,
			UIPalette.UI.PlayerGreen
		},
		{
			Colour.Teal,
			UIPalette.UI.PlayerTeal
		},
		{
			Colour.UnityCyan,
			UIPalette.UI.PlayerTeal
		},
		{
			Colour.Blue,
			UIPalette.UI.PlayerBlue
		},
		{
			Colour.UnityBlue,
			UIPalette.UI.PlayerBlue
		},
		{
			Colour.Purple,
			UIPalette.UI.PlayerPurple
		},
		{
			Colour.Pink,
			UIPalette.UI.PlayerPink
		},
		{
			Colour.Grey,
			UIPalette.UI.PlayerGrey
		},
		{
			Colour.Black,
			UIPalette.UI.PlayerBlack
		}
	};

	// Token: 0x040006F6 RID: 1782
	public const int NumberOfHandColours = 10;

	// Token: 0x040006F7 RID: 1783
	public static readonly Colour[] AllPlayerColours = new Colour[]
	{
		Colour.White,
		Colour.Brown,
		Colour.Red,
		Colour.Orange,
		Colour.Yellow,
		Colour.Green,
		Colour.Teal,
		Colour.Blue,
		Colour.Purple,
		Colour.Pink,
		Colour.Grey,
		Colour.Black
	};

	// Token: 0x040006F8 RID: 1784
	public static readonly Color[] HandPlayerColours = new Color[]
	{
		Colour.White,
		Colour.Brown,
		Colour.Red,
		Colour.Orange,
		Colour.Yellow,
		Colour.Green,
		Colour.Teal,
		Colour.Blue,
		Colour.Purple,
		Colour.Pink,
		Colour.Black
	};

	// Token: 0x040006F9 RID: 1785
	public static readonly string[] AllPlayerLabels = new string[]
	{
		"White",
		"Brown",
		"Red",
		"Orange",
		"Yellow",
		"Green",
		"Teal",
		"Blue",
		"Purple",
		"Pink",
		"Grey",
		"Black"
	};

	// Token: 0x040006FA RID: 1786
	public static readonly string[] HandPlayerLabels = new string[]
	{
		"White",
		"Brown",
		"Red",
		"Orange",
		"Yellow",
		"Green",
		"Teal",
		"Blue",
		"Purple",
		"Pink",
		"Black"
	};
}
