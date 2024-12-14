var plot = ParseInput("../input");
const char fenced = '#';
const char markedForFencing = '|';

uint result = 0;
for (int i = 0; i < plot.Length; i++)
{
    for (int j = 0; j < plot[i].Length; j++)
    {
        if (plot[i][j] == fenced)
            continue;
        (uint area, uint perimeter) = CalculateFence(plot, i, j);
        result += area * perimeter;
        FenceArea(plot, i, j);
    }
}
Console.WriteLine($"Result: {result}");

(uint area, uint perimeter) CalculateFence(char[][] plot, int i, int j)
{
    (int i, int j)[] directions = [(-1, 0), (1, 0), (0, -1), (0, 1)];
    char[] accepted = [markedForFencing, plot[i][j]];
    plot[i][j] = markedForFencing;
    uint resultPerimeter = 4;
    uint resultArea = 1;
    foreach (var direction in directions)
    {
        var newI = i + direction.i;
        var newJ = j + direction.j;
        if (newI < 0 || newI >= plot.Length || newJ < 0 || newJ >= plot.Length || !accepted.Contains(plot[newI][newJ]))
            continue;

        if (accepted.Contains(plot[newI][newJ]))
            resultPerimeter--;
        if (plot[newI][newJ] == markedForFencing)
            continue;
        (uint area, uint perimeter) = CalculateFence(plot, newI, newJ);
        resultArea += area;
        resultPerimeter += perimeter;
    }
    return (resultArea, resultPerimeter);
}

void FenceArea(char[][] plot, int i, int j)
{
    plot[i][j] = fenced;
    (int i, int j)[] directions = [(-1, 0), (1, 0), (0, -1), (0, 1)];
    foreach (var direction in directions)
    {
        var newI = i + direction.i;
        var newJ = j + direction.j;
        if (newI < 0 || newI >= plot.Length || newJ < 0 || newJ >= plot.Length || plot[newI][newJ] != markedForFencing)
            continue;
        FenceArea(plot, newI, newJ);
    }
}

char[][] ParseInput(string path)
{
    return File.ReadAllLines(path)
        .Select(line => line.ToCharArray())
        .ToArray();
}