int result = 0;
// StreamReader reader = new("../test-input");
StreamReader reader = new("../input");
List<int>?[] rules = LoadRules(reader);

for (string? line = reader.ReadLine(); line is not null; line = reader.ReadLine())
{
    int[] pages = line.Split(",").Select(int.Parse).ToArray();
    bool ok = true;
    for (int i = 0; i < pages.Length; i++)
    {
        for (int j = i + 1; j < pages.Length; j++)
        {
            if (rules[pages[j]]?.Contains(pages[i]) ?? false)
            {
                ok = false;
                break;
            }
        }
        if (!ok) break;
    }
    if (ok)
    {
        result += pages[pages.Length >> 1];
    }
}
//Expected output: 4790
Console.WriteLine(result);

List<int>?[] LoadRules(StreamReader reader)
{
    List<int>[] rules = new List<int>[100];
    for (string? line = reader.ReadLine(); line is not null && line.Length > 0; line = reader.ReadLine())
    {
        string[] rule = line.Split("|");
        int idx = int.Parse(rule[0]);
        rules[idx] ??= [];
        rules[idx].Add(int.Parse(rule[1]));
    }
    return rules;
}
