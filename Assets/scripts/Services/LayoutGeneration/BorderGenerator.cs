using Assets.scripts.Services.Spawners;
using Assets.scripts.Sprites.Environmental;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.scripts.Services.LayoutGeneration
{
    public class BorderGenerator
    {
        public AsteroidSource AsteroidSource { get; set;  }

        public BorderGenerator(AsteroidSource asteroidSource) {
            AsteroidSource = asteroidSource;
        }

        public void GenerateBorder() {
            GenerateTopBorder();
            GenerateBottomBorder();
            GenerateLeftBorder();
            GenerateRightBorder();
        }

        private void GenerateTopBorder(){
            var x = AppConstants.BorderTopLeftCornerXpos;
            var y = AppConstants.BorderTopLeftCornerYpos;
            for (int i = 0; i < 16; i++){
                Vector2 pos = new Vector2(x + 8 * i, y);
                var asteroidPrefab = AsteroidSource.GetAsteroid(Random.Range(1, 9));
                GameObject.Instantiate(asteroidPrefab, pos, asteroidPrefab.transform.rotation);
            }
        }

        private void GenerateBottomBorder() {
            var x = AppConstants.BorderBottomLeftCornerXpos;
            var y = AppConstants.BorderBottomLeftCornerYpos;
            for (int i = 0; i < 16; i++){
                Vector2 pos = new Vector2(x + 8 * i, y);
                var asteroidPrefab = AsteroidSource.GetAsteroid(Random.Range(1, 9));
                GameObject.Instantiate(asteroidPrefab, pos, asteroidPrefab.transform.rotation);
            }
        }

        private void GenerateLeftBorder() {
            var x = AppConstants.BorderBottomLeftCornerXpos;
            var y = AppConstants.BorderBottomLeftCornerYpos;
            for (int i = 1; i < 9; i++){
                Vector2 pos = new Vector2(x, y + i * 8);
                var asteroidPrefab = AsteroidSource.GetAsteroid(Random.Range(1, 9));
                GameObject.Instantiate(asteroidPrefab, pos, asteroidPrefab.transform.rotation);
            }
        }

        private void GenerateRightBorder() {
            var x = AppConstants.BorderBottomRightCornerXpos;
            var y = AppConstants.BorderBottomRightCornerYpos;
            for (int i = 1; i < 9; i++) {
                Vector2 pos = new Vector2(x, y + i * 8);
                var asteroidPrefab = AsteroidSource.GetAsteroid(Random.Range(1, 9));
                GameObject.Instantiate(asteroidPrefab, pos, asteroidPrefab.transform.rotation);
            }
        }
    }
}
