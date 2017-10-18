namespace Assets.scripts.Services.LayoutGeneration
{
    public interface ILayoutGenerator
    {
        IAsteroidInstantiator AsteroidInstantiator { get; set; }
        void GenerateBorder();
        void StoreLayoutAsteroid(StoreLayoutAsteroidArgs storeLayoutAsteroidArgs);
    }

    public class StoreLayoutAsteroidArgs{
        public int Xpos { get; set; }
        public int Ypos { get; set; }
        public int Sprite { get; set; }

        public StoreLayoutAsteroidArgs(int xpos, int ypos, int sprite) {
            Xpos = xpos;
            Ypos = ypos;
            Sprite = sprite;
        }
    }
}