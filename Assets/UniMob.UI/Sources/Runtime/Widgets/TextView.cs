using UniMob.UI.Internal;
using UniMob.UI.Widgets;
using UnityEngine;
using UIText = UnityEngine.UI.Text;

[assembly: RegisterComponentViewFactory("$$_Text",
    typeof(RectTransform), typeof(UIText), typeof(TextView))]

namespace UniMob.UI.Widgets
{
    public class TextView : View<ITextState>
    {
        private static Font _defaultFont;

        private UIText _text;

        protected override void Awake()
        {
            base.Awake();

            if (_defaultFont == null)
            {
                _defaultFont = Resources.GetBuiltinResource<Font>("Arial.ttf");
            }

            _text = GetComponent<UIText>();
            _text.font = _defaultFont;
        }

        protected override void Render()
        {
            _text.text = State.Value;
            _text.color = State.Color;
            _text.fontSize = State.FontSize;
            _text.alignment = ToTextAnchor(State.MainAxisAlignment, State.CrossAxisAlignment);
        }

        private static TextAnchor ToTextAnchor(MainAxisAlignment main, CrossAxisAlignment cross)
        {
            switch (main)
            {
                case MainAxisAlignment.Center:
                    return cross == CrossAxisAlignment.Start ? TextAnchor.MiddleLeft
                        : cross == CrossAxisAlignment.Center ? TextAnchor.MiddleCenter
                        : TextAnchor.MiddleRight;

                case MainAxisAlignment.End:
                    return cross == CrossAxisAlignment.Start ? TextAnchor.LowerLeft
                        : cross == CrossAxisAlignment.Center ? TextAnchor.LowerCenter
                        : TextAnchor.LowerRight;

                case MainAxisAlignment.Start:
                    return cross == CrossAxisAlignment.Start ? TextAnchor.UpperLeft
                        : cross == CrossAxisAlignment.Center ? TextAnchor.UpperCenter
                        : TextAnchor.UpperRight;

                default:
                    return TextAnchor.UpperLeft;
            }
        }
    }

    public interface ITextState : IViewState
    {
        string Value { get; }
        int FontSize { get; }
        Color Color { get; }
        MainAxisAlignment MainAxisAlignment { get; }
        CrossAxisAlignment CrossAxisAlignment { get; }
    }
}