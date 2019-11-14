using UnityEngine;

public class BreakableObject : ResetableObject, Target
{
    #region Private Variables

    // Determines whether this object can be broken with a charged shot only, or if a normal shot can break it as well
    [SerializeField]
    private bool _chargedOnly = true;

    // Has this object been destroyed or not
    private bool _alive = true;

    // The state of the alive variable when SaveState is called
    private bool _savedAlive = true;

    #endregion

    #region Interface and abstract class implementations

    // Interface for when the player shoots the target
    public void GetShot(bool charged, Vector3 point)
    {
        if (charged || !_chargedOnly)
        {
            // If the shot was charged, or if a normal shot also destroys the object, then get destroyed
            GetDestroyed();
        }
        else
        {
            // If only a charged shot destroys the object, resist the shot
            Resist();
        }
    }

    // Reset the state to how it was at the last save
    public override void ResetState()
    {
        _alive = _savedAlive;
        GetComponent<BoxCollider>().enabled = _alive;
        gameObject.SetActive(_alive);
    }

    // Save the current state
    public override void SaveState()
    {
        _savedAlive = _alive;
    }

    #endregion

    private void GetDestroyed()
    {
        // TODO: Do effect for being destroyed
        GetComponent<BoxCollider>().enabled = false;
        gameObject.SetActive(false);
        _alive = false;
    }

    private void Resist()
    {
        // TODO: Do effect indicating the attack failed
    }
}
