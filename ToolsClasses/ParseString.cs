using System;
using System.Collections.Generic;

namespace Buchalter.ToolsClasses;

public static class ParseString
{
    public class Token
    {
        public Token()
        {
            Text = string.Empty;
            Pos = -1;
        }

        public Token(string text, int pos)
        {
            Text = text;
            Pos = pos;
        }

        public override string ToString()
        {
            return Text;
        }

        public static implicit operator string(Token token)
        {
            return token.Text;
        }

        public string Text;
        public int Pos;

        public int Len
        {
            get { return Text.Length; }
        }

        public int LastPos
        {
            get { return Pos + Len; }
        }
    }

    public static Token[] Parse(Token targetToken, params char[] delimiters)
    {
        Token[] resultTokens = Parse(targetToken.Text, delimiters);

        int i;
        for (i = 0; i < resultTokens.Length; i++)
        {
            resultTokens[i].Pos += targetToken.Pos;
        }

        return resultTokens;
    }

    public static Token[] Parse(string targetString, params char[] delimiters)
    {
        bool inWord = false;
        bool inQuotation = false;
        bool hasInQuotation = false;

        if (delimiters.Length == 0)
        {
            delimiters = new char[] { ' ', '\t' };
        }

        if (string.IsNullOrEmpty(targetString))
        {
            return new Token[] { new Token(string.Empty, 0), };
        }

        List<Token> result = new List<Token>();
        result.Add(new Token());

        int resultLength = 1;

        int i;
        for (i = 0; i < targetString.Length; i++)
        {
            char currChar = targetString[i];

            if (Array.IndexOf(delimiters, currChar) == -1 || currChar == '"' || inQuotation)
            {
                if (!inWord && !inQuotation)
                {
                    result[resultLength - 1].Pos = i;
                }

                result[resultLength - 1].Text = result[resultLength - 1].Text + currChar;

                if (currChar == '"')
                {
                    hasInQuotation = inQuotation;
                    inQuotation = !inQuotation;
                }
                else
                {
                    if (!inQuotation)
                    {
                        inWord = true;
                    }
                }
            }
            else
            {
                if (inWord || hasInQuotation)
                {
                    result.Add(new Token());
                    resultLength++;

                    inWord = false;
                    hasInQuotation = false;
                }
            }
        }

        if (targetString.Length > 0 && result[resultLength - 1].Text.Length == 0)
        {
            result.RemoveAt(resultLength - 1);
        }

        return result.ToArray();
    }

    public static string[] ToArray(Token[] tokens)
    {
        string[] retValue = new string[tokens.Length];
        for (int i = 0; i < tokens.Length; i++)
        {
            retValue[i] = tokens[i].Text;
        }

        return retValue;
    }
}