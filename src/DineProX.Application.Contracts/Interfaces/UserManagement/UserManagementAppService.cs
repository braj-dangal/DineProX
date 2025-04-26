using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using TISAPayrollManagement.Constants.RoleManagement;
using TISAPayrollManagement.Dtos.ResponseDtos;
using TISAPayrollManagement.Dtos.UserManagement;
using TISAPayrollManagement.Entities.Notification;
using TISAPayrollManagement.Entities.RoleManagement;
using TISAPayrollManagement.Interface.UserManagement;
using TISAPayrollManagement.Permissions;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Emailing;
using Volo.Abp.Security.Encryption;
using System.Security.Cryptography;
using Volo.Abp.Identity;
using TISAPayrollManagement.Dtos.Notifications;
using Volo.Abp.Data;
using Microsoft.AspNetCore.Routing;
using DineProX.Interfaces.UserManagement;

namespace DineProX.Interfaces.UserManagement
{
    [Authorize]
    public class UserManagementAppService : ApplicationService, IUserManagementAppService
    {
        private readonly IdentityUserManager _userManager;
        private readonly IdentityRoleManager _roleManager;
        private readonly IRepository<IdentityUser, Guid> _abpUserRepository;
        private readonly IRepository<IdentityRole, Guid> _abpRoleRepository;
        private readonly IEmailSender _emailSender;
        private readonly IConfiguration _configuration;
        protected IStringEncryptionService StringEncryptionService { get; }
        private readonly IRepository<Notification, Guid> _notificationRepository;
        private readonly IRepository<RoleExtension, Guid> _roleExtensionRepository;
        private readonly IRepository<IdentityUserRole> _abpUserRoleRepository;

        public UserManagementAppService(
            IdentityUserManager userManager,
            IdentityRoleManager roleManager,
            IRepository<IdentityRole, Guid> abpRoleRepository,
            IEmailSender emailSender,
            IConfiguration configuration,
            IStringEncryptionService stringEncryptionService,
            IRepository<Notification, Guid> notificationRepository,
            IRepository<RoleExtension, Guid> roleExtensionRepository,
            IRepository<IdentityUser, Guid> abpUserRepository, IRepository<IdentityUserRole> abpUserRoleRepository)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _abpRoleRepository = abpRoleRepository;
            _emailSender = emailSender;
            _configuration = configuration;
            StringEncryptionService = stringEncryptionService;
            _notificationRepository = notificationRepository;
            _roleExtensionRepository = roleExtensionRepository;
            _abpUserRepository = abpUserRepository;
            _abpUserRoleRepository = abpUserRoleRepository;
        }

        [Authorize(TISAPayrollManagementPermissions.User.User_Create)]
        public async Task<ResponseDto<UserDto>> CreateUserAsync(CreateUserDto input)
        {
            try
            {
                Logger.LogInformation($"Create User requested by User: {CurrentUser.Id}");
                Logger.LogDebug($"CreateUser requested for : {(CurrentUser.Id, input)}");

                var checkEmailAvailable = await _userManager.FindByEmailAsync(input.Email);
                if (checkEmailAvailable != null)
                    throw new UserFriendlyException("Email address already exist", code: "400");

                var roleDetail = await _roleManager.FindByIdAsync(input.RoleId.ToString());
                if (roleDetail == null)
                    throw new UserFriendlyException("Role not found", code: "400");
                var roleExtension = await _roleExtensionRepository.FirstOrDefaultAsync(x => x.AbpRoleName.ToLower() == roleDetail.Name.ToLower());
                if (roleDetail.Name.ToLower() != RoleConstants.Admin.ToLower() && roleDetail.Name.ToLower() != RoleConstants.General.ToLower())
                {
                    if (roleExtension.IsActive == false)
                    {
                        throw new UserFriendlyException("Role is not Active");
                    }
                }

                if (input.FirstName.Trim().ToLower() == "admin")
                {
                    throw new UserFriendlyException("User already exists.");
                }

                var identityUser = new IdentityUser(Guid.Empty, input.Email?.Trim(), input.Email?.Trim(), null)
                {
                    Name = input.FirstName?.Trim().ToCapitalizeFirstLetter(),
                    Surname = input.LastName?.Trim().ToCapitalizeFirstLetter()
                };

                var fullName = identityUser.Name + " " + identityUser.Surname;
                var password = await GenerateRandomPassword();
                Logger.LogInformation("Password for user : " + password);
                identityUser.SetEmailConfirmed(true);
                var result = await _userManager.CreateAsync(identityUser, password, true);

                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(identityUser, roleDetail.Name);
                    await SendEmailAsync(identityUser.Id, identityUser.Email, fullName, identityUser.UserName, password);
                }

                Logger.LogInformation($"New User Created for : {CurrentUser.Id} with {input}");

                var newUser = new UserDto
                {
                    Id = identityUser.Id,
                    FirstName = identityUser.Name,
                    LastName = identityUser.Surname,
                    UserName = identityUser.UserName,
                    Email = identityUser.Email,
                    IsActive = identityUser.IsActive
                };
                var userRoleList = await _userManager.GetRolesAsync(identityUser);
                var roleLists = await _abpRoleRepository.GetQueryableAsync();
                var roleDtoLists = new List<RoleDto>();
                foreach (var userRole in userRoleList)
                {
                    var roleData = roleLists.FirstOrDefault(x => x.Name.Trim().ToLower() == userRole.ToLower());
                    if (roleData != null)
                    {
                        var newRole = new RoleDto()
                        {
                            RoleId = roleData.Id,
                            RoleName = roleData.Name,
                        };
                        roleDtoLists.Add(newRole);
                    }
                }

                newUser.Roles = roleDtoLists;
                var response = new ResponseDto<UserDto>
                {
                    Message = "User added Successfully",
                    Code = 200,
                    Success = true,
                    Data = newUser
                };

                return response;
            }
            catch (Exception)
            {
                Logger.LogError(nameof(CreateUserAsync));
                throw;
            }
        }

        private async Task<bool> SendEmailAsync(Guid userId, string email, string fullName, string userName, string password)
        {
            try
            {
                var loginUrl = _configuration["DomainUrl:Url"];
                var body =
                    $"Dear {fullName}, <br> You have been registered. Your credentials are email: {userName} and password: {password} <br> Login Url: <a href='{loginUrl}' style='color: blue;'>Link</a>";

                MailMessage msg = new MailMessage();
                msg.Subject = "TISA PayrollManagement";
                msg.Body = body;
                msg.To.Add(email);
                msg.IsBodyHtml = true;
                await _emailSender.SendAsync(msg);

                var template = new NotificationTempleteDto
                {
                    Title = msg.Subject,
                    body = body,
                    SenderName = CurrentUser.UserName + " " + CurrentUser.SurName,
                    ReceiverName = userName
                };

                var notification = new Notification
                {
                    ReceiverId = userId,
                    Template = JsonConvert.SerializeObject(template),
                };
                await _notificationRepository.InsertAsync(notification);
                return true;
            }
            catch (Exception)
            {
                Logger.LogError(nameof(SendEmailAsync));
                throw;
            }
        }

        private static async Task<string> GenerateRandomPassword()
        {
            int length = 2;
            const string capitalAlphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            const string smallAlphabet = "abcdefghijklmnopqrstuvwxyz";
            const string number = "0123456789";
            const string specialCharacter = "!@#$%^&*()_+";

            using (var rng = new RNGCryptoServiceProvider())
            {
                byte[] randomBytes = new byte[length];
                rng.GetBytes(randomBytes);

                var passwordBuilder = new StringBuilder();

                passwordBuilder.Append(randomBytes.Select(b => specialCharacter[b % specialCharacter.Length]).ToArray());
                passwordBuilder.Append(randomBytes.Select(b => number[b % number.Length]).ToArray());
                passwordBuilder.Append(randomBytes.Select(b => capitalAlphabet[b % capitalAlphabet.Length]).ToArray());
                passwordBuilder.Append(randomBytes.Select(b => smallAlphabet[b % smallAlphabet.Length]).ToArray());

                return passwordBuilder.ToString();
            }
        }

        [Authorize(TISAPayrollManagementPermissions.User.User_Edit)]
        public async Task<ResponseDto<UserDto>> UpdateUserAsync(Guid id, UpdateUserDto input)
        {
            try
            {
                Logger.LogInformation($"UpdateUserAsync requested by User: {CurrentUser.Id}");
                Logger.LogDebug($"UpdateUserAsync requested for: {input} and Id: {id}");

                var generalUser = RoleConstants.General;
                var userToUpdate = await _abpUserRepository.FindAsync(id);
                if (userToUpdate == null)
                    throw new UserFriendlyException("User not found", code: "400");
                var rolesToUpdate = new List<IdentityRole>();
                foreach (var role in input.RoleIds)
                {
                    var roleToUpdate = await _abpRoleRepository.FindAsync(role);
                    if (roleToUpdate == null)
                        throw new UserFriendlyException("Role not found", code: "400");
                    if (roleToUpdate.Name.ToLower() != RoleConstants.General.ToLower())
                    {
                        var roleExtension = await _roleExtensionRepository.FirstOrDefaultAsync(x => x.AbpRoleName.ToLower() == roleToUpdate.Name.ToLower());
                        if (roleExtension.IsActive == false)
                        {
                            throw new UserFriendlyException("Role is not Active");
                        }
                    }

                    rolesToUpdate.Add(roleToUpdate);
                }

                if (userToUpdate.Name.ToLower() == "admin")
                {
                    throw new UserFriendlyException("Cannot modify admin User");
                }

                userToUpdate.Name = input.FirstName.ToCapitalizeFirstLetter();
                userToUpdate.Surname = input.LastName;
                var roleLists = await _abpRoleRepository.GetQueryableAsync();
                var roleDtoLists = new List<RoleDto>();
                userToUpdate.Roles.Clear();
                foreach (var roleId in input.RoleIds)
                {
                    var roleData = roleLists.FirstOrDefault(x => x.Id == roleId);
                    if (roleData != null)
                    {
                        var newRole = new RoleDto()
                        {
                            RoleId = roleData.Id,
                            RoleName = roleData.Name,
                        };
                        roleDtoLists.Add(newRole);
                    }

                    userToUpdate.AddRole(roleId);
                }

                await _userManager.UpdateAsync(userToUpdate);

                var updatedUser = new UserDto
                {
                    Id = userToUpdate.Id,
                    FirstName = userToUpdate.Name,
                    LastName = userToUpdate.Surname,
                    UserName = userToUpdate.UserName,
                    Email = userToUpdate.Email,
                    IsActive = userToUpdate.IsActive
                };
                updatedUser.Roles = roleDtoLists;

                Logger.LogInformation($"UpdateUserAsync responded for User:{CurrentUser.Id}");
                var response = new ResponseDto<UserDto>
                {
                    Message = "User Updated Successfully",
                    Code = 200,
                    Success = true,
                    Data = updatedUser
                };
                return response;
            }
            catch (UserFriendlyException e)
            {
                Logger.LogError(e, nameof(UpdateUserAsync));
                throw;
            }
            catch (Exception)
            {
                Logger.LogError(nameof(UpdateUserAsync));
                throw;
            }
        }

        [Authorize(TISAPayrollManagementPermissions.User.User_Deactivate)]
        public async Task<DeleteResponseDto> ActivateDeactivateUserAsync(ActivateDeactivateUserDto input)
        {
            try
            {
                Logger.LogInformation($"Delete Application user requested for: {input}");
                Logger.LogDebug($"Delete Application user requested for User: {(CurrentUser.Id, input)}");

                var userToDelete = await _abpUserRepository.FindAsync(input.UserId)
                                   ?? throw new UserFriendlyException("User not found", "404");

                var usersInAdminRole = await _userManager.GetUsersInRoleAsync(RoleConstants.Admin);

                if (input.UserId == CurrentUser.Id)
                {
                    //return this.SendFailure<DeleteResponseDto>(null, "Cannot activate or deactivate oneself.");
                    throw new UserFriendlyException("Cannot activate or deactivate oneself");
                }

                if (usersInAdminRole.Contains(userToDelete))
                {
                    throw new UserFriendlyException("Cannot inactivate Admin User");
                }

                if (userToDelete.UserName.ToLower() == RoleConstants.Admin)
                {
                    throw new UserFriendlyException("Cannot inactivate Admin User");
                }

                userToDelete.SetIsActive(input.IsActive);
                await _abpUserRepository.UpdateAsync(userToDelete);

                Logger.LogInformation($"User status updated succesfullly by User:{CurrentUser.Id} for {input}");

                return new DeleteResponseDto { Code = 200, Success = true, Message = "User activation status updated successfully" };
            }

            catch (Exception)
            {
                Logger.LogError(nameof(ActivateDeactivateUserAsync));
                throw;
            }
        }

        public async Task<PagedResultDto<UserDto>> GetPagedAndSortedUserListAsync(UserFilterDto input)
        {
            try
            {
                Logger.LogInformation($"GetPagedAndSortedUserListAsync requested by User: {CurrentUser.Id}");
                Logger.LogDebug($"GetPagedAndSortedUserListAsync requested for: {input}");

                var abpUserQuery = await _abpUserRepository.GetQueryableAsync();
                var roleQuery = await _abpRoleRepository.GetQueryableAsync();
                var abpUserRoles = await _abpUserRoleRepository.GetQueryableAsync();

                abpUserQuery = abpUserQuery.Where(u => u.UserName != "admin");
                if (input.Sorting.IsNullOrWhiteSpace())
                {
                    input.Sorting = "CreationTime";
                }

                #region Filter

                if (input.RoleId != null)
                {
                    abpUserQuery = abpUserQuery.Where(x =>
                        x.Roles.Any(ur => roleQuery.Any(r => r.Id == ur.RoleId && r.Id == input.RoleId)));
                }

                if (input.IsActive != null)
                {
                    abpUserQuery = abpUserQuery.Where(x => x.IsActive == input.IsActive);
                }

                #endregion

                var userList = from user in abpUserQuery
                               join userRole in abpUserRoles on user.Id equals userRole.UserId
                               join role in roleQuery on userRole.RoleId equals role.Id
                               select new
                               {
                                   UserId = user.Id,
                                   RoleId = role.Id,
                                   roleName = role.Name
                               };
                var userResultQuery = from user in abpUserQuery
                                      join userRole in userList on user.Id equals userRole.UserId into userRoleGroup
                                      select new UserDto()
                                      {
                                          Id = user.Id,
                                          UserName = user.UserName,
                                          FirstName = user.Name,
                                          LastName = user.Surname,
                                          Email = user.Email,
                                          CreationTime = user.CreationTime,
                                          IsActive = user.IsActive,
                                          Roles = userRoleGroup.Where(x => x.UserId == user.Id).Select(x => new RoleDto()
                                          {
                                              RoleId = x.RoleId,
                                              RoleName = x.roleName
                                          }).OrderByDescending(x => x.RoleName.ToLower() == RoleConstants.General.ToLower()).ToList()
                                      };
                if (!string.IsNullOrWhiteSpace(input.SearchKeyword))
                {
                    userResultQuery = userResultQuery.Where(x =>
                        x.FirstName.ToLower().Contains(input.SearchKeyword.ToLower()) ||
                        x.LastName.ToLower().Contains(input.SearchKeyword.ToLower()) ||
                        x.Email.ToLower().Contains(input.SearchKeyword.ToLower()) ||
                        x.Roles.Any(x => x.RoleName.ToLower() == input.SearchKeyword.ToLower()));
                }

                var result = new List<UserDto>();
                switch (input.Sorting.ToLower())
                {
                    case "email":
                        result = input.SortOrder == "asc"
                            ? userResultQuery
                                .OrderBy(r => r.Email)
                                .Skip(input.SkipCount)
                                .Take(input.MaxResultCount)
                                .ToList()
                            : userResultQuery
                                .OrderByDescending(r => r.Email)
                                .Skip(input.SkipCount)
                                .Take(input.MaxResultCount)
                                .ToList();
                        break;
                    case "firstname":
                        result = input.SortOrder == "asc"
                            ? userResultQuery
                                .OrderBy(r => r.FirstName)
                                .Skip(input.SkipCount)
                                .Take(input.MaxResultCount)
                                .ToList()
                            : userResultQuery
                                .OrderByDescending(r => r.FirstName)
                                .Skip(input.SkipCount)
                                .Take(input.MaxResultCount)
                                .ToList();
                        break;
                    default:
                        result = result = input.SortOrder == "desc"
                            ? userResultQuery
                                .OrderBy(x => x.CreationTime)
                                .Skip(input.SkipCount)
                                .Take(input.MaxResultCount)
                                .ToList()
                            : userResultQuery
                                .OrderByDescending(r => r.CreationTime)
                                .Skip(input.SkipCount)
                                .Take(input.MaxResultCount)
                                .ToList();
                        break;
                }

                var totalCount = userResultQuery.Count();

                Logger.LogInformation($"GetPagedAndSortedUserListAsync responded for User:{CurrentUser.Id}");

                var pagedResult = new PagedResultDto<UserDto>(totalCount, result);
                return pagedResult;
            }
            catch (Exception e)
            {
                Logger.LogInformation("Exception Caught in UserManagementAppService : GetPagedAndSortedUserListAsync");
                Logger.LogException(e);
                throw new UserFriendlyException(e.Message);
            }
        }

        [Authorize(TISAPayrollManagementPermissions.User.Default)]
        public async Task<UserDto> GetUserByIdAsync(Guid id)
        {
            try
            {
                Logger.LogInformation($"GetUserByIdAsync requested by User: {CurrentUser.Id}");

                var checkUserId = await _abpUserRepository.FirstOrDefaultAsync(x => x.Id == id);
                if (checkUserId == null)
                    throw new UserFriendlyException("User not found", code: "400");

                var userQuery = (from user in await _abpUserRepository.GetQueryableAsync()
                                 where user.Id == id
                                 let userRoles = user.Roles
                                 from assignedRoles in userRoles.DefaultIfEmpty()
                                 join role in await _abpRoleRepository.GetQueryableAsync()
                                     on assignedRoles.RoleId equals role.Id into roleRight
                                 from role in roleRight.DefaultIfEmpty()
                                 where user.Id == id
                                 select new UserDto()
                                 {
                                     Id = user.Id,
                                     FirstName = user.Name,
                                     LastName = user.Surname,
                                     UserName = user.UserName,
                                     Email = user.Email,
                                     IsActive = user.IsActive
                                 }).FirstOrDefault();
                var userRoleList = await _userManager.GetRolesAsync(checkUserId);
                var roleLists = await _abpRoleRepository.GetQueryableAsync();
                var roleDtoLists = new List<RoleDto>();
                foreach (var userRole in userRoleList)
                {
                    var roleData = roleLists.FirstOrDefault(x => x.Name.Trim().ToLower() == userRole.ToLower());
                    if (roleData != null)
                    {
                        var newRole = new RoleDto()
                        {
                            RoleId = roleData.Id,
                            RoleName = roleData.Name,
                        };
                        roleDtoLists.Add(newRole);
                    }
                }

                userQuery.Roles = roleDtoLists;
                Logger.LogInformation($"GetUserByIdAsync responded for User:{CurrentUser.Id}");

                return userQuery;
            }
            catch (Exception)
            {
                Logger.LogError(nameof(GetUserByIdAsync));
                throw;
            }
        }
    }

    public static class StringExtensions
    {
        public static string ToCapitalizeFirstLetter(this string str)
        {
            if (string.IsNullOrEmpty(str))
            {
                return string.Empty;
            }

            return char.ToUpper(str[0]) + str.Substring(1, str.Length - 1);
        }
    }
}