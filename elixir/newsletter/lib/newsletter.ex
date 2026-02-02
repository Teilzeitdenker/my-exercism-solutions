defmodule Newsletter do
  def read_emails(path) do
    {result, data} = File.read(path)
    case result do
      :ok    ->
        case data do
          "" -> []
          _  -> String.split(data)
        end
      :error -> []
    end
  end

  def open_log(path) do
    File.open!(path, [:write])
  end

  def log_sent_email(pid, email) do
    IO.puts(pid, email)  # automatically adds a newline character
    # IO.write(pid, email <> "\n")
  end

  def close_log(pid) do
    File.close(pid)
  end

  def send_newsletter(emails_path, log_path, send_fun) do
    emails = read_emails(emails_path)
    pid = open_log(log_path)
    Enum.map(emails, fn email ->
      case send_fun.(email) do
        :ok -> log_sent_email(pid, email)
        _   -> nil
      end
    end)
    close_log(pid)
  end
end
