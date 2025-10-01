using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200009A RID: 154
public class AchievementKnilKimi : MonoBehaviour
{
	// Token: 0x06000818 RID: 2072 RVA: 0x0003888C File Offset: 0x00036A8C
	private void Start()
	{
		using (List<NetworkPhysicsObject>.Enumerator enumerator = NetworkSingleton<ManagerPhysicsObject>.Instance.GrabbableNPOs.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				if (enumerator.Current.InternalName == this.OtherObjectName)
				{
					Achievements.Set("ACH_HIDDEN_KNIL_KIMI");
					break;
				}
			}
		}
	}

	// Token: 0x0400059D RID: 1437
	public string OtherObjectName = "";
}
