using UniMob.UI;
using UniMob.UI.Widgets;
using UnityEngine;

namespace Samples.HelloWorld
{
    public class HelloWorldApp : UniMobUIApp
    {
        protected override void Initialize()
        {
        }

        protected override Widget Build(BuildContext context)
        {
            return new Container(
                backgroundColor: Color.white,
                size: WidgetSize.Stretched,
                child: new UniMobText(
                    value: "Hello World!",
                    size: WidgetSize.Stretched,
                    color: Color.blue,
                    fontSize: 60
                )
            );
        }
    }
}