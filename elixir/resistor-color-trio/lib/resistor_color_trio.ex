defmodule ResistorColorTrio do
  @colors  %{
    black: 0,
    brown: 1,
    red: 2,
    orange: 3,
    yellow: 4,
    green: 5,
    blue: 6,
    violet: 7,
    grey: 8,
    white: 9
  }
  @doc """
  Calculate the resistance value in ohm or kiloohm from resistor colors
  """
  @spec label(colors :: [atom]) :: {number, :ohms | :kiloohms}
  def label(colors) do
    [first | [second | [third | _]]] = colors
    value = (10 * @colors[first] + @colors[second]) * Integer.pow(10, @colors[third])
    if rem(value, 1000) == 0 do
      {div(value, 1000), :kiloohms}
    else
      {value, :ohms}
    end
  end
end
