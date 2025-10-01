using System;
using System.Collections.Generic;
using NewNet;
using UnityEngine;

// Token: 0x020000A6 RID: 166
public class BoardGrid : MonoBehaviour
{
	// Token: 0x06000846 RID: 2118 RVA: 0x0003A667 File Offset: 0x00038867
	private void Awake()
	{
		this.CachedScale = base.transform.localScale;
		this.NPO = base.GetComponentInParent<NetworkPhysicsObject>();
		if (base.GetComponent<BoxCollider>())
		{
			UnityEngine.Object.Destroy(base.GetComponent<BoxCollider>());
		}
	}

	// Token: 0x06000847 RID: 2119 RVA: 0x0003A69E File Offset: 0x0003889E
	private void Start()
	{
		if (Network.isServer && this.NPO && !base.GetComponentInParent<DummyObject>())
		{
			this.CalculateGrid();
		}
	}

	// Token: 0x06000848 RID: 2120 RVA: 0x0003A6C8 File Offset: 0x000388C8
	public void CalculateGrid()
	{
		this.NPO.ResetBounds();
		this.xSize = this.CachedScale.x;
		this.zSize = this.CachedScale.z;
		base.transform.localScale = Vector3.one;
		this.xNum = (int)(this.xSize / this.GridSize);
		this.zNum = (int)(this.zSize / this.GridSize);
		this.xOffset = (float)this.xNum * this.GridSize / 2f;
		this.zOffset = (float)this.zNum * this.GridSize / 2f;
		this.xStart = (this.bGridOffset ? (this.GridSize / 2f) : 0f);
		this.zStart = (this.bGridOffset ? (this.GridSize / 2f) : 0f);
		List<LuaGameObjectScript.LuaSnapPointParameters> list = new List<LuaGameObjectScript.LuaSnapPointParameters>();
		for (float num = this.xStart; num < this.xSize; num += this.GridSize)
		{
			for (float num2 = this.zStart; num2 < this.zSize; num2 += this.GridSize)
			{
				Vector3 position = new Vector3(num - this.xOffset, 0f, num2 - this.zOffset);
				list.Add(new LuaGameObjectScript.LuaSnapPointParameters
				{
					position = position
				});
			}
		}
		NetworkSingleton<SnapPointManager>.Instance.SetSnapPoints(list, this.NPO.networkView);
		this.maxDistance = this.xSize * this.xSize + this.zSize * this.zSize;
	}

	// Token: 0x06000849 RID: 2121 RVA: 0x0003A851 File Offset: 0x00038A51
	public Vector3 PositionFromCoordinate(int x, int z)
	{
		return new Vector3((float)x * this.GridSize + this.xStart - this.xOffset, 0f, -((float)z * this.GridSize + this.zStart - this.zOffset));
	}

	// Token: 0x0600084A RID: 2122 RVA: 0x0003A88C File Offset: 0x00038A8C
	public Vector3 NearestPosition(Vector3 position)
	{
		Vector3 result = Vector3.zero;
		float num = 999999f;
		for (float num2 = this.xStart; num2 < this.xSize; num2 += this.GridSize)
		{
			for (float num3 = this.zStart; num3 < this.zSize; num3 += this.GridSize)
			{
				Vector3 vector = new Vector3(num2 - this.xOffset, 0f, num3 - this.zOffset);
				float num4 = vector.x - position.x;
				float num5 = vector.z - position.z;
				float num6 = num4 * num4 + num5 * num5;
				if (num6 < num)
				{
					num = num6;
					result = vector;
				}
			}
		}
		return result;
	}

	// Token: 0x040005C2 RID: 1474
	public float yLocalPos = -2f;

	// Token: 0x040005C3 RID: 1475
	public float GridSize = 1f;

	// Token: 0x040005C4 RID: 1476
	public bool bGridOffset = true;

	// Token: 0x040005C5 RID: 1477
	public float xOffset;

	// Token: 0x040005C6 RID: 1478
	public float zOffset;

	// Token: 0x040005C7 RID: 1479
	public float xStart;

	// Token: 0x040005C8 RID: 1480
	public float zStart;

	// Token: 0x040005C9 RID: 1481
	public float xSize;

	// Token: 0x040005CA RID: 1482
	public float zSize;

	// Token: 0x040005CB RID: 1483
	public int xNum;

	// Token: 0x040005CC RID: 1484
	public int zNum;

	// Token: 0x040005CD RID: 1485
	public float maxDistance;

	// Token: 0x040005CE RID: 1486
	private Vector3 CachedScale = Vector3.zero;

	// Token: 0x040005CF RID: 1487
	private NetworkPhysicsObject NPO;
}
