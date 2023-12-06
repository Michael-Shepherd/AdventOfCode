using System.Net;
using Utilities;

namespace Days;

public static class DayFive
{
    private const string SEED_TO_SOIL = "seed-to-soil map:";
    private const string SOIL_TO_FERTILIZER = "soil-to-fertilizer map:";
    private const string FERT_TO_WATER = "fertilizer-to-water map:";
    private const string WATER_TO_LIGHT = "water-to-light map:";
    private const string LIGHT_TO_TEMP = "light-to-temperature map:";
    private const string TEMP_TO_HUM = "temperature-to-humidity map:";
    private const string HUM_TO_LOC = "humidity-to-location map:";

    public static long Handle()
    {
        return HandlePartTwo();
    }

    private static long HandlePartOne()
    {
        // Did it all in 2 :(
        string[] input = InputReader.Get(".\\input\\day_Five_input.txt");
        return -1;
    }

    private static long HandlePartTwo()
    {
        string[] stateArray = [SEED_TO_SOIL, SOIL_TO_FERTILIZER, FERT_TO_WATER, WATER_TO_LIGHT, LIGHT_TO_TEMP, TEMP_TO_HUM, HUM_TO_LOC];

        string[] input = InputReader.Get(".\\input\\day_Five_input.txt");

        var seeds = input[0].Split(": ").Last().Split(" ").Select(s =>
        {
            if (long.TryParse(s, out long seed))
            {
                return seed;
            }
            else
            {
                return -1;
            }
        }).Where(s => s >= 0).ToList();


        var seedToSoilMapperCalcList = new List<MappingCalculator>();
        var soilToFertilisMerapperCalcList = new List<MappingCalculator>();
        var fertToWaterMapperCalcList = new List<MappingCalculator>();
        var waterToLightMapperCalcList = new List<MappingCalculator>();
        var lightToTempMapperCalcList = new List<MappingCalculator>();
        var tempToHumMapperCalcList = new List<MappingCalculator>();
        var humToLocMapperCalcList = new List<MappingCalculator>();

        var section = "";
        foreach (var line in input)
        {
            if (string.IsNullOrEmpty(line))
            {
                continue;
            }

            if (stateArray.Contains(line.Trim()))
            {
                section = line;
                continue;
            }

            var numbers = line.Split(" ");
            long.TryParse(numbers[0], out long destStart);
            long.TryParse(numbers[1], out long sourceStart);
            long.TryParse(numbers[2], out long range);

            if (section == SEED_TO_SOIL)
            {
                seedToSoilMapperCalcList.Add(new MappingCalculator(destStart, sourceStart, range));
            }

            if (section == SOIL_TO_FERTILIZER)
            {
                soilToFertilisMerapperCalcList.Add(new MappingCalculator(destStart, sourceStart, range));
            }

            if (section == FERT_TO_WATER)
            {
                fertToWaterMapperCalcList.Add(new MappingCalculator(destStart, sourceStart, range));
            }

            if (section == WATER_TO_LIGHT)
            {
                waterToLightMapperCalcList.Add(new MappingCalculator(destStart, sourceStart, range));
            }

            if (section == LIGHT_TO_TEMP)
            {
                lightToTempMapperCalcList.Add(new MappingCalculator(destStart, sourceStart, range));
            }

            if (section == TEMP_TO_HUM)
            {
                tempToHumMapperCalcList.Add(new MappingCalculator(destStart, sourceStart, range));
            }

            if (section == HUM_TO_LOC)
            {
                humToLocMapperCalcList.Add(new MappingCalculator(destStart, sourceStart, range));
            }
        }

        var lowest = long.MaxValue;

        var newSeeds = new List<long>();
        var seedPairs = seeds.Count() / 2;
        Console.WriteLine(seeds.Count());

        Parallel.For(0, seedPairs, i =>
        {
            long length = seeds[i * 2 + 1] - 1;
            Parallel.For(0, length, j =>
            {
                var seed = seeds[i * 2] + j;

                var seedNumber = seed;
                // long.TryParse(seed, out long seedNumber);

                var soil = seedNumber;

                foreach (var calc in seedToSoilMapperCalcList)
                {
                    if (calc.IsSourceInRange(seedNumber))
                    {
                        soil = calc.GetDestFromSource(seedNumber);
                        break;
                    }
                }

                var fert = soil;

                foreach (var calc in soilToFertilisMerapperCalcList)
                {
                    if (calc.IsSourceInRange(soil))
                    {
                        fert = calc.GetDestFromSource(soil);
                        break;
                    }
                }

                var water = fert;

                foreach (var calc in fertToWaterMapperCalcList)
                {
                    if (calc.IsSourceInRange(fert))
                    {
                        water = calc.GetDestFromSource(fert);
                        break;
                    }
                }

                var light = water;

                foreach (var calc in waterToLightMapperCalcList)
                {
                    if (calc.IsSourceInRange(water))
                    {
                        light = calc.GetDestFromSource(water);
                        break;
                    }
                }

                var temp = light;

                foreach (var calc in lightToTempMapperCalcList)
                {
                    if (calc.IsSourceInRange(light))
                    {
                        temp = calc.GetDestFromSource(light);
                        break;
                    }
                }

                var hum = temp;

                foreach (var calc in tempToHumMapperCalcList)
                {
                    if (calc.IsSourceInRange(temp))
                    {
                        hum = calc.GetDestFromSource(temp);
                        break;
                    }
                }

                var loc = hum;

                foreach (var calc in humToLocMapperCalcList)
                {
                    if (calc.IsSourceInRange(hum))
                    {
                        loc = calc.GetDestFromSource(hum);
                        break;
                    }
                }

                if (loc < lowest)
                {
                    lowest = loc;
                    Console.WriteLine(lowest);
                }
            });
        });

        return lowest;
    }
}

class MappingCalculator
{
    public long destStart { get; set; }
    public long sourceStart { get; set; }
    public long range { get; set; }

    public MappingCalculator() { }

    public MappingCalculator(long destStart, long sourceStart, long range)
    {
        this.destStart = destStart;
        this.sourceStart = sourceStart;
        this.range = range;
    }

    public long GetDestFromSource(long source)
    {
        if (this.IsSourceInRange(source))
        {
            var offset = source - sourceStart;
            var result = destStart + offset;
            return result;
        }
        else
        {
            return source;
        }
    }

    public bool IsSourceInRange(long source)
    {
        return source >= sourceStart && source < sourceStart + range;
    }

    public override string ToString()
    {
        return $"DestStart {this.destStart} SourceStart {this.sourceStart} Range {this.range}";
    }
}
