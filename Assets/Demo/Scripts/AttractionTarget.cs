using UnityEngine;

public class AttractionTarget : MonoBehaviour, Target
{
    #region Private Variables

    // The attraction strength that will affect the player
    [SerializeField]
    private float _attractionStrength = 1f;

    // The attraction strength that will affect the player when the player fires a charged shot and ChargedShotAlternateBoost mode is used
    [SerializeField]
    private float _altAttractionStrength = 1f;

    private enum ChargedShotInteractionModes
    {
        AnyShotActivates,           // Any shot activates attraction with default strength
        NormalShotOnly,             // Only normal shot activates attraction, using default strength
        ChargedShotOnly,            // Only charged shot activates attraction, using default strength
        ChargedShotAlternateBoost,  // Normal shot activates with default strength, charged shot activates with alt attraction strength
    };

    [SerializeField]
    private ChargedShotInteractionModes _mode = ChargedShotInteractionModes.AnyShotActivates;

    #endregion

    #region Interface Implementation

    public void GetShot(bool charged, Vector3 point)
    {
        float boostStrength = 0f;
        switch (_mode)
        {
            case ChargedShotInteractionModes.AnyShotActivates:
                boostStrength = _attractionStrength;
                break;
            case ChargedShotInteractionModes.NormalShotOnly:
                boostStrength = (charged ? 0f : _attractionStrength);
                break;
            case ChargedShotInteractionModes.ChargedShotOnly:
                boostStrength = (charged ? _attractionStrength : 0f);
                break;
            case ChargedShotInteractionModes.ChargedShotAlternateBoost:
                boostStrength = (charged ? _altAttractionStrength : _attractionStrength);
                break;
        }

        if (0f != boostStrength)
        {
            // Get the line between the player and the hit position, set it's magnitude according to the attraction strength
            Vector3 boost = (point - PlayerLink.Instance.PlayerInstance.transform.position).normalized * boostStrength;

            // Boost the player
            PlayerLink.Instance.PlayerInstance.Boost(boost);
        }
    }

    #endregion
}
