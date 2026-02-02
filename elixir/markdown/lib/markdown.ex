defmodule Markdown do
  def parse(m) do
    header? = Regex.named_captures(~r/(?m)^(?<level>\#{1,6})/, m)
    lvl = case header?, do: (nil -> 0; _ -> header?["level"] |> String.length)
    m = Regex.replace(~r/(?m)^\#{1,6} (.+)$/, m, "<h#{lvl}>\\g{1}</h#{lvl}>")
    m = Regex.replace(~r/__(.+)__/, m, "<strong>\\g{1}</strong>")
    m = Regex.replace(~r/_(.+)_/, m, "<em>\\g{1}</em>")
    m = Regex.replace(~r/(?m)^\* (.+)$/, m, "<li>\\g{1}</li>")
    m = Regex.replace(~r/(?s)(<li>.+<\/li>)/, m, "<ul>\\g{1}</ul>")
    m = Regex.replace(~r/(?m)^(?!<h|<l|<u)(.+)$/, m, "<p>\\g{1}</p>")
    Regex.replace(~r/\n/, m, "")
  end
end
