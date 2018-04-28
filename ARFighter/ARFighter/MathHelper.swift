//
//  MathHelper.swift
//  ARFighter
//
//  Created by Jones on 2018/4/27.
//  Copyright © 2018年 tjwd. All rights reserved.
//

import SceneKit

class MathHelper {
    static func toString(_ matrix: matrix_float4x4) -> String {
        var rst = "";
        var r = matrix.columns.0
        rst += "\(r.x),\(r.y),\(r.z),\(r.w),";
        r = matrix.columns.1
        rst += "\(r.x),\(r.y),\(r.z),\(r.w),";
        r = matrix.columns.2
        rst += "\(r.x),\(r.y),\(r.z),\(r.w),";
        r = matrix.columns.3
        rst += "\(r.x),\(r.y),\(r.z),\(r.w),";
        return rst;
    }
}
