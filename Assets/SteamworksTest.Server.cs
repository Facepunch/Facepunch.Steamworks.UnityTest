using Steamworks;
using Steamworks.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

public partial class SteamworksTest
{
	public async Task Server( int delay = 100 )
	{
		Print( "Initializing Server" );

		//
		// Init Server
		//
		var serverInit = new SteamServerInit( "rust", "Rusty Mode" )
		{
			GamePort = 28015,
			Secure = true,
			QueryPort = 28016
		};

		Steamworks.SteamServer.Init( 252490, serverInit );

		SteamServer.LogOnAnonymous();

		Print( "Server Initliazed" );

		await Task.Delay( 1000 );

		await TestServerAuth( delay );

		await Task.Delay( 1000 );

		Print( "Closing Server" );
		Steamworks.SteamServer.Shutdown();
	}

	public async Task TestServerAuth( int delay = 100 )
	{
		var stopwatch = System.Diagnostics.Stopwatch.StartNew();
		bool finished = false;
		string failed = null;
		AuthResponse response = AuthResponse.AuthTicketInvalidAlreadyUsed;

		//
		// Clientside calls this function, gets ticket
		//
		Print( "Client: Getting ticket" );
		var clientTicket = SteamUser.GetAuthSessionTicket();

		//
		// The client sends this data to the server along with their steamid
		//
		var ticketData = clientTicket.Data;
		var clientSteamId = SteamClient.SteamId;

		//
		// Server listens to auth responses from Gabe
		//
		SteamServer.OnValidateAuthTicketResponse += ( steamid, ownerid, rsponse ) =>
		{
			finished = true;
			response = rsponse;

			if ( steamid == 0 )
				failed = $"steamid is 0! {steamid} != {ownerid} ({rsponse})";

			if ( ownerid == 0 )
				failed = $"ownerid is 0! {steamid} != {ownerid} ({rsponse})";

			if ( steamid != ownerid )
				failed = $"Steamid and Ownerid are different! {steamid} != {ownerid} ({rsponse})";
		};

		//
		// Server gets the ticket, starts authing
		//

		Print( "Server: Checking ticket" );
		if ( !SteamServer.BeginAuthSession( ticketData, clientSteamId ) )
		{
			PrintError( "BeginAuthSession returned false, called bullshit without even having to check with Gabe" );
			return;
		}

		//
		// Wait for that to go through steam
		//
		while ( !finished )
		{
			if ( stopwatch.Elapsed.TotalSeconds > 5 )
				throw new System.Exception( "Took too long waiting for AuthSessionResponse.OK" );

			await Task.Delay( 10 );
		}

		Print( "Server: Got Response" );

		if ( response != AuthResponse.OK )
		{
			PrintError( "response != AuthResponse.OK" );
			return;
		}

		Print( $"Server: response = {response}" );

		if ( failed != null )
		{
			PrintError( failed );
			return;
		}

		finished = false;
		stopwatch = System.Diagnostics.Stopwatch.StartNew();

		//
		// The client is leaving, and now wants to cancel the ticket
		//

		if ( 0 == clientTicket.Handle )
		{
			PrintError( "clientTicket.Handle == 0!" );
			return;
		}

		Print( $"Client: Cancel Ticket" );
		clientTicket.Cancel();

		//
		// We should get another callback 
		//
		while ( !finished )
		{
			if ( stopwatch.Elapsed.TotalSeconds > 5 )
			{
				PrintError( "Took too long waiting for AuthSessionResponse.AuthTicketCanceled" );
				return;
			}

			await Task.Delay( 10 );
		}

		Print( $"Server: All Good" );

		if ( failed != null )
		{
			PrintError( failed );
		}
	}
}
