using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkyARFighter.Common
{
    public enum RemotingMethodId
    {
        [RemotingDirection(RemotingDirection.Both)]
        SetupWorld = 0x01,
        [RemotingDirection(RemotingDirection.Both)]
        SyncCamera = 0x02,
        [RemotingDirection(RemotingDirection.Both)]
        CreateSceneModel = 0x03
    }

    public enum SceneModelType
    {
        标注_圆点 = 0,
		标注_圆圈,
        标注_箭头,
        标注_连线,
        平面_贴图 = 10,
        平面_视频,
        平面_挡板,
        音效 = 20,
        特效,
        按钮触发器,
        角色 = 100
    }

    public class RemotingMethodAttribute: Attribute
    {
        public RemotingMethodAttribute(RemotingMethodId methodId)
        {
            MethodId = methodId;
        }

        public RemotingMethodId MethodId { get; private set; }
    }

    public enum RemotingDirection
    {
        C2S,
        S2C,
        Both
    }
    public class RemotingDirectionAttribute : Attribute
    {
        public RemotingDirectionAttribute(RemotingDirection dir)
        {
            Direction = dir;
        }

        public RemotingDirection Direction { get; private set; }
    }

}
