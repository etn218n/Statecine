using UnityEngine;

public class LimitFPS : MonoBehaviour
{
    [RuntimeInitializeOnLoadMethod]
    public static void Limit()
    {
        QualitySettings.vSyncCount  = 1;
        Application.targetFrameRate = 144;
    }
}
