using System;
using System.Collections.Generic;
using UnityEngine;

namespace RTEditor
{
	// Token: 0x0200044F RID: 1103
	public class SphereTreeNode<T>
	{
		// Token: 0x170006CF RID: 1743
		// (get) Token: 0x06003275 RID: 12917 RVA: 0x00153C00 File Offset: 0x00151E00
		public SphereTree<T> Tree
		{
			get
			{
				return this._tree;
			}
		}

		// Token: 0x170006D0 RID: 1744
		// (get) Token: 0x06003276 RID: 12918 RVA: 0x00153C08 File Offset: 0x00151E08
		// (set) Token: 0x06003277 RID: 12919 RVA: 0x00153C10 File Offset: 0x00151E10
		public T Data
		{
			get
			{
				return this._data;
			}
			set
			{
				this._data = value;
			}
		}

		// Token: 0x170006D1 RID: 1745
		// (get) Token: 0x06003278 RID: 12920 RVA: 0x00153C19 File Offset: 0x00151E19
		public SphereTreeNode<T> Parent
		{
			get
			{
				return this._parent;
			}
		}

		// Token: 0x170006D2 RID: 1746
		// (get) Token: 0x06003279 RID: 12921 RVA: 0x00153C21 File Offset: 0x00151E21
		public List<SphereTreeNode<T>> Children
		{
			get
			{
				return this._children;
			}
		}

		// Token: 0x170006D3 RID: 1747
		// (get) Token: 0x0600327A RID: 12922 RVA: 0x00153C29 File Offset: 0x00151E29
		public int NumberOfChildren
		{
			get
			{
				return this._children.Count;
			}
		}

		// Token: 0x170006D4 RID: 1748
		// (get) Token: 0x0600327B RID: 12923 RVA: 0x00153C36 File Offset: 0x00151E36
		// (set) Token: 0x0600327C RID: 12924 RVA: 0x00153C3E File Offset: 0x00151E3E
		public Sphere3D Sphere
		{
			get
			{
				return this._sphere;
			}
			set
			{
				this._sphere = value;
			}
		}

		// Token: 0x170006D5 RID: 1749
		// (get) Token: 0x0600327D RID: 12925 RVA: 0x00153C47 File Offset: 0x00151E47
		// (set) Token: 0x0600327E RID: 12926 RVA: 0x00153C54 File Offset: 0x00151E54
		public Vector3 Center
		{
			get
			{
				return this._sphere.Center;
			}
			set
			{
				this._sphere.Center = value;
			}
		}

		// Token: 0x170006D6 RID: 1750
		// (get) Token: 0x0600327F RID: 12927 RVA: 0x00153C62 File Offset: 0x00151E62
		// (set) Token: 0x06003280 RID: 12928 RVA: 0x00153C6F File Offset: 0x00151E6F
		public float Radius
		{
			get
			{
				return this._sphere.Radius;
			}
			set
			{
				this._sphere.Radius = value;
			}
		}

		// Token: 0x170006D7 RID: 1751
		// (get) Token: 0x06003281 RID: 12929 RVA: 0x00153C7D File Offset: 0x00151E7D
		public SphereTreeNodeFlags Flags
		{
			get
			{
				return this._flags;
			}
		}

		// Token: 0x170006D8 RID: 1752
		// (get) Token: 0x06003282 RID: 12930 RVA: 0x00153C85 File Offset: 0x00151E85
		public bool IsTerminal
		{
			get
			{
				return (this._flags & SphereTreeNodeFlags.Terminal) > SphereTreeNodeFlags.None;
			}
		}

		// Token: 0x170006D9 RID: 1753
		// (get) Token: 0x06003283 RID: 12931 RVA: 0x00153C92 File Offset: 0x00151E92
		public bool IsRoot
		{
			get
			{
				return (this._flags & SphereTreeNodeFlags.Root) > SphereTreeNodeFlags.None;
			}
		}

		// Token: 0x170006DA RID: 1754
		// (get) Token: 0x06003284 RID: 12932 RVA: 0x00153C9F File Offset: 0x00151E9F
		public bool IsSuperSphere
		{
			get
			{
				return (this._flags & SphereTreeNodeFlags.SuperSphere) > SphereTreeNodeFlags.None;
			}
		}

		// Token: 0x170006DB RID: 1755
		// (get) Token: 0x06003285 RID: 12933 RVA: 0x00153CAC File Offset: 0x00151EAC
		public bool MustRecompute
		{
			get
			{
				return (this._flags & SphereTreeNodeFlags.MustRecompute) > SphereTreeNodeFlags.None;
			}
		}

		// Token: 0x170006DC RID: 1756
		// (get) Token: 0x06003286 RID: 12934 RVA: 0x00153CB9 File Offset: 0x00151EB9
		public bool MustIntegrate
		{
			get
			{
				return (this._flags & SphereTreeNodeFlags.MustIntegrate) > SphereTreeNodeFlags.None;
			}
		}

		// Token: 0x170006DD RID: 1757
		// (get) Token: 0x06003287 RID: 12935 RVA: 0x00153CC7 File Offset: 0x00151EC7
		public bool HasParent
		{
			get
			{
				return this._parent != null;
			}
		}

		// Token: 0x170006DE RID: 1758
		// (get) Token: 0x06003288 RID: 12936 RVA: 0x00153CD2 File Offset: 0x00151ED2
		public bool HasNoChildren
		{
			get
			{
				return this.NumberOfChildren == 0;
			}
		}

		// Token: 0x170006DF RID: 1759
		// (get) Token: 0x06003289 RID: 12937 RVA: 0x00153CDD File Offset: 0x00151EDD
		public bool HasChildren
		{
			get
			{
				return this.NumberOfChildren != 0;
			}
		}

		// Token: 0x0600328A RID: 12938 RVA: 0x00153CE8 File Offset: 0x00151EE8
		public SphereTreeNode(Vector3 center, float radius, SphereTree<T> tree, T data = default(T))
		{
			this._tree = tree;
			this._data = data;
			this._sphere.Center = center;
			this._sphere.Radius = radius;
		}

		// Token: 0x0600328B RID: 12939 RVA: 0x00153D22 File Offset: 0x00151F22
		public SphereTreeNode(Sphere3D sphere, SphereTree<T> tree, T data = default(T))
		{
			this._tree = tree;
			this._data = data;
			this._sphere = sphere;
		}

		// Token: 0x0600328C RID: 12940 RVA: 0x00153D4A File Offset: 0x00151F4A
		public void SetFlag(SphereTreeNodeFlags flag)
		{
			this._flags |= flag;
		}

		// Token: 0x0600328D RID: 12941 RVA: 0x00153D5A File Offset: 0x00151F5A
		public void ClearFlag(SphereTreeNodeFlags flag)
		{
			this._flags &= ~flag;
		}

		// Token: 0x0600328E RID: 12942 RVA: 0x00153D6B File Offset: 0x00151F6B
		public bool ContainsChild(SphereTreeNode<T> child)
		{
			return this._children.Contains(child);
		}

		// Token: 0x0600328F RID: 12943 RVA: 0x00153D79 File Offset: 0x00151F79
		public bool FullyContains(SphereTreeNode<T> node)
		{
			return this._sphere.FullyOverlaps(node.Sphere);
		}

		// Token: 0x06003290 RID: 12944 RVA: 0x00153D8C File Offset: 0x00151F8C
		public void Encapsulate(SphereTreeNode<T> node)
		{
			this._sphere = this._sphere.Encapsulate(node.Sphere);
		}

		// Token: 0x06003291 RID: 12945 RVA: 0x00153DA5 File Offset: 0x00151FA5
		public float GetDistanceBetweenCentersSq(SphereTreeNode<T> node)
		{
			return this._sphere.GetDistanceBetweenCentersSq(node.Sphere);
		}

		// Token: 0x06003292 RID: 12946 RVA: 0x00153DB8 File Offset: 0x00151FB8
		public void AddChild(SphereTreeNode<T> child)
		{
			if (this.ContainsChild(child))
			{
				return;
			}
			if (child.HasParent)
			{
				child.Parent.RemoveChild(child);
			}
			this._children.Add(child);
			child._parent = this;
		}

		// Token: 0x06003293 RID: 12947 RVA: 0x00153DEC File Offset: 0x00151FEC
		public void RemoveChild(SphereTreeNode<T> child)
		{
			int num = this._children.FindIndex((SphereTreeNode<T> item) => item == child);
			if (num >= 0)
			{
				this._children.RemoveAt(num);
				child._parent = null;
			}
		}

		// Token: 0x06003294 RID: 12948 RVA: 0x00153E3C File Offset: 0x0015203C
		public float GetDistanceToNodeExitPoint(SphereTreeNode<T> node)
		{
			return (this.Center - node.Center).magnitude + node.Radius;
		}

		// Token: 0x06003295 RID: 12949 RVA: 0x00153E6C File Offset: 0x0015206C
		public void RecomputeCenterAndRadius()
		{
			if (this.NumberOfChildren != 0)
			{
				Vector3 a = Vector3.zero;
				foreach (SphereTreeNode<T> sphereTreeNode in this._children)
				{
					a += sphereTreeNode.Center;
				}
				this.Center = a / (float)this.NumberOfChildren;
				float num = float.MinValue;
				foreach (SphereTreeNode<T> node in this._children)
				{
					float distanceToNodeExitPoint = this.GetDistanceToNodeExitPoint(node);
					if (distanceToNodeExitPoint > num)
					{
						num = distanceToNodeExitPoint;
					}
				}
				this.Radius = num;
				if (this.Parent != null && !this.Parent.MustRecompute)
				{
					this.Parent.RecomputeCenterAndRadius();
				}
				this.ClearFlag(SphereTreeNodeFlags.MustRecompute);
			}
		}

		// Token: 0x06003296 RID: 12950 RVA: 0x00153F6C File Offset: 0x0015216C
		public void EncapsulateChildNode(SphereTreeNode<T> node)
		{
			SphereTreeNode<T> sphereTreeNode = this;
			SphereTreeNode<T> node2 = node;
			while (sphereTreeNode != null && !sphereTreeNode.FullyContains(node2))
			{
				sphereTreeNode.Encapsulate(node2);
				node2 = sphereTreeNode;
				sphereTreeNode = sphereTreeNode.Parent;
			}
		}

		// Token: 0x0400206F RID: 8303
		private T _data;

		// Token: 0x04002070 RID: 8304
		private SphereTree<T> _tree;

		// Token: 0x04002071 RID: 8305
		private SphereTreeNode<T> _parent;

		// Token: 0x04002072 RID: 8306
		private List<SphereTreeNode<T>> _children = new List<SphereTreeNode<!0>>();

		// Token: 0x04002073 RID: 8307
		private Sphere3D _sphere;

		// Token: 0x04002074 RID: 8308
		private SphereTreeNodeFlags _flags;
	}
}
