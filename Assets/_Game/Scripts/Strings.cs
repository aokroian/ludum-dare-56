using System;
using UnityEngine;
using UnityEngine.Localization.Settings;
using UnityEngine.Localization.Tables;

public static class Strings
{
    // tutorial
    public const string FireControlsTutorialHint = "To Shoot";
    public const string LookControlsTutorialHint = "To Look";
    public const string MoveControlsTutorialHint = "To Walk";
    public const string MatchControlsTutorialHint = "To Light Matchstick";
    public const string MimicTutorialIntroMessage1 =
        "Now let's learn about mimics. They can disguise themselves as props";
    public const string MimicTutorialIntroMessage2 =
        "You can find them if you notice some object moved while it was dark.";
    public const string MimicTutorialIntroMessage3 =
        "Take a look at this prop. Try to remember where it is. The matchstick will go out soon.";
    public const string MimicTutorialMatchWentOutMessage = "The matchstick went out. Let's light it up again.";
    public const string MimicTutorialShootMimicMessage = "This item moved while it was dark! It's a mimic, shoot it!";

    public const string MouseSensitivityKey = "MouseSensitivity";
    public const string SoundVolumeKey = "SoundVolume";
    public const string GraphicsQualityKey = "GraphicsQuality";
    public const string LanguageKey = "Language";

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

    private static readonly string TestStringKey = "TestStringKey";

    public static void Localize(string key, Action<string> onComplete)
    {
        var locale = LocalizationSettings.SelectedLocale;
        var reference = (TableEntryReference) key;
        LocalizationSettings.StringDatabase.GetLocalizedStringAsync(reference, locale).Completed +=
            (it) => onComplete(it.Result);
    }

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