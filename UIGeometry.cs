using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200006A RID: 106
public class UIGeometry
{
	// Token: 0x1700008E RID: 142
	// (get) Token: 0x06000487 RID: 1159 RVA: 0x00021FCD File Offset: 0x000201CD
	public bool hasVertices
	{
		get
		{
			return this.verts.Count > 0;
		}
	}

	// Token: 0x1700008F RID: 143
	// (get) Token: 0x06000488 RID: 1160 RVA: 0x00021FDD File Offset: 0x000201DD
	public bool hasTransformed
	{
		get
		{
			return this.mRtpVerts != null && this.mRtpVerts.Count > 0 && this.mRtpVerts.Count == this.verts.Count;
		}
	}

	// Token: 0x06000489 RID: 1161 RVA: 0x0002200F File Offset: 0x0002020F
	public void Clear()
	{
		this.verts.Clear();
		this.uvs.Clear();
		this.cols.Clear();
		this.mRtpVerts.Clear();
	}

	// Token: 0x0600048A RID: 1162 RVA: 0x00022040 File Offset: 0x00020240
	public void ApplyTransform(Matrix4x4 widgetToPanel, bool generateNormals = true)
	{
		if (this.verts.Count > 0)
		{
			this.mRtpVerts.Clear();
			int i = 0;
			int count = this.verts.Count;
			while (i < count)
			{
				this.mRtpVerts.Add(widgetToPanel.MultiplyPoint3x4(this.verts[i]));
				i++;
			}
			if (generateNormals)
			{
				this.mRtpNormal = widgetToPanel.MultiplyVector(Vector3.back).normalized;
				Vector3 normalized = widgetToPanel.MultiplyVector(Vector3.right).normalized;
				this.mRtpTan = new Vector4(normalized.x, normalized.y, normalized.z, -1f);
				return;
			}
		}
		else
		{
			this.mRtpVerts.Clear();
		}
	}

	// Token: 0x0600048B RID: 1163 RVA: 0x00022100 File Offset: 0x00020300
	public void WriteToBuffers(List<Vector3> v, List<Vector2> u, List<Color> c, List<Vector3> n, List<Vector4> t, List<Vector4> u2)
	{
		if (this.mRtpVerts != null && this.mRtpVerts.Count > 0)
		{
			if (n == null)
			{
				int i = 0;
				int count = this.mRtpVerts.Count;
				while (i < count)
				{
					v.Add(this.mRtpVerts[i]);
					u.Add(this.uvs[i]);
					c.Add(this.cols[i]);
					i++;
				}
			}
			else
			{
				int j = 0;
				int count2 = this.mRtpVerts.Count;
				while (j < count2)
				{
					v.Add(this.mRtpVerts[j]);
					u.Add(this.uvs[j]);
					c.Add(this.cols[j]);
					n.Add(this.mRtpNormal);
					t.Add(this.mRtpTan);
					j++;
				}
			}
			if (u2 != null)
			{
				Vector4 zero = Vector4.zero;
				int k = 0;
				int count3 = this.verts.Count;
				while (k < count3)
				{
					zero.x = this.verts[k].x;
					zero.y = this.verts[k].y;
					u2.Add(zero);
					k++;
				}
			}
			if (this.onCustomWrite != null)
			{
				this.onCustomWrite(v, u, c, n, t, u2);
			}
		}
	}

	// Token: 0x0400033B RID: 827
	public List<Vector3> verts = new List<Vector3>();

	// Token: 0x0400033C RID: 828
	public List<Vector2> uvs = new List<Vector2>();

	// Token: 0x0400033D RID: 829
	public List<Color> cols = new List<Color>();

	// Token: 0x0400033E RID: 830
	public UIGeometry.OnCustomWrite onCustomWrite;

	// Token: 0x0400033F RID: 831
	private List<Vector3> mRtpVerts = new List<Vector3>();

	// Token: 0x04000340 RID: 832
	private Vector3 mRtpNormal;

	// Token: 0x04000341 RID: 833
	private Vector4 mRtpTan;

	// Token: 0x0200053E RID: 1342
	// (Invoke) Token: 0x060037BC RID: 14268
	public delegate void OnCustomWrite(List<Vector3> v, List<Vector2> u, List<Color> c, List<Vector3> n, List<Vector4> t, List<Vector4> u2);
}
