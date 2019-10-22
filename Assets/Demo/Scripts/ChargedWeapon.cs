using UnityEngine;
using UnityEngine.Assertions;

public interface Target
{
    void GetShot(bool charged);
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

    // The rate the weapon returns to the neautral rotation when there's no target available
    [SerializeField]
    private float _rotSlerpRate = 1f;

    // The angle treshold below which the weapon snaps to the same rotation as the camera, instead of slerping
    [SerializeField]
    private float _rotTreshold = 5f;

    private ChargedWeaponEffect _shotEffect = null;

    private Vector3 _lastHitPos = Vector3.zero;

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
        AimWeapon();

        _timer = Mathf.Max(0f, _timer - Time.deltaTime);

        // TODO: change to use input manager, instead of directly getting the input
        if (Input.GetMouseButton(0))
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
                    Shoot();
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
                    ShootCharged();
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

    private void AimWeapon()
    {
        // We send out a ray from the center of the camera
        RaycastHit hit;
        if (Physics.Raycast(_cam.transform.position, _cam.transform.TransformDirection(Vector3.forward), out hit, _maxDistance, _hitMask))
        {
            _currentTarget = hit.collider.GetComponent<Target>();

            // We check if it hit a target
            //if (null != _currentTarget)
            //{
            //    // We make the weapon immediately face the target
            //    transform.LookAt(hit.point);
            //}

            _lastHitPos = hit.point;
        }
        else
        {
            _lastHitPos = transform.position + _cam.transform.TransformDirection(Vector3.forward) * _maxDistance;
        }

        //if (null == _currentTarget)
        //{
        //    // If we don't have a target, we gradually move facing the same direction as the player head/camera
        //    if (Mathf.Abs(Quaternion.Angle(transform.rotation, _cam.transform.rotation)) < _rotTreshold)
        //    {
        //        transform.rotation = _cam.transform.rotation;
        //    }
        //    else
        //    {
        //        transform.rotation = Quaternion.Slerp(transform.rotation, _cam.transform.rotation, _rotSlerpRate * Time.deltaTime);
        //    }
        //}
    }

    private void Shoot()
    {
        if (null != _shotEffect)
        {
            _shotEffect.PlayShotEffect(_lastHitPos);
        }

        if (null != _currentTarget)
        {
            _currentTarget.GetShot(false);
        }
    }

    private void ShootCharged()
    {
        if (null != _shotEffect)
        {
            _shotEffect.PlayChargedShotEffect(_lastHitPos);
        }

        if (null != _currentTarget)
        {
            _currentTarget.GetShot(true);
        }
    }

    #endregion
}
