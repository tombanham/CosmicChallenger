using UnityEngine;
using System.Collections.Generic;
using System.Timers;
using Assets.scripts.Sprites.Environmental;
using Assets.scripts.Exception;

namespace Assets.scripts.Services.LayoutGeneration {
    public class PracticeLayoutGenerator : MonoBehaviour, ILayoutGenerator {
        List<Asteroid> layout = new List<Asteroid>();

        //the following coordinate strings include 3 characters for each of the <maxAsteroids> asteroids. (one char for -/+, two chars for value)
        //Asteroids are being stored in this way to mimic MultiplayerLayoutGenerator, however it could have been implemented differently
        //for the practice mode. Using the same method allows reuse of the various helper classes such as 'AsteroidInstantiator'.
        //Though similar, the practice and multiplayer layout generator are fundamentally different due to extending different base classes,
        //PracticeLayoutGenerator also doesn't use any SyncVars.
        string asteroidXpositions = "";
        string asteroidYpositions = "";
        //the following string includes a number for each of the asteroids indicating the sprites of each asteroid.
        //the sprites are accessed via the AsteroidSource variable.
        string asteroidSpriteNumbers = "";

        Timer SwitchLayoutTimer;

        bool switchLayout = false;

        AsteroidSource AsteroidSource;
        BorderGenerator BorderGenerator;
        IAsteroidLayout AsteroidLayout;
        public IAsteroidInstantiator AsteroidInstantiator { get; set; }

        private void Awake() {
            AsteroidSource = GameObject.Find(AppConstants.AsteroidSourceName).GetComponent<AsteroidSource>();
            BorderGenerator = new BorderGenerator(AsteroidSource);
            AsteroidInstantiator = new AsteroidInstantiator(AsteroidSource);
        }

        void Start() {
            InstantiateLayout();
        }

        public void StartSwitchLayoutTimer() {
            SwitchLayoutTimer = new Timer();
            SwitchLayoutTimer.Elapsed += delegate { SetSwitchLayout(true); };
            SwitchLayoutTimer.Interval = BalanceData.SwitchLayoutIntervalMilliseconds;
            SwitchLayoutTimer.Enabled = true;
        }

        void Update() {
            if (switchLayout) {
                SetSwitchLayout(false);
                SetNewLayout();
                InstantiateLayout();
            }
        }

        public void SetNewLayout() {
            ResetLayoutData();
            GenerateRandomLayout();
            SoundEffectsHelper.Instance.MakeLayoutChangeSound();
        }

        private void InstantiateLayout() {
            ClearLayout();
            InstantiateAsteroids();
        }

        void InstantiateAsteroids() {
            var instantiatedAsteroids = AsteroidInstantiator.Instantiate(
                new AsteroidInstantiatorArgs(asteroidXpositions, asteroidYpositions, asteroidSpriteNumbers)
            );
            layout.AddRange(instantiatedAsteroids);
        }

        public void GenerateBorder() {
            BorderGenerator.GenerateBorder();
        }

        public void GenerateRandomLayout() {
            var layoutName = (AsteroidLayoutName)Random.Range(0, 3);
            switch (layoutName) {
                case AsteroidLayoutName.PLUS:
                    GeneratePlusLayout();
                    break;
                case AsteroidLayoutName.NINESTRIPS:
                    GenerateNineStripsLayout();
                    break;
                case AsteroidLayoutName.SCATTERED:
                    GenerateScatteredLayout();
                    break;
                default:
                    throw new UnexpectedLayout("PracticeLayoutGenerator.GenerateRandomLayout attempted to generate an unexpected layout: " + layoutName);
            }
        }

        private void GeneratePlusLayout() {
            AsteroidLayout = new PlusAsteroidLayout(this);
            AsteroidLayout.ApplyLayout();
        }

        private void GenerateNineStripsLayout() {
            AsteroidLayout = new NineStripsAsteroidLayout(this);
            AsteroidLayout.ApplyLayout();
        }

        public void GenerateScatteredLayout() {
            AsteroidLayout = new ScatteredAsteroidLayout(this);
            AsteroidLayout.ApplyLayout();
        }

        public void ResetLayoutData() {
            ClearLayout();
            asteroidXpositions = "";
            asteroidYpositions = "";
            asteroidSpriteNumbers = "";
        }

        public void ClearLayout() {
            foreach (Asteroid a in layout) {
                Destroy(a.gameObject);
            }
            layout = new List<Asteroid>();
        }

        private void SetSwitchLayout(bool assignment) {
            switchLayout = assignment;
        }

        public void StoreLayoutAsteroid(StoreLayoutAsteroidArgs storeLayoutAsteroidArgs) {
            string xString, yString;
            int xpos = storeLayoutAsteroidArgs.Xpos;
            int ypos = storeLayoutAsteroidArgs.Ypos;
            int sprite = storeLayoutAsteroidArgs.Sprite;

            if (xpos >= 0) {
                if (xpos < 10) {
                    xString = "+0" + xpos;
                } else {
                    xString = "+" + xpos;
                }
            } else {
                if (xpos > -10) {
                    xString = "-0" + xpos * -1;
                } else {
                    xString = "-" + xpos * -1;
                }
            }
            if (ypos >= 0) {
                if (ypos < 10) {
                    yString = "+0" + ypos;
                } else {
                    yString = "+" + ypos;
                }
            } else {
                if (ypos > -10) {
                    yString = "-0" + ypos * -1;
                } else {
                    yString = "-" + ypos * -1;
                }

            }
            asteroidXpositions = asteroidXpositions + xString;
            asteroidYpositions = asteroidYpositions + yString;
            asteroidSpriteNumbers = asteroidSpriteNumbers + sprite;
        }
    }
}