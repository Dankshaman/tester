using System;

namespace RTEditor
{
	// Token: 0x0200043B RID: 1083
	public interface IRTEditorEventListener
	{
		// Token: 0x060031C7 RID: 12743
		bool OnCanBeSelected(ObjectSelectEventArgs selectEventArgs);

		// Token: 0x060031C8 RID: 12744
		void OnSelected(ObjectSelectEventArgs selectEventArgs);

		// Token: 0x060031C9 RID: 12745
		void OnDeselected(ObjectDeselectEventArgs deselectEventArgs);

		// Token: 0x060031CA RID: 12746
		void OnAlteredByTransformGizmo(Gizmo gizmo);
	}
}
