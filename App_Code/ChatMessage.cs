using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for ChatMessage
/// </summary>
public class ChatMessage
{
    public string message;
    public string sender;
    public int messageID;
    public string MessageTime;
    public int PlayerID;

    public ChatMessage(string message, string sender, int messageID, string sent, int PlayerID)
    {
        this.message = message;
        this.sender = sender;
        this.messageID = messageID;
        MessageTime = sent;
        this.PlayerID = PlayerID;
    }
}