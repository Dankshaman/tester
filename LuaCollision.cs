using System;
using System.Collections.Generic;
using MoonSharp.Interpreter;
using UnityEngine;

// Token: 0x02000157 RID: 343
public class LuaCollision : MonoBehaviour
{
	// Token: 0x06001130 RID: 4400 RVA: 0x000763E4 File Offset: 0x000745E4
	private void Start()
	{
		this.networkPhysicsObject = base.GetComponent<NetworkPhysicsObject>();
		if (this.bOnCollisionStay)
		{
			this.networkPhysicsObject.collisionEvents.ReqisterForCollisionStay(this);
			this.networkPhysicsObject.collisionEvents.OnCollisionStayEvent += this.ManagedOnCollisionStay;
		}
		this.networkPhysicsObject.collisionEvents.OnCollisionEnterEvent += this.ManagedOnCollisionEnter;
		this.networkPhysicsObject.collisionEvents.OnCollisionExitEvent += this.ManagedOnCollisionExit;
	}

	// Token: 0x06001131 RID: 4401 RVA: 0x0007646C File Offset: 0x0007466C
	private void OnDestroy()
	{
		if (this.bOnCollisionStay)
		{
			this.networkPhysicsObject.collisionEvents.UnreqisterForCollisionStay(this);
			this.networkPhysicsObject.collisionEvents.OnCollisionStayEvent -= this.ManagedOnCollisionStay;
		}
		this.networkPhysicsObject.collisionEvents.OnCollisionEnterEvent -= this.ManagedOnCollisionEnter;
		this.networkPhysicsObject.collisionEvents.OnCollisionExitEvent -= this.ManagedOnCollisionExit;
	}

	// Token: 0x06001132 RID: 4402 RVA: 0x000764E8 File Offset: 0x000746E8
	public void ManagedOnCollisionEnter(Collision collision)
	{
		bool flag = this.global != null && this.global.Globals["onObjectCollisionEnter"] != null;
		bool flag2 = this.lua != null && this.lua.Globals["onCollisionEnter"] != null;
		if (flag || flag2)
		{
			Table table = null;
			LuaGameObjectScript component = collision.gameObject.GetComponent<LuaGameObjectScript>();
			if (flag2)
			{
				if (component != null)
				{
					table = this.MakeLuaCollisionTable(this.lua, component, collision);
				}
				try
				{
					this.lua.Call(this.lua.Globals["onCollisionEnter"], new object[]
					{
						table
					});
				}
				catch (Exception e)
				{
					Chat.LogError("Error in Script, " + TTSUtilities.CleanName(base.gameObject.GetComponent<NetworkPhysicsObject>()) + ", onCollisionEnter function: " + LuaScript.ExceptionToMessage(e), true);
				}
			}
			if (flag)
			{
				if (component != null)
				{
					table = this.MakeLuaCollisionTable(this.global, component, collision);
				}
				try
				{
					this.global.Call(this.global.Globals["onObjectCollisionEnter"], new object[]
					{
						this.networkPhysicsObject.luaGameObjectScript,
						table
					});
				}
				catch (Exception e2)
				{
					Chat.LogError("Error in Global Script, onObjectCollisionEnter function: " + LuaScript.ExceptionToMessage(e2), true);
				}
			}
		}
	}

	// Token: 0x06001133 RID: 4403 RVA: 0x0007665C File Offset: 0x0007485C
	public void ManagedOnCollisionStay(Collision collision)
	{
		if (!this.bOnCollisionStay)
		{
			return;
		}
		bool flag = this.global != null && this.global.Globals["onObjectCollisionStay"] != null;
		bool flag2 = this.lua != null && this.lua.Globals["onCollisionStay"] != null;
		if (flag || flag2)
		{
			Table table = null;
			LuaGameObjectScript component = collision.gameObject.GetComponent<LuaGameObjectScript>();
			if (flag2)
			{
				if (component != null)
				{
					table = this.MakeLuaCollisionTable(this.lua, component, collision);
				}
				try
				{
					this.lua.Call(this.lua.Globals["onCollisionStay"], new object[]
					{
						table
					});
				}
				catch (Exception e)
				{
					Chat.LogError("Error in Script, " + TTSUtilities.CleanName(base.gameObject.GetComponent<NetworkPhysicsObject>()) + ", onCollisionStay function: " + LuaScript.ExceptionToMessage(e), true);
				}
			}
			if (flag)
			{
				if (component != null)
				{
					table = this.MakeLuaCollisionTable(this.global, component, collision);
				}
				try
				{
					this.global.Call(this.global.Globals["onObjectCollisionStay"], new object[]
					{
						this.networkPhysicsObject.luaGameObjectScript,
						table
					});
				}
				catch (Exception e2)
				{
					Chat.LogError("Error in Global Script, onObjectCollisionStay function: " + LuaScript.ExceptionToMessage(e2), true);
				}
			}
		}
	}

	// Token: 0x06001134 RID: 4404 RVA: 0x000767D8 File Offset: 0x000749D8
	public void ManagedOnCollisionExit(Collision collision)
	{
		bool flag = this.global != null && this.global.Globals["onObjectCollisionExit"] != null;
		bool flag2 = this.lua != null && this.lua.Globals["onCollisionExit"] != null;
		if (flag || flag2)
		{
			Table table = null;
			LuaGameObjectScript component = collision.gameObject.GetComponent<LuaGameObjectScript>();
			if (flag2)
			{
				if (component != null)
				{
					table = this.MakeLuaCollisionTable(this.lua, component, collision);
				}
				try
				{
					this.lua.Call(this.lua.Globals["onCollisionExit"], new object[]
					{
						table
					});
				}
				catch (Exception e)
				{
					Chat.LogError("Error in Script, " + TTSUtilities.CleanName(base.gameObject.GetComponent<NetworkPhysicsObject>()) + ", onCollisionExit function: " + LuaScript.ExceptionToMessage(e), true);
				}
			}
			if (flag)
			{
				if (component != null)
				{
					table = this.MakeLuaCollisionTable(this.global, component, collision);
				}
				try
				{
					this.global.Call(this.global.Globals["onObjectCollisionExit"], new object[]
					{
						this.networkPhysicsObject.luaGameObjectScript,
						table
					});
				}
				catch (Exception e2)
				{
					Chat.LogError("Error in Global Script, onObjectCollisionExit function: " + LuaScript.ExceptionToMessage(e2), true);
				}
			}
		}
	}

	// Token: 0x06001135 RID: 4405 RVA: 0x0007694C File Offset: 0x00074B4C
	public Table MakeLuaCollisionTable(Script script, LuaGameObjectScript LGOS, Collision collision)
	{
		Table table = new Table(script);
		Table table2 = new Table(script);
		for (int i = 0; i < collision.contacts.Length; i++)
		{
			List<float> list = new List<float>();
			list.Add(collision.contacts[i].point.x);
			list.Add(collision.contacts[i].point.y);
			list.Add(collision.contacts[i].point.z);
			table2[i + 1] = list;
		}
		List<float> list2 = new List<float>();
		list2.Add(collision.impulse.x);
		list2.Add(collision.impulse.y);
		list2.Add(collision.impulse.z);
		List<float> list3 = new List<float>();
		list3.Add(collision.impulse.x);
		list3.Add(collision.impulse.y);
		list3.Add(collision.impulse.z);
		table["collision_object"] = LGOS;
		table["contact_points"] = table2;
		table["impuslse"] = list2;
		table["relative_velocity"] = list3;
		return table;
	}

	// Token: 0x04000B09 RID: 2825
	public Script lua;

	// Token: 0x04000B0A RID: 2826
	public Script global;

	// Token: 0x04000B0B RID: 2827
	public bool bOnCollisionStay;

	// Token: 0x04000B0C RID: 2828
	private NetworkPhysicsObject networkPhysicsObject;
}
