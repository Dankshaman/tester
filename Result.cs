using System;

// Token: 0x02000149 RID: 329
public class Result
{
	// Token: 0x170002F0 RID: 752
	// (get) Token: 0x060010EA RID: 4330 RVA: 0x00075483 File Offset: 0x00073683
	public string message { get; }

	// Token: 0x060010EB RID: 4331 RVA: 0x0007548B File Offset: 0x0007368B
	public Result(string message)
	{
		this.message = message;
	}

	// Token: 0x060010EC RID: 4332 RVA: 0x0007549A File Offset: 0x0007369A
	public static implicit operator bool(Result result)
	{
		return result.message == "";
	}

	// Token: 0x060010ED RID: 4333 RVA: 0x000754AC File Offset: 0x000736AC
	public static implicit operator string(Result result)
	{
		return result.message;
	}
}
