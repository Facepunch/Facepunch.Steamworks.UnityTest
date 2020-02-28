using Steamworks;
using Steamworks.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

public partial class SteamworksTest
{
	public async Task WorkshopList( int delay = 100 )
	{
		var q = Steamworks.Ugc.Query.Items.RankedByTrend();

		var page = await q.GetPageAsync( 1 );

		foreach ( var result in page.Value.Entries.Take( 5 ) )
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


	public async Task WorkshopDownload( int delay = 100 )
	{
		var q = Steamworks.Ugc.Query.Items.SortByCreationDateAsc();

		var page = await q.GetPageAsync( 3 );
		var file = page.Value.Entries.Where( x => !x.IsInstalled ).First();

		Print( $"Found {file.Title}.." );

		if ( !file.Download( true ) )
		{
			Print( $"Download returned false!?" );
			return;
		}

		Print( $"Downloading.." );

		while ( file.NeedsUpdate ) 
		{
			await Task.Delay( 100 );

			Print( $"Downloading... ({file.DownloadAmount:0.000}) [{file.DownloadBytesDownloaded}/{file.DownloadBytesTotal}]" );
		}

		while ( !file.IsInstalled )
		{
			await Task.Delay( 100 );

			Print( $"Installing..." );
		}

		Print( $"Installed to {file.Directory}" );

		var dir = new System.IO.DirectoryInfo( file.Directory );

		Print( $"" );

		foreach ( var f in dir.EnumerateFiles() )
		{
			Print( $"{f.FullName}" );
		}
	}
}
