using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarpShot : MonoBehaviour, SpecialShot
{
    public bool Shoot(Vector3 hitPos, RaycastHit hit)
    {
        bool warped = false;

        if (null != hit.collider)
        {
            Player player = PlayerLink.Instance.PlayerInstance;
            player.Warp(hit.point - (hit.point - player.transform.position).normalized * 0.5f);
            Disappear();
            warped = true;
        }

        return warped;
    }

    public void Disappear()
    {
        Destroy(this);
    }
}
