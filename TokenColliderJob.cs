using System;
using UnityEngine;

// Token: 0x0200025B RID: 603
public class TokenColliderJob : ThreadedJob
{
	// Token: 0x06001FBD RID: 8125 RVA: 0x000E254C File Offset: 0x000E074C
	protected override void ThreadFunction()
	{
		try
		{
			this.TGO = TTSMeshCreatorThreadSafe.CreateColliders(this.TGO);
		}
		catch (Exception error)
		{
			base.SetError(error);
		}
	}

	// Token: 0x06001FBE RID: 8126 RVA: 0x000E2588 File Offset: 0x000E0788
	protected override void OnFinished()
	{
		if (base.isError)
		{
			return;
		}
		if (!this.GO)
		{
			Debug.LogWarning("Object is null for CustomTokenColliderJob OnFinished");
			return;
		}
		GameObject gameObject = new GameObject();
		gameObject.name = this.GO.name + "CompoundColliders";
		gameObject.transform.parent = this.GO.transform;
		gameObject.transform.localPosition = Vector3.zero;
		gameObject.transform.localRotation = Quaternion.identity;
		gameObject.transform.localScale = Vector3.one;
		foreach (TTSBoxCollider ttsboxCollider in this.TGO.boxColliders)
		{
			GameObject gameObject2 = new GameObject();
			BoxCollider boxCollider = gameObject2.AddComponent<BoxCollider>();
			boxCollider.center = ttsboxCollider.center;
			boxCollider.size = ttsboxCollider.size;
			gameObject2.transform.parent = gameObject.transform;
			gameObject2.transform.localPosition = Vector3.zero;
			gameObject2.transform.localRotation = Quaternion.identity;
			gameObject2.transform.localScale = Vector3.one;
		}
	}

	// Token: 0x06001FBF RID: 8127 RVA: 0x000E26C4 File Offset: 0x000E08C4
	public override void Reset()
	{
		base.Reset();
		this.GO = null;
		this.TGO = new TTSGameObject(true);
	}

	// Token: 0x04001375 RID: 4981
	public GameObject GO;

	// Token: 0x04001376 RID: 4982
	public TTSGameObject TGO;
}
