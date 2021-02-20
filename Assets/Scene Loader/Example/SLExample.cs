using UnityEngine;
using Lucerna.Utils;

public class SLExample : MonoBehaviour
{
    private void Update() {
        if (Input.GetKeyDown(KeyCode.Space))
            SceneLoader.instance.LoadSceneAsync(SceneLoader.instance.CurrentScene.name);
    }
}
