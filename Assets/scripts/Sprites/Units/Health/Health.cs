using UnityEngine;
using UnityEngine.Networking;
using Assets.scripts.Services;
using Assets.scripts.Sprites.Units;

namespace Assets.scripts.Sprites.Health {
    public class Health : NetworkBehaviour {
        [SyncVar(hook = "OnChangeHealth")]
        public int currentHealth = BalanceData.MaxHealth;

        public int currentLives = BalanceData.MaxLives;

        public RectTransform healthBar;
        private NetworkStartPosition[] spawnPoints;

        public ParticleSystem sparks;
        public ParticleSystem explosion;

        public NetworkStartPosition deathPoint;

        void Start() {
            if (isLocalPlayer) {
                spawnPoints = FindObjectsOfType<NetworkStartPosition>();
            }
        }

        public void TakeDamage(int amount) {
            Instantiate(sparks, transform.position, transform.rotation);

            if (!isServer) {
                return;
            }

            bool death = currentHealth - amount <= 0;
            if (death) {
                RpcinstantiateSparks();
                SoundEffectsHelper.Instance.MakeShipExplosionSound();
            }

            currentHealth -= amount;
            if (currentHealth <= 0) {
                currentHealth = BalanceData.MaxHealth;
                currentLives -= 1;
                if (currentLives > 0) {
                    RpcRespawn();
                } else {
                    if (isServer) {
                        if (GetComponent<Player>() == GameManager.localPlayer) {
                            GameManager.localPlayerAlive = false;
                            RpcSetDeath(false);
                        } else if (GetComponent<Player>() == GameManager.opponentPlayer) {
                            GameManager.opponentPlayerAlive = false;
                            RpcSetDeath(true);
                        }
                    }
                    RpcHidePlayer();
                }
            }
            OnChangeHealth(currentHealth);
        }

        [ClientRpc]
        void RpcSetDeath(bool localDeath) {
            if (!isServer) {
                if (localDeath) {
                    GameManager.localPlayerAlive = false;
                } else {
                    GameManager.opponentPlayerAlive = false;
                }
            }
        }

        [ClientRpc]
        void RpcinstantiateSparks() {
            var additionalSparkOnePos = new Vector2(transform.position.x - 2, transform.position.y);
            var additionalSparkTwoPos = new Vector2(transform.position.x - 2.5f, transform.position.y + 2);
            var additionalSparkThreePos = new Vector2(transform.position.x + 3, transform.position.y - 3);

            Instantiate(explosion, transform.position, transform.rotation);
            Instantiate(sparks, additionalSparkOnePos, transform.rotation);
            Instantiate(sparks, additionalSparkTwoPos, transform.rotation);
            Instantiate(sparks, additionalSparkThreePos, transform.rotation);
        }

        void OnChangeHealth(int currentHealth) {
            healthBar.sizeDelta = new Vector2(currentHealth, healthBar.sizeDelta.y);
        }

        [ClientRpc]
        void RpcRespawn() {
            if (isLocalPlayer) {
                var spawnPoint = Vector3.zero;

                if (spawnPoints != null && spawnPoints.Length > 0) {
                    spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)].transform.position;
                }

                transform.position = spawnPoint;
            }
        }

        [ClientRpc]
        void RpcHidePlayer() {
            if (isLocalPlayer) {
                transform.position = deathPoint.transform.position;
            }
        }
    }
}