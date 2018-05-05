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
//import ObjectMapper
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
    
    /// A serial queue for thread safety when modifying the SceneKit node graph.
    let updateQueue = DispatchQueue(label: Bundle.main.bundleIdentifier! + ".serialSceneKitQueue")
    let targetImages = [
        "test01",
    ]
    
    override func viewDidLoad() {
        super.viewDidLoad()
        
        // Set the view's delegate
        sceneView.delegate = self
        // Show statistics such as fps and timing information
        sceneView.showsStatistics = true
        sceneView.autoenablesDefaultLighting = true
        sceneView.debugOptions = [ARSCNDebugOptions.showFeaturePoints, ARSCNDebugOptions.showWorldOrigin]
        
        // Create scene
        sceneView.scene = GameScene(session: sceneView.session)
        
        let gesture = UITapGestureRecognizer(target: self, action: #selector(ViewController.handleTapFrom(recognizer:)))
        gesture.numberOfTapsRequired = 1
        sceneView.addGestureRecognizer(gesture)
        
        do {
            try testCereal()
        } catch {
            
        }
    }
    
    /// Creates a new AR configuration to run on the `session`.
    func resetTracking() {
        guard let referenceImages = ARReferenceImage.referenceImages(inGroupNamed: "AR Resources", bundle: nil) else {
            fatalError("Missing expected asset catalog resources.")
        }
        
        let configuration = ARWorldTrackingConfiguration()
        configuration.detectionImages = referenceImages
//        configuration.planeDetection = [.horizontal,.vertical]
        sceneView.session.run(configuration, options: [.resetTracking, .removeExistingAnchors])
//        for n in imagePlaneNodes {
//            n.removeFromParentNode()
//        }
//        imagePlaneNodes.removeAll()
    }
    
    @objc func handleTapFrom(recognizer: UITapGestureRecognizer) {
        // 获取屏幕空间坐标并传递给 ARSCNView 实例的 hitTest 方法
        let tapPoint = recognizer.location(in: self.view)
        let result = sceneView.hitTest(tapPoint, types: .featurePoint)
        if let hitResult = result.first {
            let pos = hitResult.worldTransform.columns.3
            gameScene.mainPlayer!.createModel(templateId: 1, pos: simd_float3(pos.x, pos.y, pos.z))
        }
    }
    
    func testCereal() throws {
//        let json = "[{\"values\":\"0,0,0,1\"},{\"pos\":\"1,2,3\",\"scale\":\"1.5\"},\"test01\",145]"
//        let json = "[{\"r1\":[1.0,0.0,0.0,0.0],\"r2\":[0.0,1.0,0.0,0.0],\"r3\":[0.0,0.0,1.0,0.0],\"r4\":[0.0,0.0,0.0,1.0]}]"
//        let jarray = try? JSONSerialization.jsonObject(with: json.data(using: .utf8)!, options: []) as! [Any]
//        print(jarray![0] as! [String: [Float]])
//        
//        var type = Int32(1)
//        var data1 = [UInt8](Data(buffer: UnsafeBufferPointer(start: &type, count: 1)))
//        data1.append(contentsOf: Array("[2050165252]".utf8))
//        var bytes = [UInt8](data1)
//        
//        
//        let data2 = Data(bytes: bytes)
//        let methodId: Int32 = data2.withUnsafeBytes { $0.pointee }
////        let methodId = Int32(bigEndian: data2.withUnsafeBytes { $0.pointee })
//        bytes.removeSubrange(0...3)
//        let json = String(bytes: bytes, encoding: .utf8)!
//        let args = try? JSONSerialization.jsonObject(with: json.data(using: .utf8)!, options: []) as! [Any]
//        print(args)

        //        let v = simd_float3(1,2,3)
//
//        let dic: [String: Any] = [
//            "type": 4,
//            "pos": v.toJson(),
//            "scale": v.toJson(),
//            "rotate": "abc"
//        ]
//        let data = try? JSONSerialization.data(withJSONObject: dic, options: [])
//        let context = String(data: data!, encoding: .utf8)!
//        print(context)
    }
    
    override func viewWillAppear(_ animated: Bool) {
        super.viewWillAppear(animated)
        
        // Prevent the screen from being dimmed to avoid interuppting the AR experience.
        UIApplication.shared.isIdleTimerDisabled = true
        
        resetTracking()
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
        
    }
    
    func renderer(_ renderer: SCNSceneRenderer, didUpdate node: SCNNode, for anchor: ARAnchor) {
    }
    
    func renderer(_ renderer: SCNSceneRenderer, didAdd node: SCNNode, for anchor: ARAnchor) {
        guard let imageAnchor = anchor as? ARImageAnchor else { return }
        let referenceImage = imageAnchor.referenceImage
        
        updateQueue.async {
            print("+++ recognizing image: " + referenceImage.name!)
            self.gameScene.mainPlayer?.updateWorldOrigin(imageName: referenceImage.name!, transform: anchor.transform)
            self.sceneView.session.remove(anchor: imageAnchor)
        }
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
