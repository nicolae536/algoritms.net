using System;
using System.Collections.Generic;
using System.Text;

namespace algorithms.net.dynamic_programming
{
    class ASubstringOfB
    {
        public string a;
        public string b;

        public ASubstringOfB(string inputs)
        {
            string[] data = inputs.Split(' ');
            a = data[0];
            b = data[1];

            Console.WriteLine("Number of times " + CountHowManyTimesShouldABeMultitplied());
        }

        private int CountHowManyTimesShouldABeMultitplied()
        {
            int minAIndex = 0;
            while (minAIndex < a.Length)
            {
                int aStart = minAIndex;
                while (aStart >= 0 && aStart < a.Length && b[0] != a[aStart])
                {
                    aStart++;
                }

                if (aStart >= 0 && aStart < a.Length &&
                    b[0] != a[aStart] || aStart >= a.Length)
                {
                    return -1;
                }
                else
                {
                    minAIndex = aStart + 1;

                    bool isBSubstring = true;
                    int numberOfTimes = 1;

                    int i = aStart;
                    int b1Index = 0;
                    while (b1Index < b.Length && isBSubstring)
                    {
                        if (b[b1Index] == a[i])
                        {
                            b1Index++;
                            i++;

                            if (i == a.Length)
                            {
                                i = 0;
                                numberOfTimes++;
                            }
                        }
                        else
                        {
                            isBSubstring = false;
                        }
                    }

                    if (isBSubstring)
                    {
                        return numberOfTimes;
                    }
                }
            }

            return -1;
        }

        public static void RunProblem()
        {
            string[] inputs =
            {
                "abcd cdabcdab",
                "abcdabcdabcd cdabcdab",
                "abcdabcdabcd cdabcdab1",
            };

            foreach (string item in inputs)
            {
                new ASubstringOfB(item);
            }
        }
    }
}
