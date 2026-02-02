defmodule PaintByNumber do
  use Bitwise

  def palette_bit_size(color_count) do
    case color_count do
      1    -> 1
      2    -> 1
      _    -> 1 + palette_bit_size((color_count + 1) >>> 1)
    end
  end

  def empty_picture() do
    <<>>
  end

  def test_picture() do
    <<0b00::2, 0b01::2, 0b10::2, 0b11::2>>
  end

  def prepend_pixel(picture, color_count, pixel_color_index) do
    sz = palette_bit_size(color_count)
    <<pixel_color_index :: size(sz), picture :: bitstring>>
  end

  def get_first_pixel(<<>>, _), do: nil
  def get_first_pixel(picture, color_count) do
    sz = palette_bit_size(color_count)
    <<first_pixel :: size(sz), _rest :: bitstring>> = picture
    first_pixel
  end

  def drop_first_pixel(<<>>, _), do: <<>>
  def drop_first_pixel(picture, color_count) do
    sz = palette_bit_size(color_count)
    <<_first_pixel :: size(sz), rest :: bitstring>> = picture
    rest
  end

  def concat_pictures(picture1, picture2) do
    <<picture1 :: bitstring, picture2 :: bitstring>>
  end
end
