using System;
using UnityEngine;

// Token: 0x0200024E RID: 590
[CreateAssetMenu(fileName = "TTSVersion", menuName = "ScriptableObjects/TTSVersion")]
public class TTSVersion : ScriptableObject
{
	// Token: 0x1700043A RID: 1082
	// (get) Token: 0x06001F33 RID: 7987 RVA: 0x000DE6E3 File Offset: 0x000DC8E3
	public string VersionNumber
	{
		get
		{
			return this._VersionNumber;
		}
	}

	// Token: 0x1700043B RID: 1083
	// (get) Token: 0x06001F34 RID: 7988 RVA: 0x000DE6EB File Offset: 0x000DC8EB
	public string VersionHotFix
	{
		get
		{
			return this._VersionHotFix;
		}
	}

	// Token: 0x0400130E RID: 4878
	[SerializeField]
	private string _VersionNumber;

	// Token: 0x0400130F RID: 4879
	[SerializeField]
	private string _VersionHotFix;
}
