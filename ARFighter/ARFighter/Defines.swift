//
//  Defines.swift
//  ARFighter
//
//  Created by Jones on 2018/5/5.
//  Copyright © 2018年 tjwd. All rights reserved.
//

import SceneKit

enum RemotingMethodId: Int32 {
    case Login = 0x01
    case Welcome
    case SetupWorld
    case SendMessage
    case CreateSceneModel = 0x10
    case MoveSceneModel
    case ScaleSceneModel
    case RotateSceneModel
    case DeleteSceneModel
    case AddPlayer = 0x20
    case RemovePlayer
    case SyncPlayerState
}

enum PlayerState {
    case Initial
    case SearchOrigin
    case SceneLoading
    case ScenePlaying
}

enum LoginWay: Int32 {
    case DeviceId
    case Account
    case AccessToken
}

enum OperationMode: Int32 {
    case None
    case Create
    case Delete
    case Move
    case Rotate
    case Scale
}

class GameObject {
    
    var id: Int64 = generateAutoId()
    
    static func generateAutoId() -> Int64 {
        let ct = Calendar.current
        let coms = ct.dateComponents([.year, .month, .day, .hour, .minute, .second], from: Date())
        let tail = arc4random_uniform(100000)
        let idstr = String(format: "%04d%02d%02d%02d%02d%02d%05d", coms.year!, coms.month!, coms.day!, coms.hour!, coms.minute!, coms.second!, tail)
        return Int64(idstr)!
    }
}

protocol MessageViewDelegate {
    func appendLog(msg: String)
    func alert(msg: String)
    func updatePlayerState(name: String?, state: PlayerState)
}

protocol RemotingMethodDelegate {
    func invokeMethod(methodId: RemotingMethodId, args: [Any])
}


