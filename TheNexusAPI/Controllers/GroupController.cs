using Microsoft.AspNetCore.Mvc;
using TheNexusAPI.Data;
using TheNexusAPI.Entities;
using TheNexusAPI.Services;

namespace TheNexusAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GroupController : ControllerBase
    {
        private readonly DataContext _dataContext;
        private readonly GroupService _groupService;

        private List<Group> groups = new List<Group>() { };

        public GroupController(DataContext dataContext)
        {
            _dataContext = dataContext;
            _groupService = new GroupService(_dataContext);
        }

        #region Get

        // Get all Families
        [HttpGet]
        public List<Group> GetFamilies()
        {
            return _dataContext.Group.ToList();
        }

        // Get one specific Group based on the primary key GroupId
        [HttpGet("{groupId}")]
        public Group? GetGroupByGroupId(int groupId)
        {
            return _groupService.GetGroupByGroupId(groupId);
        }

        // Get all Families who are within one group by the GroupId
        [HttpGet("group/{groupId}")]
        public List<Group> GetFamiliesByGroupId(int groupId)
        {
            return groups.FindAll(item => item.GroupId == groupId);  // ?? new List<Group>();
        }

        #endregion

        #region Update

        // Update a specific group with new data based on passed group
        [HttpPut]
        public List<Group> UpdateGroup(Group group)
        {
            return _groupService.UpdateGroup(group);
        }

        #endregion

        #region Add

        // Add the given group if they do not already exist
        [HttpPost]
        public Group AddGroup(Group group)
        {
            return _groupService.AddGroup(group);
        }

        #endregion
    }
}
