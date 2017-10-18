namespace Assets.scripts.Exception
{
    public class UnexpectedSpawnPoint : System.Exception {
        public UnexpectedSpawnPoint()
            : base() { }

        public UnexpectedSpawnPoint(string message)
            : base(message) { }
    }
}