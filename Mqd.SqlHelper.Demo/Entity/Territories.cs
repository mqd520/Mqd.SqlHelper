using System;

namespace Mqd.SqlHelper.Entity
{
	 	//Territories
		public class Territories
	{
	
      	/// <summary>
		/// TerritoryID
        /// </summary>
        public virtual string TerritoryID
        {
            get; 
            set; 
        }        
		/// <summary>
		/// TerritoryDescription
        /// </summary>
        public virtual string TerritoryDescription
        {
            get; 
            set; 
        }        
		/// <summary>
		/// RegionID
        /// </summary>
        public virtual int RegionID
        {
            get; 
            set; 
        }        
		   
	}
}