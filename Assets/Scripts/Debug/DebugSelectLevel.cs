using UnityEngine;

public class DebugSelectLevel : MonoBehaviour
{
    [SerializeField]
    private GameManager.GameScene _level = GameManager.GameScene.MainMenu;

    public void Trigger()
    {
        GameManager.Instance.SelectLevel(_level);
    }

    public void Continue()
    {
        GameManager.Instance.ContinueGame();
    }

    public void BackToMain()
    {
        GameManager.Instance.BackToMain();
    }

    public void StartNewGame()
    {
        GameManager.Instance.StartNewGame();
    }

    public void LevelSelect()
    {
        GameManager.Instance.GoToLevelSelectMenu();
    }
}
