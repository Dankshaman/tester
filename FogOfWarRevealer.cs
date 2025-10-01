using System;
using System.Collections.Generic;
using NewNet;
using UnityEngine;

// Token: 0x02000112 RID: 274
public class FogOfWarRevealer : NetworkBehavior
{
	// Token: 0x17000218 RID: 536
	// (get) Token: 0x06000DF2 RID: 3570 RVA: 0x00059B0C File Offset: 0x00057D0C
	// (set) Token: 0x06000DF3 RID: 3571 RVA: 0x00059B14 File Offset: 0x00057D14
	[Sync(Permission.Client, null, SerializationMethod.Default, false)]
	public bool Active
	{
		get
		{
			return this._active;
		}
		set
		{
			if (value == this._active)
			{
				return;
			}
			this._active = value;
			base.DirtySync("Active");
			if (value)
			{
				EventManager.TriggerFogOfWarRevealerAdd(base.gameObject);
				return;
			}
			EventManager.TriggerFogOfWarRevealerDestroy(base.gameObject);
		}
	}

	// Token: 0x17000219 RID: 537
	// (get) Token: 0x06000DF4 RID: 3572 RVA: 0x00059B4C File Offset: 0x00057D4C
	// (set) Token: 0x06000DF5 RID: 3573 RVA: 0x00059B54 File Offset: 0x00057D54
	[Sync(Permission.Client, null, SerializationMethod.Default, false)]
	public string Color
	{
		get
		{
			return this._color;
		}
		set
		{
			if (value == this._color)
			{
				return;
			}
			this._color = ((value == "Black") ? "All" : value);
			this.HideOutline();
			base.DirtySync("Color");
		}
	}

	// Token: 0x1700021A RID: 538
	// (get) Token: 0x06000DF6 RID: 3574 RVA: 0x00059B94 File Offset: 0x00057D94
	public Color AColor
	{
		get
		{
			Colour colour;
			if (Colour.TryColourFromLabel(this._color, out colour))
			{
				return colour;
			}
			return UnityEngine.Color.black;
		}
	}

	// Token: 0x1700021B RID: 539
	// (get) Token: 0x06000DF7 RID: 3575 RVA: 0x00059BBC File Offset: 0x00057DBC
	// (set) Token: 0x06000DF8 RID: 3576 RVA: 0x00059BC4 File Offset: 0x00057DC4
	[Sync(Permission.Client, null, SerializationMethod.Default, false)]
	public float Range
	{
		get
		{
			return this._range;
		}
		set
		{
			if ((double)Math.Abs(value - this._range) <= 0.001)
			{
				return;
			}
			this._range = value;
			base.DirtySync("Range");
		}
	}

	// Token: 0x1700021C RID: 540
	// (get) Token: 0x06000DF9 RID: 3577 RVA: 0x00059BF2 File Offset: 0x00057DF2
	// (set) Token: 0x06000DFA RID: 3578 RVA: 0x00059BFA File Offset: 0x00057DFA
	[Sync(Permission.Client, null, SerializationMethod.Default, false)]
	public float Height
	{
		get
		{
			return this._height;
		}
		set
		{
			if ((double)Math.Abs(value - this._height) <= 0.001)
			{
				return;
			}
			this._height = value;
			base.DirtySync("Height");
		}
	}

	// Token: 0x1700021D RID: 541
	// (get) Token: 0x06000DFB RID: 3579 RVA: 0x00059C28 File Offset: 0x00057E28
	// (set) Token: 0x06000DFC RID: 3580 RVA: 0x00059C30 File Offset: 0x00057E30
	[Sync(Permission.Client, null, SerializationMethod.Default, false)]
	public float FoV
	{
		get
		{
			return this._fov;
		}
		set
		{
			if ((double)Math.Abs(value - this._fov) <= 0.001)
			{
				return;
			}
			this._fov = value;
			base.DirtySync("FoV");
		}
	}

	// Token: 0x1700021E RID: 542
	// (get) Token: 0x06000DFD RID: 3581 RVA: 0x00059C5E File Offset: 0x00057E5E
	// (set) Token: 0x06000DFE RID: 3582 RVA: 0x00059C66 File Offset: 0x00057E66
	[Sync(Permission.Client, null, SerializationMethod.Default, false)]
	public float FoVOffset
	{
		get
		{
			return this._fovOffset;
		}
		set
		{
			if ((double)Math.Abs(value - this._fovOffset) <= 0.001)
			{
				return;
			}
			this._fovOffset = value;
			base.DirtySync("FoVOffset");
		}
	}

	// Token: 0x1700021F RID: 543
	// (get) Token: 0x06000DFF RID: 3583 RVA: 0x00059C94 File Offset: 0x00057E94
	// (set) Token: 0x06000E00 RID: 3584 RVA: 0x00059C9C File Offset: 0x00057E9C
	[Sync(Permission.Client, null, SerializationMethod.Default, false)]
	public bool ShowFoWOutline
	{
		get
		{
			return this._showFoWOutline;
		}
		set
		{
			if (value == this._showFoWOutline)
			{
				return;
			}
			if (!value)
			{
				this.HideOutline();
			}
			this._showFoWOutline = value;
			base.DirtySync("ShowFoWOutline");
		}
	}

	// Token: 0x17000220 RID: 544
	// (get) Token: 0x06000E01 RID: 3585 RVA: 0x00059CC3 File Offset: 0x00057EC3
	// (set) Token: 0x06000E02 RID: 3586 RVA: 0x00059CCB File Offset: 0x00057ECB
	public LineRenderer LineRenderer { get; private set; }

	// Token: 0x06000E03 RID: 3587 RVA: 0x00059CD4 File Offset: 0x00057ED4
	private void LazyLineRenderer()
	{
		if (this.LineRenderer != null)
		{
			return;
		}
		this.LineRenderer = base.gameObject.GetComponent<LineRenderer>();
		if (this.LineRenderer != null)
		{
			return;
		}
		this.LineRenderer = base.gameObject.AddComponent<LineRenderer>();
		this.LineRenderer.widthMultiplier = 0.1f;
	}

	// Token: 0x06000E04 RID: 3588 RVA: 0x00059D34 File Offset: 0x00057F34
	public void ShowOutline(List<Vector3> polygon, Vector3 position, string color)
	{
		this.LazyLineRenderer();
		if (color == "All")
		{
			color = "Grey";
		}
		this.LineRenderer.enabled = true;
		this.LineRenderer.material = new Material(Shader.Find("Sprites/Default"))
		{
			color = Colour.ColourFromLabel(color)
		};
		this.LineRenderer.positionCount = polygon.Count;
		for (int i = 0; i < polygon.Count; i++)
		{
			this.LineRenderer.SetPosition(i, new Vector3(polygon[i].x, position.y, polygon[i].z));
		}
	}

	// Token: 0x06000E05 RID: 3589 RVA: 0x00059DE3 File Offset: 0x00057FE3
	public void HideOutline()
	{
		if (this.LineRenderer == null)
		{
			return;
		}
		this.LineRenderer.enabled = false;
	}

	// Token: 0x04000914 RID: 2324
	private bool _active;

	// Token: 0x04000915 RID: 2325
	private string _color = "All";

	// Token: 0x04000916 RID: 2326
	private float _range = 5f;

	// Token: 0x04000917 RID: 2327
	private float _height = 1.2f;

	// Token: 0x04000918 RID: 2328
	private float _fov = 360f;

	// Token: 0x04000919 RID: 2329
	private float _fovOffset;

	// Token: 0x0400091A RID: 2330
	private bool _showFoWOutline;
}
