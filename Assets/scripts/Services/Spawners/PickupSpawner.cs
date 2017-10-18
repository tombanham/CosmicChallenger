using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Networking;
using Assets.scripts.Sprites.Units.Weapon;

namespace Assets.scripts.Services.Spawners {
    public class PickupSpawner : NetworkBehaviour {
        private System.Timers.Timer pickupSpawnTimer;

        public GameObject rocketPickup;
        public GameObject bombPickup;
        public GameObject clusterBombPickup;
        public GameObject seekerPickup;

        public static List<GameObject> activePickups;

        bool spawnRandom = false;

        void Start () {
            activePickups = new List<GameObject>();
            InitalisePickupSpawnTimer();
        }

        private void InitalisePickupSpawnTimer() {
            pickupSpawnTimer = new System.Timers.Timer();
            pickupSpawnTimer.Elapsed += delegate { SetSpawnRandom(true); };
            pickupSpawnTimer.Interval = BalanceData.PickupSpawnerIntervalMilliseconds;
            pickupSpawnTimer.Enabled = true;
        }

        private void OnDestroy() {
            if (pickupSpawnTimer != null) {
                pickupSpawnTimer.Enabled = false;
            }
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
            int x = Random.Range(-55, 55);
            int y = Random.Range(-35, 25);
            var pos = new Vector2(x,y);

            var weaponTypeNumber = Random.Range(1, 5);
            var weaponType = (WeaponType)weaponTypeNumber;
            var pickupPrefab = GetPickup(weaponType);
            var pickupObject = Instantiate(pickupPrefab, pos, pickupPrefab.transform.rotation) as GameObject;
            NetworkServer.Spawn(pickupObject);

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
