using System;

namespace I2.Loc
{
	// Token: 0x0200046C RID: 1132
	public static class PersistentStorage
	{
		// Token: 0x0600332B RID: 13099 RVA: 0x0015581B File Offset: 0x00153A1B
		public static void SetSetting_String(string key, string value)
		{
			if (PersistentStorage.mStorage == null)
			{
				PersistentStorage.mStorage = new I2CustomPersistentStorage();
			}
			PersistentStorage.mStorage.SetSetting_String(key, value);
		}

		// Token: 0x0600332C RID: 13100 RVA: 0x0015583A File Offset: 0x00153A3A
		public static string GetSetting_String(string key, string defaultValue)
		{
			if (PersistentStorage.mStorage == null)
			{
				PersistentStorage.mStorage = new I2CustomPersistentStorage();
			}
			return PersistentStorage.mStorage.GetSetting_String(key, defaultValue);
		}

		// Token: 0x0600332D RID: 13101 RVA: 0x00155859 File Offset: 0x00153A59
		public static void DeleteSetting(string key)
		{
			if (PersistentStorage.mStorage == null)
			{
				PersistentStorage.mStorage = new I2CustomPersistentStorage();
			}
			PersistentStorage.mStorage.DeleteSetting(key);
		}

		// Token: 0x0600332E RID: 13102 RVA: 0x00155877 File Offset: 0x00153A77
		public static bool HasSetting(string key)
		{
			if (PersistentStorage.mStorage == null)
			{
				PersistentStorage.mStorage = new I2CustomPersistentStorage();
			}
			return PersistentStorage.mStorage.HasSetting(key);
		}

		// Token: 0x0600332F RID: 13103 RVA: 0x00155895 File Offset: 0x00153A95
		public static void ForceSaveSettings()
		{
			if (PersistentStorage.mStorage == null)
			{
				PersistentStorage.mStorage = new I2CustomPersistentStorage();
			}
			PersistentStorage.mStorage.ForceSaveSettings();
		}

		// Token: 0x06003330 RID: 13104 RVA: 0x001558B2 File Offset: 0x00153AB2
		public static bool CanAccessFiles()
		{
			if (PersistentStorage.mStorage == null)
			{
				PersistentStorage.mStorage = new I2CustomPersistentStorage();
			}
			return PersistentStorage.mStorage.CanAccessFiles();
		}

		// Token: 0x06003331 RID: 13105 RVA: 0x001558CF File Offset: 0x00153ACF
		public static bool SaveFile(PersistentStorage.eFileType fileType, string fileName, string data, bool logExceptions = true)
		{
			if (PersistentStorage.mStorage == null)
			{
				PersistentStorage.mStorage = new I2CustomPersistentStorage();
			}
			return PersistentStorage.mStorage.SaveFile(fileType, fileName, data, logExceptions);
		}

		// Token: 0x06003332 RID: 13106 RVA: 0x001558F0 File Offset: 0x00153AF0
		public static string LoadFile(PersistentStorage.eFileType fileType, string fileName, bool logExceptions = true)
		{
			if (PersistentStorage.mStorage == null)
			{
				PersistentStorage.mStorage = new I2CustomPersistentStorage();
			}
			return PersistentStorage.mStorage.LoadFile(fileType, fileName, logExceptions);
		}

		// Token: 0x06003333 RID: 13107 RVA: 0x00155910 File Offset: 0x00153B10
		public static bool DeleteFile(PersistentStorage.eFileType fileType, string fileName, bool logExceptions = true)
		{
			if (PersistentStorage.mStorage == null)
			{
				PersistentStorage.mStorage = new I2CustomPersistentStorage();
			}
			return PersistentStorage.mStorage.DeleteFile(fileType, fileName, logExceptions);
		}

		// Token: 0x06003334 RID: 13108 RVA: 0x00155930 File Offset: 0x00153B30
		public static bool HasFile(PersistentStorage.eFileType fileType, string fileName, bool logExceptions = true)
		{
			if (PersistentStorage.mStorage == null)
			{
				PersistentStorage.mStorage = new I2CustomPersistentStorage();
			}
			return PersistentStorage.mStorage.HasFile(fileType, fileName, logExceptions);
		}

		// Token: 0x040020CF RID: 8399
		private static I2CustomPersistentStorage mStorage;

		// Token: 0x02000812 RID: 2066
		public enum eFileType
		{
			// Token: 0x04002E28 RID: 11816
			Raw,
			// Token: 0x04002E29 RID: 11817
			Persistent,
			// Token: 0x04002E2A RID: 11818
			Temporal,
			// Token: 0x04002E2B RID: 11819
			Streaming
		}
	}
}
