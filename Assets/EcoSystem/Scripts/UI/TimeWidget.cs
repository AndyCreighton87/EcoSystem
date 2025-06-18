using UnityEngine;

public class TimeWidget : MonoBehaviour
{
    public void Pause() => TimeManager.Instance.SetTimeScale(0.0f);

    public void NormalSpeed() => TimeManager.Instance.SetTimeScale(1.0f);

    public void DoubleSpeed() => TimeManager.Instance.SetTimeScale(2.0f);

    public void QuadSpeed() => TimeManager.Instance.SetTimeScale(4.0f);
}
