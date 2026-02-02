defmodule Grep do

  @spec xor(boolean(), boolean()) :: boolean()
  defp xor(a, b) do
    (a and !b) or (!a and b)
  end

  @spec print(boolean(), boolean(), String.t(), integer(), String.t()) :: String.t()
  defp print(use_file, use_line_num, file, line_num, line) do
    "#{if use_file, do: "#{file}:", else: ""}#{if use_line_num, do: "#{line_num}:", else: ""}#{line}"
  end

  @spec grep(String.t(), [String.t()], [String.t()]) :: String.t()
  def grep(pattern, flags, files) do
    multiple_files = files |> Enum.count() > 1
    line_numbers = flags |> Enum.member?("-n")
    only_file_names = flags |> Enum.member?("-l")
    case_insensitive = flags |> Enum.member?("-i")
    invert_match = flags |> Enum.member?("-v")
    match_full_lines = flags |> Enum.member?("-x")
    rgx = Regex.compile! (if match_full_lines, do: "^#{pattern}$", else: pattern), (if case_insensitive, do: "i", else: "")
    result =
      if only_file_names do
        files
        |> Enum.filter(fn file ->
            File.read!(file)
            |> String.split("\n", trim: true)
            |> Enum.any?(fn line ->
                xor(Regex.match?(rgx, line), invert_match)
            end)
        end)
        |> Enum.join("\n")
      else
        files
        |> Enum.flat_map(fn file ->
          File.read!(file)
          |> String.split("\n", trim: true)
          |> Enum.with_index(1)
          |> Enum.filter(fn {line, _} ->
            xor(Regex.match?(rgx, line), invert_match)
          end)
          |> Enum.map(fn {line, n} ->
            print(multiple_files, line_numbers, file, n, line)
          end)
        end)
        |> Enum.join("\n")
      end
    if result == "", do: "", else: result <> "\n"
  end
end
