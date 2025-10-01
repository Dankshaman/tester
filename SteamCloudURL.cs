using System;
using System.Text.RegularExpressions;

// Token: 0x0200023C RID: 572
public static class SteamCloudURL
{
	// Token: 0x06001C54 RID: 7252 RVA: 0x000C3C17 File Offset: 0x000C1E17
	public static bool IsOldCloudURL(string url)
	{
		return url.StartsWith("http://cloud-3.steamusercontent.com/ugc/", StringComparison.OrdinalIgnoreCase);
	}

	// Token: 0x06001C55 RID: 7253 RVA: 0x000C3C25 File Offset: 0x000C1E25
	public static bool IsNewCloudURL(string url)
	{
		return url.StartsWith("https://steamusercontent-a.akamaihd.net/ugc/", StringComparison.OrdinalIgnoreCase);
	}

	// Token: 0x06001C56 RID: 7254 RVA: 0x000C3C33 File Offset: 0x000C1E33
	public static bool IsCloudURL(string url)
	{
		return url.StartsWith("https://steamusercontent-a.akamaihd.net/ugc/", StringComparison.OrdinalIgnoreCase) || url.StartsWith("http://cloud-3.steamusercontent.com/ugc/", StringComparison.OrdinalIgnoreCase);
	}

	// Token: 0x06001C57 RID: 7255 RVA: 0x000C3C54 File Offset: 0x000C1E54
	public static string ConvertOldToNewCloudURL(string url)
	{
		string pattern = "http://cloud-3\\.steamusercontent\\.com/ugc/";
		string replacement = "https://steamusercontent-a.akamaihd.net/ugc/";
		return Regex.Replace(url, pattern, replacement, RegexOptions.IgnoreCase | RegexOptions.Multiline);
	}

	// Token: 0x06001C58 RID: 7256 RVA: 0x000C3C78 File Offset: 0x000C1E78
	public static string ConvertNewToOldCloudURL(string url)
	{
		string pattern = "https://steamusercontent-a\\.akamaihd\\.net/ugc/";
		string replacement = "http://cloud-3.steamusercontent.com/ugc/";
		return Regex.Replace(url, pattern, replacement, RegexOptions.IgnoreCase | RegexOptions.Multiline);
	}

	// Token: 0x040011F8 RID: 4600
	private const string oldCloudURL = "http://cloud-3.steamusercontent.com/ugc/";

	// Token: 0x040011F9 RID: 4601
	public const string newCloudURL = "https://steamusercontent-a.akamaihd.net/ugc/";
}
