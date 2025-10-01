using System;
using System.Reflection;
using NewNet;
using UnityEngine;

// Token: 0x020000AC RID: 172
public class CalculatorScript : NetworkBehavior
{
	// Token: 0x06000869 RID: 2153 RVA: 0x0003B410 File Offset: 0x00039610
	public CalculatorScript.State GetState()
	{
		return new CalculatorScript.State(this.screen.text, this.value, this.memory, this.action, this.equalsPressed);
	}

	// Token: 0x0600086A RID: 2154 RVA: 0x0003B43A File Offset: 0x0003963A
	private void Awake()
	{
		NetworkEvents.OnPlayerConnected += this.OnPlayerConnect;
	}

	// Token: 0x0600086B RID: 2155 RVA: 0x0003B44D File Offset: 0x0003964D
	private void OnDestroy()
	{
		NetworkEvents.OnPlayerConnected -= this.OnPlayerConnect;
	}

	// Token: 0x0600086C RID: 2156 RVA: 0x0003B460 File Offset: 0x00039660
	private void Start()
	{
		if (Network.isServer)
		{
			base.networkView.RPC<CalculatorScript.State>(RPCTarget.All, new Action<CalculatorScript.State>(this.SyncPlayer), this.GetState());
		}
	}

	// Token: 0x0600086D RID: 2157 RVA: 0x0003B487 File Offset: 0x00039687
	private void OnPlayerConnect(NetworkPlayer NP)
	{
		if (Network.isServer)
		{
			base.networkView.RPC<CalculatorScript.State>(NP, new Action<CalculatorScript.State>(this.SyncPlayer), this.GetState());
		}
	}

	// Token: 0x0600086E RID: 2158 RVA: 0x0003B4B0 File Offset: 0x000396B0
	[Remote(Permission.Server)]
	public void SyncPlayer(CalculatorScript.State state)
	{
		this.value = state.value;
		this.memory = state.memory;
		this.action = state.action;
		this.screen.text = state.text;
		this.equalsPressed = state.equalsPressed;
	}

	// Token: 0x0600086F RID: 2159 RVA: 0x0003B500 File Offset: 0x00039700
	[Remote("Permissions/Digital")]
	public void ButtonPress(string buttonString)
	{
		if (!PermissionsOptions.CheckAllow(PermissionsOptions.options.Digital, -1))
		{
			PermissionsOptions.BroadcastPermissionWarning("interact with the Calculator (Digital)");
			return;
		}
		if (!PlayerScript.Pointer)
		{
			return;
		}
		if (Network.isServer)
		{
			base.networkView.RPC<string>(RPCTarget.All, new Action<string>(this.RPCButtonPress), buttonString);
			return;
		}
		base.networkView.RPC<string>(RPCTarget.Server, new Action<string>(this.ButtonPress), buttonString);
	}

	// Token: 0x06000870 RID: 2160 RVA: 0x0003B574 File Offset: 0x00039774
	[Remote("Permissions/Digital")]
	private void RPCButtonPress(string buttonString)
	{
		if (this.screen.text == "" && buttonString != "OnC")
		{
			return;
		}
		int num;
		if (int.TryParse(buttonString, out num))
		{
			if (this.screen.text == "0")
			{
				this.screen.text = buttonString;
				this.equalsPressed = false;
				return;
			}
			if (this.equalsPressed)
			{
				this.screen.text = buttonString;
				this.equalsPressed = false;
				return;
			}
			if (this.screen.text.Length < 7)
			{
				this.screen.text = this.screen.text + buttonString;
			}
			return;
		}
		else
		{
			MethodInfo method = base.GetType().GetMethod(buttonString);
			if (method != null)
			{
				method.Invoke(this, null);
				return;
			}
			Debug.LogError("Method could not be found: " + buttonString);
			return;
		}
	}

	// Token: 0x06000871 RID: 2161 RVA: 0x0003B657 File Offset: 0x00039857
	public void OnC()
	{
		this.value = 0f;
		this.memory = 0f;
		this.action = CalculatorActions.NONE;
		this.equalsPressed = false;
		this.screen.text = this.value.ToString();
	}

	// Token: 0x06000872 RID: 2162 RVA: 0x0003B693 File Offset: 0x00039893
	public void Off()
	{
		this.value = 0f;
		this.memory = 0f;
		this.action = CalculatorActions.NONE;
		this.equalsPressed = false;
		this.screen.text = "";
	}

	// Token: 0x06000873 RID: 2163 RVA: 0x0003B6C9 File Offset: 0x000398C9
	public void CE()
	{
		if (this.screen.text == "")
		{
			return;
		}
		this.equalsPressed = false;
		this.screen.text = "0";
	}

	// Token: 0x06000874 RID: 2164 RVA: 0x0003B6FC File Offset: 0x000398FC
	public void Add()
	{
		this.CalcEquals();
		this.action = CalculatorActions.ADD;
		float num;
		float.TryParse(this.screen.text, out num);
		this.value = num;
		this.screen.text = "0";
	}

	// Token: 0x06000875 RID: 2165 RVA: 0x0003B740 File Offset: 0x00039940
	public void Divide()
	{
		this.CalcEquals();
		this.action = CalculatorActions.DIVIDE;
		float num;
		float.TryParse(this.screen.text, out num);
		this.value = num;
		this.screen.text = "0";
	}

	// Token: 0x06000876 RID: 2166 RVA: 0x0003B784 File Offset: 0x00039984
	public void CalcEquals()
	{
		float num = 0f;
		switch (this.action)
		{
		case CalculatorActions.ADD:
		{
			this.equalsPressed = true;
			float.TryParse(this.screen.text, out num);
			float num2 = this.value + num;
			if (num2 > 9999999f)
			{
				num2 = 9999999f;
			}
			else if (num2 < -999999f)
			{
				num2 = -999999f;
			}
			this.screen.text = num2.ToString();
			break;
		}
		case CalculatorActions.SUBTRACT:
		{
			this.equalsPressed = true;
			float.TryParse(this.screen.text, out num);
			float num2 = this.value - num;
			if (num2 > 9999999f)
			{
				num2 = 9999999f;
			}
			else if (num2 < -999999f)
			{
				num2 = -999999f;
			}
			this.screen.text = num2.ToString();
			break;
		}
		case CalculatorActions.DIVIDE:
		{
			this.equalsPressed = true;
			float.TryParse(this.screen.text, out num);
			float num2 = this.value / num;
			if (num2 > 9999999f)
			{
				num2 = 9999999f;
			}
			else if (num2 < -999999f)
			{
				num2 = -999999f;
			}
			string text = num2.ToString();
			if (text.Length > 7)
			{
				text = text.Substring(0, 7);
			}
			this.screen.text = text;
			break;
		}
		case CalculatorActions.MULTIPLY:
		{
			this.equalsPressed = true;
			float.TryParse(this.screen.text, out num);
			float num2 = this.value * num;
			if (num2 > 9999999f)
			{
				num2 = 9999999f;
			}
			else if (num2 < -999999f)
			{
				num2 = -999999f;
			}
			string text = num2.ToString();
			if (text.Length > 7)
			{
				text = text.Substring(0, 7);
			}
			this.screen.text = text;
			break;
		}
		}
		this.action = CalculatorActions.NONE;
	}

	// Token: 0x06000877 RID: 2167 RVA: 0x0003B94E File Offset: 0x00039B4E
	public void MC()
	{
		this.memory = 0f;
	}

	// Token: 0x06000878 RID: 2168 RVA: 0x0003B95C File Offset: 0x00039B5C
	public void MMinus()
	{
		float num;
		float.TryParse(this.screen.text, out num);
		string text = (num - this.memory).ToString();
		if (text.Length > 7)
		{
			text = text.Substring(0, 7);
		}
		this.screen.text = text;
		this.equalsPressed = true;
	}

	// Token: 0x06000879 RID: 2169 RVA: 0x0003B9B4 File Offset: 0x00039BB4
	public void MPlus()
	{
		float num;
		float.TryParse(this.screen.text, out num);
		string text = (num + this.memory).ToString();
		if (text.Length > 7)
		{
			text = text.Substring(0, 7);
		}
		this.screen.text = text;
		this.equalsPressed = true;
	}

	// Token: 0x0600087A RID: 2170 RVA: 0x0003BA0C File Offset: 0x00039C0C
	public void MR()
	{
		this.value = 0f;
		float num;
		float.TryParse(this.screen.text, out num);
		this.memory = num;
		this.screen.text = "0";
	}

	// Token: 0x0600087B RID: 2171 RVA: 0x0003BA50 File Offset: 0x00039C50
	public void Multiply()
	{
		this.CalcEquals();
		this.action = CalculatorActions.MULTIPLY;
		float num;
		float.TryParse(this.screen.text, out num);
		this.value = num;
		this.screen.text = "0";
	}

	// Token: 0x0600087C RID: 2172 RVA: 0x0003BA94 File Offset: 0x00039C94
	public void Percent()
	{
		if (this.action != CalculatorActions.NONE)
		{
			float num;
			float.TryParse(this.screen.text, out num);
			float num2 = num;
			num2 *= this.value / 100f;
			this.screen.text = num2.ToString();
		}
	}

	// Token: 0x0600087D RID: 2173 RVA: 0x0003BAE0 File Offset: 0x00039CE0
	public void Period()
	{
		if (!this.screen.text.Contains(".") && this.screen.text.Length < 6)
		{
			this.screen.text = this.screen.text + ".";
		}
	}

	// Token: 0x0600087E RID: 2174 RVA: 0x0003BB38 File Offset: 0x00039D38
	public void PositiveNegative()
	{
		float num;
		float.TryParse(this.screen.text, out num);
		if (num >= 0f && this.screen.text.Length < 7)
		{
			this.value = num * -1f;
			this.screen.text = this.value.ToString();
			return;
		}
		if (num < 0f)
		{
			this.value = num * -1f;
			this.screen.text = this.value.ToString();
		}
	}

	// Token: 0x0600087F RID: 2175 RVA: 0x0003BBC4 File Offset: 0x00039DC4
	public void SquareRoot()
	{
		float num;
		float.TryParse(this.screen.text, out num);
		if (num < 0f)
		{
			return;
		}
		string text = Mathf.Sqrt(num).ToString();
		if (text.Length > 7)
		{
			text = text.Substring(0, 7);
		}
		this.screen.text = text;
		this.equalsPressed = true;
	}

	// Token: 0x06000880 RID: 2176 RVA: 0x0003BC24 File Offset: 0x00039E24
	public void Subtract()
	{
		this.CalcEquals();
		this.action = CalculatorActions.SUBTRACT;
		float num;
		float.TryParse(this.screen.text, out num);
		this.value = num;
		this.screen.text = "0";
	}

	// Token: 0x040005F2 RID: 1522
	public UILabel screen;

	// Token: 0x040005F3 RID: 1523
	private float value;

	// Token: 0x040005F4 RID: 1524
	public float memory;

	// Token: 0x040005F5 RID: 1525
	private CalculatorActions action;

	// Token: 0x040005F6 RID: 1526
	private bool equalsPressed;

	// Token: 0x0200057F RID: 1407
	public struct State
	{
		// Token: 0x0600384B RID: 14411 RVA: 0x0016B735 File Offset: 0x00169935
		public State(string text, float value, float memory, CalculatorActions action, bool equalsPressed)
		{
			this.text = text;
			this.value = value;
			this.memory = memory;
			this.action = action;
			this.equalsPressed = equalsPressed;
		}

		// Token: 0x04002506 RID: 9478
		public string text;

		// Token: 0x04002507 RID: 9479
		public float value;

		// Token: 0x04002508 RID: 9480
		public float memory;

		// Token: 0x04002509 RID: 9481
		public CalculatorActions action;

		// Token: 0x0400250A RID: 9482
		public bool equalsPressed;
	}
}
