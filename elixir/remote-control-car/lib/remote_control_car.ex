defmodule RemoteControlCar do
  @enforce_keys [:nickname]
  defstruct [:nickname, battery_percentage: 100, distance_driven_in_meters: 0]


  def new(nickname \\ "none") do
    %RemoteControlCar{nickname: nickname}
  end

  # do pattern matching inside the parameter brackets
  def display_distance(%RemoteControlCar{distance_driven_in_meters: dist} = _remote_car) do
    "#{dist} meters"
  end

  def display_battery(%RemoteControlCar{battery_percentage: actual} = _remote_car) do
    if  actual > 0 do
      "Battery at #{actual}%"
    else
      "Battery empty"
    end
  end

  def drive(%RemoteControlCar{battery_percentage: actual, distance_driven_in_meters: dist} = remote_car) do
    if actual > 0 do
      %{remote_car | battery_percentage: actual - 1, distance_driven_in_meters: dist + 20}
    else
      remote_car
    end
  end
end
