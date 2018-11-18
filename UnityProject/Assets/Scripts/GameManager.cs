using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class GameManager : MonoBehaviour {

    public static GameManager singleton;
    public static PlayerInput AZERTY = null;
    public static PlayerInput QWERTY = null;

    public List<PlayerController> characters = new List<PlayerController>();
    private int currentCharacterIndex = 0;
    public PlayerController currentCharacter {
        get { return characters[currentCharacterIndex]; }
    }


    // Use this for initialization
    void Start () {
        InitLayout();
        singleton = this;
        for (int i = 1; i < characters.Count; ++i) {
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
        if (characters.Count > 0) {
            characters[currentCharacterIndex].character.NoMove();
            characters[currentCharacterIndex].enabled = false;
            currentCharacterIndex = (currentCharacterIndex + 1) % characters.Count;
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

    public static void RemoveCharacterController(PlayerController pc) {
        if(singleton.currentCharacter == pc) {
            singleton.SwapCharacter();
            singleton.characters.Remove(pc);
        } else {
            singleton.characters.Remove(pc);
        }
    }

    public static void RegisterCharacterController(PlayerController pc) {
        singleton.characters.Add(pc);
    }

    public static void SwapCharacters() {
        singleton.SwapCharacter();
    }

    public static bool isAzerty {
        get { return Application.systemLanguage == SystemLanguage.French; }
    }

}
