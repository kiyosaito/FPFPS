using UnityEngine;

public class DebugSelectLevel : MonoBehaviour
{
    [SerializeField]
    private GameManager.GameScene _level = GameManager.GameScene.MainMenu;

    public void Trigger()
    {
        GameManager.Instance.SelectLevel(_level);
    }
}
