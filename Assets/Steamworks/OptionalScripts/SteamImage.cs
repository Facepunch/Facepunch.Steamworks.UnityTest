using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Steamworks.Data;
using System;
using UnityEngine.Networking;
using System.Threading.Tasks;

public class SteamImage : MonoBehaviour
{
	public void LoadTextureFromImage( Image img )
	{
		var texture = new Texture2D( (int) img.Width, (int) img.Height );

		for ( int x = 0; x < img.Width; x++ )
			for ( int y = 0; y < img.Height; y++ )
			{
				var p = img.GetPixel( x, y );

				texture.SetPixel( x, (int) img.Height - y, new UnityEngine.Color32( p.r, p.g, p.b, p.a ) );
			}

		texture.Apply();

		ApplyTexture( texture );
	}

	public async Task LoadTextureFromUrl( string url )
	{
		//
		// If you're going to use this properly in production
		// you need to think about caching the texture maybe
		// so you don't download it every time.
		//

		UnityWebRequest request = UnityWebRequestTexture.GetTexture( url, true );

		var r = request.SendWebRequest();

		while ( !r.isDone )
		{
			await Task.Delay( 10 );
		}

		if ( request.isNetworkError || request.isHttpError )
			return;

		DownloadHandlerTexture dh = request.downloadHandler as DownloadHandlerTexture;
		dh.texture.name = url;
		ApplyTexture( dh.texture );
	}

	public virtual void ApplyTexture( Texture2D texture )
	{
		var rawImage = GetComponent<UnityEngine.UI.RawImage>();
		if ( rawImage != null )
		{
			rawImage.texture = texture;
		}
	}
}
