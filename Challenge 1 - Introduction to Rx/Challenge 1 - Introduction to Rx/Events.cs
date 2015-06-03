using System;

namespace IntroductionToRx
{
    class Events
    {
        public event Action<string> TextChanged;
        private int m_lastLength;
        private Action<string> m_LengthChangedDelegate;

        public virtual void OnTextChanged(string text)
        {
            var t = TextChanged;
            if (t != null)
            {
                t(text);
            }
        }

        public event Action<int> LengthChanged
        {
            add
            {
                // we need to store the reference to our anonymous delegate
                // otherwise we cannot unsubscribe from it
                m_LengthChangedDelegate = s =>
                {
                    var length = (s ?? string.Empty).Length;
                    if (length != m_lastLength)
                    {
                        m_lastLength = length;
                        // call our passed handler for length changed
                        value(length);
                    }
                };

                // as we can the events are much harder to compose than observables
                TextChanged += m_LengthChangedDelegate;
            }
            remove
            {
                TextChanged -= m_LengthChangedDelegate;
            }
        }
    }
}
