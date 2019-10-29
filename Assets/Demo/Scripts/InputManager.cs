using System.Collections.Generic;
using UnityEngine;

public sealed class InputManager : UnitySingleton<InputManager>
{
    #region Private Variables

    private int _mouseButtonIdx = 0;

    private Dictionary<InputKeys, KeyCode> _keyMapping = new Dictionary<InputKeys, KeyCode>();

    private float _mouseSensitivity = 1f;

    private float _keyboardSensitivity = 20f;

    private class AxisValue
    {
        public float axisValue;
        public InputKeys positiveButton;
        public InputKeys negativeButton;

        public AxisValue(InputKeys pos, InputKeys neg) { axisValue = 0f; positiveButton = pos; negativeButton = neg; }
    };

    private Dictionary<AxisInputs, AxisValue> _axisValues = new Dictionary<AxisInputs, AxisValue>();

    #endregion

    #region Public Properties

    public float MouseSensitivity
    {
        get { return _mouseSensitivity; }
        set { _mouseSensitivity = value; }
    }

    public float KeyboardSensitivity
    {
        get { return _keyboardSensitivity; }
        set { _keyboardSensitivity = value; }
    }

    #endregion

    #region Private Functions

    protected override void Setup()
    {
        // Map axis buttons to corresponding axis
        _axisValues.Add(AxisInputs.MoveHorizontal, new AxisValue(InputKeys.MoveHorizontalPositive, InputKeys.MoveHorizontalNegative));
        _axisValues.Add(AxisInputs.MoveVertical, new AxisValue(InputKeys.MoveVerticalPositive, InputKeys.MoveVerticalNegative));

        // Default keymapping
        _keyMapping.Add(InputKeys.MoveHorizontalPositive, KeyCode.D);
        _keyMapping.Add(InputKeys.MoveHorizontalNegative, KeyCode.A);
        _keyMapping.Add(InputKeys.MoveVerticalPositive, KeyCode.W);
        _keyMapping.Add(InputKeys.MoveVerticalNegative, KeyCode.S);
        _keyMapping.Add(InputKeys.Jump, KeyCode.Space);
        _keyMapping.Add(InputKeys.Escape, KeyCode.Escape);
    }

    #endregion

    #region Public Functions

    public enum AxisInputs
    {
        MoveHorizontal,
        MoveVertical,
    };

    public enum MouseAxisInputs
    {
        LookHorizontal,
        LookVertical,
    };

    public enum InputKeys
    {
        MoveHorizontalPositive,
        MoveHorizontalNegative,
        MoveVerticalPositive,
        MoveVerticalNegative,
        Jump,
        Escape,
    };

    public float GetAxis(AxisInputs axis)
    {
        return _axisValues[axis].axisValue;
    }

    public float GetMouseAxis(MouseAxisInputs axis)
    {
        float inputValue = 0f;

        switch (axis)
        {
            case MouseAxisInputs.LookHorizontal:
                inputValue = Input.GetAxis("Mouse X") * _mouseSensitivity;
                break;
            case MouseAxisInputs.LookVertical:
                inputValue = Input.GetAxis("Mouse Y") * _mouseSensitivity;
                break;
        }

        return inputValue;
    }

    public bool GetButton(InputKeys inputKey)
    {
        return Input.GetKey(_keyMapping[inputKey]);
    }

    public bool GetButtonDown(InputKeys inputKey)
    {
        return Input.GetKeyDown(_keyMapping[inputKey]);
    }

    public bool GetButtonUp(InputKeys inputKey)
    {
        return Input.GetKeyUp(_keyMapping[inputKey]);
    }

    public bool GetMouseButton()
    {
        return Input.GetMouseButton(_mouseButtonIdx);
    }

    public bool GetMouseButtonDown()
    {
        return Input.GetMouseButtonDown(_mouseButtonIdx);
    }

    public bool GetMouseButtonUp()
    {
        return Input.GetMouseButtonUp(_mouseButtonIdx);
    }

    #endregion

    #region MonoBehaviour Functions

    private void Update()
    {
        foreach (var axisValue in _axisValues)
        {
            float rawAxis = 0f;
            rawAxis += (Input.GetKey(_keyMapping[axisValue.Value.positiveButton]) ? 1f : 0f);
            rawAxis += (Input.GetKey(_keyMapping[axisValue.Value.negativeButton]) ? -1f : 0f);
            axisValue.Value.axisValue += Mathf.Clamp(rawAxis - axisValue.Value.axisValue, -1f / _keyboardSensitivity, 1f / _keyboardSensitivity);
        }
    }

    #endregion
}
