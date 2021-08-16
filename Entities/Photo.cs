using System.ComponentModel.DataAnnotations.Schema;

namespace DatingApp.Entities  //Aim: Photos to be added to users photo collection
{ //instead of creating new table by using DbSet<Photo> ,we can create inside Users table using EF Relationship  Concepts  
    [Table("Photos")]  //create a table and table name is Photos
    public class Photo
    {
        public int Id { get; set; }
        public string Url { get; set; }
        public bool IsMain { get; set; }
        public string PublicId { get; set; }
        public AppUser AppUser { get; set; }
        public int AppUserId { get; set; }
        //Each photo collection have an user
    }
}