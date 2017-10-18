using UnityEngine;
using UnityEngine.Networking;
using System;
using Assets.scripts.Services;
using Assets.scripts.Exception;

namespace Assets.scripts.Sprites.Units.Weapon {
    public class Weapon : NetworkBehaviour {

        [SyncVar]
        public int sourceShip;

        public ParticleSystem explosion;

        public bool tracking;

        public bool shotByServerPlayer = false;
        public bool cluster;
        public GameObject cluserLaser;

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
                    throw new UnexpectedWeaponType("Weapon.WeaponType is an unexpected weapon type: " + WeaponType);
            }
        }

        void OnDestroy() {
            switch (WeaponType) {
                case WeaponType.ROCKET:
                    SoundEffectsHelper.Instance.MakeRocketExplosionSound();
                    break;
                case WeaponType.BOMB:
                    SoundEffectsHelper.Instance.MakeBombExplosionSound();
                    break;
                default:
                    break;
            }

            Instantiate(explosion, transform.position, transform.rotation);
            if (WeaponType == WeaponType.BOMB) {
                SplashDamage();
            }
        }

        void OnCollisionEnter2D(Collision2D collision) {
            var hit = collision.gameObject;

            var playerShip = hit.GetComponent<Player>();

            if ((playerShip == null || playerShip.shipID != sourceShip)){
                TriggerWeapon(collision);
            }
        }

        private void TriggerWeapon(Collision2D collision) {
            if (cluster) {
                RpcspawnLasers(collision);
            }

            Destroy(gameObject);

            var health = collision.gameObject.GetComponent<Health.Health>();
            if (health != null) {
                health.TakeDamage(BalanceData.GetDamage(WeaponType));
            }
        }

        void SplashDamage() {
            if (BalanceData.AreaOfEffect >= Vector3.Distance(transform.position, GameManager.localPlayer.transform.position)) {
                var health = GameManager.localPlayer.GetComponent<Health.Health>();
                if (health != null) {
                    health.TakeDamage(BalanceData.GetDamage(WeaponType));
                }
            }
            if (GameManager.opponentPlayer != null) {
                if (BalanceData.AreaOfEffect >= Vector3.Distance(transform.position, GameManager.opponentPlayer.transform.position)) {
                    var health = GameManager.opponentPlayer.GetComponent<Health.Health>();
                    if (health != null) {
                        health.TakeDamage(BalanceData.GetDamage(WeaponType));
                    }
                }
            }
        }

        void RpcspawnLasers(Collision2D collision) {
            var thisVelocity = collision.relativeVelocity;

            var backwardLaserPos = transform.position;
            var upLaserPos = transform.position;
            var downLaserPos = transform.position;

            Quaternion upLaserRot;
            Quaternion downLaserRot;

            if (thisVelocity.x > 0) {
                upLaserRot  = new Quaternion(0, 0, -45, 0);
                downLaserRot  = new Quaternion(0, 0, 45, 0);

                backwardLaserPos.x -= 5;
                upLaserPos.x -= 5;
                downLaserPos.x -= 5;
            } else {
                upLaserRot = new Quaternion(0, 0, 45, 0);
                downLaserRot  = new Quaternion(0, 0, -45, 0);

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

        void Update() {
            if (tracking && GameManager.opponentPlayer != null) {
                Vector3 opponentLocation;

                if (shotByServerPlayer && isServer || !shotByServerPlayer && !isServer) {
                    opponentLocation = GameManager.opponentPlayer.transform.position;
                } else {
                    opponentLocation = GameManager.localPlayer.transform.position;
                }

                var newVelocity = GetComponent<Rigidbody2D>().velocity;

                var dx = Mathf.Abs(opponentLocation.x - transform.position.x);
                var dy = Mathf.Abs(opponentLocation.y - transform.position.y);

                GetComponent<Rigidbody2D>().rotation = Mathf.Rad2Deg*(Mathf.Atan(dx / dy)) + 90 ;

                if (opponentLocation.x >= transform.position.x) {
                    newVelocity.x  = BalanceData.LaserSpeed;
                } else {
                    newVelocity.x = -BalanceData.LaserSpeed;
                }
                if (opponentLocation.y >= transform.position.y) {
                    newVelocity.y = BalanceData.LaserSpeed;
                } else  {
                    newVelocity.y = -BalanceData.LaserSpeed;
                }
                GetComponent<Rigidbody2D>().velocity = newVelocity;
            }
        }
    }
}