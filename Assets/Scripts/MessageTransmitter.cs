using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MessageTransmitter : MonoBehaviour
{
	[SerializeField] private float messageExpansionSpeed = 2.0f;
	[SerializeField] private float initialMessageRange = 0.5f;

	private Message[] messageBuffer = new Message[200];
	private int messageBufferCounter = 0;
	private int messagesInUseCount;
	private List<IMessageReceiver> receivers = new List<IMessageReceiver>();

	// Use this for initialization
	void Start()
	{
		for (int i = 0; i < messageBuffer.Length; i++)
		{
			messageBuffer[i] = new Message();
		}

		ShipRadio[] radios = FindObjectsOfType<ShipRadio>();
		receivers.AddRange(radios);
    }

	private void CreateMessages()
	{
		bool leftDown = Input.GetKeyDown(KeyCode.LeftArrow);
		bool rightDown = Input.GetKeyDown(KeyCode.RightArrow);
		if (leftDown || rightDown)
		{
			if(messageBuffer[messageBufferCounter].inUse)
			{
				Debug.LogWarning("Buffer overrun! Increase its size pls");
			}
			else
			{
				messageBuffer[messageBufferCounter].left = leftDown;
				messageBuffer[messageBufferCounter].right = rightDown;
				messageBuffer[messageBufferCounter].range = initialMessageRange;
				messageBuffer[messageBufferCounter].creationPosition = transform.position;
				messageBuffer[messageBufferCounter].inUse = true;
				messageBuffer[messageBufferCounter].commandType = Message.CommandType.Begin;
				messageBufferCounter++;
				if(messageBufferCounter == messageBuffer.Length)
				{
					messageBufferCounter = 0;
				}
			}
		}

		bool leftUp = Input.GetKeyUp(KeyCode.LeftArrow);
		bool rightUp = Input.GetKeyUp(KeyCode.RightArrow);
		if (leftUp || rightUp)
		{
			if(messageBuffer[messageBufferCounter].inUse)
			{
				Debug.LogWarning("Buffer overrun! Increase its size pls");
			}
			else
			{
				messageBuffer[messageBufferCounter].left = leftUp;
				messageBuffer[messageBufferCounter].right = rightUp;
				messageBuffer[messageBufferCounter].range = initialMessageRange;
				messageBuffer[messageBufferCounter].creationPosition = transform.position;
				messageBuffer[messageBufferCounter].inUse = true;
				messageBuffer[messageBufferCounter].commandType = Message.CommandType.End;
				messageBufferCounter++;
				if(messageBufferCounter == messageBuffer.Length)
				{
					messageBufferCounter = 0;
				}
			}
		}
	}
	
	// Update is called once per frame
	void Update ()
	{
		CreateMessages();
		
		// Process message queue:
		messagesInUseCount = 0;
		for (int i = 0; i < messageBuffer.Length; i++)
		{
			Message msg = messageBuffer[i];
			if (msg.inUse)
			{
				messagesInUseCount++;
				messageBuffer[i].range += (messageExpansionSpeed * Time.deltaTime);
				DrawMessage(msg);

				for(int r = 0; r < receivers.Count; r++)
				{
					if (receivers[r].ReceivedMessage(msg, this))
					{
						// For now only one receiver can receive it
						messageBuffer[i].inUse = false;
					}
				}
			}
		}
	}

	private void DrawMessage(Message msg)
	{
		Color msgColor = Color.red;
		if(msg.commandType == Message.CommandType.End)
		{
			msgColor = Color.red;
		}
		else
		{
			if(msg.left)
			{
				msgColor = Color.blue;
			}
			else if(msg.right)
			{
				msgColor = Color.green;
			}
		}

		DrawCircle(msg.creationPosition, msg.range, msgColor, 32);
	}

	private void DrawCircle(Vector3 position, float range, Color color, int sides = 16)
	{
		Vector3 arm = Vector3.up * range;
		Quaternion q = Quaternion.AngleAxis(360.0f / sides, Vector3.forward);
		for (int i = 0; i < sides; i++)
		{
			Vector3 lastArm = arm;
			arm = q * arm;
			Debug.DrawLine(position + lastArm, position + arm, color);
		}
	}
}
