---
uid: networking
title: Networking
author: sirkingbinx
---

## Networking
GorillaLibrary has its own simple networking system called the channel system. It is built on top of Photon events and makes networking simplified for new modders.

## Setting up communication
```cs
using GorillaLibrary.Models;

// Sets up channels
commChannel = new NetChannel("mod-comm-channel");
commChannel.OnReceivedMessage += OnCommChannelMessage;

// Extra note: channels implement IDisposable, so you can also use them inside of `using` statements; see below
using NetChannel temporaryChannel = new NetChannel("i-am-sending-something");

// Here's how to send data. It does not have to be a string, it can be anything
temporaryChannel.SendMessage("Hello world"); // Send a message to everyone
temporaryChannel.SendMessage("Hello world", [ player1, player2, player3 ]); // Send a message to a list of players
temporaryChannel.SendMessage("Hello world", specificNetPlayer); // Send a message to a specific person

// Unhook your channel when you are done with it
commChannel.Dispose();

private void OnCommChannelMessage(NetChannelMessage message)
{
	// You can use this helper method
	if (message.TryGetValue(out string myMessage))
		Debug.Log(myMessage);
	
	// Or manually cast your message data
	if (message.Data is string myManualMessage)
		Debug.Log(myManualMessage);
	
	var receivers = message.Targets; // everyone who received this message
	var sender = message.Sender; // who sent the message
	
	// either Broadcast, Multicast, or Unicast
	// (everyone, multiple people, one person)
	var messageType = message.MessageType;
}
```

For API documentation on channels, see @GorillaLibrary.Models.NetChannel.
