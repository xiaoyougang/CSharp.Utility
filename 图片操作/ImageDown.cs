using System;
using System.IO;
using System.Net;
using System.Text.RegularExpressions;

namespace DotNet.Utilities
{
    /// <summary>
    /// ͼƬ����
    /// </summary>
    public class ImageDown
    {
        public ImageDown()
        { }

        #region ˽�з���
        /// <summary>
        /// ��ȡͼƬ��־
        /// </summary>
        private string[] GetImgTag(string htmlStr)
        {
            Regex regObj = new Regex("<img.+?>", RegexOptions.Compiled | RegexOptions.IgnoreCase);
            string[] strAry = new string[regObj.Matches(htmlStr).Count];
            int i = 0;
            foreach (Match matchItem in regObj.Matches(htmlStr))
            {
                strAry[i] = GetImgUrl(matchItem.Value);
                i++;
            }
            return strAry;
        }

        /// <summary>
        /// ��ȡͼƬURL��ַ
        /// </summary>
        private string GetImgUrl(string imgTagStr)
        {
            string str = "";
            Regex regObj = new Regex("http://.+.(?:jpg|gif|bmp|png)", RegexOptions.Compiled | RegexOptions.IgnoreCase);
            foreach (Match matchItem in regObj.Matches(imgTagStr))
            {
                str = matchItem.Value;
            }
            return str;
        }
        #endregion

        /// <summary>
        /// ����ͼƬ������
        /// </summary>
        /// <param name="strHTML">HTML</param>
        /// <param name="path">·��</param>
        /// <param name="nowyymm">����</param>
        /// <param name="nowdd">��</param>
        public string SaveUrlPics(string strHTML, string path)
        {
            string nowym = DateTime.Now.ToString("yyyy-MM");  //��ǰ����
            string nowdd = DateTime.Now.ToString("dd");       //�������
            path = path + nowym + "/" + nowdd;
            if (!Directory.Exists(path)) Directory.CreateDirectory(path);

            string[] imgurlAry = GetImgTag(strHTML);
            try
            {
                for (int i = 0; i < imgurlAry.Length; i++)
                {
                    string preStr = System.DateTime.Now.ToString() + "_";
                    preStr = preStr.Replace("-", "");
                    preStr = preStr.Replace(":", "");
                    preStr = preStr.Replace(" ", "");
                    WebClient wc = new WebClient();
                    wc.DownloadFile(imgurlAry[i], path + "/" + preStr + imgurlAry[i].Substring(imgurlAry[i].LastIndexOf("/") + 1));
                }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
            return strHTML;
        }
    }
}
