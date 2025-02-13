﻿using DSharpPlus.Entities;
using Eremite.Actions;
using Eremite.Data;
using Eremite.Data.DiscordData;
using Eremite.Services;
using Newtonsoft.Json;

namespace Eremite
{
    public static class Extensions
    {
        public static string GetNormalTime(this TimeSpan time) => time.ToString(@"dd\.hh\:mm\:ss");

        public static void LogStatus(this string rawJson, string fileName = "")
        {
            bool isCorrupted = rawJson == null || rawJson.Length <= 0;

            string corruptedMessage = $"[ERROR] Couldnt load {fileName}";
            string successMessage = $"[SUCCESS] {fileName} loaded successfully";

            Console.WriteLine(isCorrupted ? corruptedMessage : successMessage);
        }

        public static string ToCharacterList(this List<Character> characters, UserData user)
        {
            if (characters == null || characters.Count <= 0) return user.GetText(SetCharacterAction.noMainCharacter);

            string charactersInInventory = string.Empty;
            foreach (var character in characters)
            {
                charactersInInventory = $"{charactersInInventory} {user.GetText($"character.{character.CharacterId}.name")} <{character.StarsRarity}{Localization.StarEmoji}> ";
            }

            return charactersInInventory;
        }

        public static Character GetHighestTier(this List<Character> characters)
        {
            var highestTier = characters[0];
            foreach (var character in characters)
            {
                if (character.StarsRarity < highestTier.StarsRarity) continue;
                highestTier = character;
            }

            return highestTier;
        }

        public static DiscordColor GetCorrespondingColor(this Character character)
        {
            switch (character.StarsRarity)
            {
                case 3: return DiscordColor.Green;
                case 4: return DiscordColor.Aquamarine;
                case 5: return DiscordColor.Orange;
                case 10: return DiscordColor.Red;
                default: return DiscordColor.White;
            }
        }

        public static string GetCorrespondingQuery(this QueryElement element, UserData user)
        {
            switch (element)
            {
                case QueryElement.Username: return $"`username`='{user.Username}'";
                case QueryElement.Wallet: return $"`wallet`='{JsonConvert.SerializeObject(user.Wallet)}'";
                case QueryElement.Characters: return $"`characters`='{JsonConvert.SerializeObject(user.Characters)}'";
                case QueryElement.EquippedCharacter: return $"`equippedcharacter`='{JsonConvert.SerializeObject(user.EquippedCharacter)}'";
                case QueryElement.Stats: return $"`stats`='{JsonConvert.SerializeObject(user.Stats)}'";
                case QueryElement.Events: return $"`events`='{JsonConvert.SerializeObject(user.Events)}'";
                case QueryElement.Inventory: return $"`inventory`='{JsonConvert.SerializeObject(user.Inventory)}'";
                default: return $"`userid='{user.UserId}',`username`='{user.Username}',`wallet`='{JsonConvert.SerializeObject(user.Wallet)}'," +
                        $"`characters`='{JsonConvert.SerializeObject(user.Characters)}',`equippedcharacter`='{JsonConvert.SerializeObject(user.EquippedCharacter)}'," +
                        $"`stats`='{JsonConvert.SerializeObject(user.Stats)}',`events`='{JsonConvert.SerializeObject(user.Events)}',`inventory`='{JsonConvert.SerializeObject(user.Inventory)}'";
            }
        }

        public static List<Character> GetCharactersPoolByStar(this List<Character> allCharacters, int star)
        {
            return allCharacters.FindAll(character => character.StarsRarity == star);
        }

        public static int GetStarByChance(this Dictionary<int, int> chances)
        {
            var randomPercent = Random.Shared.Next(0, 101);
            int starRarity = 3;

            foreach (var percentage in chances)
            {
                if (randomPercent > percentage.Value) continue;
                if (starRarity > percentage.Key) continue;

                starRarity = percentage.Key;
            }

            return starRarity;
        }
    }
}
