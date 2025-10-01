using System;
using System.Runtime.InteropServices;

namespace Discord
{
	// Token: 0x020004E7 RID: 1255
	public class Discord : IDisposable
	{
		// Token: 0x06003614 RID: 13844
		[DllImport("discord_game_sdk", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
		private static extern Result DiscordCreate(uint version, ref Discord.FFICreateParams createParams, out IntPtr manager);

		// Token: 0x17000744 RID: 1860
		// (get) Token: 0x06003615 RID: 13845 RVA: 0x0016676A File Offset: 0x0016496A
		private Discord.FFIMethods Methods
		{
			get
			{
				if (this.MethodsStructure == null)
				{
					this.MethodsStructure = Marshal.PtrToStructure(this.MethodsPtr, typeof(Discord.FFIMethods));
				}
				return (Discord.FFIMethods)this.MethodsStructure;
			}
		}

		// Token: 0x06003616 RID: 13846 RVA: 0x0016679C File Offset: 0x0016499C
		public Discord(long clientId, ulong flags)
		{
			Discord.FFICreateParams fficreateParams;
			fficreateParams.ClientId = clientId;
			fficreateParams.Flags = flags;
			this.Events = default(Discord.FFIEvents);
			this.EventsPtr = Marshal.AllocHGlobal(Marshal.SizeOf<Discord.FFIEvents>(this.Events));
			fficreateParams.Events = this.EventsPtr;
			this.SelfHandle = GCHandle.Alloc(this);
			fficreateParams.EventData = GCHandle.ToIntPtr(this.SelfHandle);
			this.ApplicationEvents = default(ApplicationManager.FFIEvents);
			this.ApplicationEventsPtr = Marshal.AllocHGlobal(Marshal.SizeOf<ApplicationManager.FFIEvents>(this.ApplicationEvents));
			fficreateParams.ApplicationEvents = this.ApplicationEventsPtr;
			fficreateParams.ApplicationVersion = 1U;
			this.UserEvents = default(UserManager.FFIEvents);
			this.UserEventsPtr = Marshal.AllocHGlobal(Marshal.SizeOf<UserManager.FFIEvents>(this.UserEvents));
			fficreateParams.UserEvents = this.UserEventsPtr;
			fficreateParams.UserVersion = 1U;
			this.ImageEvents = default(ImageManager.FFIEvents);
			this.ImageEventsPtr = Marshal.AllocHGlobal(Marshal.SizeOf<ImageManager.FFIEvents>(this.ImageEvents));
			fficreateParams.ImageEvents = this.ImageEventsPtr;
			fficreateParams.ImageVersion = 1U;
			this.ActivityEvents = default(ActivityManager.FFIEvents);
			this.ActivityEventsPtr = Marshal.AllocHGlobal(Marshal.SizeOf<ActivityManager.FFIEvents>(this.ActivityEvents));
			fficreateParams.ActivityEvents = this.ActivityEventsPtr;
			fficreateParams.ActivityVersion = 1U;
			this.RelationshipEvents = default(RelationshipManager.FFIEvents);
			this.RelationshipEventsPtr = Marshal.AllocHGlobal(Marshal.SizeOf<RelationshipManager.FFIEvents>(this.RelationshipEvents));
			fficreateParams.RelationshipEvents = this.RelationshipEventsPtr;
			fficreateParams.RelationshipVersion = 1U;
			this.LobbyEvents = default(LobbyManager.FFIEvents);
			this.LobbyEventsPtr = Marshal.AllocHGlobal(Marshal.SizeOf<LobbyManager.FFIEvents>(this.LobbyEvents));
			fficreateParams.LobbyEvents = this.LobbyEventsPtr;
			fficreateParams.LobbyVersion = 1U;
			this.NetworkEvents = default(NetworkManager.FFIEvents);
			this.NetworkEventsPtr = Marshal.AllocHGlobal(Marshal.SizeOf<NetworkManager.FFIEvents>(this.NetworkEvents));
			fficreateParams.NetworkEvents = this.NetworkEventsPtr;
			fficreateParams.NetworkVersion = 1U;
			this.OverlayEvents = default(OverlayManager.FFIEvents);
			this.OverlayEventsPtr = Marshal.AllocHGlobal(Marshal.SizeOf<OverlayManager.FFIEvents>(this.OverlayEvents));
			fficreateParams.OverlayEvents = this.OverlayEventsPtr;
			fficreateParams.OverlayVersion = 1U;
			this.StorageEvents = default(StorageManager.FFIEvents);
			this.StorageEventsPtr = Marshal.AllocHGlobal(Marshal.SizeOf<StorageManager.FFIEvents>(this.StorageEvents));
			fficreateParams.StorageEvents = this.StorageEventsPtr;
			fficreateParams.StorageVersion = 1U;
			this.StoreEvents = default(StoreManager.FFIEvents);
			this.StoreEventsPtr = Marshal.AllocHGlobal(Marshal.SizeOf<StoreManager.FFIEvents>(this.StoreEvents));
			fficreateParams.StoreEvents = this.StoreEventsPtr;
			fficreateParams.StoreVersion = 1U;
			this.VoiceEvents = default(VoiceManager.FFIEvents);
			this.VoiceEventsPtr = Marshal.AllocHGlobal(Marshal.SizeOf<VoiceManager.FFIEvents>(this.VoiceEvents));
			fficreateParams.VoiceEvents = this.VoiceEventsPtr;
			fficreateParams.VoiceVersion = 1U;
			this.AchievementEvents = default(AchievementManager.FFIEvents);
			this.AchievementEventsPtr = Marshal.AllocHGlobal(Marshal.SizeOf<AchievementManager.FFIEvents>(this.AchievementEvents));
			fficreateParams.AchievementEvents = this.AchievementEventsPtr;
			fficreateParams.AchievementVersion = 1U;
			this.InitEvents(this.EventsPtr, ref this.Events);
			Result result = Discord.DiscordCreate(2U, ref fficreateParams, out this.MethodsPtr);
			if (result != Result.Ok)
			{
				this.Dispose();
				throw new ResultException(result);
			}
		}

		// Token: 0x06003617 RID: 13847 RVA: 0x00166AD1 File Offset: 0x00164CD1
		private void InitEvents(IntPtr eventsPtr, ref Discord.FFIEvents events)
		{
			Marshal.StructureToPtr<Discord.FFIEvents>(events, eventsPtr, false);
		}

		// Token: 0x06003618 RID: 13848 RVA: 0x00166AE0 File Offset: 0x00164CE0
		public void Dispose()
		{
			if (this.MethodsPtr != IntPtr.Zero)
			{
				this.Methods.Destroy(this.MethodsPtr);
			}
			this.SelfHandle.Free();
			Marshal.FreeHGlobal(this.EventsPtr);
			Marshal.FreeHGlobal(this.ApplicationEventsPtr);
			Marshal.FreeHGlobal(this.UserEventsPtr);
			Marshal.FreeHGlobal(this.ImageEventsPtr);
			Marshal.FreeHGlobal(this.ActivityEventsPtr);
			Marshal.FreeHGlobal(this.RelationshipEventsPtr);
			Marshal.FreeHGlobal(this.LobbyEventsPtr);
			Marshal.FreeHGlobal(this.NetworkEventsPtr);
			Marshal.FreeHGlobal(this.OverlayEventsPtr);
			Marshal.FreeHGlobal(this.StorageEventsPtr);
			Marshal.FreeHGlobal(this.StoreEventsPtr);
			Marshal.FreeHGlobal(this.VoiceEventsPtr);
			Marshal.FreeHGlobal(this.AchievementEventsPtr);
			if (this.setLogHook != null)
			{
				this.setLogHook.Value.Free();
			}
		}

		// Token: 0x06003619 RID: 13849 RVA: 0x00166BD0 File Offset: 0x00164DD0
		public void RunCallbacks()
		{
			Result result = this.Methods.RunCallbacks(this.MethodsPtr);
			if (result != Result.Ok)
			{
				throw new ResultException(result);
			}
		}

		// Token: 0x0600361A RID: 13850 RVA: 0x00166C00 File Offset: 0x00164E00
		[MonoPInvokeCallback]
		private static void SetLogHookCallbackImpl(IntPtr ptr, LogLevel level, string message)
		{
			((Discord.SetLogHookHandler)GCHandle.FromIntPtr(ptr).Target)(level, message);
		}

		// Token: 0x0600361B RID: 13851 RVA: 0x00166C28 File Offset: 0x00164E28
		public void SetLogHook(LogLevel minLevel, Discord.SetLogHookHandler callback)
		{
			if (this.setLogHook != null)
			{
				this.setLogHook.Value.Free();
			}
			this.setLogHook = new GCHandle?(GCHandle.Alloc(callback));
			this.Methods.SetLogHook(this.MethodsPtr, minLevel, GCHandle.ToIntPtr(this.setLogHook.Value), new Discord.FFIMethods.SetLogHookCallback(Discord.SetLogHookCallbackImpl));
		}

		// Token: 0x0600361C RID: 13852 RVA: 0x00166C99 File Offset: 0x00164E99
		public ApplicationManager GetApplicationManager()
		{
			if (this.ApplicationManagerInstance == null)
			{
				this.ApplicationManagerInstance = new ApplicationManager(this.Methods.GetApplicationManager(this.MethodsPtr), this.ApplicationEventsPtr, ref this.ApplicationEvents);
			}
			return this.ApplicationManagerInstance;
		}

		// Token: 0x0600361D RID: 13853 RVA: 0x00166CD6 File Offset: 0x00164ED6
		public UserManager GetUserManager()
		{
			if (this.UserManagerInstance == null)
			{
				this.UserManagerInstance = new UserManager(this.Methods.GetUserManager(this.MethodsPtr), this.UserEventsPtr, ref this.UserEvents);
			}
			return this.UserManagerInstance;
		}

		// Token: 0x0600361E RID: 13854 RVA: 0x00166D13 File Offset: 0x00164F13
		public ImageManager GetImageManager()
		{
			if (this.ImageManagerInstance == null)
			{
				this.ImageManagerInstance = new ImageManager(this.Methods.GetImageManager(this.MethodsPtr), this.ImageEventsPtr, ref this.ImageEvents);
			}
			return this.ImageManagerInstance;
		}

		// Token: 0x0600361F RID: 13855 RVA: 0x00166D50 File Offset: 0x00164F50
		public ActivityManager GetActivityManager()
		{
			if (this.ActivityManagerInstance == null)
			{
				this.ActivityManagerInstance = new ActivityManager(this.Methods.GetActivityManager(this.MethodsPtr), this.ActivityEventsPtr, ref this.ActivityEvents);
			}
			return this.ActivityManagerInstance;
		}

		// Token: 0x06003620 RID: 13856 RVA: 0x00166D8D File Offset: 0x00164F8D
		public RelationshipManager GetRelationshipManager()
		{
			if (this.RelationshipManagerInstance == null)
			{
				this.RelationshipManagerInstance = new RelationshipManager(this.Methods.GetRelationshipManager(this.MethodsPtr), this.RelationshipEventsPtr, ref this.RelationshipEvents);
			}
			return this.RelationshipManagerInstance;
		}

		// Token: 0x06003621 RID: 13857 RVA: 0x00166DCA File Offset: 0x00164FCA
		public LobbyManager GetLobbyManager()
		{
			if (this.LobbyManagerInstance == null)
			{
				this.LobbyManagerInstance = new LobbyManager(this.Methods.GetLobbyManager(this.MethodsPtr), this.LobbyEventsPtr, ref this.LobbyEvents);
			}
			return this.LobbyManagerInstance;
		}

		// Token: 0x06003622 RID: 13858 RVA: 0x00166E07 File Offset: 0x00165007
		public NetworkManager GetNetworkManager()
		{
			if (this.NetworkManagerInstance == null)
			{
				this.NetworkManagerInstance = new NetworkManager(this.Methods.GetNetworkManager(this.MethodsPtr), this.NetworkEventsPtr, ref this.NetworkEvents);
			}
			return this.NetworkManagerInstance;
		}

		// Token: 0x06003623 RID: 13859 RVA: 0x00166E44 File Offset: 0x00165044
		public OverlayManager GetOverlayManager()
		{
			if (this.OverlayManagerInstance == null)
			{
				this.OverlayManagerInstance = new OverlayManager(this.Methods.GetOverlayManager(this.MethodsPtr), this.OverlayEventsPtr, ref this.OverlayEvents);
			}
			return this.OverlayManagerInstance;
		}

		// Token: 0x06003624 RID: 13860 RVA: 0x00166E81 File Offset: 0x00165081
		public StorageManager GetStorageManager()
		{
			if (this.StorageManagerInstance == null)
			{
				this.StorageManagerInstance = new StorageManager(this.Methods.GetStorageManager(this.MethodsPtr), this.StorageEventsPtr, ref this.StorageEvents);
			}
			return this.StorageManagerInstance;
		}

		// Token: 0x06003625 RID: 13861 RVA: 0x00166EBE File Offset: 0x001650BE
		public StoreManager GetStoreManager()
		{
			if (this.StoreManagerInstance == null)
			{
				this.StoreManagerInstance = new StoreManager(this.Methods.GetStoreManager(this.MethodsPtr), this.StoreEventsPtr, ref this.StoreEvents);
			}
			return this.StoreManagerInstance;
		}

		// Token: 0x06003626 RID: 13862 RVA: 0x00166EFB File Offset: 0x001650FB
		public VoiceManager GetVoiceManager()
		{
			if (this.VoiceManagerInstance == null)
			{
				this.VoiceManagerInstance = new VoiceManager(this.Methods.GetVoiceManager(this.MethodsPtr), this.VoiceEventsPtr, ref this.VoiceEvents);
			}
			return this.VoiceManagerInstance;
		}

		// Token: 0x06003627 RID: 13863 RVA: 0x00166F38 File Offset: 0x00165138
		public AchievementManager GetAchievementManager()
		{
			if (this.AchievementManagerInstance == null)
			{
				this.AchievementManagerInstance = new AchievementManager(this.Methods.GetAchievementManager(this.MethodsPtr), this.AchievementEventsPtr, ref this.AchievementEvents);
			}
			return this.AchievementManagerInstance;
		}

		// Token: 0x040022DC RID: 8924
		private GCHandle SelfHandle;

		// Token: 0x040022DD RID: 8925
		private IntPtr EventsPtr;

		// Token: 0x040022DE RID: 8926
		private Discord.FFIEvents Events;

		// Token: 0x040022DF RID: 8927
		private IntPtr ApplicationEventsPtr;

		// Token: 0x040022E0 RID: 8928
		private ApplicationManager.FFIEvents ApplicationEvents;

		// Token: 0x040022E1 RID: 8929
		internal ApplicationManager ApplicationManagerInstance;

		// Token: 0x040022E2 RID: 8930
		private IntPtr UserEventsPtr;

		// Token: 0x040022E3 RID: 8931
		private UserManager.FFIEvents UserEvents;

		// Token: 0x040022E4 RID: 8932
		internal UserManager UserManagerInstance;

		// Token: 0x040022E5 RID: 8933
		private IntPtr ImageEventsPtr;

		// Token: 0x040022E6 RID: 8934
		private ImageManager.FFIEvents ImageEvents;

		// Token: 0x040022E7 RID: 8935
		internal ImageManager ImageManagerInstance;

		// Token: 0x040022E8 RID: 8936
		private IntPtr ActivityEventsPtr;

		// Token: 0x040022E9 RID: 8937
		private ActivityManager.FFIEvents ActivityEvents;

		// Token: 0x040022EA RID: 8938
		internal ActivityManager ActivityManagerInstance;

		// Token: 0x040022EB RID: 8939
		private IntPtr RelationshipEventsPtr;

		// Token: 0x040022EC RID: 8940
		private RelationshipManager.FFIEvents RelationshipEvents;

		// Token: 0x040022ED RID: 8941
		internal RelationshipManager RelationshipManagerInstance;

		// Token: 0x040022EE RID: 8942
		private IntPtr LobbyEventsPtr;

		// Token: 0x040022EF RID: 8943
		private LobbyManager.FFIEvents LobbyEvents;

		// Token: 0x040022F0 RID: 8944
		internal LobbyManager LobbyManagerInstance;

		// Token: 0x040022F1 RID: 8945
		private IntPtr NetworkEventsPtr;

		// Token: 0x040022F2 RID: 8946
		private NetworkManager.FFIEvents NetworkEvents;

		// Token: 0x040022F3 RID: 8947
		internal NetworkManager NetworkManagerInstance;

		// Token: 0x040022F4 RID: 8948
		private IntPtr OverlayEventsPtr;

		// Token: 0x040022F5 RID: 8949
		private OverlayManager.FFIEvents OverlayEvents;

		// Token: 0x040022F6 RID: 8950
		internal OverlayManager OverlayManagerInstance;

		// Token: 0x040022F7 RID: 8951
		private IntPtr StorageEventsPtr;

		// Token: 0x040022F8 RID: 8952
		private StorageManager.FFIEvents StorageEvents;

		// Token: 0x040022F9 RID: 8953
		internal StorageManager StorageManagerInstance;

		// Token: 0x040022FA RID: 8954
		private IntPtr StoreEventsPtr;

		// Token: 0x040022FB RID: 8955
		private StoreManager.FFIEvents StoreEvents;

		// Token: 0x040022FC RID: 8956
		internal StoreManager StoreManagerInstance;

		// Token: 0x040022FD RID: 8957
		private IntPtr VoiceEventsPtr;

		// Token: 0x040022FE RID: 8958
		private VoiceManager.FFIEvents VoiceEvents;

		// Token: 0x040022FF RID: 8959
		internal VoiceManager VoiceManagerInstance;

		// Token: 0x04002300 RID: 8960
		private IntPtr AchievementEventsPtr;

		// Token: 0x04002301 RID: 8961
		private AchievementManager.FFIEvents AchievementEvents;

		// Token: 0x04002302 RID: 8962
		internal AchievementManager AchievementManagerInstance;

		// Token: 0x04002303 RID: 8963
		private IntPtr MethodsPtr;

		// Token: 0x04002304 RID: 8964
		private object MethodsStructure;

		// Token: 0x04002305 RID: 8965
		private GCHandle? setLogHook;

		// Token: 0x02000845 RID: 2117
		internal struct FFIEvents
		{
		}

		// Token: 0x02000846 RID: 2118
		internal struct FFIMethods
		{
			// Token: 0x04002EB3 RID: 11955
			internal Discord.FFIMethods.DestroyHandler Destroy;

			// Token: 0x04002EB4 RID: 11956
			internal Discord.FFIMethods.RunCallbacksMethod RunCallbacks;

			// Token: 0x04002EB5 RID: 11957
			internal Discord.FFIMethods.SetLogHookMethod SetLogHook;

			// Token: 0x04002EB6 RID: 11958
			internal Discord.FFIMethods.GetApplicationManagerMethod GetApplicationManager;

			// Token: 0x04002EB7 RID: 11959
			internal Discord.FFIMethods.GetUserManagerMethod GetUserManager;

			// Token: 0x04002EB8 RID: 11960
			internal Discord.FFIMethods.GetImageManagerMethod GetImageManager;

			// Token: 0x04002EB9 RID: 11961
			internal Discord.FFIMethods.GetActivityManagerMethod GetActivityManager;

			// Token: 0x04002EBA RID: 11962
			internal Discord.FFIMethods.GetRelationshipManagerMethod GetRelationshipManager;

			// Token: 0x04002EBB RID: 11963
			internal Discord.FFIMethods.GetLobbyManagerMethod GetLobbyManager;

			// Token: 0x04002EBC RID: 11964
			internal Discord.FFIMethods.GetNetworkManagerMethod GetNetworkManager;

			// Token: 0x04002EBD RID: 11965
			internal Discord.FFIMethods.GetOverlayManagerMethod GetOverlayManager;

			// Token: 0x04002EBE RID: 11966
			internal Discord.FFIMethods.GetStorageManagerMethod GetStorageManager;

			// Token: 0x04002EBF RID: 11967
			internal Discord.FFIMethods.GetStoreManagerMethod GetStoreManager;

			// Token: 0x04002EC0 RID: 11968
			internal Discord.FFIMethods.GetVoiceManagerMethod GetVoiceManager;

			// Token: 0x04002EC1 RID: 11969
			internal Discord.FFIMethods.GetAchievementManagerMethod GetAchievementManager;

			// Token: 0x020008CA RID: 2250
			// (Invoke) Token: 0x060042EB RID: 17131
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate void DestroyHandler(IntPtr MethodsPtr);

			// Token: 0x020008CB RID: 2251
			// (Invoke) Token: 0x060042EF RID: 17135
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate Result RunCallbacksMethod(IntPtr methodsPtr);

			// Token: 0x020008CC RID: 2252
			// (Invoke) Token: 0x060042F3 RID: 17139
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate void SetLogHookCallback(IntPtr ptr, LogLevel level, [MarshalAs(UnmanagedType.LPStr)] string message);

			// Token: 0x020008CD RID: 2253
			// (Invoke) Token: 0x060042F7 RID: 17143
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate void SetLogHookMethod(IntPtr methodsPtr, LogLevel minLevel, IntPtr callbackData, Discord.FFIMethods.SetLogHookCallback callback);

			// Token: 0x020008CE RID: 2254
			// (Invoke) Token: 0x060042FB RID: 17147
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate IntPtr GetApplicationManagerMethod(IntPtr discordPtr);

			// Token: 0x020008CF RID: 2255
			// (Invoke) Token: 0x060042FF RID: 17151
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate IntPtr GetUserManagerMethod(IntPtr discordPtr);

			// Token: 0x020008D0 RID: 2256
			// (Invoke) Token: 0x06004303 RID: 17155
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate IntPtr GetImageManagerMethod(IntPtr discordPtr);

			// Token: 0x020008D1 RID: 2257
			// (Invoke) Token: 0x06004307 RID: 17159
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate IntPtr GetActivityManagerMethod(IntPtr discordPtr);

			// Token: 0x020008D2 RID: 2258
			// (Invoke) Token: 0x0600430B RID: 17163
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate IntPtr GetRelationshipManagerMethod(IntPtr discordPtr);

			// Token: 0x020008D3 RID: 2259
			// (Invoke) Token: 0x0600430F RID: 17167
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate IntPtr GetLobbyManagerMethod(IntPtr discordPtr);

			// Token: 0x020008D4 RID: 2260
			// (Invoke) Token: 0x06004313 RID: 17171
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate IntPtr GetNetworkManagerMethod(IntPtr discordPtr);

			// Token: 0x020008D5 RID: 2261
			// (Invoke) Token: 0x06004317 RID: 17175
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate IntPtr GetOverlayManagerMethod(IntPtr discordPtr);

			// Token: 0x020008D6 RID: 2262
			// (Invoke) Token: 0x0600431B RID: 17179
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate IntPtr GetStorageManagerMethod(IntPtr discordPtr);

			// Token: 0x020008D7 RID: 2263
			// (Invoke) Token: 0x0600431F RID: 17183
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate IntPtr GetStoreManagerMethod(IntPtr discordPtr);

			// Token: 0x020008D8 RID: 2264
			// (Invoke) Token: 0x06004323 RID: 17187
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate IntPtr GetVoiceManagerMethod(IntPtr discordPtr);

			// Token: 0x020008D9 RID: 2265
			// (Invoke) Token: 0x06004327 RID: 17191
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate IntPtr GetAchievementManagerMethod(IntPtr discordPtr);
		}

		// Token: 0x02000847 RID: 2119
		internal struct FFICreateParams
		{
			// Token: 0x04002EC2 RID: 11970
			internal long ClientId;

			// Token: 0x04002EC3 RID: 11971
			internal ulong Flags;

			// Token: 0x04002EC4 RID: 11972
			internal IntPtr Events;

			// Token: 0x04002EC5 RID: 11973
			internal IntPtr EventData;

			// Token: 0x04002EC6 RID: 11974
			internal IntPtr ApplicationEvents;

			// Token: 0x04002EC7 RID: 11975
			internal uint ApplicationVersion;

			// Token: 0x04002EC8 RID: 11976
			internal IntPtr UserEvents;

			// Token: 0x04002EC9 RID: 11977
			internal uint UserVersion;

			// Token: 0x04002ECA RID: 11978
			internal IntPtr ImageEvents;

			// Token: 0x04002ECB RID: 11979
			internal uint ImageVersion;

			// Token: 0x04002ECC RID: 11980
			internal IntPtr ActivityEvents;

			// Token: 0x04002ECD RID: 11981
			internal uint ActivityVersion;

			// Token: 0x04002ECE RID: 11982
			internal IntPtr RelationshipEvents;

			// Token: 0x04002ECF RID: 11983
			internal uint RelationshipVersion;

			// Token: 0x04002ED0 RID: 11984
			internal IntPtr LobbyEvents;

			// Token: 0x04002ED1 RID: 11985
			internal uint LobbyVersion;

			// Token: 0x04002ED2 RID: 11986
			internal IntPtr NetworkEvents;

			// Token: 0x04002ED3 RID: 11987
			internal uint NetworkVersion;

			// Token: 0x04002ED4 RID: 11988
			internal IntPtr OverlayEvents;

			// Token: 0x04002ED5 RID: 11989
			internal uint OverlayVersion;

			// Token: 0x04002ED6 RID: 11990
			internal IntPtr StorageEvents;

			// Token: 0x04002ED7 RID: 11991
			internal uint StorageVersion;

			// Token: 0x04002ED8 RID: 11992
			internal IntPtr StoreEvents;

			// Token: 0x04002ED9 RID: 11993
			internal uint StoreVersion;

			// Token: 0x04002EDA RID: 11994
			internal IntPtr VoiceEvents;

			// Token: 0x04002EDB RID: 11995
			internal uint VoiceVersion;

			// Token: 0x04002EDC RID: 11996
			internal IntPtr AchievementEvents;

			// Token: 0x04002EDD RID: 11997
			internal uint AchievementVersion;
		}

		// Token: 0x02000848 RID: 2120
		// (Invoke) Token: 0x06004192 RID: 16786
		public delegate void SetLogHookHandler(LogLevel level, string message);
	}
}
