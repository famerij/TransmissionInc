using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceshipControllerCombo : MonoBehaviour
{
	public enum MessageCreationType
	{
		EveryFrame,
		UpAndDown
	};

	public enum MsgCommandType
	{
		Begin,
		End
	};

	public class MessageData
	{
		public bool inUse;
		public Vector3 creationPosition;
		public float range;
		// Payload: (Could be bitflags :))
		public bool left;
		public bool right;
		// For every frame style:
		public float deltaTime;
		// For up/down style:
		public MsgCommandType commandType;
	};

	[SerializeField] private MessageCreationType creationType;
	[SerializeField] private Transform messageOrigin;
	[SerializeField] private float spaceshipSpeed = 1.0f;
	[SerializeField] private float messageExpansionSpeed = 2.0f;
	[SerializeField] private float initialMessageRange = 0.5f;
	[SerializeField] private float spaceshipRotationSpeed = 1.0f;

	//private float realTimePassed;
	private float distanceToEarthSqr;
	private MessageData[] messageBuffer = new MessageData[2000];
	private int messageBufferCounter = 0;
	private int messagesInUseCount;

	private float shipRotation;

	// Use this for initialization
	void Start ()
	{
		for (int i = 0; i < messageBuffer.Length; i++)
		{
			messageBuffer[i] = new MessageData();
		}
	}

	private void CreateMessages()
	{
		if(creationType == MessageCreationType.EveryFrame)
		{
			if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.RightArrow))
			{
				if(messageBuffer[messageBufferCounter].inUse)
				{
					Debug.LogWarning("Buffer overrun! Increase its size pls");
				}
				else
				{
					messageBuffer[messageBufferCounter].left = Input.GetKey(KeyCode.LeftArrow);
					messageBuffer[messageBufferCounter].right = Input.GetKey(KeyCode.RightArrow);
					messageBuffer[messageBufferCounter].range = initialMessageRange;
					messageBuffer[messageBufferCounter].creationPosition = messageOrigin.position;
					messageBuffer[messageBufferCounter].inUse = true;
					messageBuffer[messageBufferCounter].deltaTime = Time.deltaTime;
					messageBufferCounter++;
					if(messageBufferCounter == messageBuffer.Length)
					{
						messageBufferCounter = 0;
					}
				}
			}
		}
		else if (creationType == MessageCreationType.UpAndDown)
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
					messageBuffer[messageBufferCounter].creationPosition = messageOrigin.position;
					messageBuffer[messageBufferCounter].inUse = true;
					messageBuffer[messageBufferCounter].commandType = MsgCommandType.Begin;
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
					messageBuffer[messageBufferCounter].creationPosition = messageOrigin.position;
					messageBuffer[messageBufferCounter].inUse = true;
					messageBuffer[messageBufferCounter].commandType = MsgCommandType.End;
					messageBufferCounter++;
					if(messageBufferCounter == messageBuffer.Length)
					{
						messageBufferCounter = 0;
					}
				}
			}

		}
	}
	
	// Update is called once per frame
	void Update ()
	{
		//realTimePassed += Time.deltaTime;

		CreateMessages();
		
		// Process message queue:
		messagesInUseCount = 0;
		for (int i = 0; i < messageBuffer.Length; i++)
		{
			if (messageBuffer[i].inUse)
			{
				messagesInUseCount++;
				DrawMessage(messageBuffer[i]);

				messageBuffer[i].range += (messageExpansionSpeed * Time.deltaTime);
				float distanceSqr = (transform.position - messageBuffer[i].creationPosition).sqrMagnitude;
				if (distanceSqr <= (messageBuffer[i].range * messageBuffer[i].range))
				{
					ProcessMessage(messageBuffer[i]);
				}
			}
		}

		// Move spaceship:
		this.transform.position += (transform.up * spaceshipSpeed * Time.deltaTime);
		if(creationType == MessageCreationType.UpAndDown)
		{
			this.transform.Rotate(Vector3.forward, shipRotation * spaceshipRotationSpeed * Time.deltaTime);
		}
		//Debug.Log("Messages in use: " + messagesInUseCount);
	}

	private void ProcessMessage(MessageData msg)
	{
		msg.inUse = false;
		if(creationType == MessageCreationType.EveryFrame)
		{
			float rotation = msg.left ? 1.0f : 0.0f;
			rotation += msg.right ? -1.0f : 0.0f;

			this.transform.Rotate(Vector3.forward, rotation * spaceshipRotationSpeed * msg.deltaTime);
		}
		else if(creationType == MessageCreationType.UpAndDown)
		{
			if(msg.commandType == MsgCommandType.Begin)
			{
				shipRotation = msg.left ? 1.0f : 0.0f;
				shipRotation += msg.right ? -1.0f : 0.0f;
			}
			else if(msg.commandType == MsgCommandType.End)
			{
				shipRotation = 0.0f;
			}
		}
    }

	private void DrawMessage(MessageData msg)
	{
		Color msgColor = Color.red;
		if(msg.commandType == MsgCommandType.End)
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

		DrawCircle(messageOrigin.position, msg.range, msgColor, 32);
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
