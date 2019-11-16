using UnityEngine;
using UnityEngine.UI;
using TMPro;

[RequireComponent(typeof(Button))]
public class RebindButton : MonoBehaviour
{
    #region Private Variables

    // Text color for the default normal case and when the button mapping is duplicated, and used multiple times
    private static Color textColorNormal = Color.black;
    private static Color textColorDuplicate = Color.red;

    // The input key this button is responsible for mapping
    [SerializeField]
    private InputManager.InputKeys _inputKey = InputManager.InputKeys.None;

    // Determines if this is the primary or secondary keymapping (no functional difference, just allows two different keys to be mapped)
    [SerializeField]
    private bool _primary = true;

    // The key that's mapped to the input key
    private KeyCode _mappedKey = KeyCode.None;

    // Reference to the button text
    private TextMeshProUGUI _buttonTextDisplay = null;

    // Since ButtonTextDisplay might be referenced before Start runs, we use a private property that automatically fills the value if it's still null
    private TextMeshProUGUI ButtonTextDisplay
    {
        get
        {
            if (null == _buttonTextDisplay)
            {
                // Get the button text component reference
                _buttonTextDisplay = GetComponentInChildren<TextMeshProUGUI>();
            }

            return _buttonTextDisplay;
        }
    }

    private RebindMenu _rebindMenu = null;

    #endregion

    #region Public Properties

    public KeyCode MappedKey
    {
        get { return _mappedKey; }
    }

    #endregion

    #region MonoBehaviour Functions

    private void Start()
    {
        _rebindMenu = GetComponentInParent<RebindMenu>();

        // Connect the button onclick event to the start rebind function
        GetComponent<Button>().onClick.AddListener(() => StartRebind());

        // Initial setup of button text
        SetupButtonText();
    }

    #endregion

    #region Public Functions

    // Sets the color of the button text based on if it's a duplicate or not
    public void SetAsDuplicate(bool duplicate)
    {
        ButtonTextDisplay.color = (duplicate ? textColorDuplicate : textColorNormal);
    }

    // Set the button text
    public void SetupButtonText()
    {
        // Get the mapped key from the InputManager and update text
        _mappedKey = InputManager.Instance.GetMappedKey(_inputKey, _primary);
        ButtonTextDisplay.text = _mappedKey.ToString();
    }

    #endregion

    #region Private Functions

    // Start the rebind process
    private void StartRebind()
    {
        if (!_rebindMenu.RebindInProgress)
        {
            _rebindMenu.RebindStarted();

            // Change the button text to indicate rebinding is in progress
            ButtonTextDisplay.text = "Press Key";
            ButtonTextDisplay.color = textColorNormal;

            // Start rebind process via InputManager
            InputManager.Instance.RebindButton(_inputKey, _primary, _rebindMenu.RebindDone);
        }
    }

    #endregion
}
