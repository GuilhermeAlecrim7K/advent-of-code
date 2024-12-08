
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
    if (!ok)
    {
        result += CorrectPagesAndGetMiddleElement(pages, rules);
    }
}

//Expected output: ?
Console.WriteLine(result);

int CorrectPagesAndGetMiddleElement(int[] pages, List<int>?[] rules)
{
    MutableKeyValuePair<int, int>[] pairs = new MutableKeyValuePair<int, int>[pages.Length];
    for (int i = 0; i < pages.Length; i++)
    {
        pairs[i] = new MutableKeyValuePair<int, int>(pages[i], 0);
        foreach (int page in pages)
        {
            if (page == pages[i]) continue;
            if (rules[page]?.Contains(pages[i]) ?? false)
                pairs[i].Value++;
        }
    }
    Array.Sort(pairs, (a, b) => a.Value - b.Value);
    return pairs[pairs.Length >> 1].Key;
}

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
public class MutableKeyValuePair<TKey, TValue>
{
    public TKey Key { get; }
    public TValue Value { get; set; }

    public MutableKeyValuePair(TKey key, TValue value)
    {
        Key = key;
        Value = value;
    }
}
