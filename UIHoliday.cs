using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020002E7 RID: 743
public class UIHoliday : MonoBehaviour
{
	// Token: 0x06002447 RID: 9287 RVA: 0x001005E0 File Offset: 0x000FE7E0
	private void Start()
	{
		DateTime now = DateTime.Now;
		foreach (UIHoliday.HolidayDate holidayDate in this.Holidays)
		{
			string str = DateTime.Now.Year + "/";
			DateTime t = Convert.ToDateTime(str + holidayDate.StartDate);
			DateTime t2 = Convert.ToDateTime(str + holidayDate.EndDate);
			holidayDate.Target.SetActive(now > t && now < t2);
		}
	}

	// Token: 0x04001749 RID: 5961
	public List<UIHoliday.HolidayDate> Holidays = new List<UIHoliday.HolidayDate>();

	// Token: 0x02000760 RID: 1888
	[Serializable]
	public class HolidayDate
	{
		// Token: 0x04002BC3 RID: 11203
		public GameObject Target;

		// Token: 0x04002BC4 RID: 11204
		public string StartDate;

		// Token: 0x04002BC5 RID: 11205
		public string EndDate;
	}
}
