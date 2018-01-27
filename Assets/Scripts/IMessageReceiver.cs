using UnityEngine;

public interface IMessageReceiver
{
	// Return true if could receive it
	bool ReceivedMessage(Message msg, MessageTransmitter transmitter);
}
