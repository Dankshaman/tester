using System;

namespace RTEditor
{
	// Token: 0x020003DD RID: 989
	public enum ObjectDeselectActionType
	{
		// Token: 0x04001EA6 RID: 7846
		ClearSelectionCall,
		// Token: 0x04001EA7 RID: 7847
		SetSelectedObjectsCall,
		// Token: 0x04001EA8 RID: 7848
		RemoveObjectFromSelectionCall,
		// Token: 0x04001EA9 RID: 7849
		ClearClickAir,
		// Token: 0x04001EAA RID: 7850
		ClickAlreadySelected,
		// Token: 0x04001EAB RID: 7851
		MultiDeselect,
		// Token: 0x04001EAC RID: 7852
		Undo,
		// Token: 0x04001EAD RID: 7853
		Redo,
		// Token: 0x04001EAE RID: 7854
		DeselectInactive,
		// Token: 0x04001EAF RID: 7855
		None
	}
}
