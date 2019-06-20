using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerPanel : MonoBehaviour
{
	public Steamworks.Controller Controller;

	public UnityEngine.UI.Text DebugText;


	private void Update()
	{
		Controller.ActionSet = "InGameControls";

		DebugText.text = $"{Controller.InputType}";

		DigitalState( "fire" );
		DigitalState( "Jump" );
		DigitalState( "pause_menu" );
		AnalogState( "Move" );
	}

	private void AnalogState( string v )
	{
		var state = Controller.GetAnalogState( v );

		DebugText.text += $"\n{state.X} {state.Y}";
	}

	private void DigitalState( string v )
	{
		var state = Controller.GetDigitalState( v );
		if ( state.Pressed )
		{
			DebugText.text += $"\n{v}";
		}
	}
}
