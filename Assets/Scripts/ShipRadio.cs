using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Ship))]
public class ShipRadio : MonoBehaviour, IMessageReceiver
{
	private Ship ship;

	protected void Awake()
	{
		ship = GetComponent<Ship>();
	}


	public bool ReceivedMessage(Message msg, MessageTransmitter transmitter)
	{
		float distanceSqr = (transform.position - msg.creationPosition).sqrMagnitude;
		if (distanceSqr <= (msg.range * msg.range))
		{
			float shipRotation = 0.0f;
			if (msg.commandType == Message.CommandType.Begin)
			{
				shipRotation = msg.left ? 1.0f : 0.0f;
				shipRotation += msg.right ? -1.0f : 0.0f;
			}
			else if (msg.commandType == Message.CommandType.End)
			{
				shipRotation = 0.0f;
			}

			ship.RotationRate = shipRotation;
			return true;
		}

		return false;
	}
}
