using System;
using UnityEngine;

namespace I2.Loc
{
	// Token: 0x02000494 RID: 1172
	public class LocalizeTarget_UnityStandard_MeshRenderer : LocalizeTarget<MeshRenderer>
	{
		// Token: 0x060034C8 RID: 13512 RVA: 0x0016198C File Offset: 0x0015FB8C
		static LocalizeTarget_UnityStandard_MeshRenderer()
		{
			LocalizeTarget_UnityStandard_MeshRenderer.AutoRegister();
		}

		// Token: 0x060034C9 RID: 13513 RVA: 0x00161993 File Offset: 0x0015FB93
		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
		private static void AutoRegister()
		{
			LocalizationManager.RegisterTarget(new LocalizeTargetDesc_Type<MeshRenderer, LocalizeTarget_UnityStandard_MeshRenderer>
			{
				Name = "MeshRenderer",
				Priority = 800
			});
		}

		// Token: 0x060034CA RID: 13514 RVA: 0x001619B5 File Offset: 0x0015FBB5
		public override eTermType GetPrimaryTermType(Localize cmp)
		{
			return eTermType.Mesh;
		}

		// Token: 0x060034CB RID: 13515 RVA: 0x001619B8 File Offset: 0x0015FBB8
		public override eTermType GetSecondaryTermType(Localize cmp)
		{
			return eTermType.Material;
		}

		// Token: 0x060034CC RID: 13516 RVA: 0x00014D66 File Offset: 0x00012F66
		public override bool CanUseSecondaryTerm()
		{
			return true;
		}

		// Token: 0x060034CD RID: 13517 RVA: 0x0001E2E9 File Offset: 0x0001C4E9
		public override bool AllowMainTermToBeRTL()
		{
			return false;
		}

		// Token: 0x060034CE RID: 13518 RVA: 0x0001E2E9 File Offset: 0x0001C4E9
		public override bool AllowSecondTermToBeRTL()
		{
			return false;
		}

		// Token: 0x060034CF RID: 13519 RVA: 0x001619BC File Offset: 0x0015FBBC
		public override void GetFinalTerms(Localize cmp, string Main, string Secondary, out string primaryTerm, out string secondaryTerm)
		{
			if (this.mTarget == null)
			{
				string text;
				secondaryTerm = (text = null);
				primaryTerm = text;
			}
			else
			{
				MeshFilter component = this.mTarget.GetComponent<MeshFilter>();
				if (component == null || component.sharedMesh == null)
				{
					primaryTerm = null;
				}
				else
				{
					primaryTerm = component.sharedMesh.name;
				}
			}
			if (this.mTarget == null || this.mTarget.sharedMaterial == null)
			{
				secondaryTerm = null;
				return;
			}
			secondaryTerm = this.mTarget.sharedMaterial.name;
		}

		// Token: 0x060034D0 RID: 13520 RVA: 0x00161A54 File Offset: 0x0015FC54
		public override void DoLocalize(Localize cmp, string mainTranslation, string secondaryTranslation)
		{
			Material secondaryTranslatedObj = cmp.GetSecondaryTranslatedObj<Material>(ref mainTranslation, ref secondaryTranslation);
			if (secondaryTranslatedObj != null && this.mTarget.sharedMaterial != secondaryTranslatedObj)
			{
				this.mTarget.material = secondaryTranslatedObj;
			}
			Mesh mesh = cmp.FindTranslatedObject<Mesh>(mainTranslation);
			MeshFilter component = this.mTarget.GetComponent<MeshFilter>();
			if (mesh != null && component.sharedMesh != mesh)
			{
				component.mesh = mesh;
			}
		}
	}
}
