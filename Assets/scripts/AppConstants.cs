namespace Assets.scripts
{
    public class AppConstants
    {
        //scenes
        public const string MenuSceneName = "Menu";
        public const string GameSceneName = "Game";
        public const string PracticeSceneName = "Practice";
        public const string HelpRulesSceneName = "HelpRules";
        public const string HelpWeaponsSceneName = "HelpWeapons";
        public const string HelpPracticeSceneName = "HelpPractice";

        //navigation
        public const string NavigationBackKey = "escape";
        public const string NavigationForwardKey = "space";
        public const string NavigationStartGameKey = "m";
        public const string NavigationStartPracticeKey = "p";
        public const string NavigationHelpKey = "h";

        //game object names
        public const string NetworkManagerName = "Network Manager(Clone)";
        public const string GameManagerName = "Game Manager";
        public const string SoundEffectsHelperName = "Sound Effects Helper(clone)";
        public const string NetworkConnectionHelperName = "Network Connection Helper";
        public const string PracticePlayerName = "Practice Player(Clone)";
        public const string LayoutGeneratorName = "Layout Generator";
        public const string PracticeManagerName = "Practice Manager";
        public const string AsteroidSourceName = "AsteroidSource";

        //buttons
        public const string MenuButtonName = "MenuButton";

        //geometry
        public const string VerticalAxisName = "Vertical";
        public const string HorizontalAxisName = "Horizontal";
        public const int BorderTopLeftCornerXpos = -60;
        public const int BorderTopLeftCornerYpos = 30;
        public const int BorderTopRightCornerXpos = 60;
        public const int BorderTopRightCornerYpos = 30;
        public const int BorderBottomRightCornerXpos = 60;
        public const int BorderBottomRightCornerYpos = -40;
        public const int BorderBottomLeftCornerXpos = -60;
        public const int BorderBottomLeftCornerYpos = -40;
        public const int NumberOfLayouts = 3;

        //other
        public const string UnlimitedAmmoSymbol = "∞";
        public const int FlashingTextTimerIntervalMilliseconds = 250;
        public const int ShipTrailMaxParticles = 10;
    }
}