defmodule FileSniffer do
  def type_from_extension(extension) do
    case extension do
      "exe" -> "application/octet-stream"
      "bmp" -> "image/bmp"
      "png" -> "image/png"
      "jpg" -> "image/jpg"
      "gif" -> "image/gif"
      _     -> "No such extension"
    end
  end

  def type_from_binary(file_binary) do
    <<beginning::binary-size(8), _::binary>> = file_binary
    case beginning do
      <<0x7F, 0x45, 0x4C, 0x46, _::binary>> -> type_from_extension("exe")
      <<0x42, 0x4D, _::binary>> -> type_from_extension("bmp")
      <<0x89, 0x50, 0x4E, 0x47, 0x0D, 0x0A, 0x1A, 0x0A>> -> type_from_extension("png")
      <<0xFF, 0xD8, 0xFF, _::binary>> -> type_from_extension("jpg")
      <<0x47, 0x49, 0x46, _::binary>> -> type_from_extension("gif")
      _ -> "Cannot identify the media type"
    end
  end

  def verify(file_binary, extension) do
    should_be_media_type = type_from_binary(file_binary)
    if should_be_media_type == type_from_extension(extension)  do
      {:ok, type_from_extension(extension)}
    else
      {:error, "Warning, file format and file extension do not match."}
    end
  end
end
