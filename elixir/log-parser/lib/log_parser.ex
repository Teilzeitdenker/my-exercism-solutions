defmodule LogParser do
  def valid_line?(line) do
    line |> String.starts_with?(["[DEBUG]", "[INFO]", "[WARNING]", "[ERROR]"])
  end

  def split_line(line) do
    Regex.split(~r/<[~\*=-]*>/, line)
  end

  def remove_artifacts(line) do
    Regex.replace(~r/end-of-line[\d]+/i, line, "")
  end

  def tag_with_user_name(line) do
    if Regex.match?(~r/User[\s]+/, line) do
      [_, name] = Regex.run(~r/User[\s]+([^\s]+)/u, line)
      "[USER] #{name} #{line}"
    else
      line
    end
  end
end
