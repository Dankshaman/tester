using System;
using UnityEngine;

// Token: 0x020002A8 RID: 680
public class UIContextual : MonoBehaviour
{
	// Token: 0x0600220C RID: 8716 RVA: 0x000F50A0 File Offset: 0x000F32A0
	private void Awake()
	{
		if (!this.beenInit)
		{
			this.Init();
		}
		if (this.nameInput)
		{
			EventDelegate.Add(this.descriptionInput.onChange, new EventDelegate.Callback(this.DelayReposition));
			EventDelegate.Add(this.valueInput.onChange, new EventDelegate.Callback(this.DelayReposition));
			EventDelegate.Add(this.gmNotesInput.onChange, new EventDelegate.Callback(this.DelayReposition));
		}
	}

	// Token: 0x0600220D RID: 8717 RVA: 0x000F511F File Offset: 0x000F331F
	private void OnEnable()
	{
		this.Reposition();
		Wait.Frames(new Action(this.Reposition), 1);
		this.DelayReposition();
	}

	// Token: 0x0600220E RID: 8718 RVA: 0x000F5140 File Offset: 0x000F3340
	private void OnDestroy()
	{
		if (this.nameInput)
		{
			EventDelegate.Remove(this.descriptionInput.onChange, new EventDelegate.Callback(this.DelayReposition));
			EventDelegate.Remove(this.valueInput.onChange, new EventDelegate.Callback(this.DelayReposition));
			EventDelegate.Remove(this.gmNotesInput.onChange, new EventDelegate.Callback(this.DelayReposition));
		}
	}

	// Token: 0x0600220F RID: 8719 RVA: 0x000F51B4 File Offset: 0x000F33B4
	private void Init()
	{
		this.beenInit = true;
		this.BackgroundSprite = base.transform.Find("Background").GetComponent<UISprite>();
		this.TableTransform = base.transform.Find("Table");
		this.TableScript = this.TableTransform.GetComponent<UITable>();
		this.Spacer = this.TableTransform.Find("08 !Spacer : Object");
		if (this.bPrimaryMenu)
		{
			this.TableTransform.GetComponent<UITable>().ignoreChildren = true;
			Transform transform = this.TableTransform.Find("16 Name");
			if (transform)
			{
				this.nameInput = transform.GetComponent<UIInput>();
				this.descriptionInput = this.TableTransform.Find("17 Description").GetComponent<UIInput>();
				this.valueInput = this.TableTransform.Find("18 Value").GetComponent<UIInput>();
				this.gmNotesInput = this.TableTransform.Find("18 GMNotes").GetComponent<UIInput>();
			}
		}
		this.cacheLocalPosition = base.transform.localPosition;
	}

	// Token: 0x06002210 RID: 8720 RVA: 0x000F52C4 File Offset: 0x000F34C4
	public GameObject Add(ContextualEntry contextualEntry)
	{
		GameObject prefab = null;
		GameObject gameObject = this.TableScript.gameObject.AddChild(prefab);
		gameObject.name = "00" + contextualEntry.Name;
		gameObject.GetComponent<UILabel>().text = "            " + contextualEntry.Name;
		gameObject.transform.Find("Images").GetComponent<UISprite>().spriteName = contextualEntry.SpriteName;
		switch (contextualEntry.Type)
		{
		case ContextualType.Expand:
			gameObject.GetComponent<UIHoverEnableObjects>().enabled = true;
			gameObject.transform.Find("Arrow").gameObject.SetActive(true);
			break;
		}
		return gameObject;
	}

	// Token: 0x06002211 RID: 8721 RVA: 0x000F537D File Offset: 0x000F357D
	private void DelayReposition()
	{
		Wait.Stop(this.id);
		this.id = Wait.Time(new Action(this.Reposition), 0.33f, 1);
	}

	// Token: 0x06002212 RID: 8722 RVA: 0x000F53A8 File Offset: 0x000F35A8
	public void Reposition()
	{
		if (!this.beenInit)
		{
			this.Init();
		}
		int num = 0;
		if (this.Spacer)
		{
			bool flag = false;
			if (!this.Spacer.GetComponent<UIContextMenuSpacer>().Open)
			{
				flag = true;
			}
			else
			{
				for (int i = 0; i < this.TableTransform.childCount; i++)
				{
					GameObject gameObject = this.TableTransform.GetChild(i).gameObject;
					if (gameObject.activeSelf && !flag)
					{
						for (int j = 0; j < 8; j++)
						{
							if (gameObject.name.Contains(j.ToString("00")))
							{
								flag = true;
								break;
							}
						}
					}
				}
			}
			this.Spacer.gameObject.SetActive(flag);
		}
		for (int k = 0; k < this.TableTransform.childCount; k++)
		{
			GameObject gameObject2 = this.TableTransform.GetChild(k).gameObject;
			if (gameObject2.activeSelf)
			{
				UIWidget component = gameObject2.GetComponent<UIWidget>();
				if (component)
				{
					if (!gameObject2.GetComponent<UIInput>())
					{
						num += component.height;
					}
				}
				else if (!this.bPrimaryMenu)
				{
					for (int l = 0; l < gameObject2.transform.childCount; l++)
					{
						GameObject gameObject3 = gameObject2.transform.GetChild(l).gameObject;
						if (gameObject3.activeSelf)
						{
							UIWidget component2 = gameObject3.GetComponent<UIWidget>();
							if (component2)
							{
								num += component2.height;
							}
						}
					}
				}
			}
		}
		this.BackgroundSprite.height = num;
		this.TableScript.Reposition();
		this.TableScript.repositionNow = true;
		if (!this.bPrimaryMenu)
		{
			base.transform.localPosition = this.cacheLocalPosition;
		}
		else
		{
			Vector3 vector = UICamera.mainCamera.WorldToViewportPoint(base.transform.position);
			int num2 = num;
			if (this.nameInput)
			{
				num2 += this.nameInput.GetComponent<UIWidget>().height + this.descriptionInput.GetComponent<UIWidget>().height;
				if (this.valueInput.gameObject.activeSelf)
				{
					num2 += this.valueInput.GetComponent<UIWidget>().height;
				}
				if (this.gmNotesInput.gameObject.activeSelf)
				{
					num2 += this.gmNotesInput.GetComponent<UIWidget>().height;
				}
			}
			vector = NetworkSingleton<ManagerPhysicsObject>.Instance.ClampToScreen(vector, (float)(this.BackgroundSprite.width * 2 + 40), (float)num2, SpriteAlignment.BottomRight, true);
			Vector3 vector2 = UICamera.mainCamera.ViewportToWorldPoint(vector);
			vector2 = new Vector3(vector2.x, vector2.y, 0f);
			base.transform.position = vector2;
		}
		base.transform.RoundLocalPosition();
	}

	// Token: 0x0400157F RID: 5503
	private bool beenInit;

	// Token: 0x04001580 RID: 5504
	private UISprite BackgroundSprite;

	// Token: 0x04001581 RID: 5505
	private Transform TableTransform;

	// Token: 0x04001582 RID: 5506
	private UITable TableScript;

	// Token: 0x04001583 RID: 5507
	private Transform Spacer;

	// Token: 0x04001584 RID: 5508
	private UIInput nameInput;

	// Token: 0x04001585 RID: 5509
	private UIInput descriptionInput;

	// Token: 0x04001586 RID: 5510
	private UIInput valueInput;

	// Token: 0x04001587 RID: 5511
	private UIInput gmNotesInput;

	// Token: 0x04001588 RID: 5512
	public bool bPrimaryMenu;

	// Token: 0x04001589 RID: 5513
	private Vector3 cacheLocalPosition = Vector3.zero;

	// Token: 0x0400158A RID: 5514
	private Wait.Identifier id;
}
