defmodule RobotSimulator do
  defstruct direction: nil, position: nil
  @type direction() :: :north | :east | :south | :west
  @type position() :: {integer(), integer()}
  @type robot() :: %RobotSimulator{direction: direction(), position: position()}

  @doc """
  Create a Robot Simulator given an initial direction and position.

  Valid directions are: `:north`, `:east`, `:south`, `:west`
  """
  @spec create(direction, position) :: robot() | {:error, String.t()}
  def create(direction \\ :north, position \\ {0, 0}) do
    cond do
       not Enum.member?([:north , :east , :south , :west], direction)  -> {:error, "invalid direction"}
       not is_tuple(position) || tuple_size(position) != 2 || not is_integer(elem(position, 0)) || not is_integer(elem(position, 1)) -> {:error, "invalid position"}
       true -> %RobotSimulator{direction: direction, position: position}
    end
  end

  @doc """
  Simulate the robot's movement given a string of instructions.

  Valid instructions are: "R" (turn right), "L", (turn left), and "A" (advance)
  """
  @spec simulate(robot, instructions :: String.t()) :: robot() | {:error, String.t()}
  def simulate(robot, instructions) do
    result =
      instructions
      |> String.codepoints()
      |> Enum.reduce(robot, &move_one/2)
    if result == :error do
      {:error, "invalid instruction"}
    else
      result
    end
  end

  defp turn_left(robot) do
    new_direction = case robot.direction do
      :north -> :west
      :east -> :north
      :south -> :east
      :west -> :south
    end
    %{robot | direction: new_direction}
  end

  defp turn_right(robot) do
    new_direction = case robot.direction do
      :north -> :east
      :east -> :south
      :south -> :west
      :west -> :north
    end
    %{robot | direction: new_direction}
  end

  defp advance(%RobotSimulator{direction: dir, position: {x, y}} = robot) do
    new_position = case dir do
      :north -> {x, y + 1}
      :east -> {x + 1, y}
      :south -> {x, y - 1}
      :west -> {x - 1, y}
    end
    %{robot | position: new_position}
  end

  defp move_one(letter, robot) do
    if robot == :error do
      :error
    else
      case letter do
        "A" -> advance(robot)
        "L" -> turn_left(robot)
        "R" -> turn_right(robot)
        _ -> :error
      end
    end
  end

  @doc """
  Return the robot's direction.

  Valid directions are: `:north`, `:east`, `:south`, `:west`
  """
  @spec direction(robot) :: direction()
  def direction(robot) do
    robot.direction
  end

  @doc """
  Return the robot's position.
  """
  @spec position(robot) :: position()
  def position(robot) do
    robot.position
  end
end
