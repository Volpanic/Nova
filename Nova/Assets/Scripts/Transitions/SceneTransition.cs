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

   /// <summary>
   /// Sets the transition to not be destroyed on scene change
   /// </summary>
    public void BeginTransition()
    {
        if(!isTran)
        {
            isTran = true;
            DontDestroyOnLoad(canvasObject);
            animator.Play("ani_transition_out");
        }
    }

    /// <summary>
    /// Used by a unityEvent
    /// </summary>
    public void GoToScene()
    {
        SceneManager.LoadScene(sceneToGoTo);
    }


    /// <summary>
    /// Destroys itself.
    /// </summary>
    public void EndTransition()
    {
        GetComponentInParent<CanvasRenderer>().Clear();
        Destroy(canvasObject);
    }
}
