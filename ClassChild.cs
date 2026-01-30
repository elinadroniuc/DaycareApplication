using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DaycareApplication
{
    public class ClassChild
    {
        public int ChildId { get; set; }
        public string ChildFullName { get; set; }
        public DateTime ChildBirthday { get; set; }
        public string ChildBirthdayCertificate { get; set; }

        public ClassChild(
            int childId,
            string childFullName,
            DateTime childBirthday,
            string childBirthdayCertificate) 
        {
            ChildId = childId;
            ChildFullName = childFullName;
            ChildBirthday = childBirthday;
            ChildBirthdayCertificate = childBirthdayCertificate;
        }

        public override string ToString()
        {
            return $"{ChildId};{ChildFullName};{ChildBirthday};{ChildBirthdayCertificate};";
        }
    }
}
