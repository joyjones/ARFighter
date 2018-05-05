//
//  GameScene.swift
//  ARFighter
//
//  Created by Jones on 2018/4/29.
//  Copyright © 2018年 tjwd. All rights reserved.
//
import SceneKit
import ARKit

class SceneInfo {
    var id: Int64?
    var name: String?
    
    static func fromJson(json: [String: Any]) -> SceneInfo {
        let info = SceneInfo()
        info.id = json["id"] as? Int64
        info.name = json["name"] as? String
        return info
    }
}


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
    
    var isReadyForPlay: Bool {
        return info != nil
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
            startupName = args[0] as? String
            info = SceneInfo.fromJson(json: args[1] as! [String: Any])
            let json = args[2] as! [Any]
            for mi in json {
                let info = SceneModelInfo.fromJson(json: mi as! [String: Any])
                models[info.id!] = SceneModel(info: info, scene: self)
            }
            mainPlayer!.state = .ScenePlaying
        case .SyncCamera:
            let pid = args[0] as! Int64
            players[pid]?.cameraTransform = matrix_float4x4.fromJson(json: args[1] as! [String: [Float]])
        case .CreateSceneModel:
            _ = addModel(info: SceneModelInfo.fromJson(json: args[0] as! [String: Any]))
        case .MoveSceneModel:
            transformModel(playerId: args[0] as! Int64, modelId: args[1] as! Int64,
                           pos: simd_float3.fromJson(json: args[2] as! [String: Any]), scale: nil, rotation: nil)
        case .ScaleSceneModel:
            transformModel(playerId: args[0] as! Int64, modelId: args[1] as! Int64,
                           pos: nil, scale: simd_float3.fromJson(json: args[2] as! [String: Any]), rotation: nil)
        case .RotateSceneModel:
            transformModel(playerId: args[0] as! Int64, modelId: args[1] as! Int64,
                           pos: nil, scale: nil, rotation: simd_float3.fromJson(json: args[2] as! [String: Any]))
        }
    }
    
    func addModel(info: SceneModelInfo) -> SceneModel {
        let model = SceneModel(info: info, scene: self)
        models[model.id] = model
        return model
    }
    
    func transformModel(playerId: Int64, modelId: Int64, pos: simd_float3?, scale: simd_float3?, rotation: simd_float3?) {
        if let model = models[modelId] {
            if pos != nil {
                model.position = pos!
            }
            if scale != nil {
                model.scale = scale!
            }
            if rotation != nil {
                model.rotation = rotation!
            }
        }
    }
    
    var info: SceneInfo?
    var startupName: String?
    var session: ARSession?
    let tickInterval = 0.1
    var timer: Timer?
    var mainPlayer: MainPlayer?
    var players = [Int64: Player]()
    var models = [Int64: SceneModel]()
}
