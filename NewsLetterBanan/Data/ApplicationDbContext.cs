using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace NewsLetterBanan.Data
{
    public class ApplicationDbContext : IdentityDbContext<User>// Use User instead of IdentityUser
    {

        public virtual DbSet<Article> Articles { get; set; }
     
        public virtual DbSet<Tag> Tags { get; set; }
        public virtual DbSet<Subscription> Subscriptions { get; set; }
        public virtual DbSet<Comment> Comments { get; set; }
        public virtual DbSet<UserLikes> UserLikes { get; set; }
        public virtual DbSet<SubscriptionsType> SubscriptionsTypes { get; set; }
        public virtual DbSet<Image> Images { get; set; }
        public virtual DbSet<WeatherForecast> WeatherForecast { get; set; }
        public virtual DbSet<UserCommentLikes> UserCommentLikes { get; set; }
        public virtual DbSet<Category> Categories { get; set; }
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);


         // Simplified relationship between Tag and Article (many-to-many)
         modelBuilder.Entity<Article>()
                .HasMany(a => a.Tags)
                .WithMany(t => t.Articles)
                .UsingEntity<Dictionary<string, object>>(
                    "ArticleTag",  // Join table name
                    at => at.HasOne<Tag>().WithMany().HasForeignKey("TagId"),
                    at => at.HasOne<Article>().WithMany().HasForeignKey("ArticleId"));

            // Many-to-many relationship between Article and Category
            modelBuilder.Entity<Article>()
                .HasMany(a => a.Categories)
                .WithMany(c => c.Articles)
                .UsingEntity<Dictionary<string, object>>(
                    "ArticleCategory", // Join table name
                    ac => ac.HasOne<Category>().WithMany().HasForeignKey("CategoryId"),
                    ac => ac.HasOne<Article>().WithMany().HasForeignKey("ArticleId"));
        

        modelBuilder.Entity<UserLikes>()
                .HasOne(ul => ul.User)
                .WithMany(u => u.UserLikes)
                .HasForeignKey(ul => ul.UserId)
                .OnDelete(DeleteBehavior.Restrict);

          
            modelBuilder.Entity<UserLikes>()
                .HasOne(ul => ul.Article)
                .WithMany(a => a.UserLikes)
                .HasForeignKey(ul => ul.ArticleId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Comment>()
                .HasOne(c => c.User)
                .WithMany(u => u.Comments)
                .HasForeignKey(c => c.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Comment>()
                .HasOne(c => c.Article)
                .WithMany(a => a.Comments)
                .HasForeignKey(c => c.ArticleId)
                .OnDelete(DeleteBehavior.Restrict);

  
            modelBuilder.Entity<UserCommentLikes>()
                .HasOne(ucl => ucl.User)
                .WithMany(u => u.UserCommentLikes)
                .HasForeignKey(ucl => ucl.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            // Comment -> UserCommentLikes (Cascade delete for Comment)
            modelBuilder.Entity<UserCommentLikes>()
                .HasOne(ucl => ucl.Comment)
                .WithMany(c => c.UserCommentLikes)
                .HasForeignKey(ucl => ucl.CommentId)
                .OnDelete(DeleteBehavior.Cascade);





        }
    }

}