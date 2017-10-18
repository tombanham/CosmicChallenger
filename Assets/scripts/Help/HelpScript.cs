using UnityEngine;
using UnityEngine.SceneManagement;
using Assets.scripts.Exception;

namespace Assets.scripts.Help {
    public class HelpScript : MonoBehaviour {
        public HelpScene scene;

        void Update () {
            if (Input.GetKeyDown(AppConstants.NavigationForwardKey)) {
                NavigateForwards();
            } else if (Input.GetKey(AppConstants.NavigationBackKey)) {
                NavigateBackwards();
            }
        }

        private void NavigateForwards() {
            switch (scene) {
                case HelpScene.RULES:
                    SceneManager.LoadScene(AppConstants.HelpWeaponsSceneName);
                    break;
                case HelpScene.WEAPONS:
                    SceneManager.LoadScene(AppConstants.HelpPracticeSceneName);
                    break;
                case HelpScene.PRACTICE:
                    SceneManager.LoadScene(AppConstants.MenuSceneName);
                    break;
                default:
                    throw new UnexpectedScene("Unknown Scene: " + SceneManager.GetActiveScene().name);
            }
        }

        private void NavigateBackwards() {
            switch (scene) {
                case HelpScene.RULES:
                    SceneManager.LoadScene(AppConstants.MenuSceneName);
                    break;
                case HelpScene.WEAPONS:
                    SceneManager.LoadScene(AppConstants.HelpRulesSceneName);
                    break;
                case HelpScene.PRACTICE:
                    SceneManager.LoadScene(AppConstants.HelpWeaponsSceneName);
                    break;
                default:
                    throw new UnexpectedScene("Unknown Scene: " + SceneManager.GetActiveScene().name);
            }
        }
    }
}
