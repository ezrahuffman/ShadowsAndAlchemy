using UnityEngine;

public class Target : Enemy
{
    protected override void OnDie()
    {
        base.OnDie();

        // End game
        GameController.instance.OnTargetDie();
    }
}
