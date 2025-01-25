using System.Collections;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    bool canPresss = false;
    IEnumerator Start()
    {
        yield return new WaitForSeconds(1.5f);
        canPresss = true;
    }

    void Update()
    {
        if (canPresss && Input.anyKey)
            SceneNav.GoTo(SceneType.PlayerSelect);
    }
}
