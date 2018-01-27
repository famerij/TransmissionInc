using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{

	[SerializeField] private MessageTransmitter[] enabledTransmitters = new MessageTransmitter[0];
	[SerializeField] private MessageTransmitter[] disabledTransmitters = new MessageTransmitter[0];

	void Start ()
	{
		foreach(MessageTransmitter t in enabledTransmitters)
		{
			t.enabled = true;
		}
		foreach (MessageTransmitter t in disabledTransmitters)
		{
			t.enabled = false;
		}
	}
}
