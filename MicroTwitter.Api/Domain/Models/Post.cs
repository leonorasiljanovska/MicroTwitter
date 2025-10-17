using System.ComponentModel.DataAnnotations;

namespace MicroTwitter.Api.Domain.Models
{
    public class Post
    {
        [Key]   
        public Guid Id { get; set; }

        //[Required]
        //public string Username { get; set; }= string.Empty;

        //The content of the post is only text, with maximum of 140 characters and minimum of 12 characters
        [Required]
        [MinLength(12)]
        [MaxLength(140)]
        public string Content { get; set; }= string.Empty;
        public DateTime CreatedAt { get; set; }= DateTime.UtcNow;
        public string? ImageUrl { get; set; }  //optional Image 

        [Required]
        public Guid UserId { get; set; } 
        public User? User { get; set; } 
    }
}
