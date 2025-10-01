using System;

// Token: 0x0200020F RID: 527
public class CustomPDFState : IEquatable<CustomPDFState>
{
	// Token: 0x06001AC8 RID: 6856 RVA: 0x000BABE8 File Offset: 0x000B8DE8
	public bool Equals(CustomPDFState other)
	{
		return other != null && (this == other || (this.PDFUrl == other.PDFUrl && this.PDFPassword == other.PDFPassword && this.PDFPage == other.PDFPage && this.PDFPageOffset == other.PDFPageOffset));
	}

	// Token: 0x06001AC9 RID: 6857 RVA: 0x000BAC44 File Offset: 0x000B8E44
	public override bool Equals(object obj)
	{
		return obj != null && (this == obj || (!(obj.GetType() != base.GetType()) && this.Equals((CustomPDFState)obj)));
	}

	// Token: 0x06001ACA RID: 6858 RVA: 0x000BAC74 File Offset: 0x000B8E74
	public override int GetHashCode()
	{
		return ((((this.PDFUrl != null) ? this.PDFUrl.GetHashCode() : 0) * 397 ^ ((this.PDFPassword != null) ? this.PDFPassword.GetHashCode() : 0)) * 397 ^ this.PDFPage) * 397 ^ this.PDFPageOffset;
	}

	// Token: 0x06001ACB RID: 6859 RVA: 0x000B7302 File Offset: 0x000B5502
	public static bool operator ==(CustomPDFState left, CustomPDFState right)
	{
		return object.Equals(left, right);
	}

	// Token: 0x06001ACC RID: 6860 RVA: 0x000B730B File Offset: 0x000B550B
	public static bool operator !=(CustomPDFState left, CustomPDFState right)
	{
		return !object.Equals(left, right);
	}

	// Token: 0x040010FE RID: 4350
	[Tag(TagType.URL)]
	public string PDFUrl = "";

	// Token: 0x040010FF RID: 4351
	public string PDFPassword = "";

	// Token: 0x04001100 RID: 4352
	public int PDFPage;

	// Token: 0x04001101 RID: 4353
	public int PDFPageOffset;
}
