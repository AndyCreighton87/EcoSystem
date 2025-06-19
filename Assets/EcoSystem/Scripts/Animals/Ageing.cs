using System;
using UnityEngine;

public class Ageing : MonoBehaviour, IAnimalComponent {

    private int maxAge;
    private int age;

    private AnimalEvents events;

    public void Setup(Genome genome, AnimalEvents animalEvents) {
        events = animalEvents;

        maxAge = genome.MaxAge;
        age = genome.StartingAge;

        TimeManager.Instance.OnYearElapsed += OnYearElapsed;
    }

    private void OnYearElapsed() {
        age++;

        if (age > maxAge) {
            TimeManager.Instance.OnYearElapsed -= OnYearElapsed;
            events.DeathRequested();
        }
    }
}
