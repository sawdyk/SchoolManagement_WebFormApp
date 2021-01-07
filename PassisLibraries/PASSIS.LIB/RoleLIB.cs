using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PASSIS.LIB
{
    public class RoleLIB
    {
        public void SaveRole(Role rol)
        {
            try
            {
                PASSISLIBDataContext context = new PASSISLIBDataContext();
                context.Roles.InsertOnSubmit(rol);
                context.SubmitChanges();
            }
            catch (Exception ex)
            { throw ex; }
        }
        public IList<Role> getAllRoles()
        {
            PASSISLIBDataContext context = new PASSISLIBDataContext();
            var allRoles = context.Roles;
            return allRoles.ToList<Role>();
        }
        public IList<Role> getAllRoles(Int32 parentId)
        {
            PASSISLIBDataContext context = new PASSISLIBDataContext();
            var allRoles = from rol in context.Roles where rol.ParentRoleId == parentId select rol;
           
            return allRoles.ToList<Role>();
        }
        /// <summary>
        /// specially adapted to remove parent for the list 
        /// </summary>
        /// <param name="parentId"></param>
        /// <returns></returns>
        public IList<Role> getAllRolesSpecial()
        {
            PASSISLIBDataContext context = new PASSISLIBDataContext();
            var allRoles = from role in context.Roles where  role.Id != (int)RoleName.SystemAdmin
                           && role.Id != (int)RoleName.SchoolAdmin && role.Id != (int)RoleName.Parent && role.Id != (int)RoleName.Student
                           //&& role.Id != (int)RoleName.ClassTeacher
                           select role;

            return allRoles.ToList<Role>();
        }
        public IList<Role> RetrieveRolesByName(string name)
        {
            PASSISLIBDataContext context = new PASSISLIBDataContext();
            var allRoles = from rol in context.Roles where rol.RoleName.Contains(name) select rol;
            return allRoles.ToList<Role>();
        }
        public IList<Role> RetrieveRolesByCode(string code)
        {
            PASSISLIBDataContext context = new PASSISLIBDataContext();
            var allRoles = from rol in context.Roles where rol.RoleCode.Contains(code) select rol;
            return allRoles.ToList<Role>();
        }
        public Role RetrieveRole(Int64 Id)
        {
            try
            {
                PASSISLIBDataContext context = new PASSISLIBDataContext();
                Role rol = context.Roles.SingleOrDefault(s => s.Id == Id);
                return rol;
            }
            catch (Exception ex) { throw ex; }

        }
        public enum RoleName
        {
            SystemAdmin=1,
            SchoolAdmin=2,
            ClassTeacher=3,
            VicePrincipal=4,
            Parent=5,
            Student=6,
            Account=7,
            Teachers=9,
            Principal=10,
            Cleaner=11,
            Security=12,
            Supervisor=17
        }
    }
}
