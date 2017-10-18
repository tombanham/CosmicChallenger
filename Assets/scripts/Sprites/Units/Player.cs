using UnityEngine;
using UnityEngine.Networking;
using Assets.scripts.Sprites.Units.Weapon;

namespace Assets.scripts.Sprites.Units {
    public class Player : NetworkBehaviour {
        public Sprite shipDefault;
        public Sprite shipPlayer;

        public ParticleSystem trail;

        public GameObject laserPrefab;
        public GameObject rocketPrefab;
        public GameObject bombPrefab;
        public GameObject clusterBombPrefab;
        public GameObject seekerPrefab;

        public GameObject rocketPrefabLeft;
        public GameObject bombPrefabLeft;
        public GameObject clusterBombPrefabLeft;

        [SyncVar]
        private bool facingRight = true;

        [SyncVar]
        private Quaternion SpriteRot;

        [SyncVar]
        public int shipID;

        [SyncVar]
        public bool serverPlayer;

        void Start () {
            shipID = GameManager.NextID();

            SpriteRot = transform.rotation;

            if (!isLocalPlayer) {
                GameManager.opponentPlayer = this;
            }
        }

        public override void OnStartLocalPlayer() {
            GetComponent<SpriteRenderer>().sprite = shipPlayer;
            GameManager.localPlayer = this;
            serverPlayer = isServer;
        }

        void Update () {
            SyncRot();

            seekerPrefab.GetComponent<Weapon.Weapon>().shotByServerPlayer = serverPlayer;

            //for testing, add '|| GameManager.gameStatus == GameStatus.GAMEREADY' inside inner parethesis
            if (isLocalPlayer && (GameManager.gameStatus == GameStatus.GAMEON)) {
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

                if (GameManager.localPlayerAlive && Input.GetKeyDown(KeyCode.Space)) {
                    if (!isServer) {
                        UpdateWeaponData();
                    }
                    CmdFire();
                }

                transform.Translate(x, 0, 0);
                transform.Translate(0, y, 0);
            }
        }

        [Command]
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
            if (!isServer && transform.localRotation != SpriteRot) {
                transform.localRotation = SpriteRot;
            }
        }

        [Command]
        void CmdFire() {
            var pos = transform.position;

            if (facingRight) {
                pos.x += 5;
            } else {
                pos.x -= 5;
            }

            RpcGetServerPlayer();

            var bullet = SetBulletPrefab();

            bullet.GetComponent<Weapon.Weapon>().sourceShip = shipID;

            bullet = (GameObject)Instantiate(
                bullet,
                pos,
                bullet.transform.rotation);

            Vector2 newVelocity;

            if (facingRight) {
                newVelocity = new Vector2(BalanceData.LaserSpeed, 0);
            } else {
                newVelocity = new Vector2(-BalanceData.LaserSpeed, 0);
            }

            bullet.GetComponent<Rigidbody2D>().velocity = newVelocity;

            NetworkServer.Spawn(bullet);

            Destroy(bullet, 2.0f);

            UpdateWeaponData();
        }

        [ClientRpc]
        void RpcGetServerPlayer() {
            seekerPrefab.GetComponent<Weapon.Weapon>().shotByServerPlayer = serverPlayer;
        }

        void UpdateWeaponData() {
            if (GetComponent<Ammo>().WeaponType != WeaponType.LASER) {
                int ammo = GetComponent<Ammo>().currentAmmo;
                if (ammo == 1) {
                    GetComponent<Ammo>().SetWeapon(WeaponType.LASER);
                }
                GetComponent<Ammo>().UseAmmo();
            }
        }

        GameObject SetBulletPrefab() {
            var weaponType = GetComponent<Ammo>().WeaponType;
            switch (weaponType) {
                case WeaponType.LASER:
                    return laserPrefab;
                case WeaponType.ROCKET:
                    if(facingRight){
                        return rocketPrefab;
                    }
                    return rocketPrefabLeft;
                case WeaponType.BOMB:
                    if(facingRight){
                        return bombPrefab;
                    }
                    return bombPrefabLeft;
                case WeaponType.CLUSTERBOMB:
                    if(facingRight){
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
    }
}