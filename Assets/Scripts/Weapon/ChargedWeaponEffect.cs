using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChargedWeaponEffect : MonoBehaviour
{
    #region Private Variables

    [SerializeField]
    private LineRenderer _line = null;

    [SerializeField]
    private Animator _weaponAnimation = null;

    [SerializeField]
    private GameObject _spark = null;

    [SerializeField]
    private GameObject _powerSpark = null;

    [SerializeField]
    private GameObject chargedSoundEffect = null;

    [SerializeField]
    private GameObject normalSoundEffect = null;
    #endregion

    #region MonoBehaviour Functions

    private void Start()
    {
        _line.enabled = false;
    }

    #endregion

    #region Private Functions

    private void TurnOff()
    {
        _line.enabled = false;
    }

    #endregion

    #region Public Functions

    public void PlayShotEffect(bool charged, Vector3 targetPos)
    {
        if (charged)
        {
            chargedSoundEffect.GetComponent<SFXRandomizer>().SoundEffect();
        }
        else
        {
            normalSoundEffect.GetComponent<SFXRandomizer>().SoundEffect();
        }
        _weaponAnimation.Play("Shoot");
        Instantiate<GameObject>((charged ? _powerSpark : _spark), targetPos, transform.rotation);
    }

    #endregion
}
