using UnityEngine;

public class AnimationReset : ResetableObject, Target
{
    #region Private variablse

    [SerializeField]
    private Animator animator = null;

    [SerializeField]
    private string aliveStateName = "";

    [SerializeField]
    private string deadStateName = "";

    [SerializeField]
    private string startDyingStateName = "";

    private bool alive = true;

    private bool savedAlive = true;

    #endregion

    public void Trigger()
    {
        if (null == animator)
        {
            Debug.LogError(gameObject.name + " is missing animator");
            return;
        }

        if (alive)
        {
            alive = false;

            animator.Play(startDyingStateName);
        }
    }

    public void GetShot(bool charged, Vector3 point)
    {
        Trigger();
    }

    public override void ResetState()
    {
        if (null == animator)
        {
            Debug.LogError(gameObject.name + " is missing animator");
            return;
        }

        alive = savedAlive;

        if (alive)
        {
            animator.Play(aliveStateName);
        }
        else
        {
            animator.Play(deadStateName);
        }
    }

    public override void SaveState()
    {
        savedAlive = alive;
    }
}
