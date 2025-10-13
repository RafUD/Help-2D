using UnityEngine;

public class Shooting : MonoBehaviour
{
    [SerializeField] private Transform shootingPoint;   // where bullets spawn
    [SerializeField] private GameObject bulletPrefab;   // projectile prefab
    [SerializeField] private float fireRate = 0.2f;     // time between shots

    private float nextFireTime = 0f;
    private PlayerManager playerManager; // reference to check power-up

    private void Start()
    {
        playerManager = FindAnyObjectByType<PlayerManager>();
    }

    private void Update()
    {
        if (Time.time >= nextFireTime)
        {
            Shoot();
            nextFireTime = Time.time + fireRate;
        }
    }

    private void Shoot()
    {
        GameObject bullet = Instantiate(bulletPrefab, shootingPoint.position, shootingPoint.rotation);

        // If the player is powered up, make bullet stronger
        PlayerProjectile projectile = bullet.GetComponent<PlayerProjectile>();
        if (projectile != null)
        {
            projectile.PotionPower(playerManager.isPoweredUp);
        }
    }
}
