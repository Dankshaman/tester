using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200030A RID: 778
public class UIObjectIndicatorManager : Singleton<UIObjectIndicatorManager>
{
	// Token: 0x060025C4 RID: 9668 RVA: 0x00109D53 File Offset: 0x00107F53
	private static string TypeToSprite(UIObjectIndicatorManager.IndicatorType Type)
	{
		switch (Type)
		{
		case UIObjectIndicatorManager.IndicatorType.Search:
			return "Icon-DeckSearch";
		case UIObjectIndicatorManager.IndicatorType.Peek:
			return "Icon-Peek";
		case UIObjectIndicatorManager.IndicatorType.ObjectEnteredContainer:
			return "Icon-EnterContainer";
		default:
			return null;
		}
	}

	// Token: 0x060025C5 RID: 9669 RVA: 0x00109D7C File Offset: 0x00107F7C
	protected override void Awake()
	{
		base.Awake();
		EventManager.OnBlindfold += this.Blindfold;
		EventManager.OnVR += this.OnVR;
		EventManager.OnObjectEnterContainer += this.OnObjectEnterContainer;
	}

	// Token: 0x060025C6 RID: 9670 RVA: 0x00109DB7 File Offset: 0x00107FB7
	private void Start()
	{
		this.MainCamera = Camera.main;
		this.UIDrawCamera = UICamera.mainCamera;
	}

	// Token: 0x060025C7 RID: 9671 RVA: 0x00109DCF File Offset: 0x00107FCF
	private void OnDestroy()
	{
		EventManager.OnBlindfold -= this.Blindfold;
		EventManager.OnVR -= this.OnVR;
		EventManager.OnObjectEnterContainer -= this.OnObjectEnterContainer;
	}

	// Token: 0x060025C8 RID: 9672 RVA: 0x00109E04 File Offset: 0x00108004
	private void Blindfold(bool bBlind, int id)
	{
		if (id != NetworkID.ID)
		{
			return;
		}
		base.gameObject.SetActive(!bBlind);
	}

	// Token: 0x060025C9 RID: 9673 RVA: 0x00109E20 File Offset: 0x00108020
	private void OnVR(bool bEnabled)
	{
		if (VRHMD.isVR)
		{
			base.gameObject.transform.parent = NetworkSingleton<NetworkUI>.Instance.GUIUIRoot3D.transform;
			base.gameObject.layer = 17;
			base.gameObject.transform.Reset();
			this.MainCamera = Singleton<VRHMD>.Instance.VRCamera;
		}
	}

	// Token: 0x060025CA RID: 9674 RVA: 0x00109E80 File Offset: 0x00108080
	private void OnObjectEnterContainer(NetworkPhysicsObject Container, NetworkPhysicsObject Object)
	{
		if (!UIObjectIndicatorManager.ShowObjectEnterContainerIndicator)
		{
			return;
		}
		int num = Object.PrevHeldByPlayerID;
		if (num < 0)
		{
			num = Object.HeldByPlayerID;
		}
		if (num < 0)
		{
			Container.AddObjectEnteredContainerIndicator("Grey");
			return;
		}
		Pointer pointer = NetworkSingleton<ManagerPhysicsObject>.Instance.PointerFromID(num);
		if (Container)
		{
			Container.AddObjectEnteredContainerIndicator((pointer != null) ? pointer.PointerColorLabel : "Grey");
		}
	}

	// Token: 0x060025CB RID: 9675 RVA: 0x00109EE8 File Offset: 0x001080E8
	private UIObjectIndicatorManager.IndicatorGrid GetIndicatorGrid(NetworkPhysicsObject targetNPO, bool FindInUseOnly = false)
	{
		UIObjectIndicatorManager.IndicatorGrid indicatorGrid;
		for (int i = 0; i < this.IndicatorsGrids.Count; i++)
		{
			indicatorGrid = this.IndicatorsGrids[i];
			if (indicatorGrid.grid.gameObject.activeSelf && indicatorGrid.targetNPO == targetNPO)
			{
				return indicatorGrid;
			}
		}
		if (FindInUseOnly)
		{
			return null;
		}
		for (int j = 0; j < this.IndicatorsGrids.Count; j++)
		{
			indicatorGrid = this.IndicatorsGrids[j];
			if (!indicatorGrid.grid.gameObject.activeSelf)
			{
				return indicatorGrid;
			}
		}
		indicatorGrid = new UIObjectIndicatorManager.IndicatorGrid();
		indicatorGrid.grid = base.gameObject.AddChild(this.IndicatorGridPrefab).GetComponent<UIGrid>();
		this.IndicatorsGrids.Add(indicatorGrid);
		return indicatorGrid;
	}

	// Token: 0x060025CC RID: 9676 RVA: 0x00109FA8 File Offset: 0x001081A8
	private UIObjectIndicator GetIndicator(UIObjectIndicatorManager.IndicatorGrid indicatorGrid, UIObjectIndicatorManager.IndicatorType type, string color, bool FindInUseOnly = false)
	{
		UIObjectIndicator uiobjectIndicator;
		for (int i = 0; i < indicatorGrid.indicators.Count; i++)
		{
			uiobjectIndicator = indicatorGrid.indicators[i];
			if (uiobjectIndicator.gameObject.activeSelf && uiobjectIndicator.type == type && uiobjectIndicator.color == color)
			{
				return uiobjectIndicator;
			}
		}
		if (FindInUseOnly)
		{
			return null;
		}
		for (int j = 0; j < indicatorGrid.indicators.Count; j++)
		{
			uiobjectIndicator = indicatorGrid.indicators[j];
			if (!uiobjectIndicator.gameObject.activeSelf)
			{
				return uiobjectIndicator;
			}
		}
		uiobjectIndicator = indicatorGrid.grid.gameObject.AddChild(this.IndicatorPrefab).GetComponent<UIObjectIndicator>();
		indicatorGrid.indicators.Add(uiobjectIndicator);
		return uiobjectIndicator;
	}

	// Token: 0x060025CD RID: 9677 RVA: 0x0010A064 File Offset: 0x00108264
	public void CancelColor(string color)
	{
		for (int i = 0; i < this.IndicatorsGrids.Count; i++)
		{
			UIObjectIndicatorManager.IndicatorGrid indicatorGrid = this.IndicatorsGrids[i];
			if (indicatorGrid.grid && indicatorGrid.grid.gameObject.activeSelf)
			{
				this.RemoveIndicator(indicatorGrid.targetNPO, UIObjectIndicatorManager.IndicatorType.Peek, color);
				this.RemoveIndicator(indicatorGrid.targetNPO, UIObjectIndicatorManager.IndicatorType.Search, color);
				this.RemoveIndicator(indicatorGrid.targetNPO, UIObjectIndicatorManager.IndicatorType.ObjectEnteredContainer, color);
			}
		}
	}

	// Token: 0x060025CE RID: 9678 RVA: 0x0010A0E0 File Offset: 0x001082E0
	public void AddIndicator(NetworkPhysicsObject targetNPO, UIObjectIndicatorManager.IndicatorType type, string color, string label = "")
	{
		if (!targetNPO || targetNPO.IsInvisible)
		{
			return;
		}
		UIObjectIndicatorManager.IndicatorGrid indicatorGrid = this.GetIndicatorGrid(targetNPO, false);
		indicatorGrid.grid.gameObject.SetActive(true);
		indicatorGrid.targetNPO = targetNPO;
		UIObjectIndicator indicator = this.GetIndicator(indicatorGrid, type, color, false);
		indicator.gameObject.SetActive(false);
		indicator.sprite.spriteName = UIObjectIndicatorManager.TypeToSprite(type);
		indicator.sprite.color = Colour.ColourFromLabel(color);
		indicator.color = color;
		indicator.type = type;
		indicator.targetNPO = targetNPO;
		if (label != "")
		{
			indicator.label.text = label;
			indicator.label.color = indicator.sprite.color;
			indicator.label.gameObject.SetActive(true);
		}
		else
		{
			indicator.label.gameObject.SetActive(false);
		}
		indicator.gameObject.SetActive(true);
		indicatorGrid.grid.repositionNow = true;
		this.LateUpdate();
	}

	// Token: 0x060025CF RID: 9679 RVA: 0x0010A1E8 File Offset: 0x001083E8
	public void RemoveIndicator(NetworkPhysicsObject targetNPO, UIObjectIndicatorManager.IndicatorType type, string color = "White")
	{
		UIObjectIndicatorManager.IndicatorGrid indicatorGrid = this.GetIndicatorGrid(targetNPO, true);
		if (indicatorGrid == null)
		{
			return;
		}
		UIObjectIndicator uiobjectIndicator = this.GetIndicator(indicatorGrid, type, color, true);
		if (uiobjectIndicator == null)
		{
			return;
		}
		uiobjectIndicator.gameObject.SetActive(false);
		bool flag = true;
		for (int i = 0; i < indicatorGrid.indicators.Count; i++)
		{
			uiobjectIndicator = indicatorGrid.indicators[i];
			if (uiobjectIndicator.gameObject.activeSelf)
			{
				flag = false;
				break;
			}
		}
		if (flag)
		{
			indicatorGrid.grid.gameObject.SetActive(false);
		}
		indicatorGrid.grid.repositionNow = true;
	}

	// Token: 0x060025D0 RID: 9680 RVA: 0x0010A27C File Offset: 0x0010847C
	private void LateUpdate()
	{
		for (int i = 0; i < this.IndicatorsGrids.Count; i++)
		{
			UIObjectIndicatorManager.IndicatorGrid indicatorGrid = this.IndicatorsGrids[i];
			if (indicatorGrid.grid.gameObject.activeSelf)
			{
				NetworkPhysicsObject targetNPO = indicatorGrid.targetNPO;
				if (!targetNPO)
				{
					indicatorGrid.grid.gameObject.SetActive(false);
					for (int j = 0; j < indicatorGrid.indicators.Count; j++)
					{
						UIObjectIndicator uiobjectIndicator = indicatorGrid.indicators[j];
						if (uiobjectIndicator.gameObject.activeSelf)
						{
							uiobjectIndicator.gameObject.SetActive(false);
						}
					}
				}
				else
				{
					Vector3 position = targetNPO.transform.position;
					position.y += 0.5f + targetNPO.GetBoundsNotNormalized().extents.y;
					if (!VRHMD.isVR)
					{
						Vector3 vector = this.MainCamera.WorldToViewportPoint(position);
						if (vector.x > 0f && vector.x < 1f && vector.y > 0f && vector.y < 1f)
						{
							Vector3 vector2 = this.MainCamera.WorldToScreenPoint(position);
							if (vector2.z >= 0f)
							{
								vector2.z = 0f;
							}
							Vector3 position2 = this.UIDrawCamera.ScreenToWorldPoint(vector2);
							indicatorGrid.grid.transform.position = position2;
							indicatorGrid.grid.transform.RoundLocalPosition();
							indicatorGrid.grid.transform.localScale = Vector3.one;
							for (int k = 0; k < indicatorGrid.indicators.Count; k++)
							{
								UIObjectIndicator uiobjectIndicator2 = indicatorGrid.indicators[k];
								if (uiobjectIndicator2.gameObject.activeSelf)
								{
									uiobjectIndicator2.sprite.depth = (int)(100000f / Vector3.Distance(position, this.MainCamera.transform.position));
								}
							}
						}
						else
						{
							Vector3 vector3 = NGUITools.screenSize;
							float num = vector3.x / vector3.y;
							float num2 = (float)NetworkSingleton<NetworkUI>.Instance.GUIUIRoot.GetComponent<UIRoot>().activeHeight / 800f;
							if (vector.z < 0f)
							{
								vector.y = -vector.y;
							}
							Vector3 vector4 = Vector3.one;
							if (vector.x <= 0f)
							{
								vector4 *= 1f / (Mathf.Abs(vector.x) + 1f);
							}
							else if (vector.x >= 1f)
							{
								vector4 *= 1f / vector.x;
							}
							if (vector.y <= 0f)
							{
								vector4 *= 1f / (Mathf.Abs(vector.y) + 1f);
							}
							else if (vector.y >= 1f)
							{
								vector4 *= 1f / vector.y;
							}
							vector4.x = Mathf.Clamp(vector4.x, 0.5f, 1f);
							vector4.y = Mathf.Clamp(vector4.y, 0.5f, 1f);
							vector4.z = Mathf.Clamp(vector4.z, 0.5f, 1f);
							vector.x = Mathf.Clamp(vector.x, 0.08f, 0.95f);
							vector.y = Mathf.Clamp(vector.y, 0.05f, 0.85f);
							Vector3 localPosition = new Vector3((vector.x - 0.5f) * (800f * num * num2), (vector.y - 0.5f) * (800f * num2), 0f);
							indicatorGrid.grid.transform.localPosition = localPosition;
							indicatorGrid.grid.transform.RoundLocalPosition();
							indicatorGrid.grid.transform.localScale = vector4;
						}
					}
					else
					{
						indicatorGrid.grid.transform.position = position;
						indicatorGrid.grid.transform.eulerAngles = new Vector3(45f, this.MainCamera.transform.eulerAngles.y, 0f);
						indicatorGrid.grid.transform.localScale = Vector3.one * Vector3.Distance(position, this.MainCamera.transform.position) / 15f;
					}
				}
			}
		}
	}

	// Token: 0x04001876 RID: 6262
	public static bool ShowObjectEnterContainerIndicator = true;

	// Token: 0x04001877 RID: 6263
	public GameObject IndicatorGridPrefab;

	// Token: 0x04001878 RID: 6264
	public GameObject IndicatorPrefab;

	// Token: 0x04001879 RID: 6265
	private List<UIObjectIndicatorManager.IndicatorGrid> IndicatorsGrids = new List<UIObjectIndicatorManager.IndicatorGrid>();

	// Token: 0x0400187A RID: 6266
	private Camera MainCamera;

	// Token: 0x0400187B RID: 6267
	private Camera UIDrawCamera;

	// Token: 0x02000770 RID: 1904
	public class IndicatorGrid
	{
		// Token: 0x04002BED RID: 11245
		public UIGrid grid;

		// Token: 0x04002BEE RID: 11246
		public NetworkPhysicsObject targetNPO;

		// Token: 0x04002BEF RID: 11247
		public List<UIObjectIndicator> indicators = new List<UIObjectIndicator>();
	}

	// Token: 0x02000771 RID: 1905
	public enum IndicatorType
	{
		// Token: 0x04002BF1 RID: 11249
		Search,
		// Token: 0x04002BF2 RID: 11250
		Peek,
		// Token: 0x04002BF3 RID: 11251
		ObjectEnteredContainer
	}
}
