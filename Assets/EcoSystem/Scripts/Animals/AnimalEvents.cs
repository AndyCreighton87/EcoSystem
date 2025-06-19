using System;
using UnityEngine;

public class AnimalEvents
{
    public event Action OnDeathRequested;
    public event Action OnPerceptableSpotted;

    public void DeathRequested() => OnDeathRequested?.Invoke();

    public void PerceptableSpotted() => OnPerceptableSpotted?.Invoke();
}
