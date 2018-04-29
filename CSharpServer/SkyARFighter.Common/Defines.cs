using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkyARFighter.Common
{
    public enum RemotingMethodId
    {
        SetupWorld = 0x01,
        SyncCamera = 0x02,
        CreateObject = 0x03
    }

    public enum ObjectType
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
    
}
