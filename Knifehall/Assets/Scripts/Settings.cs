using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Settings : MonoBehaviour
{
  public Button remapButton;
  public KeyCode actionKey;
  private bool isRemapping = false;
  public TMP_Text buttonText;
  public GameObject player;

  void Start()
  {
    remapButton.onClick.AddListener(() => isRemapping = true);

    // Load the action key from prefs
    if (PlayerPrefs.HasKey("ActionKey"))
    {
      actionKey = (KeyCode)PlayerPrefs.GetInt("ActionKey");
    } else 
    {
      actionKey = KeyCode.Space;
    }

    buttonText.text = actionKey.ToString();
  }

  void Update()
  {
    // If remapping, check for key presses
    if (isRemapping)
    {
      foreach (KeyCode keyCode in System.Enum.GetValues(typeof(KeyCode)))
      {
        if (Input.GetKeyDown(keyCode))
        {
          // When key pressed, set in prefs and stop remapping
          Debug.Log(keyCode);

          PlayerPrefs.SetInt("ActionKey", (int)keyCode);
          PlayerPrefs.Save();

          buttonText.text = keyCode.ToString();

          isRemapping = false;
          break;
        }
      }
    }
  }
}