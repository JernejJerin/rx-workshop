using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IntroductionToRx
{
    class Program
    {
        static void Main(string[] args)
        {
            // Expected Output:
            // *** Events ***
            // The
            // Reactive
            // Extensions
            // 10
            // are
            // 3
            // 13
            // *** Observables ***
            // The
            // Reactive
            // Extensions
            // 10
            // are
            // 3
            // 13


            Console.WriteLine("*** Events ***");
            var events = new Events();
            Action<string> textHandler = text => Console.WriteLine(text);
            Action<int> lengthHandler = length => Console.WriteLine(length);
            events.TextChanged += textHandler;
            events.OnTextChanged("The");
            events.OnTextChanged("Reactive");
            events.LengthChanged += lengthHandler;
            events.OnTextChanged("Extensions");
            events.OnTextChanged("are");
            events.TextChanged -= textHandler;
            events.OnTextChanged("compositional");
            events.LengthChanged -= lengthHandler;
            events.OnTextChanged("!");

            Console.WriteLine("*** Observables ***");
            var observables = new Observables();
            // here we subscribe to observable
            var textChangedSubscription = observables.TextChanged.Subscribe(text => Console.WriteLine(text));
            observables.OnTextChanged("The");
            observables.OnTextChanged("Reactive");
            var lengthSubscription = observables.LengthChanged.Subscribe(length => Console.WriteLine(length));
            observables.OnTextChanged("Extensions");
            observables.OnTextChanged("are");

            // much less hassle for disposing compare to Event. We eliminate that particular subscription that is 
            // sitting there on the event source
            textChangedSubscription.Dispose();
            observables.OnTextChanged("compositional");
            lengthSubscription.Dispose();
            observables.OnTextChanged("!");
        }
    }
}
