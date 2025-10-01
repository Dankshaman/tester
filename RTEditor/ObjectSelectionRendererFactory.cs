using System;

namespace RTEditor
{
	// Token: 0x020003DA RID: 986
	public static class ObjectSelectionRendererFactory
	{
		// Token: 0x06002DF5 RID: 11765 RVA: 0x0013DAD2 File Offset: 0x0013BCD2
		public static ObjectSelectionRenderer Create(ObjectSelectionRenderMode objectSelectionRenderMode)
		{
			if (objectSelectionRenderMode == ObjectSelectionRenderMode.SelectionBoxes)
			{
				return new SelectionBoxObjectSelectionRenderer();
			}
			return null;
		}
	}
}
