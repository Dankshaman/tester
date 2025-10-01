using System;

// Token: 0x02000106 RID: 262
public interface IPoolBufferObject
{
	// Token: 0x06000CC6 RID: 3270
	void Init(int bufferSize);

	// Token: 0x06000CC7 RID: 3271
	int GetBufferSize();

	// Token: 0x06000CC8 RID: 3272
	void Clear();

	// Token: 0x06000CC9 RID: 3273
	void Resize(int newSize);
}
