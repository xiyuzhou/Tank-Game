using UnityEngine;
using Photon.Pun;
public class Bullet : MonoBehaviourPun
{
    public float speed = 10f;
    public float lifeTime = 8f; // Set the desired lifetime in seconds

    private void Update()
    {
        // Reduce the lifetime of the bullet
        lifeTime -= Time.deltaTime;

        // Check if the bullet's lifetime has expired
        if (lifeTime <= 0f)
        {
            Destroy(gameObject);
            //photonView.RPC("OnDestroy", RpcTarget.All);
            return;
        }

        MoveBullet();
    }

    private void MoveBullet()
    {
        // Move the bullet based on its forward direction
        transform.position += transform.forward * speed * Time.deltaTime;
    }

    private void OnCollisionEnter(Collision collision)
    {
        // Check if the collided object has the "Wall" tag
        if (collision.gameObject.CompareTag("wall"))
        {
            Debug.Log("hitted");

            // Calculate the reflection direction
            Vector3 incidentDirection = transform.forward;
            Vector3 normal = collision.contacts[0].normal;
            Vector3 reflectDirection = Vector3.Reflect(incidentDirection, normal).normalized;

            // Move the bullet based on the reflection direction
            transform.position = collision.contacts[0].point + reflectDirection * 0.1f; // Adjust 0.1f to avoid sticking to the wall

            // Rotate the bullet to face the reflection direction
            transform.forward = reflectDirection;
            if (collision.gameObject.GetComponent<WallHp>() != null)
            {
                collision.gameObject.GetComponent<WallHp>().HitByBullet();
            }
        }
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<TankPlayerInfo>().photonView.RPC("OnHealChange", RpcTarget.All,-1);
            Destroy(gameObject);
        }
    }
    [PunRPC]
    private void OnDestroy()
    {
        Destroy(gameObject);
    }
}
