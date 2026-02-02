defmodule BasketballWebsite do
  def extract_from_path(data, path) do
    extract_with_list(data, String.split(path, ".") )
  end

  def extract_with_list(data, path_list) do
    case path_list do
      [] -> data
      [h|t] -> extract_with_list(data[h], t)
    end
  end

  def get_in_path(data, path) do
    get_in(data, String.split(path, "."))
  end
end
