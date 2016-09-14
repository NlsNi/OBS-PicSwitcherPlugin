using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using CLROBS;
using System.Windows.Media.Imaging;
using System.Windows.Media;
namespace PicSwitcher
{
    class PicSwitcherSource:AbstractImageSource
    {
        private Object textureLock = new Object();
        private Texture texture = null;
        private XElement config;

        public PicSwitcherSource(XElement configData)//构造函数
        {
            this.config = configData;
            loadDefaultImage();
            FileSystemWatcher watcher = new FileSystemWatcher();    //文件监视
            watcher.Path = @"D:\tmp";    //待监视的路径
            watcher.IncludeSubdirectories = true;   //包含子文件夹
            watcher.EnableRaisingEvents = true;
            watcher.Filter = "*.txt";
            watcher.Changed += OnChanged;
            watcher.Created += OnCreated;
            watcher.Deleted += OnDeleted;
            
        }
        private void loadDefaultImage()
        {
            string picpath = @"D:\图片\2013-01-21\001.JPG";//设置默认图片的路径
            this.config.SetString("file", picpath);
            string imageDefault = config.GetString("file");
            if (File.Exists(imageDefault))
            {
                LoadImage(new Uri(imageDefault));
                UpdateSettings();

            }
        }
        private void OnChanged(object sender, FileSystemEventArgs e)
        {
            string path = e.FullPath;
            try
            {
                string text = File.ReadAllText(path);
                this.config.SetString("file", text);
            }
            catch (Exception exception)
            {
            }
            string img = config.GetString("file");
            if (File.Exists(img))
            {
                LoadImage(new Uri(img));
                UpdateSettings();
            }
            else
            {
                //防止图片不存在导致无法切换的情况
                loadDefaultImage();
            }            
        }
        private void OnCreated(object sender, FileSystemEventArgs e)
        {
            
        }
        private void OnDeleted(object sender, FileSystemEventArgs e)
        {
            
        }
        private void LoadImage(Uri imageUri)
        {
            BitmapImage src = new BitmapImage();
            src.BeginInit();
            src.UriSource = imageUri;
            src.EndInit();
        }
        #region 载入材质相关代码，无须修改
        private void LoadTexture(String imageFile)
        {

            lock (textureLock)
            {
                if (texture != null)
                {
                    texture.Dispose();
                    texture = null;
                }

                if (File.Exists(imageFile))
                {
                    BitmapImage src = new BitmapImage();
                    
                    src.BeginInit();
                    src.UriSource = new Uri(imageFile);
                    src.EndInit();
                    
                    WriteableBitmap wb = new WriteableBitmap(src);
              
                    texture = GS.CreateTexture((UInt32)wb.PixelWidth, (UInt32)wb.PixelHeight, GSColorFormat.GS_BGRA, null, false, false);

                    texture.SetImage(wb.BackBuffer, GSImageFormat.GS_IMAGEFORMAT_BGRA, (UInt32)(wb.PixelWidth * 4));

                    config.Parent.SetInt("cx", wb.PixelWidth);
                    config.Parent.SetInt("cy", wb.PixelHeight);

                    Size.X = (float)wb.PixelWidth;
                    Size.Y = (float)wb.PixelHeight;

                }
                else
                {
                    texture = null;
                }
            }
        }
        override public void Render(float x, float y, float width, float height)
        {
            lock (textureLock)
            {
                if (texture != null)
                {
                    GS.DrawSprite(texture, 0xFFFFFFFF, x, y, x + width, y + height);
                }
            }
        }

        public void Dispose()
        {
            lock (textureLock)
            {
                if (texture != null)
                {
                    texture.Dispose();
                    texture = null;
                }
            }
        }
        #endregion
        override public void UpdateSettings()
        {
            XElement dataElement = config.GetElement("data");
            LoadTexture(config.GetString("file"));
        }     
    }
    
}
