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
    }

    public void Update() {

    }
}

// TODO: Need to implement how time will pass before we can do age
