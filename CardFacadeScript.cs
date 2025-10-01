using System;
using UnityEngine;

// Token: 0x020000AF RID: 175
public class CardFacadeScript : MonoBehaviour
{
	// Token: 0x060008A3 RID: 2211 RVA: 0x0003DA54 File Offset: 0x0003BC54
	private void Start()
	{
		if (!this.DeckThatSpawned.GetComponent<Renderer>().enabled)
		{
			UnityEngine.Object.Destroy(base.gameObject);
			return;
		}
		base.GetComponent<Renderer>().materials = this.DeckThatSpawned.GetComponent<Renderer>().materials;
		base.GetComponent<MeshFilter>().mesh = this.DeckThatSpawned.GetComponent<MeshFilter>().sharedMesh;
		this.YOffset = base.transform.position.y - this.DeckThatSpawned.transform.position.y;
		if (UnityEngine.Random.Range(-0.9f, 0.9f) < 0f)
		{
			this.ClockwiseMulti *= -1;
		}
		base.transform.localScale = new Vector3(this.DeckThatSpawned.transform.localScale.x, base.transform.localScale.y, this.DeckThatSpawned.transform.localScale.z);
	}

	// Token: 0x060008A4 RID: 2212 RVA: 0x0003DB50 File Offset: 0x0003BD50
	private void Update()
	{
		if (!this.DeckThatSpawned)
		{
			UnityEngine.Object.Destroy(base.gameObject);
			return;
		}
		base.transform.position = new Vector3(this.DeckThatSpawned.transform.position.x, this.DeckThatSpawned.transform.position.y + this.YOffset, this.DeckThatSpawned.transform.position.z);
		float num = 650f * (float)this.ClockwiseMulti * Time.deltaTime;
		this.TotalRot += Mathf.Abs(num);
		if (this.TotalRot > 180f)
		{
			UnityEngine.Object.Destroy(base.gameObject);
			return;
		}
		base.transform.RotateAround(base.transform.position, base.transform.up, num);
	}

	// Token: 0x04000636 RID: 1590
	public GameObject DeckThatSpawned;

	// Token: 0x04000637 RID: 1591
	private int ClockwiseMulti = 1;

	// Token: 0x04000638 RID: 1592
	private float YOffset;

	// Token: 0x04000639 RID: 1593
	private float TotalRot;
}
