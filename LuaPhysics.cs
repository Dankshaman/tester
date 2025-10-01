using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using MoonSharp.Interpreter;
using UnityEngine;

// Token: 0x02000190 RID: 400
public class LuaPhysics
{
	// Token: 0x060013D0 RID: 5072 RVA: 0x00083C20 File Offset: 0x00081E20
	public List<LuaHit> Cast(LuaCast cast)
	{
		if (cast == null)
		{
			return null;
		}
		int num = 0;
		switch (cast.type)
		{
		case 1:
			num = Physics.RaycastNonAlloc(cast.origin, cast.direction, LuaPhysics.hits, cast.max_distance, HoverScript.GrabbableLayerMask);
			break;
		case 2:
			num = Physics.SphereCastNonAlloc(cast.origin, cast.size.x / 2f, cast.direction, LuaPhysics.hits, cast.max_distance, HoverScript.GrabbableLayerMask);
			break;
		case 3:
			num = Physics.BoxCastNonAlloc(cast.origin, cast.size / 2f, cast.direction, LuaPhysics.hits, Quaternion.Euler(cast.orientation), cast.max_distance, HoverScript.GrabbableLayerMask);
			break;
		}
		if (num >= LuaPhysics.hits.Length)
		{
			LuaGlobalScriptManager.Instance.LogError("Physics.cast", "exceeded the maximum number of hits", null);
		}
		if (cast.debug)
		{
			this.CreateDebugCast(cast);
		}
		List<ValueTuple<NetworkPhysicsObject, int>> list = new List<ValueTuple<NetworkPhysicsObject, int>>();
		for (int i = 0; i < num; i++)
		{
			NetworkPhysicsObject component = LuaPhysics.hits[i].collider.transform.root.GetComponent<NetworkPhysicsObject>();
			if (component)
			{
				list.Add(new ValueTuple<NetworkPhysicsObject, int>(component, i));
			}
		}
		list.Sort(delegate([TupleElementNames(new string[]
		{
			"npo",
			"index"
		})] ValueTuple<NetworkPhysicsObject, int> a, [TupleElementNames(new string[]
		{
			"npo",
			"index"
		})] ValueTuple<NetworkPhysicsObject, int> b)
		{
			if (a.Item1.ID < b.Item1.ID)
			{
				return -1;
			}
			if (a.Item1.ID > b.Item1.ID)
			{
				return 1;
			}
			float distance2 = LuaPhysics.hits[a.Item2].distance;
			float distance3 = LuaPhysics.hits[b.Item2].distance;
			if (distance2 < distance3)
			{
				return -1;
			}
			if (distance2 <= distance3)
			{
				return 0;
			}
			return 1;
		});
		List<LuaHit> list2 = new List<LuaHit>();
		for (int j = 0; j < list.Count; j++)
		{
			ValueTuple<NetworkPhysicsObject, int> valueTuple = list[j];
			NetworkPhysicsObject item = valueTuple.Item1;
			int item2 = valueTuple.Item2;
			if (j == 0 || item != list[j - 1].Item1)
			{
				float distance = LuaPhysics.hits[item2].distance;
				for (int k = 0; k <= list2.Count; k++)
				{
					if (k == list2.Count || distance <= list2[k].distance)
					{
						list2.Insert(k, new LuaHit(LuaPhysics.hits[item2], item.luaGameObjectScript));
						break;
					}
				}
			}
		}
		return list2;
	}

	// Token: 0x060013D1 RID: 5073 RVA: 0x00083E47 File Offset: 0x00082047
	public Vector3 GetGravity()
	{
		return Physics.gravity;
	}

	// Token: 0x060013D2 RID: 5074 RVA: 0x00083E4E File Offset: 0x0008204E
	public bool SetGravity(Vector3 gravity)
	{
		Physics.gravity = gravity;
		return true;
	}

	// Token: 0x060013D3 RID: 5075 RVA: 0x00083E58 File Offset: 0x00082058
	[MoonSharpHidden]
	private void CreateDebugCast(LuaCast cast)
	{
		GameObject gameObject = null;
		switch (cast.type)
		{
		case 1:
			gameObject = GameObject.CreatePrimitive(PrimitiveType.Cube);
			gameObject.transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);
			gameObject.transform.rotation = Quaternion.LookRotation(cast.direction);
			break;
		case 2:
			gameObject = GameObject.CreatePrimitive(PrimitiveType.Sphere);
			gameObject.transform.localScale = Vector3.one * cast.size.x;
			break;
		case 3:
			gameObject = GameObject.CreatePrimitive(PrimitiveType.Cube);
			gameObject.transform.localScale = cast.size;
			gameObject.transform.rotation = Quaternion.Euler(cast.orientation);
			break;
		}
		UnityEngine.Object.Destroy(gameObject.GetComponent<Collider>());
		Material material = gameObject.GetComponent<Renderer>().material;
		material.shader = Shader.Find("Marmoset/Transparent/Diffuse IBL");
		material.color = new Color(1f, 1f, 1f, 0.4f);
		float num = (cast.max_distance > 1000f) ? 1000f : cast.max_distance;
		Vector3 vector = cast.direction * num;
		Vector3 pos = new Vector3(cast.origin.x + vector.x, cast.origin.y + vector.y, cast.origin.z + vector.z);
		gameObject.transform.position = cast.origin;
		float num2 = Mathf.Max(0.025f * num, 0.5f);
		UITweener uitweener = TweenPosition.Begin(gameObject, num2, pos, true);
		float num3 = 0.1f / num2;
		uitweener.animationCurve = AnimationCurve.EaseInOut(num3, 0f, 1f - num3, 1f);
		UnityEngine.Object.Destroy(gameObject, num2);
	}

	// Token: 0x17000360 RID: 864
	// (get) Token: 0x060013D4 RID: 5076 RVA: 0x00084025 File Offset: 0x00082225
	// (set) Token: 0x060013D5 RID: 5077 RVA: 0x00084031 File Offset: 0x00082231
	public float play_area
	{
		get
		{
			return NetworkSingleton<GameOptions>.Instance.PlayArea;
		}
		set
		{
			NetworkSingleton<GameOptions>.Instance.PlayArea = value;
		}
	}

	// Token: 0x04000BAB RID: 2987
	[MoonSharpHidden]
	private static readonly RaycastHit[] hits = new RaycastHit[1000];
}
