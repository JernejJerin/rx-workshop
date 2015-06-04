using System;
using System.Linq;
using System.Reactive.Linq;
using System.Windows.Forms;
using System.Reactive;
using UnifiedProgrammingModel.DictionaryService;

namespace UnifiedProgrammingModel
{
    class Program
    {
        static void Main(string[] args)
        {
            var txt = new TextBox();
            var lst = new ListBox { Top = txt.Height + 10 };
            var frm = new Form { Controls = { txt, lst } };

            // convert .NET event "TextChanged" to IObservable
            var textChanged = Observable.FromEventPattern(txt, "TextChanged");
            
            // wrap asynchronous call to begin/end invoke with an asynchronous function which handles the asynchronous call for you
            var getSuggestions = Observable.FromAsyncPattern<string, DictionaryWord[]>(BeginMatch, EndMatch);

            var results = from _ in textChanged
                          let text = txt.Text
                          where text.Length >= 3
                          from suggestions in getSuggestions(text)  // getSuggestions returns a map from string to IObservable array
                          select suggestions;

            using (results
                .ObserveOn(lst)
                .Subscribe(words =>
                {
                    lst.Items.Clear();
                    lst.Items.AddRange(words.Select(word => word.Word).Take(10).ToArray());
                }))
            {
                Application.Run(frm);
            }
        }

        static DictServiceSoapClient service = new DictServiceSoapClient("DictServiceSoap");

        static IAsyncResult BeginMatch(string prefix, AsyncCallback callback, object state)
        {
            return service.BeginMatchInDict("wn", prefix, "prefix", callback, state);
        }

        static DictionaryWord[] EndMatch(IAsyncResult result)
        {
            return service.EndMatchInDict(result);
        }
    }
}
