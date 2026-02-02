defmodule DiffieHellman do
  def generate_private_key(prime_p) do
    Enum.random(2..prime_p - 1)
  end

  def generate_public_key(prime_p, prime_g, private_key) do
    mod_exp(prime_g, private_key, prime_p)
  end

  def generate_shared_secret(prime_p, public_key_b, private_key_a) do
    mod_exp(public_key_b, private_key_a, prime_p)
  end

  defp mod_exp(a, b, m) when m > 0, do: mod_exp(rem(a, m), rem(b, m), m, 1)
  defp mod_exp(a, b, m, r) do
    cond do
      b == 0         -> r
      rem(b, 2) == 1 -> mod_exp(a          , b - 1    , m, rem(a*r, m))
      true           -> mod_exp(rem(a*a, m), div(b, 2), m, r          )
    end
  end
end
