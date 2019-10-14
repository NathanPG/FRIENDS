using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class QuestMsg : MonoBehaviour
{

    public class MyMsgType
    {
        public static short Score = MsgType.Highest + 1;
    };

    public class QuestMessage : MessageBase
    {
        public string quest_name;
        public string quest_content;
    }

    public void SendQuest(string qn, string qc)
    {
        QuestMessage msg = new QuestMessage();
        msg.quest_name = qn;
        msg.quest_content = qc;

        //NetworkServer.SendToAll(short msgType, MessageBase msg);
        NetworkServer.SendToAll(MyMsgType.Score, msg);
        
    }
}
