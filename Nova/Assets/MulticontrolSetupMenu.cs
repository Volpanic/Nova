using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MulticontrolSetupMenu : MonoBehaviour
{
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

    private void Select_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        MenuItems[selectedItem].onClick.Invoke();
    }

    private void Down_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        selectedItem++;
        Rehighlight();
    }

    private void Up_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        selectedItem--;
        Rehighlight();
    }

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
            else // Unhighlight the others.
            {
                
            }
        }

        Debug.Log("Selected Menu Item: " + selectedItem.ToString());
    }

    private void OnDisable()
    {
        controls.Menus.Disable();
    }

    private void OnDestroy()
    {
        controls.Menus.Disable();
    }
}
