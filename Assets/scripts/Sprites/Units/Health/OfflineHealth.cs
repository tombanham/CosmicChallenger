using UnityEngine;
using System;
using Assets.scripts.Services;
using Assets.scripts.Services.Spawners;
using Assets.scripts.Sprites.Units;
using Assets.scripts.Exception;

namespace Assets.scripts.Sprites.Health {
    public class OfflineHealth : MonoBehaviour {
        public int CurrentHealth = BalanceData.MaxHealth;
        public int CurrentLives = BalanceData.MaxLives;

        public RectTransform healthBar;

        public ParticleSystem sparks;
        public ParticleSystem explosion;

        public void TakeDamage(int amount) {
            Instantiate(sparks, transform.position, transform.rotation);

            var death = CurrentHealth - amount <= 0;
            if (death) {
                RpcinstantiateSparks();
                SoundEffectsHelper.Instance.MakeShipExplosionSound();
            }

            CurrentHealth -= amount;
            if (CurrentHealth <= 0) {
                CurrentHealth = BalanceData.MaxHealth;
                OnChangeLives(CurrentLives - 1);
                if (CurrentLives > 0) {
                    RpcRespawn();
                } else {
                    Destroy(gameObject);
                }
            }

            OnChangeHealth(CurrentHealth);
        }

        void OnChangeLives(int newLives) {
            CurrentLives = newLives;
            if (GetComponent<PracticePlayer>() != null) {
                PracticeManager.SetPlayerLives(newLives);
            }
        }

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

        void RpcRespawn() {
            var SpawnPoint = (SpawnPoint)UnityEngine.Random.Range(0,2);
            var spawnPoint = Vector3.zero;

            switch (SpawnPoint) {
                case SpawnPoint.LEFT:
                    spawnPoint = new Vector3(-50, -5, 0);
                    break;
                case SpawnPoint.RIGHT:
                    spawnPoint = new Vector3(50, -5, 0);
                    break;
                default:
                    throw new UnexpectedSpawnPoint("An unexpected spawn point type was encountered in OfflineHealth.RpcRespawn");
            }

            transform.position = spawnPoint;
        }
    }
}