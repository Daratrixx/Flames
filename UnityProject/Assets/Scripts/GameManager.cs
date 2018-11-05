using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class GameManager : MonoBehaviour {

    public static GameManager singleton;
    public static PlayerInput AZERTY = null;
    public static PlayerInput QWERTY = null;

    public PlayerController[] characters = new PlayerController[0];
    private int currentCharacterIndex = 0;

    // Use this for initialization
    void Start () {
        InitLayout();
        singleton = this;
        for (int i = 1; i < characters.Length; ++i) {
            characters[i].enabled = false;
        }
        RemapControls();
    }
    void InitLayout() {
        if (AZERTY == null) {
            AZERTY = Resources.Load<PlayerInput>("ZQSD");
        }
        if (QWERTY == null) {
            QWERTY = Resources.Load<PlayerInput>("WASD");
        }
    }

    private void SwapCharacter() {
        if (characters.Length > 0) {
            characters[currentCharacterIndex].character.NoMove();
            characters[currentCharacterIndex].enabled = false;
            currentCharacterIndex = (currentCharacterIndex + 1) % characters.Length;
            characters[currentCharacterIndex].enabled = true;
            RemapControls();
        }
    }

    private void RemapControls() {
        Debug.Log(Application.systemLanguage);
        if (isAzerty) {
            characters[currentCharacterIndex].inputs = AZERTY;
        } else {
            characters[currentCharacterIndex].inputs = QWERTY;
        }
    }

    public static void SwapCharacters() {
        singleton.SwapCharacter();
    }

    public static bool isAzerty {
        get { return Application.systemLanguage == SystemLanguage.French; }
    }

}
