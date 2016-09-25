using System;

namespace Mqd.SqlHelper.Entity
{
	 	//EmployeeTerritories
		public class EmployeeTerritories
	{
	
      	/// <summary>
		/// EmployeeID
        /// </summary>
        public virtual int EmployeeID
        {
            get; 
            set; 
        }        
		/// <summary>
		/// TerritoryID
        /// </summary>
        public virtual string TerritoryID
        {
            get; 
            set; 
        }        
		   
	}
}