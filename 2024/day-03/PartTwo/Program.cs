using System.Text.RegularExpressions;

Int64 result = 0;
Regex mulRegex = new(@"mul\((\d{1,3}),(\d{1,3})\)");

StreamReader reader = new("../input");
int nextDo = 0, nextDont = -1;
for (string? line = reader.ReadLine(); line is not null; line = reader.ReadLine())
{
    if (nextDont > nextDo)
        nextDo = line.IndexOf("do()", nextDont);
    while (nextDo >= 0)
    {
        nextDont = line.IndexOf("don't()", nextDo);
        if (nextDont == -1)
        {
            nextDont = line.Length;
        }
        foreach (Match match in mulRegex.Matches(line.Substring(nextDo, nextDont - nextDo)))
        {
            // Console.WriteLine($"{match.Value} {match.Groups[1].Value} {match.Groups[2].Value}");
            result += Int64.Parse(match.Groups[1].Value) * Int64.Parse(match.Groups[2].Value);
        }
        nextDo = line.IndexOf("do()", nextDont);
    }
    // Means that the line finished with a dont and the starting point for next line must be a do
    if (nextDont < line.Length)
    {
        nextDo = 0;
        nextDont = 1;
    }
}
Console.WriteLine(result);
