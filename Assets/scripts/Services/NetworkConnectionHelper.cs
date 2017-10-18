using UnityEngine;
using UnityEngine.SceneManagement;
using Assets.scripts.Menu;
using UnityEngine.Networking;

namespace Assets.scripts.Services {
    public class NetworkConnectionHelper : MonoBehaviour {

        public bool connected = false;
        public GUIText topUItext;

        GameObject gameManagerObject;

        private void Start() {
            GetGameManager();
        }

        private void GetGameManager() {
            gameManagerObject = GameObject.Find(AppConstants.GameManagerName);
        }

        void Update () {
            if (gameManagerObject == null) {
                GetGameManager();
            }

            if (gameManagerObject != null) {
                if (!connected) {
                    //if hasn't been connect yet: wait until opponent is connected, then set connected to true
                    if (GameManager.opponentPlayer != null) {
                        connected = true;
                    }
                } else {
                    if (GameManager.opponentPlayer == null) {
                        if (GameManager.gameStatus == GameStatus.GAMEON) {
                            GameManager.gameStatus = GameStatus.OPPONENTDISC;
                        }
                    }
                }
            } else {
                /*If we haven't connected yet and the game manager doesnt exist, we take control of the top UI text.
                 * If we were previously connected but the gameManagerObject is now null, the opponent must have disconnected
                 */
                if (connected) {
                    if (GameManager.gameStatus == GameStatus.GAMEON) {
                        OpponentDisconnect();
                    }
                } else {
                    topUItext.fontSize = UIText.TitleFontSize;
                    topUItext.text = UIText.Title;
                }
            }
        }

        private void OpponentDisconnect() {
            MenuButton.HideNetGUI();
            topUItext.fontSize = UIText.StandardFontSize;
            topUItext.text = UIText.Disconnect;
            if (Input.GetKeyDown(KeyCode.Return)) {
                SceneManager.LoadScene(AppConstants.MenuSceneName);
            }
        }

        public void StopServer() {
            var networkManagerObject = GameObject.Find(AppConstants.NetworkManagerName);
            if (networkManagerObject != null) {
                NetworkManager networkManager = networkManagerObject.GetComponent<NetworkManager>();
                networkManager.StopHost();
                networkManager.StopMatchMaker();
            }
        }

        public void StopClient() {
            var networkManagerObject = GameObject.Find(AppConstants.NetworkManagerName);
            if (networkManagerObject != null) {
                NetworkManager networkManager = networkManagerObject.GetComponent<NetworkManager>();
                networkManager.StopClient();
                networkManager.StopMatchMaker();
            }
        }
    }
}