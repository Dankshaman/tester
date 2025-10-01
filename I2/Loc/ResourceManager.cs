using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace I2.Loc
{
	// Token: 0x020004B1 RID: 1201
	public class ResourceManager : MonoBehaviour
	{
		// Token: 0x17000722 RID: 1826
		// (get) Token: 0x0600355E RID: 13662 RVA: 0x001643FC File Offset: 0x001625FC
		public static ResourceManager pInstance
		{
			get
			{
				bool flag = ResourceManager.mInstance == null;
				if (ResourceManager.mInstance == null)
				{
					ResourceManager.mInstance = (ResourceManager)UnityEngine.Object.FindObjectOfType(typeof(ResourceManager));
				}
				if (ResourceManager.mInstance == null)
				{
					GameObject gameObject = new GameObject("I2ResourceManager", new Type[]
					{
						typeof(ResourceManager)
					});
					gameObject.hideFlags |= HideFlags.HideAndDontSave;
					ResourceManager.mInstance = gameObject.GetComponent<ResourceManager>();
					SceneManager.sceneLoaded += ResourceManager.MyOnLevelWasLoaded;
				}
				if (flag && Application.isPlaying)
				{
					UnityEngine.Object.DontDestroyOnLoad(ResourceManager.mInstance.gameObject);
				}
				return ResourceManager.mInstance;
			}
		}

		// Token: 0x0600355F RID: 13663 RVA: 0x001644AD File Offset: 0x001626AD
		public static void MyOnLevelWasLoaded(Scene scene, LoadSceneMode mode)
		{
			ResourceManager.pInstance.CleanResourceCache();
			LocalizationManager.UpdateSources();
		}

		// Token: 0x06003560 RID: 13664 RVA: 0x001644C0 File Offset: 0x001626C0
		public T GetAsset<T>(string Name) where T : UnityEngine.Object
		{
			T t = this.FindAsset(Name) as !!0;
			if (t != null)
			{
				return t;
			}
			return this.LoadFromResources<T>(Name);
		}

		// Token: 0x06003561 RID: 13665 RVA: 0x001644F8 File Offset: 0x001626F8
		private UnityEngine.Object FindAsset(string Name)
		{
			if (this.Assets != null)
			{
				int i = 0;
				int num = this.Assets.Length;
				while (i < num)
				{
					if (this.Assets[i] != null && this.Assets[i].name == Name)
					{
						return this.Assets[i];
					}
					i++;
				}
			}
			return null;
		}

		// Token: 0x06003562 RID: 13666 RVA: 0x00164551 File Offset: 0x00162751
		public bool HasAsset(UnityEngine.Object Obj)
		{
			return this.Assets != null && Array.IndexOf<UnityEngine.Object>(this.Assets, Obj) >= 0;
		}

		// Token: 0x06003563 RID: 13667 RVA: 0x00164570 File Offset: 0x00162770
		public T LoadFromResources<T>(string Path) where T : UnityEngine.Object
		{
			T t;
			try
			{
				UnityEngine.Object @object;
				if (string.IsNullOrEmpty(Path))
				{
					t = default(!!0);
					t = t;
				}
				else if (this.mResourcesCache.TryGetValue(Path, out @object) && @object != null)
				{
					t = (@object as !!0);
				}
				else
				{
					T t2 = default(!!0);
					if (Path.EndsWith("]", StringComparison.OrdinalIgnoreCase))
					{
						int num = Path.LastIndexOf("[", StringComparison.OrdinalIgnoreCase);
						int length = Path.Length - num - 2;
						string value = Path.Substring(num + 1, length);
						Path = Path.Substring(0, num);
						T[] array = Resources.LoadAll<T>(Path);
						int i = 0;
						int num2 = array.Length;
						while (i < num2)
						{
							if (array[i].name.Equals(value))
							{
								t2 = array[i];
								break;
							}
							i++;
						}
					}
					else
					{
						t2 = (Resources.Load(Path, typeof(!!0)) as !!0);
					}
					if (t2 == null)
					{
						t2 = this.LoadFromBundle<T>(Path);
					}
					if (t2 != null)
					{
						this.mResourcesCache[Path] = t2;
					}
					t = t2;
				}
			}
			catch (Exception ex)
			{
				Debug.LogErrorFormat("Unable to load {0} '{1}'\nERROR: {2}", new object[]
				{
					typeof(!!0),
					Path,
					ex.ToString()
				});
				t = default(!!0);
			}
			return t;
		}

		// Token: 0x06003564 RID: 13668 RVA: 0x001646FC File Offset: 0x001628FC
		public T LoadFromBundle<T>(string path) where T : UnityEngine.Object
		{
			int i = 0;
			int count = this.mBundleManagers.Count;
			while (i < count)
			{
				if (this.mBundleManagers[i] != null)
				{
					T t = this.mBundleManagers[i].LoadFromBundle(path, typeof(!!0)) as !!0;
					if (t != null)
					{
						return t;
					}
				}
				i++;
			}
			return default(!!0);
		}

		// Token: 0x06003565 RID: 13669 RVA: 0x0016476F File Offset: 0x0016296F
		public void CleanResourceCache()
		{
			this.mResourcesCache.Clear();
			Resources.UnloadUnusedAssets();
			base.CancelInvoke();
		}

		// Token: 0x040021FF RID: 8703
		private static ResourceManager mInstance;

		// Token: 0x04002200 RID: 8704
		public List<IResourceManager_Bundles> mBundleManagers = new List<IResourceManager_Bundles>();

		// Token: 0x04002201 RID: 8705
		public UnityEngine.Object[] Assets;

		// Token: 0x04002202 RID: 8706
		private readonly Dictionary<string, UnityEngine.Object> mResourcesCache = new Dictionary<string, UnityEngine.Object>(StringComparer.Ordinal);
	}
}
