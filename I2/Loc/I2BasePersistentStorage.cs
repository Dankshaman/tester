using System;
using System.IO;
using System.Text;
using UnityEngine;

namespace I2.Loc
{
	// Token: 0x0200046D RID: 1133
	public abstract class I2BasePersistentStorage
	{
		// Token: 0x06003335 RID: 13109 RVA: 0x00155950 File Offset: 0x00153B50
		public virtual void SetSetting_String(string key, string value)
		{
			try
			{
				int length = value.Length;
				int num = 8000;
				if (length <= num)
				{
					PlayerPrefs.SetString(key, value);
				}
				else
				{
					int num2 = Mathf.CeilToInt((float)length / (float)num);
					for (int i = 0; i < num2; i++)
					{
						int num3 = num * i;
						PlayerPrefs.SetString(string.Format("[I2split]{0}{1}", i, key), value.Substring(num3, Mathf.Min(num, length - num3)));
					}
					PlayerPrefs.SetString(key, "[$I2#@div$]" + num2);
				}
			}
			catch (Exception)
			{
				Debug.LogError("Error saving PlayerPrefs " + key);
			}
		}

		// Token: 0x06003336 RID: 13110 RVA: 0x001559F8 File Offset: 0x00153BF8
		public virtual string GetSetting_String(string key, string defaultValue)
		{
			string result;
			try
			{
				string text = PlayerPrefs.GetString(key, defaultValue);
				if (!string.IsNullOrEmpty(text) && text.StartsWith("[I2split]"))
				{
					int num = int.Parse(text.Substring("[I2split]".Length));
					text = "";
					for (int i = 0; i < num; i++)
					{
						text += PlayerPrefs.GetString(string.Format("[I2split]{0}{1}", i, key), "");
					}
				}
				result = text;
			}
			catch (Exception)
			{
				Debug.LogError("Error loading PlayerPrefs " + key);
				result = defaultValue;
			}
			return result;
		}

		// Token: 0x06003337 RID: 13111 RVA: 0x00155A98 File Offset: 0x00153C98
		public virtual void DeleteSetting(string key)
		{
			try
			{
				string @string = PlayerPrefs.GetString(key, null);
				if (!string.IsNullOrEmpty(@string) && @string.StartsWith("[I2split]"))
				{
					int num = int.Parse(@string.Substring("[I2split]".Length));
					for (int i = 0; i < num; i++)
					{
						PlayerPrefs.DeleteKey(string.Format("[I2split]{0}{1}", i, key));
					}
				}
				PlayerPrefs.DeleteKey(key);
			}
			catch (Exception)
			{
				Debug.LogError("Error deleting PlayerPrefs " + key);
			}
		}

		// Token: 0x06003338 RID: 13112 RVA: 0x00155B28 File Offset: 0x00153D28
		public virtual void ForceSaveSettings()
		{
			PlayerPrefs.Save();
		}

		// Token: 0x06003339 RID: 13113 RVA: 0x00155B2F File Offset: 0x00153D2F
		public virtual bool HasSetting(string key)
		{
			return PlayerPrefs.HasKey(key);
		}

		// Token: 0x0600333A RID: 13114 RVA: 0x00014D66 File Offset: 0x00012F66
		public virtual bool CanAccessFiles()
		{
			return true;
		}

		// Token: 0x0600333B RID: 13115 RVA: 0x00155B38 File Offset: 0x00153D38
		private string UpdateFilename(PersistentStorage.eFileType fileType, string fileName)
		{
			switch (fileType)
			{
			case PersistentStorage.eFileType.Persistent:
				fileName = Application.persistentDataPath + "/" + fileName;
				break;
			case PersistentStorage.eFileType.Temporal:
				fileName = Application.temporaryCachePath + "/" + fileName;
				break;
			case PersistentStorage.eFileType.Streaming:
				fileName = Application.streamingAssetsPath + "/" + fileName;
				break;
			}
			return fileName;
		}

		// Token: 0x0600333C RID: 13116 RVA: 0x00155B98 File Offset: 0x00153D98
		public virtual bool SaveFile(PersistentStorage.eFileType fileType, string fileName, string data, bool logExceptions = true)
		{
			if (!this.CanAccessFiles())
			{
				return false;
			}
			bool result;
			try
			{
				fileName = this.UpdateFilename(fileType, fileName);
				File.WriteAllText(fileName, data, Encoding.UTF8);
				result = true;
			}
			catch (Exception ex)
			{
				if (logExceptions)
				{
					Debug.LogError(string.Concat(new object[]
					{
						"Error saving file '",
						fileName,
						"'\n",
						ex
					}));
				}
				result = false;
			}
			return result;
		}

		// Token: 0x0600333D RID: 13117 RVA: 0x00155C0C File Offset: 0x00153E0C
		public virtual string LoadFile(PersistentStorage.eFileType fileType, string fileName, bool logExceptions = true)
		{
			if (!this.CanAccessFiles())
			{
				return null;
			}
			string result;
			try
			{
				fileName = this.UpdateFilename(fileType, fileName);
				result = File.ReadAllText(fileName, Encoding.UTF8);
			}
			catch (Exception ex)
			{
				if (logExceptions)
				{
					Debug.LogError(string.Concat(new object[]
					{
						"Error loading file '",
						fileName,
						"'\n",
						ex
					}));
				}
				result = null;
			}
			return result;
		}

		// Token: 0x0600333E RID: 13118 RVA: 0x00155C80 File Offset: 0x00153E80
		public virtual bool DeleteFile(PersistentStorage.eFileType fileType, string fileName, bool logExceptions = true)
		{
			if (!this.CanAccessFiles())
			{
				return false;
			}
			bool result;
			try
			{
				fileName = this.UpdateFilename(fileType, fileName);
				File.Delete(fileName);
				result = true;
			}
			catch (Exception ex)
			{
				if (logExceptions)
				{
					Debug.LogError(string.Concat(new object[]
					{
						"Error deleting file '",
						fileName,
						"'\n",
						ex
					}));
				}
				result = false;
			}
			return result;
		}

		// Token: 0x0600333F RID: 13119 RVA: 0x00155CF0 File Offset: 0x00153EF0
		public virtual bool HasFile(PersistentStorage.eFileType fileType, string fileName, bool logExceptions = true)
		{
			if (!this.CanAccessFiles())
			{
				return false;
			}
			bool result;
			try
			{
				fileName = this.UpdateFilename(fileType, fileName);
				result = File.Exists(fileName);
			}
			catch (Exception ex)
			{
				if (logExceptions)
				{
					Debug.LogError(string.Concat(new object[]
					{
						"Error requesting file '",
						fileName,
						"'\n",
						ex
					}));
				}
				result = false;
			}
			return result;
		}
	}
}
