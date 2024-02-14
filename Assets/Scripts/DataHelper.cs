
public static class DataHelper
{

    public static bool IsStringEmpty(this string a)
    {
        return a.Trim().Length == 0;
    }

    public static string ReplaceFirstOccurrence(string Source, string Find, string Replace)
    {
        int Place = Source.IndexOf(Find);
        string result = Source.Remove(Place, Find.Length).Insert(Place, Replace);
        return result;
    }

}



