using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public static class Utils
{
    public static List<T> Shuffled<T>(this List<T> original) {
        List<T> list = new List<T>(original);

        for (int i = 0; i < list.Count; i++) {
            int randomIndex = Random.Range(i, list.Count);
            (list[i], list[randomIndex]) = (list[randomIndex], list[i]);
        }

        return list;
    }
}
