using Assets.scripts.Sprites.Environmental;
using System.Collections.Generic;

namespace Assets.scripts.Services.LayoutGeneration
{
    public interface IAsteroidInstantiator
    {
        List<Asteroid> Instantiate(AsteroidInstantiatorArgs asteroidInstantiatorArgs);
    }

    public class AsteroidInstantiatorArgs{
        public string AsteroidXpositions { get; set; }
        public string AsteroidYpositions { get; set; }
        public string AsteroidSprites { get; set; }

        public AsteroidInstantiatorArgs(string asteroidXpositions, string asteroidYpositions, string asteroidSprites) {
            AsteroidXpositions = asteroidXpositions;
            AsteroidYpositions = asteroidYpositions;
            AsteroidSprites = asteroidSprites;
        }
    }
}