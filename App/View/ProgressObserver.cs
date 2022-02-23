using System;
using App.ViewModel;
using App.Interfaces;

namespace App.View
{
    class ProgressObserver : IObserver
    {
        private readonly Langue _lang = new Langue();
        public void Update(ISubject subject)
        {
            Console.Write($"\r{_lang.Translation(17)}{(subject as Save).Progress} %");
        }
    }
}