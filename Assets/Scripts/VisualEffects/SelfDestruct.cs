using UnityEngine;

public class SelfDestruct : MonoBehaviour
{
    [SerializeField]
    private float _time = 5f;

    private void Start()
    {
        Invoke("Destruct", _time);
    }

    private void Destruct()
    {
        Destroy(gameObject);
    }
}
