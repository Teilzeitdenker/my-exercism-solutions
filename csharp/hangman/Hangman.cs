using System;
using System.Collections.Immutable;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Reactive.Subjects;

public class HangmanState
{
    public string MaskedWord { get; }
    public ImmutableHashSet<char> GuessedChars { get; }
    public int RemainingGuesses { get; }
    public HangmanState(string maskedWord, ImmutableHashSet<char> guessedChars, int remainingGuesses) =>
        (MaskedWord, GuessedChars, RemainingGuesses) = (maskedWord, guessedChars, remainingGuesses);
}
public class TooManyGuessesException : Exception { }
public class Hangman
{
    private string _word;
    private BehaviorSubject<HangmanState> _subject;
    public IObservable<HangmanState> StateObservable { get => _subject.AsObservable(); }
    public IObserver<char> GuessObserver { get => Observer.Create<char>(OnNextChar); }
    public Hangman(string word) =>
        (_word, _subject) = (word, new(new(new('_', word.Length), ImmutableHashSet<char>.Empty, 9)));
    private void OnNextChar(char ch)
    {
        var state = _subject.Value;
        if (state.RemainingGuesses == 0) { _subject.OnError(new TooManyGuessesException()); return; }
        if (state.GuessedChars.Contains(ch) || !_word.Contains(ch))
            _subject.OnNext(new(state.MaskedWord, state.GuessedChars.Add(ch), state.RemainingGuesses - 1));
        else
        {
            var newHashSet = state.GuessedChars.Add(ch);
            string newMaskedWord = new(_word.Select(l => newHashSet.Contains(l) ? l : '_').ToArray());
            if (newMaskedWord == _word) _subject.OnCompleted();
            else _subject.OnNext(new(newMaskedWord, newHashSet, state.RemainingGuesses));
        }
    }
}