using UnityEngine;
using Assets.scripts.Sprites.Units.Weapon;

namespace Assets.scripts.Sprites.Units {
    public class PracticePlayer : MonoBehaviour {

        public ParticleSystem trail;

        public GameObject laserPrefab;
        public GameObject rocketPrefab;
        public GameObject bombPrefab;
        public GameObject clusterBombPrefab;
        public GameObject seekerPrefab;

        public GameObject rocketPrefabLeft;
        public GameObject bombPrefabLeft;
        public GameObject clusterBombPrefabLeft;

        private bool facingRight = true;

        private Quaternion SpriteRot;

        public int shipID;

        public int currentAmmo;
        public WeaponType WeaponType;

        void Start() {
            shipID = PracticeManager.NextID();
            SpriteRot = transform.rotation;
        }

        void Update() {
            SyncRot();

            GetComponent<Rigidbody2D>().isKinematic = false;

            var x = Input.GetAxis(AppConstants.HorizontalAxisName) * Time.deltaTime * 25.0f;
            var y = Input.GetAxis(AppConstants.VerticalAxisName) * Time.deltaTime * 25.0f;

            if (x != 0 || y != 0) {
                trail.GetComponent<ParticleSystem>().maxParticles = AppConstants.ShipTrailMaxParticles;
            } else {
                trail.GetComponent<ParticleSystem>().maxParticles = 0;
            }

            if (x > 0) {
                Cmd_FlipInVerticalAxis(flipRight: true);
            } else if (x < 0) {
                Cmd_FlipInVerticalAxis(flipRight: false);
                x = -x;
            }

            if (Input.GetKeyDown(KeyCode.Space)) {
                CmdFire();
            }

            transform.Translate(x, 0, 0);
            transform.Translate(0, y, 0);
        }

        void Cmd_FlipInVerticalAxis(bool flipRight) {
            if (flipRight != facingRight) {
                if (!facingRight) {
                    transform.Rotate(new Vector3(0, 180, 0));
                    facingRight = true;
                } else {
                    transform.Rotate(new Vector3(0, -180, 0));
                    facingRight = false;
                }
                SpriteRot = transform.localRotation;
            }
        }

        void SyncRot() {
            if (transform.localRotation != SpriteRot) {
                transform.localRotation = SpriteRot;
            }
        }

        void CmdFire() {
            var pos = transform.position;

            if (facingRight) {
                pos.x += 5;
            } else {
                pos.x -= 5;
            }

            var bullet = SetBulletPrefab();

            bullet = (GameObject)Instantiate(
                bullet,
                pos,
                bullet.transform.rotation);

            bullet.GetComponent<OfflineWeapon>().sourceShipId = shipID;

            Vector2 newVelocity;

            if (facingRight) {
                newVelocity = new Vector2(BalanceData.LaserSpeed, 0);
            } else {
                newVelocity = new Vector2(-BalanceData.LaserSpeed, 0);
            }

            bullet.GetComponent<Rigidbody2D>().velocity = newVelocity;

            Destroy(bullet, 2.0f);

            UpdateWeaponData();
        }

        void UpdateWeaponData() {
            if (WeaponType != WeaponType.LASER) {
                if (currentAmmo == 1) {
                    OnChangeWeapon(WeaponType.LASER);
                }
                currentAmmo--;
            }
            PracticeManager.UpdateAmmo(currentAmmo);
        }

        public void OnChangeWeapon(WeaponType newWeaponType) {
            WeaponType = newWeaponType;
            PracticeManager.UpdateWeapon(newWeaponType);
        }

        GameObject SetBulletPrefab() {
            switch (WeaponType) {
                case WeaponType.LASER:
                    return laserPrefab;
                case WeaponType.ROCKET:
                    if (facingRight) {
                        return rocketPrefab;
                    }
                    return rocketPrefabLeft;
                case WeaponType.BOMB:
                    if (facingRight) {
                        return bombPrefab;
                    }
                    return bombPrefabLeft;
                case WeaponType.CLUSTERBOMB:
                    if (facingRight) {
                        return clusterBombPrefab;
                    }
                    return clusterBombPrefabLeft;
                case WeaponType.SEEKERLASER:
                    return seekerPrefab;
                default:
                    return laserPrefab;
            }
        }

        void OnCollisionEnter2D(Collision2D coll) {
            GetComponent<Rigidbody2D>().isKinematic = true;
            GetComponent<Rigidbody2D>().velocity = Vector3.zero;
            GetComponent<Rigidbody2D>().angularVelocity = 0;
        }

        public void SetWeapon(WeaponType weaponType) {
            switch (weaponType) {
                case WeaponType.ROCKET:
                    currentAmmo = BalanceData.RocketAmmoAmount;
                    break;
                case WeaponType.BOMB:
                    currentAmmo = BalanceData.BombAmmoAmount;
                    break;
                case WeaponType.CLUSTERBOMB:
                    currentAmmo = BalanceData.ClusterBombAmmoAmount;
                    break;
                case WeaponType.SEEKERLASER:
                    currentAmmo = BalanceData.SeekerlaserAmmoAmount;
                    break;
                default:
                    break;
            }
            OnChangeWeapon(weaponType);
            PracticeManager.UpdateAmmo(currentAmmo);
        }
    }
}