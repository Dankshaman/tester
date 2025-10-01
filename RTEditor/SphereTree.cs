using System;
using System.Collections.Generic;
using UnityEngine;

namespace RTEditor
{
	// Token: 0x0200044E RID: 1102
	public class SphereTree<T>
	{
		// Token: 0x0600325D RID: 12893 RVA: 0x00153596 File Offset: 0x00151796
		public SphereTree(int numberOfChildNodesPerNode)
		{
			this._numberOfChildNodesPerNode = Mathf.Max(2, numberOfChildNodesPerNode);
			this.CreateRootNode();
		}

		// Token: 0x0600325E RID: 12894 RVA: 0x001535C8 File Offset: 0x001517C8
		public List<SphereTreeNodeRayHit<T>> RaycastAll(Ray ray)
		{
			List<SphereTreeNodeRayHit<T>> list = new List<SphereTreeNodeRayHit<!0>>(20);
			this.RaycastAllRecurse(ray, this._rootNode, list);
			return list;
		}

		// Token: 0x0600325F RID: 12895 RVA: 0x001535EC File Offset: 0x001517EC
		public List<SphereTreeNode<T>> OverlapSphere(Sphere3D sphere)
		{
			List<SphereTreeNode<T>> list = new List<SphereTreeNode<!0>>(20);
			this.OverlapSphereRecurse(sphere, this._rootNode, list);
			return list;
		}

		// Token: 0x06003260 RID: 12896 RVA: 0x00153610 File Offset: 0x00151810
		public List<SphereTreeNode<T>> OverlapBox(OrientedBox box)
		{
			List<SphereTreeNode<T>> list = new List<SphereTreeNode<!0>>(20);
			this.OverlapBoxRecurse(box, this._rootNode, list);
			return list;
		}

		// Token: 0x06003261 RID: 12897 RVA: 0x00153634 File Offset: 0x00151834
		public List<SphereTreeNode<T>> OverlapBox(Box box)
		{
			List<SphereTreeNode<T>> list = new List<SphereTreeNode<!0>>(20);
			this.OverlapBoxRecurse(box.ToOrientedBox(), this._rootNode, list);
			return list;
		}

		// Token: 0x06003262 RID: 12898 RVA: 0x00153660 File Offset: 0x00151860
		public void PerformPendingUpdates()
		{
			while (this._nodesPendingRecomputation.Count != 0)
			{
				SphereTreeNode<T> sphereTreeNode = this._nodesPendingRecomputation.Dequeue();
				if (sphereTreeNode.HasChildren)
				{
					sphereTreeNode.RecomputeCenterAndRadius();
				}
				else
				{
					this.RemoveNode(sphereTreeNode);
				}
			}
			while (this._terminalNodesPendingIntegration.Count != 0)
			{
				SphereTreeNode<T> nodeToIntegrate = this._terminalNodesPendingIntegration.Dequeue();
				this.IntegrateTerminalNode(nodeToIntegrate);
			}
		}

		// Token: 0x06003263 RID: 12899 RVA: 0x001536C4 File Offset: 0x001518C4
		public SphereTreeNode<T> AddTerminalNode(Sphere3D sphere, T data)
		{
			SphereTreeNode<T> sphereTreeNode = new SphereTreeNode<!0>(sphere, this, data);
			sphereTreeNode.SetFlag(SphereTreeNodeFlags.Terminal);
			this.AddNodeToIntegrationQueue(sphereTreeNode);
			return sphereTreeNode;
		}

		// Token: 0x06003264 RID: 12900 RVA: 0x001536EC File Offset: 0x001518EC
		public void RemoveNode(SphereTreeNode<T> node)
		{
			if (!node.IsRoot && node.Parent != null)
			{
				SphereTreeNode<T> sphereTreeNode = node.Parent;
				sphereTreeNode.RemoveChild(node);
				while (sphereTreeNode.Parent != null && sphereTreeNode.HasNoChildren && !sphereTreeNode.IsRoot)
				{
					SphereTreeNode<!0> parent = sphereTreeNode.Parent;
					parent.RemoveChild(sphereTreeNode);
					sphereTreeNode = parent;
				}
				this.AddNodeToRecomputationQueue(sphereTreeNode);
			}
		}

		// Token: 0x06003265 RID: 12901 RVA: 0x00153746 File Offset: 0x00151946
		public void UpdateTerminalNodeCenter(SphereTreeNode<T> terminalNode, Vector3 newCenter)
		{
			terminalNode.Center = newCenter;
			this.OnTerminalNodeSphereUpdated(terminalNode);
		}

		// Token: 0x06003266 RID: 12902 RVA: 0x00153756 File Offset: 0x00151956
		public void UpdateTerminalNodeRadius(SphereTreeNode<T> terminalNode, float newRadius)
		{
			terminalNode.Radius = newRadius;
			this.OnTerminalNodeSphereUpdated(terminalNode);
		}

		// Token: 0x06003267 RID: 12903 RVA: 0x00153766 File Offset: 0x00151966
		public void UpdateTerminalNodeCenterAndRadius(SphereTreeNode<T> terminalNode, Vector3 newCenter, float newRadius)
		{
			terminalNode.Center = newCenter;
			terminalNode.Radius = newRadius;
			this.OnTerminalNodeSphereUpdated(terminalNode);
		}

		// Token: 0x06003268 RID: 12904 RVA: 0x00153780 File Offset: 0x00151980
		public Dictionary<T, SphereTreeNode<T>> GetDataToTerminalNodeDictionary()
		{
			Dictionary<T, SphereTreeNode<T>> dictionary = new Dictionary<!0, SphereTreeNode<!0>>();
			this.GetDataToTerminalNodeDictionaryRecurse(this._rootNode, dictionary);
			return dictionary;
		}

		// Token: 0x06003269 RID: 12905 RVA: 0x001537A4 File Offset: 0x001519A4
		private void CreateRootNode()
		{
			this._rootNode = new SphereTreeNode<!0>(Vector3.zero, 10f, this, default(!0));
			this._rootNode.SetFlag(SphereTreeNodeFlags.Root | SphereTreeNodeFlags.SuperSphere);
		}

		// Token: 0x0600326A RID: 12906 RVA: 0x001537DC File Offset: 0x001519DC
		private void IntegrateTerminalNode(SphereTreeNode<T> nodeToIntegrate)
		{
			this.IntegrateTerminalNodeRecurse(nodeToIntegrate, this._rootNode);
			nodeToIntegrate.ClearFlag(SphereTreeNodeFlags.MustIntegrate);
		}

		// Token: 0x0600326B RID: 12907 RVA: 0x001537F4 File Offset: 0x001519F4
		private void IntegrateTerminalNodeRecurse(SphereTreeNode<T> nodeToIntegrate, SphereTreeNode<T> parentNode)
		{
			if (parentNode.NumberOfChildren < this._numberOfChildNodesPerNode)
			{
				parentNode.AddChild(nodeToIntegrate);
				parentNode.RecomputeCenterAndRadius();
				return;
			}
			List<SphereTreeNode<T>> children = parentNode.Children;
			SphereTreeNode<T> sphereTreeNode = this.FindClosestNode(children, nodeToIntegrate);
			if (sphereTreeNode == null)
			{
				return;
			}
			if (!sphereTreeNode.IsTerminal)
			{
				this.IntegrateTerminalNodeRecurse(nodeToIntegrate, sphereTreeNode);
				return;
			}
			SphereTreeNode<T> sphereTreeNode2 = new SphereTreeNode<!0>(sphereTreeNode.Sphere, this, default(!0));
			sphereTreeNode2.SetFlag(SphereTreeNodeFlags.SuperSphere);
			parentNode.RemoveChild(sphereTreeNode);
			parentNode.AddChild(sphereTreeNode2);
			sphereTreeNode2.AddChild(nodeToIntegrate);
			sphereTreeNode2.AddChild(sphereTreeNode);
			sphereTreeNode2.RecomputeCenterAndRadius();
			parentNode.RecomputeCenterAndRadius();
		}

		// Token: 0x0600326C RID: 12908 RVA: 0x00153888 File Offset: 0x00151A88
		private SphereTreeNode<T> FindClosestNode(List<SphereTreeNode<T>> nodes, SphereTreeNode<T> node)
		{
			float num = float.MaxValue;
			SphereTreeNode<T> result = null;
			foreach (SphereTreeNode<T> sphereTreeNode in nodes)
			{
				float distanceBetweenCentersSq = sphereTreeNode.GetDistanceBetweenCentersSq(node);
				if (distanceBetweenCentersSq < num)
				{
					num = distanceBetweenCentersSq;
					result = sphereTreeNode;
					if (num == 0f)
					{
						return result;
					}
				}
			}
			return result;
		}

		// Token: 0x0600326D RID: 12909 RVA: 0x001538FC File Offset: 0x00151AFC
		private void AddNodeToRecomputationQueue(SphereTreeNode<T> node)
		{
			if (node.IsTerminal || node.IsRoot || node.MustRecompute)
			{
				return;
			}
			if (node.IsSuperSphere)
			{
				node.SetFlag(SphereTreeNodeFlags.MustRecompute);
				this._nodesPendingRecomputation.Enqueue(node);
			}
		}

		// Token: 0x0600326E RID: 12910 RVA: 0x00153932 File Offset: 0x00151B32
		private void AddNodeToIntegrationQueue(SphereTreeNode<T> node)
		{
			if (node.IsSuperSphere || node.IsRoot || node.MustIntegrate)
			{
				return;
			}
			if (node.IsTerminal)
			{
				node.SetFlag(SphereTreeNodeFlags.MustIntegrate);
				this._terminalNodesPendingIntegration.Enqueue(node);
			}
		}

		// Token: 0x0600326F RID: 12911 RVA: 0x0015396C File Offset: 0x00151B6C
		private void OnTerminalNodeSphereUpdated(SphereTreeNode<T> terminalNode)
		{
			if (terminalNode.MustIntegrate)
			{
				return;
			}
			SphereTreeNode<T> parent = terminalNode.Parent;
			if (parent.GetDistanceToNodeExitPoint(terminalNode) > parent.Radius)
			{
				parent.RemoveChild(terminalNode);
				this.AddNodeToIntegrationQueue(terminalNode);
			}
			this.AddNodeToRecomputationQueue(parent);
		}

		// Token: 0x06003270 RID: 12912 RVA: 0x001539B0 File Offset: 0x00151BB0
		private void RenderGizmosDebugRecurse(SphereTreeNode<T> node)
		{
			Gizmos.DrawSphere(node.Sphere.Center, node.Sphere.Radius);
			foreach (SphereTreeNode<T> node2 in node.Children)
			{
				this.RenderGizmosDebugRecurse(node2);
			}
		}

		// Token: 0x06003271 RID: 12913 RVA: 0x00153A24 File Offset: 0x00151C24
		private void RaycastAllRecurse(Ray ray, SphereTreeNode<T> parentNode, List<SphereTreeNodeRayHit<T>> terminalNodeHitInfo)
		{
			float t;
			if (!parentNode.Sphere.Raycast(ray, out t))
			{
				return;
			}
			if (parentNode.IsTerminal)
			{
				terminalNodeHitInfo.Add(new SphereTreeNodeRayHit<!0>(ray, t, parentNode));
				return;
			}
			foreach (SphereTreeNode<T> parentNode2 in parentNode.Children)
			{
				this.RaycastAllRecurse(ray, parentNode2, terminalNodeHitInfo);
			}
		}

		// Token: 0x06003272 RID: 12914 RVA: 0x00153AA4 File Offset: 0x00151CA4
		private void OverlapSphereRecurse(Sphere3D sphere, SphereTreeNode<T> parentNode, List<SphereTreeNode<T>> overlappedTerminalNodes)
		{
			if (!parentNode.Sphere.OverlapsFullyOrPartially(sphere))
			{
				return;
			}
			if (parentNode.IsTerminal)
			{
				overlappedTerminalNodes.Add(parentNode);
				return;
			}
			foreach (SphereTreeNode<T> parentNode2 in parentNode.Children)
			{
				this.OverlapSphereRecurse(sphere, parentNode2, overlappedTerminalNodes);
			}
		}

		// Token: 0x06003273 RID: 12915 RVA: 0x00153B1C File Offset: 0x00151D1C
		private void OverlapBoxRecurse(OrientedBox box, SphereTreeNode<T> parentNode, List<SphereTreeNode<T>> overlappedTerminalNodes)
		{
			if (!parentNode.Sphere.OverlapsFullyOrPartially(box))
			{
				return;
			}
			if (parentNode.IsTerminal)
			{
				overlappedTerminalNodes.Add(parentNode);
				return;
			}
			foreach (SphereTreeNode<T> parentNode2 in parentNode.Children)
			{
				this.OverlapBoxRecurse(box, parentNode2, overlappedTerminalNodes);
			}
		}

		// Token: 0x06003274 RID: 12916 RVA: 0x00153B94 File Offset: 0x00151D94
		private void GetDataToTerminalNodeDictionaryRecurse(SphereTreeNode<T> parentNode, Dictionary<T, SphereTreeNode<T>> dictionary)
		{
			if (parentNode.IsTerminal)
			{
				dictionary.Add(parentNode.Data, parentNode);
				return;
			}
			foreach (SphereTreeNode<T> parentNode2 in parentNode.Children)
			{
				this.GetDataToTerminalNodeDictionaryRecurse(parentNode2, dictionary);
			}
		}

		// Token: 0x0400206B RID: 8299
		protected SphereTreeNode<T> _rootNode;

		// Token: 0x0400206C RID: 8300
		protected int _numberOfChildNodesPerNode;

		// Token: 0x0400206D RID: 8301
		protected Queue<SphereTreeNode<T>> _terminalNodesPendingIntegration = new Queue<SphereTreeNode<!0>>();

		// Token: 0x0400206E RID: 8302
		protected Queue<SphereTreeNode<T>> _nodesPendingRecomputation = new Queue<SphereTreeNode<!0>>();
	}
}
