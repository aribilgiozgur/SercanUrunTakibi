using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace SercanUrunTakibi.Models
{
    public class Product : BaseClass
    {
/*
 * Örnek Ürün, erkanplastik.com.tr'dan alınmıştır.
 * Standart kılçık:
 * Ölçü: 8mm,10mm...
 * Birim: Kutu (1 olarak geçiyor sitede ama kutu,kilo,poşet  olması lazım)
 * Adet: 10000 (Bir kutunun içerisindeki kılçık miktarı)
 * Renkler: Tüm Renkler
 * Hammadde: P.P. Naylon 
 */

        // Websitesi datası
        private Category Category { get; set; }
        public SubCategory SubCategory { get; set; }
        // Ölçü (int değil çünkü ölçü olarak "Standart" gelebiliyor)
        public String ProductSize { get; set; }        
        // Ölçüm Birimi (Kilo, tane, kutu...)
        public Measurement Measurement { get; set; }
        // Adet
        public int ProductBoxSize { get; set; }
        // Rengi
        public int ProductColor { get; set; } // 0 - Tüm renkler demek
        // Hammadde
        public int ResourceId { get; set; }
        public String ProductDescription { get; set; }

        // Diğerleri
        public double BuyPrice { get; set; }
        // Ürüne bağlı olarak kar miktarı
        public double ProfitRate { get; set; }
        public int ProductStock { get; set; }

        //Object[] paramArray = new Object[12];

        // Private empty constructor for in class usage
        public Product()
        {

        }

        // Insert constructor
        public Product(Object[] paramArray,int userId)
        {
            setValues(paramArray);

            // insert
            String sql = "insert into product (category_id, sub_category_id, product_size, measurement_id, product_box_size, product_color,"
                +" resource_id,product_description,buy_price,profit_rate,product_stock,is_active, create_user, create_date)"
                +" values"
                +" (@1,@2,@3,@4,@5,@6,@7,@8,@9,@10,@11,true,@12,GETDATE())";

            SqlCommand cmd = new SqlCommand(sql, cnn);
            cmd.Parameters.AddWithValue("@1", Category.getId());
            cmd.Parameters.AddWithValue("@2", SubCategory.getId());
            cmd.Parameters.AddWithValue("@3", ProductSize);
            cmd.Parameters.AddWithValue("@4", Measurement.getId());
            cmd.Parameters.AddWithValue("@5", ProductBoxSize);
            cmd.Parameters.AddWithValue("@6", ProductColor);
            cmd.Parameters.AddWithValue("@7", ResourceId);
            cmd.Parameters.AddWithValue("@8", ProductDescription);
            cmd.Parameters.AddWithValue("@9", BuyPrice);
            cmd.Parameters.AddWithValue("@10", ProfitRate);
            cmd.Parameters.AddWithValue("@11", ProductStock);
            cmd.Parameters.AddWithValue("@12", userId);

            try
            {
                cnn.Open();
                cmd.ExecuteNonQuery();
                cnn.Close();
            }
            catch (Exception ex) {
                ConnectionCloser(cnn);
                throw new Exception(ex.Message);
            }
            
        }
              
        // Get constructor
        public Product(int productId)
        {
            String sql = "select * from product where product_id = @productId";
            SqlCommand cmd = new SqlCommand(sql, cnn);
            cmd.Parameters.AddWithValue("@productId", productId);

            cnn.Open();
            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read()) {
                this.Id = parseInt(reader, "Id");                
                this.Category = new Category(parseInt(reader,"category_id"));
                this.SubCategory = new SubCategory(parseInt(reader,"sub_category_id"));
                this.ProductSize = parseString(reader, "product_size");
                this.Measurement = new Measurement(parseInt(reader, "measurement_id"));
                this.ProductBoxSize = parseInt(reader,"product_box_size");
                this.ProductColor = parseInt(reader,"product_color");      
                this.ResourceId = parseInt(reader,"resource_id");
                this.ProductDescription = parseString(reader,"product_description");
                this.BuyPrice = parseDouble(reader,"buy_price");       
                this.ProfitRate = parseDouble(reader,"profit_rate");
                this.ProductStock = parseInt(reader, "product_stock");
                this.IsActive = parseBool(reader, "is_active");
                this.CreateUser = parseInt(reader, "create_user");
                this.CreateDate = parseDate(reader, "create_date");
                this.LastUpdateUser = parseInt(reader, "last_update_user");
                this.LastUpdateDate = parseDate(reader, "last_update_date");
            }
            ConnectionCloser(cnn);
        }

        // Get all products (pagination ready)
        public List<Product> getAllProducts(int limit, int? offset) {
            List<Product> products = new List<Product>();

            // NOTE: OFFSET IN MSSQL!!!!
            String sql = "select * from product where is_active = 'true'"
                +" ORDER by id ASC"
                +" offset @offset ROWS";
            SqlCommand cmd = new SqlCommand(sql, cnn);
            cmd.Parameters.AddWithValue("@limit", limit);
            cmd.Parameters.AddWithValue("@offset", offset);
            cnn.Open();
            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read()) {
                Product p = setReaderToClass(reader);
                products.Add(p);
            }
            ConnectionCloser(cnn);

            return products;
        }

        // Update product
        // NOTE: Delete => IsActive = false;
        public void save(int userId){
            String sql = "update product set"
                +" category_id=@categoryId,"
                +" sub_category_id = @subCategoryId,"
                +" product_size = @productSize,"
                +" measurement_id = @measurementId," 
                +" product_box_size = @productBoxSize,"
                +" product_color = @productColor,"
                +" resource_id = @resourceId,"
                +" product_description = @productDescription,"
                +" buy_price = @buyPrice,"
                +" profit_rate = @profitRate,"
                +" product_stock = @productStock,"
                +" is_active = @isActive,"
                +" last_update_user = @userId,"
                +" last_update_date = GETDATE()"
                +" where product_id = @productId";
            SqlCommand cmd = new SqlCommand(sql, cnn);
            cmd.Parameters.AddWithValue("@categoryId", this.Category.getId());
            cmd.Parameters.AddWithValue("@subCategoryId", this.SubCategory.getId());
            cmd.Parameters.AddWithValue("@productSize", this.ProductSize);
            cmd.Parameters.AddWithValue("@measurementId", this.Measurement.getId());
            cmd.Parameters.AddWithValue("@productBoxSize", this.ProductBoxSize);
            cmd.Parameters.AddWithValue("@productColor", this.ProductColor);
            cmd.Parameters.AddWithValue("@resourceId", this.ResourceId);
            cmd.Parameters.AddWithValue("@productDescription", this.ProductDescription);
            cmd.Parameters.AddWithValue("@buyPrice", this.BuyPrice);
            cmd.Parameters.AddWithValue("@profitRate", this.ProfitRate);
            cmd.Parameters.AddWithValue("@productStock", this.ProductStock);
            cmd.Parameters.AddWithValue("@isActive", this.IsActive);
            cmd.Parameters.AddWithValue("@userId", userId);
            cmd.Parameters.AddWithValue("@productId", Id);

            cnn.Open(); cmd.ExecuteNonQuery(); ConnectionCloser(cnn);


        }


        // Helper funcs
        private void setValues(Object[] paramArray)
        {
            for (int i = 0; i < paramArray.Length; i++)
            {
                switch (i)
                {
                    case 0:
                        Category = new Category(int.Parse((string)paramArray[i]));
                        break;
                    case 1:
                        SubCategory = new SubCategory((int)paramArray[i]);
                        break;
                    case 2:
                        ProductSize = (String)paramArray[i];
                        break;
                    case 3:
                        Measurement = new Measurement((int)paramArray[i]);
                        break;
                    case 4:
                        ProductBoxSize = (int)paramArray[i];
                        break;
                    case 5:
                        ProductColor = (int)paramArray[i];
                        break;
                    case 6:
                        ResourceId = (int)paramArray[i];
                        break;
                    case 7:
                        ProductDescription = (String)paramArray[i];
                        break;
                    case 8:
                        BuyPrice = (double)paramArray[i];
                        break;
                    case 9:
                        ProfitRate = (double)paramArray[i];
                        break;
                    case 10:
                        ProductStock = (int)paramArray[i];
                        break;
                    default:
                        break;
                }
            }

        }

        public Product setReaderToClass(SqlDataReader reader) {
            Product p = new Product();
            p.Id = parseInt(reader, "Id");
            p.Category = new Category(parseInt(reader, "category_id"));
            p.SubCategory = new SubCategory(parseInt(reader, "sub_category_id"));
            p.ProductSize = parseString(reader, "product_size");
            p.Measurement = new Measurement(parseInt(reader, "measurement_id"));
            p.ProductBoxSize = parseInt(reader, "product_box_size");
            p.ProductColor = parseInt(reader, "product_color");
            p.ResourceId = parseInt(reader, "resource_id");
            p.ProductDescription = parseString(reader, "product_description");
            p.BuyPrice = parseDouble(reader, "buy_price");
            p.ProfitRate = parseDouble(reader, "profit_rate");
            p.ProductStock = parseInt(reader, "product_stock");
            p.IsActive = parseBool(reader, "is_active");
            p.CreateUser = parseInt(reader, "create_user");
            p.CreateDate = parseDate(reader, "create_date");
            p.LastUpdateUser = parseInt(reader, "last_update_user");
            p.LastUpdateDate = parseDate(reader, "last_update_date");
            return p;
        }
    }
    
}