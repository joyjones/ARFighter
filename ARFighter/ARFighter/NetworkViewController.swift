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

class NetworkViewController: UIViewController, NetworkDelegate {
    
//    private var socketServer: TcpSocketServer?
    
    private(set) var isConnected = false
    
    @IBOutlet weak var txbOutput: UITextView!
    @IBOutlet weak var btnConnectServer: UIButton!
    @IBOutlet weak var btnCreateServer: UIButton!
    @IBOutlet weak var btnStopServer: UIButton!
    
    override func viewDidLoad() {
        SocketClient.instance.delegate = self as NetworkDelegate
        SocketClient.instance.connectServer(address: "10.211.55.5", port: 8333)
    }
    
    @IBAction func connectServer(_ sender: UIButton) {
        
    }
    @IBAction func startServer(_ sender: UIButton) {
        let url = "http://apis.juhe.cn/mobile/get?phone=13301388888&key=4e602dad4a05b4d491ffb82511613158"
        HttpHelper.Shared.Get(path: url, success: onSuccess, failure: onFail)
        
//        return
//        socketServer = TcpSocketServer()
//        socketServer?.start()
    }
    @IBAction func stopServer(_ sender: UIButton) {
//        disconnectServer()
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
    
    
    func appendLog(msg: String) {
        DispatchQueue.main.async {
            self.txbOutput.text = self.txbOutput.text + msg + "\n"
        }
    }
    
    func alert(msg: String) {
        let alertController = UIAlertController(title: "", message: msg, preferredStyle: .alert)
        self.present(alertController, animated: true, completion: nil)
        
        //1.5秒后自动消失
        DispatchQueue.main.asyncAfter(deadline: DispatchTime.now() + 1.5) {
            alertController.dismiss(animated: false, completion: nil)
        }
    }
}
