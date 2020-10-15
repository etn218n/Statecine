using UnityEngine;

public class LimitFPS : MonoBehaviour
{
    [RuntimeInitializeOnLoadMethod]
    public static void Limit()
    {
        QualitySettings.vSyncCount  = 0;
        Application.targetFrameRate = 2000;
    }
}
