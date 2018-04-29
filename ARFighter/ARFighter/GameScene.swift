//
//  GameScene.swift
//  ARFighter
//
//  Created by Jones on 2018/4/29.
//  Copyright © 2018年 tjwd. All rights reserved.
//
import SceneKit

class GameScene: SCNScene {
    
    func insertGeometry(_ x: Float, _ y: Float, _ z: Float) {
        let sphere = SCNSphere()
        sphere.radius = 0.1
        let node = SCNNode(geometry: sphere)
        node.simdPosition = simd_float3(x, y, z)
        rootNode.addChildNode(node)
    }
}
