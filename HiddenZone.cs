using System;
using System.Collections.Generic;
using NewNet;
using UnityEngine;

// Token: 0x02000126 RID: 294
public class HiddenZone : Zone
{
	// Token: 0x06000F91 RID: 3985 RVA: 0x0006A430 File Offset: 0x00068630
	protected override void Start()
	{
		base.Start();
		this.hiderID = base.NPO.ID.ToString();
		NetworkEvents.OnPlayerConnected += this.OnPlayerConnected;
		EventManager.OnPlayerChangeColor += this.OnPlayerChangeColor;
		if (base.GetComponent<BoxCollider>().enabled && base.networkView.isMine)
		{
			base.networkView.RPC<string>(RPCTarget.Others, new Action<string>(this.SetOwningColor), this.OwningColorLabel);
			base.networkView.RPC<bool>(RPCTarget.Others, new Action<bool>(this.SetPointersAreHidden), this.pointersAreHidden);
			base.networkView.RPC<bool>(RPCTarget.Others, new Action<bool>(this.SetHidingIsReversed), this.isReversed);
			base.networkView.RPC<bool>(RPCTarget.Others, new Action<bool>(this.SetTranslucent), this.isTranslucent);
		}
	}

	// Token: 0x06000F92 RID: 3986 RVA: 0x0006A514 File Offset: 0x00068714
	protected override void OnDestroy()
	{
		base.OnDestroy();
		NetworkEvents.OnPlayerConnected -= this.OnPlayerConnected;
		EventManager.OnPlayerChangeColor -= this.OnPlayerChangeColor;
		for (int i = 0; i < this.pointersInZone.Count; i++)
		{
			Pointer pointer = this.pointersInZone[i];
			if (pointer)
			{
				pointer.ReferenceFollower.GetComponent<MeshLerpToObject>().IsInvisible = false;
			}
		}
	}

	// Token: 0x06000F93 RID: 3987 RVA: 0x0006A585 File Offset: 0x00068785
	protected override void OnAddObject(NetworkPhysicsObject npo)
	{
		npo.SetInvisible(this.hiderID, true, this.hiddenFlags, false, false);
		base.OnAddObject(npo);
	}

	// Token: 0x06000F94 RID: 3988 RVA: 0x0006A5A3 File Offset: 0x000687A3
	protected override void OnRemoveObject(NetworkPhysicsObject npo)
	{
		if (!npo.IsDestroyed)
		{
			npo.SetInvisible(this.hiderID, false, 2147483647U, false, false);
		}
		base.OnRemoveObject(npo);
	}

	// Token: 0x06000F95 RID: 3989 RVA: 0x0006A5C8 File Offset: 0x000687C8
	protected override bool ValidateAddObject(NetworkPhysicsObject npo)
	{
		return base.ValidateAddObject(npo) && !npo.CompareTag("Board");
	}

	// Token: 0x06000F96 RID: 3990 RVA: 0x0006A5E4 File Offset: 0x000687E4
	private void OnTriggerEnter(Collider otherCollider)
	{
		GameObject gameObject = NetworkSingleton<ManagerPhysicsObject>.Instance.GOFromCollider(otherCollider);
		NetworkPhysicsObject networkPhysicsObject = NetworkSingleton<ManagerPhysicsObject>.Instance.NPOFromGO(gameObject);
		if (gameObject.CompareTag("Pointer"))
		{
			Pointer component = gameObject.GetComponent<Pointer>();
			if (!component.networkView.isMine && this.pointersInZone.TryAddUnique(component))
			{
				this.UpdatePointerVisibility(component);
				return;
			}
		}
		else if (networkPhysicsObject)
		{
			base.AddObject(otherCollider, networkPhysicsObject);
		}
	}

	// Token: 0x06000F97 RID: 3991 RVA: 0x0006A650 File Offset: 0x00068850
	private void OnTriggerExit(Collider otherCollider)
	{
		GameObject gameObject = NetworkSingleton<ManagerPhysicsObject>.Instance.GOFromCollider(otherCollider);
		NetworkPhysicsObject networkPhysicsObject = NetworkSingleton<ManagerPhysicsObject>.Instance.NPOFromGO(gameObject);
		if (gameObject.CompareTag("Pointer"))
		{
			Pointer component = gameObject.GetComponent<Pointer>();
			if (this.pointersInZone.Remove(component))
			{
				component.ReferenceFollower.GetComponent<MeshLerpToObject>().IsInvisible = false;
				return;
			}
		}
		else if (networkPhysicsObject)
		{
			base.RemoveObject(otherCollider, networkPhysicsObject);
		}
	}

	// Token: 0x06000F98 RID: 3992 RVA: 0x0006A6B9 File Offset: 0x000688B9
	private void UpdatePointerVisibility(Pointer pointer)
	{
		if (this.pointersAreHidden)
		{
			pointer.ReferenceFollower.GetComponent<MeshLerpToObject>().IsInvisible = NetworkPhysicsObject.IsHiddenToMe(this.hiddenFlags, true);
			return;
		}
		pointer.ReferenceFollower.GetComponent<MeshLerpToObject>().IsInvisible = false;
	}

	// Token: 0x06000F99 RID: 3993 RVA: 0x0006A6F4 File Offset: 0x000688F4
	public void UpdateObjectsInZone()
	{
		for (int i = 0; i < this.pointersInZone.Count; i++)
		{
			Pointer pointer = this.pointersInZone[i];
			if (pointer)
			{
				this.UpdatePointerVisibility(pointer);
			}
		}
		for (int j = 0; j < this.ContainedNPOs.Count; j++)
		{
			NetworkPhysicsObject networkPhysicsObject = this.ContainedNPOs[j];
			if (networkPhysicsObject)
			{
				networkPhysicsObject.SetInvisible(this.hiderID, true, this.hiddenFlags, false, false);
			}
		}
		this.UpdateVisual();
	}

	// Token: 0x06000F9A RID: 3994 RVA: 0x0006A77C File Offset: 0x0006897C
	private void UpdateVisual()
	{
		bool flag = NetworkPhysicsObject.IsHiddenToMe(this.hiddenFlags, true);
		if (flag)
		{
			this.render.material.color = new Color(this.VisualColor.r, this.VisualColor.g, this.VisualColor.b, this.isTranslucent ? HiddenZone.OpacityWhenHiding : 1f);
		}
		else
		{
			this.render.material.color = new Color(this.VisualColor.r, this.VisualColor.g, this.VisualColor.b, HiddenZone.OpacityWhenNotHiding);
		}
		string text = (!this.isTranslucent && flag) ? "Marmoset/Diffuse IBL" : "Marmoset/Transparent/Diffuse IBL";
		if (this.currentShader != text)
		{
			this.currentShader = text;
			this.render.material.shader = Shader.Find(text);
		}
	}

	// Token: 0x06000F9B RID: 3995 RVA: 0x0006A868 File Offset: 0x00068A68
	public static void UpdateAllVisuals()
	{
		for (int i = 0; i < NetworkSingleton<ManagerPhysicsObject>.Instance.AllNPOs.Count; i++)
		{
			NetworkPhysicsObject networkPhysicsObject = NetworkSingleton<ManagerPhysicsObject>.Instance.AllNPOs[i];
			if (networkPhysicsObject.hiddenZone)
			{
				networkPhysicsObject.hiddenZone.UpdateVisual();
			}
		}
	}

	// Token: 0x06000F9C RID: 3996 RVA: 0x0006A8B8 File Offset: 0x00068AB8
	public void SyncSetZoneColor(string zoneColorLabel)
	{
		if (!Colour.IsColourLabel(zoneColorLabel))
		{
			return;
		}
		base.networkView.RPC<string>(RPCTarget.All, new Action<string>(this.SetOwningColor), zoneColorLabel);
	}

	// Token: 0x06000F9D RID: 3997 RVA: 0x0006A8DC File Offset: 0x00068ADC
	[Remote(Permission.Client, SendType.ReliableBuffered, null, SerializationMethod.Default)]
	public void SetOwningColor(string ownerColorLabel)
	{
		if (ownerColorLabel == "Grey")
		{
			ownerColorLabel = "Black";
		}
		this.OwningColorLabel = ownerColorLabel;
		this.VisualColor = Colour.ColourFromLabel(this.OwningColorLabel);
		this.darkVisualColor = Colour.DarkenedFromColour(this.VisualColor);
		this.highlightColor = this.darkVisualColor;
		if (this.isReversed)
		{
			this.hiddenFlags = (Colour.FlagFromLabel(this.OwningColorLabel) | 2147483648U);
		}
		else
		{
			this.hiddenFlags = Colour.InverseFlagsFromLabel(this.OwningColorLabel);
		}
		this.UpdateObjectsInZone();
	}

	// Token: 0x06000F9E RID: 3998 RVA: 0x0006A979 File Offset: 0x00068B79
	[Remote(Permission.Client, SendType.ReliableBuffered, null, SerializationMethod.Default)]
	public void SetPointersAreHidden(bool pointersAreHidden)
	{
		this.pointersAreHidden = pointersAreHidden;
		this.UpdateObjectsInZone();
	}

	// Token: 0x06000F9F RID: 3999 RVA: 0x0006A988 File Offset: 0x00068B88
	[Remote(Permission.Client, SendType.ReliableBuffered, null, SerializationMethod.Default)]
	public void SetHidingIsReversed(bool isReversed)
	{
		this.isReversed = isReversed;
		if (isReversed)
		{
			this.hiddenFlags = (Colour.FlagFromLabel(this.OwningColorLabel) | 2147483648U);
		}
		else
		{
			this.hiddenFlags = Colour.InverseFlagsFromLabel(this.OwningColorLabel);
		}
		this.UpdateObjectsInZone();
	}

	// Token: 0x06000FA0 RID: 4000 RVA: 0x0006A9C4 File Offset: 0x00068BC4
	[Remote(Permission.Client, SendType.ReliableBuffered, null, SerializationMethod.Default)]
	public void SetTranslucent(bool isTranslucent)
	{
		this.isTranslucent = isTranslucent;
		this.UpdateObjectsInZone();
	}

	// Token: 0x06000FA1 RID: 4001 RVA: 0x0006A9D4 File Offset: 0x00068BD4
	protected void OnPlayerConnected(NetworkPlayer player)
	{
		if (Network.isServer)
		{
			base.networkView.RPC<string>(player, new Action<string>(this.SetOwningColor), this.OwningColorLabel);
			base.networkView.RPC<bool>(player, new Action<bool>(this.SetPointersAreHidden), this.pointersAreHidden);
			base.networkView.RPC<bool>(player, new Action<bool>(this.SetHidingIsReversed), this.isReversed);
			base.networkView.RPC<bool>(player, new Action<bool>(this.SetTranslucent), this.isTranslucent);
		}
	}

	// Token: 0x06000FA2 RID: 4002 RVA: 0x0006AA60 File Offset: 0x00068C60
	private void OnPlayerChangeColor(PlayerState playerstate)
	{
		if (playerstate.IsMe())
		{
			this.UpdateVisual();
		}
	}

	// Token: 0x040009A6 RID: 2470
	public static float OpacityWhenHiding = 0.75f;

	// Token: 0x040009A7 RID: 2471
	public static float OpacityWhenNotHiding = 0.25f;

	// Token: 0x040009A8 RID: 2472
	public string OwningColorLabel = "White";

	// Token: 0x040009A9 RID: 2473
	private string hiderID;

	// Token: 0x040009AA RID: 2474
	private uint hiddenFlags;

	// Token: 0x040009AB RID: 2475
	public Color VisualColor = Colour.White;

	// Token: 0x040009AC RID: 2476
	private Color darkVisualColor = Colour.GreyDark;

	// Token: 0x040009AD RID: 2477
	private readonly List<Pointer> pointersInZone = new List<Pointer>();

	// Token: 0x040009AE RID: 2478
	private string currentShader = "";

	// Token: 0x040009AF RID: 2479
	public bool pointersAreHidden;

	// Token: 0x040009B0 RID: 2480
	public bool isReversed;

	// Token: 0x040009B1 RID: 2481
	public bool isTranslucent = true;
}
