using System;

namespace RTEditor
{
	// Token: 0x020003D5 RID: 981
	public static class ObjectSelectionBoxRendererFactory
	{
		// Token: 0x06002DEE RID: 11758 RVA: 0x0013DA1A File Offset: 0x0013BC1A
		public static ObjectSelectionBoxRenderer CreateObjectSelectionBoxDrawer(ObjectSelectionBoxStyle objectSelectionBoxStyle)
		{
			if (objectSelectionBoxStyle == ObjectSelectionBoxStyle.CornerLines)
			{
				return new CornerLinesObjectSelectionBoxRenderer();
			}
			if (objectSelectionBoxStyle != ObjectSelectionBoxStyle.WireBox)
			{
				return null;
			}
			return new WireObjectSelectionBoxRenderer();
		}
	}
}
