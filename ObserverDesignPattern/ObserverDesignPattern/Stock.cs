namespace ObserverDesignPattern
{
    public class Stock : Subject
    {
        private string code;
        private double price;
        private int quantity;

        public string Code
        {
            get => code;
            set
            {
                code = value;
            }
        }

        public double Price
        {
            get => price;
            set
            {
                price = value;
                Notify();
            }
        }

        public int Quantity
        {
            get => quantity; 
            set
            {
                quantity = value;
                Notify();
            }
        }

        public override void Notify()
        {
            foreach (var observer in Observers)
            {
                observer.Update(this);
            }
        }
    }
}


