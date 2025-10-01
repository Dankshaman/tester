using System;
using System.Collections.Generic;
using System.IO;
using NewNet;
using UnityEngine;

// Token: 0x020002A0 RID: 672
public class UIConfirmSaveObject : MonoBehaviour
{
	// Token: 0x060021F1 RID: 8689 RVA: 0x000F4A08 File Offset: 0x000F2C08
	private void OnClick()
	{
		this.TargetThumbnail = PlayerScript.PointerScript.InfoObject;
		this.SS = new SaveState();
		this.SS.ObjectStates = new List<ObjectState>();
		if (PlayerScript.PointerScript.InfoObject && PlayerScript.PointerScript.InfoObject.GetComponent<NetworkPhysicsObject>().Name != "")
		{
			this.startName = NGUIText.StripSymbols(PlayerScript.PointerScript.InfoObject.GetComponent<NetworkPhysicsObject>().Name);
		}
		if (Network.isServer)
		{
			this.SS.ObjectStates = PlayerScript.PointerScript.GetObjectStates(-1, true, true, false, true);
			if (this.SS.ObjectStates.Count == 0)
			{
				return;
			}
		}
		else if (!PlayerScript.PointerScript.InfoObject)
		{
			return;
		}
		if (!this.TargetThumbnail.GetComponent<NetworkPhysicsObject>().CanSeeName)
		{
			this.startName = "";
		}
		UIDialog.ShowDropDownInput("Save Object(s)", "Save", "Cancel", SerializationScript.GetLocalFolders(DirectoryScript.savedObjectsFilePath, true, ""), new Action<string, string>(this.Confirm), null, this.startName, "Name", "");
	}

	// Token: 0x060021F2 RID: 8690 RVA: 0x000F4B34 File Offset: 0x000F2D34
	private void Confirm(string folderName, string saveObjectName)
	{
		string text = SerializationScript.RemoveInvalidCharsFromFileName(saveObjectName);
		if (string.IsNullOrEmpty(text))
		{
			Chat.LogError("Error: You must name your saved object(s).", true);
			return;
		}
		if (folderName != "<Root Folder>")
		{
			text = folderName + "//" + text;
		}
		this.SaveChestName = text;
		if (File.Exists(DirectoryScript.savedObjectsFilePath + "//" + this.SaveChestName + ".json"))
		{
			UIDialog.Show(Language.Translate("{0} already exists. Do you want to overwrite this file?", this.SaveChestName), "Overwrite", "Cancel", new Action(this.Save), null);
			return;
		}
		this.Save();
	}

	// Token: 0x060021F3 RID: 8691 RVA: 0x000F4BD4 File Offset: 0x000F2DD4
	private void Save()
	{
		if (Network.isServer)
		{
			SerializationScript.Save(this.SS, this.SaveChestName, this.TargetThumbnail);
		}
		else
		{
			NetworkSingleton<ManagerPhysicsObject>.Instance.RequestChestSaveState(this.TargetThumbnail.GetComponent<NetworkPhysicsObject>().ID, this.SaveChestName);
		}
		Stats.INT_SAVE_ITEMS_TO_CHEST++;
	}

	// Token: 0x0400155E RID: 5470
	private SaveState SS;

	// Token: 0x0400155F RID: 5471
	private string startName = "";

	// Token: 0x04001560 RID: 5472
	private GameObject TargetThumbnail;

	// Token: 0x04001561 RID: 5473
	private string SaveChestName;
}
