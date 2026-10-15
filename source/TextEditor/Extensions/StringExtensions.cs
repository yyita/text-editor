namespace TextEditor;

public static class StringExtensions
{
    public static Int32 CountLeadingSpaces(this String str)
    {
        int counter = 0;
        for (int index = 0;  index < str.Length;  index++)
        {
            var character = str[index];
            if (character == ' ')
                counter++;
            else break;
        }
        return counter;
    }
    public static Int32 CountTrailingSpaces(this String str)
    {
        int counter = 0;
        for (int index = str.Length - 1;  index >= 0;  index--)
        {
            var character = str[index];
            if (character == ' ')
                counter++;
            else break;
        }
        return counter;
    }
}