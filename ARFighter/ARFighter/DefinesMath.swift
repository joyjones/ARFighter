//
//  MathHelper.swift
//  ARFighter
//
//  Created by Jones on 2018/4/27.
//  Copyright © 2018年 tjwd. All rights reserved.
//

import SceneKit

extension matrix_float4x4{
    func toJson() -> [String: [Float]]{
        return [
            "r1": [columns.0.x, columns.0.y, columns.0.z, columns.0.w],
            "r2": [columns.1.x, columns.1.y, columns.1.z, columns.1.w],
            "r3": [columns.2.x, columns.2.y, columns.2.z, columns.2.w],
            "r4": [columns.3.x, columns.3.y, columns.3.z, columns.3.w],
        ]
    }
    static func fromJson(json: [String: [Float]]) -> matrix_float4x4 {
        var mat = matrix_float4x4()
        var ary = json["r1"]!
        mat.columns.0 = float4(ary[0], ary[1], ary[2], ary[3])
        ary = json["r2"]!
        mat.columns.1 = float4(ary[0], ary[1], ary[2], ary[3])
        ary = json["r3"]!
        mat.columns.2 = float4(ary[0], ary[1], ary[2], ary[3])
        ary = json["r4"]!
        mat.columns.3 = float4(ary[0], ary[1], ary[2], ary[3])
        return mat
    }
    func toGLK() -> GLKMatrix4{
        var gm = GLKMatrix4()
        gm.m00 = columns.0.x
        gm.m01 = columns.0.y
        gm.m02 = columns.0.z
        gm.m03 = columns.0.w
        
        gm.m10 = columns.1.x
        gm.m11 = columns.1.y
        gm.m12 = columns.1.z
        gm.m13 = columns.1.w
        
        gm.m20 = columns.2.x
        gm.m21 = columns.2.y
        gm.m22 = columns.2.z
        gm.m23 = columns.2.w
        
        gm.m30 = columns.3.x
        gm.m31 = columns.3.y
        gm.m32 = columns.3.z
        gm.m33 = columns.3.w
        return gm;
    }
    
    static func fromGLK(_ mat: GLKMatrix4) -> matrix_float4x4 {
        return float4x4(
            float4(mat.m00,mat.m01,mat.m02,mat.m03),
            float4(mat.m10,mat.m11,mat.m12,mat.m13),
            float4(mat.m20,mat.m21,mat.m22,mat.m23),
            float4(mat.m30,mat.m31,mat.m32,mat.m33)
        );
    }
}

extension simd_float3 {
    static func fromJson(json: [String: Any]) -> simd_float3 {
        return simd_float3(Float(json["x"] as! Double), Float(json["y"] as! Double), Float(json["z"] as! Double))
    }
    func toJson() -> [String: Float] {
        return ["x": x, "y": y, "z": z]
    }
}

extension simd_float4 {
    static func fromJson(json: [String: Any]) -> simd_float4 {
        return simd_float4(Float(json["z"] as! Double), Float(json["y"] as! Double), Float(json["z"] as! Double), Float(json["w"] as! Double))
    }
    func toJson() -> [String: Float] {
        return ["x": x, "y": y, "z": z, "w": w]
    }
}
