//
//  SocketClient.swift
//  ARFighter
//
//  Created by Jones on 2018/5/1.
//  Copyright © 2018年 tjwd. All rights reserved.
//

import SwiftSocket

protocol NetworkDelegate {
    func appendLog(msg: String)
    func alert(msg: String)
}

protocol RemotingMethodDelegate {
    func invokeMethod(methodId id: Int32, jsonArgs args: [Any])
}

class SocketClient {
    
    static let instance = SocketClient()
    private var client: TCPClient?
    private(set) var isConnected = false
    var delegate: NetworkDelegate?
    var delegateMethods: RemotingMethodDelegate?
    
    func connectServer(address: String, port: Int32){
        client = TCPClient(address: address, port: port)
        
        switch client!.connect(timeout: 5) {
        case .success:
            isConnected = true
            DispatchQueue.main.async {
                self.delegate?.alert(msg: "连接成功！")
                self.beginReadMessage()
            }
        case .failure(let error):
            DispatchQueue.main.async {
                self.delegate?.alert(msg: error.localizedDescription)
            }
        }
    }
    
    private func disconnect(){
        client?.close()
        isConnected = false
    }
    
    private func beginReadMessage() {
        if var bytes = client!.read(1024, timeout: 1) {
            let data = Data(bytes: bytes)
            let methodId = Int32(bigEndian: data.withUnsafeBytes { $0.pointee })
            bytes.removeSubrange(1...4)
            let json = String(bytes: bytes, encoding: .utf8)!
            let args = try? JSONSerialization.jsonObject(with: json.data(using: .utf8)!, options: []) as! [Any]
            delegateMethods?.invokeMethod(methodId: methodId, jsonArgs: args!)
        }
        beginReadMessage()
    }
    
    func sendMessage(cmd: RemotingMethodId, context: String){
        var type = UInt32(cmd.rawValue)
        var data = [UInt8](Data(buffer: UnsafeBufferPointer(start: &type, count: 1)))
        data.append(contentsOf: Array(context.utf8))
        _ = client?.send(data: data)
    }
}
