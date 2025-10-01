using System;
using UnityEngine;

namespace I2.Loc
{
	// Token: 0x02000466 RID: 1126
	public class Example_LocalizedString : MonoBehaviour
	{
		// Token: 0x06003313 RID: 13075 RVA: 0x00155264 File Offset: 0x00153464
		public void Start()
		{
			Debug.Log(this._MyLocalizedString);
			Debug.Log(LocalizationManager.GetTranslation(this._NormalString, true, 0, true, false, null, null));
			Debug.Log(LocalizationManager.GetTranslation(this._StringWithTermPopup, true, 0, true, false, null, null));
			Debug.Log("Term2");
			Debug.Log(this._MyLocalizedString);
			Debug.Log("Term3");
			LocalizedString localizedString = "Term3";
			localizedString.mRTL_IgnoreArabicFix = true;
			Debug.Log(localizedString);
			LocalizedString localizedString2 = "Term3";
			localizedString2.mRTL_ConvertNumbers = true;
			localizedString2.mRTL_MaxLineLength = 20;
			Debug.Log(localizedString2);
			Debug.Log(localizedString2);
		}

		// Token: 0x040020C8 RID: 8392
		public LocalizedString _MyLocalizedString;

		// Token: 0x040020C9 RID: 8393
		public string _NormalString;

		// Token: 0x040020CA RID: 8394
		[TermsPopup("")]
		public string _StringWithTermPopup;
	}
}
