using System.Collections.Generic;
using MatchingSystem.DataLayer.Feature.Role;

namespace MatchingSystem.Service.Admin
{
    public class AssignDefaultRoleRequest
    {
        public UserRoleDto User { get; set; }
    }

    public class AssignStudentRoleRequest
    {
        public UserRoleDto User { get; set; }
        public int GroupId { get; set; }
    }

    public class AssignTutorRoleRequest
    {
        public UserRoleDto User { get; set; }
        public IEnumerable<int> AssignedGroups { get; set; }
        public int Quota { get; set; }
    }

    public class RemoveRoleRequest : AssignDefaultRoleRequest { }
    
    public interface IAdminService
    {
        public void AssignTutorRole(AssignTutorRoleRequest payload);
        public void AssignStudentRole(AssignStudentRoleRequest payload);
        public void AssignDefaultRole(AssignDefaultRoleRequest payload);
        public void RemoveDefaultRole(RemoveRoleRequest payload);
        public void RemoveStudentRole(RemoveRoleRequest payload);
        public void RemoveTutorRole(RemoveRoleRequest payload);
    } 
}

