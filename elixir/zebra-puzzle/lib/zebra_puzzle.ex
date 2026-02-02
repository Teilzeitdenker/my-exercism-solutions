defmodule ZebraPuzzle do
  @houses [:red, :green, :ivory, :yellow, :blue]
  @people [:englishman, :spaniard, :ukrainian, :norwegian, :japanese]
  @drinks [:milk, :coffee, :tea, :orange_juice, :water]
  @cigarettes [:chesterfields, :kools, :old_gold, :lucky_strike, :parliaments]
  @pets [:fox, :dog, :snails, :horse, :zebra]

  # solution by spripruven

  @doc """
  Determine who drinks the water
  """
  @spec drinks_water() :: atom
  def drinks_water() do
    solution = solve()
               |> List.flatten()
               |> Enum.find(&(&1.drink === :water))
    solution.people
  end

  @doc """
  Determine who owns the zebra
  """
  @spec owns_zebra() :: atom
  def owns_zebra() do
    solution = solve()
               |> List.flatten()
               |> Enum.find(&(&1.pet === :zebra))
    solution.people
  end

  defp solve() do
    people = perms(@people)
             |> Enum.map(&(Enum.map(&1, fn p -> %{people: p, drink: nil, house: nil, pet: nil, cigarette: nil} end)))
             |> Enum.filter(&(hd(&1).people === :norwegian))

    drinks = perms(@drinks)
             |> mix_with(people, :drink)
             |> Enum.filter(&(Enum.at(&1, 2).drink === :milk))
             |> Enum.filter(&(Enum.find(&1, fn m -> m.people === :ukrainian end).drink === :tea))

    houses = perms(@houses)
             |> mix_with(drinks, :house)
             |> Enum.filter(&(Enum.find(&1, fn m -> m.people === :englishman end).house === :red))
             |> Enum.filter(&(Enum.find(&1, fn m -> m.drink === :coffee end).house === :green))

    cigarettes = perms(@cigarettes)
                 |> mix_with(houses, :cigarette)
                 |> Enum.filter(&(Enum.find(&1, fn m -> m.house === :yellow end).cigarette === :kools))
                 |> Enum.filter(&(Enum.find(&1, fn m -> m.drink === :orange_juice end).cigarette === :lucky_strike))
                 |> Enum.filter(&(Enum.find(&1, fn m -> m.people === :japanese end).cigarette === :parliaments))

    perms(@pets)
    |> mix_with(cigarettes, :pet)
    |> Enum.filter(&(Enum.find(&1, fn m -> m.people === :spaniard end).pet === :dog))
    |> Enum.filter(&(Enum.find(&1, fn m -> m.cigarette === :old_gold end).pet === :snails))
    |> Enum.filter(&rigth_to?(&1, {:house, :green}, {:house, :ivory}))
    |> Enum.filter(&next_to?(&1, {:cigarette, :chesterfields}, {:pet, :fox}))
    |> Enum.filter(&next_to?(&1, {:cigarette, :kools}, {:pet, :horse}))
    |> Enum.filter(&next_to?(&1, {:people, :norwegian}, {:house, :blue}))
  end

  defp rigth_to?(list, {ref_key, ref_value}, {rigth_key, right_value}) do
    i = Enum.find_index(list, &(&1[rigth_key] === right_value))
    i > 0 and Enum.at(list, i-1)[ref_key] === ref_value
  end

  defp next_to?(list, {ref_key, ref_value}, {next_key, next_value}) do
    i = Enum.find_index(list, &(&1[ref_key] === ref_value))
    (i === 0 and Enum.at(list, i+1)[next_key] === next_value) or
    (i === 4 and Enum.at(list, i-1)[next_key] === next_value) or
    (i > 0 and i < 4 and (Enum.at(list, i-1)[next_key] === next_value or Enum.at(list, i+1)[next_key] === next_value))
  end

  defp mix_with(attributes, target_list, target_key) do
    attributes
    |> Enum.map(&map_to(&1, target_list, target_key))
    |> List.flatten()
    |> Enum.chunk_every(5)
  end

  defp map_to([v1,v2,v3,v4,v5], target_list, target_key) do
    Enum.map(target_list, fn [m1, m2, m3, m4, m5] -> [%{m1 | target_key => v1},
                                                      %{m2 | target_key => v2},
                                                      %{m3 | target_key => v3},
                                                      %{m4 | target_key => v4},
                                                      %{m5 | target_key => v5}] end)
  end

  defp perms([]), do: [[]]
  defp perms(list) do
    for elem <- list, rest <- perms(list--[elem]) do
      [elem|rest]
    end
  end
end
