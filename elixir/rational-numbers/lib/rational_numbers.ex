defmodule RationalNumbers do
  @type rational :: {integer, integer}

  defp num(a) do
    elem(a, 0)
  end

  defp den(a) do
    elem(a, 1)
  end

  defp inv({a, b}) do
    {b, a}
  end

  @doc """
  Add two rational numbers
  """
  @spec add(a :: rational, b :: rational) :: rational
  def add(a, b) do
    reduce({num(a)*den(b) + num(b)*den(a), den(a)*den(b)})
  end

  @doc """
  Subtract two rational numbers
  """
  @spec subtract(a :: rational, b :: rational) :: rational
  def subtract(a, b) do
    reduce({num(a)*den(b) - num(b)*den(a), den(a)*den(b)})
  end

  @doc """
  Multiply two rational numbers
  """
  @spec multiply(a :: rational, b :: rational) :: rational
  def multiply(a, b) do
    reduce({num(a)*num(b), den(a)*den(b)})
  end

  @doc """
  Divide two rational numbers
  """
  @spec divide_by(num :: rational, den :: rational) :: rational
  def divide_by(num, den) do
    multiply(num, inv(den))
  end

  @doc """
  Absolute value of a rational number
  """
  @spec abs(a :: rational) :: rational
  def abs(a) do
    reduce({Kernel.abs(num(a)), Kernel.abs(den(a))})
  end

  @doc """
  Exponentiation of a rational number by an integer
  """
  @spec pow_rational(a :: rational, n :: integer) :: rational
  def pow_rational(a, n) do
    case n do
      0 -> {1, 1}
      i when i > 0 -> reduce({Integer.pow(num(a), i), Integer.pow(den(a), i)})
      _ -> reduce({Integer.pow(den(a), -n), Integer.pow(num(a), -n)})
    end
  end

  @doc """
  Exponentiation of a real number by a rational number
  """
  @spec pow_real(x :: integer, n :: rational) :: float
  def pow_real(x, n) do
    :math.pow(x, num(n) / den(n))
  end

  @doc """
  Reduce a rational number to its lowest terms
  """
  @spec reduce(rational) :: rational
  def reduce({a, b}) do
    g = Integer.gcd(a, b)
    if b > 0 do
      {div(a, g), div(b, g)}
    else
      {div(-a, g), div(-b, g)}
    end
  end
end
