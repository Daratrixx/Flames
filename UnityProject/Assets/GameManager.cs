using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    public static GameManager singleton;

    public PlayerController[] characters = new PlayerController[0];
    private int currentCharacterIndex = 0;

    // Use this for initialization
    void Start () {
        singleton = this;
        for(int i = 1; i < characters.Length; ++i) {
            characters[i].enabled = false;
        }
    }

    private void SwapCharacter() {
        if (characters.Length > 0) {
            Debug.Log("" + currentCharacterIndex + "/" + characters.Length);
            characters[currentCharacterIndex].enabled = false;
            currentCharacterIndex = (currentCharacterIndex + 1) % characters.Length;
            Debug.Log("" + currentCharacterIndex + "/" + characters.Length);
            characters[currentCharacterIndex].enabled = true;
        }
    }

    public static void SwapCharacters() {
        singleton.SwapCharacter();
    }

}
