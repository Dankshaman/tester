using System;
using UnityEngine;
using UnityEngine.SceneManagement;

// Token: 0x0200001C RID: 28
[AddComponentMenu("NGUI/Examples/Load Level On Click")]
public class LoadLevelOnClick : MonoBehaviour
{
	// Token: 0x06000095 RID: 149 RVA: 0x00004A56 File Offset: 0x00002C56
	private void OnClick()
	{
		if (!string.IsNullOrEmpty(this.levelName))
		{
			SceneManager.LoadScene(this.levelName);
		}
	}

	// Token: 0x04000062 RID: 98
	public string levelName;
}
