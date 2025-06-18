using UnityEngine;

[RequireComponent(typeof(Needs))]
[RequireComponent(typeof(Ageing))]
[RequireComponent(typeof(Perception))]
[RequireComponent(typeof(Movement))]
[RequireComponent(typeof(Consumption))]
public class AnimalController : MonoBehaviour
{
    private AnimalEvents events;

    private Needs needs;
    private Ageing ageing;
    private Perception perception;
    private Movement movement;
    private Consumption consumption;

    private Courting courting;
    private Reproduction reproduction;

    private Genome genome;

    private void Initialize(Genome genome) {
        events = new AnimalEvents();

        needs = GetComponent<Needs>();
        ageing = GetComponent<Ageing>();
        perception = GetComponent<Perception>();
        movement = GetComponent<Movement>();
        consumption = GetComponent<Consumption>();

        courting = GetComponent<Courting>();
        reproduction = GetComponent<Reproduction>();

        needs?.Setup(genome, events);
        ageing?.Setup(genome, events);
        perception?.Setup(genome, events);
        movement?.Setup(genome, events);
        consumption?.Setup(genome, events);

        courting?.Setup(genome, events);
        reproduction?.Setup(genome, events);

        events.OnDeathRequested += HandleDeath;
        events.OnPerceptableSpotted += CheckPercetable;
    }

    private void CheckPercetable() {

    }

    private void HandleDeath() {

    }
}
