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

// Very verbose. Refactor next year?
(uint area, uint perimeter) CalculateFence(char[][] plot, int i, int j)
{
    (int i, int j, byte flag)[] directions = [(-1, 0, 0b1000), (1, 0, 0b0100), (0, -1, 0b0010), (0, 1, 0b0001)];
    byte neighbors = 0;
    char[] accepted = [markedForFencing, plot[i][j]];
    plot[i][j] = markedForFencing;
    uint resultPerimeter = 4;
    uint neighborsPerimeter = 0;
    uint resultArea = 1;
    foreach (var direction in directions)
    {
        var newI = i + direction.i;
        var newJ = j + direction.j;
        if (newI < 0 || newI >= plot.Length || newJ < 0 || newJ >= plot.Length || !accepted.Contains(plot[newI][newJ]))
            continue;

        if (accepted.Contains(plot[newI][newJ]))
        {
            resultPerimeter--;
            neighbors |= direction.flag;
        }
        if (plot[newI][newJ] == markedForFencing)
            continue;
        (uint area, uint perimeter) = CalculateFence(plot, newI, newJ);
        resultArea += area;
        neighborsPerimeter += perimeter;
    }
    switch (resultPerimeter)
    {
        case 4:
            resultPerimeter = 4;
            break;
        case 3:
            resultPerimeter = 2;
            break;
        // For the following cases, if there are at least two neighbors, it is verified if the neighbors form a 90 degree angle (thus causing a corner if there are exactly two neighbors) and a corner if there the element between the neighbors is not of the same type.
        case 2:
        case 1:
        case 0:
            resultPerimeter = 0;
            //Top-left
            if ((neighbors & 0b1010) == 0b1010)
            {
                if ((neighbors | 0b1010) == 0b1010)
                    resultPerimeter++;
                if (!accepted.Contains(plot[i - 1][j - 1]))
                    resultPerimeter++;
            }
            //Top-right
            if ((neighbors & 0b1001) == 0b1001)
            {
                if ((neighbors | 0b1001) == 0b1001)
                    resultPerimeter++;
                if (!accepted.Contains(plot[i - 1][j + 1]))
                    resultPerimeter++;
            }
            //Bottom-left
            if ((neighbors & 0b0110) == 0b0110)
            {
                if ((neighbors | 0b0110) == 0b0110)
                    resultPerimeter++;
                if (!accepted.Contains(plot[i + 1][j - 1]))
                    resultPerimeter++;
            }
            //Bottom-right
            if ((neighbors & 0b0101) == 0b0101)
            {
                if ((neighbors | 0b0101) == 0b0101)
                    resultPerimeter++;
                if (!accepted.Contains(plot[i + 1][j + 1]))
                    resultPerimeter++;
            }
            break;
    }
    return (resultArea, resultPerimeter + neighborsPerimeter);
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