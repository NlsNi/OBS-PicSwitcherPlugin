using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CLROBS;
namespace PicSwitcher
{
    public class PicSwitcherPlugin : AbstractPlugin
    {
        public PicSwitcherPlugin() //构造函数
        {
            Name = "Sample Image Plugin";
            Description = "A Plugin lets you show a picture and change picture as you wish.";
        }

        public override bool LoadPlugin()
        {
            API.Instance.AddImageSourceFactory(new PicSwitcherSourceFactory());
            //API.Instance.AddSettingsPane(new SampleSettingsPane());
            return true;
        }
    }
}
