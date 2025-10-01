using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;

// Token: 0x020000C7 RID: 199
public class CountryCodesScript : MonoBehaviour
{
	// Token: 0x060009FA RID: 2554 RVA: 0x00046272 File Offset: 0x00044472
	private void Start()
	{
		if (CountryCodesScript.CountryCodes == null)
		{
			CountryCodesScript.CountryCodes = JsonConvert.DeserializeObject<List<CountryCode>>(this.CountryJson.text);
		}
	}

	// Token: 0x060009FB RID: 2555 RVA: 0x00046290 File Offset: 0x00044490
	public static string ISOCodeToProxy(string ISOCode)
	{
		string text = CountryCodesScript.ISOCodeToContinent(ISOCode);
		if (text == "AM")
		{
			return "NA";
		}
		if (text == "EU")
		{
			return "EU";
		}
		if (text == "AF")
		{
			return "EU";
		}
		if (text == "AN")
		{
			return "EU";
		}
		if (text == "OC")
		{
			return "OC";
		}
		if (!(text == "AS"))
		{
			Chat.LogError("Error converting continent code to proxy server: " + text, true);
			return "EU";
		}
		return "OC";
	}

	// Token: 0x060009FC RID: 2556 RVA: 0x00046330 File Offset: 0x00044530
	public static CountryCode ISOCodeToCountryCode(string ISOCode)
	{
		for (int i = 0; i < CountryCodesScript.CountryCodes.Count; i++)
		{
			if (CountryCodesScript.CountryCodes[i].sISOCode == ISOCode)
			{
				return CountryCodesScript.CountryCodes[i];
			}
		}
		return null;
	}

	// Token: 0x060009FD RID: 2557 RVA: 0x00046378 File Offset: 0x00044578
	public static string ISOCodeToContinent(string ISOCode)
	{
		CountryCode countryCode = CountryCodesScript.ISOCodeToCountryCode(ISOCode);
		if (countryCode != null)
		{
			return countryCode.sContinentCode;
		}
		Chat.LogError("Error country code to continent: " + ISOCode, true);
		return "";
	}

	// Token: 0x060009FE RID: 2558 RVA: 0x000463AC File Offset: 0x000445AC
	public static string ISOCodeToName(string ISOCode)
	{
		CountryCode countryCode = CountryCodesScript.ISOCodeToCountryCode(ISOCode);
		if (countryCode != null)
		{
			return countryCode.sName;
		}
		return "";
	}

	// Token: 0x0400071B RID: 1819
	public TextAsset CountryJson;

	// Token: 0x0400071C RID: 1820
	public static List<CountryCode> CountryCodes;
}
