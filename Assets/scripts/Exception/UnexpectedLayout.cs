namespace Assets.scripts.Exception
{
    public class UnexpectedLayout : System.Exception {
        public UnexpectedLayout()
            : base() { }

        public UnexpectedLayout(string message)
            : base(message) { }
    }
}