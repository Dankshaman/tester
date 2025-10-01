using System;
using System.Collections.Generic;
using System.IO;
using Crosstales.Common.Util;
using Crosstales.FB;
using SimpleFileBrowser;
using UnityEngine;

// Token: 0x020002C8 RID: 712
public class UIFileBrowser : MonoBehaviour
{
	// Token: 0x060022F9 RID: 8953 RVA: 0x000F95D4 File Offset: 0x000F77D4
	private void Start()
	{
		if (this.input == null)
		{
			this.input = base.transform.parent.gameObject.GetComponent<UIInput>();
		}
		this.multipleFiles = (this.input == null);
		this.LoadingWheel = base.transform.Find("Loading Wheel").gameObject;
		EventManager.OnCloudUploadFinish += this.CloudUploadFinish;
	}

	// Token: 0x060022FA RID: 8954 RVA: 0x000F9648 File Offset: 0x000F7848
	private void OnDestroy()
	{
		EventManager.OnCloudUploadFinish -= this.CloudUploadFinish;
	}

	// Token: 0x060022FB RID: 8955 RVA: 0x000F965C File Offset: 0x000F785C
	private void CloudUploadFinish(string cloudName, string url)
	{
		if (this.queuedCloudNames.Remove(cloudName))
		{
			if (this.input)
			{
				this.input.value = url;
			}
			if (this.queuedCloudNames.Count == 0)
			{
				this.LoadingWheel.SetActive(false);
			}
		}
	}

	// Token: 0x060022FC RID: 8956 RVA: 0x000F96AC File Offset: 0x000F78AC
	private void OnClick()
	{
		if (this.LoadingWheel.activeSelf)
		{
			return;
		}
		string[] array = null;
		switch (this.fileType)
		{
		case FileType.All:
			array = CustomLoadingManager.SupportedAllFormatExtensions.ToArray();
			break;
		case FileType.Image:
			array = CustomLoadingManager.SupportedImageFormatExtensions.ToArray();
			break;
		case FileType.Model:
			array = CustomLoadingManager.SupportedMeshFormatExtensions.ToArray();
			break;
		case FileType.AssetBundle:
			array = CustomLoadingManager.SupportedAssetBundleFormatExtensions.ToArray();
			break;
		case FileType.Audio:
			array = CustomLoadingManager.SupportedAudioFormatExtensions.ToArray();
			break;
		case FileType.PDF:
			array = CustomLoadingManager.SupportedPDFFormatExtensions.ToArray();
			break;
		}
		string text = UIFileBrowser.GetLastPath();
		if (string.IsNullOrEmpty(text))
		{
			text = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
		}
		if (VRHMD.isVR || !UIFileBrowser.UseNativeFileBrowser)
		{
			if (array != null)
			{
				List<SimpleFileBrowser.FileBrowser.Filter> list = new List<SimpleFileBrowser.FileBrowser.Filter>
				{
					new SimpleFileBrowser.FileBrowser.Filter(null, array)
				};
				if (array.Length > 1)
				{
					for (int i = 0; i < array.Length; i++)
					{
						list.Add(new SimpleFileBrowser.FileBrowser.Filter(null, array[i]));
					}
				}
				SimpleFileBrowser.FileBrowser.SetFilters(false, list);
			}
			SimpleFileBrowser.FileBrowser.ShowLoadDialog(new SimpleFileBrowser.FileBrowser.OnSuccess(this.Output), new SimpleFileBrowser.FileBrowser.OnCancel(this.CloseWindow), false, text, Language.Translate("Open"), Language.Translate("Select"), Language.Translate("Cancel"));
			return;
		}
		ExtensionFilter[] array2 = null;
		if (array != null)
		{
			for (int j = 0; j < array.Length; j++)
			{
				array[j] = array[j].Substring(1, array[j].Length - 1);
			}
			array2 = new ExtensionFilter[(array.Length > 1) ? (array.Length + 1) : 1];
			array2[0] = new ExtensionFilter
			{
				Name = this.fileType.ToString(),
				Extensions = array
			};
			if (array.Length > 1)
			{
				for (int k = 0; k < array.Length; k++)
				{
					string text2 = array[k];
					array2[k + 1] = new ExtensionFilter
					{
						Name = text2,
						Extensions = new string[]
						{
							text2
						}
					};
				}
			}
		}
		Crosstales.Common.Util.Singleton<Crosstales.FB.FileBrowser>.Instance.OpenFilesAsync(new Action<string[]>(this.Output), Language.Translate("Open"), text, string.Empty, this.multipleFiles, array2);
	}

	// Token: 0x060022FD RID: 8957 RVA: 0x000F98E0 File Offset: 0x000F7AE0
	private async void Output(string[] paths)
	{
		await new WaitForUpdate();
		if (paths == null || paths.Length == 0)
		{
			this.CloseWindow();
		}
		else if (paths.Length == 1)
		{
			this.Output(paths[0]);
		}
		else
		{
			UIFileBrowser.SetLastPath(Path.GetDirectoryName(paths[0]));
			this.UploadToCloud(paths);
		}
	}

	// Token: 0x060022FE RID: 8958 RVA: 0x000F9924 File Offset: 0x000F7B24
	private void Output(string path)
	{
		UIFileBrowser.SetLastPath(Path.GetDirectoryName(path));
		if (this.multipleFiles)
		{
			this.UploadToCloud(path);
			return;
		}
		switch (this.dialog)
		{
		case UIFileBrowser.Dialog.Ask:
			UIDialog.Show("Upload file to the Steam Cloud or load from your local disk. (Note: Local files will not work in multiplayer)", "Cloud", "Local", delegate()
			{
				this.UploadToCloud(path);
			}, delegate()
			{
				this.CloudLocal(path);
			});
			return;
		case UIFileBrowser.Dialog.Cloud:
			this.UploadToCloud(path);
			return;
		case UIFileBrowser.Dialog.Local:
			this.CloudLocal(path);
			return;
		default:
			return;
		}
	}

	// Token: 0x060022FF RID: 8959 RVA: 0x000F99CC File Offset: 0x000F7BCC
	private void CloudLocal(string path)
	{
		string directoryName = Path.GetDirectoryName(path);
		if (string.Compare(Path.GetFullPath(directoryName).TrimEnd(new char[]
		{
			'\\'
		}), Path.GetFullPath(DirectoryScript.imageFilePath).TrimEnd(new char[]
		{
			'\\'
		}), StringComparison.InvariantCultureIgnoreCase) == 0 || string.Compare(Path.GetFullPath(directoryName).TrimEnd(new char[]
		{
			'\\'
		}), Path.GetFullPath(DirectoryScript.modelFilePath).TrimEnd(new char[]
		{
			'\\'
		}), StringComparison.InvariantCultureIgnoreCase) == 0 || string.Compare(Path.GetFullPath(directoryName).TrimEnd(new char[]
		{
			'\\'
		}), Path.GetFullPath(DirectoryScript.assetbundleFilePath).TrimEnd(new char[]
		{
			'\\'
		}), StringComparison.InvariantCultureIgnoreCase) == 0)
		{
			this.input.value = Path.GetFileName(path);
			return;
		}
		this.input.value = "file:///" + path;
	}

	// Token: 0x06002300 RID: 8960 RVA: 0x000F9AB0 File Offset: 0x000F7CB0
	private void UploadToCloud(string path)
	{
		Action<string, string> <>9__1;
		Wait.Frames(delegate
		{
			UIGridMenuCloud component = NetworkSingleton<NetworkUI>.Instance.GUICloud.GetComponent<UIGridMenuCloud>();
			string description = "Upload File";
			string leftButtonText = "Upload";
			string rightButtonText = "Cancel";
			List<string> folders = component.GetFolders();
			Action<string, string> leftButtonFunc;
			if ((leftButtonFunc = <>9__1) == null)
			{
				leftButtonFunc = (<>9__1 = delegate(string LocalFolder, string Name)
				{
					if (LocalFolder == "<Root Folder>")
					{
						LocalFolder = "";
					}
					string text = Name + Path.GetExtension(path);
					this.queuedCloudNames.Add(text);
					this.LoadingWheel.SetActive(true);
					global::Singleton<SteamManager>.Instance.UploadToCloud(text, File.ReadAllBytes(path), LocalFolder);
				});
			}
			UIDialog.ShowDropDownInput(description, leftButtonText, rightButtonText, folders, leftButtonFunc, null, Path.GetFileNameWithoutExtension(path), "Name", component.GetCurrentFolder());
		}, 1);
	}

	// Token: 0x06002301 RID: 8961 RVA: 0x000F9AD7 File Offset: 0x000F7CD7
	private void UploadToCloud(string[] paths)
	{
		Action<string> <>9__1;
		Wait.Frames(delegate
		{
			UIGridMenuCloud component = NetworkSingleton<NetworkUI>.Instance.GUICloud.GetComponent<UIGridMenuCloud>();
			string description = "Upload File";
			string leftButtonText = "Upload";
			string rightButtonText = "Cancel";
			List<string> folders = component.GetFolders();
			Action<string> leftButtonFunc;
			if ((leftButtonFunc = <>9__1) == null)
			{
				leftButtonFunc = (<>9__1 = delegate(string LocalFolder)
				{
					if (LocalFolder == "<Root Folder>")
					{
						LocalFolder = "";
					}
					foreach (string path in paths)
					{
						string fileName = Path.GetFileName(path);
						this.queuedCloudNames.Add(fileName);
						this.LoadingWheel.SetActive(true);
						global::Singleton<SteamManager>.Instance.UploadToCloud(fileName, File.ReadAllBytes(path), LocalFolder);
					}
				});
			}
			UIDialog.ShowDropDown(description, leftButtonText, rightButtonText, folders, leftButtonFunc, null, component.GetCurrentFolder());
		}, 1);
	}

	// Token: 0x06002302 RID: 8962 RVA: 0x000F9AFE File Offset: 0x000F7CFE
	public static string NormalizePath(string path)
	{
		return Path.GetFullPath(new Uri(path).LocalPath).TrimEnd(new char[]
		{
			Path.DirectorySeparatorChar,
			Path.AltDirectorySeparatorChar
		}).ToUpperInvariant();
	}

	// Token: 0x06002303 RID: 8963 RVA: 0x000025B8 File Offset: 0x000007B8
	private void CloseWindow()
	{
	}

	// Token: 0x06002304 RID: 8964 RVA: 0x000F9B30 File Offset: 0x000F7D30
	public static void SetLastPath(string path)
	{
		PlayerPrefs.SetString("FileBrowserLastPath", path);
	}

	// Token: 0x06002305 RID: 8965 RVA: 0x000F9B3D File Offset: 0x000F7D3D
	public static string GetLastPath()
	{
		return PlayerPrefs.GetString("FileBrowserLastPath");
	}

	// Token: 0x04001634 RID: 5684
	public UIFileBrowser.Dialog dialog;

	// Token: 0x04001635 RID: 5685
	public FileType fileType;

	// Token: 0x04001636 RID: 5686
	public UIInput input;

	// Token: 0x04001637 RID: 5687
	private bool multipleFiles;

	// Token: 0x04001638 RID: 5688
	private GameObject LoadingWheel;

	// Token: 0x04001639 RID: 5689
	private readonly List<string> queuedCloudNames = new List<string>();

	// Token: 0x0400163A RID: 5690
	public static bool UseNativeFileBrowser = true;

	// Token: 0x0400163B RID: 5691
	private const string PlayerPrefsLastPath = "FileBrowserLastPath";

	// Token: 0x0200071A RID: 1818
	public enum Dialog
	{
		// Token: 0x04002AA9 RID: 10921
		Ask,
		// Token: 0x04002AAA RID: 10922
		Cloud,
		// Token: 0x04002AAB RID: 10923
		Local
	}
}
