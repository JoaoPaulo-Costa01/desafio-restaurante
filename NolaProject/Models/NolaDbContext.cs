using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace NolaProject.Models;

public partial class NolaDbContext : DbContext
{
    public NolaDbContext(DbContextOptions<NolaDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Brand> Brands { get; set; }

    public virtual DbSet<Category> Categories { get; set; }

    public virtual DbSet<Channel> Channels { get; set; }

    public virtual DbSet<Customer> Customers { get; set; }

    public virtual DbSet<DeliveryAddress> DeliveryAddresses { get; set; }

    public virtual DbSet<DeliverySale> DeliverySales { get; set; }

    public virtual DbSet<Item> Items { get; set; }

    public virtual DbSet<ItemProductSale> ItemProductSales { get; set; }

    public virtual DbSet<OptionGroup> OptionGroups { get; set; }

    public virtual DbSet<Payment> Payments { get; set; }

    public virtual DbSet<PaymentType> PaymentTypes { get; set; }

    public virtual DbSet<Product> Products { get; set; }

    public virtual DbSet<ProductSale> ProductSales { get; set; }

    public virtual DbSet<Sale> Sales { get; set; }

    public virtual DbSet<Store> Stores { get; set; }

    public virtual DbSet<SubBrand> SubBrands { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Brand>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("brands_pkey");

            entity.ToTable("brands");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .HasColumnName("name");
        });

        modelBuilder.Entity<Category>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("categories_pkey");

            entity.ToTable("categories");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.BrandId).HasColumnName("brand_id");
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .HasColumnName("name");
            entity.Property(e => e.Type)
                .HasMaxLength(1)
                .HasColumnName("type");

            entity.HasOne(d => d.Brand).WithMany(p => p.Categories)
                .HasForeignKey(d => d.BrandId)
                .HasConstraintName("categories_brand_id_fkey");
        });

        modelBuilder.Entity<Channel>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("channels_pkey");

            entity.ToTable("channels");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.BrandId).HasColumnName("brand_id");
            entity.Property(e => e.Description)
                .HasMaxLength(255)
                .HasColumnName("description");
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .HasColumnName("name");
            entity.Property(e => e.Type)
                .HasMaxLength(1)
                .HasColumnName("type");

            entity.HasOne(d => d.Brand).WithMany(p => p.Channels)
                .HasForeignKey(d => d.BrandId)
                .HasConstraintName("channels_brand_id_fkey");
        });

        modelBuilder.Entity<Customer>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("customers_pkey");

            entity.ToTable("customers");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.AgreeTerms).HasColumnName("agree_terms");
            entity.Property(e => e.BirthDate).HasColumnName("birth_date");
            entity.Property(e => e.Cpf)
                .HasMaxLength(20)
                .HasColumnName("cpf");
            entity.Property(e => e.CreatedAt)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("created_at");
            entity.Property(e => e.CustomerName)
                .HasMaxLength(255)
                .HasColumnName("customer_name");
            entity.Property(e => e.Email)
                .HasMaxLength(255)
                .HasColumnName("email");
            entity.Property(e => e.Gender)
                .HasMaxLength(10)
                .HasColumnName("gender");
            entity.Property(e => e.PhoneNumber)
                .HasMaxLength(50)
                .HasColumnName("phone_number");
            entity.Property(e => e.ReceivePromotionsEmail).HasColumnName("receive_promotions_email");
            entity.Property(e => e.RegistrationOrigin)
                .HasMaxLength(100)
                .HasColumnName("registration_origin");
        });

        modelBuilder.Entity<DeliveryAddress>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("delivery_addresses_pkey");

            entity.ToTable("delivery_addresses");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.City)
                .HasMaxLength(100)
                .HasColumnName("city");
            entity.Property(e => e.Complement)
                .HasMaxLength(100)
                .HasColumnName("complement");
            entity.Property(e => e.DeliverySaleId).HasColumnName("delivery_sale_id");
            entity.Property(e => e.Latitude)
                .HasPrecision(9, 6)
                .HasColumnName("latitude");
            entity.Property(e => e.Longitude)
                .HasPrecision(9, 6)
                .HasColumnName("longitude");
            entity.Property(e => e.Neighborhood)
                .HasMaxLength(100)
                .HasColumnName("neighborhood");
            entity.Property(e => e.Number)
                .HasMaxLength(50)
                .HasColumnName("number");
            entity.Property(e => e.PostalCode)
                .HasMaxLength(20)
                .HasColumnName("postal_code");
            entity.Property(e => e.SaleId).HasColumnName("sale_id");
            entity.Property(e => e.State)
                .HasMaxLength(50)
                .HasColumnName("state");
            entity.Property(e => e.Street)
                .HasMaxLength(255)
                .HasColumnName("street");

            entity.HasOne(d => d.DeliverySale).WithMany(p => p.DeliveryAddresses)
                .HasForeignKey(d => d.DeliverySaleId)
                .HasConstraintName("delivery_addresses_delivery_sale_id_fkey");

            entity.HasOne(d => d.Sale).WithMany(p => p.DeliveryAddresses)
                .HasForeignKey(d => d.SaleId)
                .HasConstraintName("delivery_addresses_sale_id_fkey");
        });

        modelBuilder.Entity<DeliverySale>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("delivery_sales_pkey");

            entity.ToTable("delivery_sales");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CourierFee)
                .HasPrecision(10, 2)
                .HasColumnName("courier_fee");
            entity.Property(e => e.CourierName)
                .HasMaxLength(255)
                .HasColumnName("courier_name");
            entity.Property(e => e.CourierPhone)
                .HasMaxLength(50)
                .HasColumnName("courier_phone");
            entity.Property(e => e.CourierType)
                .HasMaxLength(100)
                .HasColumnName("courier_type");
            entity.Property(e => e.DeliveryFee)
                .HasPrecision(10, 2)
                .HasColumnName("delivery_fee");
            entity.Property(e => e.DeliveryType)
                .HasMaxLength(100)
                .HasColumnName("delivery_type");
            entity.Property(e => e.SaleId).HasColumnName("sale_id");
            entity.Property(e => e.Status)
                .HasMaxLength(100)
                .HasColumnName("status");

            entity.HasOne(d => d.Sale).WithMany(p => p.DeliverySales)
                .HasForeignKey(d => d.SaleId)
                .HasConstraintName("delivery_sales_sale_id_fkey");
        });

        modelBuilder.Entity<Item>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("items_pkey");

            entity.ToTable("items");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.BrandId).HasColumnName("brand_id");
            entity.Property(e => e.CategoryId).HasColumnName("category_id");
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .HasColumnName("name");
            entity.Property(e => e.PosUuid)
                .HasMaxLength(255)
                .HasColumnName("pos_uuid");
            entity.Property(e => e.SubBrandId).HasColumnName("sub_brand_id");

            entity.HasOne(d => d.Brand).WithMany(p => p.Items)
                .HasForeignKey(d => d.BrandId)
                .HasConstraintName("items_brand_id_fkey");

            entity.HasOne(d => d.Category).WithMany(p => p.Items)
                .HasForeignKey(d => d.CategoryId)
                .HasConstraintName("items_category_id_fkey");

            entity.HasOne(d => d.SubBrand).WithMany(p => p.Items)
                .HasForeignKey(d => d.SubBrandId)
                .HasConstraintName("items_sub_brand_id_fkey");
        });

        modelBuilder.Entity<ItemProductSale>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("item_product_sales_pkey");

            entity.ToTable("item_product_sales");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.AdditionalPrice)
                .HasPrecision(10, 2)
                .HasColumnName("additional_price");
            entity.Property(e => e.Amount).HasColumnName("amount");
            entity.Property(e => e.ItemId).HasColumnName("item_id");
            entity.Property(e => e.OptionGroupId).HasColumnName("option_group_id");
            entity.Property(e => e.Price)
                .HasPrecision(10, 2)
                .HasColumnName("price");
            entity.Property(e => e.ProductSaleId).HasColumnName("product_sale_id");
            entity.Property(e => e.Quantity).HasColumnName("quantity");

            entity.HasOne(d => d.Item).WithMany(p => p.ItemProductSales)
                .HasForeignKey(d => d.ItemId)
                .HasConstraintName("item_product_sales_item_id_fkey");

            entity.HasOne(d => d.OptionGroup).WithMany(p => p.ItemProductSales)
                .HasForeignKey(d => d.OptionGroupId)
                .HasConstraintName("item_product_sales_option_group_id_fkey");

            entity.HasOne(d => d.ProductSale).WithMany(p => p.ItemProductSales)
                .HasForeignKey(d => d.ProductSaleId)
                .HasConstraintName("item_product_sales_product_sale_id_fkey");
        });

        modelBuilder.Entity<OptionGroup>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("option_groups_pkey");

            entity.ToTable("option_groups");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.BrandId).HasColumnName("brand_id");
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .HasColumnName("name");

            entity.HasOne(d => d.Brand).WithMany(p => p.OptionGroups)
                .HasForeignKey(d => d.BrandId)
                .HasConstraintName("option_groups_brand_id_fkey");
        });

        modelBuilder.Entity<Payment>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("payments_pkey");

            entity.ToTable("payments");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.IsOnline).HasColumnName("is_online");
            entity.Property(e => e.PaymentTypeId).HasColumnName("payment_type_id");
            entity.Property(e => e.SaleId).HasColumnName("sale_id");
            entity.Property(e => e.Value)
                .HasPrecision(10, 2)
                .HasColumnName("value");

            entity.HasOne(d => d.PaymentType).WithMany(p => p.Payments)
                .HasForeignKey(d => d.PaymentTypeId)
                .HasConstraintName("payments_payment_type_id_fkey");

            entity.HasOne(d => d.Sale).WithMany(p => p.Payments)
                .HasForeignKey(d => d.SaleId)
                .HasConstraintName("payments_sale_id_fkey");
        });

        modelBuilder.Entity<PaymentType>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("payment_types_pkey");

            entity.ToTable("payment_types");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.BrandId).HasColumnName("brand_id");
            entity.Property(e => e.Description)
                .HasMaxLength(255)
                .HasColumnName("description");

            entity.HasOne(d => d.Brand).WithMany(p => p.PaymentTypes)
                .HasForeignKey(d => d.BrandId)
                .HasConstraintName("payment_types_brand_id_fkey");
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("products_pkey");

            entity.ToTable("products");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.BrandId).HasColumnName("brand_id");
            entity.Property(e => e.CategoryId).HasColumnName("category_id");
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .HasColumnName("name");
            entity.Property(e => e.PosUuid)
                .HasMaxLength(255)
                .HasColumnName("pos_uuid");
            entity.Property(e => e.SubBrandId).HasColumnName("sub_brand_id");

            entity.HasOne(d => d.Brand).WithMany(p => p.Products)
                .HasForeignKey(d => d.BrandId)
                .HasConstraintName("products_brand_id_fkey");

            entity.HasOne(d => d.Category).WithMany(p => p.Products)
                .HasForeignKey(d => d.CategoryId)
                .HasConstraintName("products_category_id_fkey");

            entity.HasOne(d => d.SubBrand).WithMany(p => p.Products)
                .HasForeignKey(d => d.SubBrandId)
                .HasConstraintName("products_sub_brand_id_fkey");
        });

        modelBuilder.Entity<ProductSale>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("product_sales_pkey");

            entity.ToTable("product_sales");

            entity.HasIndex(e => new { e.ProductId, e.SaleId }, "idx_product_sales_product_sale");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.BasePrice)
                .HasPrecision(10, 2)
                .HasColumnName("base_price");
            entity.Property(e => e.Observations).HasColumnName("observations");
            entity.Property(e => e.ProductId).HasColumnName("product_id");
            entity.Property(e => e.Quantity).HasColumnName("quantity");
            entity.Property(e => e.SaleId).HasColumnName("sale_id");
            entity.Property(e => e.TotalPrice)
                .HasPrecision(10, 2)
                .HasColumnName("total_price");

            entity.HasOne(d => d.Product).WithMany(p => p.ProductSales)
                .HasForeignKey(d => d.ProductId)
                .HasConstraintName("product_sales_product_id_fkey");

            entity.HasOne(d => d.Sale).WithMany(p => p.ProductSales)
                .HasForeignKey(d => d.SaleId)
                .HasConstraintName("product_sales_sale_id_fkey");
        });

        modelBuilder.Entity<Sale>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("sales_pkey");

            entity.ToTable("sales");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.ChannelId).HasColumnName("channel_id");
            entity.Property(e => e.CreatedAt)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("created_at");
            entity.Property(e => e.CustomerId).HasColumnName("customer_id");
            entity.Property(e => e.CustomerName)
                .HasMaxLength(255)
                .HasColumnName("customer_name");
            entity.Property(e => e.DeliveryFee)
                .HasPrecision(10, 2)
                .HasColumnName("delivery_fee");
            entity.Property(e => e.DeliverySeconds).HasColumnName("delivery_seconds");
            entity.Property(e => e.DiscountReason).HasColumnName("discount_reason");
            entity.Property(e => e.Origin)
                .HasMaxLength(100)
                .HasColumnName("origin");
            entity.Property(e => e.PeopleQuantity).HasColumnName("people_quantity");
            entity.Property(e => e.ProductionSeconds).HasColumnName("production_seconds");
            entity.Property(e => e.SaleStatusDesc)
                .HasMaxLength(100)
                .HasColumnName("sale_status_desc");
            entity.Property(e => e.ServiceTaxFee)
                .HasPrecision(10, 2)
                .HasColumnName("service_tax_fee");
            entity.Property(e => e.StoreId).HasColumnName("store_id");
            entity.Property(e => e.TotalAmount)
                .HasPrecision(10, 2)
                .HasColumnName("total_amount");
            entity.Property(e => e.TotalAmountItems)
                .HasPrecision(10, 2)
                .HasColumnName("total_amount_items");
            entity.Property(e => e.TotalDiscount)
                .HasPrecision(10, 2)
                .HasColumnName("total_discount");
            entity.Property(e => e.TotalIncrease)
                .HasPrecision(10, 2)
                .HasColumnName("total_increase");
            entity.Property(e => e.ValuePaid)
                .HasPrecision(10, 2)
                .HasColumnName("value_paid");

            entity.HasOne(d => d.Channel).WithMany(p => p.Sales)
                .HasForeignKey(d => d.ChannelId)
                .HasConstraintName("sales_channel_id_fkey");

            entity.HasOne(d => d.Customer).WithMany(p => p.Sales)
                .HasForeignKey(d => d.CustomerId)
                .HasConstraintName("sales_customer_id_fkey");

            entity.HasOne(d => d.Store).WithMany(p => p.Sales)
                .HasForeignKey(d => d.StoreId)
                .HasConstraintName("sales_store_id_fkey");
        });

        modelBuilder.Entity<Store>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("stores_pkey");

            entity.ToTable("stores");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.AddressNumber)
                .HasMaxLength(100)
                .HasColumnName("address_number");
            entity.Property(e => e.AddressStreet)
                .HasMaxLength(255)
                .HasColumnName("address_street");
            entity.Property(e => e.BrandId).HasColumnName("brand_id");
            entity.Property(e => e.City)
                .HasMaxLength(255)
                .HasColumnName("city");
            entity.Property(e => e.CreatedAt)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("created_at");
            entity.Property(e => e.CreationDate).HasColumnName("creation_date");
            entity.Property(e => e.District)
                .HasMaxLength(255)
                .HasColumnName("district");
            entity.Property(e => e.IsActive).HasColumnName("is_active");
            entity.Property(e => e.IsOwn).HasColumnName("is_own");
            entity.Property(e => e.Latitude)
                .HasPrecision(9, 6)
                .HasColumnName("latitude");
            entity.Property(e => e.Longitude)
                .HasPrecision(9, 6)
                .HasColumnName("longitude");
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .HasColumnName("name");
            entity.Property(e => e.State)
                .HasMaxLength(50)
                .HasColumnName("state");
            entity.Property(e => e.SubBrandId).HasColumnName("sub_brand_id");

            entity.HasOne(d => d.Brand).WithMany(p => p.Stores)
                .HasForeignKey(d => d.BrandId)
                .HasConstraintName("stores_brand_id_fkey");

            entity.HasOne(d => d.SubBrand).WithMany(p => p.Stores)
                .HasForeignKey(d => d.SubBrandId)
                .HasConstraintName("stores_sub_brand_id_fkey");
        });

        modelBuilder.Entity<SubBrand>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("sub_brands_pkey");

            entity.ToTable("sub_brands");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.BrandId).HasColumnName("brand_id");
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .HasColumnName("name");

            entity.HasOne(d => d.Brand).WithMany(p => p.SubBrands)
                .HasForeignKey(d => d.BrandId)
                .HasConstraintName("sub_brands_brand_id_fkey");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
