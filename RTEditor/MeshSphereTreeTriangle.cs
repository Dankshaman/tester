using System;

namespace RTEditor
{
	// Token: 0x0200044D RID: 1101
	public class MeshSphereTreeTriangle
	{
		// Token: 0x170006CE RID: 1742
		// (get) Token: 0x0600325B RID: 12891 RVA: 0x0015357F File Offset: 0x0015177F
		public int TriangleIndex
		{
			get
			{
				return this._triangleIndex;
			}
		}

		// Token: 0x0600325C RID: 12892 RVA: 0x00153587 File Offset: 0x00151787
		public MeshSphereTreeTriangle(int triangleIndex)
		{
			this._triangleIndex = triangleIndex;
		}

		// Token: 0x0400206A RID: 8298
		private int _triangleIndex;
	}
}
