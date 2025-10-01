using System;
using UnityEngine;

namespace I2.Loc
{
	// Token: 0x02000493 RID: 1171
	public class LocalizeTarget_UnityStandard_Child : LocalizeTarget<GameObject>
	{
		// Token: 0x060034BD RID: 13501 RVA: 0x001618DD File Offset: 0x0015FADD
		static LocalizeTarget_UnityStandard_Child()
		{
			LocalizeTarget_UnityStandard_Child.AutoRegister();
		}

		// Token: 0x060034BE RID: 13502 RVA: 0x001618E4 File Offset: 0x0015FAE4
		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
		private static void AutoRegister()
		{
			LocalizationManager.RegisterTarget(new LocalizeTargetDesc_Child
			{
				Name = "Child",
				Priority = 200
			});
		}

		// Token: 0x060034BF RID: 13503 RVA: 0x001618C5 File Offset: 0x0015FAC5
		public override bool IsValid(Localize cmp)
		{
			return cmp.transform.childCount > 1;
		}

		// Token: 0x060034C0 RID: 13504 RVA: 0x00161906 File Offset: 0x0015FB06
		public override eTermType GetPrimaryTermType(Localize cmp)
		{
			return eTermType.GameObject;
		}

		// Token: 0x060034C1 RID: 13505 RVA: 0x0001E2E9 File Offset: 0x0001C4E9
		public override eTermType GetSecondaryTermType(Localize cmp)
		{
			return eTermType.Text;
		}

		// Token: 0x060034C2 RID: 13506 RVA: 0x0001E2E9 File Offset: 0x0001C4E9
		public override bool CanUseSecondaryTerm()
		{
			return false;
		}

		// Token: 0x060034C3 RID: 13507 RVA: 0x0001E2E9 File Offset: 0x0001C4E9
		public override bool AllowMainTermToBeRTL()
		{
			return false;
		}

		// Token: 0x060034C4 RID: 13508 RVA: 0x0001E2E9 File Offset: 0x0001C4E9
		public override bool AllowSecondTermToBeRTL()
		{
			return false;
		}

		// Token: 0x060034C5 RID: 13509 RVA: 0x00161909 File Offset: 0x0015FB09
		public override void GetFinalTerms(Localize cmp, string Main, string Secondary, out string primaryTerm, out string secondaryTerm)
		{
			primaryTerm = cmp.name;
			secondaryTerm = null;
		}

		// Token: 0x060034C6 RID: 13510 RVA: 0x00161918 File Offset: 0x0015FB18
		public override void DoLocalize(Localize cmp, string mainTranslation, string secondaryTranslation)
		{
			if (string.IsNullOrEmpty(mainTranslation))
			{
				return;
			}
			Transform transform = cmp.transform;
			string text = mainTranslation;
			int num = mainTranslation.LastIndexOfAny(LanguageSourceData.CategorySeparators);
			if (num >= 0)
			{
				text = text.Substring(num + 1);
			}
			for (int i = 0; i < transform.childCount; i++)
			{
				Transform child = transform.GetChild(i);
				child.gameObject.SetActive(child.name == text);
			}
		}
	}
}
