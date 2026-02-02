defmodule EliudsEggs do
  def egg_count(0), do: 0
  def egg_count(n), do: Bitwise.&&&(n, 1) + egg_count(Bitwise.>>>(n, 1))
end
