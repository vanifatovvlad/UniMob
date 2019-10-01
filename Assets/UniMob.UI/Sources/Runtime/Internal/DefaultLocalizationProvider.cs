using System;
using System.Collections.Generic;

namespace UniMob.UI.Internal
{
    public class DefaultLocalizationProvider : ILocalizationParameterProvider, ILocalizationListener
    {
        public static readonly LocalizationParameter[] EmptyParameters = new LocalizationParameter[0];

        private readonly List<ILocalizationListener> _listeners = new List<ILocalizationListener>();

        public LocalizationParameter[] Parameters { get; }

        public DefaultLocalizationProvider(LocalizationParameter[] parameters)
        {
            Parameters = parameters;

            foreach (var parameter in parameters)
            {
                parameter.Listener = this;
            }
        }

        public string GetValue(string paramName)
        {
            for (var index = 0; index < Parameters.Length; index++)
            {
                var parameter = Parameters[index];
                if (parameter.Key.Equals(paramName, StringComparison.InvariantCulture))
                {
                    return parameter.Value;
                }
            }

            return null;
        }

        public void AddListener(ILocalizationListener listener) => _listeners.Add(listener);

        public void RemoveListener(ILocalizationListener listener) => _listeners.Remove(listener);

        public void NotifyChanged()
        {
            foreach (var listener in _listeners)
            {
                listener.NotifyChanged();
            }
        }
    }
}