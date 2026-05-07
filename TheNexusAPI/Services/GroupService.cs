using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Reflection;
using TheNexusAPI.Data;
using TheNexusAPI.Entities;

namespace TheNexusAPI.Services
{
    public class GroupService
    {
        private readonly DataContext _dataContext;
        private readonly GroupService _groupService;
        private readonly ChangeLogService _changeLog;

        public GroupService(DataContext dataContext)
        {
            _dataContext = dataContext;
            _changeLog = new ChangeLogService(_dataContext);
        }

        public Group? GetGroupByGroupId(int groupId, List<Group> group)
        {
            return group.FirstOrDefault(item => item.GroupId == groupId);// ?? new Group();
        }

        public Group? GetGroupByGroupId(int groupId)
        {
            return _dataContext.Group.FirstOrDefault(item => item.GroupId == groupId);// ?? new Group();
        }

        public List<Group> UpdateGroup(Group updatedGroup)
        {
            Group? foundGroup = GetGroupByGroupId(updatedGroup.GroupId);
            // For holding foundGroup initial value and during changelog check making sure what changed
            Group compareFoundGroup = foundGroup != null ? new Group
            {
                GroupId = foundGroup.GroupId,
                GroupName = foundGroup.GroupName,
                GroupTypeId = foundGroup.GroupTypeId,
                GroupDescription = foundGroup.GroupDescription
            } : new Group();

            if (foundGroup != null)
            {
                foundGroup.GroupId = updatedGroup.GroupId;
                foundGroup.GroupName = updatedGroup.GroupName;
                foundGroup.GroupTypeId = updatedGroup.GroupTypeId;
                foundGroup.GroupDescription = updatedGroup.GroupDescription;

            }

            try { 
                _dataContext.Group.Update(foundGroup ?? new Group());
                _dataContext.SaveChanges();
                // If updates succeed, log changes
                _changeLog.ConvertChangesForLogging(compareFoundGroup, updatedGroup);
            }
            catch(DbUpdateException ex)
            {
                // Handle exceptions related to database updates
                Console.WriteLine($"An error occurred while updating the group: {ex.Message}");
            }
            return _dataContext.Group.ToList();
        }

        public Group AddGroup(Group newGroup)//, List<Group> groups)
        {

            Group foundGroup = GetGroupByGroupId(newGroup.GroupId) ?? new Group();

            if (foundGroup.GroupId == 0)
            {
                _dataContext.Group.Add(newGroup);
                _dataContext.SaveChanges();

                return newGroup;
            }

            return new Group();
        }
    }
}
