namespace UniMob.UI
{
    public interface ILocalizationParameterProvider
    {
        LocalizationParameter[] Parameters { get; }

        string GetValue(string paramName);

        void AddListener(ILocalizationListener listener);
        void RemoveListener(ILocalizationListener listener);
    }
}