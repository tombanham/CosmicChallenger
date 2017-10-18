using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.scripts.Services {
    public class SoundEffectsHelper : MonoBehaviour {
        public static SoundEffectsHelper Instance;

        public AudioClip laserSound;
        public AudioClip bombSound;
        public AudioClip rocketSound;
        public AudioClip clusterBombSound;
        public AudioClip seekerLaserSound;

        public AudioClip bombExplosionSound;
        public AudioClip rocketExplosionSound;

        public AudioClip pickupSound;
        public AudioClip pickupExplosionSound;

        public AudioClip shipExplosionSound;

        public AudioClip layoutChangeSound;

        public AudioClip clickSound;

        private string currentScene = AppConstants.MenuSceneName;

        void Awake() {
            Instance = this;

            DontDestroyOnLoad(gameObject);
        }


        public void MakeLaserSound() {
            MakeSound(laserSound);
        }

        public void MakeBombSound() {
            MakeSound(bombSound);
        }

        public void MakeRocketSound() {
            MakeSound(rocketSound);
        }

        public void MakeClusterBombSound() {
            MakeSound(clusterBombSound);
        }

        public void MakeSeekerLaserSound() {
            MakeSound(seekerLaserSound);
        }

        public void MakeBombExplosionSound() {
            MakeSound(bombExplosionSound);
        }

        public void MakeRocketExplosionSound() {
            MakeSound(rocketExplosionSound);
        }

        public void MakePickupSound() {
            MakeSound(pickupSound);
        }

        public void MakePickupExplosionSound() {
            MakeSound(pickupExplosionSound);
        }

        public void MakeLayoutChangeSound() {
            MakeSound(layoutChangeSound);
        }

        public void MakeClickSound() {
            MakeSound(clickSound);
        }

        public void MakeShipExplosionSound() {
            MakeSound(shipExplosionSound);
        }

        private void MakeSound(AudioClip originalClip) {
            AudioSource.PlayClipAtPoint(originalClip, transform.position);
        }

        void Start () {
            DontDestroyOnLoad(gameObject);
        }

        void Update () {
            string sceneName = SceneManager.GetActiveScene().name;
            if (sceneName != currentScene) {
                MakeClickSound();
                currentScene = sceneName;
            }
        }
    }
}
