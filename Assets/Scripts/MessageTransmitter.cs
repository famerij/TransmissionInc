using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.UIElements;

public class MessageTransmitter : MonoBehaviour
{
	public bool isEnabled = true;
	
	[SerializeField] private float messageExpansionSpeed = 2.0f;
	[SerializeField] private float initialMessageRange = 0.5f;
	[SerializeField] private float maxMessageRange = -1.0f;
	[SerializeField] public bool drawMessages;
	[SerializeField] private GameObject messageVisualPrefab;
	[SerializeField] private Color messageLeftColor = Color.green;
	[SerializeField] private Color messageRightColor = Color.blue;
	[SerializeField] private Color messageStopColor = Color.red;

	[Header("Audio")]
	[SerializeField] private AudioClip messageStartSound;
	[SerializeField] private AudioClip messageEndSound;
	[SerializeField] private float maxDistanceDiminish = 50f;

	private Message[] messageBuffer = new Message[200];
	private int messageBufferCounter = 0;
	private int messagesInUseCount;
	private List<IMessageReceiver> receivers = new List<IMessageReceiver>();
	private AudioSource audioSource;
	private ShipRadio[] radios;

	private List<GameObject> messageVisuals = new List<GameObject>();

	// Use this for initialization
	void Start()
	{
		audioSource = GetComponent<AudioSource>();

		for (int i = 0; i < messageBuffer.Length; i++)
		{
			messageBuffer[i] = new Message();
		}

		radios = FindObjectsOfType<ShipRadio>();
		receivers.AddRange(radios);
		
		ToggleEnabled(isEnabled);
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

				messageBuffer[messageBufferCounter].visual = GetMessageVisual();
				messageBuffer[messageBufferCounter].visual.transform.position = transform.position;
				Color msgColor = leftDown ? messageLeftColor : messageRightColor;
                messageBuffer[messageBufferCounter].visual.GetComponentInChildren<SpriteRenderer>().color = msgColor;

				messageBufferCounter++;
				if(messageBufferCounter == messageBuffer.Length)
				{
					messageBufferCounter = 0;
				}
				
				if (drawMessages)
					audioSource.PlayOneShot(messageStartSound);
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

				messageBuffer[messageBufferCounter].visual = GetMessageVisual();
				messageBuffer[messageBufferCounter].visual.transform.position = transform.position;
				messageBuffer[messageBufferCounter].visual.GetComponentInChildren<SpriteRenderer>().color = messageStopColor;

				messageBufferCounter++;
				if(messageBufferCounter == messageBuffer.Length)
				{
					messageBufferCounter = 0;
				}
				
				if (drawMessages)
					audioSource.PlayOneShot(messageEndSound);
			}
		}
	}

	// Update is called once per frame
	void Update ()
	{
		if (!isEnabled) return;
		
		CreateMessages();

		// Process message queue:
		messagesInUseCount = 0;
		for (int i = 0; i < messageBuffer.Length; i++)
		{
			Message msg = messageBuffer[i];
			if (msg.inUse)
			{
				messagesInUseCount++;
				msg.range += (messageExpansionSpeed * Time.deltaTime);
				if (drawMessages)
				{
					DrawMessage(msg);
				}

				GameObject visual = msg.visual;
				visual.SetActive(true);
				visual.transform.localScale = new Vector3(msg.range, msg.range, msg.range);


				for (int r = 0; r < receivers.Count; r++)
				{
					if (receivers[r].ReceivedMessage(msg, this))
					{
						// For now only one receiver can receive it
						KillMessage(msg);
					}
				}

				// Cap the message range if max is set:
				if(maxMessageRange > 0.0f && msg.range >= maxMessageRange)
				{
					KillMessage(msg);
                }
			}
		}

		var distanceToRadio = Vector3.Distance(transform.position, radios[0].transform.position);
		var distanceToRadioRatio = distanceToRadio / maxDistanceDiminish;
//		Debug.LogFormat("Distance: {0:0.0}, Ratio: {1:0.0}", distanceToRadio, distanceToRadioRatio);
		audioSource.volume = Mathf.Clamp(1f - distanceToRadioRatio, 0.2f, 1f);
//		Debug.LogFormat("Distance to radio {0:0.0}", Vector3.Distance(transform.position, radios[0].transform.position));
	}

	private void KillMessage(Message msg)
	{
		msg.inUse = false;
		msg.visual.SetActive(false);
		msg.visual = null;
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

	public void ToggleEnabled(bool enabled)
	{
		isEnabled = enabled;
	}

	private GameObject GetMessageVisual()
	{
		GameObject result = null;
		for(int i = 0; i < messageVisuals.Count; i++)
		{
			if(!messageVisuals[i].activeSelf)
			{
				result = messageVisuals[i];
				break;
			}
		}

		if(result == null)
		{
			GameObject go = Instantiate<GameObject>(messageVisualPrefab, transform);
			messageVisuals.Add(go);
			result = go;
			Debug.Log("Created new visual number: " + messageVisuals.Count);
		}

		return result;
	}
}
