﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkyARFighter.Common
{
    public enum RemotingMethodId : int
    {
        [RemotingDirection(RemotingDirection.C2S)]
        Login = 0x01,
        [RemotingDirection(RemotingDirection.S2C)]
        Welcome = 0x02,
        [RemotingDirection(RemotingDirection.Both)]
        SetupWorld = 0x03,
        [RemotingDirection(RemotingDirection.Both)]
        SendMessage = 0x04,
        [RemotingDirection(RemotingDirection.Both)]
        CreateSceneModel = 0x10,
        [RemotingDirection(RemotingDirection.Both)]
        MoveSceneModel = 0x11,
        [RemotingDirection(RemotingDirection.Both)]
        ScaleSceneModel = 0x12,
        [RemotingDirection(RemotingDirection.Both)]
        RotateSceneModel = 0x13,
        [RemotingDirection(RemotingDirection.Both)]
        DeleteSceneModel = 0x14,
        [RemotingDirection(RemotingDirection.S2C)]
        AddPlayer = 0x20,
        [RemotingDirection(RemotingDirection.S2C)]
        RemovePlayer = 0x21,
        [RemotingDirection(RemotingDirection.Both)]
        SyncPlayerState = 0x22,
    }

    public enum LoginWay
    {
        DeviceId,
        Account,
        AccessToken
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

    public enum MessageType
    {
        System_Failure,
        System_Notification,
        Player_Chat,
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
