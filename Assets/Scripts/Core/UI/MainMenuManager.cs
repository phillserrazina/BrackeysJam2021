using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Lucerna.Utils;
using Lucerna.Audio;

public class MainMenuManager : MonoBehaviour
{
    public void GoToScene(string sceneName) {
        SceneLoader.instance.LoadSceneAsync(sceneName);
    }

    private void Start() {
        AudioManager.instance.Play("Main Menu");
    }
}
