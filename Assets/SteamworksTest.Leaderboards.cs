using Steamworks;
using Steamworks.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

public partial class SteamworksTest
{
	public async Task Leaderboards( int delay = 100 )
	{
		var leaderboard = await SteamUserStats.FindOrCreateLeaderboardAsync( "Testleaderboard", Steamworks.Data.LeaderboardSort.Ascending, Steamworks.Data.LeaderboardDisplay.Numeric );

		if ( !leaderboard.HasValue )
		{
			PrintError( $"Couldn't Get Leaderboard!" );
			return;
		}

		Print( $"Got leaderboard '{leaderboard.Value.Name}'" );
		Print( $"Leaderboard has '{leaderboard.Value.EntryCount}' entries!" );

		Print( "\n\nFriend Scores:\n" );

		var friendScores = await leaderboard.Value.GetScoresFromFriendsAsync();
		foreach ( var e in friendScores )
		{
			Print( $"{e.GlobalRank}: {e.Score} {e.User}" );
		}
	}
}
