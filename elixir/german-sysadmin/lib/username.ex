defmodule Username do
  # supersaublöd, das bekomme ich ja in Powershell immer nicht hin mit den Unicode-Charakteren,
  # kann also mit IEx nichts ausprobieren,
  # außerdem scheint es keine einfach Lösung zu geben, um zu überprüfen, ob ein Codepoint lowercase ist oder nicht
  # (es gibt ein Unicode-Package auf Hex, aber das kann ich ja nur lokal verwenden und bringt mir nichts für die Lösung)
  # naja, dann machen wir es halt einfach andersrum..., erst ersetzen, dann die Großbuchstaben und die Ziffern raus und
  # am Ende nicht vergessen, dass auch Unterstriche erlaubt sind, bin halt ein echter German Sysadmin  ;-)
  def sanitize(username) do
    Enum.map(username, fn char ->
      case char do
        ?ä -> 'ae'
        ?ö -> 'oe'
        ?ü -> 'ue'
        ?ß -> 'ss'
        _  -> char
      end
    end)
    |> List.flatten()
    |> Enum.filter(& &1 >= ?a and &1 <= ?z or &1 == ?_)
  end
end
