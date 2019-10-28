using UnityEngine;

public class BreakShot : MonoBehaviour, SpecialShot
{
    public bool Shoot(Vector3 hitPos, RaycastHit hit)
    {
        RaycastHit potHit = hit;
        BreakableObject pot = potHit.collider.gameObject.GetComponent<BreakableObject>();

        while (null != pot)
        {
            pot.GetShot(true, hit.point);
            pot = null;

            ChargedWeapon weapon = PlayerLink.Instance.WeaponInstance;

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
