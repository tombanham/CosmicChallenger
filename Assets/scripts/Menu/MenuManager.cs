using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using Assets.scripts.Services;

namespace Assets.scripts.Menu
{
    public class MenuManager : MonoBehaviour
    {
        public NetworkManager networkManager;
        public SoundEffectsHelper soundEffectsHelper;

        void Start(){
            var currentManager = GetNetworkManager();

            if (currentManager == null){
                networkManager = Instantiate(networkManager);
            }
            else{
                networkManager = currentManager.GetComponent<NetworkManager>();
            }

            networkManager.GetComponent<NetworkManagerHUD>().showGUI = false;

            if (SoundEffectsHelper.Instance == null) {
                Instantiate(soundEffectsHelper);
            }
        }

        public static GameObject GetNetworkManager(){
            return GameObject.Find(AppConstants.NetworkManagerName);
        }
    }
}