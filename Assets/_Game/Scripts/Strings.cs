public static class Strings
{
    // tutorial
    
    
    public const string MouseSensitivityKey = "MouseSensitivity";
    public const string SoundVolumeKey = "SoundVolume";
    public const string GraphicsQualityKey = "GraphicsQuality";

    private static readonly string[] NextNightMessages =
    {
        "I feel a presence. Another mimic is here.",
        "I hear something. It's another mimic.",
        "I can feel it. Another mimic is here.",
    };

    private static readonly string[] FirstNightMessages =
    {
        "The mimic is hunting me. It's among the items.",
        "The mimic is hiding among the items. I need to find it.",
    };

    private static readonly string[] MatchWentOutMessages =
    {
        "The match went out. The mimic acts in the dark.",
        "I can't see anything. I need to light the match.",
    };

    private static readonly string[] HitMimicMessages =
    {
        "I shot the mimic. I can rest for now.",
        "I hit the mimic. The night is a bit safer now.",
    };

    private static readonly string[] MissedMimicMessages =
    {
        "I missed the mimic. I need to look for it better.",
    };

    public static string GetNextNightMessage()
    {
        return NextNightMessages[UnityEngine.Random.Range(0, NextNightMessages.Length)];
    }

    public static string GetFirstNightMessage()
    {
        return FirstNightMessages[UnityEngine.Random.Range(0, FirstNightMessages.Length)];
    }

    public static string GetMatchWentOutMessage()
    {
        return MatchWentOutMessages[UnityEngine.Random.Range(0, MatchWentOutMessages.Length)];
    }

    public static string GetHitEnemyMessage()
    {
        return HitMimicMessages[UnityEngine.Random.Range(0, HitMimicMessages.Length)];
    }

    public static string GetMissedEnemyMessage()
    {
        return MissedMimicMessages[UnityEngine.Random.Range(0, MissedMimicMessages.Length)];
    }
}