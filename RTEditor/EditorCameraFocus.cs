using System;
using UnityEngine;

namespace RTEditor
{
	// Token: 0x020003B9 RID: 953
	public static class EditorCameraFocus
	{
		// Token: 0x06002D24 RID: 11556 RVA: 0x0013B1D0 File Offset: 0x001393D0
		public static EditorCameraFocusOperationInfo GetFocusOperationInfo(Camera camera, EditorCameraFocusSettings focusSettings)
		{
			Bounds bounds = MonoSingletonBase<EditorObjectSelection>.Instance.GetWorldBox().ToBounds();
			float num = bounds.size.x;
			if (num < bounds.size.y)
			{
				num = bounds.size.y;
			}
			if (num < bounds.size.z)
			{
				num = bounds.size.z;
			}
			return new EditorCameraFocusOperationInfo
			{
				CameraDestinationPosition = bounds.center - camera.transform.forward * num * focusSettings.FocusDistanceScale,
				FocusPoint = bounds.center,
				OrthoCameraHalfVerticalSize = num * 0.5f * focusSettings.FocusDistanceScale
			};
		}
	}
}
