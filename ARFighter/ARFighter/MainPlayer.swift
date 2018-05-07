//
//  MainPlayer.swift
//  ARFighter
//
//  Created by Jones on 2018/5/1.
//  Copyright © 2018年 tjwd. All rights reserved.
//
import SceneKit
import ARKit

class MainPlayer: Player {
    
    override init(scene: GameScene) {
        super.init(scene: scene)
    }
    
    override func initPlayer(info: PlayerInfo) {
        super.initPlayer(info: info)
        state = .SearchOrigin
        cameraModel?.visible = false
        // for test
//        server_requireScene(identityName: "test02")
    }
    
    func updateWorldOrigin(imageName: String, transform: matrix_float4x4) {
        if imageName.count == 0 || state == .Initial {
            return
        }
        if state == .ScenePlaying && parentScene?.startupMarker != nil {
            worldTransform = transformWorldOrigin(transform: transform, startupMarker: parentScene!.startupMarker!)
        }
        else {
            worldTransform = transform
        }

        parentScene?.session?.setWorldOrigin(relativeTransform: worldTransform!)
        print("init world at: \(transform.columns.3.x),\(transform.columns.3.y),\(transform.columns.3.z)")

        if state == .SearchOrigin {
            state = .SceneLoading
            server_requireScene(identityName: imageName)
        }
    }
    
    func setupWorldOrigin(startupMarker: SceneMarker) {
        if state != .SceneLoading {
            return
        }
        worldTransform = transformWorldOrigin(transform: worldTransform!, startupMarker: startupMarker)
        parentScene?.session?.setWorldOrigin(relativeTransform: worldTransform!)
        
        state = .ScenePlaying
    }
    
    private func transformWorldOrigin(transform: matrix_float4x4, startupMarker: SceneMarker) -> matrix_float4x4 {
        var transform = transform
        let rot = startupMarker.originRotation
        if rot.x != 0 {
            let rotatedTrans = GLKMatrix4RotateX(transform.toGLK(), Float.pi * (rot.x / 180.0))
            transform = matrix_float4x4.fromGLK(rotatedTrans)
        }
        if rot.y != 0 {
            let rotatedTrans = GLKMatrix4RotateY(transform.toGLK(), Float.pi * (rot.y / 180.0))
            transform = matrix_float4x4.fromGLK(rotatedTrans)
        }
        if rot.z != 0 {
            let rotatedTrans = GLKMatrix4RotateZ(transform.toGLK(), Float.pi * (rot.z / 180.0))
            transform = matrix_float4x4.fromGLK(rotatedTrans)
        }
        return transform
    }
    
    func createModel(templateId: Int64, pos: simd_float3) {
        if parentScene == nil || !parentScene!.isReadyForPlay {
            return
        }
        if templateId == 3 && parentScene?.controllingCharacter != nil {
            return
        }
        let smi = SceneModelInfo()
        smi.create_player_id = id
        smi.model_id = templateId
        smi.pos = pos
        smi.rotation = simd_float3(0)
        smi.scale = simd_float3(templateId == 3 ? 0.005 : 0.05)
        if let model = parentScene?.addModel(info: smi) {
            server_createSceneModel(model: model)
        }
    }
    
    func deleteModel(model: SceneModel) {
        if !parentScene!.removeModel(playerId: id, modelId: model.id) {
            return
        }
        SocketClient.instance.sendMessage(cmd: .DeleteSceneModel, context: [model.id])
    }
    
    func moveModel(model: SceneModel, pos: simd_float3) {
        model.position = pos
        SocketClient.instance.sendMessage(cmd: .MoveSceneModel, context: [model.id, pos.toJson()])
    }
    
    func scaleModel(model: SceneModel, scale: simd_float3) {
        model.scale = scale
        SocketClient.instance.sendMessage(cmd: .ScaleSceneModel, context: [model.id, scale.toJson()])
    }
    
    func rotateModel(model: SceneModel, rotation: simd_float3) {
        model.rotation = rotation
        SocketClient.instance.sendMessage(cmd: .RotateSceneModel, context: [model.id, rotation.toJson()])
    }
    
    override func tick(spanTime: Double) {
        _elapsedTime += spanTime
        if state == .ScenePlaying {
            if parentScene!.session!.currentFrame != nil && _elapsedTime - _lastSyncCameraTick > 0 {
                cameraTransform = parentScene!.session!.currentFrame!.camera.transform
                let newPos = cameraPos
                if _prevCameraPos == nil || _prevCameraPos!.x != newPos.x || _prevCameraPos!.y != newPos.y || _prevCameraPos!.z != newPos.z {
                    _prevCameraPos = newPos
                    server_syncPlayerState()
                    _lastSyncCameraTick = _elapsedTime
                }
                
//                parentScene!.session!.currentFrame!.camera.eulerAngles
//                let g = GLKMatrix4MakeWithQuaternion()
            }
//            // for test
//            cameraModel?.position = cameraPos
        }
        else if state == .SceneLoading {
            if _elapsedTime > 5 {
                state = .SearchOrigin
            }
        }
    }
    
    func server_requireScene(identityName: String) {
        SocketClient.instance.sendMessage(cmd: .SetupWorld, context: [identityName])
    }
    
    func server_syncPlayerState() {
        let rot = simd_float3(0, 0, 0)
        SocketClient.instance.sendMessage(cmd: .SyncPlayerState, context: [cameraPos.toJson(), rot.toJson()])
    }
    
    func server_createSceneModel(model: SceneModel){
        SocketClient.instance.sendMessage(cmd: .CreateSceneModel, context: [model.toJson()])
    }
    
    var worldTransform: matrix_float4x4?
    private var _state: PlayerState = .Initial
    private var _elapsedTime: Double = 0
    private var _lastSyncCameraTick: Double = 0
    private var _prevCameraPos: simd_float3?
    var state: PlayerState {
        get { return _state }
        set {
            if _state != newValue {
                _state = newValue
                _elapsedTime = 0
                parentScene!.delegateMsg?.updatePlayerState(name: info!.nickname, state: _state)
            }
        }
    }
}
