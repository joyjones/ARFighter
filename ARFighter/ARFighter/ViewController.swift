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
    
    override func viewDidLoad() {
        super.viewDidLoad()
        
        // Set the view's delegate
        sceneView.delegate = self
        // Show statistics such as fps and timing information
        sceneView.showsStatistics = true
        sceneView.autoenablesDefaultLighting = true
        sceneView.debugOptions = [ARSCNDebugOptions.showFeaturePoints, ARSCNDebugOptions.showWorldOrigin]
        
        // Create scene
        let scene = GameScene(session: sceneView.session)
        scene.delegateMsg = networkCtrl
        sceneView.scene = scene
        
        let gesture = UITapGestureRecognizer(target: self, action: #selector(ViewController.handleTapFrom(recognizer:)))
        gesture.numberOfTapsRequired = 1
        sceneView.addGestureRecognizer(gesture)
    }
    
    /// Creates a new AR configuration to run on the `session`.
    func resetTracking() {
        guard let referenceImages = ARReferenceImage.referenceImages(inGroupNamed: "AR Resources", bundle: nil) else {
            fatalError("Missing expected asset catalog resources.")
        }
        
        let configuration = ARWorldTrackingConfiguration()
        configuration.detectionImages = referenceImages
        configuration.planeDetection = [.horizontal, .vertical]
        sceneView.session.run(configuration, options: [.resetTracking, .removeExistingAnchors])
    }
    
    @objc func handleTapFrom(recognizer: UITapGestureRecognizer) {
        // 获取屏幕空间坐标并传递给 ARSCNView 实例的 hitTest 方法
        let tapPoint = recognizer.location(in: self.view)
        let resultFpt = sceneView.hitTest(tapPoint, types: .featurePoint)
        if gameScene.optMode == .Create {
            if resultFpt.first != nil {
                let pos = resultFpt.first!.worldTransform.columns.3
                gameScene.mainPlayer!.createModel(templateId: 1, pos: simd_float3(pos.x, pos.y, pos.z))
            }
        }
        else if gameScene.optMode == .Move {
            let resultObjs = sceneView.hitTest(tapPoint, options: [
                SCNHitTestOption.firstFoundOnly: false,
                SCNHitTestOption.ignoreHiddenNodes: false,
                SCNHitTestOption.searchMode: 1,//SCNHitTestSearchMode.closest,
            ])
            var foundModel: SceneModel? = nil
            for r in resultObjs {
                if r.node.geometry == nil {
                    continue
                }
                foundModel = gameScene.findModel(node: r.node)
                if foundModel != nil {
                    break
                }
            }
            
            if foundModel == nil {
                let selModel = gameScene.selectedModel
                if selModel != nil && resultFpt.first != nil {
                    let pos = resultFpt.first!.worldTransform.columns.3
                    gameScene.mainPlayer!.moveModel(model: selModel!, pos: simd_float3(pos.x, pos.y, pos.z))
                }
            }else{
                if gameScene.selectedModel != nil && foundModel!.id == gameScene.selectedModel!.id {
                    gameScene.selectedModel = nil
                }else{
                    gameScene.selectedModel = foundModel
                }
            }
        }
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
