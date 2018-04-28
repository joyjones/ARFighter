using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkyARFighter.Common
{
    public enum MessageType
    {
        Chat,
        SyncCamera,
        CreateObject
    }

    public class RemotingMethodAttribute: Attribute
    {
        public RemotingMethodAttribute(int methodId)
        {
            MethodId = methodId;
        }

        public int MethodId { get; private set; }
    }

    //public abstract class Message
    //{
    //    public Message(byte[] buff)
    //    {
    //        Deserialize(buff);
    //    }

    //    protected abstract void Deserialize(byte[] buff);

    //    public virtual MessageType Type { get; }

    //    public static MessageType GetType(byte[] buff)
    //    {
    //        return (MessageType)BitConverter.ToInt32(buff, 0);
    //    }
    //}

    //public class ChatMessage: Message
    //{
    //    public ChatMessage(byte[] buff)
    //        : base(buff)
    //    {
    //    }
    //    public override MessageType Type => MessageType.Chat;
    //    protected override void Deserialize(byte[] buff)
    //    {
    //    }
    //}

    //public class SyncCameraMessage : Message
    //{
    //    public SyncCameraMessage(byte[] buff)
    //        : base(buff)
    //    {
    //    }

    //    public override MessageType Type => MessageType.SyncCamera;

    //    protected override void Deserialize(byte[] buff)
    //    {
    //        cameraTransform.Deserialize(buff);
    //    }
    //    private Matrix cameraTransform;
    //}
}
