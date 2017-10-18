using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;
using System;
using Assets.scripts.Exception;

namespace Assets.scripts.Menu
{
    public class MenuButton : MonoBehaviour
    {
        public Scene scene;

        public Sprite upSprite;
        public Sprite downSprite;

        public bool buttonPress = false;

        void Update () {
            var sceneName = SceneManager.GetActiveScene().name;
            if (Input.GetKey(AppConstants.NavigationBackKey)) {
                switch (sceneName) {
                    case AppConstants.MenuSceneName:
                        Application.Quit();
                        break;
                    case AppConstants.GameSceneName:
                        StopGame();
                        SceneManager.LoadScene(AppConstants.MenuSceneName);
                        break;
                    case AppConstants.PracticeSceneName:
                        SceneManager.LoadScene(AppConstants.MenuSceneName);
                        break;
                    default:
                        throw new UnexpectedScene("SceneManager.GetActiveScene().name returned an unexpected scene name: " + sceneName);
                }
            } else if (Input.GetKey(AppConstants.NavigationStartGameKey)) {
                if (sceneName == AppConstants.MenuSceneName) {
                    StartGame();
                }
            } else if (Input.GetKey(AppConstants.NavigationStartPracticeKey)) {
                if (sceneName == AppConstants.MenuSceneName) {
                    SceneManager.LoadScene(AppConstants.PracticeSceneName);
                }
            } else if (Input.GetKey(AppConstants.NavigationHelpKey)) {
                if (sceneName == AppConstants.MenuSceneName) {
                    SceneManager.LoadScene(AppConstants.HelpRulesSceneName);
                }
            }
        }

        void OnMouseDown() {
            GetComponent<SpriteRenderer>().sprite = downSprite;
        }

        void OnMouseUp() {
            GetComponent<SpriteRenderer>().sprite = upSprite;

            var sceneName = SceneManager.GetActiveScene().name;
            if (sceneName == AppConstants.PracticeSceneName) {
                buttonPress = true;
                return;
            } else if (sceneName == AppConstants.GameSceneName) {
                StopGame();
            }

            switch (scene) {
                case Scene.MENU:
                    SceneManager.LoadScene(AppConstants.MenuSceneName);
                    break;
                case Scene.GAME:
                    StartGame();
                    break;
                case Scene.PRACTICE:
                    SceneManager.LoadScene(AppConstants.PracticeSceneName);
                    break;
                case Scene.HELP:
                    SceneManager.LoadScene(AppConstants.HelpRulesSceneName);
                    break;
                default:
                    break;
            }
        }

        void StartGame() {
            GameObject.Find(AppConstants.NetworkManagerName).GetComponent<NetworkManagerHUD>().showGUI = true;
            SceneManager.LoadScene(AppConstants.GameSceneName);
        }

        public void StopGame() {
            var networkManager = MenuManager.GetNetworkManager();
            if (networkManager != null) {
                networkManager.GetComponent<NetworkManagerHUD>().showGUI = false;
                var gameManagerObject = GameObject.Find(AppConstants.GameManagerName);
                if (gameManagerObject != null) {
                    var gameManager = gameManagerObject.GetComponent<GameManager>();
                    if (gameManager.isServer) {
                        gameManager.StopServer();
                    } else {
                        gameManager.StopClient();
                    }
                }
            }
        }

        public static void HideNetGUI() {
            var networkManager = MenuManager.GetNetworkManager();
            if (networkManager != null) {
                networkManager.GetComponent<NetworkManagerHUD>().showGUI = false;
            }
        }
    }
}