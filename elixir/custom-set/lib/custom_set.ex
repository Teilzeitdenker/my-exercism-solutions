defmodule CustomSet do
  @opaque t :: %__MODULE__{mp: map}
  defstruct mp: %{}

  @spec new(Enum.t()) :: t
  def new(en) do
    %__MODULE__{mp: Enum.reduce(en, %{}, &Map.put_new(&2, &1, nil))}
  end

  @spec empty?(t) :: boolean
  def empty?(%{mp: map}) do
    map == %{}
  end

  @spec contains?(t, any) :: boolean
  def contains?(%{mp: map}, el) do
    Map.has_key?(map, el)
  end

  @spec subset?(t, t) :: boolean
  def subset?(%{mp: map1}, %{mp: map2}) do
    Map.take(map1, Map.keys(map2)) == map1
  end

  @spec disjoint?(t, t) :: boolean
  def disjoint?(%{mp: map1}, %{mp: map2}) do
    Map.drop(map1, Map.keys(map2)) == map1
  end

  @spec equal?(t, t) :: boolean
  def equal?(%{mp: map1}, %{mp: map2}) do
    map1 == map2
  end

  @spec add(t, any) :: t
  def add(%{mp: map}, el) do
    %__MODULE__{mp: Map.put_new(map, el, nil)}
  end

  @spec intersection(t, t) :: t
  def intersection(%{mp: map1}, %{mp: map2}) do
    %__MODULE__{mp: Map.take(map1, Map.keys(map2))}
  end

  @spec difference(t, t) :: t
  def difference(%{mp: map1}, %{mp: map2}) do
    %__MODULE__{mp: Map.drop(map1, Map.keys(map2))}
  end

  @spec union(t, t) :: t
  def union(%{mp: map1}, %{mp: map2}) do
    %__MODULE__{mp: Map.merge(map1, map2)}
  end
end
