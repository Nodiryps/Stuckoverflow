using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace prid1920_g10.Models
{
    public class User : IValidatableObject
    {
        private const int PasswordAndPseudoMinLength = 3;
        private const int PasswordAndPseudoMaxLength = 10;
        private const int NameMinLength = 3;
        private const int NameMaxLength = 50;

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required(ErrorMessage = "Required")]
        // [MinLength(PasswordAndPseudoMinLength, ErrorMessage = "Minimum 3 characters")]
        // [MaxLength(PasswordAndPseudoMaxLength, ErrorMessage = "Maximum 10 characters")]
        public string Pseudo { get; set; }

        [Required(ErrorMessage = "Required")]
        // [MinLength(PasswordAndPseudoMinLength, ErrorMessage = "Minimum 3 characters")]
        // [MaxLength(PasswordAndPseudoMaxLength, ErrorMessage = "Maximum 10 characters")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Required")]
        public string Email { get; set; }

        // [MinLength(NameMinLength, ErrorMessage = "Minimum 3 characters")]
        // [MaxLength(NameMaxLength, ErrorMessage = "Maximum 50 characters")]
        public string FirstName { get; set; }

        // [MinLength(NameMinLength, ErrorMessage = "Minimum 3 characters")]
        // [MaxLength(NameMaxLength, ErrorMessage = "Maximum 50 characters")]
        public string LastName { get; set; }

        public DateTime? BirthDate { get; set; }

        public int? Age
        {
            get
            {
                if (!BirthDate.HasValue)
                    return null;
                var today = DateTime.Today;
                var age = today.Year - BirthDate.Value.Year;
                if (BirthDate.Value.Date > today.AddYears(-age)) age--;
                return age;
            }
        }

        [Required(ErrorMessage = "Required")]
        public int Reputation { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var currContext = validationContext.GetService(typeof(DbContext)) as G10Context;
            Debug.Assert(currContext != null);

            PseudoValidations(currContext);
            NameValidations();
            EmailValidations();
            BirthDateValidations();
            ReputationValidations();
            yield return new ValidationResult("");
        }

        IEnumerable<ValidationResult> ReputationValidations()
        {
            string pattern = "^[\\d]{1,2}$";
            bool isValid = Regex.IsMatch(Reputation.ToString(), pattern);
            if (!isValid && (Reputation < 0 || Reputation > 5))
                yield return new ValidationResult("Reputation < 0", new[] { nameof(Reputation) });
        }

        IEnumerable<ValidationResult> BirthDateValidations()
        {
            if (BirthDate.HasValue && BirthDate.Value.Date > DateTime.Today)
                yield return new ValidationResult("Can't be born in the future in this reality", new[] { nameof(BirthDate) });
            if (Age.HasValue && Age < 18)
                yield return new ValidationResult("Must be 18 years old", new[] { nameof(BirthDate) });
        }

        IEnumerable<ValidationResult> EmailValidations()
        {
            string emailPattern = "^\\w+@[a-zA-Z_]+?\\.[a-zA-Z]{2,3}$";
            bool isEmailValid = Regex.IsMatch(Email, emailPattern);

            if (!isEmailValid)
                yield return new ValidationResult("Invalid email", new[] { nameof(Email) });
        }

        IEnumerable<ValidationResult> NameValidations()
        {
            string namePattern = "^[A-Za-z]+$";
            bool isFirstNameValid = Regex.IsMatch(FirstName, namePattern);
            bool isLastNameValid = Regex.IsMatch(LastName, namePattern);

            if (!isFirstNameValid)
                yield return new ValidationResult("Invalid firstname (only letters)", new[] { nameof(FirstName) });
            if (!isLastNameValid)
                yield return new ValidationResult("Invalid lastname (only letters)", new[] { nameof(LastName) });
            if (!IsFirstNameLengthValid())
                yield return new ValidationResult("Invalid firstname (" + NameMinLength + "-" + NameMaxLength + " char)", new[] { nameof(FirstName) });
            if (!IsLastNameLengthValid())
                yield return new ValidationResult("Invalid lastname (" + NameMinLength + "-" + NameMaxLength + " char)", new[] { nameof(LastName) });
        }

        IEnumerable<ValidationResult> PseudoValidations(G10Context c)
        {
            string pseudoPattern = "^[\\W\\d_]+$";
            bool isPseudoValid = Regex.IsMatch(Pseudo, pseudoPattern);

            if (!IsPseudoUnique(Pseudo, c))
                yield return new ValidationResult("Pseudo already exists", new[] { nameof(Pseudo) });
            if (!isPseudoValid)
                yield return new ValidationResult("Invalid pseudo: letters/numbers/underscores allowed (Should begin by a letter)", new[] { nameof(Pseudo) });
            if (!IsPseudoLengthValid())
                yield return new ValidationResult("Invalid pseudo (" + PasswordAndPseudoMinLength + " - " + PasswordAndPseudoMaxLength + " char)", new[] { nameof(Pseudo) });
            if (!IsPseudoLengthValid())
                yield return new ValidationResult("Invalid password (" + PasswordAndPseudoMinLength + "-" + PasswordAndPseudoMaxLength + " char)", new[] { nameof(Password) });
        } 

        static bool IsPseudoUnique(string pseudo, G10Context c)
        {
            return (from u in c.Users
                    where u.Pseudo == pseudo
                    select u).FirstOrDefault() == null;
        }

        bool IsPasswordLengthValid()
        {
            return Password.Length >= PasswordAndPseudoMinLength && Password.Length <= PasswordAndPseudoMaxLength;
        }

        bool IsLastNameLengthValid()
        {
            return LastName.Length >= NameMinLength && LastName.Length <= NameMaxLength;
        }

        bool IsFirstNameLengthValid()
        {
            return FirstName.Length >= NameMinLength && FirstName.Length <= NameMaxLength;
        }

        bool IsPseudoLengthValid()
        {
            return Pseudo.Length >= PasswordAndPseudoMinLength && Pseudo.Length <= PasswordAndPseudoMaxLength;
        }
    }
}