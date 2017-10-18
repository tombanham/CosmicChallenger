namespace Assets.scripts.Exception
{
    public class UnexpectedAsteroidNumber : System.Exception {
        public UnexpectedAsteroidNumber()
            : base() { }

        public UnexpectedAsteroidNumber(string message)
            : base(message) { }
    }
}