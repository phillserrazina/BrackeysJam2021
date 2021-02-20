using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Lucerna.Utils;

public class MainMenuManager : MonoBehaviour
{
    public void GoToScene(string sceneName) {
        SceneLoader.instance.LoadSceneAsync(sceneName);
    }
}
