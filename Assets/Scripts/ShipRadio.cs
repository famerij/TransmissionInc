using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

[RequireComponent(typeof(Ship))]
public class ShipRadio : MonoBehaviour, IMessageReceiver
{
	[Header("Audio")]
	[SerializeField] private AudioClip thrusterTurnSound;
	
	private Ship ship;
	private AudioSource audioSource;

	protected void Awake()
	{
		ship = GetComponent<Ship>();
		audioSource = GetComponent<AudioSource>();
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

			audioSource.pitch = Random.Range(.9f, 1.1f);
			audioSource.PlayOneShot(thrusterTurnSound);
			return true;
		}

		return false;
	}
}
