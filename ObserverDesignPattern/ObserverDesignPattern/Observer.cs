namespace ObserverDesignPattern
{
    public abstract class Observer : IObserver
    {
        public abstract void Update(Stock stock);
    }
}
