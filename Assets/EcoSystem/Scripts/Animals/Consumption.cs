using UnityEngine;

public class Consumption : MonoBehaviour, IAnimalComponent
{
    private AnimalEvents events;

    public void Setup(Genome genome, AnimalEvents animalEvents) {
        events = animalEvents;
    }

}

// TODO: Need to implement food/water before we can consume