using UnityEngine;

namespace Assets.scripts.Services.LayoutGeneration
{
    class NineStripsAsteroidLayout : IAsteroidLayout
    {
        public AsteroidLayoutName AsteroidLayoutName { get; set; }
        public ILayoutGenerator LayoutGenerator { get; set; }

        public NineStripsAsteroidLayout(ILayoutGenerator layoutGenerator) {
            LayoutGenerator = layoutGenerator;
        }

        public void ApplyLayout(){
            ApplyLeftStrips();
            ApplyMiddleStrips();
            ApplyRightStrips();
        }

        private void ApplyLeftStrips() {
            int x = -30;
            int y = -40;
            for (var i = 1; i < 9; i++) {
                if (i == 3 || i == 6) {
                    continue;
                }
                var sprite = Random.Range(1,9);
                LayoutGenerator.StoreLayoutAsteroid(new StoreLayoutAsteroidArgs(x, y+i*7, sprite));
            }
        }

        private void ApplyMiddleStrips() {
            int x = 0;
            int y = -35;
            for (var i = 1; i < 9; i++) {
                if (i==3||i==6) {
                    continue;
                }
                var sprite = Random.Range(1, 9);
                LayoutGenerator.StoreLayoutAsteroid(new StoreLayoutAsteroidArgs(x, y + i * 7, sprite));
            }
        }

        private void ApplyRightStrips() {
            int x = 30;
            int y = -40;
            for (var i = 1; i < 9; i++) {
                if (i == 3 || i == 6) {
                    continue;
                }
                var sprite = Random.Range(1, 9);
                LayoutGenerator.StoreLayoutAsteroid(new StoreLayoutAsteroidArgs(x, y + i * 7, sprite));
            }
        }
    }
}
