using System;
using System.Linq;

namespace statistics
{
    class Program
    {
        static void Main(string[] args)
        {
            string[,] data = {
                {"StdNum", "Name", "Math", "Science", "English"},
                {"1001", "Alice", "85", "90", "78"},
                {"1002", "Bob", "92", "88", "84"},
                {"1003", "Charlie", "79", "85", "88"},
                {"1004", "David", "94", "76", "92"},
                {"1005", "Eve", "72", "95", "89"}
            };
            // You can convert string to double by
            // double.Parse(str)

            int stdCount = data.GetLength(0) - 1;
            // ---------- TODO ----------
            Console.WriteLine("Average Scores:");
            for (int c = 2; c < data.GetLength(1); c++){
                double sum_score = 0;
                for (int r = 1; r <= stdCount; r++){
                    sum_score += double.Parse(data[r, c]);
                }
                Console.WriteLine($"{data[0, c]}: {sum_score / stdCount:F2}");
            }
            
            Console.WriteLine("\nMax and min Scores:");
            for (int c = 2; c < data.GetLength(1); c++){
                int max = int.MinValue, min = int.MaxValue;
                for (int r = 1; r <= stdCount; r++){
                    if (max < int.Parse(data[r, c]))
                        max = int.Parse(data[r, c]);
                    if (min > int.Parse(data[r, c]))
                        min = int.Parse(data[r, c]);
                }
                Console.WriteLine($"{data[0, c]}: ({max}, {min})");
            }
            
            Console.WriteLine("\nStudents rank by total scores:");
            int[] scores = new int[stdCount];
            string[] ranking = new string[stdCount];
            for (int r = 1; r < data.GetLength(0); r++){
                for (int c = 2; c < data.GetLength(1); c++){
                    scores[r - 1] += int.Parse(data[r, c]);
                }
            }
            var indexedArr = scores
                .Select((value, index) => new { Index = index, Value = value }) // 인덱스와 값을 묶음
                .OrderBy(x => -x.Value)  // 값 기준으로 정렬
                .ToList();
                
            for (int i = 0; i < stdCount; i++)
                switch(i){
                    case 0: ranking[indexedArr[i].Index] = "1st"; break;
                    case 1: ranking[indexedArr[i].Index] = "2nd"; break;
                    case 2: ranking[indexedArr[i].Index] = "3rd"; break;
                    default: ranking[indexedArr[i].Index] = $"{i + 1}th"; break;
                }
                
            for (int i = 0; i < stdCount; i++)
                Console.WriteLine($"{data[i + 1, 1]}: {ranking[i]}");
            // --------------------
        }
    }
}

/* example output

Average Scores: 
Math: 84.40
Science: 86.80
English: 86.20

Max and min Scores: 
Math: (94, 72)
Science: (95, 76)
English: (92, 78)

Students rank by total scores:
Alice: 4th
Bob: 1st
Charlie: 5th
David: 2nd
Eve: 3rd

*/
