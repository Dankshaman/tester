using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020001ED RID: 493
public class RectangleSelection : Singleton<RectangleSelection>
{
	// Token: 0x060019F7 RID: 6647 RVA: 0x000B601F File Offset: 0x000B421F
	protected override void Awake()
	{
		base.Awake();
		this.objectSelectionRectangle = new TTSObjectSelectionRectangle();
		this.visualSelectionRectangle = new TTSObjectSelectionRectangle();
		this.visualSelectionRectangle.IsVisible = true;
	}

	// Token: 0x060019F8 RID: 6648 RVA: 0x000B604C File Offset: 0x000B424C
	private Vector3 OffsetInputScreenPosition()
	{
		if (HoverScript.HoverCamera == Singleton<CameraManager>.Instance.SpectatorCamera && !Singleton<SpectatorCamera>.Instance.DisplayingFullscreen)
		{
			return HoverScript.InputScreenPosition(0) - Singleton<CameraManager>.Instance.SpectatorViewCamera.Offset;
		}
		return HoverScript.InputScreenPosition(0);
	}

	// Token: 0x060019F9 RID: 6649 RVA: 0x000B609C File Offset: 0x000B429C
	public void StartSelection(List<GameObject> AlreadySelectedObjects = null, Action<GameObject> AddCallback = null, Action<GameObject> RemoveCallback = null, int SelectCountLimit = 2147483647)
	{
		if (this.Selecting)
		{
			Debug.Log("Already rectangle selecting!");
			return;
		}
		this.Selecting = true;
		this.objectSelectionRectangle.SetEnclosingRectTopLeftPoint(this.OffsetInputScreenPosition());
		this.objectSelectionRectangle.SetEnclosingRectBottomRightPoint(this.OffsetInputScreenPosition());
		this.visualSelectionRectangle.SetEnclosingRectTopLeftPoint(HoverScript.InputScreenPosition(0));
		this.visualSelectionRectangle.SetEnclosingRectBottomRightPoint(HoverScript.InputScreenPosition(0));
		this.SelectedObjects.Clear();
		this.CtrlSelectedObjects.Clear();
		if (AlreadySelectedObjects != null)
		{
			for (int i = 0; i < AlreadySelectedObjects.Count; i++)
			{
				this.CtrlSelectedObjects.Add(AlreadySelectedObjects[i]);
			}
		}
		this.AddCallback = AddCallback;
		this.RemoveCallback = RemoveCallback;
		this.SelectCountLimit = SelectCountLimit;
		Colour colour = PlayerScript.Pointer ? PlayerScript.PointerScript.PointerColour : Colour.White;
		this.visualSelectionRectangle.RenderSettings.BorderLineColor = colour;
		colour.a = 0.03f;
		this.visualSelectionRectangle.RenderSettings.FillColor = colour;
	}

	// Token: 0x060019FA RID: 6650 RVA: 0x000B61C3 File Offset: 0x000B43C3
	public void EndSelection()
	{
		if (!this.Selecting)
		{
			Debug.LogError("Not rectangle selecting!");
			return;
		}
		this.Selecting = false;
	}

	// Token: 0x060019FB RID: 6651 RVA: 0x000B61DF File Offset: 0x000B43DF
	public List<GameObject> GetSelectedObject()
	{
		return this.SelectedObjects;
	}

	// Token: 0x060019FC RID: 6652 RVA: 0x000B61E7 File Offset: 0x000B43E7
	private void Add(GameObject GO)
	{
		this.SelectedObjects.Add(GO);
		if (this.AddCallback != null)
		{
			this.AddCallback(GO);
		}
	}

	// Token: 0x060019FD RID: 6653 RVA: 0x000B6209 File Offset: 0x000B4409
	private void Remove(GameObject GO)
	{
		this.SelectedObjects.Remove(GO);
		if (this.RemoveCallback != null)
		{
			this.RemoveCallback(GO);
		}
	}

	// Token: 0x060019FE RID: 6654 RVA: 0x000B622C File Offset: 0x000B442C
	private void Update()
	{
		if (this.Selecting)
		{
			Vector3 vector = this.OffsetInputScreenPosition();
			this.onSpectatorView = (vector.z != 0f);
			this.objectSelectionRectangle.SetEnclosingRectBottomRightPoint(vector);
			this.visualSelectionRectangle.SetEnclosingRectBottomRightPoint(HoverScript.InputScreenPosition(0));
			List<NetworkPhysicsObject> grabbableNPOs = NetworkSingleton<ManagerPhysicsObject>.Instance.GrabbableNPOs;
			bool button = zInput.GetButton("Alt", ControlType.All);
			Rect enclosingRectangle = this.objectSelectionRectangle.EnclosingRectangle;
			this.selectionCamera = HoverScript.GetHoverCamera();
			if (HandCamera.Instance.camera.gameObject.activeSelf && HandCamera.Instance.GetPixelInteractionRect().Contains(this.OffsetInputScreenPosition()))
			{
				this.selectionCamera = HandCamera.Instance.camera;
			}
			else if (this.onSpectatorView)
			{
				this.selectionCamera = Singleton<CameraManager>.Instance.SpectatorCamera;
			}
			int num = 0;
			while (num < grabbableNPOs.Count && this.SelectedObjects.Count < this.SelectCountLimit)
			{
				NetworkPhysicsObject networkPhysicsObject = grabbableNPOs[num];
				GameObject gameObject = networkPhysicsObject.gameObject;
				if ((!networkPhysicsObject.IsLocked || button) && (!networkPhysicsObject.IsHidden || networkPhysicsObject.IsObscured) && gameObject.layer != 2 && networkPhysicsObject.IsGrabbable)
				{
					Vector2 v = this.selectionCamera.WorldToScreenPoint(gameObject.transform.position);
					if (enclosingRectangle.Contains(v, true) && (networkPhysicsObject.IsDragSelectable || zInput.GetButton("Shift", ControlType.All)))
					{
						if (!this.CtrlSelectedObjects.Contains(gameObject))
						{
							if (!this.SelectedObjects.Contains(gameObject) && this.SelectedObjects.Count < this.SelectCountLimit)
							{
								this.Add(gameObject);
							}
						}
						else if (this.SelectedObjects.Contains(gameObject))
						{
							this.Remove(gameObject);
						}
					}
					else if (!this.CtrlSelectedObjects.Contains(gameObject))
					{
						if (this.SelectedObjects.Contains(gameObject))
						{
							this.Remove(gameObject);
						}
					}
					else if (!this.SelectedObjects.Contains(gameObject) && this.SelectedObjects.Count < this.SelectCountLimit)
					{
						this.Add(gameObject);
					}
				}
				num++;
			}
			if (this.SelectedObjects.Count >= this.SelectCountLimit)
			{
				for (int i = 0; i < this.SelectedObjects.Count; i++)
				{
					GameObject gameObject2 = this.SelectedObjects[i];
					if (gameObject2)
					{
						NetworkPhysicsObject networkPhysicsObject2 = NetworkSingleton<ManagerPhysicsObject>.Instance.NPOFromGO(gameObject2);
						if ((!networkPhysicsObject2.IsLocked || button) && (!networkPhysicsObject2.IsHidden || networkPhysicsObject2.IsObscured) && gameObject2.layer != 2 && networkPhysicsObject2.IsGrabbable)
						{
							Vector2 v2 = this.selectionCamera.WorldToScreenPoint(gameObject2.transform.position);
							Vector3 zero = Vector3.zero;
							if (enclosingRectangle.Contains(v2, true))
							{
								if (!this.CtrlSelectedObjects.Contains(gameObject2))
								{
									if (!this.SelectedObjects.Contains(gameObject2))
									{
										this.Add(gameObject2);
									}
								}
								else if (this.SelectedObjects.Contains(gameObject2))
								{
									this.Remove(gameObject2);
								}
							}
							else if (!this.CtrlSelectedObjects.Contains(gameObject2))
							{
								if (this.SelectedObjects.Contains(gameObject2))
								{
									this.Remove(gameObject2);
								}
							}
							else if (!this.SelectedObjects.Contains(gameObject2))
							{
								this.Add(gameObject2);
							}
						}
					}
				}
			}
		}
	}

	// Token: 0x060019FF RID: 6655 RVA: 0x000B65D4 File Offset: 0x000B47D4
	private void OnRenderObject()
	{
		if (this.onSpectatorView)
		{
			return;
		}
		if (this.selectionCamera == Singleton<CameraManager>.Instance.SpectatorCamera)
		{
			if (Camera.current != Singleton<CameraManager>.Instance.UICamera)
			{
				return;
			}
		}
		else if (Camera.current != this.selectionCamera)
		{
			return;
		}
		if (this.Selecting)
		{
			this.visualSelectionRectangle.Render();
		}
	}

	// Token: 0x04000FFC RID: 4092
	private List<GameObject> SelectedObjects = new List<GameObject>();

	// Token: 0x04000FFD RID: 4093
	private List<GameObject> CtrlSelectedObjects = new List<GameObject>();

	// Token: 0x04000FFE RID: 4094
	private bool Selecting;

	// Token: 0x04000FFF RID: 4095
	private TTSObjectSelectionRectangle objectSelectionRectangle;

	// Token: 0x04001000 RID: 4096
	private TTSObjectSelectionRectangle visualSelectionRectangle;

	// Token: 0x04001001 RID: 4097
	private Action<GameObject> AddCallback;

	// Token: 0x04001002 RID: 4098
	private Action<GameObject> RemoveCallback;

	// Token: 0x04001003 RID: 4099
	private int SelectCountLimit = int.MaxValue;

	// Token: 0x04001004 RID: 4100
	private Camera selectionCamera;

	// Token: 0x04001005 RID: 4101
	private bool onSpectatorView;
}
