using UnityEngine;

public class SpecialShotAdder : ResetableObject
{
    #region Private Variables

    private bool _available = true;

    private bool _savedAvailable = true;

    private enum SpecialShotTypes
    {
        Launch,
        Warp,
        Break,
    }

    [SerializeField]
    private SpecialShotTypes _type = SpecialShotTypes.Launch;

    #endregion

    #region MonoBehaviour Functions

    // TODO: Setup layers so there are less needless triggers
    private void OnTriggerEnter(Collider other)
    {
        Player player = other.gameObject.GetComponent<Player>();

        if (null != player)
        {
            ChargedWeapon weapon = player.GetComponentInChildren<ChargedWeapon>();

            weapon.AddSpecial(AddSpecialShotComponent(weapon.gameObject));

            // TODO: Consider if special shot dispenser recharges
            gameObject.SetActive(false);
            _available = false;
        }
    }

    #endregion

    #region Private Functions

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

    #endregion

    #region Abstract class implementation

    public override void SaveState()
    {
        _savedAvailable = _available;
    }

    public override void ResetState()
    {
        _available = _savedAvailable;
        gameObject.SetActive(_available);
    }

    #endregion
}
