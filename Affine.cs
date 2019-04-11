using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VigenereAffine_Breaker
{
    static class Affine
    {
        public static int[] valuesOfA = new int[12] { 1, 3, 5, 7, 9, 11, 15, 17, 19, 21, 23, 25 };
        private const string letterlist = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        public static Dictionary<char, int> populateDictionary()
        {
            Dictionary<char, int> charToValue = new Dictionary<char, int>();
            for (int i = 0; i < 26; i++)
            {
                charToValue.Add(letterlist.ElementAt(i), i);
            }
            return charToValue;
        }

        public static string encrypt(string text, int a, string k, Dictionary<char, int> charToValue)
        {
            string cipherText = "";
            string key = k.ToUpper();
            string plainText = text.ToUpper();
            int counter = 0;
            int keyLength = key.Length;
            while (key.Length < plainText.Length)
            {
                key += key.ElementAt(counter);
                counter++;
                if (counter <= keyLength)
                {
                    counter = 0;
                }
            }
            for (int j = 0; j < plainText.Length; j++)
            {
                if (!plainText.ElementAt(j).Equals(' '))
                {
                    int temp = 0;
                    int value = charToValue[plainText.ElementAt(j)];
                    int x = 0;
                    x = (a * value) + charToValue[key.ElementAt(j)];
                    int b = 26;
                    temp = ((x % b) + b) % b;
                    cipherText += letterlist.ElementAt(temp);
                }
                else
                {
                    cipherText += " ";
                }
            }

            return cipherText;
        }
        public static string encrypt(string text, int[] a, string k, Dictionary<char, int> charToValue)
        {
            int counter = 0;
            int aCounter = 0;
            string cipherText = "";
            string key = k.ToUpper();
            string plainText = text.ToUpper();
            int keyLength = key.Length;
         
            while (key.Length < plainText.Length)
            {
                key += key.ElementAt(counter);
                counter++;
                if (counter == keyLength)
                {
                    counter = 0;
                }
            }
            for (int j = 0; j < plainText.Length; j++)
            {
                if (!plainText.ElementAt(j).Equals(' '))
                {
                    if (a.Length == aCounter)
                        aCounter = 0;
                    int temp = 0;
                    int value = charToValue[plainText.ElementAt(j)];
                    int x = 0;
                    x = (a[aCounter] * value) + charToValue[key.ElementAt(j)];
                    int b = 26;
                    temp = ((x % b) + b) % b;
                    cipherText += letterlist.ElementAt(temp);
                    aCounter++;
                }
                else
                {
                    cipherText += " ";
                }
            }

            return cipherText;
        }
        public static string decrypt(string text, int a, string k, Dictionary<char, int> charToValue)
        {
            string plainText = "";
            string key = k.ToUpper();
            string cipherText = text.ToUpper();
            int counter = 0;
            int keyLength = key.Length;
            while (key.Length < cipherText.Length)
            {
                key += key.ElementAt(counter);
                counter++;
                if (counter <= keyLength)
                {
                    counter = 0;
                }
            }
            for (int j = 0; j < cipherText.Length; j++)
            {
                if (!cipherText.ElementAt(j).Equals(' '))
                {
                    int temp = 0;
                    int value = charToValue[cipherText.ElementAt(j)];
                    int x = 0;
                    x = calculateModularMultiplicativeInverse(a) * (value - charToValue[key.ElementAt(j)]);
                    int b = 26;
                    temp = ((x % b) + b) % b;
                    plainText += letterlist.ElementAt(temp);
                }
                else
                {
                    plainText += " ";
                }
            }

            return plainText;
        }
        public static string decrypt(string text, int[] a, string k, Dictionary<char, int> charToValue)
        {
            string plainText = "";
            string key = k.ToUpper();
            string cipherText = text.ToUpper();
            int counter = 0;
            int aCounter = 0;
            int keyLength = key.Length;
            while (key.Length < cipherText.Length)
            {
                key += key.ElementAt(counter);
                counter++;
                if (counter == keyLength)
                {
                    counter = 0;
                }
            }
            for (int j = 0; j < cipherText.Length; j++)
            {
                if (!cipherText.ElementAt(j).Equals(' '))
                {
                    if (a.Length == aCounter)
                        aCounter = 0;
                    int temp = 0;
                    int value = 0;
                    if (charToValue.ContainsKey(cipherText.ElementAt(j)))
                        value = charToValue[cipherText.ElementAt(j)];
                    else
                        continue;
                    int x = 0;
                    x = calculateModularMultiplicativeInverse(a[aCounter]) * (value - charToValue[key.ElementAt(j)]);
                    int b = 26;
                    temp = ((x % b) + b) % b;
                    plainText += letterlist.ElementAt(temp);
                    aCounter++;
                }
                else
                {
                    plainText += " ";
                }
            }

            return plainText;
        }

        public static List<string> bruteForceAB(string message, string expected)
        {
            Dictionary<char, int> charToValue = populateDictionary();
            List<string> matchedCombinations = new List<string>();
            for (int i = 0; i < valuesOfA.Length; i++)
            {
                for (int l = 0; l < letterlist.Length; l++)
                {
                    string temp = encrypt(message, valuesOfA[i], letterlist[l].ToString(), charToValue);
                    //Console.WriteLine("A:[ " + valuesOfA[i] + "] B:[" + letterlist.ElementAt(l) + "]" + temp);
                    if (temp.Equals(expected.ToUpper()))
                    {                      
                        //Console.WriteLine("^^^^EXPECTED LETTER MATCHED^^^^");
                        matchedCombinations.Add("A[" + valuesOfA[i] + "]:B[" + letterlist.ElementAt(l) + "]");
                    }
                }
            }
            //Console.WriteLine("Brute force complete...");
            return matchedCombinations;
        }
        private static int calculateModularMultiplicativeInverse(int a)
        {
            const int n = 26;
            double temp = 0;
            for (double i = 1; i != 0; i++)
            {
                temp = ((i * n) + 1) / a;
                if (temp % 1 == 0)
                    return Convert.ToInt32(temp);
            }
            throw new Exception();
        }
    }
}
