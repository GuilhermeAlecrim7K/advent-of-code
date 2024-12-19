using System.Diagnostics;
using System.Text.RegularExpressions;

var configurations = ParseInput("../input");

int totalCost = 0;
foreach (var configuration in configurations)
{
    totalCost += CalculateCheapestPressesTokenCost(configuration);
}
Console.WriteLine(totalCost);

int CalculateCheapestPressesTokenCost(Configuration configuration)
{
    int result = int.MaxValue;

    const int MAX_PRESSES = 100;
    for (int bPresses = MAX_PRESSES, aPresses = 0; bPresses >= 0; bPresses--)
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
    return result == int.MaxValue ? 0 : result;
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

record Button(int X, int Y, int TokenCost)
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
                return new Button(int.Parse(m.Groups[2].Value), int.Parse(m.Groups[3].Value), tokenCost);
            })
            .Single();
    }
}
record Prize(int X, int Y)
{
    public static Prize Parse(string s)
    {
        // Prize: X=8400, Y=5400
        var regex = new Regex(@"^Prize: X=(\d+), Y=(\d+)$");
        if (!regex.IsMatch(s))
            throw new Exception($"Invalid input: {s}");
        return regex.Matches(s)
            .Select(m => new Prize(int.Parse(m.Groups[1].Value), int.Parse(m.Groups[2].Value)))
            .Single();
    }
}
