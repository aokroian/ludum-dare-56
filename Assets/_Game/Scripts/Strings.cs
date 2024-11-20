using System;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Localization.Settings;
using UnityEngine.Localization.Tables;

public static class Strings
{
    // tutorial
    public const string FireControlsTutorialHint = "FireControlsTutorialHint";
    public const string LookControlsTutorialHint = "LookControlsTutorialHint";
    public const string MoveControlsTutorialHint = "MoveControlsTutorialHint";
    public const string MatchControlsTutorialHint = "MatchControlsTutorialHint";
    public const string MimicTutorialIntroMessage1 =
        "MimicTutorialIntroMessage1";
    public const string MimicTutorialIntroMessage2 =
        "MimicTutorialIntroMessage2";
    public const string MimicTutorialIntroMessage3 =
        "MimicTutorialIntroMessage3";
    public const string MimicTutorialMatchWentOutMessage = "MimicTutorialMatchWentOutMessage";
    public const string MimicTutorialShootMimicMessage = "MimicTutorialShootMimicMessage";

    public const string MouseSensitivityKey = "MouseSensitivity";
    public const string SoundVolumeKey = "SoundVolume";
    public const string GraphicsQualityKey = "GraphicsQuality";
    public const string LanguageKey = "Language";
    
    public const string MatchesLeftZero = "MatchesLeftZero";
    public const string MatchesLeftNotZero = "MatchesLeftNotZero";
    
    public const string BulletsLeftZero = "BulletsLeftZero";
    public const string BulletsLeftNotZero = "BulletsLeftNotZero";

    private static readonly string[] NextNightMessages =
    {
        "NextNightMessages1",
        "NextNightMessages2",
        "NextNightMessages3",
    };

    private static readonly string[] FirstNightMessages =
    {
        "FirstNightMessages1",
        "FirstNightMessages2",
    };

    private static readonly string[] MatchWentOutMessages =
    {
        "MatchWentOutMessages1",
        "MatchWentOutMessages2",
    };

    private static readonly string[] HitMimicMessages =
    {
        "HitMimicMessages1",
        "HitMimicMessages2",
    };

    private static readonly string[] MissedMimicMessages =
    {
        "MissedMimicMessages1",
    };

    // private static readonly string TestStringKey = "TestStringKey";

    public static void Localize(string key, Action<string> onComplete)
    {
        var locale = LocalizationSettings.SelectedLocale;
        var reference = (TableEntryReference) key;
        LocalizationSettings.StringDatabase.GetLocalizedStringAsync(reference, locale).Completed +=
            (it) => onComplete(it.Result);
    }
    
    public static Task<string> Localize(string key)
    {
        var locale = LocalizationSettings.SelectedLocale;
        var reference = (TableEntryReference) key;
        return LocalizationSettings.StringDatabase.GetLocalizedStringAsync(reference, locale).Task;
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