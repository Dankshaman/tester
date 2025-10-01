using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020001B4 RID: 436
public class TTSAssetBundleEffects : MonoBehaviour
{
	// Token: 0x04000C6D RID: 3181
	[Tooltip("Looping effects are great for idle animations, looping sounds, or continous particle effects. Looping effects are swappable using the contextual menu on the object.")]
	public List<TTSAssetBundleEffects.TTSLoopingEffect> LoopingEffects = new List<TTSAssetBundleEffects.TTSLoopingEffect>
	{
		new TTSAssetBundleEffects.TTSLoopingEffect()
	};

	// Token: 0x04000C6E RID: 3182
	[Tooltip("Trigger effects are great for attack animations, sound effects, or spell effects. Trigger effects are executed using the contextual menu on the object.")]
	public List<TTSAssetBundleEffects.TTSTriggerEffect> TriggerEffects = new List<TTSAssetBundleEffects.TTSTriggerEffect>
	{
		new TTSAssetBundleEffects.TTSTriggerEffect()
	};

	// Token: 0x02000699 RID: 1689
	[Serializable]
	public class TTSParticle
	{
		// Token: 0x040028AF RID: 10415
		public ParticleSystem Particle;
	}

	// Token: 0x0200069A RID: 1690
	[Serializable]
	public class TTSGameObject
	{
		// Token: 0x040028B0 RID: 10416
		public GameObject gameObject;
	}

	// Token: 0x0200069B RID: 1691
	[Serializable]
	public class TTSSound
	{
		// Token: 0x06003C0B RID: 15371 RVA: 0x00178744 File Offset: 0x00176944
		public TTSSound()
		{
			this.Positional3D = true;
		}

		// Token: 0x040028B1 RID: 10417
		public AudioClip Audio;

		// Token: 0x040028B2 RID: 10418
		public bool Positional3D = true;
	}

	// Token: 0x0200069C RID: 1692
	[Serializable]
	public class TTSAnimation
	{
		// Token: 0x040028B3 RID: 10419
		public Animation AnimationComponent;

		// Token: 0x040028B4 RID: 10420
		public string AnimationName;
	}

	// Token: 0x0200069D RID: 1693
	[Serializable]
	public class TTSAnimator
	{
		// Token: 0x040028B5 RID: 10421
		public Animator AnimatorComponent;

		// Token: 0x040028B6 RID: 10422
		public string StateName;
	}

	// Token: 0x0200069E RID: 1694
	[Serializable]
	public class TTSEffect
	{
		// Token: 0x040028B7 RID: 10423
		public string Name;

		// Token: 0x040028B8 RID: 10424
		public List<TTSAssetBundleEffects.TTSGameObject> GameObjects;

		// Token: 0x040028B9 RID: 10425
		public List<TTSAssetBundleEffects.TTSParticle> Particles;

		// Token: 0x040028BA RID: 10426
		public TTSAssetBundleEffects.TTSSound Sound;

		// Token: 0x040028BB RID: 10427
		public TTSAssetBundleEffects.TTSAnimation Animation;

		// Token: 0x040028BC RID: 10428
		public TTSAssetBundleEffects.TTSAnimator Animator;
	}

	// Token: 0x0200069F RID: 1695
	[Serializable]
	public class TTSLoopingEffect : TTSAssetBundleEffects.TTSEffect
	{
	}

	// Token: 0x020006A0 RID: 1696
	[Serializable]
	public class TTSTriggerEffect : TTSAssetBundleEffects.TTSEffect
	{
		// Token: 0x040028BD RID: 10429
		public float Duration = 1f;
	}
}
