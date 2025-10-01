using System;
using UnityEngine;

namespace I2.Loc
{
	// Token: 0x02000491 RID: 1169
	public class LocalizeTarget_UnityStandard_AudioSource : LocalizeTarget<AudioSource>
	{
		// Token: 0x060034B1 RID: 13489 RVA: 0x001617EF File Offset: 0x0015F9EF
		static LocalizeTarget_UnityStandard_AudioSource()
		{
			LocalizeTarget_UnityStandard_AudioSource.AutoRegister();
		}

		// Token: 0x060034B2 RID: 13490 RVA: 0x001617F6 File Offset: 0x0015F9F6
		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
		private static void AutoRegister()
		{
			LocalizationManager.RegisterTarget(new LocalizeTargetDesc_Type<AudioSource, LocalizeTarget_UnityStandard_AudioSource>
			{
				Name = "AudioSource",
				Priority = 100
			});
		}

		// Token: 0x060034B3 RID: 13491 RVA: 0x0014A9EA File Offset: 0x00148BEA
		public override eTermType GetPrimaryTermType(Localize cmp)
		{
			return eTermType.AudioClip;
		}

		// Token: 0x060034B4 RID: 13492 RVA: 0x0001E2E9 File Offset: 0x0001C4E9
		public override eTermType GetSecondaryTermType(Localize cmp)
		{
			return eTermType.Text;
		}

		// Token: 0x060034B5 RID: 13493 RVA: 0x0001E2E9 File Offset: 0x0001C4E9
		public override bool CanUseSecondaryTerm()
		{
			return false;
		}

		// Token: 0x060034B6 RID: 13494 RVA: 0x0001E2E9 File Offset: 0x0001C4E9
		public override bool AllowMainTermToBeRTL()
		{
			return false;
		}

		// Token: 0x060034B7 RID: 13495 RVA: 0x0001E2E9 File Offset: 0x0001C4E9
		public override bool AllowSecondTermToBeRTL()
		{
			return false;
		}

		// Token: 0x060034B8 RID: 13496 RVA: 0x00161815 File Offset: 0x0015FA15
		public override void GetFinalTerms(Localize cmp, string Main, string Secondary, out string primaryTerm, out string secondaryTerm)
		{
			primaryTerm = (this.mTarget.clip ? this.mTarget.clip.name : string.Empty);
			secondaryTerm = null;
		}

		// Token: 0x060034B9 RID: 13497 RVA: 0x00161848 File Offset: 0x0015FA48
		public override void DoLocalize(Localize cmp, string mainTranslation, string secondaryTranslation)
		{
			bool flag = (this.mTarget.isPlaying || this.mTarget.loop) && Application.isPlaying;
			UnityEngine.Object clip = this.mTarget.clip;
			AudioClip audioClip = cmp.FindTranslatedObject<AudioClip>(mainTranslation);
			if (clip != audioClip)
			{
				this.mTarget.clip = audioClip;
			}
			if (flag && this.mTarget.clip)
			{
				this.mTarget.Play();
			}
		}
	}
}
