using System;
using NewNet;
using UnityEngine;

// Token: 0x0200011E RID: 286
public class GridOptions : NetworkSingleton<GridOptions>
{
	// Token: 0x170002C4 RID: 708
	// (get) Token: 0x06000F1C RID: 3868 RVA: 0x0006702D File Offset: 0x0006522D
	// (set) Token: 0x06000F1D RID: 3869 RVA: 0x00067035 File Offset: 0x00065235
	[Sync(Permission.Admin, null, SerializationMethod.Default, false)]
	public GridState gridState
	{
		get
		{
			return this._gridState;
		}
		set
		{
			this._gridState = value;
			this.UpdateGridOverlay();
			base.DirtySync("gridState");
		}
	}

	// Token: 0x06000F1E RID: 3870 RVA: 0x00067050 File Offset: 0x00065250
	protected override void Awake()
	{
		base.Awake();
		this.BoxMaterial = new Material(this.BoxMaterial);
		this.HexMaterial = new Material(this.HexMaterial);
		this.HexRotatedMaterial = new Material(this.HexRotatedMaterial);
		this.BoxMaterialThick = new Material(this.BoxMaterialThick);
		this.HexMaterialThick = new Material(this.HexMaterialThick);
		this.HexRotatedMaterialThick = new Material(this.HexRotatedMaterialThick);
	}

	// Token: 0x06000F1F RID: 3871 RVA: 0x000670CC File Offset: 0x000652CC
	private void UpdateGridOverlay()
	{
		this.GridProjector.gameObject.SetActive(this.gridState.Lines && this.gridState.Opacity > 0f);
		if (this.GridProjector.gameObject.activeSelf && this.gridState.xSize != 0f && this.gridState.ySize != 0f)
		{
			switch (this.gridState.Type)
			{
			case GridType.Box:
				if (this.gridState.ThickLines)
				{
					this.GridProjector.material = this.BoxMaterialThick;
				}
				else
				{
					this.GridProjector.material = this.BoxMaterial;
				}
				this.GridProjector.orthographicSize = 500f * this.gridState.ySize;
				this.GridProjector.aspectRatio = this.gridState.xSize / this.gridState.ySize;
				break;
			case GridType.HexHorizontal:
				if (this.gridState.ThickLines)
				{
					this.GridProjector.material = this.HexMaterialThick;
				}
				else
				{
					this.GridProjector.material = this.HexMaterial;
				}
				this.GridProjector.orthographicSize = 500f * this.gridState.ySize;
				this.GridProjector.aspectRatio = this.gridState.xSize / this.gridState.ySize * 0.8654f;
				break;
			case GridType.HexVertical:
				if (this.gridState.ThickLines)
				{
					this.GridProjector.material = this.HexRotatedMaterialThick;
				}
				else
				{
					this.GridProjector.material = this.HexRotatedMaterial;
				}
				this.GridProjector.orthographicSize = 500f * this.gridState.ySize * 0.8654f;
				this.GridProjector.aspectRatio = this.gridState.xSize / this.gridState.ySize / 0.8654f;
				break;
			}
			this.GridProjector.material.color = new Color(this.gridState.Color.r, this.gridState.Color.g, this.gridState.Color.b, this.gridState.Opacity);
			this.GridProjector.transform.position = new Vector3(this.gridState.PosOffset.x, this.GridProjector.transform.position.y, this.gridState.PosOffset.z);
		}
	}

	// Token: 0x06000F20 RID: 3872 RVA: 0x00067370 File Offset: 0x00065570
	private float calculateYsize(float y)
	{
		switch (this.gridState.Type)
		{
		default:
			return y;
		case GridType.HexHorizontal:
			return y;
		case GridType.HexVertical:
			return y;
		}
	}

	// Token: 0x06000F21 RID: 3873 RVA: 0x000673A0 File Offset: 0x000655A0
	public void UpdateSettingsFromSelectedNPOs(NetworkPhysicsObject firstNPO, NetworkPhysicsObject secondNPO)
	{
		if (!firstNPO)
		{
			return;
		}
		Vector3 position = firstNPO.transform.position;
		if (!secondNPO)
		{
			float num = position.x % this.gridState.xSize;
			float num2 = position.z % this.gridState.ySize;
			if (this.gridState.Offset)
			{
				num -= this.gridState.xSize * 0.5f;
				num2 -= this.gridState.ySize * 0.5f;
			}
			this.gridState = new GridState(this.gridState, null, null, null, null, null, null, null, null, null, null, new VectorState?(new VectorState(num, this.gridState.PosOffset.y, num2)));
			return;
		}
		Vector3 position2 = secondNPO.transform.position;
		float num3 = position2.x - position.x;
		float num4 = this.calculateYsize(position2.x - position.x);
		float x = position.x % num3;
		float z = position.z % num4;
		GridState gridState = this.gridState;
		GridType? type = null;
		bool? lines = null;
		ColourState? color = null;
		float? opacity = null;
		bool? thickLines = null;
		bool? snapping = null;
		bool? offset = null;
		bool? bothSnapping = null;
		VectorState? posOffset = new VectorState?(new VectorState(x, this.gridState.PosOffset.y, z));
		this.gridState = new GridState(gridState, type, lines, color, opacity, thickLines, snapping, offset, bothSnapping, new float?(num3), new float?(num4), posOffset);
	}

	// Token: 0x06000F22 RID: 3874 RVA: 0x00067594 File Offset: 0x00065794
	public Vector3 ScaledVector(Vector3 vector, out float x, out float y, out float z)
	{
		x = (y = (z = 0f));
		if (this.gridState.xSize == 0f || this.gridState.ySize == 0f)
		{
			return vector;
		}
		if (this.gridState.Type == GridType.HexHorizontal)
		{
			x = vector.x / this.gridState.xSize * 4f / 3f;
			z = vector.z / this.gridState.ySize / 0.8654f;
			y = Mathf.Min(x, z);
			return new Vector3(vector.x / this.gridState.xSize, vector.y, vector.z / this.gridState.ySize) / 0.8654f;
		}
		if (this.gridState.Type == GridType.HexVertical)
		{
			x = vector.x / this.gridState.xSize / 0.8654f;
			z = vector.z / this.gridState.ySize * 4f / 3f;
			y = Mathf.Min(x, z);
			return new Vector3(vector.x / this.gridState.xSize, vector.y, vector.z / this.gridState.ySize) / 0.8654f;
		}
		x = vector.x / this.gridState.xSize;
		z = vector.z / this.gridState.ySize;
		y = Mathf.Min(x, z);
		return new Vector3(vector.x / this.gridState.xSize, vector.y, vector.z / this.gridState.ySize);
	}

	// Token: 0x06000F23 RID: 3875 RVA: 0x00067761 File Offset: 0x00065961
	public bool Hexagonal()
	{
		return this.gridState.Type == GridType.HexHorizontal || this.gridState.Type == GridType.HexVertical;
	}

	// Token: 0x06000F24 RID: 3876 RVA: 0x00067784 File Offset: 0x00065984
	public void Reset()
	{
		GridState gridState = new GridState();
		if (gridState != this.gridState)
		{
			this.gridState = gridState;
		}
	}

	// Token: 0x0400095D RID: 2397
	public Projector GridProjector;

	// Token: 0x0400095E RID: 2398
	public Material BoxMaterial;

	// Token: 0x0400095F RID: 2399
	public Material HexMaterial;

	// Token: 0x04000960 RID: 2400
	public Material HexRotatedMaterial;

	// Token: 0x04000961 RID: 2401
	public Material BoxMaterialThick;

	// Token: 0x04000962 RID: 2402
	public Material HexMaterialThick;

	// Token: 0x04000963 RID: 2403
	public Material HexRotatedMaterialThick;

	// Token: 0x04000964 RID: 2404
	private const float ORTHO_MULTI = 500f;

	// Token: 0x04000965 RID: 2405
	private const float HEX_RATIO = 0.8654f;

	// Token: 0x04000966 RID: 2406
	private GridState _gridState = new GridState();
}
