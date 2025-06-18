using UnityEngine;

public class Movement : MonoBehaviour, IAnimalComponent
{
    private AnimalEvents events;

    private float speed;

    public void Setup(Genome genome, AnimalEvents animalEvents) {
        events = animalEvents;

        speed = genome.Speed;
    }
}