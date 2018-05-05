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
    case Welcome = 0x01
    case SetupWorld
    case SyncCamera
    case CreateSceneModel = 0x10
    case MoveSceneModel
    case ScaleSceneModel
    case RotateSceneModel
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
    
    func createModel(templateId: Int64, pos: simd_float3) {
        if parentScene == nil || !parentScene!.isReadyForPlay {
            return
        }
        let smi = SceneModelInfo()
        smi.create_player_id = id
        smi.model_id = templateId
        smi.pos = pos
        smi.rotation = simd_float3(0)
        smi.scale = simd_float3(1)
        if let model = parentScene?.addModel(info: smi) {
            server_createSceneModel(model: model)
        }
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
    
    func server_createSceneModel(model: SceneModel){
        SocketClient.instance.sendMessage(cmd: .CreateSceneModel, context: [model.toJson()])
    }
    
    var state = PlayerState.Initial
}
