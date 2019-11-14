using UnityEngine;

public class BreakShot : MonoBehaviour, SpecialShot
{
    public bool Shoot(Vector3 hitPos, RaycastHit hit)
    {
        RaycastHit potHit = hit;
        BreakableObject pot = null;

        if (null != hit.collider)
        {
            pot = potHit.collider.gameObject.GetComponent<BreakableObject>();
        }

        ChargedWeapon weapon = PlayerLink.Instance.WeaponInstance;

        while (null != pot)
        {
            pot.GetShot(true, hit.point);
            pot = null;

            weapon.AimWeapon();
            potHit = weapon.LastHit;

            if (null != potHit.collider)
            {
                pot = potHit.collider.gameObject.GetComponent<BreakableObject>();
            }
        }

        return true;
    }

    public void Disappear()
    {
        Destroy(this);
    }
}
