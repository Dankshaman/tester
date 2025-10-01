using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using NewNet;
using UnityEngine;
using VacuumShaders.TextureExtensions;

// Token: 0x020000C9 RID: 201
public class CustomAssetbundle : CustomObject
{
	// Token: 0x170001BC RID: 444
	// (get) Token: 0x06000A02 RID: 2562 RVA: 0x0004642E File Offset: 0x0004462E
	// (set) Token: 0x06000A03 RID: 2563 RVA: 0x00046436 File Offset: 0x00044636
	public override bool bCustomUI
	{
		get
		{
			return this.bcustomUI;
		}
		set
		{
			if (this.DummyObject && value)
			{
				return;
			}
			if (value != this.bcustomUI)
			{
				this.bcustomUI = value;
				if (value && Network.isServer)
				{
					Singleton<UICustomAssetbundle>.Instance.Queue(this);
				}
			}
		}
	}

	// Token: 0x06000A04 RID: 2564 RVA: 0x0004646D File Offset: 0x0004466D
	public override void Cancel()
	{
		if (Network.isServer && !this.bSetupOnce)
		{
			NetworkSingleton<ManagerPhysicsObject>.Instance.DestroyThisObject(base.gameObject);
		}
	}

	// Token: 0x06000A05 RID: 2565 RVA: 0x00046490 File Offset: 0x00044690
	protected override void Start()
	{
		base.Start();
		this.assetBundleEffects.LoopingEffects = null;
		this.assetBundleEffects.TriggerEffects = null;
		if (this.DummyObject)
		{
			this.StartWWWs();
			return;
		}
		if (Network.isServer)
		{
			if (this.CustomAssetbundleURL != "")
			{
				this.CallCustomRPC();
				return;
			}
			this.bCustomUI = true;
		}
	}

	// Token: 0x06000A06 RID: 2566 RVA: 0x000464F8 File Offset: 0x000446F8
	protected override void OnDestroy()
	{
		base.OnDestroy();
		if (Singleton<CustomLoadingManager>.Instance)
		{
			Singleton<CustomLoadingManager>.Instance.Assetbundle.Cleanup(this.CustomAssetbundleURL, new Action<CustomAssetbundleContainer>(this.OnLoadAssetBundle), true);
			Singleton<CustomLoadingManager>.Instance.Assetbundle.Cleanup(this.CustomAssetbundleSecondaryURL, new Action<CustomAssetbundleContainer>(this.OnLoadAssetBundleSecondary), true);
		}
	}

	// Token: 0x06000A07 RID: 2567 RVA: 0x0004655C File Offset: 0x0004475C
	public override void CallCustomRPC()
	{
		base.CallCustomRPC();
		if (this.bSetupOnce && Network.isServer)
		{
			ObjectState os = NetworkSingleton<ManagerPhysicsObject>.Instance.SaveObjectState(base.gameObject);
			NetworkSingleton<ManagerPhysicsObject>.Instance.DestroyThisObject(base.gameObject);
			NetworkSingleton<ManagerPhysicsObject>.Instance.LoadObjectState(os, false, false);
			return;
		}
		if (Network.isServer)
		{
			if (this.TypeInt == 6 && base.NPO.InternalName != NetworkSingleton<GameMode>.Instance.CustomAssetbundleBag.name)
			{
				ObjectState objectState = NetworkSingleton<ManagerPhysicsObject>.Instance.SaveObjectState(base.gameObject);
				NetworkSingleton<ManagerPhysicsObject>.Instance.DestroyThisObject(base.gameObject);
				objectState.Name = NetworkSingleton<GameMode>.Instance.CustomAssetbundleBag.name;
				NetworkSingleton<ManagerPhysicsObject>.Instance.LoadObjectState(objectState, false, false);
				return;
			}
			if (this.TypeInt != 6 && base.NPO.InternalName == NetworkSingleton<GameMode>.Instance.CustomAssetbundleBag.name)
			{
				ObjectState objectState2 = NetworkSingleton<ManagerPhysicsObject>.Instance.SaveObjectState(base.gameObject);
				NetworkSingleton<ManagerPhysicsObject>.Instance.DestroyThisObject(base.gameObject);
				objectState2.Name = NetworkSingleton<GameMode>.Instance.CustomAssetbundle.name;
				NetworkSingleton<ManagerPhysicsObject>.Instance.LoadObjectState(objectState2, false, false);
				return;
			}
			if (this.TypeInt == 7 && base.NPO.InternalName != NetworkSingleton<GameMode>.Instance.CustomAssetbundleInfiniteBag.name)
			{
				ObjectState objectState3 = NetworkSingleton<ManagerPhysicsObject>.Instance.SaveObjectState(base.gameObject);
				NetworkSingleton<ManagerPhysicsObject>.Instance.DestroyThisObject(base.gameObject);
				objectState3.Name = NetworkSingleton<GameMode>.Instance.CustomAssetbundleInfiniteBag.name;
				NetworkSingleton<ManagerPhysicsObject>.Instance.LoadObjectState(objectState3, false, false);
				return;
			}
			if (this.TypeInt != 7 && base.NPO.InternalName == NetworkSingleton<GameMode>.Instance.CustomAssetbundleInfiniteBag.name)
			{
				ObjectState objectState4 = NetworkSingleton<ManagerPhysicsObject>.Instance.SaveObjectState(base.gameObject);
				NetworkSingleton<ManagerPhysicsObject>.Instance.DestroyThisObject(base.gameObject);
				objectState4.Name = NetworkSingleton<GameMode>.Instance.CustomAssetbundle.name;
				NetworkSingleton<ManagerPhysicsObject>.Instance.LoadObjectState(objectState4, false, false);
				return;
			}
		}
		if (this.CustomAssetbundleURL != "")
		{
			this.OnCallCustomRPC();
			base.networkView.RPC<string, string, int, int>(RPCTarget.All, new Action<string, string, int, int>(this.SetCustomURL), this.CustomAssetbundleURL, this.CustomAssetbundleSecondaryURL, this.TypeInt, this.MaterialInt);
			if (this.LoopEffectIndex != 0)
			{
				this.RPCLoopEffect(this.LoopEffectIndex);
			}
		}
	}

	// Token: 0x06000A08 RID: 2568 RVA: 0x000467D4 File Offset: 0x000449D4
	public override void CallCustomRPC(NetworkPlayer NP)
	{
		if (this.CustomAssetbundleURL != "")
		{
			this.OnCallCustomRPC(NP);
			base.networkView.RPC<string, string, int, int>(NP, new Action<string, string, int, int>(this.SetCustomURL), this.CustomAssetbundleURL, this.CustomAssetbundleSecondaryURL, this.TypeInt, this.MaterialInt);
			if (this.LoopEffectIndex != 0)
			{
				base.networkView.RPC<int>(NP, new Action<int>(this.LoopEffect), this.LoopEffectIndex);
			}
		}
	}

	// Token: 0x06000A09 RID: 2569 RVA: 0x000025B8 File Offset: 0x000007B8
	protected virtual void OnCallCustomRPC()
	{
	}

	// Token: 0x06000A0A RID: 2570 RVA: 0x000025B8 File Offset: 0x000007B8
	protected virtual void OnCallCustomRPC(NetworkPlayer NP)
	{
	}

	// Token: 0x06000A0B RID: 2571 RVA: 0x00046850 File Offset: 0x00044A50
	[Remote(Permission.Admin)]
	private void SetCustomURL(string URL, string secondaryURL, int typeInt, int materialInt)
	{
		this.CustomAssetbundleURL = URL;
		this.CustomAssetbundleSecondaryURL = secondaryURL;
		this.TypeInt = typeInt;
		this.MaterialInt = materialInt;
		this.StartWWWs();
	}

	// Token: 0x06000A0C RID: 2572 RVA: 0x00046878 File Offset: 0x00044A78
	private void StartWWWs()
	{
		if (this.CustomAssetbundleSecondaryURL != "")
		{
			base.AddLoading();
			Singleton<CustomLoadingManager>.Instance.Assetbundle.Load(this.CustomAssetbundleSecondaryURL, new Action<CustomAssetbundleContainer>(this.OnLoadAssetBundleSecondary), null);
		}
		if (this.CustomAssetbundleURL != "")
		{
			base.AddLoading();
			Singleton<CustomLoadingManager>.Instance.Assetbundle.Load(this.CustomAssetbundleURL, new Action<CustomAssetbundleContainer>(this.OnLoadAssetBundle), null);
		}
	}

	// Token: 0x06000A0D RID: 2573 RVA: 0x000468F9 File Offset: 0x00044AF9
	public void OnLoadAssetBundle(CustomAssetbundleContainer customAssetBundleContainer)
	{
		if (customAssetBundleContainer.gameObjects == null)
		{
			base.RemoveLoading();
			return;
		}
		this.SetupAssetbundle(customAssetBundleContainer.gameObjects);
	}

	// Token: 0x06000A0E RID: 2574 RVA: 0x00046916 File Offset: 0x00044B16
	public void OnLoadAssetBundleSecondary(CustomAssetbundleContainer customAssetBundleContainer)
	{
		if (customAssetBundleContainer.gameObjects == null)
		{
			base.RemoveLoading();
			return;
		}
		this.SetupSecondaryAssetbundle(customAssetBundleContainer.gameObjects);
	}

	// Token: 0x06000A0F RID: 2575 RVA: 0x00046934 File Offset: 0x00044B34
	public static void SetupMaterialWithBlendMode(Material material, CustomAssetbundle.BlendMode blendMode)
	{
		switch (blendMode)
		{
		case CustomAssetbundle.BlendMode.Opaque:
			material.SetOverrideTag("RenderType", "");
			material.SetInt("_SrcBlend", 1);
			material.SetInt("_DstBlend", 0);
			material.SetInt("_ZWrite", 1);
			material.DisableKeyword("_ALPHATEST_ON");
			material.DisableKeyword("_ALPHABLEND_ON");
			material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
			material.renderQueue = -1;
			return;
		case CustomAssetbundle.BlendMode.Cutout:
			material.SetOverrideTag("RenderType", "TransparentCutout");
			material.SetInt("_SrcBlend", 1);
			material.SetInt("_DstBlend", 0);
			material.SetInt("_ZWrite", 1);
			material.EnableKeyword("_ALPHATEST_ON");
			material.DisableKeyword("_ALPHABLEND_ON");
			material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
			material.renderQueue = 2450;
			return;
		case CustomAssetbundle.BlendMode.Fade:
			material.SetOverrideTag("RenderType", "Transparent");
			material.SetInt("_SrcBlend", 5);
			material.SetInt("_DstBlend", 10);
			material.SetInt("_ZWrite", 0);
			material.DisableKeyword("_ALPHATEST_ON");
			material.EnableKeyword("_ALPHABLEND_ON");
			material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
			material.renderQueue = 3000;
			return;
		case CustomAssetbundle.BlendMode.Transparent:
			material.SetOverrideTag("RenderType", "Transparent");
			material.SetInt("_SrcBlend", 1);
			material.SetInt("_DstBlend", 10);
			material.SetInt("_ZWrite", 0);
			material.DisableKeyword("_ALPHATEST_ON");
			material.DisableKeyword("_ALPHABLEND_ON");
			material.EnableKeyword("_ALPHAPREMULTIPLY_ON");
			material.renderQueue = 3000;
			return;
		default:
			return;
		}
	}

	// Token: 0x06000A10 RID: 2576 RVA: 0x00046ADC File Offset: 0x00044CDC
	public static void CleanupAssetBundleMaterials(List<Material> mats)
	{
		for (int i = 0; i < mats.Count; i++)
		{
			Material material = mats[i];
			if (!(material == null))
			{
				if (material.shader == null || !material.shader.isSupported)
				{
					Chat.LogWarning("Shader didn't load correctly for AssetBundle material " + material.name + ". Assigning Standard shader.", true);
					material.shader = Shader.Find("Standard");
				}
				Shader shader = Shader.Find(material.shader.name);
				if (shader != null)
				{
					material.shader = shader;
				}
				if (material.HasProperty("_MainTex") && material.mainTexture && material.mainTexture.filterMode != FilterMode.Point)
				{
					TextureScript.ApplyTextSettings(material.mainTexture);
				}
				if (material.HasProperty("_BumpMap") && CustomAssetbundle.<CleanupAssetBundleMaterials>g__HasDXT5Texture|24_0(material, "_BumpMap") && material.shader == Shader.Find("Standard"))
				{
					material.shader = Shader.Find("DXT5nm/Standard");
				}
				if (material.HasProperty("_Mode"))
				{
					int blendMode = (int)material.GetFloat("_Mode");
					CustomAssetbundle.SetupMaterialWithBlendMode(material, (CustomAssetbundle.BlendMode)blendMode);
				}
			}
		}
		CustomAssetbundle.<>c__DisplayClass24_0 CS$<>8__locals1;
		CS$<>8__locals1.convertedNormals = new Dictionary<Texture2D, Texture2D>();
		foreach (KeyValuePair<Texture2D, Texture2D> keyValuePair in CS$<>8__locals1.convertedNormals)
		{
			UnityEngine.Object.DestroyImmediate(keyValuePair.Key, true);
		}
	}

	// Token: 0x06000A11 RID: 2577 RVA: 0x00046C6C File Offset: 0x00044E6C
	public static void CleanupAssetBundleGameObjects(GameObject[] gameObjects)
	{
		List<Material> list = new List<Material>();
		foreach (GameObject gameObject in gameObjects)
		{
			if (gameObject)
			{
				foreach (Component component in gameObject.GetComponentsInChildren<Component>(true))
				{
					if (component)
					{
						if (component is Rigidbody || component is AudioListener || component is NetworkView)
						{
							Debug.Log("Destroying this: " + component);
							UnityEngine.Object.DestroyImmediate(component, true);
						}
						try
						{
							if (component is Renderer)
							{
								foreach (Material material in (component as Renderer).sharedMaterials)
								{
									if (!(material == null))
									{
										list.TryAddUnique(material);
									}
								}
							}
						}
						catch (Exception ex)
						{
							Debug.LogWarning("Convert texture explode: " + ex.Message);
						}
						TTSAssetBundleEffects ttsassetBundleEffects;
						if ((ttsassetBundleEffects = (component as TTSAssetBundleEffects)) != null)
						{
							if (ttsassetBundleEffects.LoopingEffects != null)
							{
								for (int l = 0; l < ttsassetBundleEffects.LoopingEffects.Count; l++)
								{
									TTSAssetBundleEffects.TTSLoopingEffect ttsloopingEffect = ttsassetBundleEffects.LoopingEffects[l];
									if (ttsloopingEffect != null && ttsloopingEffect.Animation != null && ttsloopingEffect.Animation.AnimationComponent != null)
									{
										foreach (object obj in ttsloopingEffect.Animation.AnimationComponent)
										{
											AnimationState animationState = (AnimationState)obj;
											if (animationState.clip && !animationState.clip.legacy)
											{
												animationState.clip.legacy = true;
												Debug.Log("Convert to legacy!");
											}
										}
									}
								}
							}
							if (ttsassetBundleEffects.TriggerEffects != null)
							{
								for (int m = 0; m < ttsassetBundleEffects.TriggerEffects.Count; m++)
								{
									TTSAssetBundleEffects.TTSTriggerEffect ttstriggerEffect = ttsassetBundleEffects.TriggerEffects[m];
									if (ttstriggerEffect != null && ttstriggerEffect.Animation != null && ttstriggerEffect.Animation.AnimationComponent != null)
									{
										foreach (object obj2 in ttstriggerEffect.Animation.AnimationComponent)
										{
											AnimationState animationState2 = (AnimationState)obj2;
											if (animationState2.clip && !animationState2.clip.legacy)
											{
												animationState2.clip.legacy = true;
												Debug.Log("Convert to legacy!");
											}
										}
									}
								}
							}
						}
					}
				}
			}
		}
		CustomAssetbundle.CleanupAssetBundleMaterials(list);
	}

	// Token: 0x06000A12 RID: 2578 RVA: 0x00046F5C File Offset: 0x0004515C
	private void SetupAssetbundle(GameObject[] gameObjects)
	{
		this.SpawnGameObjects(gameObjects, true);
		base.RemoveLoading();
		this.bSetupOnce = true;
	}

	// Token: 0x06000A13 RID: 2579 RVA: 0x00046F73 File Offset: 0x00045173
	private void SetupSecondaryAssetbundle(GameObject[] gameObjects)
	{
		this.SpawnGameObjects(gameObjects, false);
		base.RemoveLoading();
		this.bSetupOnce = true;
	}

	// Token: 0x170001BD RID: 445
	// (get) Token: 0x06000A14 RID: 2580 RVA: 0x00046F8C File Offset: 0x0004518C
	public AudioSource AudioSource3D
	{
		get
		{
			if (this._AudioSource3D == null)
			{
				this._AudioSource3D = base.gameObject.AddComponent<AudioSource>();
				this._AudioSource3D.spatialBlend = 1f;
				this._AudioSource3D.rolloffMode = AudioRolloffMode.Linear;
				this._AudioSource3D.maxDistance = 150f;
				this._AudioSource3D.dopplerLevel = 0f;
			}
			return this._AudioSource3D;
		}
	}

	// Token: 0x170001BE RID: 446
	// (get) Token: 0x06000A15 RID: 2581 RVA: 0x00046FFC File Offset: 0x000451FC
	public AudioSource AudioSource2D
	{
		get
		{
			if (this._AudioSource2D == null)
			{
				this._AudioSource2D = base.gameObject.AddComponent<AudioSource>();
				this._AudioSource2D.spatialBlend = 0f;
				this._AudioSource2D.rolloffMode = AudioRolloffMode.Linear;
				this._AudioSource2D.maxDistance = 150f;
				this._AudioSource2D.dopplerLevel = 0f;
			}
			return this._AudioSource2D;
		}
	}

	// Token: 0x06000A16 RID: 2582 RVA: 0x0004706A File Offset: 0x0004526A
	public void RPCLoopEffect(int index)
	{
		base.networkView.RPC<int>(RPCTarget.All, new Action<int>(this.LoopEffect), index);
	}

	// Token: 0x06000A17 RID: 2583 RVA: 0x00047088 File Offset: 0x00045288
	[Remote(Permission.Client, SendType.ReliableBuffered, null, SerializationMethod.Default)]
	private void LoopEffect(int index)
	{
		this.LoopEffectIndex = index;
		if (this.assetBundleEffects.LoopingEffects == null || this.assetBundleEffects.LoopingEffects.Count <= index || index < 0)
		{
			return;
		}
		if (this.assetBundleEffects.TriggerEffects != null)
		{
			for (int i = 0; i < this.assetBundleEffects.TriggerEffects.Count; i++)
			{
				TTSAssetBundleEffects.TTSEffect effect = this.assetBundleEffects.TriggerEffects[i];
				this.DisableEffect(effect);
			}
		}
		for (int j = 0; j < this.assetBundleEffects.LoopingEffects.Count; j++)
		{
			TTSAssetBundleEffects.TTSEffect effect2 = this.assetBundleEffects.LoopingEffects[j];
			if (j != index)
			{
				this.DisableEffect(effect2);
			}
		}
		TTSAssetBundleEffects.TTSEffect effect3 = this.assetBundleEffects.LoopingEffects[index];
		this.EnableEffect(effect3, true);
		if (base.NPO)
		{
			EventManager.TriggerObjectLoopingEffect(base.NPO, index);
		}
	}

	// Token: 0x06000A18 RID: 2584 RVA: 0x0004716F File Offset: 0x0004536F
	public void RPCTriggerEffect(int index)
	{
		base.networkView.RPC<int>(RPCTarget.All, new Action<int>(this.TriggerEffect), index);
	}

	// Token: 0x06000A19 RID: 2585 RVA: 0x0004718C File Offset: 0x0004538C
	[Remote(Permission.Client, SendType.ReliableBuffered, null, SerializationMethod.Default)]
	private void TriggerEffect(int index)
	{
		if (this.assetBundleEffects.TriggerEffects == null || this.assetBundleEffects.TriggerEffects.Count <= index || index < 0)
		{
			return;
		}
		if (this.activeTriggerEffect != null)
		{
			this.DisableEffect(this.activeTriggerEffect);
			base.StopCoroutine(this.currentActiveTriggerCoroutine);
		}
		TTSAssetBundleEffects.TTSEffect effect = this.assetBundleEffects.TriggerEffects[index];
		this.activeTriggerEffect = effect;
		this.EnableEffect(effect, false);
		this.currentActiveTriggerCoroutine = base.StartCoroutine(this.DelayDisableEffect(effect));
		if (base.NPO)
		{
			EventManager.TriggerObjectTriggerEffect(base.NPO, index);
		}
	}

	// Token: 0x06000A1A RID: 2586 RVA: 0x0004722B File Offset: 0x0004542B
	private IEnumerator DelayDisableEffect(TTSAssetBundleEffects.TTSEffect effect)
	{
		yield return new WaitForSeconds((effect as TTSAssetBundleEffects.TTSTriggerEffect).Duration);
		this.activeTriggerEffect = null;
		this.currentActiveTriggerCoroutine = null;
		this.DisableEffect(effect);
		this.LoopEffect(this.LoopEffectIndex);
		yield break;
	}

	// Token: 0x06000A1B RID: 2587 RVA: 0x00047244 File Offset: 0x00045444
	private void EnableEffect(TTSAssetBundleEffects.TTSEffect effect, bool Loop)
	{
		if (effect.GameObjects != null && effect.GameObjects.Count > 0)
		{
			for (int i = 0; i < effect.GameObjects.Count; i++)
			{
				if (effect.GameObjects[i].gameObject)
				{
					effect.GameObjects[i].gameObject.SetActive(true);
				}
			}
		}
		if (effect.Particles != null && effect.Particles.Count > 0)
		{
			for (int j = 0; j < effect.Particles.Count; j++)
			{
				if (effect.Particles[j].Particle)
				{
					effect.Particles[j].Particle.Play();
					effect.Particles[j].Particle.main.loop = Loop;
				}
			}
		}
		if (effect.Animation != null && effect.Animation.AnimationComponent != null)
		{
			effect.Animation.AnimationComponent.CrossFade(effect.Animation.AnimationName, 0.25f);
		}
		if (effect.Animator != null && effect.Animator.AnimatorComponent != null)
		{
			effect.Animator.AnimatorComponent.Play(effect.Animator.StateName);
		}
		if (effect.Sound != null && effect.Sound.Audio != null)
		{
			AudioSource audioSource = effect.Sound.Positional3D ? this.AudioSource3D : this.AudioSource2D;
			audioSource.loop = Loop;
			if (Loop)
			{
				audioSource.clip = effect.Sound.Audio;
				audioSource.volume = SoundScript.GLOBAL_SOUND_MULTI;
				audioSource.Play();
				return;
			}
			audioSource.PlayOneShot(effect.Sound.Audio, SoundScript.GLOBAL_SOUND_MULTI);
		}
	}

	// Token: 0x06000A1C RID: 2588 RVA: 0x00047410 File Offset: 0x00045610
	private void DisableEffect(TTSAssetBundleEffects.TTSEffect effect)
	{
		if (effect.GameObjects != null && effect.GameObjects.Count > 0)
		{
			for (int i = 0; i < effect.GameObjects.Count; i++)
			{
				if (effect.GameObjects[i].gameObject && effect.GameObjects[i].gameObject.activeSelf)
				{
					effect.GameObjects[i].gameObject.SetActive(false);
				}
			}
		}
		if (effect.Particles != null && effect.Particles.Count > 0)
		{
			for (int j = 0; j < effect.Particles.Count; j++)
			{
				if (effect.Particles[j].Particle && effect.Particles[j].Particle.isPlaying)
				{
					effect.Particles[j].Particle.Stop();
				}
			}
		}
		if (effect.Animation != null)
		{
			effect.Animation.AnimationComponent != null;
		}
		if (effect.Sound != null && effect.Sound.Audio != null)
		{
			AudioSource audioSource = effect.Sound.Positional3D ? this.AudioSource3D : this.AudioSource2D;
			if (audioSource.isPlaying)
			{
				audioSource.Stop();
			}
		}
	}

	// Token: 0x06000A1D RID: 2589 RVA: 0x00047564 File Offset: 0x00045764
	private void SpawnGameObjects(GameObject[] gameObjects, bool main)
	{
		if (main)
		{
			if (this.CustomAssetbundleSecondaryURL != "" && this.secondaryAssetGameObjects == null)
			{
				this.mainAssetGameObjects = gameObjects;
				return;
			}
		}
		else
		{
			if (this.mainAssetGameObjects == null)
			{
				this.secondaryAssetGameObjects = gameObjects;
				return;
			}
			this.secondaryAssetGameObjects = gameObjects;
			gameObjects = this.mainAssetGameObjects;
			this.mainAssetGameObjects = null;
		}
		if (this.secondaryAssetGameObjects != null)
		{
			if (gameObjects.Length != 0)
			{
				if (this.secondaryAssetGameObjects.Length != 0)
				{
					GameObject[] array = gameObjects;
					GameObject[] array2 = this.secondaryAssetGameObjects;
					GameObject[] array3 = new GameObject[array.Length + array2.Length];
					Array.Copy(array, array3, array.Length);
					Array.Copy(array2, 0, array3, array.Length, array2.Length);
					gameObjects = array3;
				}
			}
			else
			{
				gameObjects = this.secondaryAssetGameObjects;
			}
			this.secondaryAssetGameObjects = null;
		}
		MeshRenderer meshRenderer = null;
		MeshFilter exists = null;
		Renderer renderer = null;
		for (int i = 0; i < gameObjects.Length; i++)
		{
			GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(gameObjects[i]);
			if (i == 0)
			{
				gameObject.transform.parent = base.gameObject.transform;
				gameObject.transform.localPosition = Vector3.zero;
				gameObject.transform.localRotation = gameObjects[i].transform.rotation;
				gameObject.transform.localScale = gameObjects[i].transform.localScale;
			}
			else
			{
				gameObject.transform.parent = base.gameObject.transform;
				gameObject.transform.localPosition = -gameObjects[0].transform.position + gameObjects[i].transform.position;
				gameObject.transform.localRotation = gameObjects[i].transform.rotation;
				gameObject.transform.localScale = gameObjects[i].transform.localScale;
			}
			if (!meshRenderer || !exists)
			{
				meshRenderer = gameObject.GetComponentInChildren<MeshRenderer>();
				if (meshRenderer)
				{
					exists = meshRenderer.GetComponent<MeshFilter>();
				}
			}
			if (!renderer)
			{
				renderer = gameObject.GetComponentInChildren<Renderer>();
			}
		}
		Color color = base.GetComponent<Renderer>().sharedMaterial.color;
		if (!this.DummyObject)
		{
			Renderer[] componentsInChildren = base.GetComponentsInChildren<Renderer>();
			List<Renderer> list = new List<Renderer>();
			if (exists)
			{
				list.Add(meshRenderer);
			}
			foreach (Renderer renderer2 in componentsInChildren)
			{
				if (renderer2 != base.GetComponent<Renderer>() && renderer2 != meshRenderer && renderer2.enabled)
				{
					list.Add(renderer2);
				}
			}
			base.NPO.Renderers = list;
			base.NPO.UpdateVisiblity(false);
		}
		if (color != Colour.White)
		{
			if (meshRenderer && meshRenderer.sharedMaterial && meshRenderer.sharedMaterial.HasProperty("_Color"))
			{
				meshRenderer.material.color = color;
			}
			else if (renderer && renderer.sharedMaterial && renderer.sharedMaterial.HasProperty("_Color"))
			{
				renderer.material.color = color;
			}
		}
		if (renderer)
		{
			base.GetComponent<MeshRenderer>().enabled = false;
			base.GetComponent<MeshFilter>().mesh = null;
		}
		UnityEngine.Object.DestroyImmediate(base.GetComponent<Collider>());
		if (base.GetComponentsInChildren<Collider>().Length == 0)
		{
			if (meshRenderer && exists)
			{
				meshRenderer.gameObject.AddComponent<BoxCollider>();
			}
			else if (renderer)
			{
				renderer.gameObject.AddComponent<BoxCollider>();
				if (renderer.GetComponent<ParticleSystem>())
				{
					renderer.GetComponent<BoxCollider>().size = Vector3.one;
				}
			}
			else
			{
				base.gameObject.AddComponent<BoxCollider>();
			}
		}
		if (!this.DummyObject)
		{
			CustomMesh.SetTypeAndMaterial(base.NPO, this.TypeInt, this.MaterialInt, -1f);
		}
		TTSAssetBundleEffects[] componentsInChildren2 = base.GetComponentsInChildren<TTSAssetBundleEffects>(true);
		for (int k = 1; k < componentsInChildren2.Length; k++)
		{
			TTSAssetBundleEffects ttsassetBundleEffects = componentsInChildren2[k];
			if (ttsassetBundleEffects)
			{
				if (ttsassetBundleEffects.LoopingEffects != null)
				{
					this.AddToList<TTSAssetBundleEffects.TTSLoopingEffect>(ref this.assetBundleEffects.LoopingEffects, ttsassetBundleEffects.LoopingEffects);
				}
				if (ttsassetBundleEffects.TriggerEffects != null)
				{
					this.AddToList<TTSAssetBundleEffects.TTSTriggerEffect>(ref this.assetBundleEffects.TriggerEffects, ttsassetBundleEffects.TriggerEffects);
				}
			}
		}
		if (!this.DummyObject)
		{
			TTSAssetBundleSounds[] componentsInChildren3 = base.GetComponentsInChildren<TTSAssetBundleSounds>(true);
			ObjectSounds objectSounds = UnityEngine.Object.Instantiate<ObjectSounds>(base.GetComponent<SoundScript>().GetSounds());
			base.GetComponent<SoundScript>().sounds = objectSounds;
			TTSAssetBundleSounds.TTSTriggerSounds ttstriggerSounds = new TTSAssetBundleSounds.TTSTriggerSounds();
			TTSAssetBundleSounds.TTSImpactSounds ttsimpactSounds = new TTSAssetBundleSounds.TTSImpactSounds();
			for (int l = 1; l < componentsInChildren3.Length; l++)
			{
				TTSAssetBundleSounds ttsassetBundleSounds = componentsInChildren3[l];
				if (ttsassetBundleSounds)
				{
					TTSAssetBundleSounds.TTSTriggerSounds triggerSounds = ttsassetBundleSounds.TriggerSounds;
					if (triggerSounds != null)
					{
						this.AddToList<AudioClip>(ref ttstriggerSounds.Pickup, triggerSounds.Pickup);
						this.AddToList<AudioClip>(ref ttstriggerSounds.Drop, triggerSounds.Drop);
						this.AddToList<AudioClip>(ref ttstriggerSounds.Shake, triggerSounds.Shake);
					}
					TTSAssetBundleSounds.TTSImpactSounds impactSounds = ttsassetBundleSounds.ImpactSounds;
					if (impactSounds != null)
					{
						this.AddToList<AudioClip>(ref ttsimpactSounds.Generic, impactSounds.Generic);
						this.AddToList<AudioClip>(ref ttsimpactSounds.Wood, impactSounds.Wood);
						this.AddToList<AudioClip>(ref ttsimpactSounds.Metal, impactSounds.Metal);
						this.AddToList<AudioClip>(ref ttsimpactSounds.Plastic, impactSounds.Plastic);
						this.AddToList<AudioClip>(ref ttsimpactSounds.Cardboard, impactSounds.Cardboard);
						this.AddToList<AudioClip>(ref ttsimpactSounds.Glass, impactSounds.Glass);
						this.AddToList<AudioClip>(ref ttsimpactSounds.Felt, impactSounds.Felt);
					}
				}
			}
			this.AddToSoundScript(ref objectSounds.Pickup, ttstriggerSounds.Pickup, null);
			if (this.CustomAssetbundleURL != "<Tiny Epic Western>TEWbulletdiceimpactred.unity3d" && this.CustomAssetbundleURL != "<Tiny Epic Western>TEWbulletdiceimpactyellow.unity3d")
			{
				this.AddToSoundScript(ref objectSounds.Drop, ttstriggerSounds.Drop, null);
			}
			this.AddToSoundScript(ref objectSounds.Shake, ttstriggerSounds.Shake, null);
			this.AddToSoundScript(ref objectSounds.Wood, ttsimpactSounds.Wood, ttsimpactSounds.Generic);
			this.AddToSoundScript(ref objectSounds.WoodSurface, ttsimpactSounds.Wood, ttsimpactSounds.Generic);
			this.AddToSoundScript(ref objectSounds.Metal, ttsimpactSounds.Metal, ttsimpactSounds.Generic);
			this.AddToSoundScript(ref objectSounds.MetalSurface, ttsimpactSounds.Metal, ttsimpactSounds.Generic);
			this.AddToSoundScript(ref objectSounds.Plastic, ttsimpactSounds.Plastic, ttsimpactSounds.Generic);
			this.AddToSoundScript(ref objectSounds.PlasticSurface, ttsimpactSounds.Plastic, ttsimpactSounds.Generic);
			this.AddToSoundScript(ref objectSounds.Felt, ttsimpactSounds.Felt, ttsimpactSounds.Generic);
			this.AddToSoundScript(ref objectSounds.FeltSurface, ttsimpactSounds.Felt, ttsimpactSounds.Generic);
			this.AddToSoundScript(ref objectSounds.Cardboard, ttsimpactSounds.Cardboard, ttsimpactSounds.Generic);
			this.AddToSoundScript(ref objectSounds.CardboardSurface, ttsimpactSounds.Cardboard, ttsimpactSounds.Generic);
			this.AddToSoundScript(ref objectSounds.Glass, ttsimpactSounds.Glass, ttsimpactSounds.Generic);
			this.AddToSoundScript(ref objectSounds.GlassSurface, ttsimpactSounds.Glass, ttsimpactSounds.Generic);
			this.assetBundleSounds.TriggerSounds = ttstriggerSounds;
			this.assetBundleSounds.ImpactSounds = ttsimpactSounds;
		}
		base.ResetObject();
		this.LoopEffect(this.LoopEffectIndex);
	}

	// Token: 0x06000A1E RID: 2590 RVA: 0x00047CC0 File Offset: 0x00045EC0
	private void AddToList<T>(ref List<T> original, List<T> add)
	{
		if (add != null && add.Count > 0)
		{
			if (original == null)
			{
				original = new List<!!0>(add);
				return;
			}
			original.AddRange(add);
		}
	}

	// Token: 0x06000A1F RID: 2591 RVA: 0x00047CE3 File Offset: 0x00045EE3
	private void AddToSoundScript(ref AudioClip[] original, List<AudioClip> add, List<AudioClip> generic)
	{
		if (add != null && add.Count > 0)
		{
			original = add.ToArray();
			return;
		}
		if (generic != null && generic.Count > 0)
		{
			original = generic.ToArray();
		}
	}

	// Token: 0x06000A21 RID: 2593 RVA: 0x00047D2C File Offset: 0x00045F2C
	[CompilerGenerated]
	internal static bool <CleanupAssetBundleMaterials>g__HasDXT5Texture|24_0(Material mat, string propertyName)
	{
		if (mat.HasProperty(propertyName))
		{
			Texture2D texture2D = mat.GetTexture(propertyName) as Texture2D;
			if (texture2D != null && texture2D.format == TextureFormat.DXT5)
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x06000A22 RID: 2594 RVA: 0x00047D68 File Offset: 0x00045F68
	[CompilerGenerated]
	internal static void <CleanupAssetBundleMaterials>g__ConvertTextureNormal|24_1(Material mat, string propertyName, ref CustomAssetbundle.<>c__DisplayClass24_0 A_2)
	{
		if (mat.HasProperty(propertyName))
		{
			Texture2D texture2D = mat.GetTexture(propertyName) as Texture2D;
			if (texture2D != null)
			{
				Texture2D value;
				if (A_2.convertedNormals.TryGetValue(texture2D, out value))
				{
					mat.SetTexture(propertyName, value);
					Debug.Log("Assign normal: " + mat.name);
					return;
				}
				if (!A_2.convertedNormals.ContainsValue(texture2D) && !texture2D.name.StartsWith("{CONVERTED}"))
				{
					Texture2D texture2D2 = new Texture2D(texture2D.width, texture2D.height, texture2D.format, texture2D.mipmapCount > 1, false);
					texture2D.ResizePro(texture2D.width, texture2D.height, out texture2D2);
					for (int i = 0; i < texture2D2.width; i++)
					{
						for (int j = 0; j < texture2D2.height; j++)
						{
							Color pixel = texture2D2.GetPixel(i, j);
							pixel.r = 1f;
							pixel.b = 1f;
							texture2D2.SetPixel(i, j, pixel);
						}
					}
					texture2D2.Apply(true, true);
					texture2D2.name = "{CONVERTED}" + texture2D.name;
					mat.SetTexture(propertyName, texture2D2);
					Debug.Log("Convert normal: " + mat.name + " : " + texture2D.name);
					texture2D.GetInstanceID();
					A_2.convertedNormals.Add(texture2D, texture2D2);
					return;
				}
				Debug.Log(string.Concat(new object[]
				{
					"Nope: ",
					mat.name,
					" : ",
					texture2D.name,
					" : ",
					texture2D.format
				}));
				return;
			}
			else
			{
				Debug.Log("No bump texture: " + mat.name);
			}
		}
	}

	// Token: 0x0400071D RID: 1821
	private bool bSetupOnce;

	// Token: 0x0400071E RID: 1822
	public string CustomAssetbundleURL = "";

	// Token: 0x0400071F RID: 1823
	public string CustomAssetbundleSecondaryURL = "";

	// Token: 0x04000720 RID: 1824
	public int TypeInt;

	// Token: 0x04000721 RID: 1825
	public int MaterialInt;

	// Token: 0x04000722 RID: 1826
	public int LoopEffectIndex;

	// Token: 0x04000723 RID: 1827
	public TTSAssetBundleEffects assetBundleEffects;

	// Token: 0x04000724 RID: 1828
	public TTSAssetBundleSounds assetBundleSounds;

	// Token: 0x04000725 RID: 1829
	private GameObject[] mainAssetGameObjects;

	// Token: 0x04000726 RID: 1830
	private GameObject[] secondaryAssetGameObjects;

	// Token: 0x04000727 RID: 1831
	private AudioSource _AudioSource3D;

	// Token: 0x04000728 RID: 1832
	private AudioSource _AudioSource2D;

	// Token: 0x04000729 RID: 1833
	private TTSAssetBundleEffects.TTSEffect activeTriggerEffect;

	// Token: 0x0400072A RID: 1834
	private Coroutine currentActiveTriggerCoroutine;

	// Token: 0x02000594 RID: 1428
	public enum BlendMode
	{
		// Token: 0x04002561 RID: 9569
		Opaque,
		// Token: 0x04002562 RID: 9570
		Cutout,
		// Token: 0x04002563 RID: 9571
		Fade,
		// Token: 0x04002564 RID: 9572
		Transparent
	}
}
