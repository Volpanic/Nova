using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class MobileButtonScript : MonoBehaviour
{
    private bool overide = false;
    private Button button = null;

    public UnityEvent OnPressed = new UnityEvent();
    public UnityEvent OnReleased = new UnityEvent();

    // Start is called before the first frame update
    void Start()
    {
        if(!Application.isMobilePlatform && !overide)
        {
            gameObject.SetActive(false);
            return;
        }
    }
    
    public void OnButtonPressed()
    {
        OnPressed.Invoke();
    }

    public void OnButtonUp()
    {
        OnReleased.Invoke();
    }
}
