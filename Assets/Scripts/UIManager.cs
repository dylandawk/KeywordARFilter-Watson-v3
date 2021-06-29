using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    GameObject inputForm;
    [SerializeField]
    GameObject resetButton;



    public void DisableInputform()
    {
        inputForm.SetActive(false);
    }

    public void EnableInputForm()
    {
        inputForm.SetActive(true);
    }

    public void EnableReset()
    {
        resetButton.SetActive(true);
    }

    public void DisableReset()
    {
        resetButton.SetActive(false);
    }

}
