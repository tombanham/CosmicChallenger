using System;
using UnityEngine.Networking;

namespace Assets.scripts.Sprites.Units.Weapon {
    public class Ammo : NetworkBehaviour {
        [SyncVar(hook = "OnChangeAmmo")]
        public int currentAmmo = BalanceData.DefaultAmmo;

        [SyncVar(hook = "OnChangeWeapon")]
        public WeaponType WeaponType = WeaponType.LASER;

        void OnChangeAmmo(int ammo) {
            if (isLocalPlayer) {
                GameManager.UpdateAmmo(ammo);
            }
        }

        void OnChangeWeapon(WeaponType weaponType) {
            if (isLocalPlayer) {
                GameManager.UpdateWeapon(weaponType);
            }
        }

        public void UseAmmo() {
           GetComponent<Ammo>().currentAmmo = GetComponent<Ammo>().currentAmmo - 1;
        }

        public void SetWeapon(WeaponType weaponType) {
            WeaponType = weaponType;
            switch (weaponType) {
                case WeaponType.ROCKET:
                    GetComponent<Ammo>().currentAmmo = BalanceData.RocketAmmoAmount;
                    break;
                case WeaponType.BOMB:
                    GetComponent<Ammo>().currentAmmo = BalanceData.BombAmmoAmount;
                    break;
                case WeaponType.CLUSTERBOMB:
                    GetComponent<Ammo>().currentAmmo = BalanceData.ClusterBombAmmoAmount;
                    break;
                case WeaponType.SEEKERLASER:
                    GetComponent<Ammo>().currentAmmo = BalanceData.SeekerlaserAmmoAmount;
                    break;
                default:
                    break;
            }
        }
    }
}