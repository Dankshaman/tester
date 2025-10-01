using System;
using UnityEngine;

// Token: 0x0200009F RID: 159
public class ArrowScript : MonoBehaviour
{
	// Token: 0x0600082D RID: 2093 RVA: 0x00039BC8 File Offset: 0x00037DC8
	private void Start()
	{
		this._camera = HoverScript.MainCamera;
		this._ui = NetworkSingleton<NetworkUI>.Instance.GUIUIRoot.GetComponent<UIRoot>();
		this._startPos = base.transform.position;
		this._downPos = new Vector3(this._startPos.x, this._startPos.y - 0.5f, this._startPos.z);
		this._upPos = new Vector3(this._startPos.x, this._startPos.y + 0.5f, this._startPos.z);
		base.GetComponent<AudioSource>().PlayOneShot(this.ArrrowSpawnSound, 0.5f * SoundScript.GLOBAL_SOUND_MULTI);
		this._offScreenIndicator = new GameObject("OffScreen Indicator");
		this._offScreenIndicator.SetActive(false);
		this._offScreenIndicator.transform.parent = this._ui.transform;
		this._offScreenIndicator.layer = this._ui.gameObject.layer;
		this._offScreenIndicator.AddComponent<SpriteRenderer>().sprite = this.OffScreenSprite;
		this._offScreenIndicator.transform.localScale = new Vector3(25f, 25f, 25f);
		this._offScreenIndicator.GetComponent<Renderer>().material.color = base.GetComponent<Renderer>().material.color;
	}

	// Token: 0x0600082E RID: 2094 RVA: 0x00039D38 File Offset: 0x00037F38
	private void Update()
	{
		if (this._count > 6)
		{
			UnityEngine.Object.Destroy(base.gameObject);
			UnityEngine.Object.Destroy(this._offScreenIndicator);
			return;
		}
		if (this._bDown)
		{
			if ((double)base.transform.position.y - 0.01 >= (double)this._downPos.y)
			{
				this._moveCount++;
				base.transform.position = Vector3.Lerp(base.transform.position, this._downPos, 13f * Time.deltaTime);
			}
			else
			{
				this._bDown = false;
				this._count++;
			}
		}
		else if ((double)base.transform.position.y + 0.01 <= (double)this._upPos.y)
		{
			this._moveCount--;
			base.transform.position = Vector3.Lerp(base.transform.position, this._upPos, 13f * Time.deltaTime);
		}
		else
		{
			this._bDown = true;
		}
		base.transform.RotateAround(base.transform.position, Vector3.up, 150f * Time.deltaTime);
		this.UpdateOffScreenIndicator();
	}

	// Token: 0x0600082F RID: 2095 RVA: 0x00039E84 File Offset: 0x00038084
	private void UpdateOffScreenIndicator()
	{
		Vector3 vector = this._camera.WorldToViewportPoint(this._startPos);
		if (vector.x > 0f && vector.x < 1f && vector.y > 0f && vector.y < 1f && vector.z > 0f)
		{
			this._offScreenIndicator.SetActive(false);
			return;
		}
		this._offScreenIndicator.SetActive(true);
		Vector3 vector2 = NGUITools.screenSize;
		float num = vector2.x / vector2.y;
		float num2 = (float)this._ui.activeHeight / 800f;
		if (vector.z < 0f)
		{
			vector.y = -vector.y;
		}
		Vector3 vector3 = new Vector3(25f, 25f, 25f);
		if (vector.x <= 0f)
		{
			vector3 *= 1f / (Mathf.Abs(vector.x) + 1f);
		}
		else if (vector.x >= 1f)
		{
			vector3 *= 1f / vector.x;
		}
		if (vector.y <= 0f)
		{
			vector3 *= 1f / (Mathf.Abs(vector.y) + 1f);
		}
		else if (vector.y >= 1f)
		{
			vector3 *= 1f / vector.y;
		}
		vector3.x = Mathf.Clamp(vector3.x, 15f, 25f);
		vector3.y = Mathf.Clamp(vector3.y, 15f, 25f);
		vector3.z = Mathf.Clamp(vector3.z, 15f, 25f);
		vector.x = Mathf.Clamp(vector.x, 0.08f, 0.95f);
		vector.y = Mathf.Clamp(vector.y, 0.05f, 0.85f);
		Vector3 a = new Vector3((vector.x - 0.5f) * (800f * num * num2), (vector.y - 0.5f) * (800f * num2), 0f);
		Vector3 vector4 = this._startPos - this._camera.ViewportToWorldPoint(vector);
		float num3 = Mathf.Atan(vector4.x / vector4.z) * 180f / 3.1415927f;
		if (vector4.z < 0f)
		{
			if (vector4.x >= 0f)
			{
				num3 += 180f;
			}
			else
			{
				num3 -= 180f;
			}
		}
		num3 -= this._camera.transform.eulerAngles.y;
		this._offScreenIndicator.transform.eulerAngles = new Vector3(0f, 0f, -num3);
		this._offScreenIndicator.transform.localPosition = a + new Vector3(Mathf.Sin(num3 * 3.1415927f / 180f), Mathf.Cos(num3 * 3.1415927f / 180f), 0f) * (float)this._moveCount * 13f * 2f * Time.deltaTime;
		this._offScreenIndicator.transform.localScale = vector3;
	}

	// Token: 0x040005AE RID: 1454
	public AudioClip ArrrowSpawnSound;

	// Token: 0x040005AF RID: 1455
	public Sprite OffScreenSprite;

	// Token: 0x040005B0 RID: 1456
	private Camera _camera;

	// Token: 0x040005B1 RID: 1457
	private UIRoot _ui;

	// Token: 0x040005B2 RID: 1458
	private GameObject _offScreenIndicator;

	// Token: 0x040005B3 RID: 1459
	private Vector3 _startPos = Vector3.zero;

	// Token: 0x040005B4 RID: 1460
	private Vector3 _downPos = Vector3.zero;

	// Token: 0x040005B5 RID: 1461
	private Vector3 _upPos = Vector3.zero;

	// Token: 0x040005B6 RID: 1462
	private const float Speed = 13f;

	// Token: 0x040005B7 RID: 1463
	private int _count;

	// Token: 0x040005B8 RID: 1464
	private int _moveCount;

	// Token: 0x040005B9 RID: 1465
	private bool _bDown = true;

	// Token: 0x040005BA RID: 1466
	private const int Bounces = 6;
}
