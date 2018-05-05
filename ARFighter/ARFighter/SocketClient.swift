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
    func invokeMethod(methodId: RemotingMethodId, args: [Any])
}

class SocketClient {
    
    static let instance = SocketClient()
    private var client: TCPClient?
    private(set) var isConnected = false
    var delegate: NetworkDelegate?
    var delegateMethods: RemotingMethodDelegate?
    
    let updateQueue = DispatchQueue(label: Bundle.main.bundleIdentifier! + ".network")

    func connectServer(address: String, port: Int32){
        client = TCPClient(address: address, port: port)
        
        switch client!.connect(timeout: 5) {
        case .success:
            isConnected = true
            updateQueue.async {
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
        while isConnected {
            if var bytes = client!.read(1024, timeout: 1) {
                let data = Data(bytes: bytes)
                let methodId = RemotingMethodId(rawValue: data.withUnsafeBytes { $0.pointee })!
                bytes.removeSubrange(0...3)
                let json = String(bytes: bytes, encoding: .utf8)!
                let args = try? JSONSerialization.jsonObject(with: json.data(using: .utf8)!, options: []) as! [Any]
                
                print("+++ invoking local method: \(methodId), args: \(json)")
                delegateMethods?.invokeMethod(methodId: methodId, args: args!)
            }
        }
    }
    
    func sendMessage(cmd: RemotingMethodId, context: Any){
        let data = try? JSONSerialization.data(withJSONObject: context, options: [])
        let context = String(data: data!, encoding: .utf8)!
        var type = Int32(cmd.rawValue)
        var bytes = [UInt8](Data(buffer: UnsafeBufferPointer(start: &type, count: 1)))
        bytes.append(contentsOf: Array(context.utf8))
        
        if cmd != RemotingMethodId.SyncCamera {
            print("+++ invoking remote method: \(cmd), args: \(context)")
        }
        _ = client?.send(data: bytes)
    }
}
