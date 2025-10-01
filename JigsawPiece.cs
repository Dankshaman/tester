using System;
using System.Collections;
using NewNet;
using UnityEngine;

// Token: 0x02000135 RID: 309
public class JigsawPiece : MonoBehaviour
{
	// Token: 0x06001030 RID: 4144 RVA: 0x0006F294 File Offset: 0x0006D494
	private void Start()
	{
		NetworkPhysicsObject component = base.GetComponent<NetworkPhysicsObject>();
		if (Network.isServer)
		{
			this.bCachedFreeze = component.IsLocked;
			component.IsLocked = true;
		}
		else
		{
			component.rigidbody.constraints = RigidbodyConstraints.None;
		}
		component.rigidbody.collisionDetectionMode = CollisionDetectionMode.Discrete;
	}

	// Token: 0x06001031 RID: 4145 RVA: 0x0006F2DC File Offset: 0x0006D4DC
	public void MovePieceIntoPosition()
	{
		Vector3 position = this.desiredPosition;
		position.y = UnityEngine.Random.Range(2f, 5f);
		this.MovePieceIntoPosition(position);
	}

	// Token: 0x06001032 RID: 4146 RVA: 0x0006F30D File Offset: 0x0006D50D
	public void MovePieceIntoPosition(Vector3 position)
	{
		this.MovePieceIntoPosition(position, new Vector3(0f, 180f, 0f));
	}

	// Token: 0x06001033 RID: 4147 RVA: 0x0006F32A File Offset: 0x0006D52A
	public void MovePieceIntoPosition(Vector3 position, Vector3 rotation)
	{
		this.MovePieceIntoPosition(position, rotation, UnityEngine.Random.Range(0f, 2f));
	}

	// Token: 0x06001034 RID: 4148 RVA: 0x0006F343 File Offset: 0x0006D543
	public void MovePieceIntoPosition(Vector3 position, Vector3 rotation, float delay)
	{
		base.StartCoroutine(this.DoMovePieceIntoPosition(position, rotation, delay));
	}

	// Token: 0x06001035 RID: 4149 RVA: 0x0006F358 File Offset: 0x0006D558
	public void MoveDelayRelativeToDistance(Vector3 origin)
	{
		float num = this.desiredPosition.x - origin.x;
		float num2 = this.desiredPosition.z - origin.z;
		float t = (num * num + num2 * num2) / CustomJigsawPuzzle.Instance.boardGrid.maxDistance;
		Vector3 position = this.desiredPosition;
		position.y = 2f;
		this.MovePieceIntoPosition(position, new Vector3(0f, 180f, 0f), Mathf.Lerp(0f, 1f, t));
	}

	// Token: 0x06001036 RID: 4150 RVA: 0x0006F3DF File Offset: 0x0006D5DF
	private IEnumerator DoMovePieceIntoPosition(Vector3 targetPosition, Vector3 rotation, float delay)
	{
		yield return new WaitForSeconds(delay);
		Vector3 position = base.transform.position;
		position.y = targetPosition.y;
		NetworkPhysicsObject npo = base.GetComponent<NetworkPhysicsObject>();
		npo.SetSmoothPosition(position, false, false, false, true, null, npo.IsLocked, false, null);
		npo.SetSmoothRotation(rotation, false, false, false, true, null, npo.IsLocked);
		while (npo.IsSmoothMoving)
		{
			yield return null;
		}
		npo.SetSmoothPosition(targetPosition, false, false, false, true, null, npo.IsLocked, false, null);
		yield break;
	}

	// Token: 0x06001037 RID: 4151 RVA: 0x0006F404 File Offset: 0x0006D604
	public bool IsInPosition()
	{
		NetworkPhysicsObject component = base.GetComponent<NetworkPhysicsObject>();
		if (!component || component.IsHeldBySomebody)
		{
			return false;
		}
		if (!Utilities.approxAngle(base.transform.rotation.eulerAngles.x, 0f, 10f))
		{
			return false;
		}
		if (!Utilities.approxAngle(base.transform.rotation.eulerAngles.y, 180f, 10f))
		{
			return false;
		}
		if (!Utilities.approxAngle(base.transform.rotation.eulerAngles.z, 0f, 10f))
		{
			return false;
		}
		Vector3 vector = CustomJigsawPuzzle.Instance.boardGrid.NearestPosition(base.transform.position);
		float num = CustomJigsawPuzzle.Instance.boardGrid.GridSize / 2f;
		return Mathf.Abs(vector.x - this.desiredPosition.x) < num && Mathf.Abs(vector.z - this.desiredPosition.z) < num;
	}

	// Token: 0x04000A4A RID: 2634
	private const float MAX_SOLVE_START_DELAY = 2f;

	// Token: 0x04000A4B RID: 2635
	private const float VICTORY_WAVE_DURATION = 1f;

	// Token: 0x04000A4C RID: 2636
	private const float SOLVE_MIN_HEIGHT = 2f;

	// Token: 0x04000A4D RID: 2637
	private const float SOLVE_MAX_HEIGHT = 5f;

	// Token: 0x04000A4E RID: 2638
	public bool bCachedFreeze;

	// Token: 0x04000A4F RID: 2639
	public Vector3 desiredPosition;

	// Token: 0x04000A50 RID: 2640
	public float radius = 1f;
}
