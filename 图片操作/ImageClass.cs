using System;
using System.Collections;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;

namespace DotNet.Utilities
{
    public class ImageClass
    {
        public ImageClass()
        { }

        #region ����ͼ
        /// <summary>
        /// ��������ͼ
        /// </summary>
        /// <param name="originalImagePath">Դͼ·��������·����</param>
        /// <param name="thumbnailPath">����ͼ·��������·����</param>
        /// <param name="width">����ͼ���</param>
        /// <param name="height">����ͼ�߶�</param>
        /// <param name="mode">��������ͼ�ķ�ʽ</param>    
        public static void MakeThumbnail(string originalImagePath, string thumbnailPath, int width, int height, string mode)
        {
            System.Drawing.Image originalImage = System.Drawing.Image.FromFile(originalImagePath);

            int towidth = width;
            int toheight = height;

            int x = 0;
            int y = 0;
            int ow = originalImage.Width;
            int oh = originalImage.Height;

            switch (mode)
            {
                case "HW":  //ָ���߿����ţ����ܱ��Σ�                
                    break;
                case "W":   //ָ�����߰�����                    
                    toheight = originalImage.Height * width / originalImage.Width;
                    break;
                case "H":   //ָ���ߣ�������
                    towidth = originalImage.Width * height / originalImage.Height;
                    break;
                case "Cut": //ָ���߿�ü��������Σ�                
                    if ((double)originalImage.Width / (double)originalImage.Height > (double)towidth / (double)toheight)
                    {
                        oh = originalImage.Height;
                        ow = originalImage.Height * towidth / toheight;
                        y = 0;
                        x = (originalImage.Width - ow) / 2;
                    }
                    else
                    {
                        ow = originalImage.Width;
                        oh = originalImage.Width * height / towidth;
                        x = 0;
                        y = (originalImage.Height - oh) / 2;
                    }
                    break;
                default:
                    break;
            }

            //�½�һ��bmpͼƬ
            System.Drawing.Image bitmap = new System.Drawing.Bitmap(towidth, toheight);

            //�½�һ������
            System.Drawing.Graphics g = System.Drawing.Graphics.FromImage(bitmap);

            //���ø�������ֵ��
            g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.High;

            //���ø�����,���ٶȳ���ƽ���̶�
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;

            //��ջ�������͸������ɫ���
            g.Clear(System.Drawing.Color.Transparent);

            //��ָ��λ�ò��Ұ�ָ����С����ԭͼƬ��ָ������
            g.DrawImage(originalImage, new System.Drawing.Rectangle(0, 0, towidth, toheight), new System.Drawing.Rectangle(x, y, ow, oh), System.Drawing.GraphicsUnit.Pixel);

            try
            {
                //��jpg��ʽ��������ͼ
                bitmap.Save(thumbnailPath, System.Drawing.Imaging.ImageFormat.Jpeg);
            }
            catch (System.Exception e)
            {
                throw e;
            }
            finally
            {
                originalImage.Dispose();
                bitmap.Dispose();
                g.Dispose();
            }
        }
        #endregion

        #region ͼƬˮӡ
        /// <summary>
        /// ͼƬˮӡ������
        /// </summary>
        /// <param name="path">��Ҫ����ˮӡ��ͼƬ·��������·����</param>
        /// <param name="waterpath">ˮӡͼƬ������·����</param>
        /// <param name="location">ˮӡλ�ã�������ȷ�Ĵ��룩</param>
        public static string ImageWatermark(string path, string waterpath, string location)
        {
            string kz_name = Path.GetExtension(path);
            if (kz_name == ".jpg" || kz_name == ".bmp" || kz_name == ".jpeg")
            {
                DateTime time = DateTime.Now;
                string filename = "" + time.Year.ToString() + time.Month.ToString() + time.Day.ToString() + time.Hour.ToString() + time.Minute.ToString() + time.Second.ToString() + time.Millisecond.ToString();
                Image img = Bitmap.FromFile(path);
                Image waterimg = Image.FromFile(waterpath);
                Graphics g = Graphics.FromImage(img);
                ArrayList loca = GetLocation(location, img, waterimg);
                g.DrawImage(waterimg, new Rectangle(int.Parse(loca[0].ToString()), int.Parse(loca[1].ToString()), waterimg.Width, waterimg.Height));
                waterimg.Dispose();
                g.Dispose();
                string newpath = Path.GetDirectoryName(path) + filename + kz_name;
                img.Save(newpath);
                img.Dispose();
                File.Copy(newpath, path, true);
                if (File.Exists(newpath))
                {
                    File.Delete(newpath);
                }
            }
            return path;
        }

        /// <summary>
        /// ͼƬˮӡλ�ô�����
        /// </summary>
        /// <param name="location">ˮӡλ��</param>
        /// <param name="img">��Ҫ���ˮӡ��ͼƬ</param>
        /// <param name="waterimg">ˮӡͼƬ</param>
        private static ArrayList GetLocation(string location, Image img, Image waterimg)
        {
            ArrayList loca = new ArrayList();
            int x = 0;
            int y = 0;

            if (location == "LT")
            {
                x = 10;
                y = 10;
            }
            else if (location == "T")
            {
                x = img.Width / 2 - waterimg.Width / 2;
                y = img.Height - waterimg.Height;
            }
            else if (location == "RT")
            {
                x = img.Width - waterimg.Width;
                y = 10;
            }
            else if (location == "LC")
            {
                x = 10;
                y = img.Height / 2 - waterimg.Height / 2;
            }
            else if (location == "C")
            {
                x = img.Width / 2 - waterimg.Width / 2;
                y = img.Height / 2 - waterimg.Height / 2;
            }
            else if (location == "RC")
            {
                x = img.Width - waterimg.Width;
                y = img.Height / 2 - waterimg.Height / 2;
            }
            else if (location == "LB")
            {
                x = 10;
                y = img.Height - waterimg.Height;
            }
            else if (location == "B")
            {
                x = img.Width / 2 - waterimg.Width / 2;
                y = img.Height - waterimg.Height;
            }
            else
            {
                x = img.Width - waterimg.Width;
                y = img.Height - waterimg.Height;
            }
            loca.Add(x);
            loca.Add(y);
            return loca;
        }
        #endregion

        #region ����ˮӡ
        /// <summary>
        /// ����ˮӡ������
        /// </summary>
        /// <param name="path">ͼƬ·��������·����</param>
        /// <param name="size">�����С</param>
        /// <param name="letter">ˮӡ����</param>
        /// <param name="color">��ɫ</param>
        /// <param name="location">ˮӡλ��</param>
        public static string LetterWatermark(string path, int size, string letter, Color color, string location)
        {
            #region

            string kz_name = Path.GetExtension(path);
            if (kz_name == ".jpg" || kz_name == ".bmp" || kz_name == ".jpeg")
            {
                DateTime time = DateTime.Now;
                string filename = "" + time.Year.ToString() + time.Month.ToString() + time.Day.ToString() + time.Hour.ToString() + time.Minute.ToString() + time.Second.ToString() + time.Millisecond.ToString();
                Image img = Bitmap.FromFile(path);
                Graphics gs = Graphics.FromImage(img);
                ArrayList loca = GetLocation(location, img, size, letter.Length);
                Font font = new Font("����", size);
                Brush br = new SolidBrush(color);
                gs.DrawString(letter, font, br, float.Parse(loca[0].ToString()), float.Parse(loca[1].ToString()));
                gs.Dispose();
                string newpath = Path.GetDirectoryName(path) + filename + kz_name;
                img.Save(newpath);
                img.Dispose();
                File.Copy(newpath, path, true);
                if (File.Exists(newpath))
                {
                    File.Delete(newpath);
                }
            }
            return path;

            #endregion
        }

        /// <summary>
        /// ����ˮӡλ�õķ���
        /// </summary>
        /// <param name="location">λ�ô���</param>
        /// <param name="img">ͼƬ����</param>
        /// <param name="width">��(��ˮӡ����Ϊ����ʱ,�������ľ�������Ĵ�С)</param>
        /// <param name="height">��(��ˮӡ����Ϊ����ʱ,�������ľ����ַ��ĳ���)</param>
        private static ArrayList GetLocation(string location, Image img, int width, int height)
        {
            #region

            ArrayList loca = new ArrayList();  //��������洢λ��
            float x = 10;
            float y = 10;

            if (location == "LT")
            {
                loca.Add(x);
                loca.Add(y);
            }
            else if (location == "T")
            {
                x = img.Width / 2 - (width * height) / 2;
                loca.Add(x);
                loca.Add(y);
            }
            else if (location == "RT")
            {
                x = img.Width - width * height;
            }
            else if (location == "LC")
            {
                y = img.Height / 2;
            }
            else if (location == "C")
            {
                x = img.Width / 2 - (width * height) / 2;
                y = img.Height / 2;
            }
            else if (location == "RC")
            {
                x = img.Width - height;
                y = img.Height / 2;
            }
            else if (location == "LB")
            {
                y = img.Height - width - 5;
            }
            else if (location == "B")
            {
                x = img.Width / 2 - (width * height) / 2;
                y = img.Height - width - 5;
            }
            else
            {
                x = img.Width - width * height;
                y = img.Height - width - 5;
            }
            loca.Add(x);
            loca.Add(y);
            return loca;

            #endregion
        }
        #endregion

        #region �����ⰵ
        /// <summary>
        /// �����ⰵ
        /// </summary>
        /// <param name="mybm">ԭʼͼƬ</param>
        /// <param name="width">ԭʼͼƬ�ĳ���</param>
        /// <param name="height">ԭʼͼƬ�ĸ߶�</param>
        /// <param name="val">���ӻ���ٵĹⰵֵ</param>
        public Bitmap LDPic(Bitmap mybm, int width, int height, int val)
        {
            Bitmap bm = new Bitmap(width, height);//��ʼ��һ����¼����������ͼƬ����
            int x, y, resultR, resultG, resultB;//x��y��ѭ�����������������Ǽ�¼����������ֵ��
            Color pixel;
            for (x = 0; x < width; x++)
            {
                for (y = 0; y < height; y++)
                {
                    pixel = mybm.GetPixel(x, y);//��ȡ��ǰ���ص�ֵ
                    resultR = pixel.R + val;//����ɫֵ�᲻�ᳬ��[0, 255]
                    resultG = pixel.G + val;//�����ɫֵ�᲻�ᳬ��[0, 255]
                    resultB = pixel.B + val;//�����ɫֵ�᲻�ᳬ��[0, 255]
                    bm.SetPixel(x, y, Color.FromArgb(resultR, resultG, resultB));//��ͼ
                }
            }
            return bm;
        }
        #endregion

        #region ��ɫ����
        /// <summary>
        /// ��ɫ����
        /// </summary>
        /// <param name="mybm">ԭʼͼƬ</param>
        /// <param name="width">ԭʼͼƬ�ĳ���</param>
        /// <param name="height">ԭʼͼƬ�ĸ߶�</param>
        public Bitmap RePic(Bitmap mybm, int width, int height)
        {
            Bitmap bm = new Bitmap(width, height);//��ʼ��һ����¼������ͼƬ�Ķ���
            int x, y, resultR, resultG, resultB;
            Color pixel;
            for (x = 0; x < width; x++)
            {
                for (y = 0; y < height; y++)
                {
                    pixel = mybm.GetPixel(x, y);//��ȡ��ǰ���������ֵ
                    resultR = 255 - pixel.R;//����
                    resultG = 255 - pixel.G;//����
                    resultB = 255 - pixel.B;//����
                    bm.SetPixel(x, y, Color.FromArgb(resultR, resultG, resultB));//��ͼ
                }
            }
            return bm;
        }
        #endregion

        #region ������
        /// <summary>
        /// ������
        /// </summary>
        /// <param name="oldBitmap">ԭʼͼƬ</param>
        /// <param name="Width">ԭʼͼƬ�ĳ���</param>
        /// <param name="Height">ԭʼͼƬ�ĸ߶�</param>
        public Bitmap FD(Bitmap oldBitmap, int Width, int Height)
        {
            Bitmap newBitmap = new Bitmap(Width, Height);
            Color color1, color2;
            for (int x = 0; x < Width - 1; x++)
            {
                for (int y = 0; y < Height - 1; y++)
                {
                    int r = 0, g = 0, b = 0;
                    color1 = oldBitmap.GetPixel(x, y);
                    color2 = oldBitmap.GetPixel(x + 1, y + 1);
                    r = Math.Abs(color1.R - color2.R + 128);
                    g = Math.Abs(color1.G - color2.G + 128);
                    b = Math.Abs(color1.B - color2.B + 128);
                    if (r > 255) r = 255;
                    if (r < 0) r = 0;
                    if (g > 255) g = 255;
                    if (g < 0) g = 0;
                    if (b > 255) b = 255;
                    if (b < 0) b = 0;
                    newBitmap.SetPixel(x, y, Color.FromArgb(r, g, b));
                }
            }
            return newBitmap;
        }
        #endregion

        #region ����ͼƬ
        /// <summary>
        /// ����ͼƬ
        /// </summary>
        /// <param name="bmp">ԭʼͼƬ</param>
        /// <param name="newW">�µĿ��</param>
        /// <param name="newH">�µĸ߶�</param>
        public static Bitmap ResizeImage(Bitmap bmp, int newW, int newH)
        {
            try
            {
                Bitmap bap = new Bitmap(newW, newH);
                Graphics g = Graphics.FromImage(bap);
                g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                g.DrawImage(bap, new Rectangle(0, 0, newW, newH), new Rectangle(0, 0, bap.Width, bap.Height), GraphicsUnit.Pixel);
                g.Dispose();
                return bap;
            }
            catch
            {
                return null;
            }
        }
        #endregion

        #region ��ɫ����
        /// <summary>
        /// ��ɫ����
        /// </summary>
        /// <param name="mybm">ԭʼͼƬ</param>
        /// <param name="width">ԭʼͼƬ�ĳ���</param>
        /// <param name="height">ԭʼͼƬ�ĸ߶�</param>
        public Bitmap FilPic(Bitmap mybm, int width, int height)
        {
            Bitmap bm = new Bitmap(width, height);//��ʼ��һ����¼��ɫЧ����ͼƬ����
            int x, y;
            Color pixel;

            for (x = 0; x < width; x++)
            {
                for (y = 0; y < height; y++)
                {
                    pixel = mybm.GetPixel(x, y);//��ȡ��ǰ���������ֵ
                    bm.SetPixel(x, y, Color.FromArgb(0, pixel.G, pixel.B));//��ͼ
                }
            }
            return bm;
        }
        #endregion

        #region ���ҷ�ת
        /// <summary>
        /// ���ҷ�ת
        /// </summary>
        /// <param name="mybm">ԭʼͼƬ</param>
        /// <param name="width">ԭʼͼƬ�ĳ���</param>
        /// <param name="height">ԭʼͼƬ�ĸ߶�</param>
        public Bitmap RevPicLR(Bitmap mybm, int width, int height)
        {
            Bitmap bm = new Bitmap(width, height);
            int x, y, z; //x,y��ѭ������,z��������¼���ص��x����ı仯��
            Color pixel;
            for (y = height - 1; y >= 0; y--)
            {
                for (x = width - 1, z = 0; x >= 0; x--)
                {
                    pixel = mybm.GetPixel(x, y);//��ȡ��ǰ���ص�ֵ
                    bm.SetPixel(z++, y, Color.FromArgb(pixel.R, pixel.G, pixel.B));//��ͼ
                }
            }
            return bm;
        }
        #endregion

        #region ���·�ת
        /// <summary>
        /// ���·�ת
        /// </summary>
        /// <param name="mybm">ԭʼͼƬ</param>
        /// <param name="width">ԭʼͼƬ�ĳ���</param>
        /// <param name="height">ԭʼͼƬ�ĸ߶�</param>
        public Bitmap RevPicUD(Bitmap mybm, int width, int height)
        {
            Bitmap bm = new Bitmap(width, height);
            int x, y, z;
            Color pixel;
            for (x = 0; x < width; x++)
            {
                for (y = height - 1, z = 0; y >= 0; y--)
                {
                    pixel = mybm.GetPixel(x, y);//��ȡ��ǰ���ص�ֵ
                    bm.SetPixel(x, z++, Color.FromArgb(pixel.R, pixel.G, pixel.B));//��ͼ
                }
            }
            return bm;
        }
        #endregion

        #region ѹ��ͼƬ
        /// <summary>
        /// ѹ����ָ���ߴ�
        /// </summary>
        /// <param name="oldfile">ԭ�ļ�</param>
        /// <param name="newfile">���ļ�</param>
        public bool Compress(string oldfile, string newfile)
        {
            try
            {
                System.Drawing.Image img = System.Drawing.Image.FromFile(oldfile);
                System.Drawing.Imaging.ImageFormat thisFormat = img.RawFormat;
                Size newSize = new Size(100, 125);
                Bitmap outBmp = new Bitmap(newSize.Width, newSize.Height);
                Graphics g = Graphics.FromImage(outBmp);
                g.CompositingQuality = CompositingQuality.HighQuality;
                g.SmoothingMode = SmoothingMode.HighQuality;
                g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                g.DrawImage(img, new Rectangle(0, 0, newSize.Width, newSize.Height), 0, 0, img.Width, img.Height, GraphicsUnit.Pixel);
                g.Dispose();
                EncoderParameters encoderParams = new EncoderParameters();
                long[] quality = new long[1];
                quality[0] = 100;
                EncoderParameter encoderParam = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, quality);
                encoderParams.Param[0] = encoderParam;
                ImageCodecInfo[] arrayICI = ImageCodecInfo.GetImageEncoders();
                ImageCodecInfo jpegICI = null;
                for (int x = 0; x < arrayICI.Length; x++)
                    if (arrayICI[x].FormatDescription.Equals("JPEG"))
                    {
                        jpegICI = arrayICI[x]; //����JPEG����
                        break;
                    }
                img.Dispose();
                if (jpegICI != null) outBmp.Save(newfile, System.Drawing.Imaging.ImageFormat.Jpeg);
                outBmp.Dispose();
                return true;
            }
            catch
            {
                return false;
            }
        }
        #endregion

        #region ͼƬ�ҶȻ�
        public Color Gray(Color c)
        {
            int rgb = Convert.ToInt32((double)(((0.3 * c.R) + (0.59 * c.G)) + (0.11 * c.B)));
            return Color.FromArgb(rgb, rgb, rgb);
        }
        #endregion

        #region ת��Ϊ�ڰ�ͼƬ
        /// <summary>
        /// ת��Ϊ�ڰ�ͼƬ
        /// </summary>
        /// <param name="mybt">Ҫ���д����ͼƬ</param>
        /// <param name="width">ͼƬ�ĳ���</param>
        /// <param name="height">ͼƬ�ĸ߶�</param>
        public Bitmap BWPic(Bitmap mybm, int width, int height)
        {
            Bitmap bm = new Bitmap(width, height);
            int x, y, result; //x,y��ѭ��������result�Ǽ�¼����������ֵ
            Color pixel;
            for (x = 0; x < width; x++)
            {
                for (y = 0; y < height; y++)
                {
                    pixel = mybm.GetPixel(x, y);//��ȡ��ǰ���������ֵ
                    result = (pixel.R + pixel.G + pixel.B) / 3;//ȡ��������ɫ��ƽ��ֵ
                    bm.SetPixel(x, y, Color.FromArgb(result, result, result));
                }
            }
            return bm;
        }
        #endregion

        #region ��ȡͼƬ�еĸ�֡
        /// <summary>
        /// ��ȡͼƬ�еĸ�֡
        /// </summary>
        /// <param name="pPath">ͼƬ·��</param>
        /// <param name="pSavePath">����·��</param>
        public void GetFrames(string pPath, string pSavedPath)
        {
            Image gif = Image.FromFile(pPath);
            FrameDimension fd = new FrameDimension(gif.FrameDimensionsList[0]);
            int count = gif.GetFrameCount(fd); //��ȡ֡��(gifͼƬ���ܰ�����֡��������ʽͼƬһ���һ֡)
            for (int i = 0; i < count; i++)    //��Jpeg��ʽ�����֡
            {
                gif.SelectActiveFrame(fd, i);
                gif.Save(pSavedPath + "\\frame_" + i + ".jpg", ImageFormat.Jpeg);
            }
        }
        #endregion
    }
}