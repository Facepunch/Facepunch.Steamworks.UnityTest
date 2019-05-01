using Steamworks;
using Steamworks.Data;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class SteamworksTest : MonoBehaviour
{
	public GameObject ButtonPrefab;
	public GameObject ButtonCanvas;

	public UnityEngine.UI.Text Text;
	private readonly List<string> lines = new List<string>();
	private readonly Dictionary<string, Func<Task>> Tests = new Dictionary<string, Func<Task>>();

	private SteamImage[] steamImages;
	private int steamImage;

	private void Start()
	{
		//
		// Log unhandled exceptions created in Async Tasks so we know when something has gone wrong
		//
		TaskScheduler.UnobservedTaskException += ( _, e ) => { Debug.LogException( e.Exception ); };

		steamImages = GetComponentsInChildren<SteamImage>( true );

		AddButton( "QUIT", () => Application.Quit() );

		AddButton( "ALL TESTS", () => CycleTests() );

		Tests["Friends"] = () => FriendTests();
		Tests["PlayedWithFriends"] = () => PlayedWithFriends();
		Tests["RandomFriends"] = () => RandomPeopleInformation();
		Tests["AppTest"] = () => AppTest();
		Tests["AchievementsTest"] = () => AchievementsTest();
		Tests["StatsTest"] = () => StatsTest();
		Tests["InventoryTest"] = () => InventoryTest();

		foreach( var t in Tests )
		{
			AddButton( t.Key, async () =>
			{
				Print( "" );
				Print( $"Running \"{t.Key}\"" );
				Print( "" );

				await t.Value();

				Print( "" );
				Print( $"Done" );
				Print( "" );
			} );
		}

		SteamClient.Init( 252490 );

		SteamInventory.OnDefinitionsUpdated += () => Print( "SteamInventory.OnDefinitionsUpdated", "#ffff00" );
		SteamInventory.OnInventoryUpdated += ( x ) => Print( $"SteamInventory.OnInventoryUpdated( {x} )", "#ffff00" );
		SteamFriends.OnPersonaStateChange += ( x ) => Print( $"SteamInventory.OnPersonaStateChange( {x} )", "#ffff00" );

		Print( $"Hello {SteamClient.Name} [{SteamClient.SteamId.Value}]" );
		Print( "" );
	}

	private void AddButton( string v, UnityEngine.Events.UnityAction p )
	{
		var go = GameObject.Instantiate( ButtonPrefab );

		go.GetComponentInChildren<UnityEngine.UI.Text>().text = v;

		go.GetComponentInChildren<UnityEngine.UI.Button>().onClick.AddListener( p );

		go.transform.SetParent( ButtonCanvas.transform, false );
	}

	public void Print( string str = "", string color = "" )
	{
		Debug.Log( str );

		if ( !string.IsNullOrEmpty( color ) )
		{
			str = $"<color={color}>{str}</color>";
		}

		lines.Add( str );

		while ( lines.Count > 300 )
		{
			lines.RemoveAt( 0 );
		}

		Text.text = string.Join( "\n", lines );
	}

	private void OnApplicationQuit()
	{
		SteamClient.Shutdown();
	}

	public void ExitGame()
	{
		Application.Quit();
	}

	public async Task CycleTests()
	{
		await Task.Delay( 400 );

		Print( "" );
		Print( $"Running Tests.." );
		Print( "" );

		await Task.Delay( 400 );

		foreach ( KeyValuePair<string, Func<Task>> test in Tests )
		{
			Print( "" );
			Print( $"Running \"{test.Key}\"" );
			Print( "" );

			await Task.Delay( 300 );

			await test.Value.Invoke();

			await Task.Delay( 300 );

			Print( "" );
			Print( $"Done \"{test.Key}\"" );
			Print( "" );

			await Task.Delay( 1000 );
		}

		Print( "" );
		Print( $"All Tests Complete." );
	}

	public async Task FriendTests()
	{
		foreach ( Friend friend in SteamFriends.GetFriends() )
		{
			Print( $"Friend: {friend.Name}\n{string.Join( ", ", friend.NameHistory )}" );
			Print( $"{friend.Relationship} / {friend.SteamLevel} / {friend.IsPlayingThisGame} / {friend.IsOnline} / {friend.Id} " );

			Image? avatar = await friend.GetSmallAvatarAsync();

			DrawImage( await friend.GetSmallAvatarAsync() );
			DrawImage( await friend.GetMediumAvatarAsync() );
			DrawImage( await friend.GetLargeAvatarAsync() );

			Print();

			await Task.Delay( 1 );
		}
	}
	public async Task PlayedWithFriends()
	{
		foreach ( Friend friend in SteamFriends.GetPlayedWith() )
		{
			await friend.RequestInfoAsync();

			Print( $"Friend: {friend.Name}\n{string.Join( ", ", friend.NameHistory )}" );
			Print( $"{friend.Relationship} / {friend.SteamLevel} / {friend.IsPlayingThisGame} / {friend.IsOnline} / {friend.Id} " );

			DrawImage( await friend.GetSmallAvatarAsync() );
			DrawImage( await friend.GetMediumAvatarAsync() );
			DrawImage( await friend.GetLargeAvatarAsync() );

			await Task.Delay( 1 );
		}
	}

	public async Task RandomPeopleInformation()
	{
		for( int i=0; i<10; i++ )
		{
			var friend = new Friend( 76561197960279927 + (ulong)UnityEngine.Random.Range( 0, 5000000 ) );

			await friend.RequestInfoAsync();

			Print( $"Friend: {friend.Name}\n{string.Join( ", ", friend.NameHistory )}" );
			Print( $"{friend.Relationship} / {friend.SteamLevel} / {friend.IsPlayingThisGame} / {friend.IsOnline} / {friend.Id} " );

			DrawImage( await friend.GetSmallAvatarAsync() );
			DrawImage( await friend.GetMediumAvatarAsync() );
			DrawImage( await friend.GetLargeAvatarAsync() );

			await Task.Delay( 1 );
		}
	}

	public void DrawImage( Steamworks.Data.Image? img )
	{
		if ( !img.HasValue ) return;

		steamImages[steamImage % steamImages.Length].LoadTextureFromImage( img.Value );

		steamImage++;
	}

	public async Task AppTest()
	{
		Print( $"SteamApps.AppOwner: {SteamApps.AppOwner}" );
		Print( $"SteamApps.AvailablLanguages: {string.Join( ", ", SteamApps.AvailablLanguages )}" );
		Print( $"SteamApps.BuildId: {SteamApps.BuildId}" );
		Print( $"SteamApps.CommandLine: {SteamApps.CommandLine}" );
		Print( $"SteamApps.CurrentBetaName: {SteamApps.CurrentBetaName}" );
		Print( $"SteamApps.GameLanguage: {SteamApps.GameLanguage}" );
		Print( $"SteamApps.IsCybercafe: {SteamApps.IsCybercafe}" );
		Print( $"SteamApps.IsLowVoilence: {SteamApps.IsLowVoilence}" );
		Print( $"SteamApps.IsSubscribed: {SteamApps.IsSubscribed}" );
		Print( $"SteamApps.IsSubscribedFromFamilySharing: {SteamApps.IsSubscribedFromFamilySharing}" );
		Print( $"SteamApps.IsSubscribedFromFreeWeekend: {SteamApps.IsSubscribedFromFreeWeekend}" );
		Print( $"SteamApps.IsVACBanned: {SteamApps.IsVACBanned}" );
		Print( $"SteamApps.PurchaseTime: {SteamApps.PurchaseTime()}" );
		Print( $"SteamApps.AppInstallDir: {SteamApps.AppInstallDir()}" );
		Print( $"SteamApps.InstalledDepots: {string.Join( ", ", SteamApps.InstalledDepots() )}" );
	}

	public async Task AchievementsTest( int delay = 10 )
	{
		foreach ( Achievement a in SteamUserStats.Achievements )
		{
			Print( $"{a.Identifier}" );
			Print( $"	a.State: {a.State}" );
			Print( $"	a.UnlockTime: {a.UnlockTime}" );
			Print( $"	a.Name: {a.Name}" );
			Print( $"	a.Description: {a.Description}" );
			Print( $"	a.GlobalUnlocked:	{a.GlobalUnlocked}" );

			DrawImage( await a.GetIconAsync() );

			await Task.Delay( delay );
		}
	} 

	public async Task StatsTest( int delay = 10 )
	{
		int players = await SteamUserStats.PlayerCountAsync();
		Print( $"current players: {players}" );

		Stat deaths = new Stat( "deaths" );

		Print( $"deaths.GetInt() - {deaths.GetInt()} ( SHOULD BE NON ZERO )" );
		Print( $"deaths.GetFloat() - {deaths.GetFloat()} ( SHOULD BE 0 )" );

		Print( $"Rust has globally had {deaths.GetGlobalInt()} deaths" );

		long[] history = await deaths.GetGlobalIntDays( 20 );

		for ( int i = 0; i < history.Length; i++ )
		{
			Print( $"{DateTime.Now.AddDays( -i ).ToShortDateString()} : {history[i]} Deaths" );
		}
	}

	public async Task InventoryTest( int delay = 100 )
	{
		await SteamInventory.WaitForDefinitions();

		InventoryResult? result = await SteamInventory.GetItems();
		if ( result.HasValue )
		{
			foreach ( InventoryItem item in result.Value.GetItems() )
			{
				Print( $"{item.Id} {item.Def.Name}" );
			}
		}
	}
}
