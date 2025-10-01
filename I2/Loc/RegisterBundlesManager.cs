using System;
using UnityEngine;

namespace I2.Loc
{
	// Token: 0x0200046A RID: 1130
	public class RegisterBundlesManager : MonoBehaviour, IResourceManager_Bundles
	{
		// Token: 0x06003324 RID: 13092 RVA: 0x0015578E File Offset: 0x0015398E
		public void OnEnable()
		{
			if (!ResourceManager.pInstance.mBundleManagers.Contains(this))
			{
				ResourceManager.pInstance.mBundleManagers.Add(this);
			}
		}

		// Token: 0x06003325 RID: 13093 RVA: 0x001557B2 File Offset: 0x001539B2
		public void OnDisable()
		{
			ResourceManager.pInstance.mBundleManagers.Remove(this);
		}

		// Token: 0x06003326 RID: 13094 RVA: 0x00079594 File Offset: 0x00077794
		public virtual UnityEngine.Object LoadFromBundle(string path, Type assetType)
		{
			return null;
		}
	}
}
