namespace UniMob.UI
{
    public sealed class LocalizationParameter
    {
        private string _currentValue;

        public LocalizationParameter(string key, string defaultValue)
        {
            Key = key;

            _currentValue = defaultValue;
        }

        public string Key { get; }

        public string Value
        {
            get => _currentValue;
            set => SetValue(value);
        }

        public void SetValue(string value)
        {
            if (value == _currentValue)
            {
                return;
            }

            _currentValue = value;
            Listener?.NotifyChanged();
        }

        internal ILocalizationListener Listener { get; set; }
    }
}