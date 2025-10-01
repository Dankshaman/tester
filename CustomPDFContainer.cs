using System;
using System.Threading;
using System.Threading.Tasks;
using Paroxe.PdfRenderer;
using UnityEngine;

// Token: 0x020000DB RID: 219
public class CustomPDFContainer : CustomContainer
{
	// Token: 0x170001CF RID: 463
	// (get) Token: 0x06000ACA RID: 2762 RVA: 0x0004BABE File Offset: 0x00049CBE
	// (set) Token: 0x06000ACB RID: 2763 RVA: 0x0004BAC6 File Offset: 0x00049CC6
	public PDFDocument PDFDocument { get; private set; }

	// Token: 0x170001D0 RID: 464
	// (get) Token: 0x06000ACC RID: 2764 RVA: 0x0004BACF File Offset: 0x00049CCF
	// (set) Token: 0x06000ACD RID: 2765 RVA: 0x0004BAD7 File Offset: 0x00049CD7
	public Texture2D[] CachedTextures { get; private set; }

	// Token: 0x170001D1 RID: 465
	// (get) Token: 0x06000ACE RID: 2766 RVA: 0x0004BAE0 File Offset: 0x00049CE0
	// (set) Token: 0x06000ACF RID: 2767 RVA: 0x0004BAE8 File Offset: 0x00049CE8
	public float ResolutionScale { get; private set; } = 1f;

	// Token: 0x06000AD0 RID: 2768 RVA: 0x0004BAF4 File Offset: 0x00049CF4
	public CustomPDFContainer(string url, PDFDocument pdfDocument)
	{
		base.url = url;
		base.nonCodeStrippedURL = Singleton<CustomLoadingManager>.Instance.PDF.NonCodeStrippedFromURL(url);
		this.PDFDocument = pdfDocument;
		if (pdfDocument != null)
		{
			this.CachedTextures = new Texture2D[pdfDocument.GetPageCount()];
		}
	}

	// Token: 0x06000AD1 RID: 2769 RVA: 0x0004BB54 File Offset: 0x00049D54
	public async Task<Texture2D> GetPageTexture(int pageNumber)
	{
		if (this.CachedTextures[pageNumber] == null)
		{
			if (this.async)
			{
				CustomPDFContainer.<>c__DisplayClass14_0 CS$<>8__locals1 = new CustomPDFContainer.<>c__DisplayClass14_0();
				CS$<>8__locals1.pagePromise = this.PDFDocument.GetPageAsync(pageNumber);
				await Task.Run(delegate()
				{
					while (!CS$<>8__locals1.pagePromise.HasFinished)
					{
						Thread.Sleep(1);
					}
				});
				if (!CS$<>8__locals1.pagePromise.HasSucceeded)
				{
					Debug.Log("Fail: pagePromise");
					return null;
				}
				PDFPage result = CS$<>8__locals1.pagePromise.Result;
				Vector2 vector = result.GetPageSize(this.ResolutionScale);
				float d = (float)Screen.height / vector.y;
				vector *= d;
				CS$<>8__locals1.renderPromise = PDFRenderer.RenderPageToTextureAsync(result, vector);
				await Task.Run(delegate()
				{
					while (!CS$<>8__locals1.renderPromise.HasFinished)
					{
						Thread.Sleep(1);
					}
				});
				if (!CS$<>8__locals1.renderPromise.HasSucceeded)
				{
					Debug.Log("Fail: pagePromise");
					return null;
				}
				Texture2D result2 = CS$<>8__locals1.renderPromise.Result;
				this.CachedTextures[pageNumber] = result2;
				CS$<>8__locals1 = null;
			}
			else
			{
				PDFPage page = this.PDFDocument.GetPage(pageNumber);
				if (page == null)
				{
					Debug.Log("Fail: page");
					return null;
				}
				Vector2 vector2 = page.GetPageSize(this.ResolutionScale);
				vector2 *= (float)Screen.height / vector2.y;
				this.CachedTextures[pageNumber] = this.PDFDocument.Renderer.RenderPageToTexture(page, (int)vector2.x, (int)vector2.y);
			}
			TextureScript.ApplyTextSettings(this.CachedTextures[pageNumber]);
		}
		return this.CachedTextures[pageNumber];
	}

	// Token: 0x06000AD2 RID: 2770 RVA: 0x0004BBA1 File Offset: 0x00049DA1
	public int GetPageCount()
	{
		return this.PDFDocument.GetPageCount();
	}

	// Token: 0x06000AD3 RID: 2771 RVA: 0x0004BBB0 File Offset: 0x00049DB0
	public override void Cleanup(bool forceCleanup = false)
	{
		if (!forceCleanup && DLCManager.URLisDLC(base.url))
		{
			return;
		}
		PDFDocument pdfdocument = this.PDFDocument;
		if (pdfdocument != null)
		{
			pdfdocument.Dispose();
		}
		if (this.CachedTextures != null)
		{
			for (int i = 0; i < this.CachedTextures.Length; i++)
			{
				UnityEngine.Object.Destroy(this.CachedTextures[i]);
			}
		}
	}

	// Token: 0x06000AD4 RID: 2772 RVA: 0x0004BC07 File Offset: 0x00049E07
	public override bool IsError()
	{
		return this.PDFDocument == null;
	}

	// Token: 0x040007A1 RID: 1953
	private readonly bool async = true;
}
