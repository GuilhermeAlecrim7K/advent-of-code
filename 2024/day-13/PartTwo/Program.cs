using System.Diagnostics;
using System.Text.RegularExpressions;

var configurations = ParseInput("../input");

long totalCost = 0;
int progress = 0;
foreach (var configuration in configurations)
{
    totalCost += CalculateCheapestPressesTokenCost(configuration);
    Console.WriteLine($"Progress: {progress++} => {totalCost}");
}
Console.WriteLine(totalCost);

long CalculateCheapestPressesTokenCost(Configuration configuration)
{
    long result = long.MaxValue;

    long MAX_B_PRESSES;
    // ! This was a poor attempt to optimize the solution. Did not work. Currently stuck.
    var lcmPresses = LCM(configuration.ButtonA.X, configuration.ButtonB.X) / configuration.ButtonB.X;
    if (configuration.ButtonB.X > configuration.ButtonB.Y)
        MAX_B_PRESSES = configuration.PrizeLocation.X / configuration.ButtonB.X + 1;
    else
        MAX_B_PRESSES = configuration.PrizeLocation.Y / configuration.ButtonB.Y + 1;

    for (long bPresses = MAX_B_PRESSES, aPresses = 0; bPresses >= 0; bPresses -= lcmPresses)
    {
        if (bPresses * configuration.ButtonB.X > configuration.PrizeLocation.X)
            continue;
        if (bPresses * configuration.ButtonB.Y > configuration.PrizeLocation.Y)
            continue;
        if ((configuration.PrizeLocation.Y - bPresses * configuration.ButtonB.Y) % configuration.ButtonA.Y != 0)
            continue;

        aPresses = (configuration.PrizeLocation.Y - bPresses * configuration.ButtonB.Y) / configuration.ButtonA.Y;
        if ((configuration.PrizeLocation.X - bPresses * configuration.ButtonB.X) != aPresses * configuration.ButtonA.X)
            continue;

        result = Math.Min(result, aPresses * configuration.ButtonA.TokenCost + bPresses * configuration.ButtonB.TokenCost);
        Debug.Assert(aPresses * configuration.ButtonA.X + bPresses * configuration.ButtonB.X == configuration.PrizeLocation.X, $"X -> A.X={aPresses * configuration.ButtonA.X}, B.X={bPresses * configuration.ButtonB.X}");
        Debug.Assert(aPresses * configuration.ButtonA.Y + bPresses * configuration.ButtonB.Y == configuration.PrizeLocation.Y, "Y");
    }
    Console.WriteLine("  " + result);
    return result == long.MaxValue ? 0 : result;
}

long LCM(long a, long b)
{
    var result = a * b / GCD(a, b);
    Console.WriteLine($"LCM({a}, {b}) = {result}");
    return result;
}

long GCD(long a, long b)
{
    if (a == 0)
        return b;
    return GCD(b % a, a);
}

IEnumerable<Configuration> ParseInput(string path)
{
    var input = File
        .ReadAllText(path)
        .Split("\n\n")
        .Select(configurations =>
        {
            var s = configurations.Trim().Split("\n");
            if (s.Length != 3)
                throw new Exception($"Invalid input: {configurations}");
            return new Configuration(
                Button.Parse(s[0]),
                Button.Parse(s[1]),
                Prize.Parse(s[2])
            );
        })
        ;

    return input;
}

record Configuration(Button ButtonA, Button ButtonB, Prize PrizeLocation);

record Button(long X, long Y, int TokenCost)
{
    public static Button Parse(string s)
    {
        // Button A: X+94 Y+34
        var regex = new Regex(@"^Button (\w): X\+(\d+), Y\+(\d+)$");
        if (!regex.IsMatch(s))
            throw new Exception($"Invalid input: {s}");
        return regex.Matches(s)
            .Select(m =>
            {
                if (m.Groups[1].Value != "A" && m.Groups[1].Value != "B")
                    throw new Exception($"Invalid input: {s}");
                var tokenCost = 0;
                if (m.Groups[1].Value == "A")
                    tokenCost = 3;
                else if (m.Groups[1].Value == "B")
                    tokenCost = 1;
                return new Button(long.Parse(m.Groups[2].Value), long.Parse(m.Groups[3].Value), tokenCost);
            })
            .Single();
    }
}
record Prize(long X, long Y)
{
    public static Prize Parse(string s)
    {
        // Prize: X=8400, Y=5400
        var regex = new Regex(@"^Prize: X=(\d+), Y=(\d+)$");
        if (!regex.IsMatch(s))
            throw new Exception($"Invalid input: {s}");
        return regex.Matches(s)
            .Select(m => new Prize(
                10000000000000 + long.Parse(m.Groups[1].Value),
                10000000000000 + long.Parse(m.Groups[2].Value)))
            .Single();
    }
}
