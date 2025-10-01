using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

// Token: 0x02000153 RID: 339
public class LoadingScript : MonoBehaviour
{
	// Token: 0x06001121 RID: 4385 RVA: 0x000761C4 File Offset: 0x000743C4
	private void Awake()
	{
		this.ScaleMulti = this.StartScale;
		this.LoadingRenderer = this.LoadPlane.GetComponent<Renderer>();
		this.LoadingAudioSource = this.LoadPlane.GetComponent<AudioSource>();
		this.ResizeLoadPlane();
		if (Utilities.IsLaunchOption("-novid") || Utilities.IsLaunchOption("-nointro"))
		{
			this.StopFadeThenLoad();
			return;
		}
		this.fadeCoroutine = base.StartCoroutine(this.Fade());
	}

	// Token: 0x06001122 RID: 4386 RVA: 0x00076236 File Offset: 0x00074436
	private void Update()
	{
		if (Input.anyKeyDown)
		{
			this.StopFadeThenLoad();
		}
		this.ResizeLoadPlane();
	}

	// Token: 0x06001123 RID: 4387 RVA: 0x0007624C File Offset: 0x0007444C
	private void ResizeLoadPlane()
	{
		float num = (float)Screen.width / (float)Screen.height;
		num = Mathf.Min(num, 1.7777778f);
		if (this.Fading)
		{
			this.ScaleMulti += Time.deltaTime * this.ScaleSpeed * 0.05f;
		}
		else
		{
			this.ScaleMulti = 1f;
		}
		this.LoadPlane.transform.localScale = new Vector3(16f * num, 1f, 9f * num) * this.ScaleMulti;
	}

	// Token: 0x06001124 RID: 4388 RVA: 0x000762DA File Offset: 0x000744DA
	private IEnumerator Fade()
	{
		this.LoadingRenderer.material.color = new Color(1f, 1f, 1f, 0f);
		yield return new WaitForSeconds(this.DelayTime);
		while (this.Fading)
		{
			Color color = this.LoadingRenderer.material.color;
			if (this.FadeIn)
			{
				color.a = Mathf.Lerp(color.a, 1f, this.t);
				this.t += Time.deltaTime * this.FadeInSpeed;
				this.LoadingRenderer.material.color = color;
				if (!this.PlayedSound && color.a >= this.SoundPlayTransparency)
				{
					this.LoadingAudioSource.Play();
					this.PlayedSound = true;
				}
				if (color.a >= 0.97f)
				{
					color.a = 1f;
					this.FadeIn = false;
					this.t = 0f;
					yield return new WaitForSeconds(this.StayTime);
				}
			}
			else
			{
				color.a = Mathf.Lerp(color.a, 0f, this.t);
				this.t += Time.deltaTime * this.FadeOutSpeed;
				this.LoadingRenderer.material.color = color;
				if (color.a <= 0f)
				{
					yield return new WaitForSeconds(this.BlackTime);
					this.Fading = false;
				}
			}
			yield return null;
		}
		if (this.Loop && Application.isEditor)
		{
			this.Fading = true;
			this.FadeIn = true;
			this.PlayedSound = false;
			this.t = 0f;
			this.ScaleMulti = this.StartScale;
			this.ResizeLoadPlane();
			this.fadeCoroutine = base.StartCoroutine(this.Fade());
		}
		else
		{
			base.StartCoroutine(this.LoadGame());
		}
		yield break;
	}

	// Token: 0x06001125 RID: 4389 RVA: 0x000762E9 File Offset: 0x000744E9
	private void StopFadeThenLoad()
	{
		if (this.Fading)
		{
			base.StartCoroutine(this.LoadGame());
			this.LoadingAudioSource.Stop();
		}
	}

	// Token: 0x06001126 RID: 4390 RVA: 0x0007630B File Offset: 0x0007450B
	private IEnumerator LoadGame()
	{
		if (this.fadeCoroutine != null)
		{
			base.StopCoroutine(this.fadeCoroutine);
		}
		this.Fading = false;
		this.LoadingRenderer.material.color = Colour.UnityWhite;
		this.ResizeLoadPlane();
		if (this.LoadingScreens.Length != 0)
		{
			this.LoadingRenderer.material.mainTexture = this.LoadingScreens[UnityEngine.Random.Range(0, this.LoadingScreens.Length)];
			this.LoadingRenderer.material.mainTexture.mipMapBias = -0.5f;
		}
		this.UICanvas.gameObject.SetActive(true);
		yield return null;
		AsyncOperation async = SceneManager.LoadSceneAsync(1);
		while (!async.isDone)
		{
			Debug.Log(Time.deltaTime + " : " + async.progress);
			yield return null;
		}
		yield break;
	}

	// Token: 0x04000AF0 RID: 2800
	public bool Loop;

	// Token: 0x04000AF1 RID: 2801
	public float DelayTime = 0.25f;

	// Token: 0x04000AF2 RID: 2802
	public float FadeInSpeed = 0.1f;

	// Token: 0x04000AF3 RID: 2803
	public float SoundPlayTransparency = 0.75f;

	// Token: 0x04000AF4 RID: 2804
	public float StayTime = 0.5f;

	// Token: 0x04000AF5 RID: 2805
	public float FadeOutSpeed = 0.5f;

	// Token: 0x04000AF6 RID: 2806
	public float BlackTime = 0.25f;

	// Token: 0x04000AF7 RID: 2807
	public float StartScale = 0.7f;

	// Token: 0x04000AF8 RID: 2808
	public float ScaleSpeed = 1f;

	// Token: 0x04000AF9 RID: 2809
	public GameObject LoadPlane;

	// Token: 0x04000AFA RID: 2810
	public Texture BerserkLogo;

	// Token: 0x04000AFB RID: 2811
	public Texture[] LoadingScreens;

	// Token: 0x04000AFC RID: 2812
	public Canvas UICanvas;

	// Token: 0x04000AFD RID: 2813
	private Renderer LoadingRenderer;

	// Token: 0x04000AFE RID: 2814
	private AudioSource LoadingAudioSource;

	// Token: 0x04000AFF RID: 2815
	private float ScaleMulti;

	// Token: 0x04000B00 RID: 2816
	private Coroutine fadeCoroutine;

	// Token: 0x04000B01 RID: 2817
	private bool Fading = true;

	// Token: 0x04000B02 RID: 2818
	private bool FadeIn = true;

	// Token: 0x04000B03 RID: 2819
	private bool PlayedSound;

	// Token: 0x04000B04 RID: 2820
	private float t;
}
