using UnityEngine;

public class Reproduction : MonoBehaviour, IAnimalComponent
{
    private AnimalEvents events;

    private float gestationPeriod;
    public void Setup(Genome genome, AnimalEvents animalEvents) {
        events = animalEvents;

        gestationPeriod = genome.GestationPeriod;
    }
}

// TODO: Should be one of the last components implemented - need the animals in the scene and doing everything else before working about reproduction
