using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CLROBS;
namespace PicSwitcher
{
    class PicSwitcherSourceFactory : AbstractImageSourceFactory
    {
        public PicSwitcherSourceFactory() //构造函数
        {
            ClassName = "PicSwitcher";
            DisplayName = ".NET picture switcher";
        }

        public override ImageSource Create(XElement data)
        {
            return new PicSwitcherSource(data);
        }

        public override bool ShowConfiguration(XElement data)
        {
            return true;//设置为true可以在来源区显示
        }
    }
}
