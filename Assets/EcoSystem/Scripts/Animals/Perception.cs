using System;
using UnityEngine;

public class Perception : MonoBehaviour, IAnimalComponent
{
    private AnimalEvents events;

    private float range;

    public void Setup(Genome genome, AnimalEvents animalEvents) {
        events = animalEvents;

        range = genome.PerceptionRange;
    }
}

// TODO: Need to implement the idea of percebtables
