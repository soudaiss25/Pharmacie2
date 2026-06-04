using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Pharmacie2.Utils
{
    public static class Utils
    {
        /// <summary>
        /// Vérifie si une chaîne contient uniquement des lettres (alphabétique)
        /// </summary>
        public static bool IsAlphabetic(string input)
        {
            return Regex.IsMatch(input, @"^[A-Za-zÀ-ÖØ-öø-ÿ]+$");
        }

        /// <summary>
        /// Vérifie si une chaîne est un email valide
        /// </summary>
        public static bool IsValidEmail(string email)
        {
            return Regex.IsMatch(email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$");
        }

        /// <summary>
        /// Vérifie si une chaîne est un numéro de téléphone valide au Sénégal
        /// </summary>
        public static bool IsValidPhoneNumber(string phoneNumber)
        {
            // Format attendu : 7X XXX XX XX (sans espace ou avec) ou +2217X XXXXXXX ou 002217X XXXXXXX
            return Regex.IsMatch(phoneNumber, @"^(?:\+221|00221)?7[0678]\d{7}$");
        }
        /// <summary>
        /// fonction qui permet de Deconcatener 2 string nom et prenom a partir du dernier espace
        /// </summary>
        /// <param name="nomPrenom"></param>
        /// <returns></returns>
        public static (string, string) Deconcatener(string nomPrenom)
        {
            if (string.IsNullOrWhiteSpace(nomPrenom))
                return ("", ""); // Retourne des valeurs vides si la chaîne est nulle ou vide
            string[] parts = nomPrenom.Split(new char[] { ' ' }, 2); // Coupe la chaîne en deux parties au maximum

            string nom = parts.Length > 0 ? parts[0] : "";
            string prenom = parts.Length > 1 ? parts[1] : "";

            return (nom, prenom);
        }

    }
}
