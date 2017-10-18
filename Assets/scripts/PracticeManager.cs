using UnityEngine;
using UnityEngine.SceneManagement;
using Assets.scripts.Menu;
using Assets.scripts.Sprites.Units;
using Assets.scripts.Sprites.Units.Weapon;
using Assets.scripts.Services.LayoutGeneration;
using Assets.scripts.Services;

namespace Assets.scripts {
    public class PracticeManager : MonoBehaviour {
        //UI
        MenuButton button;

        //Game Management
        private static int lastID;
        public PracticePlayer localPlayer;

        //Helper classes
        PracticeLayoutGenerator layoutGenerator;
        UIHelper UIHelper;

        void Start() {
            Instantiate(localPlayer);

            UIHelper = GetComponent<UIHelper>();
            UIHelper.SetUp();

            InitaliseLayoutGenerator();
            UIHelper.InitialiseUI();
            lastID = 0;
            button = GameObject.Find(AppConstants.MenuButtonName).GetComponent<MenuButton>();
        }

        private void InitaliseLayoutGenerator() {
            layoutGenerator = GameObject.Find(AppConstants.PracticeManagerName).GetComponent<PracticeLayoutGenerator>();
            layoutGenerator.GenerateBorder();
            layoutGenerator.GenerateRandomLayout();
            layoutGenerator.StartSwitchLayoutTimer();
        }

        public static void SetPlayerLives(int playerLives) {
            UIHelper.playerLives = playerLives;
        }

        public static void UpdateAmmo(int newAmmo) {
            UIHelper.playerAmmo = newAmmo;
        }

        public static void UpdateWeapon(WeaponType weapon) {
            UIHelper.playerWeapon = weapon;
            UIHelper.weaponChange = true;
        }

        void Update() {
            UIHelper.SetUIText();
            UIHelper.UpdateUIimages();

            if (button.buttonPress) {
                SceneManager.LoadScene(AppConstants.MenuSceneName);
            }
        }

        public static int NextID() {
            lastID++;
            return lastID;
        }
    }
}