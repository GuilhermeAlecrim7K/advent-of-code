StreamReader reader = new StreamReader("../input");

List<int> locations1 = new(), locations2 = new();
while (true)
{
    string? line = reader.ReadLine();
    if (line is null)
        break;
    var values = line.Split("   ");
    locations1.Add(int.Parse(values[0]));
    locations2.Add(int.Parse(values[1]));
}
// locations1 = [3, 4, 2, 1, 3, 3];
// locations2 = [4, 3, 5, 3, 9, 3];
locations1.Sort();
locations2.Sort();

int result = 0;
int i2 = 0;
int prevN = locations1[0] - 1;
int prevSum = 0;
foreach (var n in locations1)
{
    if (n == prevN)
        result += prevSum * n;
    prevSum = 0;
    for (int i = i2; i < locations2.Count && locations2[i] <= n; i++)
    {
        if (locations2[i] == n)
            prevSum++;
    }
    result += prevSum * n;
    // Console.WriteLine($"{n} {prevSum}");
}
Console.WriteLine(result);
