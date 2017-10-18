using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using System;
using Assets.scripts.Sprites.Units;
using Assets.scripts.Sprites.Units.Weapon;
using Assets.scripts.Services.LayoutGeneration;
using Assets.scripts.Sprites.Health;
using Assets.scripts.Menu;
using Assets.scripts.Services;

namespace Assets.scripts {
    public class GameManager : NetworkBehaviour {
        //Game Management
        private static int lastID = 0;
        public static GameStatus gameStatus;
        public static Player localPlayer;
        public static Player opponentPlayer;
        public static bool localPlayerAlive;
        public static bool opponentPlayerAlive;

        //Helper Classes
        private MultiplayerLayoutGenerator layoutGenerator;
        private UIHelper UIHelper;
        private NetworkConnectionHelper networkConnectionHelper;

        void Start () {
            InitialiseHelperClasses();
            layoutGenerator.GenerateBorder();
            if (isServer) {
                layoutGenerator.GenerateRandomLayout();
                layoutGenerator.StartSwitchLayoutTimer();
            }
            InitialiseGameStatus();
            InitialiseFlags();
            UIHelper.InitialiseUI();
            if (opponentPlayer != null) {
                RpcSetClientUIText(opponentPlayer.GetComponent<Ammo>().currentAmmo, localPlayer.GetComponent<Health>().currentLives, opponentPlayer.GetComponent<Health>().currentLives);
            }
        }

        private void InitialiseHelperClasses() {
            UIHelper = GetComponent<UIHelper>();
            UIHelper.SetUp();
            layoutGenerator = GameObject.Find(AppConstants.LayoutGeneratorName).GetComponent<MultiplayerLayoutGenerator>();
            networkConnectionHelper = GameObject.Find(AppConstants.NetworkConnectionHelperName).GetComponent<NetworkConnectionHelper>();
        }

        private void InitialiseGameStatus() {
            if (isServer){
                gameStatus = GameStatus.GAMEREADY;
            } else {
                gameStatus = GameStatus.GAMEON;
            }
        }

        private void InitialiseFlags() {
            localPlayerAlive = true;
            opponentPlayerAlive = true;
        }

        [ClientRpc]
        public void RpcSetClientUIText(int ammo, int serverLives, int clientLives) {
            if (isServer || UIHelper == null) {
                return;
            }

            UIHelper.topUItext.text = "";
            if (ammo == 0) {
                UIHelper.topUItext.text += UIText.FormatAmmo(AppConstants.UnlimitedAmmoSymbol);
            } else {
                UIHelper.topUItext.text += UIText.FormatAmmo(Convert.ToString(ammo));
            }
            UIHelper.topUItext.text +=  UIText.FormatLives(clientLives, serverLives);
        }

        public static void UpdateAmmo(int newAmmo) {
            UIHelper.playerAmmo = newAmmo;
        }

        public static void UpdateWeapon(WeaponType weapon) {
            UIHelper.playerWeapon = weapon;
            UIHelper.weaponChange = true;
        }

        private void GameOverCheck() {
            if (isServer) {
                if (gameStatus == GameStatus.GAMEON) {
                    bool gameOver = CheckWinConditions();
                    if (gameOver) {
                        RpcSyncGameStatus(gameStatus);
                    }
                }
            }
        }

        void Update () {
            GameOverCheck();

            switch (gameStatus) {
                case GameStatus.GAMEREADY:
                    UpdateGivenGAMEREADY();
                    break;
                case GameStatus.GAMEON:
                    UpdateGivenGAMEON();
                    break;
                case GameStatus.WIN:
                case GameStatus.LOSE:
                case GameStatus.DRAW:
                case GameStatus.OPPONENTDISC:
                    UpdateGivenGameOver();
                    break;
                default:
                    break;
            }
        }

        private void UpdateGivenGameOver() {
            if (Input.GetKeyDown(KeyCode.Return)) {
                MenuButton menuButton = GameObject.Find(AppConstants.MenuButtonName).GetComponent<MenuButton>();
                menuButton.StopGame();
                SceneManager.LoadScene(AppConstants.MenuSceneName);
            }

            if (UIHelper.textOn) {
                UIHelper.DisplayGameOverText(gameStatus);
            }
            else {
                UIHelper.topUItext.text = "";
            }

            if (UIHelper.FlashingTextTimer == null) {
                UIHelper.EnableFlashingText();
            }

            UIHelper.RemoveUIimages();
        }

        private void UpdateGivenGAMEREADY() {
            if (isServer) {
                UIHelper.SetUIText();
            }
            UIHelper.UpdateUIimages();
            if (opponentPlayer != null) {
                gameStatus = GameStatus.GAMEON;
            }
        }

        private void UpdateGivenGAMEON() {
            if (isServer) {
                UIHelper.SetUIText();
            }
            UIHelper.UpdateUIimages();
            DisconnectionCheck();
        }

        private void DisconnectionCheck() {
            if (opponentPlayer == null) {
                gameStatus = GameStatus.OPPONENTDISC;
                UIHelper.topUItext.fontSize = UIText.StandardFontSize;
                UIHelper.topUItext.text = UIText.Disconnect;
                if (isServer) {
                    StopServer();
                }
            }
        }

        [ClientRpc]
        void RpcSyncGameStatus(GameStatus newStatus) {
            if (!isServer) {
                if (newStatus == GameStatus.WIN) {
                    newStatus = GameStatus.LOSE;
                } else if (newStatus == GameStatus.LOSE) {
                    newStatus = GameStatus.WIN;
                }

                gameStatus = newStatus;
            }
        }

        private bool CheckWinConditions() {
            if (!localPlayerAlive && !opponentPlayerAlive) {
                gameStatus = GameStatus.DRAW;
                return true;
            } else if (!opponentPlayerAlive) {
                gameStatus = GameStatus.WIN;
                return true;
            } else if (!localPlayerAlive) {
                gameStatus = GameStatus.LOSE;
                return true;
            }
            return false;
        }

        public void StopServer() {
            networkConnectionHelper.StopServer();
        }

        public void StopClient() {
            networkConnectionHelper.StopClient();
        }

        public static int NextID() {
            lastID++;
            return lastID;
        }
    }
}