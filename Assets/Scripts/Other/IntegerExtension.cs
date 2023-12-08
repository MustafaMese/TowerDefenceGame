using System;

public static class IntegerExtension
{
    public static char[] ConvertToCharArray(this int value)
    {
        int numberOfDigits = (int)Math.Floor(Math.Log10(value) + 1);
        
        char[] charArray = new char[numberOfDigits];
        
        for (int i = numberOfDigits - 1; i >= 0; i--, value /= 10)
            charArray[i] = (char)(value % 10 + '0');
        
        return charArray;
    }
}