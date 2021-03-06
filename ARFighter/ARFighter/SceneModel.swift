//
//  SceneModel.swift
//  ARFighter
//
//  Created by Jones on 2018/5/2.
//  Copyright © 2018年 tjwd. All rights reserved.
//
import SceneKit

class SceneModelInfo {
    var id: Int64 = GameObject.generateAutoId()
    var scene_id: Int64?
    var model_id: Int64? = 1
    var create_player_id: Int64?
    var description: String?
    var pos_x: Float = 0
    var pos_y: Float = 0
    var pos_z: Float = 0
    var scale_x: Float = 1
    var scale_y: Float = 1
    var scale_z: Float = 1
    var rotation_x: Float = 0
    var rotation_y: Float = 0
    var rotation_z: Float = 0
    
    var pos: simd_float3 {
        get { return simd_float3(pos_x, pos_y, pos_z) }
        set { pos_x = newValue.x; pos_y = newValue.y; pos_z = newValue.z }
    }
    var scale: simd_float3 {
        get { return simd_float3(scale_x, scale_y, scale_z) }
        set { scale_x = newValue.x; scale_y = newValue.y; scale_z = newValue.z }
    }
    var rotation: simd_float3 {
        get { return simd_float3(rotation_x, rotation_y, rotation_z) }
        set { rotation_x = newValue.x; rotation_y = newValue.y; rotation_z = newValue.z }
    }
    static func fromJson(json: [String: Any]) -> SceneModelInfo {
        let smi = SceneModelInfo()
        smi.id = json["id"] as! Int64
        smi.scene_id = json["scene_id"] as? Int64
        smi.model_id = json["model_id"] as? Int64
        smi.create_player_id = json["create_player_id"] as? Int64
        smi.description = json["description"] as? String
        smi.pos_x = Float(json["pos_x"] as! Double)
        smi.pos_y = Float(json["pos_y"] as! Double)
        smi.pos_z = Float(json["pos_z"] as! Double)
        smi.scale_x = Float(json["scale_x"] as! Double)
        smi.scale_y = Float(json["scale_y"] as! Double)
        smi.scale_z = Float(json["scale_z"] as! Double)
        smi.rotation_x = Float(json["rotation_x"] as! Double)
        smi.rotation_y = Float(json["rotation_y"] as! Double)
        smi.rotation_z = Float(json["rotation_z"] as! Double)
        return smi
    }
}

class SceneModel: GameObject {
    
    enum Kind: Int32 {
        case 标注_圆点 = 0
        case 标注_圆圈
        case 标注_箭头
        case 标注_连线
        case 平面_贴图 = 10
        case 平面_视频
        case 平面_挡板
        case 音效 = 20
        case 特效
        case 按钮触发器
        case 玩家 = 100
        case 角色
    }
    
    enum State: Int32 {
        case Normal
        case Selected
    }

    init(info: SceneModelInfo, scene: GameScene) {
        self.modelId = info.model_id!
        self.parentScene = scene
        self.authorPlayerId = info.create_player_id
        
        super.init()
        
        initGeometry()
        id = info.id
        position = info.pos
        scale = info.scale
        rotation = info.rotation
    }
    
    func initGeometry() {
        switch modelId {
        case 1:
            sceneNode = SCNNode(geometry: SCNSphere(radius: 1))
        case 2:
            sceneNode = SCNNode(geometry: SCNCylinder(radius: 0.5, height: 1))
        case 3:
            sceneNode = Character(model: self, ModelFile: "art.scnassets/idle.dae")
        default:
            return
        }
        
        parentScene.rootNode.addChildNode(sceneNode!)
    }
    
    func detach() {
        sceneNode?.removeFromParentNode()
    }
    
    func attach() {
        parentScene.rootNode.addChildNode(sceneNode!)
    }
    
    func tick() {
        if let ch = sceneNode as? Character {
            ch.update(updateAtTime: 0.025, camera: parentScene.session?.currentFrame?.camera)
        }
    }
    
    let modelId: Int64
    let parentScene: GameScene
    let authorPlayerId: Int64?
    var sceneNode: SCNNode?
    
    var position: simd_float3 {
        get { return sceneNode!.simdPosition }
        set {
            sceneNode!.simdPosition = newValue
        }
    }
    var scale: simd_float3 {
        get { return sceneNode!.simdScale }
        set {
            sceneNode!.simdScale = newValue
        }
    }
    var rotation: simd_float3 {
        get {
            let rot = sceneNode!.simdRotation
            return simd_float3(rot.x, rot.y, rot.z)
        }
        set {
            sceneNode!.simdRotation = simd_float4(newValue.x, newValue.y, newValue.z, 1)
        }
    }
    
    private var _state = State.Normal
    var state: State {
        get { return _state }
        set {
            if _state != newValue {
                _state = newValue
                if _state == .Normal {
                    sceneNode?.geometry?.firstMaterial?.diffuse.contents = UIColor.white
                }
                else if _state == .Selected {
                    sceneNode?.geometry?.firstMaterial?.diffuse.contents = UIColor.orange
                }
            }
        }
    }
    var visible: Bool {
        get { return sceneNode != nil && !sceneNode!.isHidden }
        set { sceneNode?.isHidden = !newValue }
    }
    
    func toJson() -> [String: Any] {
        var kv = [String: Any]()
        kv["id"] = id
        kv["scene_id"] = parentScene.info!.id!
        kv["model_id"] = modelId
        kv["create_player_id"] = authorPlayerId
        kv["description"] = ""
        kv["pos_x"] = position.x
        kv["pos_y"] = position.y
        kv["pos_z"] = position.z
        kv["scale_x"] = scale.x
        kv["scale_y"] = scale.y
        kv["scale_z"] = scale.z
        kv["rotation_x"] = rotation.x
        kv["rotation_y"] = rotation.y
        kv["rotation_z"] = rotation.z
        return kv
    }
}

