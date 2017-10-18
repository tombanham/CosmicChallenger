using UnityEngine;
using UnityEngine.Networking;
using Assets.scripts.Services;
using Assets.scripts.Services.Spawners;
using Assets.scripts.Sprites.Units.Weapon;

namespace Assets.scripts.Sprites.Environmental {
    public class Pickup : NetworkBehaviour {

        public WeaponType WeaponType;

        public ParticleSystem explosion;

        void OnCollisionEnter2D(Collision2D collision) {
            if (!isServer) {
                return;
            }

            var hit = collision.gameObject;

            var playerAmmo = hit.GetComponent<Ammo>();

            if (playerAmmo != null) {
                SoundEffectsHelper.Instance.MakePickupSound();
                playerAmmo.SetWeapon(WeaponType);
            } else {
                SoundEffectsHelper.Instance.MakePickupExplosionSound();
                Instantiate(explosion, transform.position, transform.rotation);
            }

            PickupSpawner.activePickups.Remove(gameObject);

            Destroy(gameObject);
        }
    }
}
