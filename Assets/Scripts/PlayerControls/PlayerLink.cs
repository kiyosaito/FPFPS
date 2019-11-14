using UnityEngine;

[CreateAssetMenu(fileName = "PlayerLink", menuName = "PlayerLink", order = 52)]
public class PlayerLink : ScriptableObject
{
    #region Singleton Like Access

    private static PlayerLink _instance = null;

    public static PlayerLink Instance
    {
        get { return _instance; }
    }

    [RuntimeInitializeOnLoadMethod]
    private static void Init()
    {
        _instance = Resources.LoadAll<PlayerLink>("")[0];
    }

    #endregion

    #region Player Instance Getter

    private Player _player = null;

    public Player PlayerInstance
    {
        get
        {
            if (null == _player)
            {
                _player = FindObjectOfType<Player>();
            }

            return _player;
        }
    }

    #endregion

    #region Weapon Instance Getter

    private ChargedWeapon _weapon = null;

    public ChargedWeapon WeaponInstance
    {
        get
        {
            if (null == _weapon)
            {
                _weapon = FindObjectOfType<ChargedWeapon>();
            }

            return _weapon;
        }
    }

    #endregion

    #region Camera Look Instance Getter

    private CameraLook _cameraLook = null;

    public CameraLook CameraLookInstance
    {
        get
        {
            if (null == _cameraLook)
            {
                _cameraLook = FindObjectOfType<CameraLook>();
            }

            return _cameraLook;
        }
    }

    #endregion

    #region Player Funcion Links

    public void Jump(float height)
    {
        PlayerInstance.Jump(height);
    }

    public void Dash()
    {
        PlayerInstance.Dash();
    }

    public void Boost(Transform transform)
    {
        PlayerInstance.Boost(transform);
    }

    public void Boost(Vector3 boost)
    {
        PlayerInstance.Boost(boost);
    }

    public void AirJump()
    {
        PlayerInstance.AirJump();
    }

    #endregion
}
