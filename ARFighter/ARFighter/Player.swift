//
//  Player.swift
//  ARFighter
//
//  Created by Jones on 2018/5/1.
//  Copyright © 2018年 tjwd. All rights reserved.
//
import SceneKit
import ARKit

class PlayerInfo {
    var id: Int64 = 0
    var nickname: String?
    var account: String?
    var uniqueDeviceId: String?
    var accessToken: String?
    var lastLoginTime: Int32?
    
    static func fromJson(json: [String: Any]) -> PlayerInfo {
        let pi = PlayerInfo()
        pi.id = json["id"] as! Int64
        pi.nickname = json["nickname"] as? String
        pi.account = json["account"] as? String
        pi.uniqueDeviceId = json["unique_device_id"] as? String
        pi.lastLoginTime = json["last_login_time"] as? Int32
        pi.accessToken = json["access_token"] as? String
        return pi
    }
}

class Player: GameObject {
    init(scene: GameScene) {
        parentScene = scene
    }
    
    func initPlayer(info: PlayerInfo) {
        self.info = info
        id = info.id
    }
    
    func syncCamera(mat: matrix_float4x4) {
        cameraTransform = mat
    }
    
    func tick(spanTime: Double) {

    }
    
    var info: PlayerInfo?
    var parentScene: GameScene?
    var cameraTransform = matrix_float4x4()
}
