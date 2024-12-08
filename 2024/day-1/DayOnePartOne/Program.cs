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
locations1.Sort();
locations2.Sort();

int diff = 0;
for (int i = 0; i < locations1.Count; i++)
{
    diff += Math.Abs(locations1[i] - locations2[i]);
}
Console.WriteLine(diff);
