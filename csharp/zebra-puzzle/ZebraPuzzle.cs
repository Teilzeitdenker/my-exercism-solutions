using System;
using System.Linq;

// genetic algorithm written by Erik Schierboom and described in this video
// https://www.youtube.com/watch?v=waAFJKtsJrs&embeds_referring_euri=https%3A%2F%2Fexercism.org%2F&embeds_referring_origin=https%3A%2F%2Fexercism.org&source_ve_path=OTY3MTQ
// from timestamp 9:10 to about 29:45

public enum Nationality { Norwegian, Ukrainian, Englishman, Spaniard, Japanese };
public enum Color { Red, Blue, Yellow, Ivory, Green };
public enum Drink { Tea, Milk, Coffee, OrangeJuice, Water };
public enum Smoke { LuckyStrike, OldGold, Kools, Chesterfield, Parliaments };
public enum Pet { Zebra, Fox, Horse, Snails, Dog };

public static class ZebraPuzzle
{
    public static Nationality DrinksWater() =>
        Solution.Value.Houses.First(house => house.Drink == Drink.Water).Nationality;
    
    public static Nationality OwnsZebra() =>
        Solution.Value.Houses.First(house => house.Pet == Pet.Zebra).Nationality;
    
    private static readonly Lazy<Individual> Solution = new(Simulation.Run);
}

public record struct House(Nationality Nationality, Color Color, Drink Drink, Smoke Smoke, Pet Pet);
public record struct Individual(House[] Houses, double Fitness);
public record struct Population(Individual[] Individuals, Individual MostFit);

internal static class Simulation
{
    private const int PopulationSize = 100_000;
    private const int MaxNumberOfGenerations = 1_000;

    public static Individual Run()
    {
        var population = Initialization.RandomPopulation(PopulationSize);
        for (var i = 0; i < MaxNumberOfGenerations; i++)
        {
            if (population.MostFit.Fitness >= 1.0)  return population.MostFit;
            population = Reproduction.Evolve(population);
        }
        throw new InvalidOperationException("Could not find solution");
    }
}

internal static class Selection
{
    private const int MaxScore = 14;
    private const int TournamentSize = 5;

    public static Individual MostFit(Individual[] individuals) =>
        individuals.MaxBy(candidate => candidate.Fitness);
    
    public static Individual Tournament(Population population) =>
        Random.Shared.GetItems(population.Individuals, TournamentSize)
            .MaxBy(candidate => candidate.Fitness);
    
    public static double Fitness(House[] houses)
    {
        bool Adjacent(Predicate<House> first, Predicate<House> second) =>
            Math.Abs(Array.FindIndex(houses, first) - Array.FindIndex(houses, second)) == 1;
        var score = 0;
        if (houses.Any(house => house is { Nationality: Nationality.Englishman, Color: Color.Red })) score++;
        if (houses.Any(house => house is { Nationality: Nationality.Spaniard, Pet: Pet.Dog })) score++;
        if (houses.Any(house => house is { Drink: Drink.Coffee, Color: Color.Green })) score++;
        if (houses.Any(house => house is { Nationality: Nationality.Ukrainian, Drink: Drink.Tea })) score++;
        if (Adjacent(first => first is { Color: Color.Ivory }, second => second is { Color: Color.Green })) score++;
        if (houses.Any(house => house is { Smoke: Smoke.OldGold, Pet: Pet.Snails })) score++;
        if (houses.Any(house => house is { Smoke: Smoke.Kools, Color: Color.Yellow })) score++;
        if (houses[2] is { Drink: Drink.Milk }) score++;
        if (houses[0] is { Nationality: Nationality.Norwegian }) score++;
        if (Adjacent(first => first is { Pet: Pet.Fox }, second => second is { Smoke: Smoke.Chesterfield })) score++;
        if (Adjacent(first => first is { Smoke: Smoke.Kools }, second => second is { Pet: Pet.Horse })) score++;
        if (houses.Any(house => house is { Smoke: Smoke.LuckyStrike, Drink: Drink.OrangeJuice })) score++;
        if (houses.Any(house => house is { Nationality: Nationality.Japanese, Smoke: Smoke.Parliaments })) score++;
        if (Adjacent(first => first is { Nationality: Nationality.Norwegian }, second => second is { Color: Color.Blue })) score++;
        return (double)score / MaxScore;
    }
}

internal static class Reproduction
{
    private const int MutationRate = 19;
    private static readonly int[] HouseIndices = [0, 1, 2, 3, 4];
    
    public static Population Evolve(Population population)
    {
        var newIndividuals = Enumerable.Range(0, population.Individuals.Length)
            .Select(_ => Reproduce(Selection.Tournament(population), Selection.Tournament(population)))
            .ToArray();
        return new(newIndividuals, Selection.MostFit(newIndividuals));
    }
    
    private static Individual Reproduce(Individual parent1, Individual parent2)
    {
        var child = Crossover(parent1, parent2);
        if (ShouldMutate())
            Mutate(child);
        return child;
    }
    
    private static Individual Crossover(Individual parent1, Individual parent2)
    {
        var parentHouses = Enumerable.Range(0, 5)
            .Select(_ => Random.Shared.Next(2) == 0 ? parent1.Houses : parent2.Houses)
            .ToArray();
        var childHouses = Enumerable.Range(0, 5)
            .Select(i => new House(
                parentHouses[0][i].Nationality,
                parentHouses[1][i].Color,
                parentHouses[2][i].Drink,
                parentHouses[3][i].Smoke,
                parentHouses[4][i].Pet
            ))
            .ToArray();
        return new Individual(childHouses, Selection.Fitness(childHouses));
    }
    
    private static bool ShouldMutate() => Random.Shared.Next(0, 100) <= MutationRate;
    
    private static void Mutate(Individual individual)
    {
        Random.Shared.Shuffle(HouseIndices);
        var house1 = individual.Houses[HouseIndices[0]];
        var house2 = individual.Houses[HouseIndices[1]];
        var actions = new Action[]
        {
            () => (house1.Nationality, house2.Nationality) = (house2.Nationality, house1.Nationality),
            () => (house1.Color, house2.Color) = (house2.Color, house1.Color),
            () => (house1.Drink, house2.Drink) = (house2.Drink, house1.Drink),
            () => (house1.Smoke, house2.Smoke) = (house2.Smoke, house1.Smoke),
            () => (house1.Pet, house2.Pet) = (house2.Pet, house1.Pet)
        };
        actions[Random.Shared.Next(0, actions.Length)]();
    }
}

internal static class Initialization
{
    private static readonly Nationality[] Nations = Enum.GetValues<Nationality>();
    private static readonly Color[] Colors = Enum.GetValues<Color>();
    private static readonly Drink[] Drinks = Enum.GetValues<Drink>();
    private static readonly Smoke[] Smokes = Enum.GetValues<Smoke>();
    private static readonly Pet[] Pets = Enum.GetValues<Pet>();

    public static Population RandomPopulation(int size)
    {
        var individuals = Enumerable.Range(0, size).Select(_ => RandomIndividual()).ToArray();
        return new(individuals, Selection.MostFit(individuals));
    }
    
    private static Individual RandomIndividual()
    {
        Random.Shared.Shuffle(Nations);
        Random.Shared.Shuffle(Colors);
        Random.Shared.Shuffle(Drinks);
        Random.Shared.Shuffle(Smokes);
        Random.Shared.Shuffle(Pets);
        var houses = Enumerable.Range(0, 5).Select(RandomHouse).ToArray();
        return new Individual(houses, Selection.Fitness(houses));
    }
    
    private static House RandomHouse(int i) => new(Nations[i], Colors[i], Drinks[i], Smokes[i], Pets[i]);
}