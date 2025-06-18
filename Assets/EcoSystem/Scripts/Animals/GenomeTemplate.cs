using UnityEngine;

[CreateAssetMenu(fileName = "Genome", menuName = "EcoSystem/Animal Genome Template")]

public class GenomeTemplate : ScriptableObject
{
    public Sex sex;
    public float speed;
    public float perceptionRange;
    public float attractivness;
    public float gestationPeriod;

    public int maxAge;
    public int startingAge;

    public Genome ToGenome() {
        return new Genome {
            Sex = sex,
            Speed = speed,
            PerceptionRange = perceptionRange,
            Attractivness = attractivness,
            GestationPeriod = gestationPeriod,
            MaxAge = maxAge,
            StartingAge = startingAge
        };
    }
}
