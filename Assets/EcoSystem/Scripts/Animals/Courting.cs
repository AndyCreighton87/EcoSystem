using UnityEngine;

public class Courting : MonoBehaviour, IAnimalComponent {

    private AnimalEvents events;

    private float attractivness;

    public void Setup(Genome genome, AnimalEvents animalEvents) {
        events = animalEvents;

        attractivness = genome.Attractivness;
    }
}

// TODO: Should be one of the last components implemented - need the animals in the scene and doing everything else before working about reproduction
