using UnityEngine;

public static class StaticStats {
    private static int score;

    public static int Score {
        get {
            return score;
        } set {
            score = value;
        }
    }
}
