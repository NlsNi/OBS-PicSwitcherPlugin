**本文所述的OBS 插件编写基于OBS CLRHostPlugin.使用C# 语言写成，IDE为VS2012**

完整代码详见Github [仓库地址](https://github.com/NlsNi/OBS-PicSwitcherPlugin)

**Part Ⅰ 创建工程**

1. 首先创建一个C#的项目，项目类型为类库（ClassLibrary）,使用.NET Framework 4.5 ,名称自定，本文以PicSwitcher为例。

2. 将CLRHost.Interop.dll这个动态链接库放入工程路径下，该文件在OBS的安装路径下可以找到，C:\Program Files (x86)\OBS\plugins\CLRHostPlugin

3. 创建3个类文件，PicSwitcherPlugin.cs ,PicSwitcherSource.cs 以及PicSwitcherSourceFactory.cs.

4. 添加引用，所有引用的图如下：
   ![引用列表](http://odh8qadsk.bkt.clouddn.com/%E5%BC%95%E7%94%A8.png)

5. 为C#项目添加后期生成事件命令行，代码如下。

   ```powershell
   copy "$(TargetDir)$(TargetFileName)" "C:\Program Files\OBS\plugins\CLRHostPlugin"
   copy "$(TargetDir)$(TargetFileName)" "C:\Program Files (x86)\OBS\plugins\CLRHostPlugin"
   ```

   ​

   这一步的目的在于简化插件的安装，因为OBS的插件是直接将动态链接库放在插件目录下即可,所以编译成功后利用后期生成事件命令行可以将生成的动态链接库文件直接复制到OBS的插件目录。

   ​

**Part Ⅱ 编写代码**

**PicSwitcherPlugin.cs**

这个类是整个插件的入口，首先 using CLROBS; 添加对CLROBS命名空间的引用。

```C#
public class PicSwitcherPlugin : AbstractPlugin
    {
        public PicSwitcherPlugin() 
        {
            Name = ".NET Image Plugin";
            Description = "A Plugin lets you show a picture and change picture as you wish.";
        }

        public override bool LoadPlugin()
        {
            API.Instance.AddImageSourceFactory(new PicSwitcherSourceFactory());
            return true;
        }
    }
```

API.Instance.AddImageSourceFactory(new PicSwitcherSourceFactory());将这一行注释掉，就完成了最基本的插件（只不过没有具体的功能而已），但是在OBS的日志窗口还是可以看到插件载入的。OBS 帮助→显示日志窗口，可以看到类似的日志即表明自己编写的插件成功载入。

CLRHost::LoadPlugins() attempting to load the plugin assembly PicSwitcher
CLRHost::LoadPlugins() successfully added CLR plugin [Type: PicSwitcher.PicSwitcherPlugin, Name: .NET picture switch Plugin]

**PicSwitcherSourceFactory.cs**

OBS 采用工厂设计模式来创建新的插件实例。在SourceFactory中只要写好构造函数并覆写相应的方法即可。需要注意的是ShowConfiguration() 方法的返回值要设置为true，否则在OBS的源区域将无法找到该插件提供的源。

**PicSwitcherSource.cs**	

主要的功能代码在这个类当中实现。注意需要添加命名空间System.Windows.Media和System.Windows.Media.Imaging，其中的LoadTexture()方法是官方示例程序中的代码，无需进行修改。本文的插件是添加了文件监测功能，检测某一路径下的txt文件变化，如发生变化，则读取变化的文件之中的内容（图片的地址），然后载入该图片实现串流过程中的图片切换。





