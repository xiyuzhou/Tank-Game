using UnityEngine;
using Photon.Pun;

public class WallHp : MonoBehaviourPun
{
    public int wallHp = 6;
    private MeshRenderer wallRenderer;

    private void Start()
    {
        wallRenderer = GetComponent<MeshRenderer>();
    }
    public void HitByBullet()
    {
        photonView.RPC("HitByBulletConfig", RpcTarget.All);
    }
    [PunRPC]
    public void HitByBulletConfig()
    {
        wallHp--;

        // Calculate the color based on remaining health
        float healthPercentage = (float)wallHp / 6f; // Assuming initial wallHp is 6
        Color newColor = new Color(1f, healthPercentage, healthPercentage, 1f);

        // Update the wall's color
        wallRenderer.material.color = newColor;

        if (wallHp <= 0)
        {
            Destroy(gameObject);
        }
    }
}
