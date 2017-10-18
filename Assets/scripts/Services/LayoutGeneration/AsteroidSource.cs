using Assets.scripts.Sprites.Environmental;
using UnityEngine;
using System;
using Assets.scripts.Exception;

namespace Assets.scripts.Services.LayoutGeneration
{
    public class AsteroidSource : MonoBehaviour
    {
        public Asteroid asteroidPrefab1;
        public Asteroid asteroidPrefab2;
        public Asteroid asteroidPrefab3;
        public Asteroid asteroidPrefab4;
        public Asteroid asteroidPrefab5;
        public Asteroid asteroidPrefab6;
        public Asteroid asteroidPrefab7;
        public Asteroid asteroidPrefab8;
        public Asteroid asteroidPrefab9;

        public Asteroid GetAsteroid(int asteroidNumber){
            switch (asteroidNumber){
                case 1:
                    return asteroidPrefab1;
                case 2:
                    return asteroidPrefab2;
                case 3:
                    return asteroidPrefab3;
                case 4:
                    return asteroidPrefab4;
                case 5:
                    return asteroidPrefab5;
                case 6:
                    return asteroidPrefab6;
                case 7:
                    return asteroidPrefab7;
                case 8:
                    return asteroidPrefab8;
                case 9:
                    return asteroidPrefab9;
                default:
                    throw new UnexpectedAsteroidNumber("An asteroid which does not exist was requested: " + asteroidNumber);
            }
        }
    }


}
