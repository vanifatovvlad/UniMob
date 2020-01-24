using System.Collections.Generic;
using UniMob.UI.Widgets;
using UnityEngine;

namespace UniMob.UI.Samples.Layout
{
    public class SampleLayoutApp : UniMobUIApp
    {
        protected override void Initialize()
        {
        }

        protected override Widget Build(BuildContext context)
        {
            Widget MakeContainer(int w, int h, int n) => new Container(
                size: WidgetSize.Fixed(w, h),
                backgroundColor: Color.Lerp(Color.blue, Color.green, n / 15f)
            );

            return new Container(
                backgroundColor: Color.gray,
                child: new ScrollList(
                    crossAxisAlignment: CrossAxisAlignment.Center,
                    mainAxisAlignment: MainAxisAlignment.Center,
                    children: new List<Widget>
                    {
                        MakeContainer(100, 100, 1),
                        MakeContainer(120, 110, 2),
                        MakeContainer(110, 120, 3),
                        MakeContainer(100, 140, 4),
                        MakeContainer(150, 110, 5),
                        MakeContainer(110, 100, 6),
                        MakeContainer(100, 100, 7),
                        MakeContainer(120, 110, 8),
                        MakeContainer(100, 100, 9),
                        MakeContainer(100, 100, 10),
                    }
                )
            );
        }
    }
}