using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace SercanUrunTakibi.Models
{
    public class SubCategory : BaseClass
    {
        //Kılçık -> Standart Kılçık <-- SubCategory
        public int SubCategoryId { get; set; }
        public int CategoryId { get; set; }
        public String SubCategoryName { get; set; }
        private Category category;

        public SubCategory() { 
        
        }

        public SubCategory(int subCategoryId)
        {
            String sql = "select * from subcategory where id = @subCategoryId";
            SqlCommand cmd = new SqlCommand(sql, cnn);
            cmd.Parameters.AddWithValue("@subCategoryId", subCategoryId);
            cnn.Open();
            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                this.Id = subCategoryId;
                this.CategoryId = parseInt(reader, "category_id");
                this.SubCategoryName = parseString(reader,"sub_category_name");
                this.IsActive = parseBool(reader,"is_active");
                this.category = new Category(parseInt(reader, "category_id"));
            }
            cnn.Close();
        }

        public Category getCategory()
        {
            return category;
        }

        public List<SubCategory> getAllSubCategories(int categoryId){
            List<SubCategory> subs = new List<SubCategory>();
            String sql = "select id from subcategory where category_id=@categoryId and is_active = true";
            SqlCommand cmd = new SqlCommand(sql,cnn);
            cmd.Parameters.AddWithValue("@categoryId",categoryId);

            cnn.Open();
            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                int subCategoryId = parseInt(reader, "id");
                SubCategory s = new SubCategory(subCategoryId);
                subs.Add(s);
            }
            cnn.Close();
            return subs;
                
            

        }
        public List<SubCategory> getAllSubCategories()
        {
            List<SubCategory> subs = new List<SubCategory>();
            String sql = "select id from subcategory where is_active = 'true'";
            SqlCommand cmd = new SqlCommand(sql, cnn);           

            cnn.Open();
            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                int subCategoryId = parseInt(reader, "id");
                SubCategory s = new SubCategory(subCategoryId);
                subs.Add(s);
            }
            cnn.Close();
            return subs;
        }

        public SubCategory(int categoryId, String subCategoryName,int userId)
        {
            String sql = "insert into subcategory (category_id, sub_category_name,create_user,create_date,is_active)"
                        + " values"
                        + " (@category_id,@sub_category_name,@create_user,GETDATE(),'true')";
            SqlCommand cmd = new SqlCommand(sql, cnn);
            cmd.Parameters.AddWithValue("@category_id", categoryId);
            cmd.Parameters.AddWithValue("@sub_category_name", subCategoryName);
            cmd.Parameters.AddWithValue("@create_user", userId);

            cnn.Open();
            cmd.ExecuteNonQuery();
            cnn.Close();
        }
    }
}