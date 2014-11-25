using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Windows.Forms;

namespace DingyuehaoZiyuan.Win
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
            CreateHtml();
        }

        private void CreateHtml()
        {
            var url = "http://localhost:7390/EmersonUserInfo/Index";
            var html = GetPage(url, Encoding.UTF8);
            using (var worWriter = new StreamWriter("c://a.html", false))
            {
                worWriter.Write((html));
            }
        }
        public string GetPage(string url, Encoding encoding)
        {
            //初始化新的request对象
            var webRequest = (HttpWebRequest)WebRequest.Create(url);
            webRequest.UserAgent = "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.2; .NET CLR 1.0.3705;)";

            webRequest.CookieContainer = new CookieContainer();
            //返回Internet资源的相应
            var webResponse = (HttpWebResponse)webRequest.GetResponse();
            //获取流
            var stream = webResponse.GetResponseStream();
            //读取编码
            var reader = new StreamReader(stream, encoding);
            //整个页面内容
            var content = reader.ReadToEnd();
            reader.Close();
            webResponse.Close();

            return content;
        }
    }
}
