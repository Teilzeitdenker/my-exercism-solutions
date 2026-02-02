defmodule FlattenArray do
  @doc """
    Accept a list and return the list flattened without nil values.

    ## Examples

      iex> FlattenArray.flatten([1, [2], 3, nil])
      [1,2,3]

      iex> FlattenArray.flatten([nil, nil])
      []

  """

  @spec flatten(list) :: list
  def flatten([]), do: []
  def flatten([el|rest]) when is_list(el), do: Enum.concat(flatten(el), flatten(rest))
  def flatten([el|rest]) when is_nil(el), do: flatten(rest)
  def flatten([el|rest]), do: [el|flatten(rest)]
end
