//
//  ViewController.swift
//  ARFighter
//
//  Created by Jones on 2018/4/26.
//  Copyright © 2018年 tjwd. All rights reserved.
//

import UIKit
import SceneKit
import ARKit
import SwiftSocket
import ObjectMapper
//import Cereal

class ViewController: UIViewController, ARSCNViewDelegate {

    @IBOutlet var sceneView: ARSCNView!
    
    var gameScene: GameScene {
        get { return sceneView.scene as! GameScene }
    }
    
    lazy var networkCtrl: NetworkViewController = {
        return childViewControllers.lazy.flatMap({ $0 as? NetworkViewController }).first!
    }()
    
    var timer: Timer?
    
    override func viewDidLoad() {
        super.viewDidLoad()
        
        // Set the view's delegate
        sceneView.delegate = self
        // Show statistics such as fps and timing information
        sceneView.showsStatistics = true
        sceneView.autoenablesDefaultLighting = true
        sceneView.debugOptions = [ARSCNDebugOptions.showFeaturePoints, ARSCNDebugOptions.showWorldOrigin]
        
        // Create scene
        sceneView.scene = GameScene()
        
        let gesture = UITapGestureRecognizer(target: self, action: #selector(ViewController.handleTapFrom(recognizer:)))
        gesture.numberOfTapsRequired = 1
        sceneView.addGestureRecognizer(gesture)
        
        do {
            try testCereal()
        } catch {
            
        }
    }
    
    func enableTimer(enabled: Bool) {
        if enabled {
            timer = Timer.scheduledTimer(timeInterval: 2, target: self, selector: (#selector(ViewController.updateTimer)), userInfo: nil, repeats: true)
        }
        else{
            timer?.invalidate()
            timer = nil
        }
    }
    
    @objc func handleTapFrom(recognizer: UITapGestureRecognizer) {
        // 获取屏幕空间坐标并传递给 ARSCNView 实例的 hitTest 方法
        let tapPoint = recognizer.location(in: self.view)
        let result = sceneView.hitTest(tapPoint, types: .featurePoint)
        if let hitResult = result.first {
            let pos = hitResult.worldTransform.columns.3
            gameScene.mainPlayer!.createModel(type: .标注_圆点, pos: simd_float3(pos.x, pos.y, pos.z))
        }
    }
    
    func testCereal() throws {
//        let json = "[{\"values\":\"0,0,0,1\"},{\"pos\":\"1,2,3\",\"scale\":\"1.5\"},\"test01\",145]"
//        let json = "[{\"r1\":[1.0,0.0,0.0,0.0],\"r2\":[0.0,1.0,0.0,0.0],\"r3\":[0.0,0.0,1.0,0.0],\"r4\":[0.0,0.0,0.0,1.0]}]"
//        let jarray = try? JSONSerialization.jsonObject(with: json.data(using: .utf8)!, options: []) as! [Any]
//        print(jarray![0] as! [String: [Float]])
    }
    
    override func viewWillAppear(_ animated: Bool) {
        super.viewWillAppear(animated)
        
        // Create a session configuration
        let configuration = ARWorldTrackingConfiguration()

        // Run the view's session
        sceneView.session.run(configuration)
    }
    
    override func viewWillDisappear(_ animated: Bool) {
        super.viewWillDisappear(animated)
        
        // Pause the view's session
        sceneView.session.pause()
    }
    
    override func didReceiveMemoryWarning() {
        super.didReceiveMemoryWarning()
        // Release any cached data, images, etc that aren't in use.
    }
    
    @objc func updateTimer() {
        if !networkCtrl.isConnected { return }
        
        var mat: matrix_float4x4
        if let frame = sceneView.session.currentFrame{
            mat = frame.camera.transform
        } else {
            mat = matrix_float4x4()
            mat.columns.0.x = 1; mat.columns.1.y = 1; mat.columns.2.z = 1; mat.columns.3.w = 1
        }
        let data = NMMatrix(mat: mat).toJSONString()!
//        networkCtrl.sendMessage(cmd: ProtoType.SyncCamera, data: data)
    }
    
    func getCameraTransform() -> matrix_float4x4 {
        var mat = matrix_float4x4()
        mat.columns.0.x = 1; mat.columns.1.y = 1; mat.columns.2.z = 1
        return mat
    }
    
    func renderer(_ renderer: SCNSceneRenderer, didUpdate node: SCNNode, for anchor: ARAnchor) {
    }

    // MARK: - ARSCNViewDelegate
    
/*
    // Override to create and configure nodes for anchors added to the view's session.
    func renderer(_ renderer: SCNSceneRenderer, nodeFor anchor: ARAnchor) -> SCNNode? {
        let node = SCNNode()
     
        return node
    }
*/
    
    func session(_ session: ARSession, didFailWithError error: Error) {
        // Present an error message to the user
        
    }
    
    
    
    func sessionWasInterrupted(_ session: ARSession) {
        // Inform the user that the session has been interrupted, for example, by presenting an overlay
        
    }
    
    func sessionInterruptionEnded(_ session: ARSession) {
        // Reset tracking and/or remove existing anchors if consistent tracking is required
        
    }
}
