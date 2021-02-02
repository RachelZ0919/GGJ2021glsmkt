using UnityEngine;
using System.Collections;


public class GameMenuCommand : Command
{
    public override void Execute()
    {
        GameManager.instance.PauseOrResumeGame();
    }
}
