using System.Diagnostics;
using Boolean = System.Boolean;

namespace NumberPortability;

internal class Program
{
    private static int numberCount = 10;
    private static Dictionary<string, string> portedNumbers = new();

    static void Main(string[] args)
    {

        Boolean isported = false;
        var numbersToTest = NumberGenerator.GenerateMobileNumbers(numberCount);
        
        // Memory usage before cache building
        long memoryBeforeCache = GC.GetTotalMemory(true);
        var buildTime = Stopwatch.StartNew();
        BuildCache("numbers.txt");
        buildTime.Stop();
        // Memory usage after cache building
        long memoryAfterCache = GC.GetTotalMemory(true);
        long memoryUsedInMB = (memoryAfterCache - memoryBeforeCache) / (1024 * 1024); // Convert bytes to MB

        Console.WriteLine($"Time to build cache: {buildTime.ElapsedMilliseconds}ms");
        Console.WriteLine($"Memory used by cache: {memoryUsedInMB} MB");
        Console.WriteLine($"Time to build cache: {buildTime.ElapsedMilliseconds}ms");
        
        var checkTime = Stopwatch.StartNew();
        foreach (var number in numbersToTest)
        {
            Console.WriteLine($"The number is {number} and the service provider is { IdentifyNetwork(number, out var ported)}  and port status:{ported}");
            // IdentifyNetwork(number, out var ported);
        }
        
        checkTime.Stop();
        Console.WriteLine($"Time to check {numberCount} numbers: {checkTime.ElapsedMilliseconds}ms");


        
    }

    public static void BuildCache(string filePath)
    {
        using (StreamReader file = new StreamReader(filePath))
        {
            string line;
            while ((line = file.ReadLine()) != null)
            {
                String number = line.Split(",")[0];
                String network = line.Split(",")[1];
                int positionofSlash = network.IndexOf("-") + 1;
                String portedNumber = network.Substring(0, positionofSlash);
                if (portedNumber.Equals("P-"))
                {
                    network = network.Substring(positionofSlash, network.Length - (positionofSlash));
                    portedNumbers.Add(number, network);
                }
            }
        }
    }

    public static void SaveCache(string filePath)
    {
        using (StreamWriter writetext = new StreamWriter(filePath))
        {

            foreach (var kvp in portedNumbers)
            {
                writetext.WriteLine(kvp.Key+","+kvp.Value);
            }
        }
    }

    //just want to be able to load from the created file
    public static void LoadCache(string filePath) {
        Console.WriteLine("Loading from Stored File");
        using (StreamReader file = new StreamReader(filePath))
        { 
            string line;
            while ((line = file.ReadLine()) != null)
            {
                Console.WriteLine(line);
            }
        }

        Console.WriteLine($"Read from Stored File: {filePath}");
    }

    public static string IdentifyNetwork(string destination, out bool ported)
    {
        ported = false;
        if (portedNumbers.ContainsKey(destination))
        {
            ported = true;
            return portedNumbers[destination];
        }
        
        //really does nothing just satisfying the method, but method should have been a void function 
        return destination;

    }
}