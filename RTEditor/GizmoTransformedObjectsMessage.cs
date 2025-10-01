using System;

namespace RTEditor
{
	// Token: 0x02000435 RID: 1077
	public class GizmoTransformedObjectsMessage : Message
	{
		// Token: 0x1700068E RID: 1678
		// (get) Token: 0x060031B9 RID: 12729 RVA: 0x00150F06 File Offset: 0x0014F106
		public Gizmo Gizmo
		{
			get
			{
				return this._gizmo;
			}
		}

		// Token: 0x060031BA RID: 12730 RVA: 0x00150F0E File Offset: 0x0014F10E
		public GizmoTransformedObjectsMessage(Gizmo gizmo) : base(MessageType.GizmoTransformedObjects)
		{
			this._gizmo = gizmo;
		}

		// Token: 0x060031BB RID: 12731 RVA: 0x00150F20 File Offset: 0x0014F120
		public static void SendToInterestedListeners(Gizmo gizmo)
		{
			GizmoTransformedObjectsMessage message = new GizmoTransformedObjectsMessage(gizmo);
			MonoSingletonBase<MessageListenerDatabase>.Instance.SendMessageToInterestedListeners(message);
		}

		// Token: 0x0400201D RID: 8221
		private Gizmo _gizmo;
	}
}
