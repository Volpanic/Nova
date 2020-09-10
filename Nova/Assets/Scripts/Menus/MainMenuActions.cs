using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class MainMenuActions : MonoBehaviour
{
    public SceneTransitionMaker transition;

    public void NewGame()
    {
        if (transition == null) return;

        transition.DoTransition("scn_volcano1");
    }
    
    public void Continue()
    {
        if (transition == null) return;

        if (File.Exists(Path.Combine(Application.persistentDataPath, SaveGame.fileName)))
        {
            var load = SaveGame.LoadGame();

            if (load != null)
            {
                transition.DoTransitionMovePlayer(load,load.sceneName,new Vector2(load.playerXPos, load.playerYPos));
            }
            else
            {
                Debug.LogWarning("No Save file to respawn to.");
            }
        }
    }
    
    public void Exit()
    {
        if (transition == null) return;

        Application.Quit();
    }
}
