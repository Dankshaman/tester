using System;
using UnityEngine;

// Token: 0x0200022E RID: 558
public class ShakeDetector : MonoBehaviour
{
	// Token: 0x06001BBC RID: 7100 RVA: 0x000BEC30 File Offset: 0x000BCE30
	private void Start()
	{
		this.TargetTransform = base.transform.parent;
		this.rigidbody = base.GetComponent<Rigidbody>();
		this.PointerScript = base.transform.root.GetComponent<Pointer>();
		this.networkTouch = this.TargetTransform.GetComponent<NetworkTouch>();
	}

	// Token: 0x06001BBD RID: 7101 RVA: 0x000BEC84 File Offset: 0x000BCE84
	private void FixedUpdate()
	{
		if (this.TargetTransform == null)
		{
			UnityEngine.Object.Destroy(this);
			return;
		}
		this.PointerScript.bShaking = false;
		Vector3 position = this.TargetTransform.position;
		position.y = 1f;
		if (this.PointerScript.tapping())
		{
			position.y -= 0.8333333f;
		}
		if (this.PointerScript.raising())
		{
			position.y += 0.8333333f;
		}
		this.rigidbody.AddForce((position - this.rigidbody.position) * Time.deltaTime * 20000f * 3f);
		if (this.shakeStart != 0f && Time.time > this.shakeStart + 1f)
		{
			this.shakeStart = 0f;
		}
		if (Vector3.Dot(position - this.rigidbody.position, this.rigidbody.velocity) < -3f)
		{
			if (this.shakeStart == 0f)
			{
				this.shakeStart = Time.time;
				this.shakeCount = 0;
			}
			int num = this.shakeCount;
			this.shakeCount = num + 1;
			if (num > ManagerPhysicsObject.ShakeThreshold)
			{
				this.shakeStart = 0f;
				this.shakeCount = 0;
				this.PointerScript.bShaking = true;
				if (!this.networkTouch)
				{
					this.PointerScript.Shake(-1);
					return;
				}
				this.PointerScript.Shake(this.networkTouch.id_);
			}
		}
	}

	// Token: 0x04001176 RID: 4470
	private Transform TargetTransform;

	// Token: 0x04001177 RID: 4471
	private Rigidbody rigidbody;

	// Token: 0x04001178 RID: 4472
	private Pointer PointerScript;

	// Token: 0x04001179 RID: 4473
	private NetworkTouch networkTouch;

	// Token: 0x0400117A RID: 4474
	private int shakeCount;

	// Token: 0x0400117B RID: 4475
	private float shakeStart;
}
