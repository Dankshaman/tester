using System;

namespace RTEditor
{
	// Token: 0x0200045C RID: 1116
	public abstract class SingletonBase<T> where T : class, new()
	{
		// Token: 0x1700070F RID: 1807
		// (get) Token: 0x060032EA RID: 13034 RVA: 0x00154BF0 File Offset: 0x00152DF0
		public static T Instance
		{
			get
			{
				return SingletonBase<!0>._instance;
			}
		}

		// Token: 0x040020A6 RID: 8358
		private static T _instance = Activator.CreateInstance<T>();
	}
}
