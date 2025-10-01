using System;

namespace RTEditor
{
	// Token: 0x02000436 RID: 1078
	public class GizmoTransformOperationWasUndoneMessage : Message
	{
		// Token: 0x1700068F RID: 1679
		// (get) Token: 0x060031BC RID: 12732 RVA: 0x00150F3F File Offset: 0x0014F13F
		public Gizmo GizmoInvolvedInTransformOperation
		{
			get
			{
				return this._gizmoInvolvedInTransformOperation;
			}
		}

		// Token: 0x060031BD RID: 12733 RVA: 0x00150F47 File Offset: 0x0014F147
		public GizmoTransformOperationWasUndoneMessage(Gizmo gizmoInvolvedInTransformOperation) : base(MessageType.GizmoTransformOperationWasUndone)
		{
			this._gizmoInvolvedInTransformOperation = gizmoInvolvedInTransformOperation;
		}

		// Token: 0x060031BE RID: 12734 RVA: 0x00150F58 File Offset: 0x0014F158
		public static void SendToInterestedListeners(Gizmo gizmoInvolvedInTransformOperation)
		{
			GizmoTransformOperationWasUndoneMessage message = new GizmoTransformOperationWasUndoneMessage(gizmoInvolvedInTransformOperation);
			MonoSingletonBase<MessageListenerDatabase>.Instance.SendMessageToInterestedListeners(message);
		}

		// Token: 0x0400201E RID: 8222
		private Gizmo _gizmoInvolvedInTransformOperation;
	}
}
