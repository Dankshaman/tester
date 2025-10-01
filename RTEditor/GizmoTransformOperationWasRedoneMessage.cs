using System;

namespace RTEditor
{
	// Token: 0x02000437 RID: 1079
	public class GizmoTransformOperationWasRedoneMessage : Message
	{
		// Token: 0x17000690 RID: 1680
		// (get) Token: 0x060031BF RID: 12735 RVA: 0x00150F77 File Offset: 0x0014F177
		public Gizmo GizmoInvolvedInTransformOperation
		{
			get
			{
				return this._gizmoInvolvedInTransformOperation;
			}
		}

		// Token: 0x060031C0 RID: 12736 RVA: 0x00150F7F File Offset: 0x0014F17F
		public GizmoTransformOperationWasRedoneMessage(Gizmo gizmoInvolvedInTransformOperation) : base(MessageType.GizmoTransformOperationWasRedone)
		{
			this._gizmoInvolvedInTransformOperation = gizmoInvolvedInTransformOperation;
		}

		// Token: 0x060031C1 RID: 12737 RVA: 0x00150F90 File Offset: 0x0014F190
		public static void SendToInterestedListeners(Gizmo gizmoInvolvedInTransformOperation)
		{
			GizmoTransformOperationWasRedoneMessage message = new GizmoTransformOperationWasRedoneMessage(gizmoInvolvedInTransformOperation);
			MonoSingletonBase<MessageListenerDatabase>.Instance.SendMessageToInterestedListeners(message);
		}

		// Token: 0x0400201F RID: 8223
		private Gizmo _gizmoInvolvedInTransformOperation;
	}
}
