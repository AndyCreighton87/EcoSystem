using System;
using UnityEngine;

public class TimeManager : MonoBehaviour {

    public static TimeManager Instance;

    [SerializeField] private float secondsPerYear = 60.0f;

    public float TimeScale { get; private set; } = 1.0f;

    public float DeltaTime => Time.deltaTime * TimeScale;

    private float accumulatedTime = 0f;
    private float totalYearElapsed = 0f;

    public event Action OnYearElapsed;
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

    private void Update() {
        accumulatedTime += DeltaTime;

        while (accumulatedTime >= secondsPerYear) {
            accumulatedTime -= secondsPerYear;
            totalYearElapsed += 1;
            OnYearElapsed?.Invoke();
        }

        Debug.Log($"TotalYearsElapsed: {totalYearElapsed}");
    }

    public float GetYearFraction() => accumulatedTime / secondsPerYear;
}
