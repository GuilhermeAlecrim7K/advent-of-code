
string target = "XMAS";
int result = 0;
List<char[]> lines = new();

StreamReader reader = new("../input");
for (string? line = reader.ReadLine(); line is not null; line = reader.ReadLine())
{
    lines.Add(line.ToCharArray());
}

// lines = new();
// lines.Add("MMMSXXMASM".ToCharArray());
// lines.Add("MSAMXMSMSA".ToCharArray());
// lines.Add("AMXSXMAAMM".ToCharArray());
// lines.Add("MSAMASMSMX".ToCharArray());
// lines.Add("XMASAMXAMM".ToCharArray());
// lines.Add("XXAMMXXAMA".ToCharArray());
// lines.Add("SMSMSASXSS".ToCharArray());
// lines.Add("SAXAMASAAA".ToCharArray());
// lines.Add("MAMMMXMMMM".ToCharArray());
// lines.Add("MXMXAXMASX".ToCharArray());

for (int i = 0; i < lines.Count; i++)
{
    for (int j = 0; j < lines[i].Length; j++)
    {
        if (VerticalDown(lines, i, j)) result++;
        if (VerticalUp(lines, i, j)) result++;
        if (HorizontalRight(lines, i, j)) result++;
        if (HorizontalLeft(lines, i, j)) result++;
        if (DiagonalDownRight(lines, i, j)) result++;
        if (DiagonalDownLeft(lines, i, j)) result++;
        if (DiagonalUpRight(lines, i, j)) result++;
        if (DiagonalUpLeft(lines, i, j)) result++;
    }
}
Console.WriteLine(result);

bool VerticalDown(List<char[]> lines, int i, int j)
{
    if (i + target.Length > lines.Count) return false;
    for (int k = i; k < i + target.Length; k++)
    {
        if (lines[k].Length < j) return false;
    }
    for (int k = 0; k < target.Length; k++)
    {
        if (lines[i + k][j] != target[k]) return false;
    }
    return true;
}

bool VerticalUp(List<char[]> lines, int i, int j)
{
    if (i + 1 - target.Length < 0) return false;
    for (int k = i; k >= i + 1 - target.Length; k--)
    {
        if (lines[k].Length < j) return false;
    }
    for (int k = 0; k < target.Length; k++)
    {
        if (lines[i - k][j] != target[k]) return false;
    }
    return true;
}

bool HorizontalRight(List<char[]> lines, int i, int j)
{
    if (j + target.Length > lines[i].Length) return false;
    for (int k = 0; k < target.Length; k++)
    {
        if (lines[i][j + k] != target[k]) return false;
    }
    return true;
}

bool HorizontalLeft(List<char[]> lines, int i, int j)
{
    if (j + 1 - target.Length < 0) return false;
    for (int k = 0; k < target.Length; k++)
    {
        if (lines[i][j - k] != target[k]) return false;
    }
    return true;
}

bool DiagonalDownRight(List<char[]> lines, int i, int j)
{
    for (int k = 0; k < target.Length; k++)
    {
        if (i + k >= lines.Count || j + k >= lines[i].Length) return false;
        if (lines[i + k][j + k] != target[k]) return false;
    }
    return true;
}

bool DiagonalDownLeft(List<char[]> lines, int i, int j)
{
    for (int k = 0; k < target.Length; k++)
    {
        if (i - k < 0 || j - k < 0) return false;
        if (lines[i - k][j - k] != target[k]) return false;
    }
    return true;
}

bool DiagonalUpRight(List<char[]> lines, int i, int j)
{
    for (int k = 0; k < target.Length; k++)
    {
        if (i - k < 0 || j + k >= lines[i].Length) return false;
        if (lines[i - k][j + k] != target[k]) return false;
    }
    return true;
}

bool DiagonalUpLeft(List<char[]> lines, int i, int j)
{
    for (int k = 0; k < target.Length; k++)
    {
        if (i + k >= lines.Count || j - k < 0) return false;
        if (lines[i + k][j - k] != target[k]) return false;
    }
    return true;
}
