public static class Extensions
{
    public static T UnityRandom<T>(this T[] array)
    {
        return array[UnityEngine.Random.Range(0, array.Length)];
    }
}