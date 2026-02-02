defmodule ComplexNumbers do
  @typedoc """
  In this module, complex numbers are represented as a tuple-pair containing the real and
  imaginary parts.
  For example, the real number `1` is `{1, 0}`, the imaginary number `i` is `{0, 1}` and
  the complex number `4+3i` is `{4, 3}'.
  """
  @type complex :: {float, float}

  @doc """
  Return the real part of a complex number
  """
  @spec real(a :: complex) :: float
  def real(a) do
    a |> elem(0)
  end

  @doc """
  Return the imaginary part of a complex number
  """
  @spec imaginary(a :: complex) :: float
  def imaginary(a) do
    a |> elem(1)
  end

  @doc """
  Multiply two complex numbers, or a real and a complex number
  """
  @spec mul(a :: complex | float, b :: complex | float) :: complex
  def mul(a, b) do
    a = if is_number(a), do: {a, 0}, else: a
    b = if is_number(b), do: {b, 0}, else: b
    {elem(a, 0) * elem(b, 0) - elem(a, 1) * elem(b, 1), elem(a, 0) * elem(b, 1) + elem(a, 1) * elem(b, 0) }
  end

  @doc """
  Add two complex numbers, or a real and a complex number
  """
  @spec add(a :: complex | float, b :: complex | float) :: complex
  def add(a, b) do
    u = if is_number(a), do: {a, 0}, else: a
    w = if is_number(b), do: {b, 0}, else: b
    {elem(u, 0) + elem(w, 0), elem(u, 1) + elem(w, 1)}
  end

  @doc """
  Subtract two complex numbers, or a real and a complex number
  """
  @spec sub(a :: complex | float, b :: complex | float) :: complex
  def sub(a, b) do
    u = if is_number(a), do: {a, 0}, else: a
    w = if is_number(b), do: {b, 0}, else: b
    {elem(u, 0) - elem(w, 0), elem(u, 1) - elem(w, 1)}
  end

  @doc """
  Divide two complex numbers, or a real and a complex number
  """
  @spec div(a :: complex | float, b :: complex | float) :: complex
  def div(a, b) do
     mul(reciprocal(b), a)
  end

  @spec abs_sq(complex()) :: float()
  defp abs_sq({re, im}) do
    :math.pow(re, 2.0) + :math.pow(im, 2.0)
  end

  @spec reciprocal(complex() | float()) :: complex()
  defp reciprocal(a) do
    z = if is_number(a), do: {a, 0}, else: a
    {elem(z, 0) / abs_sq(z), - elem(z, 1) / abs_sq(z)}
  end
  @doc """
  Absolute value of a complex number
  """
  @spec abs(a :: complex | float) :: float
  def abs(a) do
    z = if is_number(a), do: {a, 0}, else: a
    :math.pow(abs_sq(z), 0.5)
  end

  @doc """
  Conjugate of a complex number
  """
  @spec conjugate(a :: complex) :: complex
  def conjugate({re, im}) do
    {re, -im}
  end

  @doc """
  Exponential of a complex number
  """
  @spec exp(a :: complex) :: complex
  def exp({re, im}) do
    mul(:math.exp(re), {:math.cos(im), :math.sin(im)})
  end
end
