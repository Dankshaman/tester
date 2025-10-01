using System;
using System.ComponentModel;

namespace Assets.Scripts.UI
{
	// Token: 0x02000397 RID: 919
	public class UIPhysicsOptions : UIReactiveMenu
	{
		// Token: 0x06002B1D RID: 11037 RVA: 0x00131390 File Offset: 0x0012F590
		protected override void Awake()
		{
			base.Awake();
			this.ReactiveElements.Add(this.GravitySlider.onChange);
			this.ReactiveElements.Add(this.PlayAreaSlider.onChange);
			this.ReactiveElements.Add(this.PhysicsFullToggle.onChange);
			this.ReactiveElements.Add(this.PhysicsSemiToggle.onChange);
			this.ReactiveElements.Add(this.PhysicsLockedToggle.onChange);
			NetworkSingleton<GameOptions>.Instance.PropertyChanged += this.OnPropertyChangedGameOptions;
		}

		// Token: 0x06002B1E RID: 11038 RVA: 0x00131427 File Offset: 0x0012F627
		protected override void OnDestroy()
		{
			base.OnDestroy();
			NetworkSingleton<GameOptions>.Instance.PropertyChanged -= this.OnPropertyChangedGameOptions;
		}

		// Token: 0x06002B1F RID: 11039 RVA: 0x000025B8 File Offset: 0x000007B8
		private void OnPropertyChangedGameOptions(object sender, PropertyChangedEventArgs e)
		{
		}

		// Token: 0x06002B20 RID: 11040 RVA: 0x000025B8 File Offset: 0x000007B8
		protected override void NetworkSync()
		{
		}

		// Token: 0x06002B21 RID: 11041 RVA: 0x00131445 File Offset: 0x0012F645
		protected override void OnEnable()
		{
			base.OnEnable();
			this.ReloadUI();
		}

		// Token: 0x06002B22 RID: 11042 RVA: 0x00131454 File Offset: 0x0012F654
		protected override void ReloadUI()
		{
			this.BlockUpdateSource = true;
			this.GravitySlider.GetComponentInChildren<UISliderRange>().floatValue = NetworkSingleton<GameOptions>.Instance.Gravity;
			this.PlayAreaSlider.value = NetworkSingleton<GameOptions>.Instance.PlayArea;
			this.PhysicsFullToggle.value = (NetworkSingleton<ServerOptions>.Instance.Physics == ServerOptions.PhysicsMode.Full);
			this.PhysicsSemiToggle.value = (NetworkSingleton<ServerOptions>.Instance.Physics == ServerOptions.PhysicsMode.Semi);
			this.PhysicsLockedToggle.value = (NetworkSingleton<ServerOptions>.Instance.Physics == ServerOptions.PhysicsMode.Lock);
			this.BlockUpdateSource = false;
		}

		// Token: 0x06002B23 RID: 11043 RVA: 0x001314E8 File Offset: 0x0012F6E8
		protected override void UpdateSource()
		{
			this.BlockUpdateSource = true;
			NetworkSingleton<GameOptions>.Instance.Gravity = this.GravitySlider.GetComponentInChildren<UISliderRange>().floatValue;
			if (this.PlayAreaSlider.value != NetworkSingleton<GameOptions>.Instance.PlayArea)
			{
				NetworkSingleton<GameOptions>.Instance.PlayArea = this.PlayAreaSlider.value;
				NetworkSingleton<GameOptions>.Instance.BoundsVisual.GetComponent<BoundsVisual>().FadeInFor(2f);
			}
			if (this.PhysicsFullToggle.value)
			{
				NetworkSingleton<ServerOptions>.Instance.Physics = ServerOptions.PhysicsMode.Full;
			}
			else if (this.PhysicsSemiToggle.value)
			{
				NetworkSingleton<ServerOptions>.Instance.Physics = ServerOptions.PhysicsMode.Semi;
			}
			else
			{
				NetworkSingleton<ServerOptions>.Instance.Physics = ServerOptions.PhysicsMode.Lock;
			}
			this.BlockUpdateSource = false;
		}

		// Token: 0x04001D41 RID: 7489
		private const float BOUNDS_VISUAL_DURATION = 2f;

		// Token: 0x04001D42 RID: 7490
		public UISlider GravitySlider;

		// Token: 0x04001D43 RID: 7491
		public UISlider PlayAreaSlider;

		// Token: 0x04001D44 RID: 7492
		public UIToggle PhysicsFullToggle;

		// Token: 0x04001D45 RID: 7493
		public UIToggle PhysicsSemiToggle;

		// Token: 0x04001D46 RID: 7494
		public UIToggle PhysicsLockedToggle;
	}
}
