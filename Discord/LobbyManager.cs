using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace Discord
{
	// Token: 0x020004ED RID: 1261
	public class LobbyManager
	{
		// Token: 0x17000749 RID: 1865
		// (get) Token: 0x06003657 RID: 13911 RVA: 0x00167937 File Offset: 0x00165B37
		private LobbyManager.FFIMethods Methods
		{
			get
			{
				if (this.MethodsStructure == null)
				{
					this.MethodsStructure = Marshal.PtrToStructure(this.MethodsPtr, typeof(LobbyManager.FFIMethods));
				}
				return (LobbyManager.FFIMethods)this.MethodsStructure;
			}
		}

		// Token: 0x14000083 RID: 131
		// (add) Token: 0x06003658 RID: 13912 RVA: 0x00167968 File Offset: 0x00165B68
		// (remove) Token: 0x06003659 RID: 13913 RVA: 0x001679A0 File Offset: 0x00165BA0
		public event LobbyManager.LobbyUpdateHandler OnLobbyUpdate;

		// Token: 0x14000084 RID: 132
		// (add) Token: 0x0600365A RID: 13914 RVA: 0x001679D8 File Offset: 0x00165BD8
		// (remove) Token: 0x0600365B RID: 13915 RVA: 0x00167A10 File Offset: 0x00165C10
		public event LobbyManager.LobbyDeleteHandler OnLobbyDelete;

		// Token: 0x14000085 RID: 133
		// (add) Token: 0x0600365C RID: 13916 RVA: 0x00167A48 File Offset: 0x00165C48
		// (remove) Token: 0x0600365D RID: 13917 RVA: 0x00167A80 File Offset: 0x00165C80
		public event LobbyManager.MemberConnectHandler OnMemberConnect;

		// Token: 0x14000086 RID: 134
		// (add) Token: 0x0600365E RID: 13918 RVA: 0x00167AB8 File Offset: 0x00165CB8
		// (remove) Token: 0x0600365F RID: 13919 RVA: 0x00167AF0 File Offset: 0x00165CF0
		public event LobbyManager.MemberUpdateHandler OnMemberUpdate;

		// Token: 0x14000087 RID: 135
		// (add) Token: 0x06003660 RID: 13920 RVA: 0x00167B28 File Offset: 0x00165D28
		// (remove) Token: 0x06003661 RID: 13921 RVA: 0x00167B60 File Offset: 0x00165D60
		public event LobbyManager.MemberDisconnectHandler OnMemberDisconnect;

		// Token: 0x14000088 RID: 136
		// (add) Token: 0x06003662 RID: 13922 RVA: 0x00167B98 File Offset: 0x00165D98
		// (remove) Token: 0x06003663 RID: 13923 RVA: 0x00167BD0 File Offset: 0x00165DD0
		public event LobbyManager.LobbyMessageHandler OnLobbyMessage;

		// Token: 0x14000089 RID: 137
		// (add) Token: 0x06003664 RID: 13924 RVA: 0x00167C08 File Offset: 0x00165E08
		// (remove) Token: 0x06003665 RID: 13925 RVA: 0x00167C40 File Offset: 0x00165E40
		public event LobbyManager.SpeakingHandler OnSpeaking;

		// Token: 0x1400008A RID: 138
		// (add) Token: 0x06003666 RID: 13926 RVA: 0x00167C78 File Offset: 0x00165E78
		// (remove) Token: 0x06003667 RID: 13927 RVA: 0x00167CB0 File Offset: 0x00165EB0
		public event LobbyManager.NetworkMessageHandler OnNetworkMessage;

		// Token: 0x06003668 RID: 13928 RVA: 0x00167CE8 File Offset: 0x00165EE8
		internal LobbyManager(IntPtr ptr, IntPtr eventsPtr, ref LobbyManager.FFIEvents events)
		{
			if (eventsPtr == IntPtr.Zero)
			{
				throw new ResultException(Result.InternalError);
			}
			this.InitEvents(eventsPtr, ref events);
			this.MethodsPtr = ptr;
			if (this.MethodsPtr == IntPtr.Zero)
			{
				throw new ResultException(Result.InternalError);
			}
		}

		// Token: 0x06003669 RID: 13929 RVA: 0x00167D38 File Offset: 0x00165F38
		private void InitEvents(IntPtr eventsPtr, ref LobbyManager.FFIEvents events)
		{
			events.OnLobbyUpdate = new LobbyManager.FFIEvents.LobbyUpdateHandler(LobbyManager.OnLobbyUpdateImpl);
			events.OnLobbyDelete = new LobbyManager.FFIEvents.LobbyDeleteHandler(LobbyManager.OnLobbyDeleteImpl);
			events.OnMemberConnect = new LobbyManager.FFIEvents.MemberConnectHandler(LobbyManager.OnMemberConnectImpl);
			events.OnMemberUpdate = new LobbyManager.FFIEvents.MemberUpdateHandler(LobbyManager.OnMemberUpdateImpl);
			events.OnMemberDisconnect = new LobbyManager.FFIEvents.MemberDisconnectHandler(LobbyManager.OnMemberDisconnectImpl);
			events.OnLobbyMessage = new LobbyManager.FFIEvents.LobbyMessageHandler(LobbyManager.OnLobbyMessageImpl);
			events.OnSpeaking = new LobbyManager.FFIEvents.SpeakingHandler(LobbyManager.OnSpeakingImpl);
			events.OnNetworkMessage = new LobbyManager.FFIEvents.NetworkMessageHandler(LobbyManager.OnNetworkMessageImpl);
			Marshal.StructureToPtr<LobbyManager.FFIEvents>(events, eventsPtr, false);
		}

		// Token: 0x0600366A RID: 13930 RVA: 0x00167DE4 File Offset: 0x00165FE4
		public LobbyTransaction GetLobbyCreateTransaction()
		{
			LobbyTransaction result = default(LobbyTransaction);
			Result result2 = this.Methods.GetLobbyCreateTransaction(this.MethodsPtr, ref result.MethodsPtr);
			if (result2 != Result.Ok)
			{
				throw new ResultException(result2);
			}
			return result;
		}

		// Token: 0x0600366B RID: 13931 RVA: 0x00167E24 File Offset: 0x00166024
		public LobbyTransaction GetLobbyUpdateTransaction(long lobbyId)
		{
			LobbyTransaction result = default(LobbyTransaction);
			Result result2 = this.Methods.GetLobbyUpdateTransaction(this.MethodsPtr, lobbyId, ref result.MethodsPtr);
			if (result2 != Result.Ok)
			{
				throw new ResultException(result2);
			}
			return result;
		}

		// Token: 0x0600366C RID: 13932 RVA: 0x00167E64 File Offset: 0x00166064
		public LobbyMemberTransaction GetMemberUpdateTransaction(long lobbyId, long userId)
		{
			LobbyMemberTransaction result = default(LobbyMemberTransaction);
			Result result2 = this.Methods.GetMemberUpdateTransaction(this.MethodsPtr, lobbyId, userId, ref result.MethodsPtr);
			if (result2 != Result.Ok)
			{
				throw new ResultException(result2);
			}
			return result;
		}

		// Token: 0x0600366D RID: 13933 RVA: 0x00167EA4 File Offset: 0x001660A4
		[MonoPInvokeCallback]
		private static void CreateLobbyCallbackImpl(IntPtr ptr, Result result, ref Lobby lobby)
		{
			GCHandle gchandle = GCHandle.FromIntPtr(ptr);
			LobbyManager.CreateLobbyHandler createLobbyHandler = (LobbyManager.CreateLobbyHandler)gchandle.Target;
			gchandle.Free();
			createLobbyHandler(result, ref lobby);
		}

		// Token: 0x0600366E RID: 13934 RVA: 0x00167ED4 File Offset: 0x001660D4
		public void CreateLobby(LobbyTransaction transaction, LobbyManager.CreateLobbyHandler callback)
		{
			GCHandle value = GCHandle.Alloc(callback);
			this.Methods.CreateLobby(this.MethodsPtr, transaction.MethodsPtr, GCHandle.ToIntPtr(value), new LobbyManager.FFIMethods.CreateLobbyCallback(LobbyManager.CreateLobbyCallbackImpl));
			transaction.MethodsPtr = IntPtr.Zero;
		}

		// Token: 0x0600366F RID: 13935 RVA: 0x00167F24 File Offset: 0x00166124
		[MonoPInvokeCallback]
		private static void UpdateLobbyCallbackImpl(IntPtr ptr, Result result)
		{
			GCHandle gchandle = GCHandle.FromIntPtr(ptr);
			LobbyManager.UpdateLobbyHandler updateLobbyHandler = (LobbyManager.UpdateLobbyHandler)gchandle.Target;
			gchandle.Free();
			updateLobbyHandler(result);
		}

		// Token: 0x06003670 RID: 13936 RVA: 0x00167F54 File Offset: 0x00166154
		public void UpdateLobby(long lobbyId, LobbyTransaction transaction, LobbyManager.UpdateLobbyHandler callback)
		{
			GCHandle value = GCHandle.Alloc(callback);
			this.Methods.UpdateLobby(this.MethodsPtr, lobbyId, transaction.MethodsPtr, GCHandle.ToIntPtr(value), new LobbyManager.FFIMethods.UpdateLobbyCallback(LobbyManager.UpdateLobbyCallbackImpl));
			transaction.MethodsPtr = IntPtr.Zero;
		}

		// Token: 0x06003671 RID: 13937 RVA: 0x00167FA4 File Offset: 0x001661A4
		[MonoPInvokeCallback]
		private static void DeleteLobbyCallbackImpl(IntPtr ptr, Result result)
		{
			GCHandle gchandle = GCHandle.FromIntPtr(ptr);
			LobbyManager.DeleteLobbyHandler deleteLobbyHandler = (LobbyManager.DeleteLobbyHandler)gchandle.Target;
			gchandle.Free();
			deleteLobbyHandler(result);
		}

		// Token: 0x06003672 RID: 13938 RVA: 0x00167FD4 File Offset: 0x001661D4
		public void DeleteLobby(long lobbyId, LobbyManager.DeleteLobbyHandler callback)
		{
			GCHandle value = GCHandle.Alloc(callback);
			this.Methods.DeleteLobby(this.MethodsPtr, lobbyId, GCHandle.ToIntPtr(value), new LobbyManager.FFIMethods.DeleteLobbyCallback(LobbyManager.DeleteLobbyCallbackImpl));
		}

		// Token: 0x06003673 RID: 13939 RVA: 0x00168014 File Offset: 0x00166214
		[MonoPInvokeCallback]
		private static void ConnectLobbyCallbackImpl(IntPtr ptr, Result result, ref Lobby lobby)
		{
			GCHandle gchandle = GCHandle.FromIntPtr(ptr);
			LobbyManager.ConnectLobbyHandler connectLobbyHandler = (LobbyManager.ConnectLobbyHandler)gchandle.Target;
			gchandle.Free();
			connectLobbyHandler(result, ref lobby);
		}

		// Token: 0x06003674 RID: 13940 RVA: 0x00168044 File Offset: 0x00166244
		public void ConnectLobby(long lobbyId, string secret, LobbyManager.ConnectLobbyHandler callback)
		{
			GCHandle value = GCHandle.Alloc(callback);
			this.Methods.ConnectLobby(this.MethodsPtr, lobbyId, secret, GCHandle.ToIntPtr(value), new LobbyManager.FFIMethods.ConnectLobbyCallback(LobbyManager.ConnectLobbyCallbackImpl));
		}

		// Token: 0x06003675 RID: 13941 RVA: 0x00168084 File Offset: 0x00166284
		[MonoPInvokeCallback]
		private static void ConnectLobbyWithActivitySecretCallbackImpl(IntPtr ptr, Result result, ref Lobby lobby)
		{
			GCHandle gchandle = GCHandle.FromIntPtr(ptr);
			LobbyManager.ConnectLobbyWithActivitySecretHandler connectLobbyWithActivitySecretHandler = (LobbyManager.ConnectLobbyWithActivitySecretHandler)gchandle.Target;
			gchandle.Free();
			connectLobbyWithActivitySecretHandler(result, ref lobby);
		}

		// Token: 0x06003676 RID: 13942 RVA: 0x001680B4 File Offset: 0x001662B4
		public void ConnectLobbyWithActivitySecret(string activitySecret, LobbyManager.ConnectLobbyWithActivitySecretHandler callback)
		{
			GCHandle value = GCHandle.Alloc(callback);
			this.Methods.ConnectLobbyWithActivitySecret(this.MethodsPtr, activitySecret, GCHandle.ToIntPtr(value), new LobbyManager.FFIMethods.ConnectLobbyWithActivitySecretCallback(LobbyManager.ConnectLobbyWithActivitySecretCallbackImpl));
		}

		// Token: 0x06003677 RID: 13943 RVA: 0x001680F4 File Offset: 0x001662F4
		[MonoPInvokeCallback]
		private static void DisconnectLobbyCallbackImpl(IntPtr ptr, Result result)
		{
			GCHandle gchandle = GCHandle.FromIntPtr(ptr);
			LobbyManager.DisconnectLobbyHandler disconnectLobbyHandler = (LobbyManager.DisconnectLobbyHandler)gchandle.Target;
			gchandle.Free();
			disconnectLobbyHandler(result);
		}

		// Token: 0x06003678 RID: 13944 RVA: 0x00168124 File Offset: 0x00166324
		public void DisconnectLobby(long lobbyId, LobbyManager.DisconnectLobbyHandler callback)
		{
			GCHandle value = GCHandle.Alloc(callback);
			this.Methods.DisconnectLobby(this.MethodsPtr, lobbyId, GCHandle.ToIntPtr(value), new LobbyManager.FFIMethods.DisconnectLobbyCallback(LobbyManager.DisconnectLobbyCallbackImpl));
		}

		// Token: 0x06003679 RID: 13945 RVA: 0x00168164 File Offset: 0x00166364
		public Lobby GetLobby(long lobbyId)
		{
			Lobby result = default(Lobby);
			Result result2 = this.Methods.GetLobby(this.MethodsPtr, lobbyId, ref result);
			if (result2 != Result.Ok)
			{
				throw new ResultException(result2);
			}
			return result;
		}

		// Token: 0x0600367A RID: 13946 RVA: 0x001681A0 File Offset: 0x001663A0
		public string GetLobbyActivitySecret(long lobbyId)
		{
			StringBuilder stringBuilder = new StringBuilder(128);
			Result result = this.Methods.GetLobbyActivitySecret(this.MethodsPtr, lobbyId, stringBuilder);
			if (result != Result.Ok)
			{
				throw new ResultException(result);
			}
			return stringBuilder.ToString();
		}

		// Token: 0x0600367B RID: 13947 RVA: 0x001681E4 File Offset: 0x001663E4
		public string GetLobbyMetadataValue(long lobbyId, string key)
		{
			StringBuilder stringBuilder = new StringBuilder(4096);
			Result result = this.Methods.GetLobbyMetadataValue(this.MethodsPtr, lobbyId, key, stringBuilder);
			if (result != Result.Ok)
			{
				throw new ResultException(result);
			}
			return stringBuilder.ToString();
		}

		// Token: 0x0600367C RID: 13948 RVA: 0x00168228 File Offset: 0x00166428
		public string GetLobbyMetadataKey(long lobbyId, int index)
		{
			StringBuilder stringBuilder = new StringBuilder(256);
			Result result = this.Methods.GetLobbyMetadataKey(this.MethodsPtr, lobbyId, index, stringBuilder);
			if (result != Result.Ok)
			{
				throw new ResultException(result);
			}
			return stringBuilder.ToString();
		}

		// Token: 0x0600367D RID: 13949 RVA: 0x0016826C File Offset: 0x0016646C
		public int LobbyMetadataCount(long lobbyId)
		{
			int result = 0;
			Result result2 = this.Methods.LobbyMetadataCount(this.MethodsPtr, lobbyId, ref result);
			if (result2 != Result.Ok)
			{
				throw new ResultException(result2);
			}
			return result;
		}

		// Token: 0x0600367E RID: 13950 RVA: 0x001682A0 File Offset: 0x001664A0
		public int MemberCount(long lobbyId)
		{
			int result = 0;
			Result result2 = this.Methods.MemberCount(this.MethodsPtr, lobbyId, ref result);
			if (result2 != Result.Ok)
			{
				throw new ResultException(result2);
			}
			return result;
		}

		// Token: 0x0600367F RID: 13951 RVA: 0x001682D4 File Offset: 0x001664D4
		public long GetMemberUserId(long lobbyId, int index)
		{
			long result = 0L;
			Result result2 = this.Methods.GetMemberUserId(this.MethodsPtr, lobbyId, index, ref result);
			if (result2 != Result.Ok)
			{
				throw new ResultException(result2);
			}
			return result;
		}

		// Token: 0x06003680 RID: 13952 RVA: 0x0016830C File Offset: 0x0016650C
		public User GetMemberUser(long lobbyId, long userId)
		{
			User result = default(User);
			Result result2 = this.Methods.GetMemberUser(this.MethodsPtr, lobbyId, userId, ref result);
			if (result2 != Result.Ok)
			{
				throw new ResultException(result2);
			}
			return result;
		}

		// Token: 0x06003681 RID: 13953 RVA: 0x00168348 File Offset: 0x00166548
		public string GetMemberMetadataValue(long lobbyId, long userId, string key)
		{
			StringBuilder stringBuilder = new StringBuilder(4096);
			Result result = this.Methods.GetMemberMetadataValue(this.MethodsPtr, lobbyId, userId, key, stringBuilder);
			if (result != Result.Ok)
			{
				throw new ResultException(result);
			}
			return stringBuilder.ToString();
		}

		// Token: 0x06003682 RID: 13954 RVA: 0x0016838C File Offset: 0x0016658C
		public string GetMemberMetadataKey(long lobbyId, long userId, int index)
		{
			StringBuilder stringBuilder = new StringBuilder(256);
			Result result = this.Methods.GetMemberMetadataKey(this.MethodsPtr, lobbyId, userId, index, stringBuilder);
			if (result != Result.Ok)
			{
				throw new ResultException(result);
			}
			return stringBuilder.ToString();
		}

		// Token: 0x06003683 RID: 13955 RVA: 0x001683D0 File Offset: 0x001665D0
		public int MemberMetadataCount(long lobbyId, long userId)
		{
			int result = 0;
			Result result2 = this.Methods.MemberMetadataCount(this.MethodsPtr, lobbyId, userId, ref result);
			if (result2 != Result.Ok)
			{
				throw new ResultException(result2);
			}
			return result;
		}

		// Token: 0x06003684 RID: 13956 RVA: 0x00168408 File Offset: 0x00166608
		[MonoPInvokeCallback]
		private static void UpdateMemberCallbackImpl(IntPtr ptr, Result result)
		{
			GCHandle gchandle = GCHandle.FromIntPtr(ptr);
			LobbyManager.UpdateMemberHandler updateMemberHandler = (LobbyManager.UpdateMemberHandler)gchandle.Target;
			gchandle.Free();
			updateMemberHandler(result);
		}

		// Token: 0x06003685 RID: 13957 RVA: 0x00168438 File Offset: 0x00166638
		public void UpdateMember(long lobbyId, long userId, LobbyMemberTransaction transaction, LobbyManager.UpdateMemberHandler callback)
		{
			GCHandle value = GCHandle.Alloc(callback);
			this.Methods.UpdateMember(this.MethodsPtr, lobbyId, userId, transaction.MethodsPtr, GCHandle.ToIntPtr(value), new LobbyManager.FFIMethods.UpdateMemberCallback(LobbyManager.UpdateMemberCallbackImpl));
			transaction.MethodsPtr = IntPtr.Zero;
		}

		// Token: 0x06003686 RID: 13958 RVA: 0x0016848C File Offset: 0x0016668C
		[MonoPInvokeCallback]
		private static void SendLobbyMessageCallbackImpl(IntPtr ptr, Result result)
		{
			GCHandle gchandle = GCHandle.FromIntPtr(ptr);
			LobbyManager.SendLobbyMessageHandler sendLobbyMessageHandler = (LobbyManager.SendLobbyMessageHandler)gchandle.Target;
			gchandle.Free();
			sendLobbyMessageHandler(result);
		}

		// Token: 0x06003687 RID: 13959 RVA: 0x001684BC File Offset: 0x001666BC
		public void SendLobbyMessage(long lobbyId, byte[] data, LobbyManager.SendLobbyMessageHandler callback)
		{
			GCHandle value = GCHandle.Alloc(callback);
			this.Methods.SendLobbyMessage(this.MethodsPtr, lobbyId, data, data.Length, GCHandle.ToIntPtr(value), new LobbyManager.FFIMethods.SendLobbyMessageCallback(LobbyManager.SendLobbyMessageCallbackImpl));
		}

		// Token: 0x06003688 RID: 13960 RVA: 0x00168500 File Offset: 0x00166700
		public LobbySearchQuery GetSearchQuery()
		{
			LobbySearchQuery result = default(LobbySearchQuery);
			Result result2 = this.Methods.GetSearchQuery(this.MethodsPtr, ref result.MethodsPtr);
			if (result2 != Result.Ok)
			{
				throw new ResultException(result2);
			}
			return result;
		}

		// Token: 0x06003689 RID: 13961 RVA: 0x00168540 File Offset: 0x00166740
		[MonoPInvokeCallback]
		private static void SearchCallbackImpl(IntPtr ptr, Result result)
		{
			GCHandle gchandle = GCHandle.FromIntPtr(ptr);
			LobbyManager.SearchHandler searchHandler = (LobbyManager.SearchHandler)gchandle.Target;
			gchandle.Free();
			searchHandler(result);
		}

		// Token: 0x0600368A RID: 13962 RVA: 0x00168570 File Offset: 0x00166770
		public void Search(LobbySearchQuery query, LobbyManager.SearchHandler callback)
		{
			GCHandle value = GCHandle.Alloc(callback);
			this.Methods.Search(this.MethodsPtr, query.MethodsPtr, GCHandle.ToIntPtr(value), new LobbyManager.FFIMethods.SearchCallback(LobbyManager.SearchCallbackImpl));
			query.MethodsPtr = IntPtr.Zero;
		}

		// Token: 0x0600368B RID: 13963 RVA: 0x001685C0 File Offset: 0x001667C0
		public int LobbyCount()
		{
			int result = 0;
			this.Methods.LobbyCount(this.MethodsPtr, ref result);
			return result;
		}

		// Token: 0x0600368C RID: 13964 RVA: 0x001685E8 File Offset: 0x001667E8
		public long GetLobbyId(int index)
		{
			long result = 0L;
			Result result2 = this.Methods.GetLobbyId(this.MethodsPtr, index, ref result);
			if (result2 != Result.Ok)
			{
				throw new ResultException(result2);
			}
			return result;
		}

		// Token: 0x0600368D RID: 13965 RVA: 0x00168620 File Offset: 0x00166820
		[MonoPInvokeCallback]
		private static void ConnectVoiceCallbackImpl(IntPtr ptr, Result result)
		{
			GCHandle gchandle = GCHandle.FromIntPtr(ptr);
			LobbyManager.ConnectVoiceHandler connectVoiceHandler = (LobbyManager.ConnectVoiceHandler)gchandle.Target;
			gchandle.Free();
			connectVoiceHandler(result);
		}

		// Token: 0x0600368E RID: 13966 RVA: 0x00168650 File Offset: 0x00166850
		public void ConnectVoice(long lobbyId, LobbyManager.ConnectVoiceHandler callback)
		{
			GCHandle value = GCHandle.Alloc(callback);
			this.Methods.ConnectVoice(this.MethodsPtr, lobbyId, GCHandle.ToIntPtr(value), new LobbyManager.FFIMethods.ConnectVoiceCallback(LobbyManager.ConnectVoiceCallbackImpl));
		}

		// Token: 0x0600368F RID: 13967 RVA: 0x00168690 File Offset: 0x00166890
		[MonoPInvokeCallback]
		private static void DisconnectVoiceCallbackImpl(IntPtr ptr, Result result)
		{
			GCHandle gchandle = GCHandle.FromIntPtr(ptr);
			LobbyManager.DisconnectVoiceHandler disconnectVoiceHandler = (LobbyManager.DisconnectVoiceHandler)gchandle.Target;
			gchandle.Free();
			disconnectVoiceHandler(result);
		}

		// Token: 0x06003690 RID: 13968 RVA: 0x001686C0 File Offset: 0x001668C0
		public void DisconnectVoice(long lobbyId, LobbyManager.DisconnectVoiceHandler callback)
		{
			GCHandle value = GCHandle.Alloc(callback);
			this.Methods.DisconnectVoice(this.MethodsPtr, lobbyId, GCHandle.ToIntPtr(value), new LobbyManager.FFIMethods.DisconnectVoiceCallback(LobbyManager.DisconnectVoiceCallbackImpl));
		}

		// Token: 0x06003691 RID: 13969 RVA: 0x00168700 File Offset: 0x00166900
		public void ConnectNetwork(long lobbyId)
		{
			Result result = this.Methods.ConnectNetwork(this.MethodsPtr, lobbyId);
			if (result != Result.Ok)
			{
				throw new ResultException(result);
			}
		}

		// Token: 0x06003692 RID: 13970 RVA: 0x00168730 File Offset: 0x00166930
		public void DisconnectNetwork(long lobbyId)
		{
			Result result = this.Methods.DisconnectNetwork(this.MethodsPtr, lobbyId);
			if (result != Result.Ok)
			{
				throw new ResultException(result);
			}
		}

		// Token: 0x06003693 RID: 13971 RVA: 0x00168760 File Offset: 0x00166960
		public void FlushNetwork()
		{
			Result result = this.Methods.FlushNetwork(this.MethodsPtr);
			if (result != Result.Ok)
			{
				throw new ResultException(result);
			}
		}

		// Token: 0x06003694 RID: 13972 RVA: 0x00168790 File Offset: 0x00166990
		public void OpenNetworkChannel(long lobbyId, byte channelId, bool reliable)
		{
			Result result = this.Methods.OpenNetworkChannel(this.MethodsPtr, lobbyId, channelId, reliable);
			if (result != Result.Ok)
			{
				throw new ResultException(result);
			}
		}

		// Token: 0x06003695 RID: 13973 RVA: 0x001687C4 File Offset: 0x001669C4
		public void SendNetworkMessage(long lobbyId, long userId, byte channelId, byte[] data)
		{
			Result result = this.Methods.SendNetworkMessage(this.MethodsPtr, lobbyId, userId, channelId, data, data.Length);
			if (result != Result.Ok)
			{
				throw new ResultException(result);
			}
		}

		// Token: 0x06003696 RID: 13974 RVA: 0x001687FC File Offset: 0x001669FC
		[MonoPInvokeCallback]
		private static void OnLobbyUpdateImpl(IntPtr ptr, long lobbyId)
		{
			Discord discord = (Discord)GCHandle.FromIntPtr(ptr).Target;
			if (discord.LobbyManagerInstance.OnLobbyUpdate != null)
			{
				discord.LobbyManagerInstance.OnLobbyUpdate(lobbyId);
			}
		}

		// Token: 0x06003697 RID: 13975 RVA: 0x0016883C File Offset: 0x00166A3C
		[MonoPInvokeCallback]
		private static void OnLobbyDeleteImpl(IntPtr ptr, long lobbyId, uint reason)
		{
			Discord discord = (Discord)GCHandle.FromIntPtr(ptr).Target;
			if (discord.LobbyManagerInstance.OnLobbyDelete != null)
			{
				discord.LobbyManagerInstance.OnLobbyDelete(lobbyId, reason);
			}
		}

		// Token: 0x06003698 RID: 13976 RVA: 0x0016887C File Offset: 0x00166A7C
		[MonoPInvokeCallback]
		private static void OnMemberConnectImpl(IntPtr ptr, long lobbyId, long userId)
		{
			Discord discord = (Discord)GCHandle.FromIntPtr(ptr).Target;
			if (discord.LobbyManagerInstance.OnMemberConnect != null)
			{
				discord.LobbyManagerInstance.OnMemberConnect(lobbyId, userId);
			}
		}

		// Token: 0x06003699 RID: 13977 RVA: 0x001688BC File Offset: 0x00166ABC
		[MonoPInvokeCallback]
		private static void OnMemberUpdateImpl(IntPtr ptr, long lobbyId, long userId)
		{
			Discord discord = (Discord)GCHandle.FromIntPtr(ptr).Target;
			if (discord.LobbyManagerInstance.OnMemberUpdate != null)
			{
				discord.LobbyManagerInstance.OnMemberUpdate(lobbyId, userId);
			}
		}

		// Token: 0x0600369A RID: 13978 RVA: 0x001688FC File Offset: 0x00166AFC
		[MonoPInvokeCallback]
		private static void OnMemberDisconnectImpl(IntPtr ptr, long lobbyId, long userId)
		{
			Discord discord = (Discord)GCHandle.FromIntPtr(ptr).Target;
			if (discord.LobbyManagerInstance.OnMemberDisconnect != null)
			{
				discord.LobbyManagerInstance.OnMemberDisconnect(lobbyId, userId);
			}
		}

		// Token: 0x0600369B RID: 13979 RVA: 0x0016893C File Offset: 0x00166B3C
		[MonoPInvokeCallback]
		private static void OnLobbyMessageImpl(IntPtr ptr, long lobbyId, long userId, IntPtr dataPtr, int dataLen)
		{
			Discord discord = (Discord)GCHandle.FromIntPtr(ptr).Target;
			if (discord.LobbyManagerInstance.OnLobbyMessage != null)
			{
				byte[] array = new byte[dataLen];
				Marshal.Copy(dataPtr, array, 0, dataLen);
				discord.LobbyManagerInstance.OnLobbyMessage(lobbyId, userId, array);
			}
		}

		// Token: 0x0600369C RID: 13980 RVA: 0x00168990 File Offset: 0x00166B90
		[MonoPInvokeCallback]
		private static void OnSpeakingImpl(IntPtr ptr, long lobbyId, long userId, bool speaking)
		{
			Discord discord = (Discord)GCHandle.FromIntPtr(ptr).Target;
			if (discord.LobbyManagerInstance.OnSpeaking != null)
			{
				discord.LobbyManagerInstance.OnSpeaking(lobbyId, userId, speaking);
			}
		}

		// Token: 0x0600369D RID: 13981 RVA: 0x001689D4 File Offset: 0x00166BD4
		[MonoPInvokeCallback]
		private static void OnNetworkMessageImpl(IntPtr ptr, long lobbyId, long userId, byte channelId, IntPtr dataPtr, int dataLen)
		{
			Discord discord = (Discord)GCHandle.FromIntPtr(ptr).Target;
			if (discord.LobbyManagerInstance.OnNetworkMessage != null)
			{
				byte[] array = new byte[dataLen];
				Marshal.Copy(dataPtr, array, 0, dataLen);
				discord.LobbyManagerInstance.OnNetworkMessage(lobbyId, userId, channelId, array);
			}
		}

		// Token: 0x0600369E RID: 13982 RVA: 0x00168A2C File Offset: 0x00166C2C
		public IEnumerable<User> GetMemberUsers(long lobbyID)
		{
			int num = this.MemberCount(lobbyID);
			List<User> list = new List<User>();
			for (int i = 0; i < num; i++)
			{
				list.Add(this.GetMemberUser(lobbyID, this.GetMemberUserId(lobbyID, i)));
			}
			return list;
		}

		// Token: 0x0600369F RID: 13983 RVA: 0x00168A69 File Offset: 0x00166C69
		public void SendLobbyMessage(long lobbyID, string data, LobbyManager.SendLobbyMessageHandler handler)
		{
			this.SendLobbyMessage(lobbyID, Encoding.UTF8.GetBytes(data), handler);
		}

		// Token: 0x04002311 RID: 8977
		private IntPtr MethodsPtr;

		// Token: 0x04002312 RID: 8978
		private object MethodsStructure;

		// Token: 0x0200085A RID: 2138
		internal struct FFIEvents
		{
			// Token: 0x04002EF1 RID: 12017
			internal LobbyManager.FFIEvents.LobbyUpdateHandler OnLobbyUpdate;

			// Token: 0x04002EF2 RID: 12018
			internal LobbyManager.FFIEvents.LobbyDeleteHandler OnLobbyDelete;

			// Token: 0x04002EF3 RID: 12019
			internal LobbyManager.FFIEvents.MemberConnectHandler OnMemberConnect;

			// Token: 0x04002EF4 RID: 12020
			internal LobbyManager.FFIEvents.MemberUpdateHandler OnMemberUpdate;

			// Token: 0x04002EF5 RID: 12021
			internal LobbyManager.FFIEvents.MemberDisconnectHandler OnMemberDisconnect;

			// Token: 0x04002EF6 RID: 12022
			internal LobbyManager.FFIEvents.LobbyMessageHandler OnLobbyMessage;

			// Token: 0x04002EF7 RID: 12023
			internal LobbyManager.FFIEvents.SpeakingHandler OnSpeaking;

			// Token: 0x04002EF8 RID: 12024
			internal LobbyManager.FFIEvents.NetworkMessageHandler OnNetworkMessage;

			// Token: 0x020008F3 RID: 2291
			// (Invoke) Token: 0x0600438F RID: 17295
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate void LobbyUpdateHandler(IntPtr ptr, long lobbyId);

			// Token: 0x020008F4 RID: 2292
			// (Invoke) Token: 0x06004393 RID: 17299
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate void LobbyDeleteHandler(IntPtr ptr, long lobbyId, uint reason);

			// Token: 0x020008F5 RID: 2293
			// (Invoke) Token: 0x06004397 RID: 17303
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate void MemberConnectHandler(IntPtr ptr, long lobbyId, long userId);

			// Token: 0x020008F6 RID: 2294
			// (Invoke) Token: 0x0600439B RID: 17307
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate void MemberUpdateHandler(IntPtr ptr, long lobbyId, long userId);

			// Token: 0x020008F7 RID: 2295
			// (Invoke) Token: 0x0600439F RID: 17311
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate void MemberDisconnectHandler(IntPtr ptr, long lobbyId, long userId);

			// Token: 0x020008F8 RID: 2296
			// (Invoke) Token: 0x060043A3 RID: 17315
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate void LobbyMessageHandler(IntPtr ptr, long lobbyId, long userId, IntPtr dataPtr, int dataLen);

			// Token: 0x020008F9 RID: 2297
			// (Invoke) Token: 0x060043A7 RID: 17319
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate void SpeakingHandler(IntPtr ptr, long lobbyId, long userId, bool speaking);

			// Token: 0x020008FA RID: 2298
			// (Invoke) Token: 0x060043AB RID: 17323
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate void NetworkMessageHandler(IntPtr ptr, long lobbyId, long userId, byte channelId, IntPtr dataPtr, int dataLen);
		}

		// Token: 0x0200085B RID: 2139
		internal struct FFIMethods
		{
			// Token: 0x04002EF9 RID: 12025
			internal LobbyManager.FFIMethods.GetLobbyCreateTransactionMethod GetLobbyCreateTransaction;

			// Token: 0x04002EFA RID: 12026
			internal LobbyManager.FFIMethods.GetLobbyUpdateTransactionMethod GetLobbyUpdateTransaction;

			// Token: 0x04002EFB RID: 12027
			internal LobbyManager.FFIMethods.GetMemberUpdateTransactionMethod GetMemberUpdateTransaction;

			// Token: 0x04002EFC RID: 12028
			internal LobbyManager.FFIMethods.CreateLobbyMethod CreateLobby;

			// Token: 0x04002EFD RID: 12029
			internal LobbyManager.FFIMethods.UpdateLobbyMethod UpdateLobby;

			// Token: 0x04002EFE RID: 12030
			internal LobbyManager.FFIMethods.DeleteLobbyMethod DeleteLobby;

			// Token: 0x04002EFF RID: 12031
			internal LobbyManager.FFIMethods.ConnectLobbyMethod ConnectLobby;

			// Token: 0x04002F00 RID: 12032
			internal LobbyManager.FFIMethods.ConnectLobbyWithActivitySecretMethod ConnectLobbyWithActivitySecret;

			// Token: 0x04002F01 RID: 12033
			internal LobbyManager.FFIMethods.DisconnectLobbyMethod DisconnectLobby;

			// Token: 0x04002F02 RID: 12034
			internal LobbyManager.FFIMethods.GetLobbyMethod GetLobby;

			// Token: 0x04002F03 RID: 12035
			internal LobbyManager.FFIMethods.GetLobbyActivitySecretMethod GetLobbyActivitySecret;

			// Token: 0x04002F04 RID: 12036
			internal LobbyManager.FFIMethods.GetLobbyMetadataValueMethod GetLobbyMetadataValue;

			// Token: 0x04002F05 RID: 12037
			internal LobbyManager.FFIMethods.GetLobbyMetadataKeyMethod GetLobbyMetadataKey;

			// Token: 0x04002F06 RID: 12038
			internal LobbyManager.FFIMethods.LobbyMetadataCountMethod LobbyMetadataCount;

			// Token: 0x04002F07 RID: 12039
			internal LobbyManager.FFIMethods.MemberCountMethod MemberCount;

			// Token: 0x04002F08 RID: 12040
			internal LobbyManager.FFIMethods.GetMemberUserIdMethod GetMemberUserId;

			// Token: 0x04002F09 RID: 12041
			internal LobbyManager.FFIMethods.GetMemberUserMethod GetMemberUser;

			// Token: 0x04002F0A RID: 12042
			internal LobbyManager.FFIMethods.GetMemberMetadataValueMethod GetMemberMetadataValue;

			// Token: 0x04002F0B RID: 12043
			internal LobbyManager.FFIMethods.GetMemberMetadataKeyMethod GetMemberMetadataKey;

			// Token: 0x04002F0C RID: 12044
			internal LobbyManager.FFIMethods.MemberMetadataCountMethod MemberMetadataCount;

			// Token: 0x04002F0D RID: 12045
			internal LobbyManager.FFIMethods.UpdateMemberMethod UpdateMember;

			// Token: 0x04002F0E RID: 12046
			internal LobbyManager.FFIMethods.SendLobbyMessageMethod SendLobbyMessage;

			// Token: 0x04002F0F RID: 12047
			internal LobbyManager.FFIMethods.GetSearchQueryMethod GetSearchQuery;

			// Token: 0x04002F10 RID: 12048
			internal LobbyManager.FFIMethods.SearchMethod Search;

			// Token: 0x04002F11 RID: 12049
			internal LobbyManager.FFIMethods.LobbyCountMethod LobbyCount;

			// Token: 0x04002F12 RID: 12050
			internal LobbyManager.FFIMethods.GetLobbyIdMethod GetLobbyId;

			// Token: 0x04002F13 RID: 12051
			internal LobbyManager.FFIMethods.ConnectVoiceMethod ConnectVoice;

			// Token: 0x04002F14 RID: 12052
			internal LobbyManager.FFIMethods.DisconnectVoiceMethod DisconnectVoice;

			// Token: 0x04002F15 RID: 12053
			internal LobbyManager.FFIMethods.ConnectNetworkMethod ConnectNetwork;

			// Token: 0x04002F16 RID: 12054
			internal LobbyManager.FFIMethods.DisconnectNetworkMethod DisconnectNetwork;

			// Token: 0x04002F17 RID: 12055
			internal LobbyManager.FFIMethods.FlushNetworkMethod FlushNetwork;

			// Token: 0x04002F18 RID: 12056
			internal LobbyManager.FFIMethods.OpenNetworkChannelMethod OpenNetworkChannel;

			// Token: 0x04002F19 RID: 12057
			internal LobbyManager.FFIMethods.SendNetworkMessageMethod SendNetworkMessage;

			// Token: 0x020008FB RID: 2299
			// (Invoke) Token: 0x060043AF RID: 17327
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate Result GetLobbyCreateTransactionMethod(IntPtr methodsPtr, ref IntPtr transaction);

			// Token: 0x020008FC RID: 2300
			// (Invoke) Token: 0x060043B3 RID: 17331
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate Result GetLobbyUpdateTransactionMethod(IntPtr methodsPtr, long lobbyId, ref IntPtr transaction);

			// Token: 0x020008FD RID: 2301
			// (Invoke) Token: 0x060043B7 RID: 17335
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate Result GetMemberUpdateTransactionMethod(IntPtr methodsPtr, long lobbyId, long userId, ref IntPtr transaction);

			// Token: 0x020008FE RID: 2302
			// (Invoke) Token: 0x060043BB RID: 17339
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate void CreateLobbyCallback(IntPtr ptr, Result result, ref Lobby lobby);

			// Token: 0x020008FF RID: 2303
			// (Invoke) Token: 0x060043BF RID: 17343
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate void CreateLobbyMethod(IntPtr methodsPtr, IntPtr transaction, IntPtr callbackData, LobbyManager.FFIMethods.CreateLobbyCallback callback);

			// Token: 0x02000900 RID: 2304
			// (Invoke) Token: 0x060043C3 RID: 17347
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate void UpdateLobbyCallback(IntPtr ptr, Result result);

			// Token: 0x02000901 RID: 2305
			// (Invoke) Token: 0x060043C7 RID: 17351
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate void UpdateLobbyMethod(IntPtr methodsPtr, long lobbyId, IntPtr transaction, IntPtr callbackData, LobbyManager.FFIMethods.UpdateLobbyCallback callback);

			// Token: 0x02000902 RID: 2306
			// (Invoke) Token: 0x060043CB RID: 17355
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate void DeleteLobbyCallback(IntPtr ptr, Result result);

			// Token: 0x02000903 RID: 2307
			// (Invoke) Token: 0x060043CF RID: 17359
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate void DeleteLobbyMethod(IntPtr methodsPtr, long lobbyId, IntPtr callbackData, LobbyManager.FFIMethods.DeleteLobbyCallback callback);

			// Token: 0x02000904 RID: 2308
			// (Invoke) Token: 0x060043D3 RID: 17363
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate void ConnectLobbyCallback(IntPtr ptr, Result result, ref Lobby lobby);

			// Token: 0x02000905 RID: 2309
			// (Invoke) Token: 0x060043D7 RID: 17367
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate void ConnectLobbyMethod(IntPtr methodsPtr, long lobbyId, [MarshalAs(UnmanagedType.LPStr)] string secret, IntPtr callbackData, LobbyManager.FFIMethods.ConnectLobbyCallback callback);

			// Token: 0x02000906 RID: 2310
			// (Invoke) Token: 0x060043DB RID: 17371
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate void ConnectLobbyWithActivitySecretCallback(IntPtr ptr, Result result, ref Lobby lobby);

			// Token: 0x02000907 RID: 2311
			// (Invoke) Token: 0x060043DF RID: 17375
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate void ConnectLobbyWithActivitySecretMethod(IntPtr methodsPtr, [MarshalAs(UnmanagedType.LPStr)] string activitySecret, IntPtr callbackData, LobbyManager.FFIMethods.ConnectLobbyWithActivitySecretCallback callback);

			// Token: 0x02000908 RID: 2312
			// (Invoke) Token: 0x060043E3 RID: 17379
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate void DisconnectLobbyCallback(IntPtr ptr, Result result);

			// Token: 0x02000909 RID: 2313
			// (Invoke) Token: 0x060043E7 RID: 17383
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate void DisconnectLobbyMethod(IntPtr methodsPtr, long lobbyId, IntPtr callbackData, LobbyManager.FFIMethods.DisconnectLobbyCallback callback);

			// Token: 0x0200090A RID: 2314
			// (Invoke) Token: 0x060043EB RID: 17387
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate Result GetLobbyMethod(IntPtr methodsPtr, long lobbyId, ref Lobby lobby);

			// Token: 0x0200090B RID: 2315
			// (Invoke) Token: 0x060043EF RID: 17391
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate Result GetLobbyActivitySecretMethod(IntPtr methodsPtr, long lobbyId, StringBuilder secret);

			// Token: 0x0200090C RID: 2316
			// (Invoke) Token: 0x060043F3 RID: 17395
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate Result GetLobbyMetadataValueMethod(IntPtr methodsPtr, long lobbyId, [MarshalAs(UnmanagedType.LPStr)] string key, StringBuilder value);

			// Token: 0x0200090D RID: 2317
			// (Invoke) Token: 0x060043F7 RID: 17399
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate Result GetLobbyMetadataKeyMethod(IntPtr methodsPtr, long lobbyId, int index, StringBuilder key);

			// Token: 0x0200090E RID: 2318
			// (Invoke) Token: 0x060043FB RID: 17403
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate Result LobbyMetadataCountMethod(IntPtr methodsPtr, long lobbyId, ref int count);

			// Token: 0x0200090F RID: 2319
			// (Invoke) Token: 0x060043FF RID: 17407
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate Result MemberCountMethod(IntPtr methodsPtr, long lobbyId, ref int count);

			// Token: 0x02000910 RID: 2320
			// (Invoke) Token: 0x06004403 RID: 17411
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate Result GetMemberUserIdMethod(IntPtr methodsPtr, long lobbyId, int index, ref long userId);

			// Token: 0x02000911 RID: 2321
			// (Invoke) Token: 0x06004407 RID: 17415
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate Result GetMemberUserMethod(IntPtr methodsPtr, long lobbyId, long userId, ref User user);

			// Token: 0x02000912 RID: 2322
			// (Invoke) Token: 0x0600440B RID: 17419
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate Result GetMemberMetadataValueMethod(IntPtr methodsPtr, long lobbyId, long userId, [MarshalAs(UnmanagedType.LPStr)] string key, StringBuilder value);

			// Token: 0x02000913 RID: 2323
			// (Invoke) Token: 0x0600440F RID: 17423
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate Result GetMemberMetadataKeyMethod(IntPtr methodsPtr, long lobbyId, long userId, int index, StringBuilder key);

			// Token: 0x02000914 RID: 2324
			// (Invoke) Token: 0x06004413 RID: 17427
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate Result MemberMetadataCountMethod(IntPtr methodsPtr, long lobbyId, long userId, ref int count);

			// Token: 0x02000915 RID: 2325
			// (Invoke) Token: 0x06004417 RID: 17431
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate void UpdateMemberCallback(IntPtr ptr, Result result);

			// Token: 0x02000916 RID: 2326
			// (Invoke) Token: 0x0600441B RID: 17435
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate void UpdateMemberMethod(IntPtr methodsPtr, long lobbyId, long userId, IntPtr transaction, IntPtr callbackData, LobbyManager.FFIMethods.UpdateMemberCallback callback);

			// Token: 0x02000917 RID: 2327
			// (Invoke) Token: 0x0600441F RID: 17439
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate void SendLobbyMessageCallback(IntPtr ptr, Result result);

			// Token: 0x02000918 RID: 2328
			// (Invoke) Token: 0x06004423 RID: 17443
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate void SendLobbyMessageMethod(IntPtr methodsPtr, long lobbyId, byte[] data, int dataLen, IntPtr callbackData, LobbyManager.FFIMethods.SendLobbyMessageCallback callback);

			// Token: 0x02000919 RID: 2329
			// (Invoke) Token: 0x06004427 RID: 17447
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate Result GetSearchQueryMethod(IntPtr methodsPtr, ref IntPtr query);

			// Token: 0x0200091A RID: 2330
			// (Invoke) Token: 0x0600442B RID: 17451
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate void SearchCallback(IntPtr ptr, Result result);

			// Token: 0x0200091B RID: 2331
			// (Invoke) Token: 0x0600442F RID: 17455
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate void SearchMethod(IntPtr methodsPtr, IntPtr query, IntPtr callbackData, LobbyManager.FFIMethods.SearchCallback callback);

			// Token: 0x0200091C RID: 2332
			// (Invoke) Token: 0x06004433 RID: 17459
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate void LobbyCountMethod(IntPtr methodsPtr, ref int count);

			// Token: 0x0200091D RID: 2333
			// (Invoke) Token: 0x06004437 RID: 17463
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate Result GetLobbyIdMethod(IntPtr methodsPtr, int index, ref long lobbyId);

			// Token: 0x0200091E RID: 2334
			// (Invoke) Token: 0x0600443B RID: 17467
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate void ConnectVoiceCallback(IntPtr ptr, Result result);

			// Token: 0x0200091F RID: 2335
			// (Invoke) Token: 0x0600443F RID: 17471
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate void ConnectVoiceMethod(IntPtr methodsPtr, long lobbyId, IntPtr callbackData, LobbyManager.FFIMethods.ConnectVoiceCallback callback);

			// Token: 0x02000920 RID: 2336
			// (Invoke) Token: 0x06004443 RID: 17475
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate void DisconnectVoiceCallback(IntPtr ptr, Result result);

			// Token: 0x02000921 RID: 2337
			// (Invoke) Token: 0x06004447 RID: 17479
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate void DisconnectVoiceMethod(IntPtr methodsPtr, long lobbyId, IntPtr callbackData, LobbyManager.FFIMethods.DisconnectVoiceCallback callback);

			// Token: 0x02000922 RID: 2338
			// (Invoke) Token: 0x0600444B RID: 17483
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate Result ConnectNetworkMethod(IntPtr methodsPtr, long lobbyId);

			// Token: 0x02000923 RID: 2339
			// (Invoke) Token: 0x0600444F RID: 17487
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate Result DisconnectNetworkMethod(IntPtr methodsPtr, long lobbyId);

			// Token: 0x02000924 RID: 2340
			// (Invoke) Token: 0x06004453 RID: 17491
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate Result FlushNetworkMethod(IntPtr methodsPtr);

			// Token: 0x02000925 RID: 2341
			// (Invoke) Token: 0x06004457 RID: 17495
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate Result OpenNetworkChannelMethod(IntPtr methodsPtr, long lobbyId, byte channelId, bool reliable);

			// Token: 0x02000926 RID: 2342
			// (Invoke) Token: 0x0600445B RID: 17499
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate Result SendNetworkMessageMethod(IntPtr methodsPtr, long lobbyId, long userId, byte channelId, byte[] data, int dataLen);
		}

		// Token: 0x0200085C RID: 2140
		// (Invoke) Token: 0x060041BA RID: 16826
		public delegate void CreateLobbyHandler(Result result, ref Lobby lobby);

		// Token: 0x0200085D RID: 2141
		// (Invoke) Token: 0x060041BE RID: 16830
		public delegate void UpdateLobbyHandler(Result result);

		// Token: 0x0200085E RID: 2142
		// (Invoke) Token: 0x060041C2 RID: 16834
		public delegate void DeleteLobbyHandler(Result result);

		// Token: 0x0200085F RID: 2143
		// (Invoke) Token: 0x060041C6 RID: 16838
		public delegate void ConnectLobbyHandler(Result result, ref Lobby lobby);

		// Token: 0x02000860 RID: 2144
		// (Invoke) Token: 0x060041CA RID: 16842
		public delegate void ConnectLobbyWithActivitySecretHandler(Result result, ref Lobby lobby);

		// Token: 0x02000861 RID: 2145
		// (Invoke) Token: 0x060041CE RID: 16846
		public delegate void DisconnectLobbyHandler(Result result);

		// Token: 0x02000862 RID: 2146
		// (Invoke) Token: 0x060041D2 RID: 16850
		public delegate void UpdateMemberHandler(Result result);

		// Token: 0x02000863 RID: 2147
		// (Invoke) Token: 0x060041D6 RID: 16854
		public delegate void SendLobbyMessageHandler(Result result);

		// Token: 0x02000864 RID: 2148
		// (Invoke) Token: 0x060041DA RID: 16858
		public delegate void SearchHandler(Result result);

		// Token: 0x02000865 RID: 2149
		// (Invoke) Token: 0x060041DE RID: 16862
		public delegate void ConnectVoiceHandler(Result result);

		// Token: 0x02000866 RID: 2150
		// (Invoke) Token: 0x060041E2 RID: 16866
		public delegate void DisconnectVoiceHandler(Result result);

		// Token: 0x02000867 RID: 2151
		// (Invoke) Token: 0x060041E6 RID: 16870
		public delegate void LobbyUpdateHandler(long lobbyId);

		// Token: 0x02000868 RID: 2152
		// (Invoke) Token: 0x060041EA RID: 16874
		public delegate void LobbyDeleteHandler(long lobbyId, uint reason);

		// Token: 0x02000869 RID: 2153
		// (Invoke) Token: 0x060041EE RID: 16878
		public delegate void MemberConnectHandler(long lobbyId, long userId);

		// Token: 0x0200086A RID: 2154
		// (Invoke) Token: 0x060041F2 RID: 16882
		public delegate void MemberUpdateHandler(long lobbyId, long userId);

		// Token: 0x0200086B RID: 2155
		// (Invoke) Token: 0x060041F6 RID: 16886
		public delegate void MemberDisconnectHandler(long lobbyId, long userId);

		// Token: 0x0200086C RID: 2156
		// (Invoke) Token: 0x060041FA RID: 16890
		public delegate void LobbyMessageHandler(long lobbyId, long userId, byte[] data);

		// Token: 0x0200086D RID: 2157
		// (Invoke) Token: 0x060041FE RID: 16894
		public delegate void SpeakingHandler(long lobbyId, long userId, bool speaking);

		// Token: 0x0200086E RID: 2158
		// (Invoke) Token: 0x06004202 RID: 16898
		public delegate void NetworkMessageHandler(long lobbyId, long userId, byte channelId, byte[] data);
	}
}
