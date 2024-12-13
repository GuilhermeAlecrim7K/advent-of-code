var antennas = LoadInput("../input");
var antennaPositions = LoadPositions(antennas);
int result = 0;

foreach (var positions in antennaPositions.Values)
{
    for (int i = 0; i < positions.Count; i++)
    {
        for (int j = i + 1; j < positions.Count; j++)
        {
            var iDiff = Math.Abs(positions[i].i - positions[j].i);
            var jDiff = Math.Abs(positions[i].j - positions[j].j);
            int leftMostI, rightMostI;
            if (positions[i].j < positions[j].j)
            {
                leftMostI = positions[i].i - iDiff;
                rightMostI = positions[j].i + iDiff;
            }
            else
            {
                leftMostI = positions[j].i + iDiff;
                rightMostI = positions[i].i - iDiff;
            }
            (int i, int j) leftMost = (leftMostI, Math.Min(positions[i].j, positions[j].j) - jDiff);
            (int i, int j) rightMost = (rightMostI, Math.Max(positions[i].j, positions[j].j) + jDiff);

            if (WithinBounds(antennas, leftMost.i, leftMost.j) && antennas[leftMost.i][leftMost.j] != '#')
            {
                antennas[leftMost.i][leftMost.j] = '#';
                result++;
            }
            if (WithinBounds(antennas, rightMost.i, rightMost.j) && antennas[rightMost.i][rightMost.j] != '#')
            {
                antennas[rightMost.i][rightMost.j] = '#';
                result++;
            }
        }
    }
}

Console.WriteLine(result);

bool WithinBounds(char[][] antennas, int i, int j)
{
    return i >= 0 && i < antennas.Length && j >= 0 && j < antennas[i].Length;
}

char[][] LoadInput(string path)
{
    StreamReader reader = new(path);
    var lines = reader.ReadToEnd().Split("\n");
    char[][] antennas = new char[lines.Length][];
    for (int i = 0; i < lines.Length; i++)
    {
        antennas[i] = new char[lines[i].Length];
        for (int j = 0; j < lines[i].Length; j++)
        {
            antennas[i][j] = lines[i][j];
        }
    }
    return antennas;
}

Dictionary<char, List<(int i, int j)>> LoadPositions(char[][] antennas)
{
    Dictionary<char, List<(int i, int j)>> result = new();
    for (int i = 0; i < antennas.Length; i++)
    {
        for (int j = 0; j < antennas[i].Length; j++)
        {
            switch (antennas[i][j])
            {
                case char c when (c >= 'A' && c <= 'Z')
                    || (c >= 'a' && c <= 'z')
                    || (c >= '0' && c <= '9'):
                    result.TryGetValue(c, out var positions);
                    positions ??= [];
                    positions.Add((i, j));
                    result[c] = positions;
                    break;
            }
        }
    }
    return result;

}