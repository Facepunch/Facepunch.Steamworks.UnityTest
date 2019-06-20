using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerManager : MonoBehaviour
{
	public GameObject ControllerPrefab;

	public List<ControllerPanel> Controllers = new List<ControllerPanel>();

    void Update()
    {
		if ( !Steamworks.SteamClient.IsValid )
			return;

		Steamworks.SteamInput.RunFrame();

		//
		// We could probably get away with doing this every couple of seconds
		// It only needs to be called to keep the list of currently active controllers up to date
		//

		foreach ( var controller in Steamworks.SteamInput.Controllers )
		{
			UpdateController( controller );
		}

		// TODO - Remove unplugged controllers
    }

	void UpdateController( Steamworks.Controller controller )
	{
		foreach ( var c in Controllers )
		{
			if ( c.Controller == controller )
				return;
		}

		var o = GameObject.Instantiate<GameObject>( ControllerPrefab );
		o.GetComponent<ControllerPanel>().Controller = controller;
		o.transform.SetParent( transform, false );
		Controllers.Add( o.GetComponent<ControllerPanel>() );
	}
}
