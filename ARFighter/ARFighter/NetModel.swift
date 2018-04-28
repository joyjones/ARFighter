//
//  NetModel.swift
//  ARFighter
//
//  Created by Jones on 2018/4/27.
//  Copyright © 2018年 tjwd. All rights reserved.
//

import SceneKit
import ObjectMapper

//import JSONCodable
//import Cereal

enum ProtoType: Int {
    case SyncCamera = 1
    case CreateObject
    case TransformObject
}

extension float4 {
    func toString() -> String {
        return "\(x),\(y),\(z),\(w)"
    }
    static func fromString(str: String) -> float4 {
        let ss = str.split(separator: ",")
        return float4([(ss[0] as NSString).floatValue,
                       (ss[1] as NSString).floatValue,
                       (ss[2] as NSString).floatValue,
                       (ss[3] as NSString).floatValue])
    }
}

extension matrix_float4x4{
    func toString() -> String{
        return "\(columns.0.toString());\(columns.1.toString());\(columns.2.toString());\(columns.3.toString())"
    }
    static func fromString(str: String) -> matrix_float4x4 {
        let ss = str.split(separator: ";")
        var mat = matrix_float4x4()
        mat.columns.0 = float4.fromString(str: String(ss[0]))
        mat.columns.1 = float4.fromString(str: String(ss[0]))
        mat.columns.2 = float4.fromString(str: String(ss[0]))
        mat.columns.3 = float4.fromString(str: String(ss[0]))
        return mat
    }
}

struct NMMatrix: Mappable {
    var values: [Float] = [0,0,0,0, 0,0,0,0, 0,0,0,0, 0,0,0,0]
    
    init?(map: Map){
    }
    
    init(mat: matrix_float4x4){
        for r in 0...3 {
            for c in 0...3 {
                values[r * 4 + c] = mat[r][c]
            }
        }
    }
    
    mutating func toMat() -> matrix_float4x4 {
        var mat = matrix_float4x4();
        for r in 0...3 {
            for c in 0...3 {
                mat[r][c] = values[r * 4 + c]
            }
        }
        return mat
    }
    
    mutating func mapping(map: Map){
        values <- map["values"]
    }
}

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
