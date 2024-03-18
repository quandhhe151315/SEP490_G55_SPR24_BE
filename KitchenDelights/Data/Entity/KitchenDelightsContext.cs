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

    public virtual DbSet<Address> Addresses { get; set; }

    public virtual DbSet<Advertisement> Advertisements { get; set; }

    public virtual DbSet<Blog> Blogs { get; set; }

    public virtual DbSet<BlogComment> BlogComments { get; set; }

    public virtual DbSet<CartItem> CartItems { get; set; }

    public virtual DbSet<Category> Categories { get; set; }

    public virtual DbSet<Country> Countries { get; set; }

    public virtual DbSet<Ingredient> Ingredients { get; set; }

    public virtual DbSet<IngredientMarketplace> IngredientMarketplaces { get; set; }

    public virtual DbSet<Marketplace> Marketplaces { get; set; }

    public virtual DbSet<Menu> Menus { get; set; }

    public virtual DbSet<News> News { get; set; }

    public virtual DbSet<PaymentHistory> PaymentHistories { get; set; }

    public virtual DbSet<Recipe> Recipes { get; set; }

    public virtual DbSet<RecipeIngredient> RecipeIngredients { get; set; }

    public virtual DbSet<RecipeRating> RecipeRatings { get; set; }

    public virtual DbSet<RestaurantRecommendation> RestaurantRecommendations { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<Status> Statuses { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<Verification> Verifications { get; set; }

    public virtual DbSet<Voucher> Vouchers { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.UseCollation("Latin1_General_100_CI_AI_SC_UTF8");

        modelBuilder.Entity<Address>(entity =>
        {
            entity.HasKey(e => e.AddressId).HasName("PK__address__CAA247C80AA07301");

            entity.ToTable("address");

            entity.Property(e => e.AddressId).HasColumnName("address_id");
            entity.Property(e => e.AddressDetails).HasColumnName("address_details");
            entity.Property(e => e.UserId).HasColumnName("user_id");

            entity.HasOne(d => d.User).WithMany(p => p.Addresses)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__address__user_id__412EB0B6");
        });

        modelBuilder.Entity<Advertisement>(entity =>
        {
            entity.HasKey(e => e.AdvertisementId).HasName("PK__advertis__7F50179172D06394");

            entity.ToTable("advertisement");

            entity.Property(e => e.AdvertisementId).HasColumnName("advertisement_id");
            entity.Property(e => e.AdvertisementImage)
                .IsUnicode(false)
                .HasColumnName("advertisement_image");
            entity.Property(e => e.AdvertisementLink)
                .IsUnicode(false)
                .HasColumnName("advertisement_link");
            entity.Property(e => e.AdvertisementStatus).HasColumnName("advertisement_status");
        });

        modelBuilder.Entity<Blog>(entity =>
        {
            entity.HasKey(e => e.BlogId).HasName("PK__blog__2975AA28CDAB3AF8");

            entity.ToTable("blog");

            entity.Property(e => e.BlogId).HasColumnName("blog_id");
            entity.Property(e => e.BlogContent).HasColumnName("blog_content");
            entity.Property(e => e.BlogImage).HasColumnName("blog_image");
            entity.Property(e => e.BlogStatus).HasColumnName("blog_status");
            entity.Property(e => e.BlogTitle).HasColumnName("blog_title");
            entity.Property(e => e.CategoryId).HasColumnName("category_id");
            entity.Property(e => e.CreateDate)
                .HasColumnType("datetime")
                .HasColumnName("create_date");
            entity.Property(e => e.UserId).HasColumnName("user_id");

            entity.HasOne(d => d.Category).WithMany(p => p.Blogs)
                .HasForeignKey(d => d.CategoryId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__blog__category_i__693CA210");

            entity.HasOne(d => d.User).WithMany(p => p.Blogs)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__blog__user_id__68487DD7");
        });

        modelBuilder.Entity<BlogComment>(entity =>
        {
            entity.HasKey(e => e.CommentId).HasName("PK__blog_com__E79576873AA5C13B");

            entity.ToTable("blog_comment");

            entity.Property(e => e.CommentId).HasColumnName("comment_id");
            entity.Property(e => e.BlogId).HasColumnName("blog_id");
            entity.Property(e => e.CommentContent).HasColumnName("comment_content");
            entity.Property(e => e.CommentStatus).HasColumnName("comment_status");
            entity.Property(e => e.CreateDate)
                .HasColumnType("datetime")
                .HasColumnName("create_date");
            entity.Property(e => e.ParentId).HasColumnName("parent_id");
            entity.Property(e => e.UserId).HasColumnName("user_id");

            entity.HasOne(d => d.Blog).WithMany(p => p.BlogComments)
                .HasForeignKey(d => d.BlogId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__blog_comm__blog___6C190EBB");

            entity.HasOne(d => d.Parent).WithMany(p => p.InverseParent)
                .HasForeignKey(d => d.ParentId)
                .HasConstraintName("FK__blog_comm__paren__6D0D32F4");

            entity.HasOne(d => d.User).WithMany(p => p.BlogComments)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__blog_comm__user___6E01572D");
        });

        modelBuilder.Entity<CartItem>(entity =>
        {
            entity.HasKey(e => new { e.UserId, e.RecipeId }).HasName("PK__cart_ite__1AE929D60312922F");

            entity.ToTable("cart_item");

            entity.Property(e => e.UserId).HasColumnName("user_id");
            entity.Property(e => e.RecipeId).HasColumnName("recipe_id");
            entity.Property(e => e.VoucherCode)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("voucher_code");

            entity.HasOne(d => d.Recipe).WithMany(p => p.CartItems)
                .HasForeignKey(d => d.RecipeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__cart_item__recip__7F2BE32F");

            entity.HasOne(d => d.User).WithMany(p => p.CartItems)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__cart_item__user___7E37BEF6");

            entity.HasOne(d => d.VoucherCodeNavigation).WithMany(p => p.CartItems)
                .HasForeignKey(d => d.VoucherCode)
                .HasConstraintName("FK__cart_item__vouch__00200768");
        });

        modelBuilder.Entity<Category>(entity =>
        {
            entity.HasKey(e => e.CategoryId).HasName("PK__category__D54EE9B44AF328E8");

            entity.ToTable("category");

            entity.Property(e => e.CategoryId).HasColumnName("category_id");
            entity.Property(e => e.CategoryName).HasColumnName("category_name");
            entity.Property(e => e.CategoryStatus).HasColumnName("category_status");
            entity.Property(e => e.CategoryType).HasColumnName("category_type");
            entity.Property(e => e.ParentId).HasColumnName("parent_id");

            entity.HasOne(d => d.Parent).WithMany(p => p.InverseParent)
                .HasForeignKey(d => d.ParentId)
                .HasConstraintName("FK__category__parent__49C3F6B7");
        });

        modelBuilder.Entity<Country>(entity =>
        {
            entity.HasKey(e => e.CountryId).HasName("PK__countrie__7E8CD0554E6C2BF8");

            entity.ToTable("countries");

            entity.Property(e => e.CountryId).HasColumnName("country_id");
            entity.Property(e => e.CountryName).HasColumnName("country_name");
            entity.Property(e => e.CountryStatus).HasColumnName("country_status");
        });

        modelBuilder.Entity<Ingredient>(entity =>
        {
            entity.HasKey(e => e.IngredientId).HasName("PK__ingredie__B0E453CF78E4D214");

            entity.ToTable("ingredient");

            entity.Property(e => e.IngredientId).HasColumnName("ingredient_id");
            entity.Property(e => e.IngredientName).HasColumnName("ingredient_name");
            entity.Property(e => e.IngredientStatus).HasColumnName("ingredient_status");
            entity.Property(e => e.IngredientUnit)
                .HasMaxLength(50)
                .HasColumnName("ingredient_unit");
        });

        modelBuilder.Entity<IngredientMarketplace>(entity =>
        {
            entity.HasKey(e => new { e.IngredientId, e.MarketplaceId }).HasName("PK__ingredie__4858CF754EE5C7B2");

            entity.ToTable("ingredient_marketplace");

            entity.Property(e => e.IngredientId).HasColumnName("ingredient_id");
            entity.Property(e => e.MarketplaceId).HasColumnName("marketplace_id");
            entity.Property(e => e.MarketplaceLink).HasColumnName("marketplace_link");

            entity.HasOne(d => d.Ingredient).WithMany(p => p.IngredientMarketplaces)
                .HasForeignKey(d => d.IngredientId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ingredien__ingre__5CD6CB2B");

            entity.HasOne(d => d.Marketplace).WithMany(p => p.IngredientMarketplaces)
                .HasForeignKey(d => d.MarketplaceId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ingredien__marke__5DCAEF64");
        });

        modelBuilder.Entity<Marketplace>(entity =>
        {
            entity.HasKey(e => e.MarketplaceId).HasName("PK__marketpl__8BC9CBAE38F15BC5");

            entity.ToTable("marketplace");

            entity.Property(e => e.MarketplaceId).HasColumnName("marketplace_id");
            entity.Property(e => e.MarketplaceLogo).HasColumnName("marketplace_logo");
            entity.Property(e => e.MarketplaceName).HasColumnName("marketplace_name");
            entity.Property(e => e.MarketplaceStatus).HasColumnName("marketplace_status");
        });

        modelBuilder.Entity<Menu>(entity =>
        {
            entity.HasKey(e => e.MenuId).HasName("PK__menu__4CA0FADCAFFFDE58");

            entity.ToTable("menu");

            entity.Property(e => e.MenuId).HasColumnName("menu_id");
            entity.Property(e => e.FeaturedImage).HasColumnName("featured_image");
            entity.Property(e => e.MenuDescription).HasColumnName("menu_description");
            entity.Property(e => e.MenuName).HasColumnName("menu_name");
            entity.Property(e => e.UserId).HasColumnName("user_id");

            entity.HasOne(d => d.User).WithMany(p => p.Menus)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__menu__user_id__74AE54BC");

            entity.HasMany(d => d.Recipes).WithMany(p => p.Menus)
                .UsingEntity<Dictionary<string, object>>(
                    "MenuRecipe",
                    r => r.HasOne<Recipe>().WithMany()
                        .HasForeignKey("RecipeId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK__menu_reci__recip__787EE5A0"),
                    l => l.HasOne<Menu>().WithMany()
                        .HasForeignKey("MenuId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK__menu_reci__menu___778AC167"),
                    j =>
                    {
                        j.HasKey("MenuId", "RecipeId").HasName("PK__menu_rec__EFF7E405B0B3146E");
                        j.ToTable("menu_recipe");
                        j.IndexerProperty<int>("MenuId").HasColumnName("menu_id");
                        j.IndexerProperty<int>("RecipeId").HasColumnName("recipe_id");
                    });
        });

        modelBuilder.Entity<News>(entity =>
        {
            entity.HasKey(e => e.NewsId).HasName("PK__news__4C27CCD82F59036E");

            entity.ToTable("news");

            entity.Property(e => e.NewsId).HasColumnName("news_id");
            entity.Property(e => e.CreateDate)
                .HasColumnType("datetime")
                .HasColumnName("create_date");
            entity.Property(e => e.FeaturedImage).HasColumnName("featured_image");
            entity.Property(e => e.NewsContent).HasColumnName("news_content");
            entity.Property(e => e.NewsStatus).HasColumnName("news_status");
            entity.Property(e => e.NewsTitle).HasColumnName("news_title");
            entity.Property(e => e.UserId).HasColumnName("user_id");

            entity.HasOne(d => d.User).WithMany(p => p.News)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__news__user_id__46E78A0C");
        });

        modelBuilder.Entity<PaymentHistory>(entity =>
        {
            entity.HasKey(e => new { e.UserId, e.RecipeId }).HasName("PK__payment___1AE929D6F2B637B6");

            entity.ToTable("payment_history");

            entity.Property(e => e.UserId).HasColumnName("user_id");
            entity.Property(e => e.RecipeId).HasColumnName("recipe_id");
            entity.Property(e => e.ActualPrice)
                .HasColumnType("money")
                .HasColumnName("actual_price");
            entity.Property(e => e.PurchaseDate)
                .HasColumnType("datetime")
                .HasColumnName("purchase_date");

            entity.HasOne(d => d.Recipe).WithMany(p => p.PaymentHistories)
                .HasForeignKey(d => d.RecipeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__payment_h__recip__03F0984C");

            entity.HasOne(d => d.User).WithMany(p => p.PaymentHistories)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__payment_h__user___02FC7413");
        });

        modelBuilder.Entity<Recipe>(entity =>
        {
            entity.HasKey(e => e.RecipeId).HasName("PK__recipe__3571ED9BDB1E442C");

            entity.ToTable("recipe");

            entity.Property(e => e.RecipeId).HasColumnName("recipe_id");
            entity.Property(e => e.CookTime).HasColumnName("cook_time");
            entity.Property(e => e.CreateDate)
                .HasColumnType("datetime")
                .HasColumnName("create_date");
            entity.Property(e => e.FeaturedImage).HasColumnName("featured_image");
            entity.Property(e => e.IsFree).HasColumnName("is_free");
            entity.Property(e => e.PreparationTime).HasColumnName("preparation_time");
            entity.Property(e => e.RecipeContent).HasColumnName("recipe_content");
            entity.Property(e => e.RecipeDescription).HasColumnName("recipe_description");
            entity.Property(e => e.RecipePrice)
                .HasColumnType("money")
                .HasColumnName("recipe_price");
            entity.Property(e => e.RecipeRating)
                .HasColumnType("decimal(2, 1)")
                .HasColumnName("recipe_rating");
            entity.Property(e => e.RecipeServe).HasColumnName("recipe_serve");
            entity.Property(e => e.RecipeStatus).HasColumnName("recipe_status");
            entity.Property(e => e.RecipeTitle).HasColumnName("recipe_title");
            entity.Property(e => e.UserId).HasColumnName("user_id");
            entity.Property(e => e.VideoLink).HasColumnName("video_link");

            entity.HasOne(d => d.User).WithMany(p => p.RecipesNavigation)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__recipe__user_id__4E88ABD4");

            entity.HasMany(d => d.Categories).WithMany(p => p.Recipes)
                .UsingEntity<Dictionary<string, object>>(
                    "RecipeCategory",
                    r => r.HasOne<Category>().WithMany()
                        .HasForeignKey("CategoryId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK__recipe_ca__categ__52593CB8"),
                    l => l.HasOne<Recipe>().WithMany()
                        .HasForeignKey("RecipeId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK__recipe_ca__recip__5165187F"),
                    j =>
                    {
                        j.HasKey("RecipeId", "CategoryId").HasName("PK__recipe_c__68250300126E0781");
                        j.ToTable("recipe_category");
                        j.IndexerProperty<int>("RecipeId").HasColumnName("recipe_id");
                        j.IndexerProperty<int>("CategoryId").HasColumnName("category_id");
                    });

            entity.HasMany(d => d.Countries).WithMany(p => p.Recipes)
                .UsingEntity<Dictionary<string, object>>(
                    "RecipeCountry",
                    r => r.HasOne<Country>().WithMany()
                        .HasForeignKey("CountryId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK__recipe_co__count__5629CD9C"),
                    l => l.HasOne<Recipe>().WithMany()
                        .HasForeignKey("RecipeId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK__recipe_co__recip__5535A963"),
                    j =>
                    {
                        j.HasKey("RecipeId", "CountryId").HasName("PK__recipe_c__7299209EA23DC05C");
                        j.ToTable("recipe_country");
                        j.IndexerProperty<int>("RecipeId").HasColumnName("recipe_id");
                        j.IndexerProperty<int>("CountryId").HasColumnName("country_id");
                    });
        });

        modelBuilder.Entity<RecipeIngredient>(entity =>
        {
            entity.HasKey(e => new { e.RecipeId, e.IngredientId }).HasName("PK__recipe_i__DE7FA8A7648F285F");

            entity.ToTable("recipe_ingredient");

            entity.Property(e => e.RecipeId).HasColumnName("recipe_id");
            entity.Property(e => e.IngredientId).HasColumnName("ingredient_id");
            entity.Property(e => e.UnitValue)
                .HasColumnType("decimal(9, 2)")
                .HasColumnName("unit_value");

            entity.HasOne(d => d.Ingredient).WithMany(p => p.RecipeIngredients)
                .HasForeignKey(d => d.IngredientId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__recipe_in__ingre__619B8048");

            entity.HasOne(d => d.Recipe).WithMany(p => p.RecipeIngredients)
                .HasForeignKey(d => d.RecipeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__recipe_in__recip__60A75C0F");
        });

        modelBuilder.Entity<RecipeRating>(entity =>
        {
            entity.HasKey(e => e.RatingId).HasName("PK__recipe_r__D35B278B9AC3232C");

            entity.ToTable("recipe_rating");

            entity.Property(e => e.RatingId).HasColumnName("rating_id");
            entity.Property(e => e.CreateDate)
                .HasColumnType("datetime")
                .HasColumnName("create_date");
            entity.Property(e => e.RatingContent).HasColumnName("rating_content");
            entity.Property(e => e.RatingStatus).HasColumnName("rating_status");
            entity.Property(e => e.RatingValue).HasColumnName("rating_value");
            entity.Property(e => e.RecipeId).HasColumnName("recipe_id");
            entity.Property(e => e.UserId).HasColumnName("user_id");

            entity.HasOne(d => d.Recipe).WithMany(p => p.RecipeRatings)
                .HasForeignKey(d => d.RecipeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__recipe_ra__recip__6477ECF3");

            entity.HasOne(d => d.User).WithMany(p => p.RecipeRatings)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__recipe_ra__user___656C112C");
        });

        modelBuilder.Entity<RestaurantRecommendation>(entity =>
        {
            entity.HasKey(e => e.RestaurantId).HasName("PK__restaura__3B0FAA91C87247B9");

            entity.ToTable("restaurant_recommendation");

            entity.Property(e => e.RestaurantId).HasColumnName("restaurant_id");
            entity.Property(e => e.FeaturedImage).HasColumnName("featured_image");
            entity.Property(e => e.RestaurantDetails).HasColumnName("restaurant_details");
            entity.Property(e => e.RestaurantLocation).HasColumnName("restaurant_location");
            entity.Property(e => e.RestaurantName).HasColumnName("restaurant_name");
            entity.Property(e => e.UserId).HasColumnName("user_id");

            entity.HasOne(d => d.User).WithMany(p => p.RestaurantRecommendations)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__restauran__user___440B1D61");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.RoleId).HasName("PK__role__760965CCF7B70844");

            entity.ToTable("role");

            entity.Property(e => e.RoleId).HasColumnName("role_id");
            entity.Property(e => e.RoleName)
                .HasMaxLength(25)
                .HasColumnName("role_name");
        });

        modelBuilder.Entity<Status>(entity =>
        {
            entity.HasKey(e => e.StatusId).HasName("PK__status__3683B531373AF1AC");

            entity.ToTable("status");

            entity.Property(e => e.StatusId).HasColumnName("status_id");
            entity.Property(e => e.StatusName)
                .HasMaxLength(25)
                .HasColumnName("status_name");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PK__users__B9BE370FC52B477A");

            entity.ToTable("users");

            entity.Property(e => e.UserId).HasColumnName("user_id");
            entity.Property(e => e.Avatar)
                .IsUnicode(false)
                .HasColumnName("avatar");
            entity.Property(e => e.Email)
                .HasMaxLength(50)
                .HasColumnName("email");
            entity.Property(e => e.FirstName)
                .HasMaxLength(50)
                .HasColumnName("first_name");
            entity.Property(e => e.Interaction).HasColumnName("interaction");
            entity.Property(e => e.LastName)
                .HasMaxLength(50)
                .HasColumnName("last_name");
            entity.Property(e => e.MiddleName)
                .HasMaxLength(50)
                .HasColumnName("middle_name");
            entity.Property(e => e.PasswordHash)
                .IsUnicode(false)
                .HasColumnName("password_hash");
            entity.Property(e => e.Phone)
                .HasMaxLength(12)
                .HasColumnName("phone");
            entity.Property(e => e.ResetExpire)
                .HasColumnType("datetime")
                .HasColumnName("reset_expire");
            entity.Property(e => e.ResetToken)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("reset_token");
            entity.Property(e => e.RoleId).HasColumnName("role_id");
            entity.Property(e => e.StatusId).HasColumnName("status_id");
            entity.Property(e => e.Username)
                .HasMaxLength(50)
                .HasColumnName("username");

            entity.HasOne(d => d.Role).WithMany(p => p.Users)
                .HasForeignKey(d => d.RoleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__users__role_id__3E52440B");

            entity.HasOne(d => d.Status).WithMany(p => p.Users)
                .HasForeignKey(d => d.StatusId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__users__status_id__3D5E1FD2");

            entity.HasMany(d => d.Recipes).WithMany(p => p.Users)
                .UsingEntity<Dictionary<string, object>>(
                    "Bookmark",
                    r => r.HasOne<Recipe>().WithMany()
                        .HasForeignKey("RecipeId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK__bookmark__recipe__71D1E811"),
                    l => l.HasOne<User>().WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK__bookmark__user_i__70DDC3D8"),
                    j =>
                    {
                        j.HasKey("UserId", "RecipeId").HasName("PK__bookmark__1AE929D6D1C0F016");
                        j.ToTable("bookmark");
                        j.IndexerProperty<int>("UserId").HasColumnName("user_id");
                        j.IndexerProperty<int>("RecipeId").HasColumnName("recipe_id");
                    });
        });

        modelBuilder.Entity<Verification>(entity =>
        {
            entity.HasKey(e => e.VerificationId).HasName("PK__verifica__24F17969C0ED2782");

            entity.ToTable("verification");

            entity.Property(e => e.VerificationId).HasColumnName("verification_id");
            entity.Property(e => e.CardBack).HasColumnName("card_back");
            entity.Property(e => e.CardFront).HasColumnName("card_front");
            entity.Property(e => e.UserId).HasColumnName("user_id");
            entity.Property(e => e.VerificationBack).HasColumnName("verification_back");
            entity.Property(e => e.VerificationDate)
                .HasColumnType("datetime")
                .HasColumnName("verification_date");
            entity.Property(e => e.VerificationFront).HasColumnName("verification_front");
            entity.Property(e => e.VerificationStatus).HasColumnName("verification_status");

            entity.HasOne(d => d.User).WithMany(p => p.Verifications)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK__verificat__user___06CD04F7");
        });

        modelBuilder.Entity<Voucher>(entity =>
        {
            entity.HasKey(e => e.VoucherCode).HasName("PK__voucher__2173106878747685");

            entity.ToTable("voucher");

            entity.Property(e => e.VoucherCode)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("voucher_code");
            entity.Property(e => e.DiscountPercentage).HasColumnName("discount_percentage");
            entity.Property(e => e.UserId).HasColumnName("user_id");

            entity.HasOne(d => d.User).WithMany(p => p.Vouchers)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__voucher__user_id__7B5B524B");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
