using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceshipController : MonoBehaviour
{
	public class MessageData
	{
		public bool inUse;
		public Vector3 creationPosition;
		public float range;
		// Payload: (Could be bitflags :))
		public bool left;
		public bool right;
	};

	[SerializeField] private Transform earthTransform;
	[SerializeField] private float spaceshipSpeed = 1.0f;
	[SerializeField] private float messageExpansionSpeed = 2.0f;
	[SerializeField] private float initialMessageRange = 0.5f;
	[SerializeField] private float spaceshipRotationSpeed = 1.0f;

	private float realTimePassed;
	private float distanceToEarthSqr;
	private MessageData[] messageBuffer = new MessageData[200];
	private int messageBufferCounter = 0;
	private int messagesInUseCount;

	// Use this for initialization
	void Start ()
	{
		for (int i = 0; i < messageBuffer.Length; i++)
		{
			messageBuffer[i] = new MessageData();
		}
	}
	
	// Update is called once per frame
	void Update ()
	{
		realTimePassed += Time.deltaTime;

		if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.RightArrow))
		{
			if(messageBuffer[messageBufferCounter].inUse)
			{
				Debug.LogWarning("Buffer overran! Increase it's size pls");
			}
			else
			{
				messageBuffer[messageBufferCounter].left = Input.GetKey(KeyCode.LeftArrow);
				messageBuffer[messageBufferCounter].right = Input.GetKey(KeyCode.RightArrow);
				messageBuffer[messageBufferCounter].range = initialMessageRange;
				messageBuffer[messageBufferCounter].creationPosition = earthTransform.position;
				messageBuffer[messageBufferCounter].inUse = true;
				messageBufferCounter++;
				if(messageBufferCounter == messageBuffer.Length)
				{
					messageBufferCounter = 0;
				}
			}
		}
		
		// Process message queue:
		messagesInUseCount = 0;
		for (int i = 0; i < messageBuffer.Length; i++)
		{
			if (messageBuffer[i].inUse)
			{
				messagesInUseCount++;
				DrawMessage(messageBuffer[i]);

				messageBuffer[i].range += (realTimePassed * messageExpansionSpeed);
				float distanceSqr = (transform.position - messageBuffer[i].creationPosition).sqrMagnitude;
				if (distanceSqr <= (messageBuffer[i].range * messageBuffer[i].range))
				{
					ProcessMessage(messageBuffer[i]);
				}
			}
		}

		// Move spaceship:
		this.transform.position += (transform.up * spaceshipSpeed);

		//Debug.Log("Messages in use: " + messagesInUseCount);
	}

	private void ProcessMessage(MessageData msg)
	{
		msg.inUse = false;
		float rotation = msg.left ? 1.0f : 0.0f;
		rotation += msg.right ? -1.0f : 0.0f;

		this.transform.Rotate(Vector3.forward, rotation * spaceshipRotationSpeed);
    }

	private void DrawMessage(MessageData msg)
	{
		DrawCircle(earthTransform.position, msg.range, Color.red, 32);
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
