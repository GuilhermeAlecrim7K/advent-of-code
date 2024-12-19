
using System.Text.RegularExpressions;

Int64 result = 0;
Regex regex = new(@"mul\((\d{1,3}),(\d{1,3})\)");
StreamReader reader = new("../input");
for (string? line = reader.ReadLine(); line != null; line = reader.ReadLine())
{
    foreach (Match match in regex.Matches(line))
    {
        // Console.WriteLine($"{match.Value} {match.Groups[1].Value} {match.Groups[2].Value}");
        result += Int64.Parse(match.Groups[1].Value) * Int64.Parse(match.Groups[2].Value);
    }

}
Console.WriteLine(result);
