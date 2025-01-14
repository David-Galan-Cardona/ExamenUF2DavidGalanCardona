using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanonRotation : MonoBehaviour
{
    public Vector3 _maxRotation;
    public Vector3 _minRotation;
    public Vector3 direction;
    public Camera mainCam;
    public Vector3 mousePos;
    public bool canShoot;
    public float timer;
    private float offset = -51.6f;
    public GameObject ShootPoint;
    public GameObject Bullet;
    public float ProjectileSpeed = 0;
    public float MaxSpeed;
    public float MinSpeed;
    float angle;
    Vector3 bulletSpeedVector;
    public GameObject PotencyBar;
    private float initialScaleX;

    private void Awake()
    {
        initialScaleX = PotencyBar.transform.localScale.x;
        _maxRotation = new Vector3 (0,0,90);
        _minRotation = new Vector3 (0,0,0);
        MaxSpeed = 8;
        MinSpeed = 1;
    }
    void Update()
    {

        mousePos = mainCam.ScreenToWorldPoint(Input.mousePosition);
        Vector3 direction = mousePos - transform.position;
  
        angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            if (angle > _maxRotation.z) {
                angle = _maxRotation.z;
            }
            else if (angle < _minRotation.z) {
                angle = _minRotation.z;
            }
        transform.rotation = Quaternion.Euler(0f, 0f, angle+offset);

        if (Input.GetMouseButton(0) && canShoot)
        {
            if (ProjectileSpeed < MaxSpeed){
                ProjectileSpeed += 1;
            }
            canShoot = false;
        }
        if(Input.GetMouseButtonUp(0))
        {
            var projectile = Instantiate(Bullet, ShootPoint.transform.position, Quaternion.identity);
            
            bulletSpeedVector = new Vector2(direction.x, direction.y).normalized;
            if (bulletSpeedVector.x < 0) {
                bulletSpeedVector.x = 0;
            }
            else if (bulletSpeedVector.y < 0) {
                bulletSpeedVector.y = 0;
            }
            projectile.GetComponent<Rigidbody2D>().velocity = bulletSpeedVector * ProjectileSpeed;
            ProjectileSpeed = MinSpeed;
        }
        CalculateBarScale();
        if (!canShoot)
            {
                timer += Time.deltaTime;
                if (timer >= 0.25f)
                {
                    canShoot = true;
                    timer = 0;
                }
            }

    }
    public void CalculateBarScale()
    {
        PotencyBar.transform.localScale = new Vector3(Mathf.Lerp(0, initialScaleX, ProjectileSpeed / MaxSpeed),
            PotencyBar.transform.localScale.y,
            PotencyBar.transform.localScale.z);
    }
}
