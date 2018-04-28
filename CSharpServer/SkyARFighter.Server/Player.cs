using SkyARFighter.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace SkyARFighter.Server
{
    public class Player
    {
        public Player(Socket socket)
        {
            UsingSocket = socket;
        }

        public Socket UsingSocket
        {
            get; private set;
        }

        #region interfaces
        [RemotingMethod(0x01)]
        public void SyncCamera(float[] values)
        {
            cameraTransform.Fill(values);
        }

        [RemotingMethod(0x02)]
        public void CreateObject()
        {
        }
        #endregion
        public bool SendMessage(string msg)
        {
            if (UsingSocket.Connected)
            {
                var bytes = Encoding.Unicode.GetBytes(msg);
                var header = BitConverter.GetBytes(bytes.Length);
                var data = new byte[header.Length + bytes.Length];
                Array.Copy(header, data, header.Length);
                Array.Copy(bytes, 0, data, 4, bytes.Length);
                UsingSocket.Send(data);
                return true;
            }
            return false;
        }

        private Matrix cameraTransform;
    }
}
