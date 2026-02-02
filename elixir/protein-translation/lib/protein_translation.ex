defmodule ProteinTranslation do
  @proteins %{
    "AUG" => "Methionine",
    "UUU" => "Phenylalanine",
    "UUC" => "Phenylalanine",
    "UUA" => "Leucine",
    "UUG" => "Leucine",
    "UCU" => "Serine",
    "UCG" => "Serine",
    "UCA" => "Serine",
    "UCC" => "Serine",
    "UAU" => "Tyrosine",
    "UAC" => "Tyrosine",
    "UGU" => "Cysteine",
    "UGC" => "Cysteine",
    "UGG" => "Tryptophan",
    "UAA" => "STOP",
    "UGA" => "STOP",
    "UAG" => "STOP",
  }

  @doc """
  Given an RNA string, return a list of proteins specified by codons, in order.
  """
  @spec of_rna(String.t()) :: {:ok, list(String.t())} | {:error, String.t()}
  def of_rna(rna) do
    results =
      rna
      |> Stream.unfold(&String.split_at(&1, 3))
      |> Enum.take_while(&(&1 != ""))
      |> Enum.reduce([], fn codon, acc -> [of_codon(codon) | acc] end)
      |> Enum.reverse()
      |> Enum.take_while(fn {_, s} -> s != "STOP" end)
    if results |> Enum.any?(fn {res, _} -> res == :error end) do
      {:error, "invalid RNA"}
    else
      {:ok , results |> Enum.map(fn {_, s} -> s end)}
    end
  end

  @doc """
  Given a codon, return the corresponding protein

  UGU -> Cysteine
  UGC -> Cysteine
  UUA -> Leucine
  UUG -> Leucine
  AUG -> Methionine
  UUU -> Phenylalanine
  UUC -> Phenylalanine
  UCU -> Serine
  UCC -> Serine
  UCA -> Serine
  UCG -> Serine
  UGG -> Tryptophan
  UAU -> Tyrosine
  UAC -> Tyrosine
  UAA -> STOP
  UAG -> STOP
  UGA -> STOP
  """
  @spec of_codon(String.t()) :: {:ok, String.t()} | {:error, String.t()}
  def of_codon(codon) do
    case @proteins |> Map.has_key?(codon) do
      true  -> {:ok, @proteins[codon]}
      false -> {:error, "invalid codon"}
    end
  end
end
