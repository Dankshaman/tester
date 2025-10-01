using System;

// Token: 0x020001AF RID: 431
public class MeshLoaderJob : ThreadedJob
{
	// Token: 0x060015F1 RID: 5617 RVA: 0x00099148 File Offset: 0x00097348
	protected override void ThreadFunction()
	{
		try
		{
			TTSObjReader ttsobjReader = new TTSObjReader();
			this.go = ttsobjReader.ConvertString(this.ModelString);
		}
		catch (Exception error)
		{
			base.SetError(error);
		}
	}

	// Token: 0x060015F2 RID: 5618 RVA: 0x000025B8 File Offset: 0x000007B8
	protected override void OnFinished()
	{
	}

	// Token: 0x060015F3 RID: 5619 RVA: 0x0009918C File Offset: 0x0009738C
	public override void Reset()
	{
		base.Reset();
		this.ModelString = null;
	}

	// Token: 0x04000C62 RID: 3170
	public string ModelString;

	// Token: 0x04000C63 RID: 3171
	public TTSGameObject go;
}
