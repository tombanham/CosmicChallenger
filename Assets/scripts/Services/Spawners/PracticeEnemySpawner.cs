using UnityEngine;
using System.Collections.Generic;

namespace Assets.scripts.Services.Spawners {
    public class PracticeEnemySpawner : MonoBehaviour {

        public GameObject enemyPrefab;
        public static List<GameObject> enemies = new List<GameObject>();

        void Start() {
            for (var i = 0; i < BalanceData.NumberOfEnemies; i++) {
                var xpos = Random.Range(-50.0f, 50.0f);
                var ypos = Random.Range(-30.0f, 20.0f);
                RpcSpawnEnemy(xpos, ypos);
            }
        }

        void RpcSpawnEnemy(float xpos, float ypos) {
            var spawnPosition = new Vector3(
                    xpos, ypos,
                    0.0f);

            var enemy = (GameObject)Instantiate(enemyPrefab, spawnPosition, enemyPrefab.transform.rotation);
            enemies.Add(enemy);
        }
    }
}