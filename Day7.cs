using AdventOfCode;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

public class Program
{
    static string[] possibleSignCombinations;
    static string possibleSigns = "+*_";
    public static void Main(string[] args)
    {
        // Custom class to read files
        string[] eqs = ReadFile.ReadFromFile();
        long sum = 0;
        foreach(string eq in eqs)
        {
            Regex regex = new Regex(@"\d+");

            MatchCollection matches = regex.Matches(eq);

           long [] numbers = new long[matches.Count];

            for (int i = 0; i < matches.Count; i++)
            {
                numbers[i] = long.Parse(matches[i].Value);
            }

            possibleSignCombinations = new string[(uint)Math.Pow(3, numbers.Length - 2)];
            GenerateSignCombinations(numbers.Length - 2);  

            Stack<long> stack;
            for (int j = 0; j < possibleSignCombinations.Length; j++)
            {

                stack = new Stack<long>();
                int signPos = 0;
                for (int i = 1; i < numbers.Length; i++)
                {
                    if(stack.Count < 2)
                    {
                        stack.Push(numbers[i]);
                    }
                    else
                    {
                        long c = stack.Pop();
                        long d = stack.Pop();
                        stack.Push(Op(c, d, possibleSignCombinations[j][signPos++]));
                        i--;
                    }

                }

                if(stack.Count == 2)
                {
                    long a = stack.Pop();
                    long b = stack.Pop();
                    stack.Push(Op(a, b, possibleSignCombinations[j][signPos]));
                }
                long result  = stack.Pop();

                if(result == numbers[0])
                {
                    sum += result;
                    break;
                }
            }

        }

        Console.WriteLine(sum);


    }

    // 3 possible signs

    public static void GenerateSignCombinations(int n)
    {
        for (int i = 0; i < possibleSignCombinations.Length; i++)
        {
            string ternary = ToTrenary(i).PadLeft(n, '0');
            string signCombination = "";
            for (int j = 0; j < ternary.Length; j++)
            {
                signCombination += possibleSigns[ternary[j] - '0'];
            }
            possibleSignCombinations[i] = signCombination;
        }
    }

    public static String ToTrenary(int value)
    {
        if (value == 0)
            return "";

        StringBuilder Sb = new StringBuilder();
        Boolean signed = false;

        if (value < 0)
        {
            signed = true;
            value = -value;
        }

        while (value > 0)
        {
            Sb.Insert(0, value % 3);
            value /= 3;
        }

        if (signed)
            Sb.Insert(0, '-');

        return Sb.ToString();
    }

    // concat operator || added

    public static long Op(long a, long b, char op)
    {
        if(op == '+')
        {
            return a + b;
        }
        else if(op == '*')
        {
            return a * b;
        }
        else
        {
            return long.Parse(b.ToString() + a.ToString());
        }
    }
}
