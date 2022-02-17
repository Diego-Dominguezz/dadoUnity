using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;



public class menuButton : MonoBehaviour
{
    public Button button;
    static int gamble;
    public InputField mainInputField;
    public Toggle hasNotWon;

    // Start is called before the first frame update
    void Start()
    {
        Button btn = button.GetComponent<Button>();

        btn.onClick.AddListener(TaskOnClick);

        mainInputField.onValueChanged.AddListener(delegate { LockInput(mainInputField); });
        button.interactable = false;

    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void TaskOnClick()
    {
        GameValues.hasNotWon = hasNotWon.isOn;
        Debug.Log(GameValues.hasNotWon);
        SceneManager.LoadScene("dadoLanzamiento");

    }
    void LockInput(InputField input)
    {
        if (input.text.Length > 0)
        {
            if ((int.Parse(input.text) < 7) && (int.Parse(input.text) > 0))
            {
                button.interactable = true;
            }
            else
            {
                button.interactable = false;
            }

        }
        else if (input.text.Length == 0)
        {
            button.interactable = false;
        }
        gamble = int.Parse(input.text);
        GameValues.gamble = gamble;

    }

}
