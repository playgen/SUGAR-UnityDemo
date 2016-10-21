using System;
using UnityEngine;
using PlayGen.SUGAR.Client;

public class WebSockets : MonoBehaviour
{
    private WebSocket _socket;
    private int _messageCounter;

	void Start ()
	{
	    _socket = new WebSocket(new Uri("ws://localhost:62312"));
        _socket.Connect();

	    SendMessage();
	}
	
	
	void Update ()
	{
	    var reply = _socket.RecieveString();

	    if (reply != null)
	    {
	        Debug.Log("Recieved: " + reply);
	        SendMessage();
	    }

	    if (_socket.Error != null)
	    {
	        Debug.LogError(_socket.Error);
	    }
	}

    private void SendMessage()
    {
        _socket.SendString("Test Message " + _messageCounter);
        _messageCounter++;
    }
}
