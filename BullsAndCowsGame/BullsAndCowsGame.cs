﻿// <summary>Contains the BullsAndCowsGame class.</summary>
//-----------------------------------------------------------------------
// <copyright file="BullsAndCowsGame.cs" company="Bulls-And-Cows-1">
//     Everything is copyrighted.
// </copyright>
//-----------------------------------------------------------------------
namespace BullsAndCows
{
    using System;
    using Common;
    using Contracts;

    /// <summary>
    /// The class holding the main logic of the game.
    /// </summary>
    public class BullsAndCowsGame : IDisposable
    {
        /// <summary>
        /// The helping number of the users using the "help" command.
        /// </summary>
        private char[] helpingNumber;

        /// <summary>
        /// The name of the current player.
        /// </summary>
        private string playerName;

        /// <summary>
        /// Initializes a new instance of the BullsAndCowsGame class.
        /// </summary>
        /// <param name="randomNumberProvider">The randomNumberProvider used to generate random numbers.</param>
        /// <param name="scoreboard">The scoreboard used to hold player's scores.</param>
        /// <param name="printer">The printer used for printing messages and different objects.</param>
        /// <param name="commandProcessor">The first command processor in the chain of responsibility.</param>
        public BullsAndCowsGame(
            IRandomNumberProvider randomNumberProvider,
            ScoreBoard scoreboard,
            IPrinter printer,
            ICommandProcessor commandProcessor)
        {
            this.RandomNumberProvider = randomNumberProvider;
            this.ScoreBoard = scoreboard;
            this.Printer = printer;
            this.CommandProcessor = commandProcessor;
        }

        /// <summary>
        /// Gets the RandomNumberProvider.
        /// </summary>
        /// <value>Holds a random number provider.</value>
        public IRandomNumberProvider RandomNumberProvider { get; private set; }

        /// <summary>
        /// Gets the scoreboard for the current game.
        /// </summary>
        /// <value>Holds the scoreboard.</value>
        public ScoreBoard ScoreBoard { get; private set; }

        /// <summary>
        /// Gets the printer used for printing messages and scores.
        /// </summary>
        /// <value>Holds an IPrinter.</value>
        public IPrinter Printer { get; private set; }

        /// <summary>
        /// Gets the first command processor.
        /// </summary>
        /// <value>Holds a command processor instance.</value>
        public ICommandProcessor CommandProcessor { get; private set; }

        /// <summary>
        /// Gets the number which has to be guessed (generated by the RandomNumberProvider).
        /// </summary>
        /// <value>Holds a string with the number which has to be guessed.</value>
        public string NumberForGuess { get; private set; }

        /// <summary>
        /// Gets or sets the number of times the user took advantage of the "help" command.
        /// </summary>
        /// <value>Holds the number of times the user cheated.</value>
        public int CheatAttemptCounter { get; set; }

        /// <summary>
        /// Gets the number of times the user suggested a number.
        /// </summary>
        /// <value>Holds the number of times the user tried to guess.</value>
        public int GuessAttemptCounter { get; private set; }

        /// <summary>
        /// Gets or sets a value indicating whether the number is guessed correctly.
        /// </summary>
        /// <value>Holds a boolean.</value>
        public bool IsGuessed { get; set; }

        /// <summary>
        /// Prints the start messages, asks user for name and initializes the game command loop.
        /// </summary>
        public void Play()
        {
            if (this.playerName == null)
            {
                this.Printer.PrintMessage(MessageType.EnterName);
                this.playerName = Console.ReadLine();
            }

            this.Printer.PrintMessage(MessageType.Welcome);
            this.Printer.PrintMessage(MessageType.GameRules);

            string input;

            while (!this.IsGuessed)
            {
                this.Printer.PrintMessage(MessageType.Command);
                input = Console.ReadLine();
                this.GuessAttemptCounter++;

                this.CommandProcessor.ProcessCommand(input, this);
            }

            ScoreBoard.AddPlayerScore(new PlayerScore(this.playerName, this.GuessAttemptCounter));
            this.Printer.PrintLeaderBoard(ScoreBoard.LeaderBoard);

            Console.ReadLine();
        }

        /// <summary>
        /// Sets the initial properties and fields used in the game.
        /// </summary>
        public void Initialize()
        {
            this.NumberForGuess = this.RandomNumberProvider.GenerateNumber(1000, 9999).ToString();
            this.GuessAttemptCounter = 0;
            this.CheatAttemptCounter = 0;
            this.IsGuessed = false;
            this.helpingNumber = new char[] { 'X', 'X', 'X', 'X' };
        }

        /// <summary>
        /// Reveals one digit in the helpingNumber and prints it to the user.
        /// </summary>
        public void RevealDigit()
        {
            bool reveald = false;
            
            while (!reveald && this.CheatAttemptCounter < 4)
            {
                int digitForReveal = this.RandomNumberProvider.GenerateNumber(0, 3);

                if (this.helpingNumber[digitForReveal] == 'X')
                {
                    this.helpingNumber[digitForReveal] =
                    this.NumberForGuess[digitForReveal];
                    reveald = true;
                }
            }

            this.Printer.PrintHelpingNumber(this.helpingNumber);
        }

        /// <summary>
        /// Disposes of the old game settings in order to be ready for a new game.
        /// </summary>
        public void Dispose()
        {
            this.Initialize();
        }
    }
}
