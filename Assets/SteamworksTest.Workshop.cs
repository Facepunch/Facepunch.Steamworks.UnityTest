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
			Print( $"" );
			Print( $"{result.Title}" );
			Print( $"	     Id: {result.Id}" );
			Print( $"	Instald: {result.IsInstalled}" );
			Print( $"	Created: {result.Created}" );
			Print( $"	Drectry: {result.Directory}" );
			Print( $"	Favrtes: {result.NumFavorites}" );
			Print( $"	Preview: {result.PreviewImageUrl}" );

			await DrawImage( result.PreviewImageUrl );
		}
	}
}
