using System;
using UnityEngine;

// Token: 0x020000FD RID: 253
public class DiceScript : MonoBehaviour
{
	// Token: 0x06000C6D RID: 3181 RVA: 0x00054958 File Offset: 0x00052B58
	private void Start()
	{
		if (base.GetComponent<NetworkPhysicsObject>().UseAltSounds)
		{
			base.GetComponent<NetworkPhysicsObject>().SetMass(base.GetComponent<Rigidbody>().mass *= 1.35f);
		}
	}

	// Token: 0x06000C6E RID: 3182 RVA: 0x00054998 File Offset: 0x00052B98
	public static void IncrementDice(GameObject Dice, bool bIncrease, int playerId)
	{
		NetworkPhysicsObject component = Dice.GetComponent<NetworkPhysicsObject>();
		int num = component.GetRotationNumber(false);
		num = (bIncrease ? (num + 1) : (num - 1));
		if (num > component.RotationValues.Count)
		{
			num = 1;
		}
		else if (num < 1)
		{
			num = component.RotationValues.Count;
		}
		component.SetRotationNumber(num, playerId);
	}

	// Token: 0x06000C6F RID: 3183 RVA: 0x000549EC File Offset: 0x00052BEC
	public static void FlipDice(GameObject Dice, int playerId)
	{
		NetworkPhysicsObject component = Dice.GetComponent<NetworkPhysicsObject>();
		int rotationNumber = component.GetRotationNumber(true);
		component.SetRotationNumber(rotationNumber, playerId);
	}

	// Token: 0x06000C70 RID: 3184 RVA: 0x00054A10 File Offset: 0x00052C10
	public static RotationValue[] DiceToRotations(GameObject Dice)
	{
		CustomDice component = Dice.GetComponent<CustomDice>();
		RotationValue[] result = null;
		NetworkPhysicsObject component2 = Dice.GetComponent<NetworkPhysicsObject>();
		if (!component2)
		{
			return null;
		}
		string internalName = component2.InternalName;
		if (internalName == "Die_6" || internalName == "Custom_Model" || (component && component.CurrentDiceType == DiceType.d6))
		{
			result = DiceScript.d6rotations;
		}
		else if (internalName == "Die_6_Rounded")
		{
			result = DiceScript.d6Roundedrotations;
		}
		else if (internalName == "Die_Piecepack")
		{
			result = DiceScript.d6PiecePackrotations;
		}
		else if (internalName == "Die_4" || (component && component.CurrentDiceType == DiceType.d4))
		{
			result = DiceScript.d4rotations;
		}
		else if (internalName == "Die_8" || (component && component.CurrentDiceType == DiceType.d8))
		{
			result = DiceScript.d8rotations;
		}
		else if (internalName == "Die_10" || (component && component.CurrentDiceType == DiceType.d10))
		{
			result = DiceScript.d10rotations;
		}
		else if (internalName == "Die_12" || (component && component.CurrentDiceType == DiceType.d12))
		{
			result = DiceScript.d12rotations;
		}
		else if (internalName == "Die_20" || (component && component.CurrentDiceType == DiceType.d20))
		{
			result = DiceScript.d20rotations;
		}
		return result;
	}

	// Token: 0x04000865 RID: 2149
	private static readonly RotationValue[] d6rotations = new RotationValue[]
	{
		new RotationValue(1f, new Vector3(-90f, 0f, 0f)),
		new RotationValue(2f, new Vector3(0f, 0f, 0f)),
		new RotationValue(3f, new Vector3(0f, 0f, -90f)),
		new RotationValue(4f, new Vector3(0f, 0f, 90f)),
		new RotationValue(5f, new Vector3(0f, 0f, -180f)),
		new RotationValue(6f, new Vector3(90f, 0f, 0f))
	};

	// Token: 0x04000866 RID: 2150
	private static readonly RotationValue[] d6Roundedrotations = new RotationValue[]
	{
		new RotationValue(1f, new Vector3(0f, 0f, 0f)),
		new RotationValue(2f, new Vector3(90f, 0f, 0f)),
		new RotationValue(3f, new Vector3(0f, 0f, 90f)),
		new RotationValue(4f, new Vector3(0f, 0f, -90f)),
		new RotationValue(5f, new Vector3(-90f, 0f, 0f)),
		new RotationValue(6f, new Vector3(0f, 0f, -180f))
	};

	// Token: 0x04000867 RID: 2151
	private static readonly RotationValue[] d6PiecePackrotations = new RotationValue[]
	{
		new RotationValue("Blank", new Vector3(0f, 90f, -90f)),
		new RotationValue(2f, new Vector3(0f, -90f, 0f)),
		new RotationValue(3f, new Vector3(-90f, -180f, 0f)),
		new RotationValue(4f, new Vector3(0f, -270f, 90f)),
		new RotationValue(5f, new Vector3(90f, 0f, 0f)),
		new RotationValue("Symbol", new Vector3(0f, -90f, -180f))
	};

	// Token: 0x04000868 RID: 2152
	private static readonly RotationValue[] d4rotations = new RotationValue[]
	{
		new RotationValue(1f, new Vector3(18f, -241f, -120f)),
		new RotationValue(2f, new Vector3(-90f, -60f, 0f)),
		new RotationValue(3f, new Vector3(18f, -121f, 0f)),
		new RotationValue(4f, new Vector3(18f, 0f, -240f))
	};

	// Token: 0x04000869 RID: 2153
	private static readonly RotationValue[] d8rotations = new RotationValue[]
	{
		new RotationValue(1f, new Vector3(-33f, 0f, 90f)),
		new RotationValue(2f, new Vector3(-33f, 0f, 180f)),
		new RotationValue(3f, new Vector3(33f, 180f, -180f)),
		new RotationValue(4f, new Vector3(33f, 180f, 90f)),
		new RotationValue(5f, new Vector3(33f, 180f, -90f)),
		new RotationValue(6f, new Vector3(33f, 180f, 0f)),
		new RotationValue(7f, new Vector3(-33f, 0f, 0f)),
		new RotationValue(8f, new Vector3(-33f, 0f, -90f))
	};

	// Token: 0x0400086A RID: 2154
	private static readonly RotationValue[] d10rotations = new RotationValue[]
	{
		new RotationValue(1f, new Vector3(-38f, 0f, 234f)),
		new RotationValue(2f, new Vector3(38f, 180f, -233f)),
		new RotationValue(3f, new Vector3(-38f, 0f, 20f)),
		new RotationValue(4f, new Vector3(38f, 180f, -17f)),
		new RotationValue(5f, new Vector3(-38f, 0f, 90f)),
		new RotationValue(6f, new Vector3(38f, 180f, -161f)),
		new RotationValue(7f, new Vector3(-38f, 0f, 307f)),
		new RotationValue(8f, new Vector3(38f, 180f, -304f)),
		new RotationValue(9f, new Vector3(-38f, 0f, 163f)),
		new RotationValue(10f, new Vector3(38f, 180f, -90f))
	};

	// Token: 0x0400086B RID: 2155
	private static readonly RotationValue[] d12rotations = new RotationValue[]
	{
		new RotationValue(1f, new Vector3(27f, 0f, 72f)),
		new RotationValue(2f, new Vector3(27f, 0f, 144f)),
		new RotationValue(3f, new Vector3(27f, 0f, -72f)),
		new RotationValue(4f, new Vector3(-27f, 180f, 180f)),
		new RotationValue(5f, new Vector3(90f, 180f, 0f)),
		new RotationValue(6f, new Vector3(27f, 0f, -144f)),
		new RotationValue(7f, new Vector3(-27f, 180f, 36f)),
		new RotationValue(8f, new Vector3(-90f, 180f, 0f)),
		new RotationValue(9f, new Vector3(27f, 0f, 0f)),
		new RotationValue(10f, new Vector3(-27f, 180f, 108f)),
		new RotationValue(11f, new Vector3(-27f, 108f, -36f)),
		new RotationValue(12f, new Vector3(-27f, 36f, -108f))
	};

	// Token: 0x0400086C RID: 2156
	private static readonly RotationValue[] d20rotations = new RotationValue[]
	{
		new RotationValue(1f, new Vector3(-11f, 60f, 17f)),
		new RotationValue(2f, new Vector3(52f, -60f, -17f)),
		new RotationValue(3f, new Vector3(-11f, -180f, 90f)),
		new RotationValue(4f, new Vector3(-11f, -180f, 162f)),
		new RotationValue(5f, new Vector3(-11f, -60f, 234f)),
		new RotationValue(6f, new Vector3(-11f, -180f, 306f)),
		new RotationValue(7f, new Vector3(52f, -60f, 55f)),
		new RotationValue(8f, new Vector3(52f, -60f, 198f)),
		new RotationValue(9f, new Vector3(52f, -60f, 127f)),
		new RotationValue(10f, new Vector3(52f, -180f, -90f)),
		new RotationValue(11f, new Vector3(308f, 0f, 90f)),
		new RotationValue(12f, new Vector3(306f, -240f, -52f)),
		new RotationValue(13f, new Vector3(-52f, -240f, 18f)),
		new RotationValue(14f, new Vector3(307f, 120f, 233f)),
		new RotationValue(15f, new Vector3(11f, 120f, -234f)),
		new RotationValue(16f, new Vector3(11f, 0f, 54f)),
		new RotationValue(17f, new Vector3(11f, -120f, -17f)),
		new RotationValue(18f, new Vector3(11f, 0f, -90f)),
		new RotationValue(19f, new Vector3(-52f, -240f, -198f)),
		new RotationValue(20f, new Vector3(11f, 0f, -162f))
	};
}
