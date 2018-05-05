//
//  SceneModel.swift
//  ARFighter
//
//  Created by Jones on 2018/5/2.
//  Copyright © 2018年 tjwd. All rights reserved.
//
import SceneKit

class SceneModelInfo {
    var pos = simd_float3()
    var scale = simd_float3()
    var rotate = simd_float4()
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
        case 角色 = 100
    }

    init(type: Kind, scene: GameScene, creator: Int64) {
        self.type = type
        self.parentScene = scene
        self.authorPlayerId = creator
        
        geometry = SCNSphere(radius: 0.1)
        sceneNode = SCNNode(geometry: geometry)
        self.parentScene.rootNode.addChildNode(sceneNode!)
    }
    
    let type: Kind
    let parentScene: GameScene
    let authorPlayerId: Int64
    var geometry: SCNGeometry?
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
    var rotation: simd_float4 {
        get { return sceneNode!.simdRotation }
        set {
            sceneNode!.simdRotation = newValue
        }
    }
}

