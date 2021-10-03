using System.Collections.Generic;
using UnityEngine;

public enum GameState
{
    DRAW,
    PLAY,
    END,
    PAUSE
}

public static class StaticVariable
{
    public static bool isLoaded = false;
    public static readonly string DATA_PLAYER = "player.data";
    public static readonly string DATA_NULL = "null.data";
    public static readonly string DATA_SOUND = "sound.data";

    public static readonly string TUTORIAL_01 = "tut_01";
    public static readonly string TUTORIAL_02 = "tut_02";
    public static readonly string TUTORIAL_03 = "tut_03";

    public static GameState GameState;

    public static int ScoreMultiplier = 5;

    public static void Shuffle<T>(this IList<T> ts)
    {
        var count = ts.Count;
        var last = count - 1;
        for (var i = 0; i < last; ++i)
        {
            var r = UnityEngine.Random.Range(i, count);
            var tmp = ts[i];
            ts[i] = ts[r];
            ts[r] = tmp;
        }
    }

    public static T[] Add<T>(this T[] target, T item)
    {
        if (target == null)
        {
            return null;
        }
        T[] result = new T[target.Length + 1];
        target.CopyTo(result, 0);
        result[target.Length] = item;
        return result;
    }

    public static T GetOnce<T>(this T[] target)
    {
        var i = Random.Range(0, target.Length);
        return target[i];
    }
}
