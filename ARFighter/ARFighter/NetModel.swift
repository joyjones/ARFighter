//
//  NetModel.swift
//  ARFighter
//
//  Created by Jones on 2018/4/27.
//  Copyright © 2018年 tjwd. All rights reserved.
//

import SceneKit
//import ObjectMapper

class GameObject {
    
    var id: Int64 = generateAutoId()
    
    static func generateAutoId() -> Int64 {
        let ct = Calendar.current
        let coms = ct.dateComponents([.year, .month, .day, .hour, .minute, .second], from: Date())
        let tail = arc4random_uniform(100000)
        let idstr = String(format: "%04d%02d%02d%02d%02d%02d%05d", coms.year!, coms.month!, coms.day!, coms.hour!, coms.minute!, coms.second!, tail)
        return Int64(idstr)!
    }
}

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

//struct NMMatrix: Mappable {
//    var values: [Float] = [0,0,0,0, 0,0,0,0, 0,0,0,0, 0,0,0,0]
//
//    init?(map: Map){
//    }
//
//    init(mat: matrix_float4x4){
//        for r in 0...3 {
//            for c in 0...3 {
//                values[r * 4 + c] = mat[r][c]
//            }
//        }
//    }
//
//    mutating func toMat() -> matrix_float4x4 {
//        var mat = matrix_float4x4();
//        for r in 0...3 {
//            for c in 0...3 {
//                mat[r][c] = values[r * 4 + c]
//            }
//        }
//        return mat
//    }
//
//    mutating func mapping(map: Map){
//        values <- map["values"]
//    }
//}

//struct MatrixTrans {
//    var rows: [float4]
//
//    init(){
//        rows = [float4]()
//    }
//    init(mat: matrix_float4x4) {
//        rows = [mat.columns.0, mat.columns.1, mat.columns.2, mat.columns.3]
//    }
//    init(rows: [float4]){
//        self.rows = rows
//    }
//
//    func toString() {
//
//    }
//    static func fromString(str: String) -> MatrixTrans {
//        return MatrixTrans()
//    }
//    func toBytes() -> [UInt8] {
//        let data = Data(buffer: UnsafeBufferPointer(start: rows, count: 4))
//        return [UInt8](data)
//    }
//    static func fromBytes(data: [UInt8]) -> MatrixTrans {
//        let data = Data(bytes: data)
//        let value: [float4] = data.withUnsafeBytes { $0.pointee }
//        return MatrixTrans(rows: value)
//    }
//}


//extension MatrixTrans: CerealType {
//    private struct Keys {
//        static let rows = "rows"
//    }
//
//    init(decoder: CerealDecoder) throws {
//        rows = try decoder.decode(key: Keys.rows) as! [float4]
//    }
//
//    func encodeWithCereal(_ encoder: inout CerealEncoder) throws {
//        try encoder.encode(rows as! IdentifyingCerealType, forKey: Keys.rows)
//    }
//}


//
//extension MatrixTrans: JSONEncodable{
//    func toJSON() throws -> Any {
//        return try JSONEncoder.create({(encoder) -> Void in
//            try encoder.encode(mat.columns.0.x, key: "m00")
//            try encoder.encode(mat.columns.0.y, key: "m01")
//            try encoder.encode(mat.columns.0.z, key: "m02")
//            try encoder.encode(mat.columns.0.w, key: "m03")
//            try encoder.encode(mat.columns.1.x, key: "m10")
//            try encoder.encode(mat.columns.1.y, key: "m11")
//            try encoder.encode(mat.columns.1.z, key: "m12")
//            try encoder.encode(mat.columns.1.w, key: "m13")
//            try encoder.encode(mat.columns.2.x, key: "m20")
//            try encoder.encode(mat.columns.2.y, key: "m21")
//            try encoder.encode(mat.columns.2.z, key: "m22")
//            try encoder.encode(mat.columns.2.w, key: "m23")
//            try encoder.encode(mat.columns.3.x, key: "m30")
//            try encoder.encode(mat.columns.3.y, key: "m31")
//            try encoder.encode(mat.columns.3.z, key: "m32")
//            try encoder.encode(mat.columns.3.w, key: "m33")
//        })
//    }
//}
//
//extension MatrixTrans: JSONDecodable{
//    init(object: JSONObject) throws {
//        let decoder = JSONDecoder(object: object)
//        let m0 = float4([
//            try decoder.decode("m00"),
//            try decoder.decode("m01"),
//            try decoder.decode("m02"),
//            try decoder.decode("m03")
//        ])
//        let m1 = float4([
//            try decoder.decode("m10"),
//            try decoder.decode("m11"),
//            try decoder.decode("m12"),
//            try decoder.decode("m13")
//        ])
//        let m2 = float4([
//            try decoder.decode("m20"),
//            try decoder.decode("m21"),
//            try decoder.decode("m22"),
//            try decoder.decode("m23")
//        ])
//        let m3 = float4([
//            try decoder.decode("m30"),
//            try decoder.decode("m31"),
//            try decoder.decode("m32"),
//            try decoder.decode("m33")
//        ])
//        mat = matrix_float4x4([m0, m1, m2, m3])
//    }
//}
