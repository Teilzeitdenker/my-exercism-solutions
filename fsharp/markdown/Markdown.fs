module Markdown
open System.Text.RegularExpressions
let parse md =
   let lvl  = Regex.Match(md, "(?m)^(#{1,6})").Groups[1].Length
   let mutable html = Regex.Replace(md, "(?m)^#{1,6} (.+)$", $"<h{lvl}>$1</h{lvl}>")
   html <- Regex.Replace(html, "__(.+)__", "<strong>$1</strong>")
   html <- Regex.Replace(html, "_(.+)_", "<em>$1</em>")
   html <- Regex.Replace(html, "(?m)^\* (.+)$", "<li>$1</li>")
   html <- Regex.Replace(html, "(?s)(<li>.*</li>)", "<ul>$1</ul>")
   html <- Regex.Replace(html, "(?m)^(?!<h|<l|<u)(.+)$", "<p>$1</p>")
   Regex.Replace(html, "\n", "")