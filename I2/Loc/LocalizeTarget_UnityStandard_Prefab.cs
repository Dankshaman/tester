using System;
using UnityEngine;

namespace I2.Loc
{
	// Token: 0x02000496 RID: 1174
	public class LocalizeTarget_UnityStandard_Prefab : LocalizeTarget<GameObject>
	{
		// Token: 0x060034D4 RID: 13524 RVA: 0x00161AD6 File Offset: 0x0015FCD6
		static LocalizeTarget_UnityStandard_Prefab()
		{
			LocalizeTarget_UnityStandard_Prefab.AutoRegister();
		}

		// Token: 0x060034D5 RID: 13525 RVA: 0x00161ADD File Offset: 0x0015FCDD
		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
		private static void AutoRegister()
		{
			LocalizationManager.RegisterTarget(new LocalizeTargetDesc_Prefab
			{
				Name = "Prefab",
				Priority = 250
			});
		}

		// Token: 0x060034D6 RID: 13526 RVA: 0x00014D66 File Offset: 0x00012F66
		public override bool IsValid(Localize cmp)
		{
			return true;
		}

		// Token: 0x060034D7 RID: 13527 RVA: 0x00161906 File Offset: 0x0015FB06
		public override eTermType GetPrimaryTermType(Localize cmp)
		{
			return eTermType.GameObject;
		}

		// Token: 0x060034D8 RID: 13528 RVA: 0x0001E2E9 File Offset: 0x0001C4E9
		public override eTermType GetSecondaryTermType(Localize cmp)
		{
			return eTermType.Text;
		}

		// Token: 0x060034D9 RID: 13529 RVA: 0x0001E2E9 File Offset: 0x0001C4E9
		public override bool CanUseSecondaryTerm()
		{
			return false;
		}

		// Token: 0x060034DA RID: 13530 RVA: 0x0001E2E9 File Offset: 0x0001C4E9
		public override bool AllowMainTermToBeRTL()
		{
			return false;
		}

		// Token: 0x060034DB RID: 13531 RVA: 0x0001E2E9 File Offset: 0x0001C4E9
		public override bool AllowSecondTermToBeRTL()
		{
			return false;
		}

		// Token: 0x060034DC RID: 13532 RVA: 0x00161909 File Offset: 0x0015FB09
		public override void GetFinalTerms(Localize cmp, string Main, string Secondary, out string primaryTerm, out string secondaryTerm)
		{
			primaryTerm = cmp.name;
			secondaryTerm = null;
		}

		// Token: 0x060034DD RID: 13533 RVA: 0x00161B00 File Offset: 0x0015FD00
		public override void DoLocalize(Localize cmp, string mainTranslation, string secondaryTranslation)
		{
			if (string.IsNullOrEmpty(mainTranslation))
			{
				return;
			}
			if (this.mTarget && this.mTarget.name == mainTranslation)
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
			Transform transform2 = this.InstantiateNewPrefab(cmp, mainTranslation);
			if (transform2 == null)
			{
				return;
			}
			transform2.name = text;
			for (int i = transform.childCount - 1; i >= 0; i--)
			{
				Transform child = transform.GetChild(i);
				if (child != transform2)
				{
					UnityEngine.Object.Destroy(child.gameObject);
				}
			}
		}

		// Token: 0x060034DE RID: 13534 RVA: 0x00161BAC File Offset: 0x0015FDAC
		private Transform InstantiateNewPrefab(Localize cmp, string mainTranslation)
		{
			GameObject gameObject = cmp.FindTranslatedObject<GameObject>(mainTranslation);
			if (gameObject == null)
			{
				return null;
			}
			GameObject mTarget = this.mTarget;
			this.mTarget = UnityEngine.Object.Instantiate<GameObject>(gameObject);
			if (this.mTarget == null)
			{
				return null;
			}
			Transform transform = cmp.transform;
			Transform transform2 = this.mTarget.transform;
			transform2.SetParent(transform);
			Transform transform3 = mTarget ? mTarget.transform : transform;
			transform2.rotation = transform3.rotation;
			transform2.position = transform3.position;
			return transform2;
		}
	}
}
