using System;
using NewNet;
using TouchScript;
using UnityEngine;
using Valve.VR;

// Token: 0x020001E1 RID: 481
public class PointerSyncs : MonoBehaviour
{
	// Token: 0x060019CD RID: 6605 RVA: 0x000B4A99 File Offset: 0x000B2C99
	private void Start()
	{
		this.NetworkTouches = base.GetComponentsInChildren<NetworkTouch>(true);
		this.NetworkTrackeds = base.GetComponentsInChildren<NetworkTracked>(true);
		this.PointerNetworkView = base.transform.root.GetComponent<NetworkView>();
	}

	// Token: 0x060019CE RID: 6606 RVA: 0x000B4ACC File Offset: 0x000B2CCC
	public void SetTouchPosition(int index, Vector3 Pos, int id)
	{
		if (index >= this.NetworkTouches.Length - 1)
		{
			return;
		}
		NetworkTouch networkTouch = this.NetworkTouches[index];
		if (!networkTouch.gameObject.activeSelf)
		{
			networkTouch.gameObject.SetActive(true);
		}
		networkTouch.id_ = id;
		networkTouch.transform.position = Pos;
		networkTouch.RefreshSetInactive();
	}

	// Token: 0x060019CF RID: 6607 RVA: 0x000B4B24 File Offset: 0x000B2D24
	public void SetTrackedTransform(Vector3 Pos, Quaternion Rot, float Scale, TrackedType trackedType, SteamVR_TrackedObject trackedObject, bool Laser, int id)
	{
		NetworkTracked networkTracked = null;
		for (int i = 0; i < this.NetworkTrackeds.Length; i++)
		{
			if (this.NetworkTrackeds[i].id_ == id || this.NetworkTrackeds[i].id_ == -1)
			{
				networkTracked = this.NetworkTrackeds[i];
			}
		}
		if (!networkTracked)
		{
			return;
		}
		if (!networkTracked.gameObject.activeSelf)
		{
			networkTracked.gameObject.SetActive(true);
		}
		networkTracked.id_ = id;
		networkTracked.transform.position = Pos;
		networkTracked.transform.rotation = Rot;
		networkTracked.trackedObject = trackedObject;
		networkTracked.trackedType = trackedType;
		networkTracked.Scale = new Vector3(Scale, Scale, Scale);
		networkTracked.Laser = Laser;
		networkTracked.RefreshSetInactive();
	}

	// Token: 0x060019D0 RID: 6608 RVA: 0x000B4BE0 File Offset: 0x000B2DE0
	public Vector3 PositionFromID(int id)
	{
		for (int i = 0; i < this.NetworkTouches.Length; i++)
		{
			if (this.NetworkTouches[i].id_ == id)
			{
				return this.NetworkTouches[i].transform.position;
			}
		}
		for (int j = 0; j < this.NetworkTrackeds.Length; j++)
		{
			if (this.NetworkTrackeds[j].id_ == id)
			{
				return this.NetworkTrackeds[j].GetPosition();
			}
		}
		if (this.PointerNetworkView.isMine)
		{
			for (int k = 0; k < TouchManager.Instance.NumberOfTouches; k++)
			{
				if (TouchManager.Instance.ActiveTouches[k].Id == id)
				{
					return HoverScript.GetWorldPositionFromScreenPos(TouchManager.Instance.ActiveTouches[k].Position);
				}
			}
		}
		return Vector3.zero;
	}

	// Token: 0x060019D1 RID: 6609 RVA: 0x000B4CB0 File Offset: 0x000B2EB0
	public Quaternion RotationFromID(int id)
	{
		for (int i = 0; i < this.NetworkTouches.Length; i++)
		{
			if (this.NetworkTouches[i].id_ == id)
			{
				return Quaternion.identity;
			}
		}
		int j = 0;
		while (j < this.NetworkTrackeds.Length)
		{
			if (this.NetworkTrackeds[j].id_ == id)
			{
				if (this.NetworkTrackeds[j].Laser)
				{
					return Quaternion.identity;
				}
				return this.NetworkTrackeds[j].transform.rotation;
			}
			else
			{
				j++;
			}
		}
		return Quaternion.identity;
	}

	// Token: 0x060019D2 RID: 6610 RVA: 0x000B4D38 File Offset: 0x000B2F38
	public NetworkTracked NetworkTrackedFromID(int id)
	{
		for (int i = 0; i < this.NetworkTrackeds.Length; i++)
		{
			if (this.NetworkTrackeds[i].id_ == id)
			{
				return this.NetworkTrackeds[i];
			}
		}
		return null;
	}

	// Token: 0x04000F8C RID: 3980
	public NetworkTouch[] NetworkTouches;

	// Token: 0x04000F8D RID: 3981
	public NetworkTracked[] NetworkTrackeds;

	// Token: 0x04000F8E RID: 3982
	private NetworkView PointerNetworkView;
}
