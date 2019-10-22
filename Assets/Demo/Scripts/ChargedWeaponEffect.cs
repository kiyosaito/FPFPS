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

    public void PlayShotEffect(Vector3 targetPos)
    {
        _line.SetPosition(0, _shotOrigin.position);
        _line.SetPosition(1, targetPos);
        _line.startColor = Color.yellow;
        _line.endColor = Color.yellow;
        _line.enabled = true;
        Invoke("TurnOff", 0.25f);
    }

    public void PlayChargedShotEffect(Vector3 targetPos)
    {
        _line.SetPosition(0, _shotOrigin.position);
        _line.SetPosition(1, targetPos);
        _line.startColor = Color.red;
        _line.endColor = Color.red;
        _line.enabled = true;
        Invoke("TurnOff", 0.25f);
    }

    #endregion
}
