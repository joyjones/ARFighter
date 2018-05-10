using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Reflection;
using System.Text;
using System.Threading;

namespace SkyARFighter.Common.Network
{
    public class Communication
    {
        public Communication(Dictionary<int, MethodInfo> methods)
        {
            msgHandlers = methods;
        }
        public int MessageCount => messages.Count;
        public (RemotingMethodId code, string args)[] Messages
        {
            get
            {
                lock (messages)
                {
                    var lst = messages.ToArray();
                    messages.Clear();
                    return lst;
                }
            }
        }
        public void StartupThread(string threadName, Socket socket, Action<string> LogAppended, Action Disconnected)
        {
            ThreadPool.QueueUserWorkItem(new WaitCallback((obj) =>
            {
                Thread.CurrentThread.Name = threadName;

                var buffer = new byte[1024];
                while (socket.Connected)
                {
                    try
                    {
                        while (!receivedNewMessage.WaitOne(100))
                        {
                            if (!socket.Connected)
                                return;
                            if (socket.Available >= 8)
                                receivedNewMessage.Set();
                        }
                        socket.Receive(buffer, 0, 8, SocketFlags.None);
                    }
                    catch (Exception ex)
                    {
                        if (ex is SocketException exs && exs.SocketErrorCode == SocketError.TimedOut)
                            continue;
                        LogAppended?.Invoke("断开连接");
                        Disconnected?.Invoke();
                        return;
                    }

                    var msgType = BitConverter.ToInt32(buffer, 0);
                    int contentLen = BitConverter.ToInt32(buffer, 4);
                    if (contentLen > 0)
                    {
                        if (!msgHandlers.TryGetValue(msgType, out MethodInfo mi))
                        {
                            LogAppended?.Invoke("收到未注册的的远程方法调用请求：" + (RemotingMethodId)msgType);
                            ReadBytes(socket, buffer, contentLen);
                        }
                        else
                        {
                            try
                            {
                                byte[] bytes = ReadBytes(socket, buffer, contentLen);
                                var args = Encoding.UTF8.GetString(bytes, 0, bytes.Length);
                                //LogAppended?.Invoke($"调用本地方法：{mi.Name}, 参数：{args}");
                                lock (messages)
                                {
                                    messages.Enqueue(((RemotingMethodId)msgType, args));
                                }
                            }
                            catch (Exception ex)
                            {
                                LogAppended?.Invoke("远程方法调用异常：" + ex.Message);
                            }
                        }
                    }
                }
            }));
        }

        private byte[] ReadBytes(Socket socket, byte[] buffer, int contentLen)
        {
            List<byte> bytes = new List<byte>();
            int restLen = contentLen;
            while (restLen > 0)
            {
                int len = socket.Receive(buffer, 0, restLen > buffer.Length ? buffer.Length : restLen, SocketFlags.None);
                bytes.AddRange(buffer.Take(len));
                restLen -= len;
            }
            return bytes.ToArray();
        }

        public bool SendMessage(Socket socket, RemotingMethodId methodId, object args, Action<string> LogAppended)
        {
            if (!socket.Connected)
                return false;

            using (var sw = new System.IO.MemoryStream())
            {
                var bsType = BitConverter.GetBytes((int)methodId);
                var json = JsonConvert.SerializeObject(args);
                var bsCtx = Encoding.UTF8.GetBytes(json);

                sw.Write(bsType, 0, bsType.Length);
                var bsLen = BitConverter.GetBytes(bsCtx.Length);
                sw.Write(bsLen, 0, bsLen.Length);
                sw.Write(bsCtx, 0, bsCtx.Length);
                var bsSend = sw.GetBuffer();
                int lenReal = bsType.Length + bsLen.Length + bsCtx.Length;
                if (lenReal < bsSend.Length)
                    bsSend = bsSend.Take(lenReal).ToArray();
                socket.Send(bsSend);

                LogAppended?.Invoke($"调用远端方法：{methodId}, 字节数: {bsSend.Length}");//, 参数：{json}");
            }
            return true;
        }

        private AutoResetEvent receivedNewMessage = new AutoResetEvent(false);
        private Dictionary<int, MethodInfo> msgHandlers;
        private Queue<(RemotingMethodId code, string args)> messages = new Queue<(RemotingMethodId code, string args)>();
    }
}
