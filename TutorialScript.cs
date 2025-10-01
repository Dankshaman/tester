using System;
using System.Collections.Generic;
using System.Diagnostics;
using NewNet;
using UnityEngine;
using Valve.VR;

// Token: 0x02000263 RID: 611
public class TutorialScript : MonoBehaviour
{
	// Token: 0x17000449 RID: 1097
	// (get) Token: 0x0600203E RID: 8254 RVA: 0x000E6CD1 File Offset: 0x000E4ED1
	// (set) Token: 0x0600203F RID: 8255 RVA: 0x000E6CD9 File Offset: 0x000E4ED9
	private TutorialScript.TutorialMenu CurrentMenu
	{
		get
		{
			return this.currentmenu;
		}
		set
		{
			if (this.currentmenu != value)
			{
				this.currentmenu = value;
				this.SetTutorialText();
				this.PlayChatSound();
			}
		}
	}

	// Token: 0x06002040 RID: 8256 RVA: 0x000E6CF8 File Offset: 0x000E4EF8
	private void OnEnable()
	{
		EventManager.OnObjectRandomize += this.OnRandomize;
		this.currentControlType = zInput.CurrentControlType;
		Chat.Log("Current control type is " + this.currentControlType + ".", ChatMessageType.All);
		this.uiLabel = base.GetComponent<UILabel>();
		this.NetworkInstance = NetworkSingleton<NetworkUI>.Instance;
		this.GameModeInstance = NetworkSingleton<GameMode>.Instance;
		this.CameraInstance = Singleton<CameraController>.Instance;
		this.Card = Network.Instantiate(this.GameModeInstance.Card, new Vector3(0f, 2f, 0f), this.GameModeInstance.Card.transform.rotation, default(NetworkPlayer));
		int num = UnityEngine.Random.Range(0, 51);
		NetworkSingleton<CardManagerScript>.Instance.SetupCard(this.Card, num, -1, false);
		this.Card.GetComponent<CardScript>().card_id_ = num;
		this.TutorialTimeHolder = Time.time;
		Wait.Frames(delegate
		{
			NetworkSingleton<NetworkUI>.Instance.GUITopBar.gameObject.SetActive(false);
			NetworkSingleton<SaveManager>.Instance.RewindSaveEnable = false;
		}, 1);
		this.timer = new Stopwatch();
		this.timer.Start();
		this.SetTutorialText();
	}

	// Token: 0x06002041 RID: 8257 RVA: 0x000E6E30 File Offset: 0x000E5030
	private void OnDestroy()
	{
		EventManager.OnObjectRandomize -= this.OnRandomize;
	}

	// Token: 0x06002042 RID: 8258 RVA: 0x000E6E43 File Offset: 0x000E5043
	private void OnRandomize(NetworkPhysicsObject npo, string player)
	{
		if (npo == this.Deck.GetComponent<NetworkPhysicsObject>())
		{
			this.DeckShuffled = true;
			return;
		}
		if (npo == this.Die.GetComponent<NetworkPhysicsObject>())
		{
			this.DiceRolled = true;
		}
	}

	// Token: 0x06002043 RID: 8259 RVA: 0x000E6E7A File Offset: 0x000E507A
	private void PlayChatSound()
	{
		this.NetworkInstance.GetComponent<SoundScript>().PlayGUISound(this.NetworkInstance.ChatSound, 0.7f, 1f);
	}

	// Token: 0x06002044 RID: 8260 RVA: 0x000E6EA4 File Offset: 0x000E50A4
	private string KeyNameToHotkey(string name)
	{
		ControlType controlType = this.currentControlType;
		if (controlType == ControlType.Controller)
		{
			name += " (Controller)";
		}
		string str = "";
		for (int i = 0; i < cInput.length; i++)
		{
			if (cInput.GetText(i) == name)
			{
				str = cInput.GetText(i, 1);
			}
		}
		return "<" + str + ">";
	}

	// Token: 0x06002045 RID: 8261 RVA: 0x000E6F08 File Offset: 0x000E5108
	private void SetTutorialText()
	{
		EventManager.TriggerUnityAnalytic("Tutorial_Step_" + this.CurrentMenu.ToString(), null, 0);
		string arg = null;
		string arg2 = null;
		if (VRHMD.isVR)
		{
			arg = SteamVR_Actions.default_grab.GetLocalizedOriginPart(SteamVR_Input_Sources.Any, new EVRInputStringBits[]
			{
				EVRInputStringBits.VRInputString_InputSource
			});
			arg2 = SteamVR_Actions.default_enable_move.GetLocalizedOriginPart(SteamVR_Input_Sources.Any, new EVRInputStringBits[]
			{
				EVRInputStringBits.VRInputString_InputSource
			});
		}
		switch (this.CurrentMenu)
		{
		case TutorialScript.TutorialMenu.Intro:
			EventManager.TriggerUnityAnalytic("Tutorial_Start", "ControlType", this.currentControlType.ToString(), 0);
			this.uiLabel.text = Language.Translate("Welcome to the tutorial!") + "\n";
			switch (this.currentControlType)
			{
			case ControlType.Keyboard:
			{
				UILabel uilabel = this.uiLabel;
				uilabel.text += Language.Translate("Please pickup the card with <Left Mouse Button>.");
				return;
			}
			case ControlType.Controller:
			{
				UILabel uilabel2 = this.uiLabel;
				uilabel2.text += Language.Translate("Please pickup the card with <Right Bumper>.");
				return;
			}
			case ControlType.Touch:
			{
				UILabel uilabel3 = this.uiLabel;
				uilabel3.text += Language.Translate("Please pickup the card using your finger.");
				return;
			}
			case ControlType.VR:
			{
				UILabel uilabel4 = this.uiLabel;
				uilabel4.text += Language.Translate("Please pickup the card using <{0}> on either controller.", arg);
				return;
			}
			default:
				return;
			}
			break;
		case TutorialScript.TutorialMenu.Stack:
			this.uiLabel.text = Language.Translate("Good! Now time to add that card to the deck.") + "\n";
			switch (this.currentControlType)
			{
			case ControlType.Keyboard:
			{
				UILabel uilabel5 = this.uiLabel;
				uilabel5.text += Language.Translate("You will need to flip it with <F> so it stacks. Then drop the card on the deck.");
				return;
			}
			case ControlType.Controller:
			{
				UILabel uilabel6 = this.uiLabel;
				uilabel6.text += Language.Translate("You will need to flip it with <X> so it stacks. Then drop the card on top of the deck.");
				return;
			}
			case ControlType.Touch:
			{
				UILabel uilabel7 = this.uiLabel;
				uilabel7.text += Language.Translate("You will need to flip it with double tap so it stacks. Then drop the card on top of the deck.");
				return;
			}
			case ControlType.VR:
			{
				UILabel uilabel8 = this.uiLabel;
				uilabel8.text += Language.Translate("You will need to flip it with <Pad Up> so it stacks. Then drop the card on top of the deck.");
				return;
			}
			default:
				return;
			}
			break;
		case TutorialScript.TutorialMenu.RemoveObject:
			this.uiLabel.text = Language.Translate("Great stacking! Now pull one card from the deck.") + "\n";
			switch (this.currentControlType)
			{
			case ControlType.Keyboard:
			{
				UILabel uilabel9 = this.uiLabel;
				uilabel9.text += Language.Translate("<Left Click> and quickly drag away from the deck.");
				return;
			}
			case ControlType.Controller:
			{
				UILabel uilabel10 = this.uiLabel;
				uilabel10.text += Language.Translate("<Right Bumper> and quickly drag away from the deck.");
				return;
			}
			case ControlType.Touch:
			{
				UILabel uilabel11 = this.uiLabel;
				uilabel11.text += Language.Translate("Quickly drag away from the deck.");
				return;
			}
			case ControlType.VR:
			{
				UILabel uilabel12 = this.uiLabel;
				uilabel12.text += Language.Translate("<{0}> and quickly drag away from the deck.", arg);
				return;
			}
			default:
				return;
			}
			break;
		case TutorialScript.TutorialMenu.MoveStack:
			this.uiLabel.text = Language.Translate("Perfect! Now let's move the entire deck of cards.") + "\n";
			switch (this.currentControlType)
			{
			case ControlType.Keyboard:
			{
				UILabel uilabel13 = this.uiLabel;
				uilabel13.text += Language.Translate("<Left Click> and hold your mouse on the deck.");
				return;
			}
			case ControlType.Controller:
			{
				UILabel uilabel14 = this.uiLabel;
				uilabel14.text += Language.Translate("<Right Bumper> and hold your pointer on the deck.");
				return;
			}
			case ControlType.Touch:
			{
				UILabel uilabel15 = this.uiLabel;
				uilabel15.text += Language.Translate("Press and hold your finder on the deck for a second then drag.");
				return;
			}
			case ControlType.VR:
			{
				UILabel uilabel16 = this.uiLabel;
				uilabel16.text += Language.Translate("<{0}> and hold your hand on the deck.", arg);
				return;
			}
			default:
				return;
			}
			break;
		case TutorialScript.TutorialMenu.Shuffle:
			this.uiLabel.text = Language.Translate("Nice! Now let's shuffle the deck of cards.") + "\n";
			switch (this.currentControlType)
			{
			case ControlType.Keyboard:
			{
				UILabel uilabel17 = this.uiLabel;
				uilabel17.text += Language.Translate("While holding the deck shake your mouse.");
				return;
			}
			case ControlType.Controller:
			{
				UILabel uilabel18 = this.uiLabel;
				uilabel18.text += Language.Translate("While holding the deck shake your pointer.");
				return;
			}
			case ControlType.Touch:
			{
				UILabel uilabel19 = this.uiLabel;
				uilabel19.text += Language.Translate("While holding the deck shake it with your finger.");
				return;
			}
			case ControlType.VR:
			{
				UILabel uilabel20 = this.uiLabel;
				uilabel20.text += Language.Translate("While holding the deck shake your hand.");
				return;
			}
			default:
				return;
			}
			break;
		case TutorialScript.TutorialMenu.Hand:
			this.uiLabel.text = Language.Translate("Great shuffling! Now let's put a card in our hand. Drop a single card above the white name on the table. When a card is in your hand other players cannot see what it is.");
			return;
		case TutorialScript.TutorialMenu.Die:
			this.uiLabel.text = Language.Translate("You are a natural! Now let's roll some dice.") + "\n";
			switch (this.currentControlType)
			{
			case ControlType.Keyboard:
			{
				UILabel uilabel21 = this.uiLabel;
				uilabel21.text += Language.Translate("While holding the die shake your mouse.");
				return;
			}
			case ControlType.Controller:
			{
				UILabel uilabel22 = this.uiLabel;
				uilabel22.text += Language.Translate("While holding the die shake your pointer.");
				return;
			}
			case ControlType.Touch:
			{
				UILabel uilabel23 = this.uiLabel;
				uilabel23.text += Language.Translate("While holding the die shake it with your finger.");
				return;
			}
			case ControlType.VR:
			{
				UILabel uilabel24 = this.uiLabel;
				uilabel24.text += Language.Translate("Hover over the die and press <Pad Right>.");
				return;
			}
			default:
				return;
			}
			break;
		case TutorialScript.TutorialMenu.CameraRotate:
			this.uiLabel.text = Language.Translate("Nice shaking! Now let's rotate the camera.") + "\n";
			switch (this.currentControlType)
			{
			case ControlType.Keyboard:
			{
				UILabel uilabel25 = this.uiLabel;
				uilabel25.text += Language.Translate("Hold down <Right Mouse Button> and move your mouse.");
				return;
			}
			case ControlType.Controller:
			{
				UILabel uilabel26 = this.uiLabel;
				uilabel26.text += Language.Translate("Use your <Right Stick>.");
				return;
			}
			case ControlType.Touch:
			{
				UILabel uilabel27 = this.uiLabel;
				uilabel27.text += Language.Translate("Use two fingers in a dragging motion.");
				return;
			}
			case ControlType.VR:
				this.uiLabel.text = Language.Translate("Nice! Now let's spin our view. Hold <{0}> on both controllers, then move your hands as if you were turning a wheel.", arg2);
				return;
			default:
				return;
			}
			break;
		case TutorialScript.TutorialMenu.CameraPan:
			this.uiLabel.text = Language.Translate("Time to move those feet!") + "\n";
			switch (this.currentControlType)
			{
			case ControlType.Keyboard:
			{
				UILabel uilabel28 = this.uiLabel;
				uilabel28.text += Language.Translate("Use <WASD> to pan the camera around.");
				return;
			}
			case ControlType.Controller:
			{
				UILabel uilabel29 = this.uiLabel;
				uilabel29.text += Language.Translate("Use <Dpad> to pan the camera around.");
				return;
			}
			case ControlType.Touch:
			{
				UILabel uilabel30 = this.uiLabel;
				uilabel30.text += Language.Translate("Drag your finger on the table or background.");
				return;
			}
			case ControlType.VR:
			{
				UILabel uilabel31 = this.uiLabel;
				uilabel31.text += Language.Translate("Hold <{0}> then drag yourself around.", arg2);
				return;
			}
			default:
				return;
			}
			break;
		case TutorialScript.TutorialMenu.CameraZoom:
			this.uiLabel.text = Language.Translate("Zoom time!") + "\n";
			switch (this.currentControlType)
			{
			case ControlType.Keyboard:
			{
				UILabel uilabel32 = this.uiLabel;
				uilabel32.text += Language.Translate("You can use the <Scroll Wheel> or <Z> to toggle zoom.");
				return;
			}
			case ControlType.Controller:
			{
				UILabel uilabel33 = this.uiLabel;
				uilabel33.text += Language.Translate("You can use the <Right Trigger> and <Left Trigger> to zoom in and out.");
				return;
			}
			case ControlType.Touch:
			{
				UILabel uilabel34 = this.uiLabel;
				uilabel34.text += Language.Translate("Use pinch to zoom in and out.");
				return;
			}
			case ControlType.VR:
			{
				UILabel uilabel35 = this.uiLabel;
				uilabel35.text += Language.Translate("Hold <{0}> on both controllers. Then move your hands closer or further apart. This will scale you up and down!", arg2);
				return;
			}
			default:
				return;
			}
			break;
		case TutorialScript.TutorialMenu.CameraReset:
			this.uiLabel.text = Language.Translate("Reset the camera!") + "\n";
			switch (this.currentControlType)
			{
			case ControlType.Keyboard:
			{
				UILabel uilabel36 = this.uiLabel;
				uilabel36.text += Language.Translate("Press <Space> to reset the camera.");
				return;
			}
			case ControlType.Controller:
			{
				UILabel uilabel37 = this.uiLabel;
				uilabel37.text += Language.Translate("Click in the <Right Stick> to reset the camera.");
				return;
			}
			case ControlType.Touch:
				this.uiLabel.text = "";
				return;
			case ControlType.VR:
			{
				UILabel uilabel38 = this.uiLabel;
				uilabel38.text += Language.Translate("Hold <{0}> down for a few seconds.", SteamVR_Actions.default_main_menu.GetLocalizedOriginPart(SteamVR_Input_Sources.Any, new EVRInputStringBits[]
				{
					EVRInputStringBits.VRInputString_InputSource
				}));
				return;
			}
			default:
				return;
			}
			break;
		case TutorialScript.TutorialMenu.ObjectZoom:
		{
			this.uiLabel.text = Language.Translate("Let's get a better look!") + "\n";
			switch (this.currentControlType)
			{
			case ControlType.Keyboard:
			{
				UILabel uilabel39 = this.uiLabel;
				uilabel39.text += Language.Translate("Hold <Alt> while mousing over an object.");
				break;
			}
			case ControlType.Controller:
			{
				UILabel uilabel40 = this.uiLabel;
				uilabel40.text += Language.Translate("Hold <Left Bumper> while hovering over an object.");
				break;
			}
			case ControlType.Touch:
			{
				UILabel uilabel41 = this.uiLabel;
				uilabel41.text += Language.Translate("Quick tap on an object.");
				break;
			}
			case ControlType.VR:
			{
				UILabel uilabel42 = this.uiLabel;
				uilabel42.text += Language.Translate("Touch <Pad Down> on a controller while hovering over an object (with either controller).");
				break;
			}
			}
			UILabel uilabel43 = this.uiLabel;
			uilabel43.text = uilabel43.text + "\n" + Language.Translate("Great for checking a dice roll or reading a card.");
			return;
		}
		case TutorialScript.TutorialMenu.ObjectRotate:
			this.uiLabel.text = Language.Translate("Great camera work! Time to rotate an object.") + "\n";
			switch (this.currentControlType)
			{
			case ControlType.Keyboard:
			{
				UILabel uilabel44 = this.uiLabel;
				uilabel44.text += Language.Translate("Grab or mouse over one of the objects, and then use <Q> or <E> to rotate it.");
				return;
			}
			case ControlType.Controller:
			{
				UILabel uilabel45 = this.uiLabel;
				uilabel45.text += Language.Translate("Use <Right Trigger> or <Left Trigger> while holding an object.");
				return;
			}
			case ControlType.Touch:
			{
				UILabel uilabel46 = this.uiLabel;
				uilabel46.text += Language.Translate("Use a two finger rotate gesture on an object.");
				return;
			}
			case ControlType.VR:
			{
				UILabel uilabel47 = this.uiLabel;
				uilabel47.text += Language.Translate("Grab one of the objects, and then use <Pad Left> or <Pad Right> to rotate it.");
				return;
			}
			default:
				return;
			}
			break;
		case TutorialScript.TutorialMenu.ObjectTap:
			this.uiLabel.text = Language.Translate("Wonderful spinning! Let's pick up more than one object.") + "\n";
			switch (this.currentControlType)
			{
			case ControlType.Keyboard:
			{
				UILabel uilabel48 = this.uiLabel;
				uilabel48.text += Language.Translate("Hold an object then press <Right Mouse> or <T> on another object.");
				return;
			}
			case ControlType.Controller:
			{
				UILabel uilabel49 = this.uiLabel;
				uilabel49.text += Language.Translate("Hold an object then press <A> on another object.");
				return;
			}
			case ControlType.Touch:
				this.uiLabel.text = "";
				return;
			case ControlType.VR:
				this.uiLabel.text = "";
				return;
			default:
				return;
			}
			break;
		case TutorialScript.TutorialMenu.BoxSelection:
		{
			this.uiLabel.text = "";
			switch (this.currentControlType)
			{
			case ControlType.Keyboard:
				this.uiLabel.text = Language.Translate("You can also grab more than one object by <Left Click> dragging a box selection.");
				break;
			case ControlType.Controller:
				this.uiLabel.text = Language.Translate("You can also grab more than one object by <Right Bumper> dragging a box selection.");
				break;
			case ControlType.Touch:
				this.uiLabel.text = Language.Translate("You can grab more than one object by Long pressing on multiple objects in a row.");
				break;
			case ControlType.VR:
				this.uiLabel.text = Language.Translate("You can grab more than one object by <{0}> dragging a box selection.", arg);
				break;
			}
			UILabel uilabel50 = this.uiLabel;
			uilabel50.text = uilabel50.text + "\n" + Language.Translate("All actions will be executed on all highlighted objects.");
			return;
		}
		case TutorialScript.TutorialMenu.Contextual:
			this.uiLabel.text = Language.Translate("Want more actions? The contextual menu has all sorts of options. You can deal cards, name objects, color tint, and much more!") + "\n";
			switch (this.currentControlType)
			{
			case ControlType.Keyboard:
			{
				UILabel uilabel51 = this.uiLabel;
				uilabel51.text += Language.Translate("<Right Click> on an object to bring it up.");
				return;
			}
			case ControlType.Controller:
			{
				UILabel uilabel52 = this.uiLabel;
				uilabel52.text += Language.Translate("<A> on an object to bring it up.");
				return;
			}
			case ControlType.Touch:
			{
				UILabel uilabel53 = this.uiLabel;
				uilabel53.text += Language.Translate("Long press on an object to bring it up.");
				return;
			}
			case ControlType.VR:
			{
				string localizedOriginPart = SteamVR_Actions.default_interface_click.GetLocalizedOriginPart(SteamVR_Input_Sources.Any, new EVRInputStringBits[]
				{
					EVRInputStringBits.VRInputString_InputSource
				});
				if (localizedOriginPart == "")
				{
					localizedOriginPart = SteamVR_Actions.default_interface_axis.GetLocalizedOriginPart(SteamVR_Input_Sources.Any, new EVRInputStringBits[]
					{
						EVRInputStringBits.VRInputString_InputSource
					});
				}
				UILabel uilabel54 = this.uiLabel;
				uilabel54.text += Language.Translate("Use <{0}> on an object to bring it up.", localizedOriginPart);
				return;
			}
			default:
				return;
			}
			break;
		case TutorialScript.TutorialMenu.ControlScheme:
			this.uiLabel.text = "";
			switch (this.currentControlType)
			{
			case ControlType.Keyboard:
			{
				UILabel uilabel55 = this.uiLabel;
				uilabel55.text += Language.Translate("<?> or </> will bring up a menu with all the controls. Open and close this menu to proceed. Optionally you can press <Esc> then click 'Help Menu'.");
				return;
			}
			case ControlType.Controller:
			{
				UILabel uilabel56 = this.uiLabel;
				uilabel56.text += Language.Translate("<Back> will bring up a menu with all the controls. Open and close the menu by hitting <Back> Twice.");
				return;
			}
			case ControlType.Touch:
				this.uiLabel.text = "";
				return;
			case ControlType.VR:
				this.uiLabel.text = "";
				return;
			default:
				return;
			}
			break;
		case TutorialScript.TutorialMenu.Flip:
			this.uiLabel.text = Language.Translate("One Last Thing! Time for some mayhem! Press the 'Flip' button on the top bar!");
			return;
		case TutorialScript.TutorialMenu.End:
			if (Time.time - this.TutorialTimeHolder <= 30f)
			{
				Achievements.Set("ACH_COMPLETE_TUTORIAL_UNDER_30_SECONDS");
			}
			Achievements.Set("ACH_COMPLETE_TUTORIAL");
			EventManager.TriggerUnityAnalytic("Tutorial_Finish", "ControlType", this.currentControlType.ToString(), 0);
			this.uiLabel.text = Language.Translate("Congratulations! You finished the tutorial! You can leave by clicking the 'Menu' button on the top bar, then click 'Main Menu'");
			return;
		default:
			return;
		}
	}

	// Token: 0x06002046 RID: 8262 RVA: 0x000E7CAF File Offset: 0x000E5EAF
	public void EndTutorial()
	{
		this.timer.Stop();
	}

	// Token: 0x06002047 RID: 8263 RVA: 0x000E7CBC File Offset: 0x000E5EBC
	private void Update()
	{
		if (this.currentControlType == ControlType.VR && !this.vrInit)
		{
			this.vrInit = true;
			Singleton<SystemConsole>.Instance.ProcessBatch("vr_hand_view_detach; vr_unbind_all; tool_grab", SystemConsole.CommandEcho.Quiet);
		}
		if (this.CurrentMenu == TutorialScript.TutorialMenu.Intro && this.Card.GetComponent<NetworkPhysicsObject>().IsHeldBySomebody)
		{
			this.CurrentMenu = TutorialScript.TutorialMenu.Stack;
			this.Deck = Network.Instantiate(this.GameModeInstance.Deck, new Vector3(-6f, 2f, 0f), this.GameModeInstance.Deck.transform.rotation, default(NetworkPlayer));
			return;
		}
		if (this.CurrentMenu == TutorialScript.TutorialMenu.Stack && !this.Card)
		{
			this.CurrentMenu = TutorialScript.TutorialMenu.RemoveObject;
			return;
		}
		if (this.CurrentMenu == TutorialScript.TutorialMenu.RemoveObject && this.Deck.GetComponent<DeckScript>().num_cards_ < 53)
		{
			this.CurrentMenu = TutorialScript.TutorialMenu.MoveStack;
			return;
		}
		if (this.CurrentMenu == TutorialScript.TutorialMenu.MoveStack && this.Deck.GetComponent<NetworkPhysicsObject>().IsHeldBySomebody)
		{
			this.DeckShuffled = false;
			this.CurrentMenu = TutorialScript.TutorialMenu.Shuffle;
			return;
		}
		if (this.CurrentMenu == TutorialScript.TutorialMenu.Shuffle)
		{
			if (this.DeckShuffled)
			{
				this.CurrentMenu = TutorialScript.TutorialMenu.Hand;
				this.WhiteHand = HandZone.GetHand("White", 0);
				return;
			}
		}
		else if (this.CurrentMenu == TutorialScript.TutorialMenu.Hand)
		{
			if (this.WhiteHand.GetComponent<HandZone>().HasHandObjects)
			{
				this.DiceRolled = false;
				this.CurrentMenu = TutorialScript.TutorialMenu.Die;
				this.Die = Network.Instantiate(this.GameModeInstance.D6, new Vector3(6f, 2f, 0f), this.GameModeInstance.Deck.transform.rotation, default(NetworkPlayer));
				return;
			}
		}
		else if (this.CurrentMenu == TutorialScript.TutorialMenu.Die)
		{
			if (this.DiceRolled)
			{
				if (this.currentControlType == ControlType.VR)
				{
					this.CurrentMenu = TutorialScript.TutorialMenu.CameraPan;
					this.PrevPosition = Singleton<VRHMD>.Instance.VRCameraRig.transform.position;
					return;
				}
				this.CurrentMenu = TutorialScript.TutorialMenu.CameraRotate;
				return;
			}
		}
		else if (this.CurrentMenu == TutorialScript.TutorialMenu.CameraRotate)
		{
			if (this.PrevCameraRot != Quaternion.identity && this.CameraInstance.transform.rotation != this.PrevCameraRot)
			{
				this.CurrentMenu = TutorialScript.TutorialMenu.CameraPan;
			}
			this.PrevCameraRot = this.CameraInstance.transform.rotation;
			if (this.currentControlType == ControlType.VR)
			{
				float num = Vector3.Distance(Singleton<VRHMD>.Instance.VRCameraRig.transform.eulerAngles, this.PrevRotation);
				if (num > 30f && num < 330f)
				{
					this.PrevScale = Singleton<VRHMD>.Instance.VRCameraRig.transform.localScale.x;
					this.CurrentMenu = TutorialScript.TutorialMenu.CameraZoom;
					return;
				}
			}
		}
		else if (this.CurrentMenu == TutorialScript.TutorialMenu.CameraPan)
		{
			if (this.PrevCameraPosition != Vector3.zero && this.PrevCameraPosition != Singleton<CameraController>.Instance.target.position)
			{
				this.CurrentMenu = TutorialScript.TutorialMenu.CameraZoom;
			}
			this.PrevCameraPosition = Singleton<CameraController>.Instance.target.position;
			if (this.currentControlType == ControlType.VR && Vector3.Distance(Singleton<VRHMD>.Instance.VRCameraRig.transform.position, this.PrevPosition) > 15f)
			{
				this.CurrentMenu = TutorialScript.TutorialMenu.CameraRotate;
				this.PrevRotation = Singleton<VRHMD>.Instance.VRCameraRig.transform.eulerAngles;
				return;
			}
		}
		else if (this.CurrentMenu == TutorialScript.TutorialMenu.CameraZoom)
		{
			if (this.PrevCameraDistance != 0f && this.PrevCameraDistance != Singleton<CameraController>.Instance.distance)
			{
				if (this.currentControlType == ControlType.Touch)
				{
					this.CurrentMenu = TutorialScript.TutorialMenu.ObjectZoom;
				}
				else
				{
					this.CurrentMenu = TutorialScript.TutorialMenu.CameraReset;
				}
			}
			this.PrevCameraDistance = Singleton<CameraController>.Instance.distance;
			if (this.currentControlType == ControlType.VR && Mathf.Abs(Singleton<VRHMD>.Instance.VRCameraRig.transform.localScale.x - this.PrevScale) > 20f)
			{
				this.CurrentMenu = TutorialScript.TutorialMenu.CameraReset;
				return;
			}
		}
		else if (this.CurrentMenu == TutorialScript.TutorialMenu.CameraReset)
		{
			if (zInput.GetButtonDown("Reset View", ControlType.All))
			{
				Network.Instantiate(this.GameModeInstance.ChessKnight, new Vector3(0f, 5f, -6f), this.GameModeInstance.ChessKnight.transform.rotation, default(NetworkPlayer));
				this.CurrentMenu = TutorialScript.TutorialMenu.ObjectZoom;
			}
			if (this.currentControlType == ControlType.VR && Singleton<VRHMD>.Instance.VRCameraRig.transform.position == Singleton<VRHMD>.Instance.VRCameraRigInitialPosition)
			{
				Network.Instantiate(this.GameModeInstance.ChessKnight, new Vector3(0f, 5f, -6f), this.GameModeInstance.ChessKnight.transform.rotation, default(NetworkPlayer));
				this.CurrentMenu = TutorialScript.TutorialMenu.ObjectZoom;
				return;
			}
		}
		else if (this.CurrentMenu == TutorialScript.TutorialMenu.ObjectZoom)
		{
			if (HoverScript.bAltZooming)
			{
				if (this.currentControlType == ControlType.Touch)
				{
					this.CurrentMenu = TutorialScript.TutorialMenu.Contextual;
				}
				else
				{
					this.CurrentMenu = TutorialScript.TutorialMenu.ObjectRotate;
				}
			}
			if (this.currentControlType == ControlType.VR && (NetworkSingleton<NetworkUI>.Instance.VRControllerManager.left.GetComponent<VRTrackedController>().currentMode == TrackedControllerMode.Zoom || NetworkSingleton<NetworkUI>.Instance.VRControllerManager.right.GetComponent<VRTrackedController>().currentMode == TrackedControllerMode.Zoom))
			{
				this.CurrentMenu = TutorialScript.TutorialMenu.ObjectRotate;
				return;
			}
		}
		else if (this.CurrentMenu == TutorialScript.TutorialMenu.ObjectRotate)
		{
			if (PlayerScript.PointerScript.bRotating)
			{
				if (this.currentControlType == ControlType.VR)
				{
					this.CurrentMenu = TutorialScript.TutorialMenu.BoxSelection;
					return;
				}
				if (this.currentControlType == ControlType.Touch)
				{
					this.CurrentMenu = TutorialScript.TutorialMenu.Contextual;
					return;
				}
				this.CurrentMenu = TutorialScript.TutorialMenu.ObjectTap;
				return;
			}
		}
		else if (this.CurrentMenu == TutorialScript.TutorialMenu.ObjectTap)
		{
			int num2 = 0;
			using (List<NetworkPhysicsObject>.Enumerator enumerator = NetworkSingleton<ManagerPhysicsObject>.Instance.GrabbableNPOs.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current.IsHeldBySomebody)
					{
						num2++;
					}
				}
			}
			if (num2 > 1)
			{
				this.CurrentMenu = TutorialScript.TutorialMenu.BoxSelection;
				return;
			}
		}
		else if (this.CurrentMenu == TutorialScript.TutorialMenu.BoxSelection)
		{
			if (PlayerScript.PointerScript.HighLightedObjects.Count > 1)
			{
				if (this.currentControlType == ControlType.Touch)
				{
					this.CurrentMenu = TutorialScript.TutorialMenu.ControlScheme;
					return;
				}
				this.CurrentMenu = TutorialScript.TutorialMenu.Contextual;
				return;
			}
		}
		else if (this.CurrentMenu == TutorialScript.TutorialMenu.Contextual)
		{
			if (PlayerScript.PointerScript.InfoObject)
			{
				if (this.currentControlType == ControlType.Touch)
				{
					this.CurrentMenu = TutorialScript.TutorialMenu.BoxSelection;
					return;
				}
				this.CurrentMenu = TutorialScript.TutorialMenu.ControlScheme;
				return;
			}
		}
		else if (this.CurrentMenu == TutorialScript.TutorialMenu.ControlScheme)
		{
			if (this.x == 0 && (NetworkSingleton<NetworkUI>.Instance.GUIControlScheme.activeSelf || NetworkSingleton<NetworkUI>.Instance.GUIControlSchemeController.activeSelf))
			{
				this.x++;
			}
			if (this.x == 1 && !NetworkSingleton<NetworkUI>.Instance.GUIControlScheme.activeSelf && !NetworkSingleton<NetworkUI>.Instance.GUIControlSchemeController.activeSelf)
			{
				this.x++;
			}
			if (this.x > 1 || this.uiLabel.text == "")
			{
				this.CurrentMenu = TutorialScript.TutorialMenu.Flip;
				this.previoustablerotation = NetworkSingleton<ManagerPhysicsObject>.Instance.Table.transform.rotation;
				Network.Instantiate(this.GameModeInstance.BlockSquare, this.SpawnPos, this.GameModeInstance.BlockSquare.transform.rotation, default(NetworkPlayer));
				Network.Instantiate(this.GameModeInstance.BlockRectangle, this.SpawnPos, this.GameModeInstance.BlockRectangle.transform.rotation, default(NetworkPlayer));
				Network.Instantiate(this.GameModeInstance.BlockTriangle, this.SpawnPos, this.GameModeInstance.BlockTriangle.transform.rotation, default(NetworkPlayer));
				Network.Instantiate(this.GameModeInstance.Deck, this.SpawnPos, this.GameModeInstance.Deck.transform.rotation, default(NetworkPlayer));
				GameObject gameObject = Network.Instantiate(this.GameModeInstance.Card, this.SpawnPos, this.GameModeInstance.Card.transform.rotation, default(NetworkPlayer));
				int num3 = UnityEngine.Random.Range(0, 52);
				NetworkSingleton<CardManagerScript>.Instance.SetupCard(gameObject, num3, -1, false);
				gameObject.GetComponent<CardScript>().card_id_ = num3;
				gameObject = Network.Instantiate(this.GameModeInstance.Card, this.SpawnPos, this.GameModeInstance.Card.transform.rotation, default(NetworkPlayer));
				NetworkSingleton<CardManagerScript>.Instance.SetupCard(gameObject, 52, -1, false);
				gameObject.GetComponent<CardScript>().card_id_ = 52;
				Network.Instantiate(this.GameModeInstance.CheckerRed, this.SpawnPos, this.GameModeInstance.CheckerRed.transform.rotation, default(NetworkPlayer));
				Network.Instantiate(this.GameModeInstance.CheckerBlack, this.SpawnPos, this.GameModeInstance.CheckerBlack.transform.rotation, default(NetworkPlayer));
				Network.Instantiate(this.GameModeInstance.CheckerWhite, this.SpawnPos, this.GameModeInstance.CheckerWhite.transform.rotation, default(NetworkPlayer));
				Network.Instantiate(this.GameModeInstance.ChessPawn, this.SpawnPos, this.GameModeInstance.ChessPawn.transform.rotation, default(NetworkPlayer));
				Network.Instantiate(this.GameModeInstance.ChessRook, this.SpawnPos, this.GameModeInstance.ChessRook.transform.rotation, default(NetworkPlayer));
				Network.Instantiate(this.GameModeInstance.ChessKnight, this.SpawnPos, this.GameModeInstance.ChessKnight.transform.rotation, default(NetworkPlayer));
				Network.Instantiate(this.GameModeInstance.ChessBishop, this.SpawnPos, this.GameModeInstance.ChessBishop.transform.rotation, default(NetworkPlayer));
				Network.Instantiate(this.GameModeInstance.ChessQueen, this.SpawnPos, this.GameModeInstance.ChessQueen.transform.rotation, default(NetworkPlayer));
				Network.Instantiate(this.GameModeInstance.ChessKing, this.SpawnPos, this.GameModeInstance.ChessKing.transform.rotation, default(NetworkPlayer));
				gameObject = Network.Instantiate(this.GameModeInstance.ChessPawn, this.SpawnPos, this.GameModeInstance.ChessPawn.transform.rotation, default(NetworkPlayer));
				gameObject.GetComponent<MaterialSyncScript>().SetMaterial(1);
				gameObject = Network.Instantiate(this.GameModeInstance.ChessRook, this.SpawnPos, this.GameModeInstance.ChessRook.transform.rotation, default(NetworkPlayer));
				gameObject.GetComponent<MaterialSyncScript>().SetMaterial(1);
				gameObject = Network.Instantiate(this.GameModeInstance.ChessKnight, this.SpawnPos, this.GameModeInstance.ChessKnight.transform.rotation, default(NetworkPlayer));
				gameObject.GetComponent<MaterialSyncScript>().SetMaterial(1);
				gameObject = Network.Instantiate(this.GameModeInstance.ChessBishop, this.SpawnPos, this.GameModeInstance.ChessBishop.transform.rotation, default(NetworkPlayer));
				gameObject.GetComponent<MaterialSyncScript>().SetMaterial(1);
				gameObject = Network.Instantiate(this.GameModeInstance.ChessQueen, this.SpawnPos, this.GameModeInstance.ChessQueen.transform.rotation, default(NetworkPlayer));
				gameObject.GetComponent<MaterialSyncScript>().SetMaterial(1);
				gameObject = Network.Instantiate(this.GameModeInstance.ChessKing, this.SpawnPos, this.GameModeInstance.ChessKing.transform.rotation, default(NetworkPlayer));
				gameObject.GetComponent<MaterialSyncScript>().SetMaterial(1);
				gameObject = Network.Instantiate(this.GameModeInstance.ChessPawn, this.SpawnPos, this.GameModeInstance.ChessPawn.transform.rotation, default(NetworkPlayer));
				gameObject.GetComponent<NetworkPhysicsObject>().SetObject(true, -1, 2);
				gameObject = Network.Instantiate(this.GameModeInstance.ChessRook, this.SpawnPos, this.GameModeInstance.ChessRook.transform.rotation, default(NetworkPlayer));
				gameObject.GetComponent<NetworkPhysicsObject>().SetObject(true, -1, 2);
				gameObject = Network.Instantiate(this.GameModeInstance.ChessKnight, this.SpawnPos, this.GameModeInstance.ChessKnight.transform.rotation, default(NetworkPlayer));
				gameObject.GetComponent<NetworkPhysicsObject>().SetObject(true, -1, 2);
				gameObject = Network.Instantiate(this.GameModeInstance.ChessBishop, this.SpawnPos, this.GameModeInstance.ChessBishop.transform.rotation, default(NetworkPlayer));
				gameObject.GetComponent<NetworkPhysicsObject>().SetObject(true, -1, 2);
				gameObject = Network.Instantiate(this.GameModeInstance.ChessQueen, this.SpawnPos, this.GameModeInstance.ChessQueen.transform.rotation, default(NetworkPlayer));
				gameObject.GetComponent<NetworkPhysicsObject>().SetObject(true, -1, 2);
				gameObject = Network.Instantiate(this.GameModeInstance.ChessKing, this.SpawnPos, this.GameModeInstance.ChessKing.transform.rotation, default(NetworkPlayer));
				gameObject.GetComponent<NetworkPhysicsObject>().SetObject(true, -1, 2);
				gameObject = Network.Instantiate(this.GameModeInstance.ChessPawn, this.SpawnPos, this.GameModeInstance.ChessPawn.transform.rotation, default(NetworkPlayer));
				gameObject.GetComponent<NetworkPhysicsObject>().SetObject(true, -1, 3);
				gameObject = Network.Instantiate(this.GameModeInstance.ChessRook, this.SpawnPos, this.GameModeInstance.ChessRook.transform.rotation, default(NetworkPlayer));
				gameObject.GetComponent<NetworkPhysicsObject>().SetObject(true, -1, 3);
				gameObject = Network.Instantiate(this.GameModeInstance.ChessKnight, this.SpawnPos, this.GameModeInstance.ChessKnight.transform.rotation, default(NetworkPlayer));
				gameObject.GetComponent<NetworkPhysicsObject>().SetObject(true, -1, 3);
				gameObject = Network.Instantiate(this.GameModeInstance.ChessBishop, this.SpawnPos, this.GameModeInstance.ChessBishop.transform.rotation, default(NetworkPlayer));
				gameObject.GetComponent<NetworkPhysicsObject>().SetObject(true, -1, 3);
				gameObject = Network.Instantiate(this.GameModeInstance.ChessQueen, this.SpawnPos, this.GameModeInstance.ChessQueen.transform.rotation, default(NetworkPlayer));
				gameObject.GetComponent<NetworkPhysicsObject>().SetObject(true, -1, 3);
				gameObject = Network.Instantiate(this.GameModeInstance.ChessKing, this.SpawnPos, this.GameModeInstance.ChessKing.transform.rotation, default(NetworkPlayer));
				gameObject.GetComponent<NetworkPhysicsObject>().SetObject(true, -1, 3);
				Network.Instantiate(this.GameModeInstance.D4, this.SpawnPos, this.GameModeInstance.D4.transform.rotation, default(NetworkPlayer));
				Network.Instantiate(this.GameModeInstance.D6, this.SpawnPos, this.GameModeInstance.D6.transform.rotation, default(NetworkPlayer));
				Network.Instantiate(this.GameModeInstance.D8, this.SpawnPos, this.GameModeInstance.D8.transform.rotation, default(NetworkPlayer));
				Network.Instantiate(this.GameModeInstance.D10, this.SpawnPos, this.GameModeInstance.D10.transform.rotation, default(NetworkPlayer));
				Network.Instantiate(this.GameModeInstance.D12, this.SpawnPos, this.GameModeInstance.D12.transform.rotation, default(NetworkPlayer));
				Network.Instantiate(this.GameModeInstance.D20, this.SpawnPos, this.GameModeInstance.D20.transform.rotation, default(NetworkPlayer));
				gameObject = Network.Instantiate(this.GameModeInstance.D4, this.SpawnPos, this.GameModeInstance.D4.transform.rotation, default(NetworkPlayer));
				gameObject.GetComponent<NetworkPhysicsObject>().SetObject(true, -1, 1);
				gameObject = Network.Instantiate(this.GameModeInstance.D6, this.SpawnPos, this.GameModeInstance.D6.transform.rotation, default(NetworkPlayer));
				gameObject.GetComponent<NetworkPhysicsObject>().SetObject(true, -1, 1);
				gameObject = Network.Instantiate(this.GameModeInstance.D8, this.SpawnPos, this.GameModeInstance.D8.transform.rotation, default(NetworkPlayer));
				gameObject.GetComponent<NetworkPhysicsObject>().SetObject(true, -1, 1);
				gameObject = Network.Instantiate(this.GameModeInstance.D10, this.SpawnPos, this.GameModeInstance.D10.transform.rotation, default(NetworkPlayer));
				gameObject.GetComponent<NetworkPhysicsObject>().SetObject(true, -1, 1);
				gameObject = Network.Instantiate(this.GameModeInstance.D12, this.SpawnPos, this.GameModeInstance.D12.transform.rotation, default(NetworkPlayer));
				gameObject.GetComponent<NetworkPhysicsObject>().SetObject(true, -1, 1);
				gameObject = Network.Instantiate(this.GameModeInstance.D20, this.SpawnPos, this.GameModeInstance.D20.transform.rotation, default(NetworkPlayer));
				gameObject.GetComponent<NetworkPhysicsObject>().SetObject(true, -1, 1);
				Network.Instantiate(this.GameModeInstance.GoPieceWhite, this.SpawnPos, this.GameModeInstance.GoPieceWhite.transform.rotation, default(NetworkPlayer));
				Network.Instantiate(this.GameModeInstance.GoPieceBlack, this.SpawnPos, this.GameModeInstance.GoPieceBlack.transform.rotation, default(NetworkPlayer));
				Network.Instantiate(this.GameModeInstance.GoBowlWhite, this.SpawnPos, this.GameModeInstance.GoBowlWhite.transform.rotation, default(NetworkPlayer));
				Network.Instantiate(this.GameModeInstance.GoBowlBlack, this.SpawnPos, this.GameModeInstance.GoBowlBlack.transform.rotation, default(NetworkPlayer));
				Network.Instantiate(this.GameModeInstance.PlayerPawn, this.SpawnPos, this.GameModeInstance.ChessPawn.transform.rotation, default(NetworkPlayer));
				gameObject = Network.Instantiate(this.GameModeInstance.PlayerPawn, this.SpawnPos, this.GameModeInstance.ChessPawn.transform.rotation, default(NetworkPlayer));
				gameObject.GetComponent<MaterialSyncScript>().SetMaterial(1);
				gameObject = Network.Instantiate(this.GameModeInstance.PlayerPawn, this.SpawnPos, this.GameModeInstance.ChessPawn.transform.rotation, default(NetworkPlayer));
				gameObject.GetComponent<MaterialSyncScript>().SetMaterial(2);
				gameObject = Network.Instantiate(this.GameModeInstance.PlayerPawn, this.SpawnPos, this.GameModeInstance.ChessPawn.transform.rotation, default(NetworkPlayer));
				gameObject.GetComponent<MaterialSyncScript>().SetMaterial(3);
				gameObject = Network.Instantiate(this.GameModeInstance.PlayerPawn, this.SpawnPos, this.GameModeInstance.ChessPawn.transform.rotation, default(NetworkPlayer));
				gameObject.GetComponent<MaterialSyncScript>().SetMaterial(4);
				gameObject = Network.Instantiate(this.GameModeInstance.PlayerPawn, this.SpawnPos, this.GameModeInstance.ChessPawn.transform.rotation, default(NetworkPlayer));
				gameObject.GetComponent<MaterialSyncScript>().SetMaterial(5);
				gameObject = Network.Instantiate(this.GameModeInstance.PlayerPawn, this.SpawnPos, this.GameModeInstance.ChessPawn.transform.rotation, default(NetworkPlayer));
				gameObject.GetComponent<MaterialSyncScript>().SetMaterial(6);
				gameObject = Network.Instantiate(this.GameModeInstance.PlayerPawn, this.SpawnPos, this.GameModeInstance.ChessPawn.transform.rotation, default(NetworkPlayer));
				gameObject.GetComponent<MaterialSyncScript>().SetMaterial(7);
				Network.Instantiate(this.GameModeInstance.Poker50, this.SpawnPos, this.GameModeInstance.Poker50.transform.rotation, default(NetworkPlayer));
				Network.Instantiate(this.GameModeInstance.Poker100, this.SpawnPos, this.GameModeInstance.Poker100.transform.rotation, default(NetworkPlayer));
				Network.Instantiate(this.GameModeInstance.Poker500, this.SpawnPos, this.GameModeInstance.Poker500.transform.rotation, default(NetworkPlayer));
				Network.Instantiate(this.GameModeInstance.Poker1000, this.SpawnPos, this.GameModeInstance.Poker1000.transform.rotation, default(NetworkPlayer));
				Network.Instantiate(this.GameModeInstance.D2, this.SpawnPos, this.GameModeInstance.D2.transform.rotation, default(NetworkPlayer));
				Network.Instantiate(this.GameModeInstance.ReversiChip, this.SpawnPos, this.GameModeInstance.ReversiChip.transform.rotation, default(NetworkPlayer));
				gameObject = Network.Instantiate(this.GameModeInstance.Domino, this.SpawnPos, this.GameModeInstance.Domino.transform.rotation, default(NetworkPlayer));
				gameObject.GetComponent<MeshSyncScript>().SetMesh(UnityEngine.Random.Range(0, 27));
				Network.Instantiate(this.GameModeInstance.BackgammonPieceBrown, this.SpawnPos, this.GameModeInstance.BackgammonPieceBrown.transform.rotation, default(NetworkPlayer));
				Network.Instantiate(this.GameModeInstance.BackgammonPieceWhite, this.SpawnPos, this.GameModeInstance.BackgammonPieceWhite.transform.rotation, default(NetworkPlayer));
				gameObject = Network.Instantiate(this.GameModeInstance.ChineseCheckersPiece, this.SpawnPos, this.GameModeInstance.ChineseCheckersPiece.transform.rotation, default(NetworkPlayer));
				gameObject.GetComponent<MaterialSyncScript>().SetMaterial(0);
				gameObject = Network.Instantiate(this.GameModeInstance.ChineseCheckersPiece, this.SpawnPos, this.GameModeInstance.ChineseCheckersPiece.transform.rotation, default(NetworkPlayer));
				gameObject.GetComponent<MaterialSyncScript>().SetMaterial(1);
				gameObject = Network.Instantiate(this.GameModeInstance.ChineseCheckersPiece, this.SpawnPos, this.GameModeInstance.ChineseCheckersPiece.transform.rotation, default(NetworkPlayer));
				gameObject.GetComponent<MaterialSyncScript>().SetMaterial(2);
				gameObject = Network.Instantiate(this.GameModeInstance.ChineseCheckersPiece, this.SpawnPos, this.GameModeInstance.ChineseCheckersPiece.transform.rotation, default(NetworkPlayer));
				gameObject.GetComponent<MaterialSyncScript>().SetMaterial(3);
				gameObject = Network.Instantiate(this.GameModeInstance.ChineseCheckersPiece, this.SpawnPos, this.GameModeInstance.ChineseCheckersPiece.transform.rotation, default(NetworkPlayer));
				gameObject.GetComponent<MaterialSyncScript>().SetMaterial(4);
				gameObject = Network.Instantiate(this.GameModeInstance.ChineseCheckersPiece, this.SpawnPos, this.GameModeInstance.ChineseCheckersPiece.transform.rotation, default(NetworkPlayer));
				gameObject.GetComponent<MaterialSyncScript>().SetMaterial(5);
				gameObject = Network.Instantiate(this.GameModeInstance.ChineseCheckersPiece, this.SpawnPos, this.GameModeInstance.ChineseCheckersPiece.transform.rotation, default(NetworkPlayer));
				gameObject.GetComponent<MaterialSyncScript>().SetMaterial(6);
				NetworkSingleton<NetworkUI>.Instance.GUITopBar.gameObject.SetActive(true);
				NetworkSingleton<SaveManager>.Instance.RewindSaveEnable = true;
				return;
			}
		}
		else if (this.CurrentMenu == TutorialScript.TutorialMenu.Flip && NetworkSingleton<ManagerPhysicsObject>.Instance.Table.transform.rotation != this.previoustablerotation)
		{
			this.CurrentMenu = TutorialScript.TutorialMenu.End;
		}
	}

	// Token: 0x040013B0 RID: 5040
	private TutorialScript.TutorialMenu currentmenu;

	// Token: 0x040013B1 RID: 5041
	private NetworkUI NetworkInstance;

	// Token: 0x040013B2 RID: 5042
	private GameMode GameModeInstance;

	// Token: 0x040013B3 RID: 5043
	private CameraController CameraInstance;

	// Token: 0x040013B4 RID: 5044
	private GameObject Card;

	// Token: 0x040013B5 RID: 5045
	private GameObject Deck;

	// Token: 0x040013B6 RID: 5046
	private GameObject Die;

	// Token: 0x040013B7 RID: 5047
	private Quaternion PrevCameraRot = Quaternion.identity;

	// Token: 0x040013B8 RID: 5048
	private Vector3 PrevCameraPosition = Vector3.zero;

	// Token: 0x040013B9 RID: 5049
	private float PrevCameraDistance;

	// Token: 0x040013BA RID: 5050
	private bool DeckShuffled;

	// Token: 0x040013BB RID: 5051
	private bool DiceRolled;

	// Token: 0x040013BC RID: 5052
	private Vector3 PrevRotation;

	// Token: 0x040013BD RID: 5053
	private Vector3 PrevPosition;

	// Token: 0x040013BE RID: 5054
	private float PrevScale;

	// Token: 0x040013BF RID: 5055
	private int x;

	// Token: 0x040013C0 RID: 5056
	private Vector3 SpawnPos = new Vector3(0f, 4f, 0f);

	// Token: 0x040013C1 RID: 5057
	private GameObject WhiteHand;

	// Token: 0x040013C2 RID: 5058
	private Quaternion previoustablerotation;

	// Token: 0x040013C3 RID: 5059
	private float TutorialTimeHolder;

	// Token: 0x040013C4 RID: 5060
	private UILabel uiLabel;

	// Token: 0x040013C5 RID: 5061
	private Stopwatch timer;

	// Token: 0x040013C6 RID: 5062
	private ControlType currentControlType = ControlType.Keyboard;

	// Token: 0x040013C7 RID: 5063
	private bool vrInit;

	// Token: 0x02000701 RID: 1793
	private enum TutorialMenu
	{
		// Token: 0x04002A5B RID: 10843
		Intro,
		// Token: 0x04002A5C RID: 10844
		Stack,
		// Token: 0x04002A5D RID: 10845
		RemoveObject,
		// Token: 0x04002A5E RID: 10846
		MoveStack,
		// Token: 0x04002A5F RID: 10847
		Shuffle,
		// Token: 0x04002A60 RID: 10848
		Hand,
		// Token: 0x04002A61 RID: 10849
		Die,
		// Token: 0x04002A62 RID: 10850
		CameraRotate,
		// Token: 0x04002A63 RID: 10851
		CameraPan,
		// Token: 0x04002A64 RID: 10852
		CameraZoom,
		// Token: 0x04002A65 RID: 10853
		CameraReset,
		// Token: 0x04002A66 RID: 10854
		ObjectZoom,
		// Token: 0x04002A67 RID: 10855
		ObjectRotate,
		// Token: 0x04002A68 RID: 10856
		ObjectTap,
		// Token: 0x04002A69 RID: 10857
		BoxSelection,
		// Token: 0x04002A6A RID: 10858
		Contextual,
		// Token: 0x04002A6B RID: 10859
		ControlScheme,
		// Token: 0x04002A6C RID: 10860
		Flip,
		// Token: 0x04002A6D RID: 10861
		End
	}
}
