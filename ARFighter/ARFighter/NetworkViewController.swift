//
//  NetworkViewController.swift
//  ARFighter
//
//  Created by Jones on 2018/4/26.
//  Copyright © 2018年 tjwd. All rights reserved.
//

import UIKit
import SceneKit
import SwiftSocket

class NetworkViewController: UIViewController {
    
    private var socketClient: TCPClient?
    private var socketServer: TcpSocketServer?
    
    private(set) var isConnected = false
    
    @IBOutlet weak var txbOutput: UITextView!
    @IBOutlet weak var btnConnectServer: UIButton!
    @IBOutlet weak var btnCreateServer: UIButton!
    @IBOutlet weak var btnStopServer: UIButton!
    
    @IBAction func connectServer(_ sender: UIButton) {
        connectServer()
    }
    @IBAction func startServer(_ sender: UIButton) {
        let url = "http://apis.juhe.cn/mobile/get?phone=13301388888&key=4e602dad4a05b4d491ffb82511613158"
        HttpHelper.Shared.Get(path: url, success: onSuccess, failure: onFail)
        
        return
        socketServer = TcpSocketServer()
        socketServer?.start()
    }
    @IBAction func stopServer(_ sender: UIButton) {
        disconnectServer()
    }
    
    func onSuccess(_ result: String)
    {
        DispatchQueue.main.async {
            self.txbOutput.text = result
        }
    }
    func onFail(_ result: Error)
    {
    }
    
    
    //用于读取并解析服务端发来的消息
    func readmsg() -> [String:Any]? {
        //read 4 byte int as type
        if let data = self.socketClient!.read(4) {
            if data.count == 4{
                let ndata = NSData(bytes: data, length: data.count)
                var len: Int32 = 0
                ndata.getBytes(&len, length: data.count)
                if let buff = self.socketClient!.read(Int(len)){
                    let msgd = Data(bytes: buff, count: buff.count)
                    if let msgi = try? JSONSerialization.jsonObject(with: msgd, options: .mutableContainers) {
                        return msgi as? [String:Any]
                    }
                }
            }
        }
        return nil
    }
    
    private func connectServer(){
        socketClient = TCPClient(address: "10.1.7.40", port: 8333)
        
//        DispatchQueue.global(qos: .background).async {
//
//
            //连接服务器
            switch self.socketClient!.connect(timeout: 5) {
            case .success:
                DispatchQueue.main.async {
                    self.alert(msg: "连接成功!", after: {})
                }
                
                self.isConnected = true
                let pv = parent as! ViewController
                pv.enableTimer(enabled: true)
                
//                var mt = MatrixTrans(mat: matrix_float4x4())
//                mt.mat.columns.0.x = 1
//                mt.mat.columns.1.y = 1
//                mt.mat.columns.2.z = 1
//                self.sendMessage(msg: mt.toJSON() as String?)
                
                //不断接收服务器发来的消息
//                while true{
//                    if let msg = readmsg(){
//                        DispatchQueue.main.async {
//                            self.processMessage(msg: msg)
//                        }
//                    }else{
////                        DispatchQueue.main.async {
////                            self.disconnectServer()
////                        }
////                        break
//                    }
//                }
            case .failure(let error):
                DispatchQueue.main.async {
                    self.alert(msg: error.localizedDescription,after: {
                    })
                }
            }
//        }
    }
    
    private func disconnectServer(){
        socketServer?.stop()
        isConnected = false
    }
    
    //发送消息
    public func sendMessage(cmd: ProtoType, data: String){
        var v = cmd.rawValue
        let t = Data(buffer: UnsafeBufferPointer(start: &v, count: 1))
        var dd = [UInt8](t)
        dd.append(contentsOf: Array(data.utf8))
        _ = socketClient?.send(data: dd)
    }
    
    //处理服务器返回的消息
    private func processMessage(msg: [String: Any]){
        let cmd = msg["cmd"] as! String
        switch(cmd){
        case "msg":
            txbOutput.text = txbOutput.text + (msg["from"] as! String) + ": " + (msg["content"] as! String) + "\n"
        default:
            print("received unknown cmd:" + cmd)
        }
    }
    
    //弹出消息框
    public func alert(msg:String,after:()->(Void)){
        let alertController = UIAlertController(title: "", message: msg, preferredStyle: .alert)
        self.present(alertController, animated: true, completion: nil)
        
        //1.5秒后自动消失
        DispatchQueue.main.asyncAfter(deadline: DispatchTime.now() + 1.5) {
            alertController.dismiss(animated: false, completion: nil)
        }
    }
}
