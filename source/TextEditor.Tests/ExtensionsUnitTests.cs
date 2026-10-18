using System.Diagnostics;

namespace TextEditor.Tests;

public static class StringExtensionsTestCases
{
    public static void Test()
    {
        Test_CountLeadingSpaces();
        Test_CountTrailingSpaces();
    }
    public static void Test_CountLeadingSpaces()
    {
        Debug.Assert(typeof(TextEditor.StringExtensions).GetMethod("CountLeadingSpaces") != null);
        Debug.Assert("    ".CountLeadingSpaces() == 4);
    }
    public static void Test_CountTrailingSpaces()
    {
        Debug.Assert(typeof(TextEditor.StringExtensions).GetMethod("CountTrailingSpaces") != null);
        Debug.Assert("    ".CountTrailingSpaces() == 4);
    }
    public static void Test_CountAdjoiningSpaces()
    {
        Debug.Assert(typeof(TextEditor.StringExtensions).GetMethod("CountAdjoiningSpaces") != null);
        Debug.Assert("        ".CountAdjoiningSpaces(4) == 8);
    }
}