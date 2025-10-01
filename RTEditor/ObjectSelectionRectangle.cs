using System;
using System.Collections.Generic;
using UnityEngine;

namespace RTEditor
{
	// Token: 0x020003DB RID: 987
	[Serializable]
	public class ObjectSelectionRectangle : ObjectSelectionShape
	{
		// Token: 0x17000593 RID: 1427
		// (get) Token: 0x06002DF6 RID: 11766 RVA: 0x0013DADE File Offset: 0x0013BCDE
		public ObjectSelectionRectangleRenderSettings RenderSettings
		{
			get
			{
				return this._renderSettings;
			}
		}

		// Token: 0x06002DF7 RID: 11767 RVA: 0x0013DAE8 File Offset: 0x0013BCE8
		public override List<GameObject> GetIntersectingGameObjects(List<GameObject> gameObjects, Camera camera)
		{
			if (!base.IsEnclosingRectangleBigEnoughForSelection())
			{
				return new List<GameObject>();
			}
			List<GameObject> list = new List<GameObject>();
			foreach (GameObject gameObject in gameObjects)
			{
				if (this._enclosingRectangle.Overlaps(gameObject.GetScreenRectangle(camera), true))
				{
					list.Add(gameObject);
				}
			}
			return list;
		}

		// Token: 0x06002DF8 RID: 11768 RVA: 0x0013DB60 File Offset: 0x0013BD60
		public override void Render()
		{
			if (this._isVisible)
			{
				GLPrimitives.Draw2DFilledRectangle(this._enclosingRectangle, this._renderSettings.FillColor, SingletonBase<MaterialPool>.Instance.Geometry2D, MonoSingletonBase<EditorCamera>.Instance.Camera);
				GLPrimitives.Draw2DRectangleBorderLines(this._enclosingRectangle, this._renderSettings.BorderLineColor, SingletonBase<MaterialPool>.Instance.GLLine, MonoSingletonBase<EditorCamera>.Instance.Camera);
			}
		}

		// Token: 0x04001EA2 RID: 7842
		[SerializeField]
		private ObjectSelectionRectangleRenderSettings _renderSettings = new ObjectSelectionRectangleRenderSettings();
	}
}
