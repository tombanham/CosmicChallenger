using UnityEngine;

namespace Assets.scripts.Services.LayoutGeneration
{
    class PlusAsteroidLayout : IAsteroidLayout
    {
        public AsteroidLayoutName AsteroidLayoutName { get; set; }
        public ILayoutGenerator LayoutGenerator { get; set; }

        public PlusAsteroidLayout(ILayoutGenerator layoutGenerator) {
            AsteroidLayoutName = AsteroidLayoutName.PLUS;
            LayoutGenerator = layoutGenerator;
        }

        public void ApplyLayout() {
            ApplyVerticalSection();
            ApplyHorizontalSection();
        }

        public void ApplyVerticalSection(){
            int x = 0;
            int y = -30;
            for (var i = 1; i < 7; i++) {
                var asteroidSprite = Random.Range(1,9);
                LayoutGenerator.StoreLayoutAsteroid(new StoreLayoutAsteroidArgs(x, y + i * 7, asteroidSprite));
            }
        }

        public void ApplyHorizontalSection() {
            int x = -50;
            int y = -5;
            for (var i = 1; i < 14; i++) {
                if (i==7) {
                    continue;
                }
                var asteroidSprite = Random.Range(1, 9);
                LayoutGenerator.StoreLayoutAsteroid(new StoreLayoutAsteroidArgs(x+i*7,y, asteroidSprite));
            }
        }
    }
}
