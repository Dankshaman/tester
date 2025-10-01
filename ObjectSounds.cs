using System;
using UnityEngine;

// Token: 0x020001D1 RID: 465
[CreateAssetMenu(fileName = "ObjectSounds", menuName = "ScriptableObjects/ObjectSounds")]
public class ObjectSounds : ScriptableObject
{
	// Token: 0x06001856 RID: 6230 RVA: 0x000A588C File Offset: 0x000A3A8C
	public AudioClip[] SoundsFromMaterial(SoundMaterialType soundMaterialType)
	{
		switch (soundMaterialType)
		{
		case SoundMaterialType.Wood:
			return ObjectSounds.GetSounds(this.Wood, this.WoodSurface);
		case SoundMaterialType.WoodSurface:
			return ObjectSounds.GetSounds(this.WoodSurface, this.Wood);
		case SoundMaterialType.Metal:
			return ObjectSounds.GetSounds(this.Metal, this.MetalSurface);
		case SoundMaterialType.MetalSurface:
			return ObjectSounds.GetSounds(this.MetalSurface, this.Metal);
		case SoundMaterialType.Plastic:
			return ObjectSounds.GetSounds(this.Plastic, this.PlasticSurface);
		case SoundMaterialType.PlasticSurface:
			return ObjectSounds.GetSounds(this.PlasticSurface, this.Plastic);
		case SoundMaterialType.Cardboard:
			return ObjectSounds.GetSounds(this.Cardboard, this.CardboardSurface);
		case SoundMaterialType.CardboardSurface:
			return ObjectSounds.GetSounds(this.CardboardSurface, this.Cardboard);
		case SoundMaterialType.Glass:
			return ObjectSounds.GetSounds(this.Glass, this.GlassSurface);
		case SoundMaterialType.GlassSurface:
			return ObjectSounds.GetSounds(this.GlassSurface, this.Glass);
		case SoundMaterialType.Felt:
			return ObjectSounds.GetSounds(this.Felt, this.FeltSurface);
		case SoundMaterialType.FeltSurface:
			return ObjectSounds.GetSounds(this.FeltSurface, this.Felt);
		case SoundMaterialType.Pickup:
			return ObjectSounds.GetSounds(this.Pickup, this.Shake);
		case SoundMaterialType.Shake:
			return ObjectSounds.GetSounds(this.Shake, this.Pickup);
		default:
			return null;
		}
	}

	// Token: 0x06001857 RID: 6231 RVA: 0x000A59DB File Offset: 0x000A3BDB
	private static AudioClip[] GetSounds(AudioClip[] a, AudioClip[] b)
	{
		if (a == null || a.Length == 0)
		{
			return b;
		}
		return a;
	}

	// Token: 0x04000E85 RID: 3717
	public AudioClip[] Wood;

	// Token: 0x04000E86 RID: 3718
	public AudioClip[] WoodSurface;

	// Token: 0x04000E87 RID: 3719
	public AudioClip[] Metal;

	// Token: 0x04000E88 RID: 3720
	public AudioClip[] MetalSurface;

	// Token: 0x04000E89 RID: 3721
	public AudioClip[] Plastic;

	// Token: 0x04000E8A RID: 3722
	public AudioClip[] PlasticSurface;

	// Token: 0x04000E8B RID: 3723
	public AudioClip[] Cardboard;

	// Token: 0x04000E8C RID: 3724
	public AudioClip[] CardboardSurface;

	// Token: 0x04000E8D RID: 3725
	public AudioClip[] Glass;

	// Token: 0x04000E8E RID: 3726
	public AudioClip[] GlassSurface;

	// Token: 0x04000E8F RID: 3727
	public AudioClip[] Felt;

	// Token: 0x04000E90 RID: 3728
	public AudioClip[] FeltSurface;

	// Token: 0x04000E91 RID: 3729
	public AudioClip[] Pickup;

	// Token: 0x04000E92 RID: 3730
	public AudioClip[] Drop;

	// Token: 0x04000E93 RID: 3731
	public AudioClip[] Shake;
}
