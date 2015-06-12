using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reactive.Subjects;
using System.Reactive.Linq;
using System.Reactive.Disposables;

namespace IntroductionToRx
{
    class Observables
    {
        ISubject<string> textChanged = new Subject<string>(); 

        public virtual void OnTextChanged(string text)
        {
            // textChanged acts here like an Observer
            // send the notification to the Observer
            // so basically provider is providing itself with the new data
            textChanged.OnNext(text);
        }


        public IObservable<string> TextChanged
        {
            get
            {
                // textChanged acts here like an observable
                return textChanged;
            }
        }

        public IObservable<int> LengthChanged
        {
            get
            {
                // DistinctUntilChanged suppress duplicate consecutive items emitted by the source Observable
                return TextChanged.Select(s => s == null ? 0 : s.Length).DistinctUntilChanged();
            }
        }
    }
}
