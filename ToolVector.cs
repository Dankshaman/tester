using System;
using System.Collections.Generic;
using System.Diagnostics;
using NewNet;
using UnityEngine;

// Token: 0x0200025D RID: 605
public class ToolVector : NetworkSingleton<ToolVector>
{
	// Token: 0x17000446 RID: 1094
	// (get) Token: 0x06001FC5 RID: 8133 RVA: 0x000E2AA2 File Offset: 0x000E0CA2
	// (set) Token: 0x06001FC6 RID: 8134 RVA: 0x000E2AAC File Offset: 0x000E0CAC
	public Color DrawColor
	{
		get
		{
			return this._drawColor;
		}
		private set
		{
			if (this._drawColor != value)
			{
				this._drawColor = value;
				this.ColorPicker.SetColor(value);
				this.drawIndicator.startColor = value.GammaToLinearSpace();
				this.drawIndicator.endColor = value.GammaToLinearSpace();
			}
		}
	}

	// Token: 0x17000447 RID: 1095
	// (get) Token: 0x06001FC7 RID: 8135 RVA: 0x000E2AFC File Offset: 0x000E0CFC
	// (set) Token: 0x06001FC8 RID: 8136 RVA: 0x000E2B04 File Offset: 0x000E0D04
	public float DrawThickness
	{
		get
		{
			return this._drawThickess;
		}
		private set
		{
			value = Mathf.Clamp(value, 0.025f, 5f);
			if (this._drawThickess != value)
			{
				this._drawThickess = value;
				this.drawIndicator.widthMultiplier = value;
				this.drawIndicator.numCapVertices = this.GetNumCapVertices(value);
			}
		}
	}

	// Token: 0x06001FC9 RID: 8137 RVA: 0x000E2B54 File Offset: 0x000E0D54
	private void Start()
	{
		NetworkEvents.OnPlayerConnected += this.NetworkEvents_OnPlayerConnected;
		EventManager.OnPlayerChangeColor += this.OnPlayerChangeColor;
		EventManager.OnNetworkObjectDestroy += this.EventManager_OnNetworkObjectDestroy;
		EventManager.OnResetTable += this.EventManager_OnResetTable;
		this.ColorPicker.ColorChanged += this.ColorPicker_ColorChanged;
		this.InitDrawingIndicator();
		this.pixelObject = new GameObject("PixelObject");
		Mesh mesh = new Mesh();
		mesh.MarkDynamic();
		this.pixelObject.AddComponent<MeshFilter>().mesh = mesh;
		this.pixelObject.AddComponent<MeshRenderer>().material = this.drawIndicator.material;
		this.pixelObject.transform.parent = base.transform;
	}

	// Token: 0x06001FCA RID: 8138 RVA: 0x000E2C20 File Offset: 0x000E0E20
	private void OnDestroy()
	{
		NetworkEvents.OnPlayerConnected -= this.NetworkEvents_OnPlayerConnected;
		EventManager.OnPlayerChangeColor -= this.OnPlayerChangeColor;
		EventManager.OnNetworkObjectDestroy -= this.EventManager_OnNetworkObjectDestroy;
		EventManager.OnResetTable -= this.EventManager_OnResetTable;
		this.ColorPicker.ColorChanged -= this.ColorPicker_ColorChanged;
	}

	// Token: 0x06001FCB RID: 8139 RVA: 0x000E2C88 File Offset: 0x000E0E88
	private void LateUpdate()
	{
		this.pointerMode = (PlayerScript.Pointer ? PlayerScript.PointerScript.CurrentPointerMode : PointerMode.None);
		this.DrawIndicator(Pointer.IsVectorTool(this.pointerMode) && this.pointerMode != PointerMode.VectorErase && !this.checkDrawing);
		if (!Pointer.IsVectorTool(this.pointerMode))
		{
			return;
		}
		if (UICamera.HoveredUIObject && !VRHMD.isVR)
		{
			return;
		}
		if (zInput.GetButtonDown("Tap", ControlType.All) && this.drawing)
		{
			this.RPCRemoveLine(this.currentDrawingGuid);
			base.networkView.RPC<uint>(RPCTarget.Others, new Action<uint>(this.RPCRemoveLine), this.currentDrawingGuid);
			this.EndDrawing();
		}
		if (zInput.GetRecursiveButton("Scale Up", ref this.recursiveScaleHolder, 0.1f, ControlType.All))
		{
			this.DrawThickness += 0.05f;
		}
		if (zInput.GetRecursiveButton("Scale Down", ref this.recursiveScaleHolder, 0.1f, ControlType.All))
		{
			this.DrawThickness -= 0.05f;
		}
		PointerMode pointerMode = this.pointerMode;
		if (pointerMode == PointerMode.Vector)
		{
			this.UpdateVectorDraw();
			return;
		}
		if (pointerMode == PointerMode.VectorErase)
		{
			this.UpdateVectorErase();
			return;
		}
		switch (pointerMode)
		{
		case PointerMode.VectorLine:
			this.UpdateVectorLine();
			return;
		case PointerMode.VectorCircle:
			this.UpdateVectorCircle();
			return;
		case PointerMode.VectorBox:
			this.UpdateVectorBox();
			return;
		case PointerMode.VectorPixel:
			this.UpdateVectorPixel();
			return;
		default:
			return;
		}
	}

	// Token: 0x06001FCC RID: 8140 RVA: 0x000E2DEC File Offset: 0x000E0FEC
	private void InitDrawingIndicator()
	{
		this.drawIndicator = UnityEngine.Object.Instantiate<GameObject>(this.LinePrefab, base.transform).GetComponent<LineRenderer>();
		this.drawIndicator.positionCount = 2;
		this.drawIndicator.gameObject.name = "DrawIndicator";
		this.drawIndicator.gameObject.SetActive(false);
	}

	// Token: 0x06001FCD RID: 8141 RVA: 0x000E2E48 File Offset: 0x000E1048
	private void DrawIndicator(bool enable)
	{
		this.drawIndicator.gameObject.SetActive(enable);
		if (enable)
		{
			this.drawIndicator.transform.rotation = (HoverScript.HoverLockLock ? Quaternion.FromToRotation(Vector3.up, HoverScript.ObjectLockHit.normal) : Quaternion.identity) * Quaternion.Euler(new Vector3(90f, 0f, 0f));
			if (HoverScript.HoverLockLock)
			{
				Vector3 localScale = HoverScript.HoverLockLock.transform.localScale;
				this.drawIndicator.transform.localScale = new Vector3(localScale.x, localScale.z, localScale.y);
			}
			else
			{
				this.drawIndicator.transform.localScale = Vector3.one;
			}
			this.drawIndicator.sortingOrder = this.GetNextSortOrder();
			Vector3 vector = this.drawIndicator.transform.InverseTransformPoint(this.GetDrawPosition());
			if (this.pointerMode == PointerMode.VectorPixel)
			{
				Vector3 position = vector;
				position.x -= this.DrawThickness / 2f;
				Vector3 position2 = vector;
				position2.x += this.DrawThickness / 2f;
				this.drawIndicator.SetPosition(0, position);
				this.drawIndicator.SetPosition(1, position2);
				this.drawIndicator.loop = true;
				return;
			}
			this.drawIndicator.SetPosition(0, vector);
			this.drawIndicator.SetPosition(1, vector);
			this.drawIndicator.loop = false;
		}
	}

	// Token: 0x06001FCE RID: 8142 RVA: 0x000E2FD1 File Offset: 0x000E11D1
	private int GetNumCapVertices(float thickness)
	{
		return (int)(4f + Mathf.Sqrt(thickness) * 7f);
	}

	// Token: 0x06001FCF RID: 8143 RVA: 0x000E2FE8 File Offset: 0x000E11E8
	private void NetworkEvents_OnPlayerConnected(NetworkPlayer player)
	{
		if (Network.isServer)
		{
			List<ToolVector.LineNetworkData> list = new List<ToolVector.LineNetworkData>();
			foreach (KeyValuePair<uint, ToolVector.VectorDrawData> keyValuePair in this.drawnLines)
			{
				ToolVector.VectorDrawData value = keyValuePair.Value;
				Vector3[] positions = new Vector3[value.line.positionCount];
				value.line.GetPositions(positions);
				list.Add(new ToolVector.LineNetworkData(keyValuePair.Key, value.attached, positions, value.color, value.thickness, value.rotation, value.loop, value.square));
			}
			if (list.Count > 0)
			{
				base.networkView.RPC<List<ToolVector.LineNetworkData>>(player, new Action<List<ToolVector.LineNetworkData>>(this.RPCAddLines), list);
			}
		}
	}

	// Token: 0x06001FD0 RID: 8144 RVA: 0x000E30C0 File Offset: 0x000E12C0
	private void EventManager_OnResetTable()
	{
		if (Network.isServer && this.drawnLines.Count > 0)
		{
			this.RemoveAll();
		}
	}

	// Token: 0x06001FD1 RID: 8145 RVA: 0x000E30DD File Offset: 0x000E12DD
	private void ColorPicker_ColorChanged(Color color)
	{
		this.DrawColor = color;
	}

	// Token: 0x06001FD2 RID: 8146 RVA: 0x000E30E6 File Offset: 0x000E12E6
	private void OnPlayerChangeColor(PlayerState playerState)
	{
		if (playerState.IsMe())
		{
			this.DrawColor = playerState.color;
		}
	}

	// Token: 0x06001FD3 RID: 8147 RVA: 0x000E30FC File Offset: 0x000E12FC
	private void EventManager_OnNetworkObjectDestroy(NetworkPhysicsObject NPO)
	{
		List<uint> list = null;
		foreach (KeyValuePair<uint, ToolVector.VectorDrawData> keyValuePair in this.drawnLines)
		{
			if (keyValuePair.Value.attached == NPO)
			{
				if (list == null)
				{
					list = new List<uint>();
				}
				list.Add(keyValuePair.Key);
			}
		}
		if (list != null)
		{
			foreach (uint key in list)
			{
				this.drawnLines.Remove(key);
			}
		}
	}

	// Token: 0x06001FD4 RID: 8148 RVA: 0x000E31B8 File Offset: 0x000E13B8
	private void StartDrawing(int NumberPositions, bool loop = false, bool square = false, Vector3? drawPos = null)
	{
		this.drawing = true;
		this.lastPixelDraw = HoverScript.PointerPixel;
		this.currentDrawingGuid = this.GetGUID();
		this.startDrawPosition = (drawPos ?? this.GetDrawPosition());
		NetworkPhysicsObject attached = HoverScript.HoverLockLock ? HoverScript.HoverLockLock.GetComponent<NetworkPhysicsObject>() : null;
		Vector3[] array = new Vector3[NumberPositions];
		for (int i = 0; i < NumberPositions; i++)
		{
			array[i] = this.startDrawPosition;
		}
		if (this.pointerMode == PointerMode.VectorPixel)
		{
			Vector3[] array2 = array;
			int num = 0;
			array2[num].x = array2[num].x - this.DrawThickness / 2f;
			Vector3[] array3 = array;
			int num2 = 1;
			array3[num2].x = array3[num2].x + this.DrawThickness / 2f;
		}
		Vector3 rotation = Vector3.zero;
		if (HoverScript.HoverLockLock)
		{
			rotation = Quaternion.FromToRotation(Vector3.up, HoverScript.HoverLockLock.transform.InverseTransformDirection(HoverScript.ObjectLockHit.normal)).eulerAngles;
		}
		ToolVector.LineNetworkData lineNetworkData = new ToolVector.LineNetworkData(this.currentDrawingGuid, attached, array, this.DrawColor, this.DrawThickness, rotation, loop, square);
		if (this.pointerMode == PointerMode.VectorPixel)
		{
			base.networkView.RPC<ToolVector.LineNetworkData>(RPCTarget.Others, new Action<ToolVector.LineNetworkData>(this.RPCAddPixel), lineNetworkData);
			this.RPCAddPixel(lineNetworkData);
			return;
		}
		base.networkView.RPC<ToolVector.LineNetworkData>(RPCTarget.Others, new Action<ToolVector.LineNetworkData>(this.RPCAddLine), lineNetworkData);
		this.RPCAddLine(lineNetworkData);
	}

	// Token: 0x06001FD5 RID: 8149 RVA: 0x000E3338 File Offset: 0x000E1538
	private bool CheckMove()
	{
		if ((Vector2.Distance(HoverScript.PointerPixel, this.lastPixelDraw) > 5f || VRHMD.isVR) && this.drawnLines.ContainsKey(this.currentDrawingGuid))
		{
			this.checkDrawing = true;
			this.lastPixelDraw = HoverScript.PointerPixel;
			return true;
		}
		return false;
	}

	// Token: 0x06001FD6 RID: 8150 RVA: 0x000E3395 File Offset: 0x000E1595
	private void EndDrawing()
	{
		this.drawing = false;
		this.checkDrawing = false;
		this.currentDrawingGuid = 0U;
		this.startDrawPosition = Vector3.zero;
		this.lastPixelDraw = Vector2.zero;
	}

	// Token: 0x06001FD7 RID: 8151 RVA: 0x00066A03 File Offset: 0x00064C03
	private bool VectorAction()
	{
		return zInput.GetButton("Grab", ControlType.All) || (VRHMD.isVR && VRTrackedController.ToolAction);
	}

	// Token: 0x06001FD8 RID: 8152 RVA: 0x00066A22 File Offset: 0x00064C22
	private bool VectorActionDown()
	{
		return zInput.GetButtonDown("Grab", ControlType.All) || (VRHMD.isVR && VRTrackedController.ToolActionDown);
	}

	// Token: 0x06001FD9 RID: 8153 RVA: 0x00066A41 File Offset: 0x00064C41
	private bool VectorActionUp()
	{
		return zInput.GetButtonUp("Grab", ControlType.All) || (VRHMD.isVR && VRTrackedController.ToolActionUp);
	}

	// Token: 0x06001FDA RID: 8154 RVA: 0x000E33C4 File Offset: 0x000E15C4
	private void UpdateVectorDraw()
	{
		if (this.VectorActionDown())
		{
			this.StartDrawing(1, false, false, null);
		}
		if (this.VectorAction() && this.drawing && this.CheckMove())
		{
			base.networkView.RPC<uint, Vector3>(RPCTarget.Others, new Action<uint, Vector3>(this.RPCAddPositionToLine), this.currentDrawingGuid, this.GetDrawPosition());
			this.RPCAddPositionToLine(this.currentDrawingGuid, this.GetDrawPosition());
		}
		if (this.VectorActionUp())
		{
			ToolVector.VectorDrawData vectorDrawData;
			if (this.drawnLines.TryGetValue(this.currentDrawingGuid, out vectorDrawData) && vectorDrawData.line.positionCount < 2)
			{
				base.networkView.RPC<uint, Vector3>(RPCTarget.Others, new Action<uint, Vector3>(this.RPCAddPositionToLine), this.currentDrawingGuid, this.GetDrawPosition());
				this.RPCAddPositionToLine(this.currentDrawingGuid, this.GetDrawPosition());
			}
			this.EndDrawing();
		}
	}

	// Token: 0x06001FDB RID: 8155 RVA: 0x000E34A0 File Offset: 0x000E16A0
	private void UpdateVectorLine()
	{
		if (this.VectorActionDown())
		{
			this.StartDrawing(2, false, false, null);
		}
		if (this.VectorAction() && this.drawing && this.CheckMove())
		{
			base.networkView.RPC<uint, int, Vector3>(RPCTarget.Others, new Action<uint, int, Vector3>(this.RPCUpdatePositionLine), this.currentDrawingGuid, 1, this.GetDrawPosition());
			this.RPCUpdatePositionLine(this.currentDrawingGuid, 1, this.GetDrawPosition());
		}
		if (this.VectorActionUp())
		{
			this.EndDrawing();
		}
	}

	// Token: 0x06001FDC RID: 8156 RVA: 0x000E3528 File Offset: 0x000E1728
	private void UpdateVectorBox()
	{
		if (this.VectorActionDown())
		{
			this.StartDrawing(this.BoxPositions.Length, true, false, null);
		}
		ToolVector.VectorDrawData vectorDrawData;
		if (this.VectorAction() && this.drawing && this.CheckMove() && this.drawnLines.TryGetValue(this.currentDrawingGuid, out vectorDrawData))
		{
			Vector3 drawPosition = this.GetDrawPosition();
			Vector3 pivot = (this.startDrawPosition + drawPosition) / 2f;
			float num = (drawPosition.y > this.startDrawPosition.y) ? drawPosition.y : this.startDrawPosition.y;
			num += 5f;
			Quaternion qangles = Quaternion.Euler(new Vector3(0f, HoverScript.MainCamera.transform.eulerAngles.y * 2f, 0f));
			Vector3 point = new Vector3(drawPosition.x, num, this.startDrawPosition.z);
			Vector3 point2 = new Vector3(this.startDrawPosition.x, num, drawPosition.z);
			this.BoxPositions[0] = this.startDrawPosition;
			this.BoxPositions[1] = Utilities.RotatePointAroundPivot(point, pivot, qangles);
			this.BoxPositions[2] = drawPosition;
			this.BoxPositions[3] = Utilities.RotatePointAroundPivot(point2, pivot, qangles);
			Vector3 vector;
			this.BoxPositions[1] = NetworkSingleton<ManagerPhysicsObject>.Instance.StaticSurfacePointBelowWorldPos(this.BoxPositions[1], out vector);
			this.BoxPositions[3] = NetworkSingleton<ManagerPhysicsObject>.Instance.StaticSurfacePointBelowWorldPos(this.BoxPositions[3], out vector);
			Vector3[] boxPositions = this.BoxPositions;
			int num2 = 1;
			boxPositions[num2].y = boxPositions[num2].y + this.GetYOffset();
			Vector3[] boxPositions2 = this.BoxPositions;
			int num3 = 3;
			boxPositions2[num3].y = boxPositions2[num3].y + this.GetYOffset();
			for (int i = 0; i < this.BoxPositions.Length; i++)
			{
				this.BoxPositions[i] = vectorDrawData.line.transform.InverseTransformPoint(this.BoxPositions[i]);
			}
			base.networkView.RPC<uint, Vector3[]>(RPCTarget.Others, new Action<uint, Vector3[]>(this.RPCSetPositionsLine), this.currentDrawingGuid, this.BoxPositions);
			this.RPCSetPositionsLine(this.currentDrawingGuid, this.BoxPositions);
		}
		if (this.VectorActionUp())
		{
			this.EndDrawing();
		}
	}

	// Token: 0x06001FDD RID: 8157 RVA: 0x000E3794 File Offset: 0x000E1994
	private void UpdateVectorCircle()
	{
		if (this.VectorActionDown())
		{
			this.StartDrawing(this.CirclePosition.Length, true, false, null);
		}
		ToolVector.VectorDrawData vectorDrawData;
		if (this.VectorAction() && this.drawing && this.CheckMove() && this.drawnLines.TryGetValue(this.currentDrawingGuid, out vectorDrawData))
		{
			Vector3 drawPosition = this.GetDrawPosition();
			float num = (drawPosition.y > this.startDrawPosition.y) ? drawPosition.y : this.startDrawPosition.y;
			num += 5f;
			Vector3 vector = new Vector3((this.startDrawPosition.x + drawPosition.x) / 2f, (this.startDrawPosition.y + drawPosition.y) / 2f, (this.startDrawPosition.z + drawPosition.z) / 2f);
			float num2 = Vector3.Distance(this.startDrawPosition, vector);
			float num3 = (float)(360 / this.CirclePosition.Length);
			for (int i = 0; i < this.CirclePosition.Length; i++)
			{
				float f = 0.017453292f * num3 * (float)i;
				float x = num2 * Mathf.Cos(f) + vector.x;
				float z = num2 * Mathf.Sin(f) + vector.z;
				this.CirclePosition[i] = new Vector3(x, num, z);
				Vector3 vector2;
				this.CirclePosition[i] = NetworkSingleton<ManagerPhysicsObject>.Instance.StaticSurfacePointBelowWorldPos(this.CirclePosition[i], out vector2);
				Vector3[] circlePosition = this.CirclePosition;
				int num4 = i;
				circlePosition[num4].y = circlePosition[num4].y + this.GetYOffset();
				this.CirclePosition[i] = vectorDrawData.line.transform.InverseTransformPoint(this.CirclePosition[i]);
			}
			base.networkView.RPC<uint, Vector3[]>(RPCTarget.Others, new Action<uint, Vector3[]>(this.RPCSetPositionsLine), this.currentDrawingGuid, this.CirclePosition);
			this.RPCSetPositionsLine(this.currentDrawingGuid, this.CirclePosition);
		}
		if (this.VectorActionUp())
		{
			this.EndDrawing();
		}
	}

	// Token: 0x06001FDE RID: 8158 RVA: 0x000E39BA File Offset: 0x000E1BBA
	private int AddVertex(Vector3 position, Color color)
	{
		this.vertices.Add(position);
		return this.vertices.Count - 1;
	}

	// Token: 0x06001FDF RID: 8159 RVA: 0x000E39D5 File Offset: 0x000E1BD5
	private bool AddTriangle(int A, int B, int C)
	{
		this.triangles.Add(A);
		this.triangles.Add(B);
		this.triangles.Add(C);
		return true;
	}

	// Token: 0x06001FE0 RID: 8160 RVA: 0x000E39FC File Offset: 0x000E1BFC
	private void DrawPixelPosition(Vector3 drawPos)
	{
		this.startDrawPosition = drawPos;
		Mesh sharedMesh = this.pixelObject.GetComponent<MeshFilter>().sharedMesh;
		sharedMesh.GetVertices(this.vertices);
		sharedMesh.GetTriangles(this.triangles, 0);
		this.DrawPixel(drawPos);
		sharedMesh.SetVertices(this.vertices);
		sharedMesh.SetTriangles(this.triangles, 0);
	}

	// Token: 0x06001FE1 RID: 8161 RVA: 0x000E3A58 File Offset: 0x000E1C58
	private void DrawPixelPositions(List<Vector3> drawPos)
	{
		this.startDrawPosition = drawPos[drawPos.Count - 1];
		new Stopwatch().Start();
		Mesh sharedMesh = this.pixelObject.GetComponent<MeshFilter>().sharedMesh;
		sharedMesh.GetVertices(this.vertices);
		sharedMesh.GetTriangles(this.triangles, 0);
		for (int i = 0; i < drawPos.Count; i++)
		{
			this.DrawPixel(drawPos[i]);
		}
		sharedMesh.SetVertices(this.vertices);
		sharedMesh.SetTriangles(this.triangles, 0);
	}

	// Token: 0x06001FE2 RID: 8162 RVA: 0x000E3AE4 File Offset: 0x000E1CE4
	private void DrawPixel(Vector3 drawPos)
	{
		Color drawColor = this.DrawColor;
		float num = this.DrawThickness / 2f;
		Vector3 vector = drawPos + new Vector3(num, 0f, num);
		Vector3 vector2 = drawPos + new Vector3(-num, 0f, num);
		Vector3 vector3 = drawPos + new Vector3(num, 0f, -num);
		Vector3 vector4 = drawPos + new Vector3(-num, 0f, -num);
		ToolVector.PixelData pixelData = new ToolVector.PixelData
		{
			position = drawPos,
			color = drawColor,
			thickness = this.DrawThickness
		};
		int i = 0;
		while (i < this.pixelDatas.Count)
		{
			ToolVector.PixelData pixelData2 = this.pixelDatas[i];
			if (pixelData2.position == drawPos)
			{
				if (pixelData2.Equals(pixelData))
				{
					return;
				}
				int num3;
				int num2 = num3 = this.vertices.IndexOfSequence(vector, vector2, vector3, vector4);
				int num4 = num2 + 1;
				int arg = num2 + 2;
				int index = num2 + 3;
				this.vertices.RemoveAt(num3);
				this.vertices.RemoveAt(num4);
				this.vertices.RemoveAt(num4);
				this.vertices.RemoveAt(index);
				int num5 = this.triangles.IndexOfSequence(num3, arg, num4);
				this.triangles.RemoveAt(num5);
				this.triangles.RemoveAt(num5 + 1);
				this.triangles.RemoveAt(num5 + 2);
				this.triangles.RemoveAt(num5 + 3);
				this.triangles.RemoveAt(num5 + 4);
				this.triangles.RemoveAt(num5 + 5);
				this.pixelDatas.Remove(pixelData2);
				break;
			}
			else
			{
				i++;
			}
		}
		this.pixelDatas.Add(pixelData);
		int a = this.AddVertex(vector, drawColor);
		int c = this.AddVertex(vector2, drawColor);
		int num6 = this.AddVertex(vector3, drawColor);
		int b = this.AddVertex(vector4, drawColor);
		this.AddTriangle(a, num6, c);
		this.AddTriangle(num6, b, c);
	}

	// Token: 0x06001FE3 RID: 8163 RVA: 0x000E3CFC File Offset: 0x000E1EFC
	private void UpdateVectorPixel()
	{
		if (this.VectorActionDown())
		{
			this.DrawPixelPosition(this.GetDrawPosition());
		}
		if (this.VectorAction())
		{
			Vector3 drawPosition = this.GetDrawPosition();
			Vector3 vector = this.startDrawPosition;
			if (vector.x != drawPosition.x || vector.z != drawPosition.z)
			{
				this.DrawPixelPosition(drawPosition);
				if (!this.PointsTouching(vector.x, vector.z, drawPosition.x, drawPosition.z))
				{
					Vector3 vector2 = drawPosition - vector;
					Vector3 normalized = vector2.normalized;
					int num = (int)(vector2.magnitude / this.DrawThickness) + 1;
					float d = vector2.magnitude / (float)num;
					List<Vector3> list = new List<Vector3>();
					for (int i = 0; i < num; i++)
					{
						Vector3 item = this.PosToGrid(vector + normalized * d * (float)(i + 1));
						if (!list.Contains(item))
						{
							list.Add(item);
						}
					}
					this.DrawPixelPositions(list);
				}
			}
		}
		if (this.VectorActionUp())
		{
			this.EndDrawing();
		}
	}

	// Token: 0x06001FE4 RID: 8164 RVA: 0x000E3E14 File Offset: 0x000E2014
	private bool PointsTouching(float startX, float startZ, float endX, float endZ)
	{
		float num = this.DrawThickness * Mathf.Sqrt(2f);
		return Mathf.Abs(endX - startX) <= num && Mathf.Abs(endZ - startZ) <= num;
	}

	// Token: 0x06001FE5 RID: 8165 RVA: 0x000E3E50 File Offset: 0x000E2050
	private void UpdateVectorErase()
	{
		if (this.VectorAction())
		{
			Vector3 drawPosition = this.GetDrawPosition();
			foreach (KeyValuePair<uint, ToolVector.VectorDrawData> keyValuePair in this.drawnLines)
			{
				LineRenderer line = keyValuePair.Value.line;
				float thickness = keyValuePair.Value.thickness;
				float num = thickness;
				Vector3 size = new Vector3(line.bounds.size.x + num, line.bounds.size.y + 1f, line.bounds.size.z + num);
				Bounds bounds = new Bounds(line.bounds.center, size);
				if (bounds.Contains(drawPosition))
				{
					for (int i = 0; i < line.positionCount; i++)
					{
						Vector3 vector = line.transform.TransformPoint(line.GetPosition(i));
						if (Vector3.Distance(vector, drawPosition) <= thickness)
						{
							base.networkView.RPC<uint>(RPCTarget.Others, new Action<uint>(this.RPCRemoveLine), keyValuePair.Key);
							this.RPCRemoveLine(keyValuePair.Key);
							return;
						}
						if (line.positionCount - 1 > i)
						{
							Vector3 vector2 = line.transform.TransformPoint(line.GetPosition(i + 1));
							float num2 = Vector3.Distance(vector, vector2);
							if (num2 > thickness)
							{
								Vector3 normalized = (vector2 - vector).normalized;
								int num3 = (int)(num2 / thickness);
								for (int j = 0; j < num3; j++)
								{
									if (Vector3.Distance(vector + normalized * thickness * (float)(j + 1), drawPosition) <= thickness)
									{
										base.networkView.RPC<uint>(RPCTarget.Others, new Action<uint>(this.RPCRemoveLine), keyValuePair.Key);
										this.RPCRemoveLine(keyValuePair.Key);
										return;
									}
								}
							}
						}
					}
				}
			}
		}
	}

	// Token: 0x06001FE6 RID: 8166 RVA: 0x000E4070 File Offset: 0x000E2270
	public uint GetGUID()
	{
		int i = 0;
		while (i < 100)
		{
			i++;
			uint num = Utilities.RandomUint();
			if (!this.drawnLines.ContainsKey(num))
			{
				return num;
			}
		}
		return 0U;
	}

	// Token: 0x06001FE7 RID: 8167 RVA: 0x000E40A4 File Offset: 0x000E22A4
	public Vector3 GetDrawPosition()
	{
		Vector3 vector = new Vector3(HoverScript.PointerPosition.x, HoverScript.PointerPosition.y + this.GetYOffset(), HoverScript.PointerPosition.z);
		if (this.pointerMode == PointerMode.VectorPixel)
		{
			vector = this.PosToGrid(vector);
		}
		return vector;
	}

	// Token: 0x06001FE8 RID: 8168 RVA: 0x000E40F0 File Offset: 0x000E22F0
	public Vector3 PosToGrid(Vector3 drawPos)
	{
		float num = 1f / this.DrawThickness;
		float num2 = num / 2f;
		drawPos = new Vector3(Mathf.Round((drawPos.x + num2) * num) / num - num2, drawPos.y, Mathf.Round((drawPos.z + num2) * num) / num - num2);
		return drawPos;
	}

	// Token: 0x06001FE9 RID: 8169 RVA: 0x000E4146 File Offset: 0x000E2346
	private float GetYOffset()
	{
		return 0.01f;
	}

	// Token: 0x06001FEA RID: 8170 RVA: 0x000E4150 File Offset: 0x000E2350
	public void SetRenderMode(bool fullyVisible)
	{
		if (fullyVisible)
		{
			ToolVector.RENDER_QUEUE = 3000;
		}
		else
		{
			ToolVector.RENDER_QUEUE = 2450;
		}
		foreach (KeyValuePair<uint, ToolVector.VectorDrawData> keyValuePair in this.drawnLines)
		{
			keyValuePair.Value.line.material.renderQueue = ToolVector.RENDER_QUEUE;
		}
	}

	// Token: 0x06001FEB RID: 8171 RVA: 0x000E41CC File Offset: 0x000E23CC
	private LineRenderer CreateLine(ToolVector.LineNetworkData networkData)
	{
		Transform parent = base.transform;
		NetworkPhysicsObject attached = networkData.attached;
		if (attached)
		{
			string text = "VectorDrawAnchor";
			Transform transform = attached.transform.Find(text);
			if (!transform)
			{
				transform = new GameObject(text).transform;
				transform.parent = attached.transform;
				transform.Reset();
			}
			parent = transform;
		}
		LineRenderer component = UnityEngine.Object.Instantiate<GameObject>(this.LinePrefab, parent).GetComponent<LineRenderer>();
		component.transform.localRotation = Quaternion.Euler(networkData.rotation) * Quaternion.Euler(new Vector3(90f, 0f, 0f));
		component.startColor = networkData.color.GammaToLinearSpace();
		component.endColor = networkData.color.GammaToLinearSpace();
		component.widthMultiplier = networkData.thickness;
		component.numCapVertices = (networkData.square ? 0 : this.GetNumCapVertices(networkData.thickness));
		component.sortingOrder = this.GetNextSortOrder();
		component.loop = networkData.loop;
		component.material.renderQueue = ToolVector.RENDER_QUEUE;
		ToolVector.VectorDrawData value = new ToolVector.VectorDrawData
		{
			color = networkData.color,
			thickness = networkData.thickness,
			line = component,
			attached = attached,
			rotation = networkData.rotation,
			loop = networkData.loop
		};
		if (attached)
		{
			attached.AddRenderer(component);
		}
		this.drawnLines.SetValue(networkData.guid, value);
		return component;
	}

	// Token: 0x06001FEC RID: 8172 RVA: 0x000E4354 File Offset: 0x000E2554
	private int GetNextSortOrder()
	{
		for (int i = this.drawnLines.Count - 1; i >= 0; i--)
		{
			ToolVector.VectorDrawData vectorDrawData = this.drawnLines.List[i];
			if (vectorDrawData.line)
			{
				return vectorDrawData.line.sortingOrder + 1;
			}
		}
		return 0;
	}

	// Token: 0x06001FED RID: 8173 RVA: 0x000E43A8 File Offset: 0x000E25A8
	private void DestroyLineInSameSpot(ToolVector.LineNetworkData networkData)
	{
		foreach (KeyValuePair<uint, ToolVector.VectorDrawData> keyValuePair in this.drawnLines)
		{
			ToolVector.VectorDrawData value = keyValuePair.Value;
			uint key = keyValuePair.Key;
			if (value.attached == networkData.attached && value.line.positionCount == networkData.positions.Length)
			{
				bool flag = true;
				for (int i = 0; i < networkData.positions.Length; i++)
				{
					if (value.line.GetPosition(i) != networkData.positions[i])
					{
						flag = false;
						break;
					}
				}
				if (flag)
				{
					this.RPCRemoveLine(key);
					break;
				}
			}
		}
	}

	// Token: 0x06001FEE RID: 8174 RVA: 0x000E447C File Offset: 0x000E267C
	[Remote(Permission.Client, SendType.ReliableBuffered, null, SerializationMethod.Default, sendType = SendType.ReliableNoDelay, validationFunction = "Permissions/Drawing")]
	private void RPCAddPixel(ToolVector.LineNetworkData networkData)
	{
		LineRenderer lineRenderer = this.CreateLine(networkData);
		for (int i = 0; i < networkData.positions.Length; i++)
		{
			networkData.positions[i] = lineRenderer.transform.InverseTransformPoint(networkData.positions[i]);
		}
		this.DestroyLineInSameSpot(networkData);
		lineRenderer.positionCount = networkData.positions.Length;
		lineRenderer.SetPositions(networkData.positions);
	}

	// Token: 0x06001FEF RID: 8175 RVA: 0x000E44E8 File Offset: 0x000E26E8
	[Remote(Permission.Client, SendType.ReliableBuffered, null, SerializationMethod.Default, sendType = SendType.ReliableNoDelay, validationFunction = "Permissions/Drawing")]
	private void RPCAddLine(ToolVector.LineNetworkData networkData)
	{
		LineRenderer lineRenderer = this.CreateLine(networkData);
		for (int i = 0; i < networkData.positions.Length; i++)
		{
			networkData.positions[i] = lineRenderer.transform.InverseTransformPoint(networkData.positions[i]);
		}
		lineRenderer.positionCount = networkData.positions.Length;
		lineRenderer.SetPositions(networkData.positions);
	}

	// Token: 0x06001FF0 RID: 8176 RVA: 0x000E4550 File Offset: 0x000E2750
	[Remote(Permission.Client, SendType.ReliableBuffered, null, SerializationMethod.Default, sendType = SendType.ReliableNoDelay, validationFunction = "Permissions/Drawing")]
	private void RPCAddLines(List<ToolVector.LineNetworkData> networkDatas)
	{
		foreach (ToolVector.LineNetworkData lineNetworkData in networkDatas)
		{
			LineRenderer lineRenderer = this.CreateLine(lineNetworkData);
			lineRenderer.positionCount = lineNetworkData.positions.Length;
			lineRenderer.SetPositions(lineNetworkData.positions);
		}
	}

	// Token: 0x06001FF1 RID: 8177 RVA: 0x000E45B8 File Offset: 0x000E27B8
	[Remote(Permission.Client, SendType.ReliableBuffered, null, SerializationMethod.Default, sendType = SendType.ReliableNoDelay, validationFunction = "Permissions/Drawing")]
	private void RPCAddPositionToLine(uint guid, Vector3 position)
	{
		ToolVector.VectorDrawData vectorDrawData;
		if (this.drawnLines.TryGetValue(guid, out vectorDrawData))
		{
			this.AddPoint(vectorDrawData.line, position);
		}
	}

	// Token: 0x06001FF2 RID: 8178 RVA: 0x000E45E4 File Offset: 0x000E27E4
	[Remote(Permission.Client, SendType.ReliableBuffered, null, SerializationMethod.Default, sendType = SendType.ReliableNoDelay, validationFunction = "Permissions/Drawing")]
	private void RPCUpdatePositionLine(uint guid, int index, Vector3 position)
	{
		ToolVector.VectorDrawData vectorDrawData;
		if (this.drawnLines.TryGetValue(guid, out vectorDrawData))
		{
			vectorDrawData.line.SetPosition(index, vectorDrawData.line.transform.InverseTransformPoint(position));
		}
	}

	// Token: 0x06001FF3 RID: 8179 RVA: 0x000E4620 File Offset: 0x000E2820
	[Remote(Permission.Client, SendType.ReliableBuffered, null, SerializationMethod.Default, sendType = SendType.ReliableNoDelay, validationFunction = "Permissions/Drawing")]
	private void RPCSetPositionsLine(uint guid, Vector3[] positions)
	{
		ToolVector.VectorDrawData vectorDrawData;
		if (this.drawnLines.TryGetValue(guid, out vectorDrawData))
		{
			vectorDrawData.line.SetPositions(positions);
		}
	}

	// Token: 0x06001FF4 RID: 8180 RVA: 0x000E4649 File Offset: 0x000E2849
	private void AddPoint(LineRenderer line, Vector3 position)
	{
		line.positionCount++;
		line.SetPosition(line.positionCount - 1, line.transform.InverseTransformPoint(position));
	}

	// Token: 0x06001FF5 RID: 8181 RVA: 0x000E4674 File Offset: 0x000E2874
	[Remote(Permission.Client, SendType.ReliableBuffered, null, SerializationMethod.Default, sendType = SendType.ReliableNoDelay, validationFunction = "Permissions/Drawing")]
	private void RPCRemoveLine(uint guid)
	{
		ToolVector.VectorDrawData vectorDrawData;
		if (this.drawnLines.TryGetValue(guid, out vectorDrawData) && vectorDrawData.line)
		{
			if (vectorDrawData.attached)
			{
				vectorDrawData.attached.TryRemoveRenderer(vectorDrawData.line);
			}
			UnityEngine.Object.Destroy(vectorDrawData.line.gameObject);
			this.drawnLines.Remove(guid);
			if (this.currentDrawingGuid == guid)
			{
				this.EndDrawing();
			}
		}
	}

	// Token: 0x06001FF6 RID: 8182 RVA: 0x000E46E7 File Offset: 0x000E28E7
	public void GUIConfirmRemoveAll()
	{
		UIDialog.Show("Erase all drawn lines?", "Yes", "No", delegate()
		{
			base.networkView.RPC<bool>(RPCTarget.All, new Action<bool>(this.RPCRemoveAll), true);
		}, null);
	}

	// Token: 0x06001FF7 RID: 8183 RVA: 0x000E470A File Offset: 0x000E290A
	public void RemoveAll()
	{
		base.networkView.RPC<bool>(RPCTarget.All, new Action<bool>(this.RPCRemoveAll), false);
	}

	// Token: 0x06001FF8 RID: 8184 RVA: 0x000E4728 File Offset: 0x000E2928
	[Remote(Permission.Client, SendType.ReliableBuffered, null, SerializationMethod.Default, sendType = SendType.ReliableNoDelay, validationFunction = "Permissions/Drawing")]
	private void RPCRemoveAll(bool notify)
	{
		if (notify)
		{
			Chat.NotifyFromNetworkSender("has erased all vector lines");
		}
		foreach (KeyValuePair<uint, ToolVector.VectorDrawData> keyValuePair in this.drawnLines)
		{
			ToolVector.VectorDrawData value = keyValuePair.Value;
			if (value.line)
			{
				if (value.attached)
				{
					value.attached.TryRemoveRenderer(value.line);
				}
				UnityEngine.Object.Destroy(value.line.gameObject);
			}
		}
		this.drawnLines.Clear();
		this.EndDrawing();
	}

	// Token: 0x06001FF9 RID: 8185 RVA: 0x000E47D0 File Offset: 0x000E29D0
	public List<VectorLineState> GetLineStates(NetworkPhysicsObject attachedNPO = null)
	{
		List<VectorLineState> list = null;
		for (int i = 0; i < this.drawnLines.Count; i++)
		{
			ToolVector.VectorDrawData vectorDrawData = this.drawnLines.List[i];
			if (vectorDrawData.attached == attachedNPO && vectorDrawData.line.positionCount > 1)
			{
				if (list == null)
				{
					list = new List<VectorLineState>();
				}
				Vector3[] array = new Vector3[vectorDrawData.line.positionCount];
				vectorDrawData.line.GetPositions(array);
				List<VectorState> list2 = new List<VectorState>(array.Length);
				foreach (Vector3 vector in array)
				{
					list2.Add(new VectorState(new Vector3(vector.x, -vector.z, vector.y)));
				}
				VectorState? rotation = null;
				if (vectorDrawData.rotation != Vector3.zero)
				{
					rotation = new VectorState?(new VectorState(vectorDrawData.rotation));
				}
				bool? loop = vectorDrawData.loop ? new bool?(true) : null;
				bool? square = vectorDrawData.square ? new bool?(true) : null;
				list.Add(new VectorLineState
				{
					color = new ColourState(vectorDrawData.color),
					thickness = vectorDrawData.thickness,
					points3 = list2,
					rotation = rotation,
					loop = loop,
					square = square
				});
			}
		}
		return list;
	}

	// Token: 0x06001FFA RID: 8186 RVA: 0x000E4958 File Offset: 0x000E2B58
	public void AddLineStates(List<VectorLineState> state, NetworkPhysicsObject attachedNPO = null)
	{
		for (int i = state.Count - 1; i > -1; i--)
		{
			if (state[i].points3.Count < 2)
			{
				state.RemoveAt(i);
			}
		}
		List<ToolVector.LineNetworkData> list = new List<ToolVector.LineNetworkData>();
		foreach (VectorLineState vectorLineState in state)
		{
			Vector3[] array = new Vector3[vectorLineState.points3.Count];
			for (int j = 0; j < vectorLineState.points3.Count; j++)
			{
				Vector3 vector = vectorLineState.points3[j].ToVector();
				array[j] = new Vector3(vector.x, vector.z, -vector.y);
			}
			List<ToolVector.LineNetworkData> list2 = list;
			uint guid = this.GetGUID();
			Vector3[] positions = array;
			Color color = vectorLineState.color.ToColour();
			float thickness = vectorLineState.thickness;
			VectorLineState vectorLineState2 = vectorLineState;
			list2.Add(new ToolVector.LineNetworkData(guid, attachedNPO, positions, color, thickness, (vectorLineState2.rotation != null) ? vectorLineState2.rotation.GetValueOrDefault().ToVector() : Vector3.zero, vectorLineState.loop ?? false, vectorLineState.square ?? false));
		}
		if (attachedNPO && attachedNPO.networkView.disableNetworking)
		{
			this.RPCAddLines(list);
			return;
		}
		base.networkView.RPC<List<ToolVector.LineNetworkData>>(RPCTarget.All, new Action<List<ToolVector.LineNetworkData>>(this.RPCAddLines), list);
	}

	// Token: 0x06001FFB RID: 8187 RVA: 0x000E4B08 File Offset: 0x000E2D08
	public void RemoveLinesAttached(NetworkPhysicsObject attachedNPO = null)
	{
		base.networkView.RPC<NetworkPhysicsObject>(RPCTarget.All, new Action<NetworkPhysicsObject>(this.RPCRemoveLinesAttached), attachedNPO);
	}

	// Token: 0x06001FFC RID: 8188 RVA: 0x000E4B24 File Offset: 0x000E2D24
	[Remote(Permission.Client, SendType.ReliableBuffered, null, SerializationMethod.Default, sendType = SendType.ReliableNoDelay, validationFunction = "Permissions/Drawing")]
	private void RPCRemoveLinesAttached(NetworkPhysicsObject attachedNPO)
	{
		List<uint> list = new List<uint>();
		foreach (KeyValuePair<uint, ToolVector.VectorDrawData> keyValuePair in this.drawnLines)
		{
			if (keyValuePair.Value.attached == attachedNPO)
			{
				list.Add(keyValuePair.Key);
			}
		}
		foreach (uint guid in list)
		{
			this.RPCRemoveLine(guid);
		}
	}

	// Token: 0x0400137A RID: 4986
	public const int DEFAULT_RENDER_QUEUE = 2450;

	// Token: 0x0400137B RID: 4987
	public const int FULLY_VISIBLE_RENDER_QUEUE = 3000;

	// Token: 0x0400137C RID: 4988
	public static int RENDER_QUEUE = 2450;

	// Token: 0x0400137D RID: 4989
	[SerializeField]
	private GameObject LinePrefab;

	// Token: 0x0400137E RID: 4990
	[SerializeField]
	private UIColorPickerInput ColorPicker;

	// Token: 0x0400137F RID: 4991
	private const int Pixel_Dead_Zone = 5;

	// Token: 0x04001380 RID: 4992
	private Color _drawColor = Color.white;

	// Token: 0x04001381 RID: 4993
	private float _drawThickess = 0.1f;

	// Token: 0x04001382 RID: 4994
	private bool drawing;

	// Token: 0x04001383 RID: 4995
	private bool checkDrawing;

	// Token: 0x04001384 RID: 4996
	private uint currentDrawingGuid;

	// Token: 0x04001385 RID: 4997
	private Vector2 lastPixelDraw = Vector2.zero;

	// Token: 0x04001386 RID: 4998
	private Vector3 startDrawPosition = Vector3.zero;

	// Token: 0x04001387 RID: 4999
	private Listionary<uint, ToolVector.VectorDrawData> drawnLines = new Listionary<uint, ToolVector.VectorDrawData>();

	// Token: 0x04001388 RID: 5000
	private LineRenderer drawIndicator;

	// Token: 0x04001389 RID: 5001
	private float recursiveScaleHolder;

	// Token: 0x0400138A RID: 5002
	private PointerMode pointerMode = PointerMode.None;

	// Token: 0x0400138B RID: 5003
	private GameObject pixelObject;

	// Token: 0x0400138C RID: 5004
	private Vector3[] BoxPositions = new Vector3[4];

	// Token: 0x0400138D RID: 5005
	private Vector3[] CirclePosition = new Vector3[36];

	// Token: 0x0400138E RID: 5006
	private List<ToolVector.PixelData> pixelDatas = new List<ToolVector.PixelData>();

	// Token: 0x0400138F RID: 5007
	private List<Vector3> vertices = new List<Vector3>();

	// Token: 0x04001390 RID: 5008
	private List<int> triangles = new List<int>();

	// Token: 0x020006FD RID: 1789
	public class VectorDrawData
	{
		// Token: 0x06003D58 RID: 15704 RVA: 0x00002594 File Offset: 0x00000794
		public VectorDrawData()
		{
		}

		// Token: 0x06003D59 RID: 15705 RVA: 0x0017B876 File Offset: 0x00179A76
		public VectorDrawData(Color color, float thickness, LineRenderer line, NetworkPhysicsObject attached)
		{
			this.color = color;
			this.thickness = thickness;
			this.line = line;
			this.attached = attached;
		}

		// Token: 0x04002A46 RID: 10822
		public Color color;

		// Token: 0x04002A47 RID: 10823
		public float thickness;

		// Token: 0x04002A48 RID: 10824
		public Vector3 rotation;

		// Token: 0x04002A49 RID: 10825
		public bool loop;

		// Token: 0x04002A4A RID: 10826
		public bool square;

		// Token: 0x04002A4B RID: 10827
		public LineRenderer line;

		// Token: 0x04002A4C RID: 10828
		public NetworkPhysicsObject attached;
	}

	// Token: 0x020006FE RID: 1790
	public class LineNetworkData
	{
		// Token: 0x06003D5A RID: 15706 RVA: 0x00002594 File Offset: 0x00000794
		public LineNetworkData()
		{
		}

		// Token: 0x06003D5B RID: 15707 RVA: 0x0017B89C File Offset: 0x00179A9C
		public LineNetworkData(uint guid, NetworkPhysicsObject attached, Vector3[] positions, Color color, float thickness, Vector3 rotation, bool loop, bool square)
		{
			this.guid = guid;
			this.attached = attached;
			this.positions = positions;
			this.color = color;
			this.thickness = thickness;
			this.rotation = rotation;
			this.loop = loop;
			this.square = square;
		}

		// Token: 0x04002A4D RID: 10829
		public uint guid;

		// Token: 0x04002A4E RID: 10830
		public NetworkPhysicsObject attached;

		// Token: 0x04002A4F RID: 10831
		public Vector3[] positions;

		// Token: 0x04002A50 RID: 10832
		public Color color;

		// Token: 0x04002A51 RID: 10833
		public float thickness;

		// Token: 0x04002A52 RID: 10834
		public Vector3 rotation;

		// Token: 0x04002A53 RID: 10835
		public bool loop;

		// Token: 0x04002A54 RID: 10836
		public bool square;
	}

	// Token: 0x020006FF RID: 1791
	public struct PixelData
	{
		// Token: 0x04002A55 RID: 10837
		public Vector3 position;

		// Token: 0x04002A56 RID: 10838
		public Color color;

		// Token: 0x04002A57 RID: 10839
		public float thickness;
	}

	// Token: 0x02000700 RID: 1792
	public struct GridData
	{
		// Token: 0x04002A58 RID: 10840
		public int x;

		// Token: 0x04002A59 RID: 10841
		public int y;
	}
}
