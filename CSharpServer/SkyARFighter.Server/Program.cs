﻿using SkyARFighter.Server.Network;
using SkyARFighter.Server.Structures;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SkyARFighter.Server
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            Game = new Game();
            Application.Run(new ServerForm());
        }

        public static TcpServer Server { get; } = new TcpServer();
        public static Game Game
        {
            get; private set;
        }
        public static string ConfigsFile
        {
            get { return ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None).FilePath; }
        }
    }
}
