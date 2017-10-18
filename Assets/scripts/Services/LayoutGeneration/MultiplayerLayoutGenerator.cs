using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Networking;
using Assets.scripts.Sprites.Environmental;
using System.Timers;
using System.Linq;
using Assets.scripts.Exception;

namespace Assets.scripts.Services.LayoutGeneration {
    public class MultiplayerLayoutGenerator : NetworkBehaviour, ILayoutGenerator {
        List<Asteroid> layout = new List<Asteroid>();

        //the following coordinate strings include 3 characters for each of the <BalanceData.MaxAsteroids> asteroids. (one char for -/+, two chars for value)
        //Asteroids are being stored in this way as synchronising encoded strings and having the client and server
        //instantiate the asteroids themselves is much easier than trying to make the server do lots of synchronised instantiations
        [SyncVar(hook = "OnChangeXpos")]
        string asteroidXpositions = "";
        [SyncVar(hook = "OnChangeYpos")]
        string asteroidYpositions = "";
        //the following string includes a number for each of the asteroids indicating the sprites of each asteroid.
        //the sprites are accessed via the AsteroidSource variable.
        [SyncVar(hook = "OnChangeSprites")]
        string asteroidSpriteNumbers = "";

        Timer SwitchLayoutTimer;

        bool switchLayoutFlag = false;

        string previousXPosString = "";

        BorderGenerator BorderGenerator;
        AsteroidSource AsteroidSource;
        IAsteroidLayout AsteroidLayout;
        public IAsteroidInstantiator AsteroidInstantiator { get; set; }

        private void Awake(){
            AsteroidSource = GameObject.Find(AppConstants.AsteroidSourceName).GetComponent<AsteroidSource>();
            BorderGenerator = new BorderGenerator(AsteroidSource);
            AsteroidInstantiator = new AsteroidInstantiator(AsteroidSource);
        }

        void Start() {
            InstantiateLayout();
        }

        public void StartSwitchLayoutTimer() {
            SwitchLayoutTimer = new Timer();
            SwitchLayoutTimer.Elapsed += delegate { SetSwitchLayoutFlag(true); };
            SwitchLayoutTimer.Interval = BalanceData.SwitchLayoutIntervalMilliseconds;
            SwitchLayoutTimer.Enabled = true;
        }

        public void GenerateBorder() {
            BorderGenerator.GenerateBorder();
        }

        void Update() {
            if (isServer && switchLayoutFlag == true) {
                     SetNewLayout();
            }
            if (previousXPosString != asteroidXpositions) {
                SetSwitchLayoutFlag(false);
                previousXPosString = asteroidXpositions;

                InstantiateLayout();
            }
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

        public void GenerateRandomLayout() {
            var layoutName = RandomAndDifferentAsteroidLayoutName();
            switch (layoutName){
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
                    throw new UnexpectedLayout("LayoutGenerator.GenerateRandomLayout attempted to generate an unexpected layout: " + layoutName);
            }
        }

        private AsteroidLayoutName RandomAndDifferentAsteroidLayoutName() {
            var layoutNumbers = Enumerable.Range(0, AppConstants.NumberOfLayouts).ToList();
            if (AsteroidLayout != null) {
                layoutNumbers.Remove((int)AsteroidLayout.AsteroidLayoutName);
            }
            var randomSelector = new System.Random();
            var randomIndex = randomSelector.Next(layoutNumbers.Count);
            var layoutName = (AsteroidLayoutName)layoutNumbers[randomIndex];
            return layoutName;
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
            OnChangeXpos("");
            OnChangeYpos("");
            OnChangeSprites("");
        }

        public void ClearLayout() {
            foreach (var asteroid in layout) {
                Destroy(asteroid.gameObject);
            }
            layout = new List<Asteroid>();
        }

        public void SetNewLayout() {
            ResetLayoutData();
            GenerateRandomLayout();
            SoundEffectsHelper.Instance.MakeLayoutChangeSound();
        }

        void SetSwitchLayoutFlag(bool assignment) {
            switchLayoutFlag = assignment;
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
            OnChangeXpos(asteroidXpositions + xString);
            OnChangeYpos(asteroidYpositions + yString);
            OnChangeSprites(asteroidSpriteNumbers + sprite);
        }

        void OnChangeXpos(string s) {
            asteroidXpositions = s;
        }
        void OnChangeYpos(string s) {
            asteroidYpositions = s;
        }
        void OnChangeSprites(string s) {
            asteroidSpriteNumbers = s;
        }
    }
}