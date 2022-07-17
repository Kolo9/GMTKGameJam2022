using System;

public class Random : IRandom {
    private static System.Random rng = new System.Random();
    public int NextInclusive(int start, int end) {
        return rng.Next(start, end + 1);
    }
}