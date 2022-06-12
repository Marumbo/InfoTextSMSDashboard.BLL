using AutoMapper;
using InfoTextSMSDashboard.BLL.Interfaces;
using InfoTextSMSDashboard.BLL.Mapping_profiles;
using InfoTextSMSDashboard.BLL.Models;
using InfoTextSMSDashboard.DAL.Models;
using InfoTextSMSDashboard.DataModels.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfoTextSMSDashboard.BLL.Services
{
    public class GroupService : IGroupService
    {
        private readonly sms_dashboardContext _context;

        private readonly IMapper _mapper;
        public GroupService(sms_dashboardContext context)
        {
            _context = context;

            var mapperConfig = new MapperConfiguration(config =>
            {
                config.AddProfile<GroupProfile>();

            });

            _mapper = mapperConfig.CreateMapper();
        }
        public async Task<OutputResponse> AddGroup(GroupsDTO group)
        {
            var groupCheck = await _context.Groups.AnyAsync(c => c.GroupId.ToString().ToLower() == group.GroupId.ToString().ToLower());

            if (groupCheck)
            {
                return new OutputResponse
                {
                    IsSuccess = false,
                    Message = "Cannot add group as group exists already"

                };

            }

            var addGroup = _mapper.Map<GroupsDTO, Group>(group);

            try
            {
                await _context.AddAsync(addGroup);
                await _context.SaveChangesAsync();

                return new OutputResponse
                {
                    IsSuccess = true,
                    Message = $"Group {group.GroupName} added to database."
                };
            }
            catch (Exception e)
            {
                return new OutputResponse
                {
                    IsSuccess = false,
                    Message = $"Error in adding Group. {e}"
                };
            }



        }

        public async Task<OutputResponse> GetAllGroups()
        {
            var groupList = await _context.Groups.ToListAsync();

            var count = groupList.Count();


            if (groupList == null)
            {
                return new OutputResponse
                {
                    IsSuccess = false,
                    Message = $"No Groups in database"
                };

            }

            return new OutputResponse
            {
                IsSuccess = true,
                Message = "List of Group",
                SuccessResult = groupList
            };
        }

        public async Task<OutputResponse> GetGroupById(int id)
        {
            var groupExists = await _context.Groups.SingleOrDefaultAsync(g => g.GroupId == id);

            if (groupExists == null)
            {
                return new OutputResponse
                {
                    IsSuccess = false,
                    Message = $"No group with given id: {id}"
                };

            }

            return new OutputResponse
            {
                IsSuccess = true,
                Message = $"Contact using {id} ",
                SuccessResult = groupExists
            };

        }

        public async Task<OutputResponse> RemoveGroup(int id)
        {
            var groupCheck = await _context.Groups.SingleOrDefaultAsync(g => g.GroupId == id);

            if (groupCheck == null)
            {
                return new OutputResponse
                {
                    IsSuccess = false,
                    Message = $"Unable to delete because no group with id: {id}"
                };
            }

            try
            {
                _context.Groups.Remove(groupCheck);

                _context.SaveChanges();

                return new OutputResponse
                {
                    IsSuccess = true,
                    Message = $"Group removed. Id {id}",

                };
            }
            catch (Exception e)
            {
                return new OutputResponse
                {
                    IsSuccess = false,
                    Message = $"Failed to delete Group. id: {id}. Error: {e}"
                };
            }

        }


        public async Task<OutputResponse> UpdateGroup(GroupsDTO group)
        {
            var groupCheck = await _context.Groups.SingleOrDefaultAsync(g => g.GroupId == group.GroupId);

            if (groupCheck == null)
            {
                return new OutputResponse
                {
                    IsSuccess = false,
                    Message = $"No group with id:{group.GroupId} in database"
                };
            }

            try
            {
                _mapper.Map<GroupsDTO, Group>(group, groupCheck);

                await _context.SaveChangesAsync();

                return new OutputResponse
                {
                    IsSuccess = true,
                    Message = $" updated group id: {group.GroupId}"

                };

            }
            catch (Exception error)
            {
                return new OutputResponse
                {
                    IsSuccess = false,
                    Message = $"unable to update group id: {group.GroupId}, error: {error}"
                };
            }
        }
    }

       
    }
