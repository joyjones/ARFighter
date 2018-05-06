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

class NetworkViewController: UIViewController, MessageViewDelegate {
    
    @IBOutlet weak var txbOutput: UITextView!
    @IBOutlet weak var btnConnectServer: UIButton!
    @IBOutlet weak var btnChangeMode: UIButton!
    @IBOutlet weak var lblPlayerState: UILabel!
    
    var parentView: ViewController {
        return parent as! ViewController
    }
    
    override func viewDidLoad() {
        SocketClient.instance.delegateMsg = self as MessageViewDelegate
    }
    
    @IBAction func onChangeMode(_ sender: UIButton) {
        var mode = OperationMode(rawValue: parentView.gameScene.optMode.rawValue + 1)
        if mode == nil {
            mode = OperationMode.None
        }
        let textMap: [OperationMode: String] = [
            OperationMode.None: "无",
            OperationMode.Create: "创建",
            OperationMode.Delete: "删除",
            OperationMode.Move: "移动",
            OperationMode.Rotate: "旋转",
            OperationMode.Scale: "缩放",
        ]
        btnChangeMode.setTitle("模式: " + textMap[mode!]!, for: [])
        parentView.gameScene.optMode = mode!
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
    
    func updatePlayerState(name: String?, state: PlayerState) {
        let textMap: [PlayerState: String] = [
            PlayerState.Initial: "未登录",
            PlayerState.SceneLoading: "正在请求场景..",
            PlayerState.SearchOrigin: "查找启动标识物..",
            PlayerState.ScenePlaying: "游戏中"
        ]
        var text = "";
        if state == .Initial || name == nil || name?.count == 0 {
            text = textMap[state]!
        }else{
            text = "\(name!): \(textMap[state]!)"
        }
        DispatchQueue.main.async {
            if state == PlayerState.Initial {
                self.lblPlayerState.textColor = UIColor.gray
            }else{
                self.lblPlayerState.textColor = UIColor.red
            }
            self.lblPlayerState.text = text
        }
    }
}
