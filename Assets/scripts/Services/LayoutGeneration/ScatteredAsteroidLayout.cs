using UnityEngine;

namespace Assets.scripts.Services.LayoutGeneration
{
    class ScatteredAsteroidLayout : IAsteroidLayout
    {
        public AsteroidLayoutName AsteroidLayoutName { get; set; }
        public ILayoutGenerator LayoutGenerator { get; set; }

        public ScatteredAsteroidLayout(ILayoutGenerator layoutGenerator) {
            LayoutGenerator = layoutGenerator;
        }

        public void ApplyLayout() {
            for (var i = 0; i < BalanceData.MaxAsteroids; i++) {
                var x = Random.Range(-55, 55);
                var y = Random.Range(-35, 25);
                var sprite = Random.Range(1, 9);
                LayoutGenerator.StoreLayoutAsteroid(new StoreLayoutAsteroidArgs(x, y, sprite));
            }
        }
    }
}
