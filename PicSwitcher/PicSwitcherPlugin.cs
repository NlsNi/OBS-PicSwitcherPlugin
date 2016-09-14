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
        public PicSwitcherPlugin() 
        {
            Name = ".NET picture switch Plugin";
            Description = "A Plugin lets you show a picture and change picture as you wish.";
        }

        public override bool LoadPlugin()
        {
            API.Instance.AddImageSourceFactory(new PicSwitcherSourceFactory());
            return true;
        }
    }
}
