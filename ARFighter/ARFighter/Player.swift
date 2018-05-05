//
//  Player.swift
//  ARFighter
//
//  Created by Jones on 2018/5/1.
//  Copyright © 2018年 tjwd. All rights reserved.
//
import SceneKit
import ARKit

class Player: GameObject {

    var parentScene: GameScene?
    var cameraTransform = matrix_float4x4()
    
    init(scene: GameScene) {
        parentScene = scene
    }
    
    func syncCamera(mat: matrix_float4x4) {
        cameraTransform = mat
    }
    
    func tick(spanTime: Double) {

    }
}
