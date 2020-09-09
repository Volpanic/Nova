using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransitionMaker : MonoBehaviour
{
    public GameObject sceneTransitionCanvas = null;

    public bool spawnPlayer = false;
    public float playerX = 0;
    public float playerY = 0;

    private void Start()
    {
        SceneManager.sceneLoaded += SceneManager_sceneLoaded;
    }

    public void DoTransition(string roomName)
    {
        GameObject canvas = GameObject.Instantiate(sceneTransitionCanvas,Vector3.zero,Quaternion.identity);
        canvas.GetComponentInChildren<SceneTransition>().sceneToGoTo = roomName;
        canvas.GetComponentInChildren<SceneTransition>().BeginTransition();
    }

    public void DoTransitionMovePlayer(GameSaveData saveData)
    {
        DontDestroyOnLoad(gameObject);
        DoTransition(saveData.sceneName);
        spawnPlayer = true;
        playerX = saveData.playerXPos;
        playerY = saveData.playerYPos;
    }
    
    public void DoTransitionMovePlayer(GameSaveData saveData,string sceneName, Vector2 position)
    {
        DontDestroyOnLoad(gameObject);
        DoTransition(sceneName);
        spawnPlayer = true;
        playerX = position.x;
        playerY = position.y;
    }

    private void SceneManager_sceneLoaded(Scene arg0, LoadSceneMode arg1)
    {
        if (spawnPlayer)
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            player.transform.position = new Vector3(playerX, playerY, player.transform.position.z);

            Destroy(gameObject);
        }
    }
}
