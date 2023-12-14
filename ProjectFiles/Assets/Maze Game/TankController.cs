using UnityEngine;
using Photon.Pun;

public class TankController : MonoBehaviourPun
{
    public float moveSpeed = 5f;
    public Transform turret;

    public float timeBetweenShots = 0.5f;

    private float lastShotTime;
    public GameObject bulletPrefab;
    public Transform bulletSpawnPoint;
    public TankPlayerInfo playerInfo;
    private Rigidbody tankRigidbody;

    void Start()
    {
        // Get the Rigidbody component on the tank
        tankRigidbody = GetComponent<Rigidbody>();

        // Make sure the Rigidbody has the correct constraints
        if (tankRigidbody != null)
        {
            tankRigidbody.freezeRotation = true; // Freeze rotation to prevent unwanted tilting
            tankRigidbody.useGravity = false; // Turn off gravity for better control
        }
    }

    void Update()
    {
        if (!photonView.IsMine || playerInfo.isDead)
        {
            // we'll handle movement for other players via the PhotonTransformView, so just return if this player isn't me
            return;
        }
        HandleMovement();
        HandleBaseRotation();
        HandleTurretRotation();
        if (Input.GetMouseButton(0))
        {
            FireBullet();
        }
    }

    void HandleMovement()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        Vector3 movement = new Vector3(horizontalInput, 0f, verticalInput);

        // Calculate the movement velocity based on the input
        Vector3 velocity = movement * moveSpeed;

        // Apply the calculated velocity directly to the Rigidbody
        tankRigidbody.velocity = velocity;
    }

    void HandleBaseRotation()
    {
        Vector3 moveDirection = new Vector3(Input.GetAxis("Horizontal"), 0f, Input.GetAxis("Vertical"));

        if (moveDirection.sqrMagnitude > 0.01f)
        {
            Quaternion toRotation = Quaternion.LookRotation(moveDirection, Vector3.up);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, Time.deltaTime * 360f);
        }
    }

    void HandleTurretRotation()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            Vector3 targetPosition = hit.point;
            targetPosition.y = turret.position.y;

            turret.LookAt(targetPosition);
        }
    }

    void FireBullet()
    {
        if (Time.time - lastShotTime < timeBetweenShots)
        {
            return;
        }

        if (bulletPrefab != null && bulletSpawnPoint != null)
        {

            photonView.RPC("SpawnBullet", RpcTarget.All, bulletSpawnPoint.position, bulletSpawnPoint.rotation);
            lastShotTime = Time.time;
        }
        else
        {
            Debug.LogError("BulletPrefab or BulletSpawnPoint is not assigned.");
        }
    }
    [PunRPC]
    void SpawnBullet(Vector3 pos, Quaternion dir)
    {
        GameObject bullet = Instantiate(bulletPrefab, pos, dir);

        Bullet bulletScript = bullet.GetComponent<Bullet>();

        if (bulletScript != null)
        {
            bulletScript.speed = 5f;
            bulletScript.lifeTime = 8f;
        }
    }
}
