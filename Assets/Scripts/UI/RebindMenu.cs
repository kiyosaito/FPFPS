﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RebindMenu : MonoBehaviour
{
    #region Private Variables

    // List to reference rebind buttons quickly
    private List<RebindButton> _buttons = null;

    private bool _rebindInProgress = false;

    #endregion

    #region Public Properties

    public bool RebindInProgress
    {
        get { return _rebindInProgress; }
    }

    #endregion

    #region MonoBehavious Functions

    private void Start()
    {
        // Initialise rebind button list
        _buttons = new List<RebindButton>(GetComponentsInChildren<RebindButton>());

        // Check for duplicate keybinds
        CheckForDuplicates();
    }

    private void RedyForRebind()
    {
        _rebindInProgress = false;
    }

    #endregion

    #region Public Functions

    public void CheckForDuplicates()
    {
        // Go through all buttons in the list
        for (int i = 0; i < _buttons.Count; ++i)
        {
            bool isDuplicate = false;

            // Get button to update it's text
            _buttons[i].SetupButtonText();

            // Compare to all other buttons in the list
            for (int j = 0; j < _buttons.Count; ++j)
            {
                if (i != j)
                {
                    // If they have the same keybind (that isn't None), then mark both as duplicates
                    if ((KeyCode.None != _buttons[i].MappedKey) && (_buttons[i].MappedKey == _buttons[j].MappedKey))
                    {
                        isDuplicate = true;
                        _buttons[j].SetAsDuplicate(true);

                        // We don't break, as there could be more duplicates later in the list, so we have to go through all
                    }
                }
            }

            _buttons[i].SetAsDuplicate(isDuplicate);
        }
    }

    public void RebindStarted()
    {
        _rebindInProgress = true;
    }

    public void RebindDone()
    {
        CheckForDuplicates();

        Invoke("RedyForRebind", 0.5f);
    }

    #endregion
}
