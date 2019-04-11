using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VigenereAffine_Breaker
{
    class Program
    {
        private static Dictionary<char, int> dict = new Dictionary<char, int>();
        private static List<List<char>> columnList = new List<List<char>>();
        private static List<int> aKey = new List<int>();
        private static string bKey = string.Empty;
        private static string ciphertext = string.Empty;
        private static int KeySize = 6;
        static void Main(string[] args)
        {
            dict = Affine.populateDictionary();
            //KeySize goes in as a parameter (can be worked out with CryptTool)
            columnList = loadCipherText(KeySize);
            int counter = 1;
            for (int i = 0; i < columnList.Count; i++)
            {
                Console.WriteLine("---Column " + counter + " ---");
                columnConfigBreak(columnList[i]);
                counter++;
            }
            attemptDecryption(ciphertext);
            Console.Read();
        }
        private static List<List<char>> loadCipherText(int _keySize)
        {
            ciphertext = File.ReadAllText("cipher.txt").ToUpper();
            List<List<char>> columns = new List<List<char>>();
            string text = ciphertext;
            int keySize = _keySize;
            int counter = 0;
            for (int i = 0; i < keySize; i++)
            {
                columns.Add(new List<char>());
            }

            for (int i = 0; i < text.Length; i++)
            {
                columns[counter].Add(text[i]);
                if (counter < keySize-1)
                    counter++;
                else
                    counter = 0;
            }
            return columns;
        }
        private static string[] findMostFrequentElementInTheArray(List<char> list)
        {
            Dictionary<char, int> frequencyDict = new Dictionary<char, int>();
            char mostCommon = list[0];
            double occurences = 0;
            int tempOccurences = 0;
            foreach (char character in list)
            {
                if (!frequencyDict.ContainsKey(character))
                {
                    frequencyDict.Add(character, 1);
                }
                else
                {
                    tempOccurences = frequencyDict[character] + 1;
                    frequencyDict[character] = tempOccurences;
                    if (occurences < tempOccurences)
                    {
                        occurences = tempOccurences;
                        mostCommon = character;
                    }
                }
            }
            Console.WriteLine("_______________________________________________________________________________");
            Console.WriteLine("The most commmon character is " + mostCommon + " And it appears " + occurences + " times out of " + list.Count + " times");
            double percentage = (occurences / list.Count) * 100;
            Console.WriteLine("Frequency percentage: " + percentage + "%");
            int[] temp = frequencyDict.Values.ToArray();
            Array.Sort(temp);
            var myKey = frequencyDict.FirstOrDefault(x => x.Value == temp[temp.Length-2]).Key;
            Console.WriteLine("The second most commmon character is " + myKey + " And it appears " + temp[temp.Length - 2] + " times out of " + list.Count + " times");
            percentage = ((double)temp[temp.Length - 2] / list.Count) * 100;
            Console.WriteLine("Frequency percentage: " + percentage + "%");
            return new string[] { mostCommon.ToString(), myKey.ToString() };
        }
        private static void columnConfigBreak(List<char> collection)
        {
            string[] aux = findMostFrequentElementInTheArray(collection);
            List<string> combinationsCol1 = Affine.bruteForceAB("E", aux[0]);
            List<string> combinationsCol2 = Affine.bruteForceAB("T", aux[1]);
            foreach (string s in combinationsCol1.Intersect(combinationsCol2))
            {
                Console.WriteLine("-------------------------------------------------------------------------------");
                Console.WriteLine("Common configuration of A:B for two most common letters is: " + s);
                Console.WriteLine("-------------------------------------------------------------------------------");
                string a = s.Split(':')[0].Replace("A[","").Replace("]","");
                string b = s.Split(':')[1].Replace("B[", "").Replace("]", "");
                aKey.Add(Int16.Parse(a));
                bKey += b;
            }
        }
        private static void attemptDecryption(string ciphertext)
        {
            Console.WriteLine("-----------------------------------Decryption attempt-----------------------------------");
            Console.WriteLine("A Key: " + string.Join(",", aKey));
            Console.WriteLine("B Key: " + bKey);
            Console.WriteLine(Affine.decrypt(ciphertext, aKey.ToArray(), bKey, dict));

        }
    }
}
