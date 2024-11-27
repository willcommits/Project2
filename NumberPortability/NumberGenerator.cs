namespace NumberPortability;

public static class NumberGenerator
{
    public static List<string> GenerateMobileNumbers(int length)
    {
        var numbers = new List<string>();
        for (var i = 0; i < length; i++)
        {
            numbers.Add(GenerateNumber());
        }

        return numbers;
    }

    private static string GenerateNumber()
    {
        var prefix = Random.Shared.Next(0, 4) switch
        {
            0 => "27747",
            1 => "27731",
            2 => "27699",
            3 => "27758",
        };

        var digits = Random.Shared.Next(0, 999_999);

        var number = digits.ToString();
        var zeros = 11 - prefix.Length - number.Length;
        var paddedPrefix = prefix;
    
        for (var i = 0; i < zeros; i++)
        {
            paddedPrefix += "0";
        }

        return $"{paddedPrefix + number}";
    }
}