using Assets.scripts.Exception;
using Assets.scripts.Sprites.Units.Weapon;

namespace Assets.scripts
{
    class BalanceData
    {
        //Weapons
        public const int LaserSpeed = 35;
        public const short RocketAmmoAmount = 1;
        public const short BombAmmoAmount = 1;
        public const short ClusterBombAmmoAmount = 2;
        public const short SeekerlaserAmmoAmount = 5;
        public const short DefaultAmmo = 0;
        public const short LaserDamage = 10;
        public const short ClusterBombDamage = 30;
        public const short ClusterLaserDamage = 10;
        public const short RocketDamage = 50;
        public const short SeekerDamage = 10;
        public const short SplashDamage = 30;
        public const short AreaOfEffect = 10;
        public const short BombDamage = 30;

        public static short GetDamage(WeaponType weaponType){
            switch (weaponType){
                case WeaponType.LASER:
                    return LaserDamage;
                case WeaponType.BOMB:
                    return BombDamage;
                case WeaponType.CLUSTERBOMB:
                    return ClusterBombDamage;
                case WeaponType.CLUSTERLASER:
                    return ClusterLaserDamage;
                case WeaponType.ROCKET:
                    return RocketDamage;
                case WeaponType.SEEKERLASER:
                    return SeekerDamage;
                default:
                    throw new UnexpectedWeaponType("Invalid Weapon Type: " + weaponType);
            }
        }

        //Pickups
        public const int PickupSpawnerIntervalMilliseconds = 1000;
        public const int MaxPickups = 1;

        //Enemy Drones
        public const int EnemyFireRateMilliseconds = 1000;
        public const short NumberOfEnemies = 3;

        //Health
        public const int MaxHealth = 100;
        public const int MaxLives = 3;

        //Layout
        public const short MaxAsteroids = 20;
        public const int SwitchLayoutIntervalMilliseconds = 10000;
    }
}