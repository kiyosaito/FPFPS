using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChargedWeaponEffect : MonoBehaviour
{
    #region Private Variables

    [SerializeField]
    private LineRenderer _line = null;

    [SerializeField]
    private Transform _shotOrigin = null;

    [SerializeField]
    private Animator _weaponAnimation = null;

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
        /*_line.SetPosition(0, _shotOrigin.position);
        _line.SetPosition(1, targetPos);
        _line.startColor = (charged ? Color.red : Color.yellow);
        _line.endColor = (charged ? Color.red : Color.yellow);
        _line.enabled = true;
        Invoke("TurnOff", 0.25f);*/

        _weaponAnimation.Play("Shoot");
    }

    #endregion
}
