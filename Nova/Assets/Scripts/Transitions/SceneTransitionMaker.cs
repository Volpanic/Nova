using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransitionMaker : MonoBehaviour
{
    [Tooltip("The prefab of the transition object")]
    public GameObject sceneTransitionCanvas = null;

    public bool spawnPlayer = false;
    public float playerX = 0;
    public float playerY = 0;

    private Scene initScene;

    private void Start()
    {
        SceneManager.sceneLoaded += SceneManager_sceneLoaded;
        initScene = SceneManager.GetActiveScene();
    }

    /// <summary>
    /// Simply goes to a room
    /// </summary>
    /// <param name="roomName"></param>
    public void DoTransition(string roomName)
    {
        if (sceneTransitionCanvas == null) return;
        GameObject canvas = GameObject.Instantiate(sceneTransitionCanvas,Vector3.zero,Quaternion.identity);
        canvas.GetComponentInChildren<SceneTransition>().sceneToGoTo = roomName;
        canvas.GetComponentInChildren<SceneTransition>().BeginTransition();
    }

    /// <summary>
    /// Moves player to loaded point and scene
    /// </summary>
    /// <param name="saveData"></param>
    public void DoTransitionMovePlayer(GameSaveData saveData)
    {
        DontDestroyOnLoad(gameObject);
        DoTransition(saveData.sceneName);
        spawnPlayer = true;
        playerX = saveData.playerXPos;
        playerY = saveData.playerYPos;
    }
    
    /// <summary>
    /// Moves player to specified point and room.
    /// </summary>
    /// <param name="saveData"></param>
    /// <param name="sceneName"></param>
    /// <param name="position"></param>
    public void DoTransitionMovePlayer(GameSaveData saveData,string sceneName, Vector2 position)
    {
        DontDestroyOnLoad(gameObject);
        DoTransition(sceneName);
        spawnPlayer = true;
        playerX = position.x;
        playerY = position.y;
    }

    /// <summary>
    /// moves the player to the desired target when the scene is actually changed.
    /// </summary>
    /// <param name="arg0"></param>
    /// <param name="arg1"></param>
    private void SceneManager_sceneLoaded(Scene arg0, LoadSceneMode arg1)
    {
        if (spawnPlayer)
        {
            GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
            foreach (GameObject player in players)
            {
                player.transform.position = new Vector3(playerX, playerY, player.transform.position.z);

                if (initScene != arg0)
                {
                    //Destroy(gameObject);
                }
            }
        }
    }
}
