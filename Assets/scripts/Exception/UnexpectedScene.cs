namespace Assets.scripts.Exception
{
    public class UnexpectedScene : System.Exception {
        public UnexpectedScene()
            : base() { }

        public UnexpectedScene(string message)
            : base(message) { }
    }
}
