using Steamworks;
using Steamworks.Data;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public partial class SteamworksTest
{
	public async Task WorkshopList( int delay = 100 )
	{
		var q = Steamworks.Ugc.Query.Items.RankedByTrend();

		var page = await q.GetPageAsync( 1 );

		foreach ( var result in page.Value.Entries )
		{
			Print( $"Result: {result.Id}" );
			Print( $"	{result.Title}" );
			Print( $"	{result.Description.Replace( '\n', ' ' ).Replace( '\r', ' ' )}" );
			Print( $"	IsInstalled: {result.IsInstalled}" );
			Print( $"	Created: {result.Created}" );
		}
	}
}
