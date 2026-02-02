defmodule KitchenCalculator do
  def get_volume( { _, vol } = _volume_pair) do
    vol
  end

  def to_milliliter({ :cup, _ } = volume_pair) do
    { :milliliter, 240.0 * get_volume(volume_pair) }
  end
  def to_milliliter({ :fluid_ounce, _ } = volume_pair) do
    { :milliliter, 30.0 * get_volume(volume_pair) }
  end
  def to_milliliter({ :teaspoon, _ } = volume_pair) do
    { :milliliter, 5.0 * get_volume(volume_pair) }
  end
  def to_milliliter({ :tablespoon, _ } = volume_pair) do
    { :milliliter, 15.0 * get_volume(volume_pair) }
  end
  def to_milliliter({ :milliliter, _ } = volume_pair) do
    { :milliliter, get_volume(volume_pair) }
  end
  def to_milliliter(_) do
    "No conversion to milliliter possible"
  end

  def from_milliliter({ :milliliter, _ } = volume_pair, :milliliter ) do
    volume_pair
  end
  def from_milliliter({ :milliliter, _ } = volume_pair, :cup ) do
    { :cup, get_volume(volume_pair) / 240.0 }
  end
  def from_milliliter({ :milliliter, _ } = volume_pair, :fluid_ounce ) do
    { :fluid_ounce, get_volume(volume_pair) / 30.0 }
  end
  def from_milliliter({ :milliliter, _ } = volume_pair, :teaspoon ) do
    { :teaspoon, get_volume(volume_pair) / 5.0 }
  end
  def from_milliliter({ :milliliter, _ } = volume_pair, :tablespoon ) do
    { :tablespoon, get_volume(volume_pair) / 15.0 }
  end
  def from_milliliter(_, _) do
    "No conversion from milliliter possible"
  end

  def convert(volume_pair, unit) do
    from_milliliter(to_milliliter(volume_pair), unit)
  end
end
