using System;
using UnityEngine;

// Token: 0x02000326 RID: 806
public class UIRandomizeZone : MonoBehaviour
{
	// Token: 0x060026A9 RID: 9897 RVA: 0x0011358D File Offset: 0x0011178D
	public void OnEditTags()
	{
		Singleton<UIComponentTagDialog>.Instance.EditRandomizeZoneTags();
		NetworkSingleton<NetworkUI>.Instance.GUIRandomizeZone.SetActive(false);
	}

	// Token: 0x060026AA RID: 9898 RVA: 0x001135A9 File Offset: 0x001117A9
	public void OnRandomize()
	{
		PlayerScript.PointerScript.InfoRandomizeZoneGO.GetComponent<RandomizeZone>().Randomize();
		NetworkSingleton<NetworkUI>.Instance.GUIRandomizeZone.SetActive(false);
	}
}
