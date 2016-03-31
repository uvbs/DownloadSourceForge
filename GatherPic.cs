using System;
using System.Net;
using System.IO;
using System.Windows.Forms;

namespace SourceForgeHTTPSvnDownload
{
    public class GatherPic
    {

        private string savePath;
        private string getUrl;
        private WebBrowser wb;
        private int iImgCount;

        //初始化参数
        public GatherPic(WebBrowser wb, string sWebUrl, string sSavePath)
        {
            this.getUrl = sWebUrl;
            this.savePath = sSavePath;
            this.wb = wb;
            this.wb.DocumentCompleted += new System.Windows.Forms.WebBrowserDocumentCompletedEventHandler(DocumentCompleted);
            start();

        }

        //开始采集
        public bool start()
        {

            if (getUrl.Trim().Equals(""))
            {
                //MessageBox.Show("哪来的虾米连网址都没输！");
                return false;
            }

            this.wb.Navigate(getUrl);
            //委托事件
            return true;
        }

        //WebBrowser.DocumentCompleted委托事件
        private void DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            //页面里框架iframe加载完成不掉用SearchImgList()
            if (wb.ReadyState != WebBrowserReadyState.Complete) return;
            //if (e.Url != wb.Document.Url) return;
            string strUrl = wb.Url.ToString();
            string strUrl2 = e.Url.ToString();
            if(strUrl2.IndexOf("HEAD/tree/BenderBot") != -1)
            {

            }
            else if (strUrl2.IndexOf("HEAD/tree/DataManager") != -1)
            {

            }
            else if (strUrl2.IndexOf("HEAD/tree/Dependancies") != -1)
            {

            }
            else if (strUrl2.IndexOf("HEAD/tree/battlenet") != -1)
            {
                
            }

            string strTreeKey = "HEAD/tree/";
            int nIx = strUrl2.IndexOf(strTreeKey);

            SearchImgList(strUrl2.Substring(nIx + strTreeKey.Length));
            //SearchImgList();
        }

        //检查出所有图片并采集到本地
        public void SearchImgList(string strChildFolder)
        {
            string sImgUrl = getUrl;
            //取得所有图片地址
            HtmlElementCollection elemColl = this.wb.Document.GetElementsByTagName("td");
            this.iImgCount = elemColl.Count;
            foreach (HtmlElement elem in elemColl)
            {
                string strClassName = elem.GetAttribute("classname");
                //调用保存远程图片函数
                if (strClassName == "nowrap")
                {
                    for (int i = 0; i < elem.Children.Count; i++)
                    {
                        string strTitle = elem.Children[i].GetAttribute("title");
                        //string str
                        if (strTitle.IndexOf(".") != -1)//radio
                        {
                            //elem.InvokeMember("click");

                            SaveImageFromWeb(sImgUrl + strChildFolder + "//" + strTitle + "?format=raw", savePath + "//" + strChildFolder ,   strTitle);
                            break;
                        }
                    }
                }
                //SaveImageFromWeb(sImgUrl, this.savePath);
            }
        }
        //保存远程图片函数
        public int SaveImageFromWeb(string imgUrl, string path, string strFilename)
        {
            string imgName = imgUrl.ToString().Substring(imgUrl.ToString().LastIndexOf("/") + 1);
            //path = path + "\\" + imgName;
            //string defaultType = ".jpg";
            //string[] imgTypes = new string[] { ".h", ".cs", ".cpp", ".cc", ".hh", ".dll", "csproj", "" };
            //string imgType = imgUrl.ToString().Substring(imgUrl.ToString().LastIndexOf("."));
            //foreach (string it in imgTypes)
            //{
            //    if (imgType.ToLower().Equals(it))
            //        break;
            //    if (it.Equals(".bmp"))
            //        imgType = defaultType;
            //}
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            try
            {
                FileStream fso = new FileStream(path + strFilename, FileMode.OpenOrCreate);
                if (fso.Length > 0)
                {
                    return 0;

                    fso.Close();
                }
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(imgUrl);
                request.UserAgent = "Mozilla/5.0 (Windows NT 6.1; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/46.0.2490.86 Safari/537.36";
                request.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8";
                //request.enc
                request.Timeout = 10000;
                WebResponse response = request.GetResponse();
                //Stream stream = response.GetResponseStream();
                if (response.ContentType.ToLower().Contains("octet-stream"))
                {
                    byte[] arrayByte = new byte[1024];
                    int imgLong = (int)response.ContentLength;
                    int l = 0;


                    StreamReader streamReader = new StreamReader(response.GetResponseStream());
                        string responseContent = streamReader.ReadToEnd();
                        //int i = streamReader.Read(arrayByte, 0, 1024);
                        fso.Write(System.Text.Encoding.ASCII.GetBytes(responseContent), 0, responseContent.Length);
                    //    l += i;
                    //}
                    fso.Close();
                    streamReader.Close();
                    //stream.Close();
                    response.Close();
                    return 1;
                }
                else
                {
                    return 0;
                }
            }
            catch (WebException ex)
            {
                MessageBox.Show(ex.Message);
                return 0;
            }
            catch (UriFormatException ex)
            {
                MessageBox.Show(ex.Message);

                return 0;
            }
        }

    }
}

//-----------------调用代码--------------------

//GatherPic gatherpic = new GatherPic(“http://www.baidu.com”,"C:\test");

//请确保c:\下存在test路径

//gatherpic.start()