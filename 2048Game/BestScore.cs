using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _2048Game
{
    public static class GameProfile
    {
        public static int bestScore;
        public static void Best(int score)
        {
            if (!System.IO.File.Exists("best.txt"))
            {
                using (StreamWriter sw = new StreamWriter("best.txt"))
                {
                    sw.Write(0);
                }
            }
                
            using (StreamReader sr = new StreamReader("best.txt"))
            {
                int k;
                k = int.Parse(sr.ReadLine());
                bestScore = k;
            }
            if(bestScore<score)
               bestScore = score;
            using (StreamWriter sw = new StreamWriter("best.txt"))
                {
                    sw.Write(bestScore);
                }
            }
        public static void Reset()
        {
            using (StreamWriter sw = new StreamWriter("best.txt"))
            {
                sw.Write(0);
            }
        }
        public static void saveGame(int[,] matrix,int score)
        {
                using (StreamWriter sw = new StreamWriter("savedGame.txt"))
                {
                    for (int i = 0; i < 4; i++)
                        for (int j = 0; j < 4; j++)
                            sw.WriteLine(matrix[i, j]);
                    sw.WriteLine(score);
                }
        }
        public static void deleteSaveGame()
        {
            File.Delete("savedGame.txt");
        }
    }
}
