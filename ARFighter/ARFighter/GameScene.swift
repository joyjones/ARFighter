//
//  GameScene.swift
//  ARFighter
//
//  Created by Jones on 2018/4/29.
//  Copyright © 2018年 tjwd. All rights reserved.
//
import SceneKit
import ARKit

class GameScene: SCNScene, RemotingMethodDelegate {
    
    enum States {
        case Offline
        case Login
        case InScene
        case SceneEdit
    }
    
    init(session: ARSession) {
        super.init()
        self.session = session
        initialize()
    }
    
    required init?(coder aDecoder: NSCoder) {
        super.init(coder: aDecoder)
        initialize()
    }
    
    private func initialize(){
        mainPlayer = MainPlayer(scene: self)
        SocketClient.instance.delegateMethods = self
        enableTimer(enabled: true)
    }
    
    func enableTimer(enabled: Bool) {
        if enabled {
            timer = Timer.scheduledTimer(timeInterval: tickInterval, target: self, selector: (#selector(GameScene.tick)), userInfo: nil, repeats: true)
        }
        else{
            timer?.invalidate()
            timer = nil
        }
    }
    
    @objc func tick() {
        mainPlayer!.tick(spanTime: tickInterval)
    }
    
    func invokeMethod(methodId: RemotingMethodId, args: [Any]) {
        switch methodId {
        case .Welcome:
            mainPlayer!.initPlayer(playerId: args[0] as! Int64)
        case .SetupWorld:
            let ms = args[1]
            mainPlayer!.setupWorld(identityName: args[0] as! String, models: [SceneModelInfo]())
        case .SyncCamera:
            let pid = args[0] as! Int64
            players[pid]?.cameraTransform = matrix_float4x4.fromJson(json: args[1] as! [String: [Float]])
        case .CreateSceneModel:
            addModel(
                creatorId: args[0] as! Int64,
                type: SceneModel.Kind(rawValue: args[1] as! Int32)!,
                pos: simd_float3.fromJson(json: args[2] as! [String: Float]),
                scale: simd_float3.fromJson(json: args[3] as! [String: Float]),
                rotate: simd_float4.fromJson(json: args[4] as! [String: Float])
            )
        }
    }
    
    func addModel(creatorId: Int64, type: SceneModel.Kind, pos: simd_float3, scale: simd_float3, rotate: simd_float4) {
        let model = SceneModel(type: type, scene: self, creator: creatorId)
        model.position = pos
        model.scale = scale
        model.rotation = rotate
        models[model.id] = model
    }
    
    var session: ARSession?
    let tickInterval = 0.1
    var timer: Timer?
    var mainPlayer: MainPlayer?
    var players = [Int64: Player]()
    var models = [Int64: SceneModel]()
}
