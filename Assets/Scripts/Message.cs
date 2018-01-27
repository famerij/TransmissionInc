using UnityEngine;

public class Message
{
	public enum CommandType
	{
		Begin,
		End
	};

	public bool inUse;
	public Vector3 creationPosition;
	public float range;
	// Payload: (Could be bitflags :))
	public bool left;
	public bool right;
	// For up/down style:
	public CommandType commandType;

	// Created later!
	public GameObject visual;
}
