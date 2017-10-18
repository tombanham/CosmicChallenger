namespace Assets.scripts.Exception
{
    public class UnexpectedGameStatus : System.Exception {
        public UnexpectedGameStatus()
            : base() { }

        public UnexpectedGameStatus(string message)
            : base(message) { }
    }
}
