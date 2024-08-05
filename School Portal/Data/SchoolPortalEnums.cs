using Microsoft.EntityFrameworkCore.Migrations;
using School_Portal.Models;

namespace School_Portal.Data
{
	//enum is usually at where enum class are constant
	// Below i created enum for course as CourseStatus and set approve button to 1 while Remove botton to 2
	public class SchoolPortalEnums
	{
		public enum CourseStatus
		{
			Approve = 1,	
			Remove = 2,
		}

		public enum CourseType { 
			
		}
		public enum SchoolType { }
		public enum SchoolTypeEnum { }
		// Below, is enum for applicationUser
		public enum UserStatus
		{
			Approve = 1,
			Decline = 2,
			Waiting = 3,
		}
        public enum PaymentStatus
        {
            Approve = 1,
            Decline = 2,
            Pending = 3,
        }
    }
}

