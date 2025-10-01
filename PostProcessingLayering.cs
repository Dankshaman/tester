using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

// Token: 0x02000096 RID: 150
public class PostProcessingLayering : MonoBehaviour
{
	// Token: 0x17000199 RID: 409
	// (get) Token: 0x06000803 RID: 2051 RVA: 0x0003835F File Offset: 0x0003655F
	// (set) Token: 0x06000804 RID: 2052 RVA: 0x00038367 File Offset: 0x00036567
	public Camera camera { get; private set; }

	// Token: 0x1700019A RID: 410
	// (get) Token: 0x06000805 RID: 2053 RVA: 0x00038370 File Offset: 0x00036570
	// (set) Token: 0x06000806 RID: 2054 RVA: 0x00038378 File Offset: 0x00036578
	public PostProcessLayer postProcessing { get; private set; }

	// Token: 0x06000807 RID: 2055 RVA: 0x00038381 File Offset: 0x00036581
	private void Awake()
	{
		PostProcessingLayering.Layers.Add(this);
		this.camera = base.GetComponent<Camera>();
		this.postProcessing = base.GetComponent<PostProcessLayer>();
	}

	// Token: 0x06000808 RID: 2056 RVA: 0x000383A6 File Offset: 0x000365A6
	private void OnDestroy()
	{
		PostProcessingLayering.Layers.Remove(this);
	}

	// Token: 0x06000809 RID: 2057 RVA: 0x000383B4 File Offset: 0x000365B4
	private void OnEnable()
	{
		this.UpdateLayers(false);
	}

	// Token: 0x0600080A RID: 2058 RVA: 0x000383BD File Offset: 0x000365BD
	private void OnDisable()
	{
		this.UpdateLayers(true);
	}

	// Token: 0x0600080B RID: 2059 RVA: 0x000383C8 File Offset: 0x000365C8
	private void UpdateLayers(bool excludeThis)
	{
		float num = -1f;
		PostProcessingLayering y = null;
		for (int i = 0; i < PostProcessingLayering.Layers.Count; i++)
		{
			PostProcessingLayering postProcessingLayering = PostProcessingLayering.Layers[i];
			if ((!excludeThis || !(postProcessingLayering == this)) && postProcessingLayering.camera.enabled && postProcessingLayering.camera.gameObject.activeSelf)
			{
				if (postProcessingLayering.camera.depth > num)
				{
					num = postProcessingLayering.camera.depth;
					y = postProcessingLayering;
				}
				this.postProcessing.enabled = (this.camera.depth >= postProcessingLayering.camera.depth && !TableScript.PURE_MODE);
			}
		}
		for (int j = 0; j < PostProcessingLayering.Layers.Count; j++)
		{
			PostProcessingLayering postProcessingLayering2 = PostProcessingLayering.Layers[j];
			postProcessingLayering2.postProcessing.enabled = (postProcessingLayering2 == y && !TableScript.PURE_MODE);
		}
	}

	// Token: 0x04000590 RID: 1424
	public static List<PostProcessingLayering> Layers = new List<PostProcessingLayering>();
}
