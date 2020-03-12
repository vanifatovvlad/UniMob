using System.Collections.Generic;
using UniMob;
using UniMob.UI;
using UniMob.UI.Widgets;
using UnityEngine;

namespace Samples.HelloWorld
{
    public class SimpleCounterApp : UniMobUIApp
    {
        private MutableAtom<int> _counter = Atom.Value(0);

        protected override void Initialize()
        {
        }

        protected override Widget Build(BuildContext context)
        {
            return new Container(
                backgroundColor: Color.white,
                size: WidgetSize.Stretched,
                child: BuildContent()
            );
        }

        private Widget BuildContent()
        {
            return new Column(
                mainAxisAlignment: MainAxisAlignment.Center,
                crossAxisAlignment: CrossAxisAlignment.Center,
                children: new List<Widget>
                {
                    BuildCounterText(),
                    BuildIncrementButton()
                }
            );
        }

        private Widget BuildCounterText()
        {
            // wrap frequently updated widgets into Builder widget
            // to reduce widget tree rebuild count (optionally)
            return new Builder(context =>
            {
                return new UniMobText(
                    value: $"Counter: {_counter.Value}",
                    fontSize: 40,
                    size: WidgetSize.Fixed(400, 100),
                    mainAxisAlignment: MainAxisAlignment.Center,
                    crossAxisAlignment: CrossAxisAlignment.Center
                );
            });
        }

        private Widget BuildIncrementButton()
        {
            return new UniMobButton(
                child: new Container(
                    backgroundColor: Color.grey,
                    child: new UniMobText(
                        value: "Increment",
                        fontSize: 40,
                        size: WidgetSize.Fixed(400, 100),
                        mainAxisAlignment: MainAxisAlignment.Center,
                        crossAxisAlignment: CrossAxisAlignment.Center
                    )
                ),
                onClick: () => _counter.Value++
            );
        }
    }
}