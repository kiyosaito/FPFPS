using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraLook : MonoBehaviour
{
    public bool isCursorHidden = true;
    public float minPitch = -80f, maxPitch = 80f;
    public Vector2 speed = new Vector2(120f, 120f); // Speed in degrees (per second)

    private Vector2 euler; // Current euler rotation 

    // Start is called before the first frame update
    void Start()
    {
        // Is the cursor supposed to be hidden?
        if (isCursorHidden)
        {
            // Lock and hide it
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false; // ... Invisible!
        }
        //Get current camera euler
        euler = transform.eulerAngles;
    }

    // Update is called once per frame
    void Update()
    {
        if (!PlayerLink.Instance.PlayerInstance.isActiveAndEnabled)
        {
            return;
        }

        isCursorHidden = isCursorHidden != (InputManager.Instance.GetButtonDown(InputManager.InputKeys.Menu));

        if (isCursorHidden)
        {
            Cursor.lockState = CursorLockMode.None;

            // Lock and hide it
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false; // ... Invisible!
        }

        // Clamp the camera on pitch
        float newPitch = Mathf.Clamp(euler.x - InputManager.Instance.GetMouseAxis(InputManager.MouseAxisInputs.LookVertical) * speed.y * Time.deltaTime, minPitch, maxPitch);

        // Rotate the camera with Mouse movement
        euler.y += InputManager.Instance.GetMouseAxis(InputManager.MouseAxisInputs.LookHorizontal) * speed.x * Time.deltaTime;
        euler.x = newPitch;

        // Apply euler to the Player & Camera seperately
        transform.parent.localEulerAngles = new Vector3(0, euler.y, 0);
        transform.localEulerAngles = new Vector3(euler.x, 0, 0);

    }

    // Reset the camera to a specific rotation, used when restoring to previous checkpoint
    public void ResetCamera(Quaternion resetRotation)
    {
        euler = resetRotation.eulerAngles;

        transform.parent.localEulerAngles = new Vector3(0, euler.y, 0);
        transform.localEulerAngles = new Vector3(euler.x, 0, 0);
    }
}
