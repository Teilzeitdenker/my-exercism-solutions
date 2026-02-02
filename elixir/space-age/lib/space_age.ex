defmodule SpaceAge do
  @type planet ::
          :mercury
          | :venus
          | :earth
          | :mars
          | :jupiter
          | :saturn
          | :uranus
          | :neptune

  @doc """
  Return the number of years a person that has lived for 'seconds' seconds is
  aged on 'planet', or an error if 'planet' is not a planet.
  """
  @spec age_on(planet, pos_integer) :: {:ok, float} | {:error, String.t()}
  def age_on(planet, seconds) do
    case planet do
      :mercury -> {:ok, calculate_age(seconds, 0.2408467)}
      :venus -> {:ok, calculate_age(seconds, 0.61519726)}
      :earth -> {:ok, calculate_age(seconds, 1.0)}
      :mars -> {:ok, calculate_age(seconds, 1.8808158)}
      :jupiter -> {:ok, calculate_age(seconds, 11.862615)}
      :saturn -> {:ok, calculate_age(seconds, 29.447498)}
      :uranus -> {:ok, calculate_age(seconds, 84.016846)}
      :neptune -> {:ok, calculate_age(seconds, 164.79132)}
      _ -> {:error, "not a planet"}
    end
  end

  @spec calculate_age(pos_integer, float) :: float
  defp calculate_age(seconds, factor) do
    earth_year_seconds = 31557600.0
    seconds / (earth_year_seconds * factor)
  end
end
