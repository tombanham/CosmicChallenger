using UnityEngine;
using Assets.scripts.Services;
using Assets.scripts.Services.Spawners;
using Assets.scripts.Sprites.Units.Weapon;
using Assets.scripts.Sprites.Units;

namespace Assets.scripts.Sprites.Environmental {
    public class PracticePickup : MonoBehaviour {

        public WeaponType WeaponType;

        public ParticleSystem explosion;

        void OnCollisionEnter2D(Collision2D collision) {
            var hit = collision.gameObject;
            var practicePlayer = hit.GetComponent<PracticePlayer>();

            if (practicePlayer != null) {
                SoundEffectsHelper.Instance.MakePickupSound();
                practicePlayer.SetWeapon(WeaponType);
            } else {
                SoundEffectsHelper.Instance.MakePickupExplosionSound();
                Instantiate(explosion, transform.position, transform.rotation);
            }

            PracticePickupSpawner.activePickups.Remove(gameObject);
            Destroy(gameObject);
        }
    }
}