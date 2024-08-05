using System.IO;

public static class CsvHelper
{
    public static int[,] LoadCsv(string filePath)
    {
        var lines = File.ReadAllLines(filePath);
        var width = lines[0].Split(',').Length;
        var height = lines.Length;

        var result = new int[width, height];

        for (var y = 0; y < height; y++)
        {
            var values = lines[y].Split(',');
            for (var x = 0; x < width; x++)
            {
                result[x, y] = int.Parse(values[x]);
            }
        }

        return result;
    }
}
