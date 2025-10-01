using System;
using System.Collections.Generic;
using UnityEngine;

namespace RTEditor
{
	// Token: 0x02000420 RID: 1056
	[Serializable]
	public class ShortcutKeys
	{
		// Token: 0x060030D1 RID: 12497 RVA: 0x0014D91C File Offset: 0x0014BB1C
		static ShortcutKeys()
		{
			ShortcutKeys._availableKeys.Add(KeyCode.Space);
			ShortcutKeys._availableKeys.Add(KeyCode.Backspace);
			ShortcutKeys._availableKeys.Add(KeyCode.Return);
			ShortcutKeys._availableKeys.Add(KeyCode.Tab);
			for (int i = 97; i <= 122; i++)
			{
				ShortcutKeys._availableKeys.Add((KeyCode)i);
			}
			for (int j = 48; j <= 57; j++)
			{
				ShortcutKeys._availableKeys.Add((KeyCode)j);
			}
			ShortcutKeys._availableKeys.Add(KeyCode.None);
			ShortcutKeys._availableKeyNames = new List<string>();
			for (int k = 0; k < ShortcutKeys._availableKeys.Count; k++)
			{
				ShortcutKeys._availableKeyNames.Add(ShortcutKeys._availableKeys[k].ToString());
			}
		}

		// Token: 0x1700064E RID: 1614
		// (get) Token: 0x060030D2 RID: 12498 RVA: 0x0014D9E1 File Offset: 0x0014BBE1
		public static List<KeyCode> AvailableKeys
		{
			get
			{
				return new List<KeyCode>(ShortcutKeys._availableKeys);
			}
		}

		// Token: 0x1700064F RID: 1615
		// (get) Token: 0x060030D3 RID: 12499 RVA: 0x0014D9ED File Offset: 0x0014BBED
		public static List<string> AvailableKeyNames
		{
			get
			{
				return new List<string>(ShortcutKeys._availableKeyNames);
			}
		}

		// Token: 0x17000650 RID: 1616
		// (get) Token: 0x060030D4 RID: 12500 RVA: 0x0014D9F9 File Offset: 0x0014BBF9
		public string Name
		{
			get
			{
				return this._name;
			}
		}

		// Token: 0x17000651 RID: 1617
		// (get) Token: 0x060030D5 RID: 12501 RVA: 0x0014DA01 File Offset: 0x0014BC01
		// (set) Token: 0x060030D6 RID: 12502 RVA: 0x0014DA0B File Offset: 0x0014BC0B
		public KeyCode Key0
		{
			get
			{
				return this._keys[0];
			}
			set
			{
				if (ShortcutKeys._availableKeys.Contains(value))
				{
					this._keys[0] = value;
				}
			}
		}

		// Token: 0x17000652 RID: 1618
		// (get) Token: 0x060030D7 RID: 12503 RVA: 0x0014DA23 File Offset: 0x0014BC23
		// (set) Token: 0x060030D8 RID: 12504 RVA: 0x0014DA2D File Offset: 0x0014BC2D
		public KeyCode Key1
		{
			get
			{
				return this._keys[1];
			}
			set
			{
				if (ShortcutKeys._availableKeys.Contains(value))
				{
					this._keys[1] = value;
				}
			}
		}

		// Token: 0x17000653 RID: 1619
		// (get) Token: 0x060030D9 RID: 12505 RVA: 0x0014DA45 File Offset: 0x0014BC45
		// (set) Token: 0x060030DA RID: 12506 RVA: 0x0014DA4D File Offset: 0x0014BC4D
		public bool LCtrl
		{
			get
			{
				return this._lCtrl;
			}
			set
			{
				this._lCtrl = value;
			}
		}

		// Token: 0x17000654 RID: 1620
		// (get) Token: 0x060030DB RID: 12507 RVA: 0x0014DA56 File Offset: 0x0014BC56
		// (set) Token: 0x060030DC RID: 12508 RVA: 0x0014DA5E File Offset: 0x0014BC5E
		public bool LCmd
		{
			get
			{
				return this._lCmd;
			}
			set
			{
				this._lCmd = value;
			}
		}

		// Token: 0x17000655 RID: 1621
		// (get) Token: 0x060030DD RID: 12509 RVA: 0x0014DA67 File Offset: 0x0014BC67
		// (set) Token: 0x060030DE RID: 12510 RVA: 0x0014DA6F File Offset: 0x0014BC6F
		public bool LAlt
		{
			get
			{
				return this._lAlt;
			}
			set
			{
				this._lAlt = value;
			}
		}

		// Token: 0x17000656 RID: 1622
		// (get) Token: 0x060030DF RID: 12511 RVA: 0x0014DA78 File Offset: 0x0014BC78
		// (set) Token: 0x060030E0 RID: 12512 RVA: 0x0014DA80 File Offset: 0x0014BC80
		public bool LShift
		{
			get
			{
				return this._lShift;
			}
			set
			{
				this._lShift = value;
			}
		}

		// Token: 0x17000657 RID: 1623
		// (get) Token: 0x060030E1 RID: 12513 RVA: 0x0014DA89 File Offset: 0x0014BC89
		// (set) Token: 0x060030E2 RID: 12514 RVA: 0x0014DA91 File Offset: 0x0014BC91
		public bool LMouseButton
		{
			get
			{
				return this._lMouseBtn;
			}
			set
			{
				this._lMouseBtn = value;
			}
		}

		// Token: 0x17000658 RID: 1624
		// (get) Token: 0x060030E3 RID: 12515 RVA: 0x0014DA9A File Offset: 0x0014BC9A
		// (set) Token: 0x060030E4 RID: 12516 RVA: 0x0014DAA2 File Offset: 0x0014BCA2
		public bool RMouseButton
		{
			get
			{
				return this._rMouseBtn;
			}
			set
			{
				this._rMouseBtn = value;
			}
		}

		// Token: 0x17000659 RID: 1625
		// (get) Token: 0x060030E5 RID: 12517 RVA: 0x0014DAAB File Offset: 0x0014BCAB
		// (set) Token: 0x060030E6 RID: 12518 RVA: 0x0014DAB3 File Offset: 0x0014BCB3
		public bool MMouseButton
		{
			get
			{
				return this._mMouseBtn;
			}
			set
			{
				this._mMouseBtn = value;
			}
		}

		// Token: 0x1700065A RID: 1626
		// (get) Token: 0x060030E7 RID: 12519 RVA: 0x0014DABC File Offset: 0x0014BCBC
		// (set) Token: 0x060030E8 RID: 12520 RVA: 0x0014DAC4 File Offset: 0x0014BCC4
		public bool UseModifiers
		{
			get
			{
				return this._useModifiers;
			}
			set
			{
				this._useModifiers = value;
			}
		}

		// Token: 0x1700065B RID: 1627
		// (get) Token: 0x060030E9 RID: 12521 RVA: 0x0014DACD File Offset: 0x0014BCCD
		// (set) Token: 0x060030EA RID: 12522 RVA: 0x0014DAD5 File Offset: 0x0014BCD5
		public bool UseMouseButtons
		{
			get
			{
				return this._useMouseButtons;
			}
			set
			{
				this._useMouseButtons = value;
			}
		}

		// Token: 0x1700065C RID: 1628
		// (get) Token: 0x060030EB RID: 12523 RVA: 0x0014DADE File Offset: 0x0014BCDE
		// (set) Token: 0x060030EC RID: 12524 RVA: 0x0014DAE6 File Offset: 0x0014BCE6
		public bool UseStrictMouseCheck
		{
			get
			{
				return this._useStrictMouseCheck;
			}
			set
			{
				this._useStrictMouseCheck = value;
			}
		}

		// Token: 0x1700065D RID: 1629
		// (get) Token: 0x060030ED RID: 12525 RVA: 0x0014DAEF File Offset: 0x0014BCEF
		// (set) Token: 0x060030EE RID: 12526 RVA: 0x0014DAF7 File Offset: 0x0014BCF7
		public bool UseStrictModifierCheck
		{
			get
			{
				return this._useStrictModifierCheck;
			}
			set
			{
				this._useStrictModifierCheck = value;
			}
		}

		// Token: 0x1700065E RID: 1630
		// (get) Token: 0x060030EF RID: 12527 RVA: 0x0014DB00 File Offset: 0x0014BD00
		// (set) Token: 0x060030F0 RID: 12528 RVA: 0x0014DB08 File Offset: 0x0014BD08
		public int NumberOfKeys
		{
			get
			{
				return this._numberOfKeys;
			}
			set
			{
				this._numberOfKeys = Mathf.Min(Mathf.Max(0, value), 2);
			}
		}

		// Token: 0x060030F1 RID: 12529 RVA: 0x0014DB20 File Offset: 0x0014BD20
		public ShortcutKeys(string name, int numberOfKeys)
		{
			this._name = name;
			this._numberOfKeys = numberOfKeys;
			for (int i = 0; i < 2; i++)
			{
				this._keys[i] = KeyCode.None;
			}
		}

		// Token: 0x060030F2 RID: 12530 RVA: 0x0014DB84 File Offset: 0x0014BD84
		public bool IsActive()
		{
			if (this.IsEmpty())
			{
				return false;
			}
			for (int i = 0; i < this._numberOfKeys; i++)
			{
				if (this._keys[i] != KeyCode.None && !Input.GetKey(this._keys[i]))
				{
					return false;
				}
			}
			if (this.UseStrictModifierCheck && (!this._useModifiers || this.HasNoModifiers()) && this.IsAnyModifierKeyPressed())
			{
				return false;
			}
			if (this._useModifiers)
			{
				if (this._lCtrl && !Input.GetKey(KeyCode.LeftControl))
				{
					return false;
				}
				if (this._lCmd && !Input.GetKey(KeyCode.LeftCommand))
				{
					return false;
				}
				if (this._lAlt && !Input.GetKey(KeyCode.LeftAlt))
				{
					return false;
				}
				if (this._lShift && !Input.GetKey(KeyCode.LeftShift))
				{
					return false;
				}
			}
			if (this.UseStrictMouseCheck && (!this._useMouseButtons || this.HasNoMouseButtons()) && this.IsAnyMouseButtonPressed())
			{
				return false;
			}
			if (this._useMouseButtons)
			{
				if (this._lMouseBtn && !Input.GetMouseButton(0))
				{
					return false;
				}
				if (this._rMouseBtn && !Input.GetMouseButton(1))
				{
					return false;
				}
				if (this._mMouseBtn && !Input.GetMouseButton(2))
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x060030F3 RID: 12531 RVA: 0x0014DCAC File Offset: 0x0014BEAC
		public bool IsActiveInCurrentFrame()
		{
			if (this.IsEmpty())
			{
				return false;
			}
			for (int i = 0; i < this._numberOfKeys; i++)
			{
				if (this._keys[i] != KeyCode.None && !Input.GetKeyDown(this._keys[i]))
				{
					return false;
				}
			}
			if (this.UseStrictModifierCheck && (!this._useModifiers || this.HasNoModifiers()) && this.IsAnyModifierKeyPressed())
			{
				return false;
			}
			if (this._useModifiers)
			{
				if (this._lCtrl && !Input.GetKey(KeyCode.LeftControl))
				{
					return false;
				}
				if (this._lCmd && !Input.GetKey(KeyCode.LeftCommand))
				{
					return false;
				}
				if (this._lAlt && !Input.GetKey(KeyCode.LeftAlt))
				{
					return false;
				}
				if (this._lShift && !Input.GetKey(KeyCode.LeftShift))
				{
					return false;
				}
			}
			if (this.UseStrictMouseCheck && (!this._useMouseButtons || this.HasNoMouseButtons()) && this.IsAnyMouseButtonPressed())
			{
				return false;
			}
			if (this._useMouseButtons)
			{
				if (this._lMouseBtn && !Input.GetMouseButtonDown(0))
				{
					return false;
				}
				if (this._rMouseBtn && !Input.GetMouseButtonDown(1))
				{
					return false;
				}
				if (this._mMouseBtn && !Input.GetMouseButtonDown(2))
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x060030F4 RID: 12532 RVA: 0x0014DDD4 File Offset: 0x0014BFD4
		public bool HasNoKeys()
		{
			KeyCode[] keys = this._keys;
			for (int i = 0; i < keys.Length; i++)
			{
				if (keys[i] != KeyCode.None)
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x060030F5 RID: 12533 RVA: 0x0014DDFE File Offset: 0x0014BFFE
		public bool HasNoModifiers()
		{
			return !this._lAlt && !this._lCmd && !this._lCtrl && !this._lShift;
		}

		// Token: 0x060030F6 RID: 12534 RVA: 0x0014DE23 File Offset: 0x0014C023
		public bool HasNoMouseButtons()
		{
			return !this._lMouseBtn && !this._rMouseBtn && !this._mMouseBtn;
		}

		// Token: 0x060030F7 RID: 12535 RVA: 0x0014DE40 File Offset: 0x0014C040
		public bool IsEmpty()
		{
			return this.HasNoKeys() && (!this._useModifiers || this.HasNoModifiers()) && (!this._useMouseButtons || this.HasNoMouseButtons());
		}

		// Token: 0x060030F8 RID: 12536 RVA: 0x0014DE6C File Offset: 0x0014C06C
		private bool IsAnyModifierKeyPressed()
		{
			return Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.LeftCommand) || Input.GetKey(KeyCode.LeftAlt) || Input.GetKey(KeyCode.LeftShift);
		}

		// Token: 0x060030F9 RID: 12537 RVA: 0x0014DEA7 File Offset: 0x0014C0A7
		private bool IsAnyMouseButtonPressed()
		{
			return Input.GetMouseButton(0) || Input.GetMouseButton(1) || Input.GetMouseButton(2);
		}

		// Token: 0x04001FBF RID: 8127
		private static List<KeyCode> _availableKeys = new List<KeyCode>();

		// Token: 0x04001FC0 RID: 8128
		private static List<string> _availableKeyNames;

		// Token: 0x04001FC1 RID: 8129
		private const int _maxNumberOfKeys = 2;

		// Token: 0x04001FC2 RID: 8130
		[SerializeField]
		private KeyCode[] _keys = new KeyCode[2];

		// Token: 0x04001FC3 RID: 8131
		[SerializeField]
		private int _numberOfKeys = 1;

		// Token: 0x04001FC4 RID: 8132
		[SerializeField]
		private bool _lCtrl;

		// Token: 0x04001FC5 RID: 8133
		[SerializeField]
		private bool _lCmd;

		// Token: 0x04001FC6 RID: 8134
		[SerializeField]
		private bool _lAlt;

		// Token: 0x04001FC7 RID: 8135
		[SerializeField]
		private bool _lShift;

		// Token: 0x04001FC8 RID: 8136
		[SerializeField]
		private bool _useModifiers = true;

		// Token: 0x04001FC9 RID: 8137
		[SerializeField]
		private bool _useStrictModifierCheck;

		// Token: 0x04001FCA RID: 8138
		[SerializeField]
		private bool _lMouseBtn;

		// Token: 0x04001FCB RID: 8139
		[SerializeField]
		private bool _rMouseBtn;

		// Token: 0x04001FCC RID: 8140
		[SerializeField]
		private bool _mMouseBtn;

		// Token: 0x04001FCD RID: 8141
		[SerializeField]
		private bool _useMouseButtons = true;

		// Token: 0x04001FCE RID: 8142
		[SerializeField]
		private bool _useStrictMouseCheck;

		// Token: 0x04001FCF RID: 8143
		[SerializeField]
		private string _name = "ShortcutKeys";
	}
}
