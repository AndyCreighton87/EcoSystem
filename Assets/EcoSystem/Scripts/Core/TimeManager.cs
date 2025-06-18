using System;
using UnityEngine;

public class TimeManager : MonoBehaviour {

    public static TimeManager Instance;

    public float TimeScale { get; private set; } = 1.0f;

    public float DeltaTime => Time.deltaTime * TimeScale;

    public event Action<float> OnTimeScaleChanged;

    private void Awake() {
        if (Instance != null) {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    public void SetTimeScale(float value) {
        TimeScale = value;
        OnTimeScaleChanged?.Invoke(TimeScale);
    }
}
