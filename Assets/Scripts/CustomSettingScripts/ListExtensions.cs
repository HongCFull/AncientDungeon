using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ListExtensions {
    /// <summary>
    /// Shuffles the element order of the specified list.
    /// </summary>
    public static void Shuffle<T>(this IList<T> myList) {
        int count = myList.Count;
        int last = count - 1;
        for (int i = 0; i < last; ++i) {
            int randIndex = UnityEngine.Random.Range(i, count);
            T tmp = myList[i];
            myList[i] = myList[randIndex];
            myList[randIndex] = tmp;
        }
    }
}
