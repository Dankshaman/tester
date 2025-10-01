using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;

namespace Assets.Scripts.UI
{
	// Token: 0x02000399 RID: 921
	public class UITagMenu : MonoBehaviour
	{
		// Token: 0x06002B36 RID: 11062 RVA: 0x00131CBC File Offset: 0x0012FEBC
		public void Awake()
		{
			if (this._isAwake)
			{
				return;
			}
			this._isAwake = true;
			this.TagContainerGame.Initialize((from x in Tags.GameCategory
			orderby x
			select x).ToList<string>());
			this.TagContainerGame.ActiveTags.CollectionChanged += this.ActiveTagsOnCollectionChanged;
			this.TagContainerAsset.Initialize((from x in Tags.Assets
			orderby x
			select x).ToList<string>());
			this.TagContainerAsset.ActiveTags.CollectionChanged += this.ActiveTagsOnCollectionChanged;
			this.TagContainerLanguage.Initialize(Tags.Language.ToList<string>());
			this.TagContainerLanguage.ActiveTags.CollectionChanged += this.ActiveTagsOnCollectionChanged;
			this.TagContainerCustom.ActiveTags.CollectionChanged += this.ActiveTagsOnCollectionChanged;
		}

		// Token: 0x06002B37 RID: 11063 RVA: 0x00131DD0 File Offset: 0x0012FFD0
		public void OnEnable()
		{
			this.OnTabClick(this.TabGame);
		}

		// Token: 0x06002B38 RID: 11064 RVA: 0x00131DE0 File Offset: 0x0012FFE0
		private void ActiveTagsOnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
		{
			NotifyCollectionChangedAction action = e.Action;
			if (action != NotifyCollectionChangedAction.Add)
			{
				if (action != NotifyCollectionChangedAction.Remove)
				{
					return;
				}
			}
			else
			{
				using (IEnumerator<string> enumerator = (from string x in e.NewItems
				where !this.ActiveTags.Contains(x)
				select x).GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						string item = enumerator.Current;
						this.ActiveTags.Add(item);
					}
					return;
				}
			}
			foreach (string item2 in e.OldItems.Cast<string>())
			{
				this.ActiveTags.Remove(item2);
			}
		}

		// Token: 0x06002B39 RID: 11065 RVA: 0x00131EA0 File Offset: 0x001300A0
		public void Initialize(List<string> activeTags)
		{
			this.TagContainerGame.SetActiveTags((from x in activeTags
			where Tags.GameCategory.Contains(x)
			select x).ToList<string>());
			this.TagContainerAsset.SetActiveTags((from x in activeTags
			where Tags.Assets.Contains(x)
			select x).ToList<string>());
			this.TagContainerLanguage.SetActiveTags((from x in activeTags
			where Tags.Language.Contains(x)
			select x).ToList<string>());
			this.TagContainerCustom.SetActiveTags((from x in activeTags
			where !Tags.GameCategory.Contains(x) && !Tags.Assets.Contains(x)
			select x).ToList<string>());
		}

		// Token: 0x06002B3A RID: 11066 RVA: 0x00131F81 File Offset: 0x00130181
		public void SetActive(GameObject sender)
		{
			base.gameObject.SetActive(true);
		}

		// Token: 0x06002B3B RID: 11067 RVA: 0x00131F90 File Offset: 0x00130190
		public void OnTabClick(UIToggle selectedTab)
		{
			if (selectedTab == this.TabGame)
			{
				this.TabGame.Set(true, true);
				this.TabAsset.Set(false, true);
				this.TabLanguage.Set(false, true);
				this.TabCustom.Set(false, true);
				this.ContentGame.SetActive(true);
				this.ContentAsset.SetActive(false);
				this.ContentLanguage.SetActive(false);
				this.ContentCustom.SetActive(false);
				return;
			}
			if (selectedTab == this.TabAsset)
			{
				this.TabGame.Set(false, true);
				this.TabAsset.Set(true, true);
				this.TabLanguage.Set(false, true);
				this.TabCustom.Set(false, true);
				this.ContentGame.SetActive(false);
				this.ContentAsset.SetActive(true);
				this.ContentLanguage.SetActive(false);
				this.ContentCustom.SetActive(false);
				return;
			}
			if (selectedTab == this.TabLanguage)
			{
				this.TabGame.Set(false, true);
				this.TabAsset.Set(false, true);
				this.TabLanguage.Set(true, true);
				this.TabCustom.Set(false, true);
				this.ContentGame.SetActive(false);
				this.ContentAsset.SetActive(false);
				this.ContentLanguage.SetActive(true);
				this.ContentCustom.SetActive(false);
				return;
			}
			if (selectedTab == this.TabCustom)
			{
				this.TabGame.Set(false, true);
				this.TabAsset.Set(false, true);
				this.TabLanguage.Set(false, true);
				this.TabCustom.Set(true, true);
				this.ContentGame.SetActive(false);
				this.ContentAsset.SetActive(false);
				this.ContentLanguage.SetActive(false);
				this.ContentCustom.SetActive(true);
			}
		}

		// Token: 0x04001D50 RID: 7504
		public UIToggle TabGame;

		// Token: 0x04001D51 RID: 7505
		public GameObject ContentGame;

		// Token: 0x04001D52 RID: 7506
		public UITagContainer TagContainerGame;

		// Token: 0x04001D53 RID: 7507
		[FormerlySerializedAs("TabObject")]
		public UIToggle TabAsset;

		// Token: 0x04001D54 RID: 7508
		[FormerlySerializedAs("ContentObject")]
		public GameObject ContentAsset;

		// Token: 0x04001D55 RID: 7509
		[FormerlySerializedAs("TagContainerObject")]
		public UITagContainer TagContainerAsset;

		// Token: 0x04001D56 RID: 7510
		public UIToggle TabLanguage;

		// Token: 0x04001D57 RID: 7511
		public GameObject ContentLanguage;

		// Token: 0x04001D58 RID: 7512
		public UITagContainer TagContainerLanguage;

		// Token: 0x04001D59 RID: 7513
		public UIToggle TabCustom;

		// Token: 0x04001D5A RID: 7514
		public GameObject ContentCustom;

		// Token: 0x04001D5B RID: 7515
		public UITagContainer TagContainerCustom;

		// Token: 0x04001D5C RID: 7516
		public ObservableCollection<string> ActiveTags = new ObservableCollection<string>();

		// Token: 0x04001D5D RID: 7517
		private bool _isAwake;
	}
}
