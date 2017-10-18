using UnityEngine;
using System.Timers;
using Assets.scripts.Services.Spawners;
using Assets.scripts.Sprites.Units.Weapon;

namespace Assets.scripts.Sprites.Units {
    public class PracticeEnemy : MonoBehaviour {
        public int shipID;
        public GameObject laserPrefab;
        bool toFire = false;

        void OnDestroy() {
            PracticeEnemySpawner.enemies.Remove(gameObject);
        }

        void Start() {
            shipID = PracticeManager.NextID();

            Timer PeriodicFireTimer = new Timer();
            PeriodicFireTimer.Elapsed += delegate { PeriodicFire(); };
            PeriodicFireTimer.Interval = BalanceData.EnemyFireRateMilliseconds;
            PeriodicFireTimer.Enabled = true;
        }

        void Update() {
            GetComponent<Rigidbody2D>().isKinematic = false;

            if (toFire) {
                CmdFire();
                toFire = false;
            }
        }

        public void PeriodicFire() {
            toFire = true;
        }

        void CmdFire() {
            var pos = transform.position;

            laserPrefab.GetComponent<OfflineWeapon>().sourceShipId = shipID;

            var bullet = (GameObject)Instantiate(
                laserPrefab,
                pos,
                laserPrefab.transform.rotation);

            var newVelocity = new Vector2(BalanceData.LaserSpeed, 0);
            bullet.GetComponent<Rigidbody2D>().velocity = newVelocity;
            Destroy(bullet, 2.0f);
        }

        void OnCollisionEnter2D(Collision2D coll) {
            GetComponent<Rigidbody2D>().isKinematic = true;
            GetComponent<Rigidbody2D>().velocity = new Vector2(0,1.4f);
            GetComponent<Rigidbody2D>().angularVelocity = 0;
        }
    }
}