using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class MainMenuActions : MonoBehaviour
{
    public SceneTransitionMaker transition;

    public void Awake()
    {
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");

        foreach(GameObject player in players)
        {
            Destroy(player);
        }
    }

    /// <summary>
    /// Goes to the first scene
    /// </summary>
    public void NewGame()
    {
        if (transition == null) return;

        transition.DoTransition("scn_volcano1");
    }

    public void MainMenu()
    {
        if (transition == null) return;

        transition.DoTransition("MainMenu");
    }

    /// <summary>
    /// goes to the tutorial scene, unless on mobile or console
    /// </summary>
    public void ToTutorial()
    {
        if (transition == null) return;

        //Only show controls on pc, Mobile and console controls can be infered.
        if (!Application.isConsolePlatform && !Application.isMobilePlatform)
        {
            transition.DoTransition("scn_tutorial");
        }
        else
        {
            NewGame();
        }
    }
    
    /// <summary>
    /// Load the save, if there is a save to load that is.
    /// </summary>
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

            Debug.Log(Path.Combine(Application.persistentDataPath, SaveGame.fileName));
        }
    }

    /// <summary>
    /// closes the game.
    /// </summary>
    public void Exit()
    {
        if (transition == null) return;

        if (!Application.isMobilePlatform && Application.platform != RuntimePlatform.WebGLPlayer)
        { 
            Application.Quit();
        }
    }
}
