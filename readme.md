**基于OBS CLR的插件开发**

1. 建立工程，类型为类库。
2. 将CLRHost.Interop.dll复制到工程目录下，添加引用。
3. using CLROBS, 继承AbstractImageSourceFactory 
4. 共3个类文件，plugin，source，sourcefactory
5. 在source类当中需要添加using System.Windows.Media.Imaging;
   using System.Windows.Media;
6. ​