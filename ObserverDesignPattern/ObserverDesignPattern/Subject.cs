using System.Collections.Generic;

namespace ObserverDesignPattern
{
    public abstract class Subject : ISubject
    {
        private IList<IObserver> observers = new List<IObserver>();

        public IList<IObserver> Observers
        {
            get => observers;
            set => observers = value;
        }

        public abstract void Notify();

        public void Attach(IObserver observer)
        {
            lock(observers)
            {
                observers.Add(observer);
            }
        }

        public void Detach(IObserver observer)
        {
            lock (observers)
            {
                observers.Remove(observer);
            }
        }
    }
}
