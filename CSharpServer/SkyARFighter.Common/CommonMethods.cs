using ICSharpCode.SharpZipLib.Zip;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SkyARFighter.Common
{
    public static class CommonMethods
    {
        #region 文件操作
        public static void InheritCreateFolder(string path, bool isFile)
        {
            char ch = '\\';
            int n = 0;
            if (!path.StartsWith("http:"))
            {
                path = path.Replace('/', '\\');
                if (path.StartsWith("\\\\"))
                {
                    n = path.IndexOf("\\", 2);
                    if (n < 0)// 共享根目录无需检测
                        return;
                    ++n;
                }
            }
            else
            {
                ch = '/';
                n = 7;
            }
            if (isFile)
                path.TrimEnd(ch);
            else if (!path.EndsWith(ch.ToString()))
                path += ch.ToString();
            while (n < path.Length)
            {
                int np = path.IndexOf(ch, n);
                if (np < 0)
                    break;
                if (np > 0)
                {
                    string subdir = path.Substring(0, np + 1);
                    if (!Directory.Exists(subdir))
                        Directory.CreateDirectory(subdir);
                }
                n = np + 1;
            }
        }
        public static void InheritDeleteFolder(string destpath, bool deleteme)
        {
            if (!Directory.Exists(destpath))
                return;
            var fs = Directory.GetFiles(destpath, "*", SearchOption.TopDirectoryOnly);
            var ds = Directory.GetDirectories(destpath, "*", SearchOption.TopDirectoryOnly);
            foreach (var f in fs)
            {
                if (!File.Exists(f))
                    continue;
                File.SetAttributes(f, FileAttributes.Normal);
                File.Delete(f);
            }
            foreach (var d in ds)
                InheritDeleteFolder(d, true);
            if (deleteme)
            {
                do
                {
                    try
                    {
                        if (Directory.Exists(destpath))
                            Directory.Delete(destpath, true);
                        break;
                    }
                    catch (System.Exception) { }
                } while (true);
            }
        }
        public static readonly string[] ImageFileExts = new string[] { ".bmp", ".jpg", ".jpeg", ".png", ".tga", ".gif" };
        public static readonly string[] CompressFileExts = new string[] { ".rar", ".zip", ".7z" };
        public static string ComputeMD5(string file)
        {
            if (!File.Exists(file))
                return "";
            MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
            byte[] bytes = File.ReadAllBytes(file);
            byte[] r = md5.ComputeHash(bytes);
            return BitConverter.ToString(r).Replace("-", "");
        }
        public static string ComputeSHA1(string file)
        {
            string hashSHA1 = string.Empty;
            //检查文件是否存在，如果文件存在则进行计算，否则返回空值
            if (File.Exists(file))
            {
                using (FileStream fs = new FileStream(file, FileMode.Open, FileAccess.Read))
                {
                    //计算文件的SHA1值
                    SHA1 calculator = SHA1.Create();
                    byte[] buffer = calculator.ComputeHash(fs);
                    calculator.Clear();
                    //将字节数组转换成十六进制的字符串形式
                    StringBuilder stringBuilder = new StringBuilder();
                    for (int i = 0; i < buffer.Length; i++)
                        stringBuilder.Append(buffer[i].ToString("x2"));
                    hashSHA1 = stringBuilder.ToString();
                }
            }
            return hashSHA1;
        }
        public static bool CompareFiles(string srcFile, string targetFile)
        {
            if (!File.Exists(targetFile))
                return false;
            FileInfo srcInfo = new FileInfo(srcFile);
            FileInfo targetInfo = new FileInfo(targetFile);
            // 长度检验
            if (srcInfo.Length != targetInfo.Length)
                return false;
            // 时间戳检验
            if (srcInfo.LastWriteTime != targetInfo.LastWriteTime)
            {
                // MD5码校验
                string mdOld = ComputeMD5(targetFile);
                string mdNew = ComputeMD5(srcFile);
                return mdOld == mdNew;
            }
            return true;
        }
        public static byte[] ImageToByte(Image img)
        {
            ImageConverter converter = new ImageConverter();
            return (byte[])converter.ConvertTo(img, typeof(byte[]));
        }
        public static bool CheckValidImageUrl(string url)
        {
            var p = @"^https?://.*\.((jpg)|(png)|(jpeg))$";
            var r = new Regex(p);
            if (!r.IsMatch(url.ToLower()))
                return false;
            return true;
        }

        public static bool UnZipFile(string zip_file, string dst_dir = null)
        {
            if (!File.Exists(zip_file))
                return false;
            ZipInputStream newinStream = new ZipInputStream(File.OpenRead(zip_file));
            if (null == newinStream)
                return false;
            string zipName = Path.GetFileNameWithoutExtension(zip_file);
            if (string.IsNullOrEmpty(dst_dir))
                dst_dir = Path.GetDirectoryName(zip_file);
            dst_dir = dst_dir.TrimEnd('\\') + "\\" + zipName;
            // 创建待解压的文件夹
            InheritCreateFolder(dst_dir, false);
            // 解压文件，不能删除原来的文件，切记
            try
            {
                ZipEntry theEntry = null;
                while ((theEntry = newinStream.GetNextEntry()) != null)
                {
                    if (string.IsNullOrEmpty(theEntry.Name))
                        continue;
                    string extName = Path.GetExtension(theEntry.Name);
                    if (string.IsNullOrEmpty(extName))
                        continue;
                    string filePath = dst_dir + "/" + theEntry.Name;
                    // 如果已经存在该文件， 删除
                    if (File.Exists(filePath))
                        File.Delete(filePath);
                    else
                        InheritCreateFolder(filePath, true);

                    FileStream newstream = new FileStream(filePath, FileMode.Create);
                    int size = 2048;
                    byte[] newbyte = new byte[size];
                    while (true)
                    {
                        size = newinStream.Read(newbyte, 0, newbyte.Length);
                        if (size > 0)
                            newstream.Write(newbyte, 0, size);
                        else
                        {
                            newstream.Close();
                            break;
                        }
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                //Debug.Log ("[Error] zip file---->>>" + zip_file);
                return false;
            }
            finally
            {
                newinStream.Close();
            }
        }
        public static string MakeRandomString(int length)
        {
            const string chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            string result = "";
            for (int i = 0; i < length; ++i)
                result += chars[Rander.Next(chars.Length)];
            return result;
        }
        #endregion
        #region 时间操作
        public static DateTime TimeStampToDateTime(int timeStamp)
        {
            DateTime dtStart = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
            long lTime = long.Parse(timeStamp.ToString() + "0000000");
            TimeSpan toNow = new TimeSpan(lTime);
            return dtStart.Add(toNow);
        }

        public static int DateTimeToTimeStamp(System.DateTime time)
        {
            System.DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1));
            return (int)(time - startTime).TotalSeconds;
        }
        #endregion
        public static Random Rander { get; } = new Random(DateTime.Now.Millisecond);
    }
}
