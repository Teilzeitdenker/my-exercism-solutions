defmodule NameBadge do
  def print(id, name, department) do
    if id != nil and department != nil do
      "[#{id}] - #{name} - #{String.upcase(department)}"
    else
      if id == nil and department != nil do
        "#{name} - #{String.upcase(department)}"
      else
        if id != nil and department == nil do
          "[#{id}] - #{name} - OWNER"
        else
          "#{name} - OWNER"
        end
      end
    end
  end
end
