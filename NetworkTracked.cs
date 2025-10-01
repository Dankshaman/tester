using System;
using NewNet;
using UnityEngine;
using Valve.VR;

// Token: 0x020001C4 RID: 452
public class NetworkTracked : NetworkSync
{
	// Token: 0x170003D7 RID: 983
	// (get) Token: 0x06001783 RID: 6019 RVA: 0x000A0887 File Offset: 0x0009EA87
	// (set) Token: 0x06001784 RID: 6020 RVA: 0x000A088F File Offset: 0x0009EA8F
	[Sync(Permission.Owner, null, SerializationMethod.Default, false)]
	public TrackedType trackedType
	{
		get
		{
			return this.trackedType_;
		}
		set
		{
			if (value != this.trackedType_)
			{
				this.SetTrackedType(value);
				base.DirtySync("trackedType");
			}
		}
	}

	// Token: 0x170003D8 RID: 984
	// (get) Token: 0x06001785 RID: 6021 RVA: 0x000A08AC File Offset: 0x0009EAAC
	// (set) Token: 0x06001786 RID: 6022 RVA: 0x000A08B4 File Offset: 0x0009EAB4
	[Sync(Permission.Owner, null, SerializationMethod.Default, false)]
	public bool Laser
	{
		get
		{
			return this.laser;
		}
		set
		{
			if (value != this.laser)
			{
				this.SetLaser(value);
				base.DirtySync("Laser");
			}
		}
	}

	// Token: 0x170003D9 RID: 985
	// (get) Token: 0x06001787 RID: 6023 RVA: 0x000A08D1 File Offset: 0x0009EAD1
	// (set) Token: 0x06001788 RID: 6024 RVA: 0x000A08D9 File Offset: 0x0009EAD9
	[Sync(Permission.Owner, null, SerializationMethod.Default, false)]
	public Vector3 Scale
	{
		get
		{
			return this.scale;
		}
		set
		{
			if (value != this.scale)
			{
				this.SetScale(value);
				base.DirtySync("Scale");
			}
		}
	}

	// Token: 0x06001789 RID: 6025 RVA: 0x000A08FC File Offset: 0x0009EAFC
	private void SetTrackedType(TrackedType trackedTypeInt)
	{
		this.trackedType_ = trackedTypeInt;
		if (base.networkView.isMine)
		{
			this.laserPointer = this.trackedObject.GetComponent<VRLaserPointer>();
			return;
		}
		Color color = Colour.ColourFromLabel(base.transform.root.GetComponent<Pointer>().PointerColorLabel);
		bool flag = false;
		TrackedType trackedType = this.trackedType;
		if (trackedType != TrackedType.Headset)
		{
			if (trackedType == TrackedType.Controller)
			{
				this.SpawnedTracked = (GameObject)UnityEngine.Object.Instantiate(Resources.Load("VR_Controller_Generic"), Vector3.zero, Quaternion.identity);
				this.laserPointer = this.SpawnedTracked.GetComponent<VRLaserPointer>();
				this.laserPointer.HideLaser = true;
				this.laserPointer.color = color;
				flag = true;
			}
		}
		else
		{
			this.SpawnedTracked = (GameObject)UnityEngine.Object.Instantiate(Resources.Load("VR_HMD_Generic"), Vector3.zero, Quaternion.identity);
		}
		if (this.SpawnedTracked)
		{
			this.SpawnedTracked.AddComponent<VRAvatarDevice>();
			this.SpawnedTracked.transform.parent = base.transform;
			this.SpawnedTracked.transform.Reset();
			this.SpawnedTracked.GetComponent<VRAvatarDevice>().IsController = flag;
			if (this.trackedType == TrackedType.Headset)
			{
				NetworkSingleton<PlayerManager>.Instance.PlayerStateFromID(base.transform.root.GetComponent<Pointer>().ID).head = this.SpawnedTracked;
			}
			Renderer[] componentsInChildren = this.SpawnedTracked.GetComponentsInChildren<Renderer>();
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				Material material = componentsInChildren[i].material;
				material.shader = Shader.Find("Marmoset/Transparent/Diffuse IBL");
				material.mainTexture = null;
				if (VREnable.DISPLAY_VR_PERIPHERALS == VREnable.VRAvatarDisplay.All || (VREnable.DISPLAY_VR_PERIPHERALS == VREnable.VRAvatarDisplay.Hands && flag))
				{
					color.a = 0.33f;
				}
				else
				{
					color.a = 0f;
				}
				material.color = color;
			}
		}
	}

	// Token: 0x0600178A RID: 6026 RVA: 0x000A0ACB File Offset: 0x0009ECCB
	private void SetLaser(bool laser)
	{
		this.laser = laser;
	}

	// Token: 0x0600178B RID: 6027 RVA: 0x000A0AD4 File Offset: 0x0009ECD4
	private void SetScale(Vector3 scale)
	{
		this.scale = scale;
		if (this.SpawnedTracked)
		{
			this.SpawnedTracked.transform.localScale = scale;
		}
	}

	// Token: 0x0600178C RID: 6028 RVA: 0x000A0AFB File Offset: 0x0009ECFB
	public Vector3 GetPosition()
	{
		if (!this.Laser)
		{
			return base.transform.position;
		}
		if (this.laserPointer)
		{
			return this.laserPointer.HitPoint;
		}
		return Vector3.zero;
	}

	// Token: 0x04000D7A RID: 3450
	public SteamVR_TrackedObject trackedObject;

	// Token: 0x04000D7B RID: 3451
	private VRLaserPointer laserPointer;

	// Token: 0x04000D7C RID: 3452
	private GameObject SpawnedTracked;

	// Token: 0x04000D7D RID: 3453
	private TrackedType trackedType_ = TrackedType.None;

	// Token: 0x04000D7E RID: 3454
	private bool laser;

	// Token: 0x04000D7F RID: 3455
	private Vector3 scale = Vector3.one;
}
