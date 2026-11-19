using Newtonsoft.Json;
using Sindvamb.Application.Host.SeedData.Models;
using Sindvamb.Domain.Entities;
using Sindvamb.Domain.Enums;
using Sindvamb.Events.BuildingBlocks.Mediator;
using Sindvamb.Repository.Contexts;
using Sindvamb.Repository.Interfaces;
using Sindvamb.Repository.ORM;
using Sindvamb.Security.Configurations;
using Sindvamb.Security.Enums;
using Sindvamb.Utils.Extension;
using Sindvamb.Utils.Helpers;

namespace Sindvamb.Application.Host.SeedData;

public class Seed
{
    public IMediatorHandler MediatorHandler { get; }
    public Guid UserId { get; set; } = Guid.Parse("da1ab9a7-e11c-4a50-b4a3-1307fa8c527a");
    public ISindvambUnitOfWork UnitOfWork { get; set; }

    public Seed(ISindvambUnitOfWork unitOfWork, IMediatorHandler mediatorHandler)
    {
        UnitOfWork = unitOfWork;
        MediatorHandler = mediatorHandler;
    }

    public async Task Start()
    {
        try
        {
            await CreateContact();
            //await AddCityAndCounty();
            await CreateRoles();
            await CreateUserStatus();
            await CreateUser();
            await CreateEducationDegree();
            //await CreateSpecialNeeds();
            await CreateCustomerType();
            await CreatePartnerUnit();
            //await AddCodeProductCategory();

#if !DEBUG
            // await CreateUserRegistrationRequest();
#endif
        }
        catch
        {
        }
    }

    private async Task CreateContact()
    {
        try
        {
            var contactCtx = UnitOfWork.Repository<Contact, Guid>();

            var contacts = await contactCtx.ToListAsync();

            var json = File.ReadAllText("SeedData/Data/contatos.json");

            var contactsJson = JsonConvert.DeserializeObject<List<ContactModel>>(json);

            foreach (var contactJson in contactsJson)
            {
                if (!contacts.Any(r => r.CpfCnpj.GetDigits().Equals(contactJson.Codigo)))
                {
                    contactCtx.Context.Contact.Add(new Contact
                    {
                        Id = Guid.NewGuid(),
                        Name = contactJson.RazaoSocial ?? contactJson.NomeFantasia,
                        CpfCnpj = contactJson.Codigo.GetDigits(),
                        Phone = contactJson.Telefone,
                        CellPhone = contactJson.Celular,
                        Fax = contactJson.Fax,
                        MainActivityCode = contactJson.AtividadeEconomica,
                        EconomicActivity = contactJson.CodigoAtividadePrincipal,
                        Address = new Address
                        {
                            Id = Guid.NewGuid(),
                            AddressLine1 = contactJson.Endereco,
                            ZipCode = contactJson.Cep,
                            City = contactJson.Cidade,
                            UF = contactJson.Uf,
                            Complement = contactJson.Complemento,
                        },
                    });
                }
            }

            UnitOfWork.SaveChanges();
        }
        catch
        {
        }
    }

    private async Task CreatePartnerUnit()
    {
        try
        {
            var partnerCtx = UnitOfWork.Repository<Partner, Guid>();

            if (!partnerCtx.Any(t => t.CpfCnpj.Equals("03656287000109")))
            {
                var partner = new Partner
                {
                    Id = Guid.Parse("8e7468ea-4a08-43fd-afc2-a00333b81651"),
                    Name = "SINDICATO DO COMÉRCIO DE VENDEDORES AMBULANTES DO DF",
                    Email = "contato@sindvamb.com.br",
                    CpfCnpj = "03656287000109",
                    Enabled = true,
                    PartnerUnits = new List<PartnerUnit>()
                    {
                        new()
                        {
                            Id = Guid.Parse("032c6ea0-a79f-420e-930b-bc527aaa47d6"),
                            Name = "SINDICATO DO COMÉRCIO DE VENDEDORES AMBULANTES DO DF",
                            Email = "contato@sindvamb.com.br",
                            CpfCnpj = "03656287000109",
                            Enabled = true,
                        }
                    }
                };

                partnerCtx.Add(partner, true);
            }
        }
        catch
        {
        }
    }

    //private async Task AddCityAndCounty()
    //{
    //    try
    //    {
    //        var districtCtx = UnitOfWork.Repository<District, Guid>();
    //        var citytCtx = UnitOfWork.Repository<City, Guid>();

    //        if (districtCtx.Any() && citytCtx.Any()) return;

    //        var estadosArr = "11|12|13|14|15|16|17|21|22|23|24|25|26|27|28|29|31|32|33|35|41|42|43|50|51|52|53".Split("|");

    //        var estados = new List<Estado>();
    //        var municipios = new List<Municipio>();

    //        var tokenService = new GenericTokentService
    //        {
    //            //BaseAddress = @"https://servicodados.ibge.gov.br/api/v1/"
    //        };

    //        var clientEstado = new HttpClientService<dynamic, List<Estado>, dynamic> { };
    //        clientEstado.Init("https://servicodados.ibge.gov.br/api/v1/localidades/estados", tokenService);

    //        var resultEstado = await clientEstado.Get();

    //        if (resultEstado.IsSuccessStatus)
    //        {
    //            if (!resultEstado.Data.IsNullOrEmpty())
    //            {
    //                estados = resultEstado.Data;
    //                estadosArr = estados.Select(s => s.Id.ToString()).ToArray();

    //                var districts = districtCtx.ToList();

    //                foreach (var estado in estados)
    //                {
    //                    var hasDistrict = districts.Any(d => d.ExtenalId.Equals(estado.Id));

    //                    if (!hasDistrict)
    //                    {
    //                        districtCtx.Add(new District
    //                        {
    //                            Name = estado.Nome,
    //                            ExtenalId = estado.Id,
    //                            Enabled = true,
    //                            Code = estado.Sigla
    //                        });
    //                    }
    //                }
    //            }
    //        }

    //        districtCtx.SaveChanges();

    //        foreach (var estadoNum in estadosArr)
    //        {
    //            var clientMunicipio = new HttpClientService<dynamic, List<Municipio>, dynamic> { };
    //            //   clientMunicipio.Init($"localidades/estados/{estadoNum}/municipios", tokenService);
    //            clientMunicipio.Init($"https://servicodados.ibge.gov.br/api/v1/localidades/estados/{estadoNum}/municipios", tokenService);

    //            var resultMunicipio = await clientMunicipio.Get();

    //            if (resultMunicipio.IsSuccessStatus)
    //            {
    //                if (!resultMunicipio.Data.IsNullOrEmpty())
    //                {
    //                    municipios = resultMunicipio.Data;

    //                    var cityties = citytCtx.ToList();

    //                    foreach (var municipio in municipios)
    //                    {
    //                        var hasMunicipio = cityties.Any(d => d.ExtenalId.Equals(municipio.Id));

    //                        if (!hasMunicipio)
    //                        {
    //                            var ufId = municipio.Microrregiao?.Mesorregiao?.UF?.Id;

    //                            if (ufId.IsNotNull() && ufId > 0)
    //                            {
    //                                var estado = districtCtx.FirstOrDefault(e => e.ExtenalId.Equals(ufId));

    //                                if (estado.IsNotNull())
    //                                {
    //                                    citytCtx.Add(new City
    //                                    {
    //                                        Name = municipio.Nome,
    //                                        ExtenalId = municipio.Id,
    //                                        Enabled = true,
    //                                        DistrictId = estado.Id
    //                                    });
    //                                }
    //                            }
    //                        }
    //                    }
    //                }
    //            }
    //        }

    //        citytCtx.SaveChanges();
    //    }
    //    catch
    //    {
    //        return false;
    //    }
    //}

    private async Task CreateRoles()
    {
        var roletCtx = UnitOfWork.Repository<Role, Guid>();


        foreach (var (key, value) in AuthRole.Roles)
        {
            foreach (EAuthRole evalue in Enum.GetValues(typeof(EAuthRole)))
            {
                if (!roletCtx.Any(r => r.Key.Equals(key)) && evalue.GetDisplayName().Equals(key))
                {
                    roletCtx.Add(new Role
                    {
                        Key = key,
                        Value = value,
                        Description = evalue.GetDescription()
                    });
                }
            }
        }

        UnitOfWork.SaveChanges();
    }

    private async Task CreateUserStatus()
    {
        IRepository<UserStatus, SindvambDbContext, Guid> ctx = UnitOfWork.Repository<UserStatus, Guid>();

        foreach (EUserStatus value in Enum.GetValues(typeof(EUserStatus)))
        {
            if (!ctx.Any(r => r.Code.Equals(value.GetDisplayName())))
            {
                ctx.Add(new UserStatus
                {
                    Code = value.GetDisplayName(),
                    Description = value.GetDescription(),
                });
            }
        }

        ctx.SaveChanges();
    }

    private async Task CreateCustomerType()
    {
        IRepository<CustomerType, SindvambDbContext, Guid> ctx = UnitOfWork.Repository<CustomerType, Guid>();

        foreach (ECustomerType value in Enum.GetValues(typeof(ECustomerType)))
        {
            if (!ctx.Any(r => r.Code.Equals(value.GetDisplayName())))
            {
                ctx.Add(new CustomerType()
                {
                    Code = value.GetDisplayName(),
                    Description = value.GetDescription(),
                });
            }
        }

        ctx.SaveChanges();
    }

    private async Task CreateEducationDegree()
    {
        IRepository<EducationDegree, SindvambDbContext, Guid> ctx = UnitOfWork.Repository<EducationDegree, Guid>();

        foreach (EEducationDegree value in Enum.GetValues(typeof(EEducationDegree)))
        {
            if (!ctx.Any(r => r.Code.Equals(value.GetDisplayName())))
            {
                ctx.Add(new EducationDegree
                {
                    Code = value.GetDisplayName(),
                    Description = value.GetDescription(),
                });
            }
        }

        ctx.SaveChanges();
    }

    // private async Task CreateSpecialNeeds()
    // {
    //     IRepository<SpecialNeeds, SindvambDbContext, Guid> ctx = UnitOfWork.Repository<SpecialNeeds, Guid>();
    //
    //     foreach (ESpecialNeeds value in Enum.GetValues(typeof(ESpecialNeeds)))
    //     {
    //         if (!ctx.Any(r => r.Code.Equals(value.GetDisplayName())))
    //         {
    //             ctx.Add(new SpecialNeeds
    //             {
    //                 Code = value.GetDisplayName(),
    //                 Description = value.GetDescription(),
    //             });
    //         }
    //     }
    //
    //     ctx.SaveChanges();
    // }

    private async Task CreateUser()
    {
        try
        {
            var userCtx = UnitOfWork.Repository<User, Guid>();

            if (!userCtx.Any(t => t.Id.Equals(UserId)))
            {
                var user = new User
                {
                    Id = UserId,
                    Name = "WILLIAN ALVES DA SILVA 33750529876",
                    Email = "willian__bsb@hotmail.com",
                    Birthdate = new DateTime(2020, 05, 01),
                    CpfCnpj = "37044943000126",
                    IsSystem = true,
                    Matricula = 336171,
                    Password = CryptoAES.EncryptStringToBytes("1q2w3E*"),
                    Address = new Address
                    {
                        Id = Guid.Parse("da1ab9a7-e11c-4a50-b4a3-1307fa8c527a"),
                        AddressLine1 = "QUADRA QNG 1",
                        Neighborhood = "TAGUATINGA NORTE",
                        Number = 1,
                        ZipCode = "72130-010",
                        City = "BRASÍLIA"
                    },
                    RoleId = UnitOfWork.Repository<Role, Guid>()
                        .FirstOrDefault(r => r.Key.Equals(Enum.GetName(EAuthRole.MASTER))).Id
                };

                var userStatusId = UnitOfWork.Repository<UserStatus, Guid>()
                    .FirstOrDefault(u => u.Code.Equals(Enum.GetName(EUserStatus.ACTIVE)))?.Id;

                user.UserStatusId = userStatusId;
                userCtx.Add(user, true);
            }
        }
        catch
        {
        }
    }

    // private async Task CreateUserRegistrationRequest()
    // {
    //     try
    //     {
    //         var users = ListaClientesFake();
    //
    //         if (users.Count > 0)
    //         {
    //             foreach (var user in users)
    //             {
    //                 await MediatorHandler.SendCommand<RegisterCommand, bool>(new RegisterCommand(user));
    //             }
    //         }
    //     }
    //     catch
    //     {
    //     }
    // }

    // public List<UserDTO> ListaClientesFake()
    // {
    //     var clientes = new List<UserDTO>();
    //
    //     if (UnitOfWork.Repository<User, Guid>().Count() <= 100)
    //     {
    //         var clienteFaker = new Faker<UserDTO>("pt_BR")
    //             .RuleFor(c => c.Name, f => f.Name.FullName(Bogus.DataSets.Name.Gender.Female))
    //             .RuleFor(c => c.Email, f => f.Internet.Email(f.Person.FirstName).ToLower())
    //             .RuleFor(c => c.Protocol, f => f.Random.Int(500, 999999))
    //             .RuleFor(c => c.Password, f => f.Random.String(8, 12));
    //
    //         clientes = clienteFaker.Generate(100);
    //     }
    //
    //     return clientes;
    // }

    //private async Task AddCodeProductCategory()
    //{

    //    var list = new List<ProductCategory>()
    //    {
    //        new ProductCategory
    //            {
    //                Code = "ADT",
    //                Description = "ATRAÇÃO E DESENVOLVIMENTO DE TALENTOS",
    //                CodeProductCategory = "SOL;CUSTOMER;PRE;DIR;GER;ADM;CLIENTEE;MASTER_ADMIN;SUPPORT"
    //            },
    //        new ProductCategory
    //            {
    //                Code = "CEP",
    //                Description = "CULTURA, ESTRATÉGIA E PLANEJAMENTO DE RH",
    //                CodeProductCategory = "PRE;DIR;GER;ADM;CLIENTEE;SUPPORT;MASTER_ADMIN"
    //            },
    //        new ProductCategory
    //            {
    //                Code = "RDC",
    //                Description = "REMUNERAÇÃO, DESEMPENHO E CARREIRA",
    //                CodeProductCategory = "PRE;DIR;GER;ADM;CLIENTEE;SUPPORT;MASTER_ADMIN"
    //            },
    //        new ProductCategory
    //            {
    //                Code = "PSB",
    //                Description = "PREVI, SAÚDE (+ BENEFÍCIOS)",
    //                CodeProductCategory = "PRE;DIR;GER;ADM;CLIENTEE;SUPPORT;MASTER_ADMIN"
    //            },
    //        new ProductCategory
    //            {
    //                Code = "SST",
    //                Description = "SAÚDE E SEGURANÇA NO TRABALHO",
    //                CodeProductCategory = "PRE;DIR;GER;ADM;CLIENTEE;SUPPORT;MASTER_ADMIN"
    //            },
    //        new ProductCategory
    //            {
    //                Code = "PES",
    //                Description = "PROJETOS ESPECIAIS",
    //                CodeProductCategory = "PRE;DIR;GER;ADM;CLIENTEE;SUPPORT;MASTER_ADMIN"
    //            }
    //    };

    //    var ctx = UnitOfWork.Repository<ProductCategory, Guid>();


    //    foreach (var item in list)
    //    {
    //        if (!ctx.Any(r => r.Code.Equals(item.Code)))
    //        {
    //            ctx.Add(item);
    //        }
    //    }

    //    await ctx.SaveChangesAsync();
    //}
}