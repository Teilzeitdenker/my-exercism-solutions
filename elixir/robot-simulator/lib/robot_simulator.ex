defmodule RobotSimulator do
  defstruct direction: nil, position: nil
  @type direction() :: :north | :east | :south | :west
  defguard is_direction(direction) when direction in [:north, :south, :east, :west]
  @type position() :: {integer(), integer()}
  defguard is_position(pos) when is_tuple(pos) and tuple_size(pos) == 2 and is_integer(elem(pos, 0)) and is_integer(elem(pos, 1))
  @type robot() :: %RobotSimulator{direction: direction(), position: position()}
  defguard is_instruction(str) when str in ["A", "L", "R"]

  @doc """
  Create a Robot Simulator given an initial direction and position.

  Valid directions are: `:north`, `:east`, `:south`, `:west`
  """
  @spec create(direction, position) :: robot() | {:error, String.t()}
  def create(direction \\ :north, position \\ {0, 0})  # header with default values
  def create(direction, _) when not is_direction(direction), do: {:error, "invalid direction"} # use defined guards in these cases
  def create(_, position) when not is_position(position), do: {:error, "invalid position"}
  def create(direction, position) do
    %RobotSimulator{direction: direction, position: position}
  end

  @doc """
  Simulate the robot's movement given a string of instructions.

  Valid instructions are: "R" (turn right), "L", (turn left), and "A" (advance)
  """
  @spec simulate(robot, instructions :: String.t()) :: robot() | {:error, String.t()}
  def simulate(robot, instructions) do
    if instructions |> String.codepoints |> Enum.all?(&is_instruction/1) do
      instructions
      |> String.codepoints()
      |> Enum.reduce(robot, &move_one/2)
    else
      {:error, "invalid instruction"}
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
    case letter do
      "A" -> advance(robot)
      "L" -> turn_left(robot)
      "R" -> turn_right(robot)
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
