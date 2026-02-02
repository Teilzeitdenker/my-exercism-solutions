defmodule GuessingGame do
  def compare(secret_number, guess \\ :nope)
  def compare(_secret_number, guess) when guess == :nope do
    "Make a guess"
  end
  def compare(_secret_number, guess) when not is_number(guess) do
    "Make a guess"
  end
  def compare(secret_number, guess) when secret_number == guess do
    "Correct"
  end
  def compare(secret_number, guess) when abs(secret_number - guess) == 1 do
    "So close"
  end
  def compare(secret_number, guess) when secret_number < guess do
    "Too high"
  end
  def compare(secret_number, guess) when secret_number > guess do
    "Too low"
  end
end
