List<long> stones = ParseInput("../input");
int blinkCount = 75;

var cache = new Dictionary<(long, int), long>();
Console.WriteLine(stones.Select(stone => ChangeStones(stone, blinkCount, cache)).Sum());

// Credits to https://www.youtube.com/@DylanBeattie for this solution
long ChangeStones(long stone, int times, Dictionary<(long, int), long> cache)
{
    if (cache.TryGetValue((stone, times), out var cachedResult))
        return cachedResult;
    if (times == 0)
        return 1;
    var result = stone switch
    {
        0 => ChangeStones(1, times - 1, cache),
        _ => stone.ToString() switch
        {
            string s when (s.Length & 1) == 0 =>
                ChangeStones(long.Parse(s[0..(s.Length >> 1)]), times - 1, cache) +
                ChangeStones(long.Parse(s[(s.Length >> 1)..]), times - 1, cache),
            _ => ChangeStones(2024 * stone, times - 1, cache),
        },
    };

    cache[(stone, times)] = result;
    return result;
}


List<long> ParseInput(string path) => (File.ReadAllText(path).Split(" ").Select(long.Parse)).ToList();
