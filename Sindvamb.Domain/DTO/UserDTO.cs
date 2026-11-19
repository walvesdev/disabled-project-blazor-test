using System.ComponentModel.DataAnnotations;

namespace Sindvamb.Domain.DTO
{
    public class UserDTO
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public UserDTO()
        {
            LoginHistory = new HashSet<LoginHistory>();
            PasswordHistory = new HashSet<PasswordHistory>();
            Audit = new HashSet<Audit>();
            SpecialNeeds = new HashSet<SpecialNeedsDTO>();
            FileControl = new HashSet<FileControlDTO>();

        }

        public int? Matricula { get; set; }

        public string Name { get; set; }

        public string? SocialName { get; set; }

        public byte[]? Avatar { get; set; }

        public string? Surname { get; set; }

        //Sexo
        public string Gender { get; set; }

        //Naturalidade
        public string? Birthplace { get; set; }

        public string? CivilStatus { get; set; }

        public string? FatherName { get; set; }

        public string MotherName { get; set; }

        public string Nationality { get; set; }

        public string RG { get; set; }

        [Required(ErrorMessage = "Por favor insira a UF do seu RG")]
        public string UFIssuingBody { get; set; }

        [Required(ErrorMessage = "Por favor insira seu CPF")]
        public string CpfCnpj { get; set; }

        public string Email { get; set; }

        public string? Password { get; set; }

        public string? Race { get; set; }

        public string? Profession { get; set; }

        [Required(ErrorMessage = "Por favor insira  seu Celular")]
        public string CellPhone { get; set; }

        public string? HomePhone { get; set; }

        public string? BusinessPhone { get; set; }

        public bool HasSpecialNeeds { get; set; }

        public string? SpecialNeedsOther { get; set; }

        public bool IsSystem { get; set; }

        public bool? IsCustomer { get; set; }

        public string? SecurelyPhrase { get; set; }

        public int? LoginAttemps { get; set; }

        public bool? PasswordPolicyEnabled { get; set; }

        [Required(ErrorMessage = "Por favor insira sua Data de Nascimento")]
        public DateTime? Birthdate { get; set; } = null;

        public DateTime? LastLoginAt { get; set; }

        public DateTime? LastPasswordChangedAt { get; set; }
        
        public Guid? PasswordResetToken { get; set; }

        public virtual Guid? EducationDegreeId { get; set; }
        public virtual EducationDegreeDTO EducationDegree { get; set; } = new();

        public Guid? RoleId { get; set; }
        
        public Role Role { get; set; } = new();

        public Guid? UserStatusId { get; set; }
        public virtual UserStatusDTO UserStatus { get; set; } = new();

        public Guid? AddressId { get; set; }

        //[ValidateComplexType]
        public AddressDTO Address { get; set; } = new();

        public virtual Guid? PartnerUnitId { get; set; } 
        public virtual PartnerUnitDTO PartnerUnit { get; set; } = new();

        public virtual ICollection<FileControlDTO> FileControl { get; set; } 

        public virtual ICollection<SpecialNeedsDTO> SpecialNeeds { get; set; }

        public virtual ICollection<LoginHistory> LoginHistory { get; set; }

        public virtual ICollection<PasswordHistory> PasswordHistory { get; set; }

        public virtual ICollection<Audit> Audit { get; set; }
    }
}
