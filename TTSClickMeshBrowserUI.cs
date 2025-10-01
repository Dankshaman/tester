using System;
using System.Collections.Generic;
using UnityEngine;
using ZenFulcrum.EmbeddedBrowser;

// Token: 0x02000392 RID: 914
public class TTSClickMeshBrowserUI : MonoBehaviour, IBrowserUI
{
	// Token: 0x06002ABF RID: 10943 RVA: 0x0012F7F9 File Offset: 0x0012D9F9
	public static TTSClickMeshBrowserUI Create(GameObject go)
	{
		return go.AddComponent<TTSClickMeshBrowserUI>();
	}

	// Token: 0x06002AC0 RID: 10944 RVA: 0x0012F804 File Offset: 0x0012DA04
	public void Awake()
	{
		this.BrowserCursor = new BrowserCursor();
		this.BrowserCursor.cursorChange += this.CursorUpdated;
		this.InputSettings = new BrowserInputSettings();
		this.NPO = base.GetComponent<NetworkPhysicsObject>();
		this.collider = base.GetComponent<Collider>();
	}

	// Token: 0x170004ED RID: 1261
	// (get) Token: 0x06002AC1 RID: 10945 RVA: 0x0012F856 File Offset: 0x0012DA56
	protected virtual Ray LookRay
	{
		get
		{
			return Camera.main.ScreenPointToRay(Input.mousePosition);
		}
	}

	// Token: 0x06002AC2 RID: 10946 RVA: 0x0012F868 File Offset: 0x0012DA68
	public virtual void InputUpdate()
	{
		List<Event> list = this.keyEvents;
		this.keyEvents = this.keyEventsLast;
		this.keyEventsLast = list;
		this.keyEvents.Clear();
		if (!UICamera.HoveredUIObject)
		{
			RaycastHit raycastHit;
			if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out raycastHit, 1000f, HoverScript.GrabbableLayerMask))
			{
				if (raycastHit.collider != this.collider)
				{
					this.MousePosition = new Vector3(0f, 0f);
					this.MouseButtons = (MouseButton)0;
					this.MouseScroll = new Vector2(0f, 0f);
					this.MouseHasFocus = false;
					this.KeyboardHasFocus = false;
					this.LookOff();
					return;
				}
				Vector3 vector = base.transform.InverseTransformPoint(raycastHit.point);
				vector.x *= this.NPO.Scale.x;
				vector.z *= this.NPO.Scale.z;
				Quaternion rotation = base.transform.rotation;
				base.transform.localRotation = Quaternion.identity;
				Bounds bounds = this.collider.bounds;
				base.transform.rotation = rotation;
				vector.x = (vector.x - bounds.extents.x) / bounds.size.x;
				vector.y = (vector.z - bounds.extents.x) / bounds.size.x;
				vector *= -1f;
				this.MouseHasFocus = true;
				this.KeyboardHasFocus = true;
				this.MousePosition = vector;
			}
		}
		else
		{
			if (!NetworkSingleton<NetworkUI>.Instance.GUITabletWindow.GetComponent<UITabletWindow>().ThisUITextureView || UICamera.HoveredUIObject != NetworkSingleton<NetworkUI>.Instance.GUITabletWindow.GetComponent<UITabletWindow>().ThisUITextureView.gameObject)
			{
				this.MousePosition = new Vector3(0f, 0f);
				this.MouseButtons = (MouseButton)0;
				this.MouseScroll = new Vector2(0f, 0f);
				this.MouseHasFocus = false;
				this.KeyboardHasFocus = false;
				this.LookOff();
				return;
			}
			Vector2 uvpoint = this.getUVPoint2(UICamera.lastWorldPosition);
			UITexture component = UICamera.HoveredUIObject.GetComponent<UITexture>();
			Vector3 v = new Vector2(uvpoint.x / (float)component.width, ((float)component.height + uvpoint.y) / (float)component.height);
			this.MouseHasFocus = true;
			this.KeyboardHasFocus = true;
			this.MousePosition = v;
		}
		MouseButton mouseButton = (MouseButton)0;
		if (Input.GetMouseButton(0))
		{
			mouseButton |= MouseButton.Left;
		}
		if (Input.GetMouseButton(1))
		{
			mouseButton |= MouseButton.Right;
		}
		if (Input.GetMouseButton(2))
		{
			mouseButton |= MouseButton.Middle;
		}
		this.MouseButtons = mouseButton;
		this.MouseScroll = Input.mouseScrollDelta;
		if (!this.KeyboardHasFocus)
		{
			return;
		}
		for (int i = 0; i < TTSClickMeshBrowserUI.keysToCheck.Length; i++)
		{
			if (Input.GetKeyDown(TTSClickMeshBrowserUI.keysToCheck[i]))
			{
				this.keyEventsLast.Insert(0, new Event
				{
					type = EventType.KeyDown,
					keyCode = TTSClickMeshBrowserUI.keysToCheck[i]
				});
			}
			else if (Input.GetKeyUp(TTSClickMeshBrowserUI.keysToCheck[i]))
			{
				this.keyEventsLast.Add(new Event
				{
					type = EventType.KeyUp,
					keyCode = TTSClickMeshBrowserUI.keysToCheck[i]
				});
			}
		}
	}

	// Token: 0x06002AC3 RID: 10947 RVA: 0x0012FBDC File Offset: 0x0012DDDC
	public bool CheckHit()
	{
		RaycastHit raycastHit;
		return Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out raycastHit, 1000f, HoverScript.GrabbableLayerMask) && !(raycastHit.collider != this.collider);
	}

	// Token: 0x06002AC4 RID: 10948 RVA: 0x0012FC24 File Offset: 0x0012DE24
	public Vector2 getUVPoint2(Vector3 hitPoint)
	{
		return UICamera.HoveredUIObject.GetComponent<UITexture>().transform.InverseTransformPoint(hitPoint);
	}

	// Token: 0x06002AC5 RID: 10949 RVA: 0x0012FC40 File Offset: 0x0012DE40
	public void OnGUI()
	{
		if (!this.KeyboardHasFocus)
		{
			return;
		}
		Event current = Event.current;
		if (current.type != EventType.KeyDown && current.type != EventType.KeyUp)
		{
			return;
		}
		this.keyEvents.Add(new Event(current));
	}

	// Token: 0x06002AC6 RID: 10950 RVA: 0x0012FC80 File Offset: 0x0012DE80
	protected void LookOn()
	{
		if (this.BrowserCursor != null)
		{
			this.CursorUpdated();
		}
		this.mouseWasOver = true;
	}

	// Token: 0x06002AC7 RID: 10951 RVA: 0x0012FC97 File Offset: 0x0012DE97
	protected void LookOff()
	{
		if (this.BrowserCursor != null && this.mouseWasOver)
		{
			this.SetCursor(null);
		}
		this.mouseWasOver = false;
	}

	// Token: 0x06002AC8 RID: 10952 RVA: 0x0012FCB7 File Offset: 0x0012DEB7
	protected void CursorUpdated()
	{
		this.SetCursor(this.BrowserCursor);
	}

	// Token: 0x06002AC9 RID: 10953 RVA: 0x0012FCC8 File Offset: 0x0012DEC8
	protected virtual void SetCursor(BrowserCursor newCursor)
	{
		if (!this.MouseHasFocus && newCursor != null)
		{
			return;
		}
		if (newCursor == null)
		{
			Cursor.visible = true;
			Utilities.SetCursor(null, Vector2.zero, CursorMode.Auto);
			return;
		}
		if (newCursor.Texture != null)
		{
			Cursor.visible = true;
			Utilities.SetCursor(newCursor.Texture, newCursor.Hotspot, CursorMode.Auto);
			return;
		}
		Cursor.visible = false;
		Utilities.SetCursor(null, Vector2.zero, CursorMode.Auto);
	}

	// Token: 0x170004EE RID: 1262
	// (get) Token: 0x06002ACA RID: 10954 RVA: 0x0012FD33 File Offset: 0x0012DF33
	// (set) Token: 0x06002ACB RID: 10955 RVA: 0x0012FD3B File Offset: 0x0012DF3B
	public bool MouseHasFocus { get; protected set; }

	// Token: 0x170004EF RID: 1263
	// (get) Token: 0x06002ACC RID: 10956 RVA: 0x0012FD44 File Offset: 0x0012DF44
	// (set) Token: 0x06002ACD RID: 10957 RVA: 0x0012FD4C File Offset: 0x0012DF4C
	public Vector2 MousePosition { get; protected set; }

	// Token: 0x170004F0 RID: 1264
	// (get) Token: 0x06002ACE RID: 10958 RVA: 0x0012FD55 File Offset: 0x0012DF55
	// (set) Token: 0x06002ACF RID: 10959 RVA: 0x0012FD5D File Offset: 0x0012DF5D
	public MouseButton MouseButtons { get; protected set; }

	// Token: 0x170004F1 RID: 1265
	// (get) Token: 0x06002AD0 RID: 10960 RVA: 0x0012FD66 File Offset: 0x0012DF66
	// (set) Token: 0x06002AD1 RID: 10961 RVA: 0x0012FD6E File Offset: 0x0012DF6E
	public Vector2 MouseScroll { get; protected set; }

	// Token: 0x170004F2 RID: 1266
	// (get) Token: 0x06002AD2 RID: 10962 RVA: 0x0012FD77 File Offset: 0x0012DF77
	// (set) Token: 0x06002AD3 RID: 10963 RVA: 0x0012FD7F File Offset: 0x0012DF7F
	public bool KeyboardHasFocus { get; protected set; }

	// Token: 0x170004F3 RID: 1267
	// (get) Token: 0x06002AD4 RID: 10964 RVA: 0x0012FD88 File Offset: 0x0012DF88
	public List<Event> KeyEvents
	{
		get
		{
			return this.keyEventsLast;
		}
	}

	// Token: 0x170004F4 RID: 1268
	// (get) Token: 0x06002AD5 RID: 10965 RVA: 0x0012FD90 File Offset: 0x0012DF90
	// (set) Token: 0x06002AD6 RID: 10966 RVA: 0x0012FD98 File Offset: 0x0012DF98
	public BrowserCursor BrowserCursor { get; protected set; }

	// Token: 0x170004F5 RID: 1269
	// (get) Token: 0x06002AD7 RID: 10967 RVA: 0x0012FDA1 File Offset: 0x0012DFA1
	// (set) Token: 0x06002AD8 RID: 10968 RVA: 0x0012FDA9 File Offset: 0x0012DFA9
	public BrowserInputSettings InputSettings { get; protected set; }

	// Token: 0x04001D0E RID: 7438
	private NetworkPhysicsObject NPO;

	// Token: 0x04001D0F RID: 7439
	private Collider collider;

	// Token: 0x04001D10 RID: 7440
	[HideInInspector]
	public float maxDistance = float.PositiveInfinity;

	// Token: 0x04001D11 RID: 7441
	protected List<Event> keyEvents = new List<Event>();

	// Token: 0x04001D12 RID: 7442
	protected List<Event> keyEventsLast = new List<Event>();

	// Token: 0x04001D13 RID: 7443
	private static readonly KeyCode[] keysToCheck = new KeyCode[]
	{
		KeyCode.LeftShift,
		KeyCode.RightShift
	};

	// Token: 0x04001D14 RID: 7444
	protected bool mouseWasOver;
}
