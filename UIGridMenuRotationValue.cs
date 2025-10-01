using System;
using System.Collections.Generic;
using RTEditor;

// Token: 0x020002DC RID: 732
public class UIGridMenuRotationValue : UIGridMenu
{
	// Token: 0x060023FA RID: 9210 RVA: 0x000FF31C File Offset: 0x000FD51C
	protected override void Awake()
	{
		base.Awake();
		EventDelegate.Add(this.AddButton.onClick, new EventDelegate.Callback(this.AddOnClick));
	}

	// Token: 0x060023FB RID: 9211 RVA: 0x000FF341 File Offset: 0x000FD541
	protected override void OnDestroy()
	{
		base.OnDestroy();
		EventDelegate.Remove(this.AddButton.onClick, new EventDelegate.Callback(this.AddOnClick));
	}

	// Token: 0x060023FC RID: 9212 RVA: 0x000FF366 File Offset: 0x000FD566
	protected override void OnDisable()
	{
		base.OnDisable();
		this.RotationValueAddMenu.gameObject.SetActive(false);
	}

	// Token: 0x060023FD RID: 9213 RVA: 0x000FF37F File Offset: 0x000FD57F
	private void AddOnClick()
	{
		this.RotationValueAddMenu.Begin(this.TargetNPO, -1);
	}

	// Token: 0x060023FE RID: 9214 RVA: 0x000FF393 File Offset: 0x000FD593
	protected override void OnEnable()
	{
		base.OnEnable();
		if (MonoSingletonBase<EditorObjectSelection>.Instance.LastSelectedGameObject)
		{
			this.TargetNPO = MonoSingletonBase<EditorObjectSelection>.Instance.LastSelectedGameObject.GetComponent<NetworkPhysicsObject>();
			this.Init();
		}
	}

	// Token: 0x060023FF RID: 9215 RVA: 0x000FF3C8 File Offset: 0x000FD5C8
	private void Init()
	{
		if (!this.TargetNPO)
		{
			return;
		}
		List<UIGridMenu.GridButtonRotationValue> list = new List<UIGridMenu.GridButtonRotationValue>();
		List<RotationValue> rotationValues = this.TargetNPO.RotationValues;
		for (int i = 0; i < rotationValues.Count; i++)
		{
			RotationValue rotationValue = rotationValues[i];
			list.Add(new UIGridMenu.GridButtonRotationValue
			{
				Name = rotationValue.value,
				TopLeftText = (i + 1).ToString(),
				NPO = this.TargetNPO,
				index = i
			});
		}
		base.Load<UIGridMenu.GridButtonRotationValue>(list, 1, "ROTATIONS", false, true);
	}

	// Token: 0x06002400 RID: 9216 RVA: 0x000FF461 File Offset: 0x000FD661
	public override void Reload(int page, bool DelayThumbnailCleanup = true)
	{
		base.Reload(page, DelayThumbnailCleanup);
		this.Init();
	}

	// Token: 0x06002401 RID: 9217 RVA: 0x000FF471 File Offset: 0x000FD671
	private void Update()
	{
		if (!MonoSingletonBase<EditorObjectSelection>.Instance.LastSelectedGameObject)
		{
			base.gameObject.SetActive(false);
			return;
		}
	}

	// Token: 0x04001715 RID: 5909
	private NetworkPhysicsObject TargetNPO;

	// Token: 0x04001716 RID: 5910
	public UIButton AddButton;

	// Token: 0x04001717 RID: 5911
	public UIRotationValue RotationValueAddMenu;
}
