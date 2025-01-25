using System.Collections;
using UnityEngine;

public class AutoSkip : MonoBehaviour
{
    IEnumerator Start()
    {
        yield return new WaitForSeconds(1);
        if(MinigameManager.Instance.RoundsLeft() !=0)
            SceneNav.GoTo(SceneType.Gameplay);
    }
}
