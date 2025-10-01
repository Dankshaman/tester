using System;
using UnityEngine;

// Token: 0x02000324 RID: 804
public class UIRPG : MonoBehaviour
{
	// Token: 0x060026A0 RID: 9888 RVA: 0x0011330C File Offset: 0x0011150C
	private void OnClick()
	{
		if (!PlayerScript.Pointer || !PlayerScript.PointerScript.InfoObject)
		{
			return;
		}
		foreach (GameObject gameObject in PlayerScript.PointerScript.GetSelectedObjects(-1, true, false))
		{
			if (gameObject && gameObject.GetComponent<RPGFigurines>())
			{
				if (this.bMode)
				{
					gameObject.GetComponent<RPGFigurines>().ChangeMode();
				}
				if (this.bAttack)
				{
					gameObject.GetComponent<RPGFigurines>().Attack();
				}
				if (this.bDie)
				{
					gameObject.GetComponent<RPGFigurines>().Die();
				}
			}
		}
	}

	// Token: 0x04001935 RID: 6453
	public bool bMode = true;

	// Token: 0x04001936 RID: 6454
	public bool bAttack;

	// Token: 0x04001937 RID: 6455
	public bool bDie;
}
