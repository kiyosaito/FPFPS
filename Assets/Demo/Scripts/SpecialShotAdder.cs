using UnityEngine;

public class SpecialShotAdder : MonoBehaviour
{
    private enum SpecialShotTypes
    {
        Launch,
        Warp,
        Break,
    }

    [SerializeField]
    private SpecialShotTypes _type = SpecialShotTypes.Launch;

    // TODO: Setup layers so there are less needless triggers
    public void OnTriggerEnter(Collider other)
    {
        Player player = other.gameObject.GetComponent<Player>();

        if (null != player)
        {
            ChargedWeapon weapon = player.GetComponentInChildren<ChargedWeapon>();

            weapon.AddSpecial(AddSpecialShotComponent(weapon.gameObject));

            // TODO: Consider if special shot dispenser recharges, or how to deal with loading back previous checkpoint
            Destroy(this.gameObject);
        }
    }

    private SpecialShot AddSpecialShotComponent(GameObject weapon)
    {
        SpecialShot special = null;

        switch (_type)
        {
            case SpecialShotTypes.Launch:
                special = ((SpecialShot)(weapon.AddComponent<LaunchShot>()));
                break;
            case SpecialShotTypes.Warp:
                special = ((SpecialShot)(weapon.AddComponent<WarpShot>()));
                break;
            case SpecialShotTypes.Break:
                special = ((SpecialShot)(weapon.AddComponent<BreakShot>()));
                break;
        }

        return special;
    }
}
