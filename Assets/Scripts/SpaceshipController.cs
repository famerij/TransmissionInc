using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceshipController : MonoBehaviour
{
	public class MessageData
	{
		public bool left;
		public bool right;
		public float creationTime;
		public float range;
		public bool inUse;
	};

	[SerializeField] private Transform earthTransform;
	[SerializeField] private float spaceshipSpeed = 1.0f;
	[SerializeField] private float messageExpansionSpeed = 2.0f;
	[SerializeField] private float initialMessageRange = 0.5f;
	[SerializeField] private float spaceshipRotationSpeed = 1.0f;

	private float realTimePassed;
	private float distanceToEarthSqr;
	private List<MessageData> messageBuffer = new List<MessageData>(1000);
	//private int messageBufferCounter = 0;

	// Use this for initialization
	void Start ()
	{
		
	}
	
	// Update is called once per frame
	void Update ()
	{
		realTimePassed += Time.deltaTime;

		if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.RightArrow))
		{
			MessageData message = new MessageData()
			{
				left = Input.GetKey(KeyCode.LeftArrow),
				right = Input.GetKey(KeyCode.RightArrow),
				creationTime = realTimePassed,
				range = initialMessageRange,
				inUse = true
			};
			messageBuffer.Add(message);
		}
		
		// Process input queue
		distanceToEarthSqr = (transform.position - earthTransform.position).sqrMagnitude;
		for (int i = 0; i < messageBuffer.Count; i++)
		{
			if (messageBuffer[i].inUse)
			{
				DrawMessage(messageBuffer[i]);
				messageBuffer[i].range += (realTimePassed * messageExpansionSpeed);
				if (distanceToEarthSqr <= (messageBuffer[i].range * messageBuffer[i].range))
				{
					ProcessMessage(messageBuffer[i]);
				}
			}
		}

		// Move spaceship:
		this.transform.position += (transform.up * spaceshipSpeed);
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
		Quaternion q = Quaternion.AngleAxis(360.0f / 16, Vector3.forward);
		for (int i = 0; i < sides; i++)
		{
			Vector3 lastArm = arm;
			arm = q * arm;
			Debug.DrawLine(position + lastArm, position + arm, color);
		}
	}
}
