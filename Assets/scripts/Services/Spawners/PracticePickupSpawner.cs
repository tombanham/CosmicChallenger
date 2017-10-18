using UnityEngine;
using System.Collections.Generic;
using System.Timers;
using Assets.scripts.Sprites.Units.Weapon;

namespace Assets.scripts.Services.Spawners {
    public class PracticePickupSpawner : MonoBehaviour {

        Timer pickupSpawnTimer;

        public GameObject rocketPickup;
        public GameObject bombPickup;
        public GameObject clusterBombPickup;
        public GameObject seekerPickup;

        public static List<GameObject> activePickups = new List<GameObject>();

        bool spawnRandom = false;

        void Start () {
            pickupSpawnTimer = new Timer();
            pickupSpawnTimer.Elapsed += delegate { SetSpawnRandom(true); };
            pickupSpawnTimer.Interval = BalanceData.PickupSpawnerIntervalMilliseconds;
            SetSpawner(true);
        }

        public void SetSpawner(bool enabled) {
            pickupSpawnTimer.Enabled = enabled;
        }

        public GameObject GetPickup(WeaponType weaponType) {
            switch (weaponType) {
                case WeaponType.ROCKET:
                    return rocketPickup;
                case WeaponType.BOMB:
                    return bombPickup;
                case WeaponType.CLUSTERBOMB:
                    return clusterBombPickup;
                case WeaponType.SEEKERLASER:
                    return seekerPickup;
                default:
                    return rocketPickup;
            }
        }

        void SetSpawnRandom(bool assignment) {
            spawnRandom = assignment;
        }

        void SpawnRandomPickup() {
            var x = Random.Range(-55, 55);
            var y = Random.Range(-35, 25);
            var pos = new Vector2(x,y);

            var weaponTypeNumber = Random.Range(1, 5);
            var pickupPrefab = GetPickup((WeaponType)weaponTypeNumber);
            var pickupObject = Instantiate(pickupPrefab, pos, pickupPrefab.transform.rotation) as GameObject;

            activePickups.Add(pickupObject);
        }

        void Update () {
            if (spawnRandom) {
                SetSpawnRandom(false);
                if (activePickups.Count < BalanceData.MaxPickups) {
                    SpawnRandomPickup();
                }
            }
        }
    }
}