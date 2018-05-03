//
//  MainPlayer.swift
//  ARFighter
//
//  Created by Jones on 2018/5/1.
//  Copyright © 2018年 tjwd. All rights reserved.
//
import SceneKit

enum RemotingMethodId: Int32 {
    case Welcome = 1
    case SetupWorld
    case SyncCamera
    case CreateSceneModel
}

class MainPlayer: Player {
    
    override init(scene: GameScene) {
        super.init(scene: scene)
    }
    
    func initPlayer(playerId: Int64) {
        id = playerId
        
        // for test
        SocketClient.instance.sendMessage(cmd: .SetupWorld, context: "")
    }
    
    func setupWorld(identityName: String){
        print("invoking: setupWorld")
    }
    
    func createModel(type: SceneModel.Kind, pos: simd_float3) {
        let scale = simd_float3(1)
        let rotate = simd_float4(0)
        parentScene?.addModel(creatorId: id, type: type, pos: pos, scale: scale, rotate: rotate)
        server_createSceneModel(type: type, pos: pos, scale: scale, rotate: rotate)
    }
    
    func server_setupWorld() {
        SocketClient.instance.sendMessage(cmd: .SetupWorld, context: "")
    }
    
    func server_createSceneModel(type: SceneModel.Kind, pos: simd_float3, scale: simd_float3, rotate: simd_float4){
        let dic: [String: Any] = [
            "type": type.rawValue,
            "pos": pos,
            "scale": scale,
            "rotate": rotate
        ]
        let data = try? JSONSerialization.data(withJSONObject: dic, options: [])
        let json = String(data: data!, encoding: .utf8)!
        SocketClient.instance.sendMessage(cmd: .CreateSceneModel, context: json)
    }
}
