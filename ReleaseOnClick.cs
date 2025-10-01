using System;
using UnityEngine;

// Token: 0x0200038B RID: 907
public class ReleaseOnClick : MonoBehaviour
{
	// Token: 0x06002A9A RID: 10906 RVA: 0x000025B8 File Offset: 0x000007B8
	private void Start()
	{
	}

	// Token: 0x06002A9B RID: 10907 RVA: 0x000025B8 File Offset: 0x000007B8
	private void Update()
	{
	}

	// Token: 0x06002A9C RID: 10908 RVA: 0x0012F5D9 File Offset: 0x0012D7D9
	private void OnMouseUp()
	{
		base.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
	}
}
