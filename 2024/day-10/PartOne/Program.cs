var map = ParseInput("../input");
int result = 0;

for (int i = 0; i < map.Length; i++)
{
    for (int j = 0; j < map[i].Length; j++)
    {
        if (map[i][j] != 0)
            continue;
        result += ParseTrails(map, i, j, 0).Distinct().Count();
    }
}
Console.WriteLine(result);

int[][] ParseInput(string path)
{
    StreamReader reader = new(path);
    List<int[]> map = new();
    for (string? line = reader.ReadLine(); line is not null; line = reader.ReadLine())
        map.Add(line.ToCharArray().Select(c => c - '0').ToArray());
    return [.. map];
}

IEnumerable<(int i, int j)> ParseTrails(int[][] map, int i, int j, int value)
{
    if (value == 9)
        yield return (i, j);
    (int, int)[] directions = [(0, 1), (0, -1), (1, 0), (-1, 0)];
    List<(int i, int j)> result = [];
    foreach ((int x, int y) in directions)
    {
        int newI = i + x;
        int newJ = j + y;
        if (newI < 0 || newI >= map.Length || newJ < 0 || newJ >= map[newI].Length || map[newI][newJ] != value + 1)
            continue;
        result.AddRange(ParseTrails(map, newI, newJ, value + 1));
    }
    foreach ((int i, int j) item in result)
        yield return item;
}