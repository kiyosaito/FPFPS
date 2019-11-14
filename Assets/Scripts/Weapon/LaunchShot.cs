using UnityEngine;

public class LaunchShot : MonoBehaviour, SpecialShot
{
    private const float _boost = 2f;

    public bool Shoot(Vector3 hitPos, RaycastHit hit)
    {
        Player player = PlayerLink.Instance.PlayerInstance;

        Vector3 boostVector = (hitPos - player.transform.position).normalized * _boost;
        player.Boost(boostVector);


        return true;
    }

    public void Disappear()
    {
        Destroy(this);
    }
}
