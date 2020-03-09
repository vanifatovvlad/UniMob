using JetBrains.Annotations;
using UnityEngine;

namespace UniMob.UI.Widgets
{
    public class Text : StatefulWidget
    {
        public Text(
            WidgetSize size,
            [NotNull] string value,
            int? fontSize = null,
            Color? color = null,
            CrossAxisAlignment crossAxisAlignment = CrossAxisAlignment.Start,
            MainAxisAlignment mainAxisAlignment = MainAxisAlignment.Start,
            [CanBeNull] Key key = null)
            : base(key)
        {
            Size = size;
            Value = value;
            Color = color ?? Color.black;
            CrossAxisAlignment = crossAxisAlignment;
            MainAxisAlignment = mainAxisAlignment;
            FontSize = fontSize ?? 12;
        }

        public WidgetSize Size { get; }

        public string Value { get; }
        public Color Color { get; }
        public CrossAxisAlignment CrossAxisAlignment { get; }
        public MainAxisAlignment MainAxisAlignment { get; }

        public int FontSize { get; }

        public override State CreateState() => new TextState();
    }

    internal class TextState : ViewState<Text>, ITextState
    {
        public override WidgetViewReference View { get; }
            = WidgetViewReference.Resource("$$_Text");

        public string Value => Widget.Value;

        public int FontSize => Widget.FontSize;
        public Color Color => Widget.Color;
        public MainAxisAlignment MainAxisAlignment => Widget.MainAxisAlignment;
        public CrossAxisAlignment CrossAxisAlignment => Widget.CrossAxisAlignment;
    }
}