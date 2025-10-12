using UnityEngine;

public class Shooting : MonoBehaviour
{
 
    [SerializeField] private Transform shootingPoint;   // where bullets spawn
    [SerializeField] private GameObject bulletPrefab;   // projectile prefab
    [SerializeField] private float fireRate = 0.2f;     // time between shots

    private float nextFireTime = 0f;

    private void Update()
    {
        // Check if enough time has passed since last shot
        if (Time.time >= nextFireTime)
        {
            Shoot();
            nextFireTime = Time.time + fireRate; // schedule next allowed shot
        }
    }

    private void Shoot()
    {
        // Instantiate bullet at fire point
        Instantiate(bulletPrefab, shootingPoint.position, shootingPoint.rotation);
    }
}
