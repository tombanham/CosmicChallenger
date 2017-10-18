using Assets.scripts.Exception;
using Assets.scripts.Sprites.Health;
using Assets.scripts.Sprites.Units.Weapon;
using System;
using System.Timers;
using UnityEngine;

namespace Assets.scripts.Services
{
    public class UIHelper : MonoBehaviour
    {
        //Static
        public static bool weaponChange = false;
        public static WeaponType playerWeapon = WeaponType.LASER;
        public static int playerAmmo = BalanceData.DefaultAmmo;

        //Instance
        //(gameManager and practice variables allow UIHelper to apply to both managers
        // with more work this class would be generalised further)
        public bool textOn = true;
        public Timer FlashingTextTimer;
        public static int playerLives;
        GameManager gameManager;
        private bool practice;

        //Assigned in editor
        public Sprite laserImage;
        public Sprite rocketImage;
        public Sprite bombImage;
        public Sprite clusterBombImage;
        public Sprite seekerLaserImage;
        public GameObject opponentPlayerImageUIObject;
        public GameObject localPlayerImageUIObject;
        public GameObject currentWeaponImageUIObject;
        public GUIText topUItext;

        public void SetUp() {
            var gameManager = GameObject.Find(AppConstants.GameManagerName);
            if (gameManager != null) {
                this.gameManager = gameManager.GetComponent<GameManager>(); ;
                practice = false;
            } else {
                practice = true;
            }
        }

        public void InitialiseUI()
        {
            localPlayerImageUIObject = Instantiate(localPlayerImageUIObject);
            currentWeaponImageUIObject = Instantiate(currentWeaponImageUIObject) as GameObject;
            if (practice){
                playerLives = BalanceData.MaxLives;
            } else {
                opponentPlayerImageUIObject = Instantiate(opponentPlayerImageUIObject);
            }
            SetUIText();
        }

        public void SetUIText() {
            if (practice) {
                SetUITextSinglePlayer();
            } else {
                SetUItextMultiplayer();
            }
        }

        private void SetUITextSinglePlayer() {
            if (playerAmmo == 0) {
                topUItext.text = UIText.SinglePlayerTopText(AppConstants.UnlimitedAmmoSymbol, playerLives);
            } else {
                topUItext.text = UIText.SinglePlayerTopText(Convert.ToString(playerAmmo), playerLives);
            }
        }

        private void SetUItextMultiplayer() {
            if (GameManager.localPlayer == null) {
                return;
            }

            if (playerAmmo == 0) {
                if (GameManager.opponentPlayer == null) {
                    topUItext.text = UIText.SinglePlayerTopText(
                        ammo: AppConstants.UnlimitedAmmoSymbol,
                        lives: GameManager.localPlayer.GetComponent<Health>().currentLives
                    );
                } else {
                    topUItext.text = UIText.MultiPlayerTopText(
                        ammo: AppConstants.UnlimitedAmmoSymbol,
                        lives: GameManager.localPlayer.GetComponent<Health>().currentLives,
                        enemyLives: GameManager.opponentPlayer.GetComponent<Health>().currentLives
                    );
                }
            } else {
                if (GameManager.opponentPlayer == null) {
                    topUItext.text = UIText.SinglePlayerTopText(
                        ammo: Convert.ToString(GameManager.localPlayer.GetComponent<Ammo>().currentAmmo),
                        lives: GameManager.localPlayer.GetComponent<Health>().currentLives
                    );
                } else {
                    topUItext.text = UIText.MultiPlayerTopText(
                        ammo: Convert.ToString(GameManager.localPlayer.GetComponent<Ammo>().currentAmmo),
                        lives: GameManager.localPlayer.GetComponent<Health>().currentLives,
                        enemyLives: GameManager.opponentPlayer.GetComponent<Health>().currentLives
                    );
                }
            }

            if (GameManager.opponentPlayer != null){
                gameManager.RpcSetClientUIText(GameManager.opponentPlayer.GetComponent<Ammo>().currentAmmo, GameManager.localPlayer.GetComponent<Health>().currentLives, GameManager.opponentPlayer.GetComponent<Health>().currentLives);
            }
        }

        public void DisplayGameOverText(GameStatus gameStatus) {
            switch (gameStatus) {
                case GameStatus.DRAW:
                    topUItext.text = UIText.Draw;
                    break;
                case GameStatus.LOSE:
                    topUItext.text = UIText.Defeat;
                    break;
                case GameStatus.OPPONENTDISC:
                    topUItext.text = UIText.Disconnect;
                    break;
                case GameStatus.WIN:
                    topUItext.text = UIText.Victory;
                    break;
                default:
                    throw new UnexpectedGameStatus("Unexpected GameStatus (given that game is over): " + gameStatus);
            }
        }

        public void RemoveUIimages() {
            if (localPlayerImageUIObject != null && opponentPlayerImageUIObject != null && currentWeaponImageUIObject != null) {
                Destroy(localPlayerImageUIObject);
                Destroy(opponentPlayerImageUIObject);
                Destroy(currentWeaponImageUIObject);
            }
        }

        public void UpdateUIimages() {
            if (!weaponChange || (!practice && GameManager.localPlayer == null)) {
                return;
            }
            UpdateWeaponImage();
            weaponChange = false;
        }

        private void UpdateWeaponImage() {
            switch (playerWeapon) {
                case WeaponType.LASER:
                    currentWeaponImageUIObject.GetComponent<SpriteRenderer>().sprite = laserImage;
                    break;
                case WeaponType.ROCKET:
                    currentWeaponImageUIObject.GetComponent<SpriteRenderer>().sprite = rocketImage;
                    break;
                case WeaponType.BOMB:
                    currentWeaponImageUIObject.GetComponent<SpriteRenderer>().sprite = bombImage;
                    break;
                case WeaponType.CLUSTERBOMB:
                    currentWeaponImageUIObject.GetComponent<SpriteRenderer>().sprite = clusterBombImage;
                    break;
                case WeaponType.SEEKERLASER:
                    currentWeaponImageUIObject.GetComponent<SpriteRenderer>().sprite = seekerLaserImage;
                    break;
                default:
                    currentWeaponImageUIObject.GetComponent<SpriteRenderer>().sprite = laserImage;
                    break;
            }
        }

        public void EnableFlashingText() {
            FlashingTextTimer = new Timer();
            FlashingTextTimer.Elapsed += delegate { textOn = !textOn; };
            FlashingTextTimer.Interval = AppConstants.FlashingTextTimerIntervalMilliseconds;
            FlashingTextTimer.Enabled = true;
        }

    }
}