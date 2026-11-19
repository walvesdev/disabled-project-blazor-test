using System.ComponentModel.DataAnnotations;
using System.Reflection;
using Sindvamb.Domain.DTO;

namespace Sindvamb.Domain.Entities
{

    public class User : EntityBaseAudit<Guid>
    {
        public User()
        {
            LoginHistory = new HashSet<LoginHistory>();
            PasswordHistory = new HashSet<PasswordHistory>();
            Audit = new HashSet<Audit>();
            SpecialNeeds = new HashSet<SpecialNeeds>();
            FileControl = new HashSet<FileControl>();

        }

        public int? Matricula { get; set; }

        public string Name { get; set; }

        public string? SocialName { get; set; }

        public string? Surname { get; set; }

        //Sexo
        public string? Gender { get; set; }

        //Naturalidade
        public string? Birthplace { get; set; }

        public string? CivilStatus { get; set; }

        public string? FatherName { get; set; }

        public string? MotherName { get; set; }

        public string? Nationality { get; set; }

        public string? RG { get; set; }

        public string? UFIssuingBody { get; set; }

        public string? CpfCnpj { get; set; }

        public string? Email { get; set; }

        public string? Password { get; set; }

        public string? Race { get; set; }

        public string? Profession { get; set; }

        public string? CellPhone { get; set; }

        public string? HomePhone { get; set; }

        public string? BusinessPhone { get; set; }

        public bool HasSpecialNeeds { get; set; }

        public string? SpecialNeedsOther { get; set; }

        public bool IsSystem { get; set; }

        public bool? IsCustomer { get; set; }

        public string? SecurelyPhrase { get; set; }

        public int? LoginAttemps { get; set; }

        public bool? PasswordPolicyEnabled { get; set; }

        public DateTime Birthdate { get; set; }

        public DateTime? LastLoginAt { get; set; }

        public DateTime? LastPasswordChangedAt { get; set; }
        public Guid? PasswordResetToken { get; set; }

        public virtual Guid? EducationDegreeId { get; set; }
        public virtual EducationDegree EducationDegree { get; set; }

        public virtual Guid? RoleId { get; set; }
        public virtual Role Role { get; set; }

        public virtual Guid? UserStatusId { get; set; }
        public virtual UserStatus UserStatus { get; set; }

        public virtual Guid? AddressId { get; set; }
        public virtual Address Address { get; set; }

        public virtual Guid? PartnerUnitId { get; set; }
        public virtual PartnerUnit PartnerUnit { get; set; }

        public virtual ICollection<SpecialNeeds> SpecialNeeds { get; set; }

        public virtual ICollection<LoginHistory> LoginHistory { get; set; }

        public virtual ICollection<PasswordHistory> PasswordHistory { get; set; }

        public virtual ICollection<Audit> Audit { get; set; }

        public virtual ICollection<FileControl> FileControl { get; set; }

        public User GetUserWithouFilesStream()
        {
            if (FileControl is null) return this; 

            foreach (var item in FileControl)
            {
                item.FileArray = null;
            }

            return this;
        }


    }
}
