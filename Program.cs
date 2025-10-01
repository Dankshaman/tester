using System;
using System.Linq;
using System.Text;
using System.Threading;
using Discord;

// Token: 0x02000002 RID: 2
internal class Program
{
	// Token: 0x06000001 RID: 1 RVA: 0x00002050 File Offset: 0x00000250
	private static void FetchAvatar(ImageManager imageManager, long userID)
	{
		imageManager.Fetch(ImageHandle.User(userID), delegate(Discord.Result result, ImageHandle handle)
		{
			if (result == Discord.Result.Ok)
			{
				byte[] data = imageManager.GetData(handle);
				Console.WriteLine("image updated {0} {1}", handle.Id, data.Length);
				return;
			}
			Console.WriteLine("image error {0}", handle.Id);
		});
	}

	// Token: 0x06000002 RID: 2 RVA: 0x00002088 File Offset: 0x00000288
	private static void UpdateActivity(Discord discord, Lobby lobby)
	{
		ActivityManager activityManager = discord.GetActivityManager();
		LobbyManager lobbyManager = discord.GetLobbyManager();
		Activity activity = default(Activity);
		activity.State = "olleh";
		activity.Details = "foo details";
		activity.Timestamps.Start = 5L;
		activity.Timestamps.End = 6L;
		activity.Assets.LargeImage = "foo largeImageKey";
		activity.Assets.LargeText = "foo largeImageText";
		activity.Assets.SmallImage = "foo smallImageKey";
		activity.Assets.SmallText = "foo smallImageText";
		activity.Party.Id = lobby.Id.ToString();
		activity.Party.Size.CurrentSize = lobbyManager.MemberCount(lobby.Id);
		activity.Party.Size.MaxSize = (int)lobby.Capacity;
		activity.Secrets.Join = lobbyManager.GetLobbyActivitySecret(lobby.Id);
		activity.Instance = true;
		Activity activity2 = activity;
		activityManager.UpdateActivity(activity2, delegate(Discord.Result result)
		{
			Console.WriteLine("Update Activity {0}", result);
		});
	}

	// Token: 0x06000003 RID: 3 RVA: 0x000021B8 File Offset: 0x000003B8
	private static void Main(string[] args)
	{
		string text = Environment.GetEnvironmentVariable("DISCORD_CLIENT_ID");
		if (text == null)
		{
			text = "418559331265675294";
		}
		Discord discord = new Discord(long.Parse(text), 0UL);
		discord.SetLogHook(LogLevel.Debug, delegate(LogLevel level, string message)
		{
			Console.WriteLine("Log[{0}] {1}", level, message);
		});
		ApplicationManager applicationManager = discord.GetApplicationManager();
		Console.WriteLine("Current Locale: {0}", applicationManager.GetCurrentLocale());
		Console.WriteLine("Current Branch: {0}", applicationManager.GetCurrentBranch());
		ActivityManager activityManager = discord.GetActivityManager();
		LobbyManager lobbyManager = discord.GetLobbyManager();
		LobbyManager.ConnectLobbyWithActivitySecretHandler <>9__17;
		activityManager.OnActivityJoin += delegate(string secret)
		{
			Console.WriteLine("OnJoin {0}", secret);
			LobbyManager lobbyManager = lobbyManager;
			LobbyManager.ConnectLobbyWithActivitySecretHandler callback;
			if ((callback = <>9__17) == null)
			{
				callback = (<>9__17 = delegate(Discord.Result result, ref Lobby lobby)
				{
					Console.WriteLine("Connected to lobby: {0}", lobby.Id);
					lobbyManager.ConnectNetwork(lobby.Id);
					lobbyManager.OpenNetworkChannel(lobby.Id, 0, true);
					foreach (User user in lobbyManager.GetMemberUsers(lobby.Id))
					{
						lobbyManager.SendNetworkMessage(lobby.Id, user.Id, 0, Encoding.UTF8.GetBytes(string.Format("Hello, {0}!", user.Username)));
					}
					Program.UpdateActivity(discord, lobby);
				});
			}
			lobbyManager.ConnectLobbyWithActivitySecret(secret, callback);
		};
		activityManager.OnActivitySpectate += delegate(string secret)
		{
			Console.WriteLine("OnSpectate {0}", secret);
		};
		activityManager.OnActivityJoinRequest += delegate(ref User user)
		{
			Console.WriteLine("OnJoinRequest {0} {1}", user.Id, user.Username);
		};
		activityManager.OnActivityInvite += delegate(ActivityActionType Type, ref User user, ref Activity activity2)
		{
			Console.WriteLine("OnInvite {0} {1} {2}", Type, user.Username, activity2.Name);
		};
		ImageManager imageManager = discord.GetImageManager();
		UserManager userManager = discord.GetUserManager();
		userManager.OnCurrentUserUpdate += delegate()
		{
			User currentUser = userManager.GetCurrentUser();
			Console.WriteLine(currentUser.Username);
			Console.WriteLine(currentUser.Id);
		};
		userManager.GetUser(450795363658366976L, delegate(Discord.Result result, ref User user)
		{
			if (result == Discord.Result.Ok)
			{
				Console.WriteLine("user fetched: {0}", user.Username);
				Program.FetchAvatar(imageManager, user.Id);
				return;
			}
			Console.WriteLine("user fetch error: {0}", result);
		});
		RelationshipManager relationshipManager = discord.GetRelationshipManager();
		relationshipManager.OnRefresh += delegate()
		{
			relationshipManager.Filter(delegate(ref Relationship relationship)
			{
				return relationship.Type == RelationshipType.Friend;
			});
			Console.WriteLine("relationships updated: {0}", relationshipManager.Count());
			for (int i = 0; i < Math.Min(relationshipManager.Count(), 10); i++)
			{
				Relationship at = relationshipManager.GetAt((uint)i);
				Console.WriteLine("relationships: {0} {1} {2} {3}", new object[]
				{
					at.Type,
					at.User.Username,
					at.Presence.Status,
					at.Presence.Activity.Name
				});
				Program.FetchAvatar(imageManager, at.User.Id);
			}
		};
		relationshipManager.OnRelationshipUpdate += delegate(ref Relationship r)
		{
			Console.WriteLine("relationship updated: {0} {1} {2} {3}", new object[]
			{
				r.Type,
				r.User.Username,
				r.Presence.Status,
				r.Presence.Activity.Name
			});
		};
		lobbyManager.OnLobbyMessage += delegate(long lobbyID, long userID, byte[] data)
		{
			Console.WriteLine("lobby message: {0} {1}", lobbyID, Encoding.UTF8.GetString(data));
		};
		lobbyManager.OnNetworkMessage += delegate(long lobbyId, long userId, byte channelId, byte[] data)
		{
			Console.WriteLine("network message: {0} {1} {2} {3}", new object[]
			{
				lobbyId,
				userId,
				channelId,
				Encoding.UTF8.GetString(data)
			});
		};
		lobbyManager.OnSpeaking += delegate(long lobbyID, long userID, bool speaking)
		{
			Console.WriteLine("lobby speaking: {0} {1} {2}", lobbyID, userID, speaking);
		};
		LobbyTransaction lobbyCreateTransaction = lobbyManager.GetLobbyCreateTransaction();
		lobbyCreateTransaction.SetCapacity(6U);
		lobbyCreateTransaction.SetType(LobbyType.Public);
		lobbyCreateTransaction.SetMetadata("a", "123");
		lobbyCreateTransaction.SetMetadata("a", "456");
		lobbyCreateTransaction.SetMetadata("b", "111");
		lobbyCreateTransaction.SetMetadata("c", "222");
		LobbyManager.SearchHandler <>9__22;
		lobbyManager.CreateLobby(lobbyCreateTransaction, delegate(Discord.Result result, ref Lobby lobby)
		{
			if (result != Discord.Result.Ok)
			{
				return;
			}
			Console.WriteLine("lobby {0} with capacity {1} and secret {2}", lobby.Id, lobby.Capacity, lobby.Secret);
			LobbyManager lobbyManager;
			foreach (string text2 in new string[]
			{
				"a",
				"b",
				"c"
			})
			{
				Console.WriteLine("{0} = {1}", text2, lobbyManager.GetLobbyMetadataValue(lobby.Id, text2));
			}
			foreach (User user in lobbyManager.GetMemberUsers(lobby.Id))
			{
				Console.WriteLine("lobby member: {0}", user.Username);
			}
			lobbyManager.SendLobbyMessage(lobby.Id, "Hello from C#!", delegate(Discord.Result _)
			{
				Console.WriteLine("sent message");
			});
			LobbyTransaction lobbyUpdateTransaction = lobbyManager.GetLobbyUpdateTransaction(lobby.Id);
			lobbyUpdateTransaction.SetMetadata("d", "e");
			lobbyUpdateTransaction.SetCapacity(16U);
			lobbyManager.UpdateLobby(lobby.Id, lobbyUpdateTransaction, delegate(Discord.Result _)
			{
				Console.WriteLine("lobby has been updated");
			});
			long lobbyID = lobby.Id;
			long userID = lobby.OwnerId;
			LobbyMemberTransaction memberUpdateTransaction = lobbyManager.GetMemberUpdateTransaction(lobbyID, userID);
			memberUpdateTransaction.SetMetadata("hello", "there");
			lobbyManager.UpdateMember(lobbyID, userID, memberUpdateTransaction, delegate(Discord.Result _)
			{
				Console.WriteLine("lobby member has been updated: {0}", lobbyManager.GetMemberMetadataValue(lobbyID, userID, "hello"));
			});
			LobbySearchQuery searchQuery = lobbyManager.GetSearchQuery();
			searchQuery.Filter("metadata.a", LobbySearchComparison.GreaterThan, LobbySearchCast.Number, "455");
			searchQuery.Sort("metadata.a", LobbySearchCast.Number, "0");
			searchQuery.Limit(1U);
			lobbyManager = lobbyManager;
			LobbySearchQuery query = searchQuery;
			LobbyManager.SearchHandler callback;
			if ((callback = <>9__22) == null)
			{
				callback = (<>9__22 = delegate(Discord.Result _)
				{
					Console.WriteLine("search returned {0} lobbies", lobbyManager.LobbyCount());
					if (lobbyManager.LobbyCount() == 1)
					{
						Console.WriteLine("first lobby secret: {0}", lobbyManager.GetLobby(lobbyManager.GetLobbyId(0)).Secret);
					}
				});
			}
			lobbyManager.Search(query, callback);
			lobbyManager.ConnectVoice(lobby.Id, delegate(Discord.Result _)
			{
				Console.WriteLine("Connected to voice chat!");
			});
			lobbyManager.ConnectNetwork(lobby.Id);
			lobbyManager.OpenNetworkChannel(lobby.Id, 0, true);
			Program.UpdateActivity(discord, lobby);
		});
		StorageManager storageManager = discord.GetStorageManager();
		byte[] contents = new byte[20000];
		new Random().NextBytes(contents);
		Console.WriteLine("storage path: {0}", storageManager.GetPath());
		StorageManager.ReadAsyncPartialHandler <>9__24;
		StorageManager.ReadAsyncHandler <>9__25;
		storageManager.WriteAsync("foo", contents, delegate(Discord.Result res)
		{
			StorageManager storageManager;
			foreach (FileStat fileStat in storageManager.Files())
			{
				Console.WriteLine("file: {0} size: {1} last_modified: {2}", fileStat.Filename, fileStat.Size, fileStat.LastModified);
			}
			storageManager = storageManager;
			string name = "foo";
			ulong offset = 400UL;
			ulong length = 50UL;
			StorageManager.ReadAsyncPartialHandler callback;
			if ((callback = <>9__24) == null)
			{
				callback = (<>9__24 = delegate(Discord.Result result, byte[] data)
				{
					Console.WriteLine("partial contents of foo match {0}", data.SequenceEqual(new ArraySegment<byte>(contents, 400, 50)));
				});
			}
			storageManager.ReadAsyncPartial(name, offset, length, callback);
			StorageManager storageManager2 = storageManager;
			string name2 = "foo";
			StorageManager.ReadAsyncHandler callback2;
			if ((callback2 = <>9__25) == null)
			{
				callback2 = (<>9__25 = delegate(Discord.Result result, byte[] data)
				{
					Console.WriteLine("length of contents {0} data {1}", contents.Length, data.Length);
					Console.WriteLine("contents of foo match {0}", data.SequenceEqual(contents));
					Console.WriteLine("foo exists? {0}", storageManager.Exists("foo"));
					storageManager.Delete("foo");
					Console.WriteLine("post-delete foo exists? {0}", storageManager.Exists("foo"));
				});
			}
			storageManager2.ReadAsync(name2, callback2);
		});
		StoreManager storeManager = discord.GetStoreManager();
		storeManager.OnEntitlementCreate += delegate(ref Entitlement entitlement)
		{
			Console.WriteLine("Entitlement Create1: {0}", entitlement.Id);
		};
		storeManager.FetchEntitlements(delegate(Discord.Result result)
		{
			if (result == Discord.Result.Ok)
			{
				foreach (Entitlement entitlement in storeManager.GetEntitlements())
				{
					Console.WriteLine("entitlement: {0} - {1} {2}", entitlement.Id, entitlement.Type, entitlement.SkuId);
				}
			}
		});
		storeManager.FetchSkus(delegate(Discord.Result result)
		{
			if (result == Discord.Result.Ok)
			{
				foreach (Sku sku in storeManager.GetSkus())
				{
					Console.WriteLine("sku: {0} - {1} {2}", sku.Name, sku.Price.Amount, sku.Price.Currency);
				}
			}
		});
		try
		{
			for (;;)
			{
				discord.RunCallbacks();
				lobbyManager.FlushNetwork();
				Thread.Sleep(16);
			}
		}
		finally
		{
			discord.Dispose();
		}
	}
}
