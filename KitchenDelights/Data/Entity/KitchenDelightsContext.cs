using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Data.Entity;

public partial class KitchenDelightsContext : DbContext
{
    public KitchenDelightsContext()
    {
    }

    public KitchenDelightsContext(DbContextOptions<KitchenDelightsContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Account> Accounts { get; set; }

    public virtual DbSet<Address> Addresses { get; set; }

    public virtual DbSet<Advertisement> Advertisements { get; set; }

    public virtual DbSet<Blog> Blogs { get; set; }

    public virtual DbSet<BlogComment> BlogComments { get; set; }

    public virtual DbSet<CartItem> CartItems { get; set; }

    public virtual DbSet<Category> Categories { get; set; }

    public virtual DbSet<Menu> Menus { get; set; }

    public virtual DbSet<News> News { get; set; }

    public virtual DbSet<PaymentHistory> PaymentHistories { get; set; }

    public virtual DbSet<Recipe> Recipes { get; set; }

    public virtual DbSet<RecipeRating> RecipeRatings { get; set; }

    public virtual DbSet<RestaurantRecommendation> RestaurantRecommendations { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<Status> Statuses { get; set; }

    public virtual DbSet<Voucher> Vouchers { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.UseCollation("Latin1_General_100_CI_AI_SC_UTF8");

        modelBuilder.Entity<Account>(entity =>
        {
            entity.HasKey(e => e.AccountId).HasName("PK__account__46A222CD7BE8C95A");

            entity.ToTable("account");

            entity.Property(e => e.AccountId).HasColumnName("account_id");
            entity.Property(e => e.Avatar)
                .HasMaxLength(1)
                .IsUnicode(false)
                .HasColumnName("avatar");
            entity.Property(e => e.Email)
                .HasMaxLength(50)
                .HasColumnName("email");
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .HasColumnName("name");
            entity.Property(e => e.PasswordHash)
                .HasMaxLength(1)
                .IsUnicode(false)
                .HasColumnName("password_hash");
            entity.Property(e => e.Phone)
                .HasMaxLength(12)
                .HasColumnName("phone");
            entity.Property(e => e.ResetExpire)
                .HasColumnType("datetime")
                .HasColumnName("reset_expire");
            entity.Property(e => e.ResetToken)
                .HasMaxLength(1)
                .IsUnicode(false)
                .HasColumnName("reset_token");
            entity.Property(e => e.RoleId).HasColumnName("role_id");
            entity.Property(e => e.StatusId).HasColumnName("status_id");

            entity.HasOne(d => d.Role).WithMany(p => p.Accounts)
                .HasForeignKey(d => d.RoleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__account__role_id__3E52440B");

            entity.HasOne(d => d.Status).WithMany(p => p.Accounts)
                .HasForeignKey(d => d.StatusId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__account__status___3D5E1FD2");

            entity.HasMany(d => d.Recipes).WithMany(p => p.Accounts)
                .UsingEntity<Dictionary<string, object>>(
                    "Bookmark",
                    r => r.HasOne<Recipe>().WithMany()
                        .HasForeignKey("RecipeId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK__bookmark__recipe__5CD6CB2B"),
                    l => l.HasOne<Account>().WithMany()
                        .HasForeignKey("AccountId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK__bookmark__accoun__5BE2A6F2"),
                    j =>
                    {
                        j.HasKey("AccountId", "RecipeId").HasName("PK__bookmark__E5F53C14542990CF");
                        j.ToTable("bookmark");
                        j.IndexerProperty<int>("AccountId").HasColumnName("account_id");
                        j.IndexerProperty<int>("RecipeId").HasColumnName("recipe_id");
                    });
        });

        modelBuilder.Entity<Address>(entity =>
        {
            entity.HasKey(e => e.AddressId).HasName("PK__address__CAA247C8166D87B6");

            entity.ToTable("address");

            entity.Property(e => e.AddressId).HasColumnName("address_id");
            entity.Property(e => e.AccountId).HasColumnName("account_id");
            entity.Property(e => e.AddressDetails)
                .HasMaxLength(1)
                .HasColumnName("address_details");

            entity.HasOne(d => d.Account).WithMany(p => p.Addresses)
                .HasForeignKey(d => d.AccountId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__address__account__412EB0B6");
        });

        modelBuilder.Entity<Advertisement>(entity =>
        {
            entity.HasKey(e => e.AdvertisementId).HasName("PK__advertis__7F5017912EE2B3CD");

            entity.ToTable("advertisement");

            entity.Property(e => e.AdvertisementId).HasColumnName("advertisement_id");
            entity.Property(e => e.AdvertisementImage)
                .HasMaxLength(1)
                .IsUnicode(false)
                .HasColumnName("advertisement_image");
            entity.Property(e => e.AdvertisementLink)
                .HasMaxLength(1)
                .IsUnicode(false)
                .HasColumnName("advertisement_link");
            entity.Property(e => e.AdvertisementStatus).HasColumnName("advertisement_status");
        });

        modelBuilder.Entity<Blog>(entity =>
        {
            entity.HasKey(e => e.BlogId).HasName("PK__blog__2975AA28C5EC6825");

            entity.ToTable("blog");

            entity.Property(e => e.BlogId).HasColumnName("blog_id");
            entity.Property(e => e.AccountId).HasColumnName("account_id");
            entity.Property(e => e.BlogContent)
                .HasMaxLength(1)
                .HasColumnName("blog_content");
            entity.Property(e => e.BlogImage)
                .HasMaxLength(1)
                .HasColumnName("blog_image");
            entity.Property(e => e.BlogStatus).HasColumnName("blog_status");
            entity.Property(e => e.BlogTitle)
                .HasMaxLength(1)
                .HasColumnName("blog_title");
            entity.Property(e => e.CreateDate)
                .HasColumnType("datetime")
                .HasColumnName("create_date");

            entity.HasOne(d => d.Account).WithMany(p => p.Blogs)
                .HasForeignKey(d => d.AccountId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__blog__account_id__5441852A");
        });

        modelBuilder.Entity<BlogComment>(entity =>
        {
            entity.HasKey(e => e.CommentId).HasName("PK__blog_com__E79576878C6428F2");

            entity.ToTable("blog_comment");

            entity.Property(e => e.CommentId).HasColumnName("comment_id");
            entity.Property(e => e.AccountId).HasColumnName("account_id");
            entity.Property(e => e.BlogId).HasColumnName("blog_id");
            entity.Property(e => e.CommentContent)
                .HasMaxLength(1)
                .HasColumnName("comment_content");
            entity.Property(e => e.CreateDate)
                .HasColumnType("datetime")
                .HasColumnName("create_date");
            entity.Property(e => e.ParentId).HasColumnName("parent_id");

            entity.HasOne(d => d.Account).WithMany(p => p.BlogComments)
                .HasForeignKey(d => d.AccountId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__blog_comm__accou__59063A47");

            entity.HasOne(d => d.Blog).WithMany(p => p.BlogComments)
                .HasForeignKey(d => d.BlogId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__blog_comm__blog___571DF1D5");

            entity.HasOne(d => d.Parent).WithMany(p => p.InverseParent)
                .HasForeignKey(d => d.ParentId)
                .HasConstraintName("FK__blog_comm__paren__5812160E");
        });

        modelBuilder.Entity<CartItem>(entity =>
        {
            entity.HasKey(e => new { e.AccountId, e.RecipeId }).HasName("PK__cart_ite__E5F53C14CC241593");

            entity.ToTable("cart_item");

            entity.Property(e => e.AccountId).HasColumnName("account_id");
            entity.Property(e => e.RecipeId).HasColumnName("recipe_id");
            entity.Property(e => e.VoucherCode)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("voucher_code");

            entity.HasOne(d => d.Account).WithMany(p => p.CartItems)
                .HasForeignKey(d => d.AccountId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__cart_item__accou__66603565");

            entity.HasOne(d => d.Recipe).WithMany(p => p.CartItems)
                .HasForeignKey(d => d.RecipeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__cart_item__recip__6754599E");

            entity.HasOne(d => d.VoucherCodeNavigation).WithMany(p => p.CartItems)
                .HasForeignKey(d => d.VoucherCode)
                .HasConstraintName("FK__cart_item__vouch__68487DD7");
        });

        modelBuilder.Entity<Category>(entity =>
        {
            entity.HasKey(e => e.CategoryId).HasName("PK__category__D54EE9B40DDA9D35");

            entity.ToTable("category");

            entity.Property(e => e.CategoryId).HasColumnName("category_id");
            entity.Property(e => e.CategoryName)
                .HasMaxLength(1)
                .HasColumnName("category_name");
            entity.Property(e => e.ParentId).HasColumnName("parent_id");

            entity.HasOne(d => d.Parent).WithMany(p => p.InverseParent)
                .HasForeignKey(d => d.ParentId)
                .HasConstraintName("FK__category__parent__49C3F6B7");
        });

        modelBuilder.Entity<Menu>(entity =>
        {
            entity.HasKey(e => e.MenuId).HasName("PK__menu__4CA0FADC75BABF7E");

            entity.ToTable("menu");

            entity.Property(e => e.MenuId).HasColumnName("menu_id");
            entity.Property(e => e.AccountId).HasColumnName("account_id");
            entity.Property(e => e.MenuName)
                .HasMaxLength(1)
                .HasColumnName("menu_name");
            entity.Property(e => e.RecipeId).HasColumnName("recipe_id");

            entity.HasOne(d => d.Account).WithMany(p => p.Menus)
                .HasForeignKey(d => d.AccountId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__menu__account_id__5FB337D6");

            entity.HasOne(d => d.Recipe).WithMany(p => p.Menus)
                .HasForeignKey(d => d.RecipeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__menu__recipe_id__60A75C0F");
        });

        modelBuilder.Entity<News>(entity =>
        {
            entity.HasKey(e => e.NewsId).HasName("PK__news__4C27CCD8258B6DDB");

            entity.ToTable("news");

            entity.Property(e => e.NewsId).HasColumnName("news_id");
            entity.Property(e => e.AccountId).HasColumnName("account_id");
            entity.Property(e => e.CreateDate)
                .HasColumnType("datetime")
                .HasColumnName("create_date");
            entity.Property(e => e.NewsContent)
                .HasMaxLength(1)
                .HasColumnName("news_content");
            entity.Property(e => e.NewsStatus).HasColumnName("news_status");
            entity.Property(e => e.NewsTitle)
                .HasMaxLength(1)
                .HasColumnName("news_title");

            entity.HasOne(d => d.Account).WithMany(p => p.News)
                .HasForeignKey(d => d.AccountId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__news__account_id__46E78A0C");
        });

        modelBuilder.Entity<PaymentHistory>(entity =>
        {
            entity.HasKey(e => new { e.AccountId, e.RecipeId }).HasName("PK__payment___E5F53C14D46D8109");

            entity.ToTable("payment_history");

            entity.Property(e => e.AccountId).HasColumnName("account_id");
            entity.Property(e => e.RecipeId).HasColumnName("recipe_id");
            entity.Property(e => e.ActualPrice)
                .HasColumnType("money")
                .HasColumnName("actual_price");
            entity.Property(e => e.PurchaseDate)
                .HasColumnType("datetime")
                .HasColumnName("purchase_date");

            entity.HasOne(d => d.Account).WithMany(p => p.PaymentHistories)
                .HasForeignKey(d => d.AccountId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__payment_h__accou__6B24EA82");

            entity.HasOne(d => d.Recipe).WithMany(p => p.PaymentHistories)
                .HasForeignKey(d => d.RecipeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__payment_h__recip__6C190EBB");
        });

        modelBuilder.Entity<Recipe>(entity =>
        {
            entity.HasKey(e => e.RecipeId).HasName("PK__recipe__3571ED9B0BBDA0F8");

            entity.ToTable("recipe");

            entity.Property(e => e.RecipeId).HasColumnName("recipe_id");
            entity.Property(e => e.AccountId).HasColumnName("account_id");
            entity.Property(e => e.CategoryId).HasColumnName("category_id");
            entity.Property(e => e.CreateDate)
                .HasColumnType("datetime")
                .HasColumnName("create_date");
            entity.Property(e => e.IsFree).HasColumnName("is_free");
            entity.Property(e => e.RecipeContent)
                .HasMaxLength(1)
                .HasColumnName("recipe_content");
            entity.Property(e => e.RecipePrice)
                .HasColumnType("money")
                .HasColumnName("recipe_price");
            entity.Property(e => e.RecipeRating).HasColumnName("recipe_rating");
            entity.Property(e => e.RecipeStatus).HasColumnName("recipe_status");
            entity.Property(e => e.RecipeTitle)
                .HasMaxLength(1)
                .HasColumnName("recipe_title");

            entity.HasOne(d => d.Account).WithMany(p => p.RecipesNavigation)
                .HasForeignKey(d => d.AccountId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__recipe__account___4CA06362");

            entity.HasOne(d => d.Category).WithMany(p => p.Recipes)
                .HasForeignKey(d => d.CategoryId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__recipe__category__4D94879B");
        });

        modelBuilder.Entity<RecipeRating>(entity =>
        {
            entity.HasKey(e => e.RatingId).HasName("PK__recipe_r__D35B278BA7082B8E");

            entity.ToTable("recipe_rating");

            entity.Property(e => e.RatingId).HasColumnName("rating_id");
            entity.Property(e => e.AccountId).HasColumnName("account_id");
            entity.Property(e => e.CreateDate)
                .HasColumnType("datetime")
                .HasColumnName("create_date");
            entity.Property(e => e.RatingContent)
                .HasMaxLength(1)
                .HasColumnName("rating_content");
            entity.Property(e => e.RatingValue).HasColumnName("rating_value");
            entity.Property(e => e.RecipeId).HasColumnName("recipe_id");

            entity.HasOne(d => d.Account).WithMany(p => p.RecipeRatings)
                .HasForeignKey(d => d.AccountId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__recipe_ra__accou__5165187F");

            entity.HasOne(d => d.Recipe).WithMany(p => p.RecipeRatings)
                .HasForeignKey(d => d.RecipeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__recipe_ra__recip__5070F446");
        });

        modelBuilder.Entity<RestaurantRecommendation>(entity =>
        {
            entity.HasKey(e => e.RestaurantId).HasName("PK__restaura__3B0FAA910AFE4CA4");

            entity.ToTable("restaurant_recommendation");

            entity.Property(e => e.RestaurantId).HasColumnName("restaurant_id");
            entity.Property(e => e.AccountId).HasColumnName("account_id");
            entity.Property(e => e.RestaurantDetails)
                .HasMaxLength(1)
                .HasColumnName("restaurant_details");
            entity.Property(e => e.RestaurantLocation)
                .HasMaxLength(1)
                .HasColumnName("restaurant_location");
            entity.Property(e => e.RestaurantName)
                .HasMaxLength(1)
                .HasColumnName("restaurant_name");

            entity.HasOne(d => d.Account).WithMany(p => p.RestaurantRecommendations)
                .HasForeignKey(d => d.AccountId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__restauran__accou__440B1D61");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.RoleId).HasName("PK__role__760965CCE8D87DEC");

            entity.ToTable("role");

            entity.Property(e => e.RoleId).HasColumnName("role_id");
            entity.Property(e => e.RoleName)
                .HasMaxLength(25)
                .HasColumnName("role_name");
        });

        modelBuilder.Entity<Status>(entity =>
        {
            entity.HasKey(e => e.StatusId).HasName("PK__status__3683B531D3A689AC");

            entity.ToTable("status");

            entity.Property(e => e.StatusId).HasColumnName("status_id");
            entity.Property(e => e.StatusName)
                .HasMaxLength(25)
                .HasColumnName("status_name");
        });

        modelBuilder.Entity<Voucher>(entity =>
        {
            entity.HasKey(e => e.VoucherCode).HasName("PK__voucher__2173106848754DCC");

            entity.ToTable("voucher");

            entity.Property(e => e.VoucherCode)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("voucher_code");
            entity.Property(e => e.AccountId).HasColumnName("account_id");
            entity.Property(e => e.DiscountPercentage).HasColumnName("discount_percentage");

            entity.HasOne(d => d.Account).WithMany(p => p.Vouchers)
                .HasForeignKey(d => d.AccountId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__voucher__account__6383C8BA");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
