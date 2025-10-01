using System;
using MoonSharp.Interpreter;
using UnityEngine;

// Token: 0x0200018E RID: 398
public class LuaGrid
{
	// Token: 0x1700034C RID: 844
	// (get) Token: 0x060013A4 RID: 5028 RVA: 0x00083181 File Offset: 0x00081381
	// (set) Token: 0x060013A5 RID: 5029 RVA: 0x0008318D File Offset: 0x0008138D
	[MoonSharpHidden]
	private GridState gridState
	{
		get
		{
			return NetworkSingleton<GridOptions>.Instance.gridState;
		}
		set
		{
			NetworkSingleton<GridOptions>.Instance.gridState = value;
		}
	}

	// Token: 0x1700034D RID: 845
	// (get) Token: 0x060013A6 RID: 5030 RVA: 0x0008319A File Offset: 0x0008139A
	// (set) Token: 0x060013A7 RID: 5031 RVA: 0x000831AC File Offset: 0x000813AC
	public int type
	{
		get
		{
			return (int)(this.gridState.Type + 1);
		}
		set
		{
			this.gridState = new GridState(this.gridState, new GridType?((GridType)(value - 1)), null, null, null, null, null, null, null, null, null, null);
		}
	}

	// Token: 0x1700034E RID: 846
	// (get) Token: 0x060013A8 RID: 5032 RVA: 0x0008322C File Offset: 0x0008142C
	// (set) Token: 0x060013A9 RID: 5033 RVA: 0x0008323C File Offset: 0x0008143C
	public bool show_lines
	{
		get
		{
			return this.gridState.Lines;
		}
		set
		{
			this.gridState = new GridState(this.gridState, null, new bool?(value), null, null, null, null, null, null, null, null, null);
		}
	}

	// Token: 0x1700034F RID: 847
	// (get) Token: 0x060013AA RID: 5034 RVA: 0x000832BB File Offset: 0x000814BB
	// (set) Token: 0x060013AB RID: 5035 RVA: 0x000832D4 File Offset: 0x000814D4
	public Color color
	{
		get
		{
			return this.gridState.Color.ToColour();
		}
		set
		{
			this.gridState = new GridState(this.gridState, null, null, new ColourState?(new ColourState(value)), null, null, null, null, null, null, null, null);
		}
	}

	// Token: 0x17000350 RID: 848
	// (get) Token: 0x060013AC RID: 5036 RVA: 0x0008335C File Offset: 0x0008155C
	// (set) Token: 0x060013AD RID: 5037 RVA: 0x0008336C File Offset: 0x0008156C
	public float opacity
	{
		get
		{
			return this.gridState.Opacity;
		}
		set
		{
			this.gridState = new GridState(this.gridState, null, null, null, new float?(value), null, null, null, null, null, null, null);
		}
	}

	// Token: 0x17000351 RID: 849
	// (get) Token: 0x060013AE RID: 5038 RVA: 0x000833EB File Offset: 0x000815EB
	// (set) Token: 0x060013AF RID: 5039 RVA: 0x000833F8 File Offset: 0x000815F8
	public bool thick_lines
	{
		get
		{
			return this.gridState.ThickLines;
		}
		set
		{
			this.gridState = new GridState(this.gridState, null, null, null, null, new bool?(value), null, null, null, null, null, null);
		}
	}

	// Token: 0x17000352 RID: 850
	// (get) Token: 0x060013B0 RID: 5040 RVA: 0x00083477 File Offset: 0x00081677
	// (set) Token: 0x060013B1 RID: 5041 RVA: 0x000834A8 File Offset: 0x000816A8
	public int snapping
	{
		get
		{
			if (this.gridState.Snapping)
			{
				return 2;
			}
			if (this.gridState.Offset)
			{
				return 3;
			}
			if (this.gridState.BothSnapping)
			{
				return 4;
			}
			return 1;
		}
		set
		{
			switch (value)
			{
			case 1:
				this.gridState = new GridState(this.gridState, null, null, null, null, null, new bool?(false), new bool?(false), new bool?(false), null, null, null);
				return;
			case 2:
				this.gridState = new GridState(this.gridState, null, null, null, null, null, new bool?(true), new bool?(false), new bool?(false), null, null, null);
				return;
			case 3:
				this.gridState = new GridState(this.gridState, null, null, null, null, null, new bool?(false), new bool?(true), new bool?(false), null, null, null);
				return;
			case 4:
				this.gridState = new GridState(this.gridState, null, null, null, null, null, new bool?(false), new bool?(false), new bool?(true), null, null, null);
				return;
			default:
				return;
			}
		}
	}

	// Token: 0x17000353 RID: 851
	// (get) Token: 0x060013B2 RID: 5042 RVA: 0x00083681 File Offset: 0x00081881
	// (set) Token: 0x060013B3 RID: 5043 RVA: 0x00083694 File Offset: 0x00081894
	public float offsetX
	{
		get
		{
			return this.gridState.PosOffset.x;
		}
		set
		{
			this.gridState = new GridState(this.gridState, null, null, null, null, null, null, null, null, null, null, new VectorState?(new VectorState(value, this.gridState.PosOffset.y, this.gridState.PosOffset.z)));
		}
	}

	// Token: 0x17000354 RID: 852
	// (get) Token: 0x060013B4 RID: 5044 RVA: 0x00083737 File Offset: 0x00081937
	// (set) Token: 0x060013B5 RID: 5045 RVA: 0x0008374C File Offset: 0x0008194C
	public float offsetY
	{
		get
		{
			return this.gridState.PosOffset.z;
		}
		set
		{
			this.gridState = new GridState(this.gridState, null, null, null, null, null, null, null, null, null, null, new VectorState?(new VectorState(this.gridState.PosOffset.x, this.gridState.PosOffset.y, value)));
		}
	}

	// Token: 0x17000355 RID: 853
	// (get) Token: 0x060013B6 RID: 5046 RVA: 0x000837EF File Offset: 0x000819EF
	// (set) Token: 0x060013B7 RID: 5047 RVA: 0x000837FC File Offset: 0x000819FC
	public float sizeX
	{
		get
		{
			return this.gridState.xSize;
		}
		set
		{
			this.gridState = new GridState(this.gridState, null, null, null, null, null, null, null, null, new float?(value), null, null);
		}
	}

	// Token: 0x17000356 RID: 854
	// (get) Token: 0x060013B8 RID: 5048 RVA: 0x0008387B File Offset: 0x00081A7B
	// (set) Token: 0x060013B9 RID: 5049 RVA: 0x00083888 File Offset: 0x00081A88
	public float sizeY
	{
		get
		{
			return this.gridState.ySize;
		}
		set
		{
			this.gridState = new GridState(this.gridState, null, null, null, null, null, null, null, null, null, new float?(value), null);
		}
	}
}
