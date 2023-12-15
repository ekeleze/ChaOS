using System;
using System.Linq;
using System.Xml;

namespace ChaOS.Apps;

public class Calc
{
    public static string Single(string equation)
    {
        string finalReturn = Calculate(equation);

        return finalReturn;
    }

    // TODO: This idea will be scrapped for now
    // TODO: While it isn't hard i dont have the time to implement a smooth way to implement it UI side
    /*public static string[] Multiple(string[] equations)
    {
        string[] finalReturn = null;

        return finalReturn;
    }*/

    private static string Calculate(string equation)
    {
        
        
        string finalReturn = equation.Replace(" ", "");
        bool done = false;

        while (!done)
        {
            // 9+9*8
            // What to extract first: 9*8
            
            /*  PEMDAS
             *  Parentheses
             *  Exponents
             *  Multiplication & Division (Left to right)
             *  Addition & Subtraction (Left to right)
             */

            // *, /, (), ^, +, -

            string[] eqLayers = { finalReturn };

            if (equation.Contains('(') && equation.Contains(')'))
            {
                // handle parentheses 
            }
            if (!equation.Contains('*') && !equation.Contains('/') && !equation.Contains('^') &&
                !equation.Contains('+') &&
                !equation.Contains('-'))
            {
                done = true;
            }
            else
            {
                switch (finalReturn)
                {
                    case { } a when a.Contains('^'):
                        string[] exponentEqNums = LocalEquation('^', finalReturn);
                        string exponentEq = exponentEqNums[0] + '^' + exponentEqNums[1];
                        
                        int exnumber1 = int.Parse(exponentEqNums[0]);
                        
                        int exnumber2 = int.Parse(exponentEqNums[1]);

                        string exresult = (exnumber1 ^ exnumber2).ToString();

                        finalReturn.Replace(exponentEq, exresult);
                        
                        break;
                    case { } a when a.Contains('*'):
                        string[] muEqNums = LocalEquation('*', finalReturn);
                        string muEq = muEqNums[0] + '*' + muEqNums[1];
                        
                        int munumber1 = int.Parse(muEqNums[0]);
                        
                        int munumber2 = int.Parse(muEqNums[1]);

                        string muresult = (munumber1 ^ munumber2).ToString();

                        finalReturn.Replace(muEq, muresult);
                        
                        break;
                    case { } a when a.Contains('/'):
                        string[] diEqNums = LocalEquation('/', finalReturn);
                        string diEq = diEqNums[0] + '/' + diEqNums[1];
                        
                        int dinumber1 = int.Parse(diEqNums[0]);
                        
                        int dinumber2 = int.Parse(diEqNums[1]);

                        string diresult = (dinumber1 ^ dinumber2).ToString();

                        finalReturn.Replace(diEq, diresult);
                        
                        break;
                    case { } a when a.Contains('+'):
                        string[] adEqNums = LocalEquation('+', finalReturn);
                        string adEq = adEqNums[0] + '+' + adEqNums[1];
                        
                        int adnumber1 = int.Parse(adEqNums[0]);
                        
                        int adnumber2 = int.Parse(adEqNums[1]);

                        string adresult = (adnumber1 ^ adnumber2).ToString();

                        finalReturn.Replace(adEq, adresult);

                        break;
                    case { } a when a.Contains('-'):
                        string[] suEqNums = LocalEquation('*', finalReturn);
                        string suEq = suEqNums[0] + '^' + suEqNums[1];
                        
                        int sunumber1 = int.Parse(suEqNums[0]);
                        
                        int sunumber2 = int.Parse(suEqNums[1]);

                        string suresult = (sunumber1 ^ sunumber2).ToString();

                        finalReturn.Replace(suEq, suresult);
                        
                        break;
                }
            }
        }

        return finalReturn;
    }

    private static string[] LocalEquation(char eqChar, string originalEq)
    {
        char[] numbers = { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9' };

        string finalReturn = eqChar.ToString();

        int location = originalEq.IndexOf(eqChar);
        int start = location - 1;

        char[] ogeq = originalEq.ToCharArray();
                        
        // Hunt backwards
        for (int i = location; i >= start; i--)
        {
            if (numbers.Contains(ogeq[i]))
            {
                finalReturn = ogeq[i] + finalReturn;
                start--;
            }
        }

        start = location + 1;
                        
        // Hunt forwards
        for (int i = location; i <= start; i++)
        {
            if (numbers.Contains(ogeq[i]))
            {
                finalReturn += ogeq[i];
                start++;
            }
        }

        return finalReturn.Split(eqChar);
    }
}