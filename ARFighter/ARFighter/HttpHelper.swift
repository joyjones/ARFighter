//
//  HttpHelper.swift
//  NavigateDemo
//
//  Created by yixin ran on 07/08/2017.
//  Copyright © 2017 yixin ran. All rights reserved.
//
import UIKit

public class HttpHelper{
    public static var Shared = HttpHelper();
    // MARK:- get请求
    func Get(path: String,success: @escaping ((_ result: String) -> ()),failure: @escaping ((_ error: Error) -> ())) {
        let url = URL(string: path.addingPercentEncoding(withAllowedCharacters: CharacterSet.urlQueryAllowed)!)
        let session = URLSession.shared
        let dataTask = session.dataTask(with: url!) { (data, respond, error) in
            if let data = data {
                if let result = String(data:data,encoding:.utf8){
                    success(result)
                }
            }else {
                failure(error!)
            }
        }
        dataTask.resume()
    }
    
    // MARK:- post请求
    func Post(path: String,paras: String,success: @escaping ((_ result: String) -> ()),failure: @escaping ((_ error: Error) -> ())) {
        let url = URL(string: path)
        var request = URLRequest.init(url: url!)
        request.httpMethod = "POST"
        request.httpBody = paras.data(using: .utf8)
        let session = URLSession.shared
        let dataTask = session.dataTask(with: request) { (data, respond, error) in
            if let data = data {
                if let result = String(data:data,encoding:.utf8){
                    success(result)
                }
            }else {
                failure(error!)
            }
        }
        dataTask.resume()
    }
}

class JsonHelper {
    static let Shared=JsonHelper();
    private let decoder=JSONDecoder();
    private let encoder=JSONEncoder();
    func ToJson<T:Codable>(_ obj:T) -> String {
        let data=try! self.encoder.encode(obj)
        let str=String(data:data,encoding:.utf8)!
        return str
    }
    func ToObject<T:Codable>(_ data:String) -> T{
        let obj=try! self.decoder.decode(T.self, from: data.data(using: .utf8)!)
        return obj;
    }
    func GetData(_ str:String) -> Data {
        return str.data(using: .utf8)!
    }
    func GetJson(_ data:Data) -> String {
        return String(data:data,encoding:.utf8)!
    }
}
