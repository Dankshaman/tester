using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.UI
{
	// Token: 0x02000398 RID: 920
	public class UITagContainer : MonoBehaviour
	{
		// Token: 0x14000067 RID: 103
		// (add) Token: 0x06002B25 RID: 11045 RVA: 0x001315A4 File Offset: 0x0012F7A4
		// (remove) Token: 0x06002B26 RID: 11046 RVA: 0x001315DC File Offset: 0x0012F7DC
		public event EventHandler TagsRepositioned;

		// Token: 0x17000508 RID: 1288
		// (get) Token: 0x06002B27 RID: 11047 RVA: 0x000F2671 File Offset: 0x000F0871
		public int Width
		{
			get
			{
				return base.gameObject.GetComponent<UISprite>().width;
			}
		}

		// Token: 0x17000509 RID: 1289
		// (get) Token: 0x06002B28 RID: 11048 RVA: 0x000F2683 File Offset: 0x000F0883
		public int Height
		{
			get
			{
				return base.gameObject.GetComponent<UISprite>().height;
			}
		}

		// Token: 0x06002B29 RID: 11049 RVA: 0x00131611 File Offset: 0x0012F811
		private void Awake()
		{
			EventManager.OnLanguageChange += this.OnLanguageChange;
		}

		// Token: 0x06002B2A RID: 11050 RVA: 0x00131624 File Offset: 0x0012F824
		private void OnDestroy()
		{
			EventManager.OnLanguageChange -= this.OnLanguageChange;
		}

		// Token: 0x06002B2B RID: 11051 RVA: 0x00131637 File Offset: 0x0012F837
		private void OnEnable()
		{
			this.OnLanguageChange("", "");
		}

		// Token: 0x06002B2C RID: 11052 RVA: 0x0013164C File Offset: 0x0012F84C
		private void OnLanguageChange(string oldCode = "", string newCode = "")
		{
			for (int i = 0; i < this.Tags.Count; i++)
			{
				this.Tags[i].Resize();
			}
			this.PositionTag();
		}

		// Token: 0x06002B2D RID: 11053 RVA: 0x00131686 File Offset: 0x0012F886
		public void Initialize(List<string> tags)
		{
			this.TagStrings = tags;
			this.Initialize();
		}

		// Token: 0x06002B2E RID: 11054 RVA: 0x00131698 File Offset: 0x0012F898
		public void AddTag(string gameTag, bool active = false, bool positionTag = false)
		{
			UITag component = UnityEngine.Object.Instantiate<GameObject>(this.TagPrefab).GetComponent<UITag>();
			component.Toggle.value = active;
			component.transform.parent = base.gameObject.transform;
			component.transform.localScale = Vector3.one;
			component.name = gameTag;
			component.Text = gameTag;
			component.IsCustomTag = this.RemoveIsDelete;
			component.Resize();
			if (this.ActiveTags.Contains(component.Text))
			{
				component.OnAddClicked(null);
			}
			component.PropertyChanged += this.TagObjectOnPropertyChanged;
			this.Tags.Add(component);
			if (active && !this.ActiveTags.Contains(component.Text))
			{
				component.OnAddClicked(null);
			}
			if (positionTag)
			{
				this.PositionTag();
			}
		}

		// Token: 0x06002B2F RID: 11055 RVA: 0x00131768 File Offset: 0x0012F968
		public void RemoveTag(string gameTag)
		{
			if (this.ActiveTags.Contains(gameTag))
			{
				this.ActiveTags.Remove(gameTag);
			}
			UITag uitag = this.Tags.FirstOrDefault((UITag x) => x.Text == gameTag);
			if (uitag != null)
			{
				uitag.PropertyChanged -= this.TagObjectOnPropertyChanged;
				this.Tags.Remove(uitag);
				UnityEngine.Object.Destroy(uitag.gameObject);
			}
			this.PositionTag();
		}

		// Token: 0x06002B30 RID: 11056 RVA: 0x001317F8 File Offset: 0x0012F9F8
		private void Initialize()
		{
			if (this._initialized)
			{
				return;
			}
			this._initialized = true;
			this._containerWidth = base.gameObject.GetComponent<UISprite>().width;
			UISprite component = base.gameObject.GetComponent<UISprite>();
			component.onChange = (UIWidget.OnDimensionsChanged)Delegate.Combine(component.onChange, new UIWidget.OnDimensionsChanged(this.OnSizeChange));
			foreach (string text in this.TagStrings)
			{
				this.AddTag(text, this.ActiveTags.Contains(text), false);
			}
			this.PositionTag();
		}

		// Token: 0x06002B31 RID: 11057 RVA: 0x001318B0 File Offset: 0x0012FAB0
		public void SetActiveTags(IEnumerable<string> activeTags)
		{
			using (List<string>.Enumerator enumerator = this.ActiveTags.ToList<string>().GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					string activeTag = enumerator.Current;
					UITag uitag = this.Tags.FirstOrDefault((UITag x) => x.Text == activeTag);
					if (uitag != null)
					{
						uitag.OnRemoveClicked(null);
					}
				}
			}
			this.ActiveTags.Clear();
			using (IEnumerator<string> enumerator2 = activeTags.GetEnumerator())
			{
				while (enumerator2.MoveNext())
				{
					string activeTag = enumerator2.Current;
					Debug.Log(activeTag);
					UITag uitag2 = this.Tags.FirstOrDefault((UITag x) => x.Text == activeTag);
					if (uitag2 == null)
					{
						this.AddTag(activeTag, true, true);
					}
					else
					{
						uitag2.OnAddClicked(null);
					}
				}
			}
		}

		// Token: 0x06002B32 RID: 11058 RVA: 0x001319BC File Offset: 0x0012FBBC
		private void OnSizeChange()
		{
			if (this._containerWidth == base.gameObject.GetComponent<UISprite>().width)
			{
				return;
			}
			this._containerWidth = base.gameObject.GetComponent<UISprite>().width;
			List<string> activeTags = this.ActiveTags.ToList<string>();
			foreach (UITag uitag in this.Tags)
			{
				uitag.PropertyChanged -= this.TagObjectOnPropertyChanged;
				UnityEngine.Object.Destroy(uitag.gameObject);
			}
			this.Tags.Clear();
			UISprite component = base.gameObject.GetComponent<UISprite>();
			component.onChange = (UIWidget.OnDimensionsChanged)Delegate.Remove(component.onChange, new UIWidget.OnDimensionsChanged(this.OnSizeChange));
			this._initialized = false;
			this.Initialize();
			this.SetActiveTags(activeTags);
		}

		// Token: 0x06002B33 RID: 11059 RVA: 0x00131AAC File Offset: 0x0012FCAC
		private void PositionTag()
		{
			List<Vector3> list = new List<Vector3>();
			float num = 7.5f;
			float num2 = 5f;
			int width = base.gameObject.GetComponent<UISprite>().width;
			int num3 = -25 - this.OffsetY;
			float num4 = num;
			foreach (UITag uitag in this.Tags)
			{
				int width2 = uitag.gameObject.GetComponent<UISprite>().width;
				if (num4 + 2f * num + (float)width2 > (float)width - num)
				{
					num4 = num;
					num3 -= 38;
				}
				float x = (float)(-(float)width) / 2f + num4 + (float)width2 / 2f + num2;
				list.Add(new Vector3(x, (float)num3, 0f));
				num4 += num2 + (float)width2;
			}
			num3 -= 25;
			base.gameObject.GetComponent<UISprite>().height = num3 * -1;
			for (int i = 0; i < this.Tags.Count; i++)
			{
				UITag uitag2 = this.Tags[i];
				Vector3 localPosition = list[i];
				localPosition.y -= (float)num3 / 2f;
				uitag2.gameObject.transform.localPosition = localPosition;
				uitag2.gameObject.transform.RoundLocalPosition();
			}
			EventHandler tagsRepositioned = this.TagsRepositioned;
			if (tagsRepositioned == null)
			{
				return;
			}
			tagsRepositioned(this, EventArgs.Empty);
		}

		// Token: 0x06002B34 RID: 11060 RVA: 0x00131C28 File Offset: 0x0012FE28
		private void TagObjectOnPropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			UITag uitag;
			if ((uitag = (sender as UITag)) == null)
			{
				return;
			}
			if (uitag.Active && !this.ActiveTags.Contains(uitag.Text))
			{
				this.ActiveTags.Add(uitag.Text);
				return;
			}
			this.ActiveTags.Remove(uitag.Text);
			if (this.RemoveIsDelete)
			{
				this.RemoveTag(uitag.Text);
			}
		}

		// Token: 0x04001D47 RID: 7495
		public GameObject TagPrefab;

		// Token: 0x04001D48 RID: 7496
		public readonly List<UITag> Tags = new List<UITag>();

		// Token: 0x04001D49 RID: 7497
		public readonly ObservableCollection<string> ActiveTags = new ObservableCollection<string>();

		// Token: 0x04001D4B RID: 7499
		public bool RemoveIsDelete;

		// Token: 0x04001D4C RID: 7500
		public int OffsetY;

		// Token: 0x04001D4D RID: 7501
		private int _containerWidth;

		// Token: 0x04001D4E RID: 7502
		private bool _initialized;

		// Token: 0x04001D4F RID: 7503
		public List<string> TagStrings = new List<string>();
	}
}
