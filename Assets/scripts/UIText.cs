namespace Assets.scripts
{
    public class UIText
    {
        public const int TitleFontSize = 60;
        public const int GameStatsFontSize = 40;
        public const int FlashingTextFontSize = 35;

        public const string Title = "        Cosmic\n        Challenger";
        public const string Victory = "               VICTORY:\n               PRESS ENTER TO CONTINUE";
        public const string Defeat = "               DEFEAT:\n               PRESS ENTER TO CONTINUE";
        public const string Draw = "               DRAW:\n               PRESS ENTER TO CONTINUE";
        public const string Disconnect = "               VICTORY! (OPPONENT DISC):\n               PRESS ENTER TO CONTINUE";

        public static string SinglePlayerTopText(string ammo, int lives){
            return FormatAmmo(ammo) + lives;
        }

        public static string MultiPlayerTopText(string ammo, int lives, int enemyLives){
            return FormatAmmo(ammo) + FormatLives(lives, enemyLives);
        }

        public static string FormatAmmo(string ammo){
            return "            " + ammo + "         ";
        }

        public static string FormatLives(int lives, int enemyLives){
            return lives + "        " + enemyLives;
        }
    }
}