StreamReader reader = new("../input");

List<IEnumerable<int>> readings = new();

while (true)
{
    string? line = reader.ReadLine();
    if (line is null)
        break;
    readings.Add(line.Split(" ").Select(int.Parse));
}
// readings.Add([7, 6, 4, 2, 1]);
// readings.Add([1, 2, 7, 8, 9]);
// readings.Add([9, 7, 6, 2, 1]);
// readings.Add([1, 3, 2, 4, 5]);
// readings.Add([1, 3, 2, 4, 5]);
// readings.Add([8, 6, 4, 4, 1]);
// readings.Add([1, 3, 6, 7, 9]);

int safeReports = 0;

foreach (var reading in readings)
{
    int? previous = null;
    Order? order = null;
    bool safe = true;
    foreach (int value in reading)
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
        safeReports++;
    // Console.WriteLine($"{reading} is {(safe ? "safe" : "unsafe")}");
}

Console.WriteLine(safeReports);

enum Order { Increasing, Decreasing }
