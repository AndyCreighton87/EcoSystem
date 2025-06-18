using UnityEngine;

public class Genome
{
    public Sex Sex;
    public float Speed;
    public float PerceptionRange;
    public float Attractivness;
    public float GestationPeriod;

    public int MaxAge;
    public int StartingAge;
}

public enum Sex { 
    Male, 
    Female 
}
