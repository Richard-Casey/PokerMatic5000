using System;
using System.Collections.Generic;
using System.Linq;

namespace PM5000
{
    // Making an enumeration for card ranks
    enum Rank
    {
        Two = 2, Three, Four, Five, Six, Seven, Eight, Nine, Ten, Jack, Queen, King, AceLow, AceHigh
    }

    //Making an enumeration for card suits
    enum Suit
    {
        Diamond = 'D', Spade = 'S', Club = 'C', Heart = 'H'
    }

    class Program
    {
        static void Main(string[] args)
        {
            bool isRunning = true;

            while (isRunning)
            {
                // Promting the user to input
                Console.WriteLine("Enter five cards seperated by a comma (Format is AH, 2H, 3H, 4H, 5H, 6H, 7H, 8H, 9H, TH, JH, QH, KH) or type 'EXIT' to quit: ");

                // Checking if user wants to exit
                if (Console.KeyAvailable && Console.ReadKey(true).Key == ConsoleKey.Escape)
                {
                    isRunning = false;
                    continue;
                }

                string input = Console.ReadLine();

                // Additional Check to see if user wants to exit
                if (input.ToUpper() == "EXIT")
                {
                    isRunning = false;
                    continue;
                }

                List<(Rank, Suit)> cards = ParseInput(input);

                if (cards != null)
                {
                    // Determine poker rank of the inputted cards
                    string pokerRank = DeterminePokerRank(cards);

                    // Set the text color to Cyan to make it a bit more readable
                    Console.ForegroundColor = ConsoleColor.Cyan;

                    // Display Rank result
                    Console.WriteLine("Poker Rank: " + pokerRank);

                    // Reset text color to default
                    Console.ResetColor();
                }
            }
        }

        // Parse the user input into a list of card tuples
        static List<(Rank, Suit)> ParseInput(string input)
        {
            List<(Rank, Suit)> cards = new List<(Rank, Suit)>();

            string[] cardStrings = input.Split(',');

            foreach (string cardString in cardStrings)
            {
                string trimmedCardString = cardString.Trim();

                if (trimmedCardString.Length != 2 && trimmedCardString.Length != 3)
                {
                    Console.WriteLine("Invalid card format: " + trimmedCardString);
                    return null; // Handle the error appropriately
                }

                char rankChar = char.ToUpper(trimmedCardString[0]);
                char suitChar = char.ToUpper(trimmedCardString[1]);

                Rank rank;
                Suit suit;

                // Recognize face cards and handle special cases
                if (rankChar == 'T') rank = Rank.Ten;
                else if (rankChar == 'J') rank = Rank.Jack;
                else if (rankChar == 'Q') rank = Rank.Queen;
                else if (rankChar == 'K') rank = Rank.King;
                else if (rankChar == 'A' || rankChar == '1') rank = Rank.AceLow;
                else if (!Enum.TryParse(rankChar.ToString(), out rank))
                {
                    Console.WriteLine("Invalid rank character: " + rankChar);
                    return null; // Handle the error appropriately
                }



                if (suitChar != 'D' && suitChar != 'S' && suitChar != 'C' && suitChar != 'H')
                {
                    Console.WriteLine("Invalid suit character: " + suitChar);
                    return null; // Handle the error appropriately
                }

                cards.Add((rank, (Suit)suitChar));
            }

            return cards;
        }

        // Determine the poker rank of the entered cards
        static string DeterminePokerRank(List<(Rank, Suit)> cards)
        {
            
            // Sort the cards by rank in ascending order
            cards.Sort((card1, card2) => card1.Item1.CompareTo(card2.Item1));

            // Check for various poker ranks in descending order of priority

            // Check for Royal Flush
            if (IsRoyalFlush(cards))
            {
                return "Royal Flush";
            }

            // Check for Straight Flush
            if (IsStraightFlush(cards))
            {
                return "Straight Flush";
            }

            // Check for Flush
            if (IsFlush(cards))
            {
                return "Flush";
            }

            // Check for Four of a Kind
            if (IsFourOfAKind(cards))
            {
                return "Four of a Kind";
            }

            // Check for Full House
            if (IsFullHouse(cards))
            {
                return "Full House";
            }

            // Check for Three of a Kind
            if (IsThreeOfAKind(cards))
            {
                return "Three of a Kind";
            }

            // Check for Straight
            if (DetermineIsStraight(cards))
            {
                return "Straight";
            }

            // Check for Two Pair
            if (IsTwoPair(cards))
            {
                return "Two Pair";
            }

            // Check for Pair
            if (IsPair(cards))
            {
                return "Pair";
            }

            // If none of the above conditions are met, it's a High Card
            return "High Card";
        }

        // Check if the cards form a straight (sequential ranks)
        static bool IsStraight(List<(Rank, Suit)> cards)
        {
            // Create a list of distinct ranks to remove duplicates
            var distinctRanks = cards.Select(card => card.Item1).Distinct().ToList();

            // Check if the distinct ranks can be ordered sequentially
            int count = distinctRanks.Count;

            for (int i = 0; i < count - 1; i++)
            {
                if (distinctRanks[i + 1] - distinctRanks[i] != 1)
                {
                    return false;
                }
            }

            return true;
        }

        // Check if the cards form a Royal Straight (sequential ranks with Ace as high)
        static bool IsRoyalStraight(List<(Rank, Suit)> cards)
        {
            // Sort the cards by rank in ascending order
            cards.Sort((card1, card2) => card1.Item1.CompareTo(card2.Item1)); // Sort by rank in ascending order

            bool isLowAce = cards[0].Item1 == Rank.Two && cards[cards.Count - 1].Item1 == Rank.AceLow;
            bool isHighAce = cards[0].Item1 == Rank.AceHigh && cards[cards.Count - 1].Item1 == Rank.Ten;

            if (isLowAce || isHighAce)
            {
                // Check for Royal Straight (Ten, Jack, Queen, King, Ace)
                return cards.Select(card => card.Item1).SequenceEqual(new[] { Rank.Ten, Rank.Jack, Rank.Queen, Rank.King, Rank.AceHigh });
            }

            return false;
        }

        // Check if the cards form a Royal Flush (Royal Straight with the same suit)
        static bool IsRoyalFlush(List<(Rank, Suit)> cards)
        {
            // Check if the ranks contain Ten, Jack, Queen, King, and Ace
            var requiredRanks = new HashSet<Rank> { Rank.Ten, Rank.Jack, Rank.Queen, Rank.King, Rank.AceHigh };
            var ranks = cards.Select(card => card.Item1);

            if (requiredRanks.All(rank => ranks.Contains(rank)))
            {
                // Check if all the suits are the same (Flush condition)
                bool isFlush = cards.All(card => card.Item2 == cards[0].Item2);

                return isFlush; // It's a Royal Flush only if it's also a Flush
            }

            return false;
        }

        // Check if the cards form a Straight Flush (straight ranks with the same suit)
        static bool IsStraightFlush(List<(Rank, Suit)> cards)
        {
            // Check if all the suits are the same
            bool isFlush = cards.All(card => card.Item2 == cards[0].Item2);

            if (!isFlush) return false;

            // Sort the cards by rank in ascending order
            cards.Sort((card1, card2) => card1.Item1.CompareTo(card2.Item1));

            // Check if the ranks form a sequential order within a spread of 5 cards
            int count = cards.Count;

            // Check for a Royal Flush (Ten, Jack, Queen, King, Ace in any order and same suit)
            if (cards.Any(card => card.Item1 == Rank.Ten) &&
                cards.Any(card => card.Item1 == Rank.Jack) &&
                cards.Any(card => card.Item1 == Rank.Queen) &&
                cards.Any(card => card.Item1 == Rank.King) &&
                cards.Any(card => card.Item1 == Rank.AceHigh))
            {
                if (cards.All(card => card.Item2 == Suit.Heart) || 
                    cards.All(card => card.Item2 == Suit.Diamond) || 
                    cards.All(card => card.Item2 == Suit.Club) || 
                    cards.All(card => card.Item2 == Suit.Spade)) 
                {
                    return true;
                }
            }

            for (int i = 0; i < count - 1; i++)
            {
                if (cards[i + 1].Item1 - cards[i].Item1 != 1 &&
                    !(i == 0 && cards[i].Item1 == Rank.AceLow && cards[i + 1].Item1 == Rank.Five)) // Account for Ace as low
                {
                    return false;
                }
            }

            return true;
        }

        // Check if all the cards have the same suit (Flush)
        static bool IsFlush(List<(Rank, Suit)> cards)
        {
            // Check if all the suits are the same
            return cards.All(card => card.Item2 == cards[0].Item2);
        }

        // Check if the cards contain four of the same rank (Four of a Kind)
        static bool IsFourOfAKind(List<(Rank, Suit)> cards)
        {
            // Check for 4 of the same Rank
            for (int i = 0; i < cards.Count - 3; i++)
            {
                if (cards[i].Item1 == cards[i + 1].Item1 && cards[i].Item1 == cards[i + 2].Item1 && cards[i].Item1 == cards[i + 3].Item1)
                {
                    return true;
                }
            }

            return false;
        }

        // Check if the cards contain a Full House (Three of a Kind and a Pair)
        static bool IsFullHouse(List<(Rank, Suit)> cards)
        {
            // Create a dictionary to count the occurrences of each rank
            var rankCounts = new Dictionary<Rank, int>();

            foreach (var card in cards)
            {
                if (!rankCounts.ContainsKey(card.Item1))
                {
                    rankCounts[card.Item1] = 1;
                }
                else
                {
                    rankCounts[card.Item1]++;
                }
            }

            // Check if there is a Three of a Kind and a Pair
            bool hasThreeOfAKind = rankCounts.ContainsValue(3);
            bool hasPair = rankCounts.ContainsValue(2);

            return hasThreeOfAKind && hasPair;
        }

        // Check if the cards contain Three of a Kind
        static bool IsThreeOfAKind(List<(Rank, Suit)> cards)
        {
            var rankCounts = cards.GroupBy(card => card.Item1).Select(group => group.Count());

            return rankCounts.Any(count => count == 3); // If there is a group of 3 cards with the same rank, it's Three of a Kind
        }

        // Check if the cards contain Two Pairs
        static bool IsTwoPair(List<(Rank, Suit)> cards)
        {
            // Check for Two Pair
            int pairCount = 0;

            for (int i = 0; i < cards.Count - 1; i++)
            {
                if (cards[i].Item1 == cards[i + 1].Item1)
                {
                    pairCount++;
                    i++; // Skip the next card of the same rank
                }
            }

            return pairCount == 2;
        }

        // Check if the cards contain a Pair
        static bool IsPair(List<(Rank, Suit)> cards)
        {
            // Check for a Pair
            for (int i = 0; i < cards.Count - 1; i++)
            {
                if (cards[i].Item1 == cards[i + 1].Item1)
                {
                    return true;
                }
            }

            return false;
        }

        // Check if the cards form a regular Straight (with Ace as both high and low)
        static bool DetermineIsStraight(List<(Rank, Suit)> cards)
        {
            // Sort the cards by rank in ascending order
            cards.Sort((card1, card2) => card1.Item1.CompareTo(card2.Item1));

            // Check for regular straight
            for (int i = 0; i < cards.Count - 1; i++)
            {
                if (cards[i].Item1 + 1 != cards[i + 1].Item1)
                {
                    // Check for Ace as low (Ace, 2, 3, 4, 5)
                    if (i == 0 && cards[i].Item1 == Rank.AceLow && cards[i + 1].Item1 == Rank.Two)
                    {
                        continue;
                    }

                    // Check for Ace as high (10, Jack, Queen, King, Ace)
                    if (i == 0 && cards[i].Item1 == Rank.Ten && cards[i + 1].Item1 == Rank.AceHigh)
                    {
                        continue;
                    }

                    return false;
                }
            }

            return true;
        }

        // Check if the cards form a Royal Straight (sequential ranks with Ace as high)
        static bool DetermineIsRoyalStraight(List<(Rank, Suit)> cards)
        {
            cards.Sort((card1, card2) => card1.Item1.CompareTo(card2.Item1)); // Sort by rank in ascending order

            bool isLowAce = cards[0].Item1 == Rank.Two && cards[cards.Count - 1].Item1 == Rank.AceLow;
            bool isHighAce = cards[0].Item1 == Rank.AceHigh && cards[cards.Count - 1].Item1 == Rank.King;

            if (isLowAce || isHighAce)
            {
                // Check for Royal Straight (Ten, Jack, Queen, King, Ace)
                return cards.Select(card => card.Item1).SequenceEqual(new[] { Rank.Ten, Rank.Jack, Rank.Queen, Rank.King, Rank.AceHigh });
            }

            return false;
        }

        // Check if the cards have the same suit (Flush)
        static bool DetermineIsFlush(List<(Rank, Suit)> cards)
        {
            return cards.All(card => card.Item2 == cards[0].Item2);
        }

        // Check if the cards form a Royal Flush (Royal Straight with the same suit)
        static bool DetermineIsRoyalFlush(List<(Rank, Suit)> cards)
        {
            var requiredRanks = new HashSet<Rank> { Rank.Ten, Rank.Jack, Rank.Queen, Rank.King, Rank.AceHigh };

            foreach (var card in cards)
            {
                if (!requiredRanks.Contains(card.Item1))
                {
                    return false; // If any required rank is missing, it's not a Royal Flush
                }
            }

            return DetermineIsFlush(cards); // Check for flush as well
        }

    }
}
