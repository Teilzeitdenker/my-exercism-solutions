defmodule RemoteControlCar do
  @enforce_keys [:nickname]
  defstruct [:nickname, battery_percentage: 100, distance_driven_in_meters: 0]


  def new(nickname \\ "none") do
    %RemoteControlCar{nickname: nickname}
  end

  def display_distance(remote_car) do
    case remote_car do
      %RemoteControlCar{distance_driven_in_meters: dist} -> "#{dist} meters"
      _ -> raise FunctionClauseError
    end
  end

  def display_battery(remote_car) do
    case remote_car do
      %RemoteControlCar{battery_percentage: actual} ->
        if  actual > 0 do
          "Battery at #{actual}%"
        else
          "Battery empty"
        end
      _ -> raise FunctionClauseError
    end
  end

  def drive(remote_car) do
    case remote_car do
      %RemoteControlCar{battery_percentage: actual, distance_driven_in_meters: dist} ->
        if actual > 0 do
          remote_car
          |> Map.replace!(:battery_percentage, actual - 1)
          |> Map.replace!(:distance_driven_in_meters, dist + 20)
        else
          remote_car
        end
      _ -> raise FunctionClauseError
    end

  end
end
