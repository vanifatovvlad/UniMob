using System;
using UniMob.UI.Internal;

namespace UniMob.UI
{
    public abstract class View<TState> : ViewBase<TState>, ILocalizationParameterProvider
        where TState : IState
    {
        private DefaultLocalizationProvider _localization;

        protected sealed override void DidStateAttached(TState state)
        {
            try
            {
                using (Atom.NoWatch)
                {
                    state.DidViewMount(this);
                }
            }
            catch (Exception ex)
            {
                Zone.Current.HandleUncaughtException(ex);
            }
        }

        protected sealed override void DidStateDetached(TState state)
        {
            try
            {
                using (Atom.NoWatch)
                {
                    state.DidViewUnmount(this);
                }
            }
            catch (Exception ex)
            {
                Zone.Current.HandleUncaughtException(ex);
            }
        }

        private ILocalizationParameterProvider GetLocalization()
        {
            return _localization
                   ?? (_localization = new DefaultLocalizationProvider(LocalizationParameters));
        }

        protected virtual LocalizationParameter[] LocalizationParameters => DefaultLocalizationProvider.EmptyParameters;

        LocalizationParameter[] ILocalizationParameterProvider.Parameters => GetLocalization().Parameters;

        string ILocalizationParameterProvider.GetValue(string paramName) => GetLocalization().GetValue(paramName);

        void ILocalizationParameterProvider.AddListener(ILocalizationListener listener) =>
            GetLocalization().AddListener(listener);

        void ILocalizationParameterProvider.RemoveListener(ILocalizationListener listener) =>
            GetLocalization().RemoveListener(listener);
    }
}