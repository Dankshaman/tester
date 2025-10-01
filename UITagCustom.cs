using System;
using Assets.Scripts.UI;
using UnityEngine;

// Token: 0x02000348 RID: 840
public class UITagCustom : MonoBehaviour
{
	// Token: 0x060027DA RID: 10202 RVA: 0x0011A2D6 File Offset: 0x001184D6
	public void Start()
	{
		this.TagContainer.TagsRepositioned += delegate(object _, EventArgs __)
		{
			this.RepositionTagContainer();
		};
	}

	// Token: 0x060027DB RID: 10203 RVA: 0x0011A2F0 File Offset: 0x001184F0
	public void OnAddClick()
	{
		string text = this.CustomInput.value.Replace("\r\n", "").Replace("\n", "").Replace("\r", "");
		if (this.TagContainer.ActiveTags.Contains(text))
		{
			return;
		}
		this.TagContainer.AddTag(text, true, true);
		this.CustomInput.value = "";
	}

	// Token: 0x060027DC RID: 10204 RVA: 0x0011A368 File Offset: 0x00118568
	private void RepositionTagContainer()
	{
		Vector3 localPosition = this.TagContainer.transform.localPosition;
		int height = this.TagContainer.GetComponent<UISprite>().height;
		this.TagContainer.transform.localPosition = new Vector3(localPosition.x, (float)(100 - (height - 80) / 2), localPosition.z);
	}

	// Token: 0x04001A2D RID: 6701
	public UIInput CustomInput;

	// Token: 0x04001A2E RID: 6702
	public UITagContainer TagContainer;
}
