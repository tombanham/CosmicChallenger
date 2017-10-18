namespace Assets.scripts.Exception
{
    public class UnexpectedWeaponType : System.Exception {
        public UnexpectedWeaponType()
            : base() { }

        public UnexpectedWeaponType(string message)
            : base(message) { }
    }
}
