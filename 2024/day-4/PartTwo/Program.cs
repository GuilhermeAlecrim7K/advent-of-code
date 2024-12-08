using System.Text.RegularExpressions;

string[] targets = [
    new("M.M|.A.|S.S"),
    new("M.S|.A.|M.S"),
    new("S.S|.A.|M.M"),
    new("S.M|.A.|S.M"),
];

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

for (int i = 0; i < lines.Count - 2; i++)
{
    for (int j = 0; j < lines[i].Length - 2; j++)
    {
        string slice = $@"{lines[i][j]}.{lines[i][j + 2]}|.{lines[i + 1][j + 1]}.|{lines[i + 2][j]}.{lines[i + 2][j + 2]}";
        foreach (string target in targets)
        {
            if (slice == target)
            {
                result++;
                break;
            }
        }
    }
}
Console.WriteLine(result);
