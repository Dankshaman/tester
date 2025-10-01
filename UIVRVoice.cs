using System;
using UnityEngine;

// Token: 0x0200035B RID: 859
public class UIVRVoice : MonoBehaviour
{
	// Token: 0x060028B6 RID: 10422 RVA: 0x0011F874 File Offset: 0x0011DA74
	private void OnClick()
	{
		if (VoiceChat.Mode != VoiceChat.VoiceMode.None)
		{
			VoiceChat.Mode = VoiceChat.VoiceMode.Toggle;
		}
		if (this.Off.activeSelf)
		{
			this.Off.SetActive(false);
			this.On.SetActive(true);
			Singleton<VoiceChat>.Instance.Talking = VoiceTalking.Game;
			return;
		}
		this.Off.SetActive(true);
		this.On.SetActive(false);
		Singleton<VoiceChat>.Instance.Talking = VoiceTalking.Off;
	}

	// Token: 0x04001ACB RID: 6859
	public GameObject Off;

	// Token: 0x04001ACC RID: 6860
	public GameObject On;
}
