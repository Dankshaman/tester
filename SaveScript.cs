using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

// Token: 0x02000225 RID: 549
public static class SaveScript
{
	// Token: 0x06001B4F RID: 6991 RVA: 0x000BC218 File Offset: 0x000BA418
	public static void SaveStateURLs(SaveState SS, Func<string, string> func)
	{
		SS.TableURL = func(SS.TableURL);
		SS.SkyURL = func(SS.SkyURL);
		if (SS.Lighting != null)
		{
			SS.Lighting.LutURL = func(SS.Lighting.LutURL);
		}
		if (SS.Decals != null)
		{
			for (int i = 0; i < SS.Decals.Count; i++)
			{
				if (SS.Decals[i].CustomDecal != null)
				{
					SS.Decals[i].CustomDecal.ImageURL = func(SS.Decals[i].CustomDecal.ImageURL);
				}
			}
		}
		if (SS.CustomUIAssets != null)
		{
			for (int j = 0; j < SS.CustomUIAssets.Count; j++)
			{
				SS.CustomUIAssets[j].URL = func(SS.CustomUIAssets[j].URL);
			}
		}
		SaveScript.ObjectStateURLs(SS.ObjectStates, func);
	}

	// Token: 0x06001B50 RID: 6992 RVA: 0x000BC330 File Offset: 0x000BA530
	private static void ObjectStateURLs(List<ObjectState> ObjectStates, Func<string, string> func)
	{
		for (int i = 0; i < ObjectStates.Count; i++)
		{
			ObjectState objectState = ObjectStates[i];
			if (objectState.CustomDeck != null)
			{
				foreach (KeyValuePair<int, CustomDeckState> keyValuePair in objectState.CustomDeck)
				{
					if (keyValuePair.Value != null)
					{
						keyValuePair.Value.FaceURL = func(keyValuePair.Value.FaceURL);
						keyValuePair.Value.BackURL = func(keyValuePair.Value.BackURL);
					}
				}
			}
			if (objectState.CustomImage != null)
			{
				objectState.CustomImage.ImageURL = func(objectState.CustomImage.ImageURL);
				objectState.CustomImage.ImageSecondaryURL = func(objectState.CustomImage.ImageSecondaryURL);
			}
			if (objectState.CustomMesh != null)
			{
				objectState.CustomMesh.MeshURL = func(objectState.CustomMesh.MeshURL);
				objectState.CustomMesh.DiffuseURL = func(objectState.CustomMesh.DiffuseURL);
				objectState.CustomMesh.MeshURL = func(objectState.CustomMesh.MeshURL);
				objectState.CustomMesh.ColliderURL = func(objectState.CustomMesh.ColliderURL);
				objectState.CustomMesh.NormalURL = func(objectState.CustomMesh.NormalURL);
			}
			if (objectState.CustomAssetbundle != null)
			{
				objectState.CustomAssetbundle.AssetbundleURL = func(objectState.CustomAssetbundle.AssetbundleURL);
				objectState.CustomAssetbundle.AssetbundleSecondaryURL = func(objectState.CustomAssetbundle.AssetbundleSecondaryURL);
			}
			if (objectState.CustomPDF != null)
			{
				objectState.CustomPDF.PDFUrl = func(objectState.CustomPDF.PDFUrl);
				objectState.CustomPDF.PDFPassword = func(objectState.CustomPDF.PDFPassword);
			}
			if (objectState.ContainedObjects != null)
			{
				SaveScript.ObjectStateURLs(objectState.ContainedObjects, func);
			}
			if (objectState.States != null)
			{
				List<ObjectState> list = new List<ObjectState>();
				foreach (KeyValuePair<int, ObjectState> keyValuePair2 in objectState.States)
				{
					list.Add(keyValuePair2.Value);
				}
				SaveScript.ObjectStateURLs(list, func);
			}
			if (objectState.AttachedDecals != null)
			{
				for (int j = 0; j < objectState.AttachedDecals.Count; j++)
				{
					if (objectState.AttachedDecals[j].CustomDecal != null)
					{
						objectState.AttachedDecals[j].CustomDecal.ImageURL = func(objectState.AttachedDecals[j].CustomDecal.ImageURL);
					}
				}
			}
			if (objectState.CustomUIAssets != null)
			{
				for (int k = 0; k < objectState.CustomUIAssets.Count; k++)
				{
					objectState.CustomUIAssets[k].URL = func(objectState.CustomUIAssets[k].URL);
				}
			}
			if (objectState.ChildObjects != null)
			{
				SaveScript.ObjectStateURLs(objectState.ChildObjects, func);
			}
		}
	}

	// Token: 0x06001B51 RID: 6993 RVA: 0x000BC6A4 File Offset: 0x000BA8A4
	public static int UpdateMatchingCustomObjects(SaveState SS, ObjectState oldOS, ObjectState newOS)
	{
		SaveScript.<>c__DisplayClass2_0 CS$<>8__locals1 = new SaveScript.<>c__DisplayClass2_0();
		CS$<>8__locals1.newOS = newOS;
		CS$<>8__locals1.count = 0;
		if (CS$<>8__locals1.newOS.CustomImage != null)
		{
			CS$<>8__locals1.json = Json.GetJson(oldOS.CustomImage, false);
			SaveScript.<UpdateMatchingCustomObjects>g__UpdateCustom|2_0(SS.ObjectStates, new Action<ObjectState>(CS$<>8__locals1.<UpdateMatchingCustomObjects>g__UpdateCustomImage|1));
		}
		if (CS$<>8__locals1.newOS.CustomMesh != null)
		{
			CS$<>8__locals1.json = Json.GetJson(oldOS.CustomMesh, false);
			SaveScript.<UpdateMatchingCustomObjects>g__UpdateCustom|2_0(SS.ObjectStates, new Action<ObjectState>(CS$<>8__locals1.<UpdateMatchingCustomObjects>g__UpdateCustomMesh|2));
		}
		if (CS$<>8__locals1.newOS.CustomAssetbundle != null)
		{
			CS$<>8__locals1.json = Json.GetJson(oldOS.CustomAssetbundle, false);
			SaveScript.<UpdateMatchingCustomObjects>g__UpdateCustom|2_0(SS.ObjectStates, new Action<ObjectState>(CS$<>8__locals1.<UpdateMatchingCustomObjects>g__UpdateCustomAssetbundle|3));
		}
		if (CS$<>8__locals1.newOS.CustomPDF != null)
		{
			CS$<>8__locals1.json = Json.GetJson(oldOS.CustomPDF, false);
			SaveScript.<UpdateMatchingCustomObjects>g__UpdateCustom|2_0(SS.ObjectStates, new Action<ObjectState>(CS$<>8__locals1.<UpdateMatchingCustomObjects>g__UpdateCustomPDF|4));
		}
		return CS$<>8__locals1.count;
	}

	// Token: 0x06001B52 RID: 6994 RVA: 0x000BC7BC File Offset: 0x000BA9BC
	private static string Strip_TTS_URL_Encoding(string url)
	{
		string text = CardManagerScript.ConvertUnique(url);
		bool flag;
		TextCode.CheckVerifyCacheCode(text, out text, out flag);
		return text;
	}

	// Token: 0x06001B53 RID: 6995 RVA: 0x000BC7DB File Offset: 0x000BA9DB
	public static List<string> GetURLs(SaveState SS)
	{
		SaveScript.URLs.Clear();
		SaveScript.SaveStateURLs(SS, new Func<string, string>(SaveScript.CheckURL));
		return SaveScript.URLs;
	}

	// Token: 0x06001B54 RID: 6996 RVA: 0x000BC800 File Offset: 0x000BAA00
	private static string CheckURL(string url)
	{
		if (!string.IsNullOrEmpty(url))
		{
			string text = SaveScript.Strip_TTS_URL_Encoding(url);
			List<string> localizations = TextCode.GetLocalizations(text, TextCode.HandleWhiteSpace.Trim);
			if (localizations.Count > 0)
			{
				using (List<string>.Enumerator enumerator = localizations.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						string item = enumerator.Current;
						SaveScript.URLs.TryAddUnique(item);
					}
					return url;
				}
			}
			SaveScript.URLs.TryAddUnique(text);
		}
		return url;
	}

	// Token: 0x06001B55 RID: 6997 RVA: 0x000BC880 File Offset: 0x000BAA80
	public static void SetURLs(SaveState SS, Dictionary<string, string> urlReplacer)
	{
		SaveScript.URLReplacer = urlReplacer;
		SaveScript.SaveStateURLs(SS, new Func<string, string>(SaveScript.SetURL));
	}

	// Token: 0x06001B56 RID: 6998 RVA: 0x000BC89C File Offset: 0x000BAA9C
	private static string SetURL(string url)
	{
		if (!string.IsNullOrEmpty(url))
		{
			string text = SaveScript.Strip_TTS_URL_Encoding(url);
			string text2;
			if (TextCode.HasLocalizationCodes(text))
			{
				foreach (KeyValuePair<string, string> keyValuePair in SaveScript.URLReplacer)
				{
					if (text.Contains(keyValuePair.Key))
					{
						text = text.Replace(keyValuePair.Key, keyValuePair.Value);
					}
				}
				url = text;
			}
			else if (SaveScript.URLReplacer.TryGetValue(text, out text2))
			{
				url = text2;
			}
		}
		return url;
	}

	// Token: 0x06001B57 RID: 6999 RVA: 0x000BC93C File Offset: 0x000BAB3C
	public static List<string> GetLocalURLs(SaveState SS)
	{
		SaveScript.LocalURLs.Clear();
		SaveScript.SaveStateURLs(SS, new Func<string, string>(SaveScript.CheckLocalURL));
		return SaveScript.LocalURLs;
	}

	// Token: 0x06001B58 RID: 7000 RVA: 0x000BC960 File Offset: 0x000BAB60
	private static string CheckLocalURL(string url)
	{
		if (!string.IsNullOrEmpty(url))
		{
			string text = SaveScript.Strip_TTS_URL_Encoding(url);
			bool flag;
			TextCode.CheckLocalizationCode(text, out text, out flag, TextCode.HandleWhiteSpace.Trim);
			if (DataRequest.IsLocalFile(text) || (!SaveScript.ValidURL(text) && !DLCManager.URLisDLC(text)))
			{
				SaveScript.LocalURLs.TryAddUnique(text);
			}
		}
		return url;
	}

	// Token: 0x06001B59 RID: 7001 RVA: 0x000BC9AC File Offset: 0x000BABAC
	public static bool ValidURL(string url)
	{
		if (!url.StartsWith("http"))
		{
			url = "http://" + url;
		}
		Uri uri;
		return Uri.TryCreate(url, UriKind.Absolute, out uri) && (uri.Scheme == Uri.UriSchemeHttp || uri.Scheme == Uri.UriSchemeHttps);
	}

	// Token: 0x06001B5B RID: 7003 RVA: 0x000BCA24 File Offset: 0x000BAC24
	[CompilerGenerated]
	internal static void <UpdateMatchingCustomObjects>g__UpdateCustom|2_0(List<ObjectState> objectStates, Action<ObjectState> updateFunction)
	{
		foreach (ObjectState objectState in objectStates)
		{
			updateFunction(objectState);
			if (objectState.ContainedObjects != null)
			{
				List<ObjectState> list = new List<ObjectState>(objectState.ContainedObjects.Count);
				foreach (ObjectState objectState2 in objectState.ContainedObjects)
				{
					list.Add(objectState2.Clone());
				}
				objectState.ContainedObjects = list;
				SaveScript.<UpdateMatchingCustomObjects>g__UpdateCustom|2_0(objectState.ContainedObjects, updateFunction);
			}
			if (objectState.ChildObjects != null)
			{
				SaveScript.<UpdateMatchingCustomObjects>g__UpdateCustom|2_0(objectState.ChildObjects, updateFunction);
			}
		}
	}

	// Token: 0x04001153 RID: 4435
	private static readonly List<string> URLs = new List<string>();

	// Token: 0x04001154 RID: 4436
	private static Dictionary<string, string> URLReplacer = new Dictionary<string, string>();

	// Token: 0x04001155 RID: 4437
	private static readonly List<string> LocalURLs = new List<string>();
}
