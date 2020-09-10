using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransition : MonoBehaviour
{

    public string sceneToGoTo;
    public Animator animator;

    private bool isTran = false;
    public GameObject canvasObject;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    public void BeginTransition()
    {
        if(!isTran)
        {
            isTran = true;
            DontDestroyOnLoad(canvasObject);
            animator.Play("ani_transition_out");
        }
    }

    public void GoToScene()
    {
        SceneManager.LoadScene(sceneToGoTo);
    }

    public void EndTransition()
    {
        GL.Clear(true,true,Color.black);
        Destroy(canvasObject);
    }
}
