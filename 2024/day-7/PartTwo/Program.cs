
using System.Diagnostics;

StreamReader reader = new("../input");

List<(long Total, int[] parts)> calibrationEquations = new();
for (string? line = reader.ReadLine(); line is not null; line = reader.ReadLine())
{
    string[] parts = line.Split(": ");
    Debug.Assert(parts.Length == 2);
    calibrationEquations.Add((long.Parse(parts[0]), parts[1].Split(" ").Select(int.Parse).ToArray()));
}

long result = 0;
Operation[] operations = [Operation.Add, Operation.Multiply, Operation.Concatenate];

foreach (var (total, parts) in calibrationEquations)
{
    if (Calculate(parts, operations, parts.Length - 1).Contains(total))
        result += total;
}

Console.WriteLine(result);

List<long> Calculate(int[] parts, Operation[] operations, int index)
{
    if (index == 0)
        return [parts[index],];
    var partials = Calculate(parts, operations, index - 1);
    List<long> result = new(capacity: partials.Count * 2);
    foreach (var partial in partials)
    {
        foreach (var operation in operations)
        {
            if (operation == Operation.Add)
                result.Add(parts[index] + partial);
            else if (operation == Operation.Multiply)
                result.Add(parts[index] * partial);
            else
                result.Add(long.Parse($"{partial}{parts[index]}"));
        }
    }
    return result;
}

enum Operation { Add, Multiply, Concatenate }