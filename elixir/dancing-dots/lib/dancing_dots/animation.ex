defmodule DancingDots.Animation do
  @type dot :: DancingDots.Dot.t()
  @type opts :: keyword
  @type error :: any
  @type frame_number :: pos_integer

  @callback init(opts()) :: {:ok, opts()} | {:error, error()}
  @callback handle_frame(dot(), frame_number(), opts()) :: dot()

  defmacro __using__(_) do
    quote do
      @behaviour DancingDots.Animation
      def init(opts), do: {:ok, opts}
      defoverridable init: 1
    end
  end
end

defmodule DancingDots.Flicker do
  use DancingDots.Animation

  @impl DancingDots.Animation
  def handle_frame(dot, frame_number, _) when rem(frame_number, 4) == 0, do: %{dot | opacity: dot.opacity / 2 }
  def handle_frame(dot, _, _), do: dot
end

defmodule DancingDots.Zoom do
  use DancingDots.Animation

  @impl DancingDots.Animation
  def init(opts) do
    velocity = opts |> Keyword.get(:velocity)
    if not is_number(opts[:velocity]) do
      {:error, "The :velocity option is required, and its value must be a number. Got: #{inspect(velocity)}"}
    else
      {:ok, opts}
    end
  end

  @impl DancingDots.Animation
  def handle_frame(dot, frame_number, opts) do
    velocity = opts |> Keyword.get(:velocity)
    %{dot | radius: dot.radius + (frame_number - 1) * velocity}
  end
end
