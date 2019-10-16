using System.Collections.Generic;

namespace ObserverDesignPattern
{
    public interface ISubject
    {
        IList<IObserver> Observers { get; set; }

        void Notify();

        void Attach(IObserver observer);

        void Detach(IObserver observer);
    }
}