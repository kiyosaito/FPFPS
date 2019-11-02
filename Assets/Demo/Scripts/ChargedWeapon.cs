using UnityEngine;
using UnityEngine.Assertions;

// Interface for targets that can be shot by the weapon
public interface Target
{
    void GetShot(bool charged, Vector3 point);
}

// Interface for special shot types
public interface SpecialShot
{
    bool Shoot(Vector3 hitPos, RaycastHit hit);

    void Disappear();
}

public class ChargedWeapon : MonoBehaviour
{
    #region Private Variables

    private enum WeaponStates
    {
        Standby,
        Charging,
        Reload,
        Charged,
        Overload,
    };

    // The current state of the weapon
    [SerializeField] // Serialized only for debug purposes
    private WeaponStates _weaponState = WeaponStates.Standby;

    // Timer to keep track of time between state changes
    [SerializeField] // Serialized only for debug purposes
    private float _timer = 0f;

    // Time it takes for the weapon to build up a charged shot
    [SerializeField]
    private float _chargeTime = 1f;

    // Time it takes for the weapon to reload after a normal or charged shot
    [SerializeField]
    private float _reloadTime = 0.3f;

    // Time it takes for the weapon to overload after it was charged up
    [SerializeField]
    private float _overloadTime = 3f;

    // Time it takes for the weapon to cool down, after it was overloaded
    [SerializeField]
    private float _cooldownTime = 5f;

    // The camera that's used for aiming (most likely the main camera)
    [SerializeField]
    private Camera _cam = null;

    // The layers the weapon can hit
    [SerializeField]
    private LayerMask _hitMask = ~0;

    // The current target, if any
    private Target _currentTarget = null;

    // The maximum distance for the weapon
    [SerializeField]
    private float _maxDistance = 100f;

    private ChargedWeaponEffect _shotEffect = null;

    private Vector3 _lastHitPos = Vector3.zero;

    private RaycastHit _lastHit = default;

    private SpecialShot _special = null;

    #endregion

    #region Public Properties

    public RaycastHit LastHit
    {
        get { return _lastHit; }
    }

    #endregion

    #region MonoBehaviour Functions

    private void Start()
    {
        // TODO: Move this to a more generic place, like a GameManager
        for (int i = 0; i < 32; ++i)
        {
            Physics.IgnoreLayerCollision(LayerMask.NameToLayer("TargetLayer"), i);
        }

        if (null == _cam)
        {
            _cam = Camera.main;
        }

        _shotEffect = GetComponent<ChargedWeaponEffect>();
    }

    private void Update()
    {
        if (!PlayerLink.Instance.PlayerInstance.isActiveAndEnabled)
        {
            return;
        }

        AimWeapon();

        _timer = Mathf.Max(0f, _timer - Time.deltaTime);

        if (InputManager.Instance.GetMouseButton())
        {
            // The shoot button is held
            switch (_weaponState)
            {
                case WeaponStates.Standby:
                    // If the weapon was in standby, start charging
                    _weaponState = WeaponStates.Charging;
                    _timer = _chargeTime;
                    break;
                case WeaponStates.Charging:
                    // If the weapon is charging, check if charging is finished
                    if (0f == _timer)
                    {
                        // If charging is done, change state to charged and start overload timer
                        _weaponState = WeaponStates.Charged;
                        _timer = _overloadTime;
                    }
                    break;
                case WeaponStates.Reload:
                    // If the weapon was reloading, check if it's done
                    if (0f == _timer)
                    {
                        // If the relaod is done, start charging the next shot
                        _weaponState = WeaponStates.Charging;
                        _timer = _chargeTime;
                    }
                    break;
                case WeaponStates.Charged:
                    // If the weapon was charged, check if it has overloaded
                    if (0f == _timer)
                    {
                        // If the weapon overloads, change state and start cooldown timer
                        _weaponState = WeaponStates.Overload;
                        _timer = _cooldownTime;
                    }
                    break;
                case WeaponStates.Overload:
                    // If the weapon was overloaded, check if the cooldown is done
                    if (0f == _timer)
                    {
                        // If the cooldown is done, start charging the next shot
                        _weaponState = WeaponStates.Charging;
                        _timer = _chargeTime;
                    }
                    break;
                default:
                    Assert.IsTrue(false, "Unhandled weapon state");
                    break;
            }
        }
        else
        {
            // The shoot button is released
            switch (_weaponState)
            {
                case WeaponStates.Standby:
                    // In standby mode do nothing
                    break;
                case WeaponStates.Charging:
                    // If the weapon was charging, release a normal shot and start reload
                    Shoot(false);
                    _weaponState = WeaponStates.Reload;
                    _timer = _reloadTime;
                    break;
                case WeaponStates.Reload:
                    // If reload is done, change to standby mode
                    if (0f == _timer)
                    {
                        _weaponState = WeaponStates.Standby;
                    }
                    break;
                case WeaponStates.Charged:
                    // If the weapon was charged, release a charged shot
                    Shoot(true);
                    _weaponState = WeaponStates.Reload;
                    _timer = _reloadTime;
                    break;
                case WeaponStates.Overload:
                    // If the weapon was overloaded, check if the cooldown is done
                    if (0f == _timer)
                    {
                        // If the cooldown is done, switch to standby
                        _weaponState = WeaponStates.Standby;
                    }
                    break;
                default:
                    Assert.IsTrue(false, "Unhandled weapon state");
                    break;
            }
        }
    }

    #endregion

    #region Private Functions

    private void Shoot(bool charged)
    {
        if (null != _shotEffect)
        {
            _shotEffect.PlayShotEffect(charged, _lastHitPos);
        }

        if ((null != _special) && charged)
        {
            if(_special.Shoot(_lastHitPos, _lastHit))
            {
                _special.Disappear();
                _special = null;
            }
        }
        else if (null != _currentTarget)
        {
            _currentTarget.GetShot(charged, _lastHitPos);
        }
    }

    #endregion

    #region Public Functions

    public void AimWeapon()
    {
        _currentTarget = null;

        // We send out a ray from the center of the camera
        RaycastHit hit;
        if (Physics.Raycast(_cam.transform.position, _cam.transform.TransformDirection(Vector3.forward), out hit, _maxDistance, _hitMask))
        {
            _currentTarget = hit.collider.GetComponent<Target>();
            _lastHitPos = hit.point;
        }
        else
        {
            _lastHitPos = transform.position + _cam.transform.TransformDirection(Vector3.forward) * _maxDistance;
        }
        _lastHit = hit;
    }

    // Function to add a special shot type to the weapon
    public void AddSpecial(SpecialShot special)
    {
        if (null != _special)
        {
            // If we already had a special shot, replace the existing one with the new one
            _special.Disappear();
        }

        _special = special;
    }

    // Reset the weapon to it's default state
    public void ResetWeapon()
    {
        // Go to standby mode, and reset timer
        _weaponState = WeaponStates.Standby;
        _timer = 0f;

        // Discard target information
        _currentTarget = null;
        _lastHitPos = Vector3.zero;
        _lastHit = default;

        // Remove special shot if one is present
        if (null != _special)
        {
            _special.Disappear();
            _special = null;
        }
    }

    #endregion
}
