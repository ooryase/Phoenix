using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scroll.GameSystem
{
    class CSVReader
    {

        public CSVReader()
        {
        }

        public int[,] GetIntMatrix(string filename, string path = "./")
        {
            var stringData = Read(filename, path);

            return ToIntMatrix(ToIntData(ToArrayData(stringData)));
        }


        private List<string[]> Read(string filename, string path)
        {
            List<string[]> stringData = new List<string[]>();

            //@"Content/" +
 //           try
 //           {
                using (var sr = new System.IO.StreamReader(@"Content" + path + filename))
                {
                    while (!sr.EndOfStream)
                    {
                        var line = sr.ReadLine();

                        var values = line.Split(',');

                        stringData.Add(values);
                    }
                }
            /*}
            catch (SystemException e)
            {
                System.Console.WriteLine(e.Message);
            }*/

            return stringData;
        }


        private string[][] ToArrayData(List<string[]> stringData)
        {
            return stringData.ToArray();
        }

        private int[][] ToIntData(string[][] data)
        {
            int row = data.Count();
            var intData = new int[row][];
            for (int i = 0; i < row; i++)
            {
                int colum = data[i].Count();
                intData[i] = new int[colum];
            }

            for (int y = 0; y < row; y++)
            {
                for (int x = 0; x < intData[y].Count(); x++)
                {
                    intData[y][x] = int.Parse(data[y][x]);
                }
            }

            return intData;
        }


        private int[,] ToIntMatrix(int[][] data)
        {
            int row = data.Count();
            int colum = data[0].Count();

            var intData = new int[row, colum];

            for (int y = 0; y < row; y++)
            {
                for (int x = 0; x < colum; x++)
                {
                    intData[y, x] = data[y][x];
                }
            }

            return intData;
        }

    }
}
