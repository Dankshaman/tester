using System;
using System.Collections;
using System.Collections.Generic;
using HighlightingSystem;
using UnityEngine;

// Token: 0x02000302 RID: 770
public class UILuaNotepad : UINotepad
{
	// Token: 0x06002521 RID: 9505 RVA: 0x00105D18 File Offset: 0x00103F18
	private void Start()
	{
		this.currentIndex = -1;
		this.Init();
		UIEventListener uieventListener = UIEventListener.Get(this.savePlay);
		uieventListener.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(uieventListener.onClick, new UIEventListener.VoidDelegate(this.OnSavePlayClicked));
		UIEventListener uieventListener2 = UIEventListener.Get(this.autoRun);
		uieventListener2.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(uieventListener2.onClick, new UIEventListener.VoidDelegate(this.OnAutoRunClicked));
		this.autoRun.GetComponentInChildren<UIToggle>().value = NetworkSingleton<NetworkUI>.Instance.bAutoRunScripts;
		base.GetComponent<UISprite>().enabled = false;
		base.transform.localPosition = new Vector3(PlayerPrefs.GetFloat("LuaNotepadX", 0f), PlayerPrefs.GetFloat("LuaNotepadY", 0f), base.transform.localPosition.z);
		base.GetComponent<UIWidget>().width = PlayerPrefs.GetInt("LuaNotepadWidth", 930);
		base.GetComponent<UIWidget>().height = PlayerPrefs.GetInt("LuaNotepadHeight", 700);
		base.GetComponent<UISprite>().enabled = true;
		UIEventListener uieventListener3 = UIEventListener.Get(this.resize.gameObject);
		uieventListener3.onDrag = (UIEventListener.VectorDelegate)Delegate.Combine(uieventListener3.onDrag, new UIEventListener.VectorDelegate(this.LuaResize));
		EventDelegate.Add(this.CustomUIAssetButton.onClick, new EventDelegate.Callback(this.OnClickCustomUI));
		EventDelegate.Add(this.ModeToggleButtonLua.onClick, new EventDelegate.Callback(this.OnClickModeToggleLua));
		EventDelegate.Add(this.ModeToggleButtonUI.onClick, new EventDelegate.Callback(this.OnClickModeToggleUI));
		EventManager.OnLuaObjectSpawn += this.OnLuaObjectSpawn;
		EventManager.OnLuaObjectDestroy += this.OnLuaObjectDestroy;
		this.UpdateModeToggleButton();
	}

	// Token: 0x06002522 RID: 9506 RVA: 0x00105EE0 File Offset: 0x001040E0
	protected override void OnDestroy()
	{
		base.OnDestroy();
		base.gameObject.SetActive(false);
		UIEventListener uieventListener = UIEventListener.Get(this.savePlay);
		uieventListener.onClick = (UIEventListener.VoidDelegate)Delegate.Remove(uieventListener.onClick, new UIEventListener.VoidDelegate(this.OnSavePlayClicked));
		UIEventListener uieventListener2 = UIEventListener.Get(this.autoRun);
		uieventListener2.onClick = (UIEventListener.VoidDelegate)Delegate.Remove(uieventListener2.onClick, new UIEventListener.VoidDelegate(this.OnAutoRunClicked));
		if (this.resize != null)
		{
			UIEventListener uieventListener3 = UIEventListener.Get(this.resize.gameObject);
			uieventListener3.onDrag = (UIEventListener.VectorDelegate)Delegate.Remove(uieventListener3.onDrag, new UIEventListener.VectorDelegate(this.LuaResize));
		}
		EventDelegate.Remove(this.CustomUIAssetButton.onClick, new EventDelegate.Callback(this.OnClickCustomUI));
		EventDelegate.Remove(this.ModeToggleButtonLua.onClick, new EventDelegate.Callback(this.OnClickModeToggleLua));
		EventDelegate.Remove(this.ModeToggleButtonUI.onClick, new EventDelegate.Callback(this.OnClickModeToggleUI));
		EventManager.OnLuaObjectSpawn -= this.OnLuaObjectSpawn;
		EventManager.OnLuaObjectDestroy -= this.OnLuaObjectDestroy;
	}

	// Token: 0x06002523 RID: 9507 RVA: 0x00106010 File Offset: 0x00104210
	private void OnClickCustomUI()
	{
		if (this.SelectedHighlighter)
		{
			NetworkSingleton<NetworkUI>.Instance.GUIUIAssets.Show(this.SelectedHighlighter.GetComponent<XmlUIScript>());
			return;
		}
		NetworkSingleton<NetworkUI>.Instance.GUIUIAssets.Show(LuaGlobalScriptManager.Instance.XmlUI);
	}

	// Token: 0x06002524 RID: 9508 RVA: 0x0010605E File Offset: 0x0010425E
	private void OnClickModeToggleLua()
	{
		this.ChangeMode(UILuaNotepad.Mode.Lua);
	}

	// Token: 0x06002525 RID: 9509 RVA: 0x00106067 File Offset: 0x00104267
	private void OnClickModeToggleUI()
	{
		this.ChangeMode(UILuaNotepad.Mode.UI);
	}

	// Token: 0x06002526 RID: 9510 RVA: 0x00106070 File Offset: 0x00104270
	private void OnDrag(Vector2 delta)
	{
		PlayerPrefs.SetInt("LuaNotepadWidth", base.GetComponent<UIWidget>().width);
		PlayerPrefs.SetInt("LuaNotepadHeight", base.GetComponent<UIWidget>().height);
		PlayerPrefs.SetFloat("LuaNotepadX", base.transform.localPosition.x);
		PlayerPrefs.SetFloat("LuaNotepadY", base.transform.localPosition.y);
	}

	// Token: 0x06002527 RID: 9511 RVA: 0x001060DC File Offset: 0x001042DC
	private void LuaResize(GameObject go, Vector2 vec2)
	{
		PlayerPrefs.SetInt("LuaNotepadWidth", base.GetComponent<UIWidget>().width);
		PlayerPrefs.SetInt("LuaNotepadHeight", base.GetComponent<UIWidget>().height);
		PlayerPrefs.SetFloat("LuaNotepadX", base.transform.localPosition.x);
		PlayerPrefs.SetFloat("LuaNotepadY", base.transform.localPosition.y);
		base.OnResize();
	}

	// Token: 0x06002528 RID: 9512 RVA: 0x0010614D File Offset: 0x0010434D
	private void InvokeLuaResize()
	{
		base.OnResize();
	}

	// Token: 0x06002529 RID: 9513 RVA: 0x00106158 File Offset: 0x00104358
	protected override void OnTabBodySelect(GameObject go, bool select)
	{
		base.OnTabBodySelect(go, select);
		if (!select && this.currentIndex >= 0)
		{
			UITab uitab = this.tabObjects[this.currentIndex];
			if (this.currentIndex == 0)
			{
				if (this.currentMode == UILuaNotepad.Mode.Lua)
				{
					LuaGlobalScriptManager.Instance.script_code = uitab.body;
					return;
				}
				LuaGlobalScriptManager.Instance.XmlUI.xmlui_code = uitab.body;
				return;
			}
			else if (this.scriptsAndTabs.ContainsKey(this.currentIndex))
			{
				LuaScript luaScript = this.scriptsAndTabs[this.currentIndex];
				if (this.currentMode == UILuaNotepad.Mode.Lua)
				{
					luaScript.script_code = uitab.body;
					return;
				}
				luaScript.XmlUI.xmlui_code = uitab.body;
			}
		}
	}

	// Token: 0x0600252A RID: 9514 RVA: 0x00106214 File Offset: 0x00104414
	protected override void OnTabClicked(GameObject go)
	{
		base.OnTabClicked(go);
		this.SelectedHighlighter = null;
		UITab component = go.GetComponent<UITab>();
		LuaScript luaScript = null;
		if (this.scriptsAndTabs.TryGetValue(component.id, out luaScript))
		{
			this.SelectedHighlighter = luaScript.gameObject.GetComponent<Highlighter>();
		}
		int id = component.id;
	}

	// Token: 0x0600252B RID: 9515 RVA: 0x00106267 File Offset: 0x00104467
	private void scrolled()
	{
		Debug.Log("Dragging! " + Time.time);
	}

	// Token: 0x0600252C RID: 9516 RVA: 0x00106282 File Offset: 0x00104482
	private void Update()
	{
		if (this.SelectedHighlighter)
		{
			this.SelectedHighlighter.On(Color.blue);
		}
	}

	// Token: 0x0600252D RID: 9517 RVA: 0x001062A4 File Offset: 0x001044A4
	public void Init(GameObject selectedObject)
	{
		base.gameObject.SetActive(true);
		this.tabTitle.enabled = false;
		base.ClearTabs();
		base.NoTabSelected();
		this.scriptsAndTabs.Clear();
		NetworkPhysicsObject networkPhysicsObject = null;
		UITab uitab2;
		if (this.currentMode == UILuaNotepad.Mode.Lua)
		{
			UITab uitab = new UITab();
			uitab.allowLocking = false;
			uitab.allowDelete = false;
			uitab.title = "Global";
			uitab.VisibleColor = Singleton<UIPalette>.Instance.CurrentThemeColours[UIPalette.UI.Motif];
			uitab.body = LuaGlobalScriptManager.Instance.script_code;
			uitab2 = this.AddTab(uitab, 0);
			uitab.allowDelete = true;
		}
		else
		{
			uitab2 = this.AddTab(new UITab
			{
				allowLocking = false,
				allowDelete = false,
				title = "[UI] Global",
				VisibleColor = Singleton<UIPalette>.Instance.CurrentThemeColours[UIPalette.UI.Motif],
				body = LuaGlobalScriptManager.Instance.XmlUI.xmlui_code
			}, 0);
		}
		uitab2.transform.GetChild(1).GetComponent<UISprite>().ThemeAs = UIPalette.UI.Motif;
		if (selectedObject != null)
		{
			networkPhysicsObject = selectedObject.GetComponent<NetworkPhysicsObject>();
			LuaGameObjectScript luaGameObjectScript = networkPhysicsObject.luaGameObjectScript;
			uitab2 = this.AddLuaGameObject(luaGameObjectScript, false, true);
			luaGameObjectScript.enabled = true;
		}
		for (int i = 0; i < NetworkSingleton<ManagerPhysicsObject>.Instance.GrabbableNPOs.Count; i++)
		{
			NetworkPhysicsObject networkPhysicsObject2 = NetworkSingleton<ManagerPhysicsObject>.Instance.GrabbableNPOs[i];
			if (!(networkPhysicsObject2 == networkPhysicsObject))
			{
				LuaGameObjectScript luaGameObjectScript2 = networkPhysicsObject2.luaGameObjectScript;
				this.AddLuaGameObject(luaGameObjectScript2, false, false);
			}
		}
		this.Reposition();
		if (this.tabObjectList.Count > 0)
		{
			base.StartCoroutine(this.DelaySelectTab(uitab2.gameObject));
		}
		this.InvokeLuaResize();
		base.Invoke("InvokeLuaResize", 0.2f);
		base.Invoke("InvokeLuaResize", 0.5f);
	}

	// Token: 0x0600252E RID: 9518 RVA: 0x00106484 File Offset: 0x00104684
	protected override UITab AddTab(UITab uiTab, int id)
	{
		UITab uitab = base.AddTab(uiTab, id);
		uitab.GetComponent<UILabel>().width += 100;
		Vector2 size = uitab.GetComponent<BoxCollider2D>().size;
		size.x += 100f;
		uitab.GetComponent<BoxCollider2D>().size = size;
		uitab.transform.GetChild(0).GetChild(0).GetComponent<UISprite>().width += 100;
		uitab.transform.GetChild(1).GetComponent<UISprite>().width += 100;
		UILabel component = uitab.GetComponent<UILabel>();
		component.bitmapFont = this.tabBody.gameObject.GetComponent<UILabel>().bitmapFont;
		component.effectStyle = UILabel.Effect.None;
		component.ThemeAs = UIPalette.UI.Label;
		return uitab;
	}

	// Token: 0x0600252F RID: 9519 RVA: 0x00106548 File Offset: 0x00104748
	private IEnumerator DelaySelectTab(GameObject selectT)
	{
		yield return null;
		this.OnTabClicked(selectT);
		yield break;
	}

	// Token: 0x06002530 RID: 9520 RVA: 0x0010655E File Offset: 0x0010475E
	public override void OnDeleteTab(UITab to)
	{
		this.tabToDelete = to;
		UIDialog.Show(Language.Translate("Are you sure you want to delete script {0}?", to.title), "Yes", "No", new Action(this.DeleteTab), null);
	}

	// Token: 0x06002531 RID: 9521 RVA: 0x00106594 File Offset: 0x00104794
	protected override void DeleteTab()
	{
		if (this.tabObjects.ContainsKey(this.tabToDelete.id))
		{
			LuaScript luaScript = this.scriptsAndTabs[this.tabToDelete.id];
			luaScript.script_code = "";
			luaScript.enabled = false;
			this.scriptsAndTabs.Remove(this.tabToDelete.id);
		}
		base.DeleteTab();
	}

	// Token: 0x06002532 RID: 9522 RVA: 0x00106600 File Offset: 0x00104800
	public UITab AddLuaGameObject(LuaGameObjectScript luaGameObject, bool bReposition = false, bool bForceAdd = false)
	{
		if (!bForceAdd && (!base.gameObject.activeSelf || this.scriptsAndTabs.ContainsValue(luaGameObject) || (string.IsNullOrEmpty(luaGameObject.script_code) && string.IsNullOrEmpty(luaGameObject.XmlUI.xmlui_code))))
		{
			return null;
		}
		UITab uitab2;
		if (this.currentMode == UILuaNotepad.Mode.Lua)
		{
			UITab uitab = new UITab();
			uitab.allowLocking = false;
			uitab.allowDelete = false;
			uitab.title = TTSUtilities.CleanName(luaGameObject.NPO) + " - " + luaGameObject.NPO.GUID;
			uitab.body = luaGameObject.script_code;
			uitab.VisibleColor = Singleton<UIPalette>.Instance.CurrentThemeColours[UIPalette.UI.ButtonNeutral];
			int nextId = base.GetNextId();
			uitab2 = this.AddTab(uitab, nextId);
			this.scriptsAndTabs.Add(nextId, luaGameObject);
		}
		else
		{
			UITab uitab3 = new UITab();
			uitab3.allowLocking = false;
			uitab3.allowDelete = false;
			uitab3.title = "[UI] " + TTSUtilities.CleanName(luaGameObject.NPO) + " - " + luaGameObject.NPO.GUID;
			uitab3.body = luaGameObject.XmlUI.xmlui_code;
			uitab3.VisibleColor = Singleton<UIPalette>.Instance.CurrentThemeColours[UIPalette.UI.ButtonNeutral];
			int nextId2 = base.GetNextId();
			uitab2 = this.AddTab(uitab3, nextId2);
			this.scriptsAndTabs.Add(nextId2, luaGameObject);
		}
		uitab2.transform.GetChild(1).GetComponent<UISprite>().ThemeAs = UIPalette.UI.ButtonNeutral;
		if (bReposition)
		{
			this.Reposition();
		}
		return uitab2;
	}

	// Token: 0x06002533 RID: 9523 RVA: 0x00106788 File Offset: 0x00104988
	public void RemoveLuaGameObject(LuaGameObjectScript luaGameObject, bool bReposition = false)
	{
		if (this == null || !base.gameObject.activeSelf)
		{
			return;
		}
		foreach (KeyValuePair<int, LuaScript> keyValuePair in this.scriptsAndTabs)
		{
			UITab tabToDelete;
			if (luaGameObject == keyValuePair.Value && this.tabObjects.TryGetValue(keyValuePair.Key, out tabToDelete))
			{
				this.tabToDelete = tabToDelete;
				this.DeleteTab();
				break;
			}
		}
		if (bReposition)
		{
			this.Reposition();
		}
	}

	// Token: 0x06002534 RID: 9524 RVA: 0x00106828 File Offset: 0x00104A28
	private void Reposition()
	{
		this.Reset();
		base.Invoke("Reset", 0.25f);
		base.Invoke("Reset", 0.5f);
		base.Invoke("Reset", 0.75f);
		base.Invoke("Reset", 1f);
	}

	// Token: 0x06002535 RID: 9525 RVA: 0x0010687C File Offset: 0x00104A7C
	private void Reset()
	{
		this.tabNameGrid.Reposition();
		this.tabNameGrid.repositionNow = true;
		this.tabScrollView.ResetPosition();
		this.tabScrollView.UpdatePosition();
		this.tabScrollView.UpdateScrollbars();
		this.bodyScrollView.ResetPosition();
		this.bodyScrollView.UpdatePosition();
		this.bodyScrollView.UpdateScrollbars();
	}

	// Token: 0x06002536 RID: 9526 RVA: 0x001068E2 File Offset: 0x00104AE2
	public void OnSavePlayClicked(GameObject go)
	{
		NetworkSingleton<ManagerPhysicsObject>.Instance.Recompile(true);
	}

	// Token: 0x06002537 RID: 9527 RVA: 0x001068EF File Offset: 0x00104AEF
	public void OnAutoRunClicked(GameObject go)
	{
		NetworkSingleton<NetworkUI>.Instance.bAutoRunScripts = this.autoRun.GetComponentInChildren<UIToggle>().value;
		PlayerPrefs.SetString("bAutoRunScripts", NetworkSingleton<NetworkUI>.Instance.bAutoRunScripts.ToString());
	}

	// Token: 0x06002538 RID: 9528 RVA: 0x00106924 File Offset: 0x00104B24
	private void ChangeMode(UILuaNotepad.Mode mode)
	{
		this.currentMode = mode;
		this.UpdateModeToggleButton();
		if (this.SelectedHighlighter)
		{
			this.Init(this.SelectedHighlighter.gameObject);
			return;
		}
		this.Init(null);
	}

	// Token: 0x06002539 RID: 9529 RVA: 0x0010695C File Offset: 0x00104B5C
	private void UpdateModeToggleButton()
	{
		if (this.currentMode == UILuaNotepad.Mode.Lua)
		{
			this.ModeToggleButtonLua.GetComponentInChildren<UIButton>().isEnabled = false;
			this.ModeToggleButtonUI.GetComponentInChildren<UIButton>().isEnabled = true;
			return;
		}
		this.ModeToggleButtonLua.GetComponentInChildren<UIButton>().isEnabled = true;
		this.ModeToggleButtonUI.GetComponentInChildren<UIButton>().isEnabled = false;
	}

	// Token: 0x0600253A RID: 9530 RVA: 0x001069B6 File Offset: 0x00104BB6
	private void OnLuaObjectSpawn(LuaGameObjectScript LGOS)
	{
		this.AddLuaGameObject(LGOS, true, false);
	}

	// Token: 0x0600253B RID: 9531 RVA: 0x001069C2 File Offset: 0x00104BC2
	private void OnLuaObjectDestroy(LuaGameObjectScript LGOS)
	{
		this.RemoveLuaGameObject(LGOS, true);
	}

	// Token: 0x04001820 RID: 6176
	private Highlighter SelectedHighlighter;

	// Token: 0x04001821 RID: 6177
	protected Dictionary<int, LuaScript> scriptsAndTabs = new Dictionary<int, LuaScript>();

	// Token: 0x04001822 RID: 6178
	public GameObject savePlay;

	// Token: 0x04001823 RID: 6179
	public GameObject autoRun;

	// Token: 0x04001824 RID: 6180
	private Colour backgroundColour;

	// Token: 0x04001825 RID: 6181
	public UIButton CustomUIAssetButton;

	// Token: 0x04001826 RID: 6182
	public UIButton ModeToggleButtonLua;

	// Token: 0x04001827 RID: 6183
	public UIButton ModeToggleButtonUI;

	// Token: 0x04001828 RID: 6184
	private UILuaNotepad.Mode currentMode;

	// Token: 0x0200076C RID: 1900
	private enum Mode
	{
		// Token: 0x04002BDE RID: 11230
		Lua,
		// Token: 0x04002BDF RID: 11231
		UI
	}
}
