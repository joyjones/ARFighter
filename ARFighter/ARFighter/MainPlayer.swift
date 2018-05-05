//
//  MainPlayer.swift
//  ARFighter
//
//  Created by Jones on 2018/5/1.
//  Copyright © 2018年 tjwd. All rights reserved.
//
import SceneKit
import ARKit

enum RemotingMethodId: Int32 {
    case Welcome = 1
    case SetupWorld
    case SyncCamera
    case CreateSceneModel
}

enum PlayerState {
    case Initial
    case SearchOrigin
    case SceneLoading
    case ScenePlaying
}

class MainPlayer: Player {
    
    override init(scene: GameScene) {
        super.init(scene: scene)
    }
    
    func initPlayer(playerId: Int64) {
        id = playerId
        
        state = .SearchOrigin
        // for test
//        server_requireScene(identityName: "test01")
    }
    
    func updateWorldOrigin(imageName: String, transform: matrix_float4x4) {
        let pos = transform.columns.3
        print("init world at: \(pos.x),\(pos.y),\(pos.z)")
        // let trans = GLKMatrix4RotateX(mat, -Float.pi * 0.5)
        parentScene?.session?.setWorldOrigin(relativeTransform: transform)
        
        if state == .SearchOrigin {
            state = .SceneLoading
            server_requireScene(identityName: imageName)
        }
    }
    
    func setupWorld(identityName: String, models: [SceneModelInfo]){
        for smi in models {
            parentScene?.addModel(creatorId: 1, type: .标注_圆点, pos: smi.pos, scale: smi.scale, rotate: smi.rotate)
        }
        state = .ScenePlaying
    }
    
    func createModel(type: SceneModel.Kind, pos: simd_float3) {
        let scale = simd_float3(1)
        let rotate = simd_float4(0)
        parentScene?.addModel(creatorId: id, type: type, pos: pos, scale: scale, rotate: rotate)
        server_createSceneModel(type: type, pos: pos, scale: scale, rotate: rotate)
    }
    
    override func tick(spanTime: Double) {
        if state == .ScenePlaying {
            if parentScene!.session!.currentFrame != nil {
                cameraTransform = parentScene!.session!.currentFrame!.camera.transform
                server_syncCamera(mat: cameraTransform)
            }
        }
    }
    
    func server_requireScene(identityName: String) {
        SocketClient.instance.sendMessage(cmd: .SetupWorld, context: [identityName])
    }
    
    func server_syncCamera(mat: matrix_float4x4) {
        SocketClient.instance.sendMessage(cmd: .SyncCamera, context: [mat.toJson()])
    }
    
    func server_createSceneModel(type: SceneModel.Kind, pos: simd_float3, scale: simd_float3, rotate: simd_float4){
        let dic: [Any] = [
            type.rawValue,
            pos.toJson(),
            scale.toJson(),
            rotate.toJson()
        ]
        SocketClient.instance.sendMessage(cmd: .CreateSceneModel, context: dic)
    }
    
    var state = PlayerState.Initial
}
