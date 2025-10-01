using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020000F0 RID: 240
public class DebugManager : Singleton<DebugManager>
{
	// Token: 0x06000BF3 RID: 3059 RVA: 0x0003A21D File Offset: 0x0003841D
	private void Start()
	{
		UnityEngine.Object.Destroy(this);
	}

	// Token: 0x06000BF4 RID: 3060 RVA: 0x000519FC File Offset: 0x0004FBFC
	public void LateUpdate()
	{
		this.debugShapeHolder.DestroyChildren();
		for (int i = 0; i < this.debugShapes.Count; i++)
		{
			DebugShape debugShape = this.debugShapes[i];
			DebugShapeType type = debugShape.type;
			GameObject gameObject;
			if (type != DebugShapeType.Line)
			{
				if (type != DebugShapeType.Sphere)
				{
				}
				gameObject = GameObject.CreatePrimitive(PrimitiveType.Sphere);
				gameObject.GetComponent<Collider>().enabled = false;
				Renderer component = gameObject.GetComponent<Renderer>();
				component.material = new Material(Shader.Find("Hidden/Internal-Colored"));
				component.material.color = debugShape.color;
				Transform transform = gameObject.transform;
				transform.position = debugShape.points[0];
				transform.localScale = new Vector3(debugShape.size, debugShape.size, debugShape.size);
			}
			else
			{
				gameObject = new GameObject("Debug Line");
				LineRenderer lineRenderer = gameObject.AddComponent<LineRenderer>();
				lineRenderer.material = new Material(Shader.Find("Hidden/Internal-Colored"));
				lineRenderer.startColor = debugShape.color;
				lineRenderer.endColor = debugShape.color;
				lineRenderer.startWidth = debugShape.size;
				lineRenderer.endWidth = debugShape.size;
				lineRenderer.positionCount = 2;
				for (int j = 0; j < debugShape.points.Length; j++)
				{
					lineRenderer.SetPosition(j, debugShape.points[j]);
				}
			}
			gameObject.transform.parent = this.debugShapeHolder;
		}
		this.debugShapes.Clear();
	}

	// Token: 0x06000BF5 RID: 3061 RVA: 0x00051B78 File Offset: 0x0004FD78
	public void Line(Vector3[] points, Color color, float size = 0.2f)
	{
		this.debugShapes.Add(new DebugShape
		{
			type = DebugShapeType.Line,
			points = points,
			color = color,
			size = size
		});
	}

	// Token: 0x06000BF6 RID: 3062 RVA: 0x00051BBC File Offset: 0x0004FDBC
	public void Sphere(Vector3 position, Color color, float size = 1f)
	{
		this.debugShapes.Add(new DebugShape
		{
			type = DebugShapeType.Sphere,
			points = new Vector3[]
			{
				position
			},
			color = color,
			size = size
		});
	}

	// Token: 0x04000834 RID: 2100
	public GameObject DebugMarker;

	// Token: 0x04000835 RID: 2101
	private Transform debugShapeHolder;

	// Token: 0x04000836 RID: 2102
	private List<DebugShape> debugShapes = new List<DebugShape>();
}
