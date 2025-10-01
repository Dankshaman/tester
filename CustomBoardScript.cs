using System;
using System.Collections;
using UnityEngine;

// Token: 0x020000CA RID: 202
public class CustomBoardScript : MonoBehaviour
{
	// Token: 0x170001BF RID: 447
	// (get) Token: 0x06000A23 RID: 2595 RVA: 0x00047F31 File Offset: 0x00046131
	// (set) Token: 0x06000A24 RID: 2596 RVA: 0x00047F39 File Offset: 0x00046139
	public float xMulti
	{
		get
		{
			return this._xMulti;
		}
		set
		{
			if (this._xMulti != value)
			{
				this._xMulti = value;
				this.SetScale();
			}
		}
	}

	// Token: 0x06000A25 RID: 2597 RVA: 0x00047F51 File Offset: 0x00046151
	private void Awake()
	{
		this.NPO = base.GetComponent<NetworkPhysicsObject>();
		this.SetScale();
	}

	// Token: 0x06000A26 RID: 2598 RVA: 0x00047F65 File Offset: 0x00046165
	private void Start()
	{
		this.SetScale();
		EventManager.OnChangeTable += this.ChangeTable;
	}

	// Token: 0x06000A27 RID: 2599 RVA: 0x00047F7E File Offset: 0x0004617E
	private void OnDestroy()
	{
		EventManager.OnChangeTable -= this.ChangeTable;
	}

	// Token: 0x06000A28 RID: 2600 RVA: 0x00047F94 File Offset: 0x00046194
	private void SetScale()
	{
		string internalName = NetworkSingleton<ManagerPhysicsObject>.Instance.TableNPO.InternalName;
		if (internalName == "Table_RPG")
		{
			base.transform.localScale = this.AdjustVector(this.LargeTableScale);
			return;
		}
		if (internalName == "Table_Custom")
		{
			base.transform.localScale = this.AdjustVector(this.ExtraLargeTableScale);
			return;
		}
		if (!(internalName == "Table_Circular"))
		{
			base.transform.localScale = this.AdjustVector(Vector3.one);
			return;
		}
		base.transform.localScale = this.AdjustVector(this.MediumTableScale);
	}

	// Token: 0x06000A29 RID: 2601 RVA: 0x00048038 File Offset: 0x00046238
	private void ChangeTable(GameObject Table)
	{
		this.StartCheck();
	}

	// Token: 0x06000A2A RID: 2602 RVA: 0x00048040 File Offset: 0x00046240
	public void StartCheck()
	{
		base.StartCoroutine(this.CheckScale());
	}

	// Token: 0x06000A2B RID: 2603 RVA: 0x0004804F File Offset: 0x0004624F
	private IEnumerator CheckScale()
	{
		for (;;)
		{
			if (NetworkSingleton<ManagerPhysicsObject>.Instance.TableNPO.InternalName == "Table_RPG")
			{
				if (!(base.transform.localScale != this.AdjustVector(this.LargeTableScale)))
				{
					break;
				}
				base.transform.localScale = Vector3.Lerp(base.transform.localScale, this.AdjustVector(this.LargeTableScale), Time.deltaTime);
				yield return null;
			}
			else if (NetworkSingleton<ManagerPhysicsObject>.Instance.TableNPO.InternalName == "Table_Custom")
			{
				if (!(base.transform.localScale != this.AdjustVector(this.ExtraLargeTableScale)))
				{
					break;
				}
				base.transform.localScale = Vector3.Lerp(base.transform.localScale, this.AdjustVector(this.ExtraLargeTableScale), Time.deltaTime);
				yield return null;
			}
			else if (NetworkSingleton<ManagerPhysicsObject>.Instance.TableNPO.InternalName == "Table_Circular")
			{
				if (!(base.transform.localScale != this.AdjustVector(this.MediumTableScale)))
				{
					break;
				}
				base.transform.localScale = Vector3.Lerp(base.transform.localScale, this.AdjustVector(this.MediumTableScale), Time.deltaTime);
				yield return null;
			}
			else
			{
				if (!(base.transform.localScale != this.AdjustVector(Vector3.one)))
				{
					break;
				}
				base.transform.localScale = Vector3.Lerp(base.transform.localScale, this.AdjustVector(Vector3.one), Time.deltaTime);
				yield return null;
			}
		}
		yield break;
	}

	// Token: 0x06000A2C RID: 2604 RVA: 0x00048060 File Offset: 0x00046260
	private Vector3 AdjustVector(Vector3 v)
	{
		Vector3 scale = this.NPO.Scale;
		return new Vector3(v.x * this.xMulti * scale.x, v.y * this.yMulti * scale.y, v.z * this.zMulti * scale.z);
	}

	// Token: 0x0400072B RID: 1835
	private NetworkPhysicsObject NPO;

	// Token: 0x0400072C RID: 1836
	private Vector3 ExtraLargeTableScale = new Vector3(1.6f, 1.6f, 1.6f);

	// Token: 0x0400072D RID: 1837
	private Vector3 LargeTableScale = new Vector3(1.4f, 1.4f, 1.4f);

	// Token: 0x0400072E RID: 1838
	private Vector3 MediumTableScale = new Vector3(1.2f, 1.2f, 1.2f);

	// Token: 0x0400072F RID: 1839
	private float _xMulti = 1f;

	// Token: 0x04000730 RID: 1840
	private float zMulti = 1f;

	// Token: 0x04000731 RID: 1841
	private float yMulti = 1f;
}
