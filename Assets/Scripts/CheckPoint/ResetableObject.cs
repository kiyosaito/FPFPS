using UnityEngine;

public abstract class ResetableObject : MonoBehaviour
{
    public abstract void SaveState();

    public abstract void ResetState();
}
