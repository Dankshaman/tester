using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.Serialization;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace NewNet
{
	// Token: 0x020003AE RID: 942
	[DisallowMultipleComponent]
	public class NetworkView : MonoBehaviour
	{
		// Token: 0x17000529 RID: 1321
		// (get) Token: 0x06002C66 RID: 11366 RVA: 0x00136EF5 File Offset: 0x001350F5
		// (set) Token: 0x06002C67 RID: 11367 RVA: 0x00136EFD File Offset: 0x001350FD
		public ushort id
		{
			get
			{
				return this.id_;
			}
			private set
			{
				this.id_ = value;
			}
		}

		// Token: 0x1700052A RID: 1322
		// (get) Token: 0x06002C68 RID: 11368 RVA: 0x00136F06 File Offset: 0x00135106
		public bool isMine
		{
			get
			{
				return this.owner == Network.player;
			}
		}

		// Token: 0x1700052B RID: 1323
		// (get) Token: 0x06002C69 RID: 11369 RVA: 0x00136F18 File Offset: 0x00135118
		// (set) Token: 0x06002C6A RID: 11370 RVA: 0x00136F20 File Offset: 0x00135120
		public NetworkPlayer owner { get; private set; } = NetworkPlayer.GetServerPlayer();

		// Token: 0x1700052C RID: 1324
		// (get) Token: 0x06002C6B RID: 11371 RVA: 0x00136F29 File Offset: 0x00135129
		// (set) Token: 0x06002C6C RID: 11372 RVA: 0x00136F31 File Offset: 0x00135131
		public bool isNetworkInstantiated { get; private set; }

		// Token: 0x1700052D RID: 1325
		// (get) Token: 0x06002C6D RID: 11373 RVA: 0x00136F3C File Offset: 0x0013513C
		public string InternalName
		{
			get
			{
				string result;
				if ((result = this._InternalName) == null)
				{
					result = (this._InternalName = Utilities.RemoveCloneFromName(base.gameObject.name));
				}
				return result;
			}
		}

		// Token: 0x06002C6E RID: 11374 RVA: 0x00136F6C File Offset: 0x0013516C
		private void Awake()
		{
			NetworkView.FindAttributeAssemblies();
			this.Init();
		}

		// Token: 0x06002C6F RID: 11375 RVA: 0x00136F79 File Offset: 0x00135179
		private void OnDestroy()
		{
			this.Cleanup();
		}

		// Token: 0x06002C70 RID: 11376 RVA: 0x00136F84 File Offset: 0x00135184
		private void Init()
		{
			if (this == null)
			{
				return;
			}
			if (this.inited)
			{
				return;
			}
			this.inited = true;
			if (this.id == 0 && NetworkView.nextCount > 0)
			{
				this.id = NetworkView.nextIds[NetworkView.nextIndex];
				this.owner = NetworkView.nextOwner;
				this.isNetworkInstantiated = (NetworkView.nextIndex == 0);
				if (this.isNetworkInstantiated)
				{
					this.ids = new ushort[NetworkView.nextCount];
					for (int i = 0; i < NetworkView.nextCount; i++)
					{
						this.ids[i] = NetworkView.nextIds[i];
					}
				}
				NetworkView.nextIndex++;
				if (NetworkView.nextIndex >= NetworkView.nextCount)
				{
					NetworkView.ResetNextId();
				}
			}
			if (this.id == 0)
			{
				base.enabled = false;
				return;
			}
			NetworkView.IdView.SetValue(this.id, this);
			this.CacheLocalAttributes();
		}

		// Token: 0x06002C71 RID: 11377 RVA: 0x00137063 File Offset: 0x00135263
		private void Cleanup()
		{
			if (this.id == 0)
			{
				return;
			}
			NetworkView.IdView.TryRemove(this.id);
		}

		// Token: 0x06002C72 RID: 11378 RVA: 0x00137080 File Offset: 0x00135280
		private void CacheLocalAttributes()
		{
			base.GetComponents<NetworkBehavior>(NetworkView.reuseNetworkBehaviorsList);
			for (int i = 0; i < NetworkView.reuseNetworkBehaviorsList.Count; i++)
			{
				NetworkBehavior behavior = NetworkView.reuseNetworkBehaviorsList[i];
				this.CacheNetworkBehavior(behavior);
			}
		}

		// Token: 0x06002C73 RID: 11379 RVA: 0x001370C0 File Offset: 0x001352C0
		private void CacheNetworkBehavior(NetworkBehavior behavior)
		{
			foreach (PropertyInfo propertyInfo in behavior.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic))
			{
				if (Attribute.IsDefined(propertyInfo, typeof(Sync)))
				{
					Sync sync = ((Sync[])propertyInfo.GetCustomAttributes(typeof(Sync), true))[0];
					RuntimeMethodHandle methodHandle = propertyInfo.GetAccessors(true)[0].MethodHandle;
					NetworkView.VarSync value = new NetworkView.PropertySync(behavior, propertyInfo, sync, methodHandle);
					if (!this.SyncVars.ContainsKey(methodHandle))
					{
						this.SyncVars.Add(methodHandle, value);
					}
				}
			}
		}

		// Token: 0x06002C74 RID: 11380 RVA: 0x00137164 File Offset: 0x00135364
		public void RegisterNetworkBehavior(NetworkBehavior behavior)
		{
			this.CacheNetworkBehavior(behavior);
		}

		// Token: 0x06002C75 RID: 11381 RVA: 0x00137170 File Offset: 0x00135370
		private bool CheckWriteHeader(MethodInfo methodInfo, RPCTarget target, out NetworkView.RPCData rpcData)
		{
			ushort redirect = 0;
			if (Network.isClient)
			{
				switch (target)
				{
				case RPCTarget.All:
					redirect = ushort.MaxValue;
					break;
				case RPCTarget.Others:
					redirect = 0;
					break;
				case RPCTarget.Server:
					redirect = 1;
					break;
				}
			}
			else
			{
				redirect = Network.player.id;
			}
			return this.WriteHead(methodInfo, redirect, out rpcData);
		}

		// Token: 0x06002C76 RID: 11382 RVA: 0x001371C4 File Offset: 0x001353C4
		private bool CheckWriteHeader(MethodInfo methodInfo, NetworkPlayer target, out NetworkView.RPCData rpcData)
		{
			ushort id;
			if (Network.isClient)
			{
				id = target.id;
			}
			else
			{
				id = Network.player.id;
			}
			return this.WriteHead(methodInfo, id, out rpcData);
		}

		// Token: 0x06002C77 RID: 11383 RVA: 0x001371FC File Offset: 0x001353FC
		private bool WriteHead(MethodInfo methodInfo, ushort redirect, out NetworkView.RPCData rpcData)
		{
			rpcData = default(NetworkView.RPCData);
			if (this.id == 0)
			{
				return false;
			}
			BitStream stream = NetworkManager.GetStream();
			stream.Rewind();
			stream.WriteByte(0);
			stream.WriteUshort(redirect);
			stream.WriteUshort(this.id);
			bool flag = true;
			ushort value;
			if (NetworkView.MethodId.TryGet(methodInfo.MethodHandle, out value))
			{
				stream.WriteUshort(value);
				NetworkView.MethodRemoteGlobal methodRemoteGlobal;
				if (NetworkView.MethodRPCs.TryGetValue(methodInfo.MethodHandle, out methodRemoteGlobal))
				{
					flag = methodRemoteGlobal.CanCall(Network.player, this);
					if (flag)
					{
						rpcData.stream = stream;
						rpcData.rpc = methodRemoteGlobal.rpc;
						return true;
					}
				}
			}
			if (flag)
			{
				UnityEngine.Debug.LogError("Failed to check write header: " + methodInfo.Name);
			}
			else
			{
				UnityEngine.Debug.Log("You are not allowed to call this rpc: " + methodInfo.Name);
			}
			NetworkManager.ReturnStream(stream);
			return false;
		}

		// Token: 0x06002C78 RID: 11384 RVA: 0x001372D6 File Offset: 0x001354D6
		private void Write<T>(NetworkView.RPCData rpcData, T value)
		{
			NetworkView.Write<T>(rpcData.stream, rpcData.rpc.serializationMethod, value);
		}

		// Token: 0x06002C79 RID: 11385 RVA: 0x001372EF File Offset: 0x001354EF
		private static void Write<T>(BitStream stream, SerializationMethod serializationMethod, T value)
		{
			if (serializationMethod == SerializationMethod.Json)
			{
				stream.WriteString(Json.GetJson(value, false));
				return;
			}
			stream.Write<T>(value, 2147483646);
		}

		// Token: 0x06002C7A RID: 11386 RVA: 0x00137314 File Offset: 0x00135514
		private static object Read(BitStream stream, SerializationMethod serializationMethod, Type type)
		{
			if (serializationMethod == SerializationMethod.Json)
			{
				return Json.Load(stream.ReadString(), type);
			}
			return stream.Read(type, 2147483646);
		}

		// Token: 0x06002C7B RID: 11387 RVA: 0x00137334 File Offset: 0x00135534
		public void RPC(RPCTarget target, Action action)
		{
			if (this.disableNetworking)
			{
				if (this.IsSendingSelf(target))
				{
					action();
				}
				return;
			}
			NetworkView.RPCData rpcData;
			if (!this.CheckWriteHeader(action.Method, target, out rpcData))
			{
				return;
			}
			this.SendRPC(target, rpcData);
		}

		// Token: 0x06002C7C RID: 11388 RVA: 0x00137374 File Offset: 0x00135574
		public void RPC<T1>(RPCTarget target, Action<T1> action, T1 arg1)
		{
			if (this.disableNetworking)
			{
				if (this.IsSendingSelf(target))
				{
					action(arg1);
				}
				return;
			}
			NetworkView.RPCData rpcData;
			if (!this.CheckWriteHeader(action.Method, target, out rpcData))
			{
				return;
			}
			this.Write<T1>(rpcData, arg1);
			this.SendRPC(target, rpcData);
		}

		// Token: 0x06002C7D RID: 11389 RVA: 0x001373BC File Offset: 0x001355BC
		public void RPC<T1, T2>(RPCTarget target, Action<T1, T2> action, T1 arg1, T2 arg2)
		{
			if (this.disableNetworking)
			{
				if (this.IsSendingSelf(target))
				{
					action(arg1, arg2);
				}
				return;
			}
			NetworkView.RPCData rpcData;
			if (!this.CheckWriteHeader(action.Method, target, out rpcData))
			{
				return;
			}
			this.Write<T1>(rpcData, arg1);
			this.Write<T2>(rpcData, arg2);
			this.SendRPC(target, rpcData);
		}

		// Token: 0x06002C7E RID: 11390 RVA: 0x00137410 File Offset: 0x00135610
		public void RPC<T1, T2, T3>(RPCTarget target, Action<T1, T2, T3> action, T1 arg1, T2 arg2, T3 arg3)
		{
			if (this.disableNetworking)
			{
				if (this.IsSendingSelf(target))
				{
					action(arg1, arg2, arg3);
				}
				return;
			}
			NetworkView.RPCData rpcData;
			if (!this.CheckWriteHeader(action.Method, target, out rpcData))
			{
				return;
			}
			this.Write<T1>(rpcData, arg1);
			this.Write<T2>(rpcData, arg2);
			this.Write<T3>(rpcData, arg3);
			this.SendRPC(target, rpcData);
		}

		// Token: 0x06002C7F RID: 11391 RVA: 0x00137470 File Offset: 0x00135670
		public void RPC<T1, T2, T3, T4>(RPCTarget target, Action<T1, T2, T3, T4> action, T1 arg1, T2 arg2, T3 arg3, T4 arg4)
		{
			if (this.disableNetworking)
			{
				if (this.IsSendingSelf(target))
				{
					action(arg1, arg2, arg3, arg4);
				}
				return;
			}
			NetworkView.RPCData rpcData;
			if (!this.CheckWriteHeader(action.Method, target, out rpcData))
			{
				return;
			}
			this.Write<T1>(rpcData, arg1);
			this.Write<T2>(rpcData, arg2);
			this.Write<T3>(rpcData, arg3);
			this.Write<T4>(rpcData, arg4);
			this.SendRPC(target, rpcData);
		}

		// Token: 0x06002C80 RID: 11392 RVA: 0x001374DC File Offset: 0x001356DC
		public void RPC<T1, T2, T3, T4, T5>(RPCTarget target, NetworkView.Action<T1, T2, T3, T4, T5> action, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5)
		{
			if (this.disableNetworking)
			{
				if (this.IsSendingSelf(target))
				{
					action(arg1, arg2, arg3, arg4, arg5);
				}
				return;
			}
			NetworkView.RPCData rpcData;
			if (!this.CheckWriteHeader(action.Method, target, out rpcData))
			{
				return;
			}
			this.Write<T1>(rpcData, arg1);
			this.Write<T2>(rpcData, arg2);
			this.Write<T3>(rpcData, arg3);
			this.Write<T4>(rpcData, arg4);
			this.Write<T5>(rpcData, arg5);
			this.SendRPC(target, rpcData);
		}

		// Token: 0x06002C81 RID: 11393 RVA: 0x00137550 File Offset: 0x00135750
		public void RPC<T1, T2, T3, T4, T5, T6>(RPCTarget target, NetworkView.Action<T1, T2, T3, T4, T5, T6> action, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6)
		{
			if (this.disableNetworking)
			{
				if (this.IsSendingSelf(target))
				{
					action(arg1, arg2, arg3, arg4, arg5, arg6);
				}
				return;
			}
			NetworkView.RPCData rpcData;
			if (!this.CheckWriteHeader(action.Method, target, out rpcData))
			{
				return;
			}
			this.Write<T1>(rpcData, arg1);
			this.Write<T2>(rpcData, arg2);
			this.Write<T3>(rpcData, arg3);
			this.Write<T4>(rpcData, arg4);
			this.Write<T5>(rpcData, arg5);
			this.Write<T6>(rpcData, arg6);
			this.SendRPC(target, rpcData);
		}

		// Token: 0x06002C82 RID: 11394 RVA: 0x001375D0 File Offset: 0x001357D0
		public void RPC<TResult>(RPCTarget target, Func<TResult> action)
		{
			if (this.disableNetworking)
			{
				if (this.IsSendingSelf(target))
				{
					action();
				}
				return;
			}
			NetworkView.RPCData rpcData;
			if (!this.CheckWriteHeader(action.Method, target, out rpcData))
			{
				return;
			}
			this.SendRPC(target, rpcData);
		}

		// Token: 0x06002C83 RID: 11395 RVA: 0x00137610 File Offset: 0x00135810
		public void RPC<T1, TResult>(RPCTarget target, Func<T1, TResult> action, T1 arg1)
		{
			if (this.disableNetworking)
			{
				if (this.IsSendingSelf(target))
				{
					action(arg1);
				}
				return;
			}
			NetworkView.RPCData rpcData;
			if (!this.CheckWriteHeader(action.Method, target, out rpcData))
			{
				return;
			}
			this.Write<T1>(rpcData, arg1);
			this.SendRPC(target, rpcData);
		}

		// Token: 0x06002C84 RID: 11396 RVA: 0x0013765C File Offset: 0x0013585C
		public void RPC<T1, T2, TResult>(RPCTarget target, Func<T1, T2, TResult> action, T1 arg1, T2 arg2)
		{
			if (this.disableNetworking)
			{
				if (this.IsSendingSelf(target))
				{
					action(arg1, arg2);
				}
				return;
			}
			NetworkView.RPCData rpcData;
			if (!this.CheckWriteHeader(action.Method, target, out rpcData))
			{
				return;
			}
			this.Write<T1>(rpcData, arg1);
			this.Write<T2>(rpcData, arg2);
			this.SendRPC(target, rpcData);
		}

		// Token: 0x06002C85 RID: 11397 RVA: 0x001376B0 File Offset: 0x001358B0
		public void RPC<T1, T2, T3, TResult>(RPCTarget target, Func<T1, T2, T3, TResult> action, T1 arg1, T2 arg2, T3 arg3)
		{
			if (this.disableNetworking)
			{
				if (this.IsSendingSelf(target))
				{
					action(arg1, arg2, arg3);
				}
				return;
			}
			NetworkView.RPCData rpcData;
			if (!this.CheckWriteHeader(action.Method, target, out rpcData))
			{
				return;
			}
			this.Write<T1>(rpcData, arg1);
			this.Write<T2>(rpcData, arg2);
			this.Write<T3>(rpcData, arg3);
			this.SendRPC(target, rpcData);
		}

		// Token: 0x06002C86 RID: 11398 RVA: 0x00137710 File Offset: 0x00135910
		public void RPC<T1, T2, T3, T4, TResult>(RPCTarget target, Func<T1, T2, T3, T4, TResult> action, T1 arg1, T2 arg2, T3 arg3, T4 arg4)
		{
			if (this.disableNetworking)
			{
				if (this.IsSendingSelf(target))
				{
					action(arg1, arg2, arg3, arg4);
				}
				return;
			}
			NetworkView.RPCData rpcData;
			if (!this.CheckWriteHeader(action.Method, target, out rpcData))
			{
				return;
			}
			this.Write<T1>(rpcData, arg1);
			this.Write<T2>(rpcData, arg2);
			this.Write<T3>(rpcData, arg3);
			this.Write<T4>(rpcData, arg4);
			this.SendRPC(target, rpcData);
		}

		// Token: 0x06002C87 RID: 11399 RVA: 0x0013777C File Offset: 0x0013597C
		public void RPC<T1, T2, T3, T4, T5, TResult>(RPCTarget target, global::Func<T1, T2, T3, T4, T5, TResult> action, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5)
		{
			if (this.disableNetworking)
			{
				if (this.IsSendingSelf(target))
				{
					action(arg1, arg2, arg3, arg4, arg5);
				}
				return;
			}
			NetworkView.RPCData rpcData;
			if (!this.CheckWriteHeader(action.Method, target, out rpcData))
			{
				return;
			}
			this.Write<T1>(rpcData, arg1);
			this.Write<T2>(rpcData, arg2);
			this.Write<T3>(rpcData, arg3);
			this.Write<T4>(rpcData, arg4);
			this.Write<T5>(rpcData, arg5);
			this.SendRPC(target, rpcData);
		}

		// Token: 0x06002C88 RID: 11400 RVA: 0x001377F4 File Offset: 0x001359F4
		public void RPC<T1, T2, T3, T4, T5, T6, TResult>(RPCTarget target, global::Func<T1, T2, T3, T4, T5, T6, TResult> action, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6)
		{
			if (this.disableNetworking)
			{
				if (this.IsSendingSelf(target))
				{
					action(arg1, arg2, arg3, arg4, arg5, arg6);
				}
				return;
			}
			NetworkView.RPCData rpcData;
			if (!this.CheckWriteHeader(action.Method, target, out rpcData))
			{
				return;
			}
			this.Write<T1>(rpcData, arg1);
			this.Write<T2>(rpcData, arg2);
			this.Write<T3>(rpcData, arg3);
			this.Write<T4>(rpcData, arg4);
			this.Write<T5>(rpcData, arg5);
			this.Write<T6>(rpcData, arg6);
			this.SendRPC(target, rpcData);
		}

		// Token: 0x06002C89 RID: 11401 RVA: 0x00137874 File Offset: 0x00135A74
		private bool IsSendingSelf(RPCTarget target)
		{
			return this.disableNetworking && (target == RPCTarget.All || (target == RPCTarget.Server && Network.isServer));
		}

		// Token: 0x06002C8A RID: 11402 RVA: 0x00137890 File Offset: 0x00135A90
		private bool IsSendingSelf(NetworkPlayer receiver)
		{
			return this.disableNetworking && receiver == Network.player;
		}

		// Token: 0x06002C8B RID: 11403 RVA: 0x001378A8 File Offset: 0x00135AA8
		public void RPC(NetworkPlayer receiver, Action action)
		{
			if (this.disableNetworking)
			{
				if (this.IsSendingSelf(receiver))
				{
					action();
				}
				return;
			}
			NetworkView.RPCData rpcData;
			if (!this.CheckWriteHeader(action.Method, receiver, out rpcData))
			{
				return;
			}
			this.SendRPC(receiver, rpcData);
		}

		// Token: 0x06002C8C RID: 11404 RVA: 0x001378E8 File Offset: 0x00135AE8
		public void RPC<T1>(NetworkPlayer receiver, Action<T1> action, T1 arg1)
		{
			if (this.disableNetworking)
			{
				if (this.IsSendingSelf(receiver))
				{
					action(arg1);
				}
				return;
			}
			NetworkView.RPCData rpcData;
			if (!this.CheckWriteHeader(action.Method, receiver, out rpcData))
			{
				return;
			}
			this.Write<T1>(rpcData, arg1);
			this.SendRPC(receiver, rpcData);
		}

		// Token: 0x06002C8D RID: 11405 RVA: 0x00137930 File Offset: 0x00135B30
		public void RPC<T1, T2>(NetworkPlayer receiver, Action<T1, T2> action, T1 arg1, T2 arg2)
		{
			if (this.disableNetworking)
			{
				if (this.IsSendingSelf(receiver))
				{
					action(arg1, arg2);
				}
				return;
			}
			NetworkView.RPCData rpcData;
			if (!this.CheckWriteHeader(action.Method, receiver, out rpcData))
			{
				return;
			}
			this.Write<T1>(rpcData, arg1);
			this.Write<T2>(rpcData, arg2);
			this.SendRPC(receiver, rpcData);
		}

		// Token: 0x06002C8E RID: 11406 RVA: 0x00137984 File Offset: 0x00135B84
		public void RPC<T1, T2, T3>(NetworkPlayer receiver, Action<T1, T2, T3> action, T1 arg1, T2 arg2, T3 arg3)
		{
			if (this.disableNetworking)
			{
				if (this.IsSendingSelf(receiver))
				{
					action(arg1, arg2, arg3);
				}
				return;
			}
			NetworkView.RPCData rpcData;
			if (!this.CheckWriteHeader(action.Method, receiver, out rpcData))
			{
				return;
			}
			this.Write<T1>(rpcData, arg1);
			this.Write<T2>(rpcData, arg2);
			this.Write<T3>(rpcData, arg3);
			this.SendRPC(receiver, rpcData);
		}

		// Token: 0x06002C8F RID: 11407 RVA: 0x001379E4 File Offset: 0x00135BE4
		public void RPC<T1, T2, T3, T4>(NetworkPlayer receiver, Action<T1, T2, T3, T4> action, T1 arg1, T2 arg2, T3 arg3, T4 arg4)
		{
			if (this.disableNetworking)
			{
				if (this.IsSendingSelf(receiver))
				{
					action(arg1, arg2, arg3, arg4);
				}
				return;
			}
			NetworkView.RPCData rpcData;
			if (!this.CheckWriteHeader(action.Method, receiver, out rpcData))
			{
				return;
			}
			this.Write<T1>(rpcData, arg1);
			this.Write<T2>(rpcData, arg2);
			this.Write<T3>(rpcData, arg3);
			this.Write<T4>(rpcData, arg4);
			this.SendRPC(receiver, rpcData);
		}

		// Token: 0x06002C90 RID: 11408 RVA: 0x00137A50 File Offset: 0x00135C50
		public void RPC<T1, T2, T3, T4, T5>(NetworkPlayer receiver, NetworkView.Action<T1, T2, T3, T4, T5> action, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5)
		{
			if (this.disableNetworking)
			{
				if (this.IsSendingSelf(receiver))
				{
					action(arg1, arg2, arg3, arg4, arg5);
				}
				return;
			}
			NetworkView.RPCData rpcData;
			if (!this.CheckWriteHeader(action.Method, receiver, out rpcData))
			{
				return;
			}
			this.Write<T1>(rpcData, arg1);
			this.Write<T2>(rpcData, arg2);
			this.Write<T3>(rpcData, arg3);
			this.Write<T4>(rpcData, arg4);
			this.Write<T5>(rpcData, arg5);
			this.SendRPC(receiver, rpcData);
		}

		// Token: 0x06002C91 RID: 11409 RVA: 0x00137AC4 File Offset: 0x00135CC4
		public void RPC<T1, T2, T3, T4, T5, T6>(NetworkPlayer receiver, NetworkView.Action<T1, T2, T3, T4, T5, T6> action, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6)
		{
			if (this.disableNetworking)
			{
				if (this.IsSendingSelf(receiver))
				{
					action(arg1, arg2, arg3, arg4, arg5, arg6);
				}
				return;
			}
			NetworkView.RPCData rpcData;
			if (!this.CheckWriteHeader(action.Method, receiver, out rpcData))
			{
				return;
			}
			this.Write<T1>(rpcData, arg1);
			this.Write<T2>(rpcData, arg2);
			this.Write<T3>(rpcData, arg3);
			this.Write<T4>(rpcData, arg4);
			this.Write<T5>(rpcData, arg5);
			this.Write<T6>(rpcData, arg6);
			this.SendRPC(receiver, rpcData);
		}

		// Token: 0x06002C92 RID: 11410 RVA: 0x00137B44 File Offset: 0x00135D44
		public void RPC<TResult>(NetworkPlayer receiver, Func<TResult> action)
		{
			if (this.disableNetworking)
			{
				if (this.IsSendingSelf(receiver))
				{
					action();
				}
				return;
			}
			NetworkView.RPCData rpcData;
			if (!this.CheckWriteHeader(action.Method, receiver, out rpcData))
			{
				return;
			}
			this.SendRPC(receiver, rpcData);
		}

		// Token: 0x06002C93 RID: 11411 RVA: 0x00137B84 File Offset: 0x00135D84
		public void RPC<T1, TResult>(NetworkPlayer receiver, Func<T1, TResult> action, T1 arg1)
		{
			if (this.disableNetworking)
			{
				if (this.IsSendingSelf(receiver))
				{
					action(arg1);
				}
				return;
			}
			NetworkView.RPCData rpcData;
			if (!this.CheckWriteHeader(action.Method, receiver, out rpcData))
			{
				return;
			}
			this.Write<T1>(rpcData, arg1);
			this.SendRPC(receiver, rpcData);
		}

		// Token: 0x06002C94 RID: 11412 RVA: 0x00137BD0 File Offset: 0x00135DD0
		public void RPC<T1, T2, TResult>(NetworkPlayer receiver, Func<T1, T2, TResult> action, T1 arg1, T2 arg2)
		{
			if (this.disableNetworking)
			{
				if (this.IsSendingSelf(receiver))
				{
					action(arg1, arg2);
				}
				return;
			}
			NetworkView.RPCData rpcData;
			if (!this.CheckWriteHeader(action.Method, receiver, out rpcData))
			{
				return;
			}
			this.Write<T1>(rpcData, arg1);
			this.Write<T2>(rpcData, arg2);
			this.SendRPC(receiver, rpcData);
		}

		// Token: 0x06002C95 RID: 11413 RVA: 0x00137C24 File Offset: 0x00135E24
		public void RPC<T1, T2, T3, TResult>(NetworkPlayer receiver, Func<T1, T2, T3, TResult> action, T1 arg1, T2 arg2, T3 arg3)
		{
			if (this.disableNetworking)
			{
				if (this.IsSendingSelf(receiver))
				{
					action(arg1, arg2, arg3);
				}
				return;
			}
			NetworkView.RPCData rpcData;
			if (!this.CheckWriteHeader(action.Method, receiver, out rpcData))
			{
				return;
			}
			this.Write<T1>(rpcData, arg1);
			this.Write<T2>(rpcData, arg2);
			this.Write<T3>(rpcData, arg3);
			this.SendRPC(receiver, rpcData);
		}

		// Token: 0x06002C96 RID: 11414 RVA: 0x00137C84 File Offset: 0x00135E84
		public void RPC<T1, T2, T3, T4, TResult>(NetworkPlayer receiver, Func<T1, T2, T3, T4, TResult> action, T1 arg1, T2 arg2, T3 arg3, T4 arg4)
		{
			if (this.disableNetworking)
			{
				if (this.IsSendingSelf(receiver))
				{
					action(arg1, arg2, arg3, arg4);
				}
				return;
			}
			NetworkView.RPCData rpcData;
			if (!this.CheckWriteHeader(action.Method, receiver, out rpcData))
			{
				return;
			}
			this.Write<T1>(rpcData, arg1);
			this.Write<T2>(rpcData, arg2);
			this.Write<T3>(rpcData, arg3);
			this.Write<T4>(rpcData, arg4);
			this.SendRPC(receiver, rpcData);
		}

		// Token: 0x06002C97 RID: 11415 RVA: 0x00137CF0 File Offset: 0x00135EF0
		public void RPC<T1, T2, T3, T4, T5, TResult>(NetworkPlayer receiver, global::Func<T1, T2, T3, T4, T5, TResult> action, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5)
		{
			if (this.disableNetworking)
			{
				if (this.IsSendingSelf(receiver))
				{
					action(arg1, arg2, arg3, arg4, arg5);
				}
				return;
			}
			NetworkView.RPCData rpcData;
			if (!this.CheckWriteHeader(action.Method, receiver, out rpcData))
			{
				return;
			}
			this.Write<T1>(rpcData, arg1);
			this.Write<T2>(rpcData, arg2);
			this.Write<T3>(rpcData, arg3);
			this.Write<T4>(rpcData, arg4);
			this.Write<T5>(rpcData, arg5);
			this.SendRPC(receiver, rpcData);
		}

		// Token: 0x06002C98 RID: 11416 RVA: 0x00137D68 File Offset: 0x00135F68
		public void RPC<T1, T2, T3, T4, T5, T6, TResult>(NetworkPlayer receiver, global::Func<T1, T2, T3, T4, T5, T6, TResult> action, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6)
		{
			if (this.disableNetworking)
			{
				if (this.IsSendingSelf(receiver))
				{
					action(arg1, arg2, arg3, arg4, arg5, arg6);
				}
				return;
			}
			NetworkView.RPCData rpcData;
			if (!this.CheckWriteHeader(action.Method, receiver, out rpcData))
			{
				return;
			}
			this.Write<T1>(rpcData, arg1);
			this.Write<T2>(rpcData, arg2);
			this.Write<T3>(rpcData, arg3);
			this.Write<T4>(rpcData, arg4);
			this.Write<T5>(rpcData, arg5);
			this.Write<T6>(rpcData, arg6);
			this.SendRPC(receiver, rpcData);
		}

		// Token: 0x06002C99 RID: 11417 RVA: 0x00137DE8 File Offset: 0x00135FE8
		private void SendRPC(RPCTarget target, NetworkView.RPCData rpcData)
		{
			if (Network.isServer)
			{
				Singleton<NetworkManager>.Instance.SendPacket(target, rpcData.stream, rpcData.rpc.sendType, -1);
			}
			else
			{
				Singleton<NetworkManager>.Instance.SendPacket(RPCTarget.Server, rpcData.stream, rpcData.rpc.sendType, -1);
			}
			NetworkManager.ReturnStream(rpcData.stream);
		}

		// Token: 0x06002C9A RID: 11418 RVA: 0x00137E44 File Offset: 0x00136044
		private void SendRPC(NetworkPlayer receiver, NetworkView.RPCData rpcData)
		{
			if (Network.isServer)
			{
				Singleton<NetworkManager>.Instance.SendPacket(receiver, rpcData.stream, rpcData.rpc.sendType, -1);
			}
			else
			{
				Singleton<NetworkManager>.Instance.SendPacket(RPCTarget.Server, rpcData.stream, rpcData.rpc.sendType, -1);
			}
			NetworkManager.ReturnStream(rpcData.stream);
		}

		// Token: 0x1700052E RID: 1326
		// (get) Token: 0x06002C9B RID: 11419 RVA: 0x00137E9F File Offset: 0x0013609F
		public bool IsSyncDirty
		{
			get
			{
				return this.dirtyVarSyncs.Count > 0;
			}
		}

		// Token: 0x06002C9C RID: 11420 RVA: 0x00137EB0 File Offset: 0x001360B0
		public void DirtySync(NetworkBehavior behavior, string callMember)
		{
			if (!base.enabled)
			{
				return;
			}
			ListReadOnly<NetworkView.VarSync> list = this.SyncVars.List;
			if (callMember == null)
			{
				for (int i = 0; i < list.Count; i++)
				{
					NetworkView.VarSync varSync = list[i];
					if (varSync.behavior == behavior)
					{
						this.dirtyVarSyncs.TryAddUnique(varSync);
					}
				}
				return;
			}
			for (int j = 0; j < list.Count; j++)
			{
				NetworkView.VarSync varSync2 = list[j];
				if (varSync2.behavior == behavior && varSync2.name == callMember)
				{
					this.dirtyVarSyncs.TryAddUnique(varSync2);
					return;
				}
			}
			UnityEngine.Debug.LogError("Could not dirty sync callMember: " + callMember + ". Pass null to DirtySync() if you are trying to dirty the class.");
		}

		// Token: 0x06002C9D RID: 11421 RVA: 0x00137F6C File Offset: 0x0013616C
		public bool CheckSync(BitStream stream)
		{
			if (!this.IsSyncDirty)
			{
				return false;
			}
			bool result = false;
			for (int i = 0; i < this.dirtyVarSyncs.Count; i++)
			{
				NetworkView.VarSync varSync = this.dirtyVarSyncs[i];
				object value;
				ushort value2;
				if (varSync.CanCall(Network.player) && varSync.CheckValueDirty(out value) && NetworkView.SyncId.TryGet(varSync.handle, out value2))
				{
					stream.WriteUshort(this.id);
					stream.WriteUshort(value2);
					NetworkView.Write<object>(stream, varSync.sync.serializationMethod, value);
					result = true;
				}
			}
			this.dirtyVarSyncs.Clear();
			return result;
		}

		// Token: 0x06002C9E RID: 11422 RVA: 0x0013800C File Offset: 0x0013620C
		public bool CheckConnectSync(BitStream stream)
		{
			if (Network.isClient)
			{
				return false;
			}
			bool result = false;
			ListReadOnly<NetworkView.VarSync> list = this.SyncVars.List;
			for (int i = 0; i < list.Count; i++)
			{
				NetworkView.VarSync varSync = list[i];
				object value;
				ushort value2;
				if (varSync.CheckStartValueDirty(out value) && NetworkView.SyncId.TryGet(varSync.handle, out value2))
				{
					stream.WriteUshort(this.id);
					stream.WriteUshort(value2);
					NetworkView.Write<object>(stream, varSync.sync.serializationMethod, value);
					result = true;
				}
			}
			return result;
		}

		// Token: 0x06002C9F RID: 11423 RVA: 0x00138098 File Offset: 0x00136298
		public static bool ReceiveRPC(BitStream stream, NetworkPlayer sender, bool reliable)
		{
			ushort num = stream.ReadUshort();
			ushort num2 = stream.ReadUshort();
			NetworkView networkView;
			if (!NetworkView.IdView.TryGetValue(num, out networkView))
			{
				string text = "";
				ISerializable key;
				NetworkView.MethodRemoteGlobal methodRemoteGlobal;
				if (NetworkView.MethodId.TryGet(num2, out key) && NetworkView.MethodRPCs.TryGetValue(key, out methodRemoteGlobal))
				{
					text = methodRemoteGlobal.name;
				}
				if (reliable)
				{
					UnityEngine.Debug.LogError(string.Concat(new object[]
					{
						"RPC-> Can't find networkView with id: ",
						num,
						" method:",
						text
					}));
				}
				return false;
			}
			ISerializable key2;
			if (!NetworkView.MethodId.TryGet(num2, out key2))
			{
				UnityEngine.Debug.LogError("RPC-> Can't find method with id: " + num2);
				return false;
			}
			NetworkView.MethodRemoteGlobal methodRemoteGlobal2;
			if (!NetworkView.MethodRPCs.TryGetValue(key2, out methodRemoteGlobal2))
			{
				UnityEngine.Debug.LogError("RPC-> Can't find method.");
				return false;
			}
			object[] parameters = methodRemoteGlobal2.parameters;
			for (int i = 0; i < parameters.Length; i++)
			{
				try
				{
					parameters[i] = NetworkView.Read(stream, methodRemoteGlobal2.rpc.serializationMethod, methodRemoteGlobal2.parameterInfos[i].ParameterType);
				}
				catch (Exception ex)
				{
					UnityEngine.Debug.LogError(string.Concat(new object[]
					{
						"RPC-> Couldn't read type: ",
						methodRemoteGlobal2.name,
						" type: ",
						methodRemoteGlobal2.parameterInfos[i].ParameterType,
						" : ",
						ex.Message
					}));
					UnityEngine.Debug.LogException(ex);
					return false;
				}
			}
			if (!methodRemoteGlobal2.CanCall(sender, networkView))
			{
				UnityEngine.Debug.LogError(string.Concat(new object[]
				{
					"RPC-> ",
					sender,
					" can't call: ",
					methodRemoteGlobal2.name
				}));
				return false;
			}
			Component component = networkView.GetComponent(methodRemoteGlobal2.classType);
			NetworkBehavior behavior;
			if (component && (behavior = (component as NetworkBehavior)) != null)
			{
				methodRemoteGlobal2.Invoke(behavior);
				return true;
			}
			UnityEngine.Debug.LogError("RPC-> Couldn't find type: " + methodRemoteGlobal2.name);
			return false;
		}

		// Token: 0x06002CA0 RID: 11424 RVA: 0x001382B4 File Offset: 0x001364B4
		public static bool ReceiveSync(BitStream stream, NetworkPlayer sender)
		{
			ushort num = stream.ReadUshort();
			ushort num2 = stream.ReadUshort();
			NetworkView networkView;
			if (!NetworkView.IdView.TryGetValue(num, out networkView))
			{
				if (sender.isServer)
				{
					UnityEngine.Debug.LogError("Sync-> Can't find networkView with id: " + num);
				}
				return false;
			}
			ISerializable key;
			if (!NetworkView.SyncId.TryGet(num2, out key))
			{
				UnityEngine.Debug.LogError("Sync-> Can't find sync with id: " + num2);
				return false;
			}
			NetworkView.VarSync varSync;
			if (networkView.SyncVars.TryGetValue(key, out varSync))
			{
				object value;
				try
				{
					value = NetworkView.Read(stream, varSync.sync.serializationMethod, varSync.type);
				}
				catch (Exception ex)
				{
					UnityEngine.Debug.LogError(string.Concat(new object[]
					{
						"Sync-> Couldn't read type: ",
						varSync.nameWithType,
						" type: ",
						varSync.type,
						" : ",
						ex.Message
					}));
					return false;
				}
				if (varSync.CanCall(sender) || sender.isServer)
				{
					varSync.SetValue(value);
					varSync.behavior.OnSync();
					return true;
				}
				UnityEngine.Debug.LogError(string.Concat(new object[]
				{
					"Sync-> ",
					sender,
					" can't call: ",
					varSync.nameWithType
				}));
				return false;
			}
			UnityEngine.Debug.LogError("Sync-> Can't find sync.");
			return false;
		}

		// Token: 0x06002CA1 RID: 11425 RVA: 0x00138428 File Offset: 0x00136628
		public static void ResetId()
		{
			NetworkView.currentId = 0;
			NetworkView.ResetNextId();
		}

		// Token: 0x06002CA2 RID: 11426 RVA: 0x00138435 File Offset: 0x00136635
		public static void ResetNextId()
		{
			NetworkView.nextIndex = 0;
			NetworkView.nextCount = 0;
			NetworkView.nextOwner = NetworkPlayer.GetServerPlayer();
		}

		// Token: 0x06002CA3 RID: 11427 RVA: 0x00138450 File Offset: 0x00136650
		private static ushort GetNextId()
		{
			if (NetworkView.currentId == 0)
			{
				NetworkView.InitLargest();
			}
			do
			{
				if (NetworkView.currentId >= 65534)
				{
					NetworkView.currentId = 0;
				}
				NetworkView.currentId += 1;
			}
			while (NetworkView.IdView.ContainsKey(NetworkView.currentId));
			return NetworkView.currentId;
		}

		// Token: 0x06002CA4 RID: 11428 RVA: 0x001384A0 File Offset: 0x001366A0
		public static void GetNextIds(ushort[] ids, int count)
		{
			for (int i = 0; i < count; i++)
			{
				ids[i] = NetworkView.GetNextId();
			}
		}

		// Token: 0x06002CA5 RID: 11429 RVA: 0x001384C1 File Offset: 0x001366C1
		public ushort[] GetIds()
		{
			return this.ids;
		}

		// Token: 0x06002CA6 RID: 11430 RVA: 0x001384C9 File Offset: 0x001366C9
		public static void SetNextIds(ushort[] ids, int count, NetworkPlayer owner)
		{
			NetworkView.nextIndex = 0;
			NetworkView.nextIds = ids;
			NetworkView.nextCount = count;
			NetworkView.nextOwner = owner;
		}

		// Token: 0x06002CA7 RID: 11431 RVA: 0x001384E4 File Offset: 0x001366E4
		private static void InitLargest()
		{
			NetworkView[] array = Resources.FindObjectsOfTypeAll<NetworkView>();
			for (int i = 0; i < array.Length; i++)
			{
				if (array[i].id >= NetworkView.currentId)
				{
					NetworkView.currentId = array[i].id;
					NetworkView.currentId += 1;
				}
			}
		}

		// Token: 0x06002CA8 RID: 11432 RVA: 0x00138530 File Offset: 0x00136730
		private static ushort GetSceneId()
		{
			NetworkView[] array = Resources.FindObjectsOfTypeAll<NetworkView>();
			for (ushort num = 1; num < 65535; num += 1)
			{
				bool flag = false;
				foreach (NetworkView networkView in array)
				{
					if (num == networkView.id)
					{
						flag = true;
						break;
					}
				}
				if (!flag)
				{
					return num;
				}
			}
			return 0;
		}

		// Token: 0x06002CA9 RID: 11433 RVA: 0x00138580 File Offset: 0x00136780
		public void InitEditorId()
		{
			if (!base.gameObject.scene.IsValid())
			{
				this.id = 0;
				return;
			}
			if (this.id == 0)
			{
				this.id = NetworkView.GetSceneId();
				UnityEngine.Debug.Log(string.Concat(new object[]
				{
					"InitEditorId: ",
					base.gameObject.name,
					" : ",
					this.id
				}));
			}
		}

		// Token: 0x06002CAA RID: 11434 RVA: 0x001385F9 File Offset: 0x001367F9
		private void OnValidate()
		{
			this.InitEditorId();
		}

		// Token: 0x06002CAB RID: 11435 RVA: 0x001385F9 File Offset: 0x001367F9
		private void Reset()
		{
			this.InitEditorId();
		}

		// Token: 0x06002CAC RID: 11436 RVA: 0x00138604 File Offset: 0x00136804
		private static void FindAttributeAssemblies()
		{
			if (NetworkView.HasFoundAttributes)
			{
				return;
			}
			NetworkView.HasFoundAttributes = true;
			Stopwatch stopwatch = Stopwatch.StartNew();
			List<NetworkView.MethodRPCSort> RPCMethods = new List<NetworkView.MethodRPCSort>();
			List<NetworkView.FieldSyncSort> list = new List<NetworkView.FieldSyncSort>();
			List<NetworkView.PropertySyncSort> SyncProperties = new List<NetworkView.PropertySyncSort>();
			Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
			for (int i = 0; i < assemblies.Length; i++)
			{
				foreach (Type type in assemblies[i].GetTypes())
				{
					if (type.IsSubclassOf(typeof(NetworkBehavior)))
					{
						foreach (MethodInfo methodInfo in type.GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic))
						{
							if (Attribute.IsDefined(methodInfo, typeof(Remote)))
							{
								Remote[] array = (Remote[])methodInfo.GetCustomAttributes(typeof(Remote), true);
								RPCMethods.Add(new NetworkView.MethodRPCSort(type, methodInfo, array[0]));
							}
						}
						foreach (PropertyInfo propertyInfo in type.GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic))
						{
							if (Attribute.IsDefined(propertyInfo, typeof(Sync)))
							{
								Sync[] array2 = (Sync[])propertyInfo.GetCustomAttributes(typeof(Sync), true);
								SyncProperties.Add(new NetworkView.PropertySyncSort(type, propertyInfo, array2[0]));
							}
						}
					}
				}
			}
			RPCMethods.Sort((NetworkView.MethodRPCSort x, NetworkView.MethodRPCSort y) => x.uniqueName.CompareTo(y.uniqueName));
			list.Sort((NetworkView.FieldSyncSort x, NetworkView.FieldSyncSort y) => x.uniqueName.CompareTo(y.uniqueName));
			SyncProperties.Sort((NetworkView.PropertySyncSort x, NetworkView.PropertySyncSort y) => x.uniqueName.CompareTo(y.uniqueName));
			ushort num = 1;
			ushort num2 = 1;
			for (int m = 0; m < RPCMethods.Count; m++)
			{
				NetworkView.MethodRPCSort methodRPCSort = RPCMethods[m];
				MethodInfo method = methodRPCSort.method;
				if (method.GetParameters().Length > 6)
				{
					UnityEngine.Debug.LogError(methodRPCSort.uniqueName + " has too many parameters. 6 is max.");
				}
				if (!methodRPCSort.classType.IsSubclassOf(typeof(NetworkBehavior)))
				{
					UnityEngine.Debug.LogError("Class is not inherited from NetworkBehavior: " + methodRPCSort.uniqueName);
				}
				else
				{
					RuntimeMethodHandle methodHandle = method.MethodHandle;
					if (!NetworkView.MethodId.Forward.ContainsKey(methodHandle))
					{
						NetworkView.MethodId.Add(methodHandle, num);
						NetworkView.MethodRPCs.Add(methodHandle, new NetworkView.MethodRemoteGlobal(methodRPCSort.uniqueName, methodRPCSort.classType, methodRPCSort.method, methodRPCSort.rpc));
						num += 1;
					}
				}
			}
			for (int n = 0; n < list.Count; n++)
			{
				RuntimeFieldHandle fieldHandle = list[n].field.FieldHandle;
				if (!list[n].classType.IsSubclassOf(typeof(NetworkBehavior)))
				{
					UnityEngine.Debug.LogError("Class is not inherited from NetworkBehavior: " + list[n].uniqueName);
				}
				else if (!NetworkView.SyncId.Forward.ContainsKey(fieldHandle))
				{
					NetworkView.SyncId.Add(fieldHandle, num2);
					num2 += 1;
				}
			}
			for (int num3 = 0; num3 < SyncProperties.Count; num3++)
			{
				PropertyInfo property = SyncProperties[num3].property;
				if (!SyncProperties[num3].classType.IsSubclassOf(typeof(NetworkBehavior)))
				{
					UnityEngine.Debug.LogError("Class is not inherited from NetworkBehavior: " + SyncProperties[num3].uniqueName);
				}
				else
				{
					MethodInfo[] accessors = property.GetAccessors(true);
					if (accessors.Length < 2)
					{
						UnityEngine.Debug.LogError(SyncProperties[num3].uniqueName + " doesn't have read write.");
					}
					RuntimeMethodHandle methodHandle2 = accessors[0].MethodHandle;
					if (!NetworkView.SyncId.Forward.ContainsKey(methodHandle2))
					{
						NetworkView.SyncId.Add(methodHandle2, num2);
						num2 += 1;
					}
				}
			}
			stopwatch.Log("Cache networking");
			Wait.Frames(delegate
			{
				foreach (NetworkView.MethodRPCSort methodRPCSort2 in RPCMethods)
				{
					Remote rpc = methodRPCSort2.rpc;
					if (rpc.validationFunction != null && !BaseNetworkAttribute.validationFunctions.ContainsKey(rpc.validationFunction))
					{
						UnityEngine.Debug.LogError("Network Attribute does not contain validation function " + rpc.validationFunction + " on " + methodRPCSort2.uniqueName);
					}
				}
				foreach (NetworkView.PropertySyncSort propertySyncSort in SyncProperties)
				{
					Sync sync = propertySyncSort.sync;
					if (sync.validationFunction != null && !BaseNetworkAttribute.validationFunctions.ContainsKey(sync.validationFunction))
					{
						UnityEngine.Debug.LogError("Network Attribute does not contain validation function " + sync.validationFunction + " on " + propertySyncSort.uniqueName);
					}
				}
			}, 3);
		}

		// Token: 0x06002CAD RID: 11437 RVA: 0x00138A94 File Offset: 0x00136C94
		public static void InitDisable()
		{
			GameObject[] rootGameObjects = SceneManager.GetActiveScene().GetRootGameObjects();
			for (int i = 0; i < rootGameObjects.Length; i++)
			{
				foreach (NetworkView networkView in rootGameObjects[i].GetComponentsInChildren<NetworkView>(true))
				{
					if (networkView && !networkView.gameObject.activeInHierarchy)
					{
						networkView.Init();
					}
				}
			}
		}

		// Token: 0x06002CAE RID: 11438 RVA: 0x00138AFD File Offset: 0x00136CFD
		public override string ToString()
		{
			return "NetworkView: " + this.id;
		}

		// Token: 0x04001DFC RID: 7676
		[SerializeField]
		private ushort id_;

		// Token: 0x04001DFF RID: 7679
		public static readonly Listionary<ushort, NetworkView> IdView = new Listionary<ushort, NetworkView>();

		// Token: 0x04001E00 RID: 7680
		private static readonly Map<ISerializable, ushort> MethodId = new Map<ISerializable, ushort>();

		// Token: 0x04001E01 RID: 7681
		private static readonly Listionary<ISerializable, NetworkView.MethodRemoteGlobal> MethodRPCs = new Listionary<ISerializable, NetworkView.MethodRemoteGlobal>();

		// Token: 0x04001E02 RID: 7682
		private static readonly Map<ISerializable, ushort> SyncId = new Map<ISerializable, ushort>();

		// Token: 0x04001E03 RID: 7683
		private static readonly List<NetworkBehavior> reuseNetworkBehaviorsList = new List<NetworkBehavior>();

		// Token: 0x04001E04 RID: 7684
		private readonly Listionary<ISerializable, NetworkView.VarSync> SyncVars = new Listionary<ISerializable, NetworkView.VarSync>();

		// Token: 0x04001E05 RID: 7685
		[NonSerialized]
		public bool disableNetworking;

		// Token: 0x04001E06 RID: 7686
		private string _InternalName;

		// Token: 0x04001E07 RID: 7687
		private ushort[] ids;

		// Token: 0x04001E08 RID: 7688
		private bool inited;

		// Token: 0x04001E09 RID: 7689
		private readonly List<NetworkView.VarSync> dirtyVarSyncs = new List<NetworkView.VarSync>();

		// Token: 0x04001E0A RID: 7690
		private static ushort currentId = 0;

		// Token: 0x04001E0B RID: 7691
		private static ushort[] nextIds = new ushort[256];

		// Token: 0x04001E0C RID: 7692
		private static int nextIndex = 0;

		// Token: 0x04001E0D RID: 7693
		private static int nextCount = 0;

		// Token: 0x04001E0E RID: 7694
		private static NetworkPlayer nextOwner = NetworkPlayer.GetServerPlayer();

		// Token: 0x04001E0F RID: 7695
		private static bool HasFoundAttributes = false;

		// Token: 0x020007DE RID: 2014
		private struct RPCData
		{
			// Token: 0x04002D83 RID: 11651
			public BitStream stream;

			// Token: 0x04002D84 RID: 11652
			public Remote rpc;
		}

		// Token: 0x020007DF RID: 2015
		// (Invoke) Token: 0x06004008 RID: 16392
		public delegate void Action<T1, T2, T3, T4, T5>(T1 p1, T2 p2, T3 p3, T4 p4, T5 p5);

		// Token: 0x020007E0 RID: 2016
		// (Invoke) Token: 0x0600400C RID: 16396
		public delegate void Action<T1, T2, T3, T4, T5, T6>(T1 p1, T2 p2, T3 p3, T4 p4, T5 p5, T6 p6);

		// Token: 0x020007E1 RID: 2017
		private struct MethodRPCSort
		{
			// Token: 0x0600400F RID: 16399 RVA: 0x001825CB File Offset: 0x001807CB
			public MethodRPCSort(Type classType, MethodInfo method, Remote rpc)
			{
				this.classType = classType;
				this.method = method;
				this.rpc = rpc;
				this.uniqueName = classType.Name + "/" + method.Name;
			}

			// Token: 0x04002D85 RID: 11653
			public Type classType;

			// Token: 0x04002D86 RID: 11654
			public MethodInfo method;

			// Token: 0x04002D87 RID: 11655
			public Remote rpc;

			// Token: 0x04002D88 RID: 11656
			public string uniqueName;
		}

		// Token: 0x020007E2 RID: 2018
		private struct FieldSyncSort
		{
			// Token: 0x06004010 RID: 16400 RVA: 0x001825FE File Offset: 0x001807FE
			public FieldSyncSort(Type classType, FieldInfo field, Sync sync)
			{
				this.classType = classType;
				this.field = field;
				this.sync = sync;
				this.uniqueName = classType.Name + "/" + field.Name;
			}

			// Token: 0x04002D89 RID: 11657
			public Type classType;

			// Token: 0x04002D8A RID: 11658
			public FieldInfo field;

			// Token: 0x04002D8B RID: 11659
			public Sync sync;

			// Token: 0x04002D8C RID: 11660
			public string uniqueName;
		}

		// Token: 0x020007E3 RID: 2019
		private struct PropertySyncSort
		{
			// Token: 0x06004011 RID: 16401 RVA: 0x00182631 File Offset: 0x00180831
			public PropertySyncSort(Type classType, PropertyInfo property, Sync sync)
			{
				this.classType = classType;
				this.property = property;
				this.sync = sync;
				this.uniqueName = classType.Name + "/" + property.Name;
			}

			// Token: 0x04002D8D RID: 11661
			public Type classType;

			// Token: 0x04002D8E RID: 11662
			public PropertyInfo property;

			// Token: 0x04002D8F RID: 11663
			public Sync sync;

			// Token: 0x04002D90 RID: 11664
			public string uniqueName;
		}

		// Token: 0x020007E4 RID: 2020
		public abstract class NetworkAttributeGlobal
		{
			// Token: 0x1700083F RID: 2111
			// (get) Token: 0x06004012 RID: 16402 RVA: 0x00182664 File Offset: 0x00180864
			// (set) Token: 0x06004013 RID: 16403 RVA: 0x0018266C File Offset: 0x0018086C
			public Type classType { get; protected set; }

			// Token: 0x17000840 RID: 2112
			// (get) Token: 0x06004014 RID: 16404 RVA: 0x00182675 File Offset: 0x00180875
			// (set) Token: 0x06004015 RID: 16405 RVA: 0x0018267D File Offset: 0x0018087D
			public BaseNetworkAttribute attribute { get; protected set; }

			// Token: 0x17000841 RID: 2113
			// (get) Token: 0x06004016 RID: 16406 RVA: 0x00182686 File Offset: 0x00180886
			// (set) Token: 0x06004017 RID: 16407 RVA: 0x0018268E File Offset: 0x0018088E
			public ISerializable handle { get; protected set; }

			// Token: 0x17000842 RID: 2114
			// (get) Token: 0x06004018 RID: 16408 RVA: 0x00182697 File Offset: 0x00180897
			// (set) Token: 0x06004019 RID: 16409 RVA: 0x0018269F File Offset: 0x0018089F
			public string name { get; protected set; }

			// Token: 0x0600401A RID: 16410 RVA: 0x001826A8 File Offset: 0x001808A8
			public bool CanCall(NetworkPlayer networkPlayer, NetworkView networkView)
			{
				if (this.attribute.validationFunction != null)
				{
					Func<NetworkPlayer, bool> func;
					BaseNetworkAttribute.validationFunctions.TryGetValue(this.attribute.validationFunction, out func);
					if (func != null && !func(networkPlayer))
					{
						return false;
					}
				}
				switch (this.attribute.permission)
				{
				case Permission.Owner:
					return networkPlayer.isServer || networkView.owner == networkPlayer;
				case Permission.Server:
					return networkPlayer.isServer;
				case Permission.Admin:
					return networkPlayer.isServer || networkPlayer.isAdmin;
				case Permission.Client:
					return networkPlayer.isServer || networkPlayer.isClient;
				default:
					return false;
				}
			}
		}

		// Token: 0x020007E5 RID: 2021
		public class MethodRemoteGlobal : NetworkView.NetworkAttributeGlobal
		{
			// Token: 0x17000843 RID: 2115
			// (get) Token: 0x0600401C RID: 16412 RVA: 0x00182753 File Offset: 0x00180953
			// (set) Token: 0x0600401D RID: 16413 RVA: 0x0018275B File Offset: 0x0018095B
			public MethodInfo method { get; private set; }

			// Token: 0x17000844 RID: 2116
			// (get) Token: 0x0600401E RID: 16414 RVA: 0x00182764 File Offset: 0x00180964
			// (set) Token: 0x0600401F RID: 16415 RVA: 0x0018276C File Offset: 0x0018096C
			public ParameterInfo[] parameterInfos { get; private set; }

			// Token: 0x17000845 RID: 2117
			// (get) Token: 0x06004020 RID: 16416 RVA: 0x00182775 File Offset: 0x00180975
			// (set) Token: 0x06004021 RID: 16417 RVA: 0x0018277D File Offset: 0x0018097D
			public object[] parameters { get; private set; }

			// Token: 0x17000846 RID: 2118
			// (get) Token: 0x06004022 RID: 16418 RVA: 0x00182786 File Offset: 0x00180986
			// (set) Token: 0x06004023 RID: 16419 RVA: 0x0018278E File Offset: 0x0018098E
			public Remote rpc { get; private set; }

			// Token: 0x06004024 RID: 16420 RVA: 0x00182798 File Offset: 0x00180998
			public MethodRemoteGlobal(string name, Type classType, MethodInfo method, Remote rpc)
			{
				base.name = name;
				base.classType = classType;
				this.method = method;
				this.parameterInfos = method.GetParameters();
				this.parameters = new object[this.parameterInfos.Length];
				this.rpc = rpc;
				base.attribute = rpc;
				base.handle = method.MethodHandle;
			}

			// Token: 0x06004025 RID: 16421 RVA: 0x00182800 File Offset: 0x00180A00
			public void Invoke(NetworkBehavior behavior)
			{
				try
				{
					this.method.Invoke(behavior, this.parameters);
				}
				catch (Exception e)
				{
					Chat.LogException("Network Remote " + base.name, e, false, false);
				}
			}
		}

		// Token: 0x020007E6 RID: 2022
		public abstract class NetworkAttribute
		{
			// Token: 0x17000847 RID: 2119
			// (get) Token: 0x06004026 RID: 16422 RVA: 0x00182850 File Offset: 0x00180A50
			// (set) Token: 0x06004027 RID: 16423 RVA: 0x00182858 File Offset: 0x00180A58
			public NetworkBehavior behavior
			{
				get
				{
					return this._behavior;
				}
				protected set
				{
					this._behavior = value;
					this.networkView = this._behavior.GetComponentInParent<NetworkView>();
				}
			}

			// Token: 0x17000848 RID: 2120
			// (get) Token: 0x06004028 RID: 16424 RVA: 0x00182872 File Offset: 0x00180A72
			// (set) Token: 0x06004029 RID: 16425 RVA: 0x0018287A File Offset: 0x00180A7A
			public NetworkView networkView { get; private set; }

			// Token: 0x17000849 RID: 2121
			// (get) Token: 0x0600402A RID: 16426 RVA: 0x00182883 File Offset: 0x00180A83
			// (set) Token: 0x0600402B RID: 16427 RVA: 0x0018288B File Offset: 0x00180A8B
			public BaseNetworkAttribute attribute { get; protected set; }

			// Token: 0x0600402C RID: 16428 RVA: 0x00182894 File Offset: 0x00180A94
			public bool CanCall(NetworkPlayer networkPlayer)
			{
				if (this.attribute.validationFunction != null)
				{
					Func<NetworkPlayer, bool> func;
					BaseNetworkAttribute.validationFunctions.TryGetValue(this.attribute.validationFunction, out func);
					if (func != null && !func(networkPlayer))
					{
						return false;
					}
				}
				switch (this.attribute.permission)
				{
				case Permission.Owner:
					return this.networkView.owner == networkPlayer;
				case Permission.Server:
					return networkPlayer.isServer;
				case Permission.Admin:
					return networkPlayer.isServer || networkPlayer.isAdmin;
				case Permission.Client:
					return networkPlayer.isServer || networkPlayer.isClient;
				default:
					return false;
				}
			}

			// Token: 0x04002D99 RID: 11673
			private NetworkBehavior _behavior;
		}

		// Token: 0x020007E7 RID: 2023
		public abstract class VarSync : NetworkView.NetworkAttribute
		{
			// Token: 0x1700084A RID: 2122
			// (get) Token: 0x0600402E RID: 16430 RVA: 0x00182939 File Offset: 0x00180B39
			// (set) Token: 0x0600402F RID: 16431 RVA: 0x00182941 File Offset: 0x00180B41
			public Sync sync { get; protected set; }

			// Token: 0x1700084B RID: 2123
			// (get) Token: 0x06004030 RID: 16432 RVA: 0x0018294A File Offset: 0x00180B4A
			// (set) Token: 0x06004031 RID: 16433 RVA: 0x00182952 File Offset: 0x00180B52
			public Type type { get; protected set; }

			// Token: 0x1700084C RID: 2124
			// (get) Token: 0x06004032 RID: 16434 RVA: 0x0018295B File Offset: 0x00180B5B
			// (set) Token: 0x06004033 RID: 16435 RVA: 0x00182963 File Offset: 0x00180B63
			public object startValue { get; protected set; }

			// Token: 0x1700084D RID: 2125
			// (get) Token: 0x06004034 RID: 16436 RVA: 0x0018296C File Offset: 0x00180B6C
			// (set) Token: 0x06004035 RID: 16437 RVA: 0x00182974 File Offset: 0x00180B74
			public object prevValue { get; protected set; }

			// Token: 0x1700084E RID: 2126
			// (get) Token: 0x06004036 RID: 16438 RVA: 0x0018297D File Offset: 0x00180B7D
			// (set) Token: 0x06004037 RID: 16439 RVA: 0x00182985 File Offset: 0x00180B85
			public int prevHash { get; protected set; }

			// Token: 0x1700084F RID: 2127
			// (get) Token: 0x06004038 RID: 16440 RVA: 0x0018298E File Offset: 0x00180B8E
			// (set) Token: 0x06004039 RID: 16441 RVA: 0x00182996 File Offset: 0x00180B96
			public ISerializable handle { get; protected set; }

			// Token: 0x17000850 RID: 2128
			// (get) Token: 0x0600403A RID: 16442 RVA: 0x0018299F File Offset: 0x00180B9F
			// (set) Token: 0x0600403B RID: 16443 RVA: 0x001829A7 File Offset: 0x00180BA7
			public string nameWithType { get; protected set; }

			// Token: 0x17000851 RID: 2129
			// (get) Token: 0x0600403C RID: 16444 RVA: 0x001829B0 File Offset: 0x00180BB0
			// (set) Token: 0x0600403D RID: 16445 RVA: 0x001829B8 File Offset: 0x00180BB8
			public string name { get; protected set; }

			// Token: 0x0600403E RID: 16446
			public abstract void SetValue(object value);

			// Token: 0x0600403F RID: 16447
			public abstract object GetValue();

			// Token: 0x06004040 RID: 16448 RVA: 0x001829C4 File Offset: 0x00180BC4
			public bool CheckValueDirty(out object value)
			{
				value = this.GetValue();
				int hash = NetworkView.VarSync.GetHash(value);
				bool result = this.sync.ignoreDirtyCheck || this.prevHash != hash || !object.Equals(value, this.prevValue);
				this.prevValue = value;
				this.prevHash = hash;
				return result;
			}

			// Token: 0x06004041 RID: 16449 RVA: 0x00182A19 File Offset: 0x00180C19
			public bool CheckStartValueDirty(out object value)
			{
				if (this.sync.ignoreDirtyCheck)
				{
					value = this.GetValue();
					return true;
				}
				value = this.prevValue;
				return !object.Equals(value, this.startValue);
			}

			// Token: 0x06004042 RID: 16450 RVA: 0x00182A4A File Offset: 0x00180C4A
			public static int GetHash(object value)
			{
				if (value == null)
				{
					return 0;
				}
				return value.GetHashCode();
			}
		}

		// Token: 0x020007E8 RID: 2024
		public class FieldSync : NetworkView.VarSync
		{
			// Token: 0x17000852 RID: 2130
			// (get) Token: 0x06004044 RID: 16452 RVA: 0x00182A5F File Offset: 0x00180C5F
			// (set) Token: 0x06004045 RID: 16453 RVA: 0x00182A67 File Offset: 0x00180C67
			public FieldInfo field { get; private set; }

			// Token: 0x06004046 RID: 16454 RVA: 0x00182A70 File Offset: 0x00180C70
			public FieldSync(NetworkBehavior behavior, FieldInfo field, Sync sync, ISerializable handle)
			{
				base.behavior = behavior;
				this.field = field;
				base.sync = sync;
				base.prevValue = field.GetValue(behavior);
				base.prevHash = NetworkView.VarSync.GetHash(base.prevValue);
				base.startValue = base.prevValue;
				base.type = field.FieldType;
				base.handle = handle;
				base.attribute = sync;
				base.name = field.Name;
				base.nameWithType = behavior.GetType().Name + "/" + base.name;
			}

			// Token: 0x06004047 RID: 16455 RVA: 0x00182B0C File Offset: 0x00180D0C
			public override void SetValue(object value)
			{
				try
				{
					this.field.SetValue(base.behavior, value);
					base.prevValue = value;
					base.prevHash = NetworkView.VarSync.GetHash(value);
				}
				catch (Exception e)
				{
					Chat.LogException("Network Sync " + base.nameWithType, e, false, false);
				}
			}

			// Token: 0x06004048 RID: 16456 RVA: 0x00182B6C File Offset: 0x00180D6C
			public override object GetValue()
			{
				return this.field.GetValue(base.behavior);
			}
		}

		// Token: 0x020007E9 RID: 2025
		public class PropertySync : NetworkView.VarSync
		{
			// Token: 0x17000853 RID: 2131
			// (get) Token: 0x06004049 RID: 16457 RVA: 0x00182B7F File Offset: 0x00180D7F
			// (set) Token: 0x0600404A RID: 16458 RVA: 0x00182B87 File Offset: 0x00180D87
			public PropertyInfo property { get; private set; }

			// Token: 0x0600404B RID: 16459 RVA: 0x00182B90 File Offset: 0x00180D90
			public PropertySync(NetworkBehavior behavior, PropertyInfo property, Sync sync, ISerializable handle)
			{
				base.behavior = behavior;
				this.property = property;
				base.sync = sync;
				base.prevValue = property.GetValue(behavior, null);
				base.prevHash = NetworkView.VarSync.GetHash(base.prevValue);
				base.startValue = base.prevValue;
				base.type = property.PropertyType;
				base.handle = handle;
				base.attribute = sync;
				base.name = property.Name;
				base.nameWithType = behavior.GetType().Name + "/" + base.name;
			}

			// Token: 0x0600404C RID: 16460 RVA: 0x00182C2C File Offset: 0x00180E2C
			public override void SetValue(object value)
			{
				try
				{
					this.property.SetValue(base.behavior, value, null);
					base.prevValue = value;
					base.prevHash = NetworkView.VarSync.GetHash(value);
				}
				catch (Exception e)
				{
					Chat.LogException("Network Sync " + base.nameWithType, e, false, false);
				}
			}

			// Token: 0x0600404D RID: 16461 RVA: 0x00182C8C File Offset: 0x00180E8C
			public override object GetValue()
			{
				return this.property.GetValue(base.behavior, null);
			}
		}
	}
}
