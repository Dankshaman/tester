using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000291 RID: 657
public class UIColorWheel : MonoBehaviour
{
	// Token: 0x06002195 RID: 8597 RVA: 0x000F1D8C File Offset: 0x000EFF8C
	private void Awake()
	{
		this.white.color = Colour.White;
		this.buttons.Add(this.white);
		this.brown.color = Colour.Brown;
		this.buttons.Add(this.brown);
		this.red.color = Colour.Red;
		this.buttons.Add(this.red);
		this.orange.color = Colour.Orange;
		this.buttons.Add(this.orange);
		this.yellow.color = Colour.Yellow;
		this.buttons.Add(this.yellow);
		this.green.color = Colour.Green;
		this.buttons.Add(this.green);
		this.teal.color = Colour.Teal;
		this.buttons.Add(this.teal);
		this.blue.color = Colour.Blue;
		this.buttons.Add(this.blue);
		this.purple.color = Colour.Purple;
		this.buttons.Add(this.purple);
		this.pink.color = Colour.Pink;
		this.buttons.Add(this.pink);
		if (this.grey != null)
		{
			this.grey.color = Colour.Grey;
			this.buttons.Add(this.grey);
		}
		this.black.color = Colour.Black;
		this.buttons.Add(this.black);
		for (int i = 0; i < this.buttons.Count; i++)
		{
			UIButton component = this.buttons[i].GetComponent<UIButton>();
			UIPalette.UI ui = Colour.UIFromColour(this.buttons[i].color);
			Colour colour = Singleton<UIPalette>.Instance.CurrentThemeColours[ui];
			component.defaultColor = colour;
			component.hover = colour;
			component.pressed = colour;
			component.disabledColor = colour;
			component.ThemeNormalAs = ui;
			component.ThemeHoverAs = ui;
			component.ThemePressedAs = ui;
			component.ThemeDisabledAs = ui;
			component.ThemeNormalAsSetting = ui;
			component.ThemeHoverAsSetting = ui;
			component.ThemePressedAsSetting = ui;
			component.ThemeDisabledAsSetting = ui;
			UIEventListener uieventListener = UIEventListener.Get(this.buttons[i].gameObject);
			uieventListener.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(uieventListener.onClick, new UIEventListener.VoidDelegate(this.OnColorClicked));
		}
	}

	// Token: 0x06002196 RID: 8598 RVA: 0x000F2068 File Offset: 0x000F0268
	private void OnDestroy()
	{
		for (int i = 0; i < this.buttons.Count; i++)
		{
			UIEventListener uieventListener = UIEventListener.Get(this.buttons[i].gameObject);
			uieventListener.onClick = (UIEventListener.VoidDelegate)Delegate.Remove(uieventListener.onClick, new UIEventListener.VoidDelegate(this.OnColorClicked));
		}
	}

	// Token: 0x06002197 RID: 8599 RVA: 0x000F20C4 File Offset: 0x000F02C4
	private void OnColorClicked(GameObject go)
	{
		if (!PlayerScript.Pointer)
		{
			return;
		}
		string label = go.name.Split(new char[]
		{
			' '
		})[0];
		Colour colour = Colour.ColourFromLabel(label);
		switch (this.colorWheelMode)
		{
		case ColorWheelMode.HiddenZone:
		{
			GameObject infoHiddenZoneGO = PlayerScript.PointerScript.InfoHiddenZoneGO;
			if (infoHiddenZoneGO)
			{
				infoHiddenZoneGO.GetComponent<HiddenZone>().SyncSetZoneColor(label);
				PlayerScript.PointerScript.ResetHiddenZoneObject();
				return;
			}
			return;
		}
		case ColorWheelMode.Hand:
		{
			GameObject infoHandObject = PlayerScript.PointerScript.InfoHandObject;
			if (infoHandObject)
			{
				infoHandObject.GetComponent<HandZone>().TriggerLabel = label;
				PlayerScript.PointerScript.ResetHandObject();
				return;
			}
			return;
		}
		case ColorWheelMode.Card:
			if (!PlayerScript.PointerScript.InfoObject)
			{
				return;
			}
			using (List<GameObject>.Enumerator enumerator = PlayerScript.PointerScript.GetSelectedObjects(-1, true, false).GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					GameObject gameObject = enumerator.Current;
					if (gameObject)
					{
						NetworkSingleton<ManagerPhysicsObject>.Instance.DealObjectToColor(gameObject.GetComponent<NetworkPhysicsObject>().ID, label, 1, 0);
					}
				}
				return;
			}
			break;
		case ColorWheelMode.Notepad:
			break;
		case ColorWheelMode.FogOfWarRevealer:
			if (PlayerScript.PointerScript.InfoObject)
			{
				using (List<GameObject>.Enumerator enumerator = PlayerScript.PointerScript.GetSelectedObjects(-1, true, false).GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						GameObject GO = enumerator.Current;
						if (GO)
						{
							NetworkPhysicsObject npo = NetworkSingleton<ManagerPhysicsObject>.Instance.NPOFromGO(GO);
							npo.fogOfWarRevealer.Color = label;
							Wait.Condition(delegate
							{
								NetworkSingleton<NetworkUI>.Instance.GUIContextualFogOfWarRevealColorSprite.color = GO.GetComponent<FogOfWarRevealer>().AColor;
							}, () => npo.fogOfWarRevealer.Color == label, float.PositiveInfinity, null);
						}
					}
					return;
				}
				goto IL_240;
			}
			return;
		case ColorWheelMode.HandReveal:
			goto IL_240;
		default:
			return;
		}
		this.Notebook.OnColorFilterSelected(colour);
		return;
		IL_240:
		NetworkSingleton<NetworkUI>.Instance.GUIContextualHandRevealColors.SetActive(false);
		NetworkSingleton<NetworkUI>.Instance.GUIContextualMenu.SetActive(false);
		if (colour != Colour.Grey)
		{
			NetworkSingleton<NetworkUI>.Instance.handZoneToReveal.RevealHandToPlayer(colour);
			NetworkSingleton<NetworkUI>.Instance.EnableGUIEndHandRevealButton(colour);
		}
	}

	// Token: 0x040014E4 RID: 5348
	public UISprite white;

	// Token: 0x040014E5 RID: 5349
	public UISprite brown;

	// Token: 0x040014E6 RID: 5350
	public UISprite red;

	// Token: 0x040014E7 RID: 5351
	public UISprite orange;

	// Token: 0x040014E8 RID: 5352
	public UISprite yellow;

	// Token: 0x040014E9 RID: 5353
	public UISprite green;

	// Token: 0x040014EA RID: 5354
	public UISprite teal;

	// Token: 0x040014EB RID: 5355
	public UISprite blue;

	// Token: 0x040014EC RID: 5356
	public UISprite purple;

	// Token: 0x040014ED RID: 5357
	public UISprite pink;

	// Token: 0x040014EE RID: 5358
	public UISprite grey;

	// Token: 0x040014EF RID: 5359
	public UISprite black;

	// Token: 0x040014F0 RID: 5360
	public ColorWheelMode colorWheelMode;

	// Token: 0x040014F1 RID: 5361
	public UINotebook Notebook;

	// Token: 0x040014F2 RID: 5362
	private List<UISprite> buttons = new List<UISprite>();
}
