using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020000A2 RID: 162
public class AttackCheckTrigger : MonoBehaviour
{
	// Token: 0x06000839 RID: 2105 RVA: 0x0003A2FD File Offset: 0x000384FD
	private void Awake()
	{
		base.gameObject.layer = 2;
	}

	// Token: 0x0600083A RID: 2106 RVA: 0x0003A30C File Offset: 0x0003850C
	private void OnTriggerEnter(Collider Other)
	{
		if (Other.transform.parent && Other.transform.parent.gameObject.GetComponent<RPGFigurines>())
		{
			this.AttackRPGObjects.Add(Other.gameObject.transform.parent.gameObject);
		}
	}

	// Token: 0x0600083B RID: 2107 RVA: 0x0003A368 File Offset: 0x00038568
	private void OnTriggerExit(Collider Other)
	{
		if (Other.transform.parent && Other.transform.parent.gameObject.GetComponent<RPGFigurines>())
		{
			this.AttackRPGObjects.Remove(Other.gameObject.transform.parent.gameObject);
		}
	}

	// Token: 0x0600083C RID: 2108 RVA: 0x0003A3C4 File Offset: 0x000385C4
	public void AttackRPG()
	{
		List<LuaGameObjectScript> list = new List<LuaGameObjectScript>();
		LuaGameObjectScript component = base.transform.root.GetComponent<LuaGameObjectScript>();
		foreach (GameObject gameObject in this.AttackRPGObjects)
		{
			gameObject.GetComponent<RPGFigurines>().GetHit();
			LuaGameObjectScript component2 = gameObject.GetComponent<LuaGameObjectScript>();
			if (!list.Contains(component2))
			{
				list.Add(component2);
				if (component2.enabled && component2.RPGFigurine.onHit != null)
				{
					component2.RPGFigurine.onHit.Function.Call(new object[]
					{
						component
					});
				}
			}
		}
		if (component.RPGFigurine.onAttack != null)
		{
			component.RPGFigurine.onAttack.Function.Call(new object[]
			{
				list
			});
		}
	}

	// Token: 0x040005BE RID: 1470
	private List<GameObject> AttackRPGObjects = new List<GameObject>();
}
