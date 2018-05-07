//
//  SceneMarker.swift
//  ARFighter
//
//  Created by Jones on 2018/5/7.
//  Copyright © 2018年 tjwd. All rights reserved.
//
import SceneKit

class SceneMarkerInfo {
    var id: Int64 = GameObject.generateAutoId()
    var scene_id: Int64?
    var marker_id: Int64?
    var scale: Float = 1
    var offset_x: Float = 0
    var offset_y: Float = 0
    var offset_z: Float = 0
    var rotation_x: Float = 0
    var rotation_y: Float = 0
    var rotation_z: Float = 0
    
    static func fromJson(json: [String: Any]) -> SceneMarkerInfo {
        let smi = SceneMarkerInfo()
        smi.id = json["id"] as! Int64
        smi.scene_id = json["scene_id"] as? Int64
        smi.marker_id = json["marker_id"] as? Int64
        smi.offset_x = Float(json["offset_x"] as! Double)
        smi.offset_y = Float(json["offset_y"] as! Double)
        smi.offset_z = Float(json["offset_z"] as! Double)
        smi.rotation_x = Float(json["rotation_x"] as! Double)
        smi.rotation_y = Float(json["rotation_y"] as! Double)
        smi.rotation_z = Float(json["rotation_z"] as! Double)
        return smi
    }
}

class SceneMarker: GameObject {
    
    init(info: SceneMarkerInfo, scene: GameScene) {
        self.markerId = info.marker_id!
        self.parentScene = scene
        self.scale = info.scale
        self.originOffset = simd_float3(info.offset_x, info.offset_y, info.offset_z)
        self.originRotation = simd_float3(info.rotation_x, info.rotation_y, info.rotation_z)
        
        super.init()
        
        id = info.id
    }
    
    let markerId: Int64
    let parentScene: GameScene
    let scale: Float
    let originOffset: simd_float3
    let originRotation: simd_float3
    
}
