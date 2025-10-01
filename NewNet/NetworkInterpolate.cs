using System;
using UnityEngine;

namespace NewNet
{
	// Token: 0x020003A9 RID: 937
	public class NetworkInterpolate : NetworkBehavior
	{
		// Token: 0x06002C1F RID: 11295 RVA: 0x00135660 File Offset: 0x00133860
		private void Awake()
		{
			this.Cache();
		}

		// Token: 0x06002C20 RID: 11296 RVA: 0x00135668 File Offset: 0x00133868
		private void Cache()
		{
			if (this.target == null)
			{
				this.target = base.transform;
			}
			if (this.syncTarget == NetworkInterpolate.SyncTarget.Rigidbody && this.targetRigidbody == null)
			{
				this.targetRigidbody = this.target.GetComponent<Rigidbody>();
			}
		}

		// Token: 0x06002C21 RID: 11297 RVA: 0x001356B8 File Offset: 0x001338B8
		public void ReceiveUnreliable(BitStream stream, uint packetId)
		{
			Vector3 position = this.GetPosition();
			Vector3 eulerAngles = this.GetRotation().eulerAngles;
			this.ReadUnreliable(stream, ref position.x, this.positionSync.x, NetworkInterpolate.SyncTransform.position);
			this.ReadUnreliable(stream, ref position.y, this.positionSync.y, NetworkInterpolate.SyncTransform.position);
			this.ReadUnreliable(stream, ref position.z, this.positionSync.z, NetworkInterpolate.SyncTransform.position);
			this.ReadUnreliable(stream, ref eulerAngles.x, this.rotationSync.x, NetworkInterpolate.SyncTransform.rotation);
			this.ReadUnreliable(stream, ref eulerAngles.y, this.rotationSync.y, NetworkInterpolate.SyncTransform.rotation);
			this.ReadUnreliable(stream, ref eulerAngles.z, this.rotationSync.z, NetworkInterpolate.SyncTransform.rotation);
			this.Interpolate(position, Quaternion.Euler(eulerAngles), packetId);
		}

		// Token: 0x06002C22 RID: 11298 RVA: 0x00135788 File Offset: 0x00133988
		public void ReceiveReliable(BitStream stream, uint packetId)
		{
			Vector3 position = this.GetPosition();
			Vector3 eulerAngles = this.GetRotation().eulerAngles;
			this.ReadReliable(stream, ref position.x, this.positionSync.x);
			this.ReadReliable(stream, ref position.y, this.positionSync.y);
			this.ReadReliable(stream, ref position.z, this.positionSync.z);
			this.ReadReliable(stream, ref eulerAngles.x, this.rotationSync.x);
			this.ReadReliable(stream, ref eulerAngles.y, this.rotationSync.y);
			this.ReadReliable(stream, ref eulerAngles.z, this.rotationSync.z);
			this.Interpolate(position, Quaternion.Euler(eulerAngles), packetId);
		}

		// Token: 0x06002C23 RID: 11299 RVA: 0x00135850 File Offset: 0x00133A50
		public bool CheckChange(BitStream stream)
		{
			bool flag = false;
			Vector3 position = this.GetPosition();
			Vector3 eulerAngles = this.GetRotation().eulerAngles;
			if (position != this.prevPosition)
			{
				flag = true;
			}
			else if (eulerAngles != this.prevRotation)
			{
				flag = true;
			}
			if (flag)
			{
				this.prevPosition = position;
				this.prevRotation = eulerAngles;
				stream.WriteUshort(base.networkView.id);
				this.WriteUnreliable(stream, position.x, this.positionSync.x, NetworkInterpolate.SyncTransform.position);
				this.WriteUnreliable(stream, position.y, this.positionSync.y, NetworkInterpolate.SyncTransform.position);
				this.WriteUnreliable(stream, position.z, this.positionSync.z, NetworkInterpolate.SyncTransform.position);
				this.WriteUnreliable(stream, eulerAngles.x, this.rotationSync.x, NetworkInterpolate.SyncTransform.rotation);
				this.WriteUnreliable(stream, eulerAngles.y, this.rotationSync.y, NetworkInterpolate.SyncTransform.rotation);
				this.WriteUnreliable(stream, eulerAngles.z, this.rotationSync.z, NetworkInterpolate.SyncTransform.rotation);
			}
			return flag;
		}

		// Token: 0x06002C24 RID: 11300 RVA: 0x00135954 File Offset: 0x00133B54
		public bool CheckFinishMove(BitStream stream)
		{
			if (!this.reliableSyncWhenFinishMoving)
			{
				return false;
			}
			bool flag = false;
			Vector3 position = this.GetPosition();
			Vector3 eulerAngles = this.GetRotation().eulerAngles;
			if (this.syncTarget == NetworkInterpolate.SyncTarget.Rigidbody)
			{
				bool flag2 = this.targetRigidbody.IsSleeping();
				if (flag2 && !this.isSleeping)
				{
					flag = true;
				}
				this.isSleeping = flag2;
			}
			if (flag)
			{
				stream.WriteUshort(base.networkView.id);
				this.WriteReliable(stream, position.x, this.positionSync.x);
				this.WriteReliable(stream, position.y, this.positionSync.y);
				this.WriteReliable(stream, position.z, this.positionSync.z);
				this.WriteReliable(stream, eulerAngles.x, this.rotationSync.x);
				this.WriteReliable(stream, eulerAngles.y, this.rotationSync.y);
				this.WriteReliable(stream, eulerAngles.z, this.rotationSync.z);
			}
			return flag;
		}

		// Token: 0x06002C25 RID: 11301 RVA: 0x00135A57 File Offset: 0x00133C57
		private void WriteUnreliable(BitStream stream, float value, NetworkInterpolate.SyncType type, NetworkInterpolate.SyncTransform transform)
		{
			if (type != NetworkInterpolate.SyncType.Lossy)
			{
				if (type == NetworkInterpolate.SyncType.Full)
				{
					stream.WriteFloat(value);
					return;
				}
			}
			else
			{
				if (transform == NetworkInterpolate.SyncTransform.position)
				{
					stream.WritePositionLossyCoordinate(value);
					return;
				}
				if (transform != NetworkInterpolate.SyncTransform.rotation)
				{
					return;
				}
				stream.WriteRotationLossyCoordinate(value);
			}
		}

		// Token: 0x06002C26 RID: 11302 RVA: 0x00135A82 File Offset: 0x00133C82
		private void ReadUnreliable(BitStream stream, ref float value, NetworkInterpolate.SyncType type, NetworkInterpolate.SyncTransform transform)
		{
			if (type != NetworkInterpolate.SyncType.Lossy)
			{
				if (type == NetworkInterpolate.SyncType.Full)
				{
					value = stream.ReadFloat();
					return;
				}
			}
			else
			{
				if (transform == NetworkInterpolate.SyncTransform.position)
				{
					value = stream.ReadPositionLossyCoordinate();
					return;
				}
				if (transform != NetworkInterpolate.SyncTransform.rotation)
				{
					return;
				}
				value = stream.ReadRotationLossyCoordinate();
			}
		}

		// Token: 0x06002C27 RID: 11303 RVA: 0x00135AB0 File Offset: 0x00133CB0
		private void WriteReliable(BitStream stream, float value, NetworkInterpolate.SyncType type)
		{
			if (type - NetworkInterpolate.SyncType.Lossy <= 1)
			{
				stream.WriteFloat(value);
			}
		}

		// Token: 0x06002C28 RID: 11304 RVA: 0x00135ABF File Offset: 0x00133CBF
		private void ReadReliable(BitStream stream, ref float value, NetworkInterpolate.SyncType type)
		{
			if (type - NetworkInterpolate.SyncType.Lossy <= 1)
			{
				value = stream.ReadFloat();
			}
		}

		// Token: 0x06002C29 RID: 11305 RVA: 0x00135AD0 File Offset: 0x00133CD0
		private void Update()
		{
			if (this.interpolating)
			{
				float num = (Time.time - this.interpolateStartTime + this.interpolateOffsetTime) / (this.InterpolationTime + this.interpolateOffsetTime);
				if (num >= 1f)
				{
					this.interpolating = false;
				}
				num = Mathf.Min(num, 1f);
				this.SetPositionAndRotation(Vector3.Lerp(this.interpolateStartPos, this.interpolateEndPos, num), Quaternion.Slerp(this.interpolateStartRot, this.interpolateEndRot, num));
			}
		}

		// Token: 0x06002C2A RID: 11306 RVA: 0x00135B4C File Offset: 0x00133D4C
		private Vector3 GetPosition()
		{
			if (this.syncTarget == NetworkInterpolate.SyncTarget.Rigidbody)
			{
				return this.targetRigidbody.position;
			}
			return this.target.position;
		}

		// Token: 0x06002C2B RID: 11307 RVA: 0x00135B6E File Offset: 0x00133D6E
		private Quaternion GetRotation()
		{
			if (this.syncTarget == NetworkInterpolate.SyncTarget.Rigidbody)
			{
				return this.targetRigidbody.rotation;
			}
			return this.target.rotation;
		}

		// Token: 0x06002C2C RID: 11308 RVA: 0x00135B90 File Offset: 0x00133D90
		private void SetPositionAndRotation(Vector3 position, Quaternion rotation)
		{
			if (this.syncTarget == NetworkInterpolate.SyncTarget.Rigidbody)
			{
				this.targetRigidbody.position = position;
				this.targetRigidbody.rotation = rotation;
				return;
			}
			this.target.SetPositionAndRotation(position, rotation);
		}

		// Token: 0x17000520 RID: 1312
		// (get) Token: 0x06002C2D RID: 11309 RVA: 0x00135BC1 File Offset: 0x00133DC1
		private float InterpolationTime
		{
			get
			{
				return 0.016f * NetworkInterpolate.InterpolateMulti;
			}
		}

		// Token: 0x17000521 RID: 1313
		// (get) Token: 0x06002C2E RID: 11310 RVA: 0x00006128 File Offset: 0x00004328
		// (set) Token: 0x06002C2F RID: 11311 RVA: 0x00006130 File Offset: 0x00004330
		private bool interpolating
		{
			get
			{
				return base.enabled;
			}
			set
			{
				base.enabled = value;
			}
		}

		// Token: 0x06002C30 RID: 11312 RVA: 0x00135BD0 File Offset: 0x00133DD0
		public void Interpolate(Vector3 position, Quaternion rotation, uint packetId)
		{
			if (packetId < this.interpolatePackedId)
			{
				return;
			}
			this.interpolatePackedId = packetId;
			if (this.InterpolationTime == 0f || Vector3.Distance(this.GetPosition(), position) > 20f)
			{
				this.interpolating = false;
				this.SetPositionAndRotation(position, rotation);
				return;
			}
			this.interpolating = true;
			this.interpolateStartTime = Time.time;
			this.interpolateOffsetTime = Time.deltaTime;
			this.interpolateStartPos = this.GetPosition();
			this.interpolateEndPos = position;
			this.interpolateStartRot = this.GetRotation();
			this.interpolateEndRot = rotation;
		}

		// Token: 0x04001DBF RID: 7615
		[NonSerialized]
		public bool reliableSyncWhenFinishMoving = true;

		// Token: 0x04001DC0 RID: 7616
		[NonSerialized]
		public NetworkInterpolate.SyncTarget syncTarget = NetworkInterpolate.SyncTarget.Rigidbody;

		// Token: 0x04001DC1 RID: 7617
		[NonSerialized]
		public NetworkInterpolate.SyncVector positionSync = new NetworkInterpolate.SyncVector();

		// Token: 0x04001DC2 RID: 7618
		[NonSerialized]
		public NetworkInterpolate.SyncVector rotationSync = new NetworkInterpolate.SyncVector();

		// Token: 0x04001DC3 RID: 7619
		private Vector3 prevPosition = Vector3.zero;

		// Token: 0x04001DC4 RID: 7620
		private Vector3 prevRotation = Vector3.zero;

		// Token: 0x04001DC5 RID: 7621
		private bool isSleeping = true;

		// Token: 0x04001DC6 RID: 7622
		private Transform target;

		// Token: 0x04001DC7 RID: 7623
		private Rigidbody targetRigidbody;

		// Token: 0x04001DC8 RID: 7624
		public static float InterpolateMulti = 1.1f;

		// Token: 0x04001DC9 RID: 7625
		private uint interpolatePackedId;

		// Token: 0x04001DCA RID: 7626
		private float interpolateStartTime;

		// Token: 0x04001DCB RID: 7627
		private float interpolateOffsetTime;

		// Token: 0x04001DCC RID: 7628
		private Vector3 interpolateStartPos;

		// Token: 0x04001DCD RID: 7629
		private Vector3 interpolateEndPos;

		// Token: 0x04001DCE RID: 7630
		private Quaternion interpolateStartRot;

		// Token: 0x04001DCF RID: 7631
		private Quaternion interpolateEndRot;

		// Token: 0x020007D8 RID: 2008
		public enum SyncType
		{
			// Token: 0x04002D71 RID: 11633
			None,
			// Token: 0x04002D72 RID: 11634
			Lossy,
			// Token: 0x04002D73 RID: 11635
			Full
		}

		// Token: 0x020007D9 RID: 2009
		public enum SyncTransform
		{
			// Token: 0x04002D75 RID: 11637
			position,
			// Token: 0x04002D76 RID: 11638
			rotation
		}

		// Token: 0x020007DA RID: 2010
		public enum SyncTarget
		{
			// Token: 0x04002D78 RID: 11640
			Transform,
			// Token: 0x04002D79 RID: 11641
			Rigidbody
		}

		// Token: 0x020007DB RID: 2011
		[Serializable]
		public class SyncVector
		{
			// Token: 0x04002D7A RID: 11642
			public NetworkInterpolate.SyncType x;

			// Token: 0x04002D7B RID: 11643
			public NetworkInterpolate.SyncType y;

			// Token: 0x04002D7C RID: 11644
			public NetworkInterpolate.SyncType z;
		}
	}
}
