using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace DingyuehaoZiyuan.Tool
{
    public class Log
    {
        #region 保存方式 txt
        /// <summary>
        /// 日志保存到不同文件
        /// </summary>
        /// <param name="name">文件名称</param>
        /// <param name="strbody">内容</param>
        /// <returns></returns>
        public static bool SaveLog(string name, string strbody)
        {
            try
            {
                var date = DateTime.Now.ToString(@"yyyyMM");
                var fileDir = AppDomain.CurrentDomain.BaseDirectory + "SiteLog\\";
                var filepath = fileDir + @"\" + date;
                if (Directory.Exists(fileDir) == false)
                {
                    fileDir = fileDir + @"\" + date + @"\";
                    filepath = fileDir;
                }
                name = name + ".txt";

                return SaveLog(filepath + @"\", name, strbody);
            }
            catch
            {
                return false;
            }
        }
        /// <summary>
        /// 普通错误日志 按日自动创建文件夹，日志文件名为 Log.txt
        /// </summary>
        /// <param name="strbody">日志详细内容</param>
        /// <returns></returns>
        public static bool SaveLog(string strbody)
        {
            strbody = string.Format("{0} {1} \r\n", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), strbody);
            return SaveLog(DateTime.Now.ToString("yyyy-MM-dd") + "_Log", strbody);
        }
        private static readonly Object ThisLock = new Object();
        /// <summary>
        /// 创建日志文件主函数（一般不直接调用）
        /// </summary>
        /// <param name="filepath">文件路径＋文件名</param>
        /// <param name="name"></param>
        /// <param name="strbody">文件内容</param>
        /// <returns></returns>
        public static bool SaveLog(string filepath, string name, string strbody)
        {

            StreamWriter myWriter = null;
            lock (ThisLock)
            {
                try
                {
                    if (Directory.Exists(filepath) == false)
                    {
                        Directory.CreateDirectory(filepath);
                    }
                    //如果文件存在，则自动追加方式写
                    myWriter = new StreamWriter(filepath + name, true, Encoding.UTF8);
                    myWriter.Write(strbody);
                    myWriter.Flush();
                    return true;

                }
                catch
                {
                    return false;
                }
                finally
                {
                    if (myWriter != null)
                        myWriter.Close();
                }
            }
        }
        #endregion
    }
}
