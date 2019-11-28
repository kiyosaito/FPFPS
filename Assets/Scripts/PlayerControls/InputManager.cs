﻿using System.Collections.Generic;
using UnityEngine;

public sealed class InputManager : UnitySingleton<InputManager>
{
    #region OptionsData

    private class OptionsData : PersistableData
    {
        public const int CurrentFormatVersion = 1;

        public float FileVersion = 0;

        public float MouseSensitivity = 1f;

        public List<(int, int, int)> Keymappings = new List<(int, int, int)>();

        public override void Save(GameDataWriter writer)
        {
            writer.Write(-CurrentFormatVersion);

            writer.Write(MouseSensitivity);

            writer.Write(Keymappings.Count);

            for (int i = 0; i < Keymappings.Count; ++i)
            {
                writer.Write(Keymappings[i].Item1);
                writer.Write(Keymappings[i].Item2);
                writer.Write(Keymappings[i].Item3);
            }
        }

        protected override void Init(GameDataReader reader)
        {
            FileVersion = -reader.ReadInt();

            if (CurrentFormatVersion == FileVersion)
            {
                MouseSensitivity = reader.ReadFloat();

                int keybindCount = reader.ReadInt();
                for (int i = 0; i < keybindCount; ++i)
                {
                    Keymappings.Add((reader.ReadInt(), reader.ReadInt(), reader.ReadInt()));
                }
            }
        }

        public OptionsData() { }
    }

    private void SaveData()
    {
        OptionsData data = new OptionsData();

        data.MouseSensitivity = _mouseSensitivity;

        foreach (var keybind in _keyMapping)
        {
            data.Keymappings.Add(((int)(keybind.Key), (int)(keybind.Value[0]), (int)(keybind.Value[1])));
        }

        GameDataWriter.SaveData("InputSettings", data);
    }

    private void LoadData()
    {
        OptionsData data = GameDataReader.LoadData<OptionsData>("InputSettings");

        if ((null != data) && (OptionsData.CurrentFormatVersion == data.FileVersion))
        {
            _mouseSensitivity = data.MouseSensitivity;

            Dictionary<InputKeys, KeyCode[]> defaultKeymapping = new Dictionary<InputKeys, KeyCode[]>();

            foreach (var keybind in data.Keymappings)
            {
                defaultKeymapping.Add((InputKeys)(keybind.Item1), new KeyCode[2] { (KeyCode)(keybind.Item2), (KeyCode)(keybind.Item3) });
            }

            Dictionary<InputKeys, KeyCode[]> tmp = _keyMapping;
            _keyMapping = defaultKeymapping;
            tmp.Clear();
            tmp = null;
        }
    }

    #endregion

    #region Private Variables

    private bool _optionsLoaded = false;

    private Dictionary<InputKeys, KeyCode[]> _keyMapping = new Dictionary<InputKeys, KeyCode[]>();

    private float _mouseSensitivity = 1f;

    private float _keyboardSensitivity = 20f;

    // (Is rebinding active, Key to rebind, Primary or secondary binding)
    private (bool, InputKeys, bool) _rebinding = (false, InputKeys.None, true);

    private class AxisValue
    {
        public float axisValue;
        public InputKeys positiveButton;
        public InputKeys negativeButton;

        public AxisValue(InputKeys pos, InputKeys neg) { axisValue = 0f; positiveButton = pos; negativeButton = neg; }
    };

    private Dictionary<AxisInputs, AxisValue> _axisValues = new Dictionary<AxisInputs, AxisValue>();

    private System.Action _rebindCallback = null;

    #endregion

    #region Public Properties

    public float MouseSensitivity
    {
        // Convert range 1 - 9 to 0.2 - 5 with 5 converting to 1
        get { return (Mathf.Log(_mouseSensitivity * 5.0625f, 1.5f) + 1f); }
        set { _mouseSensitivity = Mathf.Pow(1.5f, (Mathf.Clamp(value, 1f, 9f) - 1f)) / 5.0625f; }
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

        ResetKeybindsToDefault();
    }

    #endregion

    #region Public Functions

    public void LoadSavedOptions()
    {
        if (!_optionsLoaded)
        {
            _optionsLoaded = true;
            LoadData();
        }
    }

    public void SaveOptions()
    {
        SaveData();
    }

    public void ResetKeybindsToDefault()
    {
        Dictionary<InputKeys, KeyCode[]> defaultKeymapping = new Dictionary<InputKeys, KeyCode[]>();

        // Default keymapping
        defaultKeymapping.Add(InputKeys.MoveHorizontalPositive, new KeyCode[2] { KeyCode.D, KeyCode.RightArrow });
        defaultKeymapping.Add(InputKeys.MoveHorizontalNegative, new KeyCode[2] { KeyCode.A, KeyCode.LeftArrow });
        defaultKeymapping.Add(InputKeys.MoveVerticalPositive, new KeyCode[2] { KeyCode.W, KeyCode.UpArrow });
        defaultKeymapping.Add(InputKeys.MoveVerticalNegative, new KeyCode[2] { KeyCode.S, KeyCode.DownArrow });
        defaultKeymapping.Add(InputKeys.Jump, new KeyCode[2] { KeyCode.Space, KeyCode.Mouse1 });
        defaultKeymapping.Add(InputKeys.Menu, new KeyCode[2] { KeyCode.Escape, KeyCode.None });
        defaultKeymapping.Add(InputKeys.QuickRestart, new KeyCode[2] { KeyCode.R, KeyCode.None });
        defaultKeymapping.Add(InputKeys.Shoot, new KeyCode[2] { KeyCode.Mouse0, KeyCode.None });
        defaultKeymapping.Add(InputKeys.None, new KeyCode[2] { KeyCode.None, KeyCode.None });

        Dictionary<InputKeys, KeyCode[]> tmp = _keyMapping;
        _keyMapping = defaultKeymapping;
        tmp.Clear();
        tmp = null;
    }

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
        None,
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

        foreach (var keycode in _keyMapping[inputKey])
        {
            buttonState = buttonState || Input.GetKey(keycode);
        }

        return buttonState;
    }

    public bool GetButtonDown(InputKeys inputKey)
    {
        bool buttonState = false;

        foreach (var keycode in _keyMapping[inputKey])
        {
            buttonState = buttonState || Input.GetKeyDown(keycode);
        }

        return buttonState;
    }

    public bool GetButtonUp(InputKeys inputKey)
    {
        bool buttonState = false;

        foreach (var keycode in _keyMapping[inputKey])
        {
            buttonState = buttonState || Input.GetKeyUp(keycode);
        }

        return buttonState;
    }

    public void RebindButton(InputKeys inputKey, bool primary, System.Action callback)
    {
        if (InputKeys.None != inputKey)
        {
            _rebinding = (true, inputKey, primary);
            _rebindCallback = callback;
        }
    }

    public KeyCode GetMappedKey(InputKeys inputkey, bool primary)
    {
        return _keyMapping[inputkey][primary ? 0 : 1];
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

        if ((_rebinding.Item1) && Input.anyKeyDown)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                // Cancel rebind
                _rebinding = (false, InputKeys.None, true);

                if(null != _rebindCallback)
                {
                    _rebindCallback();
                    _rebindCallback = null;
                }
            }
            else
            {
                foreach (KeyCode vKey in System.Enum.GetValues(typeof(KeyCode)))
                {
                    if (Input.GetKeyDown(vKey))
                    {
                        // Rebind button

                        // If the other keybind is the same, set that to none
                        if (_keyMapping[_rebinding.Item2][_rebinding.Item3 ? 1 : 0] == vKey)
                        {
                            _keyMapping[_rebinding.Item2][_rebinding.Item3 ? 1 : 0] = KeyCode.None;
                        }

                        // If the new keybind is the same as the exisitng one, set the keybind to none
                        if (_keyMapping[_rebinding.Item2][_rebinding.Item3 ? 0 : 1] == vKey)
                        {
                            _keyMapping[_rebinding.Item2][_rebinding.Item3 ? 0 : 1] = KeyCode.None;
                        }
                        else
                        {
                            _keyMapping[_rebinding.Item2][_rebinding.Item3 ? 0 : 1] = vKey;
                        }

                        _rebinding = (false, InputKeys.None, true);

                        if (null != _rebindCallback)
                        {
                            _rebindCallback();
                            _rebindCallback = null;
                        }
                        break;
                    }
                }
            }
        }
    }

    #endregion
}
