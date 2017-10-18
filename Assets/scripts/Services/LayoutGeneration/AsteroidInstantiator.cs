using Assets.scripts.Sprites.Environmental;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.scripts.Services.LayoutGeneration
{
    public class AsteroidInstantiator : IAsteroidInstantiator
    {
        AsteroidSource AsteroidSource { get; set; }

        public AsteroidInstantiator(AsteroidSource asteroidSource) {
            AsteroidSource = asteroidSource;
        }

        public List<Asteroid> Instantiate(AsteroidInstantiatorArgs asteroidInstantiatorArgs) {
            var layout = new List<Asteroid>();
            for (int i = 0; i < asteroidInstantiatorArgs.AsteroidSprites.Length; i++) {
                var xString = asteroidInstantiatorArgs.AsteroidXpositions.Substring((i * 3) + 1, 2);
                var yString = asteroidInstantiatorArgs.AsteroidYpositions.Substring((i * 3) + 1, 2);
                if (xString[0] == '0') {
                    xString = xString[1].ToString();
                }
                if (yString[0] == '0') {
                    yString = yString[1].ToString();
                }
                var xpos = System.Int32.Parse(xString);
                var ypos = System.Int32.Parse(yString);
                if (asteroidInstantiatorArgs.AsteroidXpositions[(i * 3)] == '-') {
                    xpos = xpos * -1;
                }
                if (asteroidInstantiatorArgs.AsteroidYpositions[(i * 3)] == '-') {
                    ypos = ypos * -1;
                }
                var pos = new Vector2(xpos, ypos);
                var asteroidPrefab = AsteroidSource.GetAsteroid(System.Int32.Parse(asteroidInstantiatorArgs.AsteroidSprites[i].ToString()));
                var asteroidObject = GameObject.Instantiate(asteroidPrefab, pos, asteroidPrefab.transform.rotation) as Asteroid;
                layout.Add(asteroidObject);
            }
            return layout;
        }
    }
}