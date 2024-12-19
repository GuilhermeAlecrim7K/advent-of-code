int safeReports = 0;

StreamReader reader = new("../input");
// StreamReader reader = new("/home/alecrim7k/development/projects/advent-of-code/day-2/PartTwo/test-input");
for (string? line = reader.ReadLine(); line is not null; line = reader.ReadLine())
{
    IEnumerable<int> readings = (line.Split(" ").Select(int.Parse));

    if (HasSafePermutation(readings.ToList()))
        safeReports++;
}
Console.WriteLine(safeReports);

bool HasSafePermutation(List<int> readings)
{
    bool safe;
    for (int i = 0; i < readings.Count; i++)
    {
        var permutation = readings.Where((_, index) => index != i);
        safe = true;
        int? previous = null;
        Order? order = null;
        foreach (var value in permutation)
        {
            if (previous is not null)
            {
                int diff = Math.Abs(value - previous.Value);
                Order curOrder = value > previous ? Order.Increasing : Order.Decreasing;
                order ??= curOrder;
                if (curOrder != order || diff > 3 || diff < 1)
                {
                    safe = false;
                    break;
                }
            }
            previous = value;
        }
        if (safe)
            return true;
    }
    return false;
}

enum Order { Increasing, Decreasing }
