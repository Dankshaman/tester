using System;
using System.Collections.Generic;
using System.IO;
using I2.Loc;
using UnityEngine;

// Token: 0x0200013C RID: 316
public class Language : Singleton<Language>
{
	// Token: 0x170002ED RID: 749
	// (get) Token: 0x0600104F RID: 4175 RVA: 0x0006F9BF File Offset: 0x0006DBBF
	// (set) Token: 0x06001050 RID: 4176 RVA: 0x0006F9C6 File Offset: 0x0006DBC6
	public static string CurrentLanguage { get; private set; } = "English";

	// Token: 0x170002EE RID: 750
	// (get) Token: 0x06001051 RID: 4177 RVA: 0x0006F9CE File Offset: 0x0006DBCE
	// (set) Token: 0x06001052 RID: 4178 RVA: 0x0006F9D5 File Offset: 0x0006DBD5
	public static string CurrentLanguageCode { get; private set; } = "en";

	// Token: 0x06001053 RID: 4179 RVA: 0x0006F9DD File Offset: 0x0006DBDD
	public static void SetCSVFilenameFromCode(string languageCode, string translationFilename)
	{
		Language.CSVFilenameFromCode[languageCode] = translationFilename;
		PlayerPrefs.SetString("translation_for_" + languageCode.Replace("-", "_"), translationFilename);
	}

	// Token: 0x06001054 RID: 4180 RVA: 0x0006FA0C File Offset: 0x0006DC0C
	public static string CodeFromLanguage(string languageName)
	{
		for (int i = 0; i < Language.OrderedLanguageCodes.Count; i++)
		{
			string text = Language.OrderedLanguageCodes[i];
			if (Language.LanguageFromCode[text].ToLower() == languageName.ToLower())
			{
				return text;
			}
		}
		return "";
	}

	// Token: 0x06001055 RID: 4181 RVA: 0x0006FA5E File Offset: 0x0006DC5E
	public static string DefaultFileName(string languageCode)
	{
		return "localization_default_" + languageCode + ".csv";
	}

	// Token: 0x06001056 RID: 4182 RVA: 0x0006FA70 File Offset: 0x0006DC70
	public static bool TryGetLanguageName(string potentialLanguage, out string languageName)
	{
		string key = potentialLanguage.ToLower();
		if (Language.LanguageFromCode.ContainsKey(key))
		{
			languageName = Language.LanguageFromCode[key];
		}
		else
		{
			languageName = potentialLanguage;
		}
		return LocalizationManager.HasLanguage(languageName, true, true, true);
	}

	// Token: 0x06001057 RID: 4183 RVA: 0x0006FAB0 File Offset: 0x0006DCB0
	public bool SetLanguage(string potentialLanguage, bool force = false)
	{
		string text;
		if (!Language.TryGetLanguageName(potentialLanguage, out text))
		{
			return false;
		}
		string text2 = Language.CodeFromLanguage(text);
		if (text2 == Language.CurrentLanguageCode && !force)
		{
			return true;
		}
		string currentLanguageCode = Language.CurrentLanguageCode;
		Language.CurrentLanguageCode = text2;
		string text3 = Language.CSVFilenameFromCode[Language.CurrentLanguageCode];
		if (text3 != "")
		{
			string filePath = Path.Combine(DirectoryScript.translationsPath, text3);
			CSV csv;
			if (Language.TryReadValidCSVFile(Language.CurrentLanguageCode, filePath, out csv))
			{
				this.ImportCSV(csv.ToString());
			}
			else
			{
				Language.SetCSVFilenameFromCode(Language.CurrentLanguageCode, "");
				text3 = "";
			}
		}
		if (text3 == "")
		{
			CSV csv2 = Language.DefaultCSVForLanguage(Language.CurrentLanguageCode);
			this.ImportCSV(csv2.ToString());
		}
		Language.CurrentLanguage = text;
		LocalizationManager.CurrentLanguage = text;
		PlayerPrefs.SetString("language_code", text2);
		EventManager.TriggerLanguageChange(currentLanguageCode, text2);
		return true;
	}

	// Token: 0x06001058 RID: 4184 RVA: 0x0006FB98 File Offset: 0x0006DD98
	public bool SetLanguageAndTranslation(string languageCode, string translationFilename)
	{
		bool flag = languageCode == Language.CurrentLanguageCode;
		Language.SetCSVFilenameFromCode(languageCode, translationFilename);
		bool flag2 = this.SetLanguage(languageCode, true) && Language.CSVFilenameFromCode[languageCode] == translationFilename;
		if (flag2)
		{
			PlayerPrefs.SetString("translation_for_" + languageCode.Replace("-", "_"), translationFilename);
		}
		if (flag)
		{
			this.RefreshCurrentLanguage();
		}
		return flag2;
	}

	// Token: 0x06001059 RID: 4185 RVA: 0x0006FC04 File Offset: 0x0006DE04
	public static string Translate(string key)
	{
		if (key.Contains("<") || key.Contains(">"))
		{
			return LocalizationManager.GetTranslation(key.Replace("<", "(").Replace(">", ")"), true, 0, true, false, null, null) ?? key;
		}
		return LocalizationManager.GetTranslation(key, true, 0, true, false, null, null) ?? key;
	}

	// Token: 0x0600105A RID: 4186 RVA: 0x0006FC6C File Offset: 0x0006DE6C
	public static string Translate(string key, object arg0)
	{
		return string.Format(Language.Translate(key), arg0);
	}

	// Token: 0x0600105B RID: 4187 RVA: 0x0006FC7A File Offset: 0x0006DE7A
	public static string Translate(string key, object arg0, object arg1)
	{
		return string.Format(Language.Translate(key), arg0, arg1);
	}

	// Token: 0x0600105C RID: 4188 RVA: 0x0006FC89 File Offset: 0x0006DE89
	public static string Translate(string key, object arg0, object arg1, object arg2)
	{
		return string.Format(Language.Translate(key), arg0, arg1, arg2);
	}

	// Token: 0x0600105D RID: 4189 RVA: 0x0006FC99 File Offset: 0x0006DE99
	public static string Translate(string key, params object[] args)
	{
		return string.Format(Language.Translate(key), args);
	}

	// Token: 0x0600105E RID: 4190 RVA: 0x0006FCA8 File Offset: 0x0006DEA8
	public static CSV GetCurrentCustomCSVFromDisk()
	{
		string text = Language.CSVFilenameFromCode[Language.CurrentLanguageCode];
		if (string.IsNullOrEmpty(text))
		{
			return null;
		}
		string filePath = Path.Combine(DirectoryScript.translationsPath, text);
		CSV result;
		if (Language.TryReadValidCSVFile(Language.CurrentLanguageCode, filePath, out result))
		{
			return result;
		}
		return null;
	}

	// Token: 0x0600105F RID: 4191 RVA: 0x0006FCF2 File Offset: 0x0006DEF2
	public static CSV ExportCurrentCSV(string title = "Custom")
	{
		return Language.ExportCSV(LocalizationManager.CurrentLanguageCode, title);
	}

	// Token: 0x06001060 RID: 4192 RVA: 0x0006FD00 File Offset: 0x0006DF00
	public static CSV ExportCSV(string languageCode, string title = "Custom")
	{
		CSV sourceCSV = new CSV(LocalizationManager.Sources[0].Export_CSV(null, ',', true));
		return Language.ExportCSVHelper(languageCode, sourceCSV, title);
	}

	// Token: 0x06001061 RID: 4193 RVA: 0x0006FD30 File Offset: 0x0006DF30
	public static CSV ExportCSVHelper(string languageCode, CSV sourceCSV, string title = "Custom")
	{
		string header;
		if (Language.TryGetLanguageName(languageCode, out header))
		{
			CSV csv = new CSV(LocalizationManager.Sources[0].Export_CSV(null, ',', true));
			List<string> list = new List<string>(csv.RowCount)
			{
				"Default"
			};
			for (int i = 1; i < csv.RowCount; i++)
			{
				list.Add(csv[3][i]);
			}
			csv.InsertColumn(3, list);
			int num = csv.ColumnFromHeader(header);
			CSV csv2 = new CSV(csv, new int[]
			{
				0,
				2,
				3,
				num
			});
			csv2.InsertRow(1, new List<string>
			{
				"_Title",
				title
			});
			csv2.InsertRow(2, new List<string>
			{
				"_Author",
				SteamManager.SteamName
			});
			return csv2;
		}
		return null;
	}

	// Token: 0x06001062 RID: 4194 RVA: 0x0006FE10 File Offset: 0x0006E010
	public static CSV ExportDefaultCSV(string languageCode)
	{
		string csvData = Resources.Load<TextAsset>("Localization").ToString();
		CSV sourceCSV;
		try
		{
			sourceCSV = new CSV(csvData);
		}
		catch (Exception)
		{
			throw new Exception("Failed to parse Localization.csv, likely because it was not exported from the language source asset.\nPlease select file \"I2 TTS\\Resources\\I2 Languages...\", and on the Spreadsheets tab choose Export->Replace\nThen overwrite the Localization.csv file in the same folder.  You will need to check out both files.");
		}
		return Language.ExportCSVHelper(languageCode, sourceCSV, "Custom");
	}

	// Token: 0x06001063 RID: 4195 RVA: 0x0006FE60 File Offset: 0x0006E060
	public static CSV DefaultCSVForLanguage(string languageCode)
	{
		string header;
		if (Language.TryGetLanguageName(languageCode, out header))
		{
			string text = Resources.Load<TextAsset>("Localization").text;
			CSV csv;
			try
			{
				csv = new CSV(text);
			}
			catch (Exception exception)
			{
				Debug.LogException(exception);
				Debug.Log(text);
				throw new Exception("Failed to parse Localization.csv, likely because it was not exported from the language source asset.Please select file \"I2 TTS\\Resources\\I2 Languages...\", and on the Spreadsheets tab choose Export->ReplaceThen overwrite the Localization.csv file in the same folder.  You will need to check out both files.");
			}
			int num = csv.ColumnFromHeader(header);
			return new CSV(csv, new int[]
			{
				0,
				1,
				2,
				num
			});
		}
		return null;
	}

	// Token: 0x06001064 RID: 4196 RVA: 0x0006FED8 File Offset: 0x0006E0D8
	public static string ExportCSVFile(string filePath, string languageCode)
	{
		try
		{
			if ((File.GetAttributes(filePath) & FileAttributes.Directory) == FileAttributes.Directory)
			{
				filePath = Path.Combine(filePath, "tts_translation.csv");
			}
		}
		catch (Exception)
		{
		}
		if (!filePath.EndsWith(".csv"))
		{
			filePath += ".csv";
		}
		LocalizationManager.Sources[0].Export_CSV(null, ',', true);
		CSV csv = Language.ExportCSV(languageCode, "Custom");
		string result;
		try
		{
			File.WriteAllText(filePath, csv.ToString());
			result = filePath;
		}
		catch (Exception ex)
		{
			Chat.LogError("Save .csv error: " + ex.Message, true);
			result = null;
		}
		return result;
	}

	// Token: 0x06001065 RID: 4197 RVA: 0x0006FF88 File Offset: 0x0006E188
	public Result ImportCSV(string csvString)
	{
		CSV csv = new CSV(csvString);
		if (csv.ColumnFromHeader("Type") == -1)
		{
			List<string> list = new List<string>(csv.RowCount)
			{
				"Type"
			};
			for (int i = 1; i < csv.RowCount; i++)
			{
				list.Add("Text");
			}
			csv.InsertColumn(1, list);
		}
		int num = csv.RowFromKey("_Title");
		int num2 = csv.RowFromKey("_Author");
		if (num != -1)
		{
			csv.RemoveRow(num);
		}
		if (num2 != -1)
		{
			csv.RemoveRow(num2);
		}
		int num3 = csv.ColumnFromHeader("Default");
		if (num3 != -1)
		{
			csv.RemoveColumn(num3);
		}
		string text = LocalizationManager.Sources[0].Import_CSV(null, csv.ToString(), eSpreadsheetUpdateMode.Merge, ',');
		if (text == string.Empty)
		{
			this.RefreshCurrentLanguage();
			return LibString.OK();
		}
		return LibString.Error(text);
	}

	// Token: 0x06001066 RID: 4198 RVA: 0x00070070 File Offset: 0x0006E270
	public Result ImportCSVFile(string filePath)
	{
		Result result;
		try
		{
			string csvString = File.ReadAllText(filePath);
			result = this.ImportCSV(csvString);
		}
		catch (Exception ex)
		{
			result = LibString.Error("Load .csv error: " + ex.Message);
		}
		return result;
	}

	// Token: 0x06001067 RID: 4199 RVA: 0x000700B8 File Offset: 0x0006E2B8
	public static Result TryReadValidCSVFile(string languageCode, string filePath, out CSV csv)
	{
		string text = "";
		if (languageCode != "")
		{
			text = Language.LanguageFromCode[languageCode];
			if (!LocalizationManager.HasLanguage(text, true, true, true))
			{
				csv = new CSV("");
				return LibString.Error("Invalid language for import");
			}
		}
		string csvData;
		try
		{
			csvData = File.ReadAllText(filePath);
		}
		catch (Exception ex)
		{
			csv = new CSV("");
			return LibString.Error("Load .csv error: " + ex.Message);
		}
		csv = new CSV(csvData);
		return Language.isValidTranslationCSV(csv, text);
	}

	// Token: 0x06001068 RID: 4200 RVA: 0x00070158 File Offset: 0x0006E358
	private static Result isValidTranslationCSV(CSV csv, string languageName = "")
	{
		try
		{
			if (csv.RowCount < 4 || csv.ColumnCount != 4 || csv[0][0] != "Key" || csv[1][0] != "Desc" || csv[2][0] != "Default" || (languageName == "" && !LocalizationManager.HasLanguage(csv[3][0], true, true, true)) || (languageName != "" && csv[3][0] != languageName) || csv.RowFromKey("_Title") == -1 || csv.RowFromKey("_Author") == -1)
			{
				return LibString.Error("Invalid translation .csv");
			}
		}
		catch (Exception)
		{
			return LibString.Error("Invalid translation .csv");
		}
		return LibString.OK();
	}

	// Token: 0x06001069 RID: 4201 RVA: 0x00070264 File Offset: 0x0006E464
	public void RefreshCurrentLanguage()
	{
		LocalizationManager.InitializeIfNeeded();
		string supportedLanguage = LocalizationManager.GetSupportedLanguage(LocalizationManager.CurrentLanguage, false);
		if (!string.IsNullOrEmpty(supportedLanguage))
		{
			LocalizationManager.SetLanguageAndCode(supportedLanguage, LocalizationManager.GetLanguageCode(supportedLanguage), true, true);
		}
	}

	// Token: 0x0600106A RID: 4202 RVA: 0x00070298 File Offset: 0x0006E498
	protected override void Awake()
	{
		if (this.hasBeenInit)
		{
			return;
		}
		this.hasBeenInit = true;
		base.Awake();
		for (int i = 0; i < Language.OrderedLanguageCodes.Count; i++)
		{
			string text = Language.OrderedLanguageCodes[i];
			this.TranslationsFromCode[text] = new List<Translation>
			{
				new Translation(text, Language.DefaultLiteralFromCode[text], (i == 0) ? "Berserk Games" : "Google Translate", "", false)
			};
		}
		this.CheckForTranslationFiles();
	}

	// Token: 0x0600106B RID: 4203 RVA: 0x00070320 File Offset: 0x0006E520
	private void Start()
	{
		string @string = PlayerPrefs.GetString("language_code", "en");
		string string2 = PlayerPrefs.GetString("translation_for_" + @string.Replace("-", "_"));
		this.SetLanguageAndTranslation(@string, string2);
	}

	// Token: 0x0600106C RID: 4204 RVA: 0x00070368 File Offset: 0x0006E568
	public void CheckForTranslationFiles()
	{
		foreach (string path in SerializationScript.GetTranslationFiles(DirectoryScript.translationsPath, false, null))
		{
			this.registerIfTranslation(path, false);
		}
		foreach (string path2 in SerializationScript.GetTranslationFiles(DirectoryScript.workshopFilePath, false, null))
		{
			this.registerIfTranslation(path2, true);
		}
	}

	// Token: 0x0600106D RID: 4205 RVA: 0x0007040C File Offset: 0x0006E60C
	private void registerIfTranslation(string path, bool fromWorkshop)
	{
		CSV csv;
		if (Language.TryReadValidCSVFile("", path, out csv))
		{
			this.RegisterTranslation(Language.CodeFromLanguage(csv[3][0]), csv[1][csv.RowFromKey("_Title")], csv[1][csv.RowFromKey("_Author")], path, fromWorkshop);
		}
	}

	// Token: 0x0600106E RID: 4206 RVA: 0x00070478 File Offset: 0x0006E678
	public void RegisterTranslation(string code, string title, string author, string filename, bool fromWorkshop)
	{
		List<Translation> list;
		if (this.TranslationsFromCode.TryGetValue(code, out list))
		{
			for (int i = 1; i < list.Count; i++)
			{
				if (list[i].author == author && list[i].title == title && list[i].fromWorkshop == fromWorkshop)
				{
					list[i].filename = filename;
					Singleton<UILanguageSettings>.Instance.RefreshTranslations(code);
					return;
				}
				int num = (list[i].fromWorkshop == fromWorkshop) ? 0 : (list[i].fromWorkshop ? 1 : -1);
				if (num == 0)
				{
					num = list[i].author.CompareTo(author);
				}
				if (num > 0 || (num == 0 && list[i].title.CompareTo(title) > 0))
				{
					list.Insert(i, new Translation(code, title, author, filename, fromWorkshop));
					Singleton<UILanguageSettings>.Instance.RefreshTranslations(code);
					return;
				}
			}
			list.Add(new Translation(code, title, author, filename, fromWorkshop));
			Singleton<UILanguageSettings>.Instance.RefreshTranslations(code);
			return;
		}
		Chat.LogError("Cannot add translation file - unknown code: " + code, true);
	}

	// Token: 0x0600106F RID: 4207 RVA: 0x000705A8 File Offset: 0x0006E7A8
	public Translation TranslationFromCodeAndFilename(string code, string filename)
	{
		for (int i = 0; i < this.TranslationsFromCode[code].Count; i++)
		{
			Translation translation = this.TranslationsFromCode[code][i];
			if (translation.filename == filename)
			{
				return translation;
			}
		}
		return null;
	}

	// Token: 0x06001070 RID: 4208 RVA: 0x000705F8 File Offset: 0x0006E7F8
	public static void UpdateUILabel(UILabel uiLabel, string newPrimaryTerm)
	{
		Localize labelLocalize = Language.GetLabelLocalize(uiLabel);
		if (labelLocalize == null)
		{
			uiLabel.text = newPrimaryTerm;
			return;
		}
		labelLocalize.SetTerm(newPrimaryTerm);
		uiLabel.text = Language.Translate(newPrimaryTerm);
	}

	// Token: 0x06001071 RID: 4209 RVA: 0x00070630 File Offset: 0x0006E830
	public static void UpdateUITooltip(UITooltipScript uiTooltip, string newPrimaryTerm, string newSecondaryTerm = null)
	{
		Localize tooltipLocalize = Language.GetTooltipLocalize(uiTooltip);
		if (tooltipLocalize == null)
		{
			uiTooltip.Tooltip = newPrimaryTerm;
			return;
		}
		if (newSecondaryTerm == null)
		{
			tooltipLocalize.SetTerm(newPrimaryTerm);
			uiTooltip.Tooltip = Language.Translate(newPrimaryTerm);
			return;
		}
		tooltipLocalize.SetTerm(newPrimaryTerm, newSecondaryTerm);
		uiTooltip.Tooltip = Language.Translate(newPrimaryTerm);
		uiTooltip.DelayTooltip = Language.Translate(newSecondaryTerm);
	}

	// Token: 0x06001072 RID: 4210 RVA: 0x0007068C File Offset: 0x0006E88C
	public static Localize GetLabelLocalize(MonoBehaviour component)
	{
		Localize[] components = component.GetComponents<Localize>();
		for (int i = 0; i < components.Length; i++)
		{
			if (components[i].mLocalizeTarget.GetType() == typeof(LocalizeTarget_NGUI_Label))
			{
				return components[i];
			}
		}
		return null;
	}

	// Token: 0x06001073 RID: 4211 RVA: 0x000706D4 File Offset: 0x0006E8D4
	public static Localize GetTooltipLocalize(MonoBehaviour component)
	{
		Localize[] components = component.GetComponents<Localize>();
		for (int i = 0; i < components.Length; i++)
		{
			if (components[i].mLocalizeTarget.GetType() == typeof(LocalizeTarget_Tooltip))
			{
				return components[i];
			}
		}
		return null;
	}

	// Token: 0x04000A59 RID: 2649
	public static Dictionary<string, string> LanguageFromCode = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
	{
		{
			"en",
			"English"
		},
		{
			"zh-cn",
			"Chinese (Simplified)"
		},
		{
			"ru",
			"Russian"
		},
		{
			"es",
			"Spanish"
		},
		{
			"pt-br",
			"Portuguese (Brazil)"
		},
		{
			"de",
			"German"
		},
		{
			"fr",
			"French"
		},
		{
			"ko",
			"Korean"
		},
		{
			"pl",
			"Polish"
		},
		{
			"tr",
			"Turkish"
		},
		{
			"ja",
			"Japanese"
		},
		{
			"zh-tw",
			"Chinese (Traditional)"
		},
		{
			"th",
			"Thai"
		},
		{
			"it",
			"Italian"
		},
		{
			"pt",
			"Portuguese"
		},
		{
			"cs",
			"Czech"
		},
		{
			"hu",
			"Hungarian"
		},
		{
			"sv",
			"Swedish"
		},
		{
			"nl",
			"Dutch"
		},
		{
			"es-419",
			"Spanish (Latin America)"
		},
		{
			"da",
			"Danish"
		},
		{
			"fi",
			"Finnish"
		},
		{
			"nb",
			"Norwegian"
		},
		{
			"ro",
			"Romanian"
		},
		{
			"uk",
			"Ukrainian"
		},
		{
			"el",
			"Greek"
		},
		{
			"vi",
			"Vietnamese"
		},
		{
			"bg",
			"Bulgarian"
		},
		{
			"arb",
			"Arabic"
		}
	};

	// Token: 0x04000A5A RID: 2650
	public static List<string> OrderedLanguageCodes = new List<string>
	{
		"en",
		"zh-cn",
		"ru",
		"es",
		"arb",
		"bg",
		"cs",
		"da",
		"de",
		"el",
		"es-419",
		"fi",
		"fr",
		"hu",
		"it",
		"ja",
		"ko",
		"nb",
		"nl",
		"pl",
		"pt",
		"pt-br",
		"ro",
		"sv",
		"th",
		"tr",
		"uk",
		"vi",
		"zh-tw"
	};

	// Token: 0x04000A5B RID: 2651
	public static Dictionary<string, string> NativeLanguageFromCode = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
	{
		{
			"en",
			"English"
		},
		{
			"zh-cn",
			"简体字"
		},
		{
			"ru",
			"русский"
		},
		{
			"es",
			"Español"
		},
		{
			"pt-br",
			"Português (Brasil)"
		},
		{
			"de",
			"Deutsche"
		},
		{
			"fr",
			"français"
		},
		{
			"ko",
			"한국어"
		},
		{
			"pl",
			"Polskie"
		},
		{
			"tr",
			"Türk"
		},
		{
			"ja",
			"日本人"
		},
		{
			"zh-tw",
			"正體字"
		},
		{
			"th",
			"ไทย"
		},
		{
			"it",
			"italiano"
		},
		{
			"pt",
			"Português"
		},
		{
			"cs",
			"čeština"
		},
		{
			"hu",
			"Magyar"
		},
		{
			"sv",
			"svenska"
		},
		{
			"nl",
			"Nederlands"
		},
		{
			"es-419",
			"Español (Latinoamérica)"
		},
		{
			"da",
			"dansk"
		},
		{
			"fi",
			"Suomalainen"
		},
		{
			"nb",
			"norsk"
		},
		{
			"ro",
			"Română"
		},
		{
			"uk",
			"український"
		},
		{
			"el",
			"Ελληνικά"
		},
		{
			"vi",
			"Tiếng Việt"
		},
		{
			"bg",
			"български"
		},
		{
			"arb",
			"عربى"
		}
	};

	// Token: 0x04000A5C RID: 2652
	public static Dictionary<string, string> DefaultLiteralFromCode = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
	{
		{
			"en",
			"Default"
		},
		{
			"zh-cn",
			"默认"
		},
		{
			"ru",
			"По умолчанию"
		},
		{
			"es",
			"Defecto"
		},
		{
			"pt-br",
			"Padrão"
		},
		{
			"de",
			"Standard"
		},
		{
			"fr",
			"Défaut"
		},
		{
			"ko",
			"기본"
		},
		{
			"pl",
			"Domyślna"
		},
		{
			"tr",
			"Varsayılan"
		},
		{
			"ja",
			"デフォルト"
		},
		{
			"zh-tw",
			"默認"
		},
		{
			"th",
			"ค่าเริ่มต้น"
		},
		{
			"it",
			"Predefinito"
		},
		{
			"pt",
			"Padrão"
		},
		{
			"cs",
			"standardní"
		},
		{
			"hu",
			"Alapértelmezett"
		},
		{
			"sv",
			"Standard"
		},
		{
			"nl",
			"Standaard"
		},
		{
			"es-419",
			"Defecto"
		},
		{
			"da",
			"Standard"
		},
		{
			"fi",
			"oletusarvo"
		},
		{
			"nb",
			"Misligholde"
		},
		{
			"ro",
			"Mod implicit"
		},
		{
			"uk",
			"дефолт"
		},
		{
			"el",
			"Προκαθορισμένο"
		},
		{
			"vi",
			"Mặc định"
		},
		{
			"bg",
			"По подразбиране"
		},
		{
			"arb",
			"إفتراضي"
		}
	};

	// Token: 0x04000A5D RID: 2653
	public static Dictionary<string, string> CSVFilenameFromCode = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
	{
		{
			"en",
			""
		},
		{
			"zh-cn",
			""
		},
		{
			"ru",
			""
		},
		{
			"es",
			""
		},
		{
			"pt-br",
			""
		},
		{
			"de",
			""
		},
		{
			"fr",
			""
		},
		{
			"ko",
			""
		},
		{
			"pl",
			""
		},
		{
			"tr",
			""
		},
		{
			"ja",
			""
		},
		{
			"zh-tw",
			""
		},
		{
			"th",
			""
		},
		{
			"it",
			""
		},
		{
			"pt",
			""
		},
		{
			"cs",
			""
		},
		{
			"hu",
			""
		},
		{
			"sv",
			""
		},
		{
			"nl",
			""
		},
		{
			"es-419",
			""
		},
		{
			"da",
			""
		},
		{
			"fi",
			""
		},
		{
			"nb",
			""
		},
		{
			"ro",
			""
		},
		{
			"uk",
			""
		},
		{
			"el",
			""
		},
		{
			"vi",
			""
		},
		{
			"bg",
			""
		},
		{
			"arb",
			""
		}
	};

	// Token: 0x04000A5E RID: 2654
	public Dictionary<string, List<Translation>> TranslationsFromCode = new Dictionary<string, List<Translation>>(StringComparer.OrdinalIgnoreCase);

	// Token: 0x04000A5F RID: 2655
	private bool hasBeenInit;
}
