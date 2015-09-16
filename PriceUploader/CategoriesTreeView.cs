using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PriceUploader
{
    class CategoriesTreeView
    {
        private List<CategoryCharge> categoryCharge = new List<CategoryCharge>();
        private int categoryChargeID = 0;

        public void OpenCategoriesWindow(DataTable TableProductCategory, PriceModel model)
        {
            FormCategories formCategories = new FormCategories();
            formCategories.Init(TableProductCategory);
            //formCategories.ShowDialog();
        }

        internal void Init(DataTable TableProductCategory, FormCategories formCategories, PriceModel model)
        {
            //model.categories = new List<Category>();
            //foreach (var item in TableProductCategory.AsEnumerable())
            //{
            //    model.categories.Add(new Category
            //    {
            //        pc_id = Convert.ToInt32(item.ItemArray[0]),
            //        pc_parent_id = Convert.ToInt32(item.ItemArray[1]),
            //        pc_name = Convert.ToString(item.ItemArray[2]),
            //        pc_description = Convert.ToString(item.ItemArray[8])
            //    });
            //}

            categoryCharge = model.categoryCharge;

            formCategories.TreeViewCategories.Nodes.Clear();

            var RootCategories = model.categories.Where(x => x.pc_parent_id == 0).ToList();

            
            foreach (Category category in RootCategories)
            {
                // Root Nodes
                TreeNode node = null;
                //node = treeViewCategories.Nodes.Add(category.pc_id.ToString(), category.pc_name);
                this.categoryChargeID = category.pc_id;
                draw(model.categories, category, node, formCategories);

            }
        }

        private void draw(List<Category> categories, Category category, TreeNode node, FormCategories formCategories)
        {
            var subCategories = categories.Where(x => x.pc_parent_id == category.pc_id).ToList();

            if (subCategories.Count == 0)
            {
                drawLeaf(categories, category, node, formCategories);
            }
            else
            {
                drawNonLeaf(categories, category, node, formCategories);
            }
        }

        private void drawNonLeaf(List<Category> categories, Category category, TreeNode node, FormCategories formCategories)
        {
            var subCategories = categories.Where(x => x.pc_parent_id == category.pc_id).ToList();
            //Add Node 
            if (node == null)
                node = formCategories.TreeViewCategories.Nodes.Add(category.pc_id.ToString(), category.pc_name);
            else
                node = node.Nodes.Add(category.pc_id.ToString(), category.pc_name);
            foreach (Category cat in subCategories)
            {
                var subCatID = categoryCharge.Where(x => Convert.ToInt32(x.cc_pc_id) == cat.pc_id).Select(c=>c.cc_pc_id).FirstOrDefault();
                if (subCatID != null)
                {
                    this.categoryChargeID = Convert.ToInt32(subCatID);                    
                }
                var currentCategory = categories.Where(x => x.pc_id == cat.pc_id).FirstOrDefault();
                    currentCategory.pc_id_from_category_charge = this.categoryChargeID;

                draw(categories, cat, node, formCategories);
            }

        }

        private void drawLeaf(List<Category> categories, Category category, TreeNode node, FormCategories formCategories)
        {
            if (node == null)
                node = formCategories.TreeViewCategories.Nodes.Add(category.pc_id.ToString(), category.pc_name);
            else
                node.Nodes.Add(category.pc_id.ToString(), category.pc_name);


            var subCatID = categoryCharge.Where(x => Convert.ToInt32(x.cc_pc_id) == category.pc_id).Select(c => c.cc_pc_id).FirstOrDefault();
            if (subCatID != null)
            {
                this.categoryChargeID = Convert.ToInt32(subCatID);
            }
            var currentCategory = categories.Where(x => x.pc_id == category.pc_id).FirstOrDefault();
            currentCategory.pc_id_from_category_charge = this.categoryChargeID;

        }


    }
}
