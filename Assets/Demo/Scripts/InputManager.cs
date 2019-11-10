using System.Collections.Generic;
using UnityEngine;

public sealed class InputManager : UnitySingleton<InputManager>
{
    #region Private Variables

    #region Input Source

    private abstract class InputSource
    {
        public abstract bool GetInput();
        public abstract bool GetInputDown();
        public abstract bool GetInputUp();
    };

    private class InputSourceKeyboard : InputSource
    {
        private KeyCode _key = KeyCode.None;

        public override bool GetInput()
        {
            return Input.GetKey(_key);
        }

        public override bool GetInputDown()
        {
            return Input.GetKeyDown(_key);
        }

        public override bool GetInputUp()
        {
            return Input.GetKeyUp(_key);
        }

        public InputSourceKeyboard(KeyCode key)
        {
            _key = key;
        }
    };

    private class InputSourceMouse : InputSource
    {
        private int _mouseButtonIdx = 0;

        public override bool GetInput()
        {
            return Input.GetMouseButton(_mouseButtonIdx);
        }

        public override bool GetInputDown()
        {
            return Input.GetMouseButtonDown(_mouseButtonIdx);
        }

        public override bool GetInputUp()
        {
            return Input.GetMouseButtonUp(_mouseButtonIdx);
        }

        public InputSourceMouse(int idx)
        {
            _mouseButtonIdx = idx;
        }
    };

    #endregion

    private Dictionary<InputKeys, InputSource[]> _keyMapping = new Dictionary<InputKeys, InputSource[]>();

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
        _keyMapping.Add(InputKeys.MoveHorizontalPositive, new InputSource[2] { new InputSourceKeyboard(KeyCode.D), new InputSourceKeyboard(KeyCode.RightArrow) });
        _keyMapping.Add(InputKeys.MoveHorizontalNegative, new InputSource[2] { new InputSourceKeyboard(KeyCode.A), new InputSourceKeyboard(KeyCode.LeftArrow) });
        _keyMapping.Add(InputKeys.MoveVerticalPositive, new InputSource[2] { new InputSourceKeyboard(KeyCode.W), new InputSourceKeyboard(KeyCode.UpArrow) });
        _keyMapping.Add(InputKeys.MoveVerticalNegative, new InputSource[2] { new InputSourceKeyboard(KeyCode.S), new InputSourceKeyboard(KeyCode.DownArrow) });
        _keyMapping.Add(InputKeys.Jump, new InputSource[2] { new InputSourceKeyboard(KeyCode.Space), new InputSourceMouse(1) });
        _keyMapping.Add(InputKeys.Menu, new InputSource[2] { new InputSourceKeyboard(KeyCode.Escape), null });
        _keyMapping.Add(InputKeys.QuickRestart, new InputSource[2] { new InputSourceKeyboard(KeyCode.R), null });
        _keyMapping.Add(InputKeys.Shoot, new InputSource[2] { new InputSourceMouse(0), null });
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
        Menu,
        QuickRestart,
        Shoot,
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
        bool buttonState = false;

        foreach (var inputSource in _keyMapping[inputKey])
        {
            if (null != inputSource)
            {
                buttonState = buttonState || inputSource.GetInput();
            }
        }

        return buttonState;
    }

    public bool GetButtonDown(InputKeys inputKey)
    {
        bool buttonState = false;

        foreach (var inputSource in _keyMapping[inputKey])
        {
            if (null != inputSource)
            {
                buttonState = buttonState || inputSource.GetInputDown();
            }
        }

        return buttonState;
    }

    public bool GetButtonUp(InputKeys inputKey)
    {
        bool buttonState = false;

        foreach (var inputSource in _keyMapping[inputKey])
        {
            if (null != inputSource)
            {
                buttonState = buttonState || inputSource.GetInputUp();
            }
        }

        return buttonState;
    }

    #endregion

    #region MonoBehaviour Functions

    private void Update()
    {
        foreach (var axisValue in _axisValues)
        {
            float rawAxis = 0f;
            rawAxis += (GetButton(axisValue.Value.positiveButton) ? +1f : 0f);
            rawAxis += (GetButton(axisValue.Value.negativeButton) ? -1f : 0f);
            axisValue.Value.axisValue += Mathf.Clamp(rawAxis - axisValue.Value.axisValue, -1f / _keyboardSensitivity, 1f / _keyboardSensitivity);
        }
    }

    #endregion
}
