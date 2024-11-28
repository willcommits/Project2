using System.Diagnostics;
using Boolean = System.Boolean;

namespace NumberPortability;

internal class Program
{
    private static int numberCount = 10;
    private static Dictionary<String, ServiceProvider> portedNumbers = new();

    static void Main(string[] args)
    {

      
        // var numbersToTest = NumberGenerator.GenerateMobileNumbers(numberCount);
        //
        // // Memory usage before cache building
        // long memoryBeforeCache = GC.GetTotalMemory(true);
        // var buildTime = Stopwatch.StartNew();
        // BuildCache("numbers.txt");
        // buildTime.Stop();
        // // Memory usage after cache building
        // long memoryAfterCache = GC.GetTotalMemory(true);
        // long memoryUsedInMB = (memoryAfterCache - memoryBeforeCache) / (1024 * 1024); // Convert bytes to MB
        //
        // Console.WriteLine($"Time to build cache: {buildTime.ElapsedMilliseconds}ms");
        // Console.WriteLine($"Memory used by cache: {memoryUsedInMB} MB");
        // Console.WriteLine($"Time to build cache: {buildTime.ElapsedMilliseconds}ms");
        //
        // var checkTime = Stopwatch.StartNew();
        // foreach (var number in numbersToTest)
        // {
        //     IdentifyNetwork(number, out var ported);
        // }
        //
        // checkTime.Stop();
        // Console.WriteLine($"Time to check {numberCount} numbers: {checkTime.ElapsedMilliseconds}ms");
        Boolean isported = false;
        BuildCache("numbers.txt");
        //this number is ported to MTN
         IdentifyNetwork("27758481252",out isported);
        
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

                    //I need to store the network name, essentially vodacom and its ported numbers from the other different service providers
                    //upon receiving a number i need to be able to check what network it is, analyse the first 4 or 5 characters I think
                    // keep the name of the provider and loop through all the providers and their relative ported numbers but only for that particular network. this should take constant time as I have 4 providers and will only be checking
                    // that particular provider 
                    //must return the name of the provider and if provider name and network name aren't the same we say the number is ported and return the portedNetworkName 
                    
                    //must create method to come up with originL provider
                    String originalProvider = getOriginalProvider(number.Substring(0,5));
                    // the goal was never to store the whole number but every number after the provider details have been removed 
                    //given a number e.g 27747007704 we keep the number 007704 after removing provider details 
                    int startposition = 4;
                    int length=number.Length;
                    int identifier= Int32.Parse(number.Substring(startposition + 1, length - (startposition + 1)));
                    if (!portedNumbers.ContainsKey(network))
                    {
                        ServiceProvider provider=new ServiceProvider(network);
                        provider.PopulateValues(identifier,originalProvider);
                        portedNumbers.Add(network, provider);
                    }
                    else
                    {
                        ServiceProvider provider = portedNumbers[network];
                        provider.PopulateValues(identifier,originalProvider);
                    }
                }
            }
        }
        
    }

    public static String getOriginalProvider(String number)
    {
        String networkIdentifier = number.Substring(0, 5);
        if (networkIdentifier.Equals("27747"))
        {
            return "CC";
        }
        
        if (networkIdentifier.Equals("27731"))
        {
            return "MTN";
        }
        
        if (networkIdentifier.Equals("27699"))
        {
            return "TK";
        }

        if (networkIdentifier.Equals("27758"))
        {
            return "VC";
        }

        return "";
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

    public static void IdentifyNetwork(string destination, out bool ported)
    {
        int startposition = 5;
        int length=destination.Length;
        Int32 Identifier = Int32.Parse(destination.Substring(startposition + 1, length - (startposition+1)));
        String originalProvider = getOriginalProvider(destination.Substring(0,5));
        
        foreach (var kvp in portedNumbers)
        {
            if (kvp.Value.IspartOfProvider(Identifier, originalProvider))
            {
                Console.WriteLine($"The number {destination} is part of {kvp.Key} and was ported from {originalProvider}. its currently under Provider {kvp.Value.getProviderName()}");
                ported = kvp.Value.getisPorted();
                return;
            }
           
        }
        Console.WriteLine($"The number {destination} is part of {originalProvider}. its not ported ");
        ported = false;
        return;
    }
}