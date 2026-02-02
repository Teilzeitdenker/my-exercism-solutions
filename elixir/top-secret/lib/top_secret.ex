defmodule TopSecret do
  def to_ast(string) do
    Code.string_to_quoted!(string)
  end

  def decode_secret_message_part({atom, _ , list} = ast, acc) when atom in [:def, :defp] do
    {name_atom, _, args} =
      if elem(List.first(list), 0) == :when do
        list |> List.first() |> elem(2) |> List.first()
      else
        list |> List.first()
      end
    letters =
      if is_nil(args) do
        0
      else
        length(args)
      end
    append =
      if letters > 0 do
        Atom.to_string(name_atom) |> String.slice(0..letters-1)
      else
        ""
      end
    {ast, [append|acc]}
  end

  def decode_secret_message_part(ast, acc) do
    {ast, acc}
  end

  def decode_secret_message(string) do
    ast = string |> to_ast()
    {_, acc} = Macro.prewalk(ast, [], &decode_secret_message_part/2)
    acc |> Enum.reverse |> Enum.join("")
  end
end
