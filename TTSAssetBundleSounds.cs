using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

// Token: 0x020001B5 RID: 437
public class TTSAssetBundleSounds : MonoBehaviour
{
	// Token: 0x04000C6F RID: 3183
	[Tooltip("Trigger sounds are the sounds that occur when the specified action happens.")]
	public TTSAssetBundleSounds.TTSTriggerSounds TriggerSounds;

	// Token: 0x04000C70 RID: 3184
	[Tooltip("Impact sounds are the sounds that occur when objects collide. If you don't want to have to provide unique sounds for each material type just supply the generic impact sound.")]
	public TTSAssetBundleSounds.TTSImpactSounds ImpactSounds;

	// Token: 0x020006A1 RID: 1697
	[Serializable]
	public class TTSTriggerSounds
	{
		// Token: 0x040028BE RID: 10430
		public List<AudioClip> Pickup;

		// Token: 0x040028BF RID: 10431
		public List<AudioClip> Drop;

		// Token: 0x040028C0 RID: 10432
		public List<AudioClip> Shake;
	}

	// Token: 0x020006A2 RID: 1698
	[Serializable]
	public class TTSImpactSounds
	{
		// Token: 0x040028C1 RID: 10433
		public List<AudioClip> Generic;

		// Token: 0x040028C2 RID: 10434
		public List<AudioClip> Wood;

		// Token: 0x040028C3 RID: 10435
		public List<AudioClip> Metal;

		// Token: 0x040028C4 RID: 10436
		public List<AudioClip> Plastic;

		// Token: 0x040028C5 RID: 10437
		[FormerlySerializedAs("CardBoard")]
		public List<AudioClip> Cardboard;

		// Token: 0x040028C6 RID: 10438
		public List<AudioClip> Glass;

		// Token: 0x040028C7 RID: 10439
		public List<AudioClip> Felt;
	}
}
