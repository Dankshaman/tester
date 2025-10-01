using System;
using UnityEngine;

namespace I2.Loc
{
	// Token: 0x02000464 RID: 1124
	public class CallbackNotification : MonoBehaviour
	{
		// Token: 0x0600330C RID: 13068 RVA: 0x001551E8 File Offset: 0x001533E8
		public void OnModifyLocalization()
		{
			if (string.IsNullOrEmpty(Localize.MainTranslation))
			{
				return;
			}
			string translation = LocalizationManager.GetTranslation("Color/Red", true, 0, true, false, null, null);
			Localize.MainTranslation = Localize.MainTranslation.Replace("{PLAYER_COLOR}", translation);
		}
	}
}
