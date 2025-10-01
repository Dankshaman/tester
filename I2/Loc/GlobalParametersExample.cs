using System;

namespace I2.Loc
{
	// Token: 0x02000467 RID: 1127
	public class GlobalParametersExample : RegisterGlobalParameters
	{
		// Token: 0x06003315 RID: 13077 RVA: 0x00155338 File Offset: 0x00153538
		public override string GetParameterValue(string ParamName)
		{
			if (ParamName == "WINNER")
			{
				return "Javier";
			}
			if (ParamName == "NUM PLAYERS")
			{
				return 5.ToString();
			}
			return null;
		}
	}
}
