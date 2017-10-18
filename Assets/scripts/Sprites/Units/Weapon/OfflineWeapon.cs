using UnityEngine;
using Assets.scripts.Menu;
using Assets.scripts.Services;
using Assets.scripts.Sprites.Health;
using Assets.scripts.Services.Spawners;

namespace Assets.scripts.Sprites.Units.Weapon {
    public class OfflineWeapon : MonoBehaviour {

        public int sourceShipId;

        public ParticleSystem explosion;

        public bool tracking;
        public GameObject cluserLaser;

        MenuButton button;

        public WeaponType WeaponType;

        void Awake() {
            switch (WeaponType) {
                case WeaponType.LASER:
                    SoundEffectsHelper.Instance.MakeLaserSound();
                    break;
                case WeaponType.ROCKET:
                    SoundEffectsHelper.Instance.MakeRocketSound();
                    break;
                case WeaponType.BOMB:
                    SoundEffectsHelper.Instance.MakeBombSound();
                    break;
                case WeaponType.CLUSTERBOMB:
                    SoundEffectsHelper.Instance.MakeClusterBombSound();
                    break;
                case WeaponType.SEEKERLASER:
                    SoundEffectsHelper.Instance.MakeSeekerLaserSound();
                    break;
                case WeaponType.CLUSTERLASER:
                    SoundEffectsHelper.Instance.MakeLaserSound();
                    break;
                default:
                    break;
            }
        }

        void OnDestroy() {
            if (button != null && button.buttonPress) {
                return;
            }

            switch (WeaponType) {
                case WeaponType.ROCKET:
                    SoundEffectsHelper.Instance.MakeRocketExplosionSound();
                    break;
                case WeaponType.BOMB:
                    SoundEffectsHelper.Instance.MakeBombExplosionSound();
                    SplashDamage();
                    break;
                default:
                    break;
            }

            Instantiate(explosion, transform.position, transform.rotation);
        }

        void Start() {
            button = GameObject.Find(AppConstants.MenuButtonName).GetComponent<MenuButton>();
        }

        void OnCollisionEnter2D(Collision2D collision) {
            var hit = collision.gameObject;

            var playerShip = hit.GetComponent<PracticePlayer>();
            var enemyShip = hit.GetComponent<PracticeEnemy>();

            if ((playerShip == null || playerShip.shipID != sourceShipId) && (enemyShip == null || enemyShip.shipID != sourceShipId)) {
                TriggerWeapon(collision);
            }
        }

        private void TriggerWeapon(Collision2D collision) {
            if (WeaponType == WeaponType.CLUSTERBOMB) {
                RpcSpawnLasers(collision);
            }

            Destroy(gameObject);

            var healthOfObject = collision.gameObject.GetComponent<OfflineHealth>();
            if (healthOfObject != null) {
                healthOfObject.TakeDamage(BalanceData.GetDamage(WeaponType));
            }
        }

        void SplashDamage() {
            foreach (var enemy in PracticeEnemySpawner.enemies) {
                if (enemy != null && BalanceData.AreaOfEffect >= Vector3.Distance(transform.position, enemy.transform.position)) {
                    var health = enemy.GetComponent<OfflineHealth>();
                    if (health != null) {
                        health.TakeDamage(BalanceData.GetDamage(WeaponType));
                    }
                }
            }

            var practicePlayerObject = GameObject.Find(AppConstants.PracticePlayerName);
            if (practicePlayerObject == null) {
                return;
            }
            var practicePlayer = practicePlayerObject.GetComponent<PracticePlayer>();

            if (BalanceData.AreaOfEffect >= Vector3.Distance(transform.position, practicePlayer.transform.position)) {
                var health = practicePlayer.GetComponent<OfflineHealth>();
                if (health != null) {
                    health.TakeDamage(BalanceData.GetDamage(WeaponType));
                }
            }
        }

        void RpcSpawnLasers(Collision2D collision) {
            var thisVelocity = collision.relativeVelocity;

            var backwardLaserPos = transform.position;
            var upLaserPos = transform.position;
            var downLaserPos = transform.position;

            Quaternion upLaserRot;
            Quaternion downLaserRot;

            if (thisVelocity.x > 0) {
                upLaserRot = new Quaternion(0, 0, -45, 0);
                downLaserRot = new Quaternion(0, 0, 45, 0);

                backwardLaserPos.x -= 5;
                upLaserPos.x -= 5;
                downLaserPos.x -= 5;

            } else {
                upLaserRot = new Quaternion(0, 0, 45, 0);
                downLaserRot = new Quaternion(0, 0, -45, 0);

                backwardLaserPos.x += 5;
                upLaserPos.x += 5;
                downLaserPos.x += 5;
            }

            upLaserPos.y += 5;
            downLaserPos.y -= 5;

            var backwardLaser = (GameObject)Instantiate(
                cluserLaser,
                backwardLaserPos,
                cluserLaser.transform.rotation);
            var upLaser = (GameObject)Instantiate(
                cluserLaser,
                upLaserPos,
                upLaserRot);
            var downLaser = (GameObject)Instantiate(
                cluserLaser,
                downLaserPos,
                downLaserRot);

            if (thisVelocity.x > 0) {
                upLaser.transform.Rotate(new Vector3(0, 0, -45));
                downLaser.transform.Rotate(new Vector3(0, 0, 45));

                upLaser.GetComponent<Rigidbody2D>().velocity = new Vector2(-thisVelocity.x, thisVelocity.x);
                downLaser.GetComponent<Rigidbody2D>().velocity = new Vector2(-thisVelocity.x, -thisVelocity.x);
            } else {
                upLaser.transform.Rotate(new Vector3(0, 0, 45));
                downLaser.transform.Rotate(new Vector3(0, 0, -45));

                upLaser.GetComponent<Rigidbody2D>().velocity = new Vector2(-thisVelocity.x, -thisVelocity.x);
                downLaser.GetComponent<Rigidbody2D>().velocity = new Vector2(-thisVelocity.x, thisVelocity.x);
            }

            backwardLaser.GetComponent<Rigidbody2D>().velocity = new Vector2(-thisVelocity.x, 0);

            Destroy(backwardLaser, 2.0f);
            Destroy(upLaser, 2.0f);
            Destroy(downLaser, 2.0f);
        }
    }
}
