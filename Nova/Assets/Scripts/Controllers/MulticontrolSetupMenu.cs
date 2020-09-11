using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MulticontrolSetupMenu : MonoBehaviour
{
    [Tooltip("A list of button components in top-first vertical order.")]
    public List<Button> MenuItems = new List<Button>();

    private EventSystem eventSystem;
    private ControlScheme controls;
    private int selectedItem = 0;

    // Start is called before the first frame update
    void Start()
    {
        //Setup Input
        controls = new ControlScheme();
        controls.Menus.Enable();

        controls.Menus.Up.performed += Up_performed;
        controls.Menus.Down.performed += Down_performed;
        controls.Menus.Select.performed += Select_performed;
        
        eventSystem = GameObject.Find("EventSystem").GetComponent<EventSystem>();

        Rehighlight();
    }

    /// <summary>
    /// Used in button event triggers, properly highlights moused over buttons
    /// </summary>
    /// <param name="button"></param>
    public void MouseOverButton(Button button)
    {
        int pos = MenuItems.IndexOf(button);

        if(pos != -1)
        {
            selectedItem = pos;
            Rehighlight();
        }
    }

    // Invokes button action
    private void Select_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        MenuItems[selectedItem].onClick.Invoke();
    }

    //Moves menu
    private void Down_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        selectedItem++;
        Rehighlight();
    }


    //Moves menu
    private void Up_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        selectedItem--;
        Rehighlight();
    }

    /// <summary>
    /// Clamps selected item so it's not out of index, Highlights the selected
    /// </summary>
    private void Rehighlight()
    {
        if(selectedItem < 0)
        {
            selectedItem = MenuItems.Count - 1;
        }

        if(selectedItem >= MenuItems.Count)
        {
            selectedItem = 0;
        }

        for(int i = 0; i < MenuItems.Count; i++)
        {
            if(selectedItem == i) // Highlight the newly selected item
            {
                MenuItems[i].Select();
            }
        }
    }

    private void OnDisable()
    {
        if (controls != null)
        {
            controls.Menus.Disable();
        }
    }
    
    private void OnEnable()
    {
        if (controls != null)
        {
            controls.Menus.Enable();
        }
    }

    private void OnDestroy()
    {
        controls.Menus.Disable();
    }
}
