defmodule SgfParsing do
  @esc [?], ?\\, ?t, ?n]
  defmodule Sgf do
    defstruct properties: %{}, children: []
  end
  @type sgf :: %Sgf{properties: map, children: [sgf]}
  @doc """
  Parse a string into a Smart Game Format tree
  """
  @spec parse(encoded :: String.t()) :: {:ok, sgf} | {:error, String.t()}
  def parse(encoded) do
    case tree(encoded, nil) do
      e = {:error, _} -> e
      {<<>>, t} -> {:ok, t}
    end
  end
  defp tree(<<>>, nil), do: {:error, "tree missing"}
  defp tree(<<"(", rs::binary>>, no) do
    case nodes(rs, no) do
      e = {:error, _} -> e
      {r, n} -> tree(r, n)
    end
  end
  defp tree(<<")", _::binary>>, nil), do: {:error, "tree with no nodes"}
  defp tree(<<")", rs::binary>>, no), do: {rs, no}
  defp tree(<<";", _::binary>>, nil), do: {:error, "tree missing"}
  defp nodes(<<";", rs::binary>>, nil) do
    case node(rs, %Sgf{}) do
      e = {:error, _} -> e
      {r, n} -> nodes(r, n)
    end
  end
  defp nodes(<<";", rs::binary>>, no) do
    case node(rs, %Sgf{}) do
      e = {:error, _} -> e
      {r, n} -> nodes(r, %{no | children: [n | no.children]})
    end
  end
  defp nodes(rs, no), do: {rs, no}
  defp node(rs, no) do
    case key(rs, no, "") do
      e = {:error, _} -> e
      {r = <<c, _::binary>>, n} when c in [?;, ?)] -> {r, n}
      {r, n} -> key(r, n, "")
    end
  end
  defp key(<<"[", rs::binary>>, no, k) do
    case value(rs, no, k, "") do
      {r = <<"[", _::binary>>, n} -> key(r, n, k)
      {r, n} -> key(r, n, "")
    end
  end
  defp key(rs=<<c, _::binary>>, no, "") when c in [?;, ?)], do: {rs, no}
  defp key(<<c, _::binary>>, _n, _k) when c in [?;, ?)], do: {:error, "properties without delimiter"}
  defp key(<<c, rs::binary>>, no, k) when c in ?A..?Z, do: key(rs, no, k <> <<c>>)
  defp key(rs = <<"(", _::binary>>, no, "") do
    case tree(rs, nil) do
      e = {:error, _} -> e
      {r, n} -> key(r, %{no | children: no.children ++ [n]}, "")
    end
  end
  defp key(_rs, _no, _k), do: {:error, "property must be in uppercase"}
  defp value(<<"]", rs::binary>>, no, k, v) do
    new_v = String.replace(v, ~r/\h/, " ")
    {rs, %{no | properties: no.properties |> Map.update(k, [new_v], fn vs -> vs ++ [new_v] end)}}
  end
  defp value(<<"\\", "\n", rs::binary>>, no, k, v), do: value(rs, no, k, v)
  defp value(<<"\\", "\t", rs::binary>>, no, k, v), do: value(rs, no, k, v <> " ")
  defp value(<<"\\", c, rs::binary>>, no, k, v) when c in @esc, do: value(rs, no, k, v <> <<c>>)
  defp value(<<c, rs::binary>>, no, k, v), do: value(rs, no, k, v <> <<c>>)
end
